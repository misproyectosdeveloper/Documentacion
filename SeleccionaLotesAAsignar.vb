Public Class SeleccionaLotesAAsignar
    Public PArticulo As Integer
    Public PCantidad As Decimal
    Public PIndice As Integer
    Public PDeposito As Integer
    Public PComprobante As Double
    Public PEsConPrecio As Boolean
    Public PPrecio As Decimal
    Public PConexion As String
    Public PLista As List(Of FilaAsignacion)
    Public PListaOriginal As List(Of FilaAsignacion)
    Public PBloquea As Boolean
    ' 
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Dim AGranel As Boolean
    Dim FaltaAsignar As Decimal
    Private Sub ActualizaLotes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        LabelArticulo.Text = NombreArticulo(PArticulo)
        LabelDeposito.Text = NombreDeposito(PDeposito)
        HallaAGranelYMedida(PArticulo, AGranel, LabelUnidad.Text)

        LlenaCombosGrid()

        LLenaGrid()

        LabCantidad.Text = PCantidad

        If PBloquea Then
            Grid.ReadOnly = True
            ButtonAceptar.Visible = False : Grid.Columns("Stock").Visible = False
        End If

        AddHandler Dt.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanging)
        AddHandler Dt.ColumnChanged, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanged)

    End Sub
    Private Sub SeleccionaLotesAAsignar_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If Grid.RowCount <> 0 Then
            Grid.CurrentCell = Grid.Rows(0).Cells("Asignado")
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PLista.RemoveAll(AddressOf Coincidencia)

        For i As Integer = 0 To Grid.RowCount - 1
            If Not IsDBNull(Grid.Rows(i).Cells("Asignado").Value) Then
                If Grid.Rows(i).Cells("Asignado").Value > 0 Then
                    Dim Fila As FilaAsignacion = New FilaAsignacion
                    Fila.Indice = PIndice
                    Fila.Lote = Grid.Rows(i).Cells("Lote").Value
                    Fila.Secuencia = Grid.Rows(i).Cells("Secuencia").Value
                    Fila.Deposito = PDeposito
                    Fila.Asignado = Grid.Rows(i).Cells("Asignado").Value
                    Fila.Operacion = Grid.Rows(i).Cells("Operacion").Value
                    Fila.LoteOrigen = Grid.Rows(i).Cells("LoteOrigen").Value
                    Fila.SecuenciaOrigen = Grid.Rows(i).Cells("SecuenciaOrigen").Value
                    Fila.DepositoOrigen = Grid.Rows(i).Cells("DepositoOrigen").Value
                    Fila.PermisoImp = Grid.Rows(i).Cells("PermisoImp").Value
                    PLista.Add(Fila)
                End If
            End If
        Next

        If PEsConPrecio Then
            PPrecio = HallaPPrecio()
        End If

        Me.Close()

    End Sub
    Private Function HallaPPrecio() As Decimal

        Dim Precio As Decimal = 0
        Dim PrecioW As Decimal = 0

        For Each Fila As FilaAsignacion In PLista
            PrecioW = HallaPrecioLote(Fila.LoteOrigen, Fila.SecuenciaOrigen)
            If PrecioW = 0 Then Precio = 0 : Exit For
            If PrecioW <> -1000 Then Precio = Precio + PrecioW * Fila.Asignado
            '
            If PrecioW = -1000 Then
                PrecioW = LeerPrecioLote(Fila.Operacion, Fila.LoteOrigen, Fila.SecuenciaOrigen, Fila.DepositoOrigen)
                If PrecioW = 0 Then Precio = 0 : Exit For
                Precio = Precio + PrecioW * Fila.Asignado
            End If
        Next

        Return Trunca(Precio / PCantidad)

    End Function
    Private Function HallaPrecioLote(ByVal LoteOrigen As Integer, ByVal SecuenciaOrigen As Integer) As Decimal

        Dim RowsBusqueda() As DataRow

        RowsBusqueda = Dt.Select("Lote = " & LoteOrigen & " AND Secuencia = " & SecuenciaOrigen)

        If RowsBusqueda.Length <> 0 Then
            If RowsBusqueda(0).Item("PrecioF") <> 0 Then
                Return RowsBusqueda(0).Item("PrecioF")
            Else
                Return RowsBusqueda(0).Item("PrecioCompra")
            End If
        Else
            Return -1000
        End If

        Return 0

    End Function
    Private Function LeerPrecioLote(ByVal Operacion As Integer, ByVal LoteOrigen As Integer, ByVal SecuenciaOrigen As Integer, ByVal DepositoOrigen As Integer) As Decimal

        Dim Dt As New DataTable
        Dim ConexionStr As String
        Dim Precio As Decimal = 0

        If Operacion = 1 Then
            ConexionStr = Conexion
        Else
            ConexionStr = ConexionN
        End If

        If Not Tablas.Read("SELECT PrecioCompra, PrecioF FROM Lotes WHERE Lote = " & LoteOrigen & " AND Secuencia = " & SecuenciaOrigen & " AND Deposito = " & DepositoOrigen & ";", ConexionStr, Dt) Then Return 0
        If Dt.Rows.Count <> 0 Then
            If Dt.Rows(0).Item("PrecioF") <> 0 Then
                Precio = Dt.Rows(0).Item("PrecioF")
            Else
                Precio = Dt.Rows(0).Item("PrecioCompra")
            End If
        End If

        Dt.Dispose()
        Return Precio

    End Function
    Private Sub LLenaGrid()

        Dim SqlB As String = ""
        Dim SqlN As String = ""

        'PMERCADO 10-06-2025
        SqlB = "SELECT 1 as Operacion,0.00 as Asignado,0.00 as OtrasAsignaciones,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,PrecioCompra,PrecioF FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo &
                                                " ORDER BY lote,secuencia;"
        SqlN = "SELECT 2 as Operacion,0.00 as Asignado,0.00 as OtrasAsignaciones,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,PrecioCompra,PrecioF FROM Lotes WHERE Deposito = " & PDeposito & " AND Articulo = " & PArticulo &
                                                " ORDER BY lote,secuencia;"

        '''SqlB = "SELECT 1 as Operacion,0.00 as Asignado,0.00 as OtrasAsignaciones,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,PrecioCompra,PrecioF FROM Lotes WHERE  Stock > 0 AND Deposito = " & PDeposito & " AND Articulo = " & PArticulo &
        '''                                          " ORDER BY lote,secuencia;"
        ''   SqlN = "SELECT 2 as Operacion,0.00 as Asignado,0.00 as OtrasAsignaciones,Lote,Secuencia,Deposito,LoteOrigen,SecuenciaOrigen,DepositoOrigen,Stock,KilosXUnidad,Proveedor,Fecha,Calibre,PermisoImp,PrecioCompra,PrecioF FROM Lotes WHERE  Stock > 0 AND Deposito = " & PDeposito & " AND Articulo = " & PArticulo &
        ''                                          " ORDER BY lote,secuencia;"

        Dt = New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        Dim Encontrado As Boolean
        For Each Fila As FilaAsignacion In PListaOriginal
            RowsBusqueda = Dt.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            If RowsBusqueda.Length <> 0 Then
                Encontrado = False
                For Each Fila2 As FilaAsignacion In PLista
                    If Fila2.Indice = Fila.Indice And Fila2.Lote = Fila.Lote And Fila2.Secuencia = Fila.Secuencia Then
                        RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - (Fila2.Asignado - Fila.Asignado)
                        If Fila2.Indice = PIndice Then RowsBusqueda(0).Item("Asignado") = Fila2.Asignado
                        Encontrado = True
                    End If
                Next
                If Not Encontrado Then RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") + Fila.Asignado
            End If
        Next
        '
        For Each Fila As FilaAsignacion In PLista
            If Not ExisteEnListaAsignacionIndiceLotes(PListaOriginal, Fila.Indice, Fila.Lote, Fila.Secuencia) Then
                RowsBusqueda = Dt.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                If RowsBusqueda.Length <> 0 Then
                    RowsBusqueda = Dt.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
                    RowsBusqueda(0).Item("Stock") = RowsBusqueda(0).Item("Stock") - Fila.Asignado
                    If Fila.Indice = PIndice Then RowsBusqueda(0).Item("Asignado") = Fila.Asignado
                End If
            End If
        Next

        For Each Row As DataRow In Dt.Rows  'Borra los que no tienen stock y no esta asignado.
            If Row("Stock") = 0 And Row("Asignado") = 0 Then
                Row.Delete()
            Else
                If Not (Row("Lote") = Row("LoteOrigen") And Row("Secuencia") = Row("SecuenciaOrigen") And Row("Deposito") = Row("DepositoOrigen")) Then
                    Row("PermisoImp") = HallaPermisoImp(Row("Operacion"), Row("Lote"), Row("Secuencia"), Row("Deposito"))
                    If Row("PermisoImp") = "-1" Then
                        MsgBox("Error, Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " No Encontrado.")
                        Me.Close() : Exit Sub
                    End If
                End If
            End If
        Next

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Lote,Secuencia"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        Calibre.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 5;")
        Row = Calibre.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = ""
        Calibre.DataSource.Rows.Add(Row)
        Calibre.DisplayMember = "Nombre"
        Calibre.ValueMember = "Clave"

        Proveedor.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Proveedores;")
        Proveedor.DisplayMember = "Nombre"
        Proveedor.ValueMember = "Clave"

    End Sub
    Private Function Coincidencia(ByVal Fila As FilaAsignacion) As Boolean

        If Fila.Indice = PIndice Then Return True

    End Function
    Private Function Valida() As Boolean

        For i As Integer = 0 To Grid.RowCount - 1
            If Grid.Rows(i).Cells("Asignado").Value <> 0 Then
                If TieneDecimales(Grid.Rows(i).Cells("Asignado").Value) And Not AGranel Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("Asignado")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

        Dim Cantidad As Decimal = 0

        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + CDec(Grid.Rows(i).Cells("Asignado").Value)
        Next
        If Cantidad <> PCantidad And Cantidad <> 0 Then
            MsgBox("Error, Total Asignados no se corresponde con cantidad a Asignar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

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
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LoteYSecuencia" Then
            e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
            Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Asignado" Then
            If e.Value = 0 Then e.Value = Format(e.Value, "#")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Asignado" And Not e.Control Is Nothing Then
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

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Asignado" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dt_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Asignado") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If Trunca((e.Row("Stock") + e.Row("Asignado") - e.ProposedValue)) < 0 Then
                e.ProposedValue = e.Row("Asignado")
            Else
                e.Row("Stock") = Trunca(e.Row("Stock") + e.Row("Asignado") - e.ProposedValue)
            End If
        End If

    End Sub
    Private Sub Dt_ColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        Dim Cantidad As Decimal = 0

        For i As Integer = 0 To Grid.RowCount - 1
            Cantidad = Cantidad + CDec(Grid.Rows(i).Cells("Asignado").Value)
        Next

        LabelFaltaAsignar.Text = PCantidad - Cantidad

    End Sub


End Class