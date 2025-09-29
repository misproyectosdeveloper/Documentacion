<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AnalisisComprasImportacion
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
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle33 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnalisisComprasImportacion))
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.LabelCalibre = New System.Windows.Forms.Label
        Me.ComboCalibre = New System.Windows.Forms.ComboBox
        Me.CheckNoLiquidados = New System.Windows.Forms.CheckBox
        Me.CheckLiquidados = New System.Windows.Forms.CheckBox
        Me.MaskedLote = New System.Windows.Forms.MaskedTextBox
        Me.CheckReventa = New System.Windows.Forms.CheckBox
        Me.CheckConsignacion = New System.Windows.Forms.CheckBox
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.CheckSinStock = New System.Windows.Forms.CheckBox
        Me.CheckConStock = New System.Windows.Forms.CheckBox
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label3 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Button1 = New System.Windows.Forms.Button
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Especie = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Variedad = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Calibre = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoCompra = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoFinal = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Moneda = New System.Windows.Forms.DataGridViewComboBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle23.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle23
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Lote, Me.Secuencia, Me.Candado, Me.LoteYSecuencia, Me.Proveedor, Me.Articulo, Me.Especie, Me.Variedad, Me.Calibre, Me.Fecha, Me.Cantidad, Me.Stock, Me.CostoCompra, Me.CostoFinal, Me.Moneda})
        DataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle32.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle32.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle32.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle32.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle32.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle32.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle32
        Me.Grid.Location = New System.Drawing.Point(9, 68)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle33.BackColor = System.Drawing.Color.AntiqueWhite
        DataGridViewCellStyle33.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle33
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1237, 585)
        Me.Grid.TabIndex = 103
        Me.Grid.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.LabelCalibre)
        Me.Panel1.Controls.Add(Me.ComboCalibre)
        Me.Panel1.Controls.Add(Me.CheckNoLiquidados)
        Me.Panel1.Controls.Add(Me.CheckLiquidados)
        Me.Panel1.Controls.Add(Me.MaskedLote)
        Me.Panel1.Controls.Add(Me.CheckReventa)
        Me.Panel1.Controls.Add(Me.CheckConsignacion)
        Me.Panel1.Controls.Add(Me.ComboVariedad)
        Me.Panel1.Controls.Add(Me.ComboEspecie)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.CheckSinStock)
        Me.Panel1.Controls.Add(Me.CheckConStock)
        Me.Panel1.Controls.Add(Me.ComboArticulo)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(10, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1236, 62)
        Me.Panel1.TabIndex = 136
        '
        'LabelCalibre
        '
        Me.LabelCalibre.AutoSize = True
        Me.LabelCalibre.Location = New System.Drawing.Point(932, 8)
        Me.LabelCalibre.Name = "LabelCalibre"
        Me.LabelCalibre.Size = New System.Drawing.Size(39, 13)
        Me.LabelCalibre.TabIndex = 280
        Me.LabelCalibre.Text = "Calibre"
        '
        'ComboCalibre
        '
        Me.ComboCalibre.FormattingEnabled = True
        Me.ComboCalibre.Location = New System.Drawing.Point(977, 3)
        Me.ComboCalibre.Name = "ComboCalibre"
        Me.ComboCalibre.Size = New System.Drawing.Size(121, 21)
        Me.ComboCalibre.TabIndex = 279
        '
        'CheckNoLiquidados
        '
        Me.CheckNoLiquidados.BackColor = System.Drawing.Color.LightGray
        Me.CheckNoLiquidados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckNoLiquidados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckNoLiquidados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckNoLiquidados.Location = New System.Drawing.Point(1017, 32)
        Me.CheckNoLiquidados.Name = "CheckNoLiquidados"
        Me.CheckNoLiquidados.Size = New System.Drawing.Size(90, 26)
        Me.CheckNoLiquidados.TabIndex = 278
        Me.CheckNoLiquidados.Text = "No Liquidado"
        Me.CheckNoLiquidados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckNoLiquidados.UseVisualStyleBackColor = False
        '
        'CheckLiquidados
        '
        Me.CheckLiquidados.BackColor = System.Drawing.Color.LightGray
        Me.CheckLiquidados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckLiquidados.Checked = True
        Me.CheckLiquidados.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckLiquidados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckLiquidados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckLiquidados.Location = New System.Drawing.Point(938, 32)
        Me.CheckLiquidados.Name = "CheckLiquidados"
        Me.CheckLiquidados.Size = New System.Drawing.Size(89, 26)
        Me.CheckLiquidados.TabIndex = 277
        Me.CheckLiquidados.Text = "Liquidado"
        Me.CheckLiquidados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckLiquidados.UseVisualStyleBackColor = False
        '
        'MaskedLote
        '
        Me.MaskedLote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedLote.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedLote.Location = New System.Drawing.Point(54, 35)
        Me.MaskedLote.Mask = "0000000/000"
        Me.MaskedLote.Name = "MaskedLote"
        Me.MaskedLote.Size = New System.Drawing.Size(109, 20)
        Me.MaskedLote.TabIndex = 276
        Me.MaskedLote.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedLote.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'CheckReventa
        '
        Me.CheckReventa.BackColor = System.Drawing.Color.LightGray
        Me.CheckReventa.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckReventa.Checked = True
        Me.CheckReventa.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckReventa.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckReventa.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckReventa.Location = New System.Drawing.Point(671, 34)
        Me.CheckReventa.Name = "CheckReventa"
        Me.CheckReventa.Size = New System.Drawing.Size(68, 22)
        Me.CheckReventa.TabIndex = 144
        Me.CheckReventa.Text = "Reventa"
        Me.CheckReventa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckReventa.UseVisualStyleBackColor = False
        '
        'CheckConsignacion
        '
        Me.CheckConsignacion.BackColor = System.Drawing.Color.LightGray
        Me.CheckConsignacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConsignacion.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConsignacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConsignacion.Location = New System.Drawing.Point(576, 34)
        Me.CheckConsignacion.Name = "CheckConsignacion"
        Me.CheckConsignacion.Size = New System.Drawing.Size(89, 22)
        Me.CheckConsignacion.TabIndex = 143
        Me.CheckConsignacion.Text = "Consignación"
        Me.CheckConsignacion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckConsignacion.UseVisualStyleBackColor = False
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(792, 3)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(123, 21)
        Me.ComboVariedad.TabIndex = 142
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(595, 3)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(123, 21)
        Me.ComboEspecie.TabIndex = 141
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(736, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 140
        Me.Label6.Text = "Variedad"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(546, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 139
        Me.Label5.Text = "Especie"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckSinStock
        '
        Me.CheckSinStock.BackColor = System.Drawing.Color.LightGray
        Me.CheckSinStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinStock.Checked = True
        Me.CheckSinStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckSinStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinStock.Location = New System.Drawing.Point(841, 34)
        Me.CheckSinStock.Name = "CheckSinStock"
        Me.CheckSinStock.Size = New System.Drawing.Size(72, 21)
        Me.CheckSinStock.TabIndex = 136
        Me.CheckSinStock.Text = "Sin Stock"
        Me.CheckSinStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckSinStock.UseVisualStyleBackColor = False
        '
        'CheckConStock
        '
        Me.CheckConStock.BackColor = System.Drawing.Color.LightGray
        Me.CheckConStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConStock.Checked = True
        Me.CheckConStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckConStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConStock.Location = New System.Drawing.Point(757, 34)
        Me.CheckConStock.Name = "CheckConStock"
        Me.CheckConStock.Size = New System.Drawing.Size(78, 21)
        Me.CheckConStock.TabIndex = 135
        Me.CheckConStock.Text = "Con Stock"
        Me.CheckConStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckConStock.UseVisualStyleBackColor = False
        '
        'ComboArticulo
        '
        Me.ComboArticulo.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(61, 4)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(242, 22)
        Me.ComboArticulo.TabIndex = 132
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(377, 3)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(154, 21)
        Me.ComboProveedor.TabIndex = 97
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(318, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 13)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Proveedor"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(7, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 13)
        Me.Label2.TabIndex = 91
        Me.Label2.Text = "Lote"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1121, 18)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(95, 26)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(412, 38)
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
        Me.Label4.Location = New System.Drawing.Point(189, 38)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(115, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Fecha Ingreso   Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(452, 34)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(311, 34)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Articulos"
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
        Me.ButtonPrimero.Location = New System.Drawing.Point(9, 661)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 145
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(43, 661)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonAnterior.TabIndex = 146
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(75, 661)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonPosterior.TabIndex = 143
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(107, 661)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 144
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Button1
        '
        Me.Button1.AutoEllipsis = True
        Me.Button1.BackColor = System.Drawing.Color.Transparent
        Me.Button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(545, 661)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(158, 32)
        Me.Button1.TabIndex = 147
        Me.Button1.Text = "Exportar a EXCEL"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        Me.Operacion.Width = 62
        '
        'Lote
        '
        Me.Lote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle24.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle24
        Me.Lote.HeaderText = "Lotes"
        Me.Lote.MinimumWidth = 70
        Me.Lote.Name = "Lote"
        Me.Lote.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Lote.Visible = False
        Me.Lote.Width = 70
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        Me.Secuencia.Width = 64
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.Width = 30
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle25
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 80
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 80
        '
        'Proveedor
        '
        Me.Proveedor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Proveedor.DataPropertyName = "Proveedor"
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Proveedor"
        Me.Proveedor.MinimumWidth = 80
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        Me.Proveedor.Width = 80
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 110
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Width = 110
        '
        'Especie
        '
        Me.Especie.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Especie.DataPropertyName = "Especie"
        Me.Especie.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Especie.HeaderText = "Especie"
        Me.Especie.MinimumWidth = 70
        Me.Especie.Name = "Especie"
        Me.Especie.ReadOnly = True
        Me.Especie.Width = 70
        '
        'Variedad
        '
        Me.Variedad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Variedad.DataPropertyName = "Variedad"
        Me.Variedad.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Variedad.HeaderText = "Variedad"
        Me.Variedad.MinimumWidth = 70
        Me.Variedad.Name = "Variedad"
        Me.Variedad.ReadOnly = True
        Me.Variedad.Width = 70
        '
        'Calibre
        '
        Me.Calibre.DataPropertyName = "Calibre"
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Calibre.DefaultCellStyle = DataGridViewCellStyle26
        Me.Calibre.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Calibre.HeaderText = "Calibre"
        Me.Calibre.MinimumWidth = 90
        Me.Calibre.Name = "Calibre"
        Me.Calibre.ReadOnly = True
        Me.Calibre.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Calibre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Calibre.Width = 90
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.Width = 70
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle27
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MinimumWidth = 60
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 60
        '
        'Stock
        '
        Me.Stock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Stock.DataPropertyName = "Stock"
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle28
        Me.Stock.HeaderText = "Stock"
        Me.Stock.MinimumWidth = 60
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Width = 60
        '
        'CostoCompra
        '
        Me.CostoCompra.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoCompra.DataPropertyName = "CostoCompra"
        DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoCompra.DefaultCellStyle = DataGridViewCellStyle29
        Me.CostoCompra.HeaderText = "Costo Compra"
        Me.CostoCompra.MinimumWidth = 100
        Me.CostoCompra.Name = "CostoCompra"
        Me.CostoCompra.ReadOnly = True
        '
        'CostoFinal
        '
        Me.CostoFinal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoFinal.DataPropertyName = "CostoFinal"
        DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoFinal.DefaultCellStyle = DataGridViewCellStyle30
        Me.CostoFinal.HeaderText = "Costo Final"
        Me.CostoFinal.MinimumWidth = 90
        Me.CostoFinal.Name = "CostoFinal"
        Me.CostoFinal.ReadOnly = True
        Me.CostoFinal.Width = 90
        '
        'Moneda
        '
        Me.Moneda.DataPropertyName = "Moneda"
        DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Moneda.DefaultCellStyle = DataGridViewCellStyle31
        Me.Moneda.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Moneda.HeaderText = "Moneda"
        Me.Moneda.MinimumWidth = 90
        Me.Moneda.Name = "Moneda"
        Me.Moneda.Width = 90
        '
        'AnalisisComprasImportacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1256, 699)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "AnalisisComprasImportacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Análisis de Resultado"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckSinStock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckConStock As System.Windows.Forms.CheckBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents CheckReventa As System.Windows.Forms.CheckBox
    Friend WithEvents CheckConsignacion As System.Windows.Forms.CheckBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents MaskedLote As System.Windows.Forms.MaskedTextBox
    Friend WithEvents CheckNoLiquidados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckLiquidados As System.Windows.Forms.CheckBox
    Friend WithEvents ComboCalibre As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCalibre As System.Windows.Forms.Label
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Especie As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Variedad As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Calibre As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoCompra As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoFinal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Moneda As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
