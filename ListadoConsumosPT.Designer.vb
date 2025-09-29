<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaConsumosPT
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaConsumosPT))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextConsumo = New System.Windows.Forms.TextBox
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Consumo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Negocio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Costeo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Deposito = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cartel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Panel1.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextConsumo)
        Me.Panel1.Controls.Add(Me.ComboCosteo)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboNegocio)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Controls.Add(Me.ComboEstado)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Location = New System.Drawing.Point(13, 1)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(937, 61)
        Me.Panel1.TabIndex = 162
        '
        'TextConsumo
        '
        Me.TextConsumo.Location = New System.Drawing.Point(143, 30)
        Me.TextConsumo.MaxLength = 8
        Me.TextConsumo.Name = "TextConsumo"
        Me.TextConsumo.Size = New System.Drawing.Size(113, 20)
        Me.TextConsumo.TabIndex = 210
        Me.TextConsumo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(381, 2)
        Me.ComboCosteo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(235, 21)
        Me.ComboCosteo.TabIndex = 208
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(328, 6)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 209
        Me.Label3.Text = "Costeo"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(69, 2)
        Me.ComboNegocio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(243, 21)
        Me.ComboNegocio.TabIndex = 206
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(8, 6)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 207
        Me.Label2.Text = "Negocio"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(534, 23)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 30)
        Me.CheckCerrado.TabIndex = 200
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(492, 23)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 30)
        Me.CheckAbierto.TabIndex = 199
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(634, 29)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(100, 21)
        Me.ComboEstado.TabIndex = 98
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(587, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 97
        Me.Label1.Text = "Estado"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(84, 32)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 95
        Me.Label9.Text = "Consumo"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(332, 30)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(117, 21)
        Me.ComboDeposito.TabIndex = 86
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(277, 33)
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
        Me.ButtonAceptar.Location = New System.Drawing.Point(806, 31)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(108, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(783, 7)
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
        Me.Label4.Location = New System.Drawing.Point(632, 7)
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
        Me.DateTimeHasta.Location = New System.Drawing.Point(825, 4)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(677, 4)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(456, 660)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(136, 31)
        Me.ButtonExportarExcel.TabIndex = 171
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(12, 661)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 165
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(45, 661)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 166
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(78, 661)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 167
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(111, 661)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 164
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
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
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Consumo, Me.Negocio, Me.Costeo, Me.Fecha, Me.Deposito, Me.Cartel, Me.Estado})
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle5
        Me.Grid.Location = New System.Drawing.Point(12, 66)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(938, 590)
        Me.Grid.TabIndex = 163
        '
        'Operacion
        '
        Me.Operacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        Me.Operacion.Width = 62
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
        'Consumo
        '
        Me.Consumo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Consumo.DataPropertyName = "Consumo"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Consumo.DefaultCellStyle = DataGridViewCellStyle2
        Me.Consumo.HeaderText = "Consumo"
        Me.Consumo.MinimumWidth = 110
        Me.Consumo.Name = "Consumo"
        Me.Consumo.ReadOnly = True
        Me.Consumo.Width = 110
        '
        'Negocio
        '
        Me.Negocio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Negocio.DataPropertyName = "Negocio"
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        Me.Negocio.DefaultCellStyle = DataGridViewCellStyle3
        Me.Negocio.HeaderText = "Negocio"
        Me.Negocio.MinimumWidth = 160
        Me.Negocio.Name = "Negocio"
        Me.Negocio.ReadOnly = True
        Me.Negocio.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Negocio.Width = 160
        '
        'Costeo
        '
        Me.Costeo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Costeo.DataPropertyName = "Costeo"
        Me.Costeo.HeaderText = "Costeo"
        Me.Costeo.MinimumWidth = 160
        Me.Costeo.Name = "Costeo"
        Me.Costeo.ReadOnly = True
        Me.Costeo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Costeo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Costeo.Width = 160
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
        'Deposito
        '
        Me.Deposito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Deposito.DataPropertyName = "Deposito"
        Me.Deposito.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Deposito.HeaderText = "Deposito"
        Me.Deposito.MinimumWidth = 120
        Me.Deposito.Name = "Deposito"
        Me.Deposito.ReadOnly = True
        Me.Deposito.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Deposito.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Deposito.Width = 120
        '
        'Cartel
        '
        Me.Cartel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cartel.DataPropertyName = "Cartel"
        Me.Cartel.HeaderText = ""
        Me.Cartel.MinimumWidth = 120
        Me.Cartel.Name = "Cartel"
        Me.Cartel.ReadOnly = True
        Me.Cartel.Width = 120
        '
        'Estado
        '
        Me.Estado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle4
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 90
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Estado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Estado.Width = 90
        '
        'ListaConsumosPT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(962, 695)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Grid)
        Me.Name = "ListaConsumosPT"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ListadoConsumosPT"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextConsumo As System.Windows.Forms.TextBox
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Consumo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Negocio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Costeo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Deposito As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cartel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
