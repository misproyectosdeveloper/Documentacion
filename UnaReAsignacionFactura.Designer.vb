<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaReAsignacionFactura
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaReAsignacionFactura))
        Me.Grid = New System.Windows.Forms.DataGridView()
        Me.Indice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CodigoCliente = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Devueltas = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrecioBlanco = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PrecioNegro = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NetoBlanco = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.NetoNegro = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MontoBlanco = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MontoNegro = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Iva = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Neto = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MontoIva = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.TotalArticulo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextFactura = New System.Windows.Forms.TextBox()
        Me.LabelDeposito = New System.Windows.Forms.Label()
        Me.LabelCliente = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureCandado = New System.Windows.Forms.PictureBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ButtonDevolverTodos = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ButtonAsignacionAutomatica = New System.Windows.Forms.Button()
        Me.ButtonAsignacionManual = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
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
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Indice, Me.CodigoCliente, Me.Devueltas, Me.PrecioBlanco, Me.PrecioNegro, Me.NetoBlanco, Me.NetoNegro, Me.MontoBlanco, Me.MontoNegro, Me.Iva, Me.LoteYSecuencia, Me.Articulo, Me.Cantidad, Me.Precio, Me.Neto, Me.MontoIva, Me.TotalArticulo})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.Location = New System.Drawing.Point(29, 110)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle10
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(847, 458)
        Me.Grid.TabIndex = 16
        '
        'Indice
        '
        Me.Indice.DataPropertyName = "indice"
        Me.Indice.HeaderText = "Indice"
        Me.Indice.MinimumWidth = 6
        Me.Indice.Name = "Indice"
        Me.Indice.Visible = False
        Me.Indice.Width = 61
        '
        'CodigoCliente
        '
        Me.CodigoCliente.HeaderText = "CodigoCliente"
        Me.CodigoCliente.MinimumWidth = 6
        Me.CodigoCliente.Name = "CodigoCliente"
        Me.CodigoCliente.Visible = False
        Me.CodigoCliente.Width = 97
        '
        'Devueltas
        '
        Me.Devueltas.DataPropertyName = "Devueltas"
        Me.Devueltas.HeaderText = "Devueltas"
        Me.Devueltas.MinimumWidth = 6
        Me.Devueltas.Name = "Devueltas"
        Me.Devueltas.Visible = False
        Me.Devueltas.Width = 80
        '
        'PrecioBlanco
        '
        Me.PrecioBlanco.HeaderText = "PrecioBlanco"
        Me.PrecioBlanco.MinimumWidth = 6
        Me.PrecioBlanco.Name = "PrecioBlanco"
        Me.PrecioBlanco.Visible = False
        Me.PrecioBlanco.Width = 95
        '
        'PrecioNegro
        '
        Me.PrecioNegro.HeaderText = "PrecioNegro"
        Me.PrecioNegro.MinimumWidth = 6
        Me.PrecioNegro.Name = "PrecioNegro"
        Me.PrecioNegro.Visible = False
        Me.PrecioNegro.Width = 91
        '
        'NetoBlanco
        '
        Me.NetoBlanco.HeaderText = "NetoBlanco"
        Me.NetoBlanco.MinimumWidth = 6
        Me.NetoBlanco.Name = "NetoBlanco"
        Me.NetoBlanco.Visible = False
        Me.NetoBlanco.Width = 88
        '
        'NetoNegro
        '
        Me.NetoNegro.HeaderText = "NetoNegro"
        Me.NetoNegro.MinimumWidth = 6
        Me.NetoNegro.Name = "NetoNegro"
        Me.NetoNegro.Visible = False
        Me.NetoNegro.Width = 84
        '
        'MontoBlanco
        '
        Me.MontoBlanco.HeaderText = "MontoBlanco"
        Me.MontoBlanco.MinimumWidth = 6
        Me.MontoBlanco.Name = "MontoBlanco"
        Me.MontoBlanco.Visible = False
        Me.MontoBlanco.Width = 95
        '
        'MontoNegro
        '
        Me.MontoNegro.HeaderText = "MontoNegro"
        Me.MontoNegro.MinimumWidth = 6
        Me.MontoNegro.Name = "MontoNegro"
        Me.MontoNegro.Visible = False
        Me.MontoNegro.Width = 91
        '
        'Iva
        '
        Me.Iva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Iva.DataPropertyName = "Iva"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Iva.DefaultCellStyle = DataGridViewCellStyle2
        Me.Iva.HeaderText = "% Iva"
        Me.Iva.MinimumWidth = 60
        Me.Iva.Name = "Iva"
        Me.Iva.ReadOnly = True
        Me.Iva.Visible = False
        Me.Iva.Width = 60
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 70
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LoteYSecuencia.Width = 70
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle3
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 300
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.Width = 300
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle4
        Me.Cantidad.HeaderText = "Cantidad xUni"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cantidad.Width = 80
        '
        'Precio
        '
        Me.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Gainsboro
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle5
        Me.Precio.HeaderText = "Precio xUni "
        Me.Precio.MaxInputLength = 8
        Me.Precio.MinimumWidth = 84
        Me.Precio.Name = "Precio"
        Me.Precio.ReadOnly = True
        Me.Precio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Precio.Width = 84
        '
        'Neto
        '
        Me.Neto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro
        Me.Neto.DefaultCellStyle = DataGridViewCellStyle6
        Me.Neto.HeaderText = "Neto"
        Me.Neto.MinimumWidth = 90
        Me.Neto.Name = "Neto"
        Me.Neto.ReadOnly = True
        Me.Neto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Neto.Width = 90
        '
        'MontoIva
        '
        Me.MontoIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.Gainsboro
        Me.MontoIva.DefaultCellStyle = DataGridViewCellStyle7
        Me.MontoIva.HeaderText = "Iva"
        Me.MontoIva.MinimumWidth = 70
        Me.MontoIva.Name = "MontoIva"
        Me.MontoIva.ReadOnly = True
        Me.MontoIva.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.MontoIva.Width = 70
        '
        'TotalArticulo
        '
        Me.TotalArticulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TotalArticulo.DataPropertyName = "TotalArticulo"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.Gainsboro
        Me.TotalArticulo.DefaultCellStyle = DataGridViewCellStyle8
        Me.TotalArticulo.HeaderText = "Total"
        Me.TotalArticulo.MaxInputLength = 8
        Me.TotalArticulo.MinimumWidth = 90
        Me.TotalArticulo.Name = "TotalArticulo"
        Me.TotalArticulo.ReadOnly = True
        Me.TotalArticulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.TotalArticulo.Width = 90
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(31, 7)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(50, 13)
        Me.Label9.TabIndex = 160
        Me.Label9.Text = "Factura"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextFactura
        '
        Me.TextFactura.BackColor = System.Drawing.Color.White
        Me.TextFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFactura.Location = New System.Drawing.Point(88, 4)
        Me.TextFactura.MaxLength = 12
        Me.TextFactura.Name = "TextFactura"
        Me.TextFactura.ReadOnly = True
        Me.TextFactura.Size = New System.Drawing.Size(116, 20)
        Me.TextFactura.TabIndex = 159
        Me.TextFactura.TabStop = False
        Me.TextFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelDeposito
        '
        Me.LabelDeposito.BackColor = System.Drawing.Color.White
        Me.LabelDeposito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDeposito.Location = New System.Drawing.Point(600, 6)
        Me.LabelDeposito.Name = "LabelDeposito"
        Me.LabelDeposito.Size = New System.Drawing.Size(166, 19)
        Me.LabelDeposito.TabIndex = 158
        Me.LabelDeposito.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelCliente
        '
        Me.LabelCliente.BackColor = System.Drawing.Color.White
        Me.LabelCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCliente.Location = New System.Drawing.Point(87, 26)
        Me.LabelCliente.Name = "LabelCliente"
        Me.LabelCliente.Size = New System.Drawing.Size(286, 19)
        Me.LabelCliente.TabIndex = 157
        Me.LabelCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(30, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 155
        Me.Label8.Text = "Cliente"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(530, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 154
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(820, 10)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(48, 42)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 156
        Me.PictureCandado.TabStop = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(725, 577)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 161
        Me.ButtonAceptar.Text = "Grabar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonDevolverTodos
        '
        Me.ButtonDevolverTodos.BackColor = System.Drawing.Color.Gold
        Me.ButtonDevolverTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDevolverTodos.Location = New System.Drawing.Point(29, 63)
        Me.ButtonDevolverTodos.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDevolverTodos.Name = "ButtonDevolverTodos"
        Me.ButtonDevolverTodos.Size = New System.Drawing.Size(219, 20)
        Me.ButtonDevolverTodos.TabIndex = 162
        Me.ButtonDevolverTodos.Text = "Devolver todos los Lotes al Stock"
        Me.ButtonDevolverTodos.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(504, 84)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(326, 13)
        Me.Label1.TabIndex = 164
        Me.Label1.Text = "Asignación de Lotes Automático  (Según fecha ingreso)."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAsignacionAutomatica
        '
        Me.ButtonAsignacionAutomatica.BackColor = System.Drawing.Color.Thistle
        Me.ButtonAsignacionAutomatica.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsignacionAutomatica.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsignacionAutomatica.Location = New System.Drawing.Point(321, 77)
        Me.ButtonAsignacionAutomatica.Name = "ButtonAsignacionAutomatica"
        Me.ButtonAsignacionAutomatica.Size = New System.Drawing.Size(177, 27)
        Me.ButtonAsignacionAutomatica.TabIndex = 163
        Me.ButtonAsignacionAutomatica.Text = "Asignación Automatica"
        Me.ButtonAsignacionAutomatica.UseVisualStyleBackColor = False
        '
        'ButtonAsignacionManual
        '
        Me.ButtonAsignacionManual.BackColor = System.Drawing.Color.Thistle
        Me.ButtonAsignacionManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsignacionManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsignacionManual.Location = New System.Drawing.Point(321, 47)
        Me.ButtonAsignacionManual.Name = "ButtonAsignacionManual"
        Me.ButtonAsignacionManual.Size = New System.Drawing.Size(177, 27)
        Me.ButtonAsignacionManual.TabIndex = 165
        Me.ButtonAsignacionManual.Text = "Asignación Manual"
        Me.ButtonAsignacionManual.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(504, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(369, 13)
        Me.Label2.TabIndex = 166
        Me.Label2.Text = "Asignación de Lotes Manual  (Carga la grilla para su seleccion)."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UnaReAsignacionFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Beige
        Me.ClientSize = New System.Drawing.Size(902, 609)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonAsignacionManual)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonAsignacionAutomatica)
        Me.Controls.Add(Me.ButtonDevolverTodos)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TextFactura)
        Me.Controls.Add(Me.LabelDeposito)
        Me.Controls.Add(Me.LabelCliente)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Grid)
        Me.Name = "UnaReAsignacionFactura"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asignar Lotes"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextFactura As System.Windows.Forms.TextBox
    Friend WithEvents LabelDeposito As System.Windows.Forms.Label
    Friend WithEvents LabelCliente As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonDevolverTodos As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAsignacionAutomatica As System.Windows.Forms.Button
    Friend WithEvents Indice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodigoCliente As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Devueltas As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioBlanco As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioNegro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NetoBlanco As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NetoNegro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MontoBlanco As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MontoNegro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Iva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Neto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MontoIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalArticulo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonAsignacionManual As Button
    Friend WithEvents Label2 As Label
End Class
