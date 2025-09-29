Imports System.Transactions
Public Class UnChequesPropios
    ' Las semanas comienzan el viernes terminan el jueves.
    Public DtGrid As New DataTable
    Private WithEvents bs As New BindingSource
    Dim DtCuentas As DataTable
    Dim DtTrabajo As DataTable
    Dim DtDestino As DataTable
    Public DtTotales As DataTable
    Public PBloqueaFunciones As Boolean
    '
    Dim SqlB As String
    Dim SqlN As String
    Dim SqlBInter As String
    Dim SqlNInter As String
    Dim FechaDesde As Date
    Dim FechaHasta As Date
    Dim PrimeraRowVista As Integer = 0
    Private Sub ConciliacionBancaria_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        If Not PermisoTotal Then
            CheckAbierto.Visible = False
            CheckCerrado.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Checked = False
        Else
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        TextCaja.Text = Format(999, "000")
        'anulado a pedido cliente para que vea todo los movimientos.
        '   TextCaja.Text = Format(GCaja, "000")
        '   If Not GCajaTotal Then TextCaja.Enabled = False

        TextAnio.Text = Format(Date.Now.Year, "0000")

        LlenaComboTablas(ComboBancos, 26)
        ComboBancos.SelectedValue = 0
        With ComboBancos
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        With ComboCuenta
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboMoneda, 27)
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = ComboMoneda.DataSource.select("Clave = 0")
        RowsBusqueda(0).Delete()
        Dim Row As DataRow = ComboMoneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        ComboMoneda.DataSource.Rows.Add(Row)
        ComboMoneda.SelectedValue = 1
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        ArmaDtCuentas()

        DtDestino = New DataTable
        Dim SqlB As String = "SELECT 1 Tipo,Clave,Nombre FROM Tablas Where  Tipo = 26" & _
                           " UNION ALL " & _
                           "SELECT 2 Tipo,Clave,Nombre FROM Proveedores" & _
                           " UNION ALL " & _
                           "SELECT 3 Tipo,Clave,Nombre FROM Clientes" & _
                           " UNION ALL " & _
                           "SELECT 4 Tipo,Legajo As Clave,Apellidos As Nombre FROM Empleados" & _
                           " UNION ALL " & _
                           "SELECT 5 Tipo,Clave,Nombre FROM OtrosProveedores" & _
                           " UNION ALL " & _
                           "SELECT 6 Tipo,Clave,Nombre FROM MaestroFondoFijo;"

        Dim SqlN As String = "SELECT 4 Tipo,Legajo As Clave,Apellidos As Nombre FROM Empleados;"

        If Not Tablas.Read(SqlB, Conexion, DtDestino) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtDestino) Then Me.Close() : Exit Sub
        End If

        ArmaListaResumen()

        DateTimeDesde.Value = FechaDesde
        DateTimeHasta.Value = FechaHasta
        Label8.Text = "Desde " & Format(FechaDesde, "dd/MM/yyyy") & "     Hasta " & Format(FechaHasta, "dd/MM/yyyy")

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaConciliacionBancaria_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If DtGrid.Rows.Count = 0 Then Exit Sub

        For Each Row As DataRow In DtGrid.Rows
            If Row("ClaveCheque") <> 0 Then
                If DiferenciaDias(Row("FechaDepositoAnt"), Row("FechaDeposito")) <> 0 Then
                    If MsgBox("Los cambios no fueron Actualizados. Quiere Aceptar Cambios antes de regresar al menu?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                        e.Cancel = True : Exit Sub
                    Else
                        Exit Sub
                    End If
                End If
                If DiferenciaDias(Row("VencimientoAnt"), Row("Vencimiento")) <> 0 Then
                    If MsgBox("Los cambios no fueron Actualizados. Quiere Aceptar Cambios antes de regresar al menu?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                        e.Cancel = True : Exit Sub
                    Else
                        Exit Sub
                    End If
                End If
            End If
        Next

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DiferenciaDias(DateTimeDesde.Value, FechaDesde) > 0 Then
            MsgBox("Fecha Desde Fuera del Entorno del Año.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Value = FechaDesde
            Exit Sub
        End If

        If DiferenciaDias(DateTimeHasta.Value, FechaHasta) < 0 Then
            DateTimeHasta.Value = FechaHasta
            MsgBox("Fecha Hasta Fuera del Entorno del Año.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count <> 0 Then PrimeraRowVista = Grid.CurrentRow.Index

        If TextCaja.Text = "" Then TextCaja.Text = Format(GCaja, "000")

        Grid.Focus()

        For Each Row As DataRow In DtTotales.Rows
            Row("Emitido") = 0
            Row("Depositado") = 0
            Row("Vencido") = 0
        Next

        LLenaGrid()

        If CheckAfectado.Checked Then
            Grid.Columns("Afectado").Visible = True
        Else : Grid.Columns("Afectado").Visible = False
        End If

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PrimeraRowVista > Grid.Rows.Count - 1 Then
            PrimeraRowVista = Grid.Rows.Count - 1
        End If
        Grid.CurrentCell = Grid.Rows(PrimeraRowVista).Cells("Semana")

    End Sub
    Private Sub ButtonActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActualizar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtB As New DataTable
        Dim DtN As New DataTable
        Dim ImporteChequeB As Double = 0
        Dim ImporteChequeN As Double = 0
        Dim ImporteDebitoB As Double = 0
        Dim ImporteDebitoN As Double = 0
        Dim RowsBusqueda() As DataRow

        GModificacionOk = False

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row As DataRow In DtGrid.Rows
            If Row("ClaveCheque") <> 0 Then                                                      'eCheq.
                If DiferenciaDias(Row("FechaDepositoAnt"), Row("FechaDeposito")) <> 0 Or DiferenciaDias(Row("VencimientoAnt"), Row("Vencimiento")) <> 0 Or Row("eCheq") <> Row("eCheqAnt") Then
                    If Row("Operacion") = 1 Then
                        If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", Conexion, DtB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    Else
                        If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionN, DtN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    End If
                End If
            End If
        Next

        If DtB.Rows.Count = 0 And DtN.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            MsgBox("No hay Cambios.")
            Exit Sub
        End If

        For Each Row As DataRow In DtB.Rows
            RowsBusqueda = DtGrid.Select("Operacion = 1 AND MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
            Row("FechaDeposito") = RowsBusqueda(0).Item("FechaDeposito")
            Row("Fecha") = RowsBusqueda(0).Item("Vencimiento")
            Row("eCheq") = RowsBusqueda(0).Item("eCheq")    'eCheq.
            If DiferenciaDias(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito")) <> 0 Then
                If Row("MedioPago") = 2 Then
                    ActualizaImporteAActualizar(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito"), RowsBusqueda(0).Item("Importe"), ImporteChequeB)
                End If
                If Row("MedioPago") = 14 Then
                    ActualizaImporteAActualizar(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito"), RowsBusqueda(0).Item("Importe"), ImporteDebitoB)
                End If
            End If
        Next

        For Each Row As DataRow In DtN.Rows
            RowsBusqueda = DtGrid.Select("Operacion = 2 AND MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque"))
            Row("FechaDeposito") = RowsBusqueda(0).Item("FechaDeposito")
            Row("Fecha") = RowsBusqueda(0).Item("Vencimiento")
            Row("eCheq") = RowsBusqueda(0).Item("eCheq")                         'eCheq.
            If DiferenciaDias(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito")) <> 0 Then
                If Row("Mediopago") = 2 Then
                    ActualizaImporteAActualizar(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito"), RowsBusqueda(0).Item("Importe"), ImporteChequeN)
                End If
                If Row("Mediopago") = 14 Then
                    ActualizaImporteAActualizar(RowsBusqueda(0).Item("FechaDepositoAnt"), RowsBusqueda(0).Item("FechaDeposito"), RowsBusqueda(0).Item("Importe"), ImporteDebitoN)
                End If
            End If
        Next

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabezaChequeB As New DataTable
        Dim DtAsientoDetalleChequeB As New DataTable
        Dim DtAsientoCabezaChequeN As New DataTable
        Dim DtAsientoDetalleChequeN As New DataTable
        Dim DtAsientoCabezaDebitoB As New DataTable
        Dim DtAsientoDetalleDebitoB As New DataTable
        Dim DtAsientoCabezaDebitoN As New DataTable
        Dim DtAsientoDetalleDebitoN As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaChequeB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleChequeB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If ImporteChequeB <> 0 Then
                If Not ArmaArchivosAsiento("A", 6010, DtAsientoCabezaChequeB, DtAsientoDetalleChequeB, ImporteChequeB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
            If ImporteChequeN <> 0 Then
                DtAsientoCabezaChequeN = DtAsientoCabezaChequeB.Clone
                DtAsientoDetalleChequeN = DtAsientoDetalleChequeB.Clone
                If Not ArmaArchivosAsiento("A", 6010, DtAsientoCabezaChequeN, DtAsientoDetalleChequeN, ImporteChequeN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
            If ImporteDebitoB <> 0 Then
                DtAsientoCabezaDebitoB = DtAsientoCabezaChequeB.Clone
                DtAsientoDetalleDebitoB = DtAsientoDetalleChequeB.Clone
                If Not ArmaArchivosAsiento("A", 6011, DtAsientoCabezaDebitoB, DtAsientoDetalleDebitoB, ImporteDebitoB) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
            If ImporteDebitoN <> 0 Then
                DtAsientoCabezaDebitoN = DtAsientoCabezaChequeB.Clone
                DtAsientoDetalleDebitoN = DtAsientoDetalleChequeB.Clone
                If Not ArmaArchivosAsiento("A", 6011, DtAsientoCabezaDebitoN, DtAsientoDetalleDebitoN, ImporteDebitoN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
        End If

        Dim NumeroAsientoB As Double = 0
        Dim NumeroAsientoN As Double = 0
        Dim Resul As Integer = 0

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaChequeB.Rows.Count <> 0 Or DtAsientoCabezaDebitoB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Halla Ultima numeracion Asiento N
            If DtAsientoCabezaChequeN.Rows.Count <> 0 Or DtAsientoCabezaDebitoN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            '
            'Actualiza Asientos.
            If DtAsientoCabezaChequeB.Rows.Count <> 0 Then
                DtAsientoCabezaChequeB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaChequeB.Rows(0).Item("Documento") = 0
                For Each Row As DataRow In DtAsientoDetalleChequeB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
                NumeroAsientoB = NumeroAsientoB + 1
            End If
            If DtAsientoCabezaChequeN.Rows.Count <> 0 Then
                DtAsientoCabezaChequeN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaChequeN.Rows(0).Item("Documento") = 0
                For Each Row As DataRow In DtAsientoDetalleChequeN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
                NumeroAsientoN = NumeroAsientoN + 1
            End If
            If DtAsientoCabezaDebitoB.Rows.Count <> 0 Then
                DtAsientoCabezaDebitoB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaDebitoB.Rows(0).Item("Documento") = 0
                For Each Row As DataRow In DtAsientoDetalleDebitoB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            If DtAsientoCabezaDebitoN.Rows.Count <> 0 Then
                DtAsientoCabezaDebitoN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaDebitoN.Rows(0).Item("Documento") = 0
                For Each Row As DataRow In DtAsientoDetalleDebitoN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            '
            Resul = ActualizaCheques(DtB, DtN, DtAsientoCabezaChequeB, DtAsientoDetalleChequeB, DtAsientoCabezaChequeN, DtAsientoDetalleChequeN, DtAsientoCabezaDebitoB, DtAsientoDetalleDebitoB, DtAsientoCabezaDebitoN, DtAsientoDetalleDebitoN)
            '
            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

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
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        ButtonAceptar_Click(Nothing, Nothing)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonCambiarAnio_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCambiarAnio.Click

        OpcionNumero.PAnioAnt = TextAnio.Text
        OpcionNumero.ShowDialog()
        If OpcionNumero.PRegresar Then OpcionNumero.Dispose() : Exit Sub
        TextAnio.Text = Format(OpcionNumero.PAnio, "0000")
        OpcionNumero.Dispose()

        ArmaListaResumen()

        DateTimeDesde.Value = FechaDesde
        DateTimeHasta.Value = FechaHasta
        Label8.Text = "Desde " & Format(FechaDesde, "dd/MM/yyyy") & "     Hasta " & Format(FechaHasta, "dd/MM/yyyy")

        DtGrid.Clear()

    End Sub
    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Grid.Columns("Sel").Visible = False
        Grid.Columns("Sel2").Visible = False
        Grid.Columns("Candado").Visible = False

        GridAExcel(Grid, GNombreEmpresa, "CHEQUES PROPIOS   AÑO      " & TextAnio.Text, "")

        Grid.Columns("Sel").Visible = True
        Grid.Columns("Sel2").Visible = True
        Grid.Columns("Candado").Visible = True

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ActualizaCheques(ByVal DtB As DataTable, ByVal DtN As DataTable, ByVal DtAsientoCabezaChequeB As DataTable, ByVal DtAsientoDetalleChequeB As DataTable, ByVal DtAsientoCabezaChequeN As DataTable, ByVal DtAsientoDetalleChequeN As DataTable, ByVal DtAsientoCabezaDebitoB As DataTable, ByVal DtAsientoDetalleDebitoB As DataTable, ByVal DtAsientoCabezaDebitoN As DataTable, ByVal DtAsientoDetalleDebitoN As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "Cheques", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtN.GetChanges) Then
                    Resul = GrabaTabla(DtN.GetChanges, "Cheques", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaChequeB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaChequeB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleChequeB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleChequeB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaChequeN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaChequeN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleChequeN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleChequeN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaDebitoB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaDebitoB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleDebitoB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleDebitoB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoCabezaDebitoN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaDebitoN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtAsientoDetalleDebitoN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleDebitoN.GetChanges, "AsientosDetalle", ConexionN)
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
    Private Sub ButtonVerResumen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerResumen.Click

        If CheckVencidos.Checked Then Exit Sub

        UnResumenChequesPropios.ShowDialog()
        UnResumenChequesPropios.Dispose()

    End Sub
    Private Sub ButtonResumenDiario_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonResumenDiario.Click

        If CheckVencidos.Checked Then Exit Sub

        UnResumenChequesDiario.ShowDialog()
        UnResumenChequesDiario.Dispose()

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
    Private Sub ComboBancos_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboBancos.Validating

        If IsNothing(ComboBancos.SelectedValue) Then ComboBancos.SelectedValue = 0
        If ComboBancos.SelectedValue = 0 Then
            ComboCuenta.Items.Clear()
            ComboCuenta.Text = "" : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        ComboCuenta.Items.Clear()

        RowsBusqueda = DtCuentas.Select("Banco = " & ComboBancos.SelectedValue)
        For Each Row As DataRow In RowsBusqueda
            ComboCuenta.Items.Add(Row("Numero"))
        Next
        ComboCuenta.Items.Add("")
        ComboCuenta.Text = ""


    End Sub
    Private Sub ComboCuenta_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCuenta.Validating

        If IsNothing(ComboCuenta.SelectedValue) Then ComboCuenta.SelectedValue = 0

    End Sub
    Private Sub ComboMoneda_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMoneda.Validating

        If IsNothing(ComboMoneda.SelectedValue) Then ComboMoneda.SelectedValue = 0

    End Sub
    Private Sub TextCaja_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCaja.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub LLenaGrid()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()
        CreaDtTrabajo()

        Dim CuentaW As Double
        If ComboCuenta.Text <> "" Then
            CuentaW = CDbl(ComboCuenta.Text)
        Else : CuentaW = 0
        End If

        Dim DtCheques As New DataTable
        Dim Blanco As Boolean
        Dim Negro As Boolean

        If CheckAbierto.Checked Then Blanco = True
        If CheckCerrado.Checked Then Negro = True

        If Not ArmaDtChequesPropios(DtCheques, CInt(TextCaja.Text), "01/01/1000", "01/01/1000", Blanco, Negro, 0, ComboBancos.SelectedValue, CuentaW, CheckBoxSoloSecos.Checked) Then Me.Close() : Exit Sub

        For Each Row As DataRow In DtCheques.Rows
            If ChequeOK(Row("FechaDeposito")) Then
                Dim Row1 As DataRow = DtTrabajo.NewRow
                Row1("ClaveCheque") = Row("ClaveCheque")
                Row1("MedioPago") = Row("MedioPago")
                Row1("Operacion") = Row("Operacion")
                Row1("NumeroFondoFijo") = Row("NumeroFondoFijo")
                Row1("Vencimiento") = Row("Fecha")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("FechaEmision") = Row("FechaDestino")
                Row1("Numero") = Row("Numero")
                Row1("Serie") = Row("Serie")
                Row1("FechaDeposito") = Row("FechaDeposito")
                Row1("Importe") = Row("Importe")
                Row1("Origen") = Row("Destino")
                Row1("Emisor") = Row("EmisorDestino")
                Row1("Afectado") = Row("Afectado")
                Row1("TipoDestino") = Row("TipoDestino")
                Row1("Caja") = Row("Caja")
                Row1("Estado") = Row("Estado")
                Row1("eCheq") = Row("eCheq")
                DtTrabajo.Rows.Add(Row1)
            End If
        Next

        DtCheques = New DataTable
        If Not ArmaDtDebitosAutomaticosDiferido(DtCheques, CInt(TextCaja.Text), "01/01/1000", "01/01/1000", Blanco, Negro, 0, ComboBancos.SelectedValue, CuentaW, CheckBoxSoloSecos.Checked) Then Me.Close() : Exit Sub

        For Each Row As DataRow In DtCheques.Rows
            If ChequeOK(Row("FechaDeposito")) Then
                Dim Row1 As DataRow = DtTrabajo.NewRow
                Row1("ClaveCheque") = Row("ClaveCheque")
                Row1("MedioPago") = Row("MedioPago")
                Row1("Operacion") = Row("Operacion")
                Row1("NumeroFondoFijo") = 0
                Row1("Vencimiento") = Row("Fecha")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("FechaEmision") = Row("FechaDestino")
                Row1("Numero") = Row("Numero")
                Row1("Serie") = Row("Serie")
                Row1("FechaDeposito") = Row("FechaDeposito")
                Row1("Importe") = Row("Importe")
                Row1("Origen") = Row("Destino")
                Row1("Emisor") = Row("EmisorDestino")
                Row1("Afectado") = Row("Afectado")
                Row1("TipoDestino") = Row("TipoDestino")
                Row1("Caja") = Row("Caja")
                Row1("Estado") = Row("Estado")
                Row1("eCheq") = False
                DtTrabajo.Rows.Add(Row1)
            End If
        Next

        DtCheques.Dispose()

        Dim SqlBInter As String = ""
        Dim SqlNInter As String = ""

        ArmaSqlInterBanking(ComboBancos.SelectedValue, CuentaW, CInt(TextCaja.Text), SqlBInter, SqlNInter, "01/01/1000", "01/01/1000", CheckBoxSoloSecos.Checked)

        Dim DtInter As New DataTable
        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlBInter, Conexion, DtInter) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlNInter, ConexionN, DtInter) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In DtInter.Rows
            If ChequeOK(Row("FechaComprobante")) Then
                Dim Row1 As DataRow = DtTrabajo.NewRow
                Row1("ClaveCheque") = 0
                Row1("MedioPago") = Row("MedioPago")
                Row1("Operacion") = Row("Operacion")
                Row1("NumeroFondoFijo") = Row("NumeroFondoFijo")
                Row1("Vencimiento") = Row("FechaComprobante")
                Row1("Banco") = Row("Banco")
                Row1("Cuenta") = Row("Cuenta")
                Row1("FechaEmision") = Row("Fecha")
                Row1("Numero") = Row("Comprobante")
                Row1("Serie") = ""
                Row1("FechaDeposito") = Row("FechaComprobante")
                Row1("Importe") = Row("Importe")
                '         If Row("Cambio") = 1 Then
                '     Row1("Importe") = Row("Importe")
                '        Else
                '         Row1("Importe") = Trunca(Row("Importe") * Row("Cambio"))
                '         End If
                Row1("Origen") = Row("origen")
                Row1("Emisor") = Row("Emisor")
                Row1("Afectado") = 0
                Row1("TipoDestino") = 0
                Row1("Caja") = Row("Caja")
                Row1("Estado") = 1
                Row1("eCheq") = False
                DtTrabajo.Rows.Add(Row1)
            End If
        Next

        DtInter.Dispose()

        Dim Semana As Integer
        Dim SemanaAnt As Integer = 0
        Dim Moneda As Integer

        Dim View As New DataView
        View = DtTrabajo.DefaultView
        View.Sort = "Vencimiento"

        ChequesPosteriores.Visible = False

        For Each Row As DataRowView In View
            If DiferenciaDias(Row("Vencimiento"), DateTimeHasta.Value) < 0 Then
                ChequesPosteriores.Visible = True
                Exit For
            End If
            Moneda = HallaMoneda(Row("Banco"), Row("Cuenta"))
            If DatosOk(Row, Moneda) Then
                Semana = HallaSemana(Row("Vencimiento"))
                If Semana = 0 Or DiferenciaDias(Row("Vencimiento"), DateTimeDesde.Value) > 0 Then
                    AcumulaImportes(Row)
                Else
                    If Semana <> SemanaAnt Then
                        LineaTotales(SemanaAnt)
                        SemanaAnt = Semana
                    End If
                    AcumulaImportes(Row)                      'Acumula Importe.
                    LineaDetalle(Semana, Row, Moneda)         'Imprime Detalle.
                End If
            End If
        Next
        LineaTotales(SemanaAnt)
        LineaTotalesAnio()

        Grid.DataSource = Nothing
        Grid.DataSource = bs
        bs.DataSource = DtGrid

        DtTrabajo.Dispose()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function DatosOk(ByVal Row As DataRowView, ByVal Moneda As Integer) As Boolean

        If Moneda <> ComboMoneda.SelectedValue Then Return False

        If CheckNoVencidos.Checked And (Row("Estado") = 3 Or Row("Estado") = 4 Or ChequeVencido(Row("Vencimiento"), Date.Now)) Then Return False

        If CheckVencidos.Checked And (Row("Estado") <> 1 Or Not ChequeVencido(Row("Vencimiento"), Date.Now) Or Row("FechaDeposito") <> "01/01/1800") Then
            Return False
        End If

        Return True

    End Function
    Private Sub LineaDetalle(ByVal Semana As Integer, ByVal Row As DataRowView, ByVal Moneda As Integer)

        Dim RowsBusqueda() As DataRow

        Dim Row1 As DataRow = DtGrid.NewRow
        Row1("MedioPago") = Row("Mediopago")
        Row1("Color") = 1
        Row1("ColorTexto") = 0
        Row1("Operacion") = Row("Operacion")
        Row1("Semana") = Semana
        Row1("Emisor") = Row("Emisor")
        Row1("Vencimiento") = Row("Vencimiento")
        Row1("VencimientoAnt") = Row("Vencimiento")
        Row1("Fecha") = Row("FechaEmision")
        Row1("Banco") = Row("Banco")
        Row1("Cuenta") = Row("Cuenta")
        Row1("Moneda") = Moneda
        Row1("Numero") = Row("Numero")
        Row1("FechaDeposito") = Row("FechaDeposito")
        If Row("FechaDeposito") <> "01/01/1800" And Row("ClaveCheque") <> 0 Then Row1("Color") = 7
        Row1("FechaDepositoAnt") = Row("FechaDeposito")
        Row1("ClaveCheque") = Row("ClaveCheque")
        Row1("Importe") = Row("Importe")
        Row1("Cartel") = ""
        Row1("Caja") = Row("Caja")
        Row1("eCheq") = Row("eCheq")        'eCheq.  
        Row1("eCheqAnt") = Row("eCheq")     'eCheq
        Row1("EstadoCh") = 0
        If Row("MedioPago") = 2 Then Row1("TipoDocumento") = "Cheque Propio"
        If Row("MedioPago") = 4 Then Row1("TipoDocumento") = "InterBank.Propio" : Row1("Color") = 3 : Row1("EstadoCh") = 4
        If Row("MedioPago") = 11 Then Row1("TipoDocumento") = "Transf.Propia" : Row1("Color") = 3 : Row1("EstadoCh") = 4
        If Row("MedioPago") = 14 Then Row1("TipoDocumento") = "Debito Auto.Dife." : Row1("Color") = 3
        If Row("MedioPago") = 9 Then Row1("TipoDocumento") = "Debito Automatico" : Row1("Color") = 3 : Row1("EstadoCh") = 4
        If ChequeVencido(Row1("Vencimiento"), Date.Now) And Row1("FechaDeposito") = "01/01/1800" And Row("Estado") = 1 Then
            Row1("Cartel") = "VENCIDO"
            Row1("Color") = 6
            Row1("EstadoCh") = 5
        End If
        If Row("Estado") = 3 Then
            Row1("Cartel") = "ANULADO"
            Row1("Color") = 4
            Row1("EstadoCh") = 3
        End If
        If Row("Estado") = 4 Then
            Row1("Cartel") = "RECHAZADO"
            Row1("Color") = 4
            Row1("EstadoCh") = 3
        End If
        If Row("TipoDestino") = 90 Then
            Row1("EstadoCh") = 4
        End If
        If Row("NumeroFondoFijo") <> 0 Then
            RowsBusqueda = DtDestino.Select("Tipo = 6 AND Clave = " & Row("Emisor"))
        Else
            RowsBusqueda = DtDestino.Select("Tipo = " & Row("Origen") & " AND Clave = " & Row("Emisor"))
        End If
        If RowsBusqueda.Length <> 0 Then
            Row1("Emisor") = RowsBusqueda(0).Item("Nombre")
        Else
            Row1("Emisor") = ""
        End If
        Row1("Afectado") = ""
        If Row("Afectado") <> 0 Then
            Row1("Afectado") = HallaAfectado(Row("Afectado"))
        End If

        DtGrid.Rows.Add(Row1)

    End Sub
    Private Sub LineaTotales(ByVal SemanaAnt As Integer)

        Dim RowsBusqueda(0) As DataRow

        RowsBusqueda = DtTotales.Select("Semana = " & SemanaAnt)

        Dim Row3 As DataRow = DtGrid.NewRow
        Row3("Color") = 100
        Row3("ColorTexto") = 0
        Row3("Operacion") = 3
        Row3("Semana") = SemanaAnt
        Row3("Emisor") = ""
        Row3("FechaDeposito") = "01/01/1800"
        Row3("Moneda") = ComboMoneda.SelectedValue
        Row3("Importe") = RowsBusqueda(0).Item("Emitido")
        Row3("EstadoCh") = 4
        Row3("ClaveCheque") = 0
        Row3("Banco") = 1
        DtGrid.Rows.Add(Row3)

    End Sub
    Private Sub LineaTotalesAnio()

        Dim Row3 As DataRow = DtGrid.NewRow
        Row3("Color") = 100
        Row3("ColorTexto") = 0
        Row3("Operacion") = 3
        Row3("Semana") = 0
        Row3("FechaDeposito") = "01/01/1800"
        Row3("Emisor") = ""
        Row3("Moneda") = ComboMoneda.SelectedValue
        Row3("Importe") = 0
        For Each Row As DataRow In DtTotales.Rows
            If Row("semana") <> 0 Then
                Row3("Importe") = Row3("Importe") + Row("Emitido")
            End If
        Next
        Row3("EstadoCh") = 4
        Row3("ClaveCheque") = 0
        Row3("Banco") = 3
        DtGrid.Rows.Add(Row3)

    End Sub
    Private Function GrabaFechaPago(ByVal ClaveCheque As Integer, ByVal Operacion As Integer, ByVal Fecha As Date, ByVal FechaAnt As Date) As Boolean

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Dim Sql As String = "UPDATE " & "Cheques" & _
                    " Set FechaDeposito = '" & Format(Fecha, "yyyyMMdd") & "' WHERE ClaveCheque = " & ClaveCheque & " AND CAST(FLOOR(CAST(FechaDeposito AS FLOAT)) AS DATETIME) = '" & Format(FechaAnt, "yyyyMMdd") & "';"
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                If Resul = 0 Then
                    Return False
                Else
                    Return True
                End If
            End Using
        Catch ex As Exception
            Return False
        Finally
        End Try

    End Function
    Private Function GrabaVencimiento(ByVal ClaveCheque As Integer, ByVal Operacion As Integer, ByVal Fecha As Date, ByVal FechaAnt As Date) As Boolean

        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Dim Sql As String = "UPDATE " & "Cheques" & _
                    " Set Fecha = '" & Format(Fecha, "yyyyMMdd") & "' WHERE ClaveCheque = " & ClaveCheque & " AND CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) = '" & Format(FechaAnt, "yyyyMMdd") & "';"
                Dim Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                Dim Resul As Integer = CInt(Cmd.ExecuteNonQuery())
                If Resul = 0 Then
                    Return False
                Else
                    Return True
                End If
            End Using
        Catch ex As Exception
            Return False
        Finally
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 ORDER BY Nombre;")

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "Emitido"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 2
        Row("Nombre") = "Depositado"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 3
        Row("Nombre") = "Emitido Periodo"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 4
        Row("Nombre") = "Depositado Periodo"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 5
        Row("Nombre") = "Emitido Anterior"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 6
        Row("Nombre") = "Depositado Anterior"
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 7
        Row("Nombre") = "Emitido Gral."
        Banco.DataSource.Rows.Add(Row)

        Row = Banco.DataSource.NewRow()
        Row("Clave") = 8
        Row("Nombre") = "Depositado Gral."
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

        Moneda.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 27 ORDER BY Nombre;")
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 1
        Row("Nombre") = "Pesos"
        Moneda.DataSource.Rows.Add(Row)
        Row = Moneda.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Moneda.DataSource.Rows.Add(Row)
        Moneda.DisplayMember = "Nombre"
        Moneda.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim Total As New DataColumn("Total")
        Total.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(Total)

        Dim Color As New DataColumn("Color")
        Color.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Color)

        Dim ColorTexto As New DataColumn("ColorTexto")
        ColorTexto.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ColorTexto)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Semana As New DataColumn("Semana")
        Semana.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Semana)

        Dim TipoDocumento As New DataColumn("TipoDocumento")
        TipoDocumento.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(TipoDocumento)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")           'eCheq.
        DtGrid.Columns.Add(eCheq)

        Dim eCheqAnt As New DataColumn("eCheqAnt")                        'eCheq.
        eCheqAnt.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheqAnt)

        Dim Vencimiento As New DataColumn("Vencimiento")
        Vencimiento.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Vencimiento)

        Dim VencimientoAnt As New DataColumn("VencimientoAnt")
        VencimientoAnt.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(VencimientoAnt)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Moneda As New DataColumn("Moneda")
        Moneda.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Moneda)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Numero)

        Dim FechaDeposito As New DataColumn("FechaDeposito")
        FechaDeposito.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaDeposito)

        Dim FechaDepositoAnt As New DataColumn("FechaDepositoAnt")
        FechaDepositoAnt.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaDepositoAnt)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim NoDepositado As New DataColumn("NoDepositado")
        NoDepositado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(NoDepositado)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim Afectado As New DataColumn("Afectado")
        Afectado.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Afectado)

        Dim EstadoCh As New DataColumn("EstadoCh")
        EstadoCh.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(EstadoCh)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim Cartel As New DataColumn("Cartel")
        Cartel.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Cartel)

    End Sub
    Private Sub CreaDtTotales()

        DtTotales = New DataTable

        Dim Semana As New DataColumn("Semana")
        Semana.DataType = System.Type.GetType("System.Int32")
        DtTotales.Columns.Add(Semana)

        Dim Emitido As New DataColumn("Emitido")
        Emitido.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Emitido)

        Dim Depositado As New DataColumn("Depositado")
        Depositado.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Depositado)

        Dim Vencido As New DataColumn("Vencido")
        Vencido.DataType = System.Type.GetType("System.Decimal")
        DtTotales.Columns.Add(Vencido)

        Dim Desde As New DataColumn("Desde")
        Desde.DataType = System.Type.GetType("System.DateTime")
        DtTotales.Columns.Add(Desde)

        Dim Hasta As New DataColumn("Hasta")
        Hasta.DataType = System.Type.GetType("System.DateTime")
        DtTotales.Columns.Add(Hasta)

    End Sub
    Private Sub CreaDtTrabajo()

        DtTrabajo = New DataTable

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(ClaveCheque)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(MedioPago)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Operacion)

        Dim NumeroFondoFijo As New DataColumn("NumeroFondoFijo")
        NumeroFondoFijo.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(NumeroFondoFijo)

        Dim Vencimiento As New DataColumn("Vencimiento")
        Vencimiento.DataType = System.Type.GetType("System.DateTime")
        DtTrabajo.Columns.Add(Vencimiento)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtTrabajo.Columns.Add(Cuenta)

        Dim FechaEmision As New DataColumn("FechaEmision")
        FechaEmision.DataType = System.Type.GetType("System.DateTime")
        DtTrabajo.Columns.Add(FechaEmision)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Double")
        DtTrabajo.Columns.Add(Numero)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtTrabajo.Columns.Add(Serie)

        Dim FechaDeposito As New DataColumn("FechaDeposito")
        FechaDeposito.DataType = System.Type.GetType("System.DateTime")
        DtTrabajo.Columns.Add(FechaDeposito)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtTrabajo.Columns.Add(Importe)

        Dim Origen As New DataColumn("Origen")
        Origen.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Origen)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Emisor)

        Dim Afectado As New DataColumn("Afectado")
        Afectado.DataType = System.Type.GetType("System.Double")
        DtTrabajo.Columns.Add(Afectado)

        Dim TipoDestino As New DataColumn("TipoDestino")
        TipoDestino.DataType = System.Type.GetType("System.Double")
        DtTrabajo.Columns.Add(TipoDestino)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Estado)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtTrabajo.Columns.Add(Caja)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtTrabajo.Columns.Add(eCheq)

    End Sub
    Private Sub ArmaDtCuentas()

        DtCuentas = New DataTable

        If Not Tablas.Read("SELECT Banco,Numero,Moneda FROM CuentasBancarias;", Conexion, DtCuentas) Then End

    End Sub
    Private Function HallaSemana(ByVal Fecha As Date) As Integer

        For Each Row As DataRow In DtTotales.Rows
            If DiferenciaDias(Row("Desde"), Fecha) >= 0 And DiferenciaDias(Row("Hasta"), Fecha) <= 0 Then
                Return Row("Semana")
            End If
        Next

    End Function
    Private Sub ArmaListaResumen()

        Dim Anio As Integer = CInt(TextAnio.Text)
        Dim FechaDesdeW As Date
        Dim FechaHastaW As Date
        Dim NumeroDia As Integer
        Dim I As Integer

        Dim UltimaSemanaAnt = HallaSemanasDelAnio(Anio - 1)

        For I = 1 To 7
            NumeroDia = DatePart(DateInterval.Weekday, CDate(I & "/01/" & Format(Anio, "0000")))
            If NumeroDia = 6 Then Exit For
        Next
        'el dia I es viernes.
        Dim Viernes As Date = CDate(I & "/01/" & Format(Anio, "0000"))

        FechaHastaW = DateAdd(DateInterval.Day, -1, Viernes)
        FechaDesdeW = DateAdd(DateInterval.Day, -6, FechaHastaW)

        Dim Semana As Integer = 0

        CreaDtTotales()

        Dim FechaDesdeAux As Date = DateAdd(DateInterval.Day, -7, FechaDesdeW)
        Dim FechaHastaAux As Date = DateAdd(DateInterval.Day, -7, FechaHastaW)

        Dim Row As DataRow = DtTotales.NewRow
        Row("Semana") = 0
        Row("Emitido") = 0
        Row("Depositado") = 0
        Row("Vencido") = 0
        Row("Desde") = "01/01/1800"
        Row("Hasta") = DateAdd(DateInterval.Day, -1, FechaDesdeW)
        DtTotales.Rows.Add(Row)
        Do
            Row = DtTotales.NewRow
            FechaDesdeAux = DateAdd(DateInterval.Day, 7, FechaDesdeAux)
            FechaHastaAux = DateAdd(DateInterval.Day, 7, FechaHastaAux)
            If FechaHastaAux = FechaHastaW Then
                Row("Semana") = UltimaSemanaAnt
            Else
                Semana = Semana + 1
                Row("Semana") = Semana
            End If
            Row("Emitido") = 0
            Row("Depositado") = 0
            Row("Vencido") = 0
            Row("Desde") = FechaDesdeAux
            Row("Hasta") = FechaHastaAux
            DtTotales.Rows.Add(Row)
            If FechaHastaAux.Year = Anio + 1 Then Exit Do
        Loop

        FechaDesde = DtTotales.Rows(1).Item("Desde")
        FechaHasta = DtTotales.Rows(DtTotales.Rows.Count - 1).Item("Hasta")

    End Sub
    Private Function HallaSemanasDelAnio(ByVal Anio As Integer) As Integer

        Dim FechaDesdeW As Date
        Dim FechaHastaW As Date
        Dim NumeroDia As Integer
        Dim I As Integer

        For I = 1 To 7
            NumeroDia = DatePart(DateInterval.Weekday, CDate(I & "/01/" & Format(Anio, "0000")))
            If NumeroDia = 6 Then Exit For
        Next
        'el dia I es viernes.
        Dim Viernes As Date = CDate(I & "/01/" & Format(Anio, "0000"))

        FechaHastaW = DateAdd(DateInterval.Day, 6, Viernes)
        FechaDesdeW = Viernes

        Dim Semana As Integer = 0

        Dim FechaDesdeAux As Date = DateAdd(DateInterval.Day, -7, FechaDesdeW)
        Dim FechaHastaAux As Date = DateAdd(DateInterval.Day, -7, FechaHastaW)

        Do
            FechaDesdeAux = DateAdd(DateInterval.Day, 7, FechaDesdeAux)
            FechaHastaAux = DateAdd(DateInterval.Day, 7, FechaHastaAux)
            Semana = Semana + 1
            If FechaHastaAux.Year = Anio + 1 Then Exit Do
        Loop

        Return Semana

    End Function
    Private Function HallaMoneda(ByVal Banco As Integer, ByVal Cuenta As Double) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtCuentas.Select("Banco = " & Banco & " AND Numero = " & Cuenta)
        If RowsBusqueda.Length <> 0 Then
            Return RowsBusqueda(0).Item("Moneda")
        Else : Return 0
        End If

    End Function
    Private Sub AcumulaImportes(ByVal Row As DataRowView)

        If Row("Estado") <> 1 Then Exit Sub
        If ChequeVencido(Row("Vencimiento"), Date.Now) And Row("FechaDeposito") <> "01/01/1800" Then Exit Sub

        For Each Row1 As DataRow In DtTotales.Rows
            If DiferenciaDias(Row1("Desde"), Row("Vencimiento")) >= 0 And DiferenciaDias(Row1("Hasta"), Row("Vencimiento")) <= 0 Then
                Row1("Emitido") = Row1("Emitido") + Row("Importe")
                If ChequeVencido(Row("Vencimiento"), Date.Now) Then
                    If Row("FechaDeposito") = "01/01/1800" Then
                        Row1("Vencido") = Row1("Vencido") + Row("Importe")
                    End If
                Else
                    If Row("FechaDeposito") <> "01/01/1800" Then
                        Row1("Depositado") = Row1("Depositado") + Row("Importe")
                    End If
                End If
                Exit Sub
            End If
        Next

    End Sub
    Private Sub ActualizaImporteAActualizar(ByVal FechaAnt As Date, ByVal Fecha As Date, ByVal ImporteCheque As Double, ByRef Importe As Double)

        If DiferenciaDias(FechaAnt, "01/01/1800") <> 0 And DiferenciaDias(Fecha, "01/01/1800") <> 0 Then
            Exit Sub
        End If
        If DiferenciaDias(FechaAnt, "01/01/1800") = 0 And DiferenciaDias(Fecha, "01/01/1800") <> 0 Then
            Importe = Importe + ImporteCheque
        End If
        If DiferenciaDias(FechaAnt, "01/01/1800") <> 0 And DiferenciaDias(Fecha, "01/01/1800") = 0 Then
            Importe = Importe - ImporteCheque
        End If

    End Sub
    Private Function ChequeOK(ByVal FechaEntrega As Date) As Boolean

        If Not CheckDepositado.Checked And Not CheckPendientes.Checked Then Return True
        If CheckDepositado.Checked And CheckPendientes.Checked Then Return True

        If CheckDepositado.Checked And FechaEntrega <> "01/01/1800" Then Return True

        If CheckPendientes.Checked And FechaEntrega = "01/01/1800" Then Return True

        Return False

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal TipoAsiento As Integer, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Importe As Double) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = Importe
        ListaConceptos.Add(Item)
        '
        If Not Asiento(TipoAsiento, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, Date.Now, 0) Then Return False

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Semana" Then
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.LightCyan
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 3 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Yellow
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 4 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.Red
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 6 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.NavajoWhite
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 7 Then Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Drawing.Color.LightGray
            If Grid.Rows(e.RowIndex).Cells("Color").Value = 5 Then Grid.Rows(e.RowIndex).Height = 2
            If Grid.Rows(e.RowIndex).Cells("ColorTexto").Value = 1 Then Grid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Drawing.Color.Red
            If Grid.Rows(e.RowIndex).Cells("Banco").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Importe").Style.ForeColor = Drawing.Color.Green
            If Grid.Rows(e.RowIndex).Cells("Banco").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Importe").Style.ForeColor = Drawing.Color.Green
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            Grid.Rows(e.RowIndex).Cells("Sel2").Value = False
            e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "FechaDeposito" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "01/01/1800" Then
                    e.Value = ""
                    Grid.Rows(e.RowIndex).Cells("Sel").Value = False
                Else : e.Value = Format(e.Value, "dd/MM/yyyy")
                    Grid.Rows(e.RowIndex).Cells("Sel").Value = True
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

    End Sub
    Private Sub Grid_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellContentClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.Rows(e.RowIndex).Cells("Vencimiento").Value) Then
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Sel" Then
            If Grid.Rows(e.RowIndex).Cells("EstadoCh").Value <> 0 Then
                Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
                Exit Sub
            End If

            Dim Check As New DataGridViewCheckBoxCell
            Check = Grid.Rows(e.RowIndex).Cells("Sel")
            Check.Value = Not Check.Value

            If Not Check.Value Then
                Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = "01/01/1800"
            Else
                Calendario.PFecha = Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value
                Calendario.ShowDialog()
                Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = Format(Calendario.PFecha, "dd/MM/yyyy")
                Calendario.Dispose()
                If Format(Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value, "dd/MM/yyyy") = "01/01/1800" Then Exit Sub
                If DiferenciaDias(Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value, Grid.Rows(e.RowIndex).Cells("Fecha").Value) > 0 Then
                    MsgBox("Fecha Deposito menor que Fecha Emision.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = "01/01/1800"
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
                    Exit Sub
                End If
                If DiferenciaDias(Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value, Grid.Rows(e.RowIndex).Cells("Vencimiento").Value) > 0 Then
                    MsgBox("Fecha Deposito menor al Vencimiento.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = "01/01/1800"
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
                    Exit Sub
                End If
                If ChequeVencido(Grid.Rows(e.RowIndex).Cells("Vencimiento").Value, Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value) Then
                    MsgBox("Fecha Deposito Mayor al Plazo de Cobro(30 Días).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = "01/01/1800"
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
                    Exit Sub
                End If
                If DiferenciaDias(Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value, Date.Now) < 0 Then
                    MsgBox("Fecha Deposito Mayor a Fecha Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value = "01/01/1800"
                    Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
                    Exit Sub
                End If
                Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("FechaDeposito")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Sel2" Then
            If Format(Grid.Rows(e.RowIndex).Cells("FechaDeposito").Value, "dd/MM/yyyy") <> "01/01/1800" Then
                MsgBox("Cheque Fue Depositado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Vencimiento")
                Exit Sub
            End If
            Calendario.PFecha = Grid.Rows(e.RowIndex).Cells("Vencimiento").Value
            Calendario.ShowDialog()
            If ChequeVencido(Calendario.PFecha, Date.Now) Then
                MsgBox("Nueva Fecha Produce Vencimiento del Cheuqe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Else
                Grid.Rows(e.RowIndex).Cells("Vencimiento").Value = Format(Calendario.PFecha, "dd/MM/yyyy")
            End If
            Calendario.Dispose()
            Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Vencimiento")
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError

        Exit Sub

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Sel")) Then
        End If

    End Sub
  
 
    
End Class