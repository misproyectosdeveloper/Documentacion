<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaFacturasProveedor
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
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaFacturasProveedor))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextRendicion = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboProveedorFondoFijo = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextNumeroFondoFijo = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.CheckBoxDomestica = New System.Windows.Forms.CheckBox
        Me.CheckBoxExterior = New System.Windows.Forms.CheckBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.MaskedReciboOficial = New System.Windows.Forms.MaskedTextBox
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EsReventa = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EsInsumos = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EsSinComprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EsCostoLotes = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Factura = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MarcaNotaDebito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ReciboOficial = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaContable = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Concepto = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.IncoTerm = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Liquidacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Rendicion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OperacionN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CandadoN = New System.Windows.Forms.DataGridViewImageColumn
        Me.ImporteN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Total = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonReemplazar = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.ComboEstado)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextRendicion)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.ComboProveedorFondoFijo)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TextNumeroFondoFijo)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.CheckBoxDomestica)
        Me.Panel1.Controls.Add(Me.CheckBoxExterior)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.MaskedReciboOficial)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Location = New System.Drawing.Point(2, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1149, 71)
        Me.Panel1.TabIndex = 123
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.CheckCerrado)
        Me.Panel2.Controls.Add(Me.CheckAbierto)
        Me.Panel2.Location = New System.Drawing.Point(875, 47)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(97, 19)
        Me.Panel2.TabIndex = 314
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = Global.ScomerV01.My.Resources.Resources.Ccerrado
        Me.CheckCerrado.Location = New System.Drawing.Point(55, -6)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 30)
        Me.CheckCerrado.TabIndex = 270
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Transparent
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = Global.ScomerV01.My.Resources.Resources.CAbierto
        Me.CheckAbierto.Location = New System.Drawing.Point(9, -6)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 30)
        Me.CheckAbierto.TabIndex = 269
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(723, 24)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(103, 21)
        Me.ComboEstado.TabIndex = 313
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(675, 28)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 312
        Me.Label2.Text = "Estado"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextRendicion
        '
        Me.TextRendicion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRendicion.Location = New System.Drawing.Point(582, 25)
        Me.TextRendicion.MaxLength = 3
        Me.TextRendicion.Name = "TextRendicion"
        Me.TextRendicion.Size = New System.Drawing.Size(83, 20)
        Me.TextRendicion.TabIndex = 311
        Me.TextRendicion.TabStop = False
        Me.TextRendicion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(520, 28)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 13)
        Me.Label11.TabIndex = 310
        Me.Label11.Text = "Rendición"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboProveedorFondoFijo
        '
        Me.ComboProveedorFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedorFondoFijo.FormattingEnabled = True
        Me.ComboProveedorFondoFijo.Location = New System.Drawing.Point(419, 0)
        Me.ComboProveedorFondoFijo.Name = "ComboProveedorFondoFijo"
        Me.ComboProveedorFondoFijo.Size = New System.Drawing.Size(207, 21)
        Me.ComboProveedorFondoFijo.TabIndex = 309
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(329, 6)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(84, 13)
        Me.Label10.TabIndex = 308
        Me.Label10.Text = "Proveedor F.Fijo"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextNumeroFondoFijo
        '
        Me.TextNumeroFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeroFondoFijo.Location = New System.Drawing.Point(694, 2)
        Me.TextNumeroFondoFijo.MaxLength = 3
        Me.TextNumeroFondoFijo.Name = "TextNumeroFondoFijo"
        Me.TextNumeroFondoFijo.Size = New System.Drawing.Size(83, 20)
        Me.TextNumeroFondoFijo.TabIndex = 307
        Me.TextNumeroFondoFijo.TabStop = False
        Me.TextNumeroFondoFijo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(632, 5)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(55, 13)
        Me.Label9.TabIndex = 203
        Me.Label9.Text = "Nro. F.Fijo"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(68, 24)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(255, 21)
        Me.ComboAlias.TabIndex = 202
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(2, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 201
        Me.Label3.Text = "Alias"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckBoxDomestica
        '
        Me.CheckBoxDomestica.AutoSize = True
        Me.CheckBoxDomestica.Location = New System.Drawing.Point(715, 49)
        Me.CheckBoxDomestica.Name = "CheckBoxDomestica"
        Me.CheckBoxDomestica.Size = New System.Drawing.Size(76, 17)
        Me.CheckBoxDomestica.TabIndex = 200
        Me.CheckBoxDomestica.Text = "Domestica"
        Me.CheckBoxDomestica.UseVisualStyleBackColor = True
        '
        'CheckBoxExterior
        '
        Me.CheckBoxExterior.AutoSize = True
        Me.CheckBoxExterior.Location = New System.Drawing.Point(805, 49)
        Me.CheckBoxExterior.Name = "CheckBoxExterior"
        Me.CheckBoxExterior.Size = New System.Drawing.Size(61, 17)
        Me.CheckBoxExterior.TabIndex = 199
        Me.CheckBoxExterior.Text = "Exterior"
        Me.CheckBoxExterior.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(334, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 197
        Me.Label1.Text = "Factura"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedReciboOficial
        '
        Me.MaskedReciboOficial.BackColor = System.Drawing.Color.White
        Me.MaskedReciboOficial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedReciboOficial.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedReciboOficial.Location = New System.Drawing.Point(385, 24)
        Me.MaskedReciboOficial.Mask = "0000-00000000"
        Me.MaskedReciboOficial.Name = "MaskedReciboOficial"
        Me.MaskedReciboOficial.Size = New System.Drawing.Size(119, 21)
        Me.MaskedReciboOficial.TabIndex = 196
        Me.MaskedReciboOficial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedReciboOficial.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(68, 2)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(255, 21)
        Me.ComboProveedor.TabIndex = 187
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(998, 32)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(122, 22)
        Me.ButtonAceptar.TabIndex = 186
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 6)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 103
        Me.Label8.Text = "Proveedor"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(952, 6)
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
        Me.Label4.Location = New System.Drawing.Point(786, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(994, 2)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(115, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(830, 2)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(116, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.EsReventa, Me.EsInsumos, Me.EsSinComprobante, Me.EsCostoLotes, Me.Candado, Me.Factura, Me.MarcaNotaDebito, Me.ReciboOficial, Me.Proveedor, Me.Fecha, Me.FechaContable, Me.Concepto, Me.IncoTerm, Me.Liquidacion, Me.Rendicion, Me.Importe, Me.OperacionN, Me.CandadoN, Me.ImporteN, Me.Total, Me.Estado})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Lavender
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle11
        Me.Grid.Location = New System.Drawing.Point(2, 72)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.LightCyan
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.DimGray
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1149, 567)
        Me.Grid.TabIndex = 124
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        Me.Operacion.Width = 81
        '
        'EsReventa
        '
        Me.EsReventa.DataPropertyName = "EsReventa"
        Me.EsReventa.HeaderText = "EsReventa"
        Me.EsReventa.Name = "EsReventa"
        Me.EsReventa.Visible = False
        Me.EsReventa.Width = 85
        '
        'EsInsumos
        '
        Me.EsInsumos.DataPropertyName = "EsInsumos"
        Me.EsInsumos.HeaderText = "EsInsumos"
        Me.EsInsumos.Name = "EsInsumos"
        Me.EsInsumos.Visible = False
        Me.EsInsumos.Width = 83
        '
        'EsSinComprobante
        '
        Me.EsSinComprobante.DataPropertyName = "EsSinComprobante"
        Me.EsSinComprobante.HeaderText = "EsSinComprobante"
        Me.EsSinComprobante.Name = "EsSinComprobante"
        Me.EsSinComprobante.Visible = False
        Me.EsSinComprobante.Width = 122
        '
        'EsCostoLotes
        '
        Me.EsCostoLotes.DataPropertyName = "EsCostoLotes"
        Me.EsCostoLotes.HeaderText = "EsCostoLotes"
        Me.EsCostoLotes.Name = "EsCostoLotes"
        Me.EsCostoLotes.Visible = False
        Me.EsCostoLotes.Width = 97
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.Visible = False
        Me.Candado.Width = 30
        '
        'Factura
        '
        Me.Factura.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Factura.DataPropertyName = "Factura"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Factura.DefaultCellStyle = DataGridViewCellStyle2
        Me.Factura.HeaderText = "Nro.Interno"
        Me.Factura.MinimumWidth = 70
        Me.Factura.Name = "Factura"
        Me.Factura.ReadOnly = True
        Me.Factura.Width = 70
        '
        'MarcaNotaDebito
        '
        Me.MarcaNotaDebito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.MarcaNotaDebito.DataPropertyName = "MarcaNotaDebito"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Red
        Me.MarcaNotaDebito.DefaultCellStyle = DataGridViewCellStyle3
        Me.MarcaNotaDebito.HeaderText = ""
        Me.MarcaNotaDebito.MinimumWidth = 20
        Me.MarcaNotaDebito.Name = "MarcaNotaDebito"
        Me.MarcaNotaDebito.ReadOnly = True
        Me.MarcaNotaDebito.Width = 20
        '
        'ReciboOficial
        '
        Me.ReciboOficial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ReciboOficial.DataPropertyName = "ReciboOficial"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ReciboOficial.DefaultCellStyle = DataGridViewCellStyle4
        Me.ReciboOficial.HeaderText = "Factura"
        Me.ReciboOficial.MinimumWidth = 100
        Me.ReciboOficial.Name = "ReciboOficial"
        Me.ReciboOficial.ReadOnly = True
        '
        'Proveedor
        '
        Me.Proveedor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Proveedor.DataPropertyName = "Proveedor"
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Proveedor"
        Me.Proveedor.MinimumWidth = 140
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        Me.Proveedor.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Proveedor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Proveedor.Width = 140
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'FechaContable
        '
        Me.FechaContable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.FechaContable.DataPropertyName = "FechaContable"
        Me.FechaContable.HeaderText = "F.Contable"
        Me.FechaContable.MinimumWidth = 70
        Me.FechaContable.Name = "FechaContable"
        Me.FechaContable.ReadOnly = True
        Me.FechaContable.Width = 70
        '
        'Concepto
        '
        Me.Concepto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto.DataPropertyName = "Concepto"
        Me.Concepto.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Concepto.HeaderText = "Concepto G."
        Me.Concepto.MinimumWidth = 85
        Me.Concepto.Name = "Concepto"
        Me.Concepto.Width = 85
        '
        'IncoTerm
        '
        Me.IncoTerm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.IncoTerm.DataPropertyName = "Incoterm"
        Me.IncoTerm.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.IncoTerm.HeaderText = "I.T."
        Me.IncoTerm.MinimumWidth = 30
        Me.IncoTerm.Name = "IncoTerm"
        Me.IncoTerm.ReadOnly = True
        Me.IncoTerm.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.IncoTerm.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.IncoTerm.Width = 30
        '
        'Liquidacion
        '
        Me.Liquidacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Liquidacion.DataPropertyName = "Liquidacion"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Liquidacion.DefaultCellStyle = DataGridViewCellStyle5
        Me.Liquidacion.HeaderText = "Reemplazo"
        Me.Liquidacion.MinimumWidth = 70
        Me.Liquidacion.Name = "Liquidacion"
        Me.Liquidacion.ReadOnly = True
        Me.Liquidacion.Width = 70
        '
        'Rendicion
        '
        Me.Rendicion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Rendicion.DataPropertyName = "Rendicion"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Rendicion.DefaultCellStyle = DataGridViewCellStyle6
        Me.Rendicion.HeaderText = "Rendición"
        Me.Rendicion.MinimumWidth = 60
        Me.Rendicion.Name = "Rendicion"
        Me.Rendicion.ReadOnly = True
        Me.Rendicion.Width = 60
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle7
        Me.Importe.HeaderText = "Importe"
        Me.Importe.MinimumWidth = 90
        Me.Importe.Name = "Importe"
        Me.Importe.ReadOnly = True
        Me.Importe.Width = 90
        '
        'OperacionN
        '
        Me.OperacionN.DataPropertyName = "OperacionN"
        Me.OperacionN.HeaderText = "OperacionN"
        Me.OperacionN.Name = "OperacionN"
        Me.OperacionN.Visible = False
        Me.OperacionN.Width = 89
        '
        'CandadoN
        '
        Me.CandadoN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CandadoN.HeaderText = ""
        Me.CandadoN.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.CandadoN.MinimumWidth = 30
        Me.CandadoN.Name = "CandadoN"
        Me.CandadoN.ReadOnly = True
        Me.CandadoN.Width = 30
        '
        'ImporteN
        '
        Me.ImporteN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ImporteN.DataPropertyName = "ImporteN"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ImporteN.DefaultCellStyle = DataGridViewCellStyle8
        Me.ImporteN.HeaderText = "Importe"
        Me.ImporteN.MinimumWidth = 90
        Me.ImporteN.Name = "ImporteN"
        Me.ImporteN.ReadOnly = True
        Me.ImporteN.Width = 90
        '
        'Total
        '
        Me.Total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Total.DataPropertyName = "Total"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Total.DefaultCellStyle = DataGridViewCellStyle9
        Me.Total.HeaderText = "Total"
        Me.Total.MinimumWidth = 90
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Width = 90
        '
        'Estado
        '
        Me.Estado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Red
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle10
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 60
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.Width = 60
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(102, 645)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 129
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(69, 645)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 132
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(36, 645)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 131
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(3, 645)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 130
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonReemplazar
        '
        Me.ButtonReemplazar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonReemplazar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonReemplazar.Location = New System.Drawing.Point(956, 645)
        Me.ButtonReemplazar.Name = "ButtonReemplazar"
        Me.ButtonReemplazar.Size = New System.Drawing.Size(190, 29)
        Me.ButtonReemplazar.TabIndex = 144
        Me.ButtonReemplazar.Text = "Reemplazar Por Liquidación"
        Me.ButtonReemplazar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonReemplazar.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.White
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(158, 650)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 15)
        Me.Label5.TabIndex = 145
        Me.Label5.Text = " X "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(186, 650)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(228, 13)
        Me.Label6.TabIndex = 146
        Me.Label6.Text = "Facturas con Notas Debito/Credito Pendientes"
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(554, 645)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(190, 29)
        Me.ButtonExportarExcel.TabIndex = 160
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ListaFacturasProveedor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(1155, 676)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ButtonReemplazar)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "ListaFacturasProveedor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Facturas de Proveedores"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonReemplazar As System.Windows.Forms.Button
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MaskedReciboOficial As System.Windows.Forms.MaskedTextBox
    Friend WithEvents CheckBoxExterior As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDomestica As System.Windows.Forms.CheckBox
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextNumeroFondoFijo As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboProveedorFondoFijo As System.Windows.Forms.ComboBox
    Friend WithEvents TextRendicion As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EsReventa As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EsInsumos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EsSinComprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EsCostoLotes As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Factura As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MarcaNotaDebito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReciboOficial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaContable As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Concepto As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents IncoTerm As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Liquidacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Rendicion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OperacionN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CandadoN As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ImporteN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Total As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
End Class
