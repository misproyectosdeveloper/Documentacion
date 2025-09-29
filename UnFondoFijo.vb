Imports System.Transactions
Public Class UnFondoFijo
    Public PNumero As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Dim Dt As DataTable
    Dim DtNumero As DataTable
    '
    Public FondoFijo As Integer
    Public Numero As Integer
    Dim ConexionFondoFijo As String
    Private Sub UnFondoFijo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        If PNumero = 0 Then PAbierto = True

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

        If ComboFondoFijo.SelectedValue = 0 Then
            MsgBox("Falta Informar Fondo Fijo. Operación se CANCELA.")
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        Dim DtAux As DataTable = Dt.Copy
        Dim DtNumeroAux As DataTable = DtNumero.Copy

        Dim Numero As Integer = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PNumero = 0 Then
            Numero = DtNumeroAux.Rows(0).Item("Numero") + 1
            DtAux.Rows(0).Item("Numero") = Numero
            DtNumeroAux.Rows(0).Item("Numero") = Numero
        End If

        If IsNothing(DtAux.GetChanges) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        Dim Resul As Integer = ActualizaArchivo(DtAux, DtNumeroAux)

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
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            If PNumero = 0 Then PNumero = Numero
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorra.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If HallaRendiciones(CInt(TextNumero.Text)) > 0 Then
            MsgBox("Fondo Fijo Tiene Rendiciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtAux As DataTable = Dt.Copy
        Dim DtNumeroAux As DataTable = DtNumero.Copy

        DtAux.Rows(0).Delete()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaArchivo(DtAux, DtNumeroAux)

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

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PNumero = 0
        UnFondoFijo_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            PAbierto = False
            ConexionFondoFijo = ConexionN
        Else : PictureCandado.Image = ImageList1.Images.Item("Abierto")
            PAbierto = True
            ConexionFondoFijo = Conexion
        End If

    End Sub
    Private Sub ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        Dim Sql As String = "SELECT * FROM FondosFijos WHERE Numero = " & PNumero & ";"
        If Not Tablas.Read(Sql, ConexionFondoFijo, Dt) Then Me.Close() : Exit Sub
        If PNumero <> 0 And Dt.Rows.Count = 0 Then
            MsgBox("Fondo Fijo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PNumero = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("FondoFijo") = 0
            Row("Numero") = 0
            Row("Saldo") = 0
            Row("Fecha") = Now
            Row("Cerrado") = False
            Row("Comentario") = ""
            Dt.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtNumero = New DataTable
        If Not Tablas.Read("SELECT * FROM NumeracionFondoFijo;", Conexion, DtNumero) Then
            MsgBox("Error Base de Datos al leer Tabla: NumeracionFondoFijo", MsgBoxStyle.Critical)
            End
        End If
        If DtNumero.Rows.Count = 0 Then
            Dim Row As DataRow = DtNumero.NewRow
            Row("Clave") = 1
            Row("Numero") = 0
            Row("Rendicion") = 0
            DtNumero.Rows.Add(Row)
        End If

        If PNumero = 0 Then
            ComboFondoFijo.Enabled = True
            PictureCandado.Enabled = True
        Else : ComboFondoFijo.Enabled = False
            PictureCandado.Enabled = False
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
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextNumero.DataBindings.Clear()
        TextNumero.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatRendicion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = ""
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

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
    Private Function HallaRendiciones(ByVal Numero As Integer) As Integer

        Dim Sql As String = "SELECT COUNT(Rendicion) FROM RendicionFondoFijo WHERE Numero = " & Numero & ";"
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
    Private Function ActualizaArchivo(ByVal DtAux As DataTable, ByVal DtNumeroAux As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Fondo Fijo.
                If Not IsNothing(DtAux.GetChanges) Then
                    Resul = GrabaTabla(DtAux.GetChanges, "FondosFijos", ConexionFondoFijo)
                    If Resul <= 0 Then Return Resul
                End If
                'Actualiza Ultima numeracion.
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

End Class