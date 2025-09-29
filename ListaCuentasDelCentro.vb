Imports System.Transactions
Public Class ListaCuentasDelCentro
    Public PBloqueaFunciones As Boolean
    Public PCentro As Integer
    Public PNombreCentro As String
    '
    Private WithEvents bs As New BindingSource
    ' 
    Dim Dt As DataTable
    Dim DtN As DataTable
    Private Sub ListaCentrosDeCosto_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(11) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        LabelCentro.Text = Format(PCentro, "000") & " - " & PNombreCentro

        ArmaArchivos()

        ' bs.Sort = "Fecha ASC"

    End Sub
    Private Sub ListaCentrosDeCosto_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        bs.EndEdit()

        If Not IsNothing(Dt.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then
            MsgBox("No Hay Modificaciones. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaArchivo()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If Resul <= 0 Then
            MsgBox("Error al Grabar Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            ArmaArchivos()
        End If

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then
            MsgBox("No hay un Registro Actual para Eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataRowView = bs.Current
        Dim ErrorW As String = ""

        If Row.Row.RowState <> DataRowState.Added Then
            If DocumentosUsado(Row("ClaveCuenta"), ErrorW) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(ErrorW & " Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If AsientosUsado(Row("ClaveCuenta"), ErrorW, Conexion) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(ErrorW & " Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If AsientosUsado(Row("ClaveCuenta"), ErrorW, ConexionN) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox(ErrorW & " Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If MsgBox("Cuenta " & Format(Row("ClaveCuenta"), "000-000000-00") & " se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        If Row.Row.RowState = DataRowState.Detached Then
            Grid.Rows.Remove(Grid.CurrentRow)
        Else
            Grid.Rows.Remove(Grid.CurrentRow) : BorraEnN(Row("ClaveCuenta"))
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminarTodasLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarTodasLinea.Click

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then
            MsgBox("No hay un Registro Actual para Eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Row As DataGridViewRow
        Dim ErrorW As String = ""
        Dim NoBorrar As Boolean

        For I As Integer = Grid.Rows.Count - 1 To 0 Step -1
            Row = Grid.Rows(I)
            NoBorrar = False
            If DocumentosUsado(Row.Cells("ClaveCuenta").Value, ErrorW) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                NoBorrar = True
            End If
            If AsientosUsado(Row.Cells("ClaveCuenta").Value, ErrorW, Conexion) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                NoBorrar = True
            End If
            If AsientosUsado(Row.Cells("ClaveCuenta").Value, ErrorW, ConexionN) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                NoBorrar = True
            End If
            Dim ClaveCuenta As Double = Row.Cells("ClaveCuenta").Value
            If Not NoBorrar Then Grid.Rows.Remove(Row) : BorraEnN(ClaveCuenta)
        Next

        If Grid.Rows.Count <> 0 Then
            MsgBox("Los Registros no Borrados Estan Siendo Usados.", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        SeleccionarCuenta.PEsSoloCuentas = True
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuenta <> 0 Then
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = Dt.Select("ClaveCuenta = " & CDbl(PCentro & Format(SeleccionarCuenta.PCuenta, "00000000")))
            If RowsBusqueda.Length = 0 Then
                bs.AddNew()
                Dim Row As DataRowView = bs.Current
                Dim Centro As Integer
                Dim Cuenta As Integer
                Dim SubCuenta As Integer
                HallaPartesCuenta(SeleccionarCuenta.PCuenta, Centro, Cuenta, SubCuenta)
                Row("ClaveCuenta") = CDbl(PCentro & Format(SeleccionarCuenta.PCuenta, "00000000"))
                Row("Centro") = PCentro
                Row("Cuenta") = Cuenta
                Row("SubCuenta") = SeleccionarCuenta.PCuenta
                Row("Fecha") = "01/01/1800"
                Row("SaldoInicial") = 0
            Else
                MsgBox("Cuenta Ya Exite.")
            End If
        End If
        SeleccionarCuenta.Dispose()

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
    Private Sub ArmaArchivos()

        Dim Patron As String = PCentro & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Dim Sql As String = "SELECT * FROM PlanDeCuentas WHERE CAST(CAST(ClaveCuenta AS numeric) as char) LIKE '" & Patron & "';"


        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dt = New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        DtN = New DataTable
        If PermisoTotal Then
            If Not Tablas.Read(Sql, ConexionN, DtN) Then Me.Close() : Exit Sub
        End If

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Cuenta.DataSource = Tablas.Leer("SELECT Cuenta,Nombre FROM Cuentas;")
        Dim Row As DataRow = Cuenta.DataSource.newrow
        Row("Cuenta") = 0
        Row("Nombre") = " "
        Cuenta.DataSource.rows.add(Row)
        Cuenta.DisplayMember = "Nombre"
        Cuenta.ValueMember = "Cuenta"

        SubCuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SubCuentas;")
        Row = SubCuenta.DataSource.newrow
        Row("Clave") = 0
        Row("Nombre") = " "
        SubCuenta.DataSource.rows.add(Row)
        SubCuenta.DisplayMember = "Nombre"
        SubCuenta.ValueMember = "Clave"

    End Sub
    Private Sub BorraEnN(ByVal Clave As Double)

        If DtN.Rows.Count = 0 Then Exit Sub

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtN.Select("ClaveCuenta = " & Clave)
        If RowsBusqueda.Length <> 0 Then
            RowsBusqueda(0).Delete()
        End If

    End Sub
    Private Function ActualizaArchivo() As Integer

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(Dt.GetChanges) Then
                    Resul = GrabaTabla(Dt.GetChanges, "PlanDeCuentas", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtN.GetChanges) Then
                    Resul = GrabaTabla(DtN.GetChanges, "PlanDeCuentas", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Resul = 0
        End Try

    End Function
    Function DocumentosUsado(ByVal Cuenta As Double, ByRef ErrorW As String) As Boolean

        ErrorW = ""

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                ErrorW = "Usado en Asientos de Documentos del Sistema."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(ClaveCuenta) FROM SeteoDocumentos WHERE ClaveCuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Medios de Pago."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM Miselaneas WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Medios de Pago."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM Tablas WHERE Cuenta = " & Cuenta & " OR Cuenta2 = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Insumos."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM Insumos WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Articulos Servicios."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM ArticulosServicios WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                ErrorW = "Usado en Empleados."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM Empleados WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Function AsientosUsado(ByVal Cuenta As Double, ByRef ErrorW As String, ByVal ConexionStr As String) As Boolean

        ErrorW = ""

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                ErrorW = "Usado en Asientos."
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Cuenta) FROM AsientosDetalle WHERE Cuenta = " & Cuenta & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error, Base de Datos. " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting


        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ClaveCuenta" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else
                    e.Value = Format(e.Value, "000-000000-00")
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub


End Class