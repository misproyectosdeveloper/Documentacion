<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnReemplazoChequeEnContable
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnReemplazoChequeEnContable))
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextTotalOrden = New System.Windows.Forms.TextBox
        Me.TextCliente = New System.Windows.Forms.TextBox
        Me.LabelInterno = New System.Windows.Forms.Label
        Me.MaskedNota = New System.Windows.Forms.MaskedTextBox
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label16 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ClaveInterna = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.TieneLupa = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Concepto = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Operacion1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cambio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Banco = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Serie = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Numero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EmisorCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaComprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ClaveCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ClaveChequeVisual = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LabelCaja = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.Panel4.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Thistle
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.TextTotalOrden)
        Me.Panel4.Controls.Add(Me.TextCliente)
        Me.Panel4.Controls.Add(Me.LabelInterno)
        Me.Panel4.Controls.Add(Me.MaskedNota)
        Me.Panel4.Controls.Add(Me.PictureCandado)
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.DateTime1)
        Me.Panel4.Controls.Add(Me.Label16)
        Me.Panel4.Location = New System.Drawing.Point(16, 32)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1054, 64)
        Me.Panel4.TabIndex = 305
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(449, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 247
        Me.Label3.Text = "Total Orden Pago"
        '
        'TextTotalOrden
        '
        Me.TextTotalOrden.BackColor = System.Drawing.Color.White
        Me.TextTotalOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalOrden.Location = New System.Drawing.Point(576, 31)
        Me.TextTotalOrden.MaxLength = 20
        Me.TextTotalOrden.Name = "TextTotalOrden"
        Me.TextTotalOrden.ReadOnly = True
        Me.TextTotalOrden.Size = New System.Drawing.Size(111, 20)
        Me.TextTotalOrden.TabIndex = 246
        Me.TextTotalOrden.TabStop = False
        Me.TextTotalOrden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextCliente
        '
        Me.TextCliente.BackColor = System.Drawing.Color.White
        Me.TextCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCliente.Location = New System.Drawing.Point(107, 31)
        Me.TextCliente.MaxLength = 30
        Me.TextCliente.Name = "TextCliente"
        Me.TextCliente.Size = New System.Drawing.Size(292, 20)
        Me.TextCliente.TabIndex = 245
        Me.TextCliente.TabStop = False
        '
        'LabelInterno
        '
        Me.LabelInterno.AutoSize = True
        Me.LabelInterno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelInterno.Location = New System.Drawing.Point(6, 8)
        Me.LabelInterno.Name = "LabelInterno"
        Me.LabelInterno.Size = New System.Drawing.Size(81, 13)
        Me.LabelInterno.TabIndex = 244
        Me.LabelInterno.Text = "Comprobante"
        Me.LabelInterno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedNota
        '
        Me.MaskedNota.BackColor = System.Drawing.Color.White
        Me.MaskedNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedNota.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedNota.Location = New System.Drawing.Point(107, 3)
        Me.MaskedNota.Mask = "0000-00000000"
        Me.MaskedNota.Name = "MaskedNota"
        Me.MaskedNota.ReadOnly = True
        Me.MaskedNota.Size = New System.Drawing.Size(114, 21)
        Me.MaskedNota.TabIndex = 1
        Me.MaskedNota.TabStop = False
        Me.MaskedNota.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(987, 7)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 137
        Me.PictureCandado.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(788, 7)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(837, 3)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(110, 20)
        Me.DateTime1.TabIndex = 2
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(4, 36)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(46, 13)
        Me.Label16.TabIndex = 18
        Me.Label16.Text = "Cliente"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ClaveInterna, Me.Sel, Me.TieneLupa, Me.Concepto, Me.Operacion1, Me.Importe, Me.Cambio, Me.Banco, Me.Cuenta, Me.Serie, Me.Numero, Me.EmisorCheque, Me.Fecha, Me.Comprobante, Me.FechaComprobante, Me.ClaveCheque, Me.ClaveChequeVisual})
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle13
        Me.Grid.Location = New System.Drawing.Point(15, 99)
        Me.Grid.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle14.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle14.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle14
        Me.Grid.Size = New System.Drawing.Size(1055, 542)
        Me.Grid.TabIndex = 306
        '
        'ClaveInterna
        '
        Me.ClaveInterna.DataPropertyName = "ClaveInterna"
        Me.ClaveInterna.HeaderText = "ClaveInterna"
        Me.ClaveInterna.Name = "ClaveInterna"
        Me.ClaveInterna.Visible = False
        '
        'Sel
        '
        Me.Sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel.DataPropertyName = "sel"
        Me.Sel.HeaderText = "Sel"
        Me.Sel.MinimumWidth = 30
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 30
        '
        'TieneLupa
        '
        Me.TieneLupa.DataPropertyName = "TieneLupa"
        Me.TieneLupa.HeaderText = ""
        Me.TieneLupa.Name = "TieneLupa"
        Me.TieneLupa.Visible = False
        '
        'Concepto
        '
        Me.Concepto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto.DataPropertyName = "MedioPago"
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Concepto.DefaultCellStyle = DataGridViewCellStyle1
        Me.Concepto.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Concepto.HeaderText = "Concepto"
        Me.Concepto.MinimumWidth = 160
        Me.Concepto.Name = "Concepto"
        Me.Concepto.ReadOnly = True
        Me.Concepto.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Concepto.Width = 160
        '
        'Operacion1
        '
        Me.Operacion1.DataPropertyName = "Operacion"
        Me.Operacion1.HeaderText = "Operacion"
        Me.Operacion1.Name = "Operacion1"
        Me.Operacion1.Visible = False
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle2
        Me.Importe.HeaderText = "Importe "
        Me.Importe.MaxInputLength = 12
        Me.Importe.MinimumWidth = 80
        Me.Importe.Name = "Importe"
        Me.Importe.ReadOnly = True
        Me.Importe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Importe.Width = 80
        '
        'Cambio
        '
        Me.Cambio.DataPropertyName = "Cambio"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cambio.DefaultCellStyle = DataGridViewCellStyle3
        Me.Cambio.HeaderText = "Cambio"
        Me.Cambio.MaxInputLength = 7
        Me.Cambio.MinimumWidth = 50
        Me.Cambio.Name = "Cambio"
        Me.Cambio.ReadOnly = True
        Me.Cambio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cambio.Width = 50
        '
        'Banco
        '
        Me.Banco.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Banco.DataPropertyName = "Banco"
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        Me.Banco.DefaultCellStyle = DataGridViewCellStyle4
        Me.Banco.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Banco.HeaderText = "Banco"
        Me.Banco.MinimumWidth = 80
        Me.Banco.Name = "Banco"
        Me.Banco.ReadOnly = True
        Me.Banco.Width = 80
        '
        'Cuenta
        '
        Me.Cuenta.DataPropertyName = "Cuenta"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cuenta.DefaultCellStyle = DataGridViewCellStyle5
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MaxInputLength = 8
        Me.Cuenta.MinimumWidth = 90
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        Me.Cuenta.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Cuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cuenta.Width = 90
        '
        'Serie
        '
        Me.Serie.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Serie.DataPropertyName = "Serie"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Serie.DefaultCellStyle = DataGridViewCellStyle6
        Me.Serie.HeaderText = "Serie"
        Me.Serie.MaxInputLength = 1
        Me.Serie.MinimumWidth = 35
        Me.Serie.Name = "Serie"
        Me.Serie.ReadOnly = True
        Me.Serie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Serie.Width = 35
        '
        'Numero
        '
        Me.Numero.DataPropertyName = "Numero"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Numero.DefaultCellStyle = DataGridViewCellStyle7
        Me.Numero.HeaderText = "Numero"
        Me.Numero.MaxInputLength = 10
        Me.Numero.MinimumWidth = 70
        Me.Numero.Name = "Numero"
        Me.Numero.ReadOnly = True
        Me.Numero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Numero.Width = 70
        '
        'EmisorCheque
        '
        Me.EmisorCheque.DataPropertyName = "EmisorCheque"
        Me.EmisorCheque.HeaderText = "Emisor"
        Me.EmisorCheque.MaxInputLength = 30
        Me.EmisorCheque.Name = "EmisorCheque"
        Me.EmisorCheque.ReadOnly = True
        Me.EmisorCheque.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle8
        Me.Fecha.HeaderText = "Fecha Venc."
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Fecha.Width = 70
        '
        'Comprobante
        '
        Me.Comprobante.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comprobante.DataPropertyName = "Comprobante"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Comprobante.DefaultCellStyle = DataGridViewCellStyle9
        Me.Comprobante.HeaderText = "Comprobante"
        Me.Comprobante.MaxInputLength = 9
        Me.Comprobante.MinimumWidth = 80
        Me.Comprobante.Name = "Comprobante"
        Me.Comprobante.ReadOnly = True
        Me.Comprobante.Width = 80
        '
        'FechaComprobante
        '
        Me.FechaComprobante.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.FechaComprobante.DataPropertyName = "FechaComprobante"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaComprobante.DefaultCellStyle = DataGridViewCellStyle10
        Me.FechaComprobante.HeaderText = "Fecha Compr."
        Me.FechaComprobante.MinimumWidth = 80
        Me.FechaComprobante.Name = "FechaComprobante"
        Me.FechaComprobante.ReadOnly = True
        Me.FechaComprobante.Width = 80
        '
        'ClaveCheque
        '
        Me.ClaveCheque.DataPropertyName = "ClaveCheque"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Red
        Me.ClaveCheque.DefaultCellStyle = DataGridViewCellStyle11
        Me.ClaveCheque.HeaderText = "Clave Cheque"
        Me.ClaveCheque.MinimumWidth = 80
        Me.ClaveCheque.Name = "ClaveCheque"
        Me.ClaveCheque.ReadOnly = True
        Me.ClaveCheque.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ClaveCheque.Visible = False
        Me.ClaveCheque.Width = 80
        '
        'ClaveChequeVisual
        '
        Me.ClaveChequeVisual.DataPropertyName = "ClaveChequeVisual"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ClaveChequeVisual.DefaultCellStyle = DataGridViewCellStyle12
        Me.ClaveChequeVisual.HeaderText = "Clave Cheque"
        Me.ClaveChequeVisual.MinimumWidth = 80
        Me.ClaveChequeVisual.Name = "ClaveChequeVisual"
        Me.ClaveChequeVisual.ReadOnly = True
        Me.ClaveChequeVisual.Width = 80
        '
        'LabelCaja
        '
        Me.LabelCaja.AutoSize = True
        Me.LabelCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaja.Location = New System.Drawing.Point(553, 9)
        Me.LabelCaja.Name = "LabelCaja"
        Me.LabelCaja.Size = New System.Drawing.Size(47, 18)
        Me.LabelCaja.TabIndex = 312
        Me.LabelCaja.Text = "Caja "
        Me.LabelCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(502, 9)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 15)
        Me.Label13.TabIndex = 311
        Me.Label13.Text = "Caja "
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(914, 655)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(150, 29)
        Me.ButtonAceptar.TabIndex = 315
        Me.ButtonAceptar.Text = "Reemplaza Cheques "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(557, 656)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 29)
        Me.ButtonAsientoContable.TabIndex = 319
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'UnReemplazoChequeEnContable
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.NavajoWhite
        Me.ClientSize = New System.Drawing.Size(1088, 696)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.LabelCaja)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Panel4)
        Me.Name = "UnReemplazoChequeEnContable"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reemplazo Cheque En Contable"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents LabelInterno As System.Windows.Forms.Label
    Friend WithEvents MaskedNota As System.Windows.Forms.MaskedTextBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents TextCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextTotalOrden As System.Windows.Forms.TextBox
    Friend WithEvents LabelCaja As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ClaveInterna As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents TieneLupa As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Concepto As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Operacion1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cambio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Banco As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Serie As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Numero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmisorCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaComprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ClaveCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ClaveChequeVisual As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
End Class
