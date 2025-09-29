Imports System.Transactions
Public Class UnaActualizacionPedido
    Public PRemito As Decimal
    Public POperacion As Integer
    '
    Dim DtRemitoCabeza As DataTable
    Dim DtRemitoDetalle As DataTable
    Dim ConexionRemito As String
    Dim Cliente As Integer
    Dim PorCuentaYOrden As Integer
    Private Sub UnaActualizacionPedido_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If POperacion = 1 Then
            ConexionRemito = Conexion
        Else : ConexionRemito = ConexionN
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtRemitoCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Remito = " & PRemito & ";", ConexionRemito, DtRemitoCabeza) Then End

        DtRemitoDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosDetalle WHERE Remito = " & PRemito & ";", ConexionRemito, DtRemitoDetalle) Then End

        Cliente = DtRemitoCabeza.Rows(0).Item("Cliente")
        PorCuentaYOrden = DtRemitoCabeza.Rows(0).Item("PorCuentaYOrden")

        TextCliente.Text = NombreCliente(Cliente)
        TextPorCuentaYOrden.Text = NombreCliente(PorCuentaYOrden)

        If DtRemitoCabeza.Rows(0).Item("Pedido") <> 0 Then
            TextPedido.Text = DtRemitoCabeza.Rows(0).Item("Pedido")
            TextPedido.ReadOnly = True
            ButtonActualizar.Visible = False
            ButtonAnular.Visible = True
        Else
            TextPedido.ReadOnly = False
            ButtonActualizar.Visible = True
            ButtonAnular.Visible = False
        End If

        GModificacionOk = False

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub UnaActualizacionPedido_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub TextPedido_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextPedido.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ButtonActualizar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonActualizar.Click

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtPedidoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & Val(TextPedido.Text) & ";", Conexion, DtPedidoCabeza) Then End
        If DtPedidoCabeza.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido No Existe. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If DtPedidoCabeza.Rows(0).Item("Cliente") <> Cliente Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido No Corresponde para el Cliente del Remito. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If DtPedidoCabeza.Rows(0).Item("Cerrado") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido Esta Cerrado. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim PedidoCliente As String = DtPedidoCabeza.Rows(0).Item("PedidoCliente")

        DtPedidoCabeza.Dispose()

        Dim DtPedidoDetalle As New DataTable
        If Not ActualizaDetallePedido("A", "R", Val(TextPedido.Text), DtRemitoDetalle, DtPedidoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        Dim DtRemitoCabezaAux As DataTable = DtRemitoCabeza.Copy

        DtRemitoCabezaAux.Rows(0).Item("Pedido") = Val(TextPedido.Text)
        DtRemitoCabezaAux.Rows(0).Item("PedidoCliente") = PedidoCliente

        Dim Resul As Integer = ActualizaPedido(DtRemitoCabezaAux, DtPedidoDetalle)

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Actualización Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            UnaActualizacionPedido_Load(Nothing, Nothing)
        End If

        DtPedidoDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnular_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnular.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtPedidoCabeza As New DataTable
        If Not Tablas.Read("SELECT * FROM PedidosCabeza WHERE Pedido = " & Val(TextPedido.Text) & ";", Conexion, DtPedidoCabeza) Then End
        If DtPedidoCabeza.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido No Existe. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If DtPedidoCabeza.Rows(0).Item("Cerrado") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Pedido Esta Cerrado. Operación se CANCELA.", MsgBoxStyle.Information)
            Exit Sub
        End If

        DtPedidoCabeza.Dispose()

        Dim DtPedidoDetalle As New DataTable
        If Not ActualizaDetallePedido("B", "R", Val(TextPedido.Text), DtRemitoDetalle, DtPedidoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        Dim DtRemitoCabezaAux As DataTable = DtRemitoCabeza.Copy
        DtRemitoCabezaAux.Rows(0).Item("Pedido") = 0
        DtRemitoCabezaAux.Rows(0).Item("PedidoCliente") = ""

        Dim Resul As Integer = ActualizaPedido(DtRemitoCabezaAux, DtPedidoDetalle)

        If Resul < 0 Then
            MsgBox("ERROR: En base de datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Actualización Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            UnaActualizacionPedido_Load(Nothing, Nothing)
        End If

        DtPedidoDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function ActualizaPedido(ByVal DtRemitoCabezaAux As DataTable, ByVal DtPedidoDetalleAux As DataTable) As Integer

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                'Actualiza Remito.
                If Not IsNothing(DtRemitoCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitoCabezaAux.GetChanges, "RemitosCabeza", ConexionRemito)
                    If Resul <= 0 Then Return Resul
                End If

                'Actualiza Pedido.
                If Not IsNothing(DtPedidoDetalleAux.GetChanges) Then
                    Resul = GrabaTabla(DtPedidoDetalleAux.GetChanges, "PedidosDetalle", Conexion)
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
    Private Function Valida() As Boolean

        If TextPedido.Text = "" Then
            MsgBox("Debe Informar Pedido.", MsgBoxStyle.Information)
            Return False
        End If
        If CInt(TextPedido.Text) = 0 Then
            MsgBox("Debe Informar Pedido.", MsgBoxStyle.Information)
            Return False
        End If

        Return True

    End Function

   
End Class