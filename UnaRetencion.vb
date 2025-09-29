Public Class UnaRetencion
    'CodigoRetencion:  1- Retencion
    '                  2- Percepcion.
    Public PClaveRetencion As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtDocuRetenciones As DataTable
    Dim DtDocuAplicablesDeTerceros As New DataTable
    Dim DtDocuAplicablesATerceros As New DataTable
    '
    Dim CodigoRetencionAnt As Integer
    Dim OrigenPercepcionAnt As Integer
    Dim TipoImpositivoAnt As Integer
    Dim NombreW As String
    Dim UltimaClave As Integer
    Private Sub UnaRetencion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        GridAuto.AutoGenerateColumns = False

        LlenaCombosGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtDocuRetenciones = New DataTable
        If Not Tablas.Read("SELECT * FROM DocuRetenciones;", Conexion, DtDocuRetenciones) Then
            MsgBox("Error al leer Tabla: DocuRetenciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        Dt = New DataTable

        If Not Tablas.Read("SELECT * FROM Tablas WHERE Clave = " & PClaveRetencion & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 And PClaveRetencion <> 0 Then
            MsgBox("Retención/Percepción No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        'Retenciones/Precepciones que terceros hacen a la empresa.
        DtDocuAplicablesDeTerceros = New DataTable
        HallaDocuAplicablesDeTerceros(DtDocuAplicablesDeTerceros)
        For Each row As DataRow In DtDocuAplicablesDeTerceros.Rows
            RowsBusqueda = DtDocuRetenciones.Select("Clave = " & PClaveRetencion & " AND TipoDocumento = " & row("Clave"))
            If RowsBusqueda.Length <> 0 Then row("Sel") = True
        Next
        Grid.DataSource = DtDocuAplicablesDeTerceros

        'Retenciones/Precepciones que la empresa realiza a terceros.
        DtDocuAplicablesATerceros = New DataTable
        HallaDocuAplicablesATerceros(DtDocuAplicablesATerceros)
        For Each row As DataRow In DtDocuAplicablesATerceros.Rows
            RowsBusqueda = DtDocuRetenciones.Select("Clave = " & PClaveRetencion & " AND TipoDocumento = " & row("Clave"))
            If RowsBusqueda.Length <> 0 Then row("Sel") = True
        Next
        GridAuto.DataSource = DtDocuAplicablesATerceros

        If Not MuestraDatos() Then
            Me.Close()
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If PClaveRetencion = 0 Then  'No se muestra hasta tener el numero interno de la ret/perc. Es decir al grabarce.
            Panel2.Visible = False
            Panel3.Visible = False
        Else
            Panel2.Visible = True
            Panel3.Visible = True
        End If

    End Sub
    Private Sub UnaRetencion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonGarbar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGarbar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()
        GridAuto.EndEdit()

        Dim CambioFecha As Boolean
        Dim Str As String = Format(DateVigencia.Value.Year, "0000") & Format(DateVigencia.Value.Month, "00") & Format(DateVigencia.Value.Day, "00")
        If Dt.Rows(0).Item("Iva") <> Val(Str) Then CambioFecha = True

        If IsNothing(Dt.GetChanges) And IsNothing(DtDocuAplicablesDeTerceros.GetChanges) And IsNothing(DtDocuAplicablesATerceros.GetChanges) And CambioFecha = False Then
            MsgBox("No Hay Cambios.")
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Dim DtDocuRetencionesAux As DataTable = DtDocuRetenciones.Copy
        Dim DtAux As DataTable = Dt.Copy
        Dim DtRetencionesExentasAux As New DataTable

        DtAux.Rows(0).Item("Iva") = Val(Str)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        ActualizaDocuRetenciones(PClaveRetencion, DtDocuRetencionesAux)

        ActualizaArchivos(DtAux, DtDocuRetencionesAux, DtRetencionesExentasAux)

        If PClaveRetencion = 0 Then PClaveRetencion = UltimaClave

        Me.Cursor = System.Windows.Forms.Cursors.Default

        UnaRetencion_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PClaveRetencion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRow = Dt.Rows(0)

        If Row("Clave") <> 0 Then
            If FueUsada(Row("Clave")) Then MsgBox("Operación se CANCELA.", MsgBoxStyle.Critical) : Exit Sub
        End If

        If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtAux As DataTable = Dt.Copy
        Dim DtDocuRetencionesAux As DataTable = DtDocuRetenciones.Copy
        Dim DtRetencionesExentasAux As New DataTable

        For I As Integer = DtDocuRetencionesAux.Rows.Count - 1 To 0 Step -1
            Dim Row1 As DataRow = DtDocuRetencionesAux.Rows(I)
            If Row1.RowState <> DataRowState.Deleted Then
                If Row1("Clave") = Row("Clave") Then
                    Row1.Delete()
                End If
            End If
        Next

        If Not ActualizaExentoClientesProveedores("B", PClaveRetencion, DtRetencionesExentasAux) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("No se pudo Borrar esta Retec./Perc. como Exenta en Clientes y Provreedores. Operación se CANCELA.")
            Exit Sub
        End If

        DtAux.Rows(0).Delete()

        ActualizaArchivos(DtAux, DtDocuRetencionesAux, DtRetencionesExentasAux)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If GModificacionOk Then Me.Close() : Exit Sub

    End Sub
    Private Sub LupaCuentaDebito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LupaCuentaDebito.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            MaskedCuentaDebito.Text = Format(SeleccionarCuenta.PCuenta, "00000000000")
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub LupaCuentaCredito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LupaCuentaCredito.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            MaskedCuentaCredito.Text = Format(SeleccionarCuenta.PCuenta, "00000000000")
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub ButtonVerClientes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonVerClientes.Click

        Dim Directorio As String = System.AppDomain.CurrentDomain.BaseDirectory()

        MuestraExcel(Directorio & "ExcelOpcionesLibrosIva.xlsx")

    End Sub
    Private Sub TextFormula_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFormula.KeyPress

        EsNumerico(e.KeyChar, TextFormula.Text, 0)

    End Sub
    Private Sub TextTopeMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextTopeMes.KeyPress

        EsNumerico(e.KeyChar, TextTopeMes.Text, 2)

    End Sub
    Private Sub TextAlicuota_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAlicuota.KeyPress

        EsNumerico(e.KeyChar, TextAlicuota.Text, 2)

    End Sub
    Private Sub Textjurisdiccion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextJurisdiccion.KeyPress

        EsNumerico(e.KeyChar, TextJurisdiccion.Text, 0)

    End Sub
    Private Sub TextCodigoProvinciaIngresoBruto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCodigoProvinciaIngresoBruto.KeyPress

        If Not IsNumeric(e.KeyChar) Then  'OTRA MANERA ALTERNATIA A ESNUMERICO().   
            e.Handled = True
        End If

    End Sub
    Private Function MuestraDatos() As Boolean
       
        If PClaveRetencion = 0 Then
            AgregaCabeza()
        End If

        EnlazaCabeza()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub AgregaCabeza()

        Dim Row As DataRow = Dt.NewRow()
        AceraRowTablas(Row)

        Row("Tipo") = 25

        Dt.Rows.Add(Row)

    End Sub
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Row As DataRowView = MiEnlazador.Current
        CodigoRetencionAnt = Row("CodigoRetencion")
        OrigenPercepcionAnt = Row("OrigenPercepcion")
        Dim FechaStr As String = Row("Iva").ToString   'Vigencia.

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Clave")
        AddHandler Enlace.Format, AddressOf FormatCero
        TextClaveInterna.DataBindings.Clear()
        TextClaveInterna.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        AddHandler Enlace.Parse, AddressOf NombreStr
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)
        NombreW = TextNombre.Text

        Enlace = New Binding("SelectedValue", MiEnlazador, "CodigoRetencion")
        ComboCodigoRetencion.DataBindings.Clear()
        ComboCodigoRetencion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "OrigenPercepcion")
        ComboOrigenPercepcion.DataBindings.Clear()
        ComboOrigenPercepcion.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "CodigoAfipElectronico")
        comboCodigoAfipElectronico.DataBindings.Clear()
        comboCodigoAfipElectronico.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsPorProvincia")
        CheckEsPorProvincia.DataBindings.Clear()
        CheckEsPorProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuenta")
        AddHandler Enlace.Format, AddressOf FormatCuenta
        MaskedCuentaDebito.DataBindings.Clear()
        MaskedCuentaDebito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuenta2")
        AddHandler Enlace.Format, AddressOf FormatCuenta
        MaskedCuentaCredito.DataBindings.Clear()
        MaskedCuentaCredito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Formula")
        TextFormula.DataBindings.Clear()
        TextFormula.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "TopeMes")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextTopeMes.DataBindings.Clear()
        TextTopeMes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "AlicuotaRetencion")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextAlicuota.DataBindings.Clear()
        TextAlicuota.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Operador")
        AddHandler Enlace.Format, AddressOf FormatCero
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextJurisdiccion.DataBindings.Clear()
        TextJurisdiccion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CodigoProvinciaIIBB")
        TextCodigoProvinciaIngresoBruto.DataBindings.Clear()
        TextCodigoProvinciaIngresoBruto.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoPago")
        ComboTipoImpositivo.DataBindings.Clear()
        ComboTipoImpositivo.DataBindings.Add(Enlace)
        TipoImpositivoAnt = Row("TipoPago")

        If Row("Iva") <> 0 Then
            DateVigencia.Value = Strings.Left(FechaStr, 4) & "/" & Strings.Mid(FechaStr, 5, 2) & "/" & Strings.Right(FechaStr, 2) 'yyyyMMdd
        End If

    End Sub
    Private Sub FormatCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "00000000000")
        Else
            Numero.Value = Format(Numero.Value, "00000000000")
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub NombreStr(ByVal sender As Object, ByVal Nombre As ConvertEventArgs)

        Nombre.Value = Nombre.Value.ToString.Trim

    End Sub
    Private Sub FormatCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(0, "#")

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Dim Dt As New DataTable
        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "RETENCION"
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "PERCEPCION"
        Dt.Rows.Add(Row)
        '
        ComboCodigoRetencion.DataSource = Dt
        ComboCodigoRetencion.DisplayMember = "Nombre"
        ComboCodigoRetencion.ValueMember = "Clave"
        ComboCodigoRetencion.SelectedValue = 0

        'Origen Percepcion.
        Dim Dt2 As New DataTable
        Dt2.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt2.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt2.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Imp.IVA"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Imp.Ing. Bruto"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 3
        Row("Nombre") = "Otros Imp.Nacionales"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 4
        Row("Nombre") = "Imp.Municipales"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Imp.Internos"
        Dt2.Rows.Add(Row)
        '
        ComboOrigenPercepcion.DataSource = Dt2
        ComboOrigenPercepcion.DisplayMember = "Nombre"
        ComboOrigenPercepcion.ValueMember = "Clave"
        ComboOrigenPercepcion.SelectedValue = 0

        'Tipo Impositivo.
        Dim Dt3 As New DataTable
        Dt3.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt3.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt3.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Ingreso Bruto(IIBB)"
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 2
        Row("Nombre") = "SICREB"
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 3
        Row("Nombre") = "IVA"
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 4
        Row("Nombre") = "GANANCIAS"
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 5
        Row("Nombre") = "SUSS"
        Dt3.Rows.Add(Row)
        ' 
        Row = Dt3.NewRow
        Row("Clave") = 6
        Row("Nombre") = "Agente Rec. IIBB."
        Dt3.Rows.Add(Row)
        '
        ComboTipoImpositivo.DataSource = Dt3
        ComboTipoImpositivo.DisplayMember = "Nombre"
        ComboTipoImpositivo.ValueMember = "Clave"
        ComboTipoImpositivo.SelectedValue = 0

        'Para AFIP Electrónico.
        ComboCodigoAfipElectronico.DataSource = Tablas.Leer("SELECT CodigoAfipElectronico, Nombre FROM Tablas WHERE Tipo = 41;")
        Dim Row2 As DataRow = ComboCodigoAfipElectronico.DataSource.newrow()
        Row2("CodigoAfipElectronico") = 0
        Row2("Nombre") = ""
        ComboCodigoAfipElectronico.DataSource.rows.add(Row2)
        ComboCodigoAfipElectronico.DisplayMember = "Nombre"
        ComboCodigoAfipElectronico.ValueMember = "CodigoAfipElectronico"
        ComboCodigoAfipElectronico.SelectedValue = 0

    End Sub
    Private Sub ActualizaArchivos(ByVal DtAux As DataTable, ByVal DtDocuRetencionesAux As DataTable, ByVal DtRetencionesExentasAux As DataTable)

        Dim Dt As New DataTable
        UltimaClave = 0

        GModificacionOk = False

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Sql1 As String
                If Not IsNothing(DtAux.GetChanges) Then
                    Sql1 = "SELECT * FROM Tablas;"
                    Using DaP As New OleDb.OleDbDataAdapter(Sql1, Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtAux.GetChanges)
                    End Using
                    'Recupera valor de Clave(AutoIncremental) 
                    If PClaveRetencion = 0 Then
                        Tablas.Read("SELECT MAX(Clave) as Identidad FROM Tablas WHERE Tipo = 25;", Conexion, Dt)
                        UltimaClave = Dt.Rows(0).Item("Identidad")
                        For Each row As DataRow In DtRetencionesExentasAux.Rows
                            row("Clave") = UltimaClave
                        Next
                    End If
                End If
                If Not IsNothing(DtDocuRetencionesAux.GetChanges) Then
                    Sql1 = "SELECT * FROM DocuRetenciones;"
                    Using DaP As New OleDb.OleDbDataAdapter(Sql1, Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtDocuRetencionesAux.GetChanges)
                    End Using
                End If
                If Not IsNothing(DtRetencionesExentasAux.GetChanges) Then
                    Sql1 = "SELECT * FROM RetencionesExentas;"
                    Using DaP As New OleDb.OleDbDataAdapter(Sql1, Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtRetencionesExentasAux.GetChanges)
                    End Using
                End If
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox("Error de Base de datos. Operación se CANCELA." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Catch ex As DBConcurrencyException
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        Finally
        End Try

    End Sub
    Private Function RetencionOrdenPagoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Mediopago) FROM RecibosDetallePago WHERE MedioPago = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function RetencionCobranzaUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(MedioPago) FROM RecibosDetallePago WHERE MedioPago = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function RetencionNVLPUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Impuesto) FROM NVLPDetalle WHERE Impuesto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function RetencionLiquidacionProveedorUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM LiquidacionDetalleConceptos WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function RetencionFacturaProveedorUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Impuesto) FROM FacturasProveedorDetalle WHERE Impuesto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function RetencionGastoBancarioUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM GastosBancarioDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function RetencionCancelacionPrestamosUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM PrestamosMovimientoDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function PercepcionUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Comprobante) FROM RecibosPercepciones WHERE Percepcion = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function TieneATerceros() As Boolean

        For Each Row As DataRow In DtDocuAplicablesATerceros.Rows
            If Row("Sel") Then
                Return True
            End If
        Next

        Return False

    End Function
    Private Function ActualizaDocuRetenciones(ByVal Retencion As Integer, ByRef DtDocu As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        For Each Row1 As DataRow In DtDocuAplicablesDeTerceros.Rows
            RowsBusqueda = DtDocu.Select("Clave = " & Retencion & " AND TipoDocumento = " & Row1("Clave"))
            If Row1("Sel") Then
                If RowsBusqueda.Length = 0 Then
                    Dim RowA As DataRow = DtDocu.NewRow
                    RowA("Clave") = Retencion
                    RowA("TipoDocumento") = Row1("Clave")
                    DtDocu.Rows.Add(RowA)
                End If
            Else
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Delete()
                End If
            End If
        Next

        For Each Row1 As DataRow In DtDocuAplicablesATerceros.Rows
            RowsBusqueda = DtDocu.Select("Clave = " & Retencion & " AND TipoDocumento = " & Row1("Clave"))
            If Row1("Sel") Then
                If RowsBusqueda.Length = 0 Then
                    Dim RowA As DataRow = DtDocu.NewRow
                    RowA("Clave") = Retencion
                    RowA("TipoDocumento") = Row1("Clave")
                    DtDocu.Rows.Add(RowA)
                End If
            Else
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda(0).Delete()
                End If
            End If
        Next

    End Function
    Private Function ActualizaExentoClientesProveedores(ByVal Funcion As String, ByVal ClaveRetencion As Integer, ByRef DtRetencionesExentasAux As DataTable) As Boolean

        If Funcion <> "A" And Funcion <> "B" Then
            MsgBox("Error funcion-44", MsgBoxStyle.Critical)
            Return False
        End If

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * FROM RetencionesExentas WHERE Clave = " & ClaveRetencion & ";", Conexion, DtRetencionesExentasAux) Then Return False

        If Funcion = "A" Then
            If Not Tablas.Read("SELECT Clave FROM Proveedores;", Conexion, Dt) Then Return False
            For Each Row As DataRow In Dt.Rows
                Dim Row2 As DataRow = DtRetencionesExentasAux.NewRow
                Row2("Clave") = ClaveRetencion
                Row2("TipoEmisor") = 1
                Row2("Emisor") = Row("Clave")
                DtRetencionesExentasAux.Rows.Add(Row2)
            Next
            Dt = New DataTable
            If Not Tablas.Read("SELECT Clave FROM Clientes;", Conexion, Dt) Then Return False
            For Each Row As DataRow In Dt.Rows
                Dim Row2 As DataRow = DtRetencionesExentasAux.NewRow
                Row2("Clave") = ClaveRetencion
                Row2("TipoEmisor") = 2
                Row2("Emisor") = Row("Clave")
                DtRetencionesExentasAux.Rows.Add(Row2)
            Next
        End If

        If Funcion = "B" Then
            For Each Row As DataRow In DtRetencionesExentasAux.Rows
                Row.Delete()
            Next
        End If

        Return True

    End Function
    Private Sub HallaDocuAplicablesDeTerceros(ByRef dt As DataTable)

        dt = New DataTable
        Dim Row As DataRow

        Dim Sel As New DataColumn("Sel")
        Sel.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(Sel)

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(Nombre)

        Row = dt.NewRow
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 60
        Row("Nombre") = "Cobranza"
        dt.Rows.Add(Row)
        '         Row("Sel") = 0
        '         Row("Clave") = 600
        '         Row("Nombre") = "Orden de Pago"
        '           DtGrid.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 3
        Row("Nombre") = "Factura Proveedor"
        dt.Rows.Add(Row)
        '
        '        Row = DtGrid.NewRow
        '        Row("Sel") = 0
        '        Row("Clave") = 10
        '        Row("Nombre") = "Liquidacion a Proveedor"
        '        DtGrid.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 800
        Row("Nombre") = "N.V.L.P."
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 3000
        Row("Nombre") = "Gasto Bancario"
        dt.Rows.Add(Row)
        '
        '     Row = Dt.NewRow
        '     Row("Sel") = 0
        '     Row("Clave") = 5
        '     Row("Nombre") = "N.D. a Cliente"
        '     Dt.Rows.Add(Row)
        '
        '   Row = Dt.NewRow
        '   Row("Sel") = 0
        '   Row("Clave") = 7
        '   Row("Nombre") = "N.C. a Cliente"
        '   Dt.Rows.Add(Row)
        '
        '  Row = Dt.NewRow
        '  Row("Sel") = 0
        '  Row("Clave") = 6
        '  Row("Nombre") = "N.D. a Proveedor"
        '  Dt.Rows.Add(Row)
        '
        ' Row = Dt.NewRow
        ' Row("Sel") = 0
        ' Row("Clave") = 8
        ' Row("Nombre") = "N.C. a Proveedor"
        ' Dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 50
        Row("Nombre") = "N.D. del Cliente"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 70
        Row("Nombre") = "N.C. del Cliente"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 500
        Row("Nombre") = "N.D. del Proveedor"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 700
        Row("Nombre") = "N.C. del proveedor"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 604
        Row("Nombre") = "Devol. del proveedor"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 1010
        Row("Nombre") = "Cancelación Prestamo"
        dt.Rows.Add(Row)

    End Sub
    Private Sub HallaDocuAplicablesATerceros(ByRef dt As DataTable)

        dt = New DataTable

        Dim Sel As New DataColumn("Sel")
        Sel.DataType = System.Type.GetType("System.Boolean")
        dt.Columns.Add(Sel)

        Dim Clave As New DataColumn("Clave")
        Clave.DataType = System.Type.GetType("System.Int32")
        dt.Columns.Add(Clave)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        dt.Columns.Add(Nombre)

        Dim Row As DataRow = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 2
        Row("Nombre") = "Facturas Clientes"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 4
        Row("Nombre") = "N.C. por Devol."
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 5
        Row("Nombre") = "N.Debito. Finananciera"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 7
        Row("Nombre") = "N.Crédito. Finananciera"
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 10     'Para liqui.: 910,911,912 Liqui.Contable: 913  Liqui.Insumos: 915. 
        Row("Nombre") = "Liquidación a Proveedor."
        dt.Rows.Add(Row)
        '
        Row = dt.NewRow
        Row("Sel") = 0
        Row("Clave") = 600
        Row("Nombre") = "Orden de Pago"
        dt.Rows.Add(Row)

    End Sub
    Private Function FueUsada(ByVal Clave As Integer) As Boolean

        If RetencionFacturaProveedorUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionOrdenPagoUsado(Clave, Conexion) Then   'Orden pago,conbranza,notas credito/debito.
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionCobranzaUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionNVLPUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionLiquidacionProveedorUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionGastoBancarioUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If RetencionCancelacionPrestamosUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If PercepcionUsado(Clave, Conexion) Then
            MsgBox("Item esta siendo usado.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If

    End Function
    Private Function HallaTodosFormula(ByVal CodigoRetencion As Integer, ByVal Formula As Integer) As Integer

        Dim Dt As New DataTable
        Dim Clave As Integer

        If not Tablas.Read("SELECT Clave FROM Tablas WHERE CodigoRetencion = " & CodigoRetencion & " AND Formula = " & Formula & ";", Conexion, Dt) Then
            Return -1
        End If
        If Dt.Rows.Count = 0 Then
            Clave = 0
        Else
            Clave = Dt.Rows(0).Item("Clave")
        End If

        Dt.Dispose()
        Return Clave

    End Function
    Private Function Valida() As Boolean

        Dim Row As DataRow = Dt.Rows(0)

        If Row("Nombre") = "" Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextNombre.Focus()
            Return False
        End If

        If Row("Nombre") <> NombreW Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nombre) FROM Tablas WHERE Tipo = 25 AND Nombre = '" & TextNombre.Text & "';", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            MsgBox("Nombre Ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                            TextNombre.Focus()
                            Return False
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla.", MsgBoxStyle.Critical)
                Return False
            End Try
        End If

        If Row("CodigoRetencion") = 0 Then
            MsgBox("Falta Tipo.(Retención o Percepción)", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            ComboCodigoRetencion.Focus()
            Return False
        End If

        If Row("TipoPago") = 0 Then
            '      MsgBox("Debe Informar Tipo Impositivo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            '      ComboTipoImpositivo.Focus()
            '     Return False
        End If

        If ComboTipoImpositivo.SelectedValue <> TipoImpositivoAnt Then
            '    If HallaTipoImpositivoRepetido(Row("CodigoRetencion"), Row("TipoPago")) Then
            '        ComboTipoImpositivo.Focus()
            '        Return False
            '    End If
        End If

        If Row("OrigenPercepcion") = 0 Then
            MsgBox("Falta Informar Origen Percepción/Retención.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            ComboOrigenPercepcion.Focus()
            Return False
        End If

        For Each Row1 As DataRow In DtDocuAplicablesDeTerceros.Rows
            If Row1("Sel") Then
                If (Row1("Clave") <> 60 And Row1("Clave") <> 3000 And Row1("Clave") <> 1010 And Row1("Clave") <> 604 And Row1("Clave") <> 800) And Row("CodigoRetencion") = 1 Then
                    MsgBox("Tipo 'Retencion' de terceros no valida para comprobante " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Return False
                End If
                If (Row1("Clave") = 60 Or Row1("Clave") = 604) And Row("CodigoRetencion") = 2 Then
                    MsgBox("Tipo 'Percepcion' de terceros no valida para comprobante " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Return False
                End If
            End If
        Next

        Dim HayComprobantesATercerosDefinidos As Boolean = False

        For Each Row1 As DataRow In DtDocuAplicablesATerceros.Rows
            If Row1("Sel") Then
                Select Case Row1("Clave")
                    Case 2, 4, 5, 7   'Facturas a clientes,N.Credito Dev.,N.Cred Finan., N.Deb Finan.
                        Select Case Row("Formula")
                            Case 4
                                If Row("CodigoRetencion") = 1 Then
                                    MsgBox("Nro. Formula " & Row("Formula") & " No Definida para " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                    TextFormula.Focus()
                                    Return False
                                End If
                            Case Else
                                MsgBox("Nro. Formula " & Row("Formula") & " No Definida para " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                TextFormula.Focus()
                                Return False
                        End Select
                    Case 10
                        Select Case Row("Formula")
                            Case 0, 1, 2
                            Case Else
                                MsgBox("Nro. Formula " & Row("Formula") & " No Definida para " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                TextFormula.Focus()
                                Return False
                        End Select
                    Case 600
                        If Row("CodigoRetencion") = 2 Then
                            MsgBox("Orden de Pago no debe ser informada para una Percepción.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                            Return False
                        End If
                        Select Case Row("Formula")
                            Case 0, 1, 4
                            Case Else
                                MsgBox("Nro. Formula " & Row("Formula") & " No Definida para " & Row1("Nombre"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                                TextFormula.Focus()
                                Return False
                        End Select
                End Select
                HayComprobantesATercerosDefinidos = True
            End If
        Next

        If HayComprobantesATercerosDefinidos Then
            If Row("CodigoRetencion") = 2 And ComboCodigoAfipElectronico.SelectedValue = 0 Then
                MsgBox("Falta Definir Código Afip para comp. Electrónicos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                ComboCodigoAfipElectronico.Focus()
                Return False
            End If
        End If

        Select Case Row("Formula")
            Case 1
                If HayComprobantesATercerosDefinidos Then
                    If Row("TopeMes") = 0 Then
                        MsgBox("Falta Tope del Mes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        TextTopeMes.Focus()
                        Return False
                    End If
                    If Row("AlicuotaRetencion") = 0 Then
                        MsgBox("Falta Alicuota Retencion.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        TextAlicuota.Focus()
                        Return False
                    End If
                    If Row("AlicuotaRetencion") > 100 Then
                        MsgBox("Alicuota Retencion supera 100%.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        TextAlicuota.Focus()
                        Return False
                    End If
                End If
            Case 4
                If Row("Operador") = 0 Then
                    MsgBox("Debe Informar Jurisdiccion Ingreso Bruto Conv. Multilateral.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextJurisdiccion.Focus()
                    Return False
                End If
                If Row("CodigoProvinciaIIBB") = "" Then
                    MsgBox("Debe Informar Jurisdicción Provincia Ingreso Bruto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextCodigoProvinciaIngresoBruto.Focus()
                    Return False
                End If
            Case 0, 2, 4, 5
                If Row("TopeMes") <> 0 Then
                    MsgBox("Tope del Mes NO debe Informarse.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextTopeMes.Focus()
                    Return False
                End If
                If Row("AlicuotaRetencion") <> 0 Then
                    MsgBox("Alicuota Retencion NO debe Informarse.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextAlicuota.Focus()
                    Return False
                End If
            Case Else
                MsgBox("Formula NO Definida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                TextFormula.Focus()
                Return False
        End Select

        If GGeneraAsiento Then
            If Row("Cuenta") = 0 Then
                MsgBox("Falta Cuenta Debito Fiscal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                MaskedCuentaDebito.Focus()
                Return False
            End If
            If Row("Cuenta2") = 0 Then
                MsgBox("Falta Cuenta Crédito Fiscal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                MaskedCuentaCredito.Focus()
                Return False
            End If
            If Row("Cuenta") = Row("Cuenta2") Then
                MsgBox("Cuenta Debito Fiscal no debe ser igual a Crédito Fiscal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                MaskedCuentaCredito.Focus()
                Return False
            End If
        End If

        If PClaveRetencion <> 0 Then
            If CodigoRetencionAnt <> Row("CodigoRetencion") Then
                If FueUsada(Row("Clave")) Then
                    MsgBox("Tipo no se puede modificar. Hay Retenciones o Percepciones efectuadas. ", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    ComboCodigoRetencion.Focus()
                    Return False
                End If
            End If
            If OrigenPercepcionAnt <> Row("OrigenPercepcion") And OrigenPercepcionAnt <> 0 Then
                If FueUsada(Row("Clave")) Then
                    If MsgBox("Hay Retenciones o Percepciones efectuadas. Desea Continuar? (Y/N) ", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3, "Error") = MsgBoxResult.No Then
                        ComboOrigenPercepcion.Focus()
                        Return False
                    End If
                End If
            End If
        End If

        If Row("Formula") = 4 Then
            Dim Clave As Integer = HallaTodosFormula(ComboCodigoRetencion.SelectedValue, 4)
            If Clave < 0 Then
                MsgBox("Error al leer Tbala: Tablas.", MsgBoxStyle.Critical)
                Return False
            End If
            If PClaveRetencion = 0 And Clave <> 0 Then
                MsgBox("Ya Existe una " & ComboCodigoRetencion.Text & " Formula 4.", MsgBoxStyle.Critical)
                Return False
            End If
            If PClaveRetencion <> 0 And Clave <> 0 And Clave <> PClaveRetencion Then
                MsgBox("Ya Existe una " & ComboCodigoRetencion.Text & " Formula 4.", MsgBoxStyle.Critical)
                Return False
            End If
        End If

        'Nuevas validaciones sobre Ingreso Bruto
        If Row("TipoPago") = 1 Then                   'Impuesto Ingreso Bruto.
            If Row("CodigoRetencion") = 1 Or Row("CodigoRetencion") = 2 Then
                If Not Row("EsPorProvincia") Then
                    MsgBox("Percepción/Retención por Ingreso Bruto debe Distribuir por Provincia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    CheckEsPorProvincia.Focus()
                    Return False
                End If
            End If
            If Row("CodigoRetencion") = 1 Then
                If Row("Formula") <> 2 Then
                    MsgBox("Retención por Ingreso Bruto debe informar Nro. Formula = 2.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    TextFormula.Focus()
                    Return False
                End If
            End If
        End If

        Return True

    End Function
    Private Function HallaTipoImpositivoRepetido(ByVal CodigoRetencion As Integer, ByVal TipoImpositivo As Integer) As Boolean

        Dim Dt As New DataTable
        Dim Clave As Integer

        If Not Tablas.Read("SELECT Clave FROM Tablas WHERE CodigoRetencion = " & CodigoRetencion & " AND TipoPago = " & TipoImpositivo & ";", Conexion, Dt) Then
            Clave = -1
        End If
        If Dt.Rows.Count = 0 Then
            Clave = 0
        Else
            Clave = Dt.Rows(0).Item("Clave")
        End If

        Dt.Dispose()

        If Clave = -1 Then
            MsgBox("Error al leer Tablas.", MsgBoxStyle.Critical)
            Return True
        End If
        If Clave <> 0 Then
            MsgBox("Ya Existe Retención o Percepción con este Tipo Impositivo.", MsgBoxStyle.Critical)
            Return True
        End If

        Return False

    End Function


End Class