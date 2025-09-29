<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MuestraStockArticulo
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MuestraStockArticulo))
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OtrasAsignaciones = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.KilosXUnidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Calibre = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboArticulo
        '
        Me.ComboArticulo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboArticulo.Enabled = False
        Me.ComboArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(161, 33)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(235, 21)
        Me.ComboArticulo.TabIndex = 1
        '
        'ComboDeposito
        '
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(500, 32)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(116, 21)
        Me.ComboDeposito.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(440, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Deposito"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(107, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Articulo"
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.OtrasAsignaciones, Me.Lote, Me.Secuencia, Me.KilosXUnidad, Me.candado, Me.LoteYSecuencia, Me.Proveedor, Me.Fecha, Me.Calibre, Me.Stock})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke
        Me.Grid.Location = New System.Drawing.Point(113, 65)
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        Me.Grid.RowHeadersWidth = 25
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(506, 466)
        Me.Grid.TabIndex = 17
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        '
        'OtrasAsignaciones
        '
        Me.OtrasAsignaciones.DataPropertyName = "OtrasAsignaciones"
        Me.OtrasAsignaciones.HeaderText = "OtrasAsignaciones"
        Me.OtrasAsignaciones.Name = "OtrasAsignaciones"
        Me.OtrasAsignaciones.ReadOnly = True
        Me.OtrasAsignaciones.Visible = False
        '
        'Lote
        '
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle3
        Me.Lote.HeaderText = "Lote"
        Me.Lote.Name = "Lote"
        Me.Lote.ReadOnly = True
        Me.Lote.Visible = False
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.ReadOnly = True
        Me.Secuencia.Visible = False
        '
        'KilosXUnidad
        '
        Me.KilosXUnidad.DataPropertyName = "kilosXUnidad"
        Me.KilosXUnidad.HeaderText = "KilosXUnidad"
        Me.KilosXUnidad.Name = "KilosXUnidad"
        Me.KilosXUnidad.ReadOnly = True
        Me.KilosXUnidad.Visible = False
        '
        'candado
        '
        Me.candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.candado.HeaderText = ""
        Me.candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.candado.MinimumWidth = 30
        Me.candado.Name = "candado"
        Me.candado.ReadOnly = True
        Me.candado.Width = 30
        '
        'LoteYSecuencia
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle4
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 70
        '
        'Proveedor
        '
        Me.Proveedor.DataPropertyName = "Proveedor"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Proveedor.DefaultCellStyle = DataGridViewCellStyle5
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
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle6
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'Calibre
        '
        Me.Calibre.DataPropertyName = "Calibre"
        Me.Calibre.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Calibre.HeaderText = "Calibre"
        Me.Calibre.MinimumWidth = 70
        Me.Calibre.Name = "Calibre"
        Me.Calibre.ReadOnly = True
        Me.Calibre.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Calibre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Calibre.Width = 70
        '
        'Stock
        '
        Me.Stock.DataPropertyName = "Stock"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle7
        Me.Stock.HeaderText = "Stock"
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Width = 70
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'MuestraStockArticulo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(737, 564)
        Me.Controls.Add(Me.ComboArticulo)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Grid)
        Me.Name = "MuestraStockArticulo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "MuestraStockArticulo"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OtrasAsignaciones As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KilosXUnidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Calibre As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
