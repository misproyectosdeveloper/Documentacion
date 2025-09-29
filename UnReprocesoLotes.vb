Public Class UnReprocesoLotes
    Public PReproceso As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtGrid As DataTable
    Dim DtLotes As DataTable
    Dim ds As DataSet
    '
    Dim KilosXUnidadBaja As Double
    Dim cb As ComboBox
    Dim Lote As Integer
    Dim Secuencia As Integer
    Dim Stock As Decimal
    Dim Baja As Decimal
    Dim Merma As Decimal
    Dim EsAGranel As Boolean
    Dim ConexionLotes As String
    Private Sub UnReprocesoLotesNuevos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False

        LlenaComboTablas(ComboDeposito, 19)

        ComboEstado.DataSource = DtEstadoAfectaStockYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = 0

        If PReproceso = 0 Then
            OpcionReproceso.ShowDialog()
            ComboDeposito.SelectedValue = OpcionReproceso.PDeposito
            ComboArticulo.SelectedValue = OpcionReproceso.PArticulo
            Lote = OpcionReproceso.PLote
            Secuencia = OpcionReproceso.PSecuencia
            PAbierto = OpcionReproceso.PAbierto
            Stock = OpcionReproceso.PStock
            If OpcionReproceso.PEsAGranel Then
                TextStock.Text = Stock / 1000
            Else : TextStock.Text = Stock
            End If
            KilosXUnidadBaja = OpcionReproceso.PKilosXUnidad
            Baja = OpcionReproceso.PBaja
            OpcionReproceso.Dispose()
            If Lote = 0 Then Me.Close() : Exit Sub
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

        Dim DtCabezaAlta As DataTable = DtCabeza.Copy
        Dim DtDetalleAlta As DataTable = DtDetalle.Clone
        Dim DtLotesAlta As DataTable = DtLotes.Clone
        Dim DtLotesOriginal As DataTable = DtLotes.Copy

        Dim Cantidad As Decimal
        Dim Row As DataRow

        For Each RowGrid As DataRow In DtGrid.Rows
            If Not RowGrid.RowState = DataRowState.Deleted Then
                Row = DtLotesAlta.NewRow()
                Row("Lote") = DtLotes.Rows(0).Item("Lote")
                Row("Secuencia") = 0
                Row("Deposito") = ComboDeposito.SelectedValue
                Row("Articulo") = RowGrid("Articulo")
                Row("Calibre") = RowGrid("Calibre")
                Row("Cantidad") = RowGrid("Cantidad")
                Row("Fecha") = DateTime1.Value
                Row("Stock") = RowGrid("Cantidad")
                Row("Senia") = DtLotes.Rows(0).Item("Senia")
                Row("LoteOrigen") = DtLotes.Rows(0).Item("LoteOrigen")
                Row("SecuenciaOrigen") = DtLotes.Rows(0).Item("SecuenciaOrigen")
                Row("DepositoOrigen") = DtLotes.Rows(0).Item("DepositoOrigen")
                Row("TipoOperacion") = DtLotes.Rows(0).Item("TipoOperacion")
                Row("ClientePotencial") = RowGrid("Cliente")
                Row("Proveedor") = DtLotes.Rows(0).Item("Proveedor")
                Row("KilosXUnidad") = RowGrid("KilosXUnidad")
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
                Dim TotalW As Decimal = RowGrid("Cantidad") * RowGrid("KilosXUnidad")
                Cantidad = Cantidad + TotalW
                DtLotesAlta.Rows.Add(Row)
            End If
        Next

        DtLotesOriginal.Rows(0).Item("Stock") = DtLotesOriginal.Rows(0).Item("Stock") - Baja
        DtLotesOriginal.Rows(0).Item("BajaReproceso") = DtLotesOriginal.Rows(0).Item("BajaReproceso") + Baja
        DtLotesOriginal.Rows(0).Item("Merma") = DtLotesOriginal.Rows(0).Item("Merma") + Merma

        DtCabezaAlta.Rows(0).Item("Lote") = Lote
        DtCabezaAlta.Rows(0).Item("Secuencia") = Secuencia
        DtCabezaAlta.Rows(0).Item("Merma") = Merma

        For Each RowGrid As DataRow In DtGrid.Rows
            If Not RowGrid.RowState = DataRowState.Deleted Then
                Row = DtDetalleAlta.NewRow()
                Row("Lote") = DtLotes.Rows(0).Item("Lote")
                Row("Secuencia") = 0
                Row("Calibre") = RowGrid("Calibre")
                Row("KilosXUnidad") = RowGrid("KilosXUnidad")
                Row("Alta") = RowGrid("Cantidad")
                Row("ClientePotencial") = RowGrid("Cliente")
                DtDetalleAlta.Rows.Add(Row)
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtCabezaAlta, DtDetalleAlta, DtLotesOriginal, DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'Graba Reproceso.
        Dim Numero As Double = 0
        Dim NumeroLote As Double = 0
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion Reproceso.
            If PAbierto Then
                Numero = CDbl(TextComprobante.Text)
            Else
                Numero = UltimaNumeracion(ConexionN)
                If Numero < 0 Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If

            'Halla Ultima numeracion Lote.
            NumeroLote = UltimaNumeracionLote(ConexionLotes)
            If NumeroLote < 0 Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
            '
            Dim Num As Double = NumeroLote
            DtCabezaAlta.Rows(0).Item("Clave") = Numero
            For Each row1 As DataRow In DtDetalleAlta.Rows
                row1("Clave") = Numero
                Num = Num + 1
                row1("Secuencia") = Num
            Next
            Num = NumeroLote
            For Each row1 As DataRow In DtLotesAlta.Rows
                Num = Num + 1
                row1("Secuencia") = Num
            Next

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

            Resul = AltaReproceso(DtCabezaAlta, DtDetalleAlta, DtLotesAlta, DtLotesOriginal, DtAsientoCabeza, DtAsientoDetalle)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And PAbierto Then Resul = -10 : Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            MsgBox("Numero Reproceso Ya existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            PreparaArchivos()
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

        Dim DtLotesGenerados As New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For Each Row As DataRow In DtDetalle.Rows
            If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & ComboDeposito.SelectedValue & ";", ConexionLotes, DtLotesGenerados) Then Me.Close() : Exit Sub
        Next

        For Each Row As DataRow In DtLotesGenerados.Rows
            If Row("Cantidad") <> Row("Stock") Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                MsgBox("Lotes han tenido movimientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                DtLotesGenerados.Dispose()
                Exit Sub
            End If
        Next

        For Each Row As DataRow In DtLotesGenerados.Rows
            '    Row.Delete()   instruccion modificada.
            Row("Cantidad") = 0
            Row("Stock") = 0
        Next

        DtLotes.Rows(0).Item("BajaReproceso") = DtLotes.Rows(0).Item("BajaReproceso") - Baja
        DtLotes.Rows(0).Item("Merma") = DtLotes.Rows(0).Item("Merma") - Merma
        DtLotes.Rows(0).Item("Stock") = DtLotes.Rows(0).Item("Stock") + Baja
        DtCabeza.Rows(0).Item("Estado") = 3

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
                        DaP.Update(DtCabeza)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtLotes)
                        DaP.Update(DtLotesGenerados)
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

        PreparaArchivos()

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

        ArmaMerma()

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
            Row("Clave") = 0
            Row("Lote") = Lote
            Row("Secuencia") = Secuencia
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Fecha") = Now
            Row("Baja") = Baja
            Row("KilosxUnidad") = KilosXUnidadBaja
            Row("Merma") = 0
            Row("Estado") = 1
            Row("Comentario") = ""
            DtCabeza.Rows.Add(Row)
        End If

        EsAGranel = EsArticuloAGranel(HallaArticulo(DtCabeza.Rows(0).Item("Lote"), DtCabeza.Rows(0).Item("Secuencia"), ConexionLotes))

        MuestraCabeza()

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM ReprocesoDetalle WHERE Clave = " & PReproceso & ";", ConexionLotes, DtDetalle) Then Me.Close() : Exit Sub

        Dim Sql As String = "SELECT * FROM Lotes Where Lote = " & Lote & " AND Secuencia = " & Secuencia & _
               " AND Deposito = " & ComboDeposito.SelectedValue & ";"
        DtLotes = New DataTable
        If Not Tablas.Read(Sql, ConexionLotes, DtLotes) Then Me.Close() : Exit Sub
        If DtLotes.Rows.Count = 0 Then
            MsgBox("Lote No encontradao en la base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If
        If PReproceso = 0 Then
            If Stock > DtLotes.Rows(0).Item("Stock") Then
                MsgBox("Stock del Lote ha cambiado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Close() : Exit Sub
            End If
        End If

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
            If PAbierto Then
                TextComprobante.ReadOnly = False
            Else : TextComprobante.ReadOnly = True
            End If
            Panel2.Enabled = True
            Panel1.Enabled = True
            Grid.Columns("LoteYSecuencia").Visible = False
        Else
            TextComprobante.ReadOnly = True
            Panel2.Enabled = False
            Panel1.Enabled = False
            Grid.Columns("LoteYSecuencia").Visible = True
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

        TextCantidadBaja.Text = Row("Baja")
        TextMerma.Text = Row("Merma")

        Merma = Row("Merma")

        Enlace = New Binding("Text", MiEnlazador, "KilosXUnidad")
        AddHandler Enlace.Format, AddressOf FormatKilos
        TextKilosPorUnidad.DataBindings.Clear()
        TextKilosPorUnidad.DataBindings.Add(Enlace)

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

        If PReproceso <> 0 Then
            Lote = DtCabeza.Rows(0).Item("Lote")
            Secuencia = DtCabeza.Rows(0).Item("Secuencia")
            Baja = DtCabeza.Rows(0).Item("Baja")
            KilosXUnidadBaja = DtCabeza.Rows(0).Item("KilosXUnidad")
            ComboArticulo.SelectedValue = HallaArticulo(Lote, Secuencia, ConexionLotes)
        End If

        TextLote.Text = Lote & "/" & Format(Secuencia, "000")

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
    Private Function AltaReproceso(ByVal DtCabezaW As DataTable, ByVal DtDetalleW As DataTable, ByVal DtLotesW As DataTable, ByVal DtLotesOriginalW As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

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
                        DaP.Update(DtCabezaW)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ReprocesoDetalle;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtDetalleW)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtLotesW)
                        DaP.Update(DtLotesOriginalW)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtAsientoCabeza)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", Miconexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtAsientoDetalle)
                    End Using

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

        HallaEspecieYVariedad(ComboArticulo.SelectedValue, Especie, variedad)

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
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

        Dim KilosXUnidad As DataColumn = New DataColumn("KilosXUnidad")
        KilosXUnidad.DataType = System.Type.GetType("System.Double")
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
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Cliente As New DataColumn("Cliente")
        Cliente.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Cliente)

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
    Private Function CalculaKilos() As Double

        Dim Total As Double = 0

        For i As Integer = 0 To Grid.Rows.Count - 2
            If Not IsDBNull(Grid.Rows(i).Cells("Cantidad").Value) And Not IsDBNull(Grid.Rows(i).Cells("KilosXUnidad").Value) Then
                Total = Total + Grid.Rows(i).Cells("Cantidad").Value * Grid.Rows(i).Cells("KilosXUnidad").Value
            End If
        Next
        Return Total

    End Function
    Private Sub ArmaMerma()

        Dim Cantidad As Double = CalculaKilos()

        If Cantidad <> 0 Then
            Merma = Baja - Cantidad / KilosXUnidadBaja
            Merma = Trunca(Merma)
            TextMerma.Text = Format(Merma, "0.00")
        End If

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtCabezaAlta As DataTable, ByVal DtDetalleAlta As DataTable, ByVal DtLotesOriginalAux As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim RowsBusqueda() As DataRow
        Dim Precio As Decimal = 0
        Dim Centro As Integer = 0

        If Funcion = "A" Then
            RowsBusqueda = DtLotesOriginalAux.Select("Lote = " & DtCabezaAlta.Rows(0).Item("Lote") & " AND Secuencia = " & DtCabezaAlta.Rows(0).Item("Secuencia"))
            If RowsBusqueda.Length = 0 Then
                MsgBox("Error, Lote a Reprocesar No Encontrado.")
                Return False
            End If
            If RowsBusqueda(0).Item("TipoOperacion") <> 4 Then
                If Not HallaPrecioCentroTipoOperacion(RowsBusqueda(0).Item("TipoOperacion"), Centro, Precio) Then Return False
            Else
                Dim Operacion As Integer
                If PAbierto Then
                    Operacion = 1
                Else : Operacion = 2
                End If
                Dim Costeo = HallaCosteoLote(Operacion, RowsBusqueda(0).Item("Lote"))
                If Costeo = -1 Then Return False
                If Not HallaPrecioYCentroCosteo(RowsBusqueda(0).Item("Proveedor"), Costeo, Centro, Precio) Then Return False
            End If
            'Para Neto(Baja) 
            Dim Fila As New ItemLotesParaAsientos
            Fila.Centro = Centro
            Fila.MontoNeto = Trunca(Precio * RowsBusqueda(0).Item("KilosXUnidad") * Baja)
            If RowsBusqueda(0).Item("TipoOperacion") = 1 Then Fila.Clave = -111
            If RowsBusqueda(0).Item("TipoOperacion") = 2 Then Fila.Clave = -110
            If RowsBusqueda(0).Item("TipoOperacion") = 3 Then Fila.Clave = -113
            If RowsBusqueda(0).Item("TipoOperacion") = 4 Then Fila.Clave = -112
            ListaLotesParaAsiento.Add(Fila)
            'Para Merma
            Fila = New ItemLotesParaAsientos
            Fila.Centro = Centro
            Fila.MontoNeto = Trunca(Precio * RowsBusqueda(0).Item("KilosXUnidad") * Merma)
            If RowsBusqueda(0).Item("TipoOperacion") = 1 Then Fila.Clave = -101
            If RowsBusqueda(0).Item("TipoOperacion") = 2 Then Fila.Clave = -100
            If RowsBusqueda(0).Item("TipoOperacion") = 3 Then Fila.Clave = -103
            If RowsBusqueda(0).Item("TipoOperacion") = 4 Then Fila.Clave = -102
            ListaLotesParaAsiento.Add(Fila)
        End If

        'Para Altas.
        If Funcion = "A" Then
            For Each Row As DataRow In DtDetalleAlta.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Dim Fila As New ItemLotesParaAsientos
                    Fila.Centro = Centro
                    Fila.MontoNeto = Trunca(Precio * Row("KilosXUnidad") * Row("Alta"))
                    If RowsBusqueda(0).Item("TipoOperacion") = 1 Then Fila.Clave = -106
                    If RowsBusqueda(0).Item("TipoOperacion") = 2 Then Fila.Clave = -105
                    If RowsBusqueda(0).Item("TipoOperacion") = 3 Then Fila.Clave = -108
                    If RowsBusqueda(0).Item("TipoOperacion") = 4 Then Fila.Clave = -107
                    ListaLotesParaAsiento.Add(Fila)
                End If
            Next
        End If

        If Not Asiento(7000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Function HallaPrecioCentroTipoOperacion(ByVal TipoOperacion As Integer, ByRef Centro As Integer, ByRef Precio As Double) As Boolean

        Dim Dt As New DataTable

        Precio = Refe

        If Not Tablas.Read("SELECT Centro FROM Miselaneas WHERE Codigo = 1 AND Clave = " & TipoOperacion & ";", Conexion, Dt) Then Return False
        Centro = Dt.Rows(0).Item("Centro")

        Dt.Dispose()

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
    Private Function UltimaNumeracionLote(ByVal ConexionStr) As Double

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
        If DateTime1.Value < DtLotes.Rows(0).Item("Fecha") Then
            MsgBox("Fecha Reproceso no debe ser menor a fecha del lote a Reprocesar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            DateTime1.Focus()
            Return False
        End If
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

        Dim Totalkilos As Double = CalculaKilos()
        If Totalkilos > Baja * KilosXUnidadBaja Then
            If MsgBox("Cantidad Supera Stock Origen. Desea Continuar?.", MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Return False
            End If
        End If

        Return True

    End Function
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
                'Dim Totalkilos As Double = CalculaKilos()   'Se anulo para que genere merma positiva (con signo negativo).
                'If Totalkilos > Baja * KilosXUnidadBaja Then
                '  MsgBox("Cantidad Supera Stock Origen.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                '  e.Row("Cantidad") = 0
                ' End If
                ArmaMerma()
            End If
        End If

    End Sub



End Class