<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaFondosFijos
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaFondosFijos))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboFondoFijo = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ButtonSeleccionar = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.FondoFijo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Numero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cerrado = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextNumero)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboFondoFijo)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.ButtonSeleccionar)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.label1)
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Location = New System.Drawing.Point(29, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(788, 50)
        Me.Panel1.TabIndex = 160
        '
        'TextNumero
        '
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(391, 3)
        Me.TextNumero.MaxLength = 7
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(68, 20)
        Me.TextNumero.TabIndex = 206
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(342, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 13)
        Me.Label3.TabIndex = 207
        Me.Label3.Text = "Numero"
        '
        'ComboFondoFijo
        '
        Me.ComboFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboFondoFijo.FormattingEnabled = True
        Me.ComboFondoFijo.Location = New System.Drawing.Point(119, 2)
        Me.ComboFondoFijo.Name = "ComboFondoFijo"
        Me.ComboFondoFijo.Size = New System.Drawing.Size(219, 21)
        Me.ComboFondoFijo.TabIndex = 198
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(2, 5)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(108, 13)
        Me.LabelEmisor.TabIndex = 199
        Me.LabelEmisor.Text = "Proveedor Fondo Fijo"
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(666, 4)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(106, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(513, 3)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(105, 20)
        Me.DateTimeDesde.TabIndex = 31
        '
        'ButtonSeleccionar
        '
        Me.ButtonSeleccionar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonSeleccionar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSeleccionar.Location = New System.Drawing.Point(660, 27)
        Me.ButtonSeleccionar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonSeleccionar.Name = "ButtonSeleccionar"
        Me.ButtonSeleccionar.Size = New System.Drawing.Size(114, 20)
        Me.ButtonSeleccionar.TabIndex = 30
        Me.ButtonSeleccionar.Text = "Seleccionar"
        Me.ButtonSeleccionar.UseVisualStyleBackColor = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(626, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(35, 13)
        Me.Label2.TabIndex = 29
        Me.Label2.Text = "Hasta"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(471, 6)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(38, 13)
        Me.label1.TabIndex = 28
        Me.label1.Text = "Desde"
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.Yellow
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(899, 6)
        Me.Button1.Margin = New System.Windows.Forms.Padding(0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(117, 22)
        Me.Button1.TabIndex = 9
        Me.Button1.Text = "Seleccionar"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(29, 656)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 154
        Me.ButtonPrimero.TabStop = False
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(63, 656)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 155
        Me.ButtonAnterior.TabStop = False
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(95, 656)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 156
        Me.ButtonPosterior.TabStop = False
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(127, 656)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 157
        Me.ButtonUltimo.TabStop = False
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.FondoFijo, Me.Numero, Me.Fecha, Me.Saldo, Me.Cerrado, Me.Comentario})
        Me.Grid.Location = New System.Drawing.Point(29, 53)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.OldLace
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Size = New System.Drawing.Size(788, 600)
        Me.Grid.TabIndex = 161
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
        'FondoFijo
        '
        Me.FondoFijo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.FondoFijo.DataPropertyName = "FondoFijo"
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FondoFijo.DefaultCellStyle = DataGridViewCellStyle2
        Me.FondoFijo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.FondoFijo.HeaderText = "Proveedor Fondo Fijo"
        Me.FondoFijo.MinimumWidth = 180
        Me.FondoFijo.Name = "FondoFijo"
        Me.FondoFijo.ReadOnly = True
        Me.FondoFijo.Width = 180
        '
        'Numero
        '
        Me.Numero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Numero.DataPropertyName = "Numero"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Numero.DefaultCellStyle = DataGridViewCellStyle3
        Me.Numero.HeaderText = "Numero"
        Me.Numero.MinimumWidth = 90
        Me.Numero.Name = "Numero"
        Me.Numero.ReadOnly = True
        Me.Numero.Width = 90
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle4
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'Saldo
        '
        Me.Saldo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle5
        Me.Saldo.HeaderText = "Importe"
        Me.Saldo.MinimumWidth = 90
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Width = 90
        '
        'Cerrado
        '
        Me.Cerrado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cerrado.DataPropertyName = "Cerrado"
        Me.Cerrado.HeaderText = "Cerrado"
        Me.Cerrado.MinimumWidth = 60
        Me.Cerrado.Name = "Cerrado"
        Me.Cerrado.ReadOnly = True
        Me.Cerrado.Width = 60
        '
        'Comentario
        '
        Me.Comentario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comentario.DataPropertyName = "Comentario"
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.MaxInputLength = 30
        Me.Comentario.MinimumWidth = 200
        Me.Comentario.Name = "Comentario"
        Me.Comentario.ReadOnly = True
        Me.Comentario.Width = 200
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ListaFondosFijos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(851, 681)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Name = "ListaFondosFijos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Fondos Fijo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonSeleccionar As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboFondoFijo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents FondoFijo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Numero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cerrado As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
