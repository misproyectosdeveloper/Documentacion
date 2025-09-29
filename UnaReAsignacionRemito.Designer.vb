<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaReAsignacionRemito
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaReAsignacionRemito))
        Me.Grid = New System.Windows.Forms.DataGridView()
        Me.CodigoCliente = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Indice = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Deposito = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.Devueltas = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PictureCandado = New System.Windows.Forms.PictureBox()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LabelCliente = New System.Windows.Forms.Label()
        Me.LabelDeposito = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextRemito = New System.Windows.Forms.TextBox()
        Me.ButtonAceptar = New System.Windows.Forms.Button()
        Me.ButtonDevolverTodos = New System.Windows.Forms.Button()
        Me.ButtonAsignacionAutomatica = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ButtonAsignarIngreso = New System.Windows.Forms.Button()
        Me.TextIngreso = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ButtonAsignacionManual = New System.Windows.Forms.Button()
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
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CodigoCliente, Me.Indice, Me.Deposito, Me.LoteYSecuencia, Me.Articulo, Me.Devueltas, Me.Cantidad})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Location = New System.Drawing.Point(43, 122)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 51
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(695, 479)
        Me.Grid.TabIndex = 101
        '
        'CodigoCliente
        '
        Me.CodigoCliente.HeaderText = "CodigoCliente"
        Me.CodigoCliente.MinimumWidth = 6
        Me.CodigoCliente.Name = "CodigoCliente"
        Me.CodigoCliente.ReadOnly = True
        Me.CodigoCliente.Visible = False
        Me.CodigoCliente.Width = 97
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
        'Deposito
        '
        Me.Deposito.DataPropertyName = "Deposito"
        Me.Deposito.HeaderText = "Deposito"
        Me.Deposito.MinimumWidth = 6
        Me.Deposito.Name = "Deposito"
        Me.Deposito.Visible = False
        Me.Deposito.Width = 74
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle2
        Me.LoteYSecuencia.HeaderText = "Lotes"
        Me.LoteYSecuencia.MinimumWidth = 70
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
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
        Me.Articulo.MinimumWidth = 370
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.Width = 370
        '
        'Devueltas
        '
        Me.Devueltas.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Devueltas.DataPropertyName = "Devueltas"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Devueltas.DefaultCellStyle = DataGridViewCellStyle4
        Me.Devueltas.HeaderText = "Devueltas"
        Me.Devueltas.MinimumWidth = 90
        Me.Devueltas.Name = "Devueltas"
        Me.Devueltas.ReadOnly = True
        Me.Devueltas.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Devueltas.Width = 90
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle5
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 90
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cantidad.Width = 90
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(45, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 131
        Me.Label8.Text = "Cliente"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(417, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 129
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(684, 11)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(48, 42)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 132
        Me.PictureCandado.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'LabelCliente
        '
        Me.LabelCliente.BackColor = System.Drawing.Color.White
        Me.LabelCliente.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCliente.Location = New System.Drawing.Point(102, 27)
        Me.LabelCliente.Name = "LabelCliente"
        Me.LabelCliente.Size = New System.Drawing.Size(286, 19)
        Me.LabelCliente.TabIndex = 133
        Me.LabelCliente.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelDeposito
        '
        Me.LabelDeposito.BackColor = System.Drawing.Color.White
        Me.LabelDeposito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDeposito.Location = New System.Drawing.Point(487, 8)
        Me.LabelDeposito.Name = "LabelDeposito"
        Me.LabelDeposito.Size = New System.Drawing.Size(166, 19)
        Me.LabelDeposito.TabIndex = 134
        Me.LabelDeposito.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(46, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 13)
        Me.Label9.TabIndex = 153
        Me.Label9.Text = "Remito"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextRemito
        '
        Me.TextRemito.BackColor = System.Drawing.Color.White
        Me.TextRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRemito.Location = New System.Drawing.Point(103, 4)
        Me.TextRemito.MaxLength = 12
        Me.TextRemito.Name = "TextRemito"
        Me.TextRemito.ReadOnly = True
        Me.TextRemito.Size = New System.Drawing.Size(116, 20)
        Me.TextRemito.TabIndex = 152
        Me.TextRemito.TabStop = False
        Me.TextRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(587, 642)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 154
        Me.ButtonAceptar.Text = "Grabar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonDevolverTodos
        '
        Me.ButtonDevolverTodos.BackColor = System.Drawing.Color.Gold
        Me.ButtonDevolverTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDevolverTodos.Location = New System.Drawing.Point(44, 88)
        Me.ButtonDevolverTodos.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDevolverTodos.Name = "ButtonDevolverTodos"
        Me.ButtonDevolverTodos.Size = New System.Drawing.Size(219, 20)
        Me.ButtonDevolverTodos.TabIndex = 155
        Me.ButtonDevolverTodos.Text = "Devolver todos los Lotes al Stock"
        Me.ButtonDevolverTodos.UseVisualStyleBackColor = False
        '
        'ButtonAsignacionAutomatica
        '
        Me.ButtonAsignacionAutomatica.BackColor = System.Drawing.Color.Thistle
        Me.ButtonAsignacionAutomatica.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsignacionAutomatica.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsignacionAutomatica.Location = New System.Drawing.Point(282, 89)
        Me.ButtonAsignacionAutomatica.Name = "ButtonAsignacionAutomatica"
        Me.ButtonAsignacionAutomatica.Size = New System.Drawing.Size(139, 27)
        Me.ButtonAsignacionAutomatica.TabIndex = 156
        Me.ButtonAsignacionAutomatica.Text = "Selección Auto."
        Me.ButtonAsignacionAutomatica.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(430, 97)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(326, 13)
        Me.Label1.TabIndex = 157
        Me.Label1.Text = "Asignación de Lotes Automático  (Según fecha ingreso)."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(41, 612)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(330, 13)
        Me.Label2.TabIndex = 159
        Me.Label2.Text = "Asignación de Lotes Solo con el Ingreso del proveedor   "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAsignarIngreso
        '
        Me.ButtonAsignarIngreso.BackColor = System.Drawing.Color.Thistle
        Me.ButtonAsignarIngreso.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsignarIngreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsignarIngreso.Location = New System.Drawing.Point(495, 606)
        Me.ButtonAsignarIngreso.Name = "ButtonAsignarIngreso"
        Me.ButtonAsignarIngreso.Size = New System.Drawing.Size(66, 27)
        Me.ButtonAsignarIngreso.TabIndex = 158
        Me.ButtonAsignarIngreso.Text = "Asignar"
        Me.ButtonAsignarIngreso.UseVisualStyleBackColor = False
        '
        'TextIngreso
        '
        Me.TextIngreso.BackColor = System.Drawing.Color.White
        Me.TextIngreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextIngreso.ForeColor = System.Drawing.Color.Blue
        Me.TextIngreso.Location = New System.Drawing.Point(367, 608)
        Me.TextIngreso.Name = "TextIngreso"
        Me.TextIngreso.ReadOnly = True
        Me.TextIngreso.Size = New System.Drawing.Size(109, 22)
        Me.TextIngreso.TabIndex = 160
        Me.TextIngreso.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(428, 63)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(369, 13)
        Me.Label4.TabIndex = 168
        Me.Label4.Text = "Asignación de Lotes Manual  (Carga la grilla para su seleccion)."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAsignacionManual
        '
        Me.ButtonAsignacionManual.BackColor = System.Drawing.Color.Thistle
        Me.ButtonAsignacionManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsignacionManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsignacionManual.Location = New System.Drawing.Point(282, 55)
        Me.ButtonAsignacionManual.Name = "ButtonAsignacionManual"
        Me.ButtonAsignacionManual.Size = New System.Drawing.Size(140, 27)
        Me.ButtonAsignacionManual.TabIndex = 167
        Me.ButtonAsignacionManual.Text = "Selección Manual"
        Me.ButtonAsignacionManual.UseVisualStyleBackColor = False
        '
        'UnaReAsignacionRemito
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Beige
        Me.ClientSize = New System.Drawing.Size(789, 697)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ButtonAsignacionManual)
        Me.Controls.Add(Me.TextIngreso)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonAsignarIngreso)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonAsignacionAutomatica)
        Me.Controls.Add(Me.ButtonDevolverTodos)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TextRemito)
        Me.Controls.Add(Me.LabelDeposito)
        Me.Controls.Add(Me.LabelCliente)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Grid)
        Me.Name = "UnaReAsignacionRemito"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asignar Lotes"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents LabelCliente As System.Windows.Forms.Label
    Friend WithEvents LabelDeposito As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextRemito As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonDevolverTodos As System.Windows.Forms.Button
    Friend WithEvents ButtonAsignacionAutomatica As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonAsignarIngreso As System.Windows.Forms.Button
    Friend WithEvents TextIngreso As System.Windows.Forms.TextBox
    Friend WithEvents CodigoCliente As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Indice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Deposito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Devueltas As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label4 As Label
    Friend WithEvents ButtonAsignacionManual As Button
End Class
