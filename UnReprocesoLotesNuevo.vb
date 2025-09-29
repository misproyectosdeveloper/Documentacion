Public Class UnReprocesoLotesNuevo
    Public PReproceso As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtDetalleBaja As DataTable
    Dim DtGrid As DataTable
    Dim DtGridBaja As DataTable
    Dim DtLotes As DataTable
    Dim ds As DataSet
    '
    Dim ListaAlta As List(Of ItemReproceso)
    '
    Dim cb As ComboBox
    Dim TotalBaja As Decimal
    Dim ConexionLotes As String
    Dim Deposito As Integer
    Private Sub UnReprocesoLotesNuevos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        GridBajas.AutoGenerateColumns = False

        LlenaCombosGridBaja()

        LlenaComboTablas(ComboDeposito, 19)

        ComboEstado.DataSource = DtEstadoAfectaStockYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        CreaDtGridBaja()

        If PReproceso = 0 Then
            OpcionReproceso1.ShowDialog()
            If OpcionReproceso1.PRegresar Then OpcionReproceso1.Dispose() : Me.Close() : Exit Sub
            Deposito = OpcionReproceso1.PDeposito
            For Each Row As DataRow In OpcionReproceso1.PDtGridBaja.Rows
                If Row("Baja") <> 0 Then
                    Dim Row2 As DataRow = DtGridBaja.NewRow
                    Row2("AGranel") = Row("AGranel")
                    Row2("Lote") = Row("Lote")
                    Row2("Secuencia") = Row("Secuencia")
                    Row2("Articulo") = Row("Articulo")
                    Row2("Proveedor") = Row("Proveedor")
                    Row2("KilosXUnidad") = Row("KilosXUnidad")
                    Row2("Fecha") = Row("Fecha")
                    Row2("Stock") = Row("Stock")
                    Row2("Medida") = Row("Medida")
                    Row2("Baja") = Row("Baja")
                    Row2("Merma") = Row("Merma")
                    DtGridBaja.Rows.Add(Row2)
                End If
            Next
            ComboDeposito.SelectedValue = OpcionReproceso1.PDeposito
            PAbierto = OpcionReproceso1.PAbierto
            OpcionReproceso1.Dispose()
            If DtGridBaja.Rows.Count = 0 Then Me.Close() : Exit Sub
        End If

        If PermisoTotal Then
            PictureCandado.Visible = True
            If PAbierto Then
                PictureCandado.Image = UnRemito.ImageList1.Images.Item("Abierto")
                ConexionLotes = Conexion
            Else : PictureCandado.Image = UnRemito.ImageList1.Images.Item("Cerrado")
                ConexionLotes = ConexionN
            End If
        Else : PictureCandado.Visible = False
            ConexionLotes = Conexion
        End If

        If PAbierto Then
            ConexionLotes = Conexion
        Else : ConexionLotes = ConexionN
        End If

        PreparaArchivos()

        GModificacionOk = False

        Grid.DataSource = DtGrid

        AddHandler DtGrid.ColumnChanged, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanged)
        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

    End Sub
    Private Sub UnReprocesoLotes_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Private Sub UnReprocesoLotesNuevo_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PReproceso <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Genera Cabeza.
        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        'Actualiza Lotes Baja.
        Dim DtLotesBaja As New DataTable
        For Each Row In DtGridBaja.Rows
            Dim Sql As String = "SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Deposito & ";"
            If Not Tablas.Read(Sql, ConexionLotes, DtLotesBaja) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        Next
        For Each Row In DtGridBaja.Rows
            RowsBusqueda = DtLotesBaja.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
            RowsBusqueda(0).Item("Stock") = CDec(RowsBusqueda(0).Item("Stock")) - Row("Baja")
            RowsBusqueda(0).Item("BajaReproceso") = CDec(RowsBusqueda(0).Item("BajaReproceso")) + Row("Baja")
            RowsBusqueda(0).Item("Merma") = CDec(RowsBusqueda(0).Item("Merma")) + Row("Merma")
            If CDec(RowsBusqueda(0).Item("Stock")) < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Baja en el Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & "  Hace Negativo el Stock. Operación se Cancela.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        Next

        Dim DtDetalleBajaAux As DataTable = DtDetalleBaja.Clone

        For Each Row In DtGridBaja.Rows    'Arma DetalleBaja.
            Dim Row1 As DataRow = DtDetalleBajaAux.NewRow
            Row1("Clave") = 0
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Baja") = Row("Baja")
            Row1("Merma") = Row("Merma")
            If Row("Merma") < 0 Then
                If MsgBox("Se esta Creando mas kilos de Alta que de Baja para el lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " Desea Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
            DtDetalleBajaAux.Rows.Add(Row1)
        Next

        'Genera Lotes Altas.
        Dim DtDetalleAlta As DataTable = DtDetalle.Clone
        Dim DtLotesAlta As DataTable = DtLotesBaja.Clone

        'Halla Ultimas secuencias de reproceso
        Dim ListaSecuencias As New List(Of ItemReproceso)
        For Each Fila As ItemReproceso In ListaAlta
            If HallaUltimaSecuencia(ListaSecuencias, Fila.Lote) = 0 Then
                Dim FilaW As New ItemReproceso
                FilaW.Lote = Fila.Lote
                Dim SecuenciaW As Integer = UltimaNumeracionSecuenciaLote(Fila.Lote, ConexionLotes)
                If SecuenciaW = -1 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Error al Leer Tabla de Lotes. Operación se CANCELA.", MsgBoxStyle.Critical)
                    Exit Sub
                End If
                FilaW.SecuenciaReproceso = SecuenciaW + 1
                ListaSecuencias.Add(FilaW)
            End If
        Next

        Dim RowsBusquedaLote() As DataRow

        'Genera Lotes Alta.
        'Borra Lotes con alta=0:Conciste que alta no se negativa. Si es negativa cancela alta. 
        For I As Integer = ListaAlta.Count - 1 To 0 Step -1
            If ListaAlta.Item(I).Alta < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Error. Se genero un lote con alta negativa. Intentelo nuevamente modificando Cantidad de Bajas en algun lote." + vbCrLf + " Operación se CANCELA.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            If ListaAlta.Item(I).Alta = 0 Then ListaAlta.Remove(ListaAlta.Item(I))
        Next
        '
        For Each Fila As ItemReproceso In ListaAlta
            RowsBusqueda = DtGrid.Select("Articulo = " & Fila.Articulo)
            RowsBusquedaLote = DtLotesBaja.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            Dim Secuencia As Integer
            For Each FilaW As ItemReproceso In ListaSecuencias
                If FilaW.Lote = Fila.Lote Then
                    Secuencia = FilaW.SecuenciaReproceso
                    FilaW.SecuenciaReproceso = FilaW.SecuenciaReproceso + 1
                End If
            Next
            Row = DtLotesAlta.NewRow()
            Row("Lote") = Fila.Lote
            Row("Secuencia") = Secuencia
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Articulo") = Fila.Articulo
            Row("Calibre") = RowsBusqueda(0).Item("Calibre")
            Row("Cantidad") = Fila.Alta
            Row("Fecha") = DateTime1.Value
            Row("Stock") = Fila.Alta
            Row("Senia") = RowsBusquedaLote(0).Item("Senia")
            Row("LoteOrigen") = RowsBusquedaLote(0).Item("LoteOrigen")
            Row("SecuenciaOrigen") = RowsBusquedaLote(0).Item("SecuenciaOrigen")
            Row("DepositoOrigen") = RowsBusquedaLote(0).Item("DepositoOrigen")
            Row("TipoOperacion") = RowsBusquedaLote(0).Item("TipoOperacion")
            Row("ClientePotencial") = RowsBusqueda(0).Item("Cliente")
            Row("Proveedor") = RowsBusquedaLote(0).Item("Proveedor")
            Row("KilosXUnidad") = Fila.KilosXUnidad
            Row("Traslado") = 0
            Row("BajaReproceso") = 0
            Row("Directo") = -1
            Row("Descarte") = 0
            Row("Merma") = 0
            Row("Baja") = 0
            Row("Liquidado") = 0
            Row("PrecioF") = 0
            Row("PrecioCompra") = 0
            Row("PrecioPorLista") = 0
            Row("PermisoImp") = 0
            Row("MermaTr") = 0
            Row("DiferenciaInventario") = 0
            DtLotesAlta.Rows.Add(Row)
        Next

        For Each Row In DtLotesAlta.Rows 'Arma DetalleAlta.
            Dim RowAlta As DataRow = DtDetalleAlta.NewRow()
            RowAlta("Lote") = Row("Lote")
            RowAlta("Secuencia") = Row("Secuencia")
            RowAlta("Calibre") = Row("Calibre")
            RowAlta("KilosXUnidad") = Row("KilosXUnidad")
            RowAlta("Alta") = Row("Cantidad")
            RowAlta("ClientePotencial") = Row("ClientePotencial")
            DtDetalleAlta.Rows.Add(RowAlta)
        Next

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'Graba Reproceso.
        Dim Numero As Double = 0
        Dim NumeroLote As Double = 0
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Reproceso.
            Numero = CDbl(TextComprobante.Text)
            If Numero < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            DtCabezaAux.Rows(0).Item("Clave") = Numero
            For Each row1 As DataRow In DtDetalleAlta.Rows
                row1("Clave") = Numero
            Next
            For Each row1 As DataRow In DtDetalleBajaAux.Rows
                row1("Clave") = Numero
            Next
            '
            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionLotes)
                If NumeroAsiento < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos al Leer Tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = Numero
                For Each Row3 As DataRow In DtAsientoDetalle.Rows
                    Row3("Asiento") = NumeroAsiento
                Next
            End If

            Resul = AltaReproceso(DtCabezaAux, DtDetalleAlta, DtDetalleBajaAux, DtLotesAlta, DtLotesBaja, DtAsientoCabeza, DtAsientoDetalle)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i < 50 Then Resul = -10 : Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Dim NuevaClave As Integer
            If NumeracionAlternativa(TextComprobante.Text, NuevaClave) Then
                TextComprobante.Text = NuevaClave
            Else
                MsgBox("Numero Reproceso Ya existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
        End If
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            PReproceso = Numero
            UnReprocesoLotesNuevos_Load(Nothing, Nothing)
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PReproceso = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Reproceso Ya esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow

        Dim DtLotesGenerados As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row In DtDetalle.Rows
            If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & ComboDeposito.SelectedValue & ";", ConexionLotes, DtLotesGenerados) Then Me.Close() : Exit Sub
        Next

        For Each Row In DtLotesGenerados.Rows
            If Row("Cantidad") <> Row("Stock") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Lotes han tenido movimientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DtLotesGenerados.Dispose()
                Exit Sub
            End If
        Next

        For Each Row In DtLotesGenerados.Rows
            '    Row.Delete()   instruccion modificada.
            Row("Cantidad") = 0
            Row("Stock") = 0
        Next

        Dim DtLotesBaja As New DataTable
        For Each Row In DtGridBaja.Rows
            Dim Sql As String = "SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & ComboDeposito.SelectedValue & ";"
            If Not Tablas.Read(Sql, ConexionLotes, DtLotesBaja) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        Next
        For Each Row In DtLotesBaja.Rows
            RowsBusqueda = DtGridBaja.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia"))
            Row("BajaReproceso") = Row("BajaReproceso") - RowsBusqueda(0).Item("Baja")
            Row("Merma") = Row("Merma") - RowsBusqueda(0).Item("Merma")
            Row("Stock") = Row("Stock") + RowsBusqueda(0).Item("Baja")
        Next

        Dim DtCabezaAux As DataTable = DtCabeza.Copy

        DtCabezaAux.Rows(0).Item("Estado") = 3

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(7000, PReproceso, DtAsientoCabeza, ConexionLotes) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("El Reproceso se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim Trans As OleDb.OleDbTransaction

        Using Miconexion As New OleDb.OleDbConnection(ConexionLotes)
            Try
                Miconexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = Miconexion.BeginTransaction(IsolationLevel.ReadCommitted)
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ReprocesoCabeza;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtCabezaAux.GetChanges)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtLotesBaja.GetChanges)
                        DaP.Update(DtLotesGenerados.GetChanges)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtAsientoCabeza)
                    End Using
                    '
                    Trans.Commit()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("Reproceso Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Finally
                    Trans = Nothing
                End Try
            Catch ex As OleDb.OleDbException
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR de Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        UnReprocesoLotesNuevos_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PReproceso = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 7000
        If PAbierto Then
            ListaAsientos.PDocumentoB = PReproceso
        Else
            ListaAsientos.PDocumentoN = PReproceso
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PReproceso = 0
        UnReprocesoLotesNuevos_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.Rows.Count = 0 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If
        Grid.Rows.Remove(Grid.CurrentRow)

        Calcular()

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub PreparaArchivos()

        CreaDtGrid()

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoCabeza WHERE Clave = " & PReproceso & ";", ConexionLotes, DtCabeza) Then Me.Close() : Exit Sub
        If PReproceso <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Reproceso No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PReproceso = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow()
            Row("Clave") = HallaUltimaClave(ConexionLotes)
            Row("Lote") = 0
            Row("Secuencia") = 0
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Fecha") = Now
            Row("Baja") = 0
            Row("KilosxUnidad") = 0
            Row("Merma") = 0
            Row("Estado") = 1
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtDetalleBaja = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoDetalleBaja WHERE Clave = " & PReproceso & ";", ConexionLotes, DtDetalleBaja) Then Me.Close() : Exit Sub
        For Each Row As DataRow In DtDetalleBaja.Rows
            Dim Row2 As DataRow = DtGridBaja.NewRow
            Row2("Lote") = Row("Lote")
            Row2("Secuencia") = Row("Secuencia")
            Row2("Fecha") = Now
            Row2("Articulo") = 0
            Row2("KilosXUnidad") = 0
            Row2("AGranel") = False
            Row2("Medida") = ""
            Row2("Stock") = 0
            HallaDatosLotes(PAbierto, Row("Lote"), Row("Secuencia"), Row2("Fecha"), Row2("Articulo"), Row2("KilosXUnidad"), Row2("Stock"))
            HallaAGranelYMedida(Row2("Articulo"), Row2("AGranel"), Row2("Medida"))
            Row2("Merma") = Row("Merma")
            Row2("Baja") = Row("Baja")
            DtGridBaja.Rows.Add(Row2)
        Next

        GridBajas.DataSource = DtGridBaja

        TotalBaja = 0
        For Each Row As DataRow In DtGridBaja.Rows
            TotalBaja = TotalBaja + Row("Baja")
        Next
        TextTotalBaja.Text = FormatNumber(TotalBaja, 2)

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoDetalle WHERE Clave = " & PReproceso & ";", ConexionLotes, DtDetalle) Then Me.Close() : Exit Sub

        ''''''''''       Dim Sql As String = "SELECT * FROM Lotes Where Lote = " & Lote & " AND Secuencia = " & Secuencia & _
        ''''''''''''              " AND Deposito = " & ComboDeposito.SelectedValue & ";"
        '''''''     DtLotes = New DataTable
        ''''''''''      If Not Tablas.Read(Sql, ConexionLotes, DtLotes) Then Me.Close() : Exit Sub
        ''''''''''''       If DtLotes.Rows.Count = 0 Then
        '''''''''MsgBox("Lote No encontradao en la base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        '''''''''''       Me.Close() : Exit Sub
        '''''''''''       End If
        '''''       If PReproceso = 0 Then
        '''''''''''If Stock > DtLotes.Rows(0).Item("Stock") Then
        ''''''''''''MsgBox("Stock del Lote ha cambiado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        ''''''''''''''''      Me.Close() : Exit Sub
        ''''''''''''      End If
        '''''''''''''''''''''        End If

        DtGrid.Clear()

        If PReproceso <> 0 Then
            For Each Row1 As DataRow In DtDetalle.Rows
                Dim Row As DataRow = DtGrid.NewRow()
                Row("AGranel") = EsArticuloAGranel(HallaArticulo(Row1("Lote"), Row1("Secuencia"), ConexionLotes))
                Row("LoteYsecuencia") = Row1("Lote") & "/" & Format(Row1("Secuencia"), "000")
                Row("Articulo") = HallaArticulo(Row1("Lote"), Row1("Secuencia"), ConexionLotes)
                Row("KilosXUnidad") = Row1("KilosXUnidad")
                Row("Cantidad") = Row1("Alta")
                Select Case Row("AGranel")
                    Case True
                        Row("Medida") = "Kg"
                    Case False
                        Row("Medida") = "Un"
                End Select
                Row("Cliente") = Row1("ClientePotencial")
                Row("Calibre") = Row1("Calibre")
                DtGrid.Rows.Add(Row)
            Next
        End If

        LlenaCombosGrid()

        Grid.DataSource = DtGrid

        If PReproceso = 0 Then
            'Panel2.Enabled = True
            Panel1.Enabled = True
            Grid.ReadOnly = False
            GridBajas.ReadOnly = False
            Grid.Columns("LoteYSecuencia").Visible = False
            ButtonEliminarLinea.Enabled = True
        Else
            TextComprobante.ReadOnly = True
            '      Panel2.Enabled = False
            Panel1.Enabled = False
            Grid.ReadOnly = True
            GridBajas.ReadOnly = True
            Grid.Columns("LoteYSecuencia").Visible = True
            ButtonEliminarLinea.Enabled = False
        End If

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtCabeza

        Dim Row As DataRow = DtCabeza.Rows(0)

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Clave")
        AddHandler Enlace.Format, AddressOf FormatComprobante
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Deposito")
        ComboDeposito.DataBindings.Clear()
        ComboDeposito.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatTexto
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Return True

    End Function
    Private Sub FormatComprobante(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Sub FormatTexto(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Sub FormatKilos(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "0.000")

    End Sub
    Private Function HallaDatosLotes(ByVal Abierto As Boolean, ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Fecha As Date, ByRef Articulo As Integer, ByRef KilosXUnidad As Decimal, ByRef Stock As Decimal) As Boolean

        Dim Dt As New DataTable
        Dim ConexionLote As String

        If Abierto Then
            ConexionLote = Conexion
        Else : ConexionLote = ConexionN
        End If

        If Not Tablas.Read("SELECT Cantidad,Baja,Fecha,Articulo,KilosXUnidad,Stock FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & ";", ConexionLote, Dt) Then Return False
        If Dt.Rows.Count = 0 Then Return False
        Fecha = Dt.Rows(0).Item("Fecha")
        Articulo = Dt.Rows(0).Item("Articulo")
        KilosXUnidad = Dt.Rows(0).Item("KilosXUnidad")
        Stock = Dt.Rows(0).Item("Stock")

        Dt.Dispose()
        Return True

    End Function
    Private Sub Calcular()

        TotalBaja = 0

        For Each Row As DataRow In DtGridBaja.Rows
            TotalBaja = TotalBaja + Row("Baja")
        Next
        TextTotalBaja.Text = FormatNumber(TotalBaja, 2)

        If TotalBaja = 0 Then Exit Sub

        Dim ListaBaja As New List(Of ItemReproceso)
        ListaAlta = New List(Of ItemReproceso)
        Dim ListaTrabajo As New List(Of ItemReproceso)

        'CalculaBajas.
        For Each Row As DataRow In DtGridBaja.Rows
            If Row("Baja") <> 0 Then
                Dim Item As New ItemReproceso
                Item.Lote = Row("Lote")
                Item.Secuencia = Row("Secuencia")
                Item.Baja = Row("Baja")
                Item.KilosXUnidad = Row("KilosXUnidad")
                Item.TotalKilos = Row("KilosXUnidad") * Row("Baja")
                Item.Porcentaje = Item.Baja * 100 / TotalBaja
                Item.Articulo = Row("Articulo")
                ListaBaja.Add(Item)
            End If
        Next

        'Calcula Altas.
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Cantidad").Value <> 0 Then
                ProcesaArticuloAlta(ListaBaja, ListaTrabajo, Row.Cells("Cantidad").Value, Row.Cells("Articulo").Value, Row.Cells("KilosXUnidad").Value, Row.Cells("AGranel").Value)
                For Each J As ItemReproceso In ListaTrabajo
                    Dim Item As New ItemReproceso
                    Item.Lote = J.Lote
                    Item.Secuencia = J.Secuencia
                    Item.Articulo = J.Articulo
                    Item.KilosXUnidad = J.KilosXUnidad
                    Item.Alta = J.Alta
                    Item.TotalKilos = J.TotalKilos
                    ListaAlta.Add(Item)
                Next
            End If
        Next

        'Calcula merma.
        For Each Row As DataRow In DtGridBaja.Rows
            Row("Merma") = 0
            If Row("Baja") <> 0 Then
                Dim TotalKilosBaja As Decimal = Row("Baja") * Row("KilosXUnidad")
                Dim TotalKilosAlta As Decimal = 0
                For Each J As ItemReproceso In ListaAlta
                    If Row("Lote") = J.Lote And Row("Secuencia") = J.Secuencia Then
                        TotalKilosAlta = TotalKilosAlta + J.TotalKilos
                    End If
                Next
                If TotalKilosAlta <> 0 Then
                    Dim Merma As Decimal = TotalKilosBaja - TotalKilosAlta
                    Row("Merma") = Trunca(Merma / Row("KilosXUnidad"))
                End If
            End If
        Next

    End Sub
    Private Sub ProcesaArticuloAlta(ByVal ListaBaja As List(Of ItemReproceso), ByRef ListaSalida As List(Of ItemReproceso), ByVal Alta As Decimal, ByVal Articulo As Integer, ByVal KilosXUnidad As Decimal, ByVal AGranel As Boolean)

        ListaSalida.Clear()

        Dim Total As Decimal = 0
        Dim I As Integer = 0

        For Each J As ItemReproceso In ListaBaja
            I = I + 1
            Dim Item As New ItemReproceso
            Item.Lote = J.Lote
            Item.Secuencia = J.Secuencia
            Item.Articulo = Articulo
            Item.KilosXUnidad = KilosXUnidad
            Dim AltaW As Decimal = Alta * J.Porcentaje / 100
            AltaW = TruncaSR(AltaW, 0)
            Total = Total + AltaW
            Item.Alta = AltaW
            Item.TotalKilos = KilosXUnidad * AltaW
            ListaSalida.Add(Item)
        Next

        Dim Diferencia As Decimal = Alta - Total
        If Diferencia <> 0 Then
            'balancea. Pone la diferencia en algun lote.
            For Each fila As ItemReproceso In ListaSalida
                If fila.Alta + Diferencia > 0 Then fila.Alta = fila.Alta + Diferencia : fila.TotalKilos = KilosXUnidad * fila.Alta : Exit For
            Next
        End If

    End Sub
    Private Function AltaReproceso(ByVal DtCabezaAux As DataTable, ByVal DtDetalleAlta As DataTable, ByVal DtDetalleBaja As DataTable, ByVal DtLotesAlta As DataTable, ByVal DtLotesBaja As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Trans As OleDb.OleDbTransaction

        Using Miconexion As New OleDb.OleDbConnection(ConexionLotes)
            Try
                Miconexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = Miconexion.BeginTransaction(IsolationLevel.ReadCommitted)
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ReprocesoCabeza;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtCabezaAux.GetChanges)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ReprocesoDetalle;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtDetalleAlta.GetChanges)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ReprocesoDetalleBaja;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtDetalleBaja.GetChanges)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtLotesAlta.GetChanges)
                        DaP.Update(DtLotesBaja.GetChanges)
                    End Using
                    '
                    If Not IsNothing(DtAsientoCabeza.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", Miconexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoCabeza.GetChanges)
                        End Using
                        '
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", Miconexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoDetalle.GetChanges)
                        End Using
                    End If

                    Trans.Commit()
                    Return 1000
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As OleDb.OleDbException
                Return -2
            End Try
        End Using

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim Especie As Integer
        Dim Variedad As Integer

        HallaEspecieYVariedad(DtGridBaja.Rows(0).Item("Articulo"), Especie, Variedad)

        If PReproceso = 0 Then
            '           Articulo.DataSource = ArticulosActivosSegunEspecieYVariedad(Especie, Variedad)
            Articulo.DataSource = ArticulosActivosSegunEspecie(Especie)
        Else
            '           Articulo.DataSource = TodosLosArticulosSegunEspecieYVariedad(Especie, Variedad)
            Articulo.DataSource = TodosLosArticulosSegunEspecie(Especie)
        End If
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5  ORDER BY Nombre;")
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

        Cliente.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Clientes ORDER BY Nombre;")
        Row = Cliente.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Cliente.DataSource.Rows.Add(Row)
        Cliente.DisplayMember = "Nombre"
        Cliente.ValueMember = "Clave"

    End Sub
    Private Sub LlenaCombosGridBaja()

        ArticuloBaja.DataSource = TodosLosArticulos()
        Dim Row As DataRow = ArticuloBaja.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        ArticuloBaja.DataSource.Rows.Add(Row)
        ArticuloBaja.DisplayMember = "Nombre"
        ArticuloBaja.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim KilosXUnidad As DataColumn = New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(KilosXUnidad)

        Dim LoteySecuencia As DataColumn = New DataColumn("LoteySecuencia")
        LoteySecuencia.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(LoteySecuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Calibre As New DataColumn("Calibre")
        Calibre.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Calibre)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cantidad)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

    End Sub
    Private Sub CreaDtGridBaja()

        DtGridBaja = New DataTable

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGridBaja.Columns.Add(AGranel)

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Lote)

        Dim Secuencia As DataColumn = New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Articulo)

        Dim Proveedor As New DataColumn("Proveedor")
        Proveedor.DataType = System.Type.GetType("System.Int32")
        DtGridBaja.Columns.Add(Proveedor)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGridBaja.Columns.Add(Fecha)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Stock)

        Dim KilosXUnidad As New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(KilosXUnidad)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGridBaja.Columns.Add(Medida)

        Dim Baja As New DataColumn("Baja")
        Baja.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Baja)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Decimal")
        DtGridBaja.Columns.Add(Merma)

    End Sub
    Private Function ArticulosActivosSegunEspecie(ByVal Especie As Integer) As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Estado = 1 AND A.Especie = " & Especie & " ORDER BY A.Nombre;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre").ToString.PadRight(30, " ") & " (S:" & FormatCurrency(Senia, GDecimales) & " Kg:" & Row("Kilos") & ")"
        Next

        ArticulosActivosSegunEspecie = Dt

        Dt.Dispose()

    End Function
    Private Function ArticulosActivosSegunEspecieYVariedad(ByVal Especie As Integer, ByVal Variedad As Integer) As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Estado = 1 AND A.Especie = " & Especie & " AND A.Variedad = " & Variedad & " ORDER BY A.Nombre;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre").ToString.PadRight(30, " ") & " (S:" & FormatNumber(Senia, GDecimales) & " Kg:" & Row("Kilos") & ")"
        Next

        ArticulosActivosSegunEspecieYVariedad = Dt

        Dt.Dispose()

    End Function
    Private Function TodosLosArticulosSegunEspecie(ByVal Especie As Integer) As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Especie = " & Especie & " ORDER BY A.Nombre;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre").ToString.PadRight(30, " ") & " (S:" & FormatCurrency(Senia, GDecimales) & " Kg:" & Row("Kilos") & ")"
        Next

        TodosLosArticulosSegunEspecie = Dt

        Dt.Dispose()

    End Function
    Private Function TodosLosArticulosSegunEspecieYVariedad(ByVal Especie As Integer, ByVal Variedad As Integer) As DataTable

        Dim Dt As New DataTable
        Dim Senia As Double = 0

        Dt = Tablas.Leer("SELECT A.Clave,A.Nombre,A.Envase,E.Kilos FROM Articulos AS A INNER JOIN Envases AS E ON A.Envase = E.Clave WHERE A.Especie = " & Especie & " AND A.Variedad = " & Variedad & " ORDER BY A.Nombre;")

        For Each Row As DataRow In Dt.Rows
            BuscaVigencia(10, Date.Now, Senia, Row("Envase"))
            Row("Nombre") = Row("Nombre").ToString.PadRight(30, " ") & " (S:" & FormatCurrency(Senia, GDecimales) & " Kg:" & Row("Kilos") & ")"
        Next

        TodosLosArticulosSegunEspecieYVariedad = Dt

        Dt.Dispose()

    End Function
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim Precio As Decimal = 0
        Dim Centro As Integer = 0
        Dim TipoOperacion As Integer = 0
        Dim ListaDeCentros As New List(Of ItemReproceso)

        'Para Bajas.
        If Funcion = "A" Then
            For Each Row As DataRow In DtGridBaja.Rows
                TipoOperacion = HallaTipoOperacion(Row("Lote"), Row("Secuencia"), ConexionLotes)
                If TipoOperacion <> 4 Then
                    If Not HallaPrecioCentroTipoOperacion(TipoOperacion, Centro, Precio) Then Return False
                Else
                    Dim Operacion As Integer
                    If PAbierto Then
                        Operacion = 1
                    Else : Operacion = 2
                    End If
                    Dim Costeo = HallaCosteoLote(Operacion, Row("Lote"))
                    If Costeo = -1 Then Return False
                    If Not HallaPrecioYCentroCosteo(Row("Proveedor"), Costeo, Centro, Precio) Then Return False
                End If
                Dim Fila As New ItemLotesParaAsientos
                Fila.Centro = Centro
                Fila.MontoNeto = Trunca(Precio * Row("KilosXUnidad") * Row("Baja"))
                If TipoOperacion = 1 Then Fila.Clave = -111
                If TipoOperacion = 2 Then Fila.Clave = -110
                If TipoOperacion = 3 Then Fila.Clave = -113
                If TipoOperacion = 4 Then Fila.Clave = -112
                ListaLotesParaAsiento.Add(Fila)
                'Para Merma
                Fila = New ItemLotesParaAsientos
                Fila.Centro = Centro
                Fila.MontoNeto = Trunca(Precio * Row("KilosXUnidad") * Row("Merma"))
                If TipoOperacion = 1 Then Fila.Clave = -101
                If TipoOperacion = 2 Then Fila.Clave = -100
                If TipoOperacion = 3 Then Fila.Clave = -103
                If TipoOperacion = 4 Then Fila.Clave = -102
                ListaLotesParaAsiento.Add(Fila)
                '
                Dim Filaw As New ItemReproceso
                Filaw.Lote = Row("Lote")
                Filaw.Secuencia = Row("Secuencia")
                Filaw.Centro = Centro
                Filaw.Precio = Precio
                Filaw.TipoOperacion = TipoOperacion
                ListaDeCentros.Add(Filaw)
            Next
        End If

        'Para Altas.
        If Funcion = "A" Then
            For Each FilaAlta As ItemReproceso In ListaAlta
                HallaCentroYPrecio(ListaDeCentros, FilaAlta.Lote, FilaAlta.Secuencia, Centro, Precio, TipoOperacion)
                Dim Fila As New ItemLotesParaAsientos
                Fila.Centro = Centro
                Fila.MontoNeto = Trunca(Precio * FilaAlta.KilosXUnidad * FilaAlta.Alta)
                If TipoOperacion = 1 Then Fila.Clave = -106
                If TipoOperacion = 2 Then Fila.Clave = -105
                If TipoOperacion = 3 Then Fila.Clave = -108
                If TipoOperacion = 4 Then Fila.Clave = -107
                ListaLotesParaAsiento.Add(Fila)
            Next
        End If

        If Not Asiento(7000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Sub HallaCentroYPrecio(ByVal Lista As List(Of ItemReproceso), ByVal Lote As Integer, ByVal Secuencia As Integer, ByRef Centro As Integer, ByRef Precio As Decimal, ByRef TipoOperacion As Integer)

        Centro = 0
        Precio = 0

        For Each Fila As ItemReproceso In Lista
            If Fila.Lote = Lote And Fila.Secuencia = Secuencia Then
                Centro = Fila.Centro
                Precio = Fila.Precio
                TipoOperacion = Fila.TipoOperacion
                Exit Sub
            End If
        Next

    End Sub
    Private Function HallaPrecioCentroTipoOperacion(ByVal TipoOperacion As Integer, ByRef Centro As Integer, ByRef Precio As Decimal) As Boolean

        Dim Dt As New DataTable

        Precio = Refe

        If Not Tablas.Read("SELECT Centro FROM Miselaneas WHERE Codigo = 1 AND Clave = " & TipoOperacion & ";", Conexion, Dt) Then Return False
        Centro = Dt.Rows(0).Item("Centro")

        Dt.Dispose()

        Return True

    End Function
    Private Function HallaUltimaSecuencia(ByVal Lista As List(Of ItemReproceso), ByVal Lote As Integer) As Integer

        If Lista.Count = 0 Then Return 0

        For Each Fila As ItemReproceso In Lista
            If Fila.Lote = Lote Then Return Fila.SecuenciaReproceso
        Next

        Return 0

    End Function
    Private Function HallaUltimaClave(ByVal ConexionStr As String) As Integer

        Dim Dt As New DataTable
        Dim RegAnt As Integer
        Dim Diferencia As Integer

        If Not Tablas.Read("SELECT Clave FROM ReprocesoCabeza ORDER BY Clave;", ConexionStr, Dt) Then End
        If Dt.Rows.Count = 0 Then Dt.Dispose() : Return 1

        RegAnt = 1

        For Each Row As DataRow In Dt.Rows
            Diferencia = Row("Clave") - RegAnt
            If Diferencia <> 0 Then
                Exit For
            Else
                RegAnt = Row("Clave") + 1
            End If
        Next

        Dt.Dispose()
        Return RegAnt

    End Function
    Public Function NumeracionAlternativa(ByVal NumeroViejo As Decimal, ByRef NumeroNuevo As Decimal) As Boolean

        NumeroNuevo = HallaUltimaClave(ConexionLotes)
        If NumeroNuevo = NumeroViejo Then Return False
        If MsgBox("Numeración " & NumeroEditado(NumeroViejo) & " Fue Utilizada. Próxima Numeración es " & NumeroEditado(NumeroNuevo) + vbCrLf + "Desea cambiar Nro. Comprobante (Si/No)?", MsgBoxStyle.YesNo + MsgBoxStyle.Question) = MsgBoxResult.No Then
            Return False
        End If

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Clave) FROM ReprocesoCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function UltimaNumeracionSecuenciaLote(ByVal Lote As Integer, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Secuencia) FROM Lotes WHERE Lote = " & Lote & ";", Miconexion)
                    Dim Ultimo As Integer = Cmd.ExecuteScalar()
                    If Ultimo < 100 Then
                        Return 99
                    Else : Return Ultimo
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If TextComprobante.Text = "" And PAbierto Then
            MsgBox("Falta Numero Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextComprobante.Focus()
            Return False
        End If
        If Val(TextComprobante.Text) = 0 And PAbierto Then
            MsgBox("Falta Numero Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextComprobante.Focus()
            Return False
        End If

        For Each Row As DataRow In DtGridBaja.Rows
            If DiferenciaDias(Row("Fecha"), DateTime1.Value) < 0 Then
                MsgBox("Fecha Reproceso no debe ser menor a fecha del lote a Reprocesar: " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DateTime1.Focus()
                Return False
            End If
        Next
        If DtGrid.Rows.Count = 0 Then
            MsgBox("Debe Informarce los Lotes de Alta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        Dim i As Integer
        For Each Row As DataRow In DtGrid.Rows
            If Row.RowState <> DataRowState.Deleted Then
                If Row("Articulo") = 0 Then
                    MsgBox("Debe Informar Articulo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If Row("Cantidad") = 0 Then
                    MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
                If TieneDecimales(Row("Cantidad")) And Not Row("AGranel") Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID BAJA.             --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridBajas_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridBajas.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridBajas.Columns(e.ColumnIndex).Name = "LoteYSecuenciaBaja" Then
            If Not IsNothing(GridBajas.Rows(e.RowIndex).Cells("LoteBaja").Value) Then
                e.Value = GridBajas.Rows(e.RowIndex).Cells("LoteBaja").Value & "/" & Format(GridBajas.Rows(e.RowIndex).Cells("SecuenciaBaja").Value, "000")
            End If
        End If

        If GridBajas.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If GridBajas.Columns(e.ColumnIndex).Name = "Baja" Then
            If Not IsNothing(GridBajas.Rows(e.RowIndex).Cells("Baja").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = Format(e.Value, "0.00")
                End If
            End If
        End If

        If GridBajas.Columns(e.ColumnIndex).Name = "Merma" Then
            If Not IsNothing(GridBajas.Rows(e.RowIndex).Cells("Merma").Value) Then
                If e.Value = 0 Then
                    e.Value = Format(0, "#")
                Else : e.Value = Format(e.Value, "0.00")
                End If
            End If
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).ReadOnly = False Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Or Grid.Columns(e.ColumnIndex).Name = "Cliente" Or Grid.Columns(e.ColumnIndex).Name = "Calibre" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.CellBeginEdit

        If Grid.Columns(e.ColumnIndex).Name = "Cliente" Or Grid.Columns(e.ColumnIndex).Name = "Calibre" Or Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If Grid.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            e.CellStyle.ForeColor = Color.Red
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Articulo" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cliente" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Calibre" Then
            If TypeOf e.Control Is ComboBox Then
                cb = e.Control
                cb.DropDownStyle = ComboBoxStyle.DropDown
                cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                cb.AutoCompleteSource = AutoCompleteSource.ListItems
            End If
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Articulo") = 0
        e.Row("KilosXUnidad") = 0
        e.Row("LoteYSecuencia") = ""
        e.Row("Cantidad") = 0
        e.Row("Calibre") = 0
        e.Row("Cliente") = 0
        e.Row("AGranel") = False
        e.Row("Medida") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If IsDBNull(e.Row("Articulo")) Then Exit Sub
            If e.ProposedValue <> e.Row("Articulo") Then
                e.Row("Cliente") = 0
                e.Row("Calibre") = 0
                e.Row("Cantidad") = 0
            End If
            e.Row("KilosXUnidad") = HallaKilosXUnidad(e.ProposedValue)
            HallaAGranelYMedida(e.ProposedValue, e.Row("AGranel"), e.Row("Medida"))
            Grid.Refresh()
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub DtGrid_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Cantidad") Then
            If Not IsDBNull(e.Row("Cantidad")) Then
                Calcular()
            End If
        End If

    End Sub



End Class