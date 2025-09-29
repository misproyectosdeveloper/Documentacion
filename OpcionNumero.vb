Public Class OpcionNumero
    Public PAnio As Integer
    Public PAnioAnt As Integer
    Public PTieneListaDePrecios As Boolean
    Public PEsPedido As Boolean
    Public PRegresar As Boolean
    Public PEsImportacionRemito As Boolean
    Public PEsNumeroConPuntoVenta As Boolean
    Public PAbierto As Boolean
    Public PRemito As Decimal
    Public PNumero As Decimal
    Public PConCantidad As Boolean
    Public PConPrecios As Boolean
    Private Sub OpcionNumero_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label1.Text = "INGRESE AÑO A PROCESAR"

        If Not PEsImportacionRemito And Not PEsNumeroConPuntoVenta Then
            If PAnioAnt <> 0 Then
                TextAnio.Text = PAnioAnt
            Else
                TextAnio.Text = Date.Now.Year
            End If
            Panel1.Visible = True
            Panel2.Visible = False
            Panel4.Visible = False
        End If

        If PEsImportacionRemito Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
            If Not PermisoTotal Then
                PictureCandado.Visible = False
            End If
            Panel1.Visible = False
            Panel4.Visible = False
            Panel2.Visible = True
            If PTieneListaDePrecios Then
                CheckPrecios.Visible = False
                CheckTodo.Visible = False
            End If
            If PEsPedido Then Panel3.Visible = False
        End If

        If PEsNumeroConPuntoVenta Then
            Panel1.Visible = False
            Panel4.Visible = True
            Panel2.Visible = False
            Panel4.Top = Panel1.Top
        End If


        PRegresar = True

    End Sub
    Private Sub PictureCandado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureCandado.Click

        If PAbierto Then
            PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
            PAbierto = False
        Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
            PAbierto = True
        End If

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If Not PEsImportacionRemito And Not PEsNumeroConPuntoVenta Then
            PAnio = CInt(TextAnio.Text)
        End If

        If PEsImportacionRemito Then
            PRemito = MaskedRemito.Text
            PConCantidad = CheckCantidad.Checked
            PConPrecios = CheckPrecios.Checked
            If CheckTodo.Checked Then PConCantidad = True : PConPrecios = True
        End If

        If PEsNumeroConPuntoVenta Then
            PNumero = MaskedNumero.Text
        End If

        PRegresar = False

        Me.Close()

    End Sub
    Private Function Valida() As Boolean

        If Not PEsImportacionRemito And Not PEsNumeroConPuntoVenta Then
            If TextAnio.Text = "" Then
                MsgBox("Debe Informar Año.", MsgBoxStyle.Information)
                TextAnio.Focus()
                Return False
            End If
            If CInt(TextAnio.Text) <= 2000 Then
                MsgBox("Fecha debe ser mayor a 2000.", MsgBoxStyle.Information)
                TextAnio.Focus()
                Return False
            End If
        End If

        If PEsImportacionRemito Then
            If Val(MaskedRemito.Text) = 0 Then
                MsgBox("Debe Informar Remito.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedRemito.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedRemito) Then
                MsgBox("Remito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedRemito.Focus()
                Return False
            End If
        End If

        If PEsNumeroConPuntoVenta Then
            If Val(MaskedNumero.Text) = 0 Then
                MsgBox("Debe Informar Numero.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNumero.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedNumero) Then
                MsgBox("Numero Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedNumero.Focus()
                Return False
            End If
        End If

        Return True

    End Function

End Class