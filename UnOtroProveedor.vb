Imports ClassPassWord
Public Class UnOtroProveedor
    Public PProveedor As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim CuitW As String
    Private Sub UnOtroProveedore_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(4) Then PBloqueaFunciones = True

        ArmaTipoIva(ComboTipoIva)
        ComboTipoIva.SelectedValue = 0

        LlenaComboTablas(ComboPais, 28)
        ComboPais.SelectedValue = 0
        With ComboPais
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboProvincia, 31)
        ComboProvincia.SelectedValue = 0
        With ComboProvincia
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GModificacionOk = False

        If Not ArmaArchivos() Then Me.Close()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If IsNothing(Dt.GetChanges) Then MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3) : Exit Sub

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Double = ActualizaProveedor()

        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Dt.AcceptChanges()
        End If
        If Resul = -1 Then
            MsgBox("Nombre Proveedor o Cuit ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR, De Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR, Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PProveedor = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Integer = Usado(Dt.Rows(0).Item("Clave"), Conexion)
        If Resul > 0 Then
            MsgBox("El Cliente esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Resul = Usado(Dt.Rows(0).Item("Clave"), ConexionN)
        If Resul > 0 Then
            MsgBox("El Cliente esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("El Proveedor se Eliminara Definitivamente. ¿Desea Eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        MiEnlazador.Remove(MiEnlazador.Current)

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Resul = ActualizaProveedor()

        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close() : Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("ERROR, De Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR, Otro Usuario Modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub TextNombre_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNombre.KeyPress

        If ValidaStringNombres(e.KeyChar) <> "" Then
            e.KeyChar = ""
        End If

    End Sub
    Private Sub ComboProvincia_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProvincia.Validating

        If IsNothing(ComboProvincia.SelectedValue) Then ComboProvincia.SelectedValue = 0

    End Sub
    Private Sub TextSaldoInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicial.KeyPress

        If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM OtrosProveedores Where Clave = " & PProveedor & ";", Conexion, Dt) Then Me.Close() : Exit Function
        If Dt.Rows.Count = 0 And PProveedor <> 0 Then
            MsgBox("ERROR Proveedor no Existe. Opreacion se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Function
        End If

        If Dt.Rows.Count = 0 Then AgregaRegistro()

        MuestraCabeza()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Nombre")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextNombre.DataBindings.Clear()
        TextNombre.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Calle")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextCalle.DataBindings.Clear()
        TextCalle.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Localidad")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextLocalidad.DataBindings.Clear()
        TextLocalidad.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Provincia")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        ComboProvincia.DataBindings.Clear()
        ComboProvincia.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Pais")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        ComboPais.DataBindings.Clear()
        ComboPais.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Telefonos")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextTelefonos.DataBindings.Clear()
        TextTelefonos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Faxes")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextFaxes.DataBindings.Clear()
        TextFaxes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Cuit")
        AddHandler Enlace.Format, AddressOf FormatCuit
        MaskedCuit.DataBindings.Clear()
        MaskedCuit.DataBindings.Add(Enlace)
        CuitW = MaskedCuit.Text

        Enlace = New Binding("SelectedValue", MiEnlazador, "TipoIva")
        ComboTipoIva.DataBindings.Clear()
        ComboTipoIva.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "ExentoRetencion")
        CheckExentoRetencion.DataBindings.Clear()
        CheckExentoRetencion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "SaldoInicial")
        AddHandler Enlace.Format, AddressOf FormatNumerico
        AddHandler Enlace.Parse, AddressOf ParseCero
        TextSaldoInicial.DataBindings.Clear()
        TextSaldoInicial.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Nombre As ConvertEventArgs)

        Nombre.Value = Nombre.Value.ToString.Trim

    End Sub
    Private Sub FormatNumerico(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = FormatNumber(Numero.Value, GDecimales)

    End Sub
    Private Sub FormatCuit(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "00000000000")

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub AgregaRegistro()

        Dim Row As DataRow = Dt.NewRow
        Row("Nombre") = ""
        Row("Calle") = ""
        Row("Localidad") = ""
        Row("Provincia") = 0
        Row("Pais") = 0
        Row("Telefonos") = ""
        Row("Faxes") = ""
        Row("Cuit") = 0
        Row("TipoIva") = 0
        Row("Estado") = 1
        Row("ExentoRetencion") = True
        Row("SaldoInicial") = 0
        Row("Comentario") = ""
        Dt.Rows.Add(Row)

    End Sub
    Private Function ActualizaProveedor() As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM OtrosProveedores;", Miconexion)
                    Dim SqlCommandBuilder As OleDb.OleDbCommandBuilder = New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(Dt)
                End Using
                Return 1000
            End Using
        Catch ex As OleDb.OleDbException
            If ex.ErrorCode = GAltaExiste Then
                Return -1
            Else
                Return -2
            End If
        Catch ex As DBConcurrencyException
            Return 0
        Finally
        End Try

    End Function
    Public Function Usado(ByVal Proveedor As Integer, ByVal ConexionStr As String) As Integer

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM OtrasFacturasCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return 10
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Movimiento) FROM OtrosPagosCabeza WHERE Proveedor = " & Proveedor & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            Catch ex As Exception
                Return -1
            End Try
        End Using

    End Function
    Private Function Valida() As Boolean

        If TextNombre.Text = "" Then
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextNombre.Focus()
            Return False
        End If
        If ComboProvincia.SelectedValue = 0 Then
            MsgBox("Falta Provincia.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboProvincia.Focus()
            Return False
        End If
        If ComboPais.SelectedValue = 0 Then
            MsgBox("Falta Pais.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboPais.Focus()
            Return False
        End If

        If MaskedCuit.Text = CuitNumerico(GCuitEmpresa) Then
            MsgBox("Cuit informado es igual al de " & GNombreEmpresa, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedCuit.Focus()
            Return False
        End If

        Dim aa As New DllVarias
        If Not aa.ValidaCuiT(MaskedCuit.Text) Then
            MsgBox("CUIT Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedCuit.Focus()
            Return False
        End If

        If Dt.Rows(0).Item("Cuit") <> CuitW And ComboTipoIva.SelectedValue <> Exterior Then
            Try
                Using Miconexion As New OleDb.OleDbConnection(Conexion)
                    Miconexion.Open()
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuit) FROM OtrosProveedores WHERE Cuit = " & MaskedCuit.Text & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then
                            If MsgBox("Cuit Ya Existe. Quiere Continuar Igualmente?", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                                MaskedCuit.Focus()
                                Return False
                            End If
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MsgBox("Error Base de Datos al leer Tabla de OtrosProveedores.", MsgBoxStyle.Critical)
                End
            End Try
        End If

        If ComboTipoIva.SelectedValue = 0 Then
            MsgBox("Falta Tipo IVA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        If ComboTipoIva.SelectedValue = 3 Then
            MsgBox("Consumidor Final Incorrecto para un Otro Proveedor.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboTipoIva.Focus()
            Return False
        End If

        Return True

    End Function
End Class