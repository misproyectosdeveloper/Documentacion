Public Class UnCambioProveedorIngresoMercaderia
    Dim Abierto As Boolean
    Dim ConexionIngreso As String
    Dim DtIngreso As DataTable
    Dim DtLotes As DataTable
    Dim ProveedorNuevo As Integer
    Dim DtProveedorNuevo As DataTable
    Dim TipoOperacionNuevo As Integer
    Dim CosteoNuevo As Integer
    Private Sub UnCambioProveedorIngresoMercaderia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisorActual.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE Producto = " & Fruta & " ORDER BY Nombre;")
        Dim Row As DataRow = ComboEmisorActual.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisorActual.DataSource.Rows.Add(Row)
        ComboEmisorActual.DisplayMember = "Nombre"
        ComboEmisorActual.ValueMember = "Clave"
        ComboEmisorActual.SelectedValue = 0
        With ComboEmisorActual
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ' 
        ComboEmisorNuevo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE Producto = " & Fruta & " ORDER BY Nombre;")
        ComboAliasNuevo.DataSource = Tablas.Leer("SELECT Clave,Alias   FROM Proveedores WHERE Alias <> '' AND Producto = " & Fruta & " ORDER BY Alias;")
        Row = ComboEmisorNuevo.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisorNuevo.DataSource.Rows.Add(Row)
        ComboEmisorNuevo.DisplayMember = "Nombre"
        ComboEmisorNuevo.ValueMember = "Clave"
        ComboEmisorNuevo.SelectedValue = 0
        Row = ComboAliasNuevo.DataSource.NewRow
        Row("Clave") = 0
        Row("Alias") = ""
        ComboAliasNuevo.DataSource.Rows.Add(Row)
        ComboAliasNuevo.DisplayMember = "Alias"
        ComboAliasNuevo.ValueMember = "Clave"
        ComboAliasNuevo.SelectedValue = 0
        With ComboEmisorNuevo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        With ComboAliasNuevo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Abierto = True

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        Row = ComboMoneda.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        ComboMoneda.SelectedValue = 0

        If PermisoTotal Then PictureCandado.Visible = True

        PictureLupa.Image = ImageList1.Images.Item("Lupa")

        VerSucursales(0)
        VerCosteo(0)

    End Sub
    Private Sub UnCambioProveedorIngresoMercaderia_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        For Each Row As DataRow In DtIngreso.Rows
            Row("Proveedor") = ProveedorNuevo
            Row("TipoOperacion") = TipoOperacionNuevo
            Row("Costeo") = CosteoNuevo
            Row("Moneda") = ComboMoneda.SelectedValue
            Row("Sucursal") = ComboSucursales.SelectedValue
        Next
        For Each Row As DataRow In DtLotes.Rows
            Row("Proveedor") = ProveedorNuevo
            Row("TipoOperacion") = TipoOperacionNuevo
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(ConexionIngreso)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM IngresoMercaderiasCabeza;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtIngreso)
                    End Using
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtLotes)
                    End Using
                    Trans.Commit()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        If GModificacionOk Then Me.Close()

    End Sub
    Private Sub ComboProveedorActual_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisorActual.Validating

        If IsNothing(ComboEmisorActual.SelectedValue) Then ComboEmisorActual.SelectedValue = 0

    End Sub
    Private Sub ComboProveedorNuevo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisorNuevo.Validating

        If IsNothing(ComboEmisorNuevo.SelectedValue) Then ComboEmisorNuevo.SelectedValue = 0

        VerSucursales(ComboEmisorNuevo.SelectedValue)
        VerCosteo(ComboEmisorNuevo.SelectedValue)
        If ComboEmisorNuevo.SelectedValue <> 0 Then LeeDtProveedorNuevo(ComboEmisorNuevo.SelectedValue)

    End Sub
    Private Sub ComboAliasNuevo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAliasNuevo.Validating

        If IsNothing(ComboAliasNuevo.SelectedValue) Then ComboAliasNuevo.SelectedValue = 0

        VerSucursales(ComboAliasNuevo.SelectedValue)
        VerCosteo(ComboAliasNuevo.SelectedValue)
        If ComboAliasNuevo.SelectedValue <> 0 Then LeeDtProveedorNuevo(ComboAliasNuevo.SelectedValue)

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        ComboEmisorActual.SelectedValue = 0

        If Abierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            Abierto = False
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Abierto = True
        End If

    End Sub
    Private Sub TextIngreso_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIngreso.KeyPress

        ComboEmisorActual.SelectedValue = 0

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextIngreso_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextIngreso.Validating

        ComboEmisorNuevo.SelectedValue = 0
        ComboAliasNuevo.SelectedValue = 0
        ComboSucursales.SelectedValue = 0
        ComboCosteo.SelectedValue = 0
        ComboMoneda.SelectedValue = 0

    End Sub
    Private Sub TextCosteo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        If TextIngreso.Text = "" Then
            MsgBox("Debe Ingresar Numero de Ingreso.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Abierto Then
            ConexionIngreso = Conexion
        Else : ConexionIngreso = ConexionN
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtIngreso = New DataTable
        If Not Tablas.Read("SELECT * FROM IngresoMercaderiasCabeza WHERE Lote = " & TextIngreso.Text & ";", ConexionIngreso, DtIngreso) Then Me.Close() : Exit Sub
        If DtIngreso.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Ingreso No Existe.", MsgBoxStyle.Information)
            Exit Sub
        End If

        DtLotes = New DataTable
        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & TextIngreso.Text & ";", ConexionIngreso, DtLotes) Then Me.Close() : Exit Sub

        ComboEmisorActual.SelectedValue = DtIngreso.Rows(0).Item("Proveedor")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LeeDtProveedorNuevo(ByVal Emisor As Integer)

        DtProveedorNuevo = New DataTable
        If Not Tablas.Read("SELECT * FROM Proveedores WHERE Clave = " & Emisor & ";", Conexion, DtProveedorNuevo) Then Me.Close() : Exit Sub
        If DtProveedorNuevo.Rows.Count = 0 Then
            MsgBox("Proveedor " & NombreProveedor(Emisor) & " No Encontrado.", MsgBoxStyle.Critical)
            ComboEmisorNuevo.SelectedValue = 0
            ComboAliasNuevo.SelectedValue = 0
        End If

        ComboMoneda.SelectedValue = DtProveedorNuevo.Rows(0).Item("Moneda")
        DtProveedorNuevo.Dispose()

    End Sub
    Private Sub VerCosteo(ByVal Emisor As Integer)

        ComboCosteo.DataSource = Tablas.Leer("SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & Emisor & ";")
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesProveedores WHERE Estado = 1 AND Proveedor = " & Emisor & ";"
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
    Private Function Valida() As Boolean

        If TextIngreso.Text = "" Then
            MsgBox("Falta Ingresar Ingreso o Lote.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextIngreso.Focus()
            Return False
        End If

        If ComboEmisorActual.SelectedValue = 0 Then
            MsgBox("Falta Nuevo Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisorNuevo.Focus()
            Return False
        End If

        For Each Row As DataRow In DtLotes.Rows
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                If Row("Liquidado") <> 0 Then
                    MsgBox("Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & "  Esta liquidado. Operación se Cancela.", MsgBoxStyle.Critical)
                    Return False
                End If
            End If
        Next

        If DtIngreso.Rows(0).Item("TipoOperacion") = 4 Then
            If HallaCosteoCerrado(DtIngreso.Rows(0).Item("Costeo")) Then
                MsgBox("Costeo del Ingreso Cerrado.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If ComboEmisorNuevo.SelectedValue = 0 And ComboAliasNuevo.SelectedValue = 0 Then
            MsgBox("Falta Nuevo Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisorNuevo.Focus()
            Return False
        End If

        If ComboEmisorNuevo.SelectedValue <> 0 And ComboAliasNuevo.SelectedValue <> 0 Then
            MsgBox("Debe Informar Nuevo Proveedor o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisorNuevo.Focus()
            Return False
        End If

        If ComboEmisorNuevo.SelectedValue <> 0 Then ProveedorNuevo = ComboEmisorNuevo.SelectedValue
        If ComboAliasNuevo.SelectedValue <> 0 Then ProveedorNuevo = ComboAliasNuevo.SelectedValue

        If ProveedorNuevo = ComboEmisorActual.SelectedValue Then
            MsgBox("Proveedor Actual igual al Nuevo proveedor. Operación se Cancela.")
            Return False
        End If

        TipoOperacionNuevo = HallaTipoOperacion(ProveedorNuevo)
        If TipoOperacionNuevo < 0 Then
            MsgBox("Error Base de Datos al Leer Proveedores.", MsgBoxStyle.Critical)
            Return False
        End If

        CosteoNuevo = 0
        If ComboCosteo.SelectedValue <> 0 And TipoOperacionNuevo <> 4 Then
            MsgBox("No debe Informar Costeo para Proveedor que no es un Negocio.", MsgBoxStyle.Critical)
            Return False
        End If

        If TipoOperacionNuevo = 4 Then
            If ComboCosteo.SelectedValue = 0 Then
                MsgBox("Debe Informar el Nuevo Costeo.", MsgBoxStyle.Critical)
                Return False
            End If
            CosteoNuevo = ComboCosteo.SelectedValue
            Dim ListaDeArticulos As New List(Of ItemListaDePrecios)
            For Each Row As DataRow In DtLotes.Rows
                Dim Fila As New ItemListaDePrecios
                Fila.Articulo = Row("Articulo")
                ListaDeArticulos.Add(Fila)
            Next
            If Not ValidaCosteo(ProveedorNuevo, CosteoNuevo, ListaDeArticulos, DtIngreso.Rows(0).Item("Fecha")) Then Return False
        End If

        Return True

    End Function
  
    
End Class