<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnGastoBancario
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnGastoBancario))
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextTotalIva = New System.Windows.Forms.TextBox
        Me.TextBaseImponible = New System.Windows.Forms.TextBox
        Me.TextGrabado = New System.Windows.Forms.TextBox
        Me.ButtonEliminarLineaConcepto = New System.Windows.Forms.Button
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.Grid1 = New System.Windows.Forms.DataGridView
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TieneLupa = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Item = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Concepto1 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ImportePantalla = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lupa = New System.Windows.Forms.DataGridViewImageColumn
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.TextLetra = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.MaskedReciboOficial = New System.Windows.Forms.MaskedTextBox
        Me.PanelMoneda = New System.Windows.Forms.Panel
        Me.TextCambio = New System.Windows.Forms.TextBox
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.PictureAlmanaqueHasta = New System.Windows.Forms.PictureBox
        Me.TextFechaHasta = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.PictureAlmanaqueDesde = New System.Windows.Forms.PictureBox
        Me.TextFechaDesde = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureAlmanaqueContable = New System.Windows.Forms.PictureBox
        Me.TextFechaContable = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.TextCuenta = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboBancos = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextMovimiento = New System.Windows.Forms.TextBox
        Me.ButtonBaja = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.PanelMoneda.SuspendLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = ""
        Me.DataGridViewImageColumn1.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.MinimumWidth = 30
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 30
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Location = New System.Drawing.Point(24, 34)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(854, 587)
        Me.Panel1.TabIndex = 1
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Wheat
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.TextTotalIva)
        Me.Panel2.Controls.Add(Me.TextBaseImponible)
        Me.Panel2.Controls.Add(Me.TextGrabado)
        Me.Panel2.Controls.Add(Me.ButtonEliminarLineaConcepto)
        Me.Panel2.Controls.Add(Me.TextImporte)
        Me.Panel2.Controls.Add(Me.Grid1)
        Me.Panel2.Location = New System.Drawing.Point(39, 105)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(781, 471)
        Me.Panel2.TabIndex = 262
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(496, 443)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(51, 13)
        Me.Label9.TabIndex = 280
        Me.Label9.Text = "Total IVA"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(289, 443)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 13)
        Me.Label8.TabIndex = 279
        Me.Label8.Text = "Base Imponible"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(102, 443)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 278
        Me.Label3.Text = "Grabado"
        '
        'TextTotalIva
        '
        Me.TextTotalIva.BackColor = System.Drawing.Color.White
        Me.TextTotalIva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalIva.Location = New System.Drawing.Point(558, 439)
        Me.TextTotalIva.MaxLength = 20
        Me.TextTotalIva.Name = "TextTotalIva"
        Me.TextTotalIva.ReadOnly = True
        Me.TextTotalIva.Size = New System.Drawing.Size(104, 20)
        Me.TextTotalIva.TabIndex = 267
        Me.TextTotalIva.TabStop = False
        Me.TextTotalIva.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBaseImponible
        '
        Me.TextBaseImponible.BackColor = System.Drawing.Color.White
        Me.TextBaseImponible.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBaseImponible.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBaseImponible.Location = New System.Drawing.Point(378, 438)
        Me.TextBaseImponible.MaxLength = 20
        Me.TextBaseImponible.Name = "TextBaseImponible"
        Me.TextBaseImponible.ReadOnly = True
        Me.TextBaseImponible.Size = New System.Drawing.Size(104, 20)
        Me.TextBaseImponible.TabIndex = 266
        Me.TextBaseImponible.TabStop = False
        Me.TextBaseImponible.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextGrabado
        '
        Me.TextGrabado.BackColor = System.Drawing.Color.White
        Me.TextGrabado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextGrabado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextGrabado.Location = New System.Drawing.Point(157, 439)
        Me.TextGrabado.MaxLength = 20
        Me.TextGrabado.Name = "TextGrabado"
        Me.TextGrabado.ReadOnly = True
        Me.TextGrabado.Size = New System.Drawing.Size(116, 20)
        Me.TextGrabado.TabIndex = 265
        Me.TextGrabado.TabStop = False
        Me.TextGrabado.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonEliminarLineaConcepto
        '
        Me.ButtonEliminarLineaConcepto.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLineaConcepto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLineaConcepto.Location = New System.Drawing.Point(182, 399)
        Me.ButtonEliminarLineaConcepto.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLineaConcepto.Name = "ButtonEliminarLineaConcepto"
        Me.ButtonEliminarLineaConcepto.Size = New System.Drawing.Size(98, 20)
        Me.ButtonEliminarLineaConcepto.TabIndex = 264
        Me.ButtonEliminarLineaConcepto.TabStop = False
        Me.ButtonEliminarLineaConcepto.Text = "Borrar Linea"
        Me.ButtonEliminarLineaConcepto.UseVisualStyleBackColor = False
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(363, 399)
        Me.TextImporte.MaxLength = 20
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.ReadOnly = True
        Me.TextImporte.Size = New System.Drawing.Size(124, 20)
        Me.TextImporte.TabIndex = 263
        Me.TextImporte.TabStop = False
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Grid1
        '
        Me.Grid1.AllowUserToDeleteRows = False
        Me.Grid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid1.BackgroundColor = System.Drawing.Color.White
        Me.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Importe, Me.TieneLupa, Me.Item, Me.Concepto1, Me.ImportePantalla, Me.Lupa})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid1.DefaultCellStyle = DataGridViewCellStyle3
        Me.Grid1.Location = New System.Drawing.Point(182, 8)
        Me.Grid1.MultiSelect = False
        Me.Grid1.Name = "Grid1"
        Me.Grid1.RowHeadersWidth = 45
        Me.Grid1.Size = New System.Drawing.Size(401, 384)
        Me.Grid1.TabIndex = 262
        '
        'Importe
        '
        Me.Importe.DataPropertyName = "Importe"
        Me.Importe.HeaderText = "Importe"
        Me.Importe.Name = "Importe"
        Me.Importe.Visible = False
        Me.Importe.Width = 67
        '
        'TieneLupa
        '
        Me.TieneLupa.DataPropertyName = "TieneLupa"
        Me.TieneLupa.HeaderText = "TieneLupa"
        Me.TieneLupa.Name = "TieneLupa"
        Me.TieneLupa.Visible = False
        Me.TieneLupa.Width = 83
        '
        'Item
        '
        Me.Item.DataPropertyName = "Item"
        Me.Item.HeaderText = "Item"
        Me.Item.Name = "Item"
        Me.Item.Visible = False
        Me.Item.Width = 52
        '
        'Concepto1
        '
        Me.Concepto1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto1.DataPropertyName = "Concepto"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        Me.Concepto1.DefaultCellStyle = DataGridViewCellStyle1
        Me.Concepto1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Concepto1.HeaderText = "Concepto"
        Me.Concepto1.MinimumWidth = 160
        Me.Concepto1.Name = "Concepto1"
        Me.Concepto1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Concepto1.Width = 160
        '
        'ImportePantalla
        '
        Me.ImportePantalla.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ImportePantalla.DataPropertyName = "ImportePantalla"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        Me.ImportePantalla.DefaultCellStyle = DataGridViewCellStyle2
        Me.ImportePantalla.HeaderText = "Importe"
        Me.ImportePantalla.MaxInputLength = 10
        Me.ImportePantalla.MinimumWidth = 120
        Me.ImportePantalla.Name = "ImportePantalla"
        Me.ImportePantalla.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ImportePantalla.Width = 120
        '
        'Lupa
        '
        Me.Lupa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lupa.HeaderText = "Prov."
        Me.Lupa.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Lupa.MinimumWidth = 35
        Me.Lupa.Name = "Lupa"
        Me.Lupa.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Lupa.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Lupa.Width = 35
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Wheat
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.PictureCandado)
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.PanelMoneda)
        Me.Panel3.Controls.Add(Me.PictureAlmanaqueHasta)
        Me.Panel3.Controls.Add(Me.TextFechaHasta)
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.PictureAlmanaqueDesde)
        Me.Panel3.Controls.Add(Me.TextFechaDesde)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.PictureAlmanaqueContable)
        Me.Panel3.Controls.Add(Me.TextFechaContable)
        Me.Panel3.Controls.Add(Me.Label22)
        Me.Panel3.Controls.Add(Me.TextCuenta)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.ComboBancos)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.TextComentario)
        Me.Panel3.Location = New System.Drawing.Point(40, 1)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(780, 98)
        Me.Panel3.TabIndex = 15
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(726, 9)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(44, 43)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 1020
        Me.PictureCandado.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.TextLetra)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.MaskedReciboOficial)
        Me.Panel4.Location = New System.Drawing.Point(427, 29)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(311, 29)
        Me.Panel4.TabIndex = 1021
        '
        'TextLetra
        '
        Me.TextLetra.BackColor = System.Drawing.Color.White
        Me.TextLetra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextLetra.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLetra.Location = New System.Drawing.Point(114, 3)
        Me.TextLetra.MaxLength = 1
        Me.TextLetra.Name = "TextLetra"
        Me.TextLetra.Size = New System.Drawing.Size(35, 24)
        Me.TextLetra.TabIndex = 309
        Me.TextLetra.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(77, 8)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(31, 13)
        Me.Label11.TabIndex = 308
        Me.Label11.Text = "Letra"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(3, 9)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(73, 13)
        Me.Label12.TabIndex = 307
        Me.Label12.Text = "Recibo Oficial"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedReciboOficial
        '
        Me.MaskedReciboOficial.BackColor = System.Drawing.Color.White
        Me.MaskedReciboOficial.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedReciboOficial.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedReciboOficial.Location = New System.Drawing.Point(155, 3)
        Me.MaskedReciboOficial.Mask = "0000-00000000"
        Me.MaskedReciboOficial.Name = "MaskedReciboOficial"
        Me.MaskedReciboOficial.Size = New System.Drawing.Size(133, 24)
        Me.MaskedReciboOficial.TabIndex = 304
        Me.MaskedReciboOficial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedReciboOficial.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'PanelMoneda
        '
        Me.PanelMoneda.BackColor = System.Drawing.Color.Plum
        Me.PanelMoneda.Controls.Add(Me.TextCambio)
        Me.PanelMoneda.Controls.Add(Me.ComboMoneda)
        Me.PanelMoneda.Controls.Add(Me.Label17)
        Me.PanelMoneda.Controls.Add(Me.Label18)
        Me.PanelMoneda.Location = New System.Drawing.Point(331, 60)
        Me.PanelMoneda.Name = "PanelMoneda"
        Me.PanelMoneda.Size = New System.Drawing.Size(310, 31)
        Me.PanelMoneda.TabIndex = 1019
        '
        'TextCambio
        '
        Me.TextCambio.BackColor = System.Drawing.Color.White
        Me.TextCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCambio.Location = New System.Drawing.Point(233, 4)
        Me.TextCambio.MaxLength = 10
        Me.TextCambio.Name = "TextCambio"
        Me.TextCambio.Size = New System.Drawing.Size(59, 21)
        Me.TextCambio.TabIndex = 4
        Me.TextCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.ForeColor = System.Drawing.Color.Black
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(79, 5)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(96, 23)
        Me.ComboMoneda.TabIndex = 144
        Me.ComboMoneda.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(0, 7)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(75, 15)
        Me.Label17.TabIndex = 143
        Me.Label17.Text = "Importes  en"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(178, 7)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(50, 15)
        Me.Label18.TabIndex = 142
        Me.Label18.Text = "Cambio"
        '
        'PictureAlmanaqueHasta
        '
        Me.PictureAlmanaqueHasta.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueHasta.Location = New System.Drawing.Point(386, 31)
        Me.PictureAlmanaqueHasta.Name = "PictureAlmanaqueHasta"
        Me.PictureAlmanaqueHasta.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueHasta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueHasta.TabIndex = 1018
        Me.PictureAlmanaqueHasta.TabStop = False
        '
        'TextFechaHasta
        '
        Me.TextFechaHasta.BackColor = System.Drawing.Color.White
        Me.TextFechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaHasta.Location = New System.Drawing.Point(298, 31)
        Me.TextFechaHasta.MaxLength = 10
        Me.TextFechaHasta.Name = "TextFechaHasta"
        Me.TextFechaHasta.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaHasta.TabIndex = 1017
        Me.TextFechaHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(273, 36)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(18, 13)
        Me.Label5.TabIndex = 1016
        Me.Label5.Text = "Al"
        '
        'PictureAlmanaqueDesde
        '
        Me.PictureAlmanaqueDesde.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueDesde.Location = New System.Drawing.Point(240, 31)
        Me.PictureAlmanaqueDesde.Name = "PictureAlmanaqueDesde"
        Me.PictureAlmanaqueDesde.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueDesde.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueDesde.TabIndex = 1015
        Me.PictureAlmanaqueDesde.TabStop = False
        '
        'TextFechaDesde
        '
        Me.TextFechaDesde.BackColor = System.Drawing.Color.White
        Me.TextFechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaDesde.Location = New System.Drawing.Point(152, 31)
        Me.TextFechaDesde.MaxLength = 10
        Me.TextFechaDesde.Name = "TextFechaDesde"
        Me.TextFechaDesde.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaDesde.TabIndex = 1014
        Me.TextFechaDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(141, 13)
        Me.Label4.TabIndex = 1013
        Me.Label4.Text = "Periodo del Gastos  Del"
        '
        'PictureAlmanaqueContable
        '
        Me.PictureAlmanaqueContable.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueContable.Location = New System.Drawing.Point(568, 5)
        Me.PictureAlmanaqueContable.Name = "PictureAlmanaqueContable"
        Me.PictureAlmanaqueContable.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueContable.TabIndex = 1012
        Me.PictureAlmanaqueContable.TabStop = False
        '
        'TextFechaContable
        '
        Me.TextFechaContable.BackColor = System.Drawing.Color.White
        Me.TextFechaContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaContable.Location = New System.Drawing.Point(480, 5)
        Me.TextFechaContable.MaxLength = 10
        Me.TextFechaContable.Name = "TextFechaContable"
        Me.TextFechaContable.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaContable.TabIndex = 1011
        Me.TextFechaContable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(377, 8)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(96, 13)
        Me.Label22.TabIndex = 1010
        Me.Label22.Text = "Fecha Contable"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuenta
        '
        Me.TextCuenta.BackColor = System.Drawing.Color.White
        Me.TextCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuenta.Location = New System.Drawing.Point(234, 5)
        Me.TextCuenta.MaxLength = 8
        Me.TextCuenta.Name = "TextCuenta"
        Me.TextCuenta.ReadOnly = True
        Me.TextCuenta.Size = New System.Drawing.Size(130, 20)
        Me.TextCuenta.TabIndex = 279
        Me.TextCuenta.TabStop = False
        Me.TextCuenta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(183, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(47, 13)
        Me.Label2.TabIndex = 278
        Me.Label2.Text = "Cuenta"
        '
        'ComboBancos
        '
        Me.ComboBancos.BackColor = System.Drawing.Color.White
        Me.ComboBancos.Enabled = False
        Me.ComboBancos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancos.FormattingEnabled = True
        Me.ComboBancos.Location = New System.Drawing.Point(53, 4)
        Me.ComboBancos.Name = "ComboBancos"
        Me.ComboBancos.Size = New System.Drawing.Size(120, 21)
        Me.ComboBancos.TabIndex = 275
        Me.ComboBancos.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 277
        Me.Label1.Text = "Banco"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(1, 70)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 273
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(72, 68)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(224, 20)
        Me.TextComentario.TabIndex = 16
        Me.TextComentario.TabStop = False
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(435, 8)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 206
        Me.DateTime1.TabStop = False
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(770, 7)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 211
        Me.ComboEstado.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(719, 11)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 210
        Me.Label15.Text = "Estado"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(386, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 209
        Me.Label10.Text = "Fecha "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(28, 12)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 13)
        Me.Label6.TabIndex = 208
        Me.Label6.Text = "Movimiento"
        '
        'TextMovimiento
        '
        Me.TextMovimiento.BackColor = System.Drawing.Color.White
        Me.TextMovimiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMovimiento.Location = New System.Drawing.Point(107, 9)
        Me.TextMovimiento.MaxLength = 8
        Me.TextMovimiento.Name = "TextMovimiento"
        Me.TextMovimiento.ReadOnly = True
        Me.TextMovimiento.Size = New System.Drawing.Size(130, 20)
        Me.TextMovimiento.TabIndex = 207
        Me.TextMovimiento.TabStop = False
        Me.TextMovimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonBaja
        '
        Me.ButtonBaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBaja.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonBaja.Location = New System.Drawing.Point(254, 635)
        Me.ButtonBaja.Name = "ButtonBaja"
        Me.ButtonBaja.Size = New System.Drawing.Size(137, 29)
        Me.ButtonBaja.TabIndex = 213
        Me.ButtonBaja.TabStop = False
        Me.ButtonBaja.Text = "Baja Movimiento"
        Me.ButtonBaja.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBaja.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(750, 633)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(129, 29)
        Me.ButtonAceptar.TabIndex = 10
        Me.ButtonAceptar.Text = "Acepta Cambios"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(493, 634)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 28)
        Me.ButtonAsientoContable.TabIndex = 299
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(21, 635)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(144, 29)
        Me.ButtonNuevo.TabIndex = 300
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nuevo Movimiento"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'UnGastoBancario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(906, 676)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonBaja)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextMovimiento)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UnGastoBancario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gastos Bancarios"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.PanelMoneda.ResumeLayout(False)
        Me.PanelMoneda.PerformLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextCuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBancos As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextMovimiento As System.Windows.Forms.TextBox
    Friend WithEvents ButtonBaja As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureAlmanaqueContable As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaContable As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents PictureAlmanaqueHasta As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaHasta As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents PictureAlmanaqueDesde As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaDesde As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarLineaConcepto As System.Windows.Forms.Button
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Grid1 As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents PanelMoneda As System.Windows.Forms.Panel
    Friend WithEvents TextCambio As System.Windows.Forms.TextBox
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TieneLupa As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Item As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Concepto1 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ImportePantalla As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lupa As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents TextTotalIva As System.Windows.Forms.TextBox
    Friend WithEvents TextBaseImponible As System.Windows.Forms.TextBox
    Friend WithEvents TextGrabado As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents TextLetra As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents MaskedReciboOficial As System.Windows.Forms.MaskedTextBox
End Class
