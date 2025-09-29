<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnSeteoDocumento
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnSeteoDocumento))
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.Item = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Tabla = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Concepto = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Debe = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Haber = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Tabla1 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ClaveCuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lupa = New System.Windows.Forms.DataGridViewImageColumn
        Me.NombreCentro1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NombreCuenta1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NombreSubCuenta1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Item, Me.Tabla, Me.Concepto, Me.Debe, Me.Haber, Me.Tabla1, Me.ClaveCuenta, Me.Lupa, Me.NombreCentro1, Me.NombreCuenta1, Me.NombreSubCuenta1})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Location = New System.Drawing.Point(5, 60)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle7
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.Black
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1165, 374)
        Me.Grid.TabIndex = 240
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(1155, 20)
        Me.Label1.TabIndex = 241
        Me.Label1.Text = "Tipo Documento"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Location = New System.Drawing.Point(918, 441)
        Me.ButtonEliminar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonEliminar.TabIndex = 243
        Me.ButtonEliminar.Text = "Borrar Linea"
        Me.ButtonEliminar.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1051, 441)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonAceptar.TabIndex = 244
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = "."
        Me.DataGridViewImageColumn1.Image = CType(resources.GetObject("DataGridViewImageColumn1.Image"), System.Drawing.Image)
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.Width = 21
        '
        'Item
        '
        Me.Item.DataPropertyName = "Item"
        Me.Item.HeaderText = "Item"
        Me.Item.Name = "Item"
        Me.Item.Visible = False
        Me.Item.Width = 52
        '
        'Tabla
        '
        Me.Tabla.DataPropertyName = "Tabla"
        Me.Tabla.HeaderText = "Tabla"
        Me.Tabla.Name = "Tabla"
        Me.Tabla.Visible = False
        Me.Tabla.Width = 59
        '
        'Concepto
        '
        Me.Concepto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto.DataPropertyName = "Concepto"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Concepto.DefaultCellStyle = DataGridViewCellStyle2
        Me.Concepto.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Concepto.HeaderText = "Concepto"
        Me.Concepto.MinimumWidth = 330
        Me.Concepto.Name = "Concepto"
        Me.Concepto.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Concepto.Width = 330
        '
        'Debe
        '
        Me.Debe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Debe.DataPropertyName = "Debe"
        Me.Debe.HeaderText = "Debe"
        Me.Debe.MinimumWidth = 45
        Me.Debe.Name = "Debe"
        Me.Debe.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Debe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Debe.Width = 45
        '
        'Haber
        '
        Me.Haber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Haber.DataPropertyName = "Haber"
        Me.Haber.HeaderText = "Haber"
        Me.Haber.MinimumWidth = 45
        Me.Haber.Name = "Haber"
        Me.Haber.Width = 45
        '
        'Tabla1
        '
        Me.Tabla1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Tabla1.DataPropertyName = "Tabla"
        Me.Tabla1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Tabla1.HeaderText = "Tabla Origen de la Cuenta"
        Me.Tabla1.MinimumWidth = 200
        Me.Tabla1.Name = "Tabla1"
        Me.Tabla1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Tabla1.Width = 200
        '
        'ClaveCuenta
        '
        Me.ClaveCuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ClaveCuenta.DataPropertyName = "ClaveCuenta"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.ClaveCuenta.DefaultCellStyle = DataGridViewCellStyle3
        Me.ClaveCuenta.HeaderText = "Cuenta "
        Me.ClaveCuenta.MinimumWidth = 100
        Me.ClaveCuenta.Name = "ClaveCuenta"
        '
        'Lupa
        '
        Me.Lupa.HeaderText = "."
        Me.Lupa.Image = CType(resources.GetObject("Lupa.Image"), System.Drawing.Image)
        Me.Lupa.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Lupa.Name = "Lupa"
        Me.Lupa.Width = 21
        '
        'NombreCentro1
        '
        Me.NombreCentro1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NombreCentro1.DataPropertyName = "NombreCentro"
        Me.NombreCentro1.HeaderText = "Centro de Costo"
        Me.NombreCentro1.MinimumWidth = 120
        Me.NombreCentro1.Name = "NombreCentro1"
        Me.NombreCentro1.ReadOnly = True
        Me.NombreCentro1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NombreCentro1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.NombreCentro1.Width = 120
        '
        'NombreCuenta1
        '
        Me.NombreCuenta1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NombreCuenta1.DataPropertyName = "NombreCuenta"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.NombreCuenta1.DefaultCellStyle = DataGridViewCellStyle4
        Me.NombreCuenta1.HeaderText = "Cuenta"
        Me.NombreCuenta1.MinimumWidth = 120
        Me.NombreCuenta1.Name = "NombreCuenta1"
        Me.NombreCuenta1.ReadOnly = True
        Me.NombreCuenta1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NombreCuenta1.Width = 120
        '
        'NombreSubCuenta1
        '
        Me.NombreSubCuenta1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NombreSubCuenta1.DataPropertyName = "NombreSubCuenta"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.NombreSubCuenta1.DefaultCellStyle = DataGridViewCellStyle5
        Me.NombreSubCuenta1.HeaderText = "Sub-Cuenta"
        Me.NombreSubCuenta1.MinimumWidth = 120
        Me.NombreSubCuenta1.Name = "NombreSubCuenta1"
        Me.NombreSubCuenta1.ReadOnly = True
        Me.NombreSubCuenta1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.NombreSubCuenta1.Width = 120
        '
        'UnSeteoDocumento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(1182, 516)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Grid)
        Me.Name = "UnSeteoDocumento"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Seteo de Documentos del Sistema"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Item As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Tabla As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Concepto As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Debe As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Haber As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Tabla1 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ClaveCuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lupa As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents NombreCentro1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NombreCuenta1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NombreSubCuenta1 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
