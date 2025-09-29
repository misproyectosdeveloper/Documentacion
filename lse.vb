Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnaFactura
    Public PFactura As Double
    Public PCliente As Double
    Public PAbierto As Boolean
    Public PActualizacionOk As Boolean
    Public PBloqueaFunciones As Boolean
    Public PEsElectronica As Boolean
    '   
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtAsignacionLotes As DataTable
    Dim DtCabezaRemito As DataTable
    Dim DtAsignacionLotesRemito As DataTable
    '
    Dim ListaDeLotes As New List(Of FilaAsignacion)
    Dim ListaDePrecios As New List(Of ItemListaDePrecios)
    ' 
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim TipoFactura As Integer
    Dim Deposito As Integer
    Dim Remito As Double
    Dim AbiertoRemito As Boolean
    '
    Dim IndiceW As Integer
    Dim Total As Double
    Dim TotalB As Double
    Dim TotalN As Double
    Dim PagoB As Double
    Dim PagoN As Double
    Dim ConexionFactura As String
    Dim ConexionRelacionada As String
    Dim ConexionRemito As String
    Dim CtaCteAbierta As Boolean
    Dim CtaCteCerrada As Boolean
    Dim IndiceCoincidencia As Integer
    Dim cb As ComboBox
    Dim UltimoNumero As Double
    Dim ConexionLotes As String = ""
    Dim UltimoImporteRecibo As Double
    Dim Relacionada As Double
    Dim AsignadoEnRemito As Boolean
    Dim SenaTotal As Double
    Dim Bultos As Double
    Dim UltimaFechaW As DateTime
    Dim Lista As Integer
    Dim TieneListaDePrecios As Boolean
    Dim PorUnidadEnLista As Boolean
    Dim FinalEnLista As Boolean
    Dim TieneCodigoCliente As Boolean
    Dim ErrorImpresion As Boolean
    Dim TipoAsiento As Integer
    Dim TipoAsientoCosto As Integer
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim EsServicios As Boolean
    Dim EsSecos As Boolean
    Dim EsExterior As Boolean
    Private Sub UnaFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        With GridLeyenda.RowTemplate
            '    .DefaultCellStyle.BackColor = Color.Bisque
            .Height = 15
            .MinimumHeight = 15
        End With
        GridLeyenda.Rows.Add("")
        GridLeyenda.Rows.Add("")
        GridLeyenda.Rows.Add("")
        GridLeyenda.Rows.Add("")

        TextDirecto.Text = "0"
        Remito = 0
        AsignadoEnRemito = False

        If PFactura = 0 Then
            PideDatosEmisor()
            If PCliente = 0 Then Me.Close() : Exit Sub
        End If

        LlenaComboTablas(ComboDeposito, 19)

        LlenaComboTablas(ComboVendedor, 37)
        ComboVendedor.SelectedValue = 0
        With ComboVendedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboIncoTerm, 38)
        ComboIncoTerm.SelectedValue = 0
        With ComboIncoTerm
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboCliente, "", "Clientes")

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboSucursal
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        ComboEstado.DataSource = DtAfectaPendienteAnulada()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"
        ComboEstado.SelectedValue = 0

        ArmaTipoIva(ComboTipoIva)

        LlenaComboTablas(ComboPais, 28)

        If AbiertoRemito Then
            ConexionRemito = Conexion
        Else : ConexionRemito = ConexionN
        End If

        PActualizacionOk = False
        GModificacionOk()
        If Not MuestraDatos() Then Me.Close() : Exit Sub

        If Remito <> 0 Then
            ArmaListadeLotesConRemito()
            ArmaFacturaConRemito()
            If ListaDeLotes.Count <> 0 And PFactura = 0 Then ComboEstado.SelectedValue = 1 : Grid.Refresh()
            Grid.Columns("Cantidad").ReadOnly = True
            Grid.Columns("Articulo").ReadOnly = True
            ButtonEliminarLinea.Visible = False
            If PFactura = 0 And Not AbiertoRemito Then
                TextDirecto.Text = Format(0, "0.00")
                TextDirecto.ReadOnly = True
                TextAutorizar.Text = Format(100, "0.00")
            End If
            CalculaSubTotal()
        End If

        Lista = 0
        If PFactura = 0 And TieneListaDePrecios Then
            If Remito <> 0 Then
                If HallaLista(PCliente, DtCabezaRemito.Rows(0).Item("Fecha"), Lista, PorUnidadEnLista, FinalEnLista) < 0 Then Me.Close() : Exit Sub
            Else
                If HallaLista(PCliente, DateTime1.Value, Lista, PorUnidadEnLista, FinalEnLista) < 0 Then Me.Close() : Exit Sub
            End If
            If Lista = 0 Then
                MsgBox("Falta Definir Lista de Precios para este Cliente y Fecha de la Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Exit Sub
            Else
                If FinalEnLista Then
                    RadioFinal.Checked = True
                    RadioSinIva.Enabled = False
                Else
                    RadioSinIva.Checked = True
                    RadioFinal.Enabled = False
                End If
                If PorUnidadEnLista Then
                    RadioPorUnidad.Enabled = True
                    RadioPorKilo.Enabled = False
                Else
                    RadioPorKilo.Checked = True
                    RadioPorUnidad.Enabled = False
                End If
            End If
        End If

        ListaDePrecios.Clear()
        If Lista <> 0 Then
            Dim Precio As Double
            For Each Row In DtDetalle.Rows
                Precio = ArmaListaDePrecios(Row("Articulo"), DtCabezaRemito.Rows(0).Item("Fecha"), Row("Iva"))
                If Precio = 0 Then
                    MsgBox("Existe Articulos sin Precio en Lista de Precios definida para este Cliente.")
                    Me.Close() : Exit Sub
                End If
            Next
            Grid.Columns("Precio").ReadOnly = True
            CalculaSubTotal()
        End If

        LlenaCombosGrid()

        If EsServicios Or EsSecos Then
            Grid.Columns("KilosXUnidad").ReadOnly = True
            ButtonReAsignaLotes.Enabled = False
            Panel4.Enabled = False
            CheckSenia.Checked = False
            CheckSenia.Enabled = False
            ButtonNetoPorLotes.Visible = False
        End If

        If EsSecos Then
            Panel7.Visible = True
        End If

        UltimaFechaW = UltimaFecha(Conexion)
        If UltimaFechaW = "2/1/1000" Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnaFactura_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        If PFactura = 0 Then
            HacerAlta()
        Else : HacerModificacion()
        End If

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

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Factura ya esta ANULADA. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("EsExterior") Then
            MsgBox("Factura Exportación No Puede Anularse. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("ImporteDev") <> 0 Then
            MsgBox("Factura Tiene Notas de Credito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCabezaRel As New DataTable

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Recibo") <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Anular Recibo antes de Anular Factura. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Recibo") <> 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Debe Anular Recibo antes de Anular Factura Relacionada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If DtCabeza.Rows(0).Item("Importe") <> DtCabeza.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Importe") <> DtCabezaRel.Rows(0).Item("Saldo") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        Dim DtRemitoCabeza As New DataTable
        Dim DtRemitoLotes As New DataTable

        Remito = DtCabeza.Rows(0).Item("Remito")
        If Remito = 0 And DtCabezaRel.Rows.Count <> 0 Then
            Remito = DtCabezaRel.Rows(0).Item("Remito")
        End If

        If Remito <> 0 Then
            If Not HallaAsignacionLotesRemito(Remito, DtRemitoCabeza, DtRemitoLotes, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If ComboEstado.SelectedValue = 1 And DtRemitoLotes.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Reingresar Lotes al Stock. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        DtCabezaAux.Rows(0).Item("Estado") = 3
        DtCabezaAux.Rows(0).Item("Saldo") = DtCabezaAux.Rows(0).Item("Importe")
        DtCabezaAux.Rows(0).Item("Remito") = 0

        If Relacionada <> 0 Then
            DtCabezaRel.Rows(0).Item("Estado") = 3
            DtCabezaRel.Rows(0).Item("Saldo") = DtCabezaRel.Rows(0).Item("Importe")
            DtCabezaRel.Rows(0).Item("Remito") = 0
        End If

        If DtRemitoCabeza.Rows.Count <> 0 Then
            DtRemitoCabeza.Rows(0).Item("Factura") = 0
            For Each Row As DataRow In DtRemitoLotes.Rows
                Row("Facturado") = False
            Next
        End If

        Dim DtAsignacionLotesRel As New DataTable

        If DtCabezaRel.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 and Comprobante = " & Relacionada & ";", ConexionRelacionada, DtAsignacionLotesRel) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In DtAsignacionLotes.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsignacionLotesRel.Rows
            Row.Delete()
        Next

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoCabezaCostoRel As New DataTable
        Dim DtAsientoCabezaRemito As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(TipoAsiento, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabeza, ConexionFactura) Then Me.Close() : Exit Sub
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(TipoAsiento, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaRel, ConexionRelacionada) Then Me.Close() : Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaRel.Rows.Count <> 0 Then DtAsientoCabezaRel.Rows(0).Item("Estado") = 3
            '
            If DtCabeza.Rows(0).Item("Remito") = 0 Or (DtCabeza.Rows(0).Item("Remito") <> 0 And DtRemitoLotes.Rows.Count = 0) Then
                If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabeza.Rows(0).Item("Factura"), DtAsientoCabezaCosto, ConexionFactura) Then Me.Close() : Exit Sub
                If DtCabezaRel.Rows.Count <> 0 Then
                    If Not HallaAsientosCabeza(TipoAsientoCosto, DtCabezaRel.Rows(0).Item("Factura"), DtAsientoCabezaCostoRel, ConexionRelacionada) Then Me.Close() : Exit Sub
                End If
                If DtAsientoCabezaCosto.Rows.Count <> 0 Then DtAsientoCabezaCosto.Rows(0).Item("Estado") = 3
                If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then DtAsientoCabezaCostoRel.Rows(0).Item("Estado") = 3
            End If
            If Remito <> 0 And DtRemitoLotes.Rows.Count = 0 Then
                If Not HallaAsientosCabeza(6060, Remito, DtAsientoCabezaRemito, ConexionRemito) Then Me.Close() : Exit Sub
                If DtAsientoCabezaRemito.Rows.Count <> 0 Then DtAsientoCabezaRemito.Rows(0).Item("Estado") = 1
            End If
        End If

        If MsgBox("Factura se Anulara. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    If GrabaTabla(DtCabezaAux.GetChanges, "FacturasCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotes.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotes.GetChanges, "AsignacionLotes", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    If GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesRel.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesRel.GetChanges, "AsignacionLotes", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtRemitoCabeza.GetChanges) Then
                    If GrabaTabla(DtRemitoCabeza.GetChanges, "RemitosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtRemitoLotes.GetChanges) Then
                    If GrabaTabla(DtRemitoLotes.GetChanges, "AsignacionLotes", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCostoRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCostoRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRemito.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRemito.GetChanges, "AsientosCabeza", ConexionRemito) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                PActualizacionOk = True
                MsgBox("Baja Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not DtCabeza.Rows(0).Item("EsExterior") Then
            MsgBox("Solo Facturas Electrónicas Puede Borrase. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("ImporteDev") <> 0 Then
            MsgBox("Factura Tiene Notas de Credito. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 1 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Debe Previamente Reingresar Lotes al Stock. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCabezaRel As New DataTable
        Dim DtDetalleRel As New DataTable

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtDetalleRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Importe") <> DtCabeza.Rows(0).Item("Saldo") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtCabezaRel.Rows.Count <> 0 Then
            If DtCabezaRel.Rows(0).Item("Importe") <> DtCabezaRel.Rows(0).Item("Saldo") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Factura Tiene Imputaciones. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        Dim FacturaW As Double = 0
        If Relacionada = 0 Then
            FacturaW = DtCabeza.Rows(0).Item("Factura")
        Else
            If ConexionRelacionada = Conexion Then
                FacturaW = Relacionada
            Else
                FacturaW = DtCabeza.Rows(0).Item("Factura")
            End If
        End If

        If HallaCierreFactura(FacturaW) <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Factura Tiene Cierre de Factura. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        Dim DtAsientoCabezaRel As New DataTable
        Dim DtAsientoDetalleRel As New DataTable
        Dim DtAsientoCabezaCosto As New DataTable
        Dim DtAsientoDetalleCosto As New DataTable
        Dim DtAsientoCabezaCostoRel As New DataTable
        Dim DtAsientoDetalleCostoRel As New DataTable

        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsiento & " AND Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabeza.Rows(0).Item("Asiento") & ";", ConexionFactura, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                DtAsientoCabeza.Rows(0).Delete()
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row.Delete()
                Next
            End If
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsiento & " AND Documento = " & DtCabezaRel.Rows(0).Item("Factura") & ";", ConexionRelacionada, DtAsientoCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                If DtAsientoCabezaRel.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaRel.Rows(0).Item("Asiento") & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    DtAsientoCabezaRel.Rows(0).Delete()
                    For Each Row As DataRow In DtAsientoDetalleRel.Rows
                        Row.Delete()
                    Next
                End If
            End If
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsientoCosto & " AND Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoCabezaCosto) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If DtAsientoCabezaCosto.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCosto.Rows(0).Item("Asiento") & ";", ConexionFactura, DtAsientoDetalleCosto) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                DtAsientoCabezaCosto.Rows(0).Delete()
                For Each Row As DataRow In DtAsientoDetalleCosto.Rows
                    Row.Delete()
                Next
            End If
            If DtCabezaRel.Rows.Count <> 0 Then
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsientoCosto & " AND Documento = " & DtCabezaRel.Rows(0).Item("Factura") & ";", ConexionRelacionada, DtAsientoCabezaCostoRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                If DtAsientoCabezaCostoRel.Rows.Count <> 0 Then
                    If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaCostoRel.Rows(0).Item("Asiento") & ";", ConexionRelacionada, DtAsientoDetalleCostoRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    DtAsientoCabezaCostoRel.Rows(0).Delete()
                    For Each Row As DataRow In DtAsientoDetalleCostoRel.Rows
                        Row.Delete()
                    Next
                End If
            End If
        End If

        Dim DtCabezaAux As DataTable = DtCabeza.Copy
        Dim DtDetalleAux As DataTable = DtDetalle.Copy

        DtCabezaAux.Rows(0).Delete()
        For Each Row As DataRow In DtDetalle.Rows
            Row.Delete()
        Next

        If Relacionada <> 0 Then
            DtCabezaRel.Rows(0).Delete()
            For Each Row As DataRow In DtDetalleRel.Rows
                Row.Delete()
            Next
        End If

        Dim DtAsignacionLotesRel As New DataTable

        If DtCabezaRel.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 and Comprobante = " & Relacionada & ";", ConexionRelacionada, DtAsignacionLotesRel) Then Me.Close() : Exit Sub
        End If
        For Each Row As DataRow In DtAsignacionLotes.Rows
            Row.Delete()
        Next
        For Each Row As DataRow In DtAsignacionLotesRel.Rows
            Row.Delete()
        Next

        If MsgBox("Factura se Borrara del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    If GrabaTabla(DtCabezaAux.GetChanges, "FacturasCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtDetalleAux.GetChanges, "FacturasDetalle", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotes.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotes.GetChanges, "AsignacionLotes", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    If GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtDetalleRel.GetChanges, "FacturasDetalle", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsignacionLotesRel.GetChanges) Then
                    If GrabaTabla(DtAsignacionLotesRel.GetChanges, "AsignacionLotes", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                '
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    If GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtAsientoDetalleRel.GetChanges, "AsientosDetalle", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCosto.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCosto.GetChanges, "AsientosCabeza", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtAsientoDetalleCosto.GetChanges, "AsientosDetalle", ConexionFactura) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If
                If Not IsNothing(DtAsientoCabezaCostoRel.GetChanges) Then
                    If GrabaTabla(DtAsientoCabezaCostoRel.GetChanges, "AsientosCabeza", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If GrabaTabla(DtAsientoDetalleCostoRel.GetChanges, "AsientosDetalle", ConexionRelacionada) <= 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                PActualizacionOk = True
                MsgBox("Factura Borrada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Me.Close()
                Exit Sub
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonRecibo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRecibo.Click

        If GCaja = 0 Then
            MsgBox("Usuario No Tiene Caja.")
            Me.Close()
            Exit Sub
        End If

        If PFactura <> 0 And DtCabeza.Rows(0).Item("Recibo") <> 0 Then
            UnRecibo.PAbierto = PAbierto
            UnRecibo.PNota = DtCabeza.Rows(0).Item("Recibo")
            UnRecibo.PTipoNota = 60
            UnRecibo.PBloqueaFunciones = True
            UnRecibo.ShowDialog()
            UnRecibo.Dispose()
            Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            MsgBox("Recibo para Exportación debe Realizarce Por Tesoreria. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Debe Grabar Factura. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Factura Esta Anulada. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Recibo") = 0 Then
            If DtCabeza.Rows(0).Item("Importe") <> DtCabeza.Rows(0).Item("Saldo") Then
                MsgBox("Factura Ya Tiene Pago. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If Not RadioMixto.Checked And Not RadioContadoEfectivo.Checked Then
                MsgBox("Debe Informar Forma de Pago. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If DtCabeza.Rows(0).Item("Recibo") = 0 Then UnReciboPagoFactura.PEsPagoEfectivo = RadioContadoEfectivo.Checked
        UnReciboPagoFactura.PEmisor = PCliente
        UnReciboPagoFactura.PNota = DtCabeza.Rows(0).Item("Recibo")
        UnReciboPagoFactura.PFactura = PFactura
        UnReciboPagoFactura.PAbierto = PAbierto
        UnReciboPagoFactura.PEsSecos = EsSecos
        UnReciboPagoFactura.ShowDialog()
        If UnReciboPagoFactura.PActualizacionOk Then
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If
        UnReciboPagoFactura.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PFactura = 0
        UnaFactura_Load(Nothing, Nothing)
        TextDirecto.Focus()

    End Sub
    Private Sub ButtonReAsignaLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReAsignaLotes.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        If PFactura = 0 Then
            MsgBox("Factura debe ser Grabada Previamente. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Estado") = 3 Then
            MsgBox("Factura Esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Dim DtRemitoLotes As New DataTable
        Dim DtRemitoCabeza As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtCabezaRel As New DataTable

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Remito = DtCabeza.Rows(0).Item("Remito")
        If Remito = 0 And DtCabezaRel.Rows.Count <> 0 Then
            Remito = DtCabezaRel.Rows(0).Item("Remito")
        End If

        If Remito <> 0 Then
            If Not HallaAsignacionLotesRemito(Remito, DtRemitoCabeza, DtRemitoLotes, ConexionRemito) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtRemitoLotes.Rows.Count <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Asignación de Lotes Fue Realizada en su Remito. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        UnaReAsignacionFactura.PFactura = PFactura
        UnaReAsignacionFactura.PAbierto = PAbierto
        UnaReAsignacionFactura.PtipoAsiento = TipoAsiento
        UnaReAsignacionFactura.PtipoAsientoCosto = TipoAsientoCosto
        GModificacionOk = False
        UnaReAsignacionFactura.ShowDialog()
        If GModificacionOk Then PActualizacionOk = True : MuestraDatos()
        UnaReAsignacionFactura.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PFactura = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = TipoAsiento
        If PAbierto Then
            ListaAsientos.PDocumentoB = PFactura
        Else
            ListaAsientos.PDocumentoN = PFactura
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.PFactura = PFactura
        SeleccionarVarios.PEsNetoPorLotesFacturaVenta = True
        SeleccionarVarios.Show()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        'http://vb-helper.com/howto_net_print_and_preview.html

        If PFactura = 0 Then
            MsgBox("Opcion Invalida. Factura debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtCabeza.Rows(0).Item("Impreso") And PAbierto Then
            MsgBox("Factura Ya fue Impresa. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TieneCodigoCliente Then
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                Dim Codigo As Double = HallaCodigoCliente(PCliente, Row.Cells("Articulo").Value)
                If Codigo <= 0 Then
                    MsgBox("Articulo " & Row.Cells("Articulo").FormattedValue & " No Tiene Codigo Cliente.", MsgBoxStyle.Information)
                    Exit Sub
                End If
                Row.Cells("CodigoCliente").Value = Codigo
            Next
        End If

        ErrorImpresion = False
        Paginas = 0
        Copias = 1

        If PAbierto Then
            Copias = 3
        Else
            Copias = 1
        End If

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_PrintPage

        print_document.Print()

        If ErrorImpresion Then Exit Sub

        If Not GrabaImpreso() Then Exit Sub
        DtCabeza.Rows(0).Item("Impreso") = True

    End Sub
    Private Sub ComboSucursal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboSucursal.Validating

        If IsNothing(ComboSucursal.SelectedValue) Then ComboSucursal.SelectedValue = 0

        If ComboSucursal.SelectedValue <> 0 Then
            TextDestino.Text = HallaDireccionSucursalCliente(PCliente, ComboSucursal.SelectedValue)
            If TextDestino.Text = "-1" Then
                Me.Close() : Exit Sub
            End If
        Else
            TextDestino.Text = ""
        End If

    End Sub
    Public Function GrabaImpreso() As Boolean

        Dim Sql As String = "UPDATE FacturasCabeza Set Impreso = 1 WHERE Factura = " & DtCabeza.Rows(0).Item("Factura") & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionFactura)
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Miconexion.Open()
                If CInt(Cmd.ExecuteNonQuery()) = 0 Then Return False
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos al Grabar Factura. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End Try

        Return True

    End Function
    Private Sub PictureAlmanaque_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaque.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaAfip.Text = ""
        Else : TextFechaAfip.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
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
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        IndiceCoincidencia = Grid.CurrentRow.Cells("Indice").Value
        ListaDeLotes.RemoveAll(AddressOf Coincidencia)
        Grid.Rows.Remove(Grid.CurrentRow)
        CalculaSubTotal()

    End Sub
    Private Sub TextDirecto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDirecto.KeyPress

        If Asc(e.KeyChar) = 13 Then TextDirecto_Validating(Nothing, Nothing) : Exit Sub

        EsNumerico(e.KeyChar, TextDirecto.Text, GDecimales)

    End Sub
    Private Sub TextDirecto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextDirecto.Validating

        If Not IsNumeric(TextDirecto.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextDirecto.Focus()
            Exit Sub
        Else : TextDirecto.Text = FormatNumber(TextDirecto.Text, GDecimales, True, True, True)
            If CDbl(TextDirecto.Text) > 100 Then
                MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextDirecto.Focus()
                Exit Sub
            End If
            TextAutorizar.Text = FormatNumber(100 - CDbl(TextDirecto.Text), GDecimales, True, True, True)
        End If

        CalculaSubTotal()

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 0

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub TextCambio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCambio.Validating

        If TextCambio.Text = "" Then Exit Sub

        If CDbl(TextCambio.Text) = 0 Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCambio.Text = ""
            TextCambio.Focus()
        End If

    End Sub
    Private Sub ComboVendedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVendedor.Validating

        If IsNothing(ComboVendedor.SelectedValue) Then ComboVendedor.SelectedValue = 0

    End Sub
    Private Sub ComboIncoTerm_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboIncoTerm.Validating

        If IsNothing(ComboIncoTerm.SelectedValue) Then ComboIncoTerm.SelectedValue = 0

    End Sub
    Private Sub RadioFinal_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioFinal.CheckedChanged

        CalculaSubTotal()

    End Sub
    Private Sub RadioPorUnidad_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RadioPorUnidad.CheckedChanged

        If RadioPorUnidad.Checked = False Then
            Grid.Columns("Precio").HeaderText = "Precio xKgs"
        Else : Grid.Columns("Precio").HeaderText = "Precio xUni"
        End If

        CalculaSubTotal()

    End Sub
    Private Sub CheckSenia_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckSenia.CheckedChanged

        CalculaSubTotal()

    End Sub
    Private Sub ButtonRelacionada_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRelacionada.Click

        PFactura = Relacionada
        PAbierto = Not PAbierto
        If Not MuestraDatos() Then Me.Close() : Exit Sub

    End Sub
    Private Function MuestraDatos() As Boolean

        Dim Sql As String

        If PFactura = 0 Then PAbierto = True

        If PAbierto Then
            ConexionFactura = Conexion
            ConexionRelacionada = ConexionN
        Else
            ConexionFactura = ConexionN
            ConexionRelacionada = Conexion
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Sql = "SELECT * FROM FacturasCabeza WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtCabeza) Then Return False
        If PFactura <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Factura No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return False
        End If

        If DtCabeza.Rows.Count <> 0 Then
            PCliente = DtCabeza.Rows(0).Item("Cliente")
        End If

        ComboSucursal.DataSource = Nothing
        ComboSucursal.DataSource = Tablas.Leer("SELECT Nombre,Clave FROM SucursalesClientes WHERE Estado = 1 AND Cliente = " & PCliente & ";")
        Dim Row As DataRow = ComboSucursal.DataSource.NewRow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboSucursal.DataSource.Rows.Add(Row)
        ComboSucursal.DisplayMember = "Nombre"
        ComboSucursal.ValueMember = "Clave"
        ComboSucursal.SelectedValue = 0

        If Not LlenaDatosCliente(PCliente) Then Return False

        If ComboTipoIva.SelectedValue = Exterior Then
            EsExterior = True
            PanelMoneda.Visible = True
            ButtonBorrar.Visible = True
            ButtonAnula.Visible = False
        Else : PanelMoneda.Visible = False
            EsExterior = False
            ButtonBorrar.Visible = False
            ButtonAnula.Visible = True
        End If

        If PFactura = 0 Then
            If Not AgregaCabeza() Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        Relacionada = DtCabeza.Rows(0).Item("Relacionada")
        EsServicios = DtCabeza.Rows(0).Item("EsServicios")
        EsSecos = DtCabeza.Rows(0).Item("EsSecos")

        'Halla Relacionada.
        If PAbierto And DtCabeza.Rows(0).Item("Rel") And PermisoTotal And PFactura <> 0 Then
            Relacionada = HallaRelacionada(PFactura)
            If Relacionada < 0 Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        EnlazaCabeza()

        DtDetalle = New DataTable
        Sql = "SELECT * FROM FacturasDetalle WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        'Arma tabla con AsignacionLotes de facturas. 
        DtAsignacionLotes = New DataTable
        ListaDeLotes = New List(Of FilaAsignacion)
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtAsignacionLotes) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If DtAsignacionLotes.Rows.Count <> 0 Then
            For Each Row In DtAsignacionLotes.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Deposito = Row("Deposito")
                Fila.Operacion = Row("Operacion")
                Fila.Asignado = Row("Cantidad")
                Fila.Importe = Row("Importe")
                Fila.ImporteSinIva = Row("ImporteSinIva")
                'Muestra Permiso de Importacion.
                Fila.PermisoImp = HallaPermisoImp(Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Deposito)
                If Fila.PermisoImp = "-1" Then
                    MsgBox("Error, Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No Encontrado.")
                    Me.Close() : Exit Function
                End If
                ListaDeLotes.Add(Fila)
            Next
        End If

        If PFactura = 0 Then
            LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")
        Else : LabelPuntodeVenta.Text = ""
        End If

        If Relacionada <> 0 Then
            If PermisoTotal Then
                ButtonRelacionada.Visible = True
            Else : ButtonRelacionada.Visible = False
            End If
        Else : ButtonRelacionada.Visible = False
        End If

        IndiceW = 0
        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        If PermisoTotal Then
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        Grid.Columns("Indice").Visible = False
        Grid.Columns("Precio").HeaderText = "Precio xUni"

        Dim TotalNeto As Double = 0
        Dim TotalIva As Double = 0

        For I As Integer = 0 To Grid.Rows.Count - 2
            Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value)
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value, Grid.Rows(I).Cells("Iva").Value)
            TotalIva = TotalIva + Grid.Rows(I).Cells("MontoIva").Value
        Next
        TextTotalNeto.Text = FormatNumber(TotalNeto, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)
        TextSubTotal.Text = FormatNumber(TotalNeto + TotalIva, GDecimales)

        If PFactura = 0 Then
            Panel3.Visible = False
        Else
            Panel3.Visible = True
            If DtCabeza.Rows(0).Item("Recibo") <> 0 Then
                Panel3.Enabled = False
            Else : Panel3.Enabled = True
            End If
        End If

        If PFactura <> 0 Then
            ButtonAceptar.Text = "Modifica Factura"
            TextCambio.Enabled = False
            TextComentario.Enabled = False
            Panel4.Enabled = False
            Panel5.Enabled = False
            CheckSenia.Enabled = False
            ButtonEliminarLinea.Enabled = False
            PanelCandado.Visible = False
            Grid.Columns("Cantidad").ReadOnly = True
            Grid.Columns("Articulo").ReadOnly = True
            Grid.Columns("Precio").ReadOnly = True
            Grid.Columns("KilosXunidad").ReadOnly = True
            ComboSucursal.Enabled = False
            TextDestino.ReadOnly = True
        Else : ButtonAceptar.Text = "Graba Factura"
            TextCambio.Enabled = True
            TextComentario.Enabled = True
            Panel4.Enabled = True
            Panel5.Enabled = True
            CheckSenia.Enabled = True
            ButtonEliminarLinea.Enabled = True
            PictureCandado.Visible = False
            If PermisoTotal Then
                PanelCandado.Visible = True
            Else : PanelCandado.Visible = False
            End If
            CheckSenia.Checked = True
            Grid.Columns("Cantidad").ReadOnly = False
            Grid.Columns("Articulo").ReadOnly = False
            Grid.Columns("Precio").ReadOnly = False
            Grid.Columns("KilosXunidad").ReadOnly = False
            ComboSucursal.Enabled = True
            TextDestino.ReadOnly = False
        End If

        If PFactura = 0 And Lista <> 0 Then
            If FinalEnLista Then
                RadioFinal.Checked = True
                RadioSinIva.Enabled = False
            Else
                RadioSinIva.Checked = True
                RadioFinal.Enabled = False
            End If
            If PorUnidadEnLista Then
                RadioPorUnidad.Enabled = True
                RadioPorKilo.Enabled = False
            Else
                RadioPorKilo.Checked = True
                RadioPorUnidad.Enabled = False
            End If
        End If

        If DtCabeza.Rows(0).Item("EsExterior") Then
            TipoAsiento = 7006
            TipoAsientoCosto = 6071
        Else : TipoAsiento = 2
            TipoAsientoCosto = 6070
        End If
        If EsServicios Then
            TipoAsiento = 7009
        End If
        If EsSecos Then
            TipoAsiento = 7010
        End If

        LabelSena.Text = "Seña Por  " & DtCabeza.Rows(0).Item("Bultos") & " Bultos. "

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanging)
        AddHandler DtDetalle.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dtdetalle_ColumnChanged)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)
        AddHandler DtDetalle.RowChanged, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Factura")
        AddHandler Enlace.Format, AddressOf FormatFactura
        AddHandler Enlace.Parse, AddressOf FormatParseFactura
        MaskedFactura.DataBindings.Clear()
        MaskedFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Remito")
        AddHandler Enlace.Format, AddressOf FormatRemito
        TextRemito.DataBindings.Clear()
        TextRemito.DataBindings.Add(Enlace)

        If TextRemito.Text = "" Then PanelRemito.Visible = False

        Enlace = New Binding("SelectedValue", MiEnlazador, "Cliente")
        ComboCliente.DataBindings.Clear()
        ComboCliente.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Sucursal")
        ComboSucursal.DataBindings.Clear()
        ComboSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Vendedor")
        ComboVendedor.DataBindings.Clear()
        ComboVendedor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "IncoTerm")
        ComboIncoTerm.DataBindings.Clear()
        ComboIncoTerm.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        If Row("PorUnidad") Then
            RadioPorUnidad.Checked = True
        Else : RadioPorKilo.Checked = True
        End If
        If Row("Final") Then
            RadioFinal.Checked = True
        Else : RadioSinIva.Checked = True
        End If

        If Row("FormaPago") = 2 Then
            RadioCtaCte.Checked = True
        End If
        If Row("FormaPago") = 3 Then
            RadioMixto.Checked = True
        End If
        If Row("FormaPago") = 4 Then
            RadioContadoEfectivo.Checked = True
        End If

        Enlace = New Binding("Text", MiEnlazador, "Senia")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextSenia.DataBindings.Clear()
        TextSenia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextTotalFactura.DataBindings.Clear()
        TextTotalFactura.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Destino")
        TextDestino.DataBindings.Clear()
        TextDestino.DataBindings.Add(Enlace)

        If Row("FechaElectronica") = "01/01/1800" Then
            TextFechaAfip.Text = ""
        Else : TextFechaAfip.Text = Format(Row("FechaElectronica"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub FormatFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TipoFactura = Strings.Left(Numero.Value, 1)
        TextTipoFactura.Text = LetraTipoIva(TipoFactura)
        Numero.Value = Strings.Right(Numero.Value, 12)

    End Sub
    Private Sub FormatParseFactura(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedFactura.Text), "000000000000"))

    End Sub
    Private Sub FormatRemito(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 3)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Function LlenaDatosCliente(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & PCliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dta.Dispose()
            Return False
        End If

        TextCalle.Text = Dta.Rows(0).Item("Calle")
        TextLocalidad.Text = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextTelefonos.Text = Dta.Rows(0).Item("Telefonos")
        TextFaxes.Text = Dta.Rows(0).Item("Faxes")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        TextTipoFactura.Text = LetraTipoIva(Dta.Rows(0).Item("TipoIva"))
        TipoFactura = Dta.Rows(0).Item("TipoIva")
        TieneListaDePrecios = Dta.Rows(0).Item("ListaDePrecios")
        '
        If TipoFactura = 3 Then TipoFactura = 2
        '
        CtaCteCerrada = Dta.Rows(0).Item("CtaCteCerrada")
        TieneCodigoCliente = Dta.Rows(0).Item("TieneCodigoCliente")
        ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")

        If PFactura = 0 Then
            TextDirecto.Text = Format(Dta.Rows(0).Item("Directo"), "0.00")
            TextAutorizar.Text = Format(100 - Dta.Rows(0).Item("Directo"), "0.00")
            If Not PermisoTotal Then
                TextDirecto.Text = Format(100, "0.00")
                TextAutorizar.Text = Format(0, "0.00")
            End If
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Function HallaAsignacionLotesRemito(ByVal Remito As Double, ByRef DtRemitoCabezaW As DataTable, ByRef DtAsignacion As DataTable, ByRef ConexionRemito As String) As Boolean

        If DtCabeza.Rows(0).Item("Rel") Then ConexionRemito = Conexion
        If Not DtCabeza.Rows(0).Item("Rel") And Not PAbierto Then ConexionRemito = ConexionN

        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & Remito, ConexionRemito, DtRemitoCabezaW) Then Return False
        If DtRemitoCabezaW.Rows.Count = 0 Then
            If ConexionRemito = Conexion Then
                ConexionRemito = ConexionN
            Else : ConexionRemito = Conexion
            End If
            If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & Remito, ConexionRemito, DtRemitoCabezaW) Then Return False
            If DtRemitoCabezaW.Rows.Count = 0 Then Return False
        End If

        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Remito & ";", ConexionRemito, DtAsignacion) Then Return False

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        If EsServicios Then
            Articulo.DataSource = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 0;")
        Else
            If EsSecos Then
                Articulo.DataSource = Tablas.Leer("SELECT * FROM ArticulosServicios WHERE Secos = 1;")
            Else
                Articulo.DataSource = TodosLosArticulos()
            End If
        End If

        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub Print_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20
        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 10)
        Dim SaltoLinea As Integer = 4

        x = MIzq + 10 : y = MTop + 44

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            Texto = DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 120, y - 30)
            'Titulos.
            If PAbierto Then
                Texto = "CLIENTE    : " & ComboCliente.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOMICILIO  : " & TextCalle.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "LOCALIDAD  : " & TextLocalidad.Text & " " & TextProvincia.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "DOM.ENTREGA: " & ComboSucursal.Text & "  " & TextDestino.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                Texto = "CUIT       : " & TextCuit.Text & " " & ComboTipoIva.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                y = y + SaltoLinea
                'Condicion de venta.
                Texto = ""
                If DtCabeza.Rows(0).Item("FormaPago") = 1 Then
                    Texto = "CONDICION DE VENTA: Contado"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 2 Then
                    Texto = "CONDICION DE VENTA: Cuenta Corriente"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 3 Then
                    Texto = "CONDICION DE VENTA: Mixta"
                End If
                If DtCabeza.Rows(0).Item("FormaPago") = 4 Then
                    Texto = "CONDICION DE VENTA: Contado Efectivo"
                End If
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
                Texto = "REMITO: " & TextRemito.Text
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 115, y)
            Else
                Texto = "CLIENTE    : " & ComboCliente.Text & "  Nro.: " & NumeroEditado(DtCabeza.Rows(0).Item("Factura"))
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, y)
            End If

            'Grafica -Rectangulos-.
            x = MIzq + 10
            y = MTop + 74
            Dim Ancho As Integer = 178
            Dim Alto As Integer = 125
            Dim LineaArticulo As Integer = x + 90
            Dim LineaCantidad As Integer = x + 110
            Dim LineaUnitario As Integer = x + 143
            Dim LineaImporte As Integer = x + Ancho
            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer = y - SaltoLinea

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), LineaUnitario, y, LineaUnitario, y + Alto)
            'Titulos de articulo.
            Texto = "ARTICULO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x + 1, y + 2)
            Texto = "CANTIDAD"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaUnitario - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.3), x, y, x + Ancho, y)
            Dim Partes() As String
            Dim Articulo As String
            'Descripcion de Articulos.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Articulo").Value) Then Exit For
                If Row.Cells("Articulo").Value = 0 Then Exit For 'para cuando deja blancos al final.
                Yq = Yq + SaltoLinea
                'Imprime Articulo.
                Partes = Split(Row.Cells("Articulo").FormattedValue, "(")
                Articulo = Partes(0)
                If TieneCodigoCliente Then
                    Articulo = Format(Val(Row.Cells("CodigoCliente").Value), "00000000") & "  " & Articulo
                End If
                Texto = Articulo
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime cantidad.
                Dim Cantidad As Double
                If RadioPorKilo.Checked Then
                    Cantidad = Row.Cells("Cantidad").Value * Row.Cells("KilosXUnidad").Value
                Else : Cantidad = Row.Cells("Cantidad").Value
                End If
                Texto = Cantidad.ToString
                If RadioPorKilo.Checked Then
                    Texto = Texto & "Kg."
                Else : Texto = Texto & "Uni"
                End If
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Unitario.
                Dim Precio As Double = 0
                If ComboTipoIva.SelectedValue = 3 Or ComboTipoIva.SelectedValue = 2 Then
                    Precio = Row.Cells("TotalArticulo").Value / Cantidad
                    If RadioPorKilo.Checked Then
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "xKg."
                    Else
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "xUni"
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Row.Cells("TotalArticulo").Value, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    Precio = Row.Cells("Precio").Value
                    If RadioPorKilo.Checked Then
                        Precio = Precio / Row.Cells("KilosXUnidad").Value
                        Texto = FormatNumber(FormatoSinRedondeo3Decimales(Precio), 3) & "xKg."
                    Else
                        Texto = FormatNumber(Precio, 3) & "xUni"
                    End If
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaUnitario - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Row.Cells("Neto").Value, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Resuardo
            Yq = MTop + 199
            If PAbierto Then
                Texto = GNombreEmpresa & " " & "FC " & NumeroEditado(MaskedFactura.Text)
            Else
                Texto = "FC " & NumeroEditado(MaskedFactura.Text)
            End If
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)
            'Totales
            PrintFont = New Font("Courier New", 10)
            Yq = MTop + 204
            If ComboTipoIva.SelectedValue = 3 Or ComboTipoIva.SelectedValue = 2 Then
                'Neto
                Texto = "Sub-Total"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextSubTotal.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Else
                'Neto
                Texto = "Neto"
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                Texto = TextTotalNeto.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                '
                Dim ListaIva As New List(Of ItemIva)
                ArmaListaImportesIva(ListaIva)
                'Iva.
                For Each Fila As ItemIva In ListaIva
                    Yq = Yq + SaltoLinea
                    Texto = "IVA. " & FormatNumber(Fila.Iva, GDecimales)
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
                    Texto = FormatNumber(Fila.Importe, GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Next
                ListaIva = Nothing
            End If

            'Seña
            Yq = Yq + SaltoLinea
            Texto = "Seña"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = TextSenia.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Total
            Yq = Yq + SaltoLinea
            Texto = "Total"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, LineaCantidad + 5, Yq)
            Texto = TextTotalFactura.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            'Imprime Leyenda.
            Yq = MTop + 204
            For Each Row As DataGridViewRow In GridLeyenda.Rows
                Texto = Row.Cells("Leyenda").Value
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, x, Yq)
                Yq = Yq + SaltoLinea
            Next

            'Imprime Permisos de Importacion.
            Dim PermisoImportacion(0) As String
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.PermisoImp <> "" Then
                    ReDim Preserve PermisoImportacion(UBound(PermisoImportacion) + 1)
                    PermisoImportacion(UBound(PermisoImportacion)) = Fila.PermisoImp
                End If
            Next
            Yq = MTop + 227
            e.Graphics.DrawString("Permiso Imp.:  ", PrintFont, Brushes.Black, x, Yq)
            Dim TextoPermiso As String = ""
            PrintFont = New Font("Courier New", 7)
            For K As Integer = 1 To UBound(PermisoImportacion)
                TextoPermiso = TextoPermiso & " " & PermisoImportacion(K)
                If K = 3 Then
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, Yq)
                    TextoPermiso = ""
                End If
                If K = 6 Or K = 9 Then
                    Yq = Yq + SaltoLinea
                    e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, Yq)
                    TextoPermiso = ""
                End If
                If K = 9 Then Exit For
            Next
            If TextoPermiso <> "" Then
                Yq = Yq + SaltoLinea
                e.Graphics.DrawString(TextoPermiso, PrintFont, Brushes.Black, x + 29, Yq)
            End If
            PrintFont = New Font("Courier New", 10)
            '-----------------------------------------------------------
            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ErrorImpresion = True
            e.HasMorePages = False
        End Try

    End Sub
    Private Function FormatoSinRedondeo3Decimales(ByVal Numero As Double) As Double

        Dim PosicionDecimal As Integer = InStr(1, Numero.ToString, ",")
        Return CDbl(Mid(Numero.ToString, 1, PosicionDecimal + 3))

    End Function
    Private Sub ArmaListaImportesIva(ByRef Lista As List(Of ItemIva))

        Dim Esta As Boolean

        Lista = New List(Of ItemIva)

        For Each Row As DataGridViewRow In Grid.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            Esta = False
            For Each Fila As ItemIva In Lista
                If Fila.Iva = Row.Cells("Iva").Value Then
                    Fila.Importe = Fila.Importe + Row.Cells("MontoIva").Value
                    Esta = True
                End If
            Next
            If Not Esta Then
                Dim Fila As New ItemIva
                Fila.Iva = Row.Cells("Iva").Value
                Fila.Importe = Row.Cells("MontoIva").Value
                Lista.Add(Fila)
            End If
        Next

    End Sub
    Private Function Coincidencia(ByVal Fila As FilaAsignacion) As Boolean

        If Fila.Indice = IndiceCoincidencia Then Return True

    End Function
    Private Function AgregaCabeza() As Boolean

        Dim Row As DataRow

        UltimoNumero = UltimaNumeracionFactura(TipoFactura, GPuntoDeVenta)
        If UltimoNumero < 0 Then
            MsgBox("Error Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Row = DtCabeza.NewRow()
        ArmaNuevaFactura(Row)
        Row("Factura") = UltimoNumero
        Row("Cliente") = PCliente
        Row("Sucursal") = 0
        Row("Remito") = Remito
        Row("Deposito") = Deposito
        Row("TipoIva") = ComboTipoIva.SelectedValue
        Row("Fecha") = DateTime1.Value
        Row("EsServicios") = EsServicios
        Row("EsSecos") = EsSecos
        Row("Estado") = 2
        If EsServicios Or EsSecos Then Row("Estado") = 2
        Row("Final") = 1
        Row("PorUnidad") = 1
        Row("Moneda") = ComboMoneda.SelectedValue
        If ComboMoneda.SelectedValue = 1 Then
            Row("Cambio") = 1
        Else : Row("Cambio") = 0
        End If
        Row("Impreso") = False
        Row("Destino") = ""
        DtCabeza.Rows.Add(Row)

        Return True

    End Function
    Private Sub AgregaADtDetalle(ByVal Articulo As Integer, ByVal KilosXUnidad As Double, ByVal Iva As Double)

        Dim Row As DataRow

        Row = DtDetalle.NewRow()
        IndiceW = IndiceW + 1
        Row("Indice") = IndiceW
        Row("Factura") = UltimoNumero
        Row("Articulo") = 0
        Row("KilosXUnidad") = 0
        Row("Iva") = Iva
        Row("Precio") = 0
        Row("Cantidad") = 0
        Row("TotalArticulo") = 0
        Row("Devueltas") = 0
        DtDetalle.Rows.Add(Row)

    End Sub
    Private Sub AgregaRemitoADtDetalle(ByVal Indice As Integer, ByVal Articulo As Integer, ByVal KilosXUnidad As Double, ByVal Iva As Double, ByVal Cantidad As Integer)

        Dim Row As DataRow

        Row = DtDetalle.NewRow()
        Row("Indice") = Indice
        Row("Articulo") = Articulo
        Row("Precio") = 0
        Row("Cantidad") = Cantidad
        Row("KilosXUnidad") = KilosXUnidad
        Row("Iva") = Iva
        Row("TotalArticulo") = 0
        Row("Devueltas") = 0
        DtDetalle.Rows.Add(Row)

    End Sub
    Private Sub ArmaFacturaConRemito()

        Dim sql As String = ""

        'Lee cabeza del remito.
        DtCabezaRemito = New DataTable
        sql = "SELECT * FROM RemitosCabeza WHERE Remito = " & Remito & ";"
        If Not Tablas.Read(sql, ConexionRemito, DtCabezaRemito) Then Me.Close() : Exit Sub
        If DtCabezaRemito.Rows(0).Item("Factura") <> 0 Then
            MsgBox("Remito Ya fue Facturado.Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Me.Close() : Exit Sub
        End If

        TextRemito.Text = Format(Remito, "0000-00000000")

        'Pasa detalle del remito al grid.
        Dim Dt As New DataTable
        sql = "SELECT * FROM RemitosDetalle WHERE Remito = " & Remito & ";"
        If Not Tablas.Read(sql, ConexionRemito, Dt) Then Me.Close() : Exit Sub

        '    bs.DataSource = Nothing
        '    Dim dt1 As DataTable = DtDetalle.Clone
        '    DtDetalle = New DataTable
        '    DtDetalle = dt1.Copy

        Dim Kilos As Double
        Dim Iva As Double
        For Each Row As DataRow In Dt.Rows
            HallaKilosIva(Row("Articulo"), Kilos, Iva)
            If (Row("Cantidad") - Row("Devueltas")) > 0 Then
                AgregaRemitoADtDetalle(Row("Indice"), Row("Articulo"), Row("KilosXUnidad"), Iva, Row("Cantidad") - Row("Devueltas"))
            End If
        Next

        Dt.Dispose()

    End Sub
    Private Sub ArmaListadeLotesConRemito()

        Dim Sql As String

        DtAsignacionLotesRemito = New DataTable
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionRemito, DtAsignacionLotesRemito) Then Me.Close() : Exit Sub

        ListaDeLotes = New List(Of FilaAsignacion)
        For Each Row As DataRow In DtAsignacionLotesRemito.Rows
            Dim Fila As New FilaAsignacion
            Fila.Indice = Row("Indice")
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.Deposito = Row("Deposito")
            Fila.Operacion = Row("Operacion")
            Fila.Asignado = Row("Cantidad")
            Fila.Importe2 = 0
            'Muestra Permiso de Importacion.
            Fila.PermisoImp = HallaPermisoImp(Fila.Operacion, Fila.Lote, Fila.Secuencia, Fila.Deposito)
            If Fila.PermisoImp = "-1" Then
                MsgBox("Error, Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No Encontrado.")
                Me.Close() : Exit Sub
            End If
            ListaDeLotes.Add(Fila)
        Next
        If ListaDeLotes.Count <> 0 Then AsignadoEnRemito = True

    End Sub
    Private Sub CalculaSubTotal()

        If PFactura <> 0 Then Exit Sub

        Dim Precio As Double = 0
        Dim TotalNeto As Double = 0
        Dim TotalIva As Double = 0

        TotalB = 0
        TotalN = 0
        Total = 0

        If Lista <> 0 Then
            For I As Integer = 0 To Grid.Rows.Count - 2
                For Each fila As ItemListaDePrecios In ListaDePrecios
                    If fila.Articulo = Grid.Rows(I).Cells("Articulo").Value Then
                        Grid.Rows(I).Cells("Precio").Value = fila.Precio
                        Exit For
                    End If
                Next
            Next
        End If

        For I As Integer = 0 To Grid.Rows.Count - 2
            If RadioPorUnidad.Checked Then
                Precio = Grid.Rows(I).Cells("Precio").Value
            Else : Precio = Grid.Rows(I).Cells("Precio").Value * Grid.Rows(I).Cells("KilosXUnidad").Value
            End If
            '
            If Not (PEsElectronica Or EsSecos) Then
                If RadioFinal.Checked Then
                    Grid.Rows(I).Cells("PrecioBlanco").Value = Trunca3(Precio * (CDbl(TextDirecto.Text) / 100) / (1 + Grid.Rows(I).Cells("Iva").Value / 100))
                    Grid.Rows(I).Cells("PrecioNegro").Value = Trunca3(Precio * (1 - CDbl(TextDirecto.Text) / 100))
                Else
                    Grid.Rows(I).Cells("PrecioBlanco").Value = Trunca3(Precio * (CDbl(TextDirecto.Text) / 100))
                    Grid.Rows(I).Cells("PrecioNegro").Value = Trunca3(Precio * (1 - CDbl(TextDirecto.Text) / 100))
                End If
            Else
                If CDbl(TextDirecto.Text) = 100 Then
                    Grid.Rows(I).Cells("PrecioBlanco").Value = Precio
                    Grid.Rows(I).Cells("PrecioNegro").Value = 0
                End If
                If CDbl(TextDirecto.Text) = 0 Then
                    Grid.Rows(I).Cells("PrecioBlanco").Value = 0
                    Grid.Rows(I).Cells("PrecioNegro").Value = Precio
                End If
                If CDbl(TextDirecto.Text) <> 100 And CDbl(TextDirecto.Text) <> 0 Then
                    Grid.Rows(I).Cells("PrecioBlanco").Value = Precio
                    '                    Grid.Rows(I).Cells("PrecioNegro").Value = Trunca3(CDbl(TextAutorizar.Text) * Precio / CDbl(TextDirecto.Text))
                    Grid.Rows(I).Cells("PrecioNegro").Value = Trunca3(CDbl(TextAutorizar.Text) * Precio / 100)
                End If
            End If
            '
            Grid.Rows(I).Cells("NetoBlanco").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioBlanco").Value)
            Grid.Rows(I).Cells("NetoNegro").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioNegro").Value)
            Grid.Rows(I).Cells("Neto").Value = Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("NetoNegro").Value
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("PrecioBlanco").Value, Grid.Rows(I).Cells("Iva").Value)
            TotalIva = TotalIva + Grid.Rows(I).Cells("MontoIva").Value
            Grid.Rows(I).Cells("TotalArticulo").Value = Trunca(Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("NetoNegro").Value + Grid.Rows(I).Cells("MontoIva").Value)
            TotalB = Trunca((TotalB + Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("MontoIva").Value))
            TotalN = Trunca(TotalN + Grid.Rows(I).Cells("NetoNegro").Value)
            Total = TotalB + TotalN
        Next

        TextSubTotal.Text = FormatNumber(Total, GDecimales)
        TextTotalNeto.Text = FormatNumber(TotalNeto, GDecimales)
        TextTotalIva.Text = FormatNumber(TotalIva, GDecimales)

        SenaTotal = 0
        Bultos = 0

        If CheckSenia.Checked And ComboTipoIva.SelectedValue <> Exterior Then
            For i As Integer = 0 To Grid.Rows.Count - 2
                Dim Sena As Double = CalculaSena(Grid.Rows(i).Cells("Articulo").Value, DateTime1.Value)
                If Sena <> 0 Then
                    SenaTotal = SenaTotal + CalculaNeto(Grid.Rows(i).Cells("Cantidad").Value, Sena)
                    Bultos = Bultos + Grid.Rows(i).Cells("Cantidad").Value
                End If
            Next
        End If
        LabelSena.Text = "Seña Por  " & Bultos & " Bultos. "
        TextSenia.Text = FormatNumber(SenaTotal, GDecimales)
        TextTotalFactura.Text = FormatNumber(Total + SenaTotal, GDecimales)

    End Sub
    Private Sub HacerAlta()

        Dim DtCabezaB As New DataTable
        Dim DtDetalleB As New DataTable
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtAsientoCabezaRemito As New DataTable
        '
        Dim DtCabezaN As New DataTable
        Dim DtDetalleN As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable
        '
        If CDbl(TextDirecto.Text) <> 0 Then
            DtCabezaB = DtCabeza.Clone
            DtDetalleB = DtDetalle.Copy
            ArmaArchivoParaAlta(DtCabezaB, DtDetalleB, "B")
        End If

        If CDbl(TextAutorizar.Text) <> 0 Then
            DtCabezaN = DtCabeza.Clone
            DtDetalleN = DtDetalle.Copy
            ArmaArchivoParaAlta(DtCabezaN, DtDetalleN, "N")
        End If

        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count <> 0 Then
            DtCabezaB.Rows(0).Item("Rel") = True
            DtCabezaN.Rows(0).Item("Rel") = True
        End If
        If DtCabezaB.Rows.Count <> 0 And DtCabezaN.Rows.Count = 0 Then
            DtCabezaB.Rows(0).Item("Rel") = False
        End If
        If DtCabezaB.Rows.Count = 0 And DtCabezaN.Rows.Count <> 0 Then
            DtCabezaN.Rows(0).Item("Rel") = False
        End If

        'Arma Asignacion Lotes.
        Dim DtLotesB As DataTable = DtAsignacionLotes.Clone
        Dim DtLotesN As DataTable = DtAsignacionLotes.Clone
        If ListaDeLotes.Count <> 0 Then    'Caso en que provenga de un Remito con Asignacion.
            If DtCabezaB.Rows.Count <> 0 Then AsignaLotesDeFacturasVenta(DtCabezaB, DtCabezaN, ListaDeLotes, DtLotesB, DtDetalleB, 0)
            If DtCabezaN.Rows.Count <> 0 Then AsignaLotesDeFacturasVenta(DtCabezaN, DtCabezaB, ListaDeLotes, DtLotesN, DtDetalleN, 0)
        End If

        'Arma Asientos.
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Me.Close() : Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If DtCabezaB.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtCabezaB, DtDetalleB, DtAsientoCabezaB, DtAsientoDetalleB) Then Exit Sub
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtCabezaN, DtDetalleN, DtAsientoCabezaN, DtAsientoDetalleN) Then Exit Sub
            End If
            If Remito = 0 Or Not AsignadoEnRemito Then
                DtAsientoCabezaCostoB = DtAsientoCabezaB.Clone
                DtAsientoDetalleCostoB = DtAsientoDetalleB.Clone
                DtAsientoCabezaCostoN = DtAsientoCabezaB.Clone
                DtAsientoDetalleCostoN = DtAsientoDetalleB.Clone
                If DtCabezaB.Rows.Count <> 0 Then
                    If Not ArmaArchivosAsientoCosto("A", DtDetalleB, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, CDbl(TextDirecto.Text)) Then Exit Sub
                End If
                If DtCabezaN.Rows.Count <> 0 Then
                    If Not ArmaArchivosAsientoCosto("A", DtDetalleN, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, CDbl(TextAutorizar.Text)) Then Exit Sub
                End If
            End If
            If Remito <> 0 And Not AsignadoEnRemito Then
                If Not HallaAsientosCabeza(6060, Remito, DtAsientoCabezaRemito, ConexionRemito) Then Me.Close() : Exit Sub
                If DtAsientoCabezaRemito.Rows.Count <> 0 Then DtAsientoCabezaRemito.Rows(0).Item("Estado") = 3
            End If
        End If

        'Actualiza Cabeza Remito.
        Dim DtLotesRemito As New DataTable
        If Remito <> 0 Then
            If DtAsignacionLotesRemito.Rows.Count <> 0 Then
                DtLotesRemito = DtAsignacionLotesRemito.Copy
                For Each Row As DataRow In DtLotesRemito.Rows
                    Row.Item("Facturado") = True
                Next
            End If
        End If

        'Graba Facturas.
        Dim NumeroFacturaN As Double = 0
        Dim NumeroAsientoB As Double
        Dim NumeroAsientoN As Double
        Dim NumeroAsientoCostoB As Double
        Dim NumeroAsientoCostoN As Double
        Dim Patron As String = GPuntoDeVenta & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim NumeroW As Double
        Dim InternoB As Double
        Dim InternoN As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                NumeroAsientoCostoB = NumeroAsientoB + 1
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                NumeroAsientoCostoN = NumeroAsientoN + 1
            End If
            'Halla ultima numeracion interna.
            If DtCabezaB.Rows.Count <> 0 Then
                InternoB = UltimoNumeroInternoFactura(TipoFactura, Conexion)
                If InternoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                InternoN = UltimoNumeroInternoFactura(TipoFactura, ConexionN)
                If InternoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Factura N.
            If DtCabezaN.Rows.Count <> 0 Then
                NumeroFacturaN = UltimaNumeracion(ConexionN)
                If NumeroFacturaN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If

            'Actualiza numeracion factura.
            If DtCabezaB.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaB.Rows(0).Item("Factura") = UltimoNumero
                DtCabezaB.Rows(0).Item("Interno") = InternoB
                DtCabezaB.Rows(0).Item("Relacionada") = 0
                'Detalle.
                For Each Row As DataRow In DtDetalleB.Rows
                    Row("Factura") = UltimoNumero
                Next
            End If
            If DtCabezaN.Rows.Count <> 0 Then
                'Cabeza.
                DtCabezaN.Rows(0).Item("Factura") = NumeroFacturaN
                DtCabezaN.Rows(0).Item("Interno") = InternoN
                If DtCabezaB.Rows.Count <> 0 Then
                    DtCabezaN.Rows(0).Item("Relacionada") = UltimoNumero
                Else : DtCabezaN.Rows(0).Item("Relacionada") = 0
                End If
                DtCabezaN.Rows(0).Item("Recibo") = 0
                'Detalle.
                For Each Row As DataRow In DtDetalleN.Rows
                    Row("Factura") = NumeroFacturaN
                Next
            End If

            'Asignacion lotes.
            If DtLotesB.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotesB.Rows
                    Row("Comprobante") = UltimoNumero
                Next
            End If
            If DtLotesN.Rows.Count <> 0 Then
                For Each Row As DataRow In DtLotesN.Rows
                    Row("Comprobante") = NumeroFacturaN
                Next
            End If

            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroFacturaN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                DtAsientoCabezaCostoB.Rows(0).Item("Asiento") = NumeroAsientoCostoB
                DtAsientoCabezaCostoB.Rows(0).Item("Documento") = UltimoNumero
                For Each Row As DataRow In DtAsientoDetalleCostoB.Rows
                    Row("Asiento") = NumeroAsientoCostoB
                Next
            End If
            If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                DtAsientoCabezaCostoN.Rows(0).Item("Asiento") = NumeroAsientoCostoN
                DtAsientoCabezaCostoN.Rows(0).Item("Documento") = NumeroFacturaN
                For Each Row As DataRow In DtAsientoDetalleCostoN.Rows
                    Row("Asiento") = NumeroAsientoCostoN
                Next
            End If

            'Actualiza Remito. 
            If Remito <> 0 Then
                If DtCabezaB.Rows.Count <> 0 Then
                    DtCabezaRemito.Rows(0).Item("Factura") = DtCabezaB.Rows(0).Item("Factura")
                Else
                    If AbiertoRemito Then
                        DtCabezaRemito.Rows(0).Item("Factura") = 1
                    Else
                        DtCabezaRemito.Rows(0).Item("Factura") = DtCabezaN.Rows(0).Item("Factura")
                    End If
                End If
            End If

            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

            NumeroW = AltaFactura(DtCabezaB, DtDetalleB, DtCabezaN, DtDetalleN, DtLotesRemito, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, DtAsientoCabezaRemito, DtLotesB, DtLotesN)

            Me.Cursor = System.Windows.Forms.Cursors.Default

            If NumeroW >= 0 Then Exit For
            If NumeroW = -10 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -3 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -10 Then
            MsgBox("Factura Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Remito = 0
            If DtCabezaB.Rows.Count <> 0 Then
                PFactura = DtCabezaB.Rows(0).Item("Factura")
                PAbierto = True
            Else : PFactura = DtCabezaN.Rows(0).Item("Factura")
                PAbierto = False
            End If
            PActualizacionOk = True
            If Not MuestraDatos() Then Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ArmaArchivoParaAlta(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal Tipo As String)

        Dim Row As DataRow
        Dim Total As Double
        Dim SeniaW As Double
        Dim BultosW As Double

        If Tipo = "B" Then
            Total = TotalB
        Else : Total = TotalN
        End If

        'Arma Senas.
        If CDbl(TextAutorizar.Text) <> 0 And Tipo = "N" Then
            SeniaW = SenaTotal
            BultosW = Bultos
        End If
        If CDbl(TextAutorizar.Text) = 0 And Tipo = "B" Then
            SeniaW = SenaTotal
            BultosW = Bultos
        End If

        Total = Total + SeniaW

        Dim RowsBusqueda() As DataRow

        'Prepara Detalle.
        For I As Integer = 0 To Grid.Rows.Count - 2
            RowsBusqueda = DtDetalle.Select("Indice = " & Grid.Rows(I).Cells("Indice").Value)
            If Tipo = "B" Then
                RowsBusqueda(0).Item("Iva") = Grid.Rows(I).Cells("Iva").Value
                RowsBusqueda(0).Item("Precio") = Grid.Rows(I).Cells("PrecioBlanco").Value
                RowsBusqueda(0).Item("TotalArticulo") = Trunca(Grid.Rows(I).Cells("NetoBlanco").Value + Grid.Rows(I).Cells("MontoIva").Value)
            Else : RowsBusqueda(0).Item("Iva") = 0
                RowsBusqueda(0).Item("Precio") = Grid.Rows(I).Cells("PrecioNegro").Value
                RowsBusqueda(0).Item("TotalArticulo") = Grid.Rows(I).Cells("NetoNegro").Value
            End If
            RowsBusqueda(0).Item("Devueltas") = 0
        Next

        'Prepara Cabeza.
        Row = DtCabeza.NewRow
        Row("Factura") = 0
        Row("Fecha") = DateTime1.Value
        Row("Cliente") = ComboCliente.SelectedValue
        Row("Sucursal") = ComboSucursal.SelectedValue
        Row("Remito") = Remito
        Row("Deposito") = ComboDeposito.SelectedValue
        Row("FormaPago") = 2
        Row("TipoIva") = ComboTipoIva.SelectedValue
        Row("Fecha") = DateTime1.Value
        If ListaDeLotes.Count = 0 Then
            Row("Estado") = 2
        Else : Row("Estado") = 1
        End If
        If EsServicios Or EsSecos Then Row("Estado") = 2
        Row("Recibo") = 0
        Row("Senia") = SeniaW
        Row("Bultos") = BultosW
        Row("Rel") = True
        Row("EsServicios") = EsServicios
        Row("EsSecos") = EsSecos
        Row("Relacionada") = 0
        Row("Final") = RadioFinal.Checked
        Row("PorUnidad") = RadioPorUnidad.Checked
        Row("Comentario") = TextComentario.Text.Trim
        Row("Importe") = Total
        Row("Saldo") = Total
        Row("ImporteDev") = 0
        Row("Vendedor") = ComboVendedor.SelectedValue
        Row("Moneda") = ComboMoneda.SelectedValue
        Row("Cambio") = CDbl(TextCambio.Text)
        If TextFechaAfip.Text = "" Then
            Row("FechaElectronica") = "01/01/1800"
        Else : Row("FechaElectronica") = TextFechaAfip.Text
        End If
        If TextFechaContable.Text = "" Then
            Row("FechaContable") = "01/01/1800"
        Else : Row("FechaContable") = TextFechaContable.Text
        End If
        Row("Impreso") = False
        If PEsElectronica Then
            Row("EsExterior") = True
        Else : Row("EsExterior") = False
        End If
        Row("IncoTerm") = ComboIncoTerm.SelectedValue
        Row("Destino") = TextDestino.Text.Trim
        DtCabeza.Rows.Add(Row)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtFacturaCabeza As DataTable, ByVal DtFacturaDetalle As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Asignado <> 0 Then
                Dim Tipo As Integer
                Dim Centro As Integer
                Dim Fila2 As New ItemLotesParaAsientos
                If Fila.Operacion = 1 Then
                    HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, Conexion, Tipo, Centro)
                Else : HallaCentroTipoOperacion(Fila.Lote, Fila.Secuencia, ConexionN, Tipo, Centro)
                End If
                If Centro <= 0 Then
                    MsgBox("Error en Tipo Operacion en Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000"))
                    Return False
                End If
                RowsBusqueda = DtFacturaDetalle.Select("Indice = " & Fila.Indice)
                Fila2.Centro = Centro
                Fila2.MontoNeto = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                If DtFacturaCabeza.Rows(0).Item("Moneda") <> 1 Then Fila2.MontoNeto = Trunca(Fila2.MontoNeto * DtFacturaCabeza.Rows(0).Item("Cambio"))
                If Tipo = 1 Then Fila2.Clave = 301 'consignacion
                If Tipo = 2 Then Fila2.Clave = 300 'reventa
                If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
                If Tipo = 4 Then Fila2.Clave = 302 'costeo
                ListaLotesParaAsiento.Add(Fila2)
            End If
        Next

        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtFacturaDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Iva") <> 0 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = HallaClaveIva(Row("Iva"))
                    If Item.Clave <= 0 Then
                        MsgBox("Error al leer Tabla de IVA. Operación se CANCELA.")
                        Return False
                    End If
                    Item.Importe = CalculaIva(Row("Cantidad"), Row("Precio"), Row("Iva"))
                    If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
                    Item.TipoIva = 6
                    ListaIVA.Add(Item)
                End If
            End If
        Next
        '
        Dim MontoNeto As Double
        For Each Row As DataRow In DtFacturaDetalle.Rows
            MontoNeto = MontoNeto + CalculaNeto(Row("Cantidad"), Row("Precio"))
        Next
        '
        If EsServicios Or EsSecos Then
            'Arma lista de Insumos, Utiliso listaRetenciones.
            For Each Row As DataRow In DtFacturaDetalle.Rows
                Item = New ItemListaConceptosAsientos
                Item.Clave = Row("Articulo")
                Item.Importe = CalculaNeto(Row("Cantidad"), Row("Precio"))
                If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
                ListaRetenciones.Add(Item)
            Next
        Else
            If ListaLotesParaAsiento.Count = 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = 202
                Item.Importe = MontoNeto
                If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
                ListaConceptos.Add(Item)
            End If
        End If
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 204
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Senia")
        If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
        If Item.Importe <> 0 Then ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = DtFacturaCabeza.Rows(0).Item("Importe")
        If ComboMoneda.SelectedValue <> 1 Then Item.Importe = Trunca(Item.Importe * CDbl(TextCambio.Text))
        ListaConceptos.Add(Item)

        Dim FechaW As Date
        If EsSecos Then
            FechaW = DtFacturaCabeza.Rows(0).Item("FechaContable")
        Else : FechaW = DtFacturaCabeza.Rows(0).Item("Fecha")
        End If

        If Funcion = "A" Then
            If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, FechaW) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaArchivosAsientoCosto(ByVal Funcion As String, ByVal DtFacturaDetalle As DataTable, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Porcentaje As Double) As Boolean

        If EsServicios Or EsSecos Then Return True

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim ImporteTotal As Double
        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtFacturaDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Dim Kilos As Double
                Dim Iva As Double
                HallaKilosIva(Row("Articulo"), Kilos, Iva)
                ImporteTotal = ImporteTotal + Trunca(0.2 * Row("Cantidad") * Kilos * Porcentaje / 100)
            End If
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = ImporteTotal
        ListaConceptos.Add(Item)
        '
        If Funcion = "A" Then
            If Not Asiento(TipoAsientoCosto, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value) Then Return False
        End If

        Return True

    End Function
    Private Function AltaFactura(ByVal DtCabezaB As DataTable, ByVal DtDetalleB As DataTable, ByVal DtCabezaN As DataTable, ByVal DtDetalleN As DataTable, ByVal DtLotesRemito As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtAsientoCabezaCostoB As DataTable, ByVal DtAsientoDetalleCostoB As DataTable, ByVal DtAsientoCabezaCostoN As DataTable, ByVal DtAsientoDetalleCostoN As DataTable, ByVal DtAsientoCabezaRemito As DataTable, ByVal DtLotesB As DataTable, ByVal DtLotesN As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaB.GetChanges) Then
                    If Not ReGrabaUltimaNumeracionFactura(DtCabezaB.Rows(0).Item("Factura"), TipoFactura) Then
                        If ComboTipoIva.SelectedValue <> Exterior Then Return -10
                    End If
                End If

                ' graba factura B.
                Resul = Grabafactura(DtCabezaB, DtDetalleB, Conexion)
                If Resul = -1 Then Return -10
                If Resul <= 0 Then Return Resul
                ' graba factura N.
                Resul = Grabafactura(DtCabezaN, DtDetalleN, ConexionN)
                If Resul <= 0 Then Return Resul

                ' graba Asiento B.
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento N.
                If DtAsientoCabezaN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo B.
                If DtAsientoCabezaCostoB.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo N.
                If DtAsientoCabezaCostoN.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaCostoN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                    Resul = GrabaTabla(DtAsientoDetalleCostoN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento costo Remito.
                If DtAsientoCabezaRemito.Rows.Count <> 0 Then
                    Resul = GrabaTabla(DtAsientoCabezaRemito.GetChanges, "AsientosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba asignacion B.
                If Not IsNothing(DtLotesB.GetChanges) Then
                    Resul = GrabaTabla(DtLotesB.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return 0
                End If
                ' graba asignacion N.
                If Not IsNothing(DtLotesN.GetChanges) Then
                    Resul = GrabaTabla(DtLotesN.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return 0
                End If

                ' Actualiza Remito.
                If Remito <> 0 Then
                    Resul = ActualizaRemito(DtCabezaRemito, DtLotesRemito)
                    If Resul <= 0 Then Return 0
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Sub HacerModificacion()

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        Dim DtCabezaW As DataTable = DtCabeza.Copy
        Dim DtCabezaRel As New DataTable

        If TextFechaAfip.Text <> "" Then
            If DtCabezaW.Rows(0).Item("FechaElectronica") <> CDate(TextFechaAfip.Text) Then DtCabezaW.Rows(0).Item("FechaElectronica") = CDate(TextFechaAfip.Text)
        End If

        If TextFechaContable.Text <> "" Then
            If DtCabezaW.Rows(0).Item("FechaContable") <> CDate(TextFechaContable.Text) Then DtCabezaW.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
        End If

        If Relacionada <> 0 Then
            If DtCabezaRel.Rows(0).Item("FechaElectronica") <> DtCabezaW.Rows(0).Item("FechaElectronica") Then DtCabezaRel.Rows(0).Item("FechaElectronica") = DtCabezaW.Rows(0).Item("FechaElectronica")
            If DtCabezaRel.Rows(0).Item("FechaContable") <> DtCabezaW.Rows(0).Item("FechaContable") Then DtCabezaRel.Rows(0).Item("FechaContable") = DtCabezaW.Rows(0).Item("FechaContable")
            If DtCabezaRel.Rows(0).Item("Vendedor") <> DtCabezaW.Rows(0).Item("Vendedor") Then DtCabezaRel.Rows(0).Item("Vendedor") = DtCabezaW.Rows(0).Item("Vendedor")
            If DtCabezaRel.Rows(0).Item("IncoTerm") <> DtCabezaW.Rows(0).Item("IncoTerm") Then DtCabezaRel.Rows(0).Item("IncoTerm") = DtCabezaW.Rows(0).Item("IncoTerm")

        End If

        If IsNothing(DtCabezaW.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabezaW.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaW.GetChanges, "FacturasCabeza", ConexionFactura)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                If Not IsNothing(DtCabezaRel.GetChanges) Then
                    resul = GrabaTabla(DtCabezaRel.GetChanges, "FacturasCabeza", ConexionRelacionada)
                    If Resul < 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                    If Resul = 0 Then
                        Me.Cursor = System.Windows.Forms.Cursors.Default
                        MsgBox("ERROR Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Exit Sub
                    End If
                End If

                Scope.Complete()
                PActualizacionOk = True
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End Using
        Catch ex As TransactionException
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Otro usuario Modifico datos o Error de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        Finally
        End Try

        If Not MuestraDatos() Then Me.Close()

    End Sub
    Private Function LeerAsientosParaModificacion(ByRef DtAsientoDetalle As DataTable, ByRef DtAsientoDetalleRel As DataTable, ByRef Asiento As Integer, ByRef AsientoRel As Integer) As Boolean

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 2 AND C.Documento = " & Relacionada & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Return False
            If DtAsientoDetalleRel.Rows.Count <> 0 Then AsientoRel = DtAsientoDetalleRel.Rows(0).Item("Asiento")
        End If

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 2 AND C.Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoDetalle) Then Return False
        If DtAsientoDetalle.Rows.Count <> 0 Then Asiento = DtAsientoDetalle.Rows(0).Item("Asiento")

        Return True

    End Function
    Private Function LeerAsientosParaModificacionCosto(ByRef DtAsientoDetalle As DataTable, ByRef DtAsientoDetalleRel As DataTable, ByRef Asiento As Integer, ByRef AsientoRel As Integer) As Boolean

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 6070 AND C.Documento = " & Relacionada & ";", ConexionRelacionada, DtAsientoDetalleRel) Then Return False
            If DtAsientoDetalleRel.Rows.Count <> 0 Then AsientoRel = DtAsientoDetalleRel.Rows(0).Item("Asiento")
        End If

        If Not Tablas.Read("SELECT D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.Estado = 1 AND C.TipoDocumento = 6070 AND C.Documento = " & DtCabeza.Rows(0).Item("Factura") & ";", ConexionFactura, DtAsientoDetalle) Then Return False
        If DtAsientoDetalle.Rows.Count <> 0 Then Asiento = DtAsientoDetalle.Rows(0).Item("Asiento")

        Return True

    End Function
    Private Sub ArmaAsignacionLotes(ByVal DtDetalleWB As DataTable, ByVal DtLotesWB As DataTable, ByVal ComprobanteWB As Double, ByVal DtDetalleWN As DataTable, ByVal DtLotesWN As DataTable, ByVal ComprobanteWN As Double)

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        If DtDetalleWB.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWB.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWB
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWB.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = Trunca(Row("ImporteSinIva") + CalculaIva(Fila.Asignado, RowsBusqueda(0).Item("Precio"), RowsBusqueda(0).Item("Iva")))
                    If ComboMoneda.SelectedValue <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * CDbl(TextCambio.Text))
                        Row("Importe") = Trunca(Row("Importe") * CDbl(TextCambio.Text))
                    End If
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWB.Rows.Add(Row)
                End If
            Next
        End If

        If DtDetalleWN.Rows.Count <> 0 Then
            For Each Fila As FilaAsignacion In ListaDeLotes
                If Fila.Asignado <> 0 Then
                    Row = DtLotesWN.NewRow()
                    Row("TipoComprobante") = 2
                    Row("Comprobante") = ComprobanteWN
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Deposito") = Fila.Deposito
                    Row("Operacion") = Fila.Operacion
                    Row("Cantidad") = Fila.Asignado
                    Row("Rel") = False
                    RowsBusqueda = DtDetalleWN.Select("Indice = " & Fila.Indice)
                    Row("ImporteSinIva") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    Row("Importe") = CalculaNeto(Fila.Asignado, RowsBusqueda(0).Item("Precio"))
                    If ComboMoneda.SelectedValue <> 1 Then
                        Row("ImporteSinIva") = Trunca(Row("ImporteSinIva") * CDbl(TextCambio.Text))
                        Row("Importe") = Trunca(Row("Importe") * CDbl(TextCambio.Text))
                    End If
                    Row("Facturado") = False
                    Row("Liquidado") = False
                    DtLotesWN.Rows.Add(Row)
                End If
            Next
        End If

        If DtLotesWB.Rows.Count <> 0 And DtLotesWN.Rows.Count <> 0 Then
            For Each Row In DtLotesWB.Rows
                Row("Rel") = True
            Next
            For Each Row In DtLotesWN.Rows
                Row("Rel") = True
            Next
        End If

    End Sub
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Private Function ActualizaRemito(ByVal DtRemitoW As DataTable, ByVal DtLotesW As DataTable) As Double

        Dim Resul As Double = 0

        If Not IsNothing(DtRemitoW.GetChanges) Then
            Resul = GrabaTabla(DtRemitoW.GetChanges, "RemitosCabeza", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If
        If Not IsNothing(DtLotesW.GetChanges) Then
            Resul = GrabaTabla(DtLotesW.GetChanges, "AsignacionLotes", ConexionRemito)
            If Resul <= 0 Then Return Resul
        End If

        Return 1000

    End Function
    Private Function Grabafactura(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal ConexionStr As String) As Double

        Dim resul As Double = 0

        If Not IsNothing(DtCabezaW.GetChanges) Then
            resul = GrabaTabla(DtCabezaW.GetChanges, "FacturasCabeza", ConexionStr)
            If resul <= 0 Then Return resul
        End If

        If Not IsNothing(DtDetalleW.GetChanges) Then
            resul = GrabaTabla(DtDetalleW.GetChanges, "FacturasDetalle", ConexionStr)
            If resul <= 0 Then Return resul
        End If

        Return 1000

    End Function
    Private Function CalculaSena(ByVal Articulo As Integer, ByVal Fecha As Date) As Double

        Dim Envase As Integer = HallaEnvase(Articulo)
        If Envase <= 0 Then
            MsgBox("ERROR al Leer Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return -1
        End If
        Dim Sena As Double
        If Not BuscaVigencia(10, Fecha, Sena, Envase) Then Return -1
        Return Sena

    End Function
    Private Function ArmaListaDePrecios(ByVal ArticuloW As Integer, ByVal FechaW As DateTime, ByVal IvaW As Double) As Double

        Dim Precio As Double = HalloPrecioDeLista(Lista, ArticuloW, FechaW)

        If Precio <> 0 Then
            Dim Fila As New ItemListaDePrecios
            Fila.Articulo = ArticuloW
            Fila.Precio = Precio
            ListaDePrecios.Add(Fila)
        End If

        Return Precio

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Dim Patron As String = TipoFactura & Format(GPuntoDeVenta, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFactura & Format(GPuntoDeVenta, "0000") & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function UltimoNumeroInternoFactura(ByVal TipoFacturaW As Integer, ByVal ConexionStr As String) As Double

        Dim Patron As String = TipoFacturaW & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Interno) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Interno AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(TipoFacturaW & Format(1, "000000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaRelacionada(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function HallaEstadoRecibo(ByVal Recibo As Double, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Estado FROM RecibosCabeza WHERE TipoNota = 60 AND Nota = " & Recibo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then Return CInt(Ultimo)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error de Base de Datos.")
            End
        End Try

    End Function
    Private Function UltimaFecha(ByVal ConexionStr) As Date

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Fecha) FROM FacturasCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return "1/1/1000"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return "2/1/1000"
        End Try

    End Function
    Private Function HallaIvaServicio(ByVal Servicio As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Iva FROM ArticulosServicios WHERE Clave = " & Servicio & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1
        Finally
        End Try

    End Function
    Private Function ReciboOk(ByRef DtRecibosCabeza As DataTable, ByVal Factura As Double, ByVal ConexionStr As String) As Integer

        Dim DtDetalle As New DataTable

        If Not Tablas.Read("SELECT Nota,Importe FROM RecibosDetalle WHERE TipoComprobante = 2 AND Comprobante = " & Factura & ";", ConexionStr, DtDetalle) Then
            DtDetalle.Dispose()
            MsgBox("Error Base de Datos. Operación se CANCELA.")
            Return -1
        End If
        If DtDetalle.Rows.Count <> 1 Then
            DtDetalle.Dispose()
            MsgBox("Factura Tiene Imputaciones en Cta.Cte. Operación se CANCELA.")
            Return -1
        End If
        If DtRecibosCabeza.Rows(0).Item("Importe") <> DtDetalle.Rows(0).Item("Importe") Then
            MsgBox("Recibo de Cobro Fue Modificado en Cta.Cte. Operación se CANCELA.")
            Return -1
        End If

        DtDetalle.Dispose()

        Return 0

    End Function
    Private Sub PideDatosEmisor()

        If PEsElectronica Then OpcionFacturas.PEsSoloExportacion = True
        OpcionFacturas.ShowDialog()
        PCliente = OpcionFacturas.PCliente
        Deposito = OpcionFacturas.PDeposito
        Remito = OpcionFacturas.PRemito
        AbiertoRemito = OpcionFacturas.PAbiertoRemito
        EsServicios = OpcionFacturas.PEsServicios
        EsSecos = OpcionFacturas.PEsSecos
        OpcionFacturas.Dispose()

        If PCliente = 0 Then Exit Sub

        TipoFactura = HallaTipoIvaCliente(PCliente)
        If TipoFactura = 3 Then TipoFactura = 2

        TextTipoFactura.Text = LetraTipoIva(TipoFactura)

        GPuntoDeVenta = HallaPuntoVentaSegunTipo(2, TipoFactura)
        If GPuntoDeVenta = 0 Then
            MsgBox("No tiene Definido Punto de Venta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            PCliente = 0
        End If

        LabelPuntodeVenta.Text = "Punto de Venta " & Format(GPuntoDeVenta, "0000")

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTime1.Value, UltimaFechaW) > 0 And PFactura = 0 Then
            MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
        If ComboCliente.SelectedValue = 0 Then
            MsgBox("Falta Cliente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboCliente.Focus()
            Return False
        End If
        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Deposito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If CtaCteCerrada And PFactura = 0 Then
            If MsgBox("Cuenta Cte. esta Cerrada. Desea Continuar Con la Factura?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Return False
            End If
        End If

        If TextCambio.Text = "" Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue <> 1 And CDbl(TextCambio.Text) = 0 Then
            MsgBox("Falta Informar Cambio.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If
        If ComboMoneda.SelectedValue = 1 And CDbl(TextCambio.Text) <> 1 Then
            MsgBox("Error Cambio de Moneda Local debe ser 1.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCambio.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = Exterior Then
            If TextFechaAfip.Text = "" Then
                MsgBox("Falta Informar Fecha AFIP.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
            If Not ConsisteFecha(TextFechaAfip.Text) Then
                MsgBox("Fecha AFIP Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
            If DiferenciaDias(TextFechaAfip.Text, Date.Now) < 0 Then
                MsgBox("Fecha AFIP Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaAfip.Focus()
                Return False
            End If
        End If

        If EsSecos Then
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
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Not (EsServicios Or EsSecos) Then
                If Not IsNumeric(Grid.Rows(i).Cells("KilosXUnidad").Value) Then
                    MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If Grid.Rows(i).Cells("KilosXUnidad").Value = 0 Then
                    MsgBox("Debe Informar Kilos por Unidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If

            If Not IsNumeric(Grid.Rows(i).Cells("Precio").Value) Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If

            If Grid.Rows(i).Cells("Precio").Value = 0 Then
                MsgBox("Debe Informar Precio en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Or Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If Grid.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        Dim Row As DataGridViewRow = Grid.CurrentRow

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If IsDBNull(Row.Cells("Precio").Value) Then Row.Cells("Precio").Value = 0
            Row.Cells("TotalArticulo").Value = Row.Cells("Precio").Value * Row.Cells("Cantidad").Value * (1 + Row.Cells("Iva").Value / 100)
            CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "KilosXUnidad" Then
            If IsDBNull(Row.Cells("KilosXUnidad").Value) Then Row.Cells("KilosXUnidad").Value = 0
            If PFactura = 0 Then CalculaSubTotal()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If IsDBNull(Row.Cells("Cantidad").Value) Then Row.Cells("Cantidad").Value = 0
            Row.Cells("TotalArticulo").Value = Row.Cells("Precio").Value * Row.Cells("Cantidad").Value * (1 + Row.Cells("Iva").Value / 100)
            CalculaSubTotal()
            '       If HallaStock(Grid.CurrentRow.Cells("Articulo").Value, ComboDeposito.SelectedValue) - Row.Cells("Cantidad").Value < 0 Then
            '    Grid.CurrentRow.Cells("LoteYSecuencia").Value = "Insuf."
            '   Else : Grid.CurrentRow.Cells("LoteYSecuencia").Value = "O.K."
            '   End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
            If IsDBNull(Row.Cells("TotalArticulo").Value) Then Row.Cells("TotalArticulo").Value = 0
            Dim Unitario As Double = (Row.Cells("TotalArticulo").Value * (1 - Row.Cells("Iva").Value / 100)) / Row.Cells("Cantidad").Value
            Row.Cells("Precio").Value = Unitario
            Row.Cells("TotalArticulo").Value = Unitario * Row.Cells("Cantidad").Value * (1 + Row.Cells("Iva").Value / 100)
            CalculaSubTotal()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalArticulo" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "TotalArticulo" Then
            Dim x As String = "1"
            EsNumerico(x, CType(sender, TextBox).Text.Trim, GDecimales + 1)
            If x = "" Then
                MsgBox("Importe erroneo o supera dicimales permitidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Precio" Then
            Dim x As String = "1"
            EsNumerico(x, CType(sender, TextBox).Text.Trim, 3 + 1)
            If x = "" Then
                MsgBox("Importe erroneo o supera dicimales permitidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "KilosXUnidad" Then
            Dim x As String = "1"
            EsNumerico(x, CType(sender, TextBox).Text.Trim, GDecimales + 1)
            If x = "" Then
                MsgBox("Importe erroneo o supera dicimales permitidos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                CType(sender, TextBox).Text = ""
                CType(sender, TextBox).Focus()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then Exit Sub
            If Not (EsServicios Or EsSecos) Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Indice").Value) And Not IsNothing(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    If BuscaIndice(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                        e.Value = "  Asignados  " : e.CellStyle.ForeColor = Color.Green
                    Else
                        If HallaStock(Grid.Rows(e.RowIndex).Cells("Articulo").Value, ComboDeposito.SelectedValue) - Grid.Rows(e.RowIndex).Cells("Cantidad").Value < 0 Then
                            e.Value = "Insuf."
                        Else : e.Value = "No Asignado"
                        End If
                        e.CellStyle.ForeColor = Color.Red
                    End If
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Iva" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Iva").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Precio").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "MontoIva" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("MontoIva").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Neto").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TotalArticulo").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" And ComboEstado.SelectedValue <> 2 Then
            MuestraLotesAsignados.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            MuestraLotesAsignados.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            MuestraLotesAsignados.PLista = ListaDeLotes
            MuestraLotesAsignados.ShowDialog()
            MuestraLotesAsignados.Dispose()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtDetalle_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        IndiceW = IndiceW + 1
        e.Row("Indice") = IndiceW
        e.Row("Factura") = UltimoNumero
        e.Row("Articulo") = 0
        e.Row("KilosXUnidad") = 0
        e.Row("Iva") = 0
        e.Row("Precio") = 0
        e.Row("Cantidad") = 0
        e.Row("TotalArticulo") = 0
        e.Row("Devueltas") = 0

    End Sub
    Private Sub Dtdetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If Not IsDBNull(e.Row("Articulo")) Then
                If e.Row("Articulo") <> e.ProposedValue Then
                    Dim Kilos As Double
                    Dim Iva As Double
                    Dim Precio As Double
                    If EsServicios Or EsSecos Then
                        Iva = HallaIvaServicio(e.ProposedValue)
                        If Iva < 0 Then
                            MsgBox("Error Base de Datos al leer Articulos Servicios.", MsgBoxStyle.Critical)
                            e.ProposedValue = e.Row("Articulo")
                            Grid.Refresh()
                            Exit Sub
                        End If
                    Else
                        HallaKilosIva(e.ProposedValue, Kilos, Iva)
                    End If
                    If ComboTipoIva.SelectedValue = Exterior Then Iva = 0
                    If Lista <> 0 And Not EsServicios And Remito = 0 Then
                        Precio = ArmaListaDePrecios(e.ProposedValue, DateTime1.Value, Iva)
                        If Precio = 0 Then
                            MsgBox("Articulo no tiene Precio en Lista de Precios.")
                            e.ProposedValue = e.Row("Articulo")
                            Grid.Refresh()
                            Exit Sub
                        End If
                    End If
                    e.Row("KilosXUnidad") = Kilos
                    e.Row("Iva") = Iva
                    Grid.Refresh()
                End If
            End If
        End If

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If Not IsDBNull(e.Row("Cantidad")) Then
                If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Dtdetalle_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If e.ProposedValue <> 0 Then
                If TieneCodigoCliente Then
                    Dim Codigo As Double = HallaCodigoCliente(PCliente, e.ProposedValue)
                    If Codigo <= 0 Then
                        MsgBox("Articulo No Tiene Codigo Cliente.", MsgBoxStyle.Information)
                        Dim Row As DataRowView = bs.Current
                        Row.Delete()
                    End If
                End If
                If DtDetalle.Rows.Count + 1 > GLineasFacturas Then
                    MsgBox("Supera Cantidad Articulos Permitidos.", MsgBoxStyle.Information)
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                End If
            End If
        End If

    End Sub
    Private Sub DtDetalle_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 
        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 And e.Row("Precio") = 0 Then e.Row.Delete()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRIDLEYENDA.           --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridLeyenda_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridLeyenda.CellEnter

        GridLeyenda.BeginEdit(True)

    End Sub

    
End Class