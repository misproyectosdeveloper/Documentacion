Imports System.Transactions
Public Class UnaReAsignacionFactura
    Public PFactura As Double
    Public PAbierto As Boolean
    Public PtipoAsiento As Integer
    Public PtipoAsientoCosto As Integer
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '   
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtAsignacionLotes As DataTable
    '
    Dim ListaDeLotes As New List(Of FilaAsignacion)
    Dim ListaDeLotesOriginal As New List(Of FilaAsignacion)
    ' 
    Private WithEvents bs As New BindingSource
    ' 
    Dim Relacionada As Double
    Dim ConexionFactura As String
    Dim ConexionRelacionada As String

    'PMERCADO 10-06-2025
    Dim Carga_Grilla As Boolean

    Private Sub UnaReAsignacionFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'PMERCADO 10-06-2025
    End Sub
    Private Sub UnaReAsignacionFactura_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not ListasIguales() And Not GModificacionOk Then
            If MsgBox("Cambios no han sido grabados. Desesa Cerrar el programa de todos modos (S/N)", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
                e.Cancel = True
            End If
        End If

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        If ListasIguales() Then
            MsgBox("No hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtCabezaRel As New DataTable
        Dim DtDetalleRel As New DataTable
        Dim DtLotesRel As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtDetalleRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & Relacionada & ";", ConexionRelacionada, DtLotesRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
        End If

        Dim DtFacturaCabezaB As New DataTable
        Dim DtFacturaDetalleB As New DataTable
        Dim DtAsignacionLotesB As New DataTable
        Dim DtFacturaCabezaN As New DataTable
        Dim DtFacturaDetalleN As New DataTable
        Dim DtAsignacionLotesN As New DataTable

        If PAbierto Then
            DtFacturaCabezaB = DtCabeza.Copy
            DtFacturaDetalleB = DtDetalle.Copy
            DtAsignacionLotesB = DtAsignacionLotes.Copy
            DtFacturaCabezaN = DtCabezaRel.Copy
            DtFacturaDetalleN = DtDetalleRel.Copy
            DtAsignacionLotesN = DtLotesRel.Copy
        Else
            DtFacturaCabezaB = DtCabezaRel.Copy
            DtFacturaDetalleB = DtDetalleRel.Copy
            DtAsignacionLotesB = DtLotesRel.Copy
            DtFacturaCabezaN = DtCabeza.Copy
            DtFacturaDetalleN = DtDetalle.Copy
            DtAsignacionLotesN = DtAsignacionLotes.Copy
        End If

        'Actualiza Asignacion Lotes.
        If Not ArmaAsignacionFactura(2, DtFacturaCabezaB, DtFacturaDetalleB, DtFacturaCabezaN, DtFacturaDetalleN, _
                                          DtAsignacionLotesB, DtAsignacionLotesN, ListaDeLotes, PPaseDeProyectos) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        'Actualiza Stock.
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotes, PPaseDeProyectos) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        'Arma asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        'Para Costo.
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            If Not HallaAsiento(PtipoAsiento, DtFacturaCabezaB.Rows(0).Item("Factura"), DtAsientoCabezaB, DtAsientoDetalleB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not HallaAsiento(PtipoAsientoCosto, DtFacturaCabezaB.Rows(0).Item("Factura"), DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            If Not HallaAsiento(PtipoAsiento, DtFacturaCabezaN.Rows(0).Item("Factura"), DtAsientoCabezaN, DtAsientoDetalleN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not HallaAsiento(PtipoAsientoCosto, DtFacturaCabezaN.Rows(0).Item("Factura"), DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        Dim Funcion As String
        If DtAsignacionLotes.Rows.Count = 0 Then
            Funcion = "A"
        Else
            Funcion = "M"
        End If

        If DtAsientoCabezaB.Rows.Count <> 0 Or DtAsientoCabezaN.Rows.Count <> 0 Then
            If Not ArmaAsientosFacturas(Funcion, PtipoAsiento, DtFacturaCabezaB, DtFacturaCabezaN, DtFacturaDetalleB, DtFacturaDetalleN, ListaDeLotes, _
                                     DtAsignacionLotes, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, 0, 0, PPaseDeProyectos, "01/01/1800") Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If
        If DtAsientoCabezaCostoB.Rows.Count <> 0 Or DtAsientoCabezaCostoN.Rows.Count <> 0 Then
            If Not ArmaAsientosCostosFacturas(Funcion, PtipoAsientoCosto, DtFacturaCabezaB, DtFacturaCabezaN, DtFacturaDetalleB, DtFacturaDetalleN, ListaDeLotes, _
                          DtAsignacionLotes, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, PPaseDeProyectos, "01/01/1800") Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            If DtFacturaCabezaB.Rows(0).Item("Estado") = 2 Then DtFacturaCabezaB.Rows(0).Item("Estado") = 1
        End If
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            If DtFacturaCabezaN.Rows(0).Item("Estado") = 2 Then DtFacturaCabezaN.Rows(0).Item("Estado") = 1
        End If

        Dim DtBasio As New DataTable

        Dim NumeroW As Double = ActualizaAsignacion("M", DtFacturaCabezaB, DtFacturaCabezaN, DtAsignacionLotesB, DtAsignacionLotesN, _
                     DtAsientoDetalleB, DtAsientoDetalleN, DtAsientoDetalleCostoB, DtAsientoDetalleCostoN, DtStockB, DtStockN, DtBasio, DtBasio, DtBasio, DtBasio)

        If NumeroW < 0 Then
            MsgBox("ERROR: En Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonDevolverTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDevolverTodos.Click

        ButtonAsignacionManual.PerformClick()


        If DtCabeza.Rows(0).Item("Estado") <> 1 Then
            MsgBox("No Existe Asignaciones.")
            Exit Sub
        End If

        If Not ListasIguales() Then
            MsgBox("Exite Cambios en Asignaciones. Operación se CANCELA.")
            Exit Sub
        End If

        Dim DtCabezaRel As New DataTable
        Dim DtDetalleRel As New DataTable
        Dim DtLotesRel As New DataTable

        'No permito devolver lo que falta de la asignacion. Si dejara la devolucion, al dar de baja la factura debo de dar de baja la nota de credito
        'y esto disminuye el stock incorrectamente. 
        '''''''''       If HallaNotasCreditoLoteadas(PFactura, ConexionFactura) > 0 Then
        '''''''''''''''MsgBox("Existe Notas De Credito, Debe Anularlas Previamente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        '''''''''''''      Exit Sub
        '''''''''''''''    End If

        'arreglo---------------------------------------------------------------------
        'Halla Notas de Credito loteadas.
        Dim DtNotasCredito As New DataTable
        Dim DtNotasCreditoRel As New DataTable
        HallaNotasCreditoConLotes(DtNotasCredito, PFactura, ConexionFactura)         'halla devoluciones loteadas para Borrar sus AsignacionesLotes.
        HallaNotasCreditoConLotes(DtNotasCreditoRel, Relacionada, ConexionRelacionada)

        'Halla Asignacion de lotes de las notas de credito.
        Dim DtAsignacionNC As New DataTable
        Dim DtAsignacionNCRel As New DataTable
        If DtNotasCredito.Rows.Count <> 0 Then
            For Each Row As DataRow In DtNotasCredito.Rows   'halla AsignacionLotes de las devoluciones para borrarlas.
                Dim Sql As String = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 4 AND Comprobante = " & Row("NotaCredito") & ";"
                If Not Tablas.Read(Sql, ConexionFactura, DtAsignacionNC) Then Exit Sub
                Row("Estado") = 2
            Next
            For Each Row As DataRow In DtAsignacionNC.Rows
                Row.Delete()
            Next
        End If
        If DtNotasCreditoRel.Rows.Count <> 0 Then
            For Each Row As DataRow In DtNotasCreditoRel.Rows   'halla AsignacionLotes de las devoluciones para borrarlas.
                Dim Sql As String = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 4 AND Comprobante = " & Row("NotaCredito") & ";"
                If Not Tablas.Read(Sql, ConexionRelacionada, DtAsignacionNCRel) Then Exit Sub
                Row("Estado") = 2
            Next
            For Each Row As DataRow In DtAsignacionNCRel.Rows
                Row.Delete()
            Next
        End If
        '----------------------------------------------------------------------------





        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaArticuloDeshabilitado(DtDetalle) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If Relacionada <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & Relacionada & ";", ConexionRelacionada, DtLotesRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM FacturasCabeza WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtCabezaRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM FacturasDetalle WHERE Factura = " & Relacionada & ";", ConexionRelacionada, DtDetalleRel) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Me.Close() : Exit Sub
        End If

        Dim DtFacturaCabezaB As New DataTable
        Dim DtFacturaDetalleB As New DataTable
        Dim DtAsignacionLotesB As New DataTable
        Dim DtFacturaCabezaN As New DataTable
        Dim DtFacturaDetalleN As New DataTable
        Dim DtAsignacionLotesN As New DataTable

        If PAbierto Then
            DtFacturaCabezaB = DtCabeza.Copy
            DtFacturaDetalleB = DtDetalle.Copy
            DtAsignacionLotesB = DtAsignacionLotes.Copy
            DtFacturaCabezaN = DtCabezaRel.Copy
            DtFacturaDetalleN = DtDetalleRel.Copy
            DtAsignacionLotesN = DtLotesRel.Copy
        Else
            DtFacturaCabezaB = DtCabezaRel.Copy
            DtFacturaDetalleB = DtDetalleRel.Copy
            DtAsignacionLotesB = DtLotesRel.Copy
            DtFacturaCabezaN = DtCabeza.Copy
            DtFacturaDetalleN = DtDetalle.Copy
            DtAsignacionLotesN = DtAsignacionLotes.Copy
        End If

        'Acera listab de lotes para la devolucion total.
        Dim ListaDeLotesAux As New List(Of FilaAsignacion)

        'Actualiza Asignacion Lotes.
        If Not ArmaAsignacionFactura(2, DtFacturaCabezaB, DtFacturaDetalleB, DtFacturaCabezaN, DtFacturaDetalleN, _
                                          DtAsignacionLotesB, DtAsignacionLotesN, ListaDeLotesAux, PPaseDeProyectos) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        'Actualiza Stock.
        Dim DtStockB As New DataTable
        Dim DtStockN As New DataTable
        If Not ArmaStockFactura(DtAsignacionLotes, DtStockB, DtStockN, ListaDeLotesAux, PPaseDeProyectos) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub

        If Not PermisoTotal And DtStockN.Rows.Count <> 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Usuario No Autorizado(1000).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        'Arma asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        'Para Costo.
        Dim DtAsientoCabezaCostoB As New DataTable
        Dim DtAsientoDetalleCostoB As New DataTable
        Dim DtAsientoCabezaCostoN As New DataTable
        Dim DtAsientoDetalleCostoN As New DataTable

        Dim DtAsignacionAux As DataTable
        If DtFacturaCabezaB.Rows.Count <> 0 Then
            DtAsignacionAux = DtAsignacionLotesB.Copy
        Else
            DtAsignacionAux = DtAsignacionLotesN.Copy
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            If Not HallaAsiento(PtipoAsiento, DtFacturaCabezaB.Rows(0).Item("Factura"), DtAsientoCabezaB, DtAsientoDetalleB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not HallaAsiento(PtipoAsientoCosto, DtFacturaCabezaB.Rows(0).Item("Factura"), DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, Conexion) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            If Not HallaAsiento(PtipoAsiento, DtFacturaCabezaN.Rows(0).Item("Factura"), DtAsientoCabezaN, DtAsientoDetalleN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not HallaAsiento(PtipoAsientoCosto, DtFacturaCabezaN.Rows(0).Item("Factura"), DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, ConexionN) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtAsientoCabezaB.Rows.Count <> 0 Or DtAsientoCabezaN.Rows.Count <> 0 Then
            If Not ArmaAsientosFacturas("B", PtipoAsiento, DtFacturaCabezaB, DtFacturaCabezaN, DtFacturaDetalleB, DtFacturaDetalleN, ListaDeLotesOriginal, _
                                     DtAsignacionAux, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, 0, 0, PPaseDeProyectos, "01/01/1800") Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If
        If DtAsientoCabezaCostoB.Rows.Count <> 0 Or DtAsientoCabezaCostoN.Rows.Count <> 0 Then
            If Not ArmaAsientosCostosFacturas("B", PtipoAsientoCosto, DtFacturaCabezaB, DtFacturaCabezaN, DtFacturaDetalleB, DtFacturaDetalleN, ListaDeLotesOriginal, _
                          DtAsignacionAux, DtAsientoCabezaCostoB, DtAsientoDetalleCostoB, DtAsientoCabezaCostoN, DtAsientoDetalleCostoN, PPaseDeProyectos, "01/01/1800") Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        If DtFacturaCabezaB.Rows.Count <> 0 Then
            DtFacturaCabezaB.Rows(0).Item("Estado") = 2
        End If
        If DtFacturaCabezaN.Rows.Count <> 0 Then
            DtFacturaCabezaN.Rows(0).Item("Estado") = 2
        End If

        Dim NumeroW As Double = ActualizaAsignacion("M", DtFacturaCabezaB, DtFacturaCabezaN, DtAsignacionLotesB, DtAsignacionLotesN, DtAsientoDetalleB, DtAsientoDetalleN, DtAsientoDetalleCostoB, DtAsientoDetalleCostoN, DtStockB, DtStockN, DtNotasCredito, DtNotasCreditoRel, DtAsignacionNC, DtAsignacionNCRel)

        If NumeroW < 0 Then
            MsgBox("ERROR: En Base de Datos. Operación se CANCELA. ", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW = 0 Then
            MsgBox("ERROR: Otro usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If NumeroW > 0 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close() : Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsignacionAutomatica_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsignacionAutomatica.Click

        'PMERCADO 10-06-2025
        ButtonAsignacionManual.PerformClick()

        If DtAsignacionLotes.Rows.Count <> 0 Then
            MsgBox("Comprobante ya Asignado. Debe Devolver todos los Lotes.  Operación se CANCELA.")
            Exit Sub
        End If

        'ELIMINAR
        Dim Fecha1 As DateTime
        Dim Fecha2 As DateTime


        Fecha1 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

        If Not AsignacionLotesAutomatico(DtCabeza, DtDetalle, 0, ListaDeLotes) Then Exit Sub

        Fecha2 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
        Dim diferencia As TimeSpan = Fecha2 - Fecha1
        'Console.WriteLine("Días: " & diferencia.Days)
        'Console.WriteLine("Horas: " & diferencia.Hours)
        'Console.WriteLine("Minutos: " & diferencia.Minutes)
        'Console.WriteLine("Segundos: " & diferencia.Seconds)
        MsgBox("Segundos: " & diferencia.Seconds, MsgBoxStyle.Information)

        MsgBox("Asignacion Correcta.", MsgBoxStyle.Information)
        Grid.Refresh()

    End Sub
    Private Sub HallaNotasCreditoConLotes(ByRef DtNota As DataTable, ByVal Factura As Decimal, ByVal ConexionStr As String)

        DtNota = New DataTable

        Dim Sql As String = "SELECT * FROM NotasCreditoCabeza WHERE Estado = 1 AND Factura = " & Factura & ";"

        If Not Tablas.Read(Sql, ConexionStr, DtNota) Then End

    End Sub
    Private Function MuestraDatos() As Boolean

        Dim Sql As String

        If PAbierto Then
            ConexionFactura = Conexion
            ConexionRelacionada = ConexionN
        Else
            ConexionFactura = ConexionN
            ConexionRelacionada = Conexion
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        Sql = "SELECT * FROM FacturasCabeza WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtCabeza) Then Return False
        If PFactura <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Factura No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Return False
        End If

        TextFactura.Text = NumeroEditado(PFactura)
        LabelCliente.Text = NombreCliente(DtCabeza.Rows(0).Item("Cliente"))
        LabelDeposito.Text = NombreDeposito(DtCabeza.Rows(0).Item("Deposito"))

        Relacionada = DtCabeza.Rows(0).Item("Relacionada")

        'Halla Relacionada.
        If PAbierto And DtCabeza.Rows(0).Item("Rel") And PermisoTotal Then
            Relacionada = HallaRelacionada(PFactura)
            If Relacionada < 0 Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        End If

        DtDetalle = New DataTable
        Sql = "SELECT * FROM FacturasDetalle WHERE Factura = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False

        'Arma tabla con AsignacionLotes de facturas. 
        DtAsignacionLotes = New DataTable
        Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 2 AND Comprobante = " & PFactura & ";"
        If Not Tablas.Read(Sql, ConexionFactura, DtAsignacionLotes) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Return False
        If DtAsignacionLotes.Rows.Count <> 0 Then
            ListaDeLotes = New List(Of FilaAsignacion)
            ListaDeLotesOriginal = New List(Of FilaAsignacion)
            For Each Row As DataRow In DtAsignacionLotes.Rows
                Dim Fila As New FilaAsignacion
                Fila.Indice = Row("Indice")
                Fila.Lote = Row("Lote")
                Fila.Secuencia = Row("Secuencia")
                Fila.Deposito = Row("Deposito")
                Fila.Operacion = Row("Operacion")
                Fila.Asignado = Row("Cantidad")
                Fila.Importe = Row("Importe")
                Fila.ImporteSinIva = Row("ImporteSinIva")
                ListaDeLotes.Add(Fila)
                ListaDeLotesOriginal.Add(Fila)
            Next
        End If

        Grid.DataSource = bs
        bs.DataSource = DtDetalle

        If PermisoTotal Then
            If PAbierto Then
                PictureCandado.Image = ImageList1.Images.Item("Abierto")
            Else : PictureCandado.Image = ImageList1.Images.Item("Cerrado")
            End If
        End If

        Grid.Columns("Indice").Visible = False
        Grid.Columns("Precio").HeaderText = "Precio xUni"

        Dim TotalNeto As Decimal = 0
        Dim TotalIva As Decimal = 0

        For I As Integer = 0 To Grid.Rows.Count - 1
            Grid.Rows(I).Cells("Neto").Value = CalculaNeto(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value)
            TotalNeto = TotalNeto + Grid.Rows(I).Cells("Neto").Value
            Grid.Rows(I).Cells("MontoIva").Value = CalculaIva(Grid.Rows(I).Cells("Cantidad").Value, Grid.Rows(I).Cells("Precio").Value, Grid.Rows(I).Cells("Iva").Value)
            TotalIva = TotalIva + Grid.Rows(I).Cells("MontoIva").Value
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Function ActualizaAsignacion(ByVal Funcion As String, ByVal DtFacturaCabezaBW As DataTable, ByVal DtFacturaCabezaNW As DataTable, ByVal DtAsignacionLotesBW As DataTable, ByVal DtAsignacionLotesNW As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtAsientoDetalleCostoB As DataTable, ByVal DtAsientoDetalleCostoN As DataTable, ByVal DtStockB As DataTable, ByVal DtStockN As DataTable, ByVal DtDtNotasCredito As DataTable, ByVal DtDtNotasCreditoRel As DataTable, ByVal DtAsignacionNC As DataTable, ByVal DtAsignacionNCRel As DataTable) As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                ' Actualiza Facturas.
                If Not IsNothing(DtFacturaCabezaBW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaCabezaBW.GetChanges, "FacturasCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtFacturaCabezaNW.GetChanges) Then
                    Resul = GrabaTabla(DtFacturaCabezaNW.GetChanges, "FacturasCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                ' Graba asignacion.
                If Not IsNothing(DtAsignacionLotesBW.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionLotesBW.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsignacionLotesNW.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionLotesNW.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                ' ReGraba Asientos
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleCostoB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleCostoB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleCostoN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleCostoN.GetChanges, "AsientosDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                ' Actualiza Stock.
                If Not IsNothing(DtStockB.GetChanges) Then
                    Resul = GrabaTabla(DtStockB.GetChanges, "Lotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtStockN.GetChanges) Then
                    Resul = GrabaTabla(DtStockN.GetChanges, "Lotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If

                'Borra Asignaion Lotes de las notas de credito.
                If Not IsNothing(DtDtNotasCredito.GetChanges) Then
                    Resul = GrabaTabla(DtDtNotasCredito.GetChanges, "NotasCreditoCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtDtNotasCreditoRel.GetChanges) Then
                    Resul = GrabaTabla(DtDtNotasCreditoRel.GetChanges, "NotasCreditoCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsignacionNC.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionNC.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsignacionNCRel.GetChanges) Then
                    Resul = GrabaTabla(DtAsignacionNCRel.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = TodosLosArticulos()

        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function ListasIguales() As Boolean

        Dim Esta As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            Esta = False
            For Each Fila1 As FilaAsignacion In ListaDeLotesOriginal
                If Fila.Indice = Fila1.Indice And Fila.Lote = Fila1.Lote And Fila.Secuencia = Fila1.Secuencia And Fila.Asignado = Fila1.Asignado Then
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then Return False
        Next

        For Each Fila As FilaAsignacion In ListaDeLotesOriginal
            Esta = False
            For Each Fila1 As FilaAsignacion In ListaDeLotes
                If Fila.Indice = Fila1.Indice And Fila.Lote = Fila1.Lote And Fila.Secuencia = Fila1.Secuencia And Fila.Asignado = Fila1.Asignado Then
                    Esta = True
                    Exit For
                End If
            Next
            If Not Esta Then Return False
        Next

        Return True

    End Function
    Private Function BuscaIndice(ByVal Indice As Integer) As Boolean

        For Each Fila As FilaAsignacion In ListaDeLotes
            If Fila.Indice = Indice Then Return True
        Next

        Return False

    End Function
    Private Function HallaRelacionada(ByVal Factura As Double) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionN)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Factura FROM FacturasCabeza WHERE Relacionada = " & Factura & ";", Miconexion)
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
    Public Function HallaAsiento(ByVal TipoAsiento As Integer, ByVal Comprobante As Decimal, ByRef DtAsientoCabezaW As DataTable, ByRef DtAsientoDetalleW As DataTable, ByVal ConexionStr As String) As Boolean

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & TipoAsiento & " AND Documento = " & Comprobante & ";", conexionStr, DtAsientoCabezaW) Then Return False
        If DtAsientoCabezaW.Rows.Count <> 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = " & DtAsientoCabezaW.Rows(0).Item("Asiento") & ";", ConexionStr, DtAsientoDetalleW) Then Return False
        End If

        Return True

    End Function
    Private Function HallaNotasCreditoLoteadas(ByVal Factura As Double, ByVal ConexionStr As String) As Integer

        Dim Sql As String = "SELECT COUNT(NotaCredito) FROM NotasCreditoCabeza WHERE Estado = 1 AND Factura = " & Factura & ";"

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand(Sql, Miconexion)
                    Return Cmd.ExecuteScalar()
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al Leer Notas de Credito.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function Valida() As Boolean

        If Not PermisoTotal And DtCabeza.Rows(0).Item("Rel") Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Usuario No Autorizado(1000).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Dim Cantidad As Decimal, CantidadAsignada As Decimal

        For i As Integer = 0 To Grid.Rows.Count - 1
            Cantidad = Cantidad + (Grid.Rows(i).Cells("Cantidad").Value - Grid.Rows(i).Cells("Devueltas").Value)
        Next

        For Each Fila As FilaAsignacion In ListaDeLotes
            CantidadAsignada = CantidadAsignada + Fila.Asignado
        Next

        If CantidadAsignada = 0 Then
            MsgBox("Debe Asingnar Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If CantidadAsignada <> Cantidad Then
            MsgBox("Falta Asingnar Lotes. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If HallaEstadoArticulo(Grid.CurrentRow.Cells("Articulo").Value) = 3 Then
                MsgBox("Articulo " & Grid.CurrentRow.Cells("Articulo").FormattedValue & " Esta Deshabilitado; debe Activarlo previamente. Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            SeleccionaLotesAAsignar.PArticulo = Grid.CurrentRow.Cells("Articulo").Value
            SeleccionaLotesAAsignar.PDeposito = DtCabeza.Rows(0).Item("Deposito")
            SeleccionaLotesAAsignar.PCantidad = Grid.CurrentRow.Cells("Cantidad").Value - Grid.CurrentRow.Cells("Devueltas").Value
            SeleccionaLotesAAsignar.PComprobante = PFactura
            SeleccionaLotesAAsignar.PConexion = ConexionFactura
            SeleccionaLotesAAsignar.PIndice = CInt(Grid.CurrentRow.Cells("Indice").Value)
            SeleccionaLotesAAsignar.PLista = ListaDeLotes
            SeleccionaLotesAAsignar.PListaOriginal = ListaDeLotesOriginal
            SeleccionaLotesAAsignar.ShowDialog()
            ListaDeLotes = SeleccionaLotesAAsignar.PLista
            SeleccionaLotesAAsignar.Dispose()
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        'PMERCADO 10-06-2025
        If Carga_Grilla = True Then

            If e.ColumnIndex < 0 And e.RowIndex < 0 Then Exit Sub

            If IsNumeric(e.Value) Then
                If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
            End If

            If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
                If BuscaIndice(Grid.Rows(e.RowIndex).Cells("Indice").Value) Then
                    e.Value = "  Asignados  " : e.CellStyle.ForeColor = Color.Green
                Else
                    e.Value = "No Asignado" : e.CellStyle.ForeColor = Color.Red
                End If
            End If

            If Grid.Columns(e.ColumnIndex).Name = "Iva" Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "Precio" Then
                e.Value = FormatNumber(e.Value, 3)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "MontoIva" Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "Neto" Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If

            If Grid.Columns(e.ColumnIndex).Name = "TotalArticulo" Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If

        End If


    End Sub

    Private Sub ButtonAsignacionManual_Click(sender As Object, e As EventArgs) Handles ButtonAsignacionManual.Click

        'PMERCADO 10-06-2025
        Carga_Grilla = True

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False

        If Not MuestraDatos() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

    End Sub

    Private Sub Grid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles Grid.CellContentClick

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class