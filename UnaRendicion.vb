Imports System.Transactions
Public Class UnaRendicion
    Public PRendicion As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim DtNumero As DataTable
    '
    Dim FondoFijo As Integer
    Dim Numero As Integer
    Dim ConexionFondoFijo As String
    Dim ImporteW As Decimal
    Dim CerradoW As Boolean
    Private Sub UnaRendicion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(2) Then PBloqueaFunciones = True

        ComboFondoFijo.DataSource = Nothing
        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = 0
        With ComboFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        If PRendicion = 0 Then
            OpcionFondoFijo.ShowDialog()
            If OpcionFondoFijo.PRegresar Then OpcionFondoFijo.Dispose() : Me.Close() : Exit Sub
            FondoFijo = OpcionFondoFijo.PFondoFijo
            Numero = OpcionFondoFijo.PNumeroFondoFijo
            PAbierto = OpcionFondoFijo.PAbierto
            OpcionFondoFijo.Dispose()
            If FondoFijoCerrado(Numero, PAbierto) Then
                MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
                Me.Close()
                Exit Sub
            End If
        End If

        If PAbierto Then
            ConexionFondoFijo = Conexion
            PictureCandado.Image = ImageList1.Images.Item("Abierto")
        Else : ConexionFondoFijo = ConexionN
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        End If

        If Not PermisoTotal Then PictureCandado.Visible = False

        GModificacionOk = False

        ArmaArchivos()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(Numero, PAbierto) And PRendicion = 0 Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If PRendicion = 0 And IsNothing(Dt.GetChanges) Then Exit Sub

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PRendicion = 0 Then
            If HacerAlta() Then ArmaArchivos()
        Else
            If Dt.Rows(0).Item("Importe") <> ImporteW Then
                If HacerModificacionPorImporte() Then ArmaArchivos()
            End If
            If Dt.Rows(0).Item("Cerrado") <> CerradoW Then
                If Dt.Rows(0).Item("Cerrado") Then
                    If HacerModificacionPorCierre() Then ArmaArchivos()
                Else
                    If HacerModificacionPorApertura() Then ArmaArchivos()
                End If
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorra.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If FondoFijoCerrado(Numero, PAbierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        If Dt.Rows(0).Item("Importe") <> ImporteW Or Dt.Rows(0).Item("Cerrado") <> CerradoW Then
            MsgBox("Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Dt.Rows(0).Item("Cerrado") Then
            MsgBox("Rendición Cerrada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaFacturas(Dt.Rows(0).Item("Rendicion")) > 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Fondo Fijo Tiene Facturas Informadas. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtAux As DataTable = Dt.Copy

        DtAux.Rows(0).Delete()

        Dim Resul As Double

        Dim DtFondoFijoW As New DataTable
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        Dim DtNumero As New DataTable

        Resul = ActualizaArchivo(DtAux, DtFondoFijoW, DtAsientoCabeza, DtAsientoDetalle, DtNumero)

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Borrado Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
            Exit Sub
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PRendicion = 0

        UnaRendicion_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PRendicion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7204
        If PAbierto Then
            ListaAsientos.PDocumentoB = Numero
        Else
            ListaAsientos.PDocumentoN = Numero
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub TextImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImporte.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, GDecimales)

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        Dim Sql As String = "SELECT * FROM RendicionFondoFijo WHERE Rendicion = " & PRendicion & ";"
        If Not Tablas.Read(Sql, ConexionFondoFijo, Dt) Then Me.Close() : Exit Sub
        If PRendicion <> 0 And Dt.Rows.Count = 0 Then
            MsgBox("Rendición No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PRendicion = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("FondoFijo") = FondoFijo
            Row("Numero") = Numero
            Row("Fecha") = Now
            Row("Importe") = 0
            Row("ImporteFacturas") = 0
            Row("Saldo") = 0
            Row("Cerrado") = False
            Dt.Rows.Add(Row)
        End If

        MuestraCabeza()

        ImporteW = Dt.Rows(0).Item("Importe")
        CerradoW = Dt.Rows(0).Item("Cerrado")
        Numero = Dt.Rows(0).Item("Numero")

        If PRendicion = 0 Then
            CheckCerrado.Enabled = False
        Else : CheckCerrado.Enabled = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "FondoFijo")
        ComboFondoFijo.DataBindings.Clear()
        ComboFondoFijo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Numero")
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Rendicion")
        TextRendicion.DataBindings.Clear()
        TextRendicion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "ImporteFacturas")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporteFacturas.DataBindings.Clear()
        TextImporteFacturas.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Cerrado")
        CheckCerrado.DataBindings.Clear()
        CheckCerrado.DataBindings.Add(Enlace)

        TextImporteReposiciones.Text = FormatNumber(CDec(Dt.Rows(0).Item("ImporteFacturas")) - CDec(Dt.Rows(0).Item("Saldo")), GDecimales)

    End Sub
    Private Sub FormatRendicion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = ""
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function HacerAlta() As Boolean

        Dim DtNumero As New DataTable
        Dim Resul As Integer = 0
        Dim UltimoNumero As Integer

        Dim DtFondoFijoAux As New DataTable
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        For i As Integer = 1 To 50
            DtNumero = New DataTable
            If Not Tablas.Read("SELECT * FROM NumeracionFondoFijo;", Conexion, DtNumero) Then
                MsgBox("Error Base de Datos al leer Tabla: NumeracionFondoFijo", MsgBoxStyle.Critical)
                Return False
            End If
            UltimoNumero = DtNumero.Rows(0).Item("Rendicion") + 1
            Dt.Rows(0).Item("Rendicion") = UltimoNumero
            DtNumero.Rows(0).Item("Rendicion") = UltimoNumero

            Resul = ActualizaArchivo(Dt, DtFondoFijoAux, DtAsientoCabeza, DtAsientoDetalle, DtNumero)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            PRendicion = UltimoNumero
            Return True
        End If

    End Function
    Private Function HacerModificacionPorImporte() As Boolean

        If FondoFijoCerrado(Numero, PAbierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Return True
        End If

        Dim Resul As Double

        Dim DtFondoFijoAux As New DataTable
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        Dim DtNumero As New DataTable

        Resul = ActualizaArchivo(Dt, DtFondoFijoAux, DtAsientoCabeza, DtAsientoDetalle, DtNumero)

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
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
    Private Function HacerModificacionPorCierre() As Boolean

        Dim DtFondoFijoW As New DataTable
        If Not Tablas.Read("SELECT * FROM FondosFijos WHERE Numero = " & Numero & ";", ConexionFondoFijo, DtFondoFijoW) Then Return False

        If DtFondoFijoW.Rows(0).Item("Cerrado") Then
            MsgBox("Fondo Fijo ya Fue Cerrado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Dt.Rows(0).Item("ImporteFacturas") = 0 Then
            MsgBox("Rendición No Tiene Facturas Informadas. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim SaldosRendicionesCerradas As Decimal = HallaSaldosRendicionesCerradas(Numero, ConexionFondoFijo)

        If CDec(Dt.Rows(0).Item("Saldo")) + SaldosRendicionesCerradas > CDec(DtFondoFijoW.Rows(0).Item("Saldo")) Then
            MsgBox("Importe Fondo Fijo Distinto al Importe Facturas de la Rendición. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDec(Dt.Rows(0).Item("Saldo")) + SaldosRendicionesCerradas = CDec(DtFondoFijoW.Rows(0).Item("Saldo")) Then
            DtFondoFijoW.Rows(0).Item("Cerrado") = True
        End If

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento Then
            Dim ImporteParaAsiento As Decimal = Dt.Rows(0).Item("Saldo") + SaldosRendicionesCerradas
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Exit Function
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Exit Function
            If Not GeneraAsientoPorCierre(DtAsientoCabeza, DtAsientoDetalle, ImporteParaAsiento) Then Exit Function
        End If

        Dim Resul As Integer
        Dim NumeroAsiento As Double

        Dim DtNumero As New DataTable

        For I As Integer = 1 To 50
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabeza.Rows(0).Item("Asiento") = 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionFondoFijo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = Numero
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaArchivo(Dt, DtFondoFijoW, DtAsientoCabeza, DtAsientoDetalle, DtNumero)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And I = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
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
    Private Function HacerModificacionPorApertura() As Boolean

        Dim DtFondoFijoW As New DataTable
        If Not Tablas.Read("SELECT * FROM FondosFijos WHERE Numero = " & Numero & ";", ConexionFondoFijo, DtFondoFijoW) Then Return False

        DtFondoFijoW.Rows(0).Item("Cerrado") = False

        Dim SaldoRendicionesCerradas As Decimal = HallaSaldosRendicionesCerradas(Numero, ConexionFondoFijo)
        Dim ImporteParaAsiento As Decimal = SaldoRendicionesCerradas - Dt.Rows(0).Item("Saldo")

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento Then
            If Not GeneraAsientoPorApertura(DtAsientoCabeza, DtAsientoDetalle, ImporteParaAsiento) Then Return False
        End If

        Dim DtNumero As New DataTable

        Dim Resul As Integer = ActualizaArchivo(Dt, DtFondoFijoW, DtAsientoCabeza, DtAsientoDetalle, DtNumero)

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
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
    Private Function GeneraAsientoPorCierre(ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Importe As Decimal) As Boolean

        If Not HallaAsientosCabeza(7204, Numero, DtAsientoCabeza, ConexionFondoFijo) Then Return False

        If DtAsientoCabeza.Rows.Count = 0 Then
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle, Importe) Then Return False
            Return True
        End If

        'Asiento Exite.
        Dim Asiento As Double = DtAsientoCabeza.Rows(0).Item("Asiento")
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & Asiento & ";", ConexionFondoFijo, DtAsientoDetalle) Then Return False
        For Each Row As DataRow In DtAsientoDetalle.Rows
            Row.Delete()
        Next

        Dim DtAsientoCabezaAux As DataTable = DtAsientoCabeza.Clone
        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
        If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux, Importe) Then Return False

        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row("Asiento") = Asiento
            Dim Row1 As DataRow = DtAsientoDetalle.NewRow
            For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                Row1.Item(I) = Row.Item(I)
            Next
            DtAsientoDetalle.Rows.Add(Row1)
        Next

        DtAsientoCabezaAux.Dispose()
        DtAsientoDetalleAux.Dispose()

        Return True

    End Function
    Private Function GeneraAsientoPorApertura(ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Importe As Decimal) As Boolean

        If Not HallaAsientosCabeza(7204, Numero, DtAsientoCabeza, ConexionFondoFijo) Then Return False

        If DtAsientoCabeza.Rows.Count = 0 Then Return True

        Dim Asiento As Double = DtAsientoCabeza.Rows(0).Item("Asiento")
        If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & Asiento & ";", ConexionFondoFijo, DtAsientoDetalle) Then Return False
        For Each Row As DataRow In DtAsientoDetalle.Rows
            Row.Delete()
        Next

        If Importe = 0 Then
            DtAsientoCabeza.Rows(0).Delete()
            Return True
        End If

        Dim DtAsientoCabezaAux As DataTable = DtAsientoCabeza.Clone
        Dim DtAsientoDetalleAux As DataTable = DtAsientoDetalle.Clone
        If Not ArmaArchivosAsiento("A", DtAsientoCabezaAux, DtAsientoDetalleAux, Importe) Then Return False

        For Each Row As DataRow In DtAsientoDetalleAux.Rows
            Row("Asiento") = Asiento
            Dim Row1 As DataRow = DtAsientoDetalle.NewRow
            For I As Integer = 0 To DtAsientoDetalleAux.Columns.Count - 1
                Row1.Item(I) = Row.Item(I)
            Next
            DtAsientoDetalle.Rows.Add(Row1)
        Next

        DtAsientoCabezaAux.Dispose()
        DtAsientoDetalleAux.Dispose()

        Return True

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable, ByVal Importe As Double) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim Item As New ItemListaConceptosAsientos

        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = Importe
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(7204, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False
        End If

        Return True

    End Function
    Private Function ActualizaArchivo(ByVal DtAux As DataTable, ByVal DtFondoFijoAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtNumeroAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Fondo Fijo.
                If Not IsNothing(DtAux.GetChanges) Then
                    Resul = GrabaTabla(DtAux.GetChanges, "RendicionFondoFijo", ConexionFondoFijo)
                    If Resul <= 0 Then Return Resul
                End If
                'Actualiza Ultima Fondo Fijo.
                If Not IsNothing(DtFondoFijoAux.GetChanges) Then
                    Resul = GrabaTabla(DtFondoFijoAux.GetChanges, "FondosFijos", ConexionFondoFijo)
                    If Resul <= 0 Then Return Resul
                End If
                'Actualiza Asientos.
                If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabeza.GetChanges, "AsientosCabeza", ConexionFondoFijo)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalle.GetChanges, "AsientosDetalle", ConexionFondoFijo)
                    If Resul <= 0 Then Return Resul
                End If
                'Actualiza Numeracion.
                If Not IsNothing(DtNumeroAux.GetChanges) Then
                    Resul = GrabaTabla(DtNumeroAux.GetChanges, "NumeracionFondoFijo", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function HallaFacturas(ByVal Rendicion As Integer) As Integer

        Dim Sql As String = "SELECT COUNT(Factura) FROM FacturasProveedorCabeza WHERE Rendicion = " & Rendicion & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionFondoFijo)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: RendicionFondoFijo.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If Dt.Rows(0).Item("Importe") <> ImporteW And Dt.Rows(0).Item("Cerrado") <> CerradoW Then
            MsgBox("Importe y Cerrado no Deben Modificarse Simultaneamente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Dt.Rows(0).Item("Importe") <> ImporteW Then
            If Dt.Rows(0).Item("Importe") < Dt.Rows(0).Item("ImporteFacturas") Then
                MsgBox("Importe Facturas Informadas Supera Nuevo Importe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function

End Class