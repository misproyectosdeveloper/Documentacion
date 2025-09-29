Public Class UnArticulo
    Public PEsServicios As Boolean
    Public PEsSecos As Boolean
    Public PClave As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim EsAlta As Boolean
    Dim TablaIva(0) As Double
    Private Sub UnArticulo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(3) Then PBloqueaFunciones = True

        If PEsServicios Or PEsSecos Then
            Panel1.Visible = False
            Panel2.Visible = True
            TextEAN.ReadOnly = True
            If PClave = 0 Then EsAlta = True
        Else
            Panel2.Visible = False
            LlenaComboTablas(ComboEspecie, 1)
            With ComboEspecie
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboEspecie.SelectedValue = GEspecie

            LlenaComboTablas(ComboVariedad, 2)
            With ComboVariedad
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboVariedad.SelectedValue = GVariedad

            LlenaComboTablas(ComboMarca, 3)
            With ComboMarca
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboMarca.SelectedValue = GMarca

            LlenaComboTablas(ComboCategoria, 4)
            With ComboCategoria
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboCategoria.SelectedValue = GCategoria

            LLenaComboEnvases(ComboEnvase)
            With ComboEnvase
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboEnvase.SelectedValue = GEnvase

            LLenaComboEnvases(ComboSecundario)
            With ComboSecundario
                .AutoCompleteMode = AutoCompleteMode.SuggestAppend
                .AutoCompleteSource = AutoCompleteSource.ListItems
            End With
            ComboSecundario.SelectedValue = 0

            If GEspecie = 0 Or GVariedad = 0 Or GMarca = 0 Or GCategoria = 0 Or GEnvase = 0 Then EsAlta = True

            If GEspecie <> 0 Then ComboEspecie.Enabled = False
            If GVariedad <> 0 Then ComboVariedad.Enabled = False
            If GMarca <> 0 Then ComboMarca.Enabled = False
            If GCategoria <> 0 Then ComboCategoria.Enabled = False
            If GEnvase <> 0 Then ComboEnvase.Enabled = False
        End If

        ComboEstado.DataSource = DtActivoDeshabilitado()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Codigo"

        ArmaTablaIva(TablaIva)

        GModificacionOk = False

        If PEsServicios Or PEsSecos Then
            If Not MuestraArticuloServiciosYSecos() Then Me.Close()
        Else
            If Not MuestraArticuloConStock() Then Me.Close()
        End If

    End Sub
    Private Sub UnArticulo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        If IsNothing(Dt.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ActualizaArticulo() Then
            Me.Close()
            Exit Sub
        End If

        If PEsServicios Or PEsSecos Then
            If Not MuestraArticuloServiciosYSecos() Then Me.Close()
        Else
            If Not MuestraArticuloConStock() Then Me.Close()
        End If

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsAlta Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not (PEsServicios Or PEsSecos) Then
            If UsadoConStock(Dt.Rows(0).Item("Clave"), Conexion) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If UsadoConStock(Dt.Rows(0).Item("Clave"), ConexionN) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If PEsServicios Then
            If UsadoServicios(Dt.Rows(0).Item("Clave"), Conexion) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If UsadoServicios(Dt.Rows(0).Item("Clave"), ConexionN) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If PEsSecos Then
            If UsadoSecos(Dt.Rows(0).Item("Clave"), Conexion) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If UsadoSecos(Dt.Rows(0).Item("Clave"), ConexionN) Then
                MsgBox("El Articulo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If MsgBox("El articulo se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        MiEnlazador.Remove(MiEnlazador.Current)

        If ActualizaArticulo() Then
            Me.Close()
        Else
            If PEsServicios Or PEsSecos Then
                If Not MuestraArticuloServiciosYSecos() Then Me.Close()
            Else
                If Not MuestraArticuloConStock() Then Me.Close()
            End If
        End If

    End Sub
    Private Sub ButtonDeshabilitar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDeshabilitar.Click

        If EsAlta Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Articulo ya esta Deshabilitado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PEsServicios And Not PEsSecos Then
            If ArticuloTieneStock() Then Exit Sub
        End If

        Dt.Rows(0).Item("Estado") = 3

        ActualizaArticulo()

        If PEsServicios Or PEsSecos Then
            If Not MuestraArticuloServiciosYSecos() Then Me.Close()
        Else
            If Not MuestraArticuloConStock() Then Me.Close()
        End If

    End Sub
    Private Sub ButtonActiva_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActiva.Click

        If EsAlta Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 1 Then
            MsgBox("Articulo ya esta Activo. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dt.Rows(0).Item("Estado") = 1

        ActualizaArticulo()

        If PEsServicios Or PEsSecos Then
            If Not MuestraArticuloServiciosYSecos() Then Me.Close()
        Else
            If Not MuestraArticuloConStock() Then Me.Close()
        End If

    End Sub
    Private Sub PictureCuentaContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureCuentaContable.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            TextCuentaContable.Text = Format(SeleccionarCuenta.PCuenta, "000-000000-00")
        End If
        SeleccionarCuenta.Dispose()

    End Sub
    Private Sub ComboEspecie_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboEspecie.KeyDown

        If e.KeyData = 13 Then ComboEspecie_Validating(Nothing, Nothing) : Exit Sub

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0
        If ComboEspecie.SelectedValue <> 0 And EsAlta Then IvaEspecie()

    End Sub
    Private Sub ComboVariedad_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboVariedad.KeyDown

        If e.KeyData = 13 Then ComboVariedad_Validating(Nothing, Nothing) : Exit Sub

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ComboMarca_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboMarca.KeyDown

        If e.KeyData = 13 Then ComboMarca_Validating(Nothing, Nothing) : Exit Sub

    End Sub
    Private Sub ComboMarca_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboMarca.Validating

        If IsNothing(ComboMarca.SelectedValue) Then ComboMarca.SelectedValue = 0

    End Sub
    Private Sub ComboCategoria_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboCategoria.KeyDown

        If e.KeyData = 13 Then ComboCategoria_Validating(Nothing, Nothing) : Exit Sub

    End Sub
    Private Sub ComboCategoria_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboCategoria.Validating

        If IsNothing(ComboCategoria.SelectedValue) Then ComboCategoria.SelectedValue = 0

    End Sub
    Private Sub ComboEnvase_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ComboEnvase.KeyDown

        If e.KeyData = 13 Then ComboEnvase_Validating(Nothing, Nothing) : Exit Sub

    End Sub
    Private Sub ComboEnvase_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEnvase.Validating

        If IsNothing(ComboEnvase.SelectedValue) Then ComboEnvase.SelectedValue = 0

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
    Private Sub TextCantidadPrimarios_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCantidadPrimarios.KeyPress

        EsNumerico(e.KeyChar, TextCantidadPrimarios.Text, GDecimales)

    End Sub
    Private Sub TextNombre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNombre.KeyPress

        If e.KeyChar = "(" Then
            MsgBox("Caracter: ( no permitido.", MsgBoxStyle.Information)
            e.KeyChar = ""
        End If

    End Sub
    Private Sub TextEAN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextEAN.KeyPress

        EsNumerico(e.KeyChar, TextEAN.Text, 0)

    End Sub
    Private Function MuestraArticuloConStock() As Boolean

        Try
            Dim SqlStr As String = "SELECT * FROM Articulos Where Especie = " & GEspecie & " AND " & _
                "Variedad = " & GVariedad & " AND " & _
                "Marca = " & GMarca & " AND " & _
                "Categoria = " & GCategoria & " AND " & _
                "Envase = " & GEnvase & ";"

            Dt = New DataTable
            If Not Tablas.Read(SqlStr, Conexion, Dt) Then Return False

            MiEnlazador = New BindingSource
            MiEnlazador.DataSource = Dt
            If Dt.Rows.Count = 0 Then
                AgregaRegistroConStock()
                ButtonAceptar.Text = "Alta Articulo"
            Else : EsAlta = False
                ButtonAceptar.Text = "Modificar Articulo"
            End If

            Dim Enlace As Binding

            Enlace = New Binding("Text", MiEnlazador, "Nombre")
            AddHandler Enlace.Parse, AddressOf NombreStr
            TextNombre.DataBindings.Clear()
            TextNombre.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "Clave")
            AddHandler Enlace.Parse, AddressOf NombreStr
            TextInterno.DataBindings.Clear()
            TextInterno.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Especie")
            ComboEspecie.DataBindings.Clear()
            ComboEspecie.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Variedad")
            ComboVariedad.DataBindings.Clear()
            ComboVariedad.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Marca")
            ComboMarca.DataBindings.Clear()
            ComboMarca.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Categoria")
            ComboCategoria.DataBindings.Clear()
            ComboCategoria.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Envase")
            ComboEnvase.DataBindings.Clear()
            ComboEnvase.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Secundario")
            ComboSecundario.DataBindings.Clear()
            ComboSecundario.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "CantidadPrimarios")
            AddHandler Enlace.Format, AddressOf FormatCero
            AddHandler Enlace.Parse, AddressOf ParseCero
            TextCantidadPrimarios.DataBindings.Clear()
            TextCantidadPrimarios.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "Iva")
            AddHandler Enlace.Format, AddressOf FormatNumerico
            '      AddHandler Enlace.Parse, AddressOf IvaParse
            TextIva.DataBindings.Clear()
            TextIva.DataBindings.Add(Enlace)

            Enlace = New Binding("Checked", MiEnlazador, "Exento")
            CheckExento.DataBindings.Clear()
            CheckExento.DataBindings.Add(Enlace)

            Enlace = New Binding("Checked", MiEnlazador, "NoGrabado")
            CheckNoGrabado.DataBindings.Clear()
            CheckNoGrabado.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "EAN")
            AddHandler Enlace.Format, AddressOf FormatCero
            AddHandler Enlace.Parse, AddressOf ParseCero
            TextEAN.DataBindings.Clear()
            TextEAN.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
            ComboEstado.DataBindings.Clear()
            ComboEstado.DataBindings.Add(Enlace)

            Return True
        Catch Err As OleDb.OleDbException
            MsgBox("ERROR de Base de Datos, " & Err.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Finally
        End Try

    End Function
    Private Function MuestraArticuloServiciosYSecos() As Boolean

        Dim SqlStr As String

        Try
            If PEsServicios Then
                SqlStr = "SELECT * FROM ArticulosServicios Where Secos = 0 AND Clave = " & PClave & ";"
            Else
                SqlStr = "SELECT * FROM ArticulosServicios Where Secos = 1 AND Clave = " & PClave & ";"
            End If

            Dt = New DataTable
            If Not Tablas.Read(SqlStr, Conexion, Dt) Then Return False

            MiEnlazador = New BindingSource
            MiEnlazador.DataSource = Dt
            If Dt.Rows.Count = 0 Then
                AgregaRegistroServiciosYSecos()
                ButtonAceptar.Text = "Alta Articulo"
            Else : EsAlta = False
                ButtonAceptar.Text = "Modificar Articulo"
            End If

            Dim Enlace As Binding

            Enlace = New Binding("Text", MiEnlazador, "Nombre")
            AddHandler Enlace.Parse, AddressOf NombreStr
            TextNombre.DataBindings.Clear()
            TextNombre.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "Clave")
            AddHandler Enlace.Parse, AddressOf NombreStr
            TextInterno.DataBindings.Clear()
            TextInterno.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "Iva")
            AddHandler Enlace.Format, AddressOf FormatNumerico
            '      AddHandler Enlace.Parse, AddressOf IvaParse
            TextIva.DataBindings.Clear()
            TextIva.DataBindings.Add(Enlace)

            Enlace = New Binding("Checked", MiEnlazador, "Exento")
            CheckExento.DataBindings.Clear()
            CheckExento.DataBindings.Add(Enlace)

            Enlace = New Binding("Checked", MiEnlazador, "NoGrabado")
            CheckNoGrabado.DataBindings.Clear()
            CheckNoGrabado.DataBindings.Add(Enlace)

            Enlace = New Binding("Text", MiEnlazador, "Cuenta")
            AddHandler Enlace.Format, AddressOf FormatCuenta
            AddHandler Enlace.Parse, AddressOf ParseCuenta
            TextCuentaContable.DataBindings.Clear()
            TextCuentaContable.DataBindings.Add(Enlace)

            Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
            ComboEstado.DataBindings.Clear()
            ComboEstado.DataBindings.Add(Enlace)

            Return True
        Catch Err As OleDb.OleDbException
            MsgBox("ERROR de Base de Datos, " & Err.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Finally
        End Try

    End Function
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub NombreStr(ByVal sender As Object, ByVal Nombre As ConvertEventArgs)

        Nombre.Value = Nombre.Value.ToString.Trim

    End Sub
    Private Sub IvaParse(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Not IsNumeric(Numero.Value) Then
            MsgBox("Iva debe ser Numérico.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If

    End Sub
    Private Sub FormatCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(0, "#")

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(0, "#")
        Else
            Numero.Value = Format(Numero.Value, "000-000000-00")
        End If

    End Sub
    Private Sub ParseCuenta(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then
            Numero.Value = 0
        Else
            Numero.Value = Mid$(TextCuentaContable.Text, 1, 3) & Mid$(TextCuentaContable.Text, 5, 6) & Mid$(TextCuentaContable.Text, 12, 2)
        End If

    End Sub
    Private Sub AgregaRegistroConStock()

        MiEnlazador.AddNew()

        Dim Row As DataRowView = MiEnlazador.Current
        Row("Especie") = GEspecie
        Row("Variedad") = GVariedad
        Row("Marca") = GMarca
        Row("Categoria") = GCategoria
        Row("Envase") = GEnvase
        Row("Nombre") = ""
        Row("Secundario") = 0
        Row("CantidadPrimarios") = 0
        Row("EAN") = 0
        Row("Exento") = False
        Row("NoGrabado") = False
        If GEspecie <> 0 Then
            Row("Iva") = HallaIvaEspecie(GEspecie)
        Else : Row("Iva") = 0
        End If
        Row("Estado") = 1

        If Row("Iva") < 0 Then
            MsgBox("ERROR De Base De Datos,al leer Especie. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
            Exit Sub
        End If

    End Sub
    Private Sub AgregaRegistroServiciosYSecos()

        MiEnlazador.AddNew()

        Dim Row As DataRowView = MiEnlazador.Current
        Row("Nombre") = ""
        Row("Iva") = 0
        Row("Cuenta") = 0
        If PEsSecos Then
            Row("Secos") = True
        Else : Row("Secos") = False
        End If
        Row("Exento") = False
        Row("NoGrabado") = False
        Row("Estado") = 1

    End Sub
    Private Function ActualizaArticulo() As Boolean

        Dim Archivo As String
        If PEsServicios Or PEsSecos Then
            Archivo = "ArticulosServicios"
        Else : Archivo = "Articulos"
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM " & Archivo & ";", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                End Using
                If EsAlta Then
                    MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Else : MsgBox("Modificacion Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                End If
                Dt.AcceptChanges()
                GModificacionOk = True
                Return True
            End Using
        Catch Ex1 As DBConcurrencyException
            MsgBox("Otro Usuario Cambio Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Catch Ex2 As OleDb.OleDbException
            If Ex2.ErrorCode = GAltaExiste Then
                MsgBox("Articulo o Nombre Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Else
                MsgBox("ERROR Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
            Return False
        Finally
        End Try

    End Function
    Private Function ArticuloTieneStock() As Boolean

        Dim Sql As String
        Dim Articulo As Integer = Dt.Rows(0).Item("Clave")

        If Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return True
        End If

        Sql = "SELECT SUM(Stock) FROM Lotes WHERE Articulo = " & Articulo & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        If Resul > 0 Then
                            MsgBox("Articulo Tiene Stock. Operación se CANCELA.")
                            Return True
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Lotes.", MsgBoxStyle.Critical)
            End
        End Try

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Dim Resul = Cmd.ExecuteScalar()
                    If Not IsDBNull(Resul) Then
                        If Resul > 0 Then
                            MsgBox("Articulo Tiene Stock. Operación se CANCELA.")
                            Return True
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Lotes.", MsgBoxStyle.Critical)
            End
        End Try

        Return False

    End Function
    Private Sub IvaEspecie()

        Dim IvaW As Double = HallaIvaEspecie(ComboEspecie.SelectedValue)
        If IvaW < 0 Then
            MsgBox("ERROR De Base De Datos, al leer especie, Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If CDbl(TextIva.Text) <> IvaW Then
            If EsAlta Then TextIva.Text = Format(IvaW, "0.00")
        End If

    End Sub
    Public Function UsadoConStock(ByVal Articulo As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM Lotes WHERE Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM FacturasDetalle AS D INNER JOIN FacturasCabeza AS C ON D.Factura = C.Factura WHERE C.Tr = 0 AND C.EsServicios = 0 AND D.Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM RemitosDetalle WHERE Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Public Function UsadoServicios(ByVal Articulo As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM FacturasDetalle AS D INNER JOIN FacturasCabeza AS C ON D.Factura = C.Factura WHERE C.EsServicios = 1 AND D.Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Public Function UsadoSecos(ByVal Articulo As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM FacturasDetalle AS D INNER JOIN FacturasCabeza AS C ON D.Factura = C.Factura WHERE C.EsServicios = 1 AND D.Articulo = " & Articulo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function Valida() As Boolean

        TextNombre.Text = TextNombre.Text.Trim

        If TextNombre.Text.Length = 0 Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not (PEsServicios Or PEsSecos) Then
            If ComboEspecie.Text.ToString.Trim.Length = 0 Then
                MsgBox("Falta Especie.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboVariedad.Text.ToString.Trim.Length = 0 Then
                MsgBox("Falta variedad.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboMarca.Text.ToString.Trim.Length = 0 Then
                MsgBox("Falta Marca.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboCategoria.Text.ToString.Trim.Length = 0 Then
                MsgBox("Falta Categoria.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboEnvase.Text.ToString.Trim.Length = 0 Then
                MsgBox("Falta Envase.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboSecundario.SelectedValue = ComboEnvase.SelectedValue Then
                MsgBox("Envases Primario y Secundario No Deben Ser Iguales.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboSecundario.SelectedValue <> 0 And TextCantidadPrimarios.Text = "" Then
                MsgBox("Falta Cantidad Envases Primarios en el Secundario.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            If ComboSecundario.SelectedValue = 0 And TextCantidadPrimarios.Text <> "" Then
                MsgBox("Falta Informar Envase Secundario.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Else
            If TextCuentaContable.Text = "" Then
                MsgBox("Falta Cuenta Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If TextIva.Text.ToString.Trim.Length = 0 Then
            MsgBox("Debe informar Iva.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
        If CDbl(TextIva.Text) > 100 Then
            MsgBox("Iva invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CheckExento.Checked And CheckNoGrabado.Checked Then
            MsgBox("Debe seleccionar Exento o No-Grabado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CheckExento.Checked Or CheckNoGrabado.Checked Then
            If CDec(TextIva.Text) <> 0 Then
                MsgBox("Para Exento o No-Grabado Iva debe ser 0.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If
        If Not CheckExento.Checked And Not CheckNoGrabado.Checked Then
            If CDec(TextIva.Text) = 0 Then
                MsgBox("Debe definir IVA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If TextEAN.Text <> "" Then
            If Not EAN13OK(TextEAN.Text) Then
                MsgBox("Codigo EAN 13 Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        Return True

    End Function


  
   
  
End Class