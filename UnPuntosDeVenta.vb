Public Class UnPuntosDeVenta
    Private MiEnlazador As New BindingSource
    Public PBloqueaFunciones As Boolean
    '
    Dim DtPuntosDeVenta As DataTable
    Dim EsAlta As Boolean
    Dim PuntoDeVentaAnt As Integer
    Private Sub UnPuntosDeVenta_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        ComboPuntosDeVenta.DataSource = Tablas.Leer("SELECT Clave,RIGHT('0000' + CAST(Clave AS varchar),4) as Nombre FROM PuntosDeVenta WHERE Clave > 0 ORDER BY Nombre;")
        ComboPuntosDeVenta.DisplayMember = "Nombre"
        ComboPuntosDeVenta.ValueMember = "Clave"

        ComboPuntosDeVenta.Visible = True
        TextPuntosDeVenta.Visible = False
        TextPuntosDeVenta.Text = ""

        ArmaArchivoPuntoDeVenta(ComboPuntosDeVenta.SelectedValue)
        If PuntoDeVentaAnt <> 0 Then ComboPuntosDeVenta.SelectedValue = PuntoDeVentaAnt : ArmaArchivoPuntoDeVenta(ComboPuntosDeVenta.SelectedValue)

    End Sub
    Private Sub UnPuntosDeVenta_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        MiEnlazador.EndEdit()

        If Not IsNothing(DtPuntosDeVenta.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        MiEnlazador.EndEdit()

        GModificacionOk = False

        If IsNothing(DtPuntosDeVenta.GetChanges) Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PuntoDeVentaAnt = ComboPuntosDeVenta.SelectedValue

        If EsAlta Then
            If TextPuntosDeVenta.Text = "" Then
                MsgBox("Falta Informar numero Punto de Venta. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            If CInt(TextPuntosDeVenta.Text) = 0 Then
                MsgBox("Falta Informar numero Punto de Venta. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            Dim RowsBusqueda() As DataRow
            RowsBusqueda = ComboPuntosDeVenta.DataSource.select("Clave = " & CInt(TextPuntosDeVenta.Text))
            If RowsBusqueda.Length <> 0 Then
                MsgBox("Punto de Venta Ya Existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            DtPuntosDeVenta.Rows(0).Item("Clave") = CInt(TextPuntosDeVenta.Text)
            PuntoDeVentaAnt = CInt(TextPuntosDeVenta.Text)
        End If

        Dim Resul As Double

        Resul = ActualizaArchivos()
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            EsAlta = False
            UnPuntosDeVenta_Load(Nothing, Nothing)
        End If

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If EsAlta Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboPuntosDeVenta.SelectedValue = 0 Then
            MsgBox("Debe Informar Punto de venta. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If GUsaNegra And Not PermisoTotal Then
            MsgBox("Usuario No Autorizado(1000).")
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Usado(Conexion) Then
            MsgBox("Punto de Venta esta siendo Usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If
        If Usado(ConexionN) Then
            MsgBox("Punto de Venta esta siendo Usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If MsgBox("Punto de Venta se Borrara del Sistema. ¿Desea Borrarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtPuntosDeVenta.Rows(0).Delete()

        Dim Resul As Double

        Resul = ActualizaArchivos()

        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            EsAlta = False
            PuntoDeVentaAnt = 0
            UnPuntosDeVenta_Load(Nothing, Nothing)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboPuntosDeVenta_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboPuntosDeVenta.SelectionChangeCommitted

        MiEnlazador.EndEdit()

        If Not IsNothing(DtPuntosDeVenta.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
            End If
        End If

        ArmaArchivoPuntoDeVenta(ComboPuntosDeVenta.SelectedValue)

    End Sub
    Private Sub ButtonNuevoPunto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevoPunto.Click

        ComboPuntosDeVenta.Visible = False
        TextPuntosDeVenta.Visible = True
        TextPuntosDeVenta.Focus()
        EsAlta = True

        ArmaArchivoPuntoDeVenta(0)

    End Sub
    Private Sub TextFacturaA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFacturaA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFacturaB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFacturaB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFacturaC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFacturaC.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFacturaM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFacturaM.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextFacturaE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextFacturaE.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacionA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacionA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacionB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacionB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacionC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacionC.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacionM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacionM.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextLiquidacionE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextLiquidacionE.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasCreditoA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasCreditoA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasCreditoB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasCreditoB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasCreditoC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasCreditoC.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasCreditoM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasCreditoM.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasCreditoE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasCreditoE.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasDebitoA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasDebitoA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasDebitoB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasDebitoB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasDebitoC_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasDebitoC.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasDebitoM_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasDebitoM.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextNotasDebitoE_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNotasDebitoE.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextRemitos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextRemitos.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextDevolucion_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextDevolucion.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAIFacturaA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAIFacturaA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAIFacturaB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAIFacturaB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAINCreditoA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAINCreditoA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAINCreditoB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAINCreditoB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAINDebitoA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAINDebitoA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAINDebitoB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAINDebitoB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAILiquidacionA_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAILiquidacionA.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAILiquidacionB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAILiquidacionB.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCAIRemito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCAIRemito.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextPuntosDeVenta_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPuntosDeVenta.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCopiasRemitos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCopiasRemitos.KeyPress

        If InStr("123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCopiasFacturas_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCopiasFacturas.KeyPress

        If InStr("123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCopiasDebitosCreditos_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCopiasDebitosCreditos.KeyPress

        If InStr("123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextCopiasLiquidaciones_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextCopiasLiquidaciones.KeyPress

        If InStr("123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PictureAlmanaqueFacturaA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFacturaA.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaFacturaA.Text = ""
        Else : TextFechaFacturaA.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueFacturaB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueFacturaB.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaFacturaB.Text = ""
        Else : TextFechaFacturaB.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueNCreditoA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueNCreditoA.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNCreditoA.Text = ""
        Else : TextFechaNCreditoA.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueNCreditoB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueNCreditoB.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNCreditoB.Text = ""
        Else : TextFechaNCreditoB.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueNDebitoA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueNDebitoA.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNDebitoA.Text = ""
        Else : TextFechaNDebitoA.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueNDebitoB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueNDebitoB.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNDebitoB.Text = ""
        Else : TextFechaNDebitoB.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueLiquidacionA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueLiquidacionA.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaLiquidacionA.Text = ""
        Else : TextFechaLiquidacionA.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueLiquidacionB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueLiquidacionB.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaLiquidacionB.Text = ""
        Else : TextFechaLiquidacionB.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueRemito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueRemito.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaRemito.Text = ""
        Else : TextFechaRemito.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub TextFechaFacturaA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaFacturaA.Validating

        If TextFechaFacturaA.Text <> "" Then
            If Not ConsisteFecha(TextFechaFacturaA.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaFacturaA.Text = ""
                TextFechaFacturaA.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaFacturaB_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaFacturaB.Validating

        If TextFechaFacturaB.Text <> "" Then
            If Not ConsisteFecha(TextFechaFacturaB.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaFacturaB.Text = ""
                TextFechaFacturaB.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaNCreditoA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaNCreditoA.Validating

        If TextFechaNCreditoA.Text <> "" Then
            If Not ConsisteFecha(TextFechaNCreditoA.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaNCreditoA.Text = ""
                TextFechaNCreditoA.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaNCreditoB_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaNCreditoB.Validating

        If TextFechaNCreditoB.Text <> "" Then
            If Not ConsisteFecha(TextFechaNCreditoB.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaNCreditoB.Text = ""
                TextFechaNCreditoB.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaNDebitoA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaNDebitoA.Validating

        If TextFechaNDebitoA.Text <> "" Then
            If Not ConsisteFecha(TextFechaNDebitoA.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaNDebitoA.Text = ""
                TextFechaNDebitoA.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaNDebitoB_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaNDebitoB.Validating

        If TextFechaNDebitoB.Text <> "" Then
            If Not ConsisteFecha(TextFechaNDebitoB.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaNDebitoB.Text = ""
                TextFechaNDebitoB.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaLiquidacionA_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaLiquidacionA.Validating

        If TextFechaLiquidacionA.Text <> "" Then
            If Not ConsisteFecha(TextFechaLiquidacionA.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaLiquidacionA.Text = ""
                TextFechaLiquidacionA.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaLiquidacionB_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaLiquidacionB.Validating

        If TextFechaLiquidacionB.Text <> "" Then
            If Not ConsisteFecha(TextFechaLiquidacionB.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaLiquidacionB.Text = ""
                TextFechaLiquidacionB.Focus()
            End If
        End If

    End Sub
    Private Sub TextFechaRemito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles TextFechaRemito.Validating

        If TextFechaRemito.Text <> "" Then
            If Not ConsisteFecha(TextFechaRemito.Text) Then
                MsgBox("Fecha Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextFechaRemito.Text = ""
                TextFechaRemito.Focus()
            End If
        End If

    End Sub
    Private Sub ArmaArchivoPuntoDeVenta(ByVal Punto As Integer)

        DtPuntosDeVenta = New DataTable
        If Not Tablas.Read("SELECT * FROM PuntosDeVenta WHERE Clave = " & Punto & ";", Conexion, DtPuntosDeVenta) Then Me.Close() : Exit Sub
        If Punto = 0 Then
            Dim Row As DataRow = DtPuntosDeVenta.NewRow
            Row("Clave") = 0
            Row("Remitos") = 0
            Row("DevolucionMercaderia") = 0
            Row("FacturasA") = 0
            Row("FacturasB") = 0
            Row("FacturasC") = 0
            Row("FacturasM") = 0
            Row("FacturasE") = 0
            Row("LiquidacionA") = 0
            Row("LiquidacionB") = 0
            Row("LiquidacionC") = 0
            Row("LiquidacionM") = 0
            Row("LiquidacionE") = 0
            Row("NotasCreditoA") = 0
            Row("NotasCreditoB") = 0
            Row("NotasCreditoC") = 0
            Row("NotasCreditoM") = 0
            Row("NotasCreditoE") = 0
            Row("NotasDebitoA") = 0
            Row("NotasDebitoB") = 0
            Row("NotasDebitoC") = 0
            Row("NotasDebitoM") = 0
            Row("NotasDebitoE") = 0
            Row("EsReciboManual") = False
            Row("Comentario") = ""
            Row("CAIFacturaA") = 0
            Row("FechaFacturaA") = 0
            Row("CAIFacturaB") = 0
            Row("FechaFacturaB") = 0
            Row("CAINCreditoA") = 0
            Row("FechaNCreditoA") = 0
            Row("CAINCreditoB") = 0
            Row("FechaNCreditoB") = 0
            Row("CAINDebitoA") = 0
            Row("FechaNDebitoA") = 0
            Row("CAINDebitoB") = 0
            Row("FechaNDebitoB") = 0
            Row("CAILiquidacionA") = 0
            Row("FechaLiquidacionA") = 0
            Row("CAILiquidacionB") = 0
            Row("FechaLiquidacionB") = 0
            Row("CAIRemito") = 0
            Row("FechaRemito") = 0
            Row("EsRemitoManual") = False
            Row("FacturasElectronicas") = False
            Row("EsZ") = False
            Row("EsFCE") = False
            Row("EsRemitoAutoImpreso") = False
            Row("CopiasRemitos") = 0
            Row("CopiasFacturas") = 0
            Row("CopiasDebitosCreditos") = 0
            Row("CopiasLiquidaciones") = 0
            DtPuntosDeVenta.Rows.Add(Row)
        End If

        EnlazaPuntosDeVenta()

        ' esta instruccion va debajo de EnlazaPuntosDeVenta. para que se enlze cada ves que se lee.
        AddHandler DtPuntosDeVenta.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtPuntosDeVenta_ColumnChanging)

    End Sub
    Private Sub EnlazaPuntosDeVenta()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtPuntosDeVenta

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Remitos")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextRemitos.DataBindings.Clear()
        TextRemitos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "DevolucionMercaderia")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextDevolucion.DataBindings.Clear()
        TextDevolucion.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FacturasA")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextFacturaA.DataBindings.Clear()
        TextFacturaA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FacturasB")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextFacturaB.DataBindings.Clear()
        TextFacturaB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FacturasC")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextFacturaC.DataBindings.Clear()
        TextFacturaC.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FacturasM")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextFacturaM.DataBindings.Clear()
        TextFacturaM.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FacturasE")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextFacturaE.DataBindings.Clear()
        TextFacturaE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LiquidacionA")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextLiquidacionA.DataBindings.Clear()
        TextLiquidacionA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LiquidacionB")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextLiquidacionB.DataBindings.Clear()
        TextLiquidacionB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LiquidacionC")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextLiquidacionC.DataBindings.Clear()
        TextLiquidacionC.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LiquidacionM")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextLiquidacionM.DataBindings.Clear()
        TextLiquidacionM.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "LiquidacionE")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextLiquidacionE.DataBindings.Clear()
        TextLiquidacionE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasCreditoA")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasCreditoA.DataBindings.Clear()
        TextNotasCreditoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasCreditoB")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasCreditoB.DataBindings.Clear()
        TextNotasCreditoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasCreditoC")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasCreditoC.DataBindings.Clear()
        TextNotasCreditoC.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasCreditoM")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasCreditoM.DataBindings.Clear()
        TextNotasCreditoM.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasCreditoE")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasCreditoE.DataBindings.Clear()
        TextNotasCreditoE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasDebitoA")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasDebitoA.DataBindings.Clear()
        TextNotasDebitoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasDebitoB")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasDebitoB.DataBindings.Clear()
        TextNotasDebitoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasDebitoC")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasDebitoC.DataBindings.Clear()
        TextNotasDebitoC.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasDebitoM")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasDebitoM.DataBindings.Clear()
        TextNotasDebitoM.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "NotasDebitoE")
        AddHandler Enlace.Format, AddressOf FormatNumeracion
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextNotasDebitoE.DataBindings.Clear()
        TextNotasDebitoE.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Format, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsReciboManual")
        CheckReciboManual.DataBindings.Clear()
        CheckReciboManual.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAIFacturaA")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAIFacturaA.DataBindings.Clear()
        TextCAIFacturaA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaFacturaA")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaFacturaA.DataBindings.Clear()
        TextFechaFacturaA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAIFacturaB")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAIFacturaB.DataBindings.Clear()
        TextCAIFacturaB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaFacturaB")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaFacturaB.DataBindings.Clear()
        TextFechaFacturaB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAINCreditoA")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAINCreditoA.DataBindings.Clear()
        TextCAINCreditoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaNCreditoA")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaNCreditoA.DataBindings.Clear()
        TextFechaNCreditoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAINCreditoB")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAINCreditoB.DataBindings.Clear()
        TextCAINCreditoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaNCreditoB")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaNCreditoB.DataBindings.Clear()
        TextFechaNCreditoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAINDebitoA")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAINDebitoA.DataBindings.Clear()
        TextCAINDebitoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaNDebitoA")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaNDebitoA.DataBindings.Clear()
        TextFechaNDebitoA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAINDebitoB")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAINDebitoB.DataBindings.Clear()
        TextCAINDebitoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaNDebitoB")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaNDebitoB.DataBindings.Clear()
        TextFechaNDebitoB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAILiquidacionA")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAILiquidacionA.DataBindings.Clear()
        TextCAILiquidacionA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaLiquidacionA")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaLiquidacionA.DataBindings.Clear()
        TextFechaLiquidacionA.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAILiquidacionB")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAILiquidacionB.DataBindings.Clear()
        TextCAILiquidacionB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaLiquidacionB")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaLiquidacionB.DataBindings.Clear()
        TextFechaLiquidacionB.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CAIRemito")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCAIRemito.DataBindings.Clear()
        TextCAIRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "FechaRemito")
        AddHandler Enlace.Format, AddressOf FormatFecha
        AddHandler Enlace.Parse, AddressOf ParseFecha
        TextFechaRemito.DataBindings.Clear()
        TextFechaRemito.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsRemitoManual")
        CheckRemitoManual.DataBindings.Clear()
        CheckRemitoManual.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "FacturasElectronicas")
        CheckFacturasElectronicas.DataBindings.Clear()
        CheckFacturasElectronicas.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsZ")
        CheckFacturaZ.DataBindings.Clear()
        CheckFacturaZ.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsFCE")
        CheckEsFce.DataBindings.Clear()
        CheckEsFce.DataBindings.Add(Enlace)

        Enlace = New Binding("Checked", MiEnlazador, "EsRemitoAutoImpreso")
        CheckEsRemitoAutoimpreso.DataBindings.Clear()
        CheckEsRemitoAutoimpreso.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CopiasRemitos")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCopiasRemitos.DataBindings.Clear()
        TextCopiasRemitos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CopiasFacturas")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCopiasFacturas.DataBindings.Clear()
        TextCopiasFacturas.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CopiasDebitosCreditos")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCopiasDebitosCreditos.DataBindings.Clear()
        TextCopiasDebitosCreditos.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "CopiasLiquidaciones")
        AddHandler Enlace.Format, AddressOf FormatCAI
        AddHandler Enlace.Parse, AddressOf ParseNumeracion
        TextCopiasLiquidaciones.DataBindings.Clear()
        TextCopiasLiquidaciones.DataBindings.Add(Enlace)

    End Sub
    Private Sub FormatCAI(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatNumeracion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, 0)

    End Sub
    Private Sub ParseNumeracion(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Sub FormatFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = ConvierteNumeroAFecha(Numero.Value)

    End Sub
    Private Sub ParseFecha(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = ConvierteFechaANumero(Numero.Value)

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function ConvierteNumeroAFecha(ByVal Numero As Integer) As String

        If Numero.ToString.Length < 8 Then Return ""

        Dim Dia As String = Format(CInt(Mid$(Numero, 7, 2)), "00")
        Dim Mes As String = Format(CInt(Mid$(Numero, 5, 2)), "00")
        Dim Anio As String = Format(CInt(Mid$(Numero, 1, 4)), "0000")

        Return Dia & "/" & Mes & "/" & Anio

    End Function
    Private Function ConvierteFechaANumero(ByVal Fecha As String) As Integer

        If Fecha = "" Then Return 0

        Dim FechaStr As Date = CDate(Fecha)

        Dim Dia As String = Format(FechaStr.Day, "00")
        Dim Mes As String = Format(FechaStr.Month, "00")
        Dim Anio As String = Format(FechaStr.Year, "0000")

        Return Val(Anio & Mes & Dia)

    End Function
    Private Function ActualizaArchivos() As Double

        Dim Mensaje As String = Tablas.GrabarOleDb(DtPuntosDeVenta, "SELECT * FROM PuntosDeVenta;", Conexion)
        If Not GModificacionOk Then
            MsgBox(Mensaje, MsgBoxStyle.Critical)
        Else
            Return 1000
        End If

    End Function
    Private Function NumeracionFactura(ByVal Letra As String) As Double

        Dim Patron As String = HallaNumeroLetra(Letra) & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return Ultimo
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function NumeracionNotasCredito(ByVal Letra As String) As Double

        Dim NumeracionNotaCredito As Double
        Dim NumeracionNotaFinancera As Double

        Dim Patron As String = HallaNumeroLetra(Letra) & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(NotaCredito) FROM NotasCreditoCabeza WHERE CAST(CAST(NotasCreditoCabeza.NotaCredito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        NumeracionNotaCredito = CDbl(Ultimo)
                    Else : NumeracionNotaCredito = 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try
        'para tiponotas financieras 7 y 8. 
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecibosCabeza WHERE (TipoNota = 7 or TipoNota = 8) AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        NumeracionNotaFinancera = CDbl(Ultimo)
                    Else : NumeracionNotaFinancera = 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

        If NumeracionNotaCredito >= NumeracionNotaFinancera Then
            Return NumeracionNotaCredito
        Else
            Return NumeracionNotaFinancera
        End If

    End Function
    Private Function NumeracionNotasDebito(ByVal Letra As String) As Double

        'para tiponotas financieras 5 y 6. 

        Dim Patron As String = HallaNumeroLetra(Letra) & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Nota) FROM RecibosCabeza WHERE (TipoNota = 5 or TipoNota = 6) AND CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function NumeracionRemitos() As Double

        'para tiponotas financieras 5 y 6. 

        Dim Patron As String = ComboPuntosDeVenta.SelectedValue & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Remito) FROM RemitosCabeza WHERE CAST(CAST(Remito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo)
                    Else : Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Usado(ByVal ConexionStr) As Boolean

        Dim Patron As String = "[0-9]" & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM FacturasCabeza WHERE CAST(CAST(FacturasCabeza.Factura AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos Al Leer Tabla Facturas Cabeza.", MsgBoxStyle.Critical)
            End
        End Try

        Patron = ComboPuntosDeVenta.SelectedValue & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Remito) FROM RemitosCabeza WHERE CAST(CAST(RemitosCabeza.Remito AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos Al Leer Tabla Remitos Cabeza.", MsgBoxStyle.Critical)
            End
        End Try

        Patron = "[0-9]" & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Liquidacion) FROM LiquidacionCabeza WHERE CAST(CAST(LiquidacionCabeza.Liquidacion AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos Al Leer Tabla Liquidacion Cabeza.", MsgBoxStyle.Critical)
            End
        End Try

        Patron = "[0-9]" & Format(ComboPuntosDeVenta.SelectedValue, "0000") & "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]"
        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Nota) FROM RecibosCabeza WHERE CAST(CAST(RecibosCabeza.Nota AS numeric) as char)LIKE '" & Patron & "';", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos Al Leer Tabla Recibos Cabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        Dim TipoError As Integer

        If Not ConsisteCAIyFecha(TextCAIFacturaA.Text, TextFechaFacturaA.Text, TipoError) Then
            If TipoError = 1 Then TextCAIFacturaA.Focus()
            If TipoError = 2 Then TextFechaFacturaA.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAIFacturaB.Text, TextFechaFacturaB.Text, TipoError) Then
            If TipoError = 1 Then TextCAIFacturaB.Focus()
            If TipoError = 2 Then TextFechaFacturaB.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAINCreditoA.Text, TextFechaNCreditoA.Text, TipoError) Then
            If TipoError = 1 Then TextFechaNCreditoA.Focus()
            If TipoError = 2 Then TextFechaNCreditoA.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAINCreditoB.Text, TextFechaNCreditoB.Text, TipoError) Then
            If TipoError = 1 Then TextFechaNCreditoB.Focus()
            If TipoError = 2 Then TextFechaNCreditoB.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAINDebitoA.Text, TextFechaNDebitoA.Text, TipoError) Then
            If TipoError = 1 Then TextFechaNDebitoA.Focus()
            If TipoError = 2 Then TextFechaNDebitoA.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAINDebitoB.Text, TextFechaNDebitoB.Text, TipoError) Then
            If TipoError = 1 Then TextFechaNDebitoB.Focus()
            If TipoError = 2 Then TextFechaNDebitoB.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAILiquidacionA.Text, TextFechaLiquidacionA.Text, TipoError) Then
            If TipoError = 1 Then TextFechaLiquidacionA.Focus()
            If TipoError = 2 Then TextFechaLiquidacionA.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAILiquidacionB.Text, TextFechaLiquidacionB.Text, TipoError) Then
            If TipoError = 1 Then TextFechaLiquidacionB.Focus()
            If TipoError = 2 Then TextFechaLiquidacionB.Focus()
            Return False
        End If

        If Not ConsisteCAIyFecha(TextCAIRemito.Text, TextFechaRemito.Text, TipoError) Then
            If TipoError = 1 Then TextFechaRemito.Focus()
            If TipoError = 2 Then TextFechaRemito.Focus()
            Return False
        End If

        Dim Conta As Integer = 0
        If CheckFacturaZ.Checked Then Conta = Conta + 1
        If CheckEsRemitoAutoimpreso.Checked Then Conta = Conta + 1
        If CheckFacturasElectronicas.Checked Then Conta = Conta + 1
        If Conta > 1 Then
            MsgBox("Para Remitos de Auto-Impresión o Factura Z o Factura Electrónica Punto de Venta debe ser EXCLUSIVO.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Conta = 0
        If CheckFacturaZ.Checked Then Conta = Conta + 1
        If CheckEsRemitoAutoimpreso.Checked Then Conta = Conta + 1
        If CheckEsFce.Checked Then Conta = Conta + 1
        If Conta > 1 Then
            MsgBox("Para Remitos de Auto-Impresión o Factura Z o MiPyMEs(FCE) Punto de Venta debe ser EXLUSIVO.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If CheckEsRemitoAutoimpreso.Checked And CheckRemitoManual.Checked Then
            MsgBox("Remito Manual y Remito Auto-Impresión son Exluyentes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CheckFacturaZ.Checked And CheckFacturasElectronicas.Checked Then
            MsgBox("Punto de Venta no debe ser para Facturas Z Y Facturas Electrónica Simultanieamente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CheckFacturaZ.Checked Then
            If Val(TextFacturaA.Text) <> 0 Or Val(TextFacturaB.Text) <> 0 Or Val(TextFacturaC.Text) <> 0 Or Val(TextFacturaM.Text) Or Val(TextFacturaE.Text) <> 0 Then
                MsgBox("No Debe Informar Numeración en Facturas para Punto de Venta Factura Z.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If CheckEsRemitoAutoimpreso.Checked Then
            If Val(TextCAIRemito.Text) = 0 Or Val(TextFechaRemito.Text) = 0 Then
                MsgBox("Debe Informar CAI y Fecha Vencimiento para Remitos Auto-Impresos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If CheckEsFce.Checked Then
            If Not CheckFacturasElectronicas.Checked Then
                MsgBox("FCE debe estar adherido a Facura Electrónica.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If TextCopiasRemitos.Text = "" Then
            MsgBox("Debe Informar Copias en Remitos para Impresión.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextCopiasFacturas.Text = "" Then
            MsgBox("Debe Informar Copias en Facturas para Impresión.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextCopiasDebitosCreditos.Text = "" Then
            MsgBox("Debe Informar Copias de Notas Débitos/Créditos para Impresión.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If TextCopiasLiquidaciones.Text = "" Then
            MsgBox("Debe Informar Copias en Liquidaciones para Impresión.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    Private Function ConsisteCAIyFecha(ByVal Cai As String, ByVal Fecha As String, ByRef TipoError As Integer) As Boolean

        If Fecha <> "" And Cai = "" Then
            MsgBox("Falta CAI.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TipoError = 1
            Return False
        End If

        If Fecha = "" And Cai <> "" Then
            MsgBox("Falta Vencimiento.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TipoError = 2
            Return False
        End If

        TipoError = 0
        Return True

    End Function
    Private Sub DtPuntosDeVenta_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.SetColumnError(e.Column, "")

        If e.Column.ColumnName.Equals("FacturasA") Then
            If Not ValidaFactura("A", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("FacturasA")
                TextFacturaA.Text = e.Row("FacturasA")
                TextFacturaA.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("FacturasB") Then
            If Not ValidaFactura("B", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("FacturasB")
                TextFacturaB.Text = e.Row("FacturasB")
                TextFacturaB.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("FacturasC") Then
            If Not ValidaFactura("C", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("FacturasC")
                TextFacturaC.Text = e.Row("FacturasC")
                TextFacturaC.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("FacturasM") Then
            If Not ValidaFactura("M", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("FacturasM")
                TextFacturaM.Text = e.Row("FacturasM")
                TextFacturaM.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("FacturasE") Then
            '   Dim Resul As Double = NumeracionFactura("E")
            '    If Resul > CDbl(HallaNumeroLetra("E") & Format(ComboPuntosDeVenta.SelectedValue, "0000") & Format(e.ProposedValue, "00000000")) Then
            ' MsgBox("Existe Numeración Superior a la Informada.")
            ' e.ProposedValue = e.Row("FacturasE")
            ' TextFacturaE.Focus()
            'End If
            'If Resul < 0 Then
            '  MsgBox("Error Base de Datos.")
            '  e.ProposedValue = e.Row("FacturasE")
            '  TextFacturaE.Focus()
            'End If
        End If
        '
        If e.Column.ColumnName.Equals("NotasCreditoA") Then
            If Not ValidaNotaCredito("A", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasCreditoA")
                TextNotasCreditoA.Text = e.Row("NotasCreditoA")
                TextNotasCreditoA.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasCreditoB") Then
            If Not ValidaNotaCredito("B", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasCreditoB")
                TextNotasCreditoB.Text = e.Row("NotasCreditoB")
                TextNotasCreditoB.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasCreditoC") Then
            If Not ValidaNotaCredito("C", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasCreditoC")
                TextNotasCreditoC.Text = e.Row("NotasCreditoC")
                TextNotasCreditoC.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasCreditoM") Then
            If Not ValidaNotaCredito("M", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasCreditoM")
                TextNotasCreditoM.Text = e.Row("NotasCreditoM")
                TextNotasCreditoM.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasCreditoE") Then
            '    Dim Resul As Double = NumeracionNotasCredito("E")
            '   If Resul > CDbl(HallaNumeroLetra("E") & Format(ComboPuntosDeVenta.SelectedValue, "0000") & Format(e.ProposedValue, "00000000")) Then
            'MsgBox("Existe Numeración Superior a la Informada.")
            ' e.ProposedValue = e.Row("NotasCreditoE")
            ' TextNotasCreditoE.Text = e.Row("NotasCreditoE")
            ' TextNotasCreditoE.Focus()
            ' End If
            'If Resul < 0 Then
            'MsgBox("Error Base de Datos.")
            'e.ProposedValue = e.Row("NotasCreditoE")
            'TextNotasCreditoE.Text = e.Row("NotasCreditoE")
            'TextNotasCreditoE.Focus()
            ' End If
        End If
        '
        If e.Column.ColumnName.Equals("NotasDebitoA") Then
            If Not ValidaNotaDebito("A", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasDebitoA")
                TextNotasDebitoA.Text = e.Row("NotasDebitoA")
                TextNotasDebitoA.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasDebitoB") Then
            If Not ValidaNotaDebito("B", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasDebitoB")
                TextNotasDebitoB.Text = e.Row("NotasDebitoB")
                TextNotasDebitoB.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasDebitoC") Then
            If Not ValidaNotaDebito("C", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasDebitoC")
                TextNotasDebitoC.Text = e.Row("NotasDebitoC")
                TextNotasDebitoC.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasDebitoM") Then
            If Not ValidaNotaDebito("M", ComboPuntosDeVenta.SelectedValue, e.ProposedValue) Then
                e.ProposedValue = e.Row("NotasDebitoM")
                TextNotasDebitoM.Text = e.Row("NotasDebitoM")
                TextNotasDebitoM.Focus()
            End If
        End If
        If e.Column.ColumnName.Equals("NotasDebitoE") Then
            '     Dim Resul As Double = NumeracionNotasDebito("E")
            '    If Resul >= CDbl(HallaNumeroLetra("E") & (ComboPuntosDeVenta.SelectedValue, "0000") & Format(e.ProposedValue, "00000000")) Then
            'MsgBox("Existe Numeración Superior a la Informada.")
            '   e.ProposedValue = e.Row("NotasDebitoE")
            '   TextNotasDebitoE.Text = e.Row("NotasDebitoE")
            '   TextNotasDebitoE.Focus()
            'End If
            'If Resul < 0 Then
            ' MsgBox("Error Base de Datos.")
            ' e.ProposedValue = e.Row("NotasDebitoE")
            ' TextNotasDebitoE.Text = e.Row("NotasDebitoE")
            ' TextNotasDebitoE.Focus()
            'End If
        End If
        '
        If e.Column.ColumnName.Equals("Remitos") Then
            Dim Resul As Double = NumeracionRemitos()
            If Resul > CDbl(ComboPuntosDeVenta.SelectedValue & Format(e.ProposedValue, "00000000")) Then
                MsgBox("Existe Numeración Superior a la Informada.")
                e.ProposedValue = e.Row("Remitos")
                TextRemitos.Text = e.Row("Remitos")
                TextRemitos.Focus()
            End If
            If Resul < 0 Then
                MsgBox("Error Base de Datos.")
                e.ProposedValue = e.Row("Remitos")
                TextRemitos.Text = e.Row("Remitos")
                TextRemitos.Focus()
            End If
        End If

    End Sub
    Private Function ValidaFactura(ByVal Letra As String, ByVal PuntoDeVenta As Integer, ByVal Valor As Integer) As Boolean

        Dim Resul As Double = NumeracionFactura(Letra)
        If Resul > CDbl(HallaNumeroLetra(Letra) & Format(PuntoDeVenta, "0000") & Format(Valor, "00000000")) Then
            MsgBox("Existe Numeración Superior a la Informada.")
            Return False
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos.")
            Return False
        End If

        Return True

    End Function
    Private Function ValidaNotaDebito(ByVal Letra As String, ByVal PuntoDeVenta As Integer, ByVal Valor As Integer) As Boolean

        Dim Resul As Double = NumeracionNotasDebito(Letra)
        If Resul > CDbl(HallaNumeroLetra(Letra) & Format(PuntoDeVenta, "0000") & Format(Valor, "00000000")) Then
            MsgBox("Existe Numeración Superior a la Informada.")
            Return False
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos.")
            Return False
        End If

        Return True

    End Function
    Private Function ValidaNotaCredito(ByVal Letra As String, ByVal PuntoDeVenta As Integer, ByVal Valor As Integer) As Boolean

        Dim Resul As Double = NumeracionNotasCredito(Letra)
        If Resul > CDbl(HallaNumeroLetra(Letra) & Format(PuntoDeVenta, "0000") & Format(Valor, "00000000")) Then
            MsgBox("Existe Numeración Superior a la Informada.")
            Return False
        End If
        If Resul < 0 Then
            MsgBox("Error Base de Datos.")
            Return False
        End If

        Return True

    End Function
  
   
End Class