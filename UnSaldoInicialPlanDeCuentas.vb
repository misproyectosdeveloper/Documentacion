Imports System.Transactions
Public Class UnSaldoInicialPlanDeCuentas
    Dim DtB As New DataTable
    Dim DtN As New DataTable
    Private Sub UnSaldoInicialPlanDeCuentas_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoTotal Then
            Panel3.Visible = False
            PictureBox2.Visible = False
        End If

    End Sub
    Private Sub UnSaldoInicialPlanDeCuentas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If DtB.Rows.Count = 0 Then Exit Sub

        If CDbl(TextSaldoInicialB.Text) <> DtB.Rows(0).Item("SaldoInicial") Then
            DtB.Rows(0).Item("SaldoInicial") = CDbl(TextSaldoInicialB.Text)
            DtB.Rows(0).Item("Fecha") = DateTimeB.Value
        End If
        If Format(DateTimeB.Value, "dd/MM/yyyy") <> Format(DtB.Rows(0).Item("Fecha"), "dd/MM/yyyy") And CDbl(TextSaldoInicialB.Text) <> 0 Then
            DtB.Rows(0).Item("Fecha") = DateTimeB.Value
        End If

        If DtN.Rows.Count <> 0 Then
            If CDbl(TextSaldoInicialN.Text) <> DtN.Rows(0).Item("SaldoInicial") Then
                DtN.Rows(0).Item("SaldoInicial") = CDbl(TextSaldoInicialN.Text)
                DtN.Rows(0).Item("Fecha") = DateTimeN.Value
            End If
            If Format(DateTimeN.Value, "dd/MM/yyyy") <> Format(DtN.Rows(0).Item("Fecha"), "dd/MM/yyyy") And CDbl(TextSaldoInicialN.Text) <> 0 Then
                DtN.Rows(0).Item("Fecha") = DateTimeN.Value
            End If
        Else
            If CDbl(TextSaldoInicialN.Text) <> 0 And PermisoTotal Then
                CopiaTabla(DtB, DtN)
                DtN.Rows(0).Item("SaldoInicial") = CDbl(TextSaldoInicialN.Text)
                DtN.Rows(0).Item("Fecha") = DateTimeN.Value
            End If
        End If

        If IsNothing(DtB.GetChanges) And IsNothing(DtN.GetChanges) Then Exit Sub

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "PlanDeCuentas", Conexion)
                    If Resul <= 0 Then Exit Try
                End If
                '
                If Not IsNothing(DtN.GetChanges) Then
                    Resul = GrabaTabla(DtN.GetChanges, "PlanDeCuentas", ConexionN)
                    If Resul <= 0 Then Exit Try
                End If

                Scope.Complete()
                Resul = 1000
            End Using
        Catch ex As TransactionException
            Resul = 0
        End Try

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizadados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        ArmaArchivos()

    End Sub
    Private Sub TextSaldoInicialB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicialB.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicialB.Text, GDecimales)

    End Sub
    Private Sub TextSaldoInicialN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicialN.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicialN.Text, GDecimales)

    End Sub
    Private Sub TextSaldoInicialB_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextSaldoInicialB.Validating

        If TextSaldoInicialB.Text.Trim = "" Then
            TextSaldoInicialB.Text = "0.00"
        Else
            TextSaldoInicialB.Text = FormatNumber(CDbl(TextSaldoInicialB.Text), GDecimales)
        End If

    End Sub
    Private Sub TextSaldoInicialN_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextSaldoInicialN.Validating

        If TextSaldoInicialN.Text.Trim = "" Then
            TextSaldoInicialN.Text = "0.00"
        Else
            TextSaldoInicialN.Text = FormatNumber(CDbl(TextSaldoInicialN.Text), GDecimales)
        End If

    End Sub
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        SeleccionarCuenta.PCentro = 0
        SeleccionarCuenta.PEsNivelCuenta = True
        SeleccionarCuenta.ShowDialog()
        If SeleccionarCuenta.PCuentaStr <> "" Then
            If SeleccionarCuenta.PCuentaStr.Trim.Length < 11 Then
                MsgBox("Cuenta Incompleta.")
                Exit Sub
            End If
            TextCentro.Text = Mid$(SeleccionarCuenta.PCuentaStr, 1, 3).Trim
            TextCuenta.Text = Mid$(SeleccionarCuenta.PCuentaStr, 4, 6).Trim
            TextSubCuenta.Text = Mid$(SeleccionarCuenta.PCuentaStr, 10, 2).Trim
        End If
        SeleccionarCuenta.Dispose()

        ArmaArchivos()
        Panel1.Visible = True

    End Sub
    Private Sub ArmaArchivos()

        If TextCentro.Text.Trim = "" Or TextCuenta.Text.Trim = "" Or TextSubCuenta.Text.Trim = "" Then
            Exit Sub
        End If

        DtB = New DataTable

        TextSaldoInicialB.Text = "0.00"
        TextSaldoInicialN.Text = "0.00"
        DateTimeB.Value = Date.Now
        DateTimeN.Value = Date.Now

        If Not Tablas.Read("SELECT * FROM PlanDeCuentas Where ClaveCuenta = " & CDbl(TextCentro.Text & TextCuenta.Text & TextSubCuenta.Text), Conexion, DtB) Then Me.Close() : Exit Sub
        If DtB.Rows(0).Item("SaldoInicial") <> 0 Then
            TextSaldoInicialB.Text = FormatNumber(DtB.Rows(0).Item("SaldoInicial"), GDecimales)
            DateTimeB.Value = DtB.Rows(0).Item("Fecha")
        End If

        DtN = New DataTable
        If PermisoTotal Then
            If Not Tablas.Read("SELECT * FROM PlanDeCuentas Where ClaveCuenta = " & CDbl(TextCentro.Text & TextCuenta.Text & TextSubCuenta.Text), ConexionN, DtN) Then Me.Close() : Exit Sub
            If DtN.Rows.Count <> 0 Then
                If DtN.Rows(0).Item("SaldoInicial") <> 0 Then
                    TextSaldoInicialN.Text = FormatNumber(DtN.Rows(0).Item("SaldoInicial"), GDecimales)
                    DateTimeN.Value = DtN.Rows(0).Item("Fecha")
                End If
            End If
        End If

    End Sub



End Class