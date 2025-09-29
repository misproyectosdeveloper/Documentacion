Public Class UnPerfil
    Public PUsuario As Integer
    Public PNombre As String
    '
    Dim DtPerfiles As DataTable
    Dim DtGrid1 As DataTable
    Dim DtGrid2 As DataTable
    Dim cb As ComboBox
    Dim ClaveEnDtGrid2 As Integer
    Private Sub UnPerfil_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label1.Text = "Perfil del Usuario:    " & PNombre

        Grid1.AutoGenerateColumns = False
        Grid2.AutoGenerateColumns = False

        CreaDtGrid1()
        DtGrid2 = DtGrid1.Clone

        ArmaDtGrid1()

        Dim RowsBusqueda() As DataRow

        DtPerfiles = New DataTable
        If Not Tablas.Read("SELECT * FROM Perfiles WHERE Usuario = " & PUsuario & ";", Conexion, DtPerfiles) Then Me.Close() : Exit Sub

        For Each Row As DataRow In DtPerfiles.Rows
            RowsBusqueda = DtGrid1.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid1.DataSource = DtGrid1

        AddHandler DtGrid1.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid1_RowChanged)
        AddHandler DtGrid1.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid1_ColumnChanging)

    End Sub
    Private Sub UnPerfil_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Grid1.EndEdit()
        Grid2.EndEdit()

        If IsNothing(DtPerfiles.GetChanges) Then
            MsgBox("No Hubo Cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim Resul As Integer

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        For I As Integer = DtPerfiles.Rows.Count - 1 To 0 Step -1
            Dim Row As DataRow = DtPerfiles.Rows(I)
            If Row("Lectura") = False And Row("Escritura") = False And Row("Especial1") = False And Row("Especial2") = False And Row("Especial3") = False Then
                Row.Delete()
            End If
        Next

        Resul = GrabaArchivo(DtPerfiles.GetChanges, Conexion)

        If Resul = -2 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Cambios Realizados Exitosamente. Debe Reingresar al Sistema para que los Cambios Tengan EFECTO.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            DtPerfiles.AcceptChanges()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function GrabaArchivo(ByVal DtPerfilesAux As DataTable, ByVal ConexionStr As String) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM Perfiles;", Miconexion)
                    Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                    DaP.Update(DtPerfilesAux)
                End Using
                Return 1000
            End Using
        Catch ex As OleDb.OleDbException
            Return -2
        Catch ex As DBConcurrencyException
            Return 0
        Finally
        End Try

    End Function
    Private Sub ArmaDtGrid1()

        Dim Row As DataRow
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Cliente"
        Row("Menu") = 1
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Proveedores"
        Row("Menu") = 2
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Articulos"
        Row("Menu") = 3
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Otros Proveedores"
        Row("Menu") = 4
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Stock"
        Row("Menu") = 5
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Facturación"
        Row("Menu") = 6
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Tesoreria"
        Row("Menu") = 8
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Sueldos"
        Row("Menu") = 9
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Comercial"
        Row("Menu") = 10
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Contabilidad"
        Row("Menu") = 11
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Insumos"
        Row("Menu") = 12
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Control De Gestion"
        Row("Menu") = 13
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Informes"
        Row("Menu") = 14
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
        Row = DtGrid1.NewRow
        Row("Nombre") = "Tablas"
        Row("Menu") = 15
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid1.Rows.Add(Row)
        ' 
    End Sub
    Private Sub ArmaDtGrid2Cliente()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Imputa Comprobantes en Cta.Cte."
        Row("Menu") = 100
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Alta Clientes."
        Row("Menu") = 101
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Actualiza Saldos Iniciales."
        Row("Menu") = 114
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        '
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Proveedor()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Importe Total,Precio S.,Promedio"
        Row("Menu") = 200
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Precio Final"
        Row("Menu") = 210
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Precio Compra"
        Row("Menu") = 211
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Merma Final"
        Row("Menu") = 212
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Imputa Comprobantes en Cta.Cte."
        Row("Menu") = 213
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Actualiza Saldos Iniciales."
        Row("Menu") = 214
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Lista de Precios."
        Row("Menu") = 215
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Tesoreria()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Prestamos"
        Row("Menu") = 800
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Movimientos de Prestamos"
        Row("Menu") = 801
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Gastos Bancarios"
        Row("Menu") = 810
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        '
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Comercial()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Lista de Precios"
        Row("Menu") = 1000
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Pedidos"
        Row("Menu") = 1001
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        '
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Contabilidad()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Cierre Contable"
        Row("Menu") = 110
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Stock()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Modifica Precios en Remitos Aun C/lista de precios."
        Row("Menu") = 500
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        '
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Facturacion()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Modifica Precios en Factura Aun C/lista de precios."
        Row("Menu") = 600
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        '
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2ConNada()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ArmaDtGrid2Tabla()

        DtGrid2 = New DataTable
        DtGrid2 = DtGrid1.Clone

        Dim Row As DataRow
        Dim RowsBusqueda() As DataRow
        ' 
        Row = DtGrid2.NewRow
        Row("Nombre") = "Insumos"
        Row("Menu") = 150
        Row("Lectura") = False
        Row("Escritura") = False
        Row("Especial1") = False
        Row("Especial2") = False
        Row("Especial3") = False
        DtGrid2.Rows.Add(Row)
        ' 
        For Each Row In DtPerfiles.Rows
            RowsBusqueda = DtGrid2.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            End If
        Next

        Grid2.DataSource = DtGrid2

        AddHandler DtGrid2.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid2_RowChanged)
        AddHandler DtGrid2.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dtgrid2_ColumnChanging)

    End Sub
    Private Sub ActualizaPerfilDeGrid2()

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid2.Rows
            RowsBusqueda = DtPerfiles.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            Else
                Dim Row1 As DataRow = DtPerfiles.NewRow
                Row1("Usuario") = PUsuario
                Row1("Menu") = Row("Menu")
                Row1("Lectura") = Row("Lectura")
                Row1("Escritura") = Row("Escritura")
                Row1("Especial1") = Row("Especial1")
                Row1("Especial2") = Row("Especial2")
                Row1("Especial3") = Row("Especial3")
                DtPerfiles.Rows.Add(Row1)
            End If
        Next

    End Sub
    Private Sub ActualizaPerfilDeGrid1()

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtPerfiles.Select("Menu = " & Row("Menu"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("Lectura") = Row("Lectura")
                RowsBusqueda(0).Item("Escritura") = Row("Escritura")
                RowsBusqueda(0).Item("Especial1") = Row("Especial1")
                RowsBusqueda(0).Item("Especial2") = Row("Especial2")
                RowsBusqueda(0).Item("Especial3") = Row("Especial3")
            Else
                Dim Row1 As DataRow = DtPerfiles.NewRow
                Row1("Usuario") = PUsuario
                Row1("Menu") = Row("Menu")
                Row1("Lectura") = Row("Lectura")
                Row1("Escritura") = Row("Escritura")
                Row1("Especial1") = Row("Especial1")
                Row1("Especial2") = Row("Especial2")
                Row1("Especial3") = Row("Especial3")
                DtPerfiles.Rows.Add(Row1)
            End If
        Next

    End Sub
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Menu As New DataColumn("Menu")
        Menu.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Menu)

        Dim Nombre As New DataColumn("Nombre")
        Nombre.DataType = System.Type.GetType("System.String")
        DtGrid1.Columns.Add(Nombre)

        Dim Lectura As New DataColumn("Lectura")
        Lectura.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(Lectura)

        Dim Escritura As New DataColumn("Escritura")
        Escritura.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(Escritura)

        Dim Especial1 As New DataColumn("Especial1")
        Especial1.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(Especial1)

        Dim Especial2 As New DataColumn("Especial2")
        Especial2.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(Especial2)

        Dim Especial3 As New DataColumn("Especial3")
        Especial3.DataType = System.Type.GetType("System.Boolean")
        DtGrid1.Columns.Add(Especial3)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID1.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        'para manejo del autocoplete de menu.
        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellClick

        If ClaveEnDtGrid2 = Grid1.Rows(e.RowIndex).Cells("Menu").Value Then Exit Sub

        Select Case Grid1.Rows(e.RowIndex).Cells("Menu").Value
            Case 1
                ArmaDtGrid2Cliente()
            Case 2
                ArmaDtGrid2Proveedor()
            Case 5
                ArmaDtGrid2Stock()
            Case 6
                ArmaDtGrid2Facturacion()
            Case 8
                ArmaDtGrid2Tesoreria()
            Case 10
                ArmaDtGrid2Comercial()
            Case 11
                ArmaDtGrid2Contabilidad()
            Case 15
                ArmaDtGrid2Tabla()
            Case Else
                ArmaDtGrid2ConNada()
        End Select

        ClaveEnDtGrid2 = Grid1.Rows(e.RowIndex).Cells("Menu").Value

    End Sub
    Private Sub Dtgrid1_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Escritura") Then
            If e.ProposedValue = True Then
                e.Row("Lectura") = True
            End If
        End If

        If e.Column.ColumnName.Equals("Lectura") Then
            If e.ProposedValue = False Then
                e.Row("Escritura") = False
            End If
        End If

    End Sub
    Private Sub DtGrid1_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        ActualizaPerfilDeGrid1()

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID2.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid2_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid2.CellEnter

        'para manejo del autocoplete de menu.
        If Not Grid2.Columns(e.ColumnIndex).ReadOnly Then
            Grid2.BeginEdit(True)
        End If

    End Sub
    Private Sub Dtgrid2_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Escritura") Then
            If e.ProposedValue = True Then
                e.Row("Lectura") = True
            End If
        End If

        If e.Column.ColumnName.Equals("Lectura") Then
            If e.ProposedValue = False Then
                e.Row("Escritura") = False
            End If
        End If

    End Sub
    Private Sub DtGrid2_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        ActualizaPerfilDeGrid2()

    End Sub
   
End Class