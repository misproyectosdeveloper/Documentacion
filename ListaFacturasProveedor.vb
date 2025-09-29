Public Class ListaFacturasProveedor
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    Dim DtGrid As DataTable
    Private Sub ListaFacturasProveedor_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
        Dim Row As DataRow = ComboAlias.DataSource.NewRow
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

        ComboProveedorFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Row = ComboProveedorFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboProveedorFondoFijo.DataSource.rows.add(Row)
        ComboProveedorFondoFijo.DisplayMember = "Nombre"
        ComboProveedorFondoFijo.ValueMember = "Clave"
        ComboProveedorFondoFijo.SelectedValue = 0
        With ComboProveedorFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            '           Grid.Columns("FacturaN").HeaderText = ""
            Grid.Columns("ImporteN").HeaderText = ""
            Grid.Columns("Total").HeaderText = ""
        End If

        If Not PermisoTotal Then
            Panel2.Visible = False
            CheckCerrado.Checked = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaFacturas_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboProveedor.Focus()

    End Sub
    Private Sub ListaFacturasProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs)
        'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

        ' Select Case e.KeyData
        '  Case Keys.D0, Keys.NumPad0
        '        MessageBox.Show("Tecla 0")

        '  Case Keys.D1, Keys.NumPad1
        '        MessageBox.Show("Tecla 1")

        '  Case Keys.D9, Keys.NumPad9
        '        MessageBox.Show("Tecla 9")

        '  Case Keys.Return, CType(65584, Keys)
        '        MessageBox.Show("Tecla Return y la del signo igual.")
        '  End Select

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,* FROM FacturasProveedorCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM FacturasProveedorCabeza "

        Dim SqlFecha As String = "WHERE FacturasProveedorCabeza.Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND FacturasProveedorCabeza.Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim Sqlproveedor As String = ""
        If Proveedor > 0 Then
            Sqlproveedor = " AND FacturasProveedorCabeza.Proveedor = " & Proveedor
        End If

        Dim SqlReciboOficial As String = ""
        If Val(MaskedReciboOficial.Text) <> 0 Then
            Dim Patron As String = "%" & Format(Val(MaskedReciboOficial.Text), "000000000000")
            SqlReciboOficial = "AND CAST(CAST(FacturasProveedorCabeza.ReciboOficial AS numeric) as char)LIKE '" & Patron & "'"
        End If

        Dim SqlExterior As String
        If CheckBoxExterior.Checked Then
            SqlExterior = "AND FacturasProveedorCabeza.EsExterior = 1" & " "
        End If
        If CheckBoxDomestica.Checked Then
            SqlExterior = "AND FacturasProveedorCabeza.EsExterior = 0" & " "
        End If
        If CheckBoxExterior.Checked And CheckBoxDomestica.Checked Then
            SqlExterior = ""
        End If

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFecha & Sqlproveedor & SqlReciboOficial & SqlExterior & SqlEstado & ";"
        SqlN = SqlN & SqlFecha & Sqlproveedor & SqlReciboOficial & SqlExterior & SqlEstado & ";"

        CreaDtGrid()

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ButtonReemplazar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReemplazar.Click

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Not Grid.Columns(Col).Name = "Factura" Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Factura esta Anulada.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Operacion").Value = 2 Then
            MsgBox("Tipo de Factura Incorrecto.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Liquidacion").Value <> 0 Then
            MsgBox("Factura ya esta Reemplazada.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If HallaNotaDebito(Grid.CurrentCell.Value, Conexion) <> 0 Then
            MsgBox("Factura con Diferencia no puede ser Reemplazada por una Liquidación.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Saldo As Double
        Dim Remitido As Double
        Dim Facturado As Double
        Dim Baja As Double
        Dim Merma As Double
        Dim MermaTr As Double
        Dim Importe As Double
        Dim ImporteSinIva As Double
        Dim Senia As Double
        Dim Precio As Double
        Dim PrecioSSinIva As Double
        Dim Stock As Double
        Dim PrecioF As Double
        Dim Liquidado As Double
        Dim PrecioCompra As Double
        Dim CantidadInicial As Double
        Dim Descarga As Double
        Dim DescargaSinIva As Double
        Dim GastoComercial As Double
        Dim GastoComercialSinIva As Double

        Dim Fila As FilaLiquidacion

        UnaLiquidacion.PListaDeLotes = New List(Of FilaLiquidacion)

        Dim DtCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & Grid.CurrentCell.Value & ";", Conexion, DtCabeza) Then Exit Sub
        If DtCabeza.Rows(0).Item("Saldo") <> DtCabeza.Rows(0).Item("Importe") Then
            MsgBox("Factura esta imputada en cuenta corriente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            DtCabeza.Dispose()
            Exit Sub
        End If
        If DtCabeza.Rows(0).Item("EsReventa") = False Then
            MsgBox("Factura no es por Reventa.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            DtCabeza.Dispose()
            Exit Sub
        End If
        If HallaTipoIvaProveedor(DtCabeza.Rows(0).Item("Proveedor")) = Exterior Then
            MsgBox("Factura Importación NO puede ser Reemplazada.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            DtCabeza.Dispose()
            Exit Sub
        End If

        Dim ConexionCompro As String
        Dim FacturaParaCompro As Double
        If DtCabeza.Rows(0).Item("Rel") Then
            If Not PermisoTotal Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Dt.Dispose()
                Exit Sub
            End If
            ConexionCompro = ConexionN
            MsgBox(Grid.CurrentRow.Cells("FacturaN").Value)
            FacturaParaCompro = Grid.CurrentRow.Cells("FacturaN").Value
        Else
            ConexionCompro = Conexion
            FacturaParaCompro = Grid.CurrentRow.Cells("Factura").Value
        End If

        Dim DtComproFacturados As New DataTable
        If Not Tablas.Read("SELECT * FROM ComproFacturados WHERE Factura = " & FacturaParaCompro & ";", ConexionCompro, DtComproFacturados) Then Exit Sub

        Dim TotalCompro As Double = 0

        For Each Row As DataRow In DtComproFacturados.Rows
            Dim ListaOrden As String
            If Not AnalisisCostoLote(True, Row("Operacion"), DtCabeza.Rows(0).Item("Proveedor"), Row("Lote"), Row("Secuencia"), Saldo, Precio, PrecioSSinIva, Remitido, Facturado, Importe, ImporteSinIva, Baja, Merma, MermaTr, Stock, Liquidado, PrecioF, PrecioCompra, CantidadInicial, Senia, Descarga, DescargaSinIva, GastoComercial, GastoComercialSinIva, True, False, ListaOrden) Then Exit Sub
            Fila = New FilaLiquidacion
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.Operacion = Row("Operacion")
            Fila.PrecioS = Precio
            Fila.PrecioF = PrecioF
            If Row("Operacion") = 1 Then
                Fila.Articulo = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
            Else : Fila.Articulo = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
            End If
            Fila.Iniciales = CantidadInicial - Baja
            If MermaTr <> 0 Then
                Fila.Merma = MermaTr
            Else : Fila.Merma = Merma
            End If
            Fila.Merma = 0
            Fila.Aliquidar = CantidadInicial - Baja
            TotalCompro = TotalCompro + CalculaNeto(CantidadInicial - Baja, PrecioF)
            UnaLiquidacion.PListaDeLotes.Add(Fila)
        Next

        Dim IndiceCorreccion As Double = TotalCompro - DtCabeza.Rows(0).Item("Importe")
        IndiceCorreccion = IndiceCorreccion * 100 / TotalCompro

        For Each Fila In UnaLiquidacion.PListaDeLotes
            Fila.PrecioF = Fila.PrecioF - Fila.PrecioF * IndiceCorreccion / 100
        Next

        UnaLiquidacion.PLiquidacion = 0
        UnaLiquidacion.PProveedor = Grid.CurrentRow.Cells("Proveedor").Value
        UnaLiquidacion.PComision = 0
        UnaLiquidacion.PDescarga = 0
        UnaLiquidacion.PDirecto = 100
        UnaLiquidacion.PBruto = DtCabeza.Rows(0).Item("Importe")
        UnaLiquidacion.PNeto = DtCabeza.Rows(0).Item("Importe")
        UnaLiquidacion.PFactura = DtCabeza.Rows(0).Item("Factura")

        DtCabeza.Dispose()
        DtComproFacturados.Dispose()

        UnaLiquidacion.ShowDialog()
        If UnaLiquidacion.PActualizacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnaLiquidacion.Dispose()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboProveedorFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedorFondoFijo.Validating

        If IsNothing(ComboProveedorFondoFijo.SelectedValue) Then ComboProveedorFondoFijo.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextNumeroFondoFijo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeroFondoFijo.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNumeroFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextNumeroFondoFijo.Validating

        If TextNumeroFondoFijo.Text <> "" Then
            If CInt(TextNumeroFondoFijo.Text) = 0 Then TextNumeroFondoFijo.Text = ""
        End If

    End Sub
    Private Sub TextRendicion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextRendicion.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextRendicion_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextRendicion.Validating

        If TextRendicion.Text <> "" Then
            If CInt(TextRendicion.Text) = 0 Then TextRendicion.Text = ""
        End If

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Facturas de Proveedores Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub GeneraCombosGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Concepto.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 29 & ";")
        Dim Row As DataRow = Concepto.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        Concepto.DataSource.rows.add(Row)
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        IncoTerm.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 38;")
        Dim Row2 As DataRow = IncoTerm.DataSource.newrow
        Row2("Nombre") = ""
        Row2("Clave") = 0
        IncoTerm.DataSource.rows.add(Row2)
        IncoTerm.DisplayMember = "Nombre"
        IncoTerm.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dt = New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

        Dim View As New DataView

        View = Dt.DefaultView
        View.Sort = "Factura"

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRowView In View
            If OpcionOk(Row) Then
                If Row("Operacion") = 1 Or (Row("Operacion") = 2 And Row("Rel")) = 0 Then
                    Dim Row1 As DataRow = DtGrid.NewRow()
                    Row("Importe") = Trunca(Row("Cambio") * Row("Importe"))
                    Row1("Operacion") = Row("Operacion")
                    Row1("Factura") = Row("Factura")
                    Row1("ReciboOficial") = Row("ReciboOficial")
                    Row1("Proveedor") = Row("Proveedor")
                    Row1("Fecha") = Row("Fecha")
                    Row1("FechaContable") = Row("FechaContable")
                    Row1("Rendicion") = Row("Rendicion")
                    Row1("Importe") = Row("Importe")
                    Row1("EsReventa") = Row("EsReventa")
                    Row1("EsInsumos") = Row("EsInsumos")
                    Row1("EsSinComprobante") = Row("EsSinComprobante")
                    Row1("EsCostoLotes") = Row("EsAfectaCostoLotes")
                    Row1("Concepto") = Row("ConceptoGasto")
                    Row1("IncoTerm") = Row("IncoTerm")
                    If Row("NotaDebito") = -1 Then
                        Row1("MarcaNotaDebito") = "X"
                    Else : Row1("MarcaNotaDebito") = ""
                    End If
                    Row1("Estado") = 0
                    If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
                    Row1("Liquidacion") = Row("Liquidacion")
                    Row1("ImporteN") = 0
                    If Row("Operacion") = 1 And Row("Rel") Then
                        RowsBusqueda = Dt.Select("Operacion = 2 AND NRel = " & Row("Factura"))
                        If RowsBusqueda.Length <> 0 Then
                            RowsBusqueda(0).Item("Importe") = Trunca(RowsBusqueda(0).Item("Cambio") * RowsBusqueda(0).Item("Importe"))
                            Row1("OperacionN") = 2
                            Row1("FacturaN") = RowsBusqueda(0).Item("Factura")
                            Row1("ImporteN") = RowsBusqueda(0).Item("Importe")
                            Row1("Total") = Row1("Importe") + Row1("ImporteN")
                        End If
                    End If
                    DtGrid.Rows.Add(Row1)
                End If
            End If
        Next

        If Not (CheckAbierto.Checked And CheckCerrado.Checked) Then    'para controlar opcion por blanco y negro.
            For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
                Dim Row As DataRow = DtGrid.Rows(I)
                If CheckAbierto.Checked Then
                    If Row("Operacion") = 1 Then Row("ImporteN") = 0 : Row("Total") = Row("Importe")
                    If Row("Operacion") = 2 Then Row.Delete()
                End If
                If CheckCerrado.Checked Then
                    If Row("Operacion") = 1 Then
                        If Row("ImporteN") <> 0 Then
                            Row("Importe") = Row("ImporteN") : Row("ImporteN") = 0 : Row("Total") = Row("Importe")
                        Else
                            Row.Delete()
                        End If
                    End If
                End If
            Next
        End If                                                          '-----------------------------------------

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()
        View.Dispose()

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
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Factura As DataColumn = New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim ReciboOficial As DataColumn = New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Concepto)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim FechaContable As New DataColumn("FechaContable")
        FechaContable.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaContable)

        Dim Rendicion As DataColumn = New DataColumn("Rendicion")
        Rendicion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Rendicion)

        Dim EsReventa As New DataColumn("EsReventa")
        EsReventa.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsReventa)

        Dim EsInsumos As New DataColumn("EsInsumos")
        EsInsumos.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsInsumos)

        Dim EsSinComprobante As New DataColumn("EsSinComprobante")
        EsSinComprobante.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsSinComprobante)

        Dim EsCostoLotes As New DataColumn("EsCostoLotes")
        EsCostoLotes.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsCostoLotes)

        Dim Liquidacion As New DataColumn("Liquidacion")
        Liquidacion.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Liquidacion)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim MarcaNotaDebito As DataColumn = New DataColumn("MarcaNotaDebito")
        MarcaNotaDebito.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(MarcaNotaDebito)

        Dim OperacionN As DataColumn = New DataColumn("OperacionN")
        OperacionN.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(OperacionN)

        Dim FacturaN As DataColumn = New DataColumn("FacturaN")
        FacturaN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(FacturaN)

        Dim ImporteN As DataColumn = New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

        Dim IncoTerm As DataColumn = New DataColumn("IncoTerm")
        IncoTerm.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(IncoTerm)

        Dim Estado As DataColumn = New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

    End Sub
    Private Function OpcionOk(ByVal Row As DataRowView) As Boolean

        If TextNumeroFondoFijo.Text = "" And ComboProveedorFondoFijo.SelectedValue = 0 And TextRendicion.Text = "" Then Return True

        If TextRendicion.Text <> "" Then
            If CInt(TextRendicion.Text) <> Row("Rendicion") Then Return False
        End If

        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Row("Operacion") = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If TextNumeroFondoFijo.Text <> "" Or ComboProveedorFondoFijo.SelectedValue <> 0 Then
            If Row("Rendicion") = 0 Then Return False
            If Not Tablas.Read("SELECT FondoFijo,Numero FROM RendicionFondoFijo WHERE Rendicion = " & Row("Rendicion") & ";", ConexionStr, Dt) Then End
            If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False
            If TextNumeroFondoFijo.Text <> "" Then
                If Dt.Rows(0).Item("Numero") <> CInt(TextNumeroFondoFijo.Text) Then Dt.Dispose() : Return False
            End If
            If ComboProveedorFondoFijo.SelectedValue <> 0 Then
                If Dt.Rows(0).Item("FondoFijo") <> ComboProveedorFondoFijo.SelectedValue Then Dt.Dispose() : Return False
            End If
            Dt.Dispose()
        End If

        Return True

    End Function
    Private Function HallaNotaDebito(ByVal Factura As Double, ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT NotaDebito FROM FacturasProveedorCabeza WHERE Factura = " & Factura & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer FacturasProveedorCabeza.", MsgBoxStyle.Critical)
            End
        End Try


    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ComboProveedor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Proveedor o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsDBNull(e.Value) Then
                Dim Numero As String = NumeroEditado(e.Value)
                e.Value = Numero
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            If Not IsDBNull(e.Value) Then
                Dim Numero As String = NumeroEditado(e.Value)
                e.Value = Numero
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 1 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("OperacionN").Value = 2 Then Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaContable" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Liquidacion" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = NumeroEditado(e.Value)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Rendicion" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "MarcaNotaDebito" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "X" Then
                    '             e.CellStyle.BackColor = Color.Yellow
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Or Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2)
                Else : e.Value = Format(e.Value, "#")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Or Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            Dim Factura As Double = 0
            Dim Abierto As Boolean

            Factura = Grid.CurrentCell.Value
            If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Abierto = True
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Abierto = False
            End If
            If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then Abierto = False

            If Grid.Rows(e.RowIndex).Cells("EsReventa").Value Then
                UnaFacturaProveedor.PFactura = Factura
                UnaFacturaProveedor.PCodigoFactura = 900
                UnaFacturaProveedor.PProveedor = Grid.Rows(e.RowIndex).Cells("Proveedor").Value
                UnaFacturaProveedor.PAbierto = Abierto
                UnaFacturaProveedor.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("EsInsumos").Value Then
                UnaFacturaProveedor.PFactura = Factura
                UnaFacturaProveedor.PCodigoFactura = 902
                UnaFacturaProveedor.PProveedor = Grid.Rows(e.RowIndex).Cells("Proveedor").Value
                UnaFacturaProveedor.PAbierto = Abierto
                UnaFacturaProveedor.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("EsSinComprobante").Value Then
                UnaFacturaProveedor.PFactura = Factura
                    UnaFacturaProveedor.PCodigoFactura = 903
                    UnaFacturaProveedor.PProveedor = Grid.Rows(e.RowIndex).Cells("Proveedor").Value
                    UnaFacturaProveedor.PAbierto = Abierto
                    UnaFacturaProveedor.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                    Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("EsCostoLotes").Value Then
                UnaFacturaProveedor.PFactura = Factura
                UnaFacturaProveedor.PCodigoFactura = 901
                UnaFacturaProveedor.PProveedor = Grid.Rows(e.RowIndex).Cells("Proveedor").Value
                UnaFacturaProveedor.PAbierto = Abierto
                UnaFacturaProveedor.ShowDialog()
                If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
                Exit Sub
            End If
        End If

    End Sub


 
End Class