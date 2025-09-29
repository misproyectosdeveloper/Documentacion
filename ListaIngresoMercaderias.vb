Public Class ListaIngresoMercaderias
    Public PBloqueaFunciones As Boolean
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    '
    Dim DtIngresos As DataTable
    Dim DtGrid As DataTable
    Private Sub IngresoMercaderia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GeneraComboGrid()

        LlenaComboTablas(ComboDeposito, 19)
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboDeposito.SelectedValue = 0

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE Producto = " & Fruta & " OR TipoOperacion = 4 " & " ORDER BY Nombre;")
        Dim Row As DataRow = ComboProveedor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.Rows.Add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND (Producto = " & Fruta & " OR TipoOperacion = 4)" & " ORDER BY Alias;")
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

        CreaDtGrid()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaIngresoMercaderias_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 AS Operacion,C.Proveedor,C.Deposito,C.Fecha,C.Remito,C.Guia,C.Lote,C.TipoOperacion,C.Moneda,C.RemitoCliente,C.Sucursal FROM IngresoMercaderiasCabeza AS C "
        SqlN = "SELECT 2 AS Operacion,C.Proveedor,C.Deposito,C.Fecha,C.Remito,C.Guia,C.Lote,C.TipoOperacion,C.Moneda,C.RemitoCliente,C.Sucursal FROM IngresoMercaderiasCabeza AS C "

        Dim Proveedor As Integer = 0
        If ComboProveedor.SelectedValue <> 0 Then Proveedor = ComboProveedor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Proveedor = ComboAlias.SelectedValue

        Dim SqlProveedor As String
        If Proveedor = 0 Then
            SqlProveedor = "WHERE Proveedor LIKE '%' "
        Else
            SqlProveedor = "WHERE Proveedor = " & Proveedor & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlRemito As String = ""
        If MaskedRemito.Text <> "" Then
            SqlRemito = "AND Remito = " & Val(MaskedRemito.Text) & " "
        End If

        Dim SqlGuia As String = ""
        If TextGuia.Text <> "" Then
            SqlGuia = "AND Guia = " & Val(TextGuia.Text) & " "
        End If

        Dim SqlOrdenCompra As String = ""
        If TextOrdenCompra.Text <> "" Then
            SqlOrdenCompra = "AND OrdenCompra = " & Val(TextOrdenCompra.Text) & " "
        End If

        SqlB = SqlB & SqlProveedor & SqlDeposito & SqlFecha & SqlRemito & SqlGuia & SqlOrdenCompra & ";"
        SqlN = SqlN & SqlProveedor & SqlDeposito & SqlFecha & SqlRemito & SqlGuia & SqlOrdenCompra & ";"

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

    End Sub
    Private Sub ButtonDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolucion.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        UnaDevolucionLote.PLote = Grid.CurrentRow.Cells("Lote").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaDevolucionLote.PAbierto = True
        Else : UnaDevolucionLote.PAbierto = False
        End If
        UnaDevolucionLote.ShowDialog()
        UnaDevolucionLote.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonVerDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerDetalle.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        SeleccionarVarios.PEsDetalleIngreso = True
        SeleccionarVarios.PLote = Grid.CurrentRow.Cells("Lote").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            SeleccionarVarios.PAbierto = True
        Else : SeleccionarVarios.PAbierto = False
        End If
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub TextFechaDesde_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFechaHasta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextGuia_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextGuia.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextOrdenCompra_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextOrdenCompra.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Ingreso de Lotes", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckFacturados.Checked And Not CheckNoFacturados.Checked Then
            CheckFacturados.Checked = True
            CheckNoFacturados.Checked = True
        End If

        DtIngresos = New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, DtIngresos) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, DtIngresos) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView

        View = DtIngresos.DefaultView
        View.Sort = "Lote"
        DtGrid.Clear()

        Dim Liquidado As Integer
        Dim Cartel As String
        Dim Color As Integer

        For Each Row As DataRowView In View
            If Not HallaLiquidacion(Row("Operacion"), Row("Lote"), Liquidado) Then Me.Close() : Exit Sub
            If Liquidado = 1 Then
                Cartel = ""
            Else : Cartel = "Facturado"
            End If
            Color = 0
            If (CheckFacturados.Checked And CheckNoFacturados.Checked) Or (CheckFacturados.Checked And Cartel <> "") Or (CheckNoFacturados.Checked And Cartel = "") Then
                If Row("Moneda") <> 1 Then Color = 1
                AgregaADtGrid(Row("Operacion"), Row("Lote"), Row("Proveedor"), Row("Deposito"), Row("Remito"), Row("Guia"), Row("Fecha"), Row("TipoOperacion"), Row("RemitoCliente"), Row("Sucursal"), Cartel, Color)
            End If
        Next

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As DataColumn = New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim TipoOperacion As New DataColumn("TipoOperacion")
        TipoOperacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoOperacion)

        Dim Proveedor As DataColumn = New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Deposito As DataColumn = New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Remito As DataColumn = New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Guia As DataColumn = New DataColumn("Guia")
        Guia.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Guia)

        Dim Fecha As DataColumn = New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim RemitoCliente As DataColumn = New DataColumn("RemitoCliente")
        RemitoCliente.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(RemitoCliente)

        Dim Sucursal As DataColumn = New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Sucursal)

    End Sub
    Private Sub AgregaADtGrid(ByVal Operacion As Integer, ByVal Lote As Integer, ByVal Proveedor As Integer, ByVal Deposito As Integer, ByVal Remito As Double, ByVal Guia As Double, ByVal Fecha As Date, ByVal TipoOperacion As Integer, ByVal RemitoCliente As Decimal, ByVal Sucursal As Integer, ByVal Cartel As String, ByVal Color As Integer)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Operacion") = Operacion
        Row("Color") = Color
        Row("Lote") = Lote
        Row("TipoOperacion") = TipoOperacion
        Row("Proveedor") = Proveedor
        Row("Deposito") = Deposito
        Row("Remito") = Remito
        Row("RemitoCliente") = RemitoCliente
        Row("Sucursal") = Sucursal
        Row("Guia") = Guia
        Row("Fecha") = Fecha
        Row("Cartel") = Cartel
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function HallaLiquidacion(ByVal Operacion As Integer, ByVal Lote As Integer, ByRef Liquidado As Integer) As Boolean

        Dim sql As String
        Dim Dt As New DataTable
        Dim ConexionStr As String

        Liquidado = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        sql = "SELECT Liquidado FROM Lotes WHERE Lote = LoteOrigen AND Secuencia = SecuenciaOrigen AND Deposito = DepositoOrigen AND Lote = " & Lote & " AND Cantidad <> Baja;"
        If Not Tablas.Read(sql, ConexionStr, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Liquidado") = 0 Then
                Liquidado = 1
            End If
        Next

        If Dt.Rows.Count = 0 Then Liquidado = 1

        Dt.Dispose()
        Return True

    End Function
    Private Sub GeneraComboGrid()

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        TipoOperacion.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Miselaneas WHERE Codigo = 1;")
        TipoOperacion.DisplayMember = "Nombre"
        TipoOperacion.ValueMember = "Clave"

        Sucursal.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SucursalesProveedores;")
        Dim Row As DataRow = Sucursal.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Sucursal.DataSource.Rows.Add(Row)
        Sucursal.DisplayMember = "Nombre"
        Sucursal.ValueMember = "Clave"

    End Sub
    Private Function Valida() As Boolean

        If MaskedRemito.Text <> "" And Not MaskedRemito.MaskCompleted Then
            MsgBox("Debe completar Remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If

        If MaskedRemito.Text <> "" And TextGuia.Text <> "" Then
            MsgBox("Debe Informar Remito o Guia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextGuia.Focus()
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
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

        If Grid.Columns(e.ColumnIndex).Name = "Proveedor" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
            End If
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.LightBlue
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Guia" Then
            If Not IsNothing(e.Value) Then e.Value = NumeroEditado(e.Value)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsNothing(e.Value) Then
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoCliente" Then
            If e.Value <> 0 Then
                e.Value = Format(e.Value, "0000-00000000")
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If


    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        Dim Abierto As Boolean
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If
        UnIngresoMercaderia.PLote = Grid.CurrentRow.Cells("Lote").Value
        UnIngresoMercaderia.PAbierto = Abierto
        UnIngresoMercaderia.ShowDialog()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        UnIngresoMercaderia.Dispose()

    End Sub

End Class