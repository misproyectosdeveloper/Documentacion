<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UNComprobanteAImputar
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
        Dim DataGridViewCellStyle41 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle42 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle43 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle44 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle45 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle46 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle47 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle48 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UNComprobanteAImputar))
        Me.GridCompro = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Seleccion = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Tipo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.TipoVisible = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Comprobante1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Recibo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaCompro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImporteCompro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Moneda = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Asignado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextTotalFacturas = New System.Windows.Forms.TextBox
        Me.TextSaldo = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonMarcarTodos = New System.Windows.Forms.Button
        Me.ButtonDesmarcarTodos = New System.Windows.Forms.Button
        CType(Me.GridCompro, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GridCompro
        '
        Me.GridCompro.AllowUserToAddRows = False
        Me.GridCompro.AllowUserToDeleteRows = False
        Me.GridCompro.BackgroundColor = System.Drawing.Color.White
        Me.GridCompro.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridCompro.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Seleccion, Me.Candado, Me.Tipo, Me.TipoVisible, Me.Comprobante1, Me.Recibo, Me.Comentario, Me.FechaCompro, Me.ImporteCompro, Me.Moneda, Me.Saldo, Me.Asignado})
        Me.GridCompro.Location = New System.Drawing.Point(106, 78)
        Me.GridCompro.Name = "GridCompro"
        Me.GridCompro.RowHeadersWidth = 20
        Me.GridCompro.Size = New System.Drawing.Size(824, 423)
        Me.GridCompro.TabIndex = 13
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        '
        'Seleccion
        '
        Me.Seleccion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Seleccion.HeaderText = ""
        Me.Seleccion.MinimumWidth = 20
        Me.Seleccion.Name = "Seleccion"
        Me.Seleccion.Width = 20
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
        'Tipo
        '
        Me.Tipo.DataPropertyName = "Tipo"
        DataGridViewCellStyle41.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Tipo.DefaultCellStyle = DataGridViewCellStyle41
        Me.Tipo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Tipo.HeaderText = "Tipo No Visible"
        Me.Tipo.MinimumWidth = 85
        Me.Tipo.Name = "Tipo"
        Me.Tipo.ReadOnly = True
        Me.Tipo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Tipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Tipo.Visible = False
        Me.Tipo.Width = 85
        '
        'TipoVisible
        '
        Me.TipoVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoVisible.DataPropertyName = "TipoVisible"
        DataGridViewCellStyle42.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft
        Me.TipoVisible.DefaultCellStyle = DataGridViewCellStyle42
        Me.TipoVisible.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.TipoVisible.HeaderText = "Tipo"
        Me.TipoVisible.MinimumWidth = 85
        Me.TipoVisible.Name = "TipoVisible"
        Me.TipoVisible.ReadOnly = True
        Me.TipoVisible.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TipoVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TipoVisible.Width = 85
        '
        'Comprobante1
        '
        Me.Comprobante1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comprobante1.DataPropertyName = "Comprobante"
        DataGridViewCellStyle43.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Comprobante1.DefaultCellStyle = DataGridViewCellStyle43
        Me.Comprobante1.HeaderText = "Comprobante"
        Me.Comprobante1.MinimumWidth = 90
        Me.Comprobante1.Name = "Comprobante1"
        Me.Comprobante1.ReadOnly = True
        Me.Comprobante1.Visible = False
        Me.Comprobante1.Width = 90
        '
        'Recibo
        '
        Me.Recibo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Recibo.DataPropertyName = "Recibo"
        Me.Recibo.HeaderText = "Comprobante"
        Me.Recibo.MinimumWidth = 90
        Me.Recibo.Name = "Recibo"
        Me.Recibo.ReadOnly = True
        Me.Recibo.Width = 90
        '
        'Comentario
        '
        Me.Comentario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comentario.DataPropertyName = "Comentario"
        DataGridViewCellStyle44.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Comentario.DefaultCellStyle = DataGridViewCellStyle44
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.MinimumWidth = 90
        Me.Comentario.Name = "Comentario"
        Me.Comentario.ReadOnly = True
        Me.Comentario.Width = 90
        '
        'FechaCompro
        '
        Me.FechaCompro.DataPropertyName = "Fecha"
        DataGridViewCellStyle45.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaCompro.DefaultCellStyle = DataGridViewCellStyle45
        Me.FechaCompro.HeaderText = "Fecha"
        Me.FechaCompro.MinimumWidth = 70
        Me.FechaCompro.Name = "FechaCompro"
        Me.FechaCompro.ReadOnly = True
        Me.FechaCompro.Width = 70
        '
        'ImporteCompro
        '
        Me.ImporteCompro.DataPropertyName = "Importe"
        DataGridViewCellStyle46.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ImporteCompro.DefaultCellStyle = DataGridViewCellStyle46
        Me.ImporteCompro.HeaderText = "Importe"
        Me.ImporteCompro.Name = "ImporteCompro"
        Me.ImporteCompro.ReadOnly = True
        '
        'Moneda
        '
        Me.Moneda.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Moneda.DataPropertyName = "Moneda"
        Me.Moneda.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Moneda.HeaderText = "Moneda"
        Me.Moneda.MinimumWidth = 80
        Me.Moneda.Name = "Moneda"
        Me.Moneda.ReadOnly = True
        Me.Moneda.Width = 80
        '
        'Saldo
        '
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle47.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle47
        Me.Saldo.HeaderText = "Saldo"
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        '
        'Asignado
        '
        Me.Asignado.DataPropertyName = "Asignado"
        DataGridViewCellStyle48.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Asignado.DefaultCellStyle = DataGridViewCellStyle48
        Me.Asignado.HeaderText = "Imp. Asignado"
        Me.Asignado.MaxInputLength = 12
        Me.Asignado.Name = "Asignado"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(393, 41)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(161, 24)
        Me.Label1.TabIndex = 14
        Me.Label1.Text = "Total a Distribuir"
        '
        'TextTotalFacturas
        '
        Me.TextTotalFacturas.BackColor = System.Drawing.Color.White
        Me.TextTotalFacturas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalFacturas.Location = New System.Drawing.Point(786, 509)
        Me.TextTotalFacturas.MaxLength = 20
        Me.TextTotalFacturas.Name = "TextTotalFacturas"
        Me.TextTotalFacturas.ReadOnly = True
        Me.TextTotalFacturas.Size = New System.Drawing.Size(104, 20)
        Me.TextTotalFacturas.TabIndex = 232
        Me.TextTotalFacturas.TabStop = False
        Me.TextTotalFacturas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextSaldo
        '
        Me.TextSaldo.BackColor = System.Drawing.Color.White
        Me.TextSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldo.Location = New System.Drawing.Point(501, 509)
        Me.TextSaldo.MaxLength = 20
        Me.TextSaldo.Name = "TextSaldo"
        Me.TextSaldo.ReadOnly = True
        Me.TextSaldo.Size = New System.Drawing.Size(108, 20)
        Me.TextSaldo.TabIndex = 246
        Me.TextSaldo.TabStop = False
        Me.TextSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(309, 510)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(177, 16)
        Me.Label6.TabIndex = 247
        Me.Label6.Text = "Pago a Cuenta Corriente"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.GreenYellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(779, 554)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 248
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(657, 510)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 16)
        Me.Label2.TabIndex = 249
        Me.Label2.Text = "Total Imputado"
        '
        'ButtonMarcarTodos
        '
        Me.ButtonMarcarTodos.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMarcarTodos.Location = New System.Drawing.Point(106, 41)
        Me.ButtonMarcarTodos.Name = "ButtonMarcarTodos"
        Me.ButtonMarcarTodos.Size = New System.Drawing.Size(124, 21)
        Me.ButtonMarcarTodos.TabIndex = 250
        Me.ButtonMarcarTodos.Text = "Marcar Todos"
        Me.ButtonMarcarTodos.UseVisualStyleBackColor = True
        '
        'ButtonDesmarcarTodos
        '
        Me.ButtonDesmarcarTodos.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonDesmarcarTodos.Location = New System.Drawing.Point(245, 41)
        Me.ButtonDesmarcarTodos.Name = "ButtonDesmarcarTodos"
        Me.ButtonDesmarcarTodos.Size = New System.Drawing.Size(106, 21)
        Me.ButtonDesmarcarTodos.TabIndex = 251
        Me.ButtonDesmarcarTodos.Text = "Des-Marcar Todos"
        Me.ButtonDesmarcarTodos.UseVisualStyleBackColor = True
        '
        'UNComprobanteAImputar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(1026, 617)
        Me.Controls.Add(Me.GridCompro)
        Me.Controls.Add(Me.ButtonDesmarcarTodos)
        Me.Controls.Add(Me.ButtonMarcarTodos)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextSaldo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextTotalFacturas)
        Me.Controls.Add(Me.Label1)
        Me.Name = "UNComprobanteAImputar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Comprobantes A Imputar"
        CType(Me.GridCompro, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GridCompro As System.Windows.Forms.DataGridView
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Seleccion As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Tipo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents TipoVisible As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Comprobante1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Recibo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaCompro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImporteCompro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Moneda As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Asignado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextTotalFacturas As System.Windows.Forms.TextBox
    Friend WithEvents TextSaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonMarcarTodos As System.Windows.Forms.Button
    Friend WithEvents ButtonDesmarcarTodos As System.Windows.Forms.Button
End Class
