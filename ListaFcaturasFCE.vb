Imports System.Transactions
Public Class ListaFacturasFCE
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

        MaskedFactura.Text = "000000000000"

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

        SqlB = "SELECT 1 as Operacion,* FROM FacturasCabeza WHERE EsFCE = 1 AND " & SqlFecha & " "

        Dim SqlFactura As String = ""
        If Val(MaskedFactura.Text) <> 0 Then
            Dim Patron As String = "%" & Format(Val(MaskedFactura.Text), "000000000000")
            SqlFactura = "AND CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "'"
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String
        If Cliente > 0 Then
            SqlCliente = "AND Cliente = " & Cliente & " "
        End If

        Dim SqlDeposito As String
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFactura & SqlCliente & SqlDeposito & ";"

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

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

        GridAExcel(Grid, Date.Now, "Facturas FCE Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

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

        Dt = New DataTable

        CreaDtGrid()

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Factura"

        Dim DtAux As New DataTable
        Dim Sql As String = ""
        Dim Total As Double = 0
        Dim Cartel As String

        For Each Row As DataRowView In View
            Total = 0
            UnSaldoFCE.PFactura = Row("Factura")
            UnSaldoFCE.PMuestraFormulario = False
            UnSaldoFCE.ShowDialog()
            Total = UnSaldoFCE.PSaldo
            UnSaldoFCE.Dispose()
            Row("Importe") = Trunca(Row("Cambio") * Row("Importe")) + Row("Percepciones")
            Cartel = "Fact.FCE"
            AgregaADtGrid(Cartel, Row("Factura"), Row("Interno"), Row("Cliente"), Row("Fecha"), Row("Deposito"), Row("Importe"), Total)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid
        DtGrid.AcceptChanges()

        Dt.Dispose()
        DtAux.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Cartel As DataColumn = New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

        Dim Factura As DataColumn = New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim Interno As DataColumn = New DataColumn("Interno")
        Interno.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Interno)

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

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

    End Sub
    Private Sub AgregaADtGrid(ByVal Cartel As String, ByVal Factura As Double, ByVal Interno As Double, ByVal Cliente As Integer, ByVal Fecha As DateTime, ByVal Deposito As Integer, ByVal Importe As Double, _
                              ByVal Total As Double)

        Dim Row As DataRow

        Row = DtGrid.NewRow()
        Row("Cartel") = Cartel
        Row("Factura") = Factura
        Row("Interno") = Val(Strings.Right(Interno.ToString, 9))
        Row("Cliente") = Cliente
        Row("Fecha") = Fecha
        Row("Deposito") = Deposito
        Row("Importe") = Importe
        Row("Total") = Total
        DtGrid.Rows.Add(Row)

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If ComboCliente.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Cliente o Alias.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
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

        If Grid.Columns(e.ColumnIndex).Name = "Total" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Total").Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2, True, True, True)
                Else : e.Value = FormatNumber(e.Value, 2, True, True, True)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            UnSaldoFCE.PFactura = Grid.CurrentRow.Cells("Factura").Value
            UnSaldoFCE.PMuestraFormulario = True
            UnSaldoFCE.ShowDialog()
            UnSaldoFCE.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub


   
End Class