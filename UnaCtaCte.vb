Public Class UnaCtaCte
    Public PTipoEmisor As Integer
    Public PEmisor As Integer
    Public DtGrid As DataTable
    '
    Private WithEvents bs As New BindingSource
    Dim PView As DataView
    '
    Dim ConDetalle As Boolean
    Dim SinSaldoAnterior As Boolean
    Dim SqlB As String
    Dim SqlN As String
    Dim Emisor As Integer
    Private Sub UnaCtaCte_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Me.Top = 50

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
            CheckSinBalanceo.Visible = False
            CheckConBalanceo.Visible = False
            If PTipoEmisor = 2 Then CheckConBalanceo.Checked = True
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PTipoEmisor = 1 Then
            LabelClienteProveedor.Text = "Cliente"
            LabelCanalVenta.Visible = True
            ComboBoxCanalVenta.Visible = True
            Me.BackColor = Color.LemonChiffon
        Else : LabelClienteProveedor.Text = "Proveedor"
            Me.BackColor = Color.PowderBlue
            'CheckSoloDomesticos.Visible = False
            'CheckSoloExterior.Visible = False
        End If

        If PTipoEmisor = 1 Then
            ComboBoxCanalVenta.DataSource = Tablas.Leer("SELECT Clave, Nombre FROM Tablas WHERE Tipo = 23")
            Dim RowCanalVenta As DataRow = ComboBoxCanalVenta.DataSource.NewRow
            RowCanalVenta = ComboBoxCanalVenta.DataSource.NewRow
            RowCanalVenta("Clave") = 0
            RowCanalVenta("Nombre") = ""
            ComboBoxCanalVenta.DataSource.rows.add(RowCanalVenta)
            ComboBoxCanalVenta.DisplayMember = "Nombre"
            ComboBoxCanalVenta.ValueMember = "Clave"
            ComboBoxCanalVenta.SelectedValue = 0
            With ComboBoxCanalVenta
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
        End If

        If PTipoEmisor = 1 Then
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes ORDER BY Nombre;")
        End If
        If PTipoEmisor = 2 Then
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 ORDER BY Nombre;")
        End If
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = PEmisor
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PTipoEmisor = 1 Then
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '' ORDER BY Nombre;")
        End If
        If PTipoEmisor = 2 Then
            ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE TipoOperacion <> 4 AND Alias <> '' ORDER BY Alias;")
        End If
        Row = ComboAlias.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAlias.DataSource.Rows.Add(Row)
        ComboAlias.DisplayMember = "Alias"
        ComboAlias.ValueMember = "Clave"
        ComboAlias.SelectedValue = 0
        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27 ORDER BY Nombre;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        ComboMoneda.SelectedValue = 1
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-6)

        CreaDtGrid()

    End Sub
    Private Sub UnaCtaCte_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

        If e.KeyData = 112 Then
            If SinSaldoAnterior Then
                SinSaldoAnterior = False
            Else
                SinSaldoAnterior = True
            End If
            ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            If ComboEmisor.SelectedValue <> ComboAlias.SelectedValue Then
                MsgBox("Debe Seleccionar Razon Social o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                ComboEmisor.Focus()
                Exit Sub
            End If
        End If

        Emisor = 0

        If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Emisor = ComboAlias.SelectedValue

        PreparaArchivos()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Nombre As String
        Dim NombreAlias As String
        If Emisor <> 0 Then
            For Each Row As DataRow In ComboEmisor.DataSource.rows
                If Row("Clave") = Emisor Then Nombre = Row("Nombre") : Exit For
            Next
            For Each Row As DataRow In ComboAlias.DataSource.rows
                If Row("Clave") = Emisor Then NombreAlias = Row("Alias") : Exit For
            Next
            Nombre = Nombre & " (" & NombreAlias & ")"
        End If

        If PTipoEmisor = 1 And Not CheckDetalle.Checked Then
            Grid.Columns("CanalVenta").Visible = True
            Grid.Columns("CanalDistribucion").Visible = True
        End If

        GridAExcel(Grid, Date.Now, "Informe Cuentas Corrientes", Nombre)

        If PTipoEmisor = 1 And Not CheckDetalle.Checked Then
            Grid.Columns("CanalVenta").Visible = False
            Grid.Columns("CanalDistribucion").Visible = False
        End If


        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNoImputados_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNoImputados.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        InformeDocumentosNoImputados(1, DateTimeDesde.Value, DateTimeHasta.Value, CheckAbierto.Checked, CheckCerrado.Checked, SqlB, SqlN, ComboEmisor.DataSource, ComboMoneda.SelectedValue, Tipo.DataSource)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonSoloTotales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSoloTotales.Click

        Dim Comienzo As Integer
        Dim Final As Integer
        Dim LineaTotal As Integer = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            If DtGrid.Rows(I).Item("Tipo") = 7000000 Then
                LineaTotal = I
                Comienzo = I - 1
            End If
            If DtGrid.Rows(I).Item("Tipo") = 6000000 Then
                DtGrid.Rows(LineaTotal).Item("Emisor") = DtGrid.Rows(I).Item("Emisor")
                Final = I
                BorraRowGrid(Comienzo, Final)
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub ButtonCompQueLoImputan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCompQueLoImputan.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Tipo As Integer = Grid.CurrentRow.Cells("TipoOrigen").Value

        Select Case Tipo
            Case 7, 50, 60, 6, 600, 700
                MsgBox("Imputanciones en el Comprobante.")
                Exit Sub
        End Select

        If Grid.CurrentRow.Cells("TipoOrigen").Value = 0 Then Tipo = 900

        HallaDocumentosQueImputan(Tipo, Grid.CurrentRow.Cells("Comprobante").Value, Grid.CurrentRow.Cells("operacion").Value, False)

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If ComboEmisor.SelectedValue = 0 Then
            If PTipoEmisor = 1 Then
                MsgBox("Debe Seleccionar un Cliente. ", MsgBoxStyle.Information)
            Else
                MsgBox("Debe Seleccionar un Proveedor. ", MsgBoxStyle.Information)
            End If
            Exit Sub
        End If

        UnaImpresionRecibo.PEsAmbasOpr = False
        If CheckAbierto.Checked And CheckCerrado.Checked Then UnaImpresionRecibo.PEsAmbasOpr = True
        UnaImpresionRecibo.PNombreClienteCtaCte = ComboEmisor.Text
        UnaImpresionRecibo.PDesdeCtaCte = DateTimeDesde.Value
        UnaImpresionRecibo.PHastaCtaCte = DateTimeHasta.Value
        UnaImpresionRecibo.PMoneda = ComboMoneda.Text
        UnaImpresionRecibo.PtipoEmisor = PTipoEmisor
        UnaImpresionRecibo.GridCompro = Grid
        UnaImpresionCtaCte()

    End Sub
    Private Sub PreparaArchivos()

        'Trae la fecha sin hora "SELECT CAST(FLOOR(CAST(fecha AS FLOAT)) AS DATETIME) as fecha  FROM FacturasCabeza" para que lo muestre ordenado.
        'por fecha,tipo,comprobante.

        If CheckDetalle.Checked Then
            ConDetalle = True
        Else : ConDetalle = False
        End If

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckSinBalanceo.Checked And Not CheckConBalanceo.Checked Then
            CheckSinBalanceo.Checked = True
            CheckConBalanceo.Checked = True
        End If
        If Not CheckSoloDomesticos.Checked And Not CheckSoloExterior.Checked Then
            CheckSoloDomesticos.Checked = True
            CheckSoloExterior.Checked = True
        End If
        If Not CheckSinAsignar.Checked And Not CheckYaAsignados.Checked Then
            CheckSinAsignar.Checked = True
            CheckYaAsignados.Checked = True
        End If

        DtGrid.Clear()

        Dim SqlEmisorCliente As String = ""
        Dim SqlEmisorPorCuentaYOrden As String = ""
        Dim SqlEmisorEmisor As String = ""
        Dim SqlFacturaCliente As String = ""
        Dim SqlRemito As String = ""
        Dim SqlMoneda As String = ""
        Dim SqlMonedaNC As String = ""
        If PTipoEmisor = 1 Then
            If Emisor <> 0 Then
                SqlEmisorCliente = "Cliente = " & Emisor
                SqlEmisorPorCuentaYOrden = "PorCuentaYOrden = " & Emisor
                SqlEmisorEmisor = "Emisor = " & Emisor
                SqlFacturaCliente = "FacturasCabeza.Cliente = " & Emisor
            Else
                SqlEmisorCliente = "Cliente LIKE '%'"
                SqlEmisorPorCuentaYOrden = "PorCuentaYOrden LIKE '%'"
                SqlEmisorEmisor = "Emisor LIKE '%'"
                SqlFacturaCliente = "FacturasCabeza.Cliente LIKE '%'"
            End If
            If CheckSinRemitos.Checked Then
                SqlRemito = " AND Remito = -1"
            End If
        End If

        Dim SqlEmisorProveedor As String = ""
        Dim SqlEmisorACuenta As String = ""
        If PTipoEmisor = 2 Then
            If Emisor <> 0 Then
                SqlEmisorProveedor = "Proveedor = " & Emisor
                SqlEmisorEmisor = "Emisor = " & Emisor
                SqlEmisorACuenta = "ACuenta = " & Emisor
            Else
                SqlEmisorProveedor = "Proveedor LIKE '%'"
                SqlEmisorEmisor = "Emisor LIKE '%'"
                SqlEmisorACuenta = "ACuenta LIKE '%'"
                '                SqlEmisorProveedor = "Proveedor <> " & gProveedorEgresoCaja            
                '                SqlEmisorEmisor = "Emisor <> " & gProveedorEgresoCaja                
                '                SqlEmisorACuenta = "ACuenta <> " & gProveedorEgresoCaja               

            End If
        End If
        '-------Esto se Controla en la funcion: EsExteriorOLocalOk(), pero esta para traer menos fila en la select. 
        If CheckSoloDomesticos.Checked Then
            SqlMoneda = " AND Moneda = 1"
            If PTipoEmisor = 1 Then SqlMonedaNC = " AND NotasCreditoCabeza.Moneda = 1"
        End If
        If CheckSoloExterior.Checked Then
            SqlMoneda = " AND Moneda > 1"
            If PTipoEmisor = 1 Then SqlMonedaNC = " AND NotasCreditoCabeza.Moneda > 1"
        End If
        If CheckSoloDomesticos.Checked And CheckSoloExterior.Checked Then
            SqlMoneda = ""
            If PTipoEmisor = 1 Then SqlMonedaNC = ""
        End If
        '-------------------------------------------------------------------------------------------------------------------
        '---------------------------------------------------------------------------------------------------------------------
        '-----------------------En Cta.Cte. Clientes se sacaron los de ClienteOperacion <> 0 para que aparezcan los anteriores
        '-----------------------a activar el modulo de exportacion(Que llevan ClienteOperacion)-------------------------------
        '---------------------------------------------------------------------------------------------------------------------
        SqlB = ""
        SqlN = ""

        If PTipoEmisor = 1 Then
            SqlB = "SELECT 1 AS Operacion,Cliente AS Emisor, 2 AS Tipo, Saldo,FechaContable AS Fecha,Factura AS Comprobante,Importe + Percepciones AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,Esz,EsFCE FROM FacturasCabeza WHERE EsExterior = 0 AND " & SqlEmisorCliente & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Cliente AS Emisor, 2 AS Tipo, Saldo,FechaElectronica AS Fecha,Factura AS Comprobante,Importe + Percepciones AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,Esz,EsFCE FROM FacturasCabeza WHERE ClienteOperacion = 0 AND EsExterior = 1 AND " & SqlEmisorCliente & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor, 0 AS Tipo, Saldo,Fecha,Clave AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,Clave As ReciboOficial,0 As Tr,1 AS Estado,  Moneda,Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM SaldosInicialesCabeza WHERE Tipo = 3 AND " & SqlEmisorEmisor & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Cliente AS Emisor, 800 AS Tipo,Saldo,FechaContable AS Fecha,Liquidacion AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,ReciboOficial,Tr,Estado,1 AS Moneda,1 AS Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM NVLPCabeza WHERE " & SqlEmisorCliente & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE ClienteOperacion = 0 AND " & SqlEmisorEmisor & " AND (((TipoNota = 5 or TipoNota = 7 or TipoNota = 13005 or TipoNota = 13007) AND EsElectronica = 0) or TipoNota = 60 or TipoNota = 65 or TipoNota = 64)" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable AS Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE ClienteOperacion = 0 AND " & SqlEmisorEmisor & " AND (TipoNota = 5 or TipoNota = 7) AND EsElectronica = 1" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable As Fecha,Nota AS Comprobante,Importe ,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE " & SqlEmisorEmisor & " AND (TipoNota = 50 or TipoNota = 70)" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,FacturasCabeza.Cliente AS Emisor, 4 AS Tipo,0 AS Saldo,NotasCreditoCabeza.FechaContable AS Fecha,NotasCreditoCabeza.NotaCredito AS Comprobante,NotasCreditoCabeza.Importe + NotasCreditoCabeza.Percepciones AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr,NotasCreditoCabeza.Estado,NotasCreditoCabeza.Moneda,NotasCreditoCabeza.Cambio,NotasCreditoCabeza.Comentario,NotasCreditoCabeza.EsZ,NotasCreditoCabeza.EsFCE FROM NotasCreditoCabeza,FacturasCabeza WHERE FacturasCabeza.ClienteOperacion = 0 AND " & SqlFacturaCliente & SqlMonedaNC & _
                   " AND FacturasCabeza.Factura = NotasCreditoCabeza.Factura" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Cliente AS Emisor, -1 AS Tipo,Valor As Saldo,Fecha,Remito AS Comprobante,Valor AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr, Estado,1 AS Moneda,1 As Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RemitosCabeza WHERE Estado <> 3 AND Factura = 0 AND PorCuentaYOrden = 0 AND " & SqlEmisorCliente & SqlRemito & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,PorCuentaYOrden AS Emisor, -1 AS Tipo,Valor As Saldo,Fecha,Remito AS Comprobante,Valor AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr, Estado,1 AS Moneda,1 As Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RemitosCabeza WHERE Estado <> 3 AND Factura = 0 AND PorCuentaYOrden <> 0 AND " & SqlEmisorPorCuentaYOrden & SqlRemito & ";"



            SqlN = "SELECT 2 AS Operacion,Cliente AS Emisor, 2 AS Tipo, Saldo,FechaContable AS Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial, Tr,Estado,Moneda,Cambio,Comentario,Esz,EsFCE FROM FacturasCabeza WHERE EsExterior = 0 AND " & SqlEmisorCliente & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Cliente AS Emisor, 2 AS Tipo, Saldo,FechaElectronica AS Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial, Tr,Estado,Moneda,Cambio,Comentario,Esz,EsFCE FROM FacturasCabeza WHERE ClienteOperacion = 0 AND EsExterior = 1 AND " & SqlEmisorCliente & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, 0 AS Tipo, Saldo,Fecha,Clave AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,Clave As ReciboOficial,0 As Tr,1 AS Estado,Moneda,Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM SaldosInicialesCabeza WHERE Tipo = 3 AND " & SqlEmisorEmisor & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Cliente AS Emisor, 800 AS Tipo, Saldo,FechaContable AS Fecha,Liquidacion AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,ReciboOficial,Tr,Estado,1 AS Moneda,1 AS Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM NVLPCabeza WHERE " & SqlEmisorCliente & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE ClienteOperacion = 0 AND " & SqlEmisorEmisor & " AND (((TipoNota = 5 or TipoNota = 7 or TipoNota = 13005 or TipoNota = 13007) AND EsElectronica = 0) or TipoNota = 60 or TipoNota = 65 or TipoNota = 64)" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable As Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial, Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE ClienteOperacion = 0 AND " & SqlEmisorEmisor & " AND (TipoNota = 5 or TipoNota = 7) AND EsElectronica = 1" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable AS Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,EsFCE FROM RecibosCabeza WHERE " & SqlEmisorEmisor & " AND (TipoNota = 50 or TipoNota = 70)" & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,FacturasCabeza.Cliente AS Emisor, 4 AS Tipo,0 AS Saldo,NotasCreditoCabeza.FechaContable AS Fecha,NotasCreditoCabeza.NotaCredito AS Comprobante,NotasCreditoCabeza.Importe AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr,NotasCreditoCabeza.Estado,NotasCreditoCabeza.Moneda,NotasCreditoCabeza.Cambio,NotasCreditoCabeza.Comentario,NotasCreditoCabeza.EsZ,NotasCreditoCabeza.EsFCE FROM NotasCreditoCabeza,FacturasCabeza WHERE  FacturasCabeza.ClienteOperacion = 0 AND " & SqlFacturaCliente & SqlMonedaNC & _
                   " AND FacturasCabeza.Factura = NotasCreditoCabeza.Factura " & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Cliente AS Emisor, 9000 AS Tipo,Saldo, Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,Nota As ReciboOficial,0 As Tr,Estado,Moneda,Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM CierreFacturasCabeza WHERE ClienteOperacion = 0 AND " & SqlEmisorCliente & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Cliente AS Emisor, -1 AS Tipo,Valor As Saldo,Fecha,Remito AS Comprobante,Valor AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr,Estado,1 AS Moneda,1 As Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RemitosCabeza WHERE Estado <> 3 AND Factura = 0 AND PorCuentaYOrden = 0 AND " & SqlEmisorCliente & SqlRemito & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,PorCuentaYOrden AS Emisor, -1 AS Tipo,Valor As Saldo,Fecha,Remito AS Comprobante,Valor AS Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr,Estado,1 AS Moneda,1 As Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RemitosCabeza WHERE Estado <> 3 AND Factura = 0 AND PorCuentaYOrden <> 0 AND " & SqlEmisorPorCuentaYOrden & SqlRemito & ";"

        Else
            SqlB = "SELECT 1 AS Operacion,Proveedor As Emisor,2 AS Tipo, Saldo,FechaContable AS Fecha,FacturasProveedorCabeza.Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,ReciboOficial,Tr,Estado, Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND " & SqlEmisorProveedor & SqlMoneda & " AND Liquidacion = 0" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor,0 AS Tipo,Saldo,Fecha,Clave AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,Clave As ReciboOficial,0 As Tr,1 AS Estado, Moneda, Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM SaldosInicialesCabeza WHERE Tipo = 2 AND " & SqlEmisorEmisor & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Proveedor As Emisor,10 AS Tipo, Saldo,FechaContable AS Fecha,Liquidacion AS Comprobante, Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial, Tr,Estado,1 AS Moneda,1 AS Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM LiquidacionCabeza WHERE " & SqlEmisorProveedor & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor,TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorEmisor & SqlMoneda & " AND (((TipoNota = 6 OR TipoNota = 8 or TipoNota = 13006 or TipoNota = 13008) AND EsElectronica = 0) or TipoNota = 600 or TipoNota = 604)" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor,TipoNota AS Tipo,Saldo,FechaContable As Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorEmisor & SqlMoneda & " AND (TipoNota = 6 OR TipoNota = 8) AND EsElectronica = 1" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Emisor,TipoNota AS Tipo,Saldo,FechaContable AS Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE " & SqlEmisorEmisor & SqlMoneda & " AND (TipoNota = 500 or TipoNota = 700)" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,Proveedor AS Emisor,66 AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 AS ReciboOficial,0 AS Tr,Estado,1 AS Moneda,1 AS Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM RecuperoSenia WHERE Usado = 0 AND " & SqlEmisorProveedor & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,ACuenta AS Emisor,TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorACuenta & SqlMoneda & " AND ACuenta <> 0 AND (((TipoNota = 6 or TipoNota = 8 or TipoNota = 13006 or TipoNota = 13008) AND EsElectronica = 0) or TipoNota = 500 or TipoNota = 600 or TipoNota = 700)" & _
                   " UNION ALL " & _
                   "SELECT 1 AS Operacion,ACuenta AS Emisor,TipoNota AS Tipo,Saldo,FechaContable As Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorACuenta & SqlMoneda & " AND ACuenta <> 0 AND (TipoNota = 6 or TipoNota = 8) AND EsElectronica = 1;"


            SqlN = "SELECT 2 AS Operacion,Proveedor As Emisor,2 AS Tipo, Saldo,FechaContable As Fecha,Factura AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,ReciboOficial,Tr,Estado,Moneda, Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND " & SqlEmisorProveedor & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, 0 AS Tipo,Saldo,Fecha,Clave AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial,0 As Tr,1 AS Estado, Moneda,Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM SaldosInicialesCabeza WHERE Tipo = 2 AND " & SqlEmisorEmisor & SqlMoneda & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Proveedor As Emisor,10 AS Tipo, Saldo,FechaContable AS Fecha,Liquidacion AS Comprobante, Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 As ReciboOficial, Tr,Estado,1 AS Moneda,1 AS Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM LiquidacionCabeza WHERE " & SqlEmisorProveedor & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorEmisor & SqlMoneda & " AND (((TipoNota = 6 or TipoNota = 8 or TipoNota = 13006 or TipoNota = 13008) AND EsElectronica = 0) or TipoNota = 600 or TipoNota = 604)" & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable AS Fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorEmisor & SqlMoneda & " AND (TipoNota = 6 or TipoNota = 8) AND EsElectronica = 1" & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Emisor, TipoNota AS Tipo,Saldo,FechaContable As fecha,Nota AS Comprobante,Importe,ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE " & SqlEmisorEmisor & SqlMoneda & " AND (TipoNota = 500 or TipoNota = 700)" & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,Proveedor AS Emisor,66 AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe,0 AS ChequeRechazado,0 AS ClaveChequeReemplazado,0 AS ACuenta,0 AS ReciboOficial,0 AS Tr,Estado,1 AS Moneda,1 AS Cambio,'' AS Comentario,0 AS Esz,0 AS EsFCE FROM RecuperoSenia WHERE Usado = 0 AND " & SqlEmisorProveedor & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,ACuenta AS Emisor,TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorACuenta & SqlMoneda & " AND ACuenta <> 0 AND (((TipoNota = 6 or TipoNota = 8 or TipoNota = 13006 or TipoNota = 13008) AND EsElectronica = 0) or TipoNota = 500 or TipoNota = 600 or TipoNota = 700)" & _
                   " UNION ALL " & _
                   "SELECT 2 AS Operacion,ACuenta AS Emisor,TipoNota AS Tipo,Saldo,FechaContable As Fecha,Nota AS Comprobante,Importe, ChequeRechazado,ClaveChequeReemplazado,ACuenta,ReciboOficial,Tr,Estado,Moneda,Cambio,Comentario,0 AS Esz,0 AS EsFCE FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND " & SqlEmisorACuenta & SqlMoneda & " AND ACuenta <> 0 AND (TipoNota = 6 or TipoNota = 8) AND EsElectronica = 1;"
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        'Saco los proveedores de negocio.
        Dim ProveedorAnt As Integer = 0
        Dim TipoOperacion As Integer = 0
        If PTipoEmisor = 2 Then
            For Each Row As DataRow In Dt.Rows
                If ProveedorAnt <> Row("Emisor") Then
                    TipoOperacion = HallaTipoOperacion(Row("Emisor"))
                    ProveedorAnt = Row("Emisor")
                End If
                If TipoOperacion = 4 Then Row.Delete()
            Next
        End If

        If PTipoEmisor = 1 And ComboBoxCanalVenta.SelectedValue <> 0 Then
            For Each Row As DataRow In Dt.Rows
                If Not CorrespodeCanalVenta(Row("Emisor"), ComboBoxCanalVenta.SelectedValue) Then
                    Row.Delete()
                End If
            Next
        End If

        If PTipoEmisor = 1 And CheckSoloExterior.Checked And Not CheckSoloDomesticos.Checked Then
            For Each Row As DataRow In Dt.Rows
                If Not Row.RowState = DataRowState.Deleted Then
                    If Row("Tipo") = -1 Or Row("Tipo") = 800 Then
                        Row.Delete()
                    End If
                End If
            Next
        End If

        PView = New DataView
        PView = Dt.DefaultView
        PView.Sort = "Emisor,Fecha,Comprobante"

        Dim SaldoCta As Double = 0
        Dim EmisorAnt As Integer = 0
        Dim SaldoTotal As Double = 0

        For Each Row As DataRowView In PView
            If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) >= 0 Then
                If EsBalanceoOk(Row("Tr")) And EsMOnedaOk(Row("Moneda")) And EsExteriorOLocalOk(Row("Moneda")) Then
                    If EmisorAnt <> Row("Emisor") Then
                        If EmisorAnt <> 0 Then
                            AddGrid(EmisorAnt, 0, "", 0, "1/1/1800", 0, 7000000, 0, 0, 0, 0, SaldoCta, 0, Nothing)
                            SaldoTotal = SaldoTotal + SaldoCta
                        End If
                        EmisorAnt = Row("Emisor")
                        SaldoCta = 0
                        AddGrid(Row("Emisor"), 0, "", 0, "1/1/1800", 0, 6000000, 0, 0, 0, 0, SaldoCta, 0, Nothing)
                    End If
                    If DiferenciaDias(Row("Fecha"), DateTimeHasta.Value) >= 0 Then
                        ProcesaComprobante(Row, SaldoCta)
                    End If
                End If
            End If
        Next
        If EmisorAnt <> 0 Then
            AddGrid(0, 0, "", 0, "1/1/1800", 0, 7000000, 0, 0, 0, 0, SaldoCta, 0, Nothing)
            SaldoTotal = SaldoTotal + SaldoCta
        End If

        If Emisor = 0 And CheckIncluirConSaldoCero.Checked = False Then BorraCuentasConSaldocero()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        TextSaldoCta.Text = FormatNumber(SaldoTotal, GDecimales)

        Grid.Columns("Operacion").Visible = False

        Dt.Dispose()
        PView = Nothing

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ProcesaComprobante(ByVal Row As DataRowView, ByRef SaldoCta As Double)

        Dim TipoW As Integer

        If ComboMoneda.SelectedValue = 1 Then
            If Row("Moneda") <> 1 Then
                Row("Importe") = Trunca(Row("Importe") * Row("Cambio"))
                Row("Saldo") = Trunca(Row("Saldo") * Row("Cambio"))
            End If
        End If

        Select Case Row("Tipo")
            Case 0
                'Analiza Saldo Inicial.
                Select Case PTipoEmisor
                    Case 1
                        If Row("Importe") < 0 Then
                            TipoW = -11
                        Else
                            TipoW = -10
                        End If
                    Case 2
                        If Row("Importe") > 0 Then
                            TipoW = -12
                        Else
                            TipoW = -9
                        End If
                End Select
                If Row("Importe") > 0 Then
                    SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                Else
                    SaldoCta = SaldoCta - (-Row("Importe"))
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), 0, -Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                End If
            Case 2
                'Analiza una factura.
                If PTipoEmisor = 1 Then              'Cliente.
                    Dim Mensaje As String = ""
                    If Row("Estado") <> 3 Then SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        If Row("Esz") Then Mensaje = "Ticket"
                        If Row("EsFCE") Then Mensaje = "FCE"
                        AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                Else                                 'Proveedor.
                    Dim Mensaje As String = ""
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        TipoW = Row("Tipo")
                        If Row("Tr") And PermisoTotal Then TipoW = 4000000 'Mensaje = "CONTABLE"
                        AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                End If
            Case 800               'Analiza NVLP.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 4                  'Analiza Notas de Credito Con devolucion de articulo.
                Dim Mensaje As String = ""
                If Row("Estado") <> 3 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    If Row("Esz") Then Mensaje = "Ticket"
                    If Row("EsFCE") Then Mensaje = "FCE"
                    Row("Saldo") = HallaImporteNoAsignado(Row("Comprobante"), Row("Importe"), Row("Operacion"))
                    AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 50                 'Analiza Notas debitos del Cliente.   
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 500                'Analiza Notas debitos del Proveedor. 
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 60                 'Analiza Pago del Cliente. 
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 64                 'Analiza Devolución a Cliente. 
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 600                'Analiza Pago al Proveedor.
                Dim Mensaje As String = ""
                If Row("ClaveChequeReemplazado") <> 0 Then
                    If FechaOk(Row("Fecha")) Then AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), 3000001, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    Exit Sub
                End If
                If Row("ACuenta") = 0 Then
                    If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        TipoW = Row("Tipo")
                        If Row("Tr") And PermisoTotal Then TipoW = 5000000 'Mensaje = "CONTABLE"
                        AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                Else
                    If Row("Emisor") = Row("Acuenta") Then
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), 1000000, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                        End If
                    Else
                        If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe") - Row("Importe")
                        If FechaOk(Row("Fecha")) Then
                            AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), 2000000, Row("Comprobante"), Row("Importe"), Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                        End If
                    End If
                End If
            Case 604                     'Analiza Devolución del proveedores. 
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 70                       'Analiza Nota Credito del Cliente.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 700                     'Analiza Nota Credito del Proveedor.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 5, 13005                'Analiza Nota Debito.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    Dim Mensaje As String
                    If Row("ChequeRechazado") = 0 Then
                        Mensaje = ""
                        TipoW = Row("Tipo")
                    Else
                        Mensaje = ""
                        TipoW = 3000000
                    End If
                    If Row("EsFCE") Then Mensaje = "FCE"
                    AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 6, 13006                 'Analiza Nota Debito para proveedor. 
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    Dim Mensaje As String
                    If Row("ChequeRechazado") = 0 Then
                        Mensaje = ""
                        TipoW = Row("Tipo")
                    Else
                        Mensaje = ""
                        TipoW = 3000000
                    End If
                    AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 7, 13007                 'Analiza Nota Credito.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    Dim Mensaje As String
                    If Row("ChequeRechazado") = 0 Then
                        TipoW = Row("Tipo")
                    Else
                        TipoW = 3000000
                    End If
                    If Row("EsFCE") Then Mensaje = "FCE"
                    AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 8, 13008                'Analiza Nota Credito para proveedor.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    Dim Mensaje As String
                    If Row("ChequeRechazado") = 0 Then
                        Mensaje = ""
                        TipoW = Row("Tipo")
                    Else
                        Mensaje = ""
                        TipoW = 3000000
                    End If
                    AddGrid(Row("Emisor"), Row("Operacion"), Mensaje, Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), TipoW, Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 10                     'Analiza Liquidacion A Proveedores.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 30                     'Analiza Sena.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 65                     'Devolucion Sena a Cliente.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe") + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    Row("Saldo") = 0
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 66                     'Recupero Sena terceros.
                If Row("Estado") = 1 Then SaldoCta = SaldoCta - Row("Importe") + Row("Importe")
                If FechaOk(Row("Fecha")) Then
                    AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                End If
            Case 9000                     'Cierre facturas exportacion.
                If Row("Estado") = 1 Then
                    If Row("Importe") >= 0 Then
                        SaldoCta = SaldoCta + Row("Importe")
                    Else
                        SaldoCta = SaldoCta - (-Row("Importe"))
                    End If
                End If
                If FechaOk(Row("Fecha")) Then
                    If Row("Importe") >= 0 Then
                        AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    Else
                        AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), 0, -Row("Importe"), Row("Saldo"), SaldoCta, Row("Estado"), Row("Comentario"))
                    End If
                End If
            Case -1                     'Remitos Valorizados.
                Row("Importe") = HallaSaldoRemitoNVLP(Row("Operacion"), Row("Comprobante"), Row("Importe")) 'Halla saldo en caso de NVLP.
                If Row("Importe") <> 0 Then
                    If Row("Estado") <> 3 Then SaldoCta = SaldoCta + Row("Importe")
                    If FechaOk(Row("Fecha")) Then
                        AddGrid(Row("Emisor"), Row("Operacion"), "", Row("ReciboOficial"), Row("Fecha"), Row("Tipo"), Row("Tipo"), Row("Comprobante"), Row("Importe"), 0, Row("Saldo"), SaldoCta, 1, Row("Comentario"))
                    End If
                End If
            Case Else
                MsgBox("Codigo documento " & Row("Tipo") & " no contemplado.")
        End Select

        SaldoCta = Trunca(SaldoCta)

        If DiferenciaDias(Row("Fecha"), DateTimeDesde.Value) > 0 Then DtGrid.Rows(DtGrid.Rows.Count - 1).Item("SaldoCta") = SaldoCta

    End Sub
    Private Function HallaImporteNoAsignado(ByVal Nota As Decimal, ByVal ImporteNota As Decimal, ByVal Operacion As Integer) As Decimal

        Dim Dt As New DataTable
        Dim ConexionStr As String
        Select Case Operacion
            Case 1
                ConexionStr = Conexion
            Case 2
                ConexionStr = ConexionN
        End Select

        If Not Tablas.Read("SELECT Importe FROM RecibosDetalle WHERE TipoNota = 4 AND Nota = " & Nota & ";", ConexionStr, Dt) Then Return -1000
        If Dt.Rows.Count = 0 Then Return ImporteNota
        Dim Importe As Decimal = 0
        For Each Row As DataRow In Dt.Rows
            Importe = Importe + Row("Importe")
        Next

        Dt.Dispose()

        Return ImporteNota - Importe

    End Function
    Private Sub AddGrid(ByVal Emisor As Integer, ByVal Operacion As Integer, ByVal Mensaje As String, ByVal ReciboOficial As Double, ByVal Fecha As DateTime, ByVal TipoOrigen As Integer, ByVal Tipo As Integer, ByVal Comprobante As Double, _
                      ByVal Debito As Double, ByVal Credito As Double, ByVal Saldo As Double, ByVal SaldoCta As Double, ByVal Estado As Integer, ByVal Comentario As String)

        If SinSaldoAnterior And Tipo = 6000000 Then Exit Sub
        If Not ConDetalle And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub

        If Not CheckConAnuladas.Checked And Estado = 3 Then Exit Sub

        If Not CheckSinAsignar.Checked Or Not CheckYaAsignados.Checked Then
            If CheckSinAsignar.Checked And Saldo = 0 And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub
            If CheckYaAsignados.Checked And Saldo <> 0 And Tipo <> 6000000 And Tipo <> 7000000 Then Exit Sub
        End If

        Dim RowGrid As DataRow

        If Estado = 2 Then Estado = 1
        If Estado = 1 Then Estado = 0

        Dim CanalVenta As Integer = 0
        Dim CanalDistribucion As Integer = 0
        If Not CheckDetalle.Checked And PTipoEmisor = 1 Then
            Dim Dt As New DataTable
            If Tablas.Read("SELECT CanalVenta, CanalDistribucion FROM Clientes WHERE Clave = " & Emisor & ";", Conexion, Dt) Then
                If Dt.Rows.Count <> 0 Then
                    CanalVenta = Dt.Rows(0).Item("CanalVenta")
                    CanalDistribucion = Dt.Rows(0).Item("CanalDistribucion")
                End If
            End If
        End If

        RowGrid = DtGrid.NewRow()
        RowGrid("Emisor") = Emisor
        RowGrid("Operacion") = Operacion
        RowGrid("Mensaje") = Mensaje
        RowGrid("Fecha") = Format(Fecha, "dd/MM/yyyy 00:00:00")
        RowGrid("TipoOrigen") = TipoOrigen
        RowGrid("Tipo") = Tipo
        RowGrid("Comprobante") = Comprobante
        RowGrid("ReciboOficial") = ReciboOficial
        RowGrid("Debito") = Debito
        RowGrid("Credito") = Credito
        RowGrid("Saldo") = Saldo
        RowGrid("SaldoCta") = SaldoCta
        RowGrid("Estado") = Estado
        RowGrid("Comentario") = Comentario
        RowGrid("CanalVenta") = CanalVenta
        RowGrid("CanalDistribucion") = CanalDistribucion
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Function FechaOk(ByVal Fecha As DateTime) As Boolean

        If DiferenciaDias(Fecha, DateTimeDesde.Value) > 0 Then Return False
        If DiferenciaDias(Fecha, DateTimeHasta.Value) < 0 Then Return False

        Return True

    End Function
    Private Sub BorraCuentasConSaldocero()

        Dim Comienzo As Integer
        Dim Final As Integer

        For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
            If DtGrid.Rows(I).Item("Tipo") = 7000000 And DtGrid.Rows(I).Item("SaldoCta") = 0 Then
                Comienzo = I
                For Final = I To 0 Step -1
                    If DtGrid.Rows(Final).Item("Tipo") = 6000000 Then
                        BorraRowGrid(Comienzo, Final)
                        I = Final
                        Exit For
                    End If
                Next
            End If
        Next

    End Sub
    Private Sub BorraRowGrid(ByVal Comienzo As Integer, ByVal Final As Integer)

        For I As Integer = Comienzo To Final Step -1
            Dim Row As DataRow = DtGrid.Rows(I)
            Row.Delete()
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Emisor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim TipoOrigen As New DataColumn("TipoOrigen")
        TipoOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOrigen)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Recibo)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim ReciboOficial As New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim SaldoCta As New DataColumn("SaldoCta")
        SaldoCta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(SaldoCta)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim CanalVenta As New DataColumn("CanalVenta")
        CanalVenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CanalVenta)

        Dim CanalDistribucion As New DataColumn("CanalDistribucion")
        CanalDistribucion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CanalDistribucion)

    End Sub
    Private Sub LlenaCombosGrid()

        Tipo.DataSource = DtTiposComprobantes(False)
        Dim Row As DataRow = Tipo.DataSource.newrow
        Row("Nombre") = "Pago Terceros Por Orden"
        Row("Codigo") = 1000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Pago Por Cuanta Proveedor"
        Row("Codigo") = 2000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Rechazo Cheque"
        Row("Codigo") = 3000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Reemplazo Cheque"
        Row("Codigo") = 3000001
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Factura CONTABLE"
        Row("Codigo") = 4000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Pago CONTABLE"
        Row("Codigo") = 5000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "SALDO ANTERIOR"
        Row("Codigo") = 6000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "TOTAL"
        Row("Codigo") = 7000000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Recupero Seña Terceros"
        Row("Codigo") = 66
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Cierre Fact. Exportación"
        Row("Codigo") = 9000
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Remito Valorizado"
        Row("Codigo") = -1
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Saldo Inicial"
        Row("Codigo") = -10
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Saldo Inicial"
        Row("Codigo") = -9
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Crédito Saldo Inicial"
        Row("Codigo") = -11
        Tipo.DataSource.rows.add(Row)
        Row = Tipo.DataSource.newrow
        Row("Nombre") = "Debito Saldo Inicial"
        Row("Codigo") = -12
        Tipo.DataSource.rows.add(Row)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        If PTipoEmisor = 1 Then
            Emisor1.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        End If
        If PTipoEmisor = 2 Then
            Emisor1.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4;")
        End If
        Row = Emisor1.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        Emisor1.DataSource.rows.add(Row)
        Emisor1.DisplayMember = "Nombre"
        Emisor1.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        If PTipoEmisor = 1 Then
            CanalVenta.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = 23;")
            Row = CanalVenta.DataSource.newrow
            Row("Nombre") = ""
            Row("Clave") = 0
            CanalVenta.DataSource.rows.add(Row)
            CanalVenta.DisplayMember = "Nombre"
            CanalVenta.ValueMember = "Clave"
            CanalDistribucion.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = 45;")
            Row = CanalDistribucion.DataSource.newrow
            Row("Nombre") = ""
            Row("Clave") = 0
            CanalDistribucion.DataSource.rows.add(Row)
            CanalDistribucion.DisplayMember = "Nombre"
            CanalDistribucion.ValueMember = "Clave"
        End If

    End Sub
    Private Function EsBalanceoOk(ByVal Tr As Boolean) As Boolean

        If (CheckSinBalanceo.Checked And CheckConBalanceo.Checked) Or (CheckSinBalanceo.Checked And Not Tr) Or (CheckConBalanceo.Checked And Tr) Then
            Return True
        End If

        Return False

    End Function
    Private Function EsMOnedaOk(ByVal Moneda As Integer) As Boolean

        If ComboMoneda.SelectedValue = Moneda Or ComboMoneda.SelectedValue = 1 Then Return True

        Return False

    End Function
    Private Function EsExteriorOLocalOk(ByVal Moneda As Integer) As Boolean

        If CheckSoloDomesticos.Checked And CheckSoloExterior.Checked Then Return True

        If CheckSoloDomesticos.Checked And Not CheckSoloExterior.Checked Then
            If Moneda = 1 Then Return True
        End If

        If Not CheckSoloDomesticos.Checked And CheckSoloExterior.Checked Then
            If Moneda > 1 Then Return True
        End If

        Return False

    End Function
    Private Function HallaSaldoRemitoNVLP(ByVal Operacion As Integer, ByVal Remito As Double, ByVal Valor As Decimal) As Decimal

        If Not RemitoTieneNVLP(Remito, Operacion) Then
            Return Valor
        End If

        'Halla lo que falta liquidar en el remito por una NVLP.

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Indice,Lote,Secuencia,Cantidad FROM AsignacionLotes WHERE Liquidado = 0 AND TipoComprobante = 1 AND Comprobante = " & Remito & ";", ConexionStr, Dt) Then
            MsgBox("Error Base de Datos al leer Tabla: AsignacionLotes.", MsgBoxStyle.Critical)
            End
        End If

        Dim Importe As Decimal = 0
        Dim Dt2 As New DataTable

        For Each Row As DataRow In Dt.Rows
            If Not Tablas.Read("SELECT Precio FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionStr, Dt2) Then
                MsgBox("Error Base de Datos al leer Tabla: RemitosDetalle.", MsgBoxStyle.Critical)
                End
            End If
            Importe = Importe + CalculaNeto(Row("Cantidad"), Dt2.Rows(0).Item("Precio"))
        Next

        Dt.Dispose()
        Dt2.Dispose()

        Return Importe

    End Function
    Private Function CorrespodeCanalVenta(ByVal Cliente As Integer, ByVal CanalVenta As Integer) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(CanalVenta) FROM Clientes WHERE Clave = " & Cliente & " AND CanalVenta = " & CanalVenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(e.Value) Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If e.Value <> "1/1/1800" Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = NumeroEditado(e.Value)
            End If
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "SaldoCta" Then
            '       If e.Value > 0 Then Grid.Rows(e.RowIndex).Cells("SaldoCta").Style.ForeColor = Color.Red
            '      If e.Value <= 0 Then Grid.Rows(e.RowIndex).Cells("SaldoCta").Style.ForeColor = Color.Black
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Dim Emisor As Integer = 0

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        Dim Imputa As Boolean
        If PTipoEmisor = 1 Then
            If PermisoEscritura(100) Then
                Imputa = True
            Else
                Imputa = False
            End If
        End If
        If PTipoEmisor = 2 Then
            If PermisoEscritura(213) Then
                Imputa = True
            Else
                Imputa = False
            End If
        End If

        If Grid.CurrentRow.Cells("Tipo").Value = 2 Or Grid.CurrentRow.Cells("Tipo").Value = 4000000 Then
            If PTipoEmisor = 1 Then
                UnaFactura.PAbierto = Abierto
                UnaFactura.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                UnaFactura.PBloqueaFunciones = True
                UnaFactura.ShowDialog()
            Else
                Dim Tipo As Integer = HallaTipofacturaProveedor(Grid.CurrentRow.Cells("Comprobante").Value, Abierto)
                If Tipo = 1 Then
                    UnaFacturaProveedor.PProveedor = Emisor
                    UnaFacturaProveedor.PBloqueaFunciones = True
                    UnaFacturaProveedor.PAbierto = Abierto
                    UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaFacturaProveedor.PCodigoFactura = 900
                    UnaFacturaProveedor.ShowDialog()
                    UnaFacturaProveedor.Dispose()
                End If
                If Tipo = 2 Then
                    UnaFacturaProveedor.PProveedor = Emisor
                    UnaFacturaProveedor.PBloqueaFunciones = True
                    UnaFacturaProveedor.PAbierto = Abierto
                    UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaFacturaProveedor.PCodigoFactura = 902
                    UnaFacturaProveedor.ShowDialog()
                    UnaFacturaProveedor.Dispose()
                End If
                If Tipo = 3 Then
                    UnaFacturaProveedor.PProveedor = Emisor
                    UnaFacturaProveedor.PBloqueaFunciones = True
                    UnaFacturaProveedor.PAbierto = Abierto
                    UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaFacturaProveedor.PCodigoFactura = 903
                    UnaFacturaProveedor.ShowDialog()
                    UnaFacturaProveedor.Dispose()
                End If
                If Tipo = 4 Then
                    UnaFacturaProveedor.PProveedor = Emisor
                    UnaFacturaProveedor.PBloqueaFunciones = True
                    UnaFacturaProveedor.PAbierto = Abierto
                    UnaFacturaProveedor.PFactura = Grid.CurrentRow.Cells("Comprobante").Value
                    UnaFacturaProveedor.PCodigoFactura = 901
                    UnaFacturaProveedor.ShowDialog()
                    UnaFacturaProveedor.Dispose()
                End If
            End If
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 4 Then
            UnaNotaCredito.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnaNotaCredito.PAbierto = Abierto
            UnaNotaCredito.PBloqueaFunciones = True
            UnaNotaCredito.ShowDialog()
            UnaNotaCredito.Dispose()
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 60 Or Grid.CurrentRow.Cells("Tipo").Value = 600 Or Grid.CurrentRow.Cells("Tipo").Value = 65 Or _
            Grid.CurrentRow.Cells("Tipo").Value = 5000000 Or Grid.CurrentRow.Cells("Tipo").Value = 3000001 Or Grid.CurrentRow.Cells("Tipo").Value = 64 Or Grid.CurrentRow.Cells("Tipo").Value = 604 Then
            UnRecibo.PAbierto = Abierto
            UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
            UnRecibo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnRecibo.PEmisor = Emisor
            UnRecibo.PBloqueaFunciones = True
            UnRecibo.PImputa = Imputa
            UnRecibo.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 50 Or Grid.CurrentRow.Cells("Tipo").Value = 70 Or Grid.CurrentRow.Cells("Tipo").Value = 500 Or Grid.CurrentRow.Cells("Tipo").Value = 700 Or _
           Grid.CurrentRow.Cells("Tipo").Value = 5 Or Grid.CurrentRow.Cells("Tipo").Value = 7 Or Grid.CurrentRow.Cells("Tipo").Value = 6 Or Grid.CurrentRow.Cells("Tipo").Value = 8 Or _
           Grid.CurrentRow.Cells("Tipo").Value = 13005 Or Grid.CurrentRow.Cells("Tipo").Value = 13007 Or Grid.CurrentRow.Cells("Tipo").Value = 13006 Or Grid.CurrentRow.Cells("Tipo").Value = 13008 Then
            UnReciboDebitoCredito.PAbierto = Abierto
            UnReciboDebitoCredito.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
            UnReciboDebitoCredito.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnReciboDebitoCredito.PEmisor = Emisor
            UnReciboDebitoCredito.PBloqueaFunciones = True
            UnReciboDebitoCredito.PImputa = Imputa
            UnReciboDebitoCredito.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 1000000 Or Grid.CurrentRow.Cells("Tipo").Value = 2000000 Then
            UnRecibo.PAbierto = Abierto
            UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
            UnRecibo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnRecibo.PEmisor = Emisor
            UnRecibo.PBloqueaFunciones = True
            UnRecibo.PImputa = Imputa
            UnRecibo.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 3000000 Then
            UnRecibo.PAbierto = Abierto
            UnRecibo.PTipoNota = Grid.CurrentRow.Cells("TipoOrigen").Value
            UnRecibo.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnRecibo.PEmisor = Emisor
            UnRecibo.PBloqueaFunciones = True
            UnRecibo.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 10 Then
            Dim ConexionStrW As String
            If Abierto Then
                ConexionStrW = Conexion
            Else
                ConexionStrW = ConexionN
            End If
            If LiquidacionTrucha(ConexionStrW, Grid.CurrentRow.Cells("Comprobante").Value) Then
                UnaLiquidacionContable.PAbierto = Abierto
                UnaLiquidacionContable.PLiquidacion = Grid.CurrentRow.Cells("Comprobante").Value
                UnaLiquidacionContable.PBloqueaFunciones = True
                UnaLiquidacionContable.ShowDialog()
                If UnaLiquidacionContable.PActualizacionOk Then PreparaArchivos()
                UnaLiquidacionContable.Dispose()
                Exit Sub
            Else
                UnaLiquidacion.PAbierto = Abierto
                UnaLiquidacion.PBloqueaFunciones = True
                UnaLiquidacion.PLiquidacion = Grid.CurrentRow.Cells("Comprobante").Value
                UnaLiquidacion.ShowDialog()
                If UnaLiquidacion.PActualizacionOk Then PreparaArchivos()
                UnaLiquidacion.Dispose()
                Exit Sub
            End If
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 800 Then
            UnaNVLP.PAbierto = Abierto
            UnaNVLP.PBloqueaFunciones = True
            UnaNVLP.PLiquidacion = Grid.CurrentRow.Cells("Comprobante").Value
            UnaNVLP.ShowDialog()
            UnaNVLP.Dispose()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 66 Then
            UnRecuperoSenia.PAbierto = Abierto
            UnRecuperoSenia.PBloqueaFunciones = True
            UnRecuperoSenia.PNota = Grid.CurrentRow.Cells("Comprobante").Value
            UnRecuperoSenia.ShowDialog()
            UnRecuperoSenia.Dispose()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = -1 Then
            UnRemito.PAbierto = Abierto
            UnRemito.PBloqueaFunciones = True
            UnRemito.PRemito = Grid.CurrentRow.Cells("Comprobante").Value
            UnRemito.PCliente = Grid.CurrentRow.Cells("Emisor1").Value
            UnRemito.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = 9000 Then
            UnCierreFactura.PAbierto = Abierto
            UnCierreFactura.PBloqueaFunciones = True
            UnCierreFactura.PNota = Grid.CurrentRow.Cells("ReciboOficial").Value
            UnCierreFactura.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = -9 Then
            UnSaldoInicial.PBloqueaFunciones = True
            UnSaldoInicial.PAbierto = Abierto
            UnSaldoInicial.PClave = Grid.CurrentRow.Cells("Comprobante").Value
            UnSaldoInicial.ShowDialog()
            UnSaldoInicial.Dispose()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = -10 Then
            UnSaldoInicial.PBloqueaFunciones = True
            UnSaldoInicial.PAbierto = Abierto
            UnSaldoInicial.PClave = Grid.CurrentRow.Cells("Comprobante").Value
            UnSaldoInicial.ShowDialog()
            UnSaldoInicial.Dispose()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = -11 Then
            UnaImputacionSaldosIniciales.PClave = Grid.CurrentRow.Cells("Comprobante").Value
            UnaImputacionSaldosIniciales.PTipoNota = 7
            UnaImputacionSaldosIniciales.PAbierto = Abierto
            UnaImputacionSaldosIniciales.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Tipo").Value = -12 Then
            UnaImputacionSaldosIniciales.PClave = Grid.CurrentRow.Cells("Comprobante").Value
            UnaImputacionSaldosIniciales.PTipoNota = 6
            UnaImputacionSaldosIniciales.PAbierto = Abierto
            UnaImputacionSaldosIniciales.ShowDialog()
            If GModificacionOk Then PreparaArchivos()
            Exit Sub
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub









End Class