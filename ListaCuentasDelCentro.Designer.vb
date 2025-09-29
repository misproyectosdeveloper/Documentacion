<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaCuentasDelCentro
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaCuentasDelCentro))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ClaveCuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.SubCuenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.LabelCentro = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureLupa = New System.Windows.Forms.PictureBox
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.ButtonEliminarTodasLinea = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureLupa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(536, 643)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 20)
        Me.ButtonAceptar.TabIndex = 252
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.Location = New System.Drawing.Point(159, 642)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 21)
        Me.ButtonUltimo.TabIndex = 247
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(404, 643)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(114, 20)
        Me.ButtonEliminarLinea.TabIndex = 251
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPosterior.Location = New System.Drawing.Point(128, 642)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 21)
        Me.ButtonPosterior.TabIndex = 250
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior.Location = New System.Drawing.Point(98, 642)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 21)
        Me.ButtonAnterior.TabIndex = 249
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.Location = New System.Drawing.Point(65, 642)
        Me.ButtonPrimero.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 21)
        Me.ButtonPrimero.TabIndex = 248
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.LightGray
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.ClaveCuenta, Me.Cuenta, Me.SubCuenta})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.DarkGray
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle10
        Me.Grid.Location = New System.Drawing.Point(65, 45)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle11
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(585, 591)
        Me.Grid.TabIndex = 246
        '
        'ClaveCuenta
        '
        Me.ClaveCuenta.DataPropertyName = "ClaveCuenta"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ClaveCuenta.DefaultCellStyle = DataGridViewCellStyle8
        Me.ClaveCuenta.HeaderText = ""
        Me.ClaveCuenta.MaxInputLength = 3
        Me.ClaveCuenta.MinimumWidth = 110
        Me.ClaveCuenta.Name = "ClaveCuenta"
        Me.ClaveCuenta.ReadOnly = True
        Me.ClaveCuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.ClaveCuenta.Width = 110
        '
        'Cuenta
        '
        Me.Cuenta.DataPropertyName = "Cuenta"
        Me.Cuenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MinimumWidth = 200
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        Me.Cuenta.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Cuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Cuenta.Width = 200
        '
        'SubCuenta
        '
        Me.SubCuenta.DataPropertyName = "SubCuenta"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.SubCuenta.DefaultCellStyle = DataGridViewCellStyle9
        Me.SubCuenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.SubCuenta.HeaderText = "Sub-Cuenta"
        Me.SubCuenta.MinimumWidth = 200
        Me.SubCuenta.Name = "SubCuenta"
        Me.SubCuenta.ReadOnly = True
        Me.SubCuenta.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.SubCuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.SubCuenta.Width = 200
        '
        'LabelCentro
        '
        Me.LabelCentro.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCentro.Location = New System.Drawing.Point(132, 12)
        Me.LabelCentro.Name = "LabelCentro"
        Me.LabelCentro.Size = New System.Drawing.Size(502, 26)
        Me.LabelCentro.TabIndex = 254
        Me.LabelCentro.Text = "Label1"
        Me.LabelCentro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(662, 662)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(119, 13)
        Me.Label7.TabIndex = 255
        Me.Label7.Text = "Falta desarrollar eliminar"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.PictureLupa)
        Me.Panel1.Location = New System.Drawing.Point(669, 220)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(112, 126)
        Me.Panel1.TabIndex = 270
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 271
        Me.Label1.Text = "Ingresar Cuenta"
        '
        'PictureLupa
        '
        Me.PictureLupa.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupa.InitialImage = Nothing
        Me.PictureLupa.Location = New System.Drawing.Point(33, 46)
        Me.PictureLupa.Name = "PictureLupa"
        Me.PictureLupa.Size = New System.Drawing.Size(48, 49)
        Me.PictureLupa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupa.TabIndex = 270
        Me.PictureLupa.TabStop = False
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = "Cuentas"
        Me.DataGridViewImageColumn1.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 52
        '
        'ButtonEliminarTodasLinea
        '
        Me.ButtonEliminarTodasLinea.BackColor = System.Drawing.Color.Violet
        Me.ButtonEliminarTodasLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodasLinea.Location = New System.Drawing.Point(227, 643)
        Me.ButtonEliminarTodasLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodasLinea.Name = "ButtonEliminarTodasLinea"
        Me.ButtonEliminarTodasLinea.Size = New System.Drawing.Size(147, 20)
        Me.ButtonEliminarTodasLinea.TabIndex = 271
        Me.ButtonEliminarTodasLinea.Text = "Borrar Todas las Linea"
        Me.ButtonEliminarTodasLinea.UseVisualStyleBackColor = False
        '
        'ListaCuentasDelCentro
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(796, 676)
        Me.Controls.Add(Me.ButtonEliminarTodasLinea)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.LabelCentro)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonEliminarLinea)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.Grid)
        Me.Name = "ListaCuentasDelCentro"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cuentas del Centro de Costo"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureLupa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LabelCentro As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureLupa As System.Windows.Forms.PictureBox
    Friend WithEvents ClaveCuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents SubCuenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ButtonEliminarTodasLinea As System.Windows.Forms.Button
End Class
