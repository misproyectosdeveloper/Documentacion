Public Class UnCosteo
    Public PCosteo As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCosteo As DataTable
    Dim TablaIva(0) As Double
    Dim IntFechaDesdeAnt As Integer
    Dim IntFechaHastaAnt As Integer
    Dim EspecieAnt As Integer
    Dim VariedadAnt As Integer
    Private Sub UnCosteo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ComboNegocio.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;")
        Dim Row As DataRow = ComboNegocio.DataSource.newrow
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

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        ArmaTablaIva(TablaIva)

        ArmaArchivos()

    End Sub
    Private Sub ComboNegocio_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboNegocio.Validating

        If IsNothing(ComboNegocio.SelectedValue) Then ComboNegocio.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub TextCosto_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCosto.KeyPress

        EsNumerico(e.KeyChar, TextCosto.Text, GDecimales)

    End Sub
    Private Sub TextCosto_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextCosto.Validating

        If Not IsNumeric(TextCosto.Text) Then
            MsgBox("Dato Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            TextCosto.Text = "0,00"
            TextCosto.Focus()
        End If

    End Sub
    Private Sub TextIva_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextIva.Validating

        If Not IsNumeric(TextIva.Text) Then
            MsgBox("Iva debe ser Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If
        If CDbl(TextIva.Text) > 100 Then
            MsgBox("Iva Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub TextIva_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextIva.KeyPress

        EsNumerico(e.KeyChar, TextIva.Text, GDecimales)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If DtCosteo.Rows(0).Item("IntFechaDesde") <> Format(DateTimeDesde.Value, "yyyyMMdd") Then DtCosteo.Rows(0).Item("IntFechaDesde") = Format(DateTimeDesde.Value, "yyyyMMdd")
        If DtCosteo.Rows(0).Item("IntFechaHasta") <> Format(DateTimeHasta.Value, "yyyyMMdd") Then DtCosteo.Rows(0).Item("IntFechaHasta") = Format(DateTimeHasta.Value, "yyyyMMdd")

        If IsNothing(DtCosteo.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        If PCosteo = 0 Then
            Dim Numero As Integer = UltimoNumero(Conexion)
            If Numero < 0 Then
                MsgBox("Error de Base de Datos.")
                Me.Close() : Exit Sub
            End If
            DtCosteo.Rows(0).Item("Costeo") = Numero + 1
        End If

        GModificacionOk = False

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GrabaArchivo()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal Then
            MsgBox("Error de Base de Datos o Usuario NO Autorizado(1000). Operación se CANCELA.")
            Exit Sub
        End If

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PCosteo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("No se puede Actualizar(1000).", MsgBoxStyle.Critical)
            Exit Sub
        End If

        Dim Resul As Integer
        Resul = TieneIngreso(PCosteo, Conexion)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Ingresos de Mercaderia. Operación se CANCELA.")
            Exit Sub
        End If
        Resul = TieneIngreso(PCosteo, ConexionN)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Ingresos de Mercaderia. Operación se CANCELA.")
            Exit Sub
        End If
        Resul = TieneFacturasProveedor(PCosteo, Conexion)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Facturas de Proveedores. Operación se CANCELA.")
            Exit Sub
        End If
        Resul = TieneFacturasProveedor(PCosteo, ConexionN)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Facturas de Proveedores. Operación se CANCELA.")
            Exit Sub
        End If
        Resul = TieneConsumos(PCosteo, Conexion)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Consumos. Operación se CANCELA.")
            Exit Sub
        End If
        Resul = TieneConsumos(PCosteo, ConexionN)
        If Resul < 0 Then Exit Sub
        If Resul > 0 Then
            MsgBox("Costeo tiene Consumos. Operación se CANCELA.")
            Exit Sub
        End If

        If MsgBox("Costeo se Borrara del Sistema. ¿Desea Borrarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Dim DtCosteoW As DataTable = DtCosteo.Copy
        DtCosteoW.Rows(0).Delete()

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Costeos;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCosteoW.GetChanges)
                    End Using
                    GModificacionOk = True
                    MsgBox("Costeo Borrado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                    Me.Close() : Exit Sub
                Catch ex As OleDb.OleDbException
                    MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch ex As DBConcurrencyException
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        DtCosteoW.Dispose()

    End Sub
    Private Sub ArmaArchivos()

        DtCosteo = New DataTable
        If Not Tablas.Read("SELECT * FROM Costeos WHERE Costeo = " & PCosteo & ";", Conexion, DtCosteo) Then Me.Close() : Exit Sub
        If PCosteo <> 0 And DtCosteo.Rows.Count = 0 Then
            MsgBox("Costeo No Existen En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If DtCosteo.Rows.Count = 0 Then
            Dim Row As DataRow = DtCosteo.NewRow
            Row("Negocio") = 0
            Row("Costeo") = 0
            Row("Nombre") = ""
            Row("IntFechaDesde") = 0
            Row("IntFechahasta") = 0
            Row("Especie") = 0
            Row("Variedad") = 0
            Row("Costo") = 0
            Row("Iva") = 0
            Row("Cerrado") = False
            DtCosteo.Rows.Add(Row)
        End If

        MuestraCabeza()

        IntFechaDesdeAnt = DtCosteo.Rows(0).Item("IntFechaDesde")
        IntFechaHastaAnt = DtCosteo.Rows(0).Item("IntFechaHasta")
        EspecieAnt = DtCosteo.Rows(0).Item("Especie")
        VariedadAnt = DtCosteo.Rows(0).Item("Variedad")

        Dim Fecha As Integer = DtCosteo.Rows(0).Item("IntFechaDesde")
        If Fecha <> 0 Then
            DateTimeDesde.Value = Fecha.ToString.Substring(6, 2) & "/" & Fecha.ToString.Substring(4, 2) & "/" & Fecha.ToString.Substring(0, 4)
        Else
            DateTimeDesde.Value = Date.Now
        End If
        Fecha = DtCosteo.Rows(0).Item("IntFechaHasta")
        If Fecha <> 0 Then
            DateTimeHasta.Value = Fecha.ToString.Substring(6, 2) & "/" & Fecha.ToString.Substring(4, 2) & "/" & Fecha.ToString.Substring(0, 4)
        Else
            DateTimeHasta.Value = Date.Now
        End If

        If PCosteo = 0 Then
            ComboNegocio.Enabled = True
            TextNombre.ReadOnly = False
        Else
            ComboNegocio.Enabled = False
            TextNombre.ReadOnly = True
        End If

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCosteo

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazador, "Negocio")
        ComboNegocio.DataBindings.Clear()
        ComboNegocio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Costeo")
        TextCosteo.DataBindings.Clear()
        TextCosteo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Especie")
        ComboEspecie.DataBindings.Clear()
        ComboEspecie.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Variedad")
        ComboVariedad.DataBindings.Clear()
        ComboVariedad.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Costo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCosto.DataBindings.Clear()
        TextCosto.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Iva")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextIva.DataBindings.Clear()
        TextIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "Cerrado")
        CheckCerrado.DataBindings.Clear()
        CheckCerrado.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value <> 0 Then
            Numero.Value = Numero.Value.ToString.Substring(6, 2) & "/" & Numero.Value.ToString.Substring(4, 2) & "/" & Numero.Value.ToString.Substring(0, 4)
        Else : Numero.Value = Date.Now
        End If

    End Sub
    Private Sub ParseFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Strings.Right(Numero.Value, 4) & Strings.Mid(Numero.Value, 4, 2) & Strings.Left(Numero.Value, 2)

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0
        If IsNumeric(Numero.Value) Then Numero.Value = Trunca(Numero.Value)

    End Sub
    Private Function GrabaArchivo()

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Costeos;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtCosteo.GetChanges)
                    End Using
                    GModificacionOk = True
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    PCosteo = DtCosteo.Rows(0).Item("Costeo")
                    ArmaArchivos()
                Catch ex As OleDb.OleDbException
                    If ex.ErrorCode = GAltaExiste Then
                        MsgBox("Nombre Costeo ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Else
                        MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    End If
                Catch ex As DBConcurrencyException
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

    End Function
    Private Function TieneIngreso(ByVal Costeo As Integer, ByVal ConexionStr As String) As Integer
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM IngresoMercaderiasCabeza WHERE Costeo = " & Costeo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo)
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function TieneFacturasProveedor(ByVal Costeo As Integer, ByVal ConexionStr As String) As Integer
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM FacturasProveedorCabeza WHERE Costeo = " & Costeo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo)
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function TieneConsumos(ByVal Costeo As Integer, ByVal ConexionStr As String) As Integer
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Consumo) FROM ConsumosCabeza WHERE Costeo = " & Costeo & ";", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo)
                    Else : Return -1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimoNumero(ByVal ConexionStr) As Integer
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Costeo) FROM Costeos;", Miconexion)
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
    Private Function Valida() As Boolean

        If ComboNegocio.SelectedValue = 0 Then
            MsgBox("Falta Informar Negocio.")
            Return False
        End If

        If TextNombre.Text = "" Then
            MsgBox("Falta Informar Nombre Costeo.")
            Return False
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Final Menor a Fecha Inicial.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If ComboVariedad.SelectedValue <> 0 And ComboEspecie.SelectedValue = 0 Then
            MsgBox("Falta Informar Especie.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextIva.Text) = 0 Then
            MsgBox("Falta Informar IVA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim Esta As Boolean
        For Each Item As Double In TablaIva
            If Item = CDbl(TextIva.Text) Then Esta = True : Exit For
        Next
        If Esta = False Then
            MsgBox("Alicuota no Existe en el Sistema.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextCosto.Text) = 0 Then
            MsgBox("Costo Estimado debe ser Distinto de 0.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PCosteo <> 0 And (DtCosteo.Rows(0).Item("IntFechaDesde") <> IntFechaDesdeAnt Or DtCosteo.Rows(0).Item("IntFechaHasta") <> IntFechaHastaAnt Or _
            DtCosteo.Rows(0).Item("Especie") <> EspecieAnt Or DtCosteo.Rows(0).Item("Variedad") <> VariedadAnt) Then
            If Not ValidaAnteriores() Then Return False
        End If

        Return True

    End Function
    Private Function ValidaAnteriores() As Boolean

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("No se Puede Actualizar(1000).", MsgBoxStyle.Critical)
            Return False
        End If

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT C.Fecha,L.Articulo FROM IngresoMercaderiasCabeza AS C INNER JOIN Lotes AS L ON C.Lote = L.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND C.Costeo = " & PCosteo & ";", Conexion, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Not CosteoOk(Row("Articulo"), ComboEspecie.SelectedValue, ComboVariedad.SelectedValue, DateTimeDesde.Value, DateTimeHasta.Value, Row("Fecha")) Then
                MsgBox("Existe Ingresos de Mercaderias que no Coincide con la Nueva Definicion del Costeo.", MsgBoxStyle.Critical)
                Dt.Dispose()
                Return False
            End If
        Next

        Dt = New DataTable
        If Not Tablas.Read("SELECT C.Fecha,L.Articulo FROM IngresoMercaderiasCabeza AS C INNER JOIN Lotes AS L ON C.Lote = L.Lote WHERE L.Lote = L.LoteOrigen AND L.Secuencia = L.SecuenciaOrigen AND L.Deposito = L.DepositoOrigen AND C.Costeo = " & PCosteo & ";", ConexionN, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Not CosteoOk(Row("Articulo"), ComboEspecie.SelectedValue, ComboVariedad.SelectedValue, DateTimeDesde.Value, DateTimeHasta.Value, Row("Fecha")) Then
                MsgBox("Existe Ingresos de Mercaderias que no Coincide con la Nueva Definicion del Costeo.", MsgBoxStyle.Critical)
                Dt.Dispose()
                Return False
            End If
        Next

        Dt.Dispose()

        Return True

    End Function
    Private Function CosteoOk(ByVal Articulo As Integer, ByVal Especie As Integer, ByVal Variedad As Integer, ByVal FechaDesde As Date, ByVal FechaHasta As Date, ByVal FechaIngreso As Date) As Boolean

        Dim Dt As New DataTable

        If Not (Format(FechaIngreso, "yyyyMMdd") >= Format(FechaDesde, "yyyyMMdd") And Format(FechaIngreso, "yyyyMMdd") <= Format(FechaHasta, "yyyyMMdd")) Then
            Return False
        End If

        If Not Tablas.Read("SELECT Especie,Variedad FROM Articulos Where Clave = " & Articulo & ";", Conexion, Dt) Then Return False

        If Especie <> 0 Then
            If Dt.Rows(0).Item("Especie") <> Especie Then
                Dt.Dispose()
                Return False
            End If
        End If

        If Variedad <> 0 Then
            If Dt.Rows(0).Item("Variedad") <> Variedad Then
                Dt.Dispose()
                Return False
            End If
        End If

        Dt.Dispose()

        Return True

    End Function
End Class