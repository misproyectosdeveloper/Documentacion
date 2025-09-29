Option Explicit On
Imports System.Transactions
Imports System.Drawing.Printing
Imports System.Math
Public Class UnaImputacionSaldosIniciales
    Public PClave As Double
    Public PAbierto As Boolean
    Public PTipoNota As Integer
    Public PBloqueaFunciones As Boolean
    Public PImputa As Boolean
    Public PConexion As String
    Public PConexionN As String
    '
    Dim DtNotaDetalle As DataTable
    Dim DtNotaCabeza As DataTable
    ' Comprobantes Imputables
    Dim DtComprobantesCabeza As DataTable
    Dim DtFacturasCabeza As DataTable
    Dim DtLiquidacionCabeza As DataTable
    Dim DtNVLPCabeza As DataTable
    Dim DtGridCompro As DataTable
    Dim DtSena As DataTable
    '
    Private MiEnlazador As New BindingSource
    '
    Public Emisor As Integer
    Dim cb As ComboBox
    Dim ConexionNota As String
    Dim TotalFacturas As Decimal
    Dim UltimoNumero As Double = 0
    Private Sub UnaImputacionSaldosIniciales_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Conexion = "" Then Conexion = PConexion : ConexionN = PConexionN 'Si es llamado de otro proyecto. 

        Me.Top = 50

        GridCompro.AutoGenerateColumns = False
        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        If PTipoNota = 7 Then Label6.Text = "Cliente"
        If PTipoNota = 6 Then Label6.Text = "Proveedor"

        ArmaTipoIva(ComboTipoIva)

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"

        If PTipoNota = 6 Then LabelTipoNota.Text = "Nota Debito Financiera a Saldo Inicial de Proveedor"
        If PTipoNota = 7 Then LabelTipoNota.Text = "Nota Crédito Financiera a Saldo Inicial de Cliente"

        GModificacionOk = False

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionNota = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionNota = ConexionN
        End If

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        If PTipoNota = 7 Then
            Me.BackColor = Color.LightGreen
        End If
        If PTipoNota = 6 Then
            Me.BackColor = Color.LightBlue
        End If

    End Sub
    Private Sub UnaImputacionSaldosIniciales_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones And Not PImputa Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        GridCompro.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim RowsBusqueda() As DataRow

        Dim DtNotaCabezaAux As DataTable = DtNotaCabeza.Copy
        Dim DtNotaDetalleAux As DataTable = DtNotaDetalle.Copy

        Dim DtFacturasCabezaAux As DataTable = DtFacturasCabeza.Copy
        Dim DtNVLPCabezaAux As DataTable = DtNVLPCabeza.Copy
        Dim DtLiquidacionCabezaAux As DataTable = DtLiquidacionCabeza.Copy
        Dim DtSenaAux As DataTable = DtSena.Copy
        Dim DtComprobantesCabezaAux As DataTable = DtComprobantesCabeza.Copy

        'Actualiza Saldo de Comprobantes Imputados.
        ActualizaComprobantes("M", DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux)

        'Actualiza Notas Cabeza.
        DtNotaCabezaAux.Rows(0).Item("Saldo") = CDec(TextImporte.Text) - CDec(TextTotalFacturas.Text)

        'Actualiza Notas Detalle.
        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                RowsBusqueda = DtNotaDetalleAux.Select("TipoComprobante = " & Row1("Tipo") & " AND Comprobante = " & Row1("Comprobante"))
                If RowsBusqueda.Length <> 0 Then
                    If RowsBusqueda(0).Item("Importe") <> Row1.Item("Asignado") Then
                        If Row1.Item("Asignado") = 0 Then
                            RowsBusqueda(0).Delete()
                        Else
                            RowsBusqueda(0).Item("Importe") = Row1.Item("Asignado")
                        End If
                    End If
                Else
                    If Row1.Item("Asignado") <> 0 Then
                        Dim Row As DataRow = DtNotaDetalleAux.NewRow()
                        Row("TipoNota") = PTipoNota
                        Row("Clave") = DtNotaCabeza.Rows(0).Item("Clave")
                        Row("TipoComprobante") = Row1("Tipo")
                        Row("Comprobante") = Row1("Comprobante")
                        Row("Importe") = Row1.Item("Asignado")
                        DtNotaDetalleAux.Rows.Add(Row)
                    End If
                End If
            End If
        Next

        If IsNothing(DtNotaDetalleAux.GetChanges) And IsNothing(DtNotaCabezaAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GridCompro.Focus()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HacerModificacion(DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtNotaDetalleAux) Then
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonDatosCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDatosCliente.Click

        Select Case PTipoNota
            Case 7
                UnDatosEmisor.PEsCliente = True
            Case 6
                UnDatosEmisor.PEsCliente = False
        End Select

        UnDatosEmisor.PEmisor = Emisor
        UnDatosEmisor.ShowDialog()

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGridCompro()

        Dim Sql As String
        Dim Row1 As DataRow
        Dim RowsBusqueda() As DataRow

        DtNotaCabeza = New DataTable
        Sql = "SELECT * FROM SaldosInicialesCabeza WHERE Clave = " & PClave & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaCabeza) Then Return False
        If PClave <> 0 And DtNotaCabeza.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Saldo Inicial No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Emisor = DtNotaCabeza.Rows(0).Item("Emisor")

        TextImporte.Text = FormatNumber(Abs(DtNotaCabeza.Rows(0).Item("Importe")), GDecimales)

        Select Case PTipoNota
            Case 6
                TextNombre.Text = NombreProveedor(DtNotaCabeza.Rows(0).Item("Emisor"))
                HallaAliasProveedor()
            Case 7
                TextNombre.Text = NombreCliente(DtNotaCabeza.Rows(0).Item("Emisor"))
                HallaAliasCliente()
        End Select

        LlenaDatosEmisor(DtNotaCabeza.Rows(0).Item("Emisor"))

        MuestraCabeza()

        DtNotaDetalle = New DataTable
        Sql = "SELECT * FROM SaldosInicialesDetalle WHERE Clave = " & PClave & ";"
        If Not Tablas.Read(Sql, ConexionNota, DtNotaDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        'Muestra Comprobantes a Imputar.
        DtFacturasCabeza = New DataTable
        DtNVLPCabeza = New DataTable
        DtComprobantesCabeza = New DataTable
        DtLiquidacionCabeza = New DataTable
        DtSena = New DataTable

        If PTipoNota = 7 Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            If Not ArmaConNVLP() Then Return False
        End If
        If PTipoNota = 6 Then
            If Not ArmaConFacturas(PTipoNota) Then Return False
            '      If Not ArmaConLiquidaciones() Then Return False
        End If

        'Procesa Facturas.
        DtGridCompro.Clear()
        For Each Row As DataRow In DtFacturasCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 2
            Row1("Comprobante") = Row("Factura")
            If PTipoNota = 7 Then
                Row1("Recibo") = Row("Factura")
                Row1("Fecha") = Row("Fecha")
            Else
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaFactura")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa NVLP.
        For Each Row As DataRow In DtNVLPCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 800
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("ReciboOficial")
            Row1("Comentario") = Row("Comentario")
            Row1("Fecha") = Row("FechaLiquidacion")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Liquidacion.
        For Each Row As DataRow In DtLiquidacionCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = 10
            Row1("Comprobante") = Row("Liquidacion")
            Row1("Recibo") = Row("Liquidacion")
            Row1("Comentario") = Row("Comentario")
            Row1("Fecha") = Row("Fecha")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = 1
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next
        'Procesa Notas.
        For Each Row As DataRow In DtComprobantesCabeza.Rows
            Row1 = DtGridCompro.NewRow()
            If PAbierto Then
                Row1("Operacion") = 1
            Else : Row1("Operacion") = 2
            End If
            Row1("Tipo") = Row("TipoNota")
            Row1("Comprobante") = Row("Nota")
            If Row("TipoNota") = 50 Or Row("TipoNota") = 70 Or Row("TipoNota") = 500 Or Row("TipoNota") = 700 Then
                Row1("Recibo") = Row("ReciboOficial")
                Row1("Fecha") = Row("FechaReciboOficial")
            Else
                Row1("Recibo") = Row("Nota")
                Row1("Fecha") = Row("Fecha")
            End If
            Row1("Comentario") = Row("Comentario")
            Row1("Importe") = Row("Importe")
            Row1("Moneda") = Row("Moneda")
            Row1("Saldo") = Row("Saldo")
            Row1("SaldoAnt") = Row("Saldo")
            Row1("Asignado") = 0
            DtGridCompro.Rows.Add(Row1)
        Next

        For Each Row As DataRow In DtNotaDetalle.Rows
            RowsBusqueda = DtGridCompro.Select("Tipo = " & Row("TipoComprobante") & " AND Comprobante = " & Row("Comprobante"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Asignado") = Row("Importe")
            End If
        Next

        'Borra los documentos con saldo 0 y no tienen asignacion. 
        DtGridCompro.AcceptChanges()
        For Each Row As DataRow In DtGridCompro.Rows
            If Row("Saldo") = 0 And Row("Asignado") = 0 Then Row.Delete()
        Next

        GridCompro.DataSource = DtGridCompro

        If PTipoNota = 7 And ComboTipoIva.SelectedValue = Exterior Then
            Label6.Text = "Cliente Facturación"
        End If

        CalculaTotales()

        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ArmaConFacturas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""

        '------------------------------------------------------------------------------------------------------------
        'ClienteOperacion = 0 para que no aparezcan las generadas en el modulo de exportacion.-----------------------
        '------------------------------------------------------------------------------------------------------------

        If TipoNota = 5 Or TipoNota = 7 Or TipoNota = 50 Or TipoNota = 70 Or TipoNota = 60 Then
            Sql = "SELECT * FROM FacturasCabeza WHERE Tr = 0 AND ClienteOperacion = 0 AND Estado <> 3 AND FacturasCabeza.Cliente = " & Emisor & " ORDER BY Factura,Fecha;"
        Else
            Sql = "SELECT * FROM FacturasProveedorCabeza WHERE Rendicion = 0 AND Tr = 0 AND Estado = 1 AND Liquidacion = 0 AND Proveedor = " & Emisor & " ORDER BY Factura,Fecha;"
        End If
        If Not Tablas.Read(Sql, ConexionNota, DtFacturasCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNVLP() As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM NVLPCabeza WHERE Estado = 1 AND Cliente = " & Emisor & " ORDER BY Liquidacion,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtNVLPCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConNotas(ByVal TipoNota As Integer) As Boolean

        Dim Sql As String = ""


        Sql = "SELECT * FROM RecibosCabeza WHERE Estado = 1 AND TipoNota = " & TipoNota & " AND Emisor = " & Emisor & " ORDER BY Nota,Fecha;"
        If Not Tablas.Read(Sql, ConexionNota, DtComprobantesCabeza) Then Return False

        Return True

    End Function
    Private Function ArmaConLiquidaciones() As Boolean

        Dim Sql As String = ""

        Sql = "SELECT * FROM LiquidacionCabeza WHERE Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & " ORDER BY Liquidacion,Fecha;"

        If PTipoNota = 600 Then
            Sql = "SELECT * FROM LiquidacionCabeza WHERE Tr = 0 AND Estado = 1 AND LiquidacionCabeza.Proveedor = " & Emisor & " ORDER BY Liquidacion,Fecha;"
        End If

        If Not Tablas.Read(Sql, ConexionNota, DtLiquidacionCabeza) Then Return False

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtNotaCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, 3)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Tipo.DataSource = DtTiposComprobantes(PAbierto)
        Tipo.DisplayMember = "Nombre"
        Tipo.ValueMember = "Codigo"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 order By Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "PESOS"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Tipo As New DataColumn("Tipo")
        Tipo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Tipo)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Comprobante)

        Dim Comentario As New DataColumn("Comentario")
        Comentario.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Comentario)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Recibo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridCompro.Columns.Add(Fecha)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Importe)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Moneda)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Saldo)

        Dim SaldoAnt As New DataColumn("SaldoAnt")
        SaldoAnt.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(SaldoAnt)

        Dim Asignado As New DataColumn("Asignado")
        Asignado.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Asignado)

    End Sub
    Private Function HacerModificacion(ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable) As Boolean

        'Graba Facturas.
        Dim Resul As Double

        Resul = ActualizaNota("M", DtNotaCabezaAux, DtFacturasCabezaAux, DtNVLPCabezaAux, DtLiquidacionCabezaAux, DtSenaAux, DtComprobantesCabezaAux, DtNotaDetalleAux)

        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaNota(ByVal Funcion As String, ByVal DtNotaCabezaAux As DataTable, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable, ByVal DtNotaDetalleAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtNotaCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaCabezaAux.GetChanges, "SaldosInicialesCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtNotaDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtNotaDetalleAux.GetChanges, "SaldosInicialesDetalle", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtFacturasCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoFacturas(PTipoNota, DtFacturasCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtNVLPCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtNVLPCabezaAux.GetChanges, "NVLPCabeza", ConexionNota)
                    If Resul <= 0 Then Return 0
                End If
                '
                If Not IsNothing(DtComprobantesCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoNotas(DtComprobantesCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If
                '
                If Not IsNothing(DtLiquidacionCabezaAux.GetChanges) Then
                    If Not ActualizaSaldoLiquidacion(DtLiquidacionCabezaAux.GetChanges, ConexionNota) Then
                        Return 0
                    End If
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub ActualizaComprobantes(ByVal Funcion As String, ByVal DtFacturasCabezaAux As DataTable, ByVal DtNVLPCabezaAux As DataTable, ByVal DtLiquidacionCabezaAux As DataTable, ByVal DtSenaAux As DataTable, ByVal DtComprobantesCabezaAux As DataTable)

        Dim RowsBusqueda() As DataRow

        'Actualiza Saldo de Comprobantes Imputados.

        If Funcion = "M" Then
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1("Saldo") <> Row1("SaldoAnt") Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 10
                                RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 30
                                RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = Row1.Item("Saldo")
                        End Select
                    End If
                End If
            Next
        End If

        If Funcion = "B" Then
            Dim Importe As Double = 0
            For Each Row1 As DataRow In DtGridCompro.Rows
                If Row1.RowState <> DataRowState.Deleted Then
                    If Row1.Item("Asignado") <> 0 Then
                        Select Case Row1("Tipo")
                            Case 2
                                RowsBusqueda = DtFacturasCabezaAux.Select("Factura = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                            Case 10
                                RowsBusqueda = DtLiquidacionCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                            Case 30
                                RowsBusqueda = DtSenaAux.Select("Comprobante = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                            Case 800
                                RowsBusqueda = DtNVLPCabezaAux.Select("Liquidacion = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                            Case Else
                                RowsBusqueda = DtComprobantesCabezaAux.Select("TipoNota = " & Row1("Tipo") & " AND Nota = " & Row1("Comprobante"))
                                RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row1.Item("Asignado")
                        End Select
                    End If
                End If
            Next
        End If

    End Sub
    Private Sub CalculaTotales()

        TotalFacturas = 0
        For Each Row As DataRow In DtGridCompro.Rows
            If Row.RowState <> DataRowState.Deleted Then
                TotalFacturas = TotalFacturas + Row("Asignado")
            End If
        Next

        TextTotalFacturas.Text = FormatNumber(TotalFacturas, GDecimales)

    End Sub
    Private Function LlenaDatosEmisor(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable
        Dim Sql As String = ""

        If PTipoNota = 7 Then
            Sql = "SELECT TipoIva FROM Clientes WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Cliente No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Else
            Sql = "SELECT TipoIva FROM Proveedores WHERE Clave = " & Cliente & ";"
            Dta = Tablas.Leer(Sql)
            If Dta.Rows.Count = 0 Then
                MsgBox("ERROR, Proveedor No Existe o Error en la Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")

        Dta.Dispose()

        Return True

    End Function
    Public Function HallaAliasProveedor() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Proveedores WHERE Clave = " & Emisor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Public Function HallaAliasCliente() As String

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Alias FROM Proveedores WHERE Clave = " & Emisor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Proveedores.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim Row As DataRow = DtNotaCabeza.Rows(0)

        If CDbl(TextImporte.Text) < CDbl(TextTotalFacturas.Text) Then
            MsgBox("Importe Imputado Mayor al Saldo Inicial. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row1 As DataRow In DtGridCompro.Rows
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Saldo") <> Row1("SaldoAnt") Then
                    If Row1("Moneda") <> ComboMoneda.SelectedValue Then
                        MsgBox("Se Imputo a Documento en Distinta Moneda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        GridCompro.Focus()
                        Return False
                    End If
                End If
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellContentClick

        If GridCompro.Columns(e.ColumnIndex).Name = "Seleccion" Then
            Dim chkCell As DataGridViewCheckBoxCell = Me.GridCompro.Rows(e.RowIndex).Cells("Seleccion")
            chkCell.Value = Not chkCell.Value
            If chkCell.Value Then
                If GridCompro.Rows(e.RowIndex).Cells("Saldo").Value < 0 Then Exit Sub
                Dim Diferencia As Double = 0
                Diferencia = CDbl(TextImporte.Text) - TotalFacturas - GridCompro.Rows(e.RowIndex).Cells("Asignado").Value
                If Diferencia <= 0 Then Exit Sub
                If (GridCompro.Rows(e.RowIndex).Cells("Saldo").Value - Diferencia) <= 0 Then
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = GridCompro.Rows(e.RowIndex).Cells("Saldo").Value
                Else
                    GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = Diferencia
                End If
                CalculaTotales()
            Else
                GridCompro.Rows(e.RowIndex).Cells("Asignado").Value = 0
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Comprobante1" Then
            e.Value = NumeroEditado(e.Value)
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Recibo" Then
            e.Value = NumeroEditado(e.Value)
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "FechaCompro" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "ImporteCompro" Or GridCompro.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub GridCompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If CType(sender, TextBox).Text <> "" Then
                If Not IsNumeric(CType(sender, TextBox).Text) Then
                    CType(sender, TextBox).Text = ""
                    CType(sender, TextBox).Focus()
                End If
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        If GridCompro.Columns(e.ColumnIndex).Name = "Asignado" Then
            If IsDBNull(GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) Then GridCompro.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim SaldoAnt As Decimal = 0

        If (e.Column.ColumnName.Equals("Asignado")) Then
            If IsDBNull(e.Row("Asignado")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If Trunca(TotalFacturas - e.Row("Asignado") + CDec(e.ProposedValue)) > CDbl(TextImporte.Text) Then
                MsgBox("Importe Imputado Supera Saldo Inicial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                Exit Sub
            End If
            '
            SaldoAnt = e.Row("Saldo")
            '
            e.Row("Saldo") = e.Row("Saldo") + e.Row("Asignado") - CDec(e.ProposedValue)
            '
            If e.Row("Saldo") < 0 Then
                MsgBox("Imputación Supera el Saldo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Asignado")
                e.Row("Saldo") = SaldoAnt
                Exit Sub
            End If
        End If

        CalculaTotales()

    End Sub








End Class