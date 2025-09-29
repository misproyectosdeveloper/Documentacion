Imports System.Transactions
Public Class UnaTablaCajas
    Private WithEvents bs As New BindingSource
    Public PBloqueaFunciones As Boolean
    '
    Dim DtCajaB As New DataTable
    Dim DtCajaN As New DataTable
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    ' 
    Dim Caja As Integer
    Private Sub UnaTablaCajas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        Caja = PideCaja()
        If Caja = 0 Then Me.Close() : Exit Sub

        Label1.Text = "Caja " & Caja

        ArmaMedioPago(DtFormasPago)

        CreaDtGrid()

        For Each Row As DataRow In DtFormasPago.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Operacion") = 1
            Row1("Mediopago") = Row("Clave")
            Row1("Importe") = 0
            DtGrid.Rows.Add(Row1)
        Next
        If PermisoTotal Then
            For Each Row As DataRow In DtFormasPago.Rows
                Dim Row1 As DataRow = DtGrid.NewRow
                Row1("Operacion") = 2
                Row1("Mediopago") = Row("Clave")
                Row1("Importe") = 0
                DtGrid.Rows.Add(Row1)
            Next
        End If

        LlenaCombosGrid()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub UnaTablaCajas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If DtCajaB.Rows.Count = 0 And DtCajaN.Rows.Count = 0 Then Me.Dispose() : Exit Sub

        Dim DtcajaBW As DataTable = DtCajaB.Copy
        Dim DtcajaNW As DataTable = DtCajaN.Copy

        ActualizaArchivos(DtcajaBW, DtcajaNW)

        If Not IsNothing(DtcajaBW.GetChanges) Or Not IsNothing(DtcajaNW.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                e.Cancel = True
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtcajaBW As DataTable = DtCajaB.Copy
        Dim DtcajaNW As DataTable = DtCajaN.Copy

        ActualizaArchivos(DtcajaBW, DtcajaNW)

        If IsNothing(DtcajaBW.GetChanges) And IsNothing(DtcajaNW.GetChanges) Then
            MsgBox("No Hay Cambios. Operacion. Operación se CANCELA. ")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaCaja(DtcajaBW, DtcajaNW)
        If Resul < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            If Not ArmaArchivos() Then Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String
        Dim RowsBusqueda() As DataRow

        DtCajaB = New DataTable
        DtCajaN = New DataTable
        Sql = "SELECT 1 AS Operacion,* FROM CajasSaldoInicial WHERE Caja = " & Caja & ";"
        If Not Tablas.Read(Sql, Conexion, DtCajaB) Then Return False
        If PermisoTotal Then
            Sql = "SELECT 2 AS Operacion,* FROM CajasSaldoInicial WHERE Caja = " & Caja & ";"
            If Not Tablas.Read(Sql, ConexionN, DtCajaN) Then Return False
        End If

        For Each Row As DataRow In DtCajaB.Rows
            RowsBusqueda = DtGrid.Select("MedioPago = " & Row("MedioPago") & " AND Operacion = 1")
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Importe") = Row("Importe")
            Else
                MsgBox("Error, MedioPago No Encontrado.", MsgBoxStyle.Critical)
                Return False
            End If
        Next
        '
        For Each Row As DataRow In DtCajaN.Rows
            RowsBusqueda = DtGrid.Select("MedioPago = " & Row("MedioPago") & " AND Operacion = 2")
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Importe") = Row("Importe")
            Else
                MsgBox("Error, MedioPago No Encontrado.", MsgBoxStyle.Critical)
                Return False
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Grid.EndEdit()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub ActualizaArchivos(ByVal DtCajaBW As DataTable, ByVal DtCajaNW As DataTable)

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid.Rows
            If Row("Operacion") = 1 Then
                RowsBusqueda = DtCajaBW.Select("MedioPago = " & Row("MedioPago"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Importe") <> Row("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
                Else
                    Dim Row1 As DataRow = DtCajaBW.NewRow
                    Row1("Caja") = Caja
                    Row1("Mediopago") = Row("Mediopago")
                    Row1("Importe") = Row("Importe")
                    DtCajaBW.Rows.Add(Row1)
                End If
            End If
        Next

        'Borro los que tienen importe 0.
        For I As Integer = DtCajaBW.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtCajaBW.Rows(I)
            If Row("Importe") = 0 Then Row.Delete()
        Next

        If PermisoTotal Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("Operacion") = 2 Then
                    RowsBusqueda = DtCajaNW.Select("MedioPago = " & Row("MedioPago"))
                    If RowsBusqueda.Length <> 0 Then
                        If RowsBusqueda(0).Item("Importe") <> Row("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
                    Else
                        Dim Row1 As DataRow = DtCajaNW.NewRow
                        Row1("Caja") = Caja
                        Row1("Mediopago") = Row("Mediopago")
                        Row1("Importe") = Row("Importe")
                        DtCajaNW.Rows.Add(Row1)
                    End If
                End If
            Next
        End If

        'Borro los que tienen importe 0.
        For I As Integer = DtCajaNW.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtCajaNW.Rows(I)
            If Row("Importe") = 0 Then Row.Delete()
        Next

    End Sub
    Public Sub ArmaMedioPago(ByRef Dt As DataTable)

        Dt = New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))
        Dt.Columns.Add("Tipo", Type.GetType("System.Int32"))
        '
        Dim Row As DataRow = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Row("Tipo") = 1
        Dt.Rows.Add(Row)
        '
        If Not Tablas.Read("SELECT 3 AS Tipo,Clave,Nombre FROM Tablas Where Tipo = 27;", Conexion, Dt) Then End

    End Sub
    Private Sub LlenaCombosGrid()

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

    End Sub
    Private Function ActualizaCaja(ByVal DtCajaAW As DataTable, ByVal DtCajaNW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCajaAW.GetChanges) Then
                    Resul = GrabaTabla(DtCajaAW.GetChanges, "CajasSaldoInicial", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtCajaNW.GetChanges) Then
                    Resul = GrabaTabla(DtCajaNW.GetChanges, "CajasSaldoInicial", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#.##")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Not Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Then
            If CType(sender, TextBox).Text <> "" And CType(sender, TextBox).Text <> "-" Then
                If CType(sender, TextBox).Text <> "" Then
                    EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Caja") = Caja
        Row("MedioPago") = 0
        Row("Importe") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub

    
    
End Class