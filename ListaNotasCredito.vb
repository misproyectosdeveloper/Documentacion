Public Class ListaNotasCredito
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim OpcionOrdenamiento As Integer
    Private WithEvents bs As New BindingSource
    Dim Dt As DataTable
    Dim DtGrid As DataTable
    Private Sub ListaNotasCredito_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PreparaEnlace(PPaseDeProyectos)
        End If
        '----------------------------------------------------------------------------------

        If Not PermisoTotal Then
            PanelCandados.Visible = False
            CheckCerrado.Checked = False
            Grid.Columns("Candado").Visible = False
        Else
            Grid.Columns("Candado").Visible = True
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("CandadoN").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        LlenaCombo(ComboCliente, "", "Clientes")
        With ComboCliente
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboCliente.SelectedValue = 0

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

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 0
        With ComboEstado
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        MaskedNota.Text = "000000000000"
        MaskedFactura.Text = "000000000000"

        If Not PermisoTotal Then
            Grid.Columns("CandadoN").HeaderText = ""
            Grid.Columns("NotaCreditoN").HeaderText = ""
            Grid.Columns("ImporteN").HeaderText = ""
            Grid.Columns("FacturaN").HeaderText = ""
            Grid.Columns("NoImputadoN").HeaderText = ""
            Grid.Columns("Total").HeaderText = ""
        End If

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNotasCredito_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNotasCredito_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        SqlFecha = "AND NotasCreditoCabeza.Fecha BETWEEN '" & FechaParaSql(DateTimeDesde.Value) & "' AND '" & FechaParaSql(DateTimeHasta.Value.AddDays(1)) & "' "
        Dim SqlFechaFactura As String
        If FechaFacturaDesde.Text <> "" Then
            SqlFechaFactura = " AND FacturasCabeza.FechaContable BETWEEN '" & FechaParaSql(CDate(FechaFacturaDesde.Text)) & "' AND '" & FechaParaSql(CDate(FechaFacturaHasta.Text).AddDays(1)) & "' "
            SqlFecha = ""
        End If
        '------------------------------------------------------------------------------------------------------
        '-------------Se elige ClienteOperacion <> 0 para no traer los realizados antes de activar Exportacion.
        '-------------y ClienteOperacion =0 para traer los realizados antes de activar Exportacion.
        '------------------------------------------------------------------------------------------------------
        Dim StrProceso As String = ""
        If Not IsNothing(PPaseDeProyectos) Then
            StrProceso = "FacturasCabeza.EsExterior = 1 AND FacturasCabeza.ClienteOperacion <> 0 "
        Else : StrProceso = "FacturasCabeza.ClienteOperacion = 0 "
        End If

        SqlB = "SELECT 1 as Operacion,NotasCreditoCabeza.*,FacturasCabeza.Cliente as Cliente,FacturasCabeza.FechaContable,0.0 As NoImputado,0.0 As NoImputadoN FROM NotasCreditoCabeza INNER JOIN FacturasCabeza ON NotasCreditoCabeza.Factura = FacturasCabeza.Factura WHERE " & StrProceso & SqlFecha & SqlFechaFactura & " "
        SqlN = "SELECT 2 as Operacion,NotasCreditoCabeza.*,FacturasCabeza.Cliente as Cliente,FacturasCabeza.FechaContable,0.0 As NoImputado,0.0 As NoImputadoN FROM NotasCreditoCabeza INNER JOIN FacturasCabeza ON NotasCreditoCabeza.Factura = FacturasCabeza.Factura WHERE " & StrProceso & SqlFecha & SqlFechaFactura & " AND NotasCreditoCabeza.Relacionada = 0 "

        Dim SqlFactura As String = ""
        If Val(MaskedFactura.Text) <> 0 Then
            Dim Patron As String = "%" & Format(Val(MaskedFactura.Text), "000000000000")
            SqlFactura = "AND CAST(CAST(NotasCreditoCabeza.Factura AS numeric) as char)LIKE '" & Patron & "'"
        End If

        Dim SqlNota As String = ""
        If Val(MaskedNota.Text) <> 0 Then
            Dim Patron As String = "%" & Format(Val(MaskedNota.Text), "000000000000")
            SqlNota = "AND CAST(CAST(NotasCreditoCabeza.NotaCredito AS numeric) as char)LIKE '" & Patron & "'"
        End If

        Dim Cliente As Integer = 0
        If ComboCliente.SelectedValue <> 0 Then Cliente = ComboCliente.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Cliente = ComboAlias.SelectedValue

        Dim SqlCliente As String = ""
        If Cliente > 0 Then
            SqlCliente = "AND Cliente = " & Cliente & " "
        End If

        Dim SqlEstado As String
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND NotasCreditoCabeza.Estado = " & ComboEstado.SelectedValue & " "
        End If

        SqlB = SqlB & SqlFactura & SqlCliente & SqlNota & SqlEstado
        SqlN = SqlN & SqlFactura & SqlCliente & SqlNota & SqlEstado

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        BorraGrid(Grid)
        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub PictureAlmanaqueFacturaDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFacturaDesde.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            FechaFacturaDesde.Text = ""
        Else : FechaFacturaDesde.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueFacturaHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFacturaHasta.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            FechaFacturaHasta.Text = ""
        Else : FechaFacturaHasta.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ComboCliente_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCliente.Validating

        If IsNothing(ComboCliente.SelectedValue) Then ComboCliente.SelectedValue = 0

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

        GridAExcel(Grid, Date.Now, "Notas de Créditos Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

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
        Dim Total As Double = 0
        Dim NotaCreditoN As Double = 0
        Dim FacturaN As Double = 0
        Dim ImporteN As Double = 0
        Dim NoImputadoN As Double = 0
        Dim Sql As String
        Dim DtAux As New DataTable
        Dim Row2 As DataRow

        Dim DTImputado As DataTable
        Dim DTImputadoN As DataTable

        View = Dt.DefaultView
        View.Sort = "NotaCredito"

        For Each Row As DataRowView In View
            Total = 0
            NotaCreditoN = 0
            FacturaN = 0
            ImporteN = 0
            NoImputadoN = 0

            Row("Importe") = Trunca(Row("Importe") * Row("Cambio")) + Row("Percepciones")

            DTImputado = New DataTable
            DTImputadoN = New DataTable

            If Row("Operacion") = 1 And Row("Rel") And PermisoTotal Then
                Total = Row("Importe")
                Sql = "SELECT NotaCredito,Factura,Importe,Percepciones,Cambio FROM NotasCreditoCabeza WHERE Relacionada = " & Row("NotaCredito") & ";"
                DtAux.Clear()
                If Not Tablas.Read(Sql, ConexionN, DtAux) Then Me.Close() : Exit Sub
                If DtAux.Rows.Count <> 0 Then
                    Row2 = DtAux.Rows(0)
                    Row2("Importe") = Trunca(Row2("Importe") * Row2("Cambio")) + Row2("Percepciones")
                    NotaCreditoN = Row2("NotaCredito")
                    FacturaN = Row2("Factura")
                    ImporteN = Row2("Importe")
                    Total = Total + Row2("Importe")
                    '
                    ' CALCULA LO QUE FALTA IMPUTAR.
                    If Not Tablas.Read("SELECT Nota, SUM(Importe) As Imputado FROM RecibosDetalle WHERE Nota = " & Row2("NotaCredito") & " GROUP BY Nota", ConexionN, DTImputadoN) Then Me.Close()
                    NoImputadoN = Row2("Importe")
                    If DTImputadoN.Rows.Count <> 0 Then NoImputadoN = Row2("Importe") - DTImputadoN.Rows(0).Item("Imputado")
                End If
            End If

            ' CALCULA LO QUE FALTA IMPUTAR.
            ' -----------------------------------------------------------------------------
            Row("NoImputado") = Row("Importe")

            If Row("Operacion") = 1 Then
                If Not Tablas.Read("SELECT Nota, SUM(Importe) As Imputado FROM RecibosDetalle WHERE Nota = " & Row("NotaCredito") & " GROUP BY Nota", Conexion, DTImputado) Then Me.Close()
            Else
                If Not Tablas.Read("SELECT Nota, SUM(Importe) As Imputado FROM RecibosDetalle WHERE Nota = " & Row("NotaCredito") & " GROUP BY Nota", ConexionN, DTImputado) Then Me.Close()
            End If

            If DTImputado.Rows.Count <> 0 Then Row("NoImputado") = Row("Importe") - DTImputado.Rows(0).Item("Imputado")
            ' -----------------------------------------------------------------------------

            Row2 = DtGrid.NewRow
            Row2("Operacion") = Row("Operacion")
            Row2("NotaCredito") = Row("NotaCredito")
            Row2("Mensaje") = ""
            If Row("EsZ") Then
                Row2("Mensaje") = "Ticket"
            End If
            If Row("EsFCE") Then
                Row2("Mensaje") = "Fac.FCE"
            End If
            Row2("Interno") = Val(Strings.Right(Row("Interno").ToString, 9))
            Row2("Factura") = Row("Factura")
            Row2("Cliente") = Row("Cliente")
            Row2("Fecha") = Row("Fecha")
            Row2("FechaContable") = Row("FechaContable")
            Row2("Importe") = Row("Importe")
            Row2("NoImputado") = Row("NoImputado")
            '
            Row2("NotaCreditoN") = NotaCreditoN
            Row2("FacturaN") = FacturaN
            Row2("ImporteN") = ImporteN
            Row2("NoImputadoN") = NoImputadoN
            Row2("Total") = Total
            Row2("Estado") = Row("Estado")
            DtGrid.Rows.Add(Row2)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()
        DtAux.Dispose()

    End Sub
    Private Sub GeneraCombosGrid()

        Estado.DataSource = DtAfectaPendienteAnulada()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes;")
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim NotaCreito As DataColumn = New DataColumn("NotaCredito")
        NotaCreito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NotaCreito)

        Dim Mensaje As DataColumn = New DataColumn("Mensaje")
        Mensaje.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Mensaje)

        Dim Interno As DataColumn = New DataColumn("Interno")
        Interno.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Interno)

        Dim Factura As DataColumn = New DataColumn("Factura")
        Factura.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Factura)

        Dim FechaFactura As DataColumn = New DataColumn("FechaContable")
        FechaFactura.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaFactura)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Importe As DataColumn = New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim NoImputado As DataColumn = New DataColumn("NoImputado")
        NoImputado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NoImputado)

        Dim NotaCreitoN As DataColumn = New DataColumn("NotaCreditoN")
        NotaCreitoN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NotaCreitoN)

        Dim FacturaN As DataColumn = New DataColumn("FacturaN")
        FacturaN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(FacturaN)

        Dim ImporteN As DataColumn = New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(ImporteN)

        Dim NoImputadoN As DataColumn = New DataColumn("NoImputadoN")
        NoImputadoN.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NoImputadoN)

        Dim Total As DataColumn = New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Total)

        Dim Estado As DataColumn = New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

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

        If FechaFacturaDesde.Text <> "" And FechaFacturaHasta.Text = "" Then
            MsgBox("Debe Informar Fecha Factura Hasta.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If FechaFacturaDesde.Text = "" And FechaFacturaHasta.Text <> "" Then
            MsgBox("Debe Informar Fecha Factura Desde.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If FechaFacturaDesde.Text <> "" Then
            If Not ConsisteFecha(FechaFacturaDesde.Text) Then
                MsgBox("Fecha Factura Desde Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                FechaFacturaDesde.Focus()
                Return False
            End If
            If Not ConsisteFecha(FechaFacturaHasta.Text) Then
                MsgBox("Fecha Factura Hasta Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                FechaFacturaHasta.Focus()
                Return False
            End If
            If DiferenciaDias(FechaFacturaDesde.Text, FechaFacturaHasta.Text) < 0 Then
                MsgBox("Fecha Factura Desde Mayor a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                FechaFacturaDesde.Focus()
                Return False
            End If
        End If

        If Val(MaskedNota.Text) <> 0 And Not MaskedOK(MaskedNota) Then
            MsgBox("Nota de Credito Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedNota.Focus()
            Return False
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

        If Grid.Columns(e.ColumnIndex).Name = "NotaCredito" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NotaCreditoN" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    Grid.Rows(e.RowIndex).Cells("CandadoN").Value = ImageList1.Images.Item("Cerrado")
                End If
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Factura" Then
            If Not IsNothing(e.Value) Then
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FacturaN" Then
            If e.Value <> 0 Then
                e.Value = NumeroEditado(e.Value)
            Else : e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaContableFactura" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Or Grid.Columns(e.ColumnIndex).Name = "Total" Or Grid.Columns(e.ColumnIndex).Name = "NoImputado" Or Grid.Columns(e.ColumnIndex).Name = "NoImputadoN" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(e.Value, "#")
            End If
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
            If IsNothing(Grid.CurrentCell.Value) Then Exit Sub
            UnaFactura.PFactura = Grid.CurrentCell.Value
            UnaFactura.PAbierto = Abierto
            UnaFactura.ShowDialog()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NotaCredito" Then
            If IsNothing(Grid.CurrentCell.Value) Then Exit Sub
            UnaNotaCredito.PNota = Grid.Rows(e.RowIndex).Cells("NotaCredito").Value
            UnaNotaCredito.PAbierto = Abierto
            UnaNotaCredito.ShowDialog()
            UnaNotaCredito.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "NotaCreditoN" Then
            If IsNothing(Grid.CurrentCell.Value) Then Exit Sub
            UnaNotaCredito.PNota = Grid.Rows(e.RowIndex).Cells("NotaCreditoN").Value
            UnaNotaCredito.PAbierto = False
            UnaNotaCredito.ShowDialog()
            UnaNotaCredito.Dispose()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cliente" Then
            If IsNothing(Grid.CurrentCell.Value) Then Exit Sub
            UnCliente.PCliente = Grid.CurrentCell.Value
            UnCliente.ShowDialog()
            UnCliente.Dispose()
        End If

    End Sub
   
End Class