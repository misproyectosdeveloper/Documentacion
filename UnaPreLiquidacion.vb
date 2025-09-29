Option Explicit On
Imports System.Math
Public Class UnaPreLiquidacion
    Public EsConsignacion As Boolean
    Public EsReventa As Boolean
    '
    Dim DtGrid As DataTable
    Dim DtPreciosFinalesB As DataTable
    Dim DtPreciosFinalesN As DataTable
    Dim DTIVA As DataTable
    '
    Private WithEvents bs As New BindingSource
    '
    Dim ClienteAnt As Integer
    Dim Deposito As Integer
    Dim ProveedorOpr As Boolean
    Dim ConexionLiquidacion As String
    Dim Comision As Decimal
    Dim ComisionW As Decimal
    Dim DescargaW As Decimal
    Dim SeniaW As Decimal
    Dim TotalSenia As Decimal = 0
    Dim TablaIva(0) As Double
    Private Sub LiquidacionConsignatarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboProveedor.DataSource = ProveedoresDeFrutas()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0

        OpcionEmisorYLetra.PEsConDeposito = True
        OpcionEmisorYLetra.PEsProveedor = True
        OpcionEmisorYLetra.PEsLocal = True
        OpcionEmisorYLetra.PEsNoNegocio = True
        OpcionEmisorYLetra.PSoloArticulos = True
        OpcionEmisorYLetra.PictureCandado.Visible = False
        OpcionEmisorYLetra.PEsSinLetra = True
        OpcionEmisorYLetra.PEsSoloAltas = True
        OpcionEmisorYLetra.ShowDialog()
        ComboProveedor.SelectedValue = OpcionEmisorYLetra.PEmisor
        Deposito = OpcionEmisorYLetra.PDeposito
        OpcionEmisorYLetra.Dispose()
        If ComboProveedor.SelectedValue = 0 Then Me.Close() : Exit Sub

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombosGrid()

        If Not PermisoTotal Then PanelCandado.Visible = False : Grid.Columns("Candado").Visible = False

        CreaDtGrid()

        ArmaTablaIva(TablaIva)

        CompletaComboIVA(TablaIva)

        If Deposito <> 0 Then
            Label14.Text = "Solo Lotes del Deposito: " & NombreDeposito(Deposito)
        Else
            Label14.Text = ""
        End If

        ArmaArchivo()

    End Sub
    Private Sub UnaPreLiquidacion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        Grid.Enabled = True
        ButtonAceptar.Enabled = True
        TextImporteXBulto.Text = ""
        TextComision.ReadOnly = False
        TextImporteXBulto.ReadOnly = False
        TextDirecto.ReadOnly = False
        TextTotalSenia.Text = ""

        LiquidacionConsignatarios_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonIgualProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIgualProveedor.Click

        Grid.Enabled = True
        ButtonAceptar.Enabled = True
        TextImporteXBulto.Text = ""
        TextComision.ReadOnly = False
        TextImporteXBulto.ReadOnly = False
        TextDirecto.ReadOnly = False
        TextTotalSenia.Text = ""

        ArmaArchivo()

    End Sub
    Private Sub TextDirecto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDirecto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDirecto_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextDirecto.Text, GDecimales)

    End Sub
    Private Sub TextDirecto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDirecto.Validating

        If Not IsNumeric(TextDirecto.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDirecto.Focus()
            Exit Sub
        Else : TextDirecto.Text = FormatNumber(TextDirecto.Text, GDecimales, True, True, True)
            If CDbl(TextDirecto.Text) > 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextDirecto.Focus()
                Exit Sub
            End If
            TextAutorizar.Text = FormatNumber(100 - CDbl(TextDirecto.Text), GDecimales, True, True, True)
        End If

    End Sub
    Private Sub TextComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComision.KeyPress

        If Asc(e.KeyChar) = 13 Then TextComision_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextComision.Text, GDecimales)

    End Sub
    Private Sub TextComision_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextComision.Validating

        If TextComision.Text = "" Then TextComision.Text = "0"

        TextComision.Text = FormatNumber(CDbl(TextComision.Text), GDecimales, True, True, True)
        If CDbl(TextComision.Text) >= 100 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextComision.Text = FormatNumber(Comision, GDecimales)
            TextComision.Focus()
            Exit Sub
        End If

        Comision = CDbl(TextComision.Text)

        CalculaTotales()

    End Sub
    Private Sub TextIvaComision_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIvaComision.KeyPress

        If Asc(e.KeyChar) = 13 Then TextIvaComision_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextIvaComision.Text, GDecimales)

    End Sub
    Private Sub TextIvaComision_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextIvaComision.Validating

        If TextIvaComision.Text = "" Then TextIvaComision.Text = "0"

        TextIvaComision.Text = FormatNumber(CDbl(TextIvaComision.Text), GDecimales, True, True, True)
        If CDbl(TextIvaComision.Text) >= 100 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextIvaComision.Text = FormatNumber(AgregaIvaComision(100), GDecimales)
            TextIvaComision.Focus()
            Exit Sub
        End If

    End Sub
    Private Sub TextImporteXBulto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImporteXBulto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextImporteXBulto_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextImporteXBulto.Text, GDecimales)

    End Sub
    Private Sub TextImporteXBulto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextImporteXBulto.Validating

        If TextImporteXBulto.Text <> "" Then
            TextImporteXBulto.Text = FormatNumber(CDbl(TextImporteXBulto.Text), GDecimales)
        End If

        CalculaTotales()

    End Sub
    Private Sub TextSeniaXBulto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSeniaXBulto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextSeniaXBulto_Validating(Nothing, Nothing) : Exit Sub

        EsNumericoConSigno(e.KeyChar, TextSeniaXBulto.Text, GDecimales)

    End Sub
    Private Sub TextSeniaXBulto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextSeniaXBulto.Validating

        If TextSeniaXBulto.Text = "-" Then TextSeniaXBulto.Text = ""

        If TextSeniaXBulto.Text <> "" Then
            TextSeniaXBulto.Text = FormatNumber(CDec(TextSeniaXBulto.Text), GDecimales)
        End If

        CalculaTotales()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not ProveedorOpr And CDec(TextDirecto.Text) <> 0 Then
            If MsgBox("Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                If Not IgualIva(Row.Cells("Iva").Value) Then
                    MsgBox("ERROR, Lotes Seleccionados no tienen igual iva. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Exit Sub
                End If
                Exit For
            End If
        Next

        If LotesElegidos() > GLineasPreLiquidacion Then
            MsgBox("ERROR, Supera Cantidad de Lotes permitidos:( " & GLineasPreLiquidacion & " ).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        '   If CDec(TextTotalSenia.Text) > TotalSenia Then
        '        MsgBox("ERROR, Seña Mayor a la Definida en Ingreso de Lotes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
        '        Exit Sub
        '   End If

        If CalculaCantidad() <> CDec(TextUnidades.Text) Then
            MsgBox("Cantidad de Unidades de Lotes a Liquidar no coincide con Total de Unidades Liquidada.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        If CDec(TextTotal.Text) < 0 Then
            MsgBox("ERROR, Total NO Debe ser Negativo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim Esta As Boolean
        For Each Item As Double In TablaIva
            If Item = CDec(TextIvaComision.Text) Then Esta = True : Exit For
        Next
        If Esta = False Then
            MsgBox("Iva Comisión no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Not IgualIva(CDec(TextIvaComision.Text)) Then
            If MsgBox("Iva Comisión NO COINCIDE con Iva de los Articulos. Desea Continuar de todas Maneras?.(Si/No)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Dim BrutoOriginal As Decimal = 0
        If EsReventa Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    BrutoOriginal = BrutoOriginal + Row.Cells("Total").Value
                End If
            Next
        End If

        UnaLiquidacion.PLiquidacion = 0
        UnaLiquidacion.PEsReventa = EsReventa
        UnaLiquidacion.PEsConsignacion = EsConsignacion
        UnaLiquidacion.PProveedor = ComboProveedor.SelectedValue
        UnaLiquidacion.PComision = CDec(TextComision.Text)
        UnaLiquidacion.PIvaComision = CDec(TextIvaComision.Text)
        UnaLiquidacion.PMontoComision = Trunca(ComisionW)
        UnaLiquidacion.PDescarga = Trunca(DescargaW)
        UnaLiquidacion.PSenia = SeniaW
        UnaLiquidacion.PDirecto = CDec(TextDirecto.Text)
        UnaLiquidacion.PBruto = CDec(TextBruto.Text)
        UnaLiquidacion.PNeto = CDec(TextNeto.Text)
        UnaLiquidacion.PListaDeLotes = New List(Of FilaLiquidacion)
        Dim Fila As FilaLiquidacion
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Fila = New FilaLiquidacion
                Fila.Lote = Row.Cells("Lote").Value
                Fila.Secuencia = Row.Cells("Secuencia").Value
                Fila.Operacion = Row.Cells("Operacion").Value
                Fila.PrecioF = Row.Cells("PrecioF").Value
                If EsReventa Then
                    Fila.PrecioF = Fila.PrecioF * CDbl(TextBruto.Text) / BrutoOriginal
                End If
                Fila.Articulo = Row.Cells("Articulo").Value
                Fila.Iniciales = Row.Cells("Ingresados").Value
                Fila.Merma = Row.Cells("Merma").Value
                Fila.Aliquidar = Row.Cells("Cantidad").Value
                Fila.RemitoGuia = Row.Cells("RemitoGuia").Value
                Fila.Medida = Row.Cells("Medida").Value
                UnaLiquidacion.PListaDeLotes.Add(Fila)
            End If
        Next
        If UnaLiquidacion.PListaDeLotes.Count = 0 Then
            MsgBox("Debe Elegir Lotes a Liquidar.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        UnaLiquidacion.ShowDialog()
        If UnaLiquidacion.PActualizacionOk Then
            Grid.Enabled = False
            ButtonAceptar.Enabled = False
            TextComision.ReadOnly = True
            TextImporteXBulto.ReadOnly = True
            TextDirecto.ReadOnly = True
        End If
        UnaLiquidacion.Dispose()

    End Sub
    Private Sub ArmaArchivo()

        Dim Dt As New DataTable
        Dim SqlB As String = ""
        Dim SqlN As String = ""

        If Not Tablas.Read("SELECT Comision,Directo,Opr FROM Proveedores WHERE Clave = " & ComboProveedor.SelectedValue & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            MsgBox("Proveedor No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        Dim SqlDeposito As String
        If Deposito <> 0 Then
            SqlDeposito = " AND Deposito = " & Deposito
        End If

        ProveedorOpr = Dt.Rows(0).Item("Opr")
        Comision = Dt.Rows(0).Item("Comision")
        TextIvaComision.Text = Format(AgregaIvaComision(100), "0.00")
        TextDirecto.Text = Format(Dt.Rows(0).Item("Directo"), "0.00")
        TextAutorizar.Text = Format(100 - Dt.Rows(0).Item("Directo"), "0.00")
        If Not PermisoTotal Then
            TextDirecto.Text = Format(100, "0.00")
            TextAutorizar.Text = Format(0, "0.00")
        End If

        If EsConsignacion Then
            SqlB = "SELECT 1 AS Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,KilosXUnidad FROM Lotes WHERE Cantidad <> Baja AND Liquidado = 0 AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND TipoOperacion = 1 AND Lotes.Proveedor = " & ComboProveedor.SelectedValue & SqlDeposito & ";"
            SqlN = "SELECT 2 AS Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,KilosXUnidad FROM Lotes WHERE Cantidad <> Baja AND Liquidado = 0 AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND TipoOperacion = 1 AND Lotes.Proveedor = " & ComboProveedor.SelectedValue & SqlDeposito & ";"
        End If
        If EsReventa Then
            SqlB = "SELECT 1 AS Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,KilosXUnidad FROM Lotes WHERE Cantidad <> Baja AND Liquidado = 0 AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND TipoOperacion = 2 AND Lotes.Proveedor = " & ComboProveedor.SelectedValue & SqlDeposito & ";"
            SqlN = "SELECT 2 AS Operacion,Lote,Secuencia,Articulo,Proveedor,Fecha,KilosXUnidad FROM Lotes WHERE Cantidad <> Baja AND Liquidado = 0 AND Lotes.Lote = Lotes.LoteOrigen AND Lotes.Secuencia = Lotes.SecuenciaOrigen AND Lotes.Deposito = Lotes.DepositoOrigen AND TipoOperacion = 2 AND Lotes.Proveedor = " & ComboProveedor.SelectedValue & SqlDeposito & ";"
        End If

        Dt = New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        'Saco los lotes de una orden de compra porque no se LIQUIDAN.
        Dim LoteWAnt As Integer = 0
        Dim TieneOrdenCompraAnt As Boolean
        For Each Row As DataRow In Dt.Rows
            TieneOrdenCompraAnt = False
            If Row("Lote") <> LoteWAnt Then
                TieneOrdenCompraAnt = TieneOrdenCompra(Row("Lote"), Row("Operacion"))
                LoteWAnt = Row("Lote")
            End If
            If TieneOrdenCompraAnt = True Then
                Row.Delete()
            End If
        Next
        Dt.AcceptChanges()
        '-------------------------------------------------------------

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        Dim Baja As Decimal
        Dim Merma As Decimal
        Dim MermaTr As Decimal
        Dim Importe As Decimal
        Dim Senia As Decimal
        Dim Stock As Decimal
        Dim RowGrid As DataRow
        Dim PrecioF As Decimal
        Dim CantidadInicial As Decimal
        Dim Descarga As Decimal
        Dim DescargaSinIva As Decimal

        TotalSenia = 0
        Dim RemitoGuia As Double = 0
        Dim LoteAnt As Integer = 0

        DtGrid.Clear()

        For Each Row As DataRowView In View
            If Not CalculaImportes(Row("Operacion"), Row("Lote"), Row("Secuencia"), Row("Proveedor"), PrecioF, CantidadInicial, _
                                    Baja, MermaTr, Merma, Senia, Descarga, DescargaSinIva, Stock) Then Me.Close() : Exit Sub
            If PrecioF <> 0 Then
                RowGrid = DtGrid.NewRow()
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("Lote") = Row("Lote")
                RowGrid("Secuencia") = Row("Secuencia")
                RowGrid("Articulo") = Row("Articulo")
                RowGrid("Iva") = HallaIva(Row("Articulo"))
                RowGrid("Ingresados") = Trunca(CantidadInicial - Baja)
                RowGrid("Bruto") = Importe
                RowGrid("Descarga") = Descarga
                RowGrid("Senia") = Senia
                '    TotalSenia = TotalSenia + RowGrid("Senia")
                If Merma < 0 Then Merma = 0
                Dim MermaAux As Decimal = 0
                '  arreglo      If MermaTr > 0 Then
                If MermaTr <> -100 Then
                    MermaAux = MermaTr
                Else
                    If EsReventa Then                   'EsReventa y mermaTr = -100 (con mermaTr = -100 significa que debe tomar toda la merma).
                        MermaAux = 0
                    Else
                        MermaAux = Round(Merma)   'Merma=Merma + descarte
                    End If
                End If
                RowGrid("Merma") = MermaAux
                If Abs(Stock) < 0.5 Then Stock = 0
                RowGrid("Stock") = Stock
                RowGrid("Cantidad") = Trunca(CantidadInicial - Baja - MermaAux)
                If PrecioF <> 0 Then RowGrid("PrecioF") = PrecioF
                RowGrid("Total") = CalculaNeto(RowGrid("Cantidad"), RowGrid("PrecioF"))
                If Row("Lote") <> LoteAnt Then
                    If Not HallaRemitoGuia(Row("Lote"), Row("Operacion"), RemitoGuia) Then Me.Close() : Exit Sub
                    LoteAnt = Row("Lote")
                End If
                RowGrid("RemitoGuia") = RemitoGuia
                Dim AGranel As Boolean
                RowGrid("Medida") = ""
                HallaAGranelYMedida(Row("Articulo"), AGranel, RowGrid("Medida"))
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.Columns("Sel").ReadOnly = True
        Grid.DataSource = bs
        bs.DataSource = DtGrid

        CalculaTotales()

        Grid.Focus()
        Dt.Dispose()

    End Sub
    Private Sub CompletaComboIVA(ByVal TablaIVA() As Double)

        DTIVA = New DataTable

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Decimal")
        DTIVA.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DTIVA.Columns.Add(Nombre)

        For I As Integer = 0 To TablaIVA.Length - 1
            Dim FilaIVA As DataRow = DTIVA.NewRow()
            FilaIVA("Clave") = TablaIVA(I)
            FilaIVA("Nombre") = TablaIVA(I)
            DTIVA.Rows.Add(FilaIVA)
        Next

        ComboIVA.DataSource = DTIVA
        Dim RowTablaIVA As DataRow = ComboIVA.DataSource.NewRow()
        RowTablaIVA("Clave") = -1
        RowTablaIVA("Nombre") = ""
        ComboIVA.DataSource.Rows.Add(RowTablaIVA)
        ComboIVA.DisplayMember = "Nombre"
        ComboIVA.ValueMember = "Clave"
        ComboIVA.SelectedValue = -1

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Ingresados As New DataColumn("Ingresados")
        Ingresados.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Ingresados)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Iva)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Merma)

        Dim PrecioF As New DataColumn("PrecioF")
        PrecioF.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(PrecioF)

        Dim Bruto As New DataColumn("Bruto")
        Bruto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Bruto)

        Dim Flete As New DataColumn("Flete")
        Flete.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Flete)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Senia)

        Dim BoniComercial As New DataColumn("BoniComercial")
        BoniComercial.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(BoniComercial)

        Dim BoniLogistica As New DataColumn("BoniLogistica")
        BoniLogistica.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(BoniLogistica)

        Dim IngresoBruto As New DataColumn("IngresoBruto")
        IngresoBruto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(IngresoBruto)

        Dim ImpDebCred As New DataColumn("ImpDebCred")
        ImpDebCred.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImpDebCred)

        Dim Descarga As New DataColumn("Descarga")
        Descarga.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Descarga)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Total)

        Dim RemitoGuia As New DataColumn("RemitoGuia")
        RemitoGuia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(RemitoGuia)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Stock)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub CalculaTotales()

        If EsReventa Then
            CalculaTotalesReventa()
            Exit Sub
        End If

        Dim Unidades As Decimal = 0
        Dim Iniciales As Decimal = 0
        Dim Bruto As Decimal = 0
        Dim Neto As Decimal = 0
        Dim Total As Decimal = 0
        ComisionW = 0
        DescargaW = 0
        SeniaW = 0
        Dim DescargaWWW As Decimal = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Unidades = Unidades + Row.Cells("Cantidad").Value
                Iniciales = Iniciales + Row.Cells("Ingresados").Value
                Bruto = Bruto + Row.Cells("Total").Value
                'Calcula Comision.
                ComisionW = ComisionW + CalculaIva(1, Row.Cells("Total").Value, Comision)
                'Calcula Descarga.
                If TextImporteXBulto.Text = "" Then
                    DescargaWWW = Trunca(Row.Cells("Descarga1").Value / Row.Cells("Ingresados").Value)
                    DescargaW = DescargaW + Row.Cells("Descarga1").Value
                Else
                    DescargaWWW = CDbl(TextImporteXBulto.Text)
                    DescargaW = DescargaW + CalculaNeto(Row.Cells("Ingresados").Value, DescargaWWW)
                End If
                'Calcula Senia.
                If TextSeniaXBulto.Text = "" Then
                    '  SeniaW = SeniaW + Row.Cells("Senia").Value
                Else
                    '  SeniaW = SeniaW + CalculaNeto(Row.Cells("Ingresados").Value, CDbl(TextSeniaXBulto.Text))
                End If
                Dim SubTotal As Double = Row.Cells("Total").Value - CalculaIva(1, Row.Cells("Total").Value, Comision) - CalculaNeto(Row.Cells("Ingresados").Value, DescargaWWW)
            End If
        Next

        If TextSeniaXBulto.Text <> "" Then
            SeniaW = CDec(TextSeniaXBulto.Text)
        End If

        TextUnidades.Text = FormatNumber(Unidades, GDecimales)
        TextBruto.Text = FormatNumber(Bruto, GDecimales)
        TextComision.Text = FormatNumber(Comision, GDecimales)

        Neto = Bruto - ComisionW - DescargaW
        Total = Neto + SeniaW

        TextImporteComision.Text = "-" & FormatNumber(ComisionW, GDecimales)
        TextDescarga.Text = "-" & FormatNumber(DescargaW, GDecimales)
        TextTotalSenia.Text = FormatNumber(SeniaW, GDecimales)
        TextNeto.Text = FormatNumber(Neto, GDecimales)
        TextTotal.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Sub CalculaTotalesReventa()

        Dim Unidades As Decimal = 0
        Dim Iniciales As Decimal = 0
        Dim Bruto As Decimal = 0
        Dim Neto As Decimal = 0
        Dim Total As Decimal = 0

        ComisionW = 0
        DescargaW = 0
        SeniaW = 0
        Dim DescargaWWW As Decimal = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Unidades = Unidades + Row.Cells("Cantidad").Value
                Iniciales = Iniciales + Row.Cells("Ingresados").Value
                'Calcula Descarga.
                Dim DescargaWWWW As Decimal = 0
                If TextImporteXBulto.Text = "" Then
                    DescargaWWW = Trunca(Row.Cells("Descarga1").Value / Row.Cells("Ingresados").Value)
                    DescargaW = DescargaW + Row.Cells("Descarga1").Value
                    DescargaWWWW = Row.Cells("Descarga1").Value
                Else
                    DescargaWWW = CDbl(TextImporteXBulto.Text)
                    DescargaW = DescargaW + CalculaNeto(Row.Cells("Ingresados").Value, DescargaWWW)
                    DescargaWWWW = CalculaNeto(Row.Cells("Ingresados").Value, DescargaWWW)
                End If
                'Calcula Senia.
                If TextSeniaXBulto.Text = "" Then
                    '   SeniaW = SeniaW + Row.Cells("Senia").Value
                Else
                    '   SeniaW = SeniaW + CalculaNeto(Row.Cells("Ingresados").Value, CDbl(TextSeniaXBulto.Text))
                End If
                Dim BrutoWW As Decimal = (Row.Cells("Total").Value + DescargaWWWW) / (1 - Comision / 100)
                Bruto = Bruto + BrutoWW
                'Calcula Comision.
                ComisionW = ComisionW + CalculaIva(1, BrutoWW, Comision)
                Dim SubTotal As Double = BrutoWW - CalculaIva(1, BrutoWW, Comision) - CalculaNeto(Row.Cells("Ingresados").Value, DescargaWWW)
            End If
        Next

        If TextSeniaXBulto.Text <> "" Then
            SeniaW = CDec(TextSeniaXBulto.Text)
        End If

        TextUnidades.Text = FormatNumber(Unidades, GDecimales)
        TextBruto.Text = FormatNumber(Bruto, GDecimales)
        TextComision.Text = FormatNumber(Comision, GDecimales)

        Neto = Bruto - ComisionW - DescargaW
        Total = Neto + SeniaW

        TextImporteComision.Text = "-" & FormatNumber(ComisionW, GDecimales)
        TextDescarga.Text = "-" & FormatNumber(DescargaW, GDecimales)
        TextTotalSenia.Text = FormatNumber(SeniaW, GDecimales)
        TextNeto.Text = FormatNumber(Neto, GDecimales)
        TextTotal.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Function IgualIva(ByVal Iva As Double) As Boolean

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                If Row.Cells("Iva").Value <> Iva Then Return False
            End If
        Next

        Return True

    End Function
    Private Function LotesElegidos() As Integer

        Dim Cantidad As Integer = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then Cantidad = Cantidad + 1
        Next

        Return Cantidad

    End Function
    Private Function CalculaCantidad() As Decimal

        Dim Cantidad As Decimal

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Cantidad = Cantidad + Row.Cells("Cantidad").Value
            End If
        Next

        Return Cantidad

    End Function
    Private Function TieneOrdenCompra(ByVal Lote As Integer, ByVal Operacion As Integer) As Boolean

        Dim ConexionStr As String
        Dim Dt As New DataTable
        Dim Respuesta As Boolean

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT OrdenCompra FROM IngresoMercaderiasCabeza WHERE OrdenCompra <> 0 AND Lote = " & Lote & ";", ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then
            Respuesta = False
        Else
            Respuesta = True
        End If

        Return Respuesta
        Dt.Dispose()

    End Function
    Private Function CalculaImportes(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Proveedor As Integer, ByRef PrecioF As Decimal, ByRef CantidadInicial As Decimal, _
            ByRef Baja As Decimal, ByRef MermaTr As Decimal, ByRef Merma As Decimal, ByRef Senia As Decimal, ByRef Descarga As Decimal, ByRef DescargaSinIva As Decimal, ByRef Stock As Decimal) As Boolean

        Dim OrdenCompra As Decimal
        Dim Envase As Integer = 0
        Dim KilosXUnidad As Decimal
        Dim Descarte As Decimal = 0
        Dim FechaIngreso As DateTime
        Dim EsReventa As Boolean
        Dim Articulo As Integer
        Dim Fecha As Date

        PrecioF = 0
        CantidadInicial = 0
        Baja = 0
        MermaTr = 0
        Merma = 0
        Senia = 0
        Descarga = 0
        DescargaSinIva = 0
        Stock = 0

        Dim Dt As New DataTable

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
                PrecioF = Row("PrecioF")
                CantidadInicial = Row("Cantidad")
                Baja = Row("Baja")
                Articulo = Row("Articulo")
                Fecha = Row("Fecha")
                MermaTr = Row("MermaTr")
                OrdenCompra = Row("OrdenCompra")
                FechaIngreso = Row("Fecha")
                Envase = HallaEnvase(Row("Articulo"))
                If Row("TipoOperacion") = 2 Then EsReventa = True
                If Envase < 0 Then Return False
                If Row("TipoOperacion") = 2 Then       'Reventa.
                    If Row("Senia") = -1 Then
                        Senia = CalculaSenia(Envase, Row("Fecha"))  'es seña que se cobra al Cliente y sale tabla de envases.
                        If Senia < 0 Then Return False
                    Else
                        Senia = Row("Senia")
                    End If
                    Senia = CalculaNeto(CantidadInicial - Baja, Senia)
                End If
            End If
            Stock = Stock + Row("Stock") * Row("KilosXUnidad")
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
            Descarte = Descarte + Row("Descarte") * Row("KilosXUnidad")
        Next

        Stock = Trunca(Stock / KilosXUnidad)
        Merma = Trunca(Merma / KilosXUnidad)
        Descarte = Trunca(Descarte / KilosXUnidad)
        Merma = Merma + Descarte
        'Halla costo de descarga.
        If Not BuscaVigenciaValorAlicuota(11, FechaIngreso, Descarga, DescargaSinIva, Envase) Then Return False
        Descarga = CalculaNeto(CantidadInicial, Descarga)
        DescargaSinIva = CalculaNeto(CantidadInicial, DescargaSinIva)

        If EsReventa Then
            If PrecioF = 0 And OrdenCompra <> 0 Then
                PrecioF = HallaOrdenCompraYPrecio(Proveedor, Articulo, Operacion, Lote)
            End If
            If PrecioF = 0 Then
                PrecioF = HallaPrecioDeListaDePrecios(Proveedor, Lote, Fecha, Articulo, KilosXUnidad, Operacion)
            End If
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Sub ComboIVA_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboIVA.SelectedIndexChanged

        For Each FilaGrid As DataGridViewRow In Grid.Rows
            If ComboIVA.SelectedValue <> -1 Then
                Grid.DataSource.SuspendBinding()
                FilaGrid.Visible = True
                If FilaGrid.Cells("Iva").Value <> ComboIVA.SelectedValue Then FilaGrid.Visible = False
                Grid.DataSource.ResumeBinding()
                Continue For
            End If
            FilaGrid.Visible = True
        Next
    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------               MANEJO DEL GRID.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Merma" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "PrecioF" Then
            If IsDBNull(e.Value) Then e.Value = 0
            e.Value = Format(e.Value, "0.#####")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Iva" Then
            If IsDBNull(e.Value) Then e.Value = 0
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Senia" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoGuia" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        'Sel cambia de valor solo con: Grid.Columns("Sel").ReadOnly = True o Grid.ReadOnly = true

        If Grid.Columns(e.ColumnIndex).Name = "Sel" Then
            Dim Check As New DataGridViewCheckBoxCell
            Check = (Grid.Rows(e.RowIndex).Cells("Sel"))
            Check.Value = Not Check.Value
            If Check.Value Then
                If Not IgualIva(Grid.Rows(e.RowIndex).Cells("Iva").Value) Then
                    MsgBox("ERROR, Iva del Lote Seleccionado difiere de los ya elegidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                End If
                If LotesElegidos() > GLineasPreLiquidacion Then    '28
                    MsgBox("ERROR, Supera Cantidad de Lotes permitidos:( " & GLineasPreLiquidacion & " ).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                End If
            End If
            CalculaTotales()
        End If

    End Sub

   
End Class