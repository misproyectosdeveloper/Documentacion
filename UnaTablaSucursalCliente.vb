Public Class UnaTablaSucursalcliente
    Public PCliente As Integer
    Public PProveedor As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtProvincia As DataTable
    Dim DtZona As DataTable
    Private Sub UnaTablaSucursal_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Dim Row As DataRow

        DtZona = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 40;", Conexion, DtZona) Then Me.Close() : Exit Sub
        Row = DtZona.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtZona.Rows.Add(Row)

        DtProvincia = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 31;", Conexion, DtProvincia) Then Me.Close() : Exit Sub
        Row = DtProvincia.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        DtProvincia.Rows.Add(Row)

        ComboZona.DataSource = DtZona
        ComboZona.DisplayMember = "Nombre"
        ComboZona.ValueMember = "Clave"
        With ComboZona
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboZona.SelectedValue = 0

        ComboProvincia.DataSource = DtProvincia
        ComboProvincia.DisplayMember = "Nombre"
        ComboProvincia.ValueMember = "Clave"
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboProvincia.SelectedValue = 0

        LlenaComboTablas(ComboCanalDistribucion, 45)
        ComboCanalDistribucion.SelectedValue = 0
        With ComboCanalDistribucion
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"
        ComboEstado.SelectedValue = 0

        LlenaCombosGrid()

        LLenaGrid()
        BorraSeleccionGrid()
        MuestraSucursal(-100)

        If PProveedor <> 0 Then
            LabelCanalDistribucion.Visible = False
            ComboCanalDistribucion.Visible = False
            Label16.Visible = False
        End If

        Panel1.Visible = False

    End Sub
    Private Sub UnaTablaSucursal_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) And Not PBloqueaFunciones Then
            If MsgBox("Los Cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida(CInt(TextClaveOculta.Text)) Then Exit Sub

        Dim Row As DataRow
        Dim HuboCambios As Boolean

        If CInt(TextClaveOculta.Text) = 0 Then
            Row = Dt.NewRow
            HuboCambios = ArmaRegistro(Row)
            Dt.Rows.Add(Row)
        Else
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Clave = " & CInt(TextClaveOculta.Text))
            HuboCambios = ArmaRegistro(RowsBusqueda(0))
        End If

        If Not HuboCambios Then MsgBox("No Hubo Cambios.", MsgBoxStyle.Information) : Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If GrabaSucursal(Dt) Then
            LLenaGrid()
            BorraSeleccionGrid()
            Grid.Rows(Grid.Rows.Count - 1).Selected = True  'muestra eltima sucursal agregada.
            MuestraSucursal(Grid.Rows(Grid.Rows.Count - 1).Cells("Clave").Value)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PCliente <> 0 Then
            If Row("Clave") <> 0 Then
                If Usada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("Item esta siendo usado en Remitos o Facturas. Operación se CANCELA.)", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If Usada(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("Item esta siendo usado en Remitos o Facturas. Operación se CANCELA.)", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If UsadaEnPedido(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("Item esta siendo usado en un Pedido. Operación se CANCELA.)", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
        End If

        bs.RemoveCurrent()

        If GrabaSucursal(Dt) Then
            LLenaGrid()
            LimpiaPanel1()
            Panel1.Visible = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboProvincia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProvincia.Validating

        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

    End Sub
    Private Sub ComboEstado_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEstado.Validating

        If IsNothing(ComboEstado.SelectedValue) Then ComboEstado.SelectedValue = 0

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub ButtonAlta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAlta.Click

        Panel1.Visible = True
        MuestraSucursal(-100)

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

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PCliente <> 0 Then
            If Not Tablas.Read("SELECT * FROM SucursalesClientes WHERE Cliente = " & PCliente & ";", Conexion, Dt) Then End : Exit Sub
        End If
        If PProveedor <> 0 Then
            If Not Tablas.Read("SELECT * FROM SucursalesProveedores WHERE Proveedor = " & PProveedor & ";", Conexion, Dt) Then End : Exit Sub
        End If

        If PProveedor <> 0 Then Grid.Columns("CanalDistribucion").Visible = False

        Grid.DataSource = bs
        bs.DataSource = Dt

        TextNombre.Focus()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraSucursal(ByVal Clave As Integer)

        Panel1.Visible = True

        If Clave > 0 Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Clave = " & Clave)
            Dim Row As DataRow = RowsBusqueda(0)
            TextClaveOculta.Text = Row("Clave")
            TextNombre.Text = Row("Nombre")
            TextDireccion.Text = Row("Direccion")
            TextCalle.Text = Row("Calle")
            TextNumero.Text = Row("Numero")
            If Row("Numero") = 0 Then TextNumero.Text = ""
            TextCodigoPostal.Text = Row("CodigoPostal")
            TextLocalidad.Text = Row("Localidad")
            If PCliente <> 0 Then ComboCanalDistribucion.SelectedValue = Row("CanalDistribucion")
            ComboProvincia.SelectedValue = Row("Provincia")
            TextCodigoExterno.Text = Row("CodigoExterno")
            ComboZona.SelectedValue = Row("Zona")
            ComboEstado.SelectedValue = Row("Estado")
            TextComentario.Text = Row("Comentario")
        Else
            LimpiaPanel1()
        End If

    End Sub
    Private Function ArmaRegistro(ByRef Row As DataRow) As Boolean

        Dim HuboCambios As Boolean

        If CInt(TextClaveOculta.Text) = 0 Then
            If PCliente <> 0 Then
                Row("Cliente") = PCliente : Row("CanalDistribucion") = 0
            End If

            If PProveedor <> 0 Then Row("Proveedor") = PProveedor
            Row("Nombre") = ""
            Row("Direccion") = ""
            Row("Calle") = ""
            Row("Numero") = 0
            Row("CodigoPostal") = ""
            Row("Localidad") = ""
            Row("Provincia") = 0
            Row("CodigoExterno") = ""
            Row("Zona") = 0
            Row("Estado") = 0
            Row("Comentario") = ""
        End If
        If TextNombre.Text.Trim <> Row("Nombre") Then Row("Nombre") = TextNombre.Text.Trim : HuboCambios = True
        If TextDireccion.Text.Trim <> Row("Direccion") Then Row("Direccion") = TextDireccion.Text.Trim : HuboCambios = True
        If TextCalle.Text.Trim <> Row("Calle") Then Row("Calle") = TextCalle.Text.Trim : HuboCambios = True
        If TextNumero.Text = "" Then
            If Row("Numero") <> 0 Then Row("Numero") = 0 : HuboCambios = True
        Else
            If CInt(TextNumero.Text) <> Row("Numero") Then Row("Numero") = CInt(TextNumero.Text) : HuboCambios = True
        End If
        If TextCodigoPostal.Text.Trim <> Row("CodigoPostal") Then Row("CodigoPostal") = TextCodigoPostal.Text.Trim : HuboCambios = True
        If TextLocalidad.Text.Trim <> Row("Localidad") Then Row("Localidad") = TextLocalidad.Text.Trim : HuboCambios = True
        If ComboProvincia.SelectedValue <> Row("Provincia") Then Row("Provincia") = ComboProvincia.SelectedValue : HuboCambios = True
        If PCliente <> 0 Then
            If ComboCanalDistribucion.SelectedValue <> Row("CanalDistribucion") Then Row("CanalDistribucion") = ComboCanalDistribucion.SelectedValue : HuboCambios = True
        End If
        If TextCodigoExterno.Text.Trim <> Row("CodigoExterno") Then Row("CodigoExterno") = TextCodigoExterno.Text.Trim : HuboCambios = True
        If ComboZona.SelectedValue <> Row("Zona") Then Row("Zona") = ComboZona.SelectedValue : HuboCambios = True
        If ComboEstado.SelectedValue <> Row("Estado") Then Row("Estado") = ComboEstado.SelectedValue : HuboCambios = True
        If TextComentario.Text.Trim <> Row("Comentario") Then Row("Comentario") = TextComentario.Text.Trim : HuboCambios = True

        Return HuboCambios

    End Function
    Private Function GrabaSucursal(ByVal DtSucursalAux As DataTable) As Boolean

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Dim Sql As String = ""
                    If PCliente <> 0 Then
                        Sql = "SELECT * FROM SucursalesClientes;"
                    End If
                    If PProveedor <> 0 Then
                        Sql = "SELECT * FROM SucursalesProveedores;"
                    End If
                    Using DaP As New OleDb.OleDbDataAdapter(Sql, MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtSucursalAux.GetChanges)
                    End Using
                    Trans.Commit()
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                    Return True
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

        Return False

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Estado.DataSource = DtEstadoActivoYBaja()
        Row = Estado.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Estado.DataSource.rows.add(Row)
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

        Zona.DataSource = DtZona
        Row = Zona.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Zona.DataSource.rows.add(Row)
        Zona.DisplayMember = "Nombre"
        Zona.ValueMember = "Clave"

        Provincia.DataSource = DtProvincia
        Row = Provincia.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = ""
        Provincia.DataSource.rows.add(Row)
        Provincia.DisplayMember = "Nombre"
        Provincia.ValueMember = "Clave"

        If PCliente <> 0 Then
            CanalDistribucion.DataSource = ComboCanalDistribucion.DataSource.copy
            Row = CanalDistribucion.DataSource.newrow
            Row("Clave") = 0
            Row("Nombre") = ""
            CanalDistribucion.DataSource.rows.add(Row)
            CanalDistribucion.DisplayMember = "Nombre"
            CanalDistribucion.ValueMember = "Clave"
        End If
        

    End Sub
    Private Sub BorraSeleccionGrid()

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Selected = False
        Next

    End Sub
    Private Function Usada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Sucursal) FROM RemitosCabeza WHERE Sucursal = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Sucursal) FROM FacturasCabeza WHERE Sucursal = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: RemitosCabeza o FacturasCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function UsadaEnPedido(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Sucursal) FROM PedidosCabeza WHERE Sucursal = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: PedidosCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Sub LimpiaPanel1()

        TextClaveOculta.Text = "0"
        TextNombre.Text = ""
        TextDireccion.Text = ""
        TextCalle.Text = ""
        TextNumero.Text = ""
        TextCodigoPostal.Text = ""
        TextLocalidad.Text = ""
        If PCliente <> 0 Then ComboCanalDistribucion.SelectedValue = 0
        ComboProvincia.SelectedValue = 0
        TextCodigoExterno.Text = ""
        ComboZona.SelectedValue = 0
        ComboEstado.SelectedValue = 1
        TextComentario.Text = ""

    End Sub
    Private Function Valida(ByVal Clave As Integer) As Boolean

        If TextNombre.Text.Trim = "" Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextNombre.Focus()
            Exit Function
        End If

        If TextDireccion.Text.Trim = "" Then
            MsgBox("Falta Direccion.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCalle.Focus()
            Exit Function
        End If

        If TextCodigoExterno.Text.Trim = "" Then
            If MsgBox("Falta Código Externo. Quiere continuar Igualmente? (Y/N).", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3, "") = MsgBoxResult.No Then
                TextCodigoExterno.Focus() : Return False
            End If
        End If

        If ComboEstado.SelectedValue = 0 Then
            MsgBox("Falta Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            ComboEstado.Focus()
            Exit Function
        End If

        If Dt.Rows.Count <> 0 Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Clave <> " & Clave & " AND Nombre = '" & TextNombre.Text.Trim & "'")
            If RowsBusqueda.Length <> 0 Then
                MsgBox("Nombre ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextNombre.Focus()
                Return False
            End If
            '
            If TextCodigoExterno.Text.Trim <> "" Then
                RowsBusqueda = Dt.Select("Clave <> " & Clave & " AND CodigoExterno = '" & TextCodigoExterno.Text.Trim & "'")
                If RowsBusqueda.Length <> 0 Then
                    MsgBox("Codigo Externo ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextCodigoExterno.Focus()
                    Return False
                End If
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

        If Not IsDBNull(Grid.CurrentRow.Cells("Clave").Value) Then MuestraSucursal(Grid.CurrentRow.Cells("Clave").Value)

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
End Class