Public Class UnaDiferenciaInventario
    Public PComprobante As Double
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim ErrorFatal As Boolean
    Dim cb As ComboBox
    Dim ConexionDiferencia As String
    Dim DtComprobanteCabeza As DataTable
    Dim DtComprobanteDetalle As DataTable
    Dim DtGrid As DataTable
    Private Sub UnaDiferenciaInventario_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        DateTime1.Value = Now.Date

        LlenaComboTablas(ComboDeposito, 19)

        If PComprobante = 0 Then
            OpcionTransferencia.PEsDescarte = True
            OpcionTransferencia.ShowDialog()
            If OpcionTransferencia.POrigen = 0 Then Me.Close() : Exit Sub
            ComboDeposito.SelectedValue = OpcionTransferencia.POrigen
            PAbierto = OpcionTransferencia.PAbierto
            OpcionTransferencia.Dispose()
        End If

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        LlenaCombosGrid()

        GModificacionOk = False

        PreparaArchivos()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

    End Sub
    Private Sub UnaDiferenciaInventario_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PAbierto And PComprobante = 0 Then
            TextComprobante.Focus()
        Else
            Grid.Focus()
        End If

    End Sub
    Private Sub UnaDiferenciaInventario_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PComprobante <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        If PComprobante = 0 Then
            HacerAlta()
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PComprobante = 0
        UnaDiferenciaInventario_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PComprobante = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Comprobante Ya esta Anulado. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If HallaArticuloDeshabilitado(DtGrid) Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        Dim DtAsientoCabeza As New DataTable
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(6056, PComprobante, DtAsientoCabeza, ConexionDiferencia) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Comprobante se Anulara. ¿Desea Anularlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtComprobanteCabeza.Rows(0).Item("Estado") = 3

        Dim Dt As New DataTable

        Using MiConexion As New OleDb.OleDbConnection(ConexionDiferencia)
            Dim Trans As OleDb.OleDbTransaction
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM DiferenciaCabeza;", MiConexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.SelectCommand.Transaction = Trans
                    DaP.Update(DtComprobanteCabeza)
                End Using
                '
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.SelectCommand.Transaction = Trans
                    DaP.Update(DtAsientoCabeza)
                End Using
                '
                Dim Ok As Boolean = True
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", MiConexion)
                    DaP.SelectCommand.Transaction = Trans
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    For Each Row As DataRow In DtGrid.Rows
                        If Not Row.RowState = DataRowState.Deleted Then
                            If Not BorraLotes(Row("Lote"), Row("Secuencia"), Row("Cantidad"), Dt) Then Ok = False : Exit For
                            DaP.Update(Dt)
                        End If
                    Next
                End Using
                If Not Ok Then
                    Trans.Rollback()
                Else : GModificacionOk = True
                    Trans.Commit()
                    MsgBox("Comprobante Anulado Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                End If
            Catch ex As DBConcurrencyException
                Trans.Rollback()
                MsgBox("Error Otro Usuario Modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Catch err As OleDb.OleDbException
                Trans.Rollback()
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Finally
                Trans = Nothing
                Dt.Dispose()
            End Try
        End Using

        PreparaArchivos()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PComprobante = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 6056
        If PAbierto Then
            ListaAsientos.PDocumentoB = PComprobante
        Else
            ListaAsientos.PDocumentoN = PComprobante
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

        EsNumerico(e.KeyChar, TextComprobante.Text, 0)

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If
        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub PreparaArchivos()

        CreaDtGrid()

        Dim Row As DataRow

        If PAbierto Then
            ConexionDiferencia = Conexion
        Else : ConexionDiferencia = ConexionN
        End If

        DtComprobanteCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM DiferenciaCabeza WHERE Clave = " & PComprobante & ";", ConexionDiferencia, DtComprobanteCabeza) Then Me.Close() : Exit Sub
        If PComprobante <> 0 And DtComprobanteCabeza.Rows.Count = 0 Then
            MsgBox("Comprobante No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PComprobante = 0 Then
            Row = DtComprobanteCabeza.NewRow()
            Row("Clave") = 0
            Row("Deposito") = ComboDeposito.SelectedValue
            Row("Fecha") = DateTime1.Value
            Row("Estado") = 1
            Row("Comentario") = ""
            DtComprobanteCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtComprobanteDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM DiferenciaDetalle WHERE Clave = " & PComprobante & ";", ConexionDiferencia, DtComprobanteDetalle) Then Me.Close() : Exit Sub

        DtGrid.Clear()

        For Each Row1 As DataRow In DtComprobanteDetalle.Rows
            Row = DtGrid.NewRow()
            If PAbierto Then
                Row("Operacion") = 1
            Else : Row("Operacion") = 2
            End If
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Articulo") = HallaArticulo(Row1("Lote"), Row1("Secuencia"), ConexionDiferencia)
            Row("Stock") = HallaStockLote(Row1("Lote"), Row1("Secuencia"), DtComprobanteCabeza.Rows(0).Item("Deposito"), ConexionDiferencia)
            Row("Cantidad") = Row1("Cantidad")
            DtGrid.Rows.Add(Row)
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        If PComprobante = 0 Then
            If PAbierto Then
                TextComprobante.ReadOnly = False
            Else : TextComprobante.ReadOnly = True
            End If
            Grid.ReadOnly = False
            ButtonEliminarLinea.Enabled = True
            Panel1.Enabled = True
            Grid.Columns("Stock").Visible = True
            Grid.Columns("Lupa").Visible = True
        Else
            Grid.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            Panel1.Enabled = False
            Grid.Columns("Stock").Visible = False
            Grid.Columns("Lupa").Visible = False
        End If

    End Sub
    Private Sub HacerAlta()

        Dim Row As DataRow
        Dim DtCabeza As DataTable = DtComprobanteCabeza.Copy
        Dim DtDetalle As DataTable = DtComprobanteDetalle.Clone

        For Each row1 As DataRow In DtGrid.Rows
            Row = DtDetalle.NewRow()
            Row("Lote") = row1("Lote")
            Row("Secuencia") = row1("Secuencia")
            Row("Cantidad") = row1("Cantidad")
            DtDetalle.Rows.Add(Row)
        Next

        'Arma Lista de conceptos para Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        '
        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            If Not ArmaArchivosAsiento("A", DtAsientoCabeza, DtAsientoDetalle) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
        End If

        'Graba transferencia.
        Dim Numero As Double = 0
        Dim Resul As Double
        Dim NumeroAsiento As Double = 0

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For i As Integer = 1 To 50
            Numero = UltimaNumeracion(ConexionDiferencia)
            If Numero < 0 Then
                MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            '  End If
            DtCabeza.Rows(0).Item("Clave") = Numero
            For Each row1 As DataRow In DtDetalle.Rows
                row1("Clave") = Numero
            Next

            'Halla Ultima numeracion Asiento.
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionDiferencia)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = Numero
                For Each Row In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = AltaDescarte(DtCabeza, DtDetalle, DtAsientoCabeza, DtAsientoDetalle)

            If Resul = -3 Then Exit For
            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And PAbierto Then Resul = -10 : Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            MsgBox("Numero Comprobante Ya existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            PComprobante = Numero
            PreparaArchivos()
        End If

        DtCabeza.Dispose()
        DtDetalle.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function AltaDescarte(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable, ByVal DtAsientoCabeza As DataTable, ByVal DtAsientoDetalle As DataTable) As Double

        Dim Dt As New DataTable
        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionDiferencia)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM DiferenciaCabeza;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtCabeza)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM DiferenciaDetalle;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtDetalle)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtAsientoCabeza)
                    End Using
                    '
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtAsientoDetalle)
                    End Using
                    '
                    Dim Ok As Boolean = True
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        For Each Row As DataRow In DtGrid.Rows
                            If Not Row.RowState = DataRowState.Deleted Then
                                If Not ArmaLotes(Row("Lote"), Row("Secuencia"), Row("Cantidad"), Dt) Then Ok = False : Exit For
                                DaP.Update(Dt)
                            End If
                        Next
                    End Using
                    '
                    If Not Ok Then
                        Trans.Rollback()
                        Return -3
                    Else
                        Trans.Commit()
                        Return 1000
                    End If
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
                    Dt.Dispose()
                End Try
            Catch ex As OleDb.OleDbException
                Return -2
            End Try
        End Using

    End Function
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtComprobanteCabeza

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
        AddHandler Enlace.Format, AddressOf FormatTexto
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
    Private Function ArmaLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Cantidad As Decimal, ByRef Dt As DataTable) As Boolean

        Dim Sql As String

        Dt = New DataTable

        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                       " AND Deposito = " & ComboDeposito.SelectedValue & ";"
        If Not Tablas.Read(Sql, ConexionDiferencia, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) - Cantidad
        Dt.Rows(0).Item("DiferenciaInventario") = CDec(Dt.Rows(0).Item("DiferenciaInventario")) + Cantidad
        If Dt.Rows(0).Item("Stock") < 0 Then
            MsgBox("Stock negativo en Lote " & Lote & "/" & Format(Secuencia, "000"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If

        Return True

    End Function
    Private Function BorraLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Cantidad As Decimal, ByRef Dt As DataTable) As Boolean

        Dim Sql As String

        Dt = New DataTable

        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                   " AND Deposito = " & ComboDeposito.SelectedValue & ";"
        If Not Tablas.Read(Sql, ConexionDiferencia, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        Dt.Rows(0).Item("Stock") = CDec(Dt.Rows(0).Item("Stock")) + Cantidad
        Dt.Rows(0).Item("DiferenciaInventario") = CDec(Dt.Rows(0).Item("DiferenciaInventario")) - Cantidad

        Return True

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        If PComprobante = 0 Then
            Articulo.DataSource = ArticulosActivos()
        Else
            Articulo.DataSource = TodosLosArticulos()
        End If
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As DataColumn = New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Lote As DataColumn = New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Lote)

        Dim Secuencia As DataColumn = New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Secuencia)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Articulo)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtAsientoCabeza As DataTable, ByRef DtAsientoDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim Precio As Double = 0
        Dim Tipo As Integer
        Dim Centro As Integer
        Dim Item As New ItemListaConceptosAsientos
        Dim Cantidad As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            Cantidad = Row("Cantidad")
            Tipo = 0
            Centro = 0
            Dim Fila2 As New ItemLotesParaAsientos
            '
            If Row("Operacion") = 1 Then
                HallaCentroTipoOperacion(Row("Lote"), Row("Secuencia"), Conexion, Tipo, Centro)
            Else : HallaCentroTipoOperacion(Row("Lote"), Row("Secuencia"), ConexionN, Tipo, Centro)
            End If
            If Centro <= 0 Then
                MsgBox("Error en Tipo Operacion en Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"))
                Return False
            End If
            If Tipo = 4 Then
                Dim Negocio As Integer = HallaProveedorLote(Row("Operacion"), Row("Lote"), Row("Secuencia"))
                If Negocio <= 0 Then
                    MsgBox("Error al Leer Lotes " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"))
                    Return False
                End If
                Dim Costeo = HallaCosteoLote(Row("Operacion"), Row("Lote"))
                If Costeo = -1 Then Return False
                If Not HallaPrecioYCentroCosteo(Negocio, Costeo, Centro, Precio) Then Return False
            Else
                Precio = Refe
            End If
            '
            Dim Kilos As Double
            Dim Iva As Double
            HallaKilosIva(Row("Articulo"), Kilos, Iva)

            Fila2.Centro = Centro
            Fila2.MontoNeto = Trunca(Precio * Cantidad * Kilos)
            If Tipo = 1 Then Fila2.Clave = 301 'consignacion
            If Tipo = 2 Then Fila2.Clave = 300 'reventa
            If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 302 'costeo
            ListaLotesParaAsiento.Add(Fila2)
        Next

        If Not Asiento(6056, 0, ListaConceptos, DtAsientoCabeza, DtAsientoDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, DateTime1.Value, 0) Then Return False

        Return True

    End Function
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Clave) FROM DiferenciaCabeza;", Miconexion)
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
    Private Function Valida() As Boolean

        If ComboDeposito.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Deposito Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDeposito.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Lotes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        For i As Integer = 0 To Grid.RowCount - 2
            If Not IsNumeric(Grid.Rows(i).Cells("Cantidad").Value) Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value = 0 Then
                MsgBox("Debe Informar Cantidad en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
            If Grid.Rows(i).Cells("Cantidad").Value <> 0 Then
                Dim AGranel As Boolean, Medida As String
                HallaAGranelYMedida(Grid.Rows(i).Cells("Articulo").Value, AGranel, Medida)
                If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not AGranel Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

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
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub Grid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellValueChanged

        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Articulo")) Then
                Grid.Rows(e.RowIndex).Cells("Lote").Value = 0
                Grid.Rows(e.RowIndex).Cells("Secuencia").Value = 0
                Grid.Rows(e.RowIndex).Cells("LoteYSecuencia").Value = ""
                Grid.Rows(e.RowIndex).Cells("Cantidad").Value = 0
                Grid.Rows(e.RowIndex).Cells("Stock").Value = 0
                Grid.Refresh()
            End If
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            If Grid.Rows(e.RowIndex).Cells("Lote").Value = 0 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = Nothing : Exit Sub
            e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = ImageList1.Images.Item("Lupa")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then Format(e.Value, "#")
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Lupa" Then Exit Sub

        If ComboDeposito.SelectedValue = 0 Then
            MsgBox("Falta Seleccionar depositos Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then
            MsgBox("Falta Seleccionar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsSeleccionaLote = True
        SeleccionarVarios.PDeposito = ComboDeposito.SelectedValue
        SeleccionarVarios.PArticulo = Grid.Rows(e.RowIndex).Cells("Articulo").Value
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.ShowDialog()
        If SeleccionarVarios.PLote = 0 Then SeleccionarVarios.Dispose() : Exit Sub
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid.Select("Lote = " & SeleccionarVarios.PLote & " AND Secuencia = " & SeleccionarVarios.PSecuencia)
        If RowsBusqueda.Length <> 0 Then
            MsgBox("Lote ya existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            SeleccionarVarios.Dispose()
            Exit Sub
        End If

        Grid.Rows(e.RowIndex).Cells("Lote").Value = SeleccionarVarios.PLote
        Grid.Rows(e.RowIndex).Cells("Secuencia").Value = SeleccionarVarios.PSecuencia
        Grid.Rows(e.RowIndex).Cells("Stock").Value = SeleccionarVarios.PStock
        Grid.Rows(e.RowIndex).Cells("Cantidad").Value = 0
        If PAbierto Then
            Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1
        Else : Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2
        End If
        SeleccionarVarios.Dispose()

        Grid.Refresh()
        Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Cantidad")
        Grid.BeginEdit(True)

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        If PComprobante <> 0 Then e.Row.Delete()

        e.Row("Operacion") = 0
        e.Row("Lote") = 0
        e.Row("Secuencia") = 0
        e.Row("Articulo") = 0
        e.Row("Stock") = 0
        e.Row("Cantidad") = 0

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If Not IsDBNull(e.Row("Cantidad")) Then
                If e.ProposedValue <> 0 Then
                    If e.Row("Articulo") = 0 Then
                        MsgBox("Debe Selccionar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        e.ProposedValue = 0
                        Exit Sub
                    End If
                    If e.Row("Lote") = 0 Then
                        MsgBox("Debe Selccionar Lote.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        e.ProposedValue = 0
                        Exit Sub
                    End If
                    If e.ProposedValue > e.Row("Stock") Then
                        MsgBox("Cantidad Supera Stock.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                        e.ProposedValue = e.Row("Cantidad")
                        Exit Sub
                    End If
                End If
            End If
        End If

    End Sub

End Class