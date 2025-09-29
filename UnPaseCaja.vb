Imports System.Transactions
Public Class UnPaseCaja
    'Tipo de Documento 80.
    Public PPase As Decimal
    Public PCajaOrigen As Integer
    Public PCajaDestino As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    Public PDtGrid As DataTable
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtFormasPago As DataTable
    Dim DtGrid As DataTable
    Dim DtPaseCabeza As DataTable
    Dim DtPaseDetalle As DataTable
    Dim DtSaldos As DataTable
    Dim DtRecuperoSenia As DataTable
    Dim DtCheques As DataTable
    '
    Dim EsPaseTerceros As Boolean
    Dim cb As ComboBox
    Dim ConexionPase As String
    Private Sub UnPaseCaja_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("LupaCuenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        ArmaMedioPagoPase(DtFormasPago, True)

        LlenaCombosGrid()

        If PermisoTotal Then
            PictureCandado.Visible = True
        Else : PictureCandado.Visible = False
        End If

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
            ConexionPase = Conexion
        Else
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            ConexionPase = ConexionN
        End If

        ComboCajaOrigen.DataSource = ArmaDtCajas()
        ComboCajaOrigen.DisplayMember = "Nombre"
        ComboCajaOrigen.ValueMember = "Clave"

        ComboCajaDestino.DataSource = ArmaDtCajas()
        ComboCajaDestino.DisplayMember = "Nombre"
        ComboCajaDestino.ValueMember = "Clave"

        Grid.Columns("Detalle").Visible = False
        Grid.Columns("Neto").Visible = False
        Grid.Columns("Alicuota").Visible = False
        Grid.Columns("Iva").Visible = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)
        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanged)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsPaseTerceros Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Grid.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtPaseCabezaAux As DataTable = DtPaseCabeza.Copy
        Dim DtPaseDetalleAux As DataTable = DtPaseDetalle.Copy
        Dim DtChequesAux As DataTable = DtCheques.Copy
        Dim DtRecuperoSeniaAux As DataTable = DtRecuperoSenia.Copy

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ActualizaArchivos(DtPaseDetalleAux)

        If IsNothing(DtPaseCabezaAux.GetChanges) And IsNothing(DtPaseDetalleAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PPase = 0 Then
            If HacerAlta(DtPaseCabezaAux, DtPaseDetalleAux, DtChequesAux, DtRecuperoSeniaAux) Then
                ArmaArchivos()
            End If
        Else
            Dim Resul As Double = ActualizaPase("M", DtPaseCabezaAux, DtPaseDetalleAux, DtChequesAux, DtRecuperoSeniaAux)
            If Resul < 0 Then
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If Resul = 0 Then
                MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            End If
            If Resul > 0 Then
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PPase = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsPaseTerceros Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtPaseCabeza.Rows(0).Item("Aceptado") Then
            MsgBox("Pase ya fue Aceptado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Pase se Borrara del Sistema. ¿Desea Borrarla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim DtPaseCabezaAux As DataTable = DtPaseCabeza.Copy
        Dim DtPaseDetalleAux As DataTable = DtPaseDetalle.Copy
        Dim DtChequesAux As DataTable = DtCheques.Copy
        Dim DtRecuperoSeniaAux As DataTable = DtRecuperoSenia.Copy

        DtPaseCabezaAux.Rows(0).Delete()
        For Each Row As DataRow In DtPaseDetalleAux.Rows
            Row.Delete()
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Double = ActualizaPase("B", DtPaseCabezaAux, DtPaseDetalleAux, DtChequesAux, DtRecuperoSeniaAux)
        If Resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Pase Borrado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAceptaPase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptaPase.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PPase = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtPaseCabeza.Rows(0).Item("Aceptado") Then
            MsgBox("Pase ya fue Aceptado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtPaseCabezaAux As DataTable = DtPaseCabeza.Copy
        Dim DtPaseDetalleAux As DataTable = DtPaseDetalle.Copy
        Dim DtChequesAux As DataTable = DtCheques.Copy
        Dim DtRecuperoSeniaAux As DataTable = DtRecuperoSenia.Copy

        DtPaseCabezaAux.Rows(0).Item("Aceptado") = True

        For Each Row As DataRow In DtChequesAux.Rows
            Row("Caja") = GCaja
        Next
        For Each Row As DataRow In DtRecuperoSeniaAux.Rows
            Row("PaseCaja") = GCaja
        Next

        Dim Resul As Double = ActualizaPase("M", DtPaseCabezaAux, DtPaseDetalleAux, DtChequesAux, DtRecuperoSeniaAux)
        If Resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        DtPaseCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM PaseCajaCabeza WHERE Pase = " & PPase & ";", ConexionPase, DtPaseCabeza) Then Return False
        If PPase <> 0 And DtPaseCabeza.Rows.Count = 0 Then
            MsgBox("Pase de Caja No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PPase = 0 Then
            Dim Row As DataRow = DtPaseCabeza.NewRow
            Row("Pase") = 0
            Row("CajaOrigen") = PCajaOrigen
            Row("CajaDestino") = PCajaDestino
            Row("Fecha") = DateTime1.Value
            Row("Aceptado") = False
            Row("FechaAceptado") = "1/1/1800"
            DtPaseCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtPaseDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM PaseCajaDetalle WHERE Pase = " & PPase & ";", ConexionPase, DtPaseDetalle) Then Return False

        DtCheques = New DataTable
        For Each Row As DataRow In DtPaseDetalle.Rows
            If Row("ClaveCheque") <> 0 Then
                If Not Tablas.Read("SELECT * FROM Cheques WHERE MedioPago = " & Row("MedioPago") & " AND ClaveCheque = " & Row("ClaveCheque") & ";", ConexionPase, DtCheques) Then Return False
            End If
        Next
        DtRecuperoSenia = New DataTable
        For Each Row As DataRow In DtPaseDetalle.Rows
            If Row("Recibo") <> 0 Then
                If Not Tablas.Read("SELECT * FROM RecuperoSenia WHERE Nota = " & Row("Recibo") & ";", ConexionPase, DtRecuperoSenia) Then Return False
            End If
        Next

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtPaseDetalle.Rows
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("Item") = Row("Item")
            Row1("MedioPago") = Row("MedioPago")
            Row1("Importe") = Row("Importe")
            Row1("Cambio") = 0
            Row1("Bultos") = 0
            Row1("Detalle") = 0
            Row1("Alicuota") = 0
            Row1("Neto") = 0
            Row1("Comprobante") = 0
            Row1("FechaComprobante") = "1/1/1800"
            Row1("ClaveCheque") = Row("ClaveCheque")
            If Row("MedioPago") = 2 Then
                Row1("ClaveChequeVisual") = 0
            Else : Row1("ClaveChequeVisual") = Row("ClaveCheque")
            End If
            If Row("ClaveCheque") <> 0 Then
                RowsBusqueda = DtCheques.Select("ClaveCheque = " & Row("ClaveCheque"))
                If RowsBusqueda.Length <> 0 Then
                    Row1("Banco") = RowsBusqueda(0).Item("Banco")
                    Row1("Cuenta") = RowsBusqueda(0).Item("Cuenta")
                    Row1("Serie") = RowsBusqueda(0).Item("Serie")
                    Row1("Numero") = RowsBusqueda(0).Item("Numero")
                    Row1("EmisorCheque") = RowsBusqueda(0).Item("EmisorCheque")
                    Row1("Fecha") = RowsBusqueda(0).Item("Fecha")
                Else
                    Row1("Banco") = 0
                    Row1("Cuenta") = 0
                    Row1("Serie") = ""
                    Row1("Numero") = 0
                    Row1("EmisorCheque") = "Comp.No Existe."
                    Row1("Fecha") = "1/1/1800"
                End If
            Else
                Row1("Banco") = 0
                Row1("Cuenta") = 0
                Row1("Serie") = ""
                Row1("Numero") = 0
                Row1("EmisorCheque") = ""
                Row1("Fecha") = "1/1/1800"
            End If
            Row1("Recibo") = Row("Recibo")
            Row1("ClaveInterna") = Row("Recibo")
            If Row("Recibo") <> 0 Then
                RowsBusqueda = DtRecuperoSenia.Select("Nota = " & Row("Recibo"))
                Row1("Comprobante") = RowsBusqueda(0).Item("Vale")
            End If
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid
        Grid.EndEdit()

        'Precenta las lineas del grid.
        PresentaLineasGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtPaseCabeza.Rows(0).Item("CajaOrigen") = GCaja Then
            ButtonEliminarLinea.Enabled = True
            Grid.ReadOnly = False
            EsPaseTerceros = False
            ButtonAceptaPase.Visible = False
            If Not PBloqueaFunciones Then ArmaDtSaldos()
        Else
            ButtonEliminarLinea.Enabled = False
            Grid.ReadOnly = True
            EsPaseTerceros = True
        End If

        If DtPaseCabeza.Rows(0).Item("Aceptado") Then
            ButtonEliminarLinea.Enabled = False
            Grid.ReadOnly = True
            DateTimeAceptado.Visible = True
        Else
            DateTimeAceptado.Visible = False
        End If

        CalculaTotales()

        If PPase = 0 Then
            ButtonAceptar.Text = "Graba Pase Caja"
        Else
            ButtonAceptar.Text = "Modifica Pase Caja"
        End If

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtPaseCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Pase")
        AddHandler Enlace.Format, AddressOf FormatPase
        TextPase.DataBindings.Clear()
        TextPase.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CajaOrigen")
        ComboCajaOrigen.DataBindings.Clear()
        ComboCajaOrigen.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CajaDestino")
        ComboCajaDestino.DataBindings.Clear()
        ComboCajaDestino.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaAceptado")
        DateTimeAceptado.DataBindings.Clear()
        DateTimeAceptado.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Aceptado")
        CheckAceptado.DataBindings.Clear()
        CheckAceptado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatPase(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = Format(Numero.Value, "0000-00000000")
        End If

    End Sub
    Private Function HacerAlta(ByVal DtPaseCabezaAux As DataTable, ByVal DtPaseDetalleAux As DataTable, ByVal DtChequesAux As DataTable, ByVal DtRecuperoSeniaAux As DataTable) As Boolean

        'Graba Facturas.
        Dim NumeroPase As Double
        Dim Patron As String = GCaja & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim NumeroW As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroPase = UltimaNumeracion(ConexionPase)
            If NumeroPase < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DtPaseCabezaAux.Rows(0).Item("Pase") = NumeroPase
            For Each Row As DataRow In DtPaseDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Pase") = NumeroPase
                End If
            Next

            NumeroW = ActualizaPase("A", DtPaseCabezaAux, DtPaseDetalleAux, DtChequesAux, DtRecuperoSeniaAux)

            If NumeroW >= 0 Then Exit For
            If NumeroW = -2 Then Exit For
            If NumeroW = -1 And i = 50 Then Exit For
        Next

        If NumeroW = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If NumeroW > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PPase = NumeroPase
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaPase(ByVal Funcion As String, ByVal DtPaseCabezaAux As DataTable, ByVal DtPaseDetalleAux As DataTable, ByVal DtChequesAux As DataTable, ByVal DtRecuperoSeniaAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtPaseCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtPaseCabezaAux.GetChanges, "PaseCajaCabeza", ConexionPase)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtPaseDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtPaseDetalleAux.GetChanges, "PaseCajaDetalle", ConexionPase)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtChequesAux.GetChanges) Then
                    Resul = GrabaTabla(DtChequesAux.GetChanges, "Cheques", ConexionPase)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRecuperoSeniaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRecuperoSeniaAux.GetChanges, "RecuperoSenia", ConexionPase)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                If Funcion <> "B" Then
                    Return DtPaseCabezaAux.Rows(0).Item("Pase")
                Else
                    Return 1000
                End If
                '
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub ActualizaArchivos(ByRef DtDetalleAux As DataTable)

        Dim RowsBusqueda() As DataRow
        Dim Item As Integer = 0

        For Each Row As DataRow In DtDetalleAux.Rows
            If Row("Item") > Item Then Item = Row("Item")
        Next

        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("MedioPago") <> 0 Then    'se dio caso desconocido de una linea con medioPago = 0 y Importe = 0. 
                    RowsBusqueda = DtDetalleAux.Select("Item = " & Row("Item"))
                    If RowsBusqueda.Length = 0 Then
                        Dim Row1 As DataRow = DtDetalleAux.NewRow
                        Row1("Pase") = PPase
                        Item = Item + 1
                        Row1("Item") = Item
                        Row1("Mediopago") = Row("MedioPago")
                        Row1("Importe") = Row("Importe")
                        Row1("Banco") = Row("Banco")
                        Row1("Cuenta") = Row("Cuenta")
                        Row1("ClaveCheque") = Row("ClaveCheque")
                        Row1("Recibo") = Row("Recibo")
                        DtDetalleAux.Rows.Add(Row1)
                    Else
                        If Row("MedioPago") <> RowsBusqueda(0).Item("MedioPago") Then RowsBusqueda(0).Item("MedioPago") = Row("MedioPago")
                        If Row("Importe") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
                        If Row("Banco") <> RowsBusqueda(0).Item("Banco") Then RowsBusqueda(0).Item("Banco") = Row("Banco")
                        If Row("Cuenta") <> RowsBusqueda(0).Item("Cuenta") Then RowsBusqueda(0).Item("Cuenta") = Row("Cuenta")
                        If Row("ClaveCheque") <> RowsBusqueda(0).Item("ClaveCheque") Then RowsBusqueda(0).Item("ClaveCheque") = Row("ClaveCheque")
                    End If
                End If
            End If
        Next

        For Each Row As DataRow In DtPaseDetalle.Rows
            RowsBusqueda = DtGrid.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then RowsBusqueda = DtDetalleAux.Select("Item = " & Row("Item")) : RowsBusqueda(0).Delete()
        Next

    End Sub
    Private Sub CalculaTotales()

        Dim TotalChequesTerceros As Decimal = 0
        Dim TotalRecuperoVales As Decimal = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsDBNull(Row.Cells("Concepto").Value) Then
                If Row.Cells("Concepto").Value = 3 Then TotalChequesTerceros = TotalChequesTerceros + Row.Cells("Importe").Value
                If Row.Cells("Concepto").Value = 6 Then TotalRecuperoVales = TotalRecuperoVales + Row.Cells("Importe").Value
            End If
        Next

        TexTotalChequesTerceros.Text = FormatNumber(TotalChequesTerceros, GDecimales)
        TextTotalRecuperoVales.Text = FormatNumber(TotalRecuperoVales, GDecimales)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Concepto.DataSource = DtFormasPago
        Concepto.DisplayMember = "Nombre"
        Concepto.ValueMember = "Clave"

        Banco.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        Row = Banco.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Banco.DataSource.Rows.Add(Row)
        Banco.DisplayMember = "Nombre"
        Banco.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = CreaDtParaGrid()

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Recibo)

    End Sub
    Private Sub CreaDtSaldos()

        DtSaldos = New DataTable

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtSaldos.Columns.Add(MedioPago)

        Dim Saldo As New DataColumn("Saldo")
        Saldo.DataType = System.Type.GetType("System.Decimal")
        DtSaldos.Columns.Add(Saldo)

    End Sub
    Private Sub ArmaDtSaldos()
        'Arma Dt de saldos de conceptos (no cheques de terecero y recupero de senia).

        Dim RowsBusqueda() As DataRow

        Dim Operacion As Integer
        If PAbierto Then
            Operacion = 1
        Else : Operacion = 2
        End If

        CreaDtSaldos()

        For Each Row As DataRow In PDtGrid.Rows
            If Row("Operacion") = Operacion And Row("medioPago") <> 3 And Row("medioPago") <> 6 Then
                Dim Row1 As DataRow = DtSaldos.NewRow
                Row1("MedioPago") = Row("Mediopago")
                Row1("Saldo") = Row("Saldo")
                DtSaldos.Rows.Add(Row1)
            End If
        Next
        For Each Row As DataRow In DtGrid.Rows
            If Row("MedioPago") <> 3 And Row("medioPago") <> 6 Then
                RowsBusqueda = DtSaldos.Select("MedioPago = " & Row("MedioPago"))
                If RowsBusqueda.Length = 0 Then
                    Dim Row1 As DataRow = DtSaldos.NewRow
                    Row1("medioPago") = Row("medioPago")
                    Row1("Saldo") = Row("Importe")
                    PDtGrid.Rows.Add(Row1)
                Else
                    RowsBusqueda(0).Item("Saldo") = RowsBusqueda(0).Item("Saldo") + Row("Importe")
                End If
            End If
        Next

    End Sub
    Private Function HallaTipo(ByVal MedioPago As Integer) As Integer

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtFormasPago.Select("Clave = " & MedioPago)
        Return RowsBusqueda(0).Item("Tipo")

    End Function
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid.Rows.Item(Grid.CurrentRow.Index)
        Row.Delete()

        CalculaTotales()

    End Sub
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Dim Patron As String = PCajaOrigen & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Pase) FROM PaseCajaCabeza WHERE CAST(CAST(PaseCajaCabeza.Pase AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return CDbl(GCaja & Format(1, "00000000"))
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function TieneSaldo(ByVal ConexionStr As String, ByVal MedioPago As Integer, ByVal Importe As Decimal) As Boolean

        Dim RowsBusqueda() As DataRow
        Dim RowsBusqueda1() As DataRow
        Dim ImporteAnt As Decimal = 0

        RowsBusqueda = DtSaldos.Select("MedioPago = " & MedioPago)
        If RowsBusqueda.Length = 0 Then Return False

        If DtPaseDetalle.Rows.Count <> 0 Then
            RowsBusqueda1 = DtPaseDetalle.Select("MedioPago = " & MedioPago)
            If RowsBusqueda1.Length = 0 Then
                ImporteAnt = 0
            Else : ImporteAnt = RowsBusqueda1(0).Item("Importe")
            End If
        End If

        If (RowsBusqueda(0).Item("Saldo") + ImporteAnt - Importe) < 0 Then Return False

        Return True

    End Function
    Private Sub IngresaCheques(ByVal ListaDeChequesAux As List(Of ItemCheque), ByVal MedioPago As Integer)

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("MedioPago") <> MedioPago Then
                DtGrid.ImportRow(Row)
            End If
        Next

        For Each Fila As ItemCheque In ListaDeChequesAux
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("MedioPago") = MedioPago
            Row1("Banco") = Fila.Banco
            Row1("Serie") = Fila.Serie
            Row1("Numero") = Fila.Numero
            Row1("Importe") = Fila.Importe
            Row1("ClaveCheque") = Fila.ClaveCheque
            Row1("ClaveChequeVisual") = Fila.ClaveCheque
            Row1("Fecha") = Fila.Fecha
            Row1("EmisorCheque") = Fila.EmisorCheque
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub IngresaRecuperoSenia(ByVal Lista As List(Of ItemRecuperoSenia))

        Grid.DataSource = Nothing
        Dim Dt As DataTable = DtGrid.Copy
        DtGrid.Clear()

        For Each Row As DataRow In Dt.Rows
            If Row("MedioPago") <> 6 Then
                DtGrid.ImportRow(Row)
            End If
        Next

        For Each Fila As ItemRecuperoSenia In Lista
            Dim Row1 As DataRow = DtGrid.NewRow
            Row1("MedioPago") = 6
            Row1("Recibo") = Fila.Nota
            Row1("ClaveInterna") = Fila.Nota
            Row1("Comprobante") = Fila.Vale
            Row1("Importe") = Fila.Importe
            DtGrid.Rows.Add(Row1)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub PresentaLineasGrid()

        'Precenta las lineas del grid.
        Dim RowsBusqueda() As DataRow

        For Each Row As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row.Cells("Concepto").Value) Then
                RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                ArmaGridSegunConcepto(Row, RowsBusqueda(0).Item("Tipo"), 80, False, False, PAbierto)
            End If
        Next

    End Sub
    Private Function Valida() As Boolean

        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If Not ConsistePagos(Grid, DtFormasPago, 80, False) Then Return False

        Dim Row As DataGridViewRow
        Dim Operacion As Integer

        If PAbierto Then
            Operacion = 1
        Else : Operacion = 2
        End If

        For i As Integer = 0 To Grid.Rows.Count - 2
            Row = Grid.Rows(i)
            If Row.Cells("Concepto").Value <> 3 And Row.Cells("Concepto").Value <> 6 Then
                If Not TieneSaldo(ConexionPase, Row.Cells("Concepto").Value, Row.Cells("Importe").Value) Then
                    MsgBox("No Existe Saldo en Caja para este Concepto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Concepto")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

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
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Or Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            Else
                Exit Sub
            End If

            If IsNothing(cb.SelectedValue) Then Exit Sub

            If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
                If cb.SelectedIndex <= 0 Then Exit Sub
                ArmaGridSegunConcepto(Grid.Rows(e.RowIndex), HallaTipo(cb.SelectedValue), 80, False, False, PAbierto)
            End If
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        If Grid.Columns(e.ColumnIndex).Name = "Neto" Or Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(Grid.CurrentCell.ToString) Then Grid.CurrentCell.Value = 0
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If Grid.Columns(e.ColumnIndex).Name = "Concepto" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Concepto")) Then
                Grid.CurrentRow.Cells("Neto").Value = 0
                Grid.CurrentRow.Cells("Alicuota").Value = 0
                Grid.CurrentRow.Cells("Iva").Value = 0
                Grid.CurrentRow.Cells("Importe").Value = 0
                Grid.CurrentRow.Cells("Banco").Value = 0
                Grid.CurrentRow.Cells("Cuenta").Value = 0
                Grid.CurrentRow.Cells("Serie").Value = ""
                Grid.CurrentRow.Cells("Numero").Value = 0
                Grid.CurrentRow.Cells("Fecha").Value = "1/1/1800"
                Grid.CurrentRow.Cells("Cambio").Value = 0
                Grid.CurrentRow.Cells("EmisorCheque").Value = ""
                Grid.CurrentRow.Cells("ClaveCheque").Value = 0
                Grid.CurrentRow.Cells("Comprobante").Value = 0
                Grid.CurrentRow.Cells("FechaComprobante").Value = "1/1/1800"
                Grid.Refresh()
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If EsPaseTerceros Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            If Grid.CurrentRow.Cells("Concepto").Value = 3 Then
                SeleccionarCheques.PEsChequeEnCartera = True
                SeleccionarCheques.PCaja = PCajaOrigen
                SeleccionarCheques.PAbierto = PAbierto
                SeleccionarCheques.PListaDeCheques = New List(Of ItemCheque)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 3 Then
                        Dim Item As New ItemCheque
                        Item.ClaveCheque = Row("ClaveCheque")
                        SeleccionarCheques.PListaDeCheques.Add(Item)
                    End If
                Next
                SeleccionarCheques.ShowDialog()
                If SeleccionarCheques.PListaDeCheques.Count <> 0 Then
                    IngresaCheques(SeleccionarCheques.PListaDeCheques, 3)
                End If
                SeleccionarCheques.Dispose()
                PresentaLineasGrid()
                CalculaTotales()
            End If
            '
            If Grid.CurrentRow.Cells("Concepto").Value = 6 Then      'Seña terceros.
                SeleccionarVarios.PEsValeTercerosOrdenPago = True
                SeleccionarVarios.PCaja = PCajaOrigen
                SeleccionarVarios.PAbierto = PAbierto
                SeleccionarVarios.PEmisor = 0
                SeleccionarVarios.PListaDeRecuperoSenia = New List(Of ItemRecuperoSenia)
                For Each Row As DataRow In DtGrid.Rows
                    If Row("MedioPago") = 6 Then
                        Dim Item As New ItemRecuperoSenia
                        Item.Nota = Row("Recibo")
                        SeleccionarVarios.PListaDeRecuperoSenia.Add(Item)
                    End If
                Next
                SeleccionarVarios.ShowDialog()
                If SeleccionarVarios.PListaDeRecuperoSenia.Count <> 0 Then
                    IngresaRecuperoSenia(SeleccionarVarios.PListaDeRecuperoSenia)
                End If
                SeleccionarVarios.Dispose()
                PresentaLineasGrid()
                CalculaTotales()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "Neto" Or _
           Grid.Columns(e.ColumnIndex).Name = "Alicuota" Or Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "Cambio" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#.##")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaComprobante" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = "1/1/1800" Then e.Value = ""
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Or Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "ClaveCheque" Or Grid.Columns(e.ColumnIndex).Name = "ClaveChequeVisual" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, 0)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCuenta" Then
            e.Value = Nothing
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

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Detalle" Then Exit Sub
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Banco" Then Exit Sub

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Not Grid.Columns(Grid.CurrentCell.ColumnIndex).Name = "Concepto" Then
            If IsDBNull(Grid.CurrentRow.Cells("Concepto").Value) Then
                e.KeyChar = ""
                Exit Sub
            End If
            If Grid.CurrentRow.Cells("Concepto").Value = 0 Then
                e.KeyChar = ""
                Exit Sub
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Numero" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Comprobante" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuenta" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Serie" Then
            If e.KeyChar = "" Then Exit Sub
            e.KeyChar = e.KeyChar.ToString.ToUpper
            If Asc(e.KeyChar) < 65 Or Asc(e.KeyChar) > 90 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Neto" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Alicuota" Or _
           Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Importe" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cambio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Item") = 0
        Row("MedioPago") = 0
        Row("Detalle") = ""
        Row("Neto") = 0
        Row("Alicuota") = 0
        Row("ImporteIva") = 0
        Row("Banco") = 0
        Row("Fecha") = "1/1/1800"
        Row("Cuenta") = 0
        Row("Serie") = ""
        Row("Numero") = 0
        Row("EmisorCheque") = ""
        Row("Cambio") = 0
        Row("Importe") = 0
        Row("Comprobante") = 0
        Row("FechaComprobante") = "1/1/1800"
        Row("ClaveCheque") = 0
        Row("ClaveChequeVisual") = 0
        Row("ClaveInterna") = 0
        Row("Recibo") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Detalle") Then
            If IsDBNull(e.Row("Detalle")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If e.Column.ColumnName.Equals("Neto") Then
            If IsDBNull(e.Row("Neto")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If Not IsDBNull(e.Row("Alicuota")) Then
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Alicuota") / 100)
                e.Row("Importe") = e.ProposedValue + e.Row("ImporteIva")
                Grid.Refresh()
            End If
        End If

        If e.Column.ColumnName.Equals("Alicuota") Then
            If IsDBNull(e.Row("Alicuota")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue >= 100 Then
                MsgBox("Alicuota Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                e.ProposedValue = e.Row("Alicuota")
                Exit Sub
            End If
            If Not IsDBNull(e.Row("Neto")) Then
                e.Row("ImporteIva") = Trunca(e.ProposedValue * e.Row("Neto") / 100)
                e.Row("Importe") = e.Row("Neto") + e.Row("ImporteIva")
                Grid.Refresh()
            End If
        End If

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.Row("Cuenta")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Numero") Then
            If IsDBNull(e.Row("Numero")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cambio") Then
            If IsDBNull(e.Row("Cambio")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("Comprobante")) Then
            If IsDBNull(e.Row("Comprobante")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If (e.Column.ColumnName.Equals("EmisorCheque")) Then
            If IsDBNull(e.Row("EmisorCheque")) Then Exit Sub
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        CalculaTotales()

    End Sub
    Private Sub DtGrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("MedioPago")) Then
            If e.ProposedValue <> 0 And e.ProposedValue <> 3 And e.ProposedValue <> 6 Then
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtGrid.Select("MedioPago = " & e.ProposedValue)
                If RowsBusqueda.Length > 0 Then
                    MsgBox("Concepto ya Existe.", MsgBoxStyle.Information)
                    Dim Row As DataRowView = bs.Current
                    Row.Delete()
                End If
            End If
        End If

    End Sub
End Class