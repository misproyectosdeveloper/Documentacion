Public Class UnChequesEnCartera
    Public PBloqueaFunciones As Boolean
    Dim DtTotal As DataTable
    Dim DtGrid As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim FechaDesde As Date
    Dim FechaHasta As Date
    Dim FechaPrimerHasta As Date = "1/1/1800"
    Dim Total As Double
    Private Sub UnaConciliacionChequesTerceros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboProveedor, "", "Proveedores")
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboBanco, 26)
        ComboBanco.SelectedValue = 0
        With ComboBanco
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = 0
        With ComboFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        TextCaja.Text = Format(999, "000")
        'anulado a pedido cliente para que vea todo los movimientos.
        '     TextCaja.Text = Format(GCaja, "000")
        '      If Not GCajaTotal Then TextCaja.Enabled = False

        LlenaCombosGrid()

        DateTimeDesde.Value = DateTimeHasta.Value.AddDays(-30)
        DateTimeHasta.Value = "01/01/" & Date.Now.Year + 5

        LLenaGrid()

        If DtGrid.Rows.Count = 1 Then
            MsgBox("No Existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        DateTimeDesde.Value = DtGrid.Rows(0).Item("Vencimiento")
        DateTimeHasta.Value = DtGrid.Rows(DtGrid.Rows.Count - 2).Item("Vencimiento")

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaConciliacionChequesTerceros_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        For Each Row As DataRow In DtGrid.Rows
            If Row("ClaveCheque") <> 0 Then
                If DiferenciaDias(Row("VencimientoAnt"), Row("Vencimiento")) <> 0 Or Row("Ord") <> Row("ordAnt") Then
                    If MsgBox("Los cambios no fueron Actualizados. Quiere Aceptar Cambios antes de regresar al menu?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                        e.Cancel = True : Exit Sub
                    Else
                        Exit Sub
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub ButtonActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActualizar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If CInt(TextCaja.Text) <> GCaja Then
            MsgBox("Actualización solo Permitida a la caja " & GCaja)
            Exit Sub
        End If

        Dim HayErrores As Boolean
        Dim HuboCambios As Boolean

        For Each Row As DataRow In DtGrid.Rows
            If Row("ClaveCheque") <> 0 Then
                If DiferenciaDias(Row("VencimientoAnt"), Row("Vencimiento")) <> 0 Or Row("Ord") <> Row("OrdAnt") Or Row("eCheq") <> Row("eCheqAnt") Then
                    HuboCambios = True
                    If Not GrabaVencimiento(Row("ClaveCheque"), Row("Operacion"), Row("Vencimiento"), Row("Ord"), Row("eCheq")) Then HayErrores = True
                    '       Row("VencimientoAnt") = Row("Vencimiento")
                    '      Row("OrdAnt") = Row("Ord")
                End If
            End If
        Next

        If HayErrores Then
            MsgBox("Algunos Cambio NO Pudieron Realizarse Pues Fueron Modificados Por Otro USUARIO.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            LLenaGrid()
        Else
            If HuboCambios Then
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                LLenaGrid()
            End If
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextCaja.Text = "" Then TextCaja.Text = Format(GCaja, "000")

        LLenaGrid()

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("Sel2").Visible = False
        Grid.Columns("Candado").Visible = False

        GridAExcel(Grid, GNombreEmpresa, "CHEQUES EN CARTERA DESDE " & DateTimeDesde.Text & "  HASTA " & DateTimeHasta.Text, "")

        Grid.Columns("Sel2").Visible = True
        Grid.Columns("Candado").Visible = True

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
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboBanco_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBanco.Validating

        If IsNothing(ComboBanco.SelectedValue) Then ComboBanco.SelectedValue = 0

    End Sub
    Private Sub TextCaja_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCaja.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If
        If Not CheckEnCartera.Checked And Not CheckEntregados.Checked Then
            CheckEnCartera.Checked = True
            CheckEntregados.Checked = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        'Origen 1:   Banco.
        'Origen 2:   Proveedores.
        'Origen 3:   Cliente.
        'Origen 4:   Empleado.
        'Origen 5:   Otros Proveedores.
        'Origen 6:   Fondo Fijo.

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtTotal = New DataTable

        Dim Abierto As Boolean
        Dim Cerrado As Boolean
        Dim EnCartera As Boolean
        Dim Entregado As Boolean

        EnCartera = CheckEnCartera.Checked
        Entregado = CheckEntregados.Checked
        Abierto = CheckAbierto.Checked
        Cerrado = CheckCerrado.Checked

        If Not ArmaDtChequesTerceros(DtTotal, CInt(TextCaja.Text), DateTimeDesde.Value, DateTimeHasta.Value, Abierto, Cerrado, EnCartera, Entregado, 0) Then Me.Close() : Exit Sub

        Dim Moneda As Integer
        Dim Resul As Integer

        Total = 0

        Dim View As New DataView
        View = DtTotal.DefaultView
        View.Sort = "Fecha"

        Dim NombreEmisor As String
        Dim NombreDestino1 As String

        For Each Row As DataRowView In View
            If Row("Estado") <> 3 Then
                NombreEmisor = ""
                NombreDestino1 = ""
                Resul = DatoOk(Row, NombreEmisor, NombreDestino1)
                If Resul = -1 Then Me.Close() : Exit Sub
                If Resul = 0 Then
                    Moneda = 1
                    LineaDetalle(Row, Moneda, NombreEmisor, NombreDestino1, Row("FechaRecibido"), Row("FechaDestino"))
                End If
            End If
        Next
        LineaTotales(Total)

        Grid.DataSource = Nothing
        Grid.DataSource = bs
        bs.DataSource = DtGrid

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LineaDetalle(ByVal Row As DataRowView, ByVal Moneda As Integer, ByVal NombreEmisor As String, ByVal NombreDestino As String, ByVal FechaRecibo As Date, ByVal FechaEntregado As Date)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Color") = 1
        Row1("ColorTexto") = 0
        Row1("Operacion") = Row("Operacion")
        Row1("Vencimiento") = Row("Fecha")
        Row1("VencimientoAnt") = Row("Fecha")
        Row1("EmisorRecibido") = NombreEmisor
        Row1("FechaRecibido") = FechaRecibo
        Row1("EmisorCheque") = Row("EmisorCheque")
        Row1("Banco") = Row("Banco")
        Row1("Moneda") = Moneda
        Row1("Numero") = Row("Numero")
        Row1("FechaEntregado") = FechaEntregado
        Row1("EmisorEntregado") = NombreDestino
        Row1("ClaveCheque") = Row("ClaveCheque")
        Row1("Caja") = Row("Caja")
        Row1("Importe") = Row("Importe")
        Row1("Estado") = Row("Estado")
        Row1("Ord") = Row("Ord")
        Row1("OrdAnt") = Row("Ord")
        Row1("eCheq") = Row("eCheq")    'eCheq.
        Row1("eCheqAnt") = Row("eCheq")    'eCheq.
        If Row("Afectado") <> 0 And PermisoTotal And CheckAfectado.Checked Then
            Row1("Cartel") = HallaAfectado(Row("Afectado"))
        End If
        Row1("EstadoCh") = 0
        If ChequeVencido(Row("Fecha"), Date.Now) And Row("Estado") = 1 And NombreDestino = "" Then
            Row1("Cartel") = "VENCIDO"
            Row1("Color") = 6
            Row1("EstadoCh") = 5
        End If
        If Row("Estado") = 4 Then
            Row1("Cartel") = "RECHAZADO"
            Row1("Color") = 4
            Row1("EstadoCh") = 4
        End If
        If Row1("EstadoCh") = 0 And NombreDestino = "" Then Total = Total + Row("Importe")

        DtGrid.Rows.Add(Row1)

    End Sub
    Private Sub LineaTotales(ByVal Total As Double)

        Dim Row3 As DataRow = DtGrid.NewRow
        Row3("Color") = 1
        Row3("ColorTexto") = 0
        Row3("Operacion") = 3
        Row3("EmisorRecibido") = ""
        Row3("Moneda") = 1
        Row3("Importe") = Total
        Row3("EstadoCh") = 1
        Row3("ClaveCheque") = 0
        Row3("Banco") = 1
        DtGrid.Rows.Add(Row3)

    End Sub
    Private Function DatoOk(ByVal Row As DataRowView, ByRef NombreEmisor As String, ByRef NombreDestino As String) As Integer

        If CheckAbierto.Checked And Not CheckCerrado.Checked And Row("Operacion") = 2 Then Return 1
        If Not CheckAbierto.Checked And CheckCerrado.Checked And Row("Operacion") = 1 Then Return 1
        If CheckSoloRechazados.Checked And Row("Estado") <> 4 Then Return 1
        If CheckSoloeCheq.Checked And Row("Serie") <> "*" Then Return 1

        If Row("Estado") <> 4 Then
            If CheckEnCartera.Checked And Not CheckEntregados.Checked And Row("CompDestino") <> 0 Then Return 1
            '     If CheckEnCartera.Checked And Not CheckEntregados.Checked And ChequeVencido(Row("Fecha"), Date.Now) Then Return 1
            If CheckEntregados.Checked And Not CheckEnCartera.Checked And Row("CompDestino") = 0 Then Return 1
        End If

        NombreEmisor = HallaNombreEmisor(Row("Origen"), Row("Emisor"))
        If Row("Destino") <> 0 Then NombreDestino = HallaNombreDestino(Row("Destino"), Row("EmisorDestino"))

        If ComboProveedor.SelectedValue <> 0 Then
            If ComboProveedor.Text <> NombreEmisor And ComboProveedor.Text <> NombreDestino Then Return 1
        End If

        If ComboCliente.SelectedValue <> 0 Then
            If ComboCliente.Text <> NombreEmisor And ComboCliente.Text <> NombreDestino Then Return 1
        End If

        If ComboBanco.SelectedValue <> 0 Then
            If ComboBanco.Text <> NombreEmisor And ComboBanco.Text <> NombreDestino Then Return 1
        End If

        If ComboFondoFijo.SelectedValue <> 0 Then
            If ComboFondoFijo.Text <> NombreEmisor And ComboFondoFijo.Text <> NombreDestino Then Return 1
        End If

        Return 0

    End Function
    Private Function GrabaVencimiento(ByVal ClaveCheque As Integer, ByVal Operacion As Integer, ByVal Fecha As Date, ByVal Ord As Boolean, ByVal eCheq As Boolean) As Boolean

        Dim ConexionStr As String
        Dim RowsBusqueda() As DataRow
        Dim OrdW As Integer
        Dim eCheqW As Integer     'eCheq.

        If Ord Then
            OrdW = 1
        Else : OrdW = 0
        End If

        If eCheq Then       'eCheq.
            eCheqW = 1
        Else : eCheqW = 0
        End If

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Dim Sql As String = "UPDATE " & "Cheques" & _
                    " Set Fecha = '" & Format(Fecha, "yyyyMMdd") & "',Ord = " & OrdW & ",eCheq = " & eCheqW & " WHERE ClaveCheque = " & ClaveCheque & ";"      'eCheq.
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                If Resul = 0 Then
                    Return False
                Else
                    '     RowsBusqueda = DtTotal.Select("Operacion = " & Operacion & " AND ClaveCheque= " & ClaveCheque)
                    '    RowsBusqueda(0).Item("Vencimiento") = Fecha
                    Return True
                End If
            End Using
        Catch ex As Exception
            Return False
        Finally
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 ORDER BY Nombre;")

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "En Cartera"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Entregado"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 3
        Row("Nombre") = "En Cartera Anio"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 4
        Row("Nombre") = "Entregado Anio"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 5
        Row("Nombre") = "En Cartera Anterior"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 6
        Row("Nombre") = "Entregado Anterior"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 7
        Row("Nombre") = "En Cartera Gral."
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 8
        Row("Nombre") = "Entregado Gral."
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 ORDER BY Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim ColorTexto As New DataColumn("ColorTexto")
        ColorTexto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ColorTexto)

        Dim EstadoCh As New DataColumn("EstadoCh")
        EstadoCh.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(EstadoCh)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Vencimiento As New DataColumn("Vencimiento")
        Vencimiento.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Vencimiento)

        Dim VencimientoAnt As New DataColumn("VencimientoAnt")
        VencimientoAnt.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(VencimientoAnt)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim FechaEntregado As New DataColumn("FechaEntregado")
        FechaEntregado.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaEntregado)

        Dim EmisorEntregado As New DataColumn("EmisorEntregado")
        EmisorEntregado.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorEntregado)

        Dim FechaRecibido As New DataColumn("FechaRecibido")
        FechaRecibido.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaRecibido)

        Dim EmisorRecibido As New DataColumn("EmisorRecibido")
        EmisorRecibido.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorRecibido)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Ord As New DataColumn("Ord")
        Ord.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Ord)

        Dim OrdAnt As New DataColumn("OrdAnt")
        OrdAnt.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(OrdAnt)

        Dim eCheq As New DataColumn("eCheq")                            'eCheq.
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheq)

        Dim eCheqAnt As New DataColumn("eCheqAnt")                      'eCheq.
        eCheqAnt.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheqAnt)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        Grid.Rows(e.RowIndex).Cells("Sel2").Value = False

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.WhiteSmoke
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 3 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 4 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Red
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 6 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.NavajoWhite
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 5 Then Grid.Rows(e.RowIndex).Height = 2
            If Grid.Rows(e.RowIndex).Cells("ColorTexto").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Drawing.Color.Red
            If Grid.Rows(e.RowIndex).Cells("Banco").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Importe").Style.ForeColor = Drawing.Color.Green
            If Grid.Rows(e.RowIndex).Cells("Banco").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Importe").Style.ForeColor = Drawing.Color.Green
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            e.Value = FormatNumber(e.Value, GDecimales)
            Grid.Rows(e.RowIndex).Cells("Sel2").Value = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaRecibido" Or Grid.Columns(e.ColumnIndex).Name = "Vencimiento" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaEntregado" Then
            If Not IsDBNull(e.Value) Then
                If DiferenciaDias(e.Value, CDate("1/1/1800")) <> 0 Then
                    e.Value = Format(e.Value, "dd/MM/yyyy")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "ClaveCheque" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Caja" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "000")
            End If
        End If

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If Grid.Columns(e.ColumnIndex).Name = "Sel2" Then
            If Not (Grid.Rows(e.RowIndex).Cells("Estado").Value = 1 And Grid.Rows(e.RowIndex).Cells("Entregado").Value = "") Then
                Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Vencimiento")
                Exit Sub
            End If
            Calendario.PFecha = Grid.Rows(e.RowIndex).Cells("Vencimiento").Value
            Calendario.ShowDialog()
            If DiferenciaDias(Calendario.PFecha, Grid.Rows(e.RowIndex).Cells("Vencimiento").Value) > 0 Then
                MsgBox("Nueva Fecha Vencimiento debe ser mayor al Vencimiento Original.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Else
                Grid.Rows(e.RowIndex).Cells("Vencimiento").Value = Format(Calendario.PFecha, "dd/MM/yyyy")
            End If
            Calendario.Dispose()
            Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Vencimiento")
        End If


    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        '  If (e.Column.ColumnName.Equals("Sel")) Then
        '  End If

    End Sub


End Class
