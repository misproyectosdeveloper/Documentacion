Imports ClassPassWord
Public Class UnaTablaTransporte
    Dim Dt As DataTable
    Dim DtProveedores As DataTable
    Dim DtGrid As DataTable
    '
    Private WithEvents bs As New BindingSource
    Private Sub UnaTablaTransporte_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        DtProveedores = CreaTransportistas()

        ComboProveedor.DataSource = DtProveedores
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        CreaDtGrid()

        LlenaCombosGrid()

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        ComboProveedor.Enabled = False

    End Sub
    Private Sub UnaTablaTransporte_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAlta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAlta.Click

        MuestraTransporte(-100)

    End Sub
    Private Sub ButtonGrabaCambios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGrabaCambios.Click

        If Not Valida(CInt(TextClaveOculta.Text)) Then Exit Sub

        If CInt(TextClaveOculta.Text) = 0 Then
            AltaTabla()
        Else
            ModificaTabla(CInt(TextClaveOculta.Text))
        End If

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Clave = " & CInt(TextClaveOculta.Text))
        If RowsBusqueda.Length = 0 Then
            MsgBox("Transporte No Existe.", MsgBoxStyle.Critical) : Exit Sub
        End If
        RowsBusqueda(0).Delete()

        If MsgBox("Transporte se Borraran definitivamente del Sistema. Desa Continuar?.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If GrabaTransporte(Dt) Then
            LLenaGrid()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub Comboproveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub TextCamion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCamion.KeyPress

        e.KeyChar = e.KeyChar.ToString.ToUpper()

    End Sub
    Private Sub TextAcoplado_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAcoplado.KeyPress

        e.KeyChar = e.KeyChar.ToString.ToUpper()

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

        Dt = New DataTable
        DtGrid.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT * FROM Tablas WHERE Tipo = 43;", Conexion, Dt) Then End : Exit Sub

        For Each Row As DataRow In Dt.Rows
            Dim RowG As DataRow = DtGrid.NewRow
            RowG("Clave") = Row("Clave")
            RowG("Tipo") = Row("Tipo")
            RowG("Nombre") = Row("Nombre")
            RowG("Proveedor") = Row("Operador")
            RowG("Camion") = Strings.Mid(Row("Comentario"), 1, 6)
            RowG("Acoplado") = Strings.Mid(Row("Comentario"), 8, 6)
            DtGrid.Rows.Add(RowG)
        Next

        If Dt.Rows.Count = 0 Then
            TextNombre.Text = ""
            TextNombre.ReadOnly = True
            ComboProveedor.Enabled = False
            TextCamion.Text = ""
            TextCamion.ReadOnly = True
            TextAcoplado.Text = ""
            TextAcoplado.ReadOnly = True
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Remito As New DataColumn("Clave")
        Remito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Remito)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Tipo)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Proveedor)

        Dim Camion As New DataColumn("Camion")
        Camion.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Camion)

        Dim Acoplado As New DataColumn("Acoplado")
        Acoplado.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Acoplado)

    End Sub
    Private Sub MuestraTransporte(ByVal Clave As Integer)

        TextNombre.ReadOnly = False
        ComboProveedor.Enabled = True
        TextCamion.ReadOnly = False
        TextAcoplado.ReadOnly = False

        If Clave > 0 Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = DtGrid.Select("Clave = " & Clave)
            Dim Row As DataRow = RowsBusqueda(0)
            TextClaveOculta.Text = Row("Clave")
            TextNombre.Text = Row("Nombre")
            ComboProveedor.SelectedValue = Row("Proveedor")
            TextCamion.Text = Row("Camion")
            TextAcoplado.Text = Row("Acoplado")
            ComboProveedor.Enabled = False
        Else
            TextClaveOculta.Text = 0
            TextNombre.Text = ""
            ComboProveedor.SelectedValue = 0
            TextCamion.Text = ""
            TextAcoplado.Text = ""
            ComboProveedor.Enabled = True
        End If

    End Sub
    Private Sub AltaTabla()

        Dim Row As DataRow = Dt.NewRow
        ArmaRowTabla(Row)
        Row("Tipo") = 43
        Row("Nombre") = TextNombre.Text.Trim
        Row("Operador") = ComboProveedor.SelectedValue
        Row("Comentario") = ArmaComentario("")
        Dt.Rows.Add(Row)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If GrabaTransporte(Dt) Then
            LLenaGrid()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ModificaTabla(ByVal Clave As Integer)

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Clave = " & Clave)
        If RowsBusqueda.Length = 0 Then
            MsgBox("Transporte No Existe.", MsgBoxStyle.Critical) : Exit Sub
        End If

        RowsBusqueda(0).Item("Nombre") = TextNombre.Text.Trim
        RowsBusqueda(0).Item("Comentario") = ArmaComentario(RowsBusqueda(0).Item("Comentario"))

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If GrabaTransporte(Dt) Then
            LLenaGrid()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ArmaComentario(ByVal Comentario As String) As String

        Dim Com As String = TextCamion.Text
        If TextAcoplado.Text <> "" Then Com = Com & ";" & TextAcoplado.Text
        If Comentario <> Com Then Return Com

        Return ""

    End Function
    Private Function GrabaTransporte(ByVal DtAux As DataTable) As Boolean

        MsgBox(Tablas.GrabarOleDb(DtAux, "SELECT * FROM Tablas;", Conexion))
        If GModificacionOk Then Return True

    End Function
    Private Sub ArmaRowTabla(ByRef Row As DataRow)

        AceraRowTablas(Row)

    End Sub
    Private Sub LlenaCombosGrid()

        Proveedor.DataSource = DtProveedores
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    Private Function CreaTransportistas() As DataTable

        Dim DtGrid As New DataTable

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Nombre)

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Clave)

        Dim Row As DataRow = DtGrid.NewRow
        Row("Nombre") = "Propio"
        Row("Clave") = 0
        DtGrid.Rows.Add(Row)

        If Not Tablas.Read("SELECT Nombre,Clave FROM Proveedores WHERE Producto = 8;", Conexion, DtGrid) Then Return Nothing

        Return DtGrid

    End Function
    Private Function Valida(ByVal Clave As Integer) As Boolean

        If TextNombre.Text = "" Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextNombre.Focus()
            Exit Function
        End If

        If TextCamion.Text.Trim <> "" Then
            If Not PatenteOk(TextCamion.Text.Trim) Then
                MsgBox("Patente Camión Erronea.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextCamion.Focus()
                Exit Function
            End If
        End If

        If TextAcoplado.Text.Trim <> "" Then
            If Not PatenteOk(TextAcoplado.Text.Trim) Then
                MsgBox("Patente Acoplado Erronea.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextAcoplado.Focus()
                Exit Function
            End If
        End If

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid.Select("Clave <> " & Clave & " AND Nombre = '" & TextNombre.Text.Trim & "'")
        If RowsBusqueda.Length <> 0 Then
            MsgBox("Nombre ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            ComboProveedor.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        '   If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

        If Not IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then MuestraTransporte(Grid.CurrentRow.Cells("Clave").Value)

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

  
  
End Class