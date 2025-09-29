Public Class ListaNotasFinancierasOtrosProveedores
    Private WithEvents bs As New BindingSource
    Dim Dta As DataTable
    Private Sub ListaNotasFinancierasOtrosProveedores_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        LabelClienteProveedor.Text = "Otros Proveedor"
        ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores ORDER BY Nombre;")
        Dim Row As DataRow = ComboEmisor.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.rows.add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboTipo.DataSource = ArchivoTipoNotas()
        ComboTipo.DisplayMember = "Nombre"
        ComboTipo.ValueMember = "Clave"

        ComboTipo.SelectedValue = 0
        With ComboTipo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        Entrada.Hide()

    End Sub
    Private Sub ListaNotasFinancierasOtrosProveedores_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

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

        PreparaArchivos()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub ComboTipo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboTipo.Validating

        If IsNothing(ComboTipo.SelectedValue) Then ComboTipo.SelectedValue = 0

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

        GridAExcel(Grid, Date.Now, "Notas Financieras Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PreparaArchivos()

        Dta = New DataTable

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        Dim SqlFecha As String
        SqlFecha = "Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "'"

        Dim SqlEstado As String = ""
        If ComboEstado.SelectedValue <> 0 Then
            SqlEstado = "AND Estado = " & ComboEstado.SelectedValue & " "
        End If

        Dim StrEmisor As String
        If ComboEmisor.SelectedValue <> 0 Then
            StrEmisor = " AND Proveedor = " & ComboEmisor.SelectedValue
        End If

        Dim StrTipo As String
        If ComboTipo.SelectedValue <> 0 Then
            StrEmisor = " AND TipoNota = " & ComboTipo.SelectedValue
        End If

        SqlB = "SELECT 1 AS Operacion,Proveedor,Estado, TipoNota,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Movimiento,Importe,Saldo,Caja,ChequeRechazado FROM OtrosPagosCabeza WHERE (TipoNota = 5005 OR TipoNota = 5007) AND " & SqlFecha & " "
        SqlN = "SELECT 2 AS Operacion,Proveedor,Estado, TipoNota,Saldo,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,Movimiento,Importe,Saldo,Caja,ChequeRechazado FROM OtrosPagosCabeza WHERE (TipoNota = 5005 OR TipoNota = 5007) AND " & SqlFecha & " "


        SqlB = SqlB & StrEmisor & SqlEstado & StrEmisor & StrTipo & ";"
        SqlN = SqlN & StrEmisor & SqlEstado & StrEmisor & StrTipo & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dta) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dta) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dta.DefaultView
        View.Sort = "Movimiento"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub LlenaCombosGrid()

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Emisor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM OtrosProveedores;")
        Emisor.DisplayMember = "Nombre"
        Emisor.ValueMember = "Clave"

        Tipo.DataSource = ArchivoTipoNotas()
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Clave"

    End Sub
    Private Function ArchivoTipoNotas() As DataTable

        Dim Dt As New DataTable

        Dim Clave As DataColumn = New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        Dt.Columns.Add(Clave)

        Dim Nombre As DataColumn = New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        Dt.Columns.Add(Nombre)

        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 5005
        Row("Nombre") = "Notas Debito(F)"
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 5007
        Row("Nombre") = "Notas Credito(F)"
        Dt.Rows.Add(Row)
        '
        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Dt.Rows.Add(Row)

        Return Dt

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If e.Value <> 0 Then
                If PermisoTotal Then
                    If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                    End If
                End If
            Else
                e.Value = ""
            End If
            If Grid.Rows(e.RowIndex).Cells("ChequeRechazado").Value <> 0 Then
                Grid.Rows(e.RowIndex).Cells("Mensaje").Value = "Rechazo Cheque"
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
        UnReciboDebitoCreditoOtrosProveedores.PAbierto = Abierto
        UnReciboDebitoCreditoOtrosProveedores.PTipoNota = Grid.CurrentRow.Cells("Tipo").Value
        UnReciboDebitoCreditoOtrosProveedores.PNota = Grid.CurrentRow.Cells("Comprobante").Value
        UnReciboDebitoCreditoOtrosProveedores.ShowDialog()
        If GModificacionOk Then PreparaArchivos()

    End Sub

End Class