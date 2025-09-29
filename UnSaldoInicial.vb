Imports System.Math
Imports System.Transactions
Public Class UnSaldoInicial
    Public PClave As Integer
    Public PAbierto As Integer
    Public PTipo As Integer
    Public PEmisor As Integer
    Public PMoneda As Integer
    Public PBloqueaFunciones As Boolean
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '
    Private MiEnlazadorB As New BindingSource
    Private MiEnlazadorN As New BindingSource
    '
    Dim DtB As DataTable
    Dim DtN As DataTable
    Dim ImporteB As Double = 0
    Dim ImporteN As Double = 0
    Dim Cambio As Double = 0
    Dim Fecha As Date
    Private Sub UnSaldoInicial_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PreparaEnlace(PPaseDeProyectos)
        End If
        '----------------------------------------------------------------------------------

        If PTipo = 3 Then
            If Not PermisoEscritura(114) Then PBloqueaFunciones = True
        End If
        If PTipo = 2 Then
            If Not PermisoEscritura(214) Then PBloqueaFunciones = True
        End If

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        Dim Row As DataRow = ComboMoneda.DataSource.newrow
        Row("Nombre") = "PESOS"
        Row("Clave") = 1
        ComboMoneda.DataSource.rows.add(Row)
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"

        GModificacionOk = False

        ArmaArchivos()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazadorB.EndEdit()
        MiEnlazadorN.EndEdit()

        'Si hay camio de moneda o cambio y no puede acceder a la N. no lo hace.
        If DtN.Rows.Count = 0 And DtB.Rows(0).Item("Rel") Then
            If DtB.Rows(0).Item("Moneda") <> PMoneda Then
                MsgBox("No es Posible Actualizar Saldos Iniciales (1000). Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If DtB.Rows(0).Item("Cambio") <> Cambio Then
                MsgBox("No es Posible Actualizar Saldos Iniciales (1000). Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If DtB.Rows(0).Item("Fecha") <> Fecha Then
                MsgBox("No es Posible Actualizar Saldos Iniciales (1000). Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End If

        If DtN.Rows.Count <> 0 Then
            If DtB.Rows(0).Item("Moneda") <> DtN.Rows(0).Item("Moneda") Then DtN.Rows(0).Item("Moneda") = DtB.Rows(0).Item("Moneda")
            If DtB.Rows(0).Item("Cambio") <> DtN.Rows(0).Item("Cambio") Then DtN.Rows(0).Item("Cambio") = DtB.Rows(0).Item("Cambio")
            If DtB.Rows(0).Item("Fecha") <> DtN.Rows(0).Item("Fecha") Then DtN.Rows(0).Item("Fecha") = DtB.Rows(0).Item("Fecha")
        End If

        If Not Valida() Then Exit Sub

        Dim DtBW As DataTable = DtB.Copy
        Dim DtNW As DataTable = DtN.Copy

        If DtBW.Rows.Count <> 0 And DtNW.Rows.Count <> 0 Then
            'pone la marca de rel. 
            DtBW.Rows(0).Item("Rel") = True
            DtNW.Rows(0).Item("Rel") = True
        End If

        If DtBW.Rows.Count <> 0 And DtNW.Rows.Count <> 0 Then
            'Borra en las dos bases solo si saldoinicial es cero en ambas. 
            If DtBW.Rows(0).Item("Importe") = 0 And DtNW.Rows(0).Item("Importe") = 0 Then
                DtBW.Rows(0).Delete()
                DtNW.Rows(0).Delete()
            Else
                'Si en la N saldoinicial es cero pero no en B entonces la borra de la base N. 
                If DtNW.Rows(0).Item("Importe") = 0 Then
                    DtNW.Rows(0).Delete()
                    DtBW.Rows(0).Item("Rel") = False
                End If
            End If
        End If

        If DtBW.Rows.Count <> 0 Then
            If DtBW.Rows(0).RowState <> DataRowState.Deleted Then
                If DtBW.Rows(0).Item("Importe") <> ImporteB Then
                    DtBW.Rows(0).Item("Saldo") = Trunca(Abs(DtBW.Rows(0).Item("Importe")) - (Abs(ImporteB) - DtBW.Rows(0).Item("Saldo")))
                End If
            End If
        End If
        If DtNW.Rows.Count <> 0 Then
            If DtNW.Rows(0).RowState <> DataRowState.Deleted Then
                If DtNW.Rows(0).Item("Importe") <> ImporteN Then
                    DtNW.Rows(0).Item("Saldo") = Trunca(Abs(DtNW.Rows(0).Item("Importe")) - (Abs(ImporteN) - DtNW.Rows(0).Item("Saldo")))
                End If
            End If
        End If

        If IsNothing(DtNW.GetChanges) And IsNothing(DtBW.GetChanges) Then
            MsgBox("No hay Cambio. Operación se CANCELA.")
            Exit Sub
        End If

        Dim Resul As Double = ActualizaSaldoInicial(DtBW, DtNW)

        If Resul < 0 Then
            MsgBox("ERROR: En Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
        End If

        Me.Close()

    End Sub
    Private Sub TextSaldoInicial_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicialB.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicialB.Text, GDecimales)

    End Sub
    Private Sub TextSaldoInicialN_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextSaldoInicialN.KeyPress

        EsNumericoConSigno(e.KeyChar, TextSaldoInicialN.Text, GDecimales)

    End Sub
    Private Sub TextCambio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCambio.KeyPress

        EsNumerico(e.KeyChar, TextCambio.Text, 3)

    End Sub
    Private Sub ArmaArchivos()

        If PClave <> 0 Then
            HallaTipoEmisor(PClave, PAbierto, PTipo, PEmisor)
        End If

        DtB = New DataTable
        DtN = New DataTable

        If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = " & PTipo & " AND Emisor = " & PEmisor & ";", Conexion, DtB) Then End
        If DtB.Rows.Count = 0 Then
            Dim Row1 As DataRow = DtB.NewRow
            Row1("Emisor") = PEmisor
            Row1("Tipo") = PTipo
            Row1("Fecha") = DateTime1.Value
            Row1("Importe") = 0
            Row1("Saldo") = 0
            Row1("Moneda") = PMoneda
            If PMoneda = 1 Then
                Row1("Cambio") = 1
            Else
                Row1("Cambio") = 0
            End If
            Row1("Rel") = False
            DtB.Rows.Add(Row1)
            ImporteB = 0
        End If

        If PermisoTotal Then
            If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Tipo = " & PTipo & " AND Emisor = " & PEmisor & ";", ConexionN, DtN) Then End
            If DtN.Rows.Count = 0 Then
                Dim Row1 As DataRow = DtN.NewRow
                Row1("Emisor") = PEmisor
                Row1("Tipo") = PTipo
                Row1("Fecha") = DateTime1.Value
                Row1("Importe") = 0
                Row1("Saldo") = 0
                Row1("Moneda") = PMoneda
                If PMoneda = 1 Then
                    Row1("Cambio") = 1
                Else
                    Row1("Cambio") = 0
                End If
                Row1("Rel") = False
                DtN.Rows.Add(Row1)
                ImporteN = 0
            End If
        Else
            DtN = DtB.Clone
        End If

        MuestraCabezaB()
        If PermisoTotal Then MuestraCabezaN()

        If PermisoTotal Then Panel7.Visible = True
        If ComboMoneda.SelectedValue = 1 Then
            TextCambio.ReadOnly = True
        End If

    End Sub
    Private Sub MuestraCabezaB()

        MiEnlazadorB = New BindingSource
        MiEnlazadorB.DataSource = DtB

        Dim Row As DataRow = DtB.Rows(0)

        Dim Enlace As Binding

        Enlace = New Binding("SelectedValue", MiEnlazadorB, "Moneda")
        ComboMoneda.DataBindings.Clear()
        ComboMoneda.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorB, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextSaldoInicialB.DataBindings.Clear()
        TextSaldoInicialB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorB, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImputableB.DataBindings.Clear()
        TextImputableB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorB, "Cambio")
        AddHandler Enlace.Format, AddressOf FormatCambio
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextCambio.DataBindings.Clear()
        TextCambio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorB, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        ImporteB = Row("Importe")
        Cambio = Row("Cambio")
        Fecha = Row("Fecha")

    End Sub
    Private Sub MuestraCabezaN()

        MiEnlazadorN = New BindingSource
        MiEnlazadorN.DataSource = DtN

        Dim Row As DataRow = DtN.Rows(0)

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazadorN, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        AddHandler Enlace.Parse, AddressOf ParseImporte
        TextSaldoInicialN.DataBindings.Clear()
        TextSaldoInicialN.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazadorN, "Saldo")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImputableN.DataBindings.Clear()
        TextImputableN.DataBindings.Add(Enlace)

        ImporteN = Row("Importe")

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatCambio(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If IsDBNull(Numero.Value) Then Numero.Value = 0
        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 3)
        End If

    End Sub
    Private Sub ParseImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function ActualizaSaldoInicial(ByVal DtBW As DataTable, ByVal DtNW As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtBW.GetChanges) Then
                    Resul = GrabaTabla(DtBW.GetChanges, "SaldosInicialesCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtNW.GetChanges) Then
                    Resul = GrabaTabla(DtNW.GetChanges, "SaldosInicialesCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                GModificacionOk = True
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Function EsIgualSigno(ByVal Importe1 As Double, ByVal Importe2 As Double) As Boolean

        If Importe1 = 0 Then Return True
        If Importe2 = 0 Then Return True

        If Importe1 > 0 And Importe2 > 0 Then Return True
        If Importe1 < 0 And Importe2 < 0 Then Return True

        Return False

    End Function
    Private Sub HallaTipoEmisor(ByVal Clave As Integer, ByVal Abierto As Boolean, ByRef Tipo As Integer, ByRef Emisor As Integer)

        Dim Dt As New DataTable
        Dim ConexionStr As String

        If Abierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT Tipo,Emisor FROM SaldosInicialesCabeza WHERE Clave = " & PClave & ";", ConexionStr, Dt) Then End
        Tipo = Dt.Rows(0).Item("Tipo")
        Emisor = Dt.Rows(0).Item("Emisor")

        Dt.Dispose()

    End Sub
    Private Function Valida() As Boolean

        Dim Row As DataRow

        Row = DtB.Rows(0)
        'Consiste Blanco.
        If Row("Importe") <> 0 And Row("Cambio") = 0 Then
            MsgBox("Falta Informar Cambio. Operación se CANCELA.")
            TextCambio.Focus()
            Return False
        End If
        If Row("Cambio") <> 0 Then
            If ComboMoneda.SelectedValue = 1 And Row("Cambio") <> 1 Then
                MsgBox("Cambio incorrecto para Moneda Nacional. Operación se CANCELA.")
                TextCambio.Focus()
                Return False
            End If
        End If
        If Not EsIgualSigno(Row("Importe"), ImporteB) And Abs(ImporteB) <> Row("Saldo") Then
            MsgBox("Importe no puede cambiar de signo con Imputaciones. Operación se CANCELA.")
            TextSaldoInicialB.Focus()
            Return False
        End If
        If Abs(Row("Importe")) < Trunca(Abs(ImporteB) - Row("Saldo")) Then
            MsgBox("Nuevo Saldo Menor al Importe Ya Imputado. Operación se CANCELA.")
            TextSaldoInicialB.Focus()
            Return False
        End If

        If PermisoTotal Then
            Row = DtN.Rows(0)
            'Consiste Negro.
            If Row("Importe") <> 0 And Row("Cambio") = 0 Then
                MsgBox("Falta Informar Cambio. Operación se CANCELA.")
                TextCambio.Focus()
                Return False
            End If
            If Row("Cambio") <> 0 Then
                If ComboMoneda.SelectedValue = 1 And Row("Cambio") <> 1 Then
                    MsgBox("Cambio incorrecto para Moneda Nacional. Operación se CANCELA.")
                    TextCambio.Focus()
                    Return False
                End If
            End If
            If Not EsIgualSigno(Row("Importe"), ImporteN) And Abs(ImporteN) <> Row("Saldo") Then
                MsgBox("Importe no puede cambiar de signo con Imputaciones. Operación se CANCELA.")
                TextSaldoInicialN.Focus()
                Return False
            End If
            If Abs(Row("Importe")) < Trunca(Abs(ImporteN) - Row("Saldo")) Then
                MsgBox("Nuevo Saldo Menor al Importe Ya Imputado. Operación se CANCELA.")
                TextSaldoInicialN.Focus()
                Return False
            End If
        End If

        Return True

    End Function


End Class