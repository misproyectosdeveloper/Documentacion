Public Class SeleccionarCheques
    Public PEsChequesPropiosEmitidoParaTr As Boolean
    Public PEsChequesTercerosEntregadoParaTr As Boolean
    Public PEsChequesTercerosRecibidosParaTr As Boolean
    Public PEsChequeEnCartera As Boolean
    Public PEsChequeNoEnPase As Boolean
    Public PEsSiyNoVencidos As Boolean
    Public PEsTodo As Boolean
    Public PCaja As Integer
    Public PBloqueado As Boolean
    Public PAbierto As Boolean
    Public PEsSoloUno As Boolean
    Public POperacion As Integer
    Public PClave As Integer
    Public PBanco As Integer
    Public PUltimaSerie As String
    Public PUltimoNumero As Integer
    Public PImporte As Double
    Public PCuenta As Double
    Public PFecha As Date
    Public PEmisorCheque As String
    Public PeCheq As Boolean
    '  
    Public PListaDeCheques As List(Of ItemCheque)
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As New DataTable
    Private Sub SeleccionarVarios_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        Grid.AllowUserToAddRows = False
        Grid.AllowUserToDeleteRows = False
        '      Grid.AlternatingRowsDefaultCellStyle.BackColor = Color.White
        Grid.DefaultCellStyle.BackColor = Color.White
        Grid.BackgroundColor = Color.White
        Grid.Columns.Clear()

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        DateTimeHasta.Value = Date.Now.AddYears(1)
        DateTimeDesde.Value = Date.Now.AddDays(-29)

        If PEsChequesPropiosEmitidoParaTr Or PEsChequesTercerosEntregadoParaTr Or PEsChequesTercerosRecibidosParaTr Then
            PEsSiyNoVencidos = True
        End If
        If Not PEsSiyNoVencidos Then
            Panel1.Visible = False
        End If
        If PEsChequeEnCartera Then
            Panel1.Visible = True
        End If

        ButtonOpcion_Click(Nothing, Nothing)

        If DtGrid.Rows.Count <> 0 Then
            DateTimeHasta.Value = DtGrid.Rows(DtGrid.Rows.Count - 1).Item("Fecha")
        Else : DateTimeHasta.Value = Date.Now
        End If

        Grid.Width = 0
        For i As Integer = 0 To Grid.Columns.Count - 1
            If Grid.Columns(i).Visible = True Then
                Grid.Width = Grid.Width + Grid.Columns(i).Width + 3
            End If
        Next
        Grid.Width = Grid.Width + 60
        Panel1.Width = Grid.Width
        Me.Width = Grid.Width + 80

        Grid.Left = Me.Width / 2 - Grid.Width / 2
        ButtonAceptar.Left = Me.Width / 2 - ButtonAceptar.Width / 2
        Panel1.Left = Me.Width / 2 - Panel1.Width / 2
        Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 And Grid.Focused And ButtonAceptar.Visible = True Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Dim Con As Integer = 0

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then Con = Con + 1
        Next
        If Con = 0 Then
            MsgBox("Debe elegir un Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Exit Sub
        End If

        If PEsChequeEnCartera And PEsSoloUno Then
            If Con > 1 Then
                MsgBox("Debe elegir un Solo Item.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Exit Sub
            End If
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    If Row.Cells("Operacion").Value = 1 Then
                        PAbierto = True
                    Else : PAbierto = False
                    End If
                    POperacion = Row.Cells("Operacion").Value
                    PClave = Row.Cells("ClaveCheque").Value
                    PBanco = Row.Cells("Banco").Value
                    Pcuenta = Row.Cells("Cuenta").Value
                    PUltimaSerie = Row.Cells("Serie").Value
                    PUltimoNumero = Row.Cells("Numero").Value
                    PImporte = Row.Cells("Importe").Value
                    PFecha = Row.Cells("Fecha").Value
                    PEmisorCheque = Row.Cells("EmisorCheque").Value
                    PeCheq = Row.Cells("eCheq").Value
                    Me.Close() : Exit Sub
                End If
            Next
        End If

        If PEsChequesPropiosEmitidoParaTr Or PEsChequesTercerosEntregadoParaTr Or PEsChequesTercerosRecibidosParaTr Or (PEsChequeEnCartera And Not PEsSoloUno) Then
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Sel").Value Then
                    Dim Item As New ItemCheque
                    Item.Operacion = Row.Cells("Operacion").Value
                    Item.MedioPago = Row.Cells("MedioPago").Value
                    Item.Banco = Row.Cells("Banco").Value
                    Item.ClaveCheque = Row.Cells("ClaveCheque").Value
                    Item.Serie = Row.Cells("Serie").Value
                    Item.Numero = Row.Cells("Numero").Value
                    Item.Cuenta = Row.Cells("Cuenta").Value
                    Item.Importe = Row.Cells("Importe").Value
                    Item.EmisorCheque = Row.Cells("EmisorCheque").Value
                    Item.Fecha = Format(Row.Cells("Fecha").Value, "dd/MM/yyyy")
                    Item.eCheq = Row.Cells("eCheq").Value
                    PListaDeCheques.Add(Item)
                End If
            Next
            Me.Close() : Exit Sub
        End If

    End Sub
    Private Sub ButtonOpcion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOpcion.Click

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PEsChequesPropiosEmitidoParaTr Then GenerarChequesPropiosParaTr()
        If PEsChequesTercerosEntregadoParaTr Or PEsChequesTercerosRecibidosParaTr Then GenerarChequesTercerosParaTr()
        If PEsChequeEnCartera Then GenerarChequeEnCartera()

    End Sub
    Private Sub ButtonMarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMarcarTodos.Click

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Sel").Value = True
        Next

    End Sub
    Private Sub ButtonDesmarcarTodos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDesmarcarTodos.Click

        For Each Row As DataGridViewRow In Grid.Rows
            Row.Cells("Sel").Value = False
        Next

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
    Private Sub GenerarChequesPropiosParaTr()

        Grid.Columns.Clear()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.Font = New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim style1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        style1.Font = New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "MedioPago"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("MedioPago").DataPropertyName = "MedioPago"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 40
        GridChekBox.Width = 40
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "eCheq"
        GridChekBox.ReadOnly = True
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("eCheq").DataPropertyName = "eCheq"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DefaultCellStyle.Font = style1.Font
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        ''''     GridComboBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridComboBox.ReadOnly = True
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Banco"
        GridComboBox.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Banco"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Banco").DataPropertyName = "Banco"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        ''''   GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 70
        GridTextBox.MaxInputLength = 70
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Cuenta"
        GridTextBox.Name = "Cuenta"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cuenta").DataPropertyName = "Cuenta"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        '''    GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Serie"
        GridTextBox.Name = "Serie"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Serie").DataPropertyName = "Serie"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Numero"
        GridTextBox.Name = "Numero"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Numero").DataPropertyName = "Numero"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Fecha Emision"
        GridTextBox.Name = "FechaEmision"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("FechaEmision").DataPropertyName = "FechaEmision"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        '''  GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 150
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "ENTREGADO A"
        GridTextBox.Name = "Emisor"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Emisor").DataPropertyName = "Emisor"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.Width = 40
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Caja"
        GridTextBox.Name = "Caja"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Caja").DataPropertyName = "Caja"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Emisor Cheque"
        GridTextBox.Name = "EmisorCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("EmisorCheque").DataPropertyName = "EmisorCheque"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = False
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave Cheque"
        GridTextBox.Name = "ClaveCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ClaveCheque").DataPropertyName = "ClaveCheque"

        Grid.AllowUserToOrderColumns = True

        CreaDtGridCheques()

        Dim Dt As New DataTable
        Dim RowGrid As DataRow

        If Not ArmaDtChequesPropios(Dt, 999, DateTimeDesde.Value, DateTimeHasta.Value, PAbierto, Not PAbierto, 0, 0, 0, False) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha"

        For Each Row As DataRowView In View
            If Row("Estado") = 1 And Row("Afectado") = 0 Then
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("Mediopago") = Row("MedioPago")
                RowGrid("ClaveCheque") = Row("ClaveCheque")
                RowGrid("Banco") = Row("Banco")
                RowGrid("Cuenta") = Row("Cuenta")
                RowGrid("Serie") = Row("Serie")
                RowGrid("Numero") = Row("Numero")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("FechaEmision") = Row("FechaDestino")
                RowGrid("Importe") = Row("Importe")
                RowGrid("Caja") = Row("Caja")
                RowGrid("EmisorCheque") = Row("EmisorCheque")
                RowGrid("Emisor") = HallaNombreDestino(Row("Destino"), Row("EmisorDestino"))
                RowGrid("eCheq") = Row("eCheq")
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False
        If PBloqueado Then
            ButtonAceptar.Visible = False
            Grid.Columns("Sel").Visible = False
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        'Marca los seleccionados.
        For Each Item As ItemCheque In PListaDeCheques
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("ClaveCheque").Value = Item.ClaveCheque Then
                    Row.Cells("Sel").Value = True : Exit For
                End If
            Next
        Next

        PListaDeCheques.Clear()
        Dt.Dispose()

    End Sub
    Private Sub GenerarChequesTercerosParaTr()

        Grid.Columns.Clear()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.Font = New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim style1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        style1.Font = New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "MedioPago"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("MedioPago").DataPropertyName = "MedioPago"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 40
        GridChekBox.Width = 40
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "eCheq"
        GridChekBox.ReadOnly = True
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("eCheq").DataPropertyName = "eCheq"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DefaultCellStyle.Font = style1.Font
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        '''   GridComboBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridComboBox.ReadOnly = True
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Banco"
        GridComboBox.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Banco"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Banco").DataPropertyName = "Banco"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        '''       GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Serie"
        GridTextBox.Name = "Serie"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Serie").DataPropertyName = "Serie"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Numero"
        GridTextBox.Name = "Numero"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Numero").DataPropertyName = "Numero"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        ''''      GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 60
        GridTextBox.MaxInputLength = 60
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = False
        GridTextBox.HeaderText = "Cuenta"
        GridTextBox.Name = "Cuenta"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cuenta").DataPropertyName = "Cuenta"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Fecha Emision"
        GridTextBox.Name = "FechaEmision"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("FechaEmision").DataPropertyName = "FechaEmision"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        ''''     GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 150
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "RECIBIDO DE"
        GridTextBox.Name = "Emisor"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Emisor").DataPropertyName = "Emisor"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        ''''     GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 150
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "ENTREGADO A"
        GridTextBox.Name = "EntregadoA"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("EntregadoA").DataPropertyName = "EntregadoA"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.Width = 40
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Caja"
        GridTextBox.Name = "Caja"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Caja").DataPropertyName = "Caja"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Emisor Cheque"
        GridTextBox.Name = "EmisorCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("EmisorCheque").DataPropertyName = "EmisorCheque"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave Cheque"
        GridTextBox.Name = "ClaveCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ClaveCheque").DataPropertyName = "ClaveCheque"

        Grid.AllowUserToOrderColumns = True

        CreaDtGridCheques()

        Dim SqlB As String
        Dim SqlN As String

        'Origen 1:   Banco.
        'Origen 2:   Proveedores.
        'Origen 3:   Cliente.
        'Origen 4:   Empleado.
        'Origen 5:   Otros Proveedores.

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Abierto As Boolean
        Dim Cerrado As Boolean

        If PAbierto Then
            Abierto = True
            Cerrado = False
        Else
            Abierto = False
            Cerrado = True
        End If

        Dim Dt As New DataTable

        If PEsChequesTercerosEntregadoParaTr Then
            If Not ArmaDtChequesTerceros(Dt, 999, DateTimeDesde.Value, DateTimeHasta.Value, Abierto, Cerrado, False, True, 0) Then Me.Close() : Exit Sub
        End If
        If PEsChequesTercerosRecibidosParaTr Then
            If Not ArmaDtChequesTerceros(Dt, 999, DateTimeDesde.Value, DateTimeHasta.Value, Abierto, Cerrado, True, True, 0) Then Me.Close() : Exit Sub
        End If

        Dim DtAux As New DataTable
        Dim RowGrid As DataRow

        'Saca los cheques de pase de caja no aceptados. 
        '        Dim SqlCaja As String = " AND C.CajaOrigen = " & GCaja
        Dim SqlCaja As String = ""

        SqlB = "SELECT 1 AS Operacion,P.ClaveCheque FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE C.Aceptado = 0 " & SqlCaja & ";"
        SqlN = "SELECT 2 AS Operacion,P.ClaveCheque FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE C.Aceptado = 0 " & SqlCaja & ";"

        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, DtAux) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read(SqlN, ConexionN, DtAux) Then Me.Close() : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtAux.Rows
            RowsBusqueda = Dt.Select("ClaveCheque = " & Row("ClaveCheque") & " AND Operacion = " & Row("Operacion"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Delete()
        Next

        DtAux.Dispose()

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha"

        Dim ConexionStr As String
        If PAbierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim NombreEmisor As String = ""
        Dim NombreDestino1 As String = ""

        For Each Row As DataRowView In View
            If Row("Estado") = 1 And Row("Afectado") = 0 Then
                NombreEmisor = ""
                NombreDestino1 = ""
                NombreEmisor = HallaNombreEmisor(Row("Origen"), Row("Emisor"))
                If Row("Destino") <> 0 Then NombreDestino1 = HallaNombreDestino(Row("Destino"), Row("EmisorDestino"))
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("MedioPago") = Row("MedioPago")
                RowGrid("ClaveCheque") = Row("ClaveCheque")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("FechaEmision") = Row("FechaDestino")
                RowGrid("Banco") = Row("Banco")
                RowGrid("Serie") = Row("Serie")
                RowGrid("Numero") = Row("Numero")
                RowGrid("Cuenta") = Row("Cuenta")
                RowGrid("Importe") = Row("Importe")
                RowGrid("EmisorCheque") = Row("EmisorCheque")
                RowGrid("Caja") = Row("Caja")
                RowGrid("Emisor") = NombreEmisor
                RowGrid("EntregadoA") = NombreDestino1
                RowGrid("eCheq") = Row("eCheq")
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False
        If PBloqueado Then
            ButtonAceptar.Visible = False
            Grid.Columns("Sel").Visible = False
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        'Marca los seleccionados.
        For Each Item As ItemCheque In PListaDeCheques
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("ClaveCheque").Value = Item.ClaveCheque Then
                    Row.Cells("Sel").Value = True : Exit For
                End If
            Next
        Next

        Me.Cursor = System.Windows.Forms.Cursors.Default

        PListaDeCheques.Clear()
        Dt.Dispose()

    End Sub
    Private Sub GenerarChequeEnCartera()

        Grid.Columns.Clear()

        Dim GridTextBox As DataGridViewTextBoxColumn
        Dim GridChekBox As DataGridViewCheckBoxColumn
        Dim GridComboBox As DataGridViewComboBoxColumn
        Dim GridImageBox As DataGridViewImageColumn

        Dim style As DataGridViewCellStyle = New DataGridViewCellStyle()
        style.Font = New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point)
        Dim style1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        style1.Font = New Font("Arial", 9, FontStyle.Regular, GraphicsUnit.Point)

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 20
        GridChekBox.Width = 20
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = ""
        GridChekBox.Name = "Sel"
        Grid.Columns.Add(GridChekBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "Operacion"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Operacion").DataPropertyName = "Operacion"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = ""
        GridTextBox.Name = "MedioPago"
        GridTextBox.Visible = False
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("MedioPago").DataPropertyName = "MedioPago"

        GridImageBox = New DataGridViewImageColumn
        GridImageBox.ImageLayout = DataGridViewImageCellLayout.Zoom
        GridImageBox.MinimumWidth = 20
        GridImageBox.Width = 20
        GridImageBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridImageBox.HeaderText = ""
        GridImageBox.Name = "Candado"
        Grid.Columns.Add(GridImageBox)

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Importe"
        GridTextBox.Name = "Importe"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Importe").DataPropertyName = "Importe"

        GridChekBox = New DataGridViewCheckBoxColumn
        GridChekBox.MinimumWidth = 40
        GridChekBox.Width = 40
        GridChekBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridChekBox.HeaderText = "eCheq"
        GridChekBox.Name = "eCheq"
        GridChekBox.ReadOnly = True
        Grid.Columns.Add(GridChekBox)
        Grid.Columns.Item("eCheq").DataPropertyName = "eCheq"

        GridComboBox = New DataGridViewComboBoxColumn
        GridComboBox.DefaultCellStyle.ForeColor = Color.Black
        GridComboBox.DefaultCellStyle.Font = style1.Font
        GridComboBox.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
        GridComboBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridComboBox.ReadOnly = True
        GridComboBox.MinimumWidth = 80
        GridComboBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridComboBox.HeaderText = "Banco"
        GridComboBox.DataSource = Tablas.Leer("Select Nombre,Clave From Tablas WHERE Tipo = 26 order By Nombre;")
        GridComboBox.DisplayMember = "Nombre"
        GridComboBox.ValueMember = "Clave"
        GridComboBox.Name = "Banco"
        Grid.Columns.Add(GridComboBox)
        Grid.Columns.Item("Banco").DataPropertyName = "Banco"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 60
        GridTextBox.MaxInputLength = 60
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = False
        GridTextBox.HeaderText = "Cuenta"
        GridTextBox.Name = "Cuenta"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Cuenta").DataPropertyName = "Cuenta"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 10
        GridTextBox.MaxInputLength = 10
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Serie"
        GridTextBox.Name = "Serie"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Serie").DataPropertyName = "Serie"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 90
        GridTextBox.MaxInputLength = 90
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Numero"
        GridTextBox.Name = "Numero"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Numero").DataPropertyName = "Numero"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.MaxInputLength = 40
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "Fecha"
        GridTextBox.Name = "Fecha"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Fecha").DataPropertyName = "Fecha"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 150
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "RECIBIDO DE"
        GridTextBox.Name = "Emisor"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Emisor").DataPropertyName = "Emisor"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.SortMode = DataGridViewColumnSortMode.NotSortable
        GridTextBox.MinimumWidth = 150
        GridTextBox.MaxInputLength = 150
        GridTextBox.ReadOnly = True
        GridTextBox.HeaderText = "ENTREGADO A"
        GridTextBox.Name = "EntregadoA"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("EntregadoA").DataPropertyName = "EntregadoA"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 40
        GridTextBox.Width = 40
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Caja"
        GridTextBox.Name = "Caja"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("Caja").DataPropertyName = "Caja"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Font = style1.Font
        GridTextBox.MinimumWidth = 120
        GridTextBox.MaxInputLength = 120
        GridTextBox.ReadOnly = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Emisor Cheque"
        GridTextBox.Name = "EmisorCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("EmisorCheque").DataPropertyName = "EmisorCheque"

        GridTextBox = New DataGridViewTextBoxColumn
        GridTextBox.DefaultCellStyle.ForeColor = Color.Black
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft
        GridTextBox.MinimumWidth = 80
        GridTextBox.MaxInputLength = 80
        GridTextBox.ReadOnly = True
        GridTextBox.Visible = True
        GridTextBox.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        GridTextBox.HeaderText = "Clave Cheque"
        GridTextBox.Name = "ClaveCheque"
        GridTextBox.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        Grid.Columns.Add(GridTextBox)
        Grid.Columns.Item("ClaveCheque").DataPropertyName = "ClaveCheque"

        CreaDtGridCheques()

        Dim SqlB As String
        Dim SqlN As String

        Dim Abierto As Boolean
        Dim Cerrado As Boolean

        If PEstodo Then
            Abierto = True
            Cerrado = True
        Else
            If PAbierto Then
                Abierto = True
                Cerrado = False
            Else
                Abierto = False
                Cerrado = True
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Dt As New DataTable

        If Not ArmaDtChequesTerceros(Dt, PCaja, DateTimeDesde.Value, DateTimeHasta.Value, Abierto, Cerrado, True, False, 0) Then Me.Close() : Exit Sub

        Dim DtAux As New DataTable
        Dim RowGrid As DataRow

        'Saca los cheques de pase de caja no aceptados. 
        Dim SqlCaja As String = " AND C.CajaOrigen = " & PCaja

        SqlB = "SELECT 1 AS Operacion,P.ClaveCheque FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE C.Aceptado = 0 " & SqlCaja & ";"
        SqlN = "SELECT 2 AS Operacion,P.ClaveCheque FROM PaseCajaCabeza AS C INNER JOIN PaseCajaDetalle AS P ON C.Pase = P.Pase WHERE C.Aceptado = 0 " & SqlCaja & ";"

        If PAbierto Then
            If Not Tablas.Read(SqlB, Conexion, DtAux) Then Me.Close() : Exit Sub
        Else
            If Not Tablas.Read(SqlN, ConexionN, DtAux) Then Me.Close() : Exit Sub
        End If

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtAux.Rows
            RowsBusqueda = Dt.Select("ClaveCheque = " & Row("ClaveCheque") & " AND Operacion = " & Row("Operacion"))
            If RowsBusqueda.Length <> 0 Then RowsBusqueda(0).Delete()
        Next

        DtAux.Dispose()

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Fecha"

        Dim ConexionStr As String
        If PAbierto Then
            ConexionStr = Conexion
        Else : ConexionStr = ConexionN
        End If

        Dim NombreEmisor As String = ""
        Dim NombreDestino1 As String = ""

        For Each Row As DataRowView In View
            If Row("Estado") = 1 Then
                NombreEmisor = ""
                NombreDestino1 = ""
                NombreEmisor = HallaNombreEmisor(Row("Origen"), Row("Emisor"))
                If Row("Destino") <> 0 Then NombreDestino1 = HallaNombreDestino(Row("Destino"), Row("EmisorDestino"))
                RowGrid = DtGrid.NewRow
                RowGrid("Operacion") = Row("Operacion")
                RowGrid("MedioPago") = Row("MedioPago")
                RowGrid("ClaveCheque") = Row("ClaveCheque")
                RowGrid("Fecha") = Row("Fecha")
                RowGrid("Banco") = Row("Banco")
                RowGrid("Cuenta") = Row("Cuenta")
                RowGrid("Serie") = Row("Serie")
                RowGrid("Numero") = Row("Numero")
                RowGrid("Importe") = Row("Importe")
                RowGrid("EmisorCheque") = Row("EmisorCheque")
                RowGrid("Caja") = Row("Caja")
                RowGrid("Emisor") = NombreEmisor
                RowGrid("EntregadoA") = NombreDestino1
                RowGrid("eCheq") = Row("eCheq")
                DtGrid.Rows.Add(RowGrid)
            End If
        Next

        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False
        If PBloqueado Then
            ButtonAceptar.Visible = False
            Grid.Columns("Sel").Visible = False
        End If

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        'Marca los seleccionados.
        If Not PEsSoloUno Then
            For Each Item As ItemCheque In PListaDeCheques
                For Each Row As DataGridViewRow In Grid.Rows
                    If Row.Cells("ClaveCheque").Value = Item.ClaveCheque Then
                        Row.Cells("Sel").Value = True : Exit For
                    End If
                Next
            Next
            PListaDeCheques.Clear()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

        PClave = 0

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGridCheques()

        DtGrid = New DataTable

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim FechaEmision As New DataColumn("FechaEmision")
        FechaEmision.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaEmision)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Numero)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Importe)

        Dim EntregadoA As New DataColumn("EntregadoA")
        EntregadoA.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EntregadoA)

        Dim Emisor As New DataColumn("Emisor")
        Emisor.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Emisor)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim Caja As New DataColumn("Caja")
        Caja.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Caja)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheq)

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Or Grid.Columns(e.ColumnIndex).Name = "ReciboOficial" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = NumeroEditado(e.Value)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Banco" Then
            If PermisoTotal Then
                If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("Operacion").Value) Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Numero" Then
            e.Value = FormatNumber(e.Value, 0)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Importe" Then
            e.Value = FormatNumber(e.Value, GDecimales)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "UltimoNumero" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, 0)
            Else : e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Caja" Then
            e.Value = Format(e.Value, "0000")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Or Grid.Columns(e.ColumnIndex).Name = "FechaEmision" Then
            If Not IsDBNull(e.Value) Then
                e.Value = Format(e.Value, "dd/MM/yyyy")
            End If
        End If
    End Sub

End Class