Public Class OpcionImportacion
    Public PDtImportacion As DataTable
    Public PRemito As Decimal
    Public PAbierto As Boolean
    '
    Dim Dt As DataTable
    Private Sub OpcionImportacion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Row As DataRow

        Dt = New DataTable

        If Not Tablas.Read("SELECT Clave,Nombre,Base1,Base2 FROM Empresas WHERE Clave <> " & GClaveEmpresa & ";", ConexionEmpresa, Dt) Then End
        For Each Row In Dt.Rows
            Row("Nombre") = Desencriptar(Row("Nombre"))
        Next
        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Base1") = ""
        Row("Base2") = ""
        Dt.Rows.Add(Row)

        ComboEmpresa.DataSource = Dt
        ComboEmpresa.DisplayMember = "Nombre"
        ComboEmpresa.ValueMember = "Clave"
        ComboEmpresa.SelectedValue = 0
        With ComboEmpresa
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
        PAbierto = True

        If Not PermisoTotal Then
            PictureCandado.Visible = False
        End If

        MaskedRemito.Text = "000000000000"

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim ConexionAsociado As String

        Dim RowsBusqueda() As DataRow
        RowsBusqueda = Dt.Select("Clave = " & ComboEmpresa.SelectedValue)

        If PAbierto Then
            ConexionAsociado = ConexionW1.Replace("XXXX", RowsBusqueda(0).Item("Base1")).Replace("YYYY", GServidor)
        Else
            ConexionAsociado = ConexionW1.Replace("XXXX", RowsBusqueda(0).Item("Base2")).Replace("YYYY", GServidor)
        End If

        Dim ConexionAsociadoBlanca = ConexionW1.Replace("XXXX", RowsBusqueda(0).Item("Base1")).Replace("YYYY", GServidor)

        Dim Dt1 As New DataTable
        If Not Tablas.Read("SELECT Cliente,Estado FROM RemitosCabeza WHERE Remito = " & Val(MaskedRemito.Text) & ";", ConexionAsociado, Dt1) Then End
        If Dt1.Rows.Count = 0 Then
            MsgBox("Remito No Existe.", MsgBoxStyle.Information)
            Dt1.Dispose()
            Exit Sub
        End If
        If Dt1.Rows(0).Item("Estado") = 3 Then
            MsgBox("Remito Esta Anulado.", MsgBoxStyle.Information)
            Dt1.Dispose()
            Exit Sub
        End If

        Dim Cliente As Integer = Dt1.Rows(0).Item("Cliente")

        Dim DT2 As New DataTable
        Dim Cantidad As Decimal = 0

        If Not Tablas.Read("SELECT Articulo,(Cantidad - Devueltas) AS Cantidad FROM RemitosDetalle WHERE Cantidad <> Devueltas AND Remito = " & Val(MaskedRemito.Text) & ";", ConexionAsociado, DT2) Then End
        For Each Row As DataRow In DT2.Rows
            Dim ArticuloAsociado As String = NombreArticuloAsociado(Row("Articulo"), ConexionAsociadoBlanca)
            Row("Articulo") = HallaCodigoCliente(Cliente, Row("Articulo"), ConexionAsociadoBlanca)
            If Row("Articulo") = 0 Then
                MsgBox("Falta Codigo Cliente para Articulo " & ArticuloAsociado & " del Remito. Operación se CANCELA.", MsgBoxStyle.Critical)
                Dt1.Dispose()
                Exit Sub
            End If
            If NombreArticuloYEstado(Row("Articulo"), Conexion) = "" Then
                MsgBox("Codigo Cliente para Articulo " & ArticuloAsociado & " No Existe en la Empresa Destino o esta Deshabilitado. Operación se CANCELA.", MsgBoxStyle.Critical)
                Dt1.Dispose()
                Exit Sub
            End If
            Cantidad = Cantidad + Row("Cantidad")
        Next

        If Cantidad = 0 Then
            MsgBox("Remito Fue Devuelto. Operación se CANCELA.", MsgBoxStyle.Critical)
            Dt1.Dispose()
            Exit Sub
        End If

        PDtImportacion = DT2.Copy
        PRemito = Val(MaskedRemito.Text)

        Dt1.Dispose()
        DT2.Dispose()

        Me.Close()

    End Sub
    Private Sub ComboEmpresa_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEmpresa.Validating

        If IsNothing(ComboEmpresa.SelectedValue) Then ComboEmpresa.SelectedValue = 0

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Function HallaCodigoCliente(ByVal Cliente As Integer, ByVal Articulo As Integer, ByVal ConexionStr As String) As Double

        Dim Codigo As Double = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT CodigoDeCliente FROM CodigosCliente WHERE Cliente = " & Cliente & " AND Articulo = " & Articulo & ";", Miconexion)
                    If Not Decimal.TryParse(Cmd.ExecuteScalar(), Codigo) Then Return -1
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Codigos del Cliente.", MsgBoxStyle.Critical)
            Return -1
        End Try

    End Function
    Public Function NombreArticuloAsociado(ByVal Clave As Integer, ByVal ConexionStr As String) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        If Not Tablas.Read("SELECT Nombre FROM Articulos WHERE Clave = " & Clave & ";", ConexionStr, Dta) Then End
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta = Nothing
        Return Nombre

    End Function
    Private Function Valida() As Boolean

        If ComboEmpresa.SelectedValue = 0 Then
            MsgBox("Falta Informar Empresa.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Val(MaskedRemito.Text) = 0 Then
            MsgBox("Falta Informar Remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If

        If Not MaskedOK2(MaskedRemito) Then
            MsgBox("Numero Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedRemito.Focus()
            Return False
        End If


        Return True

    End Function

End Class