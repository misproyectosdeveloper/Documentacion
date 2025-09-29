<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaIngresoMercaderias
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaIngresoMercaderias))
        Me.Dt = New System.Data.DataTable
        Me.Ds = New System.Data.DataSet
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Color = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TipoOperacion = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Remito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Guia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cartel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Deposito = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Sucursal = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.RemitoCliente = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextOrdenCompra = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextGuia = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.CheckNoFacturados = New System.Windows.Forms.CheckBox
        Me.CheckFacturados = New System.Windows.Forms.CheckBox
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonVerDetalle = New System.Windows.Forms.Button
        Me.ButtonDevolucion = New System.Windows.Forms.Button
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        CType(Me.Dt, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Dt
        '
        Me.Dt.TableName = "Dt"
        '
        'Ds
        '
        Me.Ds.DataSetName = "DataSet"
        Me.Ds.Tables.AddRange(New System.Data.DataTable() {Me.Dt})
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Color, Me.Candado, Me.Lote, Me.TipoOperacion, Me.Fecha, Me.Remito, Me.Guia, Me.Cartel, Me.Deposito, Me.Proveedor, Me.Sucursal, Me.RemitoCliente})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.Location = New System.Drawing.Point(9, 55)
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        Me.Grid.RowHeadersWidth = 30
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Blue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1033, 581)
        Me.Grid.TabIndex = 86
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operaion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        Me.Operacion.Width = 75
        '
        'Color
        '
        Me.Color.DataPropertyName = "Color"
        Me.Color.HeaderText = "Color"
        Me.Color.Name = "Color"
        Me.Color.ReadOnly = True
        Me.Color.Visible = False
        Me.Color.Width = 56
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 20
        Me.Candado.Name = "Candado"
        Me.Candado.ReadOnly = True
        Me.Candado.Width = 20
        '
        'Lote
        '
        Me.Lote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle2
        Me.Lote.HeaderText = "Lote"
        Me.Lote.MinimumWidth = 70
        Me.Lote.Name = "Lote"
        Me.Lote.ReadOnly = True
        Me.Lote.Width = 70
        '
        'TipoOperacion
        '
        Me.TipoOperacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoOperacion.DataPropertyName = "TipoOperacion"
        Me.TipoOperacion.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.TipoOperacion.HeaderText = ""
        Me.TipoOperacion.MinimumWidth = 120
        Me.TipoOperacion.Name = "TipoOperacion"
        Me.TipoOperacion.ReadOnly = True
        Me.TipoOperacion.Width = 120
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle3
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 80
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 80
        '
        'Remito
        '
        Me.Remito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Remito.DataPropertyName = "Remito"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Remito.DefaultCellStyle = DataGridViewCellStyle4
        Me.Remito.HeaderText = "Remito"
        Me.Remito.MinimumWidth = 90
        Me.Remito.Name = "Remito"
        Me.Remito.ReadOnly = True
        Me.Remito.Width = 90
        '
        'Guia
        '
        Me.Guia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Guia.DataPropertyName = "Guia"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Guia.DefaultCellStyle = DataGridViewCellStyle5
        Me.Guia.HeaderText = "Guia"
        Me.Guia.MinimumWidth = 80
        Me.Guia.Name = "Guia"
        Me.Guia.ReadOnly = True
        Me.Guia.Width = 80
        '
        'Cartel
        '
        Me.Cartel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cartel.DataPropertyName = "Cartel"
        Me.Cartel.HeaderText = ""
        Me.Cartel.MinimumWidth = 70
        Me.Cartel.Name = "Cartel"
        Me.Cartel.ReadOnly = True
        Me.Cartel.Width = 70
        '
        'Deposito
        '
        Me.Deposito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Deposito.DataPropertyName = "Deposito"
        Me.Deposito.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Deposito.HeaderText = "Deposito"
        Me.Deposito.MinimumWidth = 100
        Me.Deposito.Name = "Deposito"
        Me.Deposito.ReadOnly = True
        Me.Deposito.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Deposito.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Proveedor
        '
        Me.Proveedor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Proveedor.DataPropertyName = "Proveedor"
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Proveedor"
        Me.Proveedor.MinimumWidth = 150
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        Me.Proveedor.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Proveedor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Proveedor.Width = 150
        '
        'Sucursal
        '
        Me.Sucursal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sucursal.DataPropertyName = "Sucursal"
        Me.Sucursal.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Sucursal.HeaderText = "Sucursal"
        Me.Sucursal.MinimumWidth = 100
        Me.Sucursal.Name = "Sucursal"
        Me.Sucursal.ReadOnly = True
        '
        'RemitoCliente
        '
        Me.RemitoCliente.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.RemitoCliente.DataPropertyName = "RemitoCliente"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.RemitoCliente.DefaultCellStyle = DataGridViewCellStyle6
        Me.RemitoCliente.HeaderText = "Remito Cliente"
        Me.RemitoCliente.MinimumWidth = 100
        Me.RemitoCliente.Name = "RemitoCliente"
        Me.RemitoCliente.ReadOnly = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextOrdenCompra)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TextGuia)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.MaskedRemito)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.CheckNoFacturados)
        Me.Panel1.Controls.Add(Me.CheckFacturados)
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(9, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1033, 52)
        Me.Panel1.TabIndex = 96
        '
        'TextOrdenCompra
        '
        Me.TextOrdenCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextOrdenCompra.Location = New System.Drawing.Point(924, 27)
        Me.TextOrdenCompra.MaxLength = 12
        Me.TextOrdenCompra.Name = "TextOrdenCompra"
        Me.TextOrdenCompra.Size = New System.Drawing.Size(101, 20)
        Me.TextOrdenCompra.TabIndex = 211
        Me.TextOrdenCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(862, 31)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(57, 13)
        Me.Label10.TabIndex = 212
        Me.Label10.Text = "O. Compra"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextGuia
        '
        Me.TextGuia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextGuia.Location = New System.Drawing.Point(759, 27)
        Me.TextGuia.MaxLength = 12
        Me.TextGuia.Name = "TextGuia"
        Me.TextGuia.Size = New System.Drawing.Size(101, 20)
        Me.TextGuia.TabIndex = 209
        Me.TextGuia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(724, 31)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 13)
        Me.Label8.TabIndex = 210
        Me.Label8.Text = "Guia"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedRemito
        '
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(618, 28)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(100, 20)
        Me.MaskedRemito.TabIndex = 208
        Me.MaskedRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(572, 31)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 13)
        Me.Label9.TabIndex = 207
        Me.Label9.Text = "Remito"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(71, 25)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(188, 21)
        Me.ComboAlias.TabIndex = 206
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(7, 28)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 13)
        Me.Label5.TabIndex = 205
        Me.Label5.Text = "Alias"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckNoFacturados
        '
        Me.CheckNoFacturados.BackColor = System.Drawing.Color.LightGray
        Me.CheckNoFacturados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckNoFacturados.Checked = True
        Me.CheckNoFacturados.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckNoFacturados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckNoFacturados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckNoFacturados.Location = New System.Drawing.Point(425, 0)
        Me.CheckNoFacturados.Name = "CheckNoFacturados"
        Me.CheckNoFacturados.Size = New System.Drawing.Size(120, 23)
        Me.CheckNoFacturados.TabIndex = 204
        Me.CheckNoFacturados.Text = "No Liq/Facturados"
        Me.CheckNoFacturados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckNoFacturados.UseVisualStyleBackColor = False
        '
        'CheckFacturados
        '
        Me.CheckFacturados.BackColor = System.Drawing.Color.LightGray
        Me.CheckFacturados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckFacturados.Checked = True
        Me.CheckFacturados.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckFacturados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckFacturados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckFacturados.Location = New System.Drawing.Point(318, 0)
        Me.CheckFacturados.Name = "CheckFacturados"
        Me.CheckFacturados.Size = New System.Drawing.Size(99, 23)
        Me.CheckFacturados.TabIndex = 203
        Me.CheckFacturados.Text = "Liq/Facturados"
        Me.CheckFacturados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckFacturados.UseVisualStyleBackColor = False
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.LightGray
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(865, -2)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 25)
        Me.CheckCerrado.TabIndex = 202
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.LightGray
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(823, -2)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 25)
        Me.CheckAbierto.TabIndex = 201
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(662, 2)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(136, 21)
        Me.ComboDeposito.TabIndex = 86
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(607, 7)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 85
        Me.Label6.Text = "Deposito"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(912, 2)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(113, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(424, 30)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(276, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(465, 26)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(97, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(319, 26)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(96, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(70, 2)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(223, 21)
        Me.ComboProveedor.TabIndex = 20
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Proveedor"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(9, 641)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 120
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(41, 641)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonAnterior.TabIndex = 121
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(72, 641)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonPosterior.TabIndex = 122
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(103, 641)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 119
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.LightBlue
        Me.Label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label1.Location = New System.Drawing.Point(181, 643)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 19)
        Me.Label1.TabIndex = 124
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(229, 646)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 13)
        Me.Label2.TabIndex = 125
        Me.Label2.Text = "Importación"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonVerDetalle
        '
        Me.ButtonVerDetalle.BackColor = System.Drawing.Color.NavajoWhite
        Me.ButtonVerDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerDetalle.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonVerDetalle.Location = New System.Drawing.Point(334, 640)
        Me.ButtonVerDetalle.Name = "ButtonVerDetalle"
        Me.ButtonVerDetalle.Size = New System.Drawing.Size(157, 32)
        Me.ButtonVerDetalle.TabIndex = 123
        Me.ButtonVerDetalle.Text = "Detalle del Lote"
        Me.ButtonVerDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerDetalle.UseVisualStyleBackColor = False
        '
        'ButtonDevolucion
        '
        Me.ButtonDevolucion.BackColor = System.Drawing.Color.NavajoWhite
        Me.ButtonDevolucion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonDevolucion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDevolucion.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.ButtonDevolucion.Location = New System.Drawing.Point(885, 639)
        Me.ButtonDevolucion.Name = "ButtonDevolucion"
        Me.ButtonDevolucion.Size = New System.Drawing.Size(157, 32)
        Me.ButtonDevolucion.TabIndex = 113
        Me.ButtonDevolucion.Text = "Devolución"
        Me.ButtonDevolucion.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonDevolucion.UseVisualStyleBackColor = False
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(607, 641)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(151, 32)
        Me.ButtonExportarExcel.TabIndex = 160
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ListaIngresoMercaderias
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.NavajoWhite
        Me.ClientSize = New System.Drawing.Size(1054, 676)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonVerDetalle)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonDevolucion)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "ListaIngresoMercaderias"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ingreso de Mercaderia"
        CType(Me.Dt, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Dt As System.Data.DataTable
    Friend WithEvents Ds As System.Data.DataSet
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonDevolucion As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonVerDetalle As System.Windows.Forms.Button
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents CheckNoFacturados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckFacturados As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Color As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TipoOperacion As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Remito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Guia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cartel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Deposito As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Sucursal As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents RemitoCliente As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextGuia As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents TextOrdenCompra As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
