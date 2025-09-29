Public Class UnBalance
    Public PBloqueaFunciones As Boolean
    Public PClave As Integer
    '
    Private MiEnlazador As New BindingSource
    '
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Private Sub UnBalance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        GModificacionOk = False

        ArmaArchivos()

    End Sub
    Private Sub UnBalance_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        If Dt.Rows(0).Item("Desde") <> CDate(TextDesde.Text) Then
            Dt.Rows(0).Item("Desde") = TextDesde.Text
        End If
        If Dt.Rows(0).Item("Hasta") <> CDate(TextHasta.Text) Then
            Dt.Rows(0).Item("Hasta") = TextHasta.Text
        End If

        If IsNothing(Dt.GetChanges) Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Resul As Integer = ActualizaArchivo(Dt)

        If Resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            PClave = HallaClave
        End If

        ArmaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorra_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorra.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PClave = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Balance se Borrara Definitivamente del Sistema. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAux As DataTable = Dt.Copy

        DtAux.Rows(0).Delete()

        Dim Resul As Integer = ActualizaArchivo(DtAux)

        If Resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Borrado Realizado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
            Exit Sub
        End If

    End Sub
    Private Sub PictureAlmanaqueDesde_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueDesde.Click

        If TextDesde.Text = "" Then
            Calendario.PFecha = "01/01/1800"
        Else : Calendario.PFecha = TextDesde.Text
        End If

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextDesde.Text = ""
        Else
            TextDesde.Text = Format(Calendario.PFecha, "01/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueHasta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueHasta.Click

        If TextHasta.Text = "" Then
            Calendario.PFecha = "01/01/1800"
        Else : Calendario.PFecha = TextHasta.Text
        End If

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextHasta.Text = ""
        Else
            Dim FechaW As Date = DateSerial(Year(Calendario.PFecha), Month(Calendario.PFecha) + 1, 0)
            TextHasta.Text = Format(FechaW, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ArmaArchivos()

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Sql As String = "SELECT * FROM Balances WHERE Clave = " & PClave & ";"

        If Not Tablas.Read(Sql, Conexion, Dt) Then End : Exit Sub
        If PClave <> 0 And Dt.Rows.Count = 0 Then
            MsgBox("Balance No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PClave = 0 Then
            Dim Row As DataRow = Dt.NewRow
            Row("Clave") = 0
            Row("Desde") = "01/01/1800"
            Row("Hasta") = "01/01/1800"
            Row("Cerrado") = False
            Dt.Rows.Add(Row)
        End If

        MuestraCabeza()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = Dt

        Dim Enlace As Binding

        If Dt.Rows(0).Item("Desde") <> "01/01/1800" Then
            TextDesde.Text = Format(Dt.Rows(0).Item("Desde"), "dd/MM/yyyy")
        End If

        If Dt.Rows(0).Item("Hasta") <> "01/01/1800" Then
            TextHasta.Text = Format(Dt.Rows(0).Item("Hasta"), "dd/MM/yyyy")
        End If

        Enlace = New Binding("Checked", MiEnlazador, "Cerrado")
        CheckCerrado.DataBindings.Clear()
        CheckCerrado.DataBindings.Add(Enlace)

    End Sub
    Private Function ActualizaArchivo(ByVal DtAux As DataTable) As Integer

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Balances;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtAux.GetChanges)
                    End Using
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Return -1
                Catch ex As DBConcurrencyException
                    Return 0
                Finally
                End Try
            Catch ex As Exception
                Return -1
            End Try
        End Using

    End Function
    Private Function HallaFechaRepetida() As Integer

        Dim SqlFecha As String
        SqlFecha = "Desde < '" & Format(DateAdd(DateInterval.Day, 1, CDate(TextHasta.Text)), "yyyyMMdd") & "' AND Hasta >= '" & Format(CDate(TextDesde.Text), "yyyyMMdd") & "'"

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Balances WHERE " & SqlFecha & " AND Clave <> " & PClave & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: Balanceos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            End
        Finally
        End Try

    End Function
    Private Function HallaPeriodosCerrados() As Boolean

        Dim FechaInicial As Date = DateAdd(DateInterval.Month, -1, CDate(TextDesde.Text))
        Dim Dt As New DataTable

        Do
            FechaInicial = DateAdd(DateInterval.Month, 1, FechaInicial)
            Dim Mes As Integer = FechaInicial.Month
            Dim Anio As Integer = FechaInicial.Year
            Dt.Clear()
            If Not Tablas.Read("SELECT Cerrado FROM CierreContable WHERE Mes = " & Mes & " AND Anio = " & Anio & ";", Conexion, Dt) Then End
            If Dt.Rows.Count = 0 Then Dt.Dispose() : Return False
            If Not Dt.Rows(0).Item("Cerrado") Then Dt.Dispose() : Return False
            If FechaInicial.Year = CDate(TextHasta.Text).Year And FechaInicial.Month = CDate(TextHasta.Text).Month Then Exit Do
        Loop

        Return True

    End Function
    Public Function HallaClave() As Integer

        Dim SqlFecha As String
        Dim SqlFechaDesde = "Year(Desde) = " & CDate(TextDesde.Text).Year & " AND Month(Desde) = " & CDate(TextDesde.Text).Month & " AND Day(Desde) = " & CDate(TextDesde.Text).Day
        Dim SqlFechaHasta = "Year(Hasta) = " & CDate(TextHasta.Text).Year & " AND Month(Hasta) = " & CDate(TextHasta.Text).Month & " AND Day(Hasta) = " & CDate(TextHasta.Text).Day

        SqlFecha = SqlFechaDesde & " AND " & SqlFechaHasta

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Clave FROM Balances WHERE " & SqlFecha & ";", Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Tabla: Balances. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If Not ConsisteFecha(TextDesde.Text) Then
            MsgBox("Fecha Desde Incorrecta. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextDesde.Focus()
            Return False
        End If

        If Not ConsisteFecha(TextHasta.Text) Then
            MsgBox("Fecha Hasta Incorrecta. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextHasta.Focus()
            Return False
        End If

        If DiferenciaDias(CDate(TextDesde.Text), CDate(TextHasta.Text)) < 0 Then
            MsgBox("Fechas Invalidas. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        If Dt.Rows(0).Item("Desde") <> CDate(TextDesde.Text) Or Dt.Rows(0).Item("Hasta") <> CDate(TextHasta.Text) Then
            If HallaFechaRepetida() > 0 Then
                MsgBox("Entorno de Fecha Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Return False
            End If
        End If

        If CheckCerrado.Checked Then
            If Not HallaPeriodosCerrados() Then
                MsgBox("Existe Periodos No Cerrado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Return False
            End If
        End If

        Return True

    End Function
  

    
End Class