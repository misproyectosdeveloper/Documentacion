<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaTransferenciasLogisticas
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
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.TextComprobante = New System.Windows.Forms.TextBox
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboDepositoDestino = New System.Windows.Forms.ComboBox
        Me.ComboDepositoOrigen = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Comprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Origen = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Destino = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(112, 651)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 134
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'TextComprobante
        '
        Me.TextComprobante.BackColor = System.Drawing.Color.White
        Me.TextComprobante.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComprobante.Location = New System.Drawing.Point(101, 3)
        Me.TextComprobante.MaxLength = 8
        Me.TextComprobante.Name = "TextComprobante"
        Me.TextComprobante.Size = New System.Drawing.Size(120, 20)
        Me.TextComprobante.TabIndex = 203
        Me.TextComprobante.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(144, 651)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonAnterior.TabIndex = 135
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(175, 651)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonPosterior.TabIndex = 132
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 31)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(94, 13)
        Me.Label9.TabIndex = 190
        Me.Label9.Text = "Depositos:  Origen"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Comprobante, Me.Fecha, Me.Origen, Me.Destino, Me.Cantidad, Me.Estado})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.Location = New System.Drawing.Point(113, 57)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Grid.Size = New System.Drawing.Size(559, 590)
        Me.Grid.TabIndex = 131
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboDepositoDestino)
        Me.Panel1.Controls.Add(Me.TextComprobante)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ComboDepositoOrigen)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Location = New System.Drawing.Point(113, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(559, 52)
        Me.Panel1.TabIndex = 130
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(246, 31)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 13)
        Me.Label1.TabIndex = 205
        Me.Label1.Text = "Destino"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDepositoDestino
        '
        Me.ComboDepositoDestino.BackColor = System.Drawing.Color.White
        Me.ComboDepositoDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDepositoDestino.FormattingEnabled = True
        Me.ComboDepositoDestino.Location = New System.Drawing.Point(297, 27)
        Me.ComboDepositoDestino.Name = "ComboDepositoDestino"
        Me.ComboDepositoDestino.Size = New System.Drawing.Size(130, 21)
        Me.ComboDepositoDestino.TabIndex = 204
        '
        'ComboDepositoOrigen
        '
        Me.ComboDepositoOrigen.BackColor = System.Drawing.Color.White
        Me.ComboDepositoOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDepositoOrigen.FormattingEnabled = True
        Me.ComboDepositoOrigen.Location = New System.Drawing.Point(110, 27)
        Me.ComboDepositoOrigen.Name = "ComboDepositoOrigen"
        Me.ComboDepositoOrigen.Size = New System.Drawing.Size(130, 21)
        Me.ComboDepositoOrigen.TabIndex = 189
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(70, 13)
        Me.Label6.TabIndex = 187
        Me.Label6.Text = "Comprobante"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(449, 26)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(98, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(384, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(227, 5)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(38, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(424, 2)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(106, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(269, 1)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(106, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(206, 651)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 133
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(502, 651)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(166, 25)
        Me.ButtonExportarExcel.TabIndex = 162
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Operacion.DefaultCellStyle = DataGridViewCellStyle2
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.MinimumWidth = 100
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        '
        'Candado
        '
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.ReadOnly = True
        Me.Candado.Width = 30
        '
        'Comprobante
        '
        Me.Comprobante.DataPropertyName = "Transferencia"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Comprobante.DefaultCellStyle = DataGridViewCellStyle3
        Me.Comprobante.HeaderText = "Comprobante"
        Me.Comprobante.MinimumWidth = 100
        Me.Comprobante.Name = "Comprobante"
        Me.Comprobante.ReadOnly = True
        '
        'Fecha
        '
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle4
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 80
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 80
        '
        'Origen
        '
        Me.Origen.DataPropertyName = "Origen"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Origen.DefaultCellStyle = DataGridViewCellStyle5
        Me.Origen.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Origen.HeaderText = "Origen"
        Me.Origen.MinimumWidth = 80
        Me.Origen.Name = "Origen"
        Me.Origen.ReadOnly = True
        Me.Origen.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Origen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Origen.Width = 80
        '
        'Destino
        '
        Me.Destino.DataPropertyName = "Destino"
        Me.Destino.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Destino.HeaderText = "Destino"
        Me.Destino.MinimumWidth = 80
        Me.Destino.Name = "Destino"
        Me.Destino.ReadOnly = True
        Me.Destino.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Destino.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Destino.Width = 80
        '
        'Cantidad
        '
        Me.Cantidad.DataPropertyName = "Cantidad"
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 65
        '
        'Estado
        '
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Red
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle6
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 80
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Estado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Estado.Width = 80
        '
        'ListaTransferenciasLogisticas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(784, 676)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Name = "ListaTransferenciasLogisticas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Transferencias Artículos Logísticos"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents TextComprobante As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboDepositoOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboDepositoDestino As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Comprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Origen As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Destino As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
