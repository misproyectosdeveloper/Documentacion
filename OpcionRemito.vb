Public Class OpcionRemito
    Public PAbierto As Boolean
    Public PCliente As Integer
    Public PDeposito As Integer
    Public PFechaEntrega As Date
    Public PPedido As Integer
    Public PPuntoDeVentaManual As Integer
    Public PEsArticulosLoteados As Boolean
    Public PEsSoloAltas As Boolean
    Public PIgualCliente As Integer
    Public PIgualDeposito As Integer
    'Para remito a partir de un ingreso.
    Public PIngreso As Integer
    Public PDepositoIngreso As Integer
    Public PAbiertoIngreso As Boolean
    'Para Por Cuenta y Orden.
    Public PPorCuentaYOrden As Integer
    Public PSucursalPorCuentaYOrden As Integer
    '-----------------------------------
    Public PSucursal As Integer
    Public PLista As Integer
    Public PPorUnidadEnLista As Boolean
    Public PFinalEnLista As Boolean
    Public PDtPedido As DataTable
    Public PRegresar As Boolean
    '
    Dim SucursalRemito As Integer
    Private Sub OpcionRecibo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisor.DataSource = New DataTable
        ComboAlias.DataSource = New DataTable
        If Not Tablas.Read("SELECT Clave,Nombre,Estado FROM Clientes WHERE TipoIva <> 4;", Conexion, ComboEmisor.DataSource) Then Me.Close() : Exit Sub
        If Not Tablas.Read("SELECT Clave,Alias  FROM Clientes WHERE Alias <> '' AND TipoIva <> 4;", Conexion, ComboAlias.DataSource) Then Me.Close() : Exit Sub
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.Rows.Add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = 0
        With ComboEmisor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

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

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        If ComboDeposito.Items.Count = 2 Then ComboDeposito.SelectedIndex = 0

        ComboPuntoDeVenta.DataSource = New DataTable
        ComboPuntoDeVenta.DataSource = Tablas.Leer("SELECT Clave,'' AS Nombre FROM PuntosDeVenta WHERE EsRemitoManual = 1;")
        Row = ComboPuntoDeVenta.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboPuntoDeVenta.DataSource.Rows.Add(Row)
        For Each Row In ComboPuntoDeVenta.DataSource.rows
            Row("Nombre") = Format(Row("Clave"), "0000")
        Next
        ComboPuntoDeVenta.DisplayMember = "Nombre"
        ComboPuntoDeVenta.ValueMember = "Clave"
        ComboPuntoDeVenta.SelectedValue = 0
        With ComboPuntoDeVenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If
        PictureCandado.Image = ImageList1.Images.Item("Abierto")
        PAbierto = True

        CreaDtPedido()

        If PIngreso <> 0 Then
            FechaEntrega.Value = HallaFechaIngreso(PIngreso, PAbiertoIngreso)
            ComboDeposito.SelectedValue = PDepositoIngreso
            ComboDeposito.Enabled = False
            Panel2.Visible = False
        Else
            If PIgualCliente <> 0 Then ComboEmisor.SelectedValue = PIgualCliente
            If PIgualDeposito <> 0 Then ComboDeposito.SelectedValue = PIgualDeposito
        End If

        Select Case GCuitEmpresa
            Case "33-70893431-9", "33-71217222-9"
            Case Else
                '  CheckArticulosLoteados.Enabled = False
        End Select

        PCliente = 0
        PPedido = 0
        PSucursal = 0
        PLista = 0
        PPuntoDeVentaManual = 0
        PPorCuentaYOrden = 0
        PSucursalPorCuentaYOrden = 0

        PRegresar = True

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ComboEmisor.SelectedValue <> 0 Then PCliente = ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then PCliente = ComboAlias.SelectedValue

        PDeposito = ComboDeposito.SelectedValue
        PFechaEntrega = FechaEntrega.Value
        PEsArticulosLoteados = CheckArticulosLoteados.Checked
        If ComboSucursales.SelectedValue <> 0 Then
            PSucursal = ComboSucursales.SelectedValue
        End If

        If CheckRemitoManual.Checked Then
            PPuntoDeVentaManual = ComboPuntoDeVenta.SelectedValue
        Else
            PPuntoDeVentaManual = 0
        End If

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ButtonPorCuentaYOrden_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPorCuentaYOrden.Click

        OpcionClientes.PFechaEntrega = FechaEntrega.Value
        OpcionClientes.ShowDialog()
        If OpcionClientes.PRegresar Then OpcionClientes.Dispose() : Exit Sub

        TextPorCuentaYOrden.Text = OpcionClientes.PNombre
        TextSucursalPorCuentaYOrden.Text = OpcionClientes.PNombreSucursal
        PPorCuentaYOrden = OpcionClientes.PEmisor
        PSucursalPorCuentaYOrden = OpcionClientes.PSucursal

        OpcionClientes.Dispose()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue <> 0 Then Exit Sub
        If ComboEmisor.SelectedValue <> 0 Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

        CheckPedidos.Checked = False
        PPedido = 0
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        If ComboAlias.SelectedValue = 0 And ComboEmisor.SelectedValue <> 0 Then Exit Sub
        If ComboAlias.SelectedValue <> 0 Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

        CheckPedidos.Checked = False
        PPedido = 0
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboSucursales_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboSucursales.SelectionChangeCommitted

        ComboSucursales_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

        CheckPedidos.Checked = False
        PPedido = 0
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub ComboPuntoDeVenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboPuntoDeVenta.Validating

        If IsNothing(ComboPuntoDeVenta.SelectedValue) Then ComboPuntoDeVenta.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub CheckPedidos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckPedidos.Click

        If CheckPedidos.Checked = True Then
            PCliente = ObtieneCliente()
            If PCliente = 0 Then
                MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckPedidos.Checked = False
                Exit Sub
            End If
            If ComboDeposito.SelectedValue = 0 Then
                MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckPedidos.Checked = False
                Exit Sub
            End If
            ButtonVerPedido.Visible = True
            CheckArticulosLoteados.Checked = False
        Else
            ButtonVerPedido.Visible = False
            PPedido = 0 : PSucursal = 0 : PDtPedido.Clear()
        End If

    End Sub
    Private Sub ButtonVerPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerPedido.Click

        PCliente = ObtieneCliente()

        SeleccionaArticulosPedido.PEsRemito = True
        SeleccionaArticulosPedido.PCliente = PCliente
        SeleccionaArticulosPedido.PSucursal = ComboSucursales.SelectedValue
        SeleccionaArticulosPedido.PPorCuentaYOrden = PPorCuentaYOrden
        SeleccionaArticulosPedido.PSucursalPorCuentaYOrden = PSucursalPorCuentaYOrden
        SeleccionaArticulosPedido.PFechaEntrega = FechaEntrega.Value
        SeleccionaArticulosPedido.PDtPedido = PDtPedido
        SeleccionaArticulosPedido.PPedido = PPedido
        SeleccionaArticulosPedido.ShowDialog()
        PPedido = SeleccionaArticulosPedido.PPedido
        PSucursal = SeleccionaArticulosPedido.PSucursal
        SeleccionaArticulosPedido.Dispose()

    End Sub
    Private Sub CheckArticulosLoteados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckArticulosLoteados.Click

        If Not CheckArticulosLoteados.Checked Then
            CheckArticulosLoteados.Checked = False
            Exit Sub
        End If

        CheckPedidos.Checked = False
        PPedido = 0
        PDtPedido.Clear()

    End Sub
    Private Sub CheckRemitoManual_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckRemitoManual.Click

        If Not CheckRemitoManual.Checked Then
            ComboPuntoDeVenta.SelectedValue = 0
        End If

    End Sub
    Private Sub VerSucursales(ByVal Emisor As Integer)

        If Emisor = 0 Then
            ComboSucursales.DataSource = Nothing
            Exit Sub
        End If

        Dim Sql As String

        Sql = "SELECT Clave,Nombre FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & Emisor & ";"
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
    Private Sub CreaDtPedido()

        PDtPedido = New DataTable

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        PDtPedido.Columns.Add(Articulo)

    End Sub
    Private Function ObtieneCliente() As Integer

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then Return 0
        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then Return 0

        If ComboEmisor.SelectedValue <> 0 Then Return ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Return ComboAlias.SelectedValue

    End Function
    Private Function HallaFechaIngreso(ByVal Ingreso As Integer, ByVal AbiertoIngreso As Boolean) As Date

        Dim ConexionStr As String
        If AbiertoIngreso Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT Fecha FROM IngresoMercaderiasCabeza WHERE Lote = " & Ingreso & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If
        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Informar Nombre o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If DiferenciaDias(Date.Now, FechaEntrega.Value) < 0 And Not CheckRemitoManual.Checked And PIngreso = 0 Then
            '     MsgBox("Fecha Entrega Debe ser Mayor o Igual a Fecha Actual.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            '     Return False
        End If

        If Not HallaOperacionCliente(ObtieneCliente) And PAbierto Then
            If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        If PPorCuentaYOrden <> 0 And ComboEmisor.SelectedValue = PPorCuentaYOrden Then
            MsgBox("Cliente No debe ser Igual a Cliente Por Cuenta y Orden.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PPedido <> 0 And ComboSucursales.SelectedValue <> 0 Then
            If ComboSucursales.SelectedValue <> PSucursal Then
                '    MsgBox("Sucursal No Coincide con Sucursal del Pedido.", MsgBoxStyle.Critical)
                '   Return False
            End If
        End If

        If CheckPedidos.Checked Then
            If PDtPedido.Rows.Count = 0 Or PPedido = 0 Then
                MsgBox("Falta Ingresar Pedido.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If PIngreso <> 0 Then
            If PAbierto <> PAbiertoIngreso Then
                MsgBox("Candado del Remito Distinto al correspondiente al Ingreso.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        'Halla lista de precios del cliente o del cliente por cta. y orden.  
        Dim ClienteW As Integer = ObtieneCliente()
        Dim SucursalW As Integer = ComboSucursales.SelectedValue
        If PPorCuentaYOrden <> 0 Then
            ClienteW = PPorCuentaYOrden
            SucursalW = PSucursalPorCuentaYOrden
        End If
        PLista = HallaListaPrecios(ClienteW, SucursalW, FechaEntrega.Value, PPorUnidadEnLista, PFinalEnLista)
        If PLista = -1 Then Return False
        '------------------------------------------------------------------

        If PIngreso <> 0 And PLista > 0 Then
            Dim Dt As New DataTable
            Dim ConexionIngreso As String = ""
            If PAbiertoIngreso Then
                ConexionIngreso = Conexion
            Else : ConexionIngreso = ConexionN
            End If
            If Not Tablas.Read("SELECT Articulo FROM Lotes WHERE Secuencia < 100 AND  Lote = " & PIngreso & ";", ConexionIngreso, Dt) Then End
            For Each Row As DataRow In Dt.Rows
                If Not ArticuloEstaEnLista(PLista, Row("Articulo")) Then
                    MsgBox("Articulo  " & NombreArticulo(Row("Articulo")) & " No esta definido en Lista " & PLista, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dt.Dispose()
                    Return False
                End If
            Next
            Dt.Dispose()
        End If

        If PPedido <> 0 And PLista > 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Articulo FROM PedidosDetalle WHERE Pedido = " & PPedido & ";", Conexion, Dt) Then End
            For Each Row As DataRow In Dt.Rows
                If Not ArticuloEstaEnLista(PLista, Row("Articulo")) Then
                    MsgBox("Articulo  " & NombreArticulo(Row("Articulo")) & " No esta definido en Lista " & PLista, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dt.Dispose()
                    Return False
                End If
            Next
            Dt.Dispose()
        End If

        If CheckRemitoManual.Checked Then
            If ComboPuntoDeVenta.SelectedValue = 0 Then
                MsgBox("Falta Ingresar Punto de Venta para Remito Manual.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If PEsSoloAltas Then
            Dim Emisor As Integer
            If ComboEmisor.SelectedValue <> 0 Then Emisor = ComboEmisor.SelectedValue
            If ComboAlias.SelectedValue <> 0 Then Emisor = ComboAlias.SelectedValue
            '
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.Select("Clave = " & Emisor)
            If RowsBusqueda(0).Item("Estado") = 3 Then
                MsgBox("Cliente Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu --> Lista Clientes--> Cliente --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function
    Private Function HallaOperacionCliente(ByVal Cliente As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Opr FROM Clientes WHERE Clave =  " & Cliente & ";", Miconexion)
                    If Cmd.ExecuteScalar = 0 Then
                        Return False
                    Else
                        Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: Clientes.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : End
            End
        End Try

    End Function

   
    
    
End Class