Public Class TrasabilidadLotes
    ' RECORDAR  valor false equivale 0.
    '           valor true  equivale 1.
    Dim DtGrid As DataTable
    Dim DtLotes As DataTable
    Dim DtArticulos As DataTable
    '
    Private WithEvents bs As New BindingSource
    '
    Dim SqlN As String
    Dim SqlB As String
    Private Sub TrasabilidadPorLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        GeneraCombosGrid()

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboProveedor.DataSource = ProveedoresDeFrutasYCosteo()
        ComboProveedor.DisplayMember = "Nombre"
        ComboProveedor.ValueMember = "Clave"
        ComboProveedor.SelectedValue = 0
        With ComboProveedor
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = 0
        With ComboArticulo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaComboTablas(ComboEspecie, 1)
        With ComboEspecie
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboEspecie.SelectedValue = 0

        LlenaComboTablas(ComboVariedad, 2)
        With ComboVariedad
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With
        ComboVariedad.SelectedValue = 0

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If

    End Sub
    Private Sub TrasabilidadLotes_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        SqlB = "SELECT '1' AS Operacion,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Fecha,KilosXUnidad,Articulo,Cantidad,Baja,Traslado,Descarte,DiferenciaInventario,Merma,BajaReproceso FROM Lotes "
        SqlN = "SELECT '2' AS Operacion,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Fecha,KilosXUnidad,Articulo,Cantidad,Baja,Traslado,Descarte,DiferenciaInventario,Merma,BajaReproceso FROM Lotes "

        Dim SqlArticulo As String = ""
        If ComboArticulo.SelectedValue <> 0 Then
            SqlArticulo = "WHERE Articulo = " & ComboArticulo.SelectedValue & " "
        Else : SqlArticulo = "WHERE Articulo LIKE '%' "
        End If

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia)
        End If

        Dim SqlLote As String = ""
        If Lote <> 0 Then
            SqlLote = "AND LoteOrigen = " & Lote & " AND SecuenciaOrigen = " & Secuencia & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlProveedor As String = ""
        If ComboProveedor.SelectedValue <> 0 Then
            SqlProveedor = "AND Proveedor = " & ComboProveedor.SelectedValue & " "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Deposito = " & ComboDeposito.SelectedValue & " "
        End If

        SqlB = SqlB & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlDeposito & ";"
        SqlN = SqlN & SqlArticulo & SqlLote & SqlFecha & SqlProveedor & SqlDeposito & ";"

        CreaDtGrid()

        LLenaDtLotes()

        If DtLotes.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        LLenaGrid()

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonRemitosPorLote_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRemitosPorLote.Click

        If Grid.Rows.Count = 0 Then Exit Sub
        If IsDBNull(Grid.CurrentRow.Cells("Lote").Value) Then Exit Sub

        Dim Col As Integer = Me.Grid.CurrentCell.ColumnIndex()

        RemitosPorLote(Grid.CurrentRow.Cells("Lote").Value, Grid.CurrentRow.Cells("Secuencia").Value, Grid.CurrentRow.Cells("Deposito").Value, Grid.CurrentRow.Cells("Operacion").Value)

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Trazabilidad de Lotes", "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ComboProveedor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboProveedor.Validating

        If IsNothing(ComboProveedor.SelectedValue) Then ComboProveedor.SelectedValue = 0

    End Sub
    Private Sub ComboArticulor_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboArticulo.Validating

        If IsNothing(ComboArticulo.SelectedValue) Then ComboArticulo.SelectedValue = 0

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub ComboEspecie_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboEspecie.Validating

        If IsNothing(ComboEspecie.SelectedValue) Then ComboEspecie.SelectedValue = 0

    End Sub
    Private Sub ComboVariedad_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboVariedad.Validating

        If IsNothing(ComboVariedad.SelectedValue) Then ComboVariedad.SelectedValue = 0

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub LLenaDtLotes()

        DtLotes = New DataTable
        If Not Tablas.Read(SqlB, Conexion, DtLotes) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtLotes) Then Me.Close() : Exit Sub
        End If

        If ComboEspecie.SelectedValue = 0 And ComboVariedad.SelectedValue = 0 Then Exit Sub

        Dim SqlEspecie As String = ""
        If ComboEspecie.SelectedValue <> 0 Then
            SqlEspecie = "AND Especie = " & ComboEspecie.SelectedValue
        End If

        Dim SqlVariedad As String = ""
        If ComboVariedad.SelectedValue <> 0 Then
            SqlVariedad = "AND Variedad = " & ComboVariedad.SelectedValue
        End If

        Dim Sql As String = "SELECT Clave FROM Articulos WHERE Clave LIKE '%' " & SqlEspecie & SqlVariedad & ";"

        Dim Dt As New DataTable
        If Not Tablas.Read(Sql, Conexion, Dt) Then End

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtLotes.Rows
            RowsBusqueda = Dt.Select("Clave = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Row.Delete()
            End If
        Next

    End Sub
    Private Sub LLenaGrid()

        Dim LoteAnt As String = ""
        Dim LoteOrigenAnt As String = ""
        Dim DtLote As New DataTable
        Dim Sql As String = ""
        Dim RowGrid As DataRow
        Dim Cantidad As Decimal = 0
        Dim Baja As Decimal = 0
        Dim Descarte As Decimal = 0
        Dim Merma As Decimal = 0
        Dim DiferenciaInventario As Decimal = 0
        Dim Remitido As Decimal = 0
        Dim Facturado As Decimal = 0
        Dim KilosXUnidad As Decimal
        Dim Importe As Decimal = 0
        Dim ImporteSinIva As Decimal = 0
        Dim RemitidoL As Decimal = 0
        Dim FacturadoL As Decimal = 0
        Dim ImporteL As Decimal = 0
        Dim ImporteSinIvaL As Decimal = 0
        Dim MermaNVLPL As Decimal = 0
        Dim View As New DataView

        View = DtLotes.DefaultView
        View.Sort = "LoteOrigen,SecuenciaOrigen,Lote,Secuencia,Fecha"

        For Each Row As DataRowView In View
            If LoteOrigenAnt <> Row("LoteOrigen") & "/" & Format(Row("SecuenciaOrigen"), "000") Then
                If LoteOrigenAnt <> "" Then
                    AgregaTotal(LoteOrigenAnt, Cantidad, Baja, Descarte, Merma, DiferenciaInventario, Remitido, Facturado, KilosXUnidad)
                End If
                KilosXUnidad = Row("KilosXUnidad")
                Cantidad = 0
                Descarte = 0
                Merma = 0
                DiferenciaInventario = 0
                Remitido = 0
                Facturado = 0
                Importe = 0
                ImporteSinIva = 0
                LoteOrigenAnt = Row("LoteOrigen") & "/" & Format(Row("SecuenciaOrigen"), "000")
            End If
            'Procesa Lote.
            If Not ProcesaMovimientos(Row("Lote"), Row("Secuencia"), Row("Deposito"), RemitidoL, FacturadoL, ImporteL, ImporteSinIvaL, MermaNVLPL) Then Me.Close() : Exit Sub
            'Agrega registro en DtGrid.
            RowGrid = DtGrid.NewRow()
            RowGrid("Operacion") = Row("Operacion")
            RowGrid("Lote") = Row("Lote")
            RowGrid("Secuencia") = Row("Secuencia")
            RowGrid("Deposito") = Row("Deposito")
            RowGrid("Articulo") = Row("Articulo")
            RowGrid("Fecha") = Row("Fecha")
            Dim Stock As Decimal = Row("Cantidad") - Row("Traslado") - Row("BajaReproceso") - Row("Baja") - Row("Descarte") - Row("DiferenciaInventario") - RemitidoL - FacturadoL
            If RadioCantidadEnUni.Checked Then
                RowGrid("Cantidad") = Row("Cantidad")
                RowGrid("Baja") = Row("Baja")
                RowGrid("Traslado") = Row("Traslado")
                RowGrid("Descarte") = Row("Descarte")
                RowGrid("Remitido") = RemitidoL
                RowGrid("Facturado") = FacturadoL
                RowGrid("Merma") = Row("Merma")
                RowGrid("DiferenciaInventario") = Row("DiferenciaInventario")
                RowGrid("BajaReproceso") = Row("BajaReproceso")
                RowGrid("Stock") = Stock
                If Row("Secuencia") > 99 Then
                    RowGrid("AltaReproceso") = Row("Cantidad")
                    RowGrid("Cantidad") = 0
                Else
                    RowGrid("AltaReproceso") = 0
                End If
            Else : RowGrid("Cantidad") = Row("Cantidad") * Row("KilosXUnidad")
                RowGrid("Baja") = Row("Baja") * Row("KilosXUnidad")
                RowGrid("Traslado") = Row("Traslado") * Row("KilosXUnidad")
                RowGrid("Descarte") = Row("Descarte") * Row("KilosXUnidad")
                RowGrid("Remitido") = RemitidoL * Row("KilosXUnidad")
                RowGrid("Facturado") = FacturadoL * Row("KilosXUnidad")
                RowGrid("Merma") = Row("Merma") * Row("KilosXUnidad")
                RowGrid("DiferenciaInventario") = Row("DiferenciaInventario") * Row("KilosXUnidad")
                RowGrid("BajaReproceso") = Row("BajaReproceso") * Row("KilosXUnidad")
                RowGrid("Stock") = Stock * Row("KilosXUnidad")
                If Row("Secuencia") > 99 Then
                    RowGrid("AltaReproceso") = Row("Cantidad") * Row("KilosXUnidad")
                    RowGrid("Cantidad") = 0
                Else
                    RowGrid("AltaReproceso") = 0
                End If
            End If
            '            RowGrid("BajaReproceso") = Row("BajaReproceso") - (Row("Merma") - MermaNVLPL)
            '            RowGrid("Stock") = Row("Cantidad") - Row("Traslado") - Row("BajaReproceso") - MermaNVLPL - Row("Baja") - Row("Descarte") - Row("DiferenciaInventario") - RemitidoL - FacturadoL
            DtGrid.Rows.Add(RowGrid)
            If Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen") Then
                If RadioCantidadEnUni.Checked Then
                    Cantidad = Row("Cantidad")
                    Baja = Row("Baja")
                Else
                    Cantidad = Row("Cantidad") * Row("KilosXUnidad")
                    Baja = Row("Baja") * Row("KilosXUnidad")
                End If
            End If
            Descarte = Descarte + Row("Descarte") * Row("KilosXUnidad")
            Merma = Merma + Row("Merma") * Row("KilosXUnidad")
            DiferenciaInventario = DiferenciaInventario + Row("DiferenciaInventario") * Row("KilosXUnidad")
            Remitido = Remitido + RemitidoL * Row("KilosXUnidad")
            Facturado = Facturado + FacturadoL * Row("KilosXUnidad")
        Next
        If Cantidad <> 0 Then AgregaTotal(LoteOrigenAnt, Cantidad, Baja, Descarte, Merma, DiferenciaInventario, Remitido, Facturado, KilosXUnidad)

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        For y As Integer = 0 To Grid.Rows.Count - 1
            If IsDBNull(Grid.Rows(y).Cells("Lote").Value) Then
                For i As Integer = 0 To Grid.Columns.Count - 1
                    Grid.Rows(y).Cells(i).Style.BackColor = Color.Yellow
                    '  Grid.Item(i, Grid.Rows.Count - 1).Style.BackColor = Color.Yellow
                Next
            End If
        Next

    End Sub
    Private Sub AgregaTotal(ByVal LoteAnt As String, ByVal Cantidad As Decimal, ByVal Baja As Decimal, ByVal Descarte As Decimal, ByVal Merma As Decimal, ByVal DiferenciaInventario As Decimal, ByVal Remitido As Decimal, ByVal Facturado As Decimal, ByVal kilosXUnidad As Decimal)

        If CheckSinTotales.Checked Then Exit Sub

        Dim Cartel As String = "TOTAL Lote:  " & LoteAnt
        Dim DescarteW As Decimal
        Dim MermaW As Decimal
        Dim DiferenciaInventarioW As Decimal
        Dim RemitidoW As Decimal
        Dim FacturadoW As Decimal
        Dim StockW As Decimal
        Dim RowGrid As DataRow

        If RadioCantidadEnUni.Checked Then
            DescarteW = Descarte / kilosXUnidad
            MermaW = Merma / kilosXUnidad
            DiferenciaInventarioW = DiferenciaInventario / kilosXUnidad
            RemitidoW = Remitido / kilosXUnidad
            FacturadoW = Facturado / kilosXUnidad
            StockW = Cantidad - MermaW - DiferenciaInventarioW - RemitidoW - FacturadoW - Baja - DescarteW
        Else
            DescarteW = Descarte
            MermaW = Merma
            DiferenciaInventarioW = DiferenciaInventario
            RemitidoW = Remitido
            FacturadoW = Facturado
            StockW = Cantidad - MermaW - DiferenciaInventarioW - RemitidoW - FacturadoW - Baja - DescarteW
        End If

        RowGrid = DtGrid.NewRow()
        RowGrid("Cantidad") = Cantidad
        RowGrid("Baja") = Baja
        RowGrid("Traslado") = 0
        RowGrid("AltaReproceso") = 0
        RowGrid("BajaReproceso") = 0
        RowGrid("Descarte") = DescarteW
        RowGrid("Merma") = MermaW
        RowGrid("DiferenciaInventario") = DiferenciaInventarioW
        RowGrid("Remitido") = RemitidoW
        RowGrid("Facturado") = FacturadoW
        RowGrid("Stock") = StockW
        DtGrid.Rows.Add(RowGrid)

    End Sub
    Private Sub GeneraCombosGrid()

        Deposito.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 19;")
        Deposito.DisplayMember = "Nombre"
        Deposito.ValueMember = "Clave"

        Articulo.DataSource = TodosLosArticulos()
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Deposito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Baja As New DataColumn("Baja")
        Baja.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Baja)

        Dim Traslado As New DataColumn("Traslado")
        Traslado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Traslado)

        Dim BajaReproceso As New DataColumn("BajaReproceso")
        BajaReproceso.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(BajaReproceso)

        Dim AltaReproceso As New DataColumn("AltaReproceso")
        AltaReproceso.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(AltaReproceso)

        Dim Descarte As New DataColumn("Descarte")
        Descarte.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Descarte)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Merma)

        Dim DiferenciaInventario As New DataColumn("DiferenciaInventario")
        DiferenciaInventario.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(DiferenciaInventario)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Remitido)

        Dim Facturado As New DataColumn("Facturado")
        Facturado.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Facturado)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

    End Sub
    Private Function ProcesaMovimientos(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Deposito As Integer, _
                      ByRef Remitido As Double, ByRef Facturado As Double, ByRef Importe As Double, ByRef ImporteSinIva As Double, ByRef MermaNVLP As Double) As Boolean

        Remitido = 0
        Facturado = 0
        Importe = 0
        ImporteSinIva = 0
        MermaNVLP = 0

        Dim StrLote As String = "AsignacionLotes.Lote = " & Lote & " AND AsignacionLotes.Secuencia = " & Secuencia & " AND AsignacionLotes.Deposito = " & Deposito

        'Proceso Remitos..............................................................................................................
        Dim Resul As Double = CantidadRemitida(Lote, Secuencia, Deposito, Conexion)
        If Resul < 0 Then
            MsgBox("Error, Base de Datos al leer asignado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        Remitido = Resul
        If PermisoTotal Then
            Resul = CantidadRemitida(Lote, Secuencia, Deposito, ConexionN)
            If Resul < 0 Then
                MsgBox("Error, Base de Datos al leer asignado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            Remitido = Remitido + Resul
        End If

        'Proceso facturas.................................................................................................................. 
        Dim StrFacturas As String = "SELECT SUM(Cantidad) As Cantidad,SUM(Importe) AS Importe,SUM(ImporteSinIva) AS ImporteSinIva " & _
                                    "FROM AsignacionLotes WHERE TipoComprobante = 2 AND " & StrLote
        Dim Dt As New DataTable
        If Not Tablas.Read(StrFacturas, Conexion, Dt) Then Return False
        If Not IsDBNull(Dt.Rows(0).Item("Cantidad")) Then
            Facturado = Dt.Rows(0).Item("Cantidad")
            Importe = Dt.Rows(0).Item("Importe")
            ImporteSinIva = Dt.Rows(0).Item("ImporteSinIva")
        End If
        If PermisoTotal Then
            StrFacturas = "SELECT SUM(Cantidad) AS Cantidad,SUM(Importe) AS Importe,SUM(ImporteSinIva) AS ImporteSinIva " & _
                            "FROM AsignacionLotes WHERE Rel = 0 AND TipoComprobante = 2 AND " & StrLote
            Dt.Clear()
            If Not Tablas.Read(StrFacturas, ConexionN, Dt) Then Return False
            If Not IsDBNull(Dt.Rows(0).Item("Cantidad")) Then
                Facturado = Facturado + Dt.Rows(0).Item("Cantidad")
                Importe = Importe + Dt.Rows(0).Item("Importe")
                ImporteSinIva = ImporteSinIva + Dt.Rows(0).Item("ImporteSinIva")
            End If
            'Halla facturado con rel = false, para no repetir
            '  StrFacturas = "SELECT SUM(Importe) AS Importe,SUM(ImporteSinIva) AS ImporteSinIva " & _
            '                    "FROM AsignacionLotes WHERE Rel = 1 AND TipoComprobante = 2 AND " & StrLote
            Dt.Clear()
            If Not Tablas.Read(StrFacturas, ConexionN, Dt) Then Return False
            If Not IsDBNull(Dt.Rows(0).Item("Importe")) Then
                Importe = Importe + Dt.Rows(0).Item("Importe")
                ImporteSinIva = ImporteSinIva + Dt.Rows(0).Item("ImporteSinIva")
            End If
        End If

        'Proceso NVLP ......................................................................................................................... 
        Dt.Clear()
        StrLote = "C.Lote = " & Lote & " AND C.Secuencia = " & Secuencia & " AND C.Deposito = " & Deposito
        StrFacturas = "SELECT SUM(C.Cantidad) AS Cantidad,SUM(C.Merma) AS Merma,SUM(C.NetoConIva) AS Importe,SUM(C.NetoSinIva) AS ImporteSinIva " & _
                                    "FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
                                    "WHERE F.Estado = 1 AND " & StrLote & ";"
        If Not Tablas.Read(StrFacturas, Conexion, Dt) Then Return False
        If Not IsDBNull(Dt.Rows(0).Item("Cantidad")) Then
            Facturado = Facturado + Dt.Rows(0).Item("Cantidad")
            Importe = Importe + Dt.Rows(0).Item("Importe")
            ImporteSinIva = ImporteSinIva + Dt.Rows(0).Item("ImporteSinIva")
            MermaNVLP = Dt.Rows(0).Item("Merma")
        End If
        If PermisoTotal Then
            Dt.Clear()
            StrFacturas = "SELECT SUM(C.Cantidad) AS Cantidad,SUM(C.Merma) AS Merma,SUM(C.NetoConIva) AS Importe,SUM(C.NetoSinIva) AS ImporteSinIva " & _
                          "FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
                          "WHERE F.Estado = 1 AND F.Rel = 0 AND " & StrLote & ";"   'Con Rel = 0(false) para sumar cantidad de los que tienen negro solo. 
            If Not Tablas.Read(StrFacturas, ConexionN, Dt) Then Return False
            If Not IsDBNull(Dt.Rows(0).Item("Cantidad")) Then
                Facturado = Facturado + Dt.Rows(0).Item("Cantidad")
                Importe = Importe + Dt.Rows(0).Item("Importe")
                ImporteSinIva = ImporteSinIva + Dt.Rows(0).Item("ImporteSinIva")
                MermaNVLP = MermaNVLP + Dt.Rows(0).Item("Merma")
            End If
            Dt.Clear()
            '     StrFacturas = "SELECT SUM(C.NetoConIva) AS Importe,SUM(C.NetoSinIva) AS ImporteSinIva " & _
            '                      "FROM NVLPLotes AS C INNER JOIN NVLPCabeza AS F ON C.Liquidacion = F.Liquidacion " & _
            '                       "WHERE F.Estado = 1 AND F.Rel = 1 AND " & StrLote & ";"   'Con Rel = 1(true) y cantidad = 0 para no sumar dos veces.
            '      If Not Tablas.Read(StrFacturas, ConexionN, Dt) Then Return False
            '     If Not IsDBNull(Dt.Rows(0).Item("Importe")) Then
            '         Importe = Importe + Dt.Rows(0).Item("Importe")
            '         ImporteSinIva = ImporteSinIva + Dt.Rows(0).Item("ImporteSinIva")
            '     End If
        End If

        Return True

    End Function
    Private Function Valida() As Boolean

        Dim Lote As Integer = 0
        Dim Secuencia As Integer = 0
        If Val(MaskedLote.Text) <> 0 Then
            If Not ConsisteMaskedLote(MaskedLote.Text, Lote, Secuencia) Then
                MaskedLote.Focus()
                Return False
            End If
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
              Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Merma" Or Grid.Columns(e.ColumnIndex).Name = "Descarte" Or Grid.Columns(e.ColumnIndex).Name = "DiferenciaInventario" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Baja" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Traslado" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "BajaReproceso" Or Grid.Columns(e.ColumnIndex).Name = "AltaReproceso" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Remitido" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Facturado" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ImporteFacturado" Or Grid.Columns(e.ColumnIndex).Name = "ImporteSinIva" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) And Not IsNothing(e.Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub



End Class