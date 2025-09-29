<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaEmpleados
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
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaEmpleados))
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Legajo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Apellidos = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombres = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Ingreso = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Baja = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Banco = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Bruto = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BrutoN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.LemonChiffon
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle11
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Legajo, Me.Apellidos, Me.Nombres, Me.Ingreso, Me.Baja, Me.Banco, Me.Cuenta, Me.Bruto, Me.BrutoN, Me.Estado})
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle18
        Me.Grid.Location = New System.Drawing.Point(16, 38)
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        Me.Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle19.BackColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        DataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle19
        Me.Grid.RowHeadersWidth = 30
        DataGridViewCellStyle20.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle20
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(887, 597)
        Me.Grid.TabIndex = 2
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Operacion.DefaultCellStyle = DataGridViewCellStyle12
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.MinimumWidth = 70
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        Me.Operacion.Width = 81
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
        'Legajo
        '
        Me.Legajo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Legajo.DataPropertyName = "Legajo"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Legajo.DefaultCellStyle = DataGridViewCellStyle13
        Me.Legajo.HeaderText = "Legajo"
        Me.Legajo.MinimumWidth = 65
        Me.Legajo.Name = "Legajo"
        Me.Legajo.ReadOnly = True
        Me.Legajo.Width = 65
        '
        'Apellidos
        '
        Me.Apellidos.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Apellidos.DataPropertyName = "Apellidos"
        Me.Apellidos.HeaderText = "Apellidos"
        Me.Apellidos.MinimumWidth = 120
        Me.Apellidos.Name = "Apellidos"
        Me.Apellidos.ReadOnly = True
        Me.Apellidos.Width = 120
        '
        'Nombres
        '
        Me.Nombres.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Nombres.DataPropertyName = "Nombres"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Nombres.DefaultCellStyle = DataGridViewCellStyle14
        Me.Nombres.HeaderText = "Nombres"
        Me.Nombres.MinimumWidth = 110
        Me.Nombres.Name = "Nombres"
        Me.Nombres.ReadOnly = True
        Me.Nombres.Width = 110
        '
        'Ingreso
        '
        Me.Ingreso.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Ingreso.DataPropertyName = "Ingreso"
        Me.Ingreso.HeaderText = "Ingreso"
        Me.Ingreso.MinimumWidth = 70
        Me.Ingreso.Name = "Ingreso"
        Me.Ingreso.ReadOnly = True
        Me.Ingreso.Width = 70
        '
        'Baja
        '
        Me.Baja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Baja.DataPropertyName = "Baja"
        Me.Baja.HeaderText = "Baja"
        Me.Baja.MinimumWidth = 70
        Me.Baja.Name = "Baja"
        Me.Baja.ReadOnly = True
        Me.Baja.Width = 70
        '
        'Banco
        '
        Me.Banco.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Banco.DataPropertyName = "Banco"
        Me.Banco.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Banco.HeaderText = "Banco"
        Me.Banco.MinimumWidth = 70
        Me.Banco.Name = "Banco"
        Me.Banco.ReadOnly = True
        Me.Banco.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Banco.Width = 70
        '
        'Cuenta
        '
        Me.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuenta.DataPropertyName = "Cuenta"
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MinimumWidth = 70
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        Me.Cuenta.Width = 70
        '
        'Bruto
        '
        Me.Bruto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Bruto.DataPropertyName = "Bruto"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Bruto.DefaultCellStyle = DataGridViewCellStyle15
        Me.Bruto.HeaderText = "Bruto"
        Me.Bruto.MinimumWidth = 80
        Me.Bruto.Name = "Bruto"
        Me.Bruto.ReadOnly = True
        Me.Bruto.Width = 80
        '
        'BrutoN
        '
        Me.BrutoN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.BrutoN.DataPropertyName = "BrutoN"
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.BrutoN.DefaultCellStyle = DataGridViewCellStyle16
        Me.BrutoN.HeaderText = "Bruto(N)"
        Me.BrutoN.MinimumWidth = 80
        Me.BrutoN.Name = "BrutoN"
        Me.BrutoN.ReadOnly = True
        Me.BrutoN.Width = 80
        '
        'Estado
        '
        Me.Estado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle17.ForeColor = System.Drawing.Color.Red
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle17
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 70
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Estado.Width = 70
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(119, 641)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 146
        Me.ButtonUltimo.TabStop = False
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(85, 641)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 149
        Me.ButtonPosterior.TabStop = False
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(51, 641)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 148
        Me.ButtonAnterior.TabStop = False
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(17, 641)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 147
        Me.ButtonPrimero.TabStop = False
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Location = New System.Drawing.Point(16, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(887, 35)
        Me.Panel1.TabIndex = 151
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(561, 4)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 30)
        Me.CheckCerrado.TabIndex = 8
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(519, 4)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 30)
        Me.CheckAbierto.TabIndex = 7
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(686, 8)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(117, 22)
        Me.ButtonAceptar.TabIndex = 9
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(391, 642)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(151, 32)
        Me.ButtonExportarExcel.TabIndex = 164
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ListaEmpleados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(920, 676)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "ListaEmpleados"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Empleados"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Legajo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Apellidos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombres As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ingreso As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Baja As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Banco As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Bruto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BrutoN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
End Class
