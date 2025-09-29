Public Class ListaPasesDeCaja
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    Dim DtFormasPago As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaPasesDeCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ArmaMedioPagoPase(DtFormasPago, True)

        ComboCaja.DataSource = ArmaDtCajas()
        Dim Row As DataRow = ComboCaja.DataSource.newrow
        row("Nombre") = "999"
        row("Clave") = 999
        ComboCaja.DataSource.Rows.Add(Row)
        ComboCaja.DisplayMember = "Nombre"
        ComboCaja.ValueMember = "Clave"
        ComboCaja.SelectedValue = GCaja
        With ComboCaja
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        If Not GCajaTotal Then
            ComboCaja.Enabled = False
        End If

        DateTimeDesde.Value = DateTimeHasta.Value.AddMonths(-1)

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaNotasTerceros_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ComboCaja_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCaja.Validating

        If IsNothing(ComboCaja.SelectedValue) Then ComboCaja.SelectedValue = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Desde debe ser menor o igual a Fecha Hasta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim SqlFecha As String
        SqlFecha = "C.Fecha BETWEEN '" & Format(DateTimeDesde.Value, "yyyyMMdd 00:00:00") & "' AND '" & Format(DateTimeHasta.Value, "yyyyMMdd 23:59:59") & "' "

        SqlB = "SELECT 1 as Operacion,C.Pase AS Pase,C.CajaOrigen AS CajaOrigen,C.CajaDestino AS CajaDestino,C.Fecha AS Fecha,C.Aceptado AS Aceptado,D.MedioPago AS MedioPago,D.ClaveCheque As ClaveCheque,D.Importe AS Importe FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE " & SqlFecha & " AND (C.CajaOrigen = " & ComboCaja.SelectedValue & " OR C.CajaDestino = " & ComboCaja.SelectedValue & ") "
        SqlN = "SELECT 2 as Operacion,C.Pase AS Pase,C.CajaOrigen AS CajaOrigen,C.CajaDestino AS CajaDestino,C.Fecha AS Fecha,C.Aceptado AS Aceptado,D.MedioPago AS MedioPago,D.ClaveCheque As ClaveCheque,D.Importe AS Importe FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE " & SqlFecha & " AND (C.CajaOrigen = " & ComboCaja.SelectedValue & " OR C.CajaDestino = " & ComboCaja.SelectedValue & ") "

        '       SqlB = SqlB & SqlFactura & SqlCliente & SqlNota
        '       SqlN = SqlN & SqlFactura & SqlCliente & SqlNota

        CreaDtGrid()

        LLenaGrid()

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
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
    Private Sub LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        Dim Debito As Double = 0
        Dim Credito As Double = 0

        Dim DtCheques As New DataTable
        For Each Row As DataRow In Dt.Rows
            If Row("ClaveCheque") <> 0 Then
                If Row("Operacion") = 1 Then
                    If Not Tablas.Read("SELECT 1 As Operacion,* FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", Conexion, DtCheques) Then Me.Close() : Exit Sub
                Else
                    If Not Tablas.Read("SELECT 2 As Operacion,* FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionN, DtCheques) Then Me.Close() : Exit Sub
                End If
            End If
        Next

        View = Dt.DefaultView
        View.Sort = "Fecha"

        Dim rowsBusqueda() As DataRow
        Dim PaseAnt As Integer = 0
        Dim Color As Boolean

        For Each Row As DataRowView In View
            If PaseAnt <> Row("Pase") Then Color = Not Color : PaseAnt = Row("Pase")
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Color") = Color
            Row1("Pase") = Row("Pase")
            Row1("MedioPago") = Row("MedioPago")
            Row1("CajaOrigen") = Row("CajaOrigen")
            Row1("CajaDestino") = Row("CajaDestino")
            Row1("Aceptado") = Row("Aceptado")
            Row1("Fecha") = Row("Fecha")
            Row1("Operacion") = Row("Operacion")
            If Row("CajaOrigen") = ComboCaja.SelectedValue Then
                Row1("Credito") = Row("Importe")
            Else : Row1("Debito") = Row("Importe")
            End If
            If Row("ClaveCheque") <> 0 Then
                rowsBusqueda = DtCheques.Select("ClaveCheque = " & Row("ClaveCheque") & " AND Operacion = " & Row("Operacion"))
                If rowsBusqueda.Length <> 0 Then
                    Row1("Banco") = rowsBusqueda(0).Item("Banco")
                    Row1("Serie") = rowsBusqueda(0).Item("Serie")
                    Row1("Numero") = rowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = rowsBusqueda(0).Item("EmisorCheque")
                    Row1("Vencimiento") = rowsBusqueda(0).Item("Fecha")
                Else
                    Row1("Banco") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Vencimiento") = "1/1/1800"
                End If
            Else
                Row1("Banco") = 0
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Vencimiento") = "1/1/1800"
            End If
            DtGrid.Rows.Add(Row1)
        Next

        DtCheques.Dispose()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Pase As New DataColumn("Pase")
        Pase.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Pase)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Debito As New DataColumn("Debito")
        Debito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debito)

        Dim Credito As New DataColumn("Credito")
        Credito.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Credito)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim CajaOrigen As New DataColumn("CajaOrigen")
        CajaOrigen.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CajaOrigen)

        Dim CajaDestino As New DataColumn("CajaDestino")
        CajaDestino.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(CajaDestino)

        Dim Aceptado As New DataColumn("Aceptado")
        Aceptado.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Aceptado)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim Vencimiento As New DataColumn("Vencimiento")
        Vencimiento.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Vencimiento)

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Color)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 ORDER BY Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Pase" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If Grid.Rows(e.RowIndex).Cells("Color").Value Then
                    Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.AntiqueWhite
                Else : Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.WhiteSmoke
                End If
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debito" Or Grid.Columns(e.ColumnIndex).Name = "Credito" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = Format(0, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 0)
                Else : e.Value = Format(0, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CajaOrigen" Or Grid.Columns(e.ColumnIndex).Name = "CajaDestino" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then e.Value = Format(e.Value, "0000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "Vencimiento" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
                If e.Value = "1/1/1800" Then e.Value = ""
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

        UnPaseCaja.PPase = Grid.Rows(e.RowIndex).Cells("Pase").Value
        UnPaseCaja.PBloqueaFunciones = True
        UnPaseCaja.PAbierto = Abierto
        UnPaseCaja.ShowDialog()
        UnPaseCaja.Dispose()

    End Sub

    
End Class