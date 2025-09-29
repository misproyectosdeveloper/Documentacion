Public Class UnaTablaCierreContable
    Public PBloqueaFunciones As Boolean
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim cb As ComboBox
    Dim Opcion1LibrosIva As Boolean
    Private Sub UnaTablaCierreContable_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 60

        Grid.AutoGenerateColumns = False

        TextDesde.Text = Date.Now.Year - 1
        TextHasta.Text = Date.Now.Year

        GExcepcion = HallaDatoGenerico("SELECT Opcion1LibrosIva FROM DatosEmpresa WHERE Indice = 1;", Conexion, Opcion1LibrosIva)
        If Not IsNothing(GExcepcion) Then
            MsgBox("Error al Leer Tabla: DatosEmpresa." + vbCrLf + vbCrLf + GExcepcion.Message)
            Me.Close() : Exit Sub
        End If

        Dim UltimoAnioW As Integer = UltimoAnio(Conexion)
        If UltimoAnioW = -1 Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        If UltimoAnioW > Date.Now.Year Then
            TextHasta.Text = UltimoAnioW
        End If

        LlenaCombosGrid()

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonSeleccionar_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaTablaSucursal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then Exit Sub

        If Dt.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row As DataRow In Dt.GetChanges.Rows
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Anio = " & Row("Anio") & " AND Mes = " & Row("Mes"))
            RowsBusqueda(0).Item("Opcion") = Opcion1LibrosIva
        Next

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM CierreContable;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(Dt.GetChanges)
                    End Using
                    Trans.Commit()
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        Me.Cursor = System.Windows.Forms.Cursors.Default

        LLenaGrid()

    End Sub
    Private Sub ButtonSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeleccionar.Click

        LLenaGrid()

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        If HallaBalanceCerrado(Row("Mes"), Row("Anio")) Then
            MsgBox("Periodo Corresponde a un Balance Cerrado.")
            Exit Sub
        End If

        bs.RemoveCurrent()

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
    Private Sub TextDesde_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDesde.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub LLenaGrid()

        If TextDesde.Text = "" Then
            MsgBox("Incorrecto Añio Desde", MsgBoxStyle.Information)
            Exit Sub
        End If
        If TextHasta.Text = "" Then
            MsgBox("Incorrecto Añio Hasta", MsgBoxStyle.Information)
            Exit Sub
        End If
        If CInt(TextDesde.Text) > CInt(TextHasta.Text) Then
            MsgBox("Añio Hasta debe ser Mayor o Igual a Añio Desde.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT * FROM CierreContable WHERE Anio <= " & CInt(TextHasta.Text) & " AND Anio >= " & CInt(TextDesde.Text) & " ORDER BY Anio,Mes;"

        If Not Tablas.Read(Sql, Conexion, Dt) Then End : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Anio").ReadOnly = True
            Grid.Rows(I).Cells("Mes").ReadOnly = True
            If Grid.Rows(I).Cells("Cerrado").Value Then
                Grid.Rows(I).Cells("OpcionPantalla").Value = Grid.Rows(I).Cells("Opcion").Value
            Else
                Grid.Rows(I).Cells("OpcionPantalla").Value = Opcion1LibrosIva
            End If
        Next

        AddHandler Dt.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanging)
        AddHandler Dt.RowChanging, New DataRowChangeEventHandler(AddressOf Dt_RowChanging)
        AddHandler Dt.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dt_NewRow)
        AddHandler Dt.RowDeleting, New DataRowChangeEventHandler(AddressOf Dt_Deleting)
        AddHandler Dt.RowChanged, New DataRowChangeEventHandler(AddressOf Dt_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Dim DtMeses As New DataTable

        Dim Mes1 As New DataColumn("Mes")
        Mes1.DataType = System.Type.GetType("System.Int32")
        DtMeses.Columns.Add(Mes1)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtMeses.Columns.Add(Nombre)

        Dim Row As DataRow = DtMeses.NewRow
        Row("Mes") = 1
        Row("Nombre") = "Enero"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 2
        Row("Nombre") = "Febrero"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 3
        Row("Nombre") = "Marzo"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 4
        Row("Nombre") = "Abril"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 5
        Row("Nombre") = "Mayo"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 6
        Row("Nombre") = "Junio"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 7
        Row("Nombre") = "Julio"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 8
        Row("Nombre") = "Agosto"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 9
        Row("Nombre") = "Septiembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 10
        Row("Nombre") = "Octubre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 11
        Row("Nombre") = "Noviembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 12
        Row("Nombre") = "Diciembre"
        DtMeses.Rows.Add(Row)
        Row = DtMeses.NewRow
        Row("Mes") = 0
        Row("Nombre") = ""
        DtMeses.Rows.Add(Row)
        '
        Mes.DataSource = DtMeses
        Mes.DisplayMember = "Nombre"
        Mes.ValueMember = "Mes"

        Dim DtOpcion As New DataTable
        '
        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Boolean")
        DtOpcion.Columns.Add(Clave)
        '
        Dim NombreOpcion As New DataColumn("NombreOpcion")
        NombreOpcion.DataType = System.Type.GetType("System.String")
        DtOpcion.Columns.Add(NombreOpcion)
        '
        Row = DtOpcion.NewRow
        Row("Clave") = True
        Row("NombreOpcion") = "Opcion 1"
        DtOpcion.Rows.Add(Row)
        '
        Row = DtOpcion.NewRow
        Row("Clave") = False
        Row("NombreOpcion") = "Opcion 2"
        DtOpcion.Rows.Add(Row)
        '
        ''''    Opcion.DataSource = DtOpcion
        ''''      Opcion.DisplayMember = "NombreOpcion"
        ''''      Opcion.ValueMember = "Clave"

        OpcionPantalla.DataSource = DtOpcion
        OpcionPantalla.DisplayMember = "NombreOpcion"
        OpcionPantalla.ValueMember = "Clave"

    End Sub
    Private Function HallaBalanceCerrado(ByVal Mes As Integer, ByVal Anio As Integer) As Boolean

        Dim Dt As New DataTable
        Dim IntDesde As Integer
        Dim IntHasta As Integer
        Dim IntFecha As Integer = Format(Anio, "0000") & Format(Mes, "00")

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT Desde,Hasta,Cerrado FROM Balances;", Conexion, Dt) Then Me.Cursor = System.Windows.Forms.Cursors.Default : End
        For Each Row As DataRow In Dt.Rows
            IntDesde = Format(Row("Desde").year, "0000") & Format(Row("Desde").month, "00")
            IntHasta = Format(Row("Hasta").year, "0000") & Format(Row("Hasta").month, "00")
            If IntDesde <= IntFecha And IntFecha <= IntHasta Then
                If Row("Cerrado") Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Dt.Dispose() : Return True
                Else
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Dt.Dispose() : Return False
                End If
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return False

    End Function
    Private Function UltimoAnio(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Anio) FROM CierreContable;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 1900
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function

    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Mes" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Anio" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloEntero_KeyPress
        End If

    End Sub
    Private Sub SoloEntero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Anio" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(0, "#")
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dt_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Anio") = 0
        e.Row("Mes") = 0
        e.Row("Cerrado") = False

    End Sub
    Private Sub Dt_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Anio") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

    End Sub
    Private Sub Dt_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

        If e.Row("Anio") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Año.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If
        If e.Row("Anio") < Date.Now.Year - 3 Or e.Row("Anio") > Date.Now.Year + 1 Then
            e.Row.RowError = "Error."
            MsgBox("Año fuera del Limite Permitido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If e.Row("Mes") = 0 Then
            e.Row.RowError = "Error."
            MsgBox("Falta Mes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

    End Sub
    Private Sub Dt_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        Dim RowsBusqueda() As DataRow

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub
        If e.Row.RowError <> "" Then Exit Sub

        RowsBusqueda = Dt.Select("Anio = " & e.Row("Anio") & " AND Mes = " & e.Row("Mes"))
        If RowsBusqueda.Length > 1 Then
            e.Row.RowError = "Error."
            MsgBox("Fecha Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If HallaBalanceCerrado(e.Row("Mes"), e.Row("Anio")) Then
            If Not e.Row("Cerrado") Then
                MsgBox("Periodo Corresponde a un Balance Cerrado.")
                e.Row.RowError = "Error."
            End If
        End If

    End Sub
    Private Sub Dt_Deleting(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

    End Sub


End Class