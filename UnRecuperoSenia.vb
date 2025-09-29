Public Class UnRecuperoSenia
    Public PNota As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim ConexionNota As String
    Dim ImporteW As Double
    Private Sub UnRecuperoSenia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(8) Then PBloqueaFunciones = True

        LlenaCombo(ComboProveedor, "", "Proveedores")

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PNota = 0 Then
            PideDatosEmisor()
            If ComboProveedor.SelectedValue = 0 Then Me.Close() : Exit Sub
        End If

        If PAbierto Then
            ConexionNota = Conexion
        Else : ConexionNota = ConexionN
        End If

        If PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Abierto")
        If Not PAbierto Then PictureCandado.Image = ImageList1.Images.Item("Cerrado")
        If Not PermisoTotal Then PictureCandado.Visible = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        GModificacionOk = False

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

        If PNota <> 0 And Dt.Rows(0).Item("Caja") <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & Dt.Rows(0).Item("Caja") & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtAux As DataTable = Dt.Copy

        If DtAux.Rows(0).Item("FechaVale") <> CDate(TextFechaVale.Text) Then DtAux.Rows(0).Item("FechaVale") = TextFechaVale.Text
        If PNota = 0 Then DtAux.Rows(0).Item("Saldo") = DtAux.Rows(0).Item("Importe")
        If PNota <> 0 Then
            If ImporteW <> DtAux.Rows(0).Item("Importe") Then
                Dim Asignado As Double = ImporteW - DtAux.Rows(0).Item("Saldo")
                If DtAux.Rows(0).Item("Importe") < Asignado Then
                    MsgBox("Nuevo Importe Hace Negativo el Saldo Asignado en Cuenta Corriente. Operación se CANCELA.")
                    Exit Sub
                Else
                    DtAux.Rows(0).Item("Saldo") = DtAux.Rows(0).Item("Importe") - Asignado
                End If
            End If
            If DtAux.Rows(0).Item("Usado") Then
                MsgBox("Vale fue Usado en orden de Pago. Operación se CANCELA.")
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PNota <> 0 Then
            If ExiteEnPaseCaja(ConexionNota, PNota) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Vale Esta en Proceso de Pase de Caja. Operación se CANCELA.")
                ArmaArchivos()
                Exit Sub
            End If
        End If

        If IsNothing(DtAux.GetChanges) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            HacerAlta(DtAux)
        Else
            HacerModificacion(DtAux)
        End If

        ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PNota = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Dt.Rows(0).Item("Caja") <> GCaja Then
            MsgBox("Modificación Solo Permitida para Caja " & Dt.Rows(0).Item("Caja") & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Recibo esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtAux As DataTable = Dt.Copy

        If DtAux.Rows(0).Item("Saldo") <> DtAux.Rows(0).Item("Importe") Then
            MsgBox("Recibo tiene Pago en Cuanta Corriente. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If DtAux.Rows(0).Item("Usado") Then
            MsgBox("Vale fue Usado en orden de Pago. Operación se CANCELA.")
            Exit Sub
        End If
        If DtAux.Rows(0).Item("PaseCaja") <> GCaja Then
            MsgBox("Vale Esta en la Caja " & DtAux.Rows(0).Item("PaseCaja") & ". Para la Baja Debe Retornarlo Nuevamente. Operación se CANCELA.")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If ExiteEnPaseCaja(ConexionNota, PNota) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Vale Esta en Proceso de Pase de Caja. Operación se CANCELA.")
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalle As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(7020, DtAux.Rows(0).Item("Nota"), DtAsientoCabeza, ConexionNota) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            For Each Row As DataRow In DtAsientoCabeza.Rows
                Row("Estado") = 3
            Next
        End If

        If MsgBox("Recupero Seña se Dara de Baja. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtAux.Rows(0).Item("Estado") = 3

        Dim resul As Integer = ActualizaMovi("B", DtAux, DtAsientoCabeza, DtAsientoDetalle, DtAsientoCabezaAux, ConexionNota)

        If resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul > 0 Then
            MsgBox("Movimiento Fue Dado de Baja Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PNota = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7020
        If PAbierto Then
            ListaAsientos.PDocumentoB = PNota
        Else
            ListaAsientos.PDocumentoN = PNota
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        UnRecuperoSenia_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevaIgualProveedor_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualProveedor.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNota = 0
        If Not ArmaArchivos() Then Me.Close() : Exit Sub

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaVale.Text = ""
        Else : TextFechaVale.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub TextImporte_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextImporte.KeyPress

        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub Textimporte_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextImporte.Validating

        If Not IsNumeric(TextImporte.Text) Then MsgBox("Incorrecto Importe.", MsgBoxStyle.Critical)

    End Sub
    Private Sub TextCantidad_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCantidad.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextVale_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextVale.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String

        Dt = New DataTable
        Sql = "SELECT * FROM RecuperoSenia WHERE Nota = " & PNota & ";"
        If Not Tablas.Read(Sql, ConexionNota, Dt) Then Return False
        If PNota <> 0 And Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Recupero Seña No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PNota = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("Nota") = 0
            Row("Proveedor") = ComboProveedor.SelectedValue
            Row("Vale") = 0
            Row("FechaVale") = "01/01/1800"
            Row("Fecha") = Now
            Row("Cantidad") = 0
            Row("Importe") = 0
            Row("Caja") = GCaja
            Row("PaseCaja") = GCaja
            Row("Usado") = False
            Row("Estado") = 1
            Row("Saldo") = 0
            Dt.Rows.Add(Row)
        End If

        MuestraCabeza()

        If PNota = 0 Then
            TextVale.ReadOnly = False
        Else : TextVale.ReadOnly = True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nota")
        AddHandler Enlace.Format, AddressOf FormatRecibo
        TextNota.DataBindings.Clear()
        TextNota.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Proveedor")
        ComboProveedor.DataBindings.Clear()
        ComboProveedor.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Vale")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextVale.DataBindings.Clear()
        TextVale.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cantidad")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextCantidad.DataBindings.Clear()
        TextCantidad.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Caja")
        TextCaja.DataBindings.Clear()
        TextCaja.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Usado")
        CheckUsado.DataBindings.Clear()
        CheckUsado.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        ImporteW = Row("Importe")

        If Row("FechaVale") = "01/01/1800" Then
            TextFechaVale.Text = ""
        Else
            TextFechaVale.Text = Format(Row("FechaVale"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatRecibo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub PideDatosEmisor()

        OpcionEmisorYLetra.PEsProveedor = True
        OpcionEmisorYLetra.PEsLocal = True
        OpcionEmisorYLetra.PEsSinLetra = True
        OpcionEmisorYLetra.PictureCandado.Visible = True
        OpcionEmisorYLetra.ShowDialog()
        ComboProveedor.SelectedValue = OpcionEmisorYLetra.PEmisor
        PAbierto = OpcionEmisorYLetra.PAbierto
        OpcionEmisorYLetra.Dispose()

    End Sub
    Private Function HacerAlta(ByVal DtAux As DataTable) As Boolean

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Return False
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Return False
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Return False
        End If

        Dim Resul As Double
        Dim NumeroNota As Integer
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion recupero.
            NumeroNota = UltimaNumeracion(ConexionNota)
            If NumeroNota < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            DtAux.Rows(0).Item("Nota") = NumeroNota
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroNota
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("A", DtAux, DtAsientoCabeza, DtAsientoDetalle, DtAsientoCabezaAux, ConexionNota)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -1 Then
            MsgBox("Error Base de Dato o Recupero Seña ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PNota = NumeroNota
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtAux As DataTable) As Boolean

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoCabezaAux As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If GGeneraAsiento Then
            If ImporteW <> DtAux.Rows(0).Item("Importe") Then
                If Not HallaAsientosCabeza(7020, DtAux.Rows(0).Item("Nota"), DtAsientoCabezaAux, ConexionNota) Then Return False
                For Each Row As DataRow In DtAsientoCabezaAux.Rows
                    Row("Estado") = 3
                Next
                If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Return False
                If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Return False
                If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Return False
            End If
        End If

        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionNota)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = DtAux.Rows(0).Item("Nota")
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("M", DtAux, DtAsientoCabeza, DtAsientoDetalle, DtAsientoCabezaaux, ConexionNota)

            If Resul >= 0 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtAux As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal ConexionStr As String) As Double

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    If Not IsNothing(DtAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RecuperoSenia;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoCabezaAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoCabeza.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoDetalle.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoDetalle.GetChanges)
                        End Using
                    End If
                    '
                    Trans.Commit()
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim Item As ItemListaConceptosAsientos

        If Funcion = "A" Then
            Item = New ItemListaConceptosAsientos
            Item.Clave = 6
            Item.Importe = CDbl(TextImporte.Text)
            ListaConceptos.Add(Item)
        End If

        If Not Asiento(7020, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Function ValeExiste(ByVal ConexionStr As String, ByVal Vale As Double, ByVal Proveedor As Integer) As Boolean

        Dim Sql As String = ""

        Sql = "SELECT Nota FROM RecuperoSenia WHERE Vale = " & Vale & " AND Proveedor = " & Proveedor & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else : Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al Leer Tabla Recupero de Vales.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecuperoSenia;", Miconexion)
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
    Private Function ExiteEnPaseCaja(ByVal ConexionStr As String, ByVal Recibo As Integer) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Recibo FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS D ON C.Pase = D.Pase WHERE C.Aceptado = 0 AND D.Recibo = " & Recibo & ";", Miconexion)
                    If Cmd.ExecuteScalar() = 0 Then
                        Return False
                    Else : Return True
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim Row As DataRow = Dt.Rows(0)

        If Row("Vale") = 0 Then
            MsgBox("Debe Informar Vale", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextVale.Focus()
            Return False
        End If

        If PNota = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
            If ValeExiste(ConexionNota, CDbl(TextVale.Text), ComboProveedor.SelectedValue) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Vale Ya Existe.", MsgBoxStyle.Critical)
                Return False
            End If
            Me.Cursor = System.Windows.Forms.Cursors.Default
        End If

        If Not ConsisteFecha(TextFechaVale.Text) Then
            MsgBox("Fecha Vale Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaVale.Focus()
            Return False
        End If

        If DiferenciaDias(DateTime1.Value, TextFechaVale.Text) < -365 Then
            MsgBox("Fecha Vale Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaVale.Focus()
            Return False
        End If

        If DiferenciaDias(DateTime1.Value, TextFechaVale.Text) > 0 Then
            MsgBox("Fecha Vale Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaVale.Focus()
            Return False
        End If

        If Row("Cantidad") = 0 Then
            MsgBox("Falta Cantidad de Envases.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextCantidad.Focus()
            Return False
        End If

        If Row("Importe") = 0 Then
            MsgBox("Falta Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextImporte.Focus()
            Return False
        End If

        If Not HallaOPR(ComboProveedor.SelectedValue, "P") And PAbierto Then
            If MsgBox("Proveedor Solo Opera a Candado Cerrado. Debe Cambiar Candado en Proveedor. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Return True

    End Function


   
End Class