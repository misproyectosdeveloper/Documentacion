Public Class OpcionIngreso
    ' Si orden de compra es mixta solo muestra la orden en B.
    Public PAbierto As Boolean
    Public PSoloInsumos As Boolean
    Public PSoloFrutas As Boolean
    Public PEsSoloAltas As Boolean
    Public PArticulosLogisticos As Boolean
    Public PConOrdenCompra As Boolean
    Public PRegresar As Boolean
    Public POrdenCompra As Double
    Public PEmisor As Integer
    Public PEmisorAnt As Integer
    Public PNegocioAnt As Integer
    Public PDepositoAnt As Integer
    Public PNombreEmisor As String
    Public Plista As Integer
    Public Zona As Integer = 0
    Public PPorUnidadEnLista As Boolean
    Public PFinalEnLista As Boolean
    'Para importacion.
    Public PRemitoImportacion As Decimal
    Public PDtImportacion As DataTable
    Public PAbiertoImportacion As Boolean
    Public PFechaRemito As Date = Now
    Public PSucursal As Integer
    '
    Dim ProveedorW As Integer = 0
    Private Sub OpcionIngreso_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Dim Row As DataRow

        If PSoloInsumos Then
            ComboEmisor.DataSource = ProveedoresDeInsumos()
            ComboEmisor.DisplayMember = "Nombre"
            ComboEmisor.ValueMember = "Clave"
            ComboEmisor.SelectedValue = 0

            ComboAlias.DataSource = ProveedoresDeInsumosAlias()
            ComboAlias.DisplayMember = "Alias"
            ComboAlias.ValueMember = "Clave"
            ComboAlias.SelectedValue = 0
            ComboNegocio.Visible = False
            Label1.Visible = False
            '
            LlenaComboTablas(ComboDeposito, 20)
            ComboDeposito.SelectedValue = 0
            Grid.Visible = True
            Me.BackColor = Color.Thistle
            If PArticulosLogisticos Then
                ComboNegocio.Visible = False
                Label1.Visible = False
            End If
        Else
            If PSoloFrutas Then
                ComboEmisor.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado FROM Proveedores WHERE TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
                Row = ComboEmisor.DataSource.newrow
                Row("Clave") = 0
                Row("Nombre") = ""
                ComboEmisor.DataSource.rows.add(Row)
                ComboEmisor.DisplayMember = "Nombre"
                ComboEmisor.ValueMember = "Clave"
                ComboEmisor.SelectedValue = PEmisorAnt
                '
                ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '' AND TipoOperacion <> 4 AND Producto = " & Fruta & " ORDER BY Alias;")
                Row = ComboAlias.DataSource.newrow
                Row("Clave") = 0
                Row("Alias") = ""
                ComboAlias.DataSource.rows.add(Row)
                ComboAlias.DisplayMember = "Alias"
                ComboAlias.ValueMember = "Clave"
                ComboAlias.SelectedValue = 0
                ' 
                ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre,Estado FROM Proveedores WHERE TipoOperacion = 4 AND Producto = " & Fruta & " ORDER BY Nombre;")
                Row = ComboNegocio.DataSource.newrow
                Row("Clave") = 0
                Row("Nombre") = ""
                ComboNegocio.DataSource.rows.add(Row)
                ComboNegocio.DisplayMember = "Nombre"
                ComboNegocio.ValueMember = "Clave"
                ComboNegocio.SelectedValue = PNegocioAnt
                '
                LlenaComboTablas(ComboDeposito, 19)
                ComboDeposito.SelectedValue = PDepositoAnt
                PanelSucursal.Visible = True
            Else
                ComboEmisor.DataSource = Tablas.Leer("Select Clave,Nombre,Estado From Proveedores ORDER BY Nombre;")
                ComboEmisor.SelectedValue = 0
                ComboAlias.DataSource = Tablas.Leer("SELECT Clave,Alias FROM Proveedores WHERE Alias <> '';")
                Row = ComboAlias.DataSource.newrow
                Row("Clave") = 0
                Row("Alias") = ""
                ComboAlias.DataSource.rows.add(Row)
                ComboAlias.DisplayMember = "Alias"
                ComboAlias.ValueMember = "Clave"
                ComboAlias.SelectedValue = 0
                '
                LlenaComboTablas(ComboDeposito, 19)
                ComboDeposito.SelectedValue = 0
                ComboNegocio.Visible = False
                Label1.Visible = False
            End If
        End If

        If PConOrdenCompra Then
            Grid.Visible = True
            ButtonOrdenCompra.Visible = True
        Else
            Grid.Visible = False
            ButtonOrdenCompra.Visible = False
        End If

        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboAlias
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If Not PSoloFrutas Then
            ButtonImportar.Visible = False : ButtonImportarExcel.Visible = False
        End If

        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        PAbierto = True

        If PSoloInsumos Then
            PictureCandado.Visible = False
        End If

        If PConOrdenCompra And PEmisorAnt <> 0 Then
            ComboEmisor_Validating(Nothing, Nothing)
        End If

        PRegresar = True
        PEmisor = 0
        PRemitoImportacion = 0

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PEmisor = ComboEmisor.SelectedValue : PNombreEmisor = ComboEmisor.Text
        If ComboAlias.SelectedValue <> 0 Then PEmisor = ComboAlias.SelectedValue : PNombreEmisor = NombreProveedor(ComboAlias.SelectedValue)
        If ComboNegocio.SelectedValue <> 0 Then PEmisor = ComboNegocio.SelectedValue : PNombreEmisor = ComboNegocio.Text

        POrdenCompra = 0
        If PSoloInsumos Or PSoloFrutas Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    POrdenCompra = Row.Cells("Orden").Value
                End If
            Next
        End If

        If PSoloFrutas Then
            PSucursal = ComboSucursales.SelectedValue
            PFechaRemito = FechaIngreso.Value
        End If

        PRegresar = False
        Me.Close()

    End Sub
    Private Function HallaProveedor() As Integer

        If ComboEmisor.SelectedValue <> 0 Then
            Return ComboEmisor.SelectedValue
        Else
            If ComboAlias.SelectedValue <> 0 Then
                Return ComboAlias.SelectedValue
            End If
        End If

    End Function
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If ComboNegocio.SelectedValue <> 0 Then
            MsgBox("Opción solo valida para Proveedor Pertenecientes al Grupo.") : Exit Sub
        End If
        If HallaProveedor() = 0 Then
            MsgBox("Debe Informar Proveedor o Alias.") : Exit Sub
        End If
        If Not EsProveedorDelGupo(HallaProveedor()) Then
            MsgBox("Opción solo valida para Proveedor Pertenecientes al Grupo.") : Exit Sub
        End If

        PDtImportacion = New DataTable
        PRemitoImportacion = 0

        OpcionImportacion.ShowDialog()
        If OpcionImportacion.PRemito <> 0 Then
            PRemitoImportacion = OpcionImportacion.PRemito
            PDtImportacion = OpcionImportacion.PDtImportacion
            PAbiertoImportacion = OpcionImportacion.PAbierto
        End If

        OpcionImportacion.Dispose()

    End Sub
    Private Sub ButtonImportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportarExcel.Click

        If ComboNegocio.SelectedValue <> 0 Then
            MsgBox("Opción solo valida para Proveedor Pertenecientes al Grupo.") : Exit Sub
        End If
        If HallaProveedor() = 0 Then
            MsgBox("Debe Informar Proveedor o Alias.") : Exit Sub
        End If
        If Not EsProveedorDelGupo(HallaProveedor()) Then
            MsgBox("Opción solo valida para Proveedor Pertenecientes al Grupo.") : Exit Sub
        End If

        PDtImportacion = New DataTable
        PRemitoImportacion = 0

        OpcionImportacionExcel.PProveedor = HallaProveedor()
        OpcionImportacionExcel.ShowDialog()
        If OpcionImportacionExcel.PRemito <> 0 Then
            PRemitoImportacion = OpcionImportacionExcel.PRemito
            PDtImportacion = OpcionImportacionExcel.PDtImportacion
            PAbiertoImportacion = OpcionImportacionExcel.PAbierto
            PFechaRemito = OpcionImportacionExcel.PFechaRemito
        End If

        OpcionImportacionExcel.Dispose()

    End Sub
    Private Sub ComboEmisor_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboEmisor.KeyUp

        If e.KeyValue = 13 Then ComboEmisor_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboAlias_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboAlias.KeyUp

        If e.KeyValue = 13 Then ComboAlias_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboEmisor_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboEmisor.SelectionChangeCommitted

        ComboEmisor_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboAlias_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboAlias.SelectionChangeCommitted

        Comboalias_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0 : Exit Sub

        If ComboEmisor.SelectedValue <> 0 And ComboEmisor.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboEmisor.SelectedValue = 0
            Exit Sub
        End If

        ComboAlias.SelectedValue = 0

        If ComboEmisor.SelectedValue = ProveedorW Then Exit Sub

        Grid.DataSource = Nothing

        If PConOrdenCompra Then ButtonOrdenCompra_Click(Nothing, Nothing)

        If PSoloFrutas Then
            VerSucursales(ComboEmisor.SelectedValue)
            If EsProveedorDelGupo(ComboEmisor.SelectedValue) Then
                ButtonImportar.Visible = True
                ButtonImportarExcel.Visible = True
            Else
                ButtonImportar.Visible = False
                ButtonImportarExcel.Visible = False
            End If

        End If

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0 : Exit Sub

        If ComboAlias.SelectedValue <> 0 And ComboAlias.SelectedValue = GProveedorEgresoCaja Then
            MsgBox("Proveedor Egreso de Caja Inválida pera este Comprobante.", MsgBoxStyle.Information)
            ComboAlias.SelectedValue = 0
            Exit Sub
        End If

        ComboEmisor.SelectedValue = 0

        If ComboAlias.SelectedValue = ProveedorW Then Exit Sub

        Grid.DataSource = Nothing

        If PConOrdenCompra Then ButtonOrdenCompra_Click(Nothing, Nothing)

        If PSoloFrutas Then
            VerSucursales(ComboAlias.SelectedValue)
            If EsProveedorDelGupo(ComboAlias.SelectedValue) Then
                ButtonImportar.Visible = True
                ButtonImportarExcel.Visible = True
            Else
                ButtonImportar.Visible = False
                ButtonImportarExcel.Visible = False
            End If
        End If

    End Sub
    Private Sub ComboNegocio_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboNegocio.SelectionChangeCommitted

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

        ComboSucursales.DataSource = Nothing

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

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

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 And ComboNegocio.SelectedValue = 0 Then
            MsgBox("Falta Proveedor o Negocio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If (ComboEmisor.SelectedValue <> 0 Or ComboAlias.SelectedValue <> 0) And ComboNegocio.SelectedValue <> 0 Then
            MsgBox("Debe Elegir Proveedor o Negocio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If

        If Not PArticulosLogisticos Then
            Dim TipoOperacion As Integer = HallaTipoOperacion(ComboEmisor.SelectedValue)
            If TipoOperacion < 0 Then
                MsgBox("Error de Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
                ComboEmisor.Focus()
                Return False
            End If
            If TipoOperacion = 4 And Not PAbierto Then
                MsgBox("Candado Cerrado no Valido para Unidad de Negocio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                PictureCandado.Focus()
                Return False
            End If
        End If

        If PRemitoImportacion <> 0 Then
            If PAbierto <> PAbiertoImportacion Then
                MsgBox("Candado para Ingreso No Coincide con Candado de la Importación.")
                Return False
            End If
        End If

        Dim Con As Integer = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then Con = Con + 1
        Next
        If Con > 1 Then
            MsgBox("Solo debe elegir una Orden de Compra.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If PSoloInsumos And PConOrdenCompra Then
            If Grid.Rows.Count = 0 Then
                MsgBox("No Existe Ordenes de Comupra Pendientes para Insumos.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Con = 0 Then
                MsgBox("Debe elegir una Orden de Compra.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        If PEsSoloAltas And ComboNegocio.SelectedValue = 0 Then
            Dim Emisor As Integer
            If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then Emisor = ComboAlias.SelectedValue
            '
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.Select("Clave = " & Emisor)
            If RowsBusqueda(0).Item("Estado") = 3 Then
                MsgBox("Proveedor Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Proveedores--> Proveedor --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If PSoloFrutas And Not PConOrdenCompra Then
            If ComboSucursales.Items.Count > 1 And ComboSucursales.SelectedValue = 0 Then
                MsgBox("Debe Informar Sucursal.", MsgBoxStyle.Critical)
                ComboSucursales.Focus()
                Return False
            End If
            If HallaProveedor() <> 0 Then
                Plista = HallaListaPrecioProveedor(HallaProveedor, ComboSucursales.SelectedValue, FechaIngreso.Value, PPorUnidadEnLista, PFinalEnLista, Zona)
                If Plista = -1 Then
                    Return False
                End If
            End If
        End If

        Return True

    End Function
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Orden" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "00000000")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

    End Sub
    Private Sub ButtonOrdenCompra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOrdenCompra.Click

        Dim Dt As New DataTable
        Dim Proveedor As Integer

        Dim SqlTipo As String = ""
        If PSoloFrutas Then
            SqlTipo = " AND Tipo = 1 "
        End If
        If PSoloInsumos Then
            SqlTipo = " AND Tipo = 2 "
        End If

        Proveedor = HallaProveedor()

        If Proveedor = ProveedorW Then Exit Sub
        ProveedorW = Proveedor   'guarda proveedor por si aprienta dos veces 'VER ORDEN COMPRA'

        Dim SqlB As String = "SELECT Orden,Fecha FROM OrdenCompraCabeza WHERE Estado = 1 " & SqlTipo & "AND Proveedor = " & Proveedor & " ORDER BY Orden;"

        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub

        For Each Row As DataRow In Dt.Rows
            If OrdenCompraCompleta(Row("Orden")) Then Row.Delete()
        Next

        Grid.DataSource = Dt

        Dt.Dispose()

    End Sub

    
End Class