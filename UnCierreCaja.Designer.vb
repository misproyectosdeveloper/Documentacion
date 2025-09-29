<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnCierreCaja
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnCierreCaja))
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Concepto = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Moneda = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.SaldoAnt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Debito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Credito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ComboCaja = New System.Windows.Forms.ComboBox
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonListaChequesTerceros = New System.Windows.Forms.Button
        Me.ButtonPaseCaja = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Grid2 = New System.Windows.Forms.DataGridView
        Me.Operacion2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado2 = New System.Windows.Forms.DataGridViewImageColumn
        Me.Pase2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Grid1 = New System.Windows.Forms.DataGridView
        Me.Operacion1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.Pase1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Concepto, Me.Moneda, Me.SaldoAnt, Me.Debito, Me.Credito, Me.Saldo})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Location = New System.Drawing.Point(51, 87)
        Me.Grid.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.Size = New System.Drawing.Size(695, 274)
        Me.Grid.TabIndex = 14
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
        Me.Candado.Width = 30
        '
        'Concepto
        '
        Me.Concepto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto.DataPropertyName = "MedioPago"
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Concepto.DefaultCellStyle = DataGridViewCellStyle1
        Me.Concepto.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Concepto.HeaderText = "Concepto"
        Me.Concepto.MinimumWidth = 130
        Me.Concepto.Name = "Concepto"
        Me.Concepto.ReadOnly = True
        Me.Concepto.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Concepto.Width = 130
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
        'SaldoAnt
        '
        Me.SaldoAnt.DataPropertyName = "SaldoAnt"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.SaldoAnt.DefaultCellStyle = DataGridViewCellStyle2
        Me.SaldoAnt.HeaderText = "Saldo  Anterior"
        Me.SaldoAnt.Name = "SaldoAnt"
        Me.SaldoAnt.ReadOnly = True
        '
        'Debito
        '
        Me.Debito.DataPropertyName = "Debito"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Debito.DefaultCellStyle = DataGridViewCellStyle3
        Me.Debito.HeaderText = "Debito"
        Me.Debito.Name = "Debito"
        Me.Debito.ReadOnly = True
        '
        'Credito
        '
        Me.Credito.DataPropertyName = "Credito"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Credito.DefaultCellStyle = DataGridViewCellStyle4
        Me.Credito.HeaderText = "Credito"
        Me.Credito.Name = "Credito"
        Me.Credito.ReadOnly = True
        '
        'Saldo
        '
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle5
        Me.Saldo.HeaderText = "Saldo  Actual"
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        '
        'ComboCaja
        '
        Me.ComboCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCaja.FormattingEnabled = True
        Me.ComboCaja.Location = New System.Drawing.Point(91, 59)
        Me.ComboCaja.Name = "ComboCaja"
        Me.ComboCaja.Size = New System.Drawing.Size(121, 24)
        Me.ComboCaja.TabIndex = 16
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(633, 62)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 31
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(583, 66)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(42, 13)
        Me.Label4.TabIndex = 34
        Me.Label4.Text = "Fecha"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(50, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 35
        Me.Label1.Text = "Caja"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonListaChequesTerceros
        '
        Me.ButtonListaChequesTerceros.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonListaChequesTerceros.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonListaChequesTerceros.Location = New System.Drawing.Point(545, 520)
        Me.ButtonListaChequesTerceros.Name = "ButtonListaChequesTerceros"
        Me.ButtonListaChequesTerceros.Size = New System.Drawing.Size(196, 29)
        Me.ButtonListaChequesTerceros.TabIndex = 51
        Me.ButtonListaChequesTerceros.Text = "Cheques Terceros en Cartera"
        Me.ButtonListaChequesTerceros.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonListaChequesTerceros.UseVisualStyleBackColor = True
        '
        'ButtonPaseCaja
        '
        Me.ButtonPaseCaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonPaseCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPaseCaja.Location = New System.Drawing.Point(50, 519)
        Me.ButtonPaseCaja.Name = "ButtonPaseCaja"
        Me.ButtonPaseCaja.Size = New System.Drawing.Size(187, 29)
        Me.ButtonPaseCaja.TabIndex = 52
        Me.ButtonPaseCaja.Text = "Pase De Caja"
        Me.ButtonPaseCaja.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonPaseCaja.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Grid2
        '
        Me.Grid2.AllowUserToAddRows = False
        Me.Grid2.AllowUserToDeleteRows = False
        Me.Grid2.BackgroundColor = System.Drawing.Color.White
        Me.Grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion2, Me.Candado2, Me.Pase2})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid2.DefaultCellStyle = DataGridViewCellStyle9
        Me.Grid2.Location = New System.Drawing.Point(574, 391)
        Me.Grid2.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid2.Name = "Grid2"
        Me.Grid2.RowHeadersWidth = 20
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        Me.Grid2.RowsDefaultCellStyle = DataGridViewCellStyle10
        Me.Grid2.Size = New System.Drawing.Size(177, 65)
        Me.Grid2.TabIndex = 53
        '
        'Operacion2
        '
        Me.Operacion2.HeaderText = "Operacion"
        Me.Operacion2.Name = "Operacion2"
        Me.Operacion2.ReadOnly = True
        Me.Operacion2.Visible = False
        '
        'Candado2
        '
        Me.Candado2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado2.HeaderText = ""
        Me.Candado2.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado2.MinimumWidth = 30
        Me.Candado2.Name = "Candado2"
        Me.Candado2.ReadOnly = True
        Me.Candado2.Width = 30
        '
        'Pase2
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Pase2.DefaultCellStyle = DataGridViewCellStyle8
        Me.Pase2.HeaderText = "Pase"
        Me.Pase2.Name = "Pase2"
        Me.Pase2.ReadOnly = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(576, 372)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(171, 13)
        Me.Label2.TabIndex = 54
        Me.Label2.Text = "Pases Propios No Aceptados"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(48, 373)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(201, 13)
        Me.Label3.TabIndex = 56
        Me.Label3.Text = "Pases de Terceros  No Aceptados"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid1
        '
        Me.Grid1.AllowUserToAddRows = False
        Me.Grid1.AllowUserToDeleteRows = False
        Me.Grid1.BackgroundColor = System.Drawing.Color.White
        Me.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion1, Me.Candado1, Me.Pase1})
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid1.DefaultCellStyle = DataGridViewCellStyle12
        Me.Grid1.Location = New System.Drawing.Point(60, 392)
        Me.Grid1.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid1.Name = "Grid1"
        Me.Grid1.RowHeadersWidth = 20
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.White
        Me.Grid1.RowsDefaultCellStyle = DataGridViewCellStyle13
        Me.Grid1.Size = New System.Drawing.Size(177, 65)
        Me.Grid1.TabIndex = 55
        '
        'Operacion1
        '
        Me.Operacion1.HeaderText = "Operacion"
        Me.Operacion1.Name = "Operacion1"
        Me.Operacion1.ReadOnly = True
        Me.Operacion1.Visible = False
        '
        'Candado1
        '
        Me.Candado1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado1.HeaderText = ""
        Me.Candado1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado1.MinimumWidth = 30
        Me.Candado1.Name = "Candado1"
        Me.Candado1.ReadOnly = True
        Me.Candado1.Width = 30
        '
        'Pase1
        '
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Pase1.DefaultCellStyle = DataGridViewCellStyle11
        Me.Pase1.HeaderText = "Pase"
        Me.Pase1.Name = "Pase1"
        Me.Pase1.ReadOnly = True
        '
        'UnCierreCaja
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(803, 570)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Grid1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Grid2)
        Me.Controls.Add(Me.ButtonPaseCaja)
        Me.Controls.Add(Me.ButtonListaChequesTerceros)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.ComboCaja)
        Me.Controls.Add(Me.Grid)
        Me.Name = "UnCierreCaja"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cierre de Caja"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboCaja As System.Windows.Forms.ComboBox
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonListaChequesTerceros As System.Windows.Forms.Button
    Friend WithEvents ButtonPaseCaja As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Grid2 As System.Windows.Forms.DataGridView
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Grid1 As System.Windows.Forms.DataGridView
    Friend WithEvents Operacion1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Pase1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Operacion2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado2 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Pase2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Concepto As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Moneda As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents SaldoAnt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Debito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Credito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
