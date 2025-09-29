Imports System.Transactions
Imports System.Math
Public Class UnaFacturaProveedorFondoFijo
    Public PRendicion As Integer
    Public PFactura As Double
    Public PTipoFactura As Integer
    Public PProveedor As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtFacturaCabezaB As DataTable
    Dim DtFacturaDetalleB As DataTable
    Dim DtFacturaCabezaN As DataTable
    Dim DtFacturaDetalleN As DataTable
    Dim DtRetencionProvinciaB As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    Dim DtComprobanteCabezaB As DataTable
    Dim DtComprobanteCabezaN As DataTable
    Dim DtComproFacturados As New DataTable
    Dim DtGrid As DataTable
    Dim DtGridCompro As DataTable
    '
    Dim FechaContableAnt As Date
    Dim ConexionFactura As String
    Dim TipoAsiento As Integer
    Dim UltimafechaContableW As DateTime
    Dim FondoFijo As Integer
    Dim NumeroFondoFijo As Integer
    Dim ConexionComproFacturados As String = ""
    Dim ClaveComproFacturados As Double = 0
    Private Sub UnaFacturaProveedor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not PermisoEscritura(2) Then PBloqueaFunciones = True

        Me.Top = 30

        Grid.Columns("Lupa").DefaultCellStyle.NullValue = Nothing

        If PFactura = 0 Then
            Opciones()
            If PProveedor = 0 Then Me.Close() : Exit Sub
        End If

        Grid.AutoGenerateColumns = False

        DateTime1.Value = Now.Date

        ComboEmisor.DataSource = Tablas.Leer("Select Clave,Nombre From Proveedores ORDER BY Nombre;")
        Dim Row As DataRow = ComboEmisor.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        ComboEmisor.DataSource.Rows.Add(Row)
        ComboEmisor.DisplayMember = "Nombre"
        ComboEmisor.ValueMember = "Clave"
        ComboEmisor.SelectedValue = PProveedor

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        LlenaComboTablas(ComboConceptoGasto, 29)
        ComboConceptoGasto.SelectedValue = 0
        With ComboConceptoGasto
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Row = ComboNegocio.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboNegocio.DataSource.rows.add(Row)
        ComboNegocio.DisplayMember = "Nombre"
        ComboNegocio.ValueMember = "Clave"
        ComboNegocio.SelectedValue = 0
        With ComboNegocio
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        With ComboCosteo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PAbierto Then
            ConexionFactura = Conexion
        Else
            ConexionFactura = ConexionN
        End If

        ArmaArchivos()

        If PAbierto Then
            Grid.Columns("ImporteN").ReadOnly = True
        Else
            Grid.Columns("ImporteB").ReadOnly = True
        End If

        CheckSecos.Visible = True

        GModificacionOk = False

        If Not PermisoTotal Then
            Grid.Columns("ImporteN").Visible = False
            TextTotalN.Visible = False
        End If

        UltimafechaContableW = UltimaFechacontable(Conexion, 3)
        If UltimafechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnaFacturaProveedor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(NumeroFondoFijo, PAbierto) Then
            MsgBox("Fondo Fijo Cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        If CDbl(TextTotalN.Text) <> 0 Then
            ConexionComproFacturados = ConexionN
        Else : ConexionComproFacturados = Conexion
        End If

        Dim DtFacturaCabezaBW As New DataTable
        Dim DtFacturaCabezaNW As New DataTable
        If PFactura = 0 Then
            If CDbl(TextTotalB.Text) <> 0 Then DtFacturaCabezaBW = DtFacturaCabezaB.Copy
            If CDbl(TextTotalN.Text) <> 0 Then DtFacturaCabezaNW = DtFacturaCabezaB.Copy
        Else
            DtFacturaCabezaBW = DtFacturaCabezaB.Copy
            DtFacturaCabezaNW = DtFacturaCabezaN.Copy
        End If
        Dim DtFacturaDetalleBW As DataTable = DtFacturaDetalleB.Copy
        Dim DtFacturaDetalleNW As DataTable = DtFacturaDetalleN.Copy
        Dim DtComproFacturadosW As DataTable = DtComproFacturados.Copy
        Dim DtRetencionProvinciaW As DataTable = DtRetencionProvinciaB.Copy

        If Not ActualizaArchivos(DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW) Then ArmaArchivos() : Exit Sub

        If IsNothing(DtFacturaCabezaBW.GetChanges) And IsNothing(DtFacturaCabezaNW.GetChanges) And IsNothing(DtFacturaDetalleBW.GetChanges) And _
           IsNothing(DtFacturaDetalleNW.GetChanges) And IsNothing(DtComproFacturadosW.GetChanges) And IsNothing(DtRetencionProvinciaW.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtRendicionW As New DataTable

        If PFactura = 0 Then
            If Not Tablas.Read("SELECT * FROM RendicionFondoFijo WHERE Rendicion = " & PRendicion & ";", ConexionFactura, DtRendicionW) Then
                MsgBox("Error Base de Datos en Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
            Dim Importe As Decimal
            If CDec(TextTotalB.Text) <> 0 Then
                Importe = CDec(TextTotalB.Text)
            Else : Importe = CDec(TextTotalN.Text)
            End If
            DtRendicionW.Rows(0).Item("ImporteFacturas") = CDec(DtRendicionW.Rows(0).Item("ImporteFacturas")) + Importe
            DtRendicionW.Rows(0).Item("Saldo") = CDec(DtRendicionW.Rows(0).Item("Saldo")) + Importe
            If DtRendicionW.Rows(0).Item("Importe") < DtRendicionW.Rows(0).Item("ImporteFacturas") Then
                MsgBox("Importe de la Factura Supera Importe Tope de la Rendición. Operación se CANCELA.")
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PFactura = 0 Then
            HacerAlta(DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW, DtRendicionW)
        Else
            HacerModificacion(DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW, DtRendicionW)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(NumeroFondoFijo, PAbierto) Then
            MsgBox("Fondo Fijo Cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If RendicionCerrada(PRendicion, ConexionFactura) Then
            MsgBox("Rendición Cerrada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            If DtFacturaCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("ERROR, Factura Ya esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Actualiza Rendicion.
        Dim DtRendicionW As New DataTable
        '
        Dim Importe As Decimal
        If CDbl(TextTotalB.Text) <> 0 Then
            Importe = CDec(TextTotalB.Text)
        Else
            Importe = CDec(TextTotalN.Text)
        End If
        ' 
        If Not Tablas.Read("SELECT * FROM RendicionFondoFijo WHERE Rendicion = " & PRendicion & ";", ConexionFactura, DtRendicionW) Then
            MsgBox("Error Base de Datos en Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If
        Dim Pagado As Decimal = DtRendicionW.Rows(0).Item("ImporteFacturas") - DtRendicionW.Rows(0).Item("Saldo")
        DtRendicionW.Rows(0).Item("ImporteFacturas") = CDec(DtRendicionW.Rows(0).Item("ImporteFacturas")) - Importe
        DtRendicionW.Rows(0).Item("Saldo") = CDec(DtRendicionW.Rows(0).Item("Saldo")) - Importe
        If Pagado > DtRendicionW.Rows(0).Item("ImporteFacturas") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Importe de la Reposiciones Supera Importe de las Facturas. Operación se CANCELA.")
            Exit Sub
        End If
        '

        Dim DtFacturaCabezaBW As DataTable = DtFacturaCabezaB.Copy
        Dim DtFacturaCabezaNW As DataTable = DtFacturaCabezaN.Copy
        Dim DtFacturaDetalleBW As DataTable = DtFacturaDetalleB.Copy
        Dim DtFacturaDetalleNW As DataTable = DtFacturaDetalleN.Copy
        Dim DtRetencionProvinciaW As DataTable = DtRetencionProvinciaB.Copy
        Dim DtComproFacturadosW As DataTable = DtComproFacturados.Copy

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If GGeneraAsiento Then
            If DtFacturaCabezaB.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsiento, DtFacturaCabezaB.Rows(0).Item("Factura"), DtAsientoCabezaB, Conexion) Then Me.Close() : Exit Sub
            End If
            If DtFacturaCabezaN.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsiento, DtFacturaCabezaN.Rows(0).Item("Factura"), DtAsientoCabezaN, ConexionN) Then Me.Close() : Exit Sub
            End If
            If DtAsientoCabezaB.Rows.Count <> 0 Then DtAsientoCabezaB.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaN.Rows.Count <> 0 Then DtAsientoCabezaN.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Factura se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If DtFacturaCabezaBW.Rows.Count <> 0 Then DtFacturaCabezaBW.Rows(0).Item("Estado") = 3
        If DtFacturaCabezaNW.Rows.Count <> 0 Then DtFacturaCabezaNW.Rows(0).Item("Estado") = 3

        Dim Resul As Double = ActualizaFactura(DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW, DtRendicionW)
        '
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Factura Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonLotesAImputar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonLotesAImputar.Click

        If ComboConceptoGasto.SelectedValue = 0 Then
            MsgBox("Debe Informar Concepto del Gasto.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura <> 0 Then OpcionLotesAImputarFacturasProveedor.PBloqueaFunciones = True
        OpcionLotesAImputarFacturasProveedor.PConceptoGasto = ComboConceptoGasto.SelectedValue
        OpcionLotesAImputarFacturasProveedor.PEsConceptosGasto = True
        OpcionLotesAImputarFacturasProveedor.PDtGrid = DtGridCompro
        OpcionLotesAImputarFacturasProveedor.ShowDialog()
        OpcionLotesAImputarFacturasProveedor.Dispose()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsNetoPorLotesAfectados = True
        SeleccionarVarios.PLiquidacion = PFactura
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosEmisor.Click

        If ComboEmisor.SelectedValue = 0 Then Exit Sub

        UnDatosEmisor.PEsProveedor = True

        UnDatosEmisor.PEmisor = ComboEmisor.SelectedValue
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Sub PictureAlmanaqueFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFactura.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaFactura.Text = ""
        Else : TextFechaFactura.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PFactura = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If DtFacturaCabezaB.Rows.Count <> 0 Then ListaAsientos.PDocumentoB = DtFacturaCabezaB.Rows(0).Item("Factura")
        If DtFacturaCabezaN.Rows.Count <> 0 Then ListaAsientos.PDocumentoN = DtFacturaCabezaN.Rows(0).Item("Factura")
        ListaAsientos.Show()

    End Sub
    Private Sub PictureLupaCuenta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupaCuenta.Click

        If PFactura <> 0 Then Exit Sub

        Dim ImporteNetoGrabadoInformado As Decimal
        Dim ImporteNetoNoGrabadoInformado As Decimal

        Grid.EndEdit()

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Clave = 1")
        ImporteNetoGrabadoInformado = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        ImporteNetoGrabadoInformado = ImporteNetoGrabadoInformado + RowsBusqueda(0).Item("ImporteB")
        ImporteNetoNoGrabadoInformado = RowsBusqueda(0).Item("ImporteN")

        SeleccionarCuentaDocumento.PListaDeCuentas = New List(Of ItemCuentasAsientos)

        Dim Item As New ListViewItem

        For I As Integer = 0 To ListCuentas.Items.Count - 1
            Dim Fila As New ItemCuentasAsientos
            Dim CuentaStr As String = ListCuentas.Items.Item(I).SubItems(0).Text
            Fila.Cuenta = Mid(CuentaStr, 1, 3) & Mid(CuentaStr, 5, 6) & Mid(CuentaStr, 12, 2)
            Fila.ImporteB = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
            Fila.ImporteN = CDec(ListCuentas.Items.Item(I).SubItems(2).Text)
            SeleccionarCuentaDocumento.PListaDeCuentas.Add(Fila)
        Next

        SeleccionarCuentaDocumento.PImporteB = ImporteNetoGrabadoInformado
        SeleccionarCuentaDocumento.PImporteN = ImporteNetoNoGrabadoInformado
        SeleccionarCuentaDocumento.ShowDialog()
        If SeleccionarCuentaDocumento.PAcepto Then
            ListCuentas.Clear()
            For Each Fila As ItemCuentasAsientos In SeleccionarCuentaDocumento.PListaDeCuentas
                Item = New ListViewItem(Format(Fila.Cuenta, "000-000000-00"))
                Item.SubItems.Add(Fila.ImporteB.ToString)
                Item.SubItems.Add(Fila.ImporteN.ToString)
                ListCuentas.Items.Add(Item)
            Next
        End If

        SeleccionarCuentaDocumento.Dispose()

    End Sub
    Private Sub ComboNegocio_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboNegocio.SelectionChangeCommitted

        Dim SqlFecha As String = ""
        SqlFecha = "IntFechaDesde <= " & Format(DateTime1.Value, "yyyyMMdd") & " AND IntFechaHasta >= " & Format(DateTime1.Value, "yyyyMMdd") & ";"
        Dim Sql As String = "SELECT Costeo,Nombre FROM Costeos WHERE Negocio = " & ComboNegocio.SelectedValue & " AND Cerrado = 0 AND " & SqlFecha
        ComboCosteo.DataSource = New DataTable
        ComboCosteo.DataSource = Tablas.Leer(Sql)
        Dim Row As DataRow = ComboCosteo.DataSource.newrow
        Row("Costeo") = 0
        Row("Nombre") = ""
        ComboCosteo.DataSource.rows.add(Row)
        ComboCosteo.DisplayMember = "Nombre"
        ComboCosteo.ValueMember = "Costeo"
        ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub HacerModificacion(ByVal DtFacturaCabezaBW As DataTable, ByVal DtFacturaCabezaNW As DataTable, ByVal DtFacturaDetalleBW As DataTable, ByVal DtFacturaDetalleNW As DataTable, ByVal DtComproFacturadosW As DataTable, ByVal DtRetencionProvinciaW As DataTable, ByVal DtRendicionW As DataTable)

        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If FechaContableAnt <> CDate(TextFechaContable.Text) Then
            If GGeneraAsiento Then
                If DtFacturaCabezaBW.Rows.Count <> 0 Then
                    If Not HallaAsientosCabeza(TipoAsiento, DtFacturaCabezaBW.Rows(0).Item("Factura"), DtAsientoCabezaB, Conexion) Then Me.Close() : Exit Sub
                    DtAsientoCabezaB.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
                End If
                If DtFacturaCabezaNW.Rows.Count <> 0 Then
                    If Not HallaAsientosCabeza(TipoAsiento, DtFacturaCabezaNW.Rows(0).Item("Factura"), DtAsientoCabezaN, ConexionN) Then Me.Close() : Exit Sub
                    DtAsientoCabezaN.Rows(0).Item("intFecha") = Format(CDate(TextFechaContable.Text), "yyyyMMdd")
                End If
            End If
        End If

        Dim Resul As Double = ActualizaFactura(DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW, DtRendicionW)
        '
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        ArmaArchivos()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(NumeroFondoFijo, PAbierto) Then
            MsgBox("Fondo Fijo Cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If RendicionCerrada(PRendicion, ConexionFactura) Then
            MsgBox("Rendición Cerrada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PFactura = 0
        UnaFacturaProveedor_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevaIgualProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualProveedor.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(NumeroFondoFijo, PAbierto) Then
            MsgBox("Fondo Fijo Cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If RendicionCerrada(PRendicion, ConexionFactura) Then
            MsgBox("Rendición Cerrada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PFactura = 0
        DateTime1.Value = Now.Date
        ArmaArchivos()

    End Sub
    Private Sub MaskedReciboOficial_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedReciboOficial.Validating

        If Val(MaskedReciboOficial.Text) = 0 Then Exit Sub

        If Not MaskedOK(MaskedReciboOficial) Then
            MsgBox("Factura Proveedor Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedReciboOficial.Text = "000000000000"
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub ComboConceptoGasto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboConceptoGasto.Validating

        If IsNothing(ComboConceptoGasto.SelectedValue) Then ComboConceptoGasto.SelectedValue = 0

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

    End Sub
    Private Sub ComboCosteo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCosteo.Validating

        If IsNothing(ComboCosteo.SelectedValue) Then ComboCosteo.SelectedValue = 0

    End Sub
    Private Sub ArmaArchivos()

        CreaDtGridCompro()
        CreaDtRetencionProvinciaAux()

        ArmaGrid()

        DtFacturaCabezaB = New DataTable
        DtFacturaCabezaN = New DataTable
        DtFacturaDetalleB = New DataTable
        DtFacturaDetalleN = New DataTable
        DtRetencionProvinciaB = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PAbierto Then
            If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & PFactura & ";", Conexion, DtFacturaCabezaB) Then Me.Close() : Exit Sub
            If PFactura <> 0 And DtFacturaCabezaB.Rows.Count = 0 Then
                MsgBox("Factura No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
            If PFactura <> 0 Then FechaContableAnt = DtFacturaCabezaB.Rows(0).Item("FechaContable")
            If Not Tablas.Read("SELECT * FROM FacturasProveedorDetalle WHERE Factura = " & PFactura & ";", Conexion, DtFacturaDetalleB) Then Me.Close() : Exit Sub
            If DtFacturaCabezaB.Rows.Count <> 0 Then PProveedor = DtFacturaCabezaB.Rows(0).Item("Proveedor")
            If DtFacturaCabezaB.Rows.Count <> 0 And PermisoTotal Then
                If DtFacturaCabezaB.Rows(0).Item("Rel") = True Then
                    If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Nrel = " & PFactura & ";", ConexionN, DtFacturaCabezaN) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM FacturasProveedorDetalle WHERE Factura = " & DtFacturaCabezaN.Rows(0).Item("Factura") & ";", ConexionN, DtFacturaDetalleN) Then Me.Close() : Exit Sub
                Else
                    DtFacturaCabezaN = DtFacturaCabezaB.Clone
                    DtFacturaDetalleN = DtFacturaDetalleB.Clone
                End If
            Else
                DtFacturaCabezaN = DtFacturaCabezaB.Clone
                DtFacturaDetalleN = DtFacturaDetalleB.Clone
            End If
        Else
            If PermisoTotal Then
                If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & PFactura & ";", ConexionN, DtFacturaCabezaN) Then Me.Close() : Exit Sub
                If PFactura <> 0 And DtFacturaCabezaN.Rows.Count = 0 Then
                    MsgBox("Factura No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                If PFactura <> 0 Then FechaContableAnt = DtFacturaCabezaN.Rows(0).Item("FechaContable")
                If Not Tablas.Read("SELECT * FROM FacturasProveedorDetalle WHERE Factura = " & PFactura & ";", ConexionN, DtFacturaDetalleN) Then Me.Close() : Exit Sub
                If DtFacturaCabezaN.Rows.Count <> 0 Then PProveedor = DtFacturaCabezaN.Rows(0).Item("Proveedor")
                If DtFacturaCabezaN.Rows.Count <> 0 Then
                    If DtFacturaCabezaN.Rows(0).Item("Rel") = True Then
                        If Not Tablas.Read("SELECT * FROM FacturasProveedorCabeza WHERE Factura = " & DtFacturaCabezaN.Rows(0).Item("NRel") & ";", Conexion, DtFacturaCabezaB) Then Me.Close() : Exit Sub
                        If Not Tablas.Read("SELECT * FROM FacturasProveedorDetalle WHERE Factura = " & DtFacturaCabezaB.Rows(0).Item("Factura") & ";", Conexion, DtFacturaDetalleB) Then Me.Close() : Exit Sub
                    Else
                        DtFacturaCabezaB = DtFacturaCabezaN.Clone
                        DtFacturaDetalleB = DtFacturaDetalleN.Clone
                    End If
                Else
                    DtFacturaCabezaB = DtFacturaCabezaN.Clone
                    DtFacturaDetalleB = DtFacturaDetalleN.Clone
                End If
            End If
        End If

        LlenaDatosEmisor(PProveedor)

        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = 5000 AND Nota = " & PFactura & ";", Conexion, DtRetencionProvinciaB) Then Me.Close() : Exit Sub
        DtRetencionProvinciaAux.Clear()
        For Each Row As DataRow In DtRetencionProvinciaB.Rows
            Dim Row1 As DataRow = DtRetencionProvinciaAux.NewRow
            Row1("Retencion") = Row("Retencion")
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            DtRetencionProvinciaAux.Rows.Add(Row1)
        Next

        If PFactura = 0 Then
            Dim Row As DataRow = DtFacturaCabezaB.NewRow
            ArmaNuevaFacturaProveedor(Row)
            Row("Factura") = 0
            Row("ReciboOficial") = CDbl(PTipoFactura & Format(0, "000000000000"))
            Row("Proveedor") = PProveedor
            Row("Fecha") = DateTime1.Value
            Row("FechaFactura") = "01/01/1800"
            Row("FechaContable") = "01/01/1800"
            Row("EsAfectaCostoLotes") = True
            Row("Estado") = 1
            Row("Tr") = False
            Row("EsExterior") = False
            Row("Moneda") = 1
            Row("Cambio") = 1
            Row("Rendicion") = PRendicion
            DtFacturaCabezaB.Rows.Add(Row)
            DtFacturaCabezaN = DtFacturaCabezaB.Copy
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            ComboNegocio.SelectedValue = HallaNegocio(DtFacturaCabezaB.Rows(0).Item("Costeo"))
        Else
            If DtFacturaCabezaN.Rows.Count <> 0 Then
                ComboNegocio.SelectedValue = HallaNegocio(DtFacturaCabezaN.Rows(0).Item("Costeo"))
            End If
        End If

        MuestraCabeza()

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtFacturaDetalleB.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Item("ImporteB") = Row("Importe")
        Next
        For Each Row As DataRow In DtFacturaDetalleN.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Item("ImporteN") = Row("Importe")
        Next

        'Borra Items sin Importes
        If PFactura <> 0 Then
            For I As Integer = DtGrid.Rows.Count - 1 To 0 Step -1
                Dim Row As DataRow = DtGrid.Rows(I)
                RowsBusqueda = DtGrid.Select("Clave = " & Row.Item("Clave"))
                If RowsBusqueda(0).Item("Tipo") = 25 Then
                    If Row("ImporteB") = 0 And Row("ImporteN") = 0 Then Row.Delete()
                End If
            Next
            DtGrid.AcceptChanges()
        End If

        'Si la factura Tiene parte N los Comprobantes facturados se graban en la Base N.
        ClaveComproFacturados = 0
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            ConexionComproFacturados = ConexionN
            ClaveComproFacturados = DtFacturaCabezaN.Rows(0).Item("Factura")
        Else
            ConexionComproFacturados = Conexion
            ClaveComproFacturados = DtFacturaCabezaB.Rows(0).Item("Factura")
        End If
        '
        DtComproFacturados = New DataTable
        If Not Tablas.Read("SELECT * FROM ComproFacturados WHERE Factura = " & ClaveComproFacturados & ";", ConexionComproFacturados, DtComproFacturados) Then Me.Close() : Exit Sub

        For Each Row1 As DataRow In DtComproFacturados.Rows
            Dim Row As DataRow = DtGridCompro.NewRow
            Row("Operacion") = Row1("Operacion")
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Ingreso") = HallaCantidadLote(Row1("Lote"), Row1("Secuencia"), Row1("Operacion"))
            Row("Remito") = Row1("Remito")
            DtGridCompro.Rows.Add(Row)
        Next

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            PRendicion = DtFacturaCabezaB.Rows(0).Item("Rendicion")
        Else
            PRendicion = DtFacturaCabezaN.Rows(0).Item("Rendicion")
        End If

        'Lee datos del Fondo Fijo.
        Dim DtRendicion As New DataTable
        If Not Tablas.Read("SELECT FondoFijo,Numero FROM RendicionFondoFijo WHERE Rendicion = " & PRendicion & ";", ConexionFactura, DtRendicion) Then
            MsgBox("Error Base de Datos en Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If
        TextNombreFondoFijo.Text = NombreProveedorFondoFijo(DtRendicion.Rows(0).Item("FondoFijo"))
        TextNumero.Text = DtRendicion.Rows(0).Item("Numero")
        FondoFijo = DtRendicion.Rows(0).Item("FondoFijo")
        NumeroFondoFijo = DtRendicion.Rows(0).Item("Numero")
        DtRendicion.Dispose()

        If PFactura = 0 Then
            Grid.ReadOnly = False
            ButtonAceptar.Text = "Graba Rendición"
            MaskedFacturaN.Visible = False
            MaskedReciboOficial.ReadOnly = False
        Else
            Grid.ReadOnly = True
            ButtonAceptar.Text = "Modifica Rendición"
            MaskedReciboOficial.ReadOnly = True
            If DtFacturaCabezaB.Rows.Count <> 0 And DtFacturaCabezaN.Rows.Count <> 0 Then
                MaskedFacturaN.Text = Format(DtFacturaCabezaN.Rows(0).Item("Factura"), "00000000")
                MaskedFacturaN.Visible = True
            Else : MaskedFacturaN.Visible = False
            End If
            If DtFacturaCabezaB.Rows.Count <> 0 And DtFacturaCabezaN.Rows.Count = 0 Then
                Grid.Columns("ImporteN").ReadOnly = True
            End If
            If DtFacturaCabezaB.Rows.Count = 0 And DtFacturaCabezaN.Rows.Count <> 0 Then
                Grid.Columns("ImporteB").ReadOnly = True
            End If
            If ComboEstado.SelectedValue = 3 Then
                Panel4.Enabled = False
            Else
                Panel4.Enabled = True
            End If
        End If

        CalculaTotal()

        TipoAsiento = 7203

        If GGeneraAsiento Then
            Dim Conta = TieneTabla1(TipoAsiento)
            If Conta < 0 Then
                MsgBox("Error Base de Datos al leer Seteo de Documento.")
                Me.Close()
                Exit Sub
            End If
            If Conta = 0 Then
                LabelCuentas.Visible = False
                ListCuentas.Visible = False
                PictureLupaCuenta.Visible = False
            End If
        End If

        ListCuentas.Clear()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function MuestraCabeza() As Boolean

        Dim Row As DataRowView

        MiEnlazador = New BindingSource
        If DtFacturaCabezaB.Rows.Count <> 0 Then
            MiEnlazador.DataSource = DtFacturaCabezaB
        Else : MiEnlazador.DataSource = DtFacturaCabezaN
        End If

        Dim Enlace As Binding
        Row = MiEnlazador.Current

        Enlace = New Binding("Text", MiEnlazador, "Factura")
        AddHandler Enlace.Format, AddressOf FormatFactura
        MaskedFactura.DataBindings.Clear()
        MaskedFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Rendicion")
        TextRendicion.DataBindings.Clear()
        TextRendicion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        AddHandler Enlace.Parse, AddressOf FormatParseReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboEmisor.DataBindings.Clear()
        ComboEmisor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "ConceptoGasto")
        ComboConceptoGasto.DataBindings.Clear()
        ComboConceptoGasto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha") 'fecha debe ir delante de costeo para que muestra los de esas fecha.
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        If Row("Costeo") <> 0 Then
            ComboNegocio.SelectedValue = HallaNegocio(Row("Costeo"))
            ComboNegocio_SelectionChangeCommitted(Nothing, Nothing)
        End If

        Enlace = New Binding("SelectedValue", MiEnlazador, "Costeo")
        ComboCosteo.DataBindings.Clear()
        ComboCosteo.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Secos")
        CheckSecos.DataBindings.Clear()
        CheckSecos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatText
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        If Row("FechaFactura") = "01/01/1800" Then
            TextFechaFactura.Text = ""
        Else : TextFechaFactura.Text = Format(Row("FechaFactura"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

    End Function
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000")

    End Sub
    Private Sub FormatParseReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(PTipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000"))

    End Sub
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TextTipoFactura.Text = LetraTipoIva(Strings.Left(Numero.Value, 1))
        PTipoFactura = Strings.Left(Numero.Value, 1)

        Numero.Value = Strings.Right(Numero.Value, 12)

    End Sub
    Private Sub FormatText(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function ActualizaArchivos(ByRef DtFacturaCabezaBW As DataTable, ByRef DtFacturaCabezaNW As DataTable, ByRef DtFacturaDetalleBW As DataTable, ByRef DtFacturaDetalleNW As DataTable, ByRef DtComproFacturadosW As DataTable, ByRef DtRetencionProvinciaW As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        If DtFacturaCabezaBW.Rows.Count <> 0 Then
            If Format(DtFacturaCabezaBW.Rows(0).Item("FechaFactura"), "dd/MM/yyyy") <> CDate(TextFechaFactura.Text) Then DtFacturaCabezaBW.Rows(0).Item("FechaFactura") = CDate(TextFechaFactura.Text)
            If Format(DtFacturaCabezaBW.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtFacturaCabezaBW.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If DtFacturaCabezaBW.Rows(0).Item("ConceptoGasto") <> ComboConceptoGasto.SelectedValue Then DtFacturaCabezaBW.Rows(0).Item("ConceptoGasto") = ComboConceptoGasto.SelectedValue
            If DtFacturaCabezaBW.Rows(0).Item("Costeo") <> ComboCosteo.SelectedValue Then DtFacturaCabezaBW.Rows(0).Item("Costeo") = ComboCosteo.SelectedValue
            If DtFacturaCabezaBW.Rows(0).Item("Secos") <> CheckSecos.Checked Then DtFacturaCabezaBW.Rows(0).Item("Secos") = CheckSecos.Checked
        End If
        If DtFacturaCabezaNW.Rows.Count <> 0 Then
            If Format(DtFacturaCabezaNW.Rows(0).Item("FechaFactura"), "dd/MM/yyyy") <> CDate(TextFechaFactura.Text) Then DtFacturaCabezaNW.Rows(0).Item("FechaFactura") = CDate(TextFechaFactura.Text)
            If Format(DtFacturaCabezaNW.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtFacturaCabezaNW.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If DtFacturaCabezaNW.Rows(0).Item("ConceptoGasto") <> ComboConceptoGasto.SelectedValue Then DtFacturaCabezaNW.Rows(0).Item("ConceptoGasto") = ComboConceptoGasto.SelectedValue
            If DtFacturaCabezaNW.Rows(0).Item("Costeo") <> ComboCosteo.SelectedValue Then DtFacturaCabezaNW.Rows(0).Item("Costeo") = ComboCosteo.SelectedValue
            If DtFacturaCabezaNW.Rows(0).Item("Secos") <> CheckSecos.Checked Then DtFacturaCabezaNW.Rows(0).Item("Secos") = CheckSecos.Checked
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        For Each Row As DataRow In DtRetencionProvinciaAux.Rows
            RowsBusqueda = DtRetencionProvinciaW.Select("Retencion = " & Row("Retencion") & " AND Provincia = " & Row("Provincia"))
            If RowsBusqueda.Length <> 0 Then
                If Row("Importe") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
            Else
                Dim Row1 As DataRow = DtRetencionProvinciaW.NewRow
                Row1("TipoNota") = 5000
                Row1("Nota") = DtFacturaCabezaBW.Rows(0).Item("Factura")
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaW.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtRetencionProvinciaW.Rows
            RowsBusqueda = DtRetencionProvinciaAux.Select("Retencion = " & Row("Retencion") & " AND Provincia = " & Row("Provincia"))
            If RowsBusqueda.Length = 0 Then Row.Delete()
        Next

        If PFactura <> 0 Then Return True

        'Actualizo Cabeza de facturas.
        If DtFacturaCabezaBW.Rows.Count <> 0 Then
            DtFacturaCabezaBW.Rows(0).Item("Importe") = CDbl(TextTotalB.Text)
            DtFacturaCabezaBW.Rows(0).Item("Saldo") = DtFacturaCabezaBW.Rows(0).Item("Importe")
        End If

        If DtFacturaCabezaNW.Rows.Count <> 0 Then
            DtFacturaCabezaNW.Rows(0).Item("Importe") = CDbl(TextTotalN.Text)
            DtFacturaCabezaNW.Rows(0).Item("Saldo") = DtFacturaCabezaNW.Rows(0).Item("Importe")
        End If

        'Actualiza detalle factura(Impuestos).
        For Each Row As DataRow In DtGrid.Rows
            If Row("ImporteB") <> 0 Then
                Dim Row1 As DataRow = DtFacturaDetalleBW.NewRow
                Row1("Factura") = 0
                Row1("Impuesto") = Row("Clave")
                Row1("Importe") = Row("ImporteB")
                DtFacturaDetalleBW.Rows.Add(Row1)
            End If
        Next

        For Each Row As DataRow In DtGrid.Rows
            If Row("ImporteN") <> 0 Then
                Dim Row1 As DataRow = DtFacturaDetalleNW.NewRow
                Row1("Factura") = 0
                Row1("Impuesto") = Row("Clave")
                Row1("Importe") = Row("ImporteN")
                DtFacturaDetalleNW.Rows.Add(Row1)
            End If
        Next

        'Actualizo ComproFacturados.
        ActualizaComprobantesFacturadosAfectaLotes(DtComproFacturadosW)

        Return True

    End Function
    Private Sub ActualizaComprobantesFacturadosAfectaLotes(ByRef DtComproFacturadosW As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Lotes Imputados.
        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtComproFacturadosW.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row As DataRow = DtComproFacturadosW.NewRow()
                    Row("Factura") = ClaveComproFacturados
                    Row("Operacion") = Row1("Operacion")
                    Row("Lote") = Row1("Lote")
                    Row("Secuencia") = Row1("Secuencia")
                    Row("OrdenCompra") = 0
                    Row("Remito") = Row1("Remito")
                    Row("Ingreso") = Row1("Ingreso")
                    Row("Importe") = 0
                    Row("Senia") = 0
                    Row("ImporteConIva") = 0
                    Row("ImporteSinIva") = 0
                    DtComproFacturadosW.Rows.Add(Row)
                End If
            End If
        Next
        For Each Row1 As DataRow In DtComproFacturadosW.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtGridCompro.Select("Lote = " & Row1("Lote") & " AND Secuencia = " & Row1("Secuencia"))
                If RowsBusqueda.Length = 0 Then Row1.Delete()
            End If
        Next

        If IsNothing(DtComproFacturadosW.GetChanges) Then Exit Sub

        'prorrotea importe comprobantes.  
        Dim ImporteConIva As Decimal
        Dim ImporteSinIva As Decimal
        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") <> 10 Then
                ImporteConIva = ImporteConIva + Row("ImporteB") + Row("ImporteN")
                If Row("Clave") = 1 Or Row("Clave") = 2 Then ImporteSinIva = ImporteSinIva + Row("ImporteB") + Row("ImporteN")
            End If
        Next
        '
        Dim TotalLotes As Decimal = 0

        For Each Row As DataRow In DtComproFacturadosW.Rows
            Row("ImporteConIva") = ImporteConIva * Row("Ingreso")
            Row("ImporteSinIva") = ImporteSinIva * Row("Ingreso")
            TotalLotes = TotalLotes + Row("Ingreso")
        Next
        '
        For Each Row As DataRow In DtComproFacturadosW.Rows
            Row("ImporteConIva") = Trunca(Row("ImporteConIva") / TotalLotes)
            Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") / TotalLotes)
        Next

    End Sub
    Private Sub ArmaGrid()

        DtGrid = ArmaConceptosPagoProveedores(False, PFactura)

        Dim Rowsbusqueda() As DataRow

        Rowsbusqueda = DtGrid.Select("Clave = 0")
        Rowsbusqueda(0).Delete()

        Grid.DataSource = DtGrid

        Grid.Sort(Grid.Columns("Clave"), System.ComponentModel.ListSortDirection.Ascending)

        For Each Row1 As DataGridViewRow In Grid.Rows
            If Row1.Cells("Clave").Value <> 2 And Row1.Cells("Clave").Value <> 10 Then Row1.Cells("ImporteN").ReadOnly = True
            '     If Row1.Cells("Alicuota").Value <> 0 Then Row1.Cells("ImporteB").ReadOnly = True
            If Row1.Cells("Alicuota").Value = 0 Then Row1.Cells("Sel").ReadOnly = True
            '    If Row1.Cells("Clave").Value = 2 Then Row1.Cells("ImporteB").ReadOnly = True
            If Row1.Cells("Clave").Value = 1 Then Row1.Cells("ImporteN").ReadOnly = True
            If Row1.Cells("Clave").Value = 1 And GTipoIva = 2 Then Row1.Cells("ImporteB").ReadOnly = True
            Rowsbusqueda = DtGrid.Select("Clave = " & Row1.Cells("Clave").Value)
            If Rowsbusqueda(0).Item("Tipo") = 25 Then
                If EsRetencionPorProvincia(Row1.Cells("Clave").Value) Then
                    Row1.Cells("TieneLupa").Value = True
                Else : Row1.Cells("TieneLupa").Value = False
                End If
            End If
            If Rowsbusqueda(0).Item("Tipo") = 22 Then
                If GTipoIva = 2 Then Row1.Cells("ImporteB").ReadOnly = True
            End If
        Next

    End Sub
    Private Sub HacerAlta(ByVal DtFacturaCabezaBW As DataTable, ByVal DtFacturaCabezaNW As DataTable, ByVal DtFacturaDetalleBW As DataTable, ByVal DtFacturaDetalleNW As DataTable, ByVal DtComproFacturadosW As DataTable, ByVal DtRetencionProvinciaW As DataTable, ByVal DtRendicionW As DataTable)

        'Arma Tipo Operaciones de Lotes para Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If CDbl(TextTotalB.Text) <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaB, DtAsientoDetalleB, 1) Then Exit Sub
            End If
            If CDbl(TextTotalN.Text) <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaN, DtAsientoDetalleN, 2) Then Exit Sub
            End If
        End If

        'Graba Factura.
        Dim NumeroB As Double = 0
        Dim NumeroN As Double = 0
        Dim NumeroAsientoB As Double = 0
        Dim NumeroAsientoN As Double = 0
        Dim Resul As Double = 0

        For i As Integer = 1 To 50
            If CDbl(TextTotalB.Text) <> 0 Then
                NumeroB = UltimaNumeracion(Conexion)
                If NumeroB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    ArmaArchivos()
                    Exit Sub
                End If
            End If
            If CDbl(TextTotalN.Text) <> 0 Then
                NumeroN = UltimaNumeracion(ConexionN)
                If NumeroN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    ArmaArchivos()
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            '
            If CDbl(TextTotalB.Text) <> 0 Then
                DtFacturaCabezaBW.Rows(0).Item("Factura") = NumeroB
                If CDbl(TextTotalN.Text) <> 0 Then
                    DtFacturaCabezaBW.Rows(0).Item("Rel") = True
                Else : DtFacturaCabezaBW.Rows(0).Item("Rel") = False
                End If
                DtFacturaCabezaBW.Rows(0).Item("NRel") = 0
                For Each Row As DataRow In DtFacturaDetalleBW.Rows
                    Row("Factura") = NumeroB
                Next
                For Each Row As DataRow In DtRetencionProvinciaW.Rows
                    Row("Nota") = NumeroB
                Next
            End If
            '
            If CDbl(TextTotalN.Text) <> 0 Then
                DtFacturaCabezaNW.Rows(0).Item("Factura") = NumeroN
                If CDbl(TextTotalB.Text) <> 0 Then
                    DtFacturaCabezaNW.Rows(0).Item("Rel") = True
                Else : DtFacturaCabezaNW.Rows(0).Item("Rel") = False
                End If
                DtFacturaCabezaNW.Rows(0).Item("NRel") = NumeroB
                For Each Row As DataRow In DtFacturaDetalleNW.Rows
                    Row("Factura") = NumeroN
                Next
            End If

            'Actualiza ComproFacturados.
            Dim FacturaParaCompro As Double
            If ConexionComproFacturados = Conexion Then
                FacturaParaCompro = NumeroB
            Else : FacturaParaCompro = NumeroN
            End If
            For Each Row As DataRow In DtComproFacturadosW.Rows
                Row("Factura") = FacturaParaCompro
            Next

            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = NumeroB
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If

            Resul = ActualizaFactura(DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtFacturaCabezaBW, DtFacturaCabezaNW, DtFacturaDetalleBW, DtFacturaDetalleNW, DtComproFacturadosW, DtRetencionProvinciaW, DtRendicionW)
            '
            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -10 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            MsgBox("Factura Ya Fue Ingresada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            If NumeroB <> 0 Then
                PFactura = NumeroB
                PAbierto = True
            Else : PFactura = NumeroN
                PAbierto = False
            End If
            GModificacionOk = True
        End If

        ArmaArchivos()

    End Sub
    Private Function ActualizaFactura(ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtFacturaCabezaBW As DataTable, ByVal DtFacturaCabezaNW As DataTable, ByVal DtFacturaDetalleBW As DataTable, ByVal DtFacturaDetalleNW As DataTable, ByVal DtComproFacturadosW As DataTable, ByVal DtRetencionProvinciaBAux As DataTable, ByVal DtRendicionAux As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtFacturaCabezaBW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaCabezaBW.GetChanges, "FacturasProveedorCabeza", Conexion)
                    If Resul <= 0 Then
                        If Resul = -1 Then
                            Return -10
                        Else
                            Return Resul
                        End If
                    End If
                End If
                '
                If Not IsNothing(DtFacturaDetalleBW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaDetalleBW.GetChanges, "FacturasProveedorDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRetencionProvinciaBAux.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaBAux.GetChanges, "RecibosRetenciones", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtFacturaCabezaNW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaCabezaNW.GetChanges, "FacturasProveedorCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtFacturaDetalleNW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaDetalleNW.GetChanges, "FacturasProveedorDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtComproFacturadosW.GetChanges) Then
                    Resul = GrabaTabla(DtComproFacturadosW.GetChanges, "ComproFacturados", ConexionComproFacturados)
                    If Resul <= 0 Then Return Resul
                End If
                '
                ' graba Asiento B.
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento N.
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                'Actualiza Rendicion Fondo Fijo.
                If Not IsNothing(DtRendicionAux.GetChanges) Then
                    Resul = GrabaTabla(DtRendicionAux.GetChanges, "RendicionFondoFijo", ConexionFactura)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Sub LlenaDatosEmisor(ByVal Proveedor As Integer)

        Dim Dta As New DataTable
        Dim Sql As String = ""

        Sql = "SELECT * FROM Proveedores WHERE Clave = " & Proveedor & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, Proveedor ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")

        Dta.Dispose()

    End Sub
    Private Sub CalculaTotal()

        Dim TotalB As Decimal = 0
        Dim TotalN As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            TotalB = TotalB + Row("ImporteB")
            TotalN = TotalN + Row("ImporteN")
        Next

        TextTotalB.Text = FormatNumber(TotalB, GDecimales)
        TextTotalN.Text = FormatNumber(TotalN, GDecimales)

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Articulo)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Importe)

        Dim Senia As New DataColumn("Senia")
        Senia.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Senia)

        Dim OrdenCompra As New DataColumn("OrdenCompra")
        OrdenCompra.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(OrdenCompra)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Remito)

        Dim Ingreso As New DataColumn("Ingreso")
        Ingreso.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Ingreso)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

    End Sub
    Private Sub CreaDtRetencionProvinciaAux()

        DtRetencionProvinciaAux = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasProveedorCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else
                        Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionRendicion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Rendicion) FROM RendicionFondoFijo;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else
                        Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Sub Opciones()

        OpcionEmisorYLetra.PEsLocal = True
        OpcionEmisorYLetra.PEsProveedor = True
        OpcionEmisorYLetra.PEsFacturaProveedor = True

        OpcionEmisorYLetra.PEsNoNegocio = True
        OpcionEmisorYLetra.PictureCandado.Visible = False
        OpcionEmisorYLetra.ShowDialog()
        PProveedor = OpcionEmisorYLetra.PEmisor
        PTipoFactura = OpcionEmisorYLetra.PNumeroLetra
        OpcionEmisorYLetra.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Operacion As Integer) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim Item As New ItemListaConceptosAsientos
        Dim ImporteNetoGrabadoInformado As Decimal
        Dim ImporteNetoNoGrabadoInformado As Decimal
        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtGrid.Select("Clave = 1")
        ImporteNetoGrabadoInformado = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        ImporteNetoGrabadoInformado = ImporteNetoGrabadoInformado + RowsBusqueda(0).Item("ImporteB")
        ImporteNetoNoGrabadoInformado = RowsBusqueda(0).Item("ImporteN")
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        If Operacion = 1 Then
            Item.Importe = ImporteNetoGrabadoInformado
        Else
            Item.Importe = ImporteNetoNoGrabadoInformado
        End If
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)

        'Arma Lista con Cuentas definidas en documento.
        If ListCuentas.Visible Then
            Dim ImporteB As Decimal
            Dim ImporteN As Decimal
            For I As Integer = 0 To ListCuentas.Items.Count - 1
                Dim Fila As New ItemCuentasAsientos
                Dim Cuenta As String = ListCuentas.Items.Item(I).SubItems(0).Text
                Fila.Cuenta = Mid$(Cuenta, 1, 3) & Mid$(Cuenta, 5, 6) & Mid$(Cuenta, 12, 2)
                Fila.Clave = 202
                If Operacion = 1 Then
                    Fila.Importe = CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
                Else
                    Fila.Importe = CDec(ListCuentas.Items.Item(I).SubItems(2).Text)
                End If
                ListaCuentas.Add(Fila)
                ImporteB = ImporteB + CDec(ListCuentas.Items.Item(I).SubItems(1).Text)
                ImporteN = ImporteN + CDec(ListCuentas.Items.Item(I).SubItems(2).Text)
            Next
            If Operacion = 1 Then
                If ImporteB <> ImporteNetoGrabadoInformado Then
                    MsgBox("Importe de Cuentas Informada Difiere del Importe de la Factura")
                    Return False
                End If
            Else
                If ImporteN <> ImporteNetoNoGrabadoInformado Then
                    MsgBox("Importe de Cuentas Informada Difiere del Importe de la Factura")
                    Return False
                End If
            End If
        End If

        'Arma lista de iva.
        For Each Row As DataRow In DtGrid.Rows
            If Row("Tipo") = 22 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Clave")
                If Operacion = 1 Then
                    Item.Importe = Row("ImporteB")
                Else : Item.Importe = Row("ImporteN")
                End If
                Item.TipoIva = 5
                If Item.Importe <> 0 Then ListaIVA.Add(Item)
            End If
        Next

        'Arma lista de Retencion.
        Item = New ItemListaConceptosAsientos
        For Each Row As DataRow In DtGrid.Rows
            If Row("Tipo") = 25 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Clave")
                If Operacion = 1 Then
                    Item.Importe = Row("ImporteB")
                Else : Item.Importe = Row("ImporteN")
                End If
                Item.TipoIva = 9
                If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
            End If
        Next

        'Arma lista con los conceptos.
        Item = New ItemListaConceptosAsientos
        For Each Row As DataRow In DtGrid.Rows
            If Row("Tipo") <> 22 And Row("Tipo") <> 25 And Row("Clave") <> 1 And Row("Clave") <> 2 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Clave")
                If Operacion = 1 Then
                    Item.Importe = Row("ImporteB")
                Else : Item.Importe = Row("ImporteN")
                End If
                If Item.Importe <> 0 Then ListaConceptos.Add(Item)
            End If
        Next

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        If Operacion = 1 Then
            Item.Importe = CDec(TextTotalB.Text)
        Else
            Item.Importe = CDec(TextTotalN.Text)
        End If
        ListaConceptos.Add(Item)

        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False

        Return True

    End Function
    Private Function ExisteFactura(ByVal Factura As Double, ByVal Proveedor As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(ReciboOficial) FROM FacturasProveedorCabeza WHERE Estado = 1 AND ReciboOficial = " & Factura & " AND Proveedor = " & Proveedor & ";", Miconexion)
                    Return CDbl(Cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function RendicionCerrada(ByVal Rendicion As Integer, ByVal ConexionStr As String) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Cerrado FROM RendicionFondoFijo WHERE Rendicion = " & Rendicion & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: RendicionFondoFijo. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            If DtFacturaCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("Error, en este momento no puede modificar la Factura(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If CDbl(TextTotalB.Text) = 0 And CDbl(TextTotalN.Text) = 0 Then
            MsgBox("Falta Informar Neto. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If ComboNegocio.SelectedValue <> 0 And DtGridCompro.Rows.Count <> 0 Then
            MsgBox("No esta Permitido Imputar a Lotes y Negocio Simultaneamente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim HayIva As Boolean = False
        For Each Row As DataRow In DtGrid.Rows
            If Row("Iva") <> 0 And Row("ImporteB") <> 0 Then HayIva = True
        Next

        If Grid.Rows(0).Cells("ImporteB").Value <> 0 And Not HayIva And ComboTipoIva.SelectedValue <> Exterior Then
            MsgBox("Falta Informar Iva. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        If Grid.Rows(0).Cells("ImporteB").Value = 0 And HayIva Then
            MsgBox("No debe Informar Iva. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        If Grid.Rows(0).Cells("ImporteB").Value = 0 And Grid.Rows(1).Cells("ImporteB").Value = 0 And CDbl(TextTotalB.Text) <> 0 Then
            MsgBox("Falta Informar Neto en Importe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        'Conciste Ivas.
        Dim BaseImponible As Decimal = 0
        Dim Grabado As Decimal = 0
        If HayIva Then
            For Each Row As DataRow In DtGrid.Rows
                If Row("Clave") = 1 Then Grabado = Row("ImporteB")
                If Row("Tipo") = 22 And Row("Iva") <> 0 And Row("ImporteB") <> 0 Then
                    BaseImponible = BaseImponible + Trunca(Row("ImporteB") * 100 / Row("Iva"))
                End If
            Next
            If Abs(Grabado - BaseImponible) > GTolerancia Then
                If MsgBox("Base-Imponible de los Ivas Informados difiere del Neto-Grabado Informado. " + vbCrLf + "Puede producir inconsistencias en Informes Impositivos. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then Return False
            End If
        End If

        Dim ImporteRetencion As Decimal = 0
        For Each Row1 As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row1.Cells("Clave").Value) Then
                If Row1.Cells("TieneLupa").Value Then
                    ImporteRetencion = ImporteRetencion + Row1.Cells("ImporteB").Value
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Decimal
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        If CDbl(TextTotalB.Text) <> 0 And PFactura = 0 Then
            If Val(MaskedReciboOficial.Text) = 0 Then
                MsgBox("Falta Informar Factura Proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            Select Case ExisteFactura(CDbl(PTipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000")), PProveedor)
                Case Is < 0
                    MsgBox("ERROR, Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
                Case Is > 0
                    MsgBox("Factura Proveedor Ya Exsite. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    MaskedReciboOficial.Focus()
                    Return False
            End Select
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            Dim Imputado As Decimal = DtFacturaCabezaB.Rows(0).Item("Importe") - DtFacturaCabezaB.Rows(0).Item("Saldo")
            If CDec(TextTotalB.Text) < Imputado Then
                MsgBox("Nuevo Importe de Factura Supera importe Imputado en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            Dim Imputado As Decimal = DtFacturaCabezaN.Rows(0).Item("Importe") - DtFacturaCabezaN.Rows(0).Item("Saldo")
            If CDec(TextTotalN.Text) < Imputado Then
                MsgBox("Nuevo Importe de Factura Supera importe Imputado en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        If ComboConceptoGasto.SelectedValue = 0 Then
            MsgBox("Falta Informar Concepto del Gasto. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboConceptoGasto.Focus()
            Return False
        End If

        If ComboNegocio.SelectedValue <> 0 And ComboCosteo.SelectedValue = 0 Then
            MsgBox("Falta Informar Costeo del Negocio. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCosteo.Focus()
            Return False
        End If

        If TextFechaFactura.Text = "" Then
            MsgBox("Falta Informar Fecha Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaFactura.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaFactura.Text) Then
            MsgBox("Fecha Factura Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaFactura.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaFactura.Text, Date.Now) < 0 Then
            MsgBox("Fecha Factura Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaFactura.Focus()
            Return False
        End If

        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaContable.Text) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
            MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If

        If ListCuentas.Visible And PFactura = 0 Then
            If ListCuentas.Items.Count = 0 Then
                MsgBox("Falta Informar Cuenta Contable. Operación se CANCELA.")
                Return False
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

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If Grid.Columns(e.ColumnIndex).Name = "Sel" Then
            If Grid.Rows(e.RowIndex).Cells("Alicuota").Value = 0 Then Exit Sub
            Dim Check As New DataGridViewCheckBoxCell
            Check = Grid.Rows(e.RowIndex).Cells("Sel")
            Check.Value = Not Check.Value
            If Not Check.Value Then
                Grid.Rows(e.RowIndex).Cells("ImporteB").Value = 0
            Else
                Grid.Rows(e.RowIndex).Cells("ImporteB").Value = CalculaIva(1, Grid.Rows(0).Cells("ImporteB").Value, Grid.Rows(e.RowIndex).Cells("Alicuota").Value)
            End If
            CalculaTotal()
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If Not Me.Visible Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" And Grid.Rows(e.RowIndex).Cells("Clave").Value = 1 Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value = True Then
                    Row.Cells("Sel").Value = False
                    Row.Cells("ImporteB").Value = 0
                End If
            Next
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TieneLupa").Value) Then
                If Grid.Rows(e.RowIndex).Cells("TieneLupa").Value Then
                    e.Value = ImageList1.Images.Item("Lupa")
                Else : e.Value = Nothing
                End If
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not Grid.CurrentRow.Cells("TieneLupa").Value Then Exit Sub
            If Grid.CurrentRow.Cells("ImporteB").Value = 0 Then
                MsgBox("Debe Informar Importe")
                Exit Sub
            End If
            If PFactura <> 0 Then SeleccionarRetencionesProvincias.PFuncionBloqueada = True
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid.CurrentRow.Cells("ImporteB").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid.CurrentRow.Cells("Clave").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
        End If

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("ImporteB") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("ImporteN") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row("ImporteB") = 0 Then
            For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
                If DtRetencionProvinciaAux.Rows(I).Item("Retencion") = e.Row("Clave") Then DtRetencionProvinciaAux.Rows(I).Delete()
            Next
        End If

    End Sub


    Private Sub TextFechaFactura_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextFechaFactura.TextChanged

    End Sub
End Class