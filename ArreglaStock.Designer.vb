<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArreglaStock
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Button1 = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Especie = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Ingreso = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(12, 1)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(233, 33)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
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
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Lote, Me.Secuencia, Me.Especie, Me.Articulo, Me.Ingreso, Me.Stock, Me.Cantidad})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.Location = New System.Drawing.Point(17, 39)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(757, 536)
        Me.Grid.TabIndex = 99
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(284, 594)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(191, 35)
        Me.ButtonExportarExcel.TabIndex = 165
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'Operacion
        '
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Width = 81
        '
        'Lote
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle2
        Me.Lote.HeaderText = "Lote"
        Me.Lote.Name = "Lote"
        Me.Lote.ReadOnly = True
        Me.Lote.Width = 53
        '
        'Secuencia
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Secuencia.DefaultCellStyle = DataGridViewCellStyle3
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.ReadOnly = True
        Me.Secuencia.Width = 83
        '
        'Especie
        '
        Me.Especie.HeaderText = "Especie"
        Me.Especie.Name = "Especie"
        Me.Especie.ReadOnly = True
        Me.Especie.Width = 70
        '
        'Articulo
        '
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Width = 67
        '
        'Ingreso
        '
        Me.Ingreso.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Ingreso.DataPropertyName = "Ingreso"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Ingreso.DefaultCellStyle = DataGridViewCellStyle4
        Me.Ingreso.HeaderText = "Ingreso"
        Me.Ingreso.MinimumWidth = 70
        Me.Ingreso.Name = "Ingreso"
        Me.Ingreso.ReadOnly = True
        Me.Ingreso.Width = 70
        '
        'Stock
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle5
        Me.Stock.HeaderText = "Stock"
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Width = 60
        '
        'Cantidad
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle6
        Me.Cantidad.HeaderText = "Asignado"
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 76
        '
        'ArreglaStock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(798, 632)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Button1)
        Me.Name = "ArreglaStock"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ArreglaStock"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Especie As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ingreso As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
