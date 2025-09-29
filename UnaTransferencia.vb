Public Class UnaTransferencia
    Public PTrans As Double
    Public PAbierto As Double
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim DtTransCabeza As DataTable
    Dim DtTransDetalle As DataTable
    ' 
    Dim ErrorFatal As Boolean
    Dim cb As ComboBox
    Dim ConexionTrans As String
    Private Sub UnaTrasladoDeMercaderia_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        If Not PermisoEscritura(5) Then PBloqueaFunciones = True

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        LlenaComboTablas(ComboDepositoOrigen, 19)
        LlenaComboTablas(ComboDepositoDestino, 19)

        If PTrans = 0 Then
            OpcionTransferencia.ShowDialog()
            If OpcionTransferencia.POrigen = 0 Then OpcionTransferencia.Dispose() : Me.Close() : Exit Sub
            ComboDepositoOrigen.SelectedValue = OpcionTransferencia.POrigen
            ComboDepositoDestino.SelectedValue = OpcionTransferencia.PDestino
            PAbierto = OpcionTransferencia.PAbierto
            OpcionTransferencia.Dispose()
        End If

        If Not LLenaComboEstado() Then Me.Close() : Exit Sub

        LlenaCombosGrid()

        CreaDtGrid()

        GModificacionOk = False

        ArmaArchivos()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid_NewRow)

    End Sub
    Private Sub UnaTransferencia_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        TextComprobante.Focus()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PTrans <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        MiEnlazador.EndEdit()
        Grid.EndEdit()

        If Not Valida() Then Exit Sub

        Dim Row As DataRow

        Dim DtCabeza As DataTable = DtTransCabeza.Copy
        Dim DtDetalle As DataTable = DtTransDetalle.Clone

        For Each row1 As DataRow In DtGrid.Rows
            Row = DtDetalle.NewRow()
            Row("Lote") = row1("Lote")
            Row("Secuencia") = row1("Secuencia")
            Row("Cantidad") = row1("Cantidad")
            DtDetalle.Rows.Add(Row)
        Next

        'Graba transferencia.
        Dim Resul As Double

        For i As Integer = 1 To 50
            DtCabeza.Rows(0).Item("Transferencia") = CDbl(TextComprobante.Text)
            For Each row1 As DataRow In DtDetalle.Rows
                row1("Transferencia") = CDbl(TextComprobante.Text)
            Next

            Resul = AltaTransferencia(DtCabeza, DtDetalle)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And PAbierto Then Resul = -10 : Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -10 Then
            MsgBox("Numero Transferencia Ya existe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("ERROR Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            PTrans = CDbl(TextComprobante.Text)
            UnaTrasladoDeMercaderia_Load(Nothing, Nothing)
        End If

        DtCabeza.Dispose()
        DtDetalle.Dispose()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PTrans = 0
        UnaTrasladoDeMercaderia_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PTrans = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Transferencia Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Transferencia se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        DtTransCabeza.Rows(0).Item("estado") = 3

        Dim Dt As New DataTable
        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionTrans)
            Try
                MiConexion.Open()
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransCabeza;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtTransCabeza)
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
                    '               
                    If Not Ok Then
                        Trans.Rollback()
                    Else : Trans.Commit()
                        MsgBox("Transferencia Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                        GModificacionOk = True
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
            Catch err As OleDb.OleDbException
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        UnaTrasladoDeMercaderia_Load(Nothing, Nothing)

    End Sub
    Private Sub TextComprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        EsNumerico(e.KeyChar, TextComprobante.Text, 0)

    End Sub
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If Grid.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If
        Grid.Rows.Remove(Grid.CurrentRow)

    End Sub
    Private Sub ArmaArchivos()

        Dim Row As DataRow

        If PAbierto Then
            ConexionTrans = Conexion
        Else : ConexionTrans = ConexionN
        End If

        DtTransCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM TransCabeza WHERE Transferencia = " & PTrans & ";", ConexionTrans, DtTransCabeza) Then Me.Close() : Exit Sub
        If PTrans <> 0 And DtTransCabeza.Rows.Count = 0 Then
            MsgBox("Transferencia No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PTrans = 0 Then
            Row = DtTransCabeza.NewRow()
            Row("Transferencia") = 0
            Row("Origen") = ComboDepositoOrigen.SelectedValue
            Row("Destino") = ComboDepositoDestino.SelectedValue
            Row("Fecha") = Now
            Row("Estado") = 1
            Row("Comentario") = ""
            DtTransCabeza.Rows.Add(Row)
        End If

        MuestraCabeza()

        DtTransDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM TransDetalle WHERE Transferencia = " & PTrans & ";", ConexionTrans, DtTransDetalle) Then Me.Close() : Exit Sub

        DtGrid.Clear()

        For Each Row1 As DataRow In DtTransDetalle.Rows
            Row = DtGrid.NewRow()
            If PAbierto Then
                Row("Operacion") = 1
            Else : Row("Operacion") = 2
            End If
            Row("Lote") = Row1("Lote")
            Row("Secuencia") = Row1("Secuencia")
            Row("Articulo") = HallaArticulo(Row1("Lote"), Row1("Secuencia"), ConexionTrans)
            Row("Stock") = HallaStockLote(Row1("Lote"), Row1("Secuencia"), DtTransCabeza.Rows(0).Item("Origen"), ConexionTrans)
            Row("Cantidad") = Row1("Cantidad")
            Row("AGranel") = False
            Row("Medida") = ""
            HallaAGranelYMedida(Row("Articulo"), Row("AGranel"), Row("Medida"))
            DtGrid.Rows.Add(Row)
        Next

        Grid.DataSource = DtGrid

        If PTrans = 0 Then
            TextComprobante.ReadOnly = False
            Grid.ReadOnly = False
            ButtonEliminarLinea.Enabled = True
            Panel1.Enabled = True
            Grid.Columns("Stock").Visible = True
            Grid.Columns("Lupa").Visible = True
        Else
            TextComprobante.ReadOnly = True
            Grid.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            Panel1.Enabled = False
            Grid.Columns("Stock").Visible = False
            Grid.Columns("Lupa").Visible = False
        End If

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtTransCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Transferencia")
        AddHandler Enlace.Format, AddressOf FormatComprobante
        TextComprobante.DataBindings.Clear()
        TextComprobante.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Origen")
        ComboDepositoOrigen.DataBindings.Clear()
        ComboDepositoOrigen.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Destino")
        ComboDepositoDestino.DataBindings.Clear()
        ComboDepositoDestino.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Return True

    End Function
    Private Sub FormatComprobante(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Format(Numero.Value, "#")

    End Sub
    Private Function AltaTransferencia(ByVal DtCabeza As DataTable, ByVal DtDetalle As DataTable) As Double

        Dim Dt As New DataTable
        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionTrans)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                Try
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransCabeza;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtCabeza)
                    End Using
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM TransDetalle;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(DtDetalle)
                    End Using
                    '
                    Dim Ok As Boolean = True
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Lotes;", MiConexion)
                        DaP.SelectCommand.Transaction = Trans
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        For Each Row As DataRow In DtGrid.Rows
                            If Not Row.RowState = DataRowState.Deleted Then
                                If Not ArmaLotes(Row("Lote"), Row("Secuencia"), Row("Cantidad"), Dt) Then Ok = False : Exit For
                                DaP.Update(Dt)
                            End If
                        Next
                    End Using
                    If Not Ok Then
                        Trans.Rollback()
                        Return 0
                    Else
                        Trans.Commit()
                        Return 1000
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Finally
                    Dt.Dispose()
                    Trans = Nothing
                End Try
            Catch ex As OleDb.OleDbException
                Return -2
            End Try
     End Using

    End Function
    Private Function ArmaLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Cantidad As Double, ByRef Dt As DataTable) As Boolean

        Dim Sql As String

        'deposito origen.
        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                       " AND Deposito = " & ComboDepositoOrigen.SelectedValue & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        Dt.Rows(0).Item("Stock") = Dt.Rows(0).Item("Stock") - Cantidad
        Dt.Rows(0).Item("Traslado") = Dt.Rows(0).Item("Traslado") + Cantidad
        If Dt.Rows(0).Item("Stock") < 0 Then
            MsgBox("Stock negativo en Lote " & Lote & "/" & Format(Secuencia, "000"), MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        'deposito destino.
        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                   " AND Deposito = " & ComboDepositoDestino.SelectedValue & ";"
        If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
        If Dt.Rows.Count = 1 Then
            Dim row As DataRow
            row = Dt.NewRow()
            row("Lote") = Lote
            row("Secuencia") = Secuencia
            row("Deposito") = ComboDepositoDestino.SelectedValue
            row("Articulo") = Dt.Rows(0).Item("Articulo")
            row("Calibre") = Dt.Rows(0).Item("Calibre")
            row("Cantidad") = Cantidad
            row("LoteOrigen") = Dt.Rows(0).Item("LoteOrigen")
            row("SecuenciaOrigen") = Dt.Rows(0).Item("SecuenciaOrigen")
            row("DepositoOrigen") = Dt.Rows(0).Item("DepositoOrigen")
            row("TipoOperacion") = Dt.Rows(0).Item("TipoOperacion")
            row("Senia") = Dt.Rows(0).Item("Senia")
            row("Directo") = Dt.Rows(0).Item("Directo")
            row("ClientePotencial") = Dt.Rows(0).Item("ClientePotencial")
            row("Proveedor") = Dt.Rows(0).Item("Proveedor")
            row("KilosXUnidad") = Dt.Rows(0).Item("KilosXUnidad")
            row("Fecha") = DateTime1.Value
            row("Stock") = Cantidad
            row("Traslado") = 0
            row("BajaReproceso") = 0
            row("Descarte") = 0
            row("Merma") = 0
            row("Baja") = 0
            row("Liquidado") = 0
            row("PrecioF") = 0
            row("PrecioCompra") = 0
            row("PrecioPorLista") = 0
            row("PermisoImp") = 0
            row("MermaTr") = 0
            row("DiferenciaInventario") = 0
            Dt.Rows.Add(row)
        Else
            Dt.Rows(1).Item("Stock") = Dt.Rows(1).Item("Stock") + Cantidad
            Dt.Rows(1).Item("Traslado") = Dt.Rows(1).Item("Traslado") - Cantidad
            'traslado puede ser < 0 o > 0  esto es para no modificar Cantidad.
        End If

        Return True

    End Function
    Private Function BorraLotes(ByVal Lote As Integer, ByVal Secuencia As Integer, ByVal Cantidad As Double, ByRef Dt As DataTable) As Boolean

        Dim Sql As String

        'deposito destino.
        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                       " AND Deposito = " & ComboDepositoDestino.SelectedValue & ";"
        Dt = New DataTable
        If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        Dt.Rows(0).Item("Stock") = Dt.Rows(0).Item("Stock") - Cantidad
        Dt.Rows(0).Item("Traslado") = Dt.Rows(0).Item("Traslado") + Cantidad
        If Dt.Rows(0).Item("Stock") < 0 Then
            MsgBox("Stock negativo en Lote " & Lote & "/" & Format(Secuencia, "000") & ". Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        'deposito origen.
        Sql = "SELECT * FROM Lotes WHERE Lote = " & Lote & " AND Secuencia = " & Secuencia & _
                   " AND Deposito = " & ComboDepositoOrigen.SelectedValue & ";"
        If Not Tablas.Read(Sql, ConexionTrans, Dt) Then Return False
        If Dt.Rows.Count = 0 Then
            MsgBox("Lote " & Lote & "/" & Format(Secuencia, "000") & " No se encuentra en Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Return False
        End If
        Dt.Rows(1).Item("Stock") = Dt.Rows(1).Item("Stock") + Cantidad
        Dt.Rows(1).Item("Traslado") = Dt.Rows(1).Item("Traslado") - Cantidad

        Return True

    End Function
    Private Function LeerNombreArticulo(ByVal Lote As Integer, ByVal Secuencia As Integer) As String

        Dim Dta As New DataTable
        Dim Nombre As String = ""

        Dta = Tablas.Leer("SELECT Nombre FROM Articulos,Lotes WHERE Lotes.Lote = " & Lote & " AND Lotes.Secuencia = " & Secuencia & _
                          " AND Articulos.Clave = Lotes.Articulo;")
        If Dta.Rows.Count <> 0 Then Nombre = Dta.Rows(0).Item("Nombre")
        Dta = Nothing
        Return Nombre

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        If PTrans = 0 Then
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

        Dim AGranel As DataColumn = New DataColumn("AGranel")
        AGranel.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(AGranel)

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

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Medida)

        Dim Stock As New DataColumn("Stock")
        Stock.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Stock)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cantidad)

    End Sub
    Private Function LLenaComboEstado() As Boolean

        ComboEstado.DataSource = DtEstadoAfectaStockYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        Return True

    End Function
    Private Function Valida() As Boolean

        If TextComprobante.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextComprobante.Focus()
            Return False
        End If
        If ComboDepositoOrigen.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Deposito Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoOrigen.Focus()
            Return False
        End If
        If ComboDepositoDestino.Text.ToString.Trim.Length = 0 Then
            MsgBox("Falta Deposito Destino.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoDestino.Focus()
            Return False
        End If
        If ComboDepositoOrigen.SelectedValue = ComboDepositoDestino.SelectedValue Then
            MsgBox("Deposito Origen no debe ser igual as Deposito Destino.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            ComboDepositoOrigen.Focus()
            Return False
        End If
        If Grid.Rows.Count = 1 Then
            MsgBox("Falta Informar Articulos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If
        For i As Integer = 0 To Grid.RowCount - 2
            If Grid.Rows(i).Cells("Articulo").Value = 0 Then
                MsgBox("Debe Informar Articulo en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Articulo")
                Grid.BeginEdit(True)
                Return False
            End If
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
            If TieneDecimales(Grid.Rows(i).Cells("Cantidad").Value) And Not Grid.Rows(i).Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.CurrentCell = Grid.Rows(i).Cells("Cantidad")
                Grid.BeginEdit(True)
                Return False
            End If
        Next

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        'para manejo del autocoplete de articulos.
        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
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
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            e.Value = Nothing
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cantidad" Or Grid.Columns(e.ColumnIndex).Name = "Stock" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = FormatNumber(e.Value, 2)
                Else : e.Value = Format(e.Value, "#")
                End If
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
            AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Lupa" Then Exit Sub

        If ComboDepositoOrigen.SelectedValue = 0 Then
            MsgBox("Falta Seleccionar depositos Origen.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Grid.Rows(e.RowIndex).Cells("Articulo").Value = 0 Then
            MsgBox("Falta Seleccionar Articulo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsSeleccionaLote = True
        SeleccionarVarios.PDeposito = ComboDepositoOrigen.SelectedValue
        SeleccionarVarios.PArticulo = Grid.Rows(e.RowIndex).Cells("Articulo").Value
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.ShowDialog()
        If SeleccionarVarios.PLote = 0 Then SeleccionarVarios.Dispose() : Exit Sub
        Dim RowsBusqueda() As DataRow
        RowsBusqueda = DtGrid.Select("Lote = " & SeleccionarVarios.PLote & " AND Secuencia = " & SeleccionarVarios.PSecuencia)
        If RowsBusqueda.Length <> 0 Then
            MsgBox("Lote ya Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
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
        Grid.Refresh()
        Grid.CurrentCell = Grid.Rows(e.RowIndex).Cells("Cantidad")
        Grid.BeginEdit(True)

        SeleccionarVarios.Dispose()

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Operacion") = 0
        e.Row("Lote") = 0
        e.Row("Secuencia") = 0
        e.Row("Articulo") = 0
        e.Row("Stock") = 0
        e.Row("Cantidad") = 0
        e.Row("AGranel") = False
        e.Row("Medida") = ""

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Articulo") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue <> 0 Then
                HallaAGranelYMedida(e.ProposedValue, e.Row("AGranel"), e.Row("Medida"))
            End If
        End If

        If e.Column.ColumnName.Equals("Cantidad") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
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

    End Sub

End Class