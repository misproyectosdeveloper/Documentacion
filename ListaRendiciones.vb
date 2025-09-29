Public Class ListaRendiciones
    Public PBloqueaFunciones As Boolean
    '
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    Private Sub ListaRendiciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 40

        Grid.AutoGenerateColumns = False

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        ComboFondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = ComboFondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        ComboFondoFijo.DataSource.rows.add(Row)
        ComboFondoFijo.DisplayMember = "Nombre"
        ComboFondoFijo.ValueMember = "Clave"
        ComboFondoFijo.SelectedValue = 0
        With ComboFondoFijo
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        LlenaCombosGrid()

        If Not PermisoTotal Then
            Grid.Columns("Candado").Visible = False
            CheckAbierto.Visible = False
            CheckAbierto.Checked = True
            CheckCerrado.Visible = False
            CheckCerrado.Checked = False
        End If

        LLenaGrid()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonSeleccionar_Click(Nothing, Nothing)

    End Sub
    Private Sub UnaTablaRendicion_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

        Entrada.Activate()

    End Sub
    Private Sub ButtonSeleccionar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSeleccionar.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If TextNumero.Text <> "" Then
            If CInt(TextNumero.Text) = 0 Then
                MsgBox("Numero No debe ser igual a 0.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextNumero.Text = ""
                Exit Sub
            End If
        End If

        If TextRendicion.Text <> "" Then
            If CInt(TextRendicion.Text) = 0 Then
                MsgBox("Rendición No debe ser igual a 0.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                TextNumero.Text = ""
                Exit Sub
            End If
        End If

        LLenaGrid()

    End Sub
    Private Sub ButtonAgregar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAgregar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If Grid.CurrentRow.Cells("Cerrado").Value Then
            MsgBox("Rendición Cerrada. Operación se CANCELA.")
            Exit Sub
        End If

        Dim Abierto As Boolean = False
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            Abierto = True
        End If

        If FondoFijoCerrado(Grid.CurrentRow.Cells("Numero").Value, Abierto) Then
            MsgBox("Fondo Fijo cerrado.", MsgBoxStyle.Critical)
            Me.Close()
            Exit Sub
        End If

        UnaFacturaProveedorFondoFijo.PFactura = 0
        UnaFacturaProveedorFondoFijo.PAbierto = Abierto
        UnaFacturaProveedorFondoFijo.PRendicion = Grid.CurrentRow.Cells("Rendicion").Value
        UnaFacturaProveedorFondoFijo.ShowDialog()
        If GModificacionOk Then LLenaGrid()

    End Sub
    Private Sub ComboFondoFijo_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboFondoFijo.Validating

        If IsNothing(ComboFondoFijo.SelectedValue) Then ComboFondoFijo.SelectedValue = 0

    End Sub
    Private Sub TextNumero_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextNumero.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

    End Sub
    Private Sub TextRendicin_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextRendicion.KeyPress

        EsNumerico(e.KeyChar, TextNumero.Text, 0)

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

        Dim SqlFecha As String
        SqlFecha = "Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        Dim SqlFondoFijo As String = ""
        If ComboFondoFijo.SelectedValue <> 0 Then
            SqlFondoFijo = " AND FondoFijo = " & ComboFondoFijo.SelectedValue
        End If

        Dim SqlNumero As String = ""
        If TextNumero.Text <> "" Then
            SqlNumero = " AND Numero = " & CInt(TextNumero.Text)
        End If

        Dim SqlRendicion As String = ""
        If TextRendicion.Text <> "" Then
            SqlNumero = " AND Rendicion = " & CInt(TextRendicion.Text)
        End If

        Dim SqlB As String = "SELECT 1 AS Operacion,* FROM RendicionFondoFijo WHERE " & SqlFecha & SqlFondoFijo & SqlNumero & SqlRendicion & ";"
        Dim SqlN As String = "SELECT 2 AS Operacion,* FROM RendicionFondoFijo WHERE " & SqlFecha & SqlFondoFijo & SqlNumero & SqlRendicion & ";"

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then End : Exit Sub
        End If
        If CheckCerrado.Checked Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then End : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "FondoFijo,Numero,Rendicion"

        Grid.DataSource = bs
        bs.DataSource = View

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        FondoFijo.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM MaestroFondoFijo;")
        Dim Row As DataRow = FondoFijo.DataSource.newrow
        Row("Nombre") = ""
        Row("Clave") = 0
        FondoFijo.DataSource.rows.add(Row)
        FondoFijo.DisplayMember = "Nombre"
        FondoFijo.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "FondoFijo" Then
            If PermisoTotal Then
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Or Grid.Columns(e.ColumnIndex).Name = "ImporteFacturas" Then
            If e.Value = 0 Then
                e.Value = Format(0, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Saldo" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        UnaRendicion.PRendicion = Grid.CurrentRow.Cells("Rendicion").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaRendicion.PAbierto = True
        Else : UnaRendicion.PAbierto = False
        End If
        UnaRendicion.ShowDialog()
        UnaRendicion.Dispose()
        If GModificacionOk Then LLenaGrid()

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub




    
End Class