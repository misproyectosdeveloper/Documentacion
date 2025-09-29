<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaRecepcion
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaRecepcion))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.CantidadOrden = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Devueltas = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextFechaRemito = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueRemito = New System.Windows.Forms.PictureBox
        Me.MaskedFactura = New System.Windows.Forms.MaskedTextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.MaskedOrden = New System.Windows.Forms.MaskedTextBox
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.MaskedIngreso = New System.Windows.Forms.MaskedTextBox
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel2.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaqueRemito, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Location = New System.Drawing.Point(46, 133)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(721, 477)
        Me.Panel2.TabIndex = 173
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.LightYellow
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Sel, Me.Articulo, Me.CantidadOrden, Me.Saldo, Me.Cantidad, Me.Devueltas})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle15
        Me.Grid.Location = New System.Drawing.Point(20, 2)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(681, 458)
        Me.Grid.TabIndex = 10
        '
        'Sel
        '
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 20
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 20
        '
        'Articulo
        '
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle10
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 250
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Articulo.Width = 250
        '
        'CantidadOrden
        '
        Me.CantidadOrden.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CantidadOrden.DataPropertyName = "CantidadOrden"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CantidadOrden.DefaultCellStyle = DataGridViewCellStyle11
        Me.CantidadOrden.HeaderText = "Cant. Orden"
        Me.CantidadOrden.MaxInputLength = 8
        Me.CantidadOrden.MinimumWidth = 90
        Me.CantidadOrden.Name = "CantidadOrden"
        Me.CantidadOrden.ReadOnly = True
        Me.CantidadOrden.Width = 90
        '
        'Saldo
        '
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle12
        Me.Saldo.HeaderText = "Saldo Orden"
        Me.Saldo.MinimumWidth = 80
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Width = 91
        '
        'Cantidad
        '
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle13
        Me.Cantidad.HeaderText = "Recepción"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Width = 84
        '
        'Devueltas
        '
        Me.Devueltas.DataPropertyName = "Devueltas"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Devueltas.DefaultCellStyle = DataGridViewCellStyle14
        Me.Devueltas.HeaderText = "Devueltas"
        Me.Devueltas.Name = "Devueltas"
        Me.Devueltas.ReadOnly = True
        Me.Devueltas.Width = 80
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextFechaRemito)
        Me.Panel1.Controls.Add(Me.PictureAlmanaqueRemito)
        Me.Panel1.Controls.Add(Me.MaskedFactura)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.MaskedOrden)
        Me.Panel1.Controls.Add(Me.MaskedRemito)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Location = New System.Drawing.Point(46, 29)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(721, 103)
        Me.Panel1.TabIndex = 172
        '
        'TextFechaRemito
        '
        Me.TextFechaRemito.BackColor = System.Drawing.Color.White
        Me.TextFechaRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaRemito.Location = New System.Drawing.Point(308, 28)
        Me.TextFechaRemito.MaxLength = 10
        Me.TextFechaRemito.Name = "TextFechaRemito"
        Me.TextFechaRemito.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaRemito.TabIndex = 1008
        Me.TextFechaRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueRemito
        '
        Me.PictureAlmanaqueRemito.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueRemito.Location = New System.Drawing.Point(394, 27)
        Me.PictureAlmanaqueRemito.Name = "PictureAlmanaqueRemito"
        Me.PictureAlmanaqueRemito.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueRemito.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueRemito.TabIndex = 1009
        Me.PictureAlmanaqueRemito.TabStop = False
        '
        'MaskedFactura
        '
        Me.MaskedFactura.BackColor = System.Drawing.Color.White
        Me.MaskedFactura.Enabled = False
        Me.MaskedFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedFactura.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedFactura.Location = New System.Drawing.Point(275, 53)
        Me.MaskedFactura.Mask = "00000000"
        Me.MaskedFactura.Name = "MaskedFactura"
        Me.MaskedFactura.ReadOnly = True
        Me.MaskedFactura.Size = New System.Drawing.Size(117, 21)
        Me.MaskedFactura.TabIndex = 202
        Me.MaskedFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedFactura.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(220, 56)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(43, 13)
        Me.Label10.TabIndex = 201
        Me.Label10.Text = "Factura"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(217, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 13)
        Me.Label3.TabIndex = 198
        Me.Label3.Text = "Fecha Remito"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedOrden
        '
        Me.MaskedOrden.BackColor = System.Drawing.Color.White
        Me.MaskedOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedOrden.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedOrden.Location = New System.Drawing.Point(89, 51)
        Me.MaskedOrden.Mask = "00000000"
        Me.MaskedOrden.Name = "MaskedOrden"
        Me.MaskedOrden.ReadOnly = True
        Me.MaskedOrden.Size = New System.Drawing.Size(97, 21)
        Me.MaskedOrden.TabIndex = 197
        Me.MaskedOrden.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedOrden.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'MaskedRemito
        '
        Me.MaskedRemito.BackColor = System.Drawing.Color.White
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(90, 26)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(112, 21)
        Me.MaskedRemito.TabIndex = 196
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 77)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 15)
        Me.Label7.TabIndex = 115
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Location = New System.Drawing.Point(90, 76)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(334, 20)
        Me.TextComentario.TabIndex = 8
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(596, 3)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(105, 20)
        Me.DateTime1.TabIndex = 4
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(560, 26)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(143, 21)
        Me.ComboDeposito.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(506, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Deposito"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(555, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(37, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Fecha"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(10, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Orden Compra"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Remito"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboProveedor
        '
        Me.ComboProveedor.BackColor = System.Drawing.Color.White
        Me.ComboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboProveedor.Enabled = False
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(89, 4)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(277, 21)
        Me.ComboProveedor.TabIndex = 0
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 9)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Proveedor"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(657, 5)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 184
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(44, 10)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(68, 13)
        Me.Label9.TabIndex = 182
        Me.Label9.Text = "Recepción"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(606, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(46, 13)
        Me.Label5.TabIndex = 180
        Me.Label5.Text = "Estado"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedIngreso
        '
        Me.MaskedIngreso.BackColor = System.Drawing.Color.White
        Me.MaskedIngreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedIngreso.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedIngreso.Location = New System.Drawing.Point(124, 6)
        Me.MaskedIngreso.Mask = "00000000"
        Me.MaskedIngreso.Name = "MaskedIngreso"
        Me.MaskedIngreso.ReadOnly = True
        Me.MaskedIngreso.Size = New System.Drawing.Size(97, 21)
        Me.MaskedIngreso.TabIndex = 198
        Me.MaskedIngreso.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedIngreso.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(230, 623)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(136, 29)
        Me.ButtonAnula.TabIndex = 174
        Me.ButtonAnula.Text = "Anular Recepción"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(629, 622)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(137, 29)
        Me.ButtonAceptar.TabIndex = 171
        Me.ButtonAceptar.Text = "Graba Recepción"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.Location = New System.Drawing.Point(46, 622)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(137, 29)
        Me.ButtonNuevo.TabIndex = 170
        Me.ButtonNuevo.Text = "Nuevo Recepción"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(428, 623)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 29)
        Me.ButtonAsientoContable.TabIndex = 300
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'UnaRecepcion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(821, 664)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.MaskedIngreso)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Name = "UnaRecepcion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Recepción de Insumos"
        Me.Panel2.ResumeLayout(False)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaqueRemito, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents MaskedOrden As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents MaskedIngreso As System.Windows.Forms.MaskedTextBox
    Friend WithEvents MaskedFactura As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents TextFechaRemito As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueRemito As System.Windows.Forms.PictureBox
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CantidadOrden As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Devueltas As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
