Public Class SeleccionaLotesDevolucion
    Public PArticulo As Integer
    Public PCantidad As Decimal
    Public PIndice As Integer
    Public PDeposito As Integer
    Public PEsAumento As Boolean
    Public PLista As List(Of FilaAsignacion)
    Public PBloquea As Boolean
    Public PPaseDeProyectos As ItemPaseDeProyectos
    ' 
    Private WithEvents bs As New BindingSource
    '
    Dim AGranel As Boolean
    Private Sub SeleccionaLotesDevolucion_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            PermisoTotal = PPaseDeProyectos.PermisoTotal
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False
        LlenaCombo(ComboArticulo, "", "Articulos")
        ComboArticulo.SelectedValue = PArticulo
        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = PDeposito
        LabCantidad.Text = PCantidad

        HallaAGranelYMedida(PArticulo, AGranel, LabelUnidad.Text)

        LlenaCombosGrid()

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

        BorraGrid(Grid)

        LLenaGrid()

        If PEsAumento Then Grid.Columns("ADevolver").HeaderText = "Aumento"

        If PBloquea Then Grid.ReadOnly = True : ButtonAceptar.Enabled = False
        '        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub SeleccionaLotesDevolucion_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If Grid.RowCount <> 0 Then
            Grid.CurrentCell = Grid.Rows(0).Cells("ADevolver")
        End If

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        PLista.RemoveAll(AddressOf Coincidencia)

        For i As Integer = 0 To Grid.RowCount - 1
            Dim Fila As FilaAsignacion = New FilaAsignacion
            Fila.Indice = PIndice
            Fila.Lote = Grid.Rows(i).Cells("Lote").Value
            Fila.Secuencia = Grid.Rows(i).Cells("Secuencia").Value
            Fila.Deposito = PDeposito
            Fila.Devolucion = Grid.Rows(i).Cells("ADevolver").Value
            Fila.Asignado = Grid.Rows(i).Cells("Asignado").Value
            Fila.Operacion = Grid.Rows(i).Cells("Operacion").Value
            PLista.Add(Fila)
        Next

        Me.Close()

    End Sub
    Private Function Coincidencia(ByVal Fila As FilaAsignacion) As Boolean

        If Fila.Indice = PIndice Then Return True

    End Function
    Private Sub LLenaGrid()

        Dim Sql As String = ""

        Dim Dt As New DataTable

        For Each Fila As FilaAsignacion In PLista
            If Fila.Operacion = 1 And Fila.Indice = PIndice Then
                Sql = "SELECT 1 as Operacion,0.0 as Asignado,0.0 as ADevolver, Lote,Secuencia,KilosXUnidad,Proveedor,Calibre,Stock,Deposito FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & PDeposito & ";"
                If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub
            End If
        Next
        If PermisoTotal Then
            For Each Fila As FilaAsignacion In PLista
                If Fila.Operacion = 2 And Fila.Indice = PIndice Then
                    Sql = "SELECT 2 as Operacion,0.0 as Asignado,0.0 as ADevolver,Lote,Secuencia,KilosXUnidad,Proveedor,Calibre,Stock,Deposito FROM Lotes WHERE Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & PDeposito & ";"
                    If Not Tablas.Read(Sql, ConexionN, Dt) Then Me.Close() : Exit Sub
                End If
            Next
        End If

        Dim RowsBusqueda() As DataRow
        For Each Fila As FilaAsignacion In PLista
            If Fila.Indice = PIndice And Fila.Operacion = 1 Then
                RowsBusqueda = Dt.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                If RowsBusqueda.Length = 0 Then
                    MsgBox("Error: Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No fue encontrado en Stock.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Else
                    RowsBusqueda(0).Item("Asignado") = Fila.Asignado
                    RowsBusqueda(0).Item("ADevolver") = Fila.Devolucion
                End If
            End If
        Next
        If PermisoTotal Then
            For Each Fila As FilaAsignacion In PLista
                If Fila.Indice = PIndice And Fila.Operacion = 2 Then
                    RowsBusqueda = Dt.Select("Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito)
                    If RowsBusqueda.Length = 0 Then
                        MsgBox("Error: Lote " & Fila.Lote & "/" & Format(Fila.Secuencia, "000") & " No fue encontrado en Stock.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Else
                        RowsBusqueda(0).Item("Asignado") = Fila.Asignado
                        RowsBusqueda(0).Item("ADevolver") = Fila.Devolucion
                    End If
                End If
            Next
        End If

        Grid.DataSource = bs
        bs.DataSource = Dt

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
    Private Function Valida() As Boolean

        For i As Integer = 0 To Grid.RowCount - 1
            If Grid.Rows(i).Cells("ADevolver").Value <> 0 Then
                If TieneDecimales(Grid.Rows(i).Cells("ADevolver").Value) And Not AGranel Then
                    MsgBox("Cantidad no debe tener Decimales en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("ADevolver")
                    Grid.BeginEdit(True)
                    Return False
                End If
            End If
        Next

        Dim Cantidad As Decimal = 0

        For i As Integer = 0 To Grid.RowCount - 1
            If Grid.Rows(i).Cells("Adevolver").Value <> 0 Then
                If Grid.Rows(i).Cells("ADevolver").Value > Grid.Rows(i).Cells("Asignado").Value And Not PEsAumento Then
                    MsgBox("Devolución supera Asignado. Linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Grid.CurrentCell = Grid.Rows(i).Cells("ADevolver")
                    Grid.BeginEdit(True)
                    Return False
                End If
                Cantidad = Cantidad + CDec(Grid.Rows(i).Cells("ADevolver").Value)
            End If
        Next
        If Cantidad <> PCantidad And Cantidad <> 0 Then
            MsgBox("Error, Cantidad de Lotes no se corresponde con Cantidad Total.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
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
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Lote").Value) And _
               Not IsNothing(Grid.Rows(e.RowIndex).Cells("Lote").Value) Then
                e.Value = Grid.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(Grid.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
                    Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                Else : Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "ADevolver" Then
            If IsDBNull(e.Value) Then e.Value = 0
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" And Not e.Control Is Nothing Then
            Dim Texto As TextBox = CType(e.Control, TextBox)
            AddHandler Texto.KeyPress, AddressOf SoloDecimales_KeyPress
            AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged
        End If

    End Sub
    Private Sub SoloDecimales_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "ADevolver" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

End Class