Public Class Cantidades
    Private Sub Cantidades_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim Dt As New DataTable

        Dim Cantidad As Double
        Dim Baja As Double
        Dim Traslado As Double
        Dim Bajareproceso As Double
        Dim Stock As Double
        Dim Descarte As Double

        If Not Tablas.Read("SELECT * FROM Lotes;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = cantidad + Row("Cantidad")
            Baja = Baja + Row("Baja")
            Traslado = Traslado + Row("Traslado")
            Bajareproceso = Bajareproceso + Row("BajaReproceso")
            Stock = Stock + Row("Stock")
            Descarte = Descarte + Row("Descarte")
        Next
        TextCantidad.Text = Format(Cantidad, "0.00")
        TextBaja.Text = Format(Baja, "0.00")
        TextTraslado.Text = Format(Traslado, "0.00")
        TextBajareproceso.Text = Format(Bajareproceso, "0.00")
        TextStock.Text = Format(Stock, "0.00")
        TextDescarte.Text = Format(Descarte, "0.00")

        Cantidad = 0
        Baja = 0
        Traslado = 0
        Bajareproceso = 0
        Stock = 0
        Descarte = 0

        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM Lotes;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
            Baja = Baja + Row("Baja")
            Traslado = Traslado + Row("Traslado")
            Bajareproceso = Bajareproceso + Row("BajaReproceso")
            Stock = Stock + Row("Stock")
            Descarte = Descarte + Row("Descarte")
        Next
        TextCantidadN.Text = Format(Cantidad, "0.00")
        TextBajaN.Text = Format(Baja, "0.00")
        TextTrasladoN.Text = Format(Traslado, "0.00")
        TextBajaReprocesoN.Text = Format(Bajareproceso, "0.00")
        TextStockN.Text = Format(Stock, "0.00")
        TextDescarteN.Text = Format(Descarte, "0.00")

        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM devolucionMercaDetalle;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next
        TextCantidadDevolucion.Text = Format(Cantidad, "0.00")

        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM devolucionMercaDetalle;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next
        TextCantidadDevolucionN.Text = Format(Cantidad, "0.00")

        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoCabeza;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Baja")
        Next
        TextReprocesoBaja.Text = Format(Cantidad, "0.00")
        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoCabeza;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Baja")
        Next
        TextReprocesoBajaN.Text = Format(Cantidad, "0.00")

        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoDetalle;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Alta")
        Next
        TextReprocesoAlta.Text = Format(Cantidad, "0.00")
        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoDetalle;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Alta")
        Next
        TextReprocesoAltaN.Text = Format(Cantidad, "0.00")

        Cantidad = 0
        Dim Devueltas As Double = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosDetalle;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
            Devueltas = Devueltas + Row("Devueltas")
        Next
        TextRemitosCantidad.Text = Format(Cantidad, "0.00")
        TextRemitosDevueltas.Text = Format(Devueltas, "0.00")
        Cantidad = 0
        Devueltas = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM RemitosDetalle;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
            Devueltas = Devueltas + Row("Devueltas")
        Next
        TextRemitoscantidadN.Text = Format(Cantidad, "0.00")
        TextRemitosDevueltasN.Text = Format(Devueltas, "0.00")

        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM AsignacionLotes;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next
        TextAsignacionLotesCantidad.Text = Format(Cantidad, "0.00")
        Cantidad = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM AsignacionLotes;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next
        TextAsignacionLotesCantidadN.Text = Format(Cantidad, "0.00")


        Cantidad = 0
        Devueltas = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM FacturasDetalle;", Conexion, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
            Devueltas = Devueltas + Row("Devueltas")
        Next
        TextFacturasCantidad.Text = Format(Cantidad, "0.00")
        TextfacturasDevueltas.Text = Format(Devueltas, "0.00")
        Cantidad = 0
        Devueltas = 0
        Dt = New DataTable
        If Not Tablas.Read("SELECT * FROM FacturasDetalle;", ConexionN, Dt) Then End
        For Each Row As DataRow In Dt.Rows
            Cantidad = Cantidad + Row("Cantidad")
            Devueltas = Devueltas + Row("Devueltas")
        Next
        TextFacturasCantidadN.Text = Format(Cantidad, "0.00")
        TextfacturasDevueltasn.Text = Format(Devueltas, "0.00")

    End Sub

  
End Class