Public Class UnMediosPago
    Public PTipoNota As Integer
    Public PAbierto As Boolean
    Public PEmisor As Integer
    Public PDtGrid As DataTable
    Public PBloqueaFunciones As Boolean
    Public PDtFormasPago As DataTable
    Public PDtRetencionProvincia As DataTable
    Public PDtRetencionesAutomaticas As DataTable
    Public PlistaDePercepciones As List(Of ItemIvaReten)
    Public PImporte As Decimal
    Public PEsExterior As Boolean
    Public PEsBancoNegro As Boolean
    Public PEsBancoBlanco As Boolean
    Public PDiferenciaDeCambio As Boolean
    Public PMoneda As Integer
    Public PCambio As Decimal
    Public PEsTr As Boolean
    Public PEsAlta As Boolean
    Public PEsTrOPagoEspecial As Boolean
    Public PImporteAInformar As Decimal
    Public PEsEnMonedaExtranjera As Boolean
    Public PEsBancoLiquidaDivisa As Boolean
    Public PEsChequeRechazado As Boolean
    Public PEsOrdenPagoImportador As Boolean
    Public PEsRetencionManual As Boolean
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '
    Dim DtGrid As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    Dim DtCuentas As DataTable
    Dim DtBancos As DataTable
    Dim TablaIva(0) As Double
    Dim BancoPredeterminado As Integer
    Dim CuentaPredeterminada As Decimal
    Dim SeriePredeterminada As String
    Dim NumeracionInicialPredeterminada As Integer
    Dim NumeracionfinalPredeterminada As Integer
    Dim UltimoNumeroPredeterminado As Integer
    Dim TotalConceptos As Decimal
    Dim cb As ComboBox
    Dim DTAux As DataTable
    Dim FilaSeleccionada(3) As Long
    Private Sub UnMediosPago_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False
        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = Nothing

        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        ComboMoneda.SelectedValue = PMoneda

        ArmaTablaIva(TablaIva)

        ArmaDtCuentas()

        DtGrid = CreaDtParaGrid()

        CopiaTabla(PDtGrid, DtGrid)
        For Each RowW As DataRow In DtGrid.Rows   'Convierto los eCheq con MedioPago = 2 al codigo 1002.
            If RowW("MedioPago") = 2 Then
                If RowW("eCheq") Then RowW("MedioPago") = 1002
            End If
        Next

        DtRetencionProvinciaAux = PDtRetencionProvincia

        TextCambio.Text = FormatNumber(PCambio, 3)

        If PImporteAInformar <> 0 Then
            Label1.Text = "Importe a Informar " & FormatNumber(PImporteAInformar, GDecimales)
            Label1.Visible = True
        End If

        Grid.DataSource = DtGrid

        Me.Size = New Size(1229, 660)
        ButtonAceptar.Location = New Point(558, 580)

        AgregaECheq(PDtFormasPago)  'Agrego el codigo 1002 eCheq propio solo para los pagos de la empresa.

        LlenaCombosGrid()

        If PBloqueaFunciones Then
            Grid.ReadOnly = True 'Siempre Poner despues de Grid.DataSource = DtGrid.
            ButtonEliminarLinea.Enabled = False
        End If

        CalculaTotales()

        OcultaColumnas()

        PresentaLineasGrid(Grid, PDtFormasPago, PTipoNota, PEsTr, PEsRetencionManual, PAbierto)

        If PEsEnMonedaExtranjera Then
            Grid.Columns("Cambio").Visible = False
        End If

        If PEsExterior Then
            PanelMoneda.Visible = True
        End If

        If PTipoNota = 600 Or PTipoNota = 64 Or PTipoNota = 5010 Or PTipoNota = 1010 Then
            Grid.Columns("Concepto").HeaderText = "Medio Pago"
            Grid.Columns("Serie").ReadOnly = True
        End If
        If PTipoNota = 60 Or PTipoNota = 65 Or PTipoNota = 604 Or PTipoNota = 5020 Then
            Grid.Columns("Concepto").HeaderText = "Medio Cobro"
        End If
        If PTipoNota = 65 Then
            Grid.Columns("Serie").ReadOnly = True
        End If
        If PTipoNota = 60 Or PTipoNota = 600 Or PTipoNota = 65 Or PTipoNota = 64 Or PTipoNota = 604 Then
            If PEsExterior Then
                Grid.Columns("Cambio").Visible = False
            Else
                Grid.Columns("Cambio").Visible = True
            End If
        End If

        If PEsChequeRechazado Then
            Grid.Columns("Concepto").ReadOnly = True
            Grid.Columns("Detalle").ReadOnly = True
            Grid.Columns("Neto").ReadOnly = True
            ButtonEliminarLinea.Enabled = False
        End If

        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Grid.Width = 0
        For i As Integer = 0 To Grid.Columns.Count - 1
            If Grid.Columns(i).Visible = True Then
                Grid.Width = Grid.Width + Grid.Columns(i).Width
            End If
        Next
        Grid.Width = Grid.Width + 50
        Grid.Left = PanelGrid.Width / 2 - Grid.Width / 2
        ButtonEliminarLinea.Left = Grid.Left + 20

        If Not PEsAlta Then
            Select Case PTipoNota
                Case 60, 64, 65
                    If EsClienteDelGupo(PEmisor) Then
                        ButtonExportar.Visible = True
                        ButtonImportar.Visible = True
                    End If
                Case 600, 604
                    If EsProveedorDelGupo(PEmisor) Then
                        ButtonExportar.Visible = True
                        ButtonImportar.Visible = True
                    End If
            End Select
        End If

        ButtonExportar.Visible = True
        ButtonImportar.Visible = True

        CreaDTAux()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        For Each Row As DataRow In DtGrid.Rows     'convierto los conceptos eCheq propios con clave = 1002 a 2 clave normal para cheques. 
            If Row("MedioPago") = 1002 Then Row("MedioPago") = 2
        Next

        PDtGrid = DtGrid.Copy
        PDtRetencionProvincia = DtRetencionProvinciaAux.Copy
        PImporte = CDbl(TextTotalMedioPago.Text)

        Me.Close()

    End Sub
    Private Sub ButtonExportar_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportar.Click

        If PTipoNota <> 600 And PTipoNota <> 64 Then
            MsgBox("Función Habilitada Solo para Ordenes de Pago. Operación se CANCELA.")
            Exit Sub
        End If

        ExportaCheques(DtGrid, PEmisor)

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If PTipoNota <> 60 And PTipoNota <> 604 Then
            MsgBox("Función Habilitada Solo para Cobranzas. Operación se CANCELA.")
            Exit Sub
        End If

        ImportaCheques(DtGrid)

    End Sub
    Private Sub RestituyeIndice()

        Dim IDFila As Integer = 0

        For J As Integer = 0 To DtGrid.Rows.Count - 1
            DtGrid.Rows(J)("ID") = IDFila + 1
            IDFila += 1
        Next

    End Sub
    Private Sub OcultaColumnas()

        Select Case PTipoNota
            Case 60, 600, 65, 6000, 6001, 64, 604, 1010, 4010
                Grid.Columns("Detalle").Visible = False
                Grid.Columns("Neto").Visible = False
                Grid.Columns("Alicuota").Visible = False
                Grid.Columns("Iva").Visible = False
            Case Else
                Grid.Columns("Banco").Visible = False
                Grid.Columns("Fecha").Visible = False
                Grid.Columns("Serie").Visible = False
                Grid.Columns("Cuenta").Visible = False
                Grid.Columns("LupaCuenta").Visible = False
                Grid.Columns("Numero").Visible = False
                Grid.Columns("Serie").Visible = False
                Grid.Columns("EmisorCheque").Visible = False
                Grid.Columns("ClaveChequeVisual").Visible = False
                Grid.Columns("Cambio").Visible = False
                Grid.Columns("Comprobante").Visible = False
                Grid.Columns("FechaComprobante").Visible = False
                Grid.Columns("Bultos").Visible = False
                Grid.Columns("eCheq").Visible = False
        End Select

    End Sub
    Private Sub LlenaCombosGrid()

        Dim FilaAuxiliar As DataRow

        DtBancos = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")

        '-----------------------------------------------------------------------------------------'
        Concepto.DataSource = PDtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Banco.DataSource = DtBancos
        FilaAuxiliar = Banco.DataSource.NewRow()
        FilaAuxiliar("Clave") = 0
        FilaAuxiliar("Nombre") = " "
        Banco.DataSource.Rows.Add(FilaAuxiliar)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

        '------------------------------------------------------------------------------------------'

        Dim DTConcepto As New DataTable
        DTConcepto = PDtFormasPago.Copy

        For I As Integer = DTConcepto.Rows.Count - 1 To 0 Step -1
            If DTConcepto.Rows(I).RowState = DataRowState.Deleted Then Continue For
            Select Case DTConcepto.Rows(I).Item("Clave")
                Case 2, 4, 9, 11, 14, 1002
                Case Else
                    DTConcepto.Rows(I).Delete()
            End Select
        Next
        DTConcepto.AcceptChanges()

        ComboConcepto.DataSource = DTConcepto
        ComboConcepto.DisplayMember = "Nombre"
        ComboConcepto.ValueMember = "Clave"
        ComboConcepto.SelectedValue = 0

    End Sub
    Private Sub AgregaECheq(ByRef Dt As DataTable)

        For Each RowW As DataRow In Dt.Rows
            If RowW.RowState <> DataRowState.Deleted Then
                If RowW("Clave") = 2 Then
                    PanelMultiplesCheques.Visible = True
                    ButtonAceptar.Location = New Point(558, 656)
                    Me.Size = New Size(1229, 728)

                    Dim Row As DataRow = Dt.NewRow
                    Row("Clave") = 1002
                    Row("Nombre") = "eCheq Propio"
                    Row("Tipo") = 2
                    Dt.Rows.Add(Row)
                    Exit Sub
                End If
            End If
        Next

    End Sub
    Private Sub CalculaTotales()

        TotalConceptos = 0
        If Not PEsExterior And Not PEsEnMonedaExtranjera Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Cambio").Value = 0 Then
                    TotalConceptos = TotalConceptos + Row.Cells("Importe").Value
                Else : TotalConceptos = TotalConceptos + Trunca(Row.Cells("Cambio").Value * Row.Cells("Importe").Value)
                End If
            Next
        Else
            For Each Row As DataGridViewRow In Grid.Rows
                TotalConceptos = TotalConceptos + Row.Cells("Importe").Value
            Next
        End If

        TextTotalMedioPago.Text = FormatNumber(TotalConceptos, GDecimales)

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = PDtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        If Grid.CurrentRow.Cells("Concepto").ReadOnly Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)

        If Row("TieneLupa") Then
            For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
                Dim Row1 As DataRow = DtRetencionProvinciaAux.Rows(I)
                If Row1("Retencion") = Row("Mediopago") Then Row1.Delete()
            Next
        End If

        If Grid.CurrentRow.Cells("Numero").Value <> 0 And Grid.CurrentRow.Cells("Concepto").Value = 2 Then
            If MsgBox("Se Enumerarán Nuevamente Los Cheques Propios, Correspondientes A El Banco: " & Grid.CurrentRow.Cells("Banco").FormattedValue & " Y Cuenta: " & Grid.CurrentRow.Cells("Cuenta").Value & ", Una Vez Borrada Esta Línea. Se Incluyen, También, Aquellos Modificados Por Teclado. Desea Continuar?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Borra Linea.") = MsgBoxResult.Yes Then
                FilaSeleccionada(0) = Row("Banco")
                FilaSeleccionada(1) = Row("Cuenta")
                FilaSeleccionada(2) = Row("MedioPago")
            End If
        End If

        Row.Delete()

        If FilaSeleccionada(2) = 2 Then
            EnumeraCheques(FilaSeleccionada(0), FilaSeleccionada(1))
        Else
            RestituyeIndice()
        End If

        CalculaTotales()

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,Moneda,0.0 AS Debito,0.0 AS Credito FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    Private Sub IngresaCheques(ByVal ListaDeChequesAux As List(Of ItemCheque))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            ''''           If (Row("ClaveCheque") <> 0 And Row("MedioPago") <> ListaDeChequesAux(0).MedioPago) Or (Row("ClaveCheque") = 0 And Row("MedioPago") <> 2 And Row("MedioPago") <> 3) Then
            ''''''''''''''      DtGrid.ImportRow(Row)
            ''''''''''''''       End If
        Next
        For Each Row As DataRow In Dt.Rows
            If Row("Item") <> 0 Then
                DtGrid.ImportRow(Row)
            End If
            If Row("Item") = 0 Then
                If Row("MedioPago") <> ListaDeChequesAux(0).MedioPago Then DtGrid.ImportRow(Row) : Continue For
                If Row("ClaveCheque") <> ListaDeChequesAux(0).ClaveCheque And Row("ClaveCheque") <> 0 Then DtGrid.ImportRow(Row) : Continue For
            End If
        Next
        For Each Item As ItemCheque In ListaDeChequesAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = 0
            Row1("MedioPago") = Item.MedioPago
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("ImporteIva") = 0
            Row1("Banco") = Item.Banco
            Row1("Fecha") = Item.Fecha
            Row1("Cuenta") = Item.Cuenta
            Row1("Serie") = Item.Serie
            Row1("Numero") = Item.Numero
            Row1("EmisorCheque") = Item.EmisorCheque
            Row1("Cambio") = 0
            Row1("Importe") = Item.Importe
            Row1("Comprobante") = 0
            Row1("eCheq") = Item.echeq
            Row1("FechaComprobante") = "1/1/1800"
            Row1("ClaveCheque") = Item.ClaveCheque
            Row1("ClaveInterna") = 0
            If Item.MedioPago = 2 Then
                Row1("NumeracionInicial") = 1
                Row1("NumeracionFinal") = 999999999
            End If
            If Item.MedioPago = 2 Then
                Row1("ClaveChequeVisual") = 0
            Else : Row1("ClaveChequeVisual") = Item.ClaveCheque
            End If
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        PresentaLineasGrid(Grid, PDtFormasPago, PTipoNota, PEsTr, PEsRetencionManual, PAbierto)
        Grid.EndEdit()

        RestituyeIndice()

        Dt.Dispose()

    End Sub
    Private Sub IngresaRecuperoSenia(ByVal ListaDeRecuperoSeniaAux As List(Of ItemRecuperoSenia))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("Item") <> 0 Then
                DtGrid.ImportRow(Row)
            End If
            If Row("Item") = 0 And Row("MedioPago") <> 6 Then
                DtGrid.ImportRow(Row)
            End If
        Next

        For Each Item As ItemRecuperoSenia In ListaDeRecuperoSeniaAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = 0
            Row1("MedioPago") = 6
            Row1("Detalle") = ""
            Row1("Neto") = 0
            Row1("Alicuota") = 0
            Row1("ImporteIva") = 0
            Row1("Banco") = 0
            Row1("Fecha") = "1/1/1800"
            Row1("Cuenta") = 0
            Row1("Serie") = ""
            Row1("Numero") = 0
            Row1("EmisorCheque") = ""
            Row1("Cambio") = 0
            Row1("Importe") = Item.Importe
            Row1("Comprobante") = Item.Vale
            Row1("FechaComprobante") = Item.Fecha
            Row1("ClaveCheque") = 0
            Row1("ClaveInterna") = Item.Nota
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = DtGrid
        PresentaLineasGrid(Grid, PDtFormasPago, PTipoNota, PEsTr, PEsRetencionManual, PAbierto)
        Grid.EndEdit()

        RestituyeIndice()

        Dt.Dispose()

    End Sub
    Private Function HallaUltimoNumeroGrabado(ByVal Banco As Integer, ByVal Cuenta As Double) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("eCheq = 0 AND Banco = " & Banco & " AND Cuenta = " & Cuenta & " AND NUMERO <> 0")
        If RowsBusqueda.Length <> 0 Then
            Return RowsBusqueda(RowsBusqueda.Length - 1).Item("Numero")
        Else
            Return 0
        End If

    End Function
    Private Function HallaSecuenciaCheque(ByVal Banco As Integer, ByVal Cuenta As Double, ByVal Numero As Integer) As Integer

        Dim RowsBusqueda() As DataRow


        RowsBusqueda = DTAux.Select("Banco = " & Banco & " AND Cuenta = " & Cuenta)
        Dim NumeroFinal As Integer = RowsBusqueda(0).Item("UltimoNumero")
        Dim UltimoNumeroUsado As Integer = RowsBusqueda(0).Item("Numero")

        Dim NumeroW As Integer

        If Numero = 0 Then
            NumeroW = UltimoNumeroUsado
        Else
            NumeroW = Numero
        End If

        Do
            NumeroW = NumeroW + 1
            If NumeroW > NumeroFinal Then MsgBox("Numeración Fuera del rango de la Chequera.") : Return 0
            Return NumeroW
        Loop

    End Function
    Private Sub EnumeraCheques(ByRef Banco As Integer, ByVal Cuenta As Double)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("eCheq = 0 AND MedioPago = 2 AND Banco = " & Banco & " AND Cuenta = " & Cuenta)
        If RowsBusqueda.Length = 0 Then Exit Sub

        For I As Integer = 0 To RowsBusqueda.Length - 1
            If I = 0 Then
                RowsBusqueda(I).Item("Numero") = HallaSecuenciaCheque(Banco, Cuenta, 0)
            Else
                RowsBusqueda(I).Item("Numero") = HallaSecuenciaCheque(Banco, Cuenta, RowsBusqueda(I - 1).Item("Numero"))
            End If
        Next

    End Sub
    ' SE CREA EL DATATABLE AUXILIAR PARA GUARDAR LOS DATOS DE LA CUENTA SELECCIONADA.
    Private Sub CreaDTAux()

        DTAux = New DataTable

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DTAux.Columns.Add(Numero)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DTAux.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DTAux.Columns.Add(Cuenta)

        Dim UltimoNumero As New DataColumn("UltimoNumero")
        UltimoNumero.DataType = System.Type.GetType("System.Int32")
        DTAux.Columns.Add(UltimoNumero)

    End Sub
    Private Sub LlenaDTAux(ByVal Numero As Long, ByVal Banco As Integer, ByVal Cuenta As Double, ByVal UltimoNumero As Long)

        If DTAux.Select("Banco = " & Banco & " AND Cuenta = " & Cuenta).Length = 0 Then
            Dim Row As DataRow = DTAux.NewRow
            Row("Numero") = Numero
            Row("Banco") = Banco
            Row("Cuenta") = Cuenta
            Row("UltimoNumero") = UltimoNumero
            DTAux.Rows.Add(Row)
        End If

    End Sub
    Private Function Valida() As Boolean

        If DtGrid.HasErrors Then
            MsgBox("Debe corregir errores. Función se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
            Return False
        End If

        Dim RowsBusqueda() As DataRow

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextTotalMedioPago.Text) = 0 Then
            MsgBox("Falta Informar Importes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not ConsistePagos(Grid, PDtFormasPago, PTipoNota, PEsTr) Then Return False

        For Each Row As DataRow In DtGrid.Rows
            If Row("Alicuota") <> 0 And (PDiferenciaDeCambio Or PEsExterior) Then
                MsgBox("IVA No corresponde para Exportación o Importación.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        Dim ImporteRetencion As Decimal = 0
        For Each Row1 As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row1.Cells("Concepto").Value) Then
                If Row1.Cells("TieneLupa").Value Then
                    ImporteRetencion = ImporteRetencion + Row1.Cells("Importe").Value
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Decimal
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        If Not PEsOrdenPagoImportador Then
            If PEsExterior Or PEsEnMonedaExtranjera Then
                For Each Row1 As DataGridViewRow In Grid.Rows
                    If Not IsNothing(Row1.Cells("Concepto").Value) Then
                        RowsBusqueda = PDtFormasPago.Select("Clave = " & Row1.Cells("Concepto").Value)
                        'Actualizo cambio de moneda con el cambio declarado en recibo.
                        If (RowsBusqueda(0).Item("Tipo") = 1 Or RowsBusqueda(0).Item("Tipo") = 3) And RowsBusqueda(0).Item("Clave") <> ComboMoneda.SelectedValue Then
                            MsgBox("Moneda no se corresponde con la que Opera el Cliente o proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.Focus()
                            Return False
                        End If
                        If RowsBusqueda(0).Item("Tipo") = 3 Then Row1.Cells("Cambio").Value = CDbl(TextCambio.Text)
                    End If
                Next
            End If
        End If

        If PEsOrdenPagoImportador Then
            For Each Row1 As DataGridViewRow In Grid.Rows
                If Not IsNothing(Row1.Cells("Concepto").Value) Then
                    RowsBusqueda = PDtFormasPago.Select("Clave = " & Row1.Cells("Concepto").Value)
                    If RowsBusqueda(0).Item("Tipo") = 1 Or RowsBusqueda(0).Item("Tipo") = 3 Then
                        If RowsBusqueda(0).Item("Clave") <> ComboMoneda.SelectedValue Then
                            MsgBox("Moneda no se corresponde con la que Opera el Proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(Row1.Index).Cells("Concepto")
                            Grid.BeginEdit(True)
                            Return False
                        Else
                            If RowsBusqueda(0).Item("Tipo") = 3 Then Row1.Cells("Cambio").Value = CDbl(TextCambio.Text)
                        End If
                    End If
                    If RowsBusqueda(0).Item("Tipo") = 7 Then
                        Dim Moneda As Integer = HallaMonedaDeLaCuenta(Row1.Cells("Banco").Value, Row1.Cells("Cuenta").Value, DtCuentas)
                        If Moneda <> 1 And Moneda <> ComboMoneda.SelectedValue Then
                            MsgBox("Moneda de la Cuenta no se corresponde con la que Opera el Proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            Grid.CurrentCell = Grid.Rows(Row1.Index).Cells("Concepto")
                            Grid.BeginEdit(True)
                            Return False
                        End If
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

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If
            If IsNothing(cb.SelectedValue) Then Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If
            '
            If IsNothing(cb.SelectedValue) Then Exit Sub
            If cb.SelectedIndex <= 0 Then Exit Sub
            '
            RowsBusqueda = PDtFormasPago.Select("Clave = " & cb.SelectedValue)
            Grid.Rows(e.RowIndex).Cells("Concepto").Value = cb.SelectedValue
            Grid.Rows(e.RowIndex).Cells("TieneLupa").Value = False
            If RowsBusqueda(0).Item("Tipo") = 4 Then
                If PDtRetencionesAutomaticas.Rows.Count <> 0 Then
                    RowsBusqueda1 = PDtRetencionesAutomaticas.Select("Clave = " & cb.SelectedValue)
                    If RowsBusqueda1.Length <> 0 Then
                        Grid.Rows(e.RowIndex).Cells("TieneLupa").Value = False
                        Exit Sub
                    End If
                End If
                If EsRetencionPorProvincia(cb.SelectedValue) Then
                    Grid.Rows(e.RowIndex).Cells("TieneLupa").Value = True
                End If
                If PEsRetencionManual Then
                    Grid.Rows(e.RowIndex).Cells("TieneLupa").Value = False
                End If
            End If
            ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), RowsBusqueda(0).Item("Tipo"), PTipoNota, PEsTr, PEsRetencionManual, PAbierto)
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Or Grid.Columns(e.ColumnIndex).Name = "Bultos" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0
            CalculaImportePercepciones()
            CalculaTotales()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0 : Exit Sub
            '
            If (Grid.CurrentRow.Cells("Concepto").Value = 2 Or Grid.CurrentRow.Cells("Concepto").Value = 1002) And Not PEsTr And CuentaPredeterminada <> 0 Then
                If Grid.CurrentRow.Cells("Numero").Value = 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = BancoPredeterminado
                    Grid.CurrentRow.Cells("Cuenta").Value = CuentaPredeterminada
                    Grid.CurrentRow.Cells("Serie").Value = SeriePredeterminada
                    Grid.CurrentRow.Cells("NumeracionInicial").Value = NumeracionInicialPredeterminada
                    Grid.CurrentRow.Cells("NumeracionFinal").Value = NumeracionfinalPredeterminada
                    If Not Grid.CurrentRow.Cells("Concepto").Value = 1002 Then       'no lo hace para echeq.
                        Dim UltimoNumeroGrabado As Integer = HallaUltimoNumeroGrabado(BancoPredeterminado, CuentaPredeterminada)
                        Grid.CurrentRow.Cells("Numero").Value = HallaSecuenciaCheque(BancoPredeterminado, CuentaPredeterminada, UltimoNumeroGrabado)
                    Else
                        Grid.CurrentRow.Cells("Serie").Value = "00"
                    End If
                End If
            End If
        End If

    End Sub
    Private Function EstaEnListaDePercepciones(ByVal Clave As Integer, ByRef Importe As Decimal) As Boolean

        For Each Fila As ItemIvaReten In PlistaDePercepciones
            If Fila.Clave = Clave Then Importe = Fila.Importe : Return True
        Next

        Return False

    End Function
    Private Sub CalculaImportePercepciones()

        If IsNothing(PlistaDePercepciones) Then Exit Sub

        If PlistaDePercepciones.Count = 0 Then Exit Sub

        Dim TotalNeto As Decimal = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Not EstaEnListaDePercepciones(Row.Cells("Concepto").Value, Nothing) Then TotalNeto = TotalNeto + Row.Cells("Neto").Value
        Next

        CalculaPercepciones(PlistaDePercepciones, TotalNeto)
        Dim Importe As Decimal = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If EstaEnListaDePercepciones(Row.Cells("Concepto").Value, Importe) Then
                Row.Cells("Importe").Value = Importe
            End If
        Next

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Neto").Value = 0
                Grid.CurrentRow.Cells("Alicuota").Value = 0
                Grid.CurrentRow.Cells("Iva").Value = 0
                Grid.CurrentRow.Cells("Importe").Value = 0
                Grid.CurrentRow.Cells("Banco").Value = 0
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.CurrentRow.Cells("Serie").Value = ""
                Grid.CurrentRow.Cells("Numero").Value = 0
                Grid.CurrentRow.Cells("Fecha").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Cambio").Value = 0
                Grid.CurrentRow.Cells("EmisorCheque").Value = ""
                Grid.CurrentRow.Cells("ClaveCheque").Value = 0
                Grid.CurrentRow.Cells("ClaveChequeVisual").Value = 0
                Grid.CurrentRow.Cells("ClaveInterna").Value = 0
                Grid.CurrentRow.Cells("Comprobante").Value = 0
                Grid.CurrentRow.Cells("FechaComprobante").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Bultos").Value = 0
                Grid.CurrentRow.Cells("eCheq").Value = False
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 1002 Then
                    Grid.CurrentRow.Cells("eCheq").Value = True
                End If
                Grid.Refresh()
                CalculaTotales()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Banco")) Then
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not Grid.CurrentRow.Cells("TieneLupa").Value Then Exit Sub
            If Grid.CurrentRow.Cells("Importe").Value = 0 Then
                MsgBox("Debe Informar Importe.")
                Exit Sub
            End If
            If PTipoNota = 60 Or PTipoNota = 604 Then   'con el comprobante viene una hoja con la retenc/perc. con fecha y numero.
                If Grid.CurrentRow.Cells("Comprobante").Value = 0 Then
                    MsgBox("Debe Informar Comprobante.")
                    Exit Sub
                End If
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtGrid.Select("MedioPago = " & Grid.CurrentRow.Cells("Concepto").Value & " AND Comprobante = " & Grid.CurrentRow.Cells("Comprobante").Value)
                If RowsBusqueda.Length > 1 Then
                    MsgBox("Comprobante Ya Existe.")
                    Exit Sub
                End If
            End If
            SeleccionarRetencionesProvincias.PFuncionBloqueada = PBloqueaFunciones
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid.CurrentRow.Cells("Importe").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid.CurrentRow.Cells("Concepto").Value
            SeleccionarRetencionesProvincias.PComprobante = Grid.CurrentRow.Cells("Comprobante").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

        If PBloqueaFunciones Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then Exit Sub
            If (Grid.CurrentRow.Cells("Concepto").Value = 2 Or Grid.CurrentRow.Cells("Concepto").Value = 1002) And Not PEsTr Then
                If Grid.CurrentRow.Cells("Numero").Value = 0 Then
                    ListaBancos.PEsSeleccionaCuenta = True
                    ListaBancos.PEsSoloPesos = True
                    ListaBancos.PConChequera = True
                    ListaBancos.ShowDialog()
                    If ListaBancos.PCuenta <> 0 Then
                        Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                        Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                        If Grid.CurrentRow.Cells("Concepto").Value = 2 Then
                            Grid.CurrentRow.Cells("Serie").Value = ListaBancos.PSerie
                        Else
                            Grid.CurrentRow.Cells("Serie").Value = "00"
                        End If
                        Grid.CurrentRow.Cells("NumeracionInicial").Value = ListaBancos.PNumeracionInicial
                        Grid.CurrentRow.Cells("NumeracionFinal").Value = ListaBancos.PNumeracionFinal
                        ' INGRESA DATOS AL DATATABLE AUXILIAR A PARTIR DE LA CUENTA SELECCIONADA.
                        LlenaDTAux(ListaBancos.PUltimoNumero, ListaBancos.PBanco, ListaBancos.PCuenta, ListaBancos.PNumeracionFinal)
                        If Grid.CurrentRow.Cells("Concepto").Value = 2 Then
                            Dim UltimoNumeroGrabado As Integer = HallaUltimoNumeroGrabado(ListaBancos.PBanco, ListaBancos.PCuenta)
                            Grid.CurrentRow.Cells("Numero").Value = HallaSecuenciaCheque(ListaBancos.PBanco, ListaBancos.PCuenta, UltimoNumeroGrabado)
                        End If
                    End If
                    ListaBancos.Dispose()
                End If
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 2 And PEsTr Then
                SeleccionarCheques.PEsChequesPropiosEmitidoParaTr = True
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 2 And Row("ClaveCheque") <> 0 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.PAbierto = False
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques)
                    CalculaTotales()
                End If
                SeleccionarCheques.Dispose()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 3 And PEsTr Then
                If PTipoNota = 6001 Then Exit Sub
                Select Case PTipoNota
                    Case 60
                        SeleccionarCheques.PEsChequesTercerosRecibidosParaTr = True
                        SeleccionarCheques.PAbierto = False
                    Case 600
                        If PEsTrOPagoEspecial Then
                            SeleccionarCheques.PCaja = GCaja
                            SeleccionarCheques.PEsChequeEnCartera = True
                            SeleccionarCheques.PAbierto = True
                        Else
                            SeleccionarCheques.PEsChequesTercerosEntregadoParaTr = True
                            SeleccionarCheques.PAbierto = False
                        End If
                End Select
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 3 And Row("ClaveCheque") <> 0 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques)
                    CalculaTotales()
                End If
                SeleccionarCheques.Dispose()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 3 And Not PEsTr Then
                If PTipoNota = 60 Or PTipoNota = 6001 Or PTipoNota = 604 Then Exit Sub
                SeleccionarCheques.PCaja = GCaja
                SeleccionarCheques.PEsChequeEnCartera = True
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 3 And Row("ClaveCheque") <> 0 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.PAbierto = PAbierto
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques)
                    CalculaTotales()
                End If
                SeleccionarCheques.Dispose()
            End If
            '
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Or HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 15 Then
                ListaBancos.PEsBancoNegro = PEsBancoNegro
                ListaBancos.PEsBancoBlanco = PEsBancoBlanco
                ListaBancos.PEsSeleccionaCuenta = True
                ListaBancos.PEsBancoLiquidaDivisa = PEsBancoLiquidaDivisa
                If Not PEsOrdenPagoImportador Then
                    If PEsEnMonedaExtranjera Then
                        ListaBancos.PMoneda = PMoneda
                    Else
                        ListaBancos.PEsSoloPesos = True
                    End If
                End If
                ListaBancos.ShowDialog()
                If ListaBancos.PCuenta <> 0 Then
                    Grid.CurrentRow.Cells("Banco").Value = ListaBancos.PBanco
                    Grid.CurrentRow.Cells("Cuenta").Value = ListaBancos.PCuenta
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Comprobante")
                End If
                ListaBancos.Dispose()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 6 Then      'Seña terceros.
                If PTipoNota <> 600 Then Exit Sub
                SeleccionarVarios.PEsValeTercerosOrdenPago = True
                SeleccionarVarios.PCaja = GCaja
                SeleccionarVarios.PAbierto = PAbierto
                SeleccionarVarios.PEmisor = PEmisor
                SeleccionarVarios.PListaDeRecuperoSenia = New List(Of ItemRecuperoSenia)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 6 And Row("ClaveInterna") <> 0 Then
                        Dim Item As New ItemRecuperoSenia
                        Item.Nota = Row("ClaveInterna")
                        SeleccionarVarios.PListaDeRecuperoSenia.Add(Item)
                    End If
                Next
                SeleccionarVarios.ShowDialog()
                If SeleccionarVarios.PListaDeRecuperoSenia.Count <> 0 Then
                    IngresaRecuperoSenia(SeleccionarVarios.PListaDeRecuperoSenia)
                    CalculaTotales()
                End If
                SeleccionarVarios.Dispose()
            End If
        End If
        '
        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If PEsTr Then Exit Sub
            If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 1002 Or Grid.Rows(e.RowIndex).Cells("Concepto").Value = 2 Or ((PTipoNota = 60 Or PTipoNota = 604) And Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3) Or (PTipoNota = 6001 And Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3) Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 7 Or HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 15 Or (HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 4 And (PTipoNota = 60 Or PTipoNota = 604)) Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
            If HallaTipo(Grid.CurrentRow.Cells("Concepto").Value) = 10 Then
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Grid.EndEdit()
                Calendario.Dispose()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or _
           Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "Bultos" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then
                    e.Value = ""
                Else : e.Value = Format(e.Value, "dd/MM/yyyy")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveChequeVisual" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TieneLupa").Value) Then
                If Grid.Rows(e.RowIndex).Cells("TieneLupa").Value Then
                    e.Value = ImageList1.Images.Item("Lupa")
                Else : e.Value = Nothing
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Detalle" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Banco" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then
                e.KeyChar = ""
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Concepto").Value = 0 Then
                e.KeyChar = ""
                Exit Sub
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Bultos" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Numero" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Comprobante" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuenta" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Serie" Then
            If e.KeyChar = "" Then Exit Sub
            e.KeyChar = e.KeyChar.ToString.ToUpper
            If Asc(e.KeyChar) <> 42 Then
                If Asc(e.KeyChar) < 65 Or Asc(e.KeyChar) > 90 Then e.KeyChar = ""
            End If
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Bultos" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Item") = 0
        Row("MedioPago") = 0
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Bultos") = 0
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("ClaveChequeVisual") = 0
        Row("ClaveInterna") = 0
        Row("TieneLupa") = False
        Row("eCheq") = False
        Row("ID") = Grid.Rows.Count
        Row("NumeracionInicial") = 0
        Row("NumeracionFinal") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.ClearErrors()

        If e.Column.ColumnName.Equals("MedioPago") Then
            Dim RowsBusqueda() As DataRow
            Dim RowsBusqueda1() As DataRow
            RowsBusqueda = PDtFormasPago.Select("Clave = " & e.ProposedValue)
            If RowsBusqueda(0).Item("Tipo") = 4 And PTipoNota <> 60 Then
                RowsBusqueda1 = DtGrid.Select("MedioPago = " & cb.SelectedValue)
                If RowsBusqueda1.Length > 0 Then
                    MsgBox("Retención o Percepción ya Existe.")
                    e.ProposedValue = e.Row("MedioPago")
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Detalle") Then
            If IsDBNull(e.Row("Detalle")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If e.Column.ColumnName.Equals("Neto") Then
            If IsDBNull(e.Row("Neto")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If Not IsDBNull(e.Row("Alicuota")) Then
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Alicuota") / 100)
                e.Row("Importe") = Trunca(e.ProposedValue + e.Row("ImporteIva"))
                Grid.Refresh()
            End If
        End If

        If e.Column.ColumnName.Equals("Alicuota") Then
            If IsDBNull(e.Row("Alicuota")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue >= 100 Then
                MsgBox("Alicuota Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Alicuota")
                Exit Sub
            End If
            Dim Esta As Boolean
            For Each Item As Double In TablaIva
                If Item = e.ProposedValue Then Esta = True : Exit For
            Next
            If Esta = False Then
                MsgBox("Alicuota no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Alicuota")
            End If
            If Not IsDBNull(e.Row("Neto")) Then
                e.Row("Neto") = Trunca(e.Row("Neto") / (1 + e.ProposedValue / 100))
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Neto") / 100)
                e.Row("Importe") = Trunca(e.Row("Neto") + e.Row("ImporteIva"))
                Grid.Refresh()
            End If
        End If

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.Row("Cuenta")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Numero") Then
            If IsDBNull(e.Row("Numero")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.Row.Item("MedioPago") = 2 Then
                If (Not e.ProposedValue = 0 Or Not e.ProposedValue.ToString = "0") And PTipoNota <> 600 Then
                    ' SI NO ESTÁ DENTRO DEL RANGO MUESTRA MENSAJE DE ERROR. DE SI ESTARLO, VERIFICA QUE NO SE REPITA.
                    ' EN AMBOS CASOS, EL VALOR DE LA CELDA VUELVE A SER EL ANTERIOR AL "PROPUESTO". ES DECIR, BORRA EL CAMBIO.
                    If e.ProposedValue > e.Row.Item("NumeracionFinal") Or e.ProposedValue < e.Row.Item("NumeracionInicial") Then
                        MsgBox("El Número De Cheque Está Fuera Del Rango De La Numeración De La Chequera!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : e.ProposedValue = e.Row.Item("Numero")
                    Else
                        If DtGrid.Select("Numero = " & e.ProposedValue & " And Banco = " & e.Row.Item("Banco") & " And Cuenta = " & e.Row.Item("Cuenta") & "").Length <> 0 Then
                            MsgBox("El Número De Cheque Ya Está En Uso En La Chequera!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : e.ProposedValue = e.Row.Item("Numero")
                        End If
                    End If
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Cambio") Then
            If IsDBNull(e.Row("Cambio")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca3(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Bultos") Then
            If IsDBNull(e.Row("Bultos")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("Comprobante")) Then
            If IsDBNull(e.Row("Comprobante")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("EmisorCheque")) Then
            If IsDBNull(e.Row("EmisorCheque")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If (e.Column.ColumnName.Equals("Fecha") Or e.Column.ColumnName.Equals("FechaComprobante")) Then
            If IsDBNull(e.Row("Fecha")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = "1/1/1800"
        End If

        CalculaTotales()

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If DtGrid.Rows.Count > GLineasPagoRecibos And (PTipoNota <> 600 And PTipoNota <> 604 And PTipoNota <> 64 And PTipoNota <> 60) Then
            MsgBox("Supera Cantidad Items Permitidos.", MsgBoxStyle.Information) '
            e.Row.Delete()
            CalculaTotales()
            Exit Sub
        End If

        CalculaTotales()

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick
        'funciona solo con valor de Grid.Columns("eCheq").ReadOnly = True o Grid.ReadOnly = true

        If Grid.Columns(e.ColumnIndex).Name = "eCheq" Then
            Dim chkCell As DataGridViewCheckBoxCell = Grid.Rows(e.RowIndex).Cells("eCheq")
            chkCell.Value = Not chkCell.Value
            If chkCell.Value Then
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value <> 3 Then
                    Me.Grid.Rows(e.RowIndex).Cells("eCheq").Value = Not chkCell.Value
                End If
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3 And (PTipoNota = 600 Or PTipoNota = 65 Or PTipoNota = 64 Or PTipoNota = 6000) Then
                    Me.Grid.Rows(e.RowIndex).Cells("eCheq").Value = Not chkCell.Value
                End If
            Else
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 1002 Then
                    Me.Grid.Rows(e.RowIndex).Cells("eCheq").Value = Not chkCell.Value
                End If
                If Grid.Rows(e.RowIndex).Cells("Concepto").Value = 3 And (PTipoNota = 600 Or PTipoNota = 65 Or PTipoNota = 64 Or PTipoNota = 6000) Then
                    Me.Grid.Rows(e.RowIndex).Cells("eCheq").Value = Not chkCell.Value
                End If
            End If
        End If

    End Sub
    Private Sub ButtonAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgregar.Click

        If ComboConcepto.SelectedValue = 0 Or ComboNumerico.Value = 0 Or TextBoxBanco.Text = "" Or TextBoxCuenta.Text = "" Then
            MsgBox("DEBE ELEGIR CORRECTAMENTE LOS SIGUIENTES CAMPOS: CONCEPTO - CANTIDAD DE FILAS - BANCO - CUENTA.", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        End If

        Dim Row As DataRow

        For i As Integer = 0 To ComboNumerico.Value - 1
            Row = DtGrid.NewRow()
            Row("Item") = 0
            Row("MedioPago") = ComboConcepto.SelectedValue
            Row("Detalle") = ""
            Row("Neto") = 0
            Row("Alicuota") = 0
            Row("ImporteIva") = 0
            Row("Banco") = BancoPredeterminado
            Row("Fecha") = "1/1/1800"
            Row("Cuenta") = CuentaPredeterminada
            Row("Serie") = SeriePredeterminada
            Row("EmisorCheque") = ""
            Row("Cambio") = 0
            Row("Importe") = 0
            Row("Bultos") = 0
            Row("Comprobante") = 0
            Row("FechaComprobante") = "1/1/1800"
            Row("ClaveCheque") = 0
            Row("ClaveChequeVisual") = 0
            Row("ClaveInterna") = 0
            Row("TieneLupa") = False
            Row("eCheq") = False
            Row("ID") = Grid.Rows.Count
            Row("NumeracionInicial") = 0
            Row("NumeracionFinal") = 0

            Select Case ComboConcepto.SelectedValue
                Case 2
                    Row("NumeracionInicial") = NumeracionInicialPredeterminada
                    Row("NumeracionFinal") = NumeracionfinalPredeterminada

                    Dim UltimoNumeroGrabado As Integer = HallaUltimoNumeroGrabado(BancoPredeterminado, CuentaPredeterminada)
                    Row("Numero") = HallaSecuenciaCheque(BancoPredeterminado, CuentaPredeterminada, UltimoNumeroGrabado)

                    If Row("Numero") = 0 Then Exit For
                    LlenaDTAux(UltimoNumeroPredeterminado, BancoPredeterminado, CuentaPredeterminada, NumeracionfinalPredeterminada)
                Case 1002
                    Row("eCheq") = True
                    Row("Serie") = "00"
                    Row("Numero") = 0
                Case 4, 9, 11, 14
                    Row("Serie") = ""
                    If GCuitEmpresa = GPatagonia Or GCuitEmpresa = GCuadroNorte Then
                        Row("Comprobante") = Row("ID")
                        Row("FechaComprobante") = Today.Date
                    End If
            End Select

            DtGrid.Rows.Add(Row)
        Next

        PresentaLineasGrid(Grid, PDtFormasPago, PTipoNota, PEsTr, PEsRetencionManual, PAbierto)

    End Sub
    Private Sub VerLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VerLupa.Click

        ListaBancos.PEsSeleccionaCuenta = True
        ListaBancos.PEsSoloPesos = True
        ListaBancos.PConChequera = True
        BancoPredeterminado = 0
        CuentaPredeterminada = 0
        ListaBancos.ShowDialog()
        If ListaBancos.PCuenta <> 0 Then
            BancoPredeterminado = ListaBancos.PBanco
            CuentaPredeterminada = ListaBancos.PCuenta
            SeriePredeterminada = ListaBancos.PSerie
            NumeracionInicialPredeterminada = ListaBancos.PNumeracionInicial
            NumeracionfinalPredeterminada = ListaBancos.PNumeracionFinal
            UltimoNumeroPredeterminado = ListaBancos.PUltimoNumero
            LlenaDTAux(UltimoNumeroPredeterminado, BancoPredeterminado, CuentaPredeterminada, NumeracionfinalPredeterminada)
        End If
        ListaBancos.Dispose()

        TextBoxBanco.Text = DtBancos.Select("CLAVE = " & BancoPredeterminado & "")(0).Item("Nombre")
        TextBoxCuenta.Text = CuentaPredeterminada
        If CuentaPredeterminada = 0 Then TextBoxCuenta.Text = ""

    End Sub
End Class