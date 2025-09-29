Public Class ListaRemitos
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaRemitos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        GeneraCombosGrid()

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")
        ComboCliente.SelectedValue = 0
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Clientes WHERE Alias <> '';")
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

        LlenaCombo(ComboPorCuentaYOrden, "", "Clientes")
        ComboPorCuentaYOrden.SelectedValue = 0
        With ComboPorCuentaYOrden
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

        MaskedRemito.Text = "000000000000"

        CreaDtGrid()

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            ButtonReemplazar.Visible = False
            Grid.Columns("RemitoReemplazado").Visible = False
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRemitos_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub ListaRemitos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Valida() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        SqlB = "SELECT 1 as Operacion,* FROM RemitosCabeza "
        SqlN = "SELECT 2 as Operacion,* FROM RemitosCabeza "

        Dim SqlRemito As String = ""
        If Val(MaskedRemito.Text) <> 0 Then
            SqlRemito = "WHERE Remito = " & Val(MaskedRemito.Text) & " "
        Else : SqlRemito = "WHERE Remito LIKE '%' "
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String = ""
        If Cliente > 0 Then
            SqlCliente = "AND Cliente = " & Cliente & " "
        End If

        Dim SqlPorCuentaYOrden As String = ""
        If ComboPorCuentaYOrden.SelectedValue > 0 Then
            SqlPorCuentaYOrden = "AND PorCuentaYOrden = " & ComboPorCuentaYOrden.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            If ComboEstado.SelectedValue = 2 Then
                SqlEstado = "AND Estado = 2 AND Factura = 0 "
            Else
                SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
            End If
        End If

        Dim SqlNoConfirmados As String = ""
        If CheckNoConfirmados.Checked Then
            SqlNoConfirmados = "AND Confirmado = 0 "
            SqlEstado = ""
        End If

        Dim SqlSucursal As String = ""
        If ComboSucursales.SelectedValue <> 0 Then
            SqlSucursal = "AND Sucursal = " & ComboSucursales.SelectedValue
        End If

        SqlB = SqlB & SqlRemito & SqlCliente & SqlDeposito & SqlFecha & SqlEstado & SqlNoConfirmados & SqlPorCuentaYOrden & SqlSucursal & ";"
        SqlN = SqlN & SqlRemito & SqlCliente & SqlDeposito & SqlFecha & SqlEstado & SqlNoConfirmados & SqlPorCuentaYOrden & SqlSucursal & ";"

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position
        Grid.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonDevolucion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolucion.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Remito Esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Factura").Value <> 0 Then
            MsgBox("Opción Invalida para Remitos Facturados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Cartel").Value <> "" Then
            MsgBox("Remito Ya esta Facturado o Tiene N.V.L.P. Solo se puede devolver por Nota de Credito. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Confirmado").Value Then
            MsgBox("Remito Ya esta Confirmado por el Cliente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        UnaDevolucion.PRemito = Grid.CurrentRow.Cells("Remito").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaDevolucion.PAbierto = True
        Else : UnaDevolucion.PAbierto = False
        End If
        GModificacionOk = False
        UnaDevolucion.ShowDialog()
        UnaDevolucion.Dispose()

    End Sub
    Private Sub ButtonSucursal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSucursal.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        MsgBox(NombreSucursalCliente(Grid.CurrentRow.Cells("Cliente").Value, Grid.CurrentRow.Cells("Sucursal").Value))

    End Sub
    Private Sub ButtonVerDetalle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerDetalle.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Remito Esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Estado").Value <> 1 Then
            MsgBox("Remito No Tiene Lotes Asignados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If
        If Grid.CurrentRow.Cells("Factura").Value <> 0 Then
            MsgBox("Opción Invalida para Remitos Facturados. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        SeleccionarVarios.PEsDetalleRemito = True
        SeleccionarVarios.PRemito = Grid.CurrentRow.Cells("Remito").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            SeleccionarVarios.PAbierto = True
        Else : SeleccionarVarios.PAbierto = False
        End If
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ButtonActualizarPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActualizarPedido.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Remito Esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        UnaActualizacionPedido.PRemito = Grid.CurrentRow.Cells("Remito").Value
        UnaActualizacionPedido.POperacion = Grid.CurrentRow.Cells("Operacion").Value
        UnaActualizacionPedido.ShowDialog()
        If GModificacionOk Then LLenaGrid()
        UnaActualizacionPedido.Dispose()

    End Sub
    Private Sub ButtonReemplazar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReemplazar.Click

        If Grid.Rows.Count = 0 Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Remito NO DEBE estar ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Operacion").Value = 2 Then
            MsgBox("Remito NO HABILITADO para esta Función. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Confirmado").Value Then
            MsgBox("Remito Esta Confirmado Por el Cliente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        UnRemitoReemplazo.PRemito = Grid.CurrentRow.Cells("Remito").Value
        UnRemitoReemplazo.PAbierto = True
        UnRemitoReemplazo.PEsReemplazo = True
        UnRemitoReemplazo.ShowDialog()
        UnRemitoReemplazo.Dispose()
        If GModificacionOk Then LLenaGrid()

    End Sub
    Private Sub ButtonActivaAnulado_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActivaAnulado.Click

        If Grid.CurrentRow.Cells("Estado").Value <> 3 Then
            MsgBox("Remito DEBE estar ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If Not GAdministrador Then
            If Not "putasuerte" = InputBox("Clave Autotizada ") Then Exit Sub
        End If

        Dim RemitoReemplazanteActivo As Decimal = HallaRemitoReemplazanteActivo(Grid.CurrentRow.Cells("Remito").Value, ConexionN)

        If RemitoReemplazanteActivo <> 0 Then
            MsgBox("Este Remito fue Reemplazado por el Remito " & NumeroEditado(RemitoReemplazanteActivo) & " No ANULADO" + vbCrLf + "Debe Anularlo para continuar. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3, "")
            Exit Sub
        End If

        UnRemitoReemplazo.PRemito = Grid.CurrentRow.Cells("Remito").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnRemitoReemplazo.PAbierto = True
        Else : UnRemitoReemplazo.PAbierto = False
        End If
        UnRemitoReemplazo.PEsRecupera = True
        UnRemitoReemplazo.ShowDialog()
        UnRemitoReemplazo.Dispose()
        If GModificacionOk Then LLenaGrid()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Remitos", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsignarLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsignarLotes.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        If Not (Grid.Columns(Col).Name = "Remito") Then Exit Sub

        If Grid.CurrentRow.Cells("Estado").Value = 3 Then
            MsgBox("Remito Anulado. Operación se CANCELA.")
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Factura").Value <> 0 Then
            MsgBox("Remito Ya esta Facturado. Asignación debe realizarce en la Factura. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim Remito As Decimal
        Dim Ingreso As Decimal
        Dim Abierto As Boolean
        Dim ConexionStr As String
        Remito = Grid.CurrentRow.Cells("Remito").Value
        Ingreso = Grid.CurrentRow.Cells("Ingreso").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
            ConexionStr = Conexion
        Else
            Abierto = False
            ConexionStr = ConexionN
        End If

        UnaReAsignacionRemito.PIngreso = Ingreso
        UnaReAsignacionRemito.PRemito = Remito
        UnaReAsignacionRemito.PAbierto = Abierto
        UnaReAsignacionRemito.ShowDialog()
        UnaReAsignacionRemito.Dispose()

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Confirmado" Then Exit Sub
        Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        If Not RegrabaConfirmacion(Grid.Rows(e.RowIndex).Cells("Remito").Value, Grid.Rows(e.RowIndex).Cells("Confirmado").Value, Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
            MsgBox("Error al Grabar Confirmación.")
            Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = Not Grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value
        End If
        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

        VerSucursales(ComboCliente.SelectedValue)

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

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
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

        Estado.DataSource = DtAfectaPendienteAnulada()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        If Not CheckFacturados.Checked And Not CheckNoFacturados.Checked Then
            CheckFacturados.Checked = True
            CheckNoFacturados.Checked = True
        End If
        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked And PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Remito"

        Dim Cantidad As Decimal
        Dim Liquidado As Decimal
        Dim Factura As Double
        Dim Merma As Decimal
        Dim LiquidadoTotalmente As Boolean

        For I As Integer = 0 To View.Count - 1
            Cantidad = 0
            Liquidado = 0
            Merma = 0
            Factura = 0
            LiquidadoTotalmente = False
            If Not HallaCantidad(View(I), Cantidad) Then Me.Close() : Exit Sub
            If View(I).Item("Factura") = 0 And View(I).Item("Estado") = 1 Then
                If Not HallaLiquidado(View(I).Item("Remito"), View(I).Item("Operacion"), Liquidado, Merma, LiquidadoTotalmente) Then Me.Close() : Exit Sub
            End If
            Detalle(View(I), View(I).Item("Cliente"), View(I).Item("Sucursal"), Cantidad, Liquidado, Merma, View(I).Item("Factura"), View(I).Item("Operacion"), LiquidadoTotalmente)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

        Dt.Dispose()

    End Sub
    Private Sub Detalle(ByVal Row As DataRowView, ByVal Cliente As Integer, ByVal Sucursal As Integer, ByVal Cantidad As Decimal, ByVal Liquidado As Decimal, ByVal Merma As Decimal, ByVal Factura As Double, ByVal OperacionFactura As Integer, ByVal LiquidadoTotalmente As Boolean)

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("Operacion") = Row("Operacion")
        Row1("Remito") = Row("Remito")
        Row1("Sucursal") = Row("Sucursal")
        Row1("NombreSucursal") = ""
        If Row1("Sucursal") <> 0 Then Row1("NombreSucursal") = NombreSucursalCliente(Cliente, Sucursal)
        Row1("Cliente") = Row("Cliente")
        Row1("Fecha") = Format(Row("Fecha"), "dd/MM/yyy 00:00:00")
        Row1("Deposito") = Row("Deposito")
        Row1("OperacionFactura") = OperacionFactura
        Row1("Factura") = Factura
        Row1("Ingreso") = Row("Ingreso")
        Row1("Cantidad") = Cantidad
        Row1("RemitoReemplazado") = Row("RemitoReemplazado")
        Row1("Estado") = Row("Estado")
        Row1("Confirmado") = Row("Confirmado")
        Row1("ConfirmadoInvisible") = Row("Confirmado")
        Row1("Cartel") = ""
        If Factura <> 0 Then Row1("Cartel") = "Facturado"
        If LiquidadoTotalmente Then Row1("Cartel") = "Liquidado"
        If (CheckFacturados.Checked And Row1("Cartel") <> "") Or (CheckNoFacturados.Checked And Row1("Cartel") = "") Or _
                (CheckFacturados.Checked And CheckNoFacturados.Checked) Then
            DtGrid.Rows.Add(Row1)
        End If

    End Sub
    Private Function HallaRemitoReemplazanteActivo(ByVal Remito As Decimal, ByVal ConexionStr As String) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Remito FROM RemitosCabeza WHERE Estado <> 3 AND RemitoReemplazado = " & Remito & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al leer Tabla: RemitosCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Emisor & ";"
        ComboSucursales.DataSource = New DataTable
        ComboSucursales.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboSucursales.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboSucursales.DataSource.rows.add(Row)
        ComboSucursales.DisplayMember = "Nombre"
        ComboSucursales.ValueMember = "Clave"
        ComboSucursales.SelectedValue = 0
        With ComboSucursales
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Function HallaCantidad(ByVal ViewW As DataRowView, ByRef Cantidad As Decimal) As Boolean

        Dim Sql As String
        Dim ConexionStr As String
        Dim Dt As New DataTable

        If ViewW.Item("Operacion") = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Sql = "SELECT D.Cantidad,D.Devueltas FROM RemitosCabeza AS C INNER JOIN RemitosDetalle As D ON C.Remito = D.Remito WHERE C.Remito = " & ViewW.Item("Remito") & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad") - Row("Devueltas")
        Next

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaFactura(ByVal Remito As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaRemitoReemplazante(ByVal Remito As Decimal, ByVal ConexionStr As String) As Decimal

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Remito FROM RemitosCabeza WHERE RemitoReemplazado = " & Remito & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al Leer Tabla: RemitosCabeza", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function RegrabaConfirmacion(ByVal Remito As Decimal, ByVal Confirmado As Boolean, ByVal Operacion As String) As Boolean

        Dim Dt As DataTable
        Dim ConexionStr As String
        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RemitosCabeza WHERE Remito = " & Remito & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    Dt = New DataTable
                    DaP.Fill(Dt)
                    Dt.Rows(0).Item("Confirmado") = Confirmado
                    DaP.Update(Dt)
                End Using
                Dt.Dispose()
                Return True
            End Using
        Catch ex As OleDb.OleDbException
            Return False
        Catch ex As DBConcurrencyException
            Return False
        Finally
        End Try

    End Function
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remito)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Sucursal As New DataColumn("Sucursal")
        Sucursal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Sucursal)

        Dim NombreSucursal As DataColumn = New DataColumn("NombreSucursal")
        NombreSucursal.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(NombreSucursal)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim OperacionFactura As New DataColumn("OperacionFactura")
        OperacionFactura.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(OperacionFactura)

        Dim Factura As New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Ingreso)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim RemitoReemplazado As New DataColumn("RemitoReemplazado")
        RemitoReemplazado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(RemitoReemplazado)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Confirmado As New DataColumn("Confirmado")
        Confirmado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Confirmado)

        Dim ConfirmadoInvisible As New DataColumn("ConfirmadoInvisible")
        ConfirmadoInvisible.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(ConfirmadoInvisible)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

    End Sub
    Private Function HallaLiquidado(ByVal Remito As Double, ByVal Operacion As Integer, ByRef Liquidado As Decimal, ByRef Merma As Decimal, ByRef LiquidadoTotalmente As Boolean) As Boolean

        Dim ConexionStr As String
        Dim Sql As String
        Dim Dt As New DataTable

        Liquidado = 0
        Merma = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Sql = "SELECT Lote,Secuencia,Deposito,Cantidad,Liquidado,Operacion AS OperacionLote FROM AsignacionLotes WHERE Cantidad <> 0 AND TipoComprobante = 1 AND Comprobante = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Liquidado") Then
                Dim Liquidacion As Decimal
                Dim ReciboOficial As Decimal
                Dim Vendido As Decimal
                If Not HallaNVLP(Row("OperacionLote"), Row.Item("Lote"), Row.Item("Secuencia"), Row.Item("Deposito"), Operacion, Remito, Liquidacion, ReciboOficial, Vendido) Then Return False
                Liquidado = Liquidado + Vendido
                Merma = Merma + Row.Item("Cantidad") - Vendido
            End If
        Next

        LiquidadoTotalmente = True
        If Dt.Rows.Count = 0 Then
            LiquidadoTotalmente = False
        Else
            For Each Row As DataRow In Dt.Rows
                If Not Row("Liquidado") Then LiquidadoTotalmente = False : Exit For
            Next
        End If

        Dt.Dispose()
        Return True

    End Function
    Private Function Valida() As Boolean

        If Val(MaskedRemito.Text) <> 0 And Not MaskedOK(MaskedRemito) Then
            MsgBox("Remito Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = NumeroEditado(e.Value)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Facturado" Or Grid.Columns(e.ColumnIndex).Name = "NoFacturado" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Ingreso" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "RemitoReemplazado" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = NumeroEditado(e.Value)
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

        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            UnRemito.PRemito = Grid.Rows(e.RowIndex).Cells("Remito").Value
            UnRemito.PCliente = Grid.Rows(e.RowIndex).Cells("Cliente").Value
            UnRemito.PDeposito = Grid.Rows(e.RowIndex).Cells("Deposito").Value
            UnRemito.PAbierto = Abierto
            UnRemito.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Grid.Rows(e.RowIndex).Cells("Factura").Value = 0 Then Exit Sub
            If Grid.Rows(e.RowIndex).Cells("OperacionFactura").Value = 1 Then
                Abierto = True
            Else : Abierto = False
            End If
            UnaFactura.PFactura = Grid.Rows(e.RowIndex).Cells("Factura").Value
            UnaFactura.PAbierto = Abierto
            UnaFactura.PBloqueaFunciones = True
            UnaFactura.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
   
End Class