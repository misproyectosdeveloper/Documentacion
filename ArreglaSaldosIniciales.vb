Imports System.Transactions
Imports System.Math
Public Class ArreglaSaldosIniciales

    Dim SQLFacturas As String = ""
    Dim SQLRemitos As String = ""
    Dim SQLIngresoMerca As String = ""

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim DTSaldosInicialesB As New DataTable
        If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Clave = 0;", Conexion, DTSaldosInicialesB) Then End
        Dim DTSaldosInicialesN As New DataTable
        If Not Tablas.Read("SELECT * FROM SaldosInicialesCabeza WHERE Clave = 0;", ConexionN, DTSaldosInicialesN) Then End


        Dim DTClientes As New DataTable
        If Not Tablas.Read("SELECT * FROM Clientes;", Conexion, DTClientes) Then End
        For Each Row As DataRow In DTClientes.Rows
            Dim SaldoInicialN = HallaSaldoN(Row("Clave"), 3)
            If SaldoInicialN <> 0 Then
                Alta(DTSaldosInicialesB, Row("Clave"), Row("SaldoInicial"), Row("Moneda"), Row("Cambio"), 3, True)
                Alta(DTSaldosInicialesN, Row("Clave"), SaldoInicialN, Row("Moneda"), Row("Cambio"), 3, True)
            End If
            If SaldoInicialN = 0 And Row("SaldoInicial") <> 0 Then
                Alta(DTSaldosInicialesB, Row("Clave"), Row("SaldoInicial"), Row("Moneda"), Row("Cambio"), 3, False)
            End If
        Next

        Dim DTProveedores As New DataTable
        If Not Tablas.Read("SELECT * FROM Proveedores;", Conexion, DTProveedores) Then End
        For Each Row As DataRow In DTProveedores.Rows
            Dim SaldoInicialN = HallaSaldoN(Row("Clave"), 2)
            If SaldoInicialN <> 0 Then
                Alta(DTSaldosInicialesB, Row("Clave"), Row("SaldoInicial"), Row("Moneda"), Row("Cambio"), 2, True)
                Alta(DTSaldosInicialesN, Row("Clave"), SaldoInicialN, Row("Moneda"), Row("Cambio"), 2, True)
            End If
            If SaldoInicialN = 0 And Row("SaldoInicial") <> 0 Then
                Alta(DTSaldosInicialesB, Row("Clave"), Row("SaldoInicial"), Row("Moneda"), Row("Cambio"), 2, False)
            End If
        Next

        DTClientes.Dispose()
        DTProveedores.Dispose()

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                '
                If Not IsNothing(DTSaldosInicialesB.GetChanges) Then
                    Resul = GrabaTabla(DTSaldosInicialesB.GetChanges, "SaldosInicialesCabeza", Conexion)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                If Not IsNothing(DTSaldosInicialesN.GetChanges) Then
                    Resul = GrabaTabla(DTSaldosInicialesN.GetChanges, "SaldosInicialesCabeza", ConexionN)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Termino")


    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim DtCabezaB As New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Valor <> 0 AND Estado <> 3;", Conexion, DtCabezaB) Then End
        Dim DtDetalleB As New DataTable
        For Each Row As DataRow In DtCabezaB.Rows
            Dim Sql As String = "SELECT * FROM RemitosDetalle WHERE Remito = " & Row("Remito") & " AND TipoPrecio = 2;"
            If Not Tablas.Read(Sql, Conexion, DtDetalleB) Then End
        Next
        ArreglaTablasRemitos(1, DtCabezaB, DtDetalleB)
        '
        Dim DtCabezaN As New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosCabeza WHERE Valor <> 0 AND Estado <> 3;", ConexionN, DtCabezaN) Then End
        Dim DtDetalleN As New DataTable
        For Each Row As DataRow In DtCabezaN.Rows
            Dim Sql As String = "SELECT * FROM RemitosDetalle WHERE Remito = " & Row("Remito") & " AND TipoPrecio = 2;"
            If Not Tablas.Read(Sql, ConexionN, DtDetalleN) Then End
        Next
        ArreglaTablasRemitos(2, DtCabezaN, DtDetalleN)

        Dim Resul As Integer = ActualizaArchivo(DtCabezaB, DtDetalleB, DtCabezaN, DtDetalleN)
        If Resul = 1000 Then
            MsgBox("Termino correctamente.")
        End If
        If Resul = -10 Then
            MsgBox("Error, no se pudo arreglar remitos.")
        End If

        DtCabezaB.Dispose()
        DtDetalleB.Dispose()
        DtCabezaN.Dispose()
        DtDetalleN.Dispose()

    End Sub
    Private Sub PasaFacturas_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub Alta(ByVal Dt As DataTable, ByVal Emisor As Integer, ByVal Importe As Double, ByVal Moneda As Integer, ByVal Cambio As Double, ByVal Tipo As Integer, ByVal Rel As Boolean)

        Dim Row1 As DataRow = Dt.NewRow
        Row1("Emisor") = Emisor
        Row1("Tipo") = Tipo
        Row1("Importe") = Importe
        Row1("Moneda") = Moneda
        Row1("Cambio") = Cambio
        Row1("Fecha") = "31/12/2012"
        Row1("Saldo") = Abs(Importe)
        Row1("Rel") = Rel
        Dt.Rows.Add(Row1)

    End Sub
    Private Function HallaSaldoN(ByVal Clave As Integer, ByVal Tipo As Integer) As Double

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT SaldoInicial From SaldosInicialesCtaCte WHERE Codigo = " & Tipo & " and Clave = " & Clave & ";", ConexionN, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            HallaSaldoN = Dt.Rows(0).Item("SaldoInicial")
        Else
            HallaSaldoN = 0
        End If

        Dt.Dispose()

    End Function
    Private Function esta(ByVal factura As Double, ByVal operacion As Integer) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Comprobante From ComprobantesOperacion WHERE TipoComprobante = 2 and Operacion = " & operacion & " AND comprobante = " & factura & ";", ConexionN, Dt) Then End
        If Dt.Rows.Count <> 0 Then
            esta = True
        Else
            esta = False
        End If

        Dt.Dispose()

    End Function
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT * FROM CodigosCliente;", Conexion, DT) Then End

        If DT.Rows.Count = 0 Then MsgBox("NO HAY DATOS EN LA TABLA: CodigosCliente!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Error!") : Exit Sub

        Dim DT2 As New DataTable
        If Not Tablas.Read("SELECT CodigoDeCliente FROM CodigosCliente WHERE CodigoDeCliente <> '';", Conexion, DT2) Then End
        If DT2.Rows.Count <> 0 Then
            MsgBox("CodigodeCliente Ya Existe") : Exit Sub
        End If

        For Each Row As DataRow In DT.Rows
            Dim Cod2 As String = Row("CodigoCliente").ToString
            Row("CodigoDeCliente") = Cod2
        Next

        Graba(DT, Conexion, "CodigosCliente")

    End Sub
    Private Sub ArreglaTablasRemitos(ByVal Operacion As Integer, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable)


        For Each Row As DataRow In DtDetalle.Rows
            Row("Precio") = Row("Precio") / Row("KilosXUnidad")
        Next
        'Modifico Valor en RemitosCabeza.
        For Each Row As DataRow In DtCabeza.Rows
            Row("Valor") = ArreglaValorRemito(Operacion, Row("Remito"))
        Next

    End Sub
    Private Function ArreglaValorRemito(ByVal Operacion As Integer, ByVal Remito As Decimal) As Decimal

        Dim Total As Decimal = 0
        Dim Precio As Decimal = 0
        Dim DtDetalleWWW As New DataTable
        Dim ConexionStr As String

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        Dim Sql As String = "SELECT * FROM RemitosDetalle WHERE Remito = " & Remito & ";"
        If Not Tablas.Read(Sql, ConexionStr, DtDetalleWWW) Then End

        For Each Row As DataRow In DtDetalleWWW.Rows
            If Operacion = 1 Then
                Precio = Row("Precio")
            Else
                Dim Iva As Decimal = HallaIva(Row("Articulo"))
                Precio = Trunca(Row("Precio") / (1 + Iva / 100))
            End If
            Total = Total + CalculaNeto(Row("Cantidad") - Row("Devueltas"), Precio)
        Next

        DtDetalleWWW.Dispose()
        Return Total

    End Function
    Private Function ActualizaArchivo(ByVal DtCabezaAux As DataTable, ByVal DtDetalleaux As DataTable, ByVal DtCabezaCerradoAux As DataTable, ByVal DtDetalleCerradoaux As DataTable) As Integer

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtCabezaAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaAux.GetChanges, "RemitosCabeza", Conexion)
                    If Resul <= 0 Then Exit Function
                End If
                '
                If Not IsNothing(DtDetalleaux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleaux.GetChanges, "RemitosDetalle", Conexion)
                    If Resul <= 0 Then Exit Function
                End If
                '
                If Not IsNothing(DtCabezaCerradoAux.GetChanges) Then
                    Resul = GrabaTabla(DtCabezaCerradoAux.GetChanges, "RemitosCabeza", ConexionN)
                    If Resul <= 0 Then Exit Function
                End If
                '
                If Not IsNothing(DtDetalleCerradoaux.GetChanges) Then
                    Resul = GrabaTabla(DtDetalleCerradoaux.GetChanges, "RemitosDetalle", ConexionN)
                    If Resul <= 0 Then Exit Function
                End If
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Return -10
        End Try

    End Function
    Private Sub ButtonArreglaPrecios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonArreglaPrecios.Click

        Dim DTPedidosDetalle As New DataTable
        Dim DTPedidosCabeza As New DataTable
        Dim DTListaPrecioDetalle As New DataTable

        Dim SQLPedidos As String = ""
        Dim SQLListaPrecio As String = ""

        ' ---------------------------------------------------- PEDIDOS -------------------------------------------------- '
        SQLPedidos = "SELECT PD.*,PorUnidad FROM PedidosDetalle PD, PedidosCabeza PC WHERE PD.Pedido = PC.Pedido"
        If Not Tablas.Read(SQLPedidos, Conexion, DTPedidosDetalle) Then End

        For Each FilaPD As DataRow In DTPedidosDetalle.Rows
            If FilaPD("Precio") <> 0 Then
                If FilaPD("PorUnidad") = False Then FilaPD("TipoPrecio") = 2
                If FilaPD("PorUnidad") = True Then FilaPD("TipoPrecio") = 1
            End If
        Next

        DTPedidosDetalle.Columns.RemoveAt(DTPedidosDetalle.Columns.Count - 1)

        Graba(DTPedidosDetalle, Conexion, "PedidosDetalle")

        ' ------------------------------------------------------------------------------------------------------------------------ '

        ' ---------------------------------------------------- LISTA DE PRECIOS -------------------------------------------------- '
        SQLListaPrecio = "SELECT LPD.*,PorUnidad FROM ListaDePreciosCabeza LPC, ListaDePreciosDetalle LPD WHERE LPC.Lista = LPD.Lista ORDER BY LPC.Lista"

        If Not Tablas.Read(SQLListaPrecio, Conexion, DTListaPrecioDetalle) Then End

        CambiaValores(DTListaPrecioDetalle)

        Graba(DTListaPrecioDetalle, Conexion, "ListaDePreciosDetalle")
        ' ------------------------------------------------------------------------------------------------------------------------ '

    End Sub
    Private Sub CambiaValores(ByRef Tabla As DataTable)

        For Each Fila As DataRow In Tabla.Rows
            If Fila("PorUnidad") = 0 Then Fila("TipoPrecio") = 2
            If Fila("PorUnidad") = 1 Then Fila("TipoPrecio") = 1

            If Fila("PorUnidad") = False Then Fila("TipoPrecio") = 2
            If Fila("PorUnidad") = True Then Fila("TipoPrecio") = 1

        Next

        Tabla.Columns.RemoveAt(Tabla.Columns.Count - 1)

    End Sub

    Private Sub ButtonFacturasB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFacturasB.Click

        Dim DTFacturasDetalle As New DataTable

        SQLFacturas = "SELECT FD.*,PorUnidad FROM FacturasDetalle FD, FacturasCabeza FC WHERE FD.Factura = FC.Factura"

        If Not Tablas.Read(SQLFacturas, Conexion, DTFacturasDetalle) Then End
        CambiaValores(DTFacturasDetalle)

        Graba(DTFacturasDetalle, Conexion, "FacturasDetalle")

    End Sub

    Private Sub ButtonFacturasN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFacturasN.Click

        Dim DTFacturasDetalleN As New DataTable

        SQLFacturas = "SELECT FD.*,PorUnidad FROM FacturasDetalle FD, FacturasCabeza FC WHERE FD.Factura = FC.Factura"

        If Not Tablas.Read(SQLFacturas, ConexionN, DTFacturasDetalleN) Then End
        CambiaValores(DTFacturasDetalleN)

        Graba(DTFacturasDetalleN, ConexionN, "FacturasDetalle")

    End Sub

    Private Sub ButtonRemitosB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemitosB.Click

        Dim DTRemitosDetalle As New DataTable

        SQLRemitos = "SELECT *,1 As PorUnidad FROM RemitosDetalle WHERE Precio <> 0"

        If Not Tablas.Read(SQLRemitos, Conexion, DTRemitosDetalle) Then End
        CambiaValores(DTRemitosDetalle)

        Graba(DTRemitosDetalle, Conexion, "RemitosDetalle")

    End Sub

    Private Sub ButtonRemitosN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemitosN.Click

        Dim DTRemitosDetalleN As New DataTable

        SQLRemitos = "SELECT *,1 As PorUnidad FROM RemitosDetalle WHERE Precio <> 0"

        If Not Tablas.Read(SQLRemitos, ConexionN, DTRemitosDetalleN) Then End
        CambiaValores(DTRemitosDetalleN)

        Graba(DTRemitosDetalleN, ConexionN, "RemitosDetalle")

    End Sub
    Private Sub GrabaWWWWWW(ByVal Tabla As DataTable, ByVal ConexionBase As String, ByVal NombreTabla As String)

        If IsNothing(Tabla.GetChanges) Then
            MsgBox("No hay Cambios.") : Exit Sub
        End If

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 35, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                ' ---------------------- TABLA ---------------------- '
                If Not IsNothing(Tabla.GetChanges) Then
                    Resul = GrabaTabla(Tabla.GetChanges, NombreTabla, ConexionBase)
                    If Resul <= 0 Then MsgBox("ERROR AL GRABAR SOBRE LA TABLA: " & NombreTabla & " ", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
                    MsgBox("TABLA: " & NombreTabla & " ACTUALIZADA CON EXITO!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "RESULTADO.")
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

    End Sub
    Private Sub Graba(ByVal Tabla As DataTable, ByVal ConexionBase As String, ByVal NombreTabla As String)

        If IsNothing(Tabla.GetChanges) Then
            MsgBox("No hay Cambios.") : Exit Sub
        End If

        Dim Resul As Double = 0

        If Not IsNothing(Tabla.GetChanges) Then
            Resul = GrabaTabla(Tabla.GetChanges, NombreTabla, ConexionBase)
            If Resul <= 0 Then MsgBox("ERROR AL GRABAR SOBRE LA TABLA: " & NombreTabla & " ", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
            MsgBox("TABLA: " & NombreTabla & " ACTUALIZADA CON EXITO!", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "RESULTADO.")
        End If

    End Sub
End Class