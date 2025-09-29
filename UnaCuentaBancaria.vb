Public Class UnaCuentaBancaria
    Public PBanco As Integer
    Public PSucursal As Integer
    Public PCuenta As Decimal
    Public PEsModificarSucursal As Boolean
    Public PEsAltaSucursal As Boolean
    Public PEsModificarCuenta As Boolean
    Public PEsAltaCuenta As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Dt As DataTable
    Dim DtCuenta As DataTable
    Dim NombreSucursalAnt As String
    Private Sub UnaCuentaBancaria_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        LlenaComboTablas(ComboBancos, 26)
        ComboBancos.SelectedValue = PBanco

        LlenaComboTablas(ComboMoneda, 27)
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "Pesos"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)

        ComboTipo.Items.Clear()
        ComboTipo.Items.Add("")
        ComboTipo.Items.Add("CA")
        ComboTipo.Items.Add("CC")

        Dim RowsBusqueda() As DataRow

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM CuentasBancarias WHERE Banco = " & PBanco & ";", Conexion, Dt) Then Me.Close() : Exit Sub

        If PEsModificarSucursal Then
            PanelSucursal.Visible = True
            If PSucursal <> 0 Then
                RowsBusqueda = Dt.Select("Sucursal = " & PSucursal)
                TextSucursal.Text = PSucursal
                TextNombreSucursal.Text = RowsBusqueda(0).Item("NombreSucursal")
                NombreSucursalAnt = RowsBusqueda(0).Item("NombreSucursal")
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PEsModificarCuenta Then
            PanelSucursal.Enabled = False
            TextNumero.ReadOnly = True
            If Not MuestraDatosCuenta() Then Me.Close() : Exit Sub
        End If
        If PEsAltaCuenta Then
            PanelSucursal.Enabled = False
            If Not MuestraDatosCuenta() Then Me.Close() : Exit Sub
        End If
        If PEsModificarSucursal Then
            Panel2.Visible = False
            TextSucursal.ReadOnly = True
            MuestraDatosSucursal()
        End If
        If PEsAltaSucursal Then
            MuestraDatosCuenta()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        MiEnlazador.EndEdit()

        If PEsModificarCuenta Or PEsAltaCuenta Then
            If Not ValidaCuenta() Then Exit Sub
            If PEsAltaCuenta Then PCuenta = TextNumero.Text
            If GrabaArchivo(DtCuenta) Then Me.Close() : Exit Sub
        End If
        If PEsAltaSucursal Then
            If Not ValidaSucursal() Then Exit Sub
            If Not ValidaCuenta() Then Exit Sub
            PSucursal = TextSucursal.Text
            PCuenta = TextNumero.Text
            If GrabaArchivo(DtCuenta) Then Me.Close() : Exit Sub
        End If
        If PEsModificarSucursal Then
            If TextNombreSucursal.Text.Trim <> NombreSucursalAnt Then
                If Not ValidaSucursal() Then Exit Sub
                Dim DtAux As DataTable = Dt.Copy
                For Each Row As DataRow In DtAux.Rows
                    If Row("Sucursal") = PSucursal Then
                        Row("NombreSucursal") = TextNombreSucursal.Text.Trim
                    End If
                Next
                If GrabaArchivo(DtAux) Then Me.Close() : Exit Sub
            End If
        End If

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If Not PEsModificarCuenta Then
            MsgBox("Opcion Invalida. Se Debe Borrar las Cuentas. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If CuentaUsada(ComboBancos.SelectedValue, CDec(TextNumero.Text), Conexion) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If CuentaUsada(ComboBancos.SelectedValue, CDec(TextNumero.Text), ConexionN) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Cuenta se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtCuentaAux As DataTable
        DtCuentaAux = DtCuenta.Copy

        DtCuentaAux.Rows(0).Delete()
        If GrabaArchivo(DtCuentaAux) Then Me.Close()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub CheckTieneChequera_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckTieneChequera.Click

        If CheckTieneChequera.Checked Then
            Panel3.Visible = True
        Else : Panel3.Visible = False
        End If

    End Sub
    Private Sub TextNombreSucursal_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextNombreSucursal.Validating

        TextNombreSucursal.Text = TextNombreSucursal.Text.Trim

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub TextSaldoInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicial.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicial.Text, 2)

    End Sub
    Private Sub TextNumeracionInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeracionInicial.KeyPress

        EsNumerico(e.KeyChar, TextNumeracionInicial.Text, 0)

    End Sub
    Private Sub TextNumeracionFinal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumeracionFinal.KeyPress

        EsNumerico(e.KeyChar, TextNumeracionFinal.Text, 0)

    End Sub
    Private Sub TextCbu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCbu.KeyPress

        EsNumerico(e.KeyChar, TextCbu.Text, 0)

    End Sub
    Private Sub TextSucursal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSucursal.KeyPress

        EsNumerico(e.KeyChar, TextSucursal.Text, 0)

    End Sub
    Private Sub TextUltimoNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextUltimoNumero.KeyPress

        EsNumerico(e.KeyChar, TextUltimoNumero.Text, 0)

    End Sub
    Private Function MuestraDatosCuenta() As Boolean                    'MuestraDatosCuenta

        DtCuenta = New DataTable

        Dim Sql As String = "SELECT * FROM CuentasBancarias WHERE Banco = " & PBanco & " AND Numero = " & PCuenta & ";"

        If Not Tablas.Read(Sql, Conexion, DtCuenta) Then Return False

        If PCuenta <> 0 And DtCuenta.Rows.Count = 0 Then
            MsgBox("Cuenta No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim NombreSucursal As String
        If PEsAltaCuenta Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("Sucursal = " & PSucursal)
            NombreSucursal = RowsBusqueda(0).Item("NombreSucursal")
        End If

        If PEsAltaCuenta Or PEsAltaSucursal Then
            Dim Row As DataRow = DtCuenta.NewRow
            Row("Banco") = PBanco
            Row("Sucursal") = PSucursal
            Row("NombreSucursal") = NombreSucursal
            Row("TipoCuenta") = 0
            Row("Numero") = 0
            Row("TieneChequera") = False
            Row("LiquidaDivisa") = False
            Row("UltimaSerie") = ""
            Row("UltimoNumero") = 0
            Row("Cbu") = ""
            Row("SaldoInicial") = 0
            Row("NumeracionInicial") = 0
            Row("NumeracionFinal") = 0
            Row("Moneda") = 1
            DtCuenta.Rows.Add(Row)
        End If

        EnlazaCabeza()

        Return True

    End Function
    Private Sub MuestraDatosSucursal()

        Dim NombreSucursal As String
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = Dt.Select("Sucursal = " & PSucursal)
        NombreSucursal = RowsBusqueda(0).Item("NombreSucursal")

        TextSucursal.Text = PSucursal
        TextNombreSucursal.Text = NombreSucursal

    End Sub
    Private Sub EnlazaCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCuenta

        Dim Row As DataRowView = MiEnlazador.Current

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Sucursal")
        TextSucursal.DataBindings.Clear()
        TextSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NombreSucursal")
        TextNombreSucursal.DataBindings.Clear()
        TextNombreSucursal.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Numero")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedIndex", MiEnlazador, "TipoCuenta")
        ComboTipo.DataBindings.Clear()
        ComboTipo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "SaldoInicial")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextSaldoInicial.DataBindings.Clear()
        TextSaldoInicial.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cbu")
        TextCbu.DataBindings.Clear()
        TextCbu.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "TieneChequera")
        CheckTieneChequera.DataBindings.Clear()
        CheckTieneChequera.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "LiquidaDivisa")
        CheckLiquidaDivisa.DataBindings.Clear()
        CheckLiquidaDivisa.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "UltimaSerie")
        TextUltimaSerie.DataBindings.Clear()
        TextUltimaSerie.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "UltimoNumero")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextUltimoNumero.DataBindings.Clear()
        TextUltimoNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NumeracionInicial")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextNumeracionInicial.DataBindings.Clear()
        TextNumeracionInicial.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NumeracionFinal")
        AddHandler Enlace.Format, AddressOf FormatEntero
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextNumeracionFinal.DataBindings.Clear()
        TextNumeracionFinal.DataBindings.Add(Enlace)

        If Row("TieneChequera") Then Panel3.Visible = True

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        End If

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
    Private Function ExisteNombre(ByVal Banco As Integer, ByVal Nombre As String) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Banco = " & Banco & " AND NombreSucursal = '" & Nombre & "'")
        If RowsBusqueda.Length <> 0 Then Return True

        Return False

    End Function
    Private Function ExisteSucursal(ByVal Banco As Integer, ByVal Codigo As Integer) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Banco = " & Banco & " AND Sucursal = " & Codigo)
        If RowsBusqueda.Length <> 0 Then Return True

        Return False

    End Function
    Private Function ExisteCuenta(ByVal Banco As Integer, ByVal Numero As Decimal) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Banco = " & Banco & " AND Numero = " & Numero)
        If RowsBusqueda.Length <> 0 Then Return True

        Return False

    End Function
    Private Function CuentaFueUsada(ByVal Banco As Integer, ByVal Numero As Double) As Boolean

        If GUsaNegra And Not PermisoTotal Then
            Return True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If CuentaUsada(Banco, Numero, Conexion) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If
        If CuentaUsada(Banco, Numero, ConexionN) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return True
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return False

    End Function
    Private Function CuentaUsada(ByVal Banco As Integer, ByVal Clave As Double, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM OtrosPagosPago WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM PrestamosMovimientoPago WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM RecibosDetallePago WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM SueldosMovimientoPago WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM MovimientosBancarioCabeza WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM CompraDivisasPago WHERE Banco = " & Banco & " AND Cuenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function GrabaArchivo(ByVal DtCuentaAux As DataTable) As Boolean

        If IsNothing(DtCuentaAux.GetChanges) Then Exit Function

        If PEsAltaCuenta Or PEsAltaSucursal Then
            If EstaRepetida(PBanco, DtCuentaAux.Rows(0).Item("Numero")) <> 0 Then
                MsgBox("Error, Otro Usuario dio de Alta esta Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Dim Sql As String
                Sql = "SELECT * FROM CuentasBancarias;"
                Using DaP As New OleDb.OleDbDataAdapter(Sql, Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtCuentaAux.GetChanges)
                End Using
                MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                Return True
            End Using
        Catch ex As OleDb.OleDbException
            MsgBox("Error de Base de datos. Algunos cambios no se pudieron realizar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Catch ex As DBConcurrencyException
            MsgBox("Error,Otro Usuario modifico Datos. Algunos cambios no se pudieron realizar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Finally
        End Try

    End Function
    Private Function EstaRepetida(ByVal Banco As Integer, ByVal Sucursal As Decimal) As Integer

        Dim Sql As String = "SELECT COUNT(Numero) FROM CuentasBancarias WHERE Banco = " & Banco & " AND Numero = " & Sucursal & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            Return -1000
        End Try

    End Function
    Private Function ValidaSucursal() As Boolean

        If TextSucursal.Text = "" Then
            MsgBox("Falta Codigo Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextSucursal.Focus()
            Return False
        End If

        If CDbl(TextSucursal.Text) = 0 Then
            MsgBox("Erroneo Codigo Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextSucursal.Focus()
            Return False
        End If

        If PEsAltaSucursal Then
            If ExisteSucursal(ComboBancos.SelectedValue, CInt(TextSucursal.Text)) Then
                MsgBox("Codigo Sucursal ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextSucursal.Focus()
                Return False
            End If
        End If

        If TextNombreSucursal.Text.Trim = "" Then
            MsgBox("Falta Nombre Sucursal.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNombreSucursal.Focus()
            Return False
        End If

        If NombreSucursalAnt <> TextNombreSucursal.Text.Trim Then
            If ExisteNombre(ComboBancos.SelectedValue, TextNombreSucursal.Text.Trim) Then
                MsgBox("Nombre Sucursal ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNombreSucursal.Focus()
                Return False
            End If
        End If

        Return True

    End Function
    Private Function ValidaCuenta() As Boolean

        If ComboBancos.SelectedValue = 0 Then
            MsgBox("Falta Banco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboBancos.Focus()
            Return False
        End If

        If TextNumero.Text = "" Then
            MsgBox("Falta Numero de cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumero.Focus()
            Return False
        End If

        If CDec(TextNumero.Text) = 0 Then
            MsgBox("Falta Numero de cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNumero.Focus()
            Return False
        End If

        If PEsAltaCuenta Or PEsAltaSucursal Then
            If ExisteCuenta(ComboBancos.SelectedValue, CDec(TextNumero.Text)) Then
                MsgBox("Numero de cuenta ya existe en este Banco.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumero.Focus()
                Return False
            End If
        End If

        If ComboTipo.SelectedIndex = 0 Then
            MsgBox("Falta Tipo Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipo.Focus()
            Return False
        End If

        If CheckTieneChequera.Checked And ComboTipo.SelectedIndex = 1 Then
            MsgBox("Chequera no corresponde con Caja de Ahorro.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            CheckTieneChequera.Focus()
            Return False
        End If

        If CheckTieneChequera.Checked Then
            If TextUltimaSerie.Text = "" Then
                MsgBox("Falta Ultima Serie Cheque.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextUltimaSerie.Focus()
                Return False
            End If
            If TextNumeracionInicial.Text = "" Then
                MsgBox("Falta Numeración Inicial de Chequera.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionInicial.Focus()
                Return False
            End If
            If TextNumeracionFinal.Text = "" Then
                MsgBox("Falta Numeración Final de Chequera.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionFinal.Focus()
                Return False
            End If
            If Not IsNumeric(TextNumeracionInicial.Text) Then
                MsgBox("Numeración Inicial de Chequera No Numerica.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionInicial.Focus()
                Return False
            End If
            If Not IsNumeric(TextNumeracionFinal.Text) Then
                MsgBox("Numeración Final de Chequera No Numerica.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionFinal.Focus()
                Return False
            End If
            If CInt(TextNumeracionInicial.Text) = 0 Or CInt(TextNumeracionFinal.Text) = 0 Then
                MsgBox("Numeración de Chequera NO Debe ser cero.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionInicial.Focus()
                Return False
            End If
            If CInt(TextNumeracionInicial.Text) > CInt(TextNumeracionFinal.Text) Then
                MsgBox("Numeración de Chequera Inicial mayor a Final.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextNumeracionInicial.Focus()
                Return False
            End If
        End If

        If ComboMoneda.SelectedValue = 0 Then
            MsgBox("Falta informar Moneda.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboMoneda.Focus()
            Return False
        End If
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = Dt.Select("Banco = " & ComboBancos.SelectedValue & " AND Sucursal = " & CInt(TextSucursal.Text) & " AND Numero = " & CDec(TextNumero.Text))
        If RowsBusqueda.Length <> 0 Then
            If RowsBusqueda(0).Item("Moneda") <> ComboMoneda.SelectedValue Then
                If GUsaNegra And Not PermisoTotal Then
                    MsgBox("ERROR Usuario no esta Autorizado a Cambiar Moneda(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If CuentaFueUsada(ComboBancos.SelectedValue, CDec(TextNumero.Text)) Then
                    MsgBox("Moneda No se puede Cambiar, Cuenta Ya Fue Usada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        End If

        If CheckLiquidaDivisa.Checked Then
            If ComboMoneda.SelectedValue = 1 Then
                MsgBox("Liquida Divisa Valido solo para Moneda Extranjera.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                CheckLiquidaDivisa.Focus()
                Return False
            End If
        End If

        Return True

    End Function



   
End Class