Imports System.Transactions
Imports System.Math
Public Class ListaFacturas
    Public PSoloPendientes As Boolean
    Public PBloqueaFunciones As Boolean
    Dim SqlB As String
    Dim SqlN As String
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    Dim DtGrid As DataTable
    Dim HayError As Boolean
    Private Sub ListaFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 40

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboCliente.DataSource = Tablas.Leer("Select Clave,Nombre From Clientes WHERE TipoIva <> 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboCliente.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboCliente.DataSource.Rows.Add(Row)
        ComboCliente.DisplayMember = "Nombre"
        ComboCliente.ValueMember = "Clave"
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboCliente.SelectedValue = 0

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE TipoIva <> 4  AND Alias <> '';")
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

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If Not PermisoTotal Then
            Grid.Columns("CandadoN").HeaderText = ""
            Grid.Columns("FacturaN").HeaderText = ""
            Grid.Columns("ImporteN").HeaderText = ""
            Grid.Columns("Total").HeaderText = ""
        End If

        MaskedFactura.Text = "000000000000"

        If PSoloPendientes Then ComboEstado.SelectedValue = 2

        If Not PermisoTotal Then
            PanelCandados.Visible = False
            CheckCerrado.Checked = False
            Grid.Columns("Candado").Visible = False
        Else
            Grid.Columns("Candado").Visible = True
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaFacturas_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaFacturas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()
        Entrada.Activate()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        SqlFecha = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlB = "SELECT 1 as Operacion,* FROM FacturasCabeza WHERE ClienteOperacion = 0 AND " & SqlFecha & " "
        SqlN = "SELECT 2 as Operacion,* FROM FacturasCabeza WHERE ClienteOperacion = 0 AND " & SqlFecha & " AND Relacionada = 0 "

        If PSoloPendientes Then
            SqlB = "SELECT 1 as Operacion,* FROM FacturasCabeza WHERE Tr = 0 AND ClienteOperacion = 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & " "
            SqlN = "SELECT 2 as Operacion,* FROM FacturasCabeza WHERE Tr = 0 AND ClienteOperacion = 0 AND EsServicios = 0 AND EsSecos = 0 AND " & SqlFecha & " AND Relacionada = 0 "
        End If

        Dim SqlFactura As String = ""
        If Val(MaskedFactura.Text) <> 0 Then
            Dim Patron As String = "%" & Format(Val(MaskedFactura.Text), "000000000000")
            SqlFactura = "AND CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "'"
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String = ""
        If Cliente > 0 Then
            SqlCliente = "AND Cliente = " & Cliente & " "
        End If

        Dim SqlDeposito As String
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND FacturasCabeza.Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim SqlFacturasServicio As String = ""
        If CheckFacturasServicios.Checked = True Then SqlFacturasServicio = "AND FacturasCabeza.EsServicios = 1 "
        If CheckSinServicio.Checked = True Then
            If SqlFacturasServicio <> "" Then
                SqlFacturasServicio = "AND (FacturasCabeza.EsServicios = 1 OR FacturasCabeza.EsServicios = 0)"
            Else
                SqlFacturasServicio = "AND FacturasCabeza.EsServicios = 0 "
            End If
        End If

        Dim SqlNoAnuladaNotaCredito As String = ""  'arreglo para Juan. Solo muestra las no anuladas.
        If CheckAnuladaNotaCredito.Checked = True Then
            SqlNoAnuladaNotaCredito = "AND (FacturasCabeza.Importe - FacturasCabeza.ImporteDev) > 0.5"
        End If

        Dim SqlTipoFactura As String = ""
        If CheckFacturasConRemito.Checked Then SqlTipoFactura = "AND FacturasCabeza.Remito <> 0 "
        If CheckFacturasSinRemito.Checked Then
            If SqlTipoFactura = "" Then SqlTipoFactura = "AND FacturasCabeza.Remito = 0 AND EsServicios = 0 " Else SqlTipoFactura = ""
        End If
        If CheckTodasFacturas.Checked Then SqlTipoFactura = ""
        If CheckTickets.Checked Then SqlTipoFactura = "AND FacturasCabeza.EsZ = 1 "

        Dim SqlNoConfirmados As String = ""
        If CheckNoConfirmados.Checked Then
            SqlNoConfirmados = "AND Confirmado = 0 "
            SqlTipoFactura = "AND FacturasCabeza.Remito = 0 "
        End If

        Dim SqlContables As String = ""
        If CheckSinContables.Checked Then
            SqlContables = "AND Tr = 0 "
        End If
        If CheckSoloContables.Checked Then
            SqlContables = "AND Tr = 1 "
        End If

        SqlB = SqlB & SqlFactura & SqlCliente & SqlDeposito & SqlEstado & SqlTipoFactura & SqlNoConfirmados & SqlFacturasServicio & SqlNoAnuladaNotaCredito & SqlContables & ";"
        SqlN = SqlN & SqlFactura & SqlCliente & SqlDeposito & SqlEstado & SqlTipoFactura & SqlNoConfirmados & SqlFacturasServicio & SqlNoAnuladaNotaCredito & SqlContables & ";"
        ' ------------

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub ButtonModifica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonModificar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If IsNothing(DtGrid.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtFacturasB As New DataTable
        Dim DtFacturasN As New DataTable
        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        If Not IsNothing(DtGrid.GetChanges) Then
            For Each Row As DataRow In DtGrid.GetChanges.Rows
                Sql = "SELECT * FROM FacturasCabeza WHERE Factura = " & Row("Factura") & ";"
                If Row("Operacion") = 1 Then
                    If Not Tablas.Read(Sql, Conexion, DtFacturasB) Then Me.Close() : Exit Sub
                Else
                    If Not Tablas.Read(Sql, ConexionN, DtFacturasN) Then Me.Close() : Exit Sub
                End If
                If Row("FacturaN") <> 0 Then
                    Sql = "SELECT * FROM FacturasCabeza WHERE Factura = " & Row("FacturaN") & ";"
                    If Not Tablas.Read(Sql, ConexionN, DtFacturasN) Then Me.Close() : Exit Sub
                End If
            Next
        End If

        For Each Row As DataRow In DtFacturasB.Rows
            RowsBusqueda = DtGrid.GetChanges.Select("Factura = " & Row("Factura") & " AND Operacion = 1")
            Row("Confirmado") = RowsBusqueda(0).Item("Confirmado")
        Next
        For Each Row As DataRow In DtFacturasN.Rows
            RowsBusqueda = DtGrid.GetChanges.Select("FacturaN = " & Row("Factura"))
            If RowsBusqueda.Length = 0 Then
                RowsBusqueda = DtGrid.GetChanges.Select("Factura = " & Row("Factura") & " AND Operacion = 2")
            End If
            Row("Confirmado") = RowsBusqueda(0).Item("Confirmado")
        Next

        Dim Resul As Boolean = ActualizaLotes(DtFacturasB, DtFacturasN)

        DtFacturasB.Dispose()
        DtFacturasN.Dispose()

        If Resul Then
            DtGrid.AcceptChanges()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNotaDeCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNotaDeCredito.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Not (Grid.Columns(Col).Name = "Factura" Or Grid.Columns(Col).Name = "FacturaN") Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Nota de Credito No se puede Realizar en una Factura Anulada. Operación se CANCELA.")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Confirmado").Value Then
            MsgBox("Factura fue Confirmada por el Cliente. Operación se CANCELA.")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Tr").Value Then
            MsgBox("Factura No Habilitada para Devolucion. Operación se CANCELA.")
            Exit Sub
        End If

        GModificacionOk = False
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaNotaCredito.PFacturaB = Grid.CurrentRow.Cells("Factura").Value
            UnaNotaCredito.PFacturaN = Grid.CurrentRow.Cells("FacturaN").Value
        Else
            UnaNotaCredito.PFacturaB = 0
            UnaNotaCredito.PFacturaN = Grid.CurrentRow.Cells("Factura").Value
        End If
        UnaNotaCredito.PNota = 0
        UnaNotaCredito.ShowDialog()
        UnaNotaCredito.Dispose()

        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonCuerreFacturaExportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCuerreFacturaExportacion.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Modificación No se puede Realizar en una Factura Anulada.")
            Exit Sub
        End If

        If Not EsExterior(Grid.CurrentRow.Cells("Factura").Value) Then
            MsgBox("Factura No permitida para Modificación.")
            Exit Sub
        End If

        If HallaEstadoFactura(Grid.CurrentRow.Cells("Factura").Value, Conexion) <> 1 Then
            MsgBox("Factura Debe Afectar Stock.")
            Exit Sub
        End If

        If HallaCierreFacturasExterior(Grid.CurrentRow.Cells("Factura").Value) <> 0 Then
            MsgBox("Factura Ya Tiene Cierre.")
            Exit Sub
        End If

        UnCierreFactura.PFacturaB = Grid.CurrentRow.Cells("Factura").Value
        UnCierreFactura.PFacturaN = Grid.CurrentRow.Cells("FacturaN").Value
        UnCierreFactura.PNota = 0
        UnCierreFactura.ShowDialog()
        UnCierreFactura.Dispose()

        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonSucursal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSucursal.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        MsgBox(NombreSucursalCliente(Grid.CurrentRow.Cells("Cliente").Value, Grid.CurrentRow.Cells("Sucursal").Value))

    End Sub
    Private Sub ButtonCeder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCeder.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Grid.Columns(Col).Name = "FacturaN" Then
            MsgBox("Factura No puede ser Cedida. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Not Grid.Columns(Col).Name = "Factura" Then
            MsgBox("Debe Seleccionar una Factura. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Operacion").Value = 2 Then
            MsgBox("Factura No puede ser Cedida. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Factura Anulada. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        UnaCesionFactura.PFactura = Grid.CurrentRow.Cells("Factura").Value
        UnaCesionFactura.ShowDialog()
        UnaCesionFactura.Dispose()

    End Sub
    Private Sub ButtonAsignarLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsignarLotes.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Not (Grid.Columns(Col).Name = "Factura" Or Grid.Columns(Col).Name = "FacturaN") Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Factura Anulada. Operación se CANCELA.")
            Exit Sub
        End If

        If HallaCierreFacturasExterior(Grid.CurrentRow.Cells("Factura").Value) <> 0 Then
            MsgBox("Factura Ya Tiene Cierre. Operación se CANCELA.")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Tr").Value Or Grid.CurrentRow.Cells("EsServicios").Value Then
            MsgBox("Factura No Habilitada para Loteo. Operación se CANCELA.")
            Exit Sub
        End If

        Dim Factura As Decimal
        Dim Abierto As Boolean
        Dim ConexionStr As String
        If Grid.Columns(Col).Name = "Factura" Then
            Factura = Grid.CurrentRow.Cells("Factura").Value
            If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
                Abierto = True
                ConexionStr = Conexion
            Else
                Abierto = False
                ConexionStr = ConexionN
            End If
        End If
        If Grid.Columns(Col).Name = "FacturaN" Then
            Factura = Grid.CurrentRow.Cells("FacturaN").Value
            Abierto = False
            ConexionStr = ConexionN
        End If
        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Factura & ";", ConexionStr, Dt) Then Exit Sub

        Dim TipoAsiento As Integer
        Dim TipoAsientoCosto As Integer

        HallaTipoAsientoFactura(Dt.Rows(0).Item("EsExterior"), Dt.Rows(0).Item("EsServicios"), Dt.Rows(0).Item("EsSecos"), Dt.Rows(0).Item("Tr"), TipoAsiento, TipoAsientoCosto)

        UnaReAsignacionFactura.PFactura = Factura
        UnaReAsignacionFactura.PAbierto = Abierto
        UnaReAsignacionFactura.PtipoAsiento = TipoAsiento
        UnaReAsignacionFactura.PtipoAsientoCosto = TipoAsientoCosto
        UnaReAsignacionFactura.ShowDialog()
        UnaReAsignacionFactura.Dispose()

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

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

        GridAExcel(Grid, Date.Now, "Facturas Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub

    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        IncoTerm.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 38;")
        Dim Row As DataRow = IncoTerm.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        IncoTerm.DataSource.rows.add(Row)
        IncoTerm.DisplayMember = "Nombre"
        IncoTerm.ValueMember = "Clave"

        Estado.DataSource = DtAfectaPendienteAnulada()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

        CreaDtGrid()

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
        View.Sort = "Factura"

        Dim DtAux As New DataTable
        Dim Sql As String = ""
        Dim Total As Double = 0
        Dim FacturaN As Double = 0
        Dim ImporteN As Double = 0
        Dim Tr As Boolean
        Dim Cartel As String

        For Each Row As DataRowView In View
            Total = 0
            FacturaN = 0
            ImporteN = 0
            Tr = Row("Tr")
            Row("Importe") = Trunca(Row("Cambio") * Row("Importe")) + Row("Percepciones")
            If Not Row("EsZ") Then
                Cartel = "Factura"
            Else : Cartel = "Ticket"
            End If
            If Row("EsFCE") Then
                Cartel = "Fact.FCE"
            End If
            If Row("Operacion") = 1 And Row("Rel") And PermisoTotal Then
                Total = Row("Importe")
                Sql = "SELECT Factura,Importe,Percepciones,Estado,Cambio FROM FacturasCabeza WHERE Relacionada = " & Row("Factura") & ";"
                DtAux.Clear()
                If Not Tablas.Read(Sql, ConexionN, DtAux) Then Me.Close() : Exit Sub
                If DtAux.Rows.Count <> 0 Then
                    Dim Row2 As DataRow
                    Row2 = DtAux.Rows(0)
                    Row2("Importe") = Trunca(Row2("Cambio") * Row2("Importe")) + Row2("Percepciones")
                    FacturaN = Row2("Factura")
                    ImporteN = Row2("Importe")
                    Total = Total + Row2("Importe")
                End If
                AgregaADtGrid(Row("Operacion"), Cartel, Row("Factura"), Row("Interno"), Row("Remito"), Row("Recibo"), Row("Cliente"), Row("Fecha"), Row("Deposito"), Row("Importe"), Row("Estado"), _
                                                Row("IncoTerm"), FacturaN, ImporteN, Total, Row("EsServicios"), Row("Sucursal"), Row("Confirmado"), Tr, Row("EsZ"))
            Else
                AgregaADtGrid(Row("Operacion"), Cartel, Row("Factura"), Row("Interno"), Row("Remito"), Row("Recibo"), Row("Cliente"), Row("Fecha"), Row("Deposito"), Row("Importe"), Row("Estado"), _
                                                Row("IncoTerm"), 0, 0, 0, Row("EsServicios"), Row("Sucursal"), Row("Confirmado"), Tr, Row("EsZ"))
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid
        DtGrid.AcceptChanges()

        Dt.Dispose()
        DtAux.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim EsServicios As DataColumn = New DataColumn("EsServicios")
        EsServicios.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsServicios)

        Dim EsZ As DataColumn = New DataColumn("EsZ")
        EsZ.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(EsZ)

        Dim Cartel As DataColumn = New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Factura As DataColumn = New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim Sucursal As DataColumn = New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Sucursal)

        Dim NombreSucursal As DataColumn = New DataColumn("NombreSucursal")
        NombreSucursal.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreSucursal)

        Dim AbiertoRemito As DataColumn = New DataColumn("AbiertoRemito")
        AbiertoRemito.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AbiertoRemito)

        Dim TieneRemito As DataColumn = New DataColumn("TieneRemito")
        TieneRemito.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(TieneRemito)

        Dim Recibo As DataColumn = New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Recibo)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Deposito As DataColumn = New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Confirmado As New DataColumn("Confirmado")
        Confirmado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Confirmado)

        Dim Tr As New DataColumn("Tr")
        Tr.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Tr)

        Dim IncoTerm As New DataColumn("IncoTerm")
        IncoTerm.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(IncoTerm)

        Dim FacturaN As DataColumn = New DataColumn("FacturaN")
        FacturaN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(FacturaN)

        Dim ImporteN As DataColumn = New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

    End Sub
    Private Sub AgregaADtGrid(ByVal Operacion As Integer, ByVal Cartel As String, ByVal Factura As Double, ByVal Interno As Double, ByVal Remito As Double, ByVal Recibo As Integer, ByVal Cliente As Integer, ByVal Fecha As DateTime, ByVal Deposito As Integer, ByVal Importe As Double, _
                              ByVal Estado As Integer, ByVal IncoTerm As Integer, ByVal FacturaN As Double, ByVal ImporteN As Double, ByVal Total As Double, ByVal EsServicios As Boolean, ByVal Sucursal As Integer, ByVal Confirmado As Boolean, ByVal Tr As Boolean, ByVal EsZ As Boolean)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Operacion") = Operacion
        Row("EsServicios") = EsServicios
        Row("Cartel") = Cartel
        Row("Factura") = Factura
        If Remito <> 0 Then Row("TieneRemito") = True
        Row("Sucursal") = Sucursal
        Row("NombreSucursal") = ""
        If Row("Sucursal") <> 0 Then Row("NombreSucursal") = NombreSucursalCliente(Cliente, Sucursal)
        Row("Recibo") = Recibo
        Row("Cliente") = Cliente
        Row("Fecha") = Fecha
        Row("Deposito") = Deposito
        Row("Importe") = Importe
        Row("IncoTerm") = IncoTerm
        Row("Estado") = Estado
        Row("Confirmado") = Confirmado
        Row("Tr") = Tr
        Row("EsZ") = EsZ
        Row("FacturaN") = FacturaN
        Row("ImporteN") = ImporteN
        Row("Total") = Total
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function ActualizaLotes(ByVal DtFacturasB As DataTable, ByVal DtFacturasN As DataTable) As Boolean

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If DtFacturasB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtFacturasB, "FacturasCabeza", Conexion)
                    If Resul < 0 Then
                        MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                    If Resul = 0 Then
                        MsgBox("Error Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                End If

                If DtFacturasN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtFacturasN, "FacturasCabeza", ConexionN)
                    If Resul < 0 Then
                        MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                    If Resul = 0 Then
                        MsgBox("Error Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        Return False
                    End If
                End If

                Scope.Complete()
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Return True
            End Using
        Catch ex As TransactionException
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        Finally
        End Try

    End Function
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If CheckSinContables.Checked And CheckSoloContables.Checked Then
            MsgBox("Mal selección en Si-Contable y Solo-Contables.")
            CheckSinContables.Checked = False : CheckSoloContables.Checked = False
        End If

        If Val(MaskedFactura.Text) <> 0 And Not MaskedOK(MaskedFactura) Then
            MsgBox("Factura Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedFactura.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                        Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                    End If
                End If
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TieneRemito").Value) Then
                    Grid.Rows(e.RowIndex).Cells("ConRemito").Value = Grid.Rows(e.RowIndex).Cells("TieneRemito").Value
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Recibo" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Recibo").Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "0000-00000000")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Importe").Value) Then
                e.Value = FormatNumber(e.Value, 2, True, True, True)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            If Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = NumeroEditado(e.Value)
                Else : e.Value = ""
                End If
                If Grid.Rows(e.RowIndex).Cells("FacturaN").Value <> 0 Then
                    Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("ImporteN").Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2, True, True, True)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Total").Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2, True, True, True)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Estado" Then
            If Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 3 Then e.CellStyle.ForeColor = Color.Red
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

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            UnaFactura.PFactura = Grid.CurrentCell.Value
            UnaFactura.PCliente = Grid.CurrentRow.Cells("Cliente").Value
            UnaFactura.PAbierto = Abierto
            UnaFactura.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            If Grid.CurrentCell.Value = 0 Then Exit Sub
            UnaFactura.PFactura = Grid.CurrentCell.Value
            UnaFactura.PCliente = Grid.CurrentRow.Cells("Cliente").Value
            UnaFactura.PAbierto = False
            UnaFactura.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Grid.CurrentCell.Value = 0 Then Exit Sub
            UnRemito.PRemito = Grid.CurrentCell.Value
            UnRemito.PCliente = Grid.CurrentRow.Cells("Cliente").Value
            Dim Operacion As Integer = HallaOperacionRemito(Abierto, Grid.CurrentCell.Value, Grid.CurrentRow.Cells("Factura").Value, Grid.CurrentRow.Cells("FacturaN").Value)
            If Operacion = 1 Then UnRemito.PAbierto = True
            If Operacion = 2 Then UnRemito.PAbierto = False
            If Operacion = 0 Then
                MsgBox("Error Al Leer Remitos Cabeza.")
                Exit Sub
            End If
            UnRemito.ShowDialog()
        End If
    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick
        'funciona solo con valor de Grid.Columns("Confirmado").ReadOnly = True o Grid.ReadOnly = true

        If Grid.Columns(e.ColumnIndex).Name = "Confirmado" Then
            Dim chkCell As DataGridViewCheckBoxCell = Me.Grid.Rows(e.RowIndex).Cells("Confirmado")
            chkCell.Value = Not chkCell.Value
            If Grid.Rows(e.RowIndex).Cells("ConRemito").Value Then
                Me.Grid.Rows(e.RowIndex).Cells("Confirmado").Value = Not chkCell.Value
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub CheckFacturasServicios_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFacturasServicios.CheckedChanged

        If CheckFacturasServicios.Checked Then
            CheckFacturasConRemito.Checked = False
            CheckTickets.Checked = False
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckFacturasConRemito_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFacturasConRemito.CheckedChanged

        If CheckFacturasConRemito.Checked Then
            CheckTickets.Checked = False
            CheckTodasFacturas.Checked = False
            CheckFacturasServicios.Checked = False
        End If

    End Sub
    Private Sub CheckTickets_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckTickets.CheckedChanged

        If CheckTickets.Checked Then
            CheckFacturasConRemito.Checked = False
            CheckFacturasServicios.Checked = False
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckFacturasSinRemito_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFacturasSinRemito.CheckedChanged

        If CheckFacturasSinRemito.Checked Then
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckAnuladaNotaCredito_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckAnuladaNotaCredito.CheckedChanged

        If CheckAnuladaNotaCredito.Checked Then
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckSinContables_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckSinContables.CheckedChanged

        If CheckSinContables.Checked Then
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckSoloContables_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckSoloContables.CheckedChanged

        If CheckSoloContables.Checked Then
            CheckTodasFacturas.Checked = False
        End If

    End Sub
    Private Sub CheckTodasFacturas_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckTodasFacturas.CheckedChanged

        If Not CheckTodasFacturas.Checked Then Exit Sub

        Dim Check As CheckBox
        For Each ElementoPanel As Control In PanelChecks.Controls
            If ElementoPanel.Name = CheckTodasFacturas.Name Then Continue For
            Check = ElementoPanel
            Check.Checked = False
        Next

    End Sub
End Class