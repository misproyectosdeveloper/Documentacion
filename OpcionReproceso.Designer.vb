<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionReproceso
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextBaja = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.ButtonVerLotes = New System.Windows.Forms.Button
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.KilosXUnidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Sel, Me.Lote, Me.Secuencia, Me.LoteYSecuencia, Me.Fecha, Me.Proveedor, Me.KilosXUnidad, Me.Stock})
        Me.Grid.Location = New System.Drawing.Point(48, 148)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        Me.Grid.Size = New System.Drawing.Size(563, 374)
        Me.Grid.TabIndex = 4
        '
        'ComboArticulo
        '
        Me.ComboArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(288, 27)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(332, 21)
        Me.ComboArticulo.TabIndex = 2
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(233, 31)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(50, 13)
        Me.LabelEmisor.TabIndex = 147
        Me.LabelEmisor.Text = "Articulo"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(13, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 146
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(78, 27)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(142, 21)
        Me.ComboDeposito.TabIndex = 1
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(269, 560)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 5
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextBaja
        '
        Me.TextBaja.BackColor = System.Drawing.Color.White
        Me.TextBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBaja.Location = New System.Drawing.Point(329, 82)
        Me.TextBaja.MaxLength = 7
        Me.TextBaja.Name = "TextBaja"
        Me.TextBaja.Size = New System.Drawing.Size(119, 20)
        Me.TextBaja.TabIndex = 3
        Me.TextBaja.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(206, 79)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(110, 28)
        Me.Label7.TabIndex = 150
        Me.Label7.Text = "Unidades de Baja"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonVerLotes
        '
        Me.ButtonVerLotes.BackColor = System.Drawing.Color.Plum
        Me.ButtonVerLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerLotes.Location = New System.Drawing.Point(235, 115)
        Me.ButtonVerLotes.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonVerLotes.Name = "ButtonVerLotes"
        Me.ButtonVerLotes.Size = New System.Drawing.Size(204, 26)
        Me.ButtonVerLotes.TabIndex = 4
        Me.ButtonVerLotes.Text = "Seleccionar Lote a Reprocesar"
        Me.ButtonVerLotes.UseVisualStyleBackColor = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.ReadOnly = True
        Me.Candado.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Candado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Candado.Width = 30
        '
        'Sel
        '
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 30
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 30
        '
        'Lote
        '
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle1
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
        Me.Secuencia.Visible = False
        '
        'LoteYSecuencia
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle2
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 90
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 90
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle3
        Me.Fecha.HeaderText = "Ingreso"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'Proveedor
        '
        Me.Proveedor.DataPropertyName = "Proveedor"
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Proveedor"
        Me.Proveedor.MinimumWidth = 120
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        Me.Proveedor.Width = 120
        '
        'KilosXUnidad
        '
        Me.KilosXUnidad.DataPropertyName = "KilosXUnidad"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.KilosXUnidad.DefaultCellStyle = DataGridViewCellStyle4
        Me.KilosXUnidad.HeaderText = "Kg. X Uni."
        Me.KilosXUnidad.MinimumWidth = 80
        Me.KilosXUnidad.Name = "KilosXUnidad"
        Me.KilosXUnidad.ReadOnly = True
        Me.KilosXUnidad.Width = 80
        '
        'Stock
        '
        Me.Stock.DataPropertyName = "Stock"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle5
        Me.Stock.HeaderText = "Stock"
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Stock.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'OpcionReproceso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(668, 608)
        Me.Controls.Add(Me.ButtonVerLotes)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.TextBaja)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.ComboArticulo)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionReproceso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Reproceso"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextBaja As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerLotes As System.Windows.Forms.Button
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents KilosXUnidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
