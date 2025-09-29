Public Class UnaListaDePrecios
    Public PLista As Integer
    Public PBloqueaFunciones As Boolean
    Public PEsVendedor As Boolean
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtArticulos As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim IntFechaDesdeAnt As Integer
    Dim IntFechaHastaAnt As Integer
    Dim cb As ComboBox
    Dim Cliente As Integer
    Dim ListaDiaria As Boolean
    Dim Zona As Integer
    Private Sub ListaDePrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(1000) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        If PLista = 0 Then
            Opciones()
            If Cliente = 0 Then Me.Close() : Exit Sub
        End If

        LlenaCombosGrid()   'Tiene que ir despues de Opciones.

        If GTipoIva = 2 Then              'Cuando la empresa que factura es EXENTA.
            Panel5.Enabled = False
            RadioSinIva.Checked = True
        End If

        LLenaGrid()

        ComboTipoPrecio.DisplayMember = "Text"
        ComboTipoPrecio.ValueMember = "Value"
        Dim tb As New DataTable
        tb.Columns.Add("Text", GetType(String))
        tb.Columns.Add("Value", GetType(Integer))
        tb.Rows.Add("", 0)
        tb.Rows.Add("Uni.", 1)
        tb.Rows.Add("Kgs.", 2)
        ComboTipoPrecio.DataSource = tb

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If DtCabeza.Rows(0).Item("IntFechaDesde") <> Format(DateTimeDesde.Value, "yyyyMMdd") Then DtCabeza.Rows(0).Item("IntFechaDesde") = Format(DateTimeDesde.Value, "yyyyMMdd")
        If DtCabeza.Rows(0).Item("IntFechaHasta") <> Format(DateTimeHasta.Value, "yyyyMMdd") Then DtCabeza.Rows(0).Item("IntFechaHasta") = Format(DateTimeHasta.Value, "yyyyMMdd")
        If DtCabeza.Rows(0).Item("Final") <> RadioFinal.Checked Then DtCabeza.Rows(0).Item("Final") = RadioFinal.Checked
        If DtCabeza.Rows(0).Item("Zona") <> Zona Then DtCabeza.Rows(0).Item("Zona") = Zona

        If IsNothing(DtDetalle.GetChanges) And IsNothing(DtCabeza.GetChanges) Then
            MsgBox("No Hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        If Not IsNothing(DtDetalle.GetChanges) Then
            If ListaDiaria Then
                For Each Row As DataRow In DtDetalle.Rows
                    If Row.RowState <> DataRowState.Deleted Then
                        Row("Martes") = Row("Lunes")
                        Row("Miercoles") = Row("Lunes")
                        Row("Jueves") = Row("Lunes")
                        Row("Viernes") = Row("Lunes")
                        Row("Sabado") = Row("Lunes")
                        Row("Domingo") = Row("Lunes")
                    End If
                Next
            End If
            'Saco los que tienen precio = 0.
            For I As Integer = DtDetalle.Rows.Count - 1 To 0 Step -1
                Dim Row As DataRow = DtDetalle.Rows(I)
                If Row.RowState <> DataRowState.Deleted Then
                    If GCuitEmpresa <> GTux And GCuitEmpresa <> GPruebaEnTux Then
                        If Row("Lunes") = 0 Or Row("Martes") = 0 Or Row("Miercoles") = 0 Or Row("Jueves") = 0 Or Row("Viernes") = 0 Or Row("Sabado") = 0 Or Row("Domingo") = 0 Then
                            Row.Delete()
                        End If
                    End If
                End If
            Next
        End If

        If PLista = 0 Then
            HacerAlta()
        Else
            HacerModificacion()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBorrar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBorrar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLista = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If MsgBox("Lista se Borrara del Sistema. ¿Desea Borrarla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza.Rows(0).Delete()
        For Each Row As DataRow In DtDetalle.Rows
            If Row.RowState <> DataRowState.Deleted Then
                Row.Delete()
            End If
        Next

        Dim resul As Integer = ActualizaLista()

        If resul = -2 Then
            MsgBox("Error Base de Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            LLenaGrid()
        End If
        If resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            LLenaGrid()
        End If
        If resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If DtDetalle.Rows.Count <> 0 Then
            If MsgBox("Existe Articulos. Desa Continuar?." + vbCrLf + vbCrLf + "Si continua los Articulos se Mantendrán.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Cliente As Integer = 0
        Dim Numero As Integer = 0
        Dim RowsBusqueda() As DataRow

        If PEsVendedor Then
            OpcionEmisor.PEsVendedor = True
        Else
            OpcionEmisor.PEsCliente = True
        End If
        OpcionEmisor.PEsSinCandado = True
        OpcionEmisor.PEsListaDePrecios = True
        OpcionEmisor.ShowDialog()
        Cliente = OpcionEmisor.PEmisor
        Numero = OpcionEmisor.PNumero
        OpcionEmisor.Dispose()
        If Cliente = 0 Then Exit Sub

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT ListaDiaria FROM ListaDePreciosCabeza WHERE Cliente = " & Cliente & " AND Lista = " & Numero & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Lista No Existe.")
            Dt.Dispose()
            Exit Sub
        End If
        If Dt.Rows(0).Item("ListaDiaria") And Not ListaDiaria Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Lista A Importar Debe ser Lista a Semana Completa.")
            Dt.Dispose()
            Exit Sub
        End If
        If Not Dt.Rows(0).Item("ListaDiaria") And ListaDiaria Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Lista A Importar Debe ser Lista Diaria.")
            Dt.Dispose()
            Exit Sub
        End If

        Dt = New DataTable
        If Not Tablas.Read("SELECT D.* FROM ListaDePreciosDetalle AS D INNER JOIN Articulos As A ON D.Articulo = A.Clave WHERE Lista = " & Numero & " ORDER BY A.Nombre;", Conexion, Dt) Then Me.Close() : Exit Sub

        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                If ArticuloExiste(Row("Articulo")) Then
                    Dim Row1 As DataRow = DtDetalle.NewRow
                    Row1("Lista") = PLista
                    For I As Integer = 1 To Dt.Columns.Count - 1
                        Row1(I) = Row(I)
                    Next
                    DtDetalle.Rows.Add(Row1)
                End If
            End If
        Next

        Grid.Refresh()
        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImportarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportarTodos.Click

        If DtDetalle.Rows.Count <> 0 Then
            If MsgBox("Existe Articulos. Desa Continuar?." + vbCrLf + vbCrLf + "Si continua los articulos se Conservaran.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Clave FROM Articulos WHERE Estado = 1 ORDER BY Nombre;", Conexion, Dt) Then Me.Close() : Exit Sub

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                If ArticuloExiste(Row("Clave")) Then
                    Dim Row1 As DataRow = DtDetalle.NewRow
                    Row1("Lista") = PLista
                    Row1("Articulo") = Row("Clave")
                    Row1("Lunes") = 0
                    Row1("Martes") = 0
                    Row1("Miercoles") = 0
                    Row1("Jueves") = 0
                    Row1("Viernes") = 0
                    Row1("Sabado") = 0
                    Row1("Domingo") = 0
                    DtDetalle.Rows.Add(Row1)
                End If
            End If
        Next

        Grid.Refresh()
        Dt.Dispose()

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        bs.RemoveCurrent()

    End Sub
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Lista de Precios", Label1.Text & " Desde " & DateTimeDesde.Value & " AL " & DateTimeHasta.Value)

        Me.Cursor = System.Windows.Forms.Cursors.Default

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
    Private Sub LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DtCabeza = New DataTable
        If Not Tablas.Read("SELECT * FROM ListaDePreciosCabeza WHERE Lista = " & PLista & ";", Conexion, DtCabeza) Then Me.Close() : Exit Sub
        If PLista <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Lista de Precio No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PLista = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Lista") = 0
            Row("Cliente") = Cliente  'o vendedor
            Row("IntFechaDesde") = 0
            Row("IntFechaHasta") = 0
            Row("PorUnidad") = False    'Ya no se usa.
            Row("Final") = False
            Row("ListaDiaria") = ListaDiaria
            Row("Zona") = Zona
            Row("EsPorVendedor") = PEsVendedor
            DtCabeza.Rows.Add(Row)
        End If

        LabelZona.Text = NombreTabla(DtCabeza.Rows(0).Item("Zona"))
        Zona = DtCabeza.Rows(0).Item("Zona")
        If PEsVendedor Then
            Label1.Text = NombreVendedor(DtCabeza.Rows(0).Item("Cliente"))
            Label3.Visible = False : LabelZona.Visible = False
        Else
            Label1.Text = NombreCliente(DtCabeza.Rows(0).Item("Cliente"))
        End If
        IntFechaDesdeAnt = DtCabeza.Rows(0).Item("IntFechaDesde")
        IntFechaHastaAnt = DtCabeza.Rows(0).Item("IntFechaHasta")
        ListaDiaria = DtCabeza.Rows(0).Item("ListaDiaria")
        Cliente = DtCabeza.Rows(0).Item("Cliente")

        If PLista <> 0 Then
            DateTimeDesde.Value = DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(6, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(4, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(0, 4)
            DateTimeHasta.Value = DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(6, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(4, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(0, 4)
            RadioFinal.Checked = DtCabeza.Rows(0).Item("Final")
            RadioSinIva.Checked = Not DtCabeza.Rows(0).Item("Final")
            TextLista.Text = PLista
        End If

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM ListaDePreciosDetalle AS L INNER JOIN Articulos AS A ON A.Clave = L.Articulo WHERE A.Estado = 1 AND L.Lista = " & PLista & " ORDER BY A.Nombre;", Conexion, DtDetalle) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = DtDetalle
        Grid.EndEdit()  'ponerlo para que no cancele ?????.

        'Esto es para que en los articulos ya grabados si lo quiere cambiar solo lo haga con 'Borrar Linea' y dar de alta.
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Articulo").Value <> 0 Then
                Row.Cells("Articulo").ReadOnly = True
            End If
        Next

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtDetalle_ColumnChanging)
        AddHandler DtDetalle.RowChanging, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanging)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)

        If ListaDiaria Then
            Grid.Columns("Lunes").HeaderText = "Precio"
            Grid.Columns("Martes").Visible = False
            Grid.Columns("Miercoles").Visible = False
            Grid.Columns("Jueves").Visible = False
            Grid.Columns("Viernes").Visible = False
            Grid.Columns("Sabado").Visible = False
            Grid.Columns("Domingo").Visible = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub HacerAlta()

        Dim NumeroLista As Double
        Dim Resul As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroLista = UltimaNumeracion(Conexion)
            If NumeroLista < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If

            DtCabeza.Rows(0).Item("Lista") = NumeroLista
            For Each Row As DataRow In DtDetalle.Rows
                Row("Lista") = NumeroLista
            Next

            Resul = ActualizaLista()

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PLista = NumeroLista
            GModificacionOk = True
            LLenaGrid()
        End If

    End Sub
    Private Sub HacerModificacion()

        Dim Resul As Double

        Resul = ActualizaLista()

        If Resul = -2 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            LLenaGrid()
        End If

    End Sub
    Private Function ActualizaLista() As Double

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    '   If Not IsNothing(DtCabeza.GetChanges) Then  Anulada para que detecte si otro usuario borro la lista. 
                    Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ListaDePreciosCabeza;", MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(DtCabeza)
                    End Using
                    '     End If
                    If Not IsNothing(DtDetalle.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM ListaDePreciosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtDetalle.GetChanges)
                        End Using
                    End If
                    Trans.Commit()
                    Return 1000
                Catch ex As OleDb.OleDbException
                    MsgBox(ex.Message)
                    Trans.Rollback()
                    Return -2
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Private Sub Opciones()

        OpcionListaDePrecios.PEsCliente = True
        OpcionListaDePrecios.ShowDialog()
        If OpcionListaDePrecios.PRegresar Then OpcionListaDePrecios.Dispose() : Cliente = 0 : Exit Sub
        Cliente = OpcionListaDePrecios.PEmisor
        Label1.Text = OpcionListaDePrecios.PNombre
        ListaDiaria = OpcionListaDePrecios.PListaDiaria
        Zona = OpcionListaDePrecios.PZona
        PEsVendedor = OpcionListaDePrecios.PEsVendedor
        OpcionListaDePrecios.Dispose()

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        DtArticulos = New DataTable
        DtArticulos = ArticulosActivos()

        If TieneCodigoCliente(Cliente) Then
            ActualizoConCodigoCliente(DtArticulos, Cliente)
        End If

        Articulo.DataSource = DtArticulos
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

        Dim Dt As New DataTable
        ArmaTipoPrecio(Dt)
        TipoPrecio.DataSource = Dt.Copy
        TipoPrecio.DisplayMember = "Nombre"
        TipoPrecio.ValueMember = "Clave"
        Dt.Dispose()

    End Sub
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Lista) FROM ListaDePreciosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CInt(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Public Function FechaOk() As Integer

        Dim FechaDesdeNew As Integer = Format(DateTimeDesde.Value, "yyyyMMdd")
        Dim FechaHastaNew As Integer = Format(DateTimeHasta.Value, "yyyyMMdd")

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT IntFechaDesde,IntFechaHasta FROM ListaDePreciosCabeza WHERE Lista <> " & PLista & " AND Zona = " & Zona & " AND Cliente = " & Cliente & " ORDER BY IntFechaDesde;", Conexion, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            If FechaDesdeNew <= Row("IntFechaHasta") And FechaHastaNew >= Row("IntFechaDesde") Then Dt.Dispose() : Return 10
            If FechaHastaNew <= Row("IntFechaDesde") Then Dt.Dispose() : Return 0
        Next

        Dt.Dispose()

        Return 0

    End Function
    Private Function ArticuloExiste(ByVal Articulo As Integer) As Boolean

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = DtArticulos.Select("Clave = " & Articulo)
        If RowsBusqueda.Length <> 0 Then Return True

    End Function
    Private Function Valida() As Boolean

        If PLista <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT * FROM ListaDePreciosCabeza WHERE Lista = " & PLista & ";", Conexion, Dt) Then Dt.Dispose() : Return False
            If Dt.Rows.Count = 0 Then
                MsgBox("Lista fue Borrada por Otro Usuario. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Dt.Dispose() : Return False
            End If
            Dt.Dispose()
        End If

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            DateTimeDesde.Focus()
            Exit Function
        End If

        If Not ListaDiaria Then
            If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 6 Then
                MsgBox("Fecha Invalida. Debe Completar como Mínimo una Semana. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                DateTimeDesde.Focus()
                Exit Function
            End If
        End If

        If Not RadioSinIva.Checked And Not RadioFinal.Checked Then
            MsgBox("Falta informar si Precio es Sin Iva o Final. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        ' halla si hay otra lista para el mismo entorno de fecha.
        If IntFechaDesdeAnt <> Format(DateTimeDesde.Value, "yyyyMMdd") Or IntFechaHastaAnt <> Format(DateTimeHasta.Value, "yyyyMMdd") Then
            Dim ResulW As Integer = FechaOk()
            If ResulW < 0 Then
                MsgBox("Error Base de datos. Operación se CANCELA.")
                Me.Close()
                Exit Function
            End If
            If ResulW > 0 Then
                MsgBox("Ya Existe Una Lista en este entorno de Fechas. Operación se CANCELA.")
                Exit Function
            End If
        End If

        If DtDetalle.HasErrors Then
            MsgBox("Hay Errores. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellLeave

        'para manejo del autocoplete de articulos.
        If Grid.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
                If cb.SelectedIndex < 0 Then cb.SelectedValue = 0
            End If
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Articulo" Then Exit Sub

        If Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloConDecimal_KeyPress
            AddHandler Texto.TextChanged, AddressOf TextoConDecimal_TextChanged
        End If

    End Sub
    Private Sub SoloConDecimal_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextoConDecimal_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name <> "Articulo" And Grid.Columns(e.ColumnIndex).Name <> "TipoPrecio" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub DtDetalle_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Lista") = PLista
        e.Row("Articulo") = 0
        e.Row("TipoPrecio") = ComboTipoPrecio.SelectedValue
        e.Row("Lunes") = 0
        e.Row("Martes") = 0
        e.Row("Miercoles") = 0
        e.Row("Jueves") = 0
        e.Row("Viernes") = 0
        e.Row("Sabado") = 0
        e.Row("Domingo") = 0

    End Sub
    Private Sub DtDetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.SetColumnError(e.Column, "")

        If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        e.ProposedValue = Trunca(e.ProposedValue)

        If e.Column.ColumnName.Equals("Articulo") Then
            If e.ProposedValue <> 0 Then
                Dim RowsBusqueda() As DataRow
                RowsBusqueda = DtDetalle.Select("Articulo = " & e.ProposedValue)
                If RowsBusqueda.Length <> 0 Then
                    MsgBox("Articulo ya Existe.")
                    e.ProposedValue = 0
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Private Sub DtDetalle_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub

        If e.Row("Articulo") = 0 Then
            e.Row.SetColumnError("Articulo", "Error")
        End If

        If GCuitEmpresa <> GTux And GCuitEmpresa <> GPruebaEnTux Then
            If e.Row("Lunes") = 0 Then
                e.Row.SetColumnError("Lunes", "Error")
            End If
        End If

        If e.Row("TipoPrecio") = 0 Then
            e.Row.SetColumnError("TipoPrecio", "Error")
            MsgBox("Falta informar si precio es por Unidad o por U.Med. Operación se CANCELA.")
        End If

        If ListaDiaria Then
            If e.Row.GetColumnsInError.Length <> 0 Then Grid.Refresh()
            Exit Sub
        End If

        If e.Row("Martes") = 0 Then
            e.Row.SetColumnError("Martes", "Error")
        End If
        If e.Row("Miercoles") = 0 Then
            e.Row.SetColumnError("Miercoles", "Error")
        End If
        If e.Row("Jueves") = 0 Then
            e.Row.SetColumnError("Jueves", "Error")
        End If
        If e.Row("Viernes") = 0 Then
            e.Row.SetColumnError("Viernes", "Error")
        End If
        If e.Row("Sabado") = 0 Then
            e.Row.SetColumnError("Sabado", "Error")
        End If
        If e.Row("Domingo") = 0 Then
            e.Row.SetColumnError("Domingo", "Error")
        End If

        If e.Row.GetColumnsInError.Length <> 0 Then Grid.Refresh()

    End Sub

  
    
    
  
End Class