Imports System.Transactions
Public Class UnaListaDePreciosProveedores
    Public PLista As Integer
    Public PBloqueaFunciones As Boolean
    '
    Dim DtCabeza As DataTable
    Dim DtDetalle As DataTable
    Dim DtDetalleAnt As DataTable
    ' 
    Private WithEvents bs As New BindingSource
    '
    Dim IntFechaDesdeAnt As Integer
    Dim IntFechaHastaAnt As Integer
    Dim cb As ComboBox
    Dim Proveedor As Integer
    Dim Zona As Integer
    Private Sub ListaDePrecios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim PerLecturaOk As Boolean = PermisoLectura(215)
        Dim PerEscrituraOk As Boolean = PermisoEscritura(215)

        If Not PermisoEscritura(2) Then
            PBloqueaFunciones = True
        Else   ' Me fijo sub-menu.
            If PerLecturaOk And Not PerEscrituraOk Then PBloqueaFunciones = True
        End If

        Me.Top = 50

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        If PLista = 0 Then
            Opciones()
            If Proveedor = 0 Then Me.Close() : Exit Sub
        End If

        If GTipoIva = 2 Then
            Panel5.Enabled = False
            RadioSinIva.Checked = True
        End If

        LLenaGrid()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Not Valida() Then Exit Sub

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If DtCabeza.Rows(0).Item("IntFechaDesde") <> Format(DateTimeDesde.Value, "yyyyMMdd") Then DtCabeza.Rows(0).Item("IntFechaDesde") = Format(DateTimeDesde.Value, "yyyyMMdd")
        If DtCabeza.Rows(0).Item("IntFechaHasta") <> Format(DateTimeHasta.Value, "yyyyMMdd") Then DtCabeza.Rows(0).Item("IntFechaHasta") = Format(DateTimeHasta.Value, "yyyyMMdd")
        If DtCabeza.Rows(0).Item("PorUnidad") <> RadioPorUnidad.Checked Then DtCabeza.Rows(0).Item("PorUnidad") = RadioPorUnidad.Checked
        If DtCabeza.Rows(0).Item("Final") <> RadioFinal.Checked Then DtCabeza.Rows(0).Item("Final") = RadioFinal.Checked
        If DtCabeza.Rows(0).Item("Zona") <> Zona Then DtCabeza.Rows(0).Item("Zona") = Zona

        If IsNothing(DtDetalle.GetChanges) And IsNothing(DtCabeza.GetChanges) Then
            MsgBox("No Hay Cambios. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
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

        Dim Resul As Double

        Resul = ActualizaLista()

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
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Me.Close()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If PLista <> 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtDetalle.Rows.Count <> 0 Then
            If MsgBox("Existe Articulos. Desa Continuar igualmente (Los Articulos se Mantendrán)?.", MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                Exit Sub
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Proveedor As Integer = 0
        Dim Numero As Integer = 0
        Dim RowsBusqueda() As DataRow

        OpcionEmisor.PEsProveedor = True
        OpcionEmisor.PEsSinCandado = True
        OpcionEmisor.PEsListaDePrecios = True
        OpcionEmisor.ShowDialog()
        Proveedor = OpcionEmisor.PEmisor
        Numero = OpcionEmisor.PNumero
        OpcionEmisor.Dispose()
        If Proveedor = 0 Then Exit Sub

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT D.* FROM ListaDePreciosProveedoresCabeza AS C INNER JOIN ListaDePreciosProveedoresDetalle AS D ON C.Lista = D.Lista WHERE C.Proveedor = " & Proveedor & " AND C.Lista = " & Numero & ";", Conexion, Dt) Then Me.Close() : Exit Sub
        If Dt.Rows.Count = 0 Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            MsgBox("Lista No Existe.")
            Dt.Dispose()
            Exit Sub
        End If
        For Each Row As DataRow In Dt.Rows
            RowsBusqueda = DtDetalle.Select("Articulo = " & Row("Articulo"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = DtDetalle.NewRow
                For I As Integer = 0 To Dt.Columns.Count - 1
                    Row1(I) = Row(I)
                Next
                DtDetalle.Rows.Add(Row1)
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

        GridAExcel(Grid, Date.Now, "Lista de Precios de Proveedores", Label1.Text & " Desde " & DateTimeDesde.Value & " AL " & DateTimeHasta.Value)

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
        If Not Tablas.Read("SELECT * FROM ListaDePreciosProveedoresCabeza WHERE Lista = " & PLista & ";", Conexion, DtCabeza) Then Me.Close() : Exit Sub
        If PLista <> 0 And DtCabeza.Rows.Count = 0 Then
            MsgBox("Lista de Precio No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If

        If PLista = 0 Then
            Dim Row As DataRow = DtCabeza.NewRow
            Row("Lista") = 0
            Row("Proveedor") = Proveedor
            Row("IntFechaDesde") = 0
            Row("IntFechaHasta") = 0
            Row("PorUnidad") = False
            Row("Final") = False
            Row("Zona") = Zona
            DtCabeza.Rows.Add(Row)
        End If

        LabelZona.Text = NombreTabla(DtCabeza.Rows(0).Item("Zona"))
        Label1.Text = NombreProveedor(DtCabeza.Rows(0).Item("Proveedor"))
        Proveedor = DtCabeza.Rows(0).Item("Proveedor")
        IntFechaDesdeAnt = DtCabeza.Rows(0).Item("IntFechaDesde")
        IntFechaHastaAnt = DtCabeza.Rows(0).Item("IntFechaHasta")
        Zona = DtCabeza.Rows(0).Item("Zona")

        If PLista <> 0 Then
            DateTimeDesde.Value = DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(6, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(4, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaDesde").ToString.Substring(0, 4)
            DateTimeHasta.Value = DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(6, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(4, 2) & "/" & DtCabeza.Rows(0).Item("IntFechaHasta").ToString.Substring(0, 4)
            RadioPorUnidad.Checked = DtCabeza.Rows(0).Item("PorUnidad")
            RadioPorKilo.Checked = Not DtCabeza.Rows(0).Item("PorUnidad")
            RadioFinal.Checked = DtCabeza.Rows(0).Item("Final")
            RadioSinIva.Checked = Not DtCabeza.Rows(0).Item("Final")
            TextLista.Text = PLista
        End If

        DtDetalle = New DataTable
        If Not Tablas.Read("SELECT * FROM ListaDePreciosProveedoresDetalle AS L INNER JOIN Articulos AS A ON A.Clave = L.Articulo WHERE A.Estado = 1 AND L.Lista = " & PLista & " ORDER BY A.Nombre;", Conexion, DtDetalle) Then Me.Close() : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = DtDetalle
        Grid.EndEdit()  'ponerlo para que no cancele ?????.

        'Esto es para que en los articulos ya grabados si lo quiere cambiar solo lo haga con 'Borrar Linea' y dar de alta.
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Articulo").Value <> 0 Then
                Row.Cells("Articulo").ReadOnly = True
            End If
        Next

        DtDetalleAnt = DtDetalle.Copy

        AddHandler DtDetalle.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtDetalle_ColumnChanging)
        AddHandler DtDetalle.RowChanging, New DataRowChangeEventHandler(AddressOf DtDetalle_RowChanging)
        AddHandler DtDetalle.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtDetalle_NewRow)

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
            MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            LLenaGrid()
        End If

    End Sub
    Private Function ActualizaLista() As Double

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        '  MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtCabeza.GetChanges) Then
                    Resul = GrabaTabla(DtCabeza.GetChanges, "ListaDePreciosProveedoresCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                If Not IsNothing(DtDetalle.GetChanges) Then
                    Resul = GrabaTabla(DtDetalle.GetChanges, "ListaDePreciosProveedoresDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If

                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        End Try

    End Function
    Private Sub Opciones()

        OpcionListaDePrecios.PEsProveedor = True
        OpcionListaDePrecios.BackColor = Color.PowderBlue
        OpcionListaDePrecios.ShowDialog()
        If OpcionListaDePrecios.PRegresar Then OpcionListaDePrecios.Dispose() : Proveedor = 0 : Exit Sub
        Proveedor = OpcionListaDePrecios.PEmisor
        Label1.Text = OpcionListaDePrecios.PNombre
        Zona = OpcionListaDePrecios.PZona
        OpcionListaDePrecios.Dispose()

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Articulo.DataSource = ArticulosActivos()
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function CalculaPrecio(ByVal Articulo As Integer, ByVal Precio As Decimal) As Decimal

        Dim Iva As Decimal
        Dim Kilos As Decimal

        HallaKilosIva(Articulo, Kilos, Iva)

        If RadioSinIva.Checked Then
            Precio = Precio + CalculaIva(1, Precio, Iva)
        End If
        If RadioPorKilo.Checked Then
            Precio = Trunca5(Precio * Kilos)
        End If

        Return Precio

    End Function
    Private Function TieneIgualListaPrecio(ByVal Lote As Integer, ByVal Proveedor As Integer, ByVal Fecha As Date, ByVal ConexionStr As String) As Boolean

        Dim PorUnidadEnLista As Boolean
        Dim FinalEnLista As Boolean
        Dim Lista As Integer
        Dim Sucursal As Integer

        Dim Dt As New DataTable
        Dim Sql As String = "SELECT Sucursal FROM IngresoMercaderiasCabeza WHERE Lote = " & Lote & ";"
        If Not Tablas.Read(Sql, ConexionStr, Dt) Then Return -1
        Sucursal = Dt.Rows(0).Item("Sucursal")
        Dt.Dispose()

        If Sucursal = 0 And DtCabeza.Rows(0).Item("Zona") <> 0 Then Return False
        If Sucursal <> 0 And DtCabeza.Rows(0).Item("Zona") = 0 Then Return False

        If Sucursal <> 0 Then
            Dim Zona As Integer = HallaZonaProveedor(Proveedor, Sucursal)
            If Zona <> DtCabeza.Rows(0).Item("Zona") Then Return False
        End If

        Return True

    End Function
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Lista) FROM ListaDePreciosProveedoresCabeza;", Miconexion)
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
        If Not Tablas.Read("SELECT IntFechaDesde,IntFechaHasta FROM ListaDePreciosProveedoresCabeza WHERE Lista <> " & PLista & " AND Zona = " & Zona & " AND Proveedor = " & Proveedor & " ORDER BY IntFechaDesde;", Conexion, Dt) Then Return -1
        For Each Row As DataRow In Dt.Rows
            If FechaDesdeNew <= Row("IntFechaHasta") And FechaHastaNew >= Row("IntFechaDesde") Then Dt.Dispose() : Return 10
            If FechaHastaNew <= Row("IntFechaDesde") Then Dt.Dispose() : Return 0
        Next

        Dt.Dispose()

        Return 0

    End Function
    Private Function Valida() As Boolean

        If PLista <> 0 Then
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT * FROM ListaDePreciosProveedoresCabeza WHERE Lista = " & PLista & ";", Conexion, Dt) Then Dt.Dispose() : Return False
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

        If DtDetalle.Rows.Count = 0 Then
            MsgBox("Falta informar Articulos. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        For Each Row As DataRow In DtDetalle.Rows
            If Row.HasErrors Then
                MsgBox("Hay Errores. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Function
            End If
        Next

        If Not RadioPorUnidad.Checked And Not RadioPorKilo.Checked Then
            MsgBox("Falta informar si Precio es por Unidad o por Kilo. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        If Not RadioSinIva.Checked And Not RadioFinal.Checked Then
            MsgBox("Falta informar si Precio es Sin Iva o Final. Operación se CANCELA.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

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

        If Grid.Columns(e.ColumnIndex).Name <> "Articulo" Then
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
        e.Row("Precio") = 0

    End Sub
    Private Sub DtDetalle_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.SetColumnError(e.Column, "")

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

        If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        e.ProposedValue = Trunca(e.ProposedValue)

    End Sub
    Private Sub DtDetalle_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.RowState = DataRowState.Deleted Then Exit Sub

        If e.Row("Articulo") = 0 Then
            e.Row.SetColumnError("Articulo", "Error")
        End If

        If e.Row("Precio") = 0 Then
            e.Row.SetColumnError("Precio", "Error")
        End If

        If e.Row.GetColumnsInError.Length <> 0 Then Grid.Refresh()

    End Sub




End Class