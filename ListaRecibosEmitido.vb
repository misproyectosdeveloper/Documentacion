Public Class ListaRecibosEmitido
    Public PTipoEmisor As Integer
    Public PTipoNota As Integer
    Public PEsFinanciera As Boolean
    '  
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim NotaAnt As Integer = 0
    Dim TipoNota As Integer
    Private Sub ListaRecibosEmitido_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Dim Row As DataRow = ComboProveedor.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboProveedor.DataSource.rows.add(Row)
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAcuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Row = ComboAcuenta.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboAcuenta.DataSource.rows.add(Row)
        ComboAcuenta.DisplayMember = "Nombre"
        ComboAcuenta.ValueMember = "Clave"
        ComboAcuenta.SelectedValue = 0
        With ComboAcuenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboCliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Row = ComboCliente.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.rows.add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        ComboCliente.SelectedValue = 0
        With ComboCliente
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

        ComboTipo.DataSource = ArmaDtTipoNota()
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"
        ComboTipo.SelectedValue = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        TextCaja.Text = Format(GCaja, "000")
        If Not GCajaTotal And Not PermisoTotal Then TextCaja.Enabled = False

        LlenaCombosGrid()

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
            ButtonReenplazoCheque.Visible = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRecibosEmitido_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Entrada.Hide()

    End Sub
    Private Sub ListaRecibosEmitido_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

        Entrada.Show()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextCaja.Text = "" Then TextCaja.Text = Format(GCaja, "000")

        PreparaArchivos()

    End Sub
    Private Sub ButtonReenplazoCheque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReenplazoCheque.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Operacion = " & Grid.CurrentRow.Cells("Operacion").Value & " AND Tipo = " & Grid.CurrentRow.Cells("Tipo").Value & " AND Comprobante = " & Grid.CurrentRow.Cells("Comprobante").Value)
        If RowsBusqueda(0).Item("Estado") <> 0 Then
            MsgBox("Nota Anulada.")
            Exit Sub
        End If
        If Not RowsBusqueda(0).Item("Tr") Then
            MsgBox("Nota No Es CONTABLE.")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnReemplazoChequeEnContable.PAbierto = True
        Else : UnReemplazoChequeEnContable.PAbierto = False
        End If
        UnReemplazoChequeEnContable.PTipoNota = Grid.CurrentRow.Cells("Tipo").Value
        UnReemplazoChequeEnContable.PNota = Grid.CurrentRow.Cells("Comprobante").Value
        UnReemplazoChequeEnContable.ShowDialog()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Recibos Emitidos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboAcuenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAcuenta.Validating

        If IsNothing(ComboAcuenta.SelectedValue) Then ComboAcuenta.SelectedValue = 0

    End Sub
    Private Sub ComboTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

    End Sub
    Private Sub ComboProveedorFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedorFondoFijo.Validating

        If IsNothing(ComboProveedorFondoFijo.SelectedValue) Then ComboProveedorFondoFijo.SelectedValue = 0

    End Sub
    Private Sub TextCaja_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCaja.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNumeroFondoFijo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeroFondoFijo.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNumeroFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextNumeroFondoFijo.Validating

        If TextNumeroFondoFijo.Text <> "" Then
            If CInt(TextNumeroFondoFijo.Text) = 0 Then TextNumeroFondoFijo.Text = ""
        End If

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
    Private Sub PreparaArchivos()

        'Trae la fecha sin hora "SELECT CAST(FLOOR(CAST(fecha AS FLOAT)) AS DATETIME) as fecha  FROM FacturasCabeza" para que lo muestre ordenado.
        'por fecha,tipo,comprobante.

        DtGrid.Clear()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        Dim SqlFecha As String = ""
        SqlFecha = "Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"

        Dim SqlCaja As String = ""
        If CInt(TextCaja.Text) <> 999 Then
            SqlCaja = " AND Caja = " & CInt(TextCaja.Text)
        End If

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = " AND Emisor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlProveedorOtrosPagos As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedorOtrosPagos = " AND Proveedor = " & ComboProveedor.SelectedValue
        End If

        Dim SqlAcuenta As String = ""
        If ComboAcuenta.SelectedValue <> 0 Then
            SqlAcuenta = " AND ACuenta = " & ComboAcuenta.SelectedValue
        End If

        Dim SqlCliente As String = ""
        If ComboCliente.SelectedValue <> 0 Then
            SqlCliente = " AND Emisor = " & ComboCliente.SelectedValue
        End If

        SqlB = "SELECT 1 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio,0 AS NumeroFondoFijo FROM RecibosCabeza WHERE (TipoNota = 60 or TipoNota = 65 or TipoNota = 64) AND " & SqlFecha & SqlCaja & SqlCliente & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,Proveedor As Emisor,0 AS Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Movimiento AS Comprobante,0 AS ReciboOficial,'1/1/1800' AS FechaReciboOficial,Importe,Caja,0 AS ACuenta,0 As Interno,1 AS Cambio,0 AS NumeroFondoFijo FROM OtrosPagosCabeza WHERE (TipoNota = 5010 OR TipoNota = 5020) AND " & SqlFecha & SqlCaja & SqlProveedorOtrosPagos & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio,NumeroFondoFijo FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND (TipoNota = 600 or TipoNota = 604) AND " & SqlFecha & SqlCaja & SqlProveedor & SqlAcuenta & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,Emisor,Tr,Estado, 7004 AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio,NumeroFondoFijo FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND TipoNota = 600 AND " & SqlFecha & SqlCaja & SqlProveedor & SqlAcuenta & _
                   " UNION ALL " & _
               "SELECT 1 AS Operacion,FondoFijo AS Emisor,0 AS Tr,Estado, Tipo,Saldo,Fecha,Movimiento AS Comprobante,0 AS ReciboOficial,'1/1/1800' AS FechaReciboOficial,Importe,Caja,0 AS ACuenta,0 AS Interno,1 AS Cambio,Numero AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza WHERE (Tipo <> 7005 AND Tipo <> 7007) AND " & SqlFecha & SqlCaja & ";"


        SqlN = "SELECT 2 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio,0 AS NumeroFondoFijo FROM RecibosCabeza WHERE (TipoNota = 60 or TipoNota = 65 or TipoNota = 64) AND " & SqlFecha & SqlCaja & SqlCliente & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,Proveedor As Emisor,0 AS Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Movimiento AS Comprobante,0 AS ReciboOficial,'1/1/1800' AS FechaReciboOficial,Importe,Caja,0 AS ACuenta,0 As Interno,1 AS Cambio,0 AS NumeroFondoFijo FROM OtrosPagosCabeza WHERE (TipoNota = 5010 OR TipoNota = 5020) AND " & SqlFecha & SqlCaja & SqlProveedorOtrosPagos & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio, NumeroFondoFijo FROM RecibosCabeza WHERE NumeroFondoFijo = 0 AND (TipoNota = 600 or TipoNota = 604) AND " & SqlFecha & SqlCaja & SqlProveedor & SqlAcuenta & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,Emisor,Tr,Estado, 7004 AS Tipo,Saldo,Fecha,Nota AS Comprobante,ReciboOficial,FechaReciboOficial,Importe,Caja,ACuenta,Interno,Cambio,NumeroFondoFijo FROM RecibosCabeza WHERE NumeroFondoFijo <> 0 AND TipoNota = 600 AND " & SqlFecha & SqlCaja & SqlProveedor & SqlAcuenta & _
                   " UNION ALL " & _
               "SELECT 2 AS Operacion,FondoFijo AS Emisor,0 AS Tr,Estado,Tipo,Saldo,Fecha,Movimiento AS Comprobante,0 AS ReciboOficial,'1/1/1800' AS FechaReciboOficial,Importe,Caja,0 AS ACuenta,0 AS Interno,1 AS Cambio,Numero AS NumeroFondoFijo FROM MovimientosFondoFijoCabeza WHERE (Tipo <> 7005 AND Tipo <> 7007) AND " & SqlFecha & SqlCaja & ";"

        Dim Dt As New DataTable

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Tipo,Comprobante"

        For Each Row As DataRowView In View
            If OpcionOk(Row) Then
                AddGrid(Row)
            End If
        Next

        Dt.Dispose()
        View.Dispose()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub AddGrid(ByVal Row As DataRowView)

        Dim RowGrid As DataRow

        Row("Importe") = Trunca(Row("Importe") * Row("Cambio"))
        Row("Saldo") = Trunca(Row("Saldo") * Row("Cambio"))

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Row("Operacion")
        RowGrid("NombreEmisor") = ""
        If Row("Tipo") = 600 Or Row("Tipo") = 604 Then RowGrid("NombreEmisor") = NombreProveedor(Row("Emisor"))
        If Row("Tipo") = 7004 Then RowGrid("NombreEmisor") = NombreProveedorFondoFijo(Row("Emisor"))
        If Row("Tipo") = 60 Or Row("Tipo") = 65 Or Row("Tipo") = 64 Then RowGrid("NombreEmisor") = NombreCliente(Row("Emisor"))
        If Row("Tipo") = 5010 Or Row("Tipo") = 5020 Then RowGrid("NombreEmisor") = NombreDestino(Row("Emisor"))
        If Row("Tipo") = 7001 Or Row("Tipo") = 7002 Then RowGrid("NombreEmisor") = NombreProveedorFondoFijo(Row("Emisor"))
        RowGrid("Tipo") = Row("Tipo")
        RowGrid("Comprobante") = Row("Comprobante")
        If Row("Interno").ToString.Length > 9 Then
            RowGrid("Interno") = Val(Strings.Right(Row("Interno").ToString, 9))
        Else : RowGrid("Interno") = 0
        End If
        RowGrid("Fecha") = Format(Row("Fecha"), "dd/MM/yyyy 00:00:00")
        RowGrid("ReciboOficial") = Row("ReciboOficial")
        RowGrid("FechaReciboOficial") = Row("FechaReciboOficial")
        RowGrid("Importe") = Row("Importe")
        If Row("Tipo") = 7001 Or Row("Tipo") = 7002 Then
            If FondoCerrado(Row("NumeroFondoFijo"), Row("Operacion")) Then
                Row("Saldo") = 0
            End If
        End If
        RowGrid("Saldo") = Row("Saldo")
        RowGrid("Caja") = Row("Caja")
        RowGrid("Estado") = 0
        If Row("Estado") <> 1 Then RowGrid("Estado") = Row("Estado")
        RowGrid("Mensaje") = ""
        RowGrid("NumeroFondoFijo") = Row("NumeroFondoFijo")
        RowGrid("Tr") = Row("Tr")
        If Row("Tr") And PermisoTotal Then RowGrid("Mensaje") = "CONTABLE"
        If Row("ACuenta") <> 0 Then RowGrid("Mensaje") = "A Cuenta: " & NombreProveedor(Row("ACuenta"))
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Function OpcionOk(ByVal Row As DataRowView) As Boolean

        If ComboTipo.SelectedValue = -600 Then
            If Row("Tipo") = 600 And Row("Emisor") = GProveedorEgresoCaja Then Return True
        End If

        If ComboTipo.SelectedValue <> 0 And ComboTipo.SelectedValue <> Row("Tipo") Then Return False

        If TextNumeroFondoFijo.Text <> "" Then
            If CInt(TextNumeroFondoFijo.Text) <> Row("NumeroFondoFijo") Then Return False
        End If

        If ComboProveedorFondoFijo.SelectedValue <> 0 Then
            If Not (Row("Tipo") = 7004 Or Row("Tipo") = 7001 Or Row("Tipo") = 7002) Then
                Return False
            End If
            If ComboProveedorFondoFijo.SelectedValue <> Row("Emisor") Then Return False
        End If

        Return True

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim NombreEmisor As New DataColumn("NombreEmisor")
        NombreEmisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreEmisor)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim Interno As New DataColumn("Interno")
        Interno.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Interno)

        Dim NumeroFondoFijo As New DataColumn("NumeroFondoFijo")
        NumeroFondoFijo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeroFondoFijo)

        Dim ReciboOficial As New DataColumn("ReciboOficial")
        ReciboOficial.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ReciboOficial)

        Dim FechaReciboOficial As New DataColumn("FechaReciboOficial")
        FechaReciboOficial.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaReciboOficial)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim Tr As New DataColumn("Tr")
        Tr.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Tr)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

    End Sub
    Private Sub LlenaCombosGrid()

        Tipo.DataSource = ArmaDtTipoNota()
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Function FondoCerrado(ByVal Numero As Integer, ByVal Abierto As Boolean) As Boolean

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
    Private Function ArmaDtTipoNota() As DataTable

        Dim Row As DataRow
        Dim Dt As New DataTable

        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Try
            Row = Dt.NewRow
            Row("Clave") = 60
            Row("Nombre") = "Cobranza"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 64
            Row("Nombre") = "Devolución a Cliente"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 65
            Row("Nombre") = "Devolución Seña"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 600
            Row("Nombre") = "Orden pago"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = -600
            Row("Nombre") = "Orden pago Egreso Caja"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 604
            Row("Nombre") = "Devolución del Proveedor"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 5010
            Row("Nombre") = "Otros Pagos"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 5020
            Row("Nombre") = "Devolución del Proveedor"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 7004
            Row("Nombre") = "Reposición"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 7001
            Row("Nombre") = "Aumento F.Fijo"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 7002
            Row("Nombre") = "Disminuye F.Fijo"
            Dt.Rows.Add(Row)
            Row = Dt.NewRow
            Row("Clave") = 0
            Row("Nombre") = ""
            Dt.Rows.Add(Row)
            Return Dt
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Sub ArmaComboTipoFinancieros()

        Dim Row As DataRow

        ComboTipo.DataSource = New DataTable

        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        ComboTipo.DataSource.Columns.Add(Clave)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        ComboTipo.DataSource.Columns.Add(Nombre)

        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Notas Debito (F)"
        ComboTipo.DataSource.Rows.Add(Row)
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 7
        Row("Nombre") = "Notas Credito(F)"
        ComboTipo.DataSource.Rows.Add(Row)
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipo.DataSource.Rows.Add(Row)

        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value <> 0 Then
                If TipoNota <> 8000 And TipoNota <> 65 Then
                    e.Value = NumeroEditado(e.Value)
                Else : e.Value = FormatNumber(e.Value, 0)
                End If
                If PermisoTotal Then
                    If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                    End If
                End If
            Else
                e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Interno" Then
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NumeroFondoFijo" Then
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaReciboOficial" Then
            If Format(e.Value, "dd/MM/yyyy") = "01/01/1800" Then
                e.Value = ""
            Else
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            Dim Abierto As Boolean
            If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
                Abierto = True
            Else
                Abierto = False
            End If
            If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 5010 Or Grid.Rows(e.RowIndex).Cells("Tipo").Value = 5020 Then
                If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 5010 Then
                    If EsReemplazoChequeOtrosProveedores(5010, Grid.Rows(e.RowIndex).Cells("Comprobante").Value, Abierto) Then
                        UnChequeReemplazo.PTipoNota = 5010
                        UnChequeReemplazo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                        UnChequeReemplazo.PAbierto = Abierto
                        UnChequeReemplazo.ShowDialog()
                        If GModificacionOk Then PreparaArchivos()
                        Exit Sub
                    End If
                End If
                UnReciboOtrosProveedores.PAbierto = Abierto
                UnReciboOtrosProveedores.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                UnReciboOtrosProveedores.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                UnReciboOtrosProveedores.ShowDialog()
                If GModificacionOk Then PreparaArchivos()
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 7001 Or Grid.Rows(e.RowIndex).Cells("Tipo").Value = 7002 Then
                If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 7001 Then
                    If EsReemplazoChequeFondoFijo(Grid.Rows(e.RowIndex).Cells("Comprobante").Value, Abierto) Then
                        UnChequeReemplazo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                        UnChequeReemplazo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                        UnChequeReemplazo.PAbierto = Abierto
                        UnChequeReemplazo.POrigenDestino = 6
                        UnChequeReemplazo.ShowDialog()
                        If GModificacionOk Then PreparaArchivos()
                        Exit Sub
                    End If
                End If
                UnMovimientoFondoFijo.PAbierto = Abierto
                UnMovimientoFondoFijo.PMovimiento = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                UnMovimientoFondoFijo.ShowDialog()
                If GModificacionOk Then PreparaArchivos()
                Exit Sub
            End If
            If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 7004 Then
                UnReciboReposicion.PAbierto = Abierto
                UnReciboReposicion.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                UnReciboReposicion.ShowDialog()
                If GModificacionOk Then PreparaArchivos()
                Exit Sub
            End If
            Select Case Grid.Rows(e.RowIndex).Cells("Tipo").Value
                Case 60, 64, 65, 600, 604
                    If EsReemplazoCheque(Grid.Rows(e.RowIndex).Cells("Tipo").Value, Grid.Rows(e.RowIndex).Cells("Comprobante").Value, Abierto) Then
                        UnChequeReemplazo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                        UnChequeReemplazo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante").Value
                        UnChequeReemplazo.PAbierto = Abierto
                        UnChequeReemplazo.ShowDialog()
                        If GModificacionOk Then PreparaArchivos()
                        Exit Sub
                    End If
                    UnRecibo.PAbierto = Abierto
                    UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                    UnRecibo.PNota = Grid.CurrentCell.Value
                    UnRecibo.PEmisor = ComboProveedor.SelectedValue
                    UnRecibo.ShowDialog()
                    If GModificacionOk Then PreparaArchivos()
                Case Else
                    UnReciboDebitoCredito.PAbierto = Abierto
                    UnReciboDebitoCredito.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                    UnReciboDebitoCredito.PNota = Grid.CurrentCell.Value
                    UnReciboDebitoCredito.PEmisor = ComboProveedor.SelectedValue
                    UnReciboDebitoCredito.ShowDialog()
                    If GModificacionOk Then PreparaArchivos()
            End Select
        End If

    End Sub

End Class