Public Class ListaNotasTerceros
    Public PTipoEmisor As Integer
    Public PTipoNota As Integer
    Public PEsFinanciera As Boolean
    '  
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim NotaAnt As Integer = 0
    Dim TipoNota As Integer
    Private Sub ListaNotasTerceros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PTipoEmisor = 1 Then
            LabelClienteProveedor.Text = "Cliente"
            LlenaCombo(ComboEmisor, "", "Clientes")
            Me.BackColor = Color.LemonChiffon
        End If
        If PTipoEmisor = 2 Then
            LabelClienteProveedor.Text = "Proveedor"
            Me.BackColor = Color.PowderBlue
            ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4 " & " ORDER BY Nombre;")
            Dim Row As DataRow = ComboEmisor.DataSource.newrow
            Row("Clave") = 0
            Row("Nombre") = ""
            ComboEmisor.DataSource.rows.add(Row)
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
        End If
        ComboEmisor.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PTipoNota = 60 Or PTipoNota = 600 Then
            LabelTipoNota.Visible = False
            ComboTipo.Visible = False
        Else
            If PTipoEmisor = 1 Then
                If PEsFinanciera Then
                    ArmaComboTipoFinancieros()
                    Me.BackColor = Color.LightGreen
                Else
                    ArmaComboTipoClientes()
                End If
            Else : ArmaTiposProveedor(ComboTipo)
            End If
            ComboTipo.SelectedValue = 0
        End If

        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PTipoNota = 60 Then Me.Text = "Recibo de Cobro"
        If PTipoNota = 600 Then Me.Text = "Orden de Pago"

        LlenaCombosGrid()

        CreaDtGrid()

        If PTipoNota <> 0 Then ButtonAceptar_Click(Nothing, Nothing)

        Entrada.Hide()

    End Sub
    Private Sub ListaNotasTerceros_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        ComboEmisor.Focus()

    End Sub
    Private Sub ListaNotasTerceros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()
        Entrada.Show()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PTipoNota = 60 Or PTipoNota = 600 Then
            TipoNota = PTipoNota
        Else
            If ComboTipo.SelectedValue = 0 Then
                MsgBox("Falta Seleccionar Tipo Nota.", MsgBoxStyle.Information)
                Exit Sub
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PreparaArchivos()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboTipo_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboTipo.SelectedValueChanged

        If Not IsNumeric(ComboTipo.SelectedValue) Then Exit Sub

        CheckImputado.Visible = False : CheckNoImputado.Visible = False
        CheckImputado.Checked = True : CheckNoImputado.Checked = True
        Select Case ComboTipo.SelectedValue
            Case 700, 7, 50, 13007, 6
                CheckImputado.Visible = True : CheckNoImputado.Visible = True
        End Select

    End Sub
    Private Sub ComboTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

        If ComboTipo.SelectedValue = 0 Then ComboTipo.SelectedValue = NotaAnt
        NotaAnt = ComboTipo.SelectedValue
        TipoNota = ComboTipo.SelectedValue

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

        GridAExcel(Grid, Date.Now, "Notas de Terceros Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PreparaArchivos()

        'Trae la fecha sin hora "SELECT CAST(FLOOR(CAST(fecha AS FLOAT)) AS DATETIME) as fecha  FROM FacturasCabeza" para que lo muestre ordenado.
        'por fecha,tipo,comprobante.

        DtGrid.Clear()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        If TipoNota = 8000 Then
            SqlB = "SELECT 1 AS Operacion,RecibosCabeza.Emisor,Reciboscabeza.Tr,RecibosDetallePago.Nota AS Comprobante,RecibosCabeza.Estado AS Estado, RecibosCabeza.TipoNota AS Tipo,0 AS Saldo,CAST(FLOOR(CAST(RecibosCabeza.Fecha AS FLOAT)) AS DATETIME) AS Fecha,RecibosDetallePago.Comprobante AS Comprobante2,CAST(FLOOR(CAST(RecibosDetallePago.FechaComprobante AS FLOAT)) AS DATETIME) as FechaCompro,RecibosDetallePago.Importe AS Importe,RecibosCabeza.Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosDetallePago,RecibosCabeza WHERE RecibosDetallePago.Nota = RecibosCabeza.Nota AND RecibosDetallePago.TipoNota = RecibosCabeza.TipoNota AND MedioPago = " & 8 & " "
            SqlN = "SELECT 2 AS Operacion,RecibosCabeza.Emisor,RecibosCabeza.Tr,RecibosDetallePago.Nota AS Comprobante,RecibosCabeza.Estado AS Estado, RecibosCabeza.TipoNota AS Tipo,0 AS Saldo,CAST(FLOOR(CAST(RecibosCabeza.Fecha AS FLOAT)) AS DATETIME) AS Fecha,RecibosDetallePago.Comprobante AS Comprobante2,CAST(FLOOR(CAST(RecibosDetallePago.FechaComprobante AS FLOAT)) AS DATETIME) as FechaCompro,RecibosDetallePago.Importe AS Importe,RecibosCabeza.Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosDetallePago,RecibosCabeza WHERE RecibosDetallePago.Nota = RecibosCabeza.Nota AND RecibosDetallePago.TipoNota = RecibosCabeza.TipoNota AND MedioPago = " & 8 & " "
        Else
            If TipoNota = 65 Then
                SqlB = "SELECT 1 AS Operacion,Emisor,Tr,Estado,TipoNota AS Tipo,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Nota AS Comprobante,ReciboOficial AS Comprobante2,CAST(FLOOR(CAST(FechaReciboOficial AS FLOAT)) AS DATETIME) AS FechaCompro,Importe,Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosCabeza WHERE TipoNota = " & 65 & " "
                SqlN = "SELECT 2 AS Operacion,Emisor,Tr,Estado,TipoNota AS Tipo,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Nota AS Comprobante,ReciboOficial AS Comprobante2,CAST(FLOOR(CAST(FechaReciboOficial AS FLOAT)) AS DATETIME) AS FechaCompro,Importe,Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosCabeza WHERE TipoNota = " & 65 & " "
            Else
                SqlB = "SELECT 1 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Nota AS Comprobante,ReciboOficial AS Comprobante2,CAST(FLOOR(CAST(FechaReciboOficial AS FLOAT)) AS DATETIME) AS FechaCompro,Importe,Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " "
                SqlN = "SELECT 2 AS Operacion,Emisor,Tr,Estado, TipoNota AS Tipo,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Nota AS Comprobante,ReciboOficial AS Comprobante2,CAST(FLOOR(CAST(FechaReciboOficial AS FLOAT)) AS DATETIME) AS FechaCompro,Importe,Caja,Comentario,RecibosCabeza.Cambio,EsFCE FROM RecibosCabeza WHERE TipoNota = " & TipoNota & " "
            End If
        End If

        Dim SqlFecha As String
        SqlFecha = " AND Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"
        Dim SqlFecha8 As String
        SqlFecha8 = " AND RecibosDetallePago.FechaComprobante BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"
        Dim SqlFecha65 As String
        SqlFecha65 = " AND FechaReciboOficial BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"
        Dim SqlFecha30 As String
        SqlFecha30 = " Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim StrEmisor30 As String
        Dim StrEmisor As String
        If ComboEmisor.SelectedValue <> 0 Then
            StrEmisor30 = " AND Cliente = " & ComboEmisor.SelectedValue
            StrEmisor = " AND RecibosCabeza.Emisor = " & ComboEmisor.SelectedValue
        End If

        If TipoNota = 8000 Then
            SqlB = SqlB & SqlFecha8
            SqlN = SqlN & SqlFecha8
        Else
            If TipoNota = 65 Then
                SqlB = SqlB & SqlFecha65
                SqlN = SqlN & SqlFecha65
            Else
                SqlB = SqlB & SqlFecha
                SqlN = SqlN & SqlFecha
            End If
        End If

        Dim SqlImputado As String = ""
        If CheckImputado.Checked And Not CheckNoImputado.Checked Then
            SqlImputado = " AND Saldo = 0"
        End If
        If Not CheckImputado.Checked And CheckNoImputado.Checked Then
            SqlImputado = " AND Saldo <> 0"
        End If

        SqlB = SqlB & StrEmisor & SqlEstado & SqlImputado & ";"
        SqlN = SqlN & StrEmisor & SqlEstado & SqlImputado & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Comprobante"

        If TipoNota = 50 Then
            Grid.Columns("Comprobante").HeaderText = "N.Debito(C)"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If
        If TipoNota = 60 Then
            Grid.Columns("Comprobante").HeaderText = "Recibo Pago"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If
        If TipoNota = 70 Then
            Grid.Columns("Comprobante").HeaderText = "N.Credito(C)"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If
        If TipoNota = 65 Or TipoNota = 8000 Then
            Grid.Columns("Comprobante").HeaderText = "    Vale"
            Grid.Columns("Comprobante2").HeaderText = "    Recibo"
        End If
        If TipoNota = 500 Then
            Grid.Columns("Comprobante").HeaderText = "N.Debito(P)"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If
        If TipoNota = 600 Then
            Grid.Columns("Comprobante").HeaderText = "Orden pago"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If
        If TipoNota = 700 Then
            Grid.Columns("Comprobante").HeaderText = "N.Credito(P)"
            Grid.Columns("Comprobante2").HeaderText = "Recibo Of."
        End If

        For Each Row As DataRowView In View
            '    If GCajaTotal Or (Not GCajaTotal And GCaja = Row("Caja")) Then
            Row("Importe") = Trunca(Row("Importe") * Row("Cambio"))
            Row("Saldo") = Trunca(Row("Saldo") * Row("Cambio"))
            Dim Cartel As String = ""
            If Row("EsFCE") Then Cartel = "Fac.FCE"
            If TipoNota = 8000 Or TipoNota = 65 Then
                AddGrid(Row("Operacion"), Row("Emisor"), Row("Tr"), Row("FechaCompro"), Row("Tipo"), Row("Comprobante2"), Row("Comprobante"), Row("Fecha"), Row("Importe"), Row("Saldo"), Row("Estado"), Row("Comentario"), Cartel)
            Else
                AddGrid(Row("Operacion"), Row("Emisor"), Row("Tr"), Row("Fecha"), Row("Tipo"), Row("Comprobante"), Row("Comprobante2"), Row("FechaCompro"), Row("Importe"), Row("Saldo"), Row("Estado"), Row("Comentario"), Cartel)
            End If
            '    End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub AddGrid(ByVal Operacion As Integer, ByVal Emisor As Integer, ByVal Tr As Boolean, ByVal Fecha As DateTime, ByVal Tipo As Integer, ByVal Comprobante As Double, ByVal Comprobante2 As Double, _
    ByVal FechaCompro As Date, ByVal Importe As Double, ByVal Saldo As Double, ByVal Estado As Integer, ByVal Comentario As String, ByVal Cartel As String)

        Dim RowGrid As DataRow

        RowGrid = DtGrid.NewRow()
        RowGrid("Operacion") = Operacion
        RowGrid("Emisor") = Emisor
        RowGrid("Fecha") = Fecha
        RowGrid("Tipo") = Tipo
        RowGrid("Comprobante") = Comprobante
        RowGrid("Comprobante2") = Comprobante2
        RowGrid("Cartel") = Cartel
        RowGrid("FechaCompro") = FechaCompro
        RowGrid("Comentario") = Comentario
        RowGrid("Importe") = Importe
        RowGrid("Saldo") = Saldo
        RowGrid("Estado") = 0
        If Estado <> 1 Then RowGrid("Estado") = Estado
        RowGrid("Mensaje") = ""
        If Tr And PermisoTotal Then RowGrid("Mensaje") = "CONTABLE"
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Emisor)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim Comprobante2 As New DataColumn("Comprobante2")
        Comprobante2.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante2)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Comentario)

        Dim FechaCompro As New DataColumn("FechaCompro")
        FechaCompro.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaCompro)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Saldo)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Mensaje As New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        If PTipoEmisor = 1 Then
            Emisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Else : Emisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion <> 4;")
        End If
        Emisor.DisplayMember = "Nombre"
        Emisor.ValueMember = "Clave"

    End Sub
    Private Sub ArmaComboTipoClientes()

        Dim Row As DataRow

        ComboTipo.DataSource = New DataTable

        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        ComboTipo.DataSource.Columns.Add(Clave)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        ComboTipo.DataSource.Columns.Add(Nombre)

        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 50
        Row("Nombre") = "Notas Debito del Clientes"
        ComboTipo.DataSource.Rows.Add(Row)
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 70
        Row("Nombre") = "Notas Credito del Clientes"
        ComboTipo.DataSource.Rows.Add(Row)
        If PermisoTotal Then
            Row = ComboTipo.DataSource.NewRow
            Row("Clave") = 65
            Row("Nombre") = "Devolucion Señas"
            ComboTipo.DataSource.Rows.Add(Row)
            Row = ComboTipo.DataSource.NewRow
            Row("Clave") = 8000
            Row("Nombre") = "Vales En Cta.Cte."
            ComboTipo.DataSource.Rows.Add(Row)
        End If
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboTipo.DataSource.Rows.Add(Row)

        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"

    End Sub
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
        Row("Nombre") = "Notas Debito(F)"
        ComboTipo.DataSource.Rows.Add(Row)
        '
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 7
        Row("Nombre") = "Notas Credito(F)"
        ComboTipo.DataSource.Rows.Add(Row)
        '
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 13005
        Row("Nombre") = "Notas Debito(F) Interna"
        ComboTipo.DataSource.Rows.Add(Row)
        '
        Row = ComboTipo.DataSource.NewRow
        Row("Clave") = 13007
        Row("Nombre") = "Notas Credito(F) Interna"
        ComboTipo.DataSource.Rows.Add(Row)
        '
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

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante2" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
            If Grid.Rows(e.RowIndex).Cells("Comprobante2").Value = 0 Then e.Value = ""
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
            If Grid.Rows(e.RowIndex).Cells("Tipo").Value = 65 Then
                UnRecibo.PAbierto = Abierto
                UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                UnRecibo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante2").Value
                UnRecibo.PEmisor = ComboEmisor.SelectedValue
                UnRecibo.ShowDialog()
                If GModificacionOk Then PreparaArchivos()
                Exit Sub
            End If
            If TipoNota = 8000 Then
                UnRecibo.PAbierto = Abierto
                UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                UnRecibo.PNota = Grid.Rows(e.RowIndex).Cells("Comprobante2").Value
                UnRecibo.PEmisor = ComboEmisor.SelectedValue
                UnRecibo.Show()
                If GModificacionOk Then PreparaArchivos()
                Exit Sub
            End If
            Select Case TipoNota
                Case 60, 64, 65, 600, 604
                    UnRecibo.PAbierto = Abierto
                    UnRecibo.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                    UnRecibo.PNota = Grid.CurrentCell.Value
                    UnRecibo.PEmisor = ComboEmisor.SelectedValue
                    UnRecibo.ShowDialog()
                    If GModificacionOk Then PreparaArchivos()
                Case Else
                    UnReciboDebitoCredito.PAbierto = Abierto
                    UnReciboDebitoCredito.PTipoNota = Grid.Rows(e.RowIndex).Cells("Tipo").Value
                    UnReciboDebitoCredito.PNota = Grid.CurrentCell.Value
                    UnReciboDebitoCredito.PEmisor = ComboEmisor.SelectedValue
                    UnReciboDebitoCredito.ShowDialog()
                    If GModificacionOk Then PreparaArchivos()
            End Select
        End If

    End Sub
  
End Class