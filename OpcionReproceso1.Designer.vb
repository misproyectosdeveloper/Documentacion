<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionReproceso1
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextTotalBaja = New System.Windows.Forms.TextBox
        Me.GridBajas = New System.Windows.Forms.DataGridView
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.ButtonVerLotes = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Medida = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Calibre = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Baja = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.GridBajas, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(555, 456)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(110, 28)
        Me.Label11.TabIndex = 169
        Me.Label11.Text = "Total Baja"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotalBaja
        '
        Me.TextTotalBaja.BackColor = System.Drawing.Color.White
        Me.TextTotalBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalBaja.Location = New System.Drawing.Point(670, 460)
        Me.TextTotalBaja.MaxLength = 8
        Me.TextTotalBaja.Name = "TextTotalBaja"
        Me.TextTotalBaja.ReadOnly = True
        Me.TextTotalBaja.Size = New System.Drawing.Size(128, 20)
        Me.TextTotalBaja.TabIndex = 168
        Me.TextTotalBaja.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GridBajas
        '
        Me.GridBajas.AllowUserToAddRows = False
        Me.GridBajas.AllowUserToDeleteRows = False
        Me.GridBajas.BackgroundColor = System.Drawing.Color.White
        Me.GridBajas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridBajas.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Lote, Me.Secuencia, Me.LoteYSecuencia, Me.Fecha, Me.Proveedor, Me.Medida, Me.Calibre, Me.Stock, Me.Baja})
        Me.GridBajas.Location = New System.Drawing.Point(113, 140)
        Me.GridBajas.Name = "GridBajas"
        Me.GridBajas.RowHeadersWidth = 40
        Me.GridBajas.Size = New System.Drawing.Size(688, 313)
        Me.GridBajas.TabIndex = 165
        '
        'ComboArticulo
        '
        Me.ComboArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(491, 39)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(332, 21)
        Me.ComboArticulo.TabIndex = 163
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(422, 43)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(50, 13)
        Me.LabelEmisor.TabIndex = 167
        Me.LabelEmisor.Text = "Articulo"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(45, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 166
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(110, 39)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(142, 21)
        Me.ComboDeposito.TabIndex = 162
        '
        'ButtonVerLotes
        '
        Me.ButtonVerLotes.BackColor = System.Drawing.Color.Plum
        Me.ButtonVerLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerLotes.Location = New System.Drawing.Point(360, 109)
        Me.ButtonVerLotes.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonVerLotes.Name = "ButtonVerLotes"
        Me.ButtonVerLotes.Size = New System.Drawing.Size(204, 26)
        Me.ButtonVerLotes.TabIndex = 164
        Me.ButtonVerLotes.Text = "Seleccionar Lote a Reprocesar"
        Me.ButtonVerLotes.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(391, 559)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 170
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
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
        'Medida
        '
        Me.Medida.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Medida.DataPropertyName = "Medida"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Medida.DefaultCellStyle = DataGridViewCellStyle4
        Me.Medida.HeaderText = "Un"
        Me.Medida.MinimumWidth = 30
        Me.Medida.Name = "Medida"
        Me.Medida.ReadOnly = True
        Me.Medida.Width = 30
        '
        'Calibre
        '
        Me.Calibre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Calibre.DataPropertyName = "Calibre"
        Me.Calibre.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Calibre.HeaderText = "Calibre"
        Me.Calibre.MinimumWidth = 60
        Me.Calibre.Name = "Calibre"
        Me.Calibre.ReadOnly = True
        Me.Calibre.Width = 60
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
        'Baja
        '
        Me.Baja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Baja.DataPropertyName = "Baja"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Baja.DefaultCellStyle = DataGridViewCellStyle6
        Me.Baja.HeaderText = "Baja"
        Me.Baja.MaxInputLength = 10
        Me.Baja.MinimumWidth = 110
        Me.Baja.Name = "Baja"
        Me.Baja.Width = 110
        '
        'OpcionReproceso1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(881, 612)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TextTotalBaja)
        Me.Controls.Add(Me.GridBajas)
        Me.Controls.Add(Me.ComboArticulo)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.ButtonVerLotes)
        Me.Name = "OpcionReproceso1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Reproceso"
        CType(Me.GridBajas, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextTotalBaja As System.Windows.Forms.TextBox
    Friend WithEvents GridBajas As System.Windows.Forms.DataGridView
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonVerLotes As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Medida As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Calibre As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Baja As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
