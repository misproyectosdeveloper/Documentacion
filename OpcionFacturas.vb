Public Class OpcionFacturas
    Public PCliente As Integer
    Public PDeposito As Integer
    Public PRemito As Double
    Public PPedido As Integer
    Public PSucursal As Integer
    Public PFechaEntrega As Date
    Public PEsSoloExportacion As Double
    Public PEsSoloAltas As Boolean
    Public PAbiertoRemito As Boolean
    Public PEstadoRemito As Integer
    Public PEsServicios As Boolean
    Public PEsSecos As Boolean
    Public PListaRemitos As List(Of ItemRemito)
    Public PDtPedido As DataTable
    Public PLista As Integer
    Public PPorUnidadEnLista As Boolean
    Public PFinalEnLista As Boolean
    Public PRegresar As Boolean
    Public PEsContable As Boolean
    Public PPuedeTenerFCE As Boolean
    Public PEsArticulosLoteados As Boolean
    Public PEsZ As Boolean
    Public PPuntoDeVenta As Integer
    Public PDesde As Integer
    Public PHasta As Integer
    Public PEsTicketBCAM As Boolean
    Public PEsTicket As Boolean
    Public PIgualCliente As Integer
    Public PIgualDeposito As Integer
    'Para FCE.
    Public PEsFCE As Boolean
    Public PFacturaFCE As Decimal
    Public PEsAnulacion As Boolean
    Public PCBU As String
    Public PMinimo As Decimal
    Public POPcionAgente As String
    Public PFechaPago As Date
    '
    Dim TieneListaPrecios As Boolean
    Dim Dt As New DataTable
    Private Sub OpcionFacturas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboEmisor.DataSource = New DataTable
        ComboAlias.DataSource = New DataTable
        If Not PEsSoloExportacion Then
            If Not Tablas.Read("SELECT Clave,Nombre,Estado,EsFCE FROM Clientes WHERE DeOperacion = 0 AND TipoIva <> 4 ORDER BY Nombre;", Conexion, ComboEmisor.DataSource) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT Clave,Alias,EsFCE FROM Clientes WHERE DeOperacion = 0 AND Alias <> '' AND TipoIva <> 4 ORDER BY Alias;", Conexion, ComboAlias.DataSource) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read("SELECT Clave,Nombre,Estado,EsFCE FROM Clientes WHERE DeOperacion = 0 AND TipoIva = 4 ORDER BY Nombre;", Conexion, ComboEmisor.DataSource) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT Clave,Alias,EsFCE FROM Clientes WHERE DeOperacion = 0 AND Alias <> '' AND TipoIva = 4 ORDER BY Alias;", Conexion, ComboAlias.DataSource) Then Me.Close() : Exit Sub
        End If
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.Rows.Add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = PIgualCliente
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
        ComboDeposito.SelectedValue = PIgualDeposito
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        If ComboDeposito.Items.Count = 2 Then ComboDeposito.SelectedIndex = 0

        ComboPuntosDeVentaZ.DataSource = Tablas.Leer("SELECT Clave,RIGHT('0000' + CAST(Clave AS varchar),4) as Nombre FROM PuntosDeVenta WHERE Clave > 0 AND EsZ = 1 ORDER BY Nombre;")
        Row = ComboPuntosDeVentaZ.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboPuntosDeVentaZ.DataSource.Rows.Add(Row)
        ComboPuntosDeVentaZ.DisplayMember = "Nombre"
        ComboPuntosDeVentaZ.ValueMember = "Clave"
        ComboPuntosDeVentaZ.SelectedValue = 0
        With ComboPuntosDeVentaZ
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        Select Case GCuitEmpresa
            Case "33-70893431-9", "33-71217222-9"
            Case Else
                ''    CheckArticulosLoteados.Enabled = False
        End Select

        If PEsSoloExportacion Then
            Panel1.Visible = False
        End If

        If PEsContable Then
            CheckRemitos.Visible = False
            CheckPedidos.Visible = False
            CheckRemitos.Visible = False
        End If

        CreaDtPedido()

        PRemito = 0
        PCliente = 0
        PPedido = 0
        PRegresar = True
        PLista = 0
        'Para Tickets.
        PPuntoDeVenta = 0
        PDesde = 0
        PHasta = 0
        PEsTicketBCAM = False
        PEsTicket = False

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        'Si es FCE........................................................
        If PPuedeTenerFCE Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboEmisor.DataSource.Select("Clave = " & ObtieneCliente())
            If RowsBusqueda(0).Item("EsFCE") Then
                UnCompobanteFCE.PEsFactura = True
                UnCompobanteFCE.PCliente = ObtieneCliente()
                UnCompobanteFCE.ShowDialog()
                If UnCompobanteFCE.PRegresar Then
                    PRegresar = True
                    UnCompobanteFCE.Dispose()
                    Me.Close() : Exit Sub
                End If
                If UnCompobanteFCE.PEsFCE = True Then
                    PCBU = UnCompobanteFCE.PCBU
                    PMinimo = UnCompobanteFCE.PMinimo
                    POPcionAgente = UnCompobanteFCE.POpcionAgente
                    PEsFCE = True
                End If
                UnCompobanteFCE.Dispose()
            End If
        End If
        '-----------------------------------------------------------------

        If CheckRemitos.Checked Then
            PAbiertoRemito = PListaRemitos.Item(0).Abierto
            PEstadoRemito = PListaRemitos.Item(0).Estado
        End If

        PCliente = ObtieneCliente()
        PDeposito = ComboDeposito.SelectedValue
        PEsServicios = CheckBoxEsServicios.Checked
        PEsSecos = CheckBoxEsSecos.Checked
        PEsArticulosLoteados = CheckArticulosLoteados.Checked
        PEsZ = CheckFacturaZ.Checked
        PFechaEntrega = FechaEntrega.Value
        If ComboSucursales.SelectedValue <> 0 Then
            PSucursal = ComboSucursales.SelectedValue
        End If
        If CheckFacturaZ.Checked Then
            PPuntoDeVenta = ComboPuntosDeVentaZ.SelectedValue
            PDesde = TextDesde.Text
            PHasta = TextHasta.Text
            PEsTicket = RadioTicket.Checked
            PEsTicketBCAM = RadioTicketBCAM.Checked
        End If

        PRegresar = False

        Me.Close()

    End Sub
    Private Sub ComboEmisor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmisor.Validating

        If IsNothing(ComboEmisor.SelectedValue) Then ComboEmisor.SelectedValue = 0

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue <> 0 Then Exit Sub
        If ComboEmisor.SelectedValue <> 0 Then ComboAlias.SelectedValue = 0

        VerSucursales(ComboEmisor.SelectedValue)

        Dt.Clear()
        CheckRemitos.Checked = False
        PListaRemitos.Clear()
        CheckPedidos.Checked = False
        PDtPedido.Clear()
        PPedido = 0
        ButtonVerRemitos.Visible = False
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub ComboAlias_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboAlias.Validating

        If IsNothing(ComboAlias.SelectedValue) Then ComboAlias.SelectedValue = 0

        If ComboAlias.SelectedValue = 0 And ComboEmisor.SelectedValue <> 0 Then Exit Sub
        If ComboAlias.SelectedValue <> 0 Then ComboEmisor.SelectedValue = 0

        VerSucursales(ComboAlias.SelectedValue)

        Dt.Clear()
        CheckRemitos.Checked = False
        PListaRemitos.Clear()
        CheckPedidos.Checked = False
        PDtPedido.Clear()
        PPedido = 0
        ButtonVerRemitos.Visible = False
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub ComboSucursale_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboSucursales.SelectionChangeCommitted

        ComboSucursales_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboSucursales_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursales.Validating

        If IsNothing(ComboSucursales.SelectedValue) Then ComboSucursales.SelectedValue = 0

        Dt.Clear()
        CheckRemitos.Checked = False
        PListaRemitos.Clear()
        CheckPedidos.Checked = False
        PDtPedido.Clear()
        PPedido = 0
        ButtonVerRemitos.Visible = False
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub TextUltimoTicket_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextUltimoTicket.KeyPress

        EsNumerico(e.KeyChar, TextUltimoTicket.Text, 0)

    End Sub
    Private Sub TextUltimoTicket_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextUltimoTicket.Validating

        CalculaDesdeHastaTicket()

    End Sub
    Private Sub TextCantidadTicket_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCantidadTicket.KeyPress

        EsNumerico(e.KeyChar, TextCantidadTicket.Text, 0)

    End Sub
    Private Sub TextCantidadTicket_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCantidadTicket.Validating

        CalculaDesdeHastaTicket()

    End Sub
    Private Sub ComboDeposito_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboDeposito.SelectionChangeCommitted

        ComboDeposito_Validating(Nothing, Nothing)

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

        Dt.Clear()
        CheckRemitos.Checked = False
        PListaRemitos.Clear()
        CheckPedidos.Checked = False
        PDtPedido.Clear()
        PPedido = 0
        ButtonVerRemitos.Visible = False
        ButtonVerPedido.Visible = False

    End Sub
    Private Sub CheckRemitos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckRemitos.Click

        If CheckRemitos.Checked = True Then
            CheckPedidos.Checked = False
            CheckBoxEsSecos.Checked = False
            CheckBoxEsServicios.Checked = False
            CheckArticulosLoteados.Checked = False
            PDtPedido.Clear()
            PPedido = 0
            PCliente = ObtieneCliente()
            If PCliente = 0 Then
                MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckRemitos.Checked = False
                Exit Sub
            End If
            If ComboDeposito.SelectedValue = 0 Then
                MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckRemitos.Checked = False
                Exit Sub
            End If
            ButtonVerRemitos.Visible = True
            ButtonVerPedido.Visible = False
        Else : ButtonVerRemitos.Visible = False
            PListaRemitos.Clear()
        End If

    End Sub
    Private Sub ButtonVerRemitos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerRemitos.Click

        PCliente = ObtieneCliente()

        SeleccionarRemitosParaFactura.PCliente = PCliente
        SeleccionarRemitosParaFactura.PDeposito = ComboDeposito.SelectedValue
        SeleccionarRemitosParaFactura.PSucursal = ComboSucursales.SelectedValue
        SeleccionarRemitosParaFactura.PListaRemitos = PListaRemitos
        SeleccionarRemitosParaFactura.ShowDialog()
        SeleccionarRemitosParaFactura.Dispose()

    End Sub
    Private Sub CheckPedidos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckPedidos.Click

        If CheckPedidos.Checked = True Then
            CheckBoxEsSecos.Checked = False
            CheckBoxEsServicios.Checked = False
            CheckRemitos.Checked = False
            CheckArticulosLoteados.Checked = False
            CheckFacturaZ.Checked = False
            CheckFacturasZ_Click(Nothing, Nothing)
            Panel3.Visible = False
            PListaRemitos.Clear()
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
            ButtonVerRemitos.Visible = False
        Else
            ButtonVerPedido.Visible = False
            PPedido = 0 : PDtPedido.Clear() : PSucursal = 0
        End If

    End Sub
    Private Sub ButtonVerPedido_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerPedido.Click

        PCliente = ObtieneCliente()

        SeleccionaArticulosPedido.PEsFactura = True
        SeleccionaArticulosPedido.PCliente = PCliente
        SeleccionaArticulosPedido.PSucursal = ComboSucursales.SelectedValue
        SeleccionaArticulosPedido.PFechaEntrega = FechaEntrega.Value
        SeleccionaArticulosPedido.PDtPedido = PDtPedido
        SeleccionaArticulosPedido.PPedido = PPedido
        SeleccionaArticulosPedido.ShowDialog()
        PPedido = SeleccionaArticulosPedido.PPedido
        PSucursal = SeleccionaArticulosPedido.PSucursal
        SeleccionaArticulosPedido.Dispose()

    End Sub
    Private Sub CheckBoxEsSecos_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxEsSecos.Click

        CheckBoxEsServicios.Checked = False
        CheckRemitos.Checked = False
        CheckPedidos.Checked = False
        PPedido = 0
        PDtPedido.Clear()
        PListaRemitos.Clear()

    End Sub
    Private Sub CheckBoxEsServicios_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBoxEsServicios.Click

        CheckBoxEsSecos.Checked = False
        CheckRemitos.Checked = False
        CheckPedidos.Checked = False
        CheckArticulosLoteados.Checked = False
        PPedido = 0
        PDtPedido.Clear()
        PListaRemitos.Clear()

    End Sub
    Private Sub CheckArticulosLoteados_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckArticulosLoteados.Click

        If Not CheckArticulosLoteados.Checked Then
            CheckArticulosLoteados.Checked = False
            Exit Sub
        End If

        CheckBoxEsSecos.Checked = False
        CheckRemitos.Checked = False
        CheckPedidos.Checked = False
        CheckBoxEsServicios.Checked = False
        PPedido = 0
        PDtPedido.Clear()
        PListaRemitos.Clear()

    End Sub
    Private Sub CheckFacturasZ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckFacturaZ.Click

        If CheckFacturaZ.Checked = True Then
            CheckPedidos.Checked = False
            PPedido = 0
            PDtPedido.Clear()
            Panel3.Visible = True
        Else
            ComboPuntosDeVentaZ.SelectedValue = 0
            TextDesde.Text = ""
            TextHasta.Text = ""
            RadioTicket.Checked = False
            RadioTicketBCAM.Checked = False
            Panel3.Visible = False
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
    Private Function HallaNombreProveedor(ByVal Ingreso As Integer, ByVal Operacion As Integer) As String

        Dim StrConexion As String

        If Operacion = 1 Then
            StrConexion = Conexion
        Else : StrConexion = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(StrConexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Proveedor FROM IngresoMercaderiasCabeza WHERE Lote = " & Ingreso & ";", Miconexion)
                    Return NombreProveedor(Cmd.ExecuteScalar)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function ObtieneCliente() As Integer

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then Return 0
        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then Return 0

        If ComboEmisor.SelectedValue <> 0 Then Return ComboEmisor.SelectedValue
        If ComboAlias.SelectedValue <> 0 Then Return ComboAlias.SelectedValue

    End Function
    Private Function ControlaParametrosListaDePrecios(ByVal TipoListaDePrecios As Integer) As Boolean

        Dim Lista As Integer
        Dim Primero As Boolean = True
        Dim FinalEnLista As Boolean
        Dim PorUnidadEnLista As Boolean
        Dim FinalEnListaW As Boolean
        Dim PorUnidadEnListaW As Boolean
        Dim ConexionStr As String
        Dim Sucursal As Integer

        Dim DtRemitosCabeza As DataTable
        Dim DtRemitosDetalle As DataTable

        For Each Item As ItemRemito In PListaRemitos
            If Item.Abierto Then
                ConexionStr = Conexion
            Else : ConexionStr = ConexionN
            End If
            DtRemitosCabeza = New DataTable
            If Not Tablas.Read("SELECT FechaEntrega,Sucursal,PorCuentaYOrden,SucursalPorCuentaYOrden FROM RemitosCabeza WHERE Remito = " & Item.Remito & ";", ConexionStr, DtRemitosCabeza) Then Return False
            If DtRemitosCabeza.Rows(0).Item("PorCuentaYOrden") <> 0 Then
                Sucursal = DtRemitosCabeza.Rows(0).Item("SucursalPorCuentaYOrden")
            Else
                Sucursal = DtRemitosCabeza.Rows(0).Item("Sucursal")
            End If
            Lista = HallaListaPrecios(PCliente, Sucursal, DtRemitosCabeza.Rows(0).Item("FechaEntrega"), PorUnidadEnListaW, FinalEnListaW)
            If Lista = -1 Then Return False
            If Lista <= 0 Then
                MsgBox("Falta Lista de Precios Para la Fecha del Remito " & NumeroEditado(Item.Remito) & " ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Primero Then
                FinalEnLista = FinalEnListaW
                PorUnidadEnLista = PorUnidadEnListaW
                Primero = False
            Else
                If PorUnidadEnListaW <> PorUnidadEnLista Or FinalEnLista <> FinalEnLista Then
                    MsgBox("Final-En-Lista o Por-Unidad en Tipo de Lista " & Lista & " en Remito " & NumeroEditado(Item.Remito) & " No Coinciden.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
            DtRemitosDetalle = New DataTable
            If Not Tablas.Read("SELECT Articulo FROM RemitosDetalle WHERE Remito = " & Item.Remito & ";", ConexionStr, DtRemitosDetalle) Then Return False
            For Each Row1 As DataRow In DtRemitosDetalle.Rows
                If Not ArticuloEstaEnLista(Lista, Row1("Articulo")) Then
                    MsgBox("Articulo  " & NombreArticulo(Row1("Articulo")) & " No esta definido en Lista " & Lista & " en Remito " & NumeroEditado(Item.Remito), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            Next
            DtRemitosCabeza.Dispose()
            DtRemitosDetalle.Dispose()
        Next

        Return True

    End Function
    Private Sub CreaDtPedido()

        PDtPedido = New DataTable

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        PDtPedido.Columns.Add(Articulo)

    End Sub
    Private Sub CalculaDesdeHastaTicket()

        Dim Ultimo As Decimal
        Dim Cantidad As Decimal

        If TextUltimoTicket.Text = "" Then
            Ultimo = 0
        Else
            Ultimo = CDec(TextUltimoTicket.Text)
        End If

        If TextCantidadTicket.Text = "" Then
            Cantidad = 0
        Else
            Cantidad = CDec(TextCantidadTicket.Text)
        End If

        TextDesde.Text = "" : TextHasta.Text = ""

        If Ultimo <> 0 And Cantidad <> 0 Then
            TextHasta.Text = Ultimo
            TextDesde.Text = Ultimo - Cantidad + 1
        End If

    End Sub
    Private Function Valida() As Boolean

        If ComboEmisor.SelectedValue = 0 And ComboAlias.SelectedValue = 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If
        If ComboEmisor.SelectedValue <> 0 And ComboAlias.SelectedValue <> 0 Then
            MsgBox("Debe Selecciionar Cliente o Alias.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboEmisor.Focus()
            Return False
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If DiferenciaDias(Date.Now, FechaEntrega.Value) < 0 Then
            '   MsgBox("Fecha Entrega Debe ser Mayor o Igual a Fecha Actual.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            '   Return False
        End If

        If PPedido <> 0 And ComboSucursales.SelectedValue <> 0 Then
            If ComboSucursales.SelectedValue <> PSucursal Then
                '    MsgBox("Sucursal Informada No Coincide con Sucursal del Pedido.", MsgBoxStyle.Critical)
                '    Return False
            End If
        End If

        Dim Cliente As Integer = ObtieneCliente()

        If Not CheckBoxEsServicios.Checked And Not CheckBoxEsSecos.Checked And Not CheckRemitos.Checked Then
            PLista = HallaListaPrecios(Cliente, ComboSucursales.SelectedValue, FechaEntrega.Value, PPorUnidadEnLista, PFinalEnLista)
            If PLista = -1 Then Return False
        End If
        If CheckBoxEsServicios.Checked Or CheckBoxEsSecos.Checked Or CheckRemitos.Checked Then
            PLista = 0
        End If

        If CheckRemitos.Checked Then
            If PListaRemitos.Count = 0 Then
                MsgBox("Falta Ingresar Remitos.", MsgBoxStyle.Critical)
                Return False
            End If
            Dim TipoListaDePrecios As Integer
            TipoListaDePrecios = TieneListaDePreciosW(PCliente)
            If TipoListaDePrecios > 0 Then
                If Not ControlaParametrosListaDePrecios(TipoListaDePrecios) Then Return False
            End If
        End If

        If CheckPedidos.Checked Then
            If PDtPedido.Rows.Count = 0 Or PPedido = 0 Then
                MsgBox("Falta Ingresar Pedido.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        If CheckFacturaZ.Checked And ComboPuntosDeVentaZ.SelectedValue = 0 Then
            MsgBox("Falta Ingresar Punto de Venta.", MsgBoxStyle.Critical)
            Return False
        End If
        If Not CheckFacturaZ.Checked And ComboPuntosDeVentaZ.SelectedValue <> 0 Then
            MsgBox("Falta Definir si es Factura Z.", MsgBoxStyle.Critical)
            Return False
        End If
        If CheckFacturaZ.Checked Then
            If TextDesde.Text = "" Or TextHasta.Text = "" Then
                MsgBox("Falta Definir Comprobantes Deste/Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            If Val(TextDesde.Text) = 0 Or Val(TextHasta.Text) = 0 Then
                MsgBox("Falta Definir Comprobantes Deste/Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            If Val(TextDesde.Text) > Val(TextHasta.Text) Then
                MsgBox("Comprobante Desde debe ser Menor o Igual a Comprobante Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            Dim Tipo As Integer
            Dim TipoFactura As Integer = LetrasPermitidasCliente(HallaTipoIvaCliente(Cliente), 1)
            Tipo = TipoFactura
            If RadioTicket.Checked Then Tipo = 9
            Dim FacturasW As Integer = FacturasConComprobantesZ(0, Val(TextDesde.Text), Val(TextHasta.Text), ComboPuntosDeVentaZ.SelectedValue, Tipo)
            If FacturasW > 0 Then
                MsgBox("Existen Comprobante Informados dentro de Comprobante-Desde, Comprobante-Hasta.", MsgBoxStyle.Critical)
                Return False
            End If
            If FacturasW < 0 Then
                MsgBox("Error al Leer Tabla FacturasCabeza.", MsgBoxStyle.Critical)
                Return False
            End If
            If PPedido <> 0 Then
                MsgBox("No se debe Informat Pedido en Ticket Z.", MsgBoxStyle.Critical)
                Return False
            End If
            If Not RadioTicket.Checked And Not RadioTicketBCAM.Checked Then
                MsgBox("Debe Informar Tipo De Ticket.", MsgBoxStyle.Critical)
                Return False
            End If
            If Tipo = 1 Or Tipo = 5 Then
                If TextDesde.Text <> TextHasta.Text Then
                    MsgBox("Comprobante-Desde debe ser igual a Comprobante-Hasta para Ticket A o M.", MsgBoxStyle.Critical)
                    Return False
                End If
            End If
        Else
            If TextDesde.Text <> "" Or TextHasta.Text <> "" Then
                MsgBox("Comprobantes Desde y Hasta Solo Valido para Ticket Z.", MsgBoxStyle.Critical)
                Return False
            End If
            If Not (Not RadioTicket.Checked And Not RadioTicketBCAM.Checked) Then
                MsgBox("No Debe Informar Tipo De Ticket.", MsgBoxStyle.Critical)
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
                MsgBox("Cliente Esta dado de Baja. Debe cambiar estado en : " + vbCrLf + "Menu -->Clientes--> Selecciona Cliente --> Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function

    
   
End Class