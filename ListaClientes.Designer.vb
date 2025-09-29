<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaClientes
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckClientesDeFacturacion = New System.Windows.Forms.CheckBox
        Me.CheckClientesDeOperacion = New System.Windows.Forms.CheckBox
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboCliente = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.Clave = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.AliasW = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Calle = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Numero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Localidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Provincia = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Pais = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.DeOperacion = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.CanalVenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.CanalDistribucion = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Panel1.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckClientesDeFacturacion)
        Me.Panel1.Controls.Add(Me.CheckClientesDeOperacion)
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.ComboCliente)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Location = New System.Drawing.Point(9, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1119, 38)
        Me.Panel1.TabIndex = 98
        '
        'CheckClientesDeFacturacion
        '
        Me.CheckClientesDeFacturacion.BackColor = System.Drawing.Color.Transparent
        Me.CheckClientesDeFacturacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckClientesDeFacturacion.Checked = True
        Me.CheckClientesDeFacturacion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckClientesDeFacturacion.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckClientesDeFacturacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckClientesDeFacturacion.Location = New System.Drawing.Point(516, 3)
        Me.CheckClientesDeFacturacion.Name = "CheckClientesDeFacturacion"
        Me.CheckClientesDeFacturacion.Size = New System.Drawing.Size(96, 30)
        Me.CheckClientesDeFacturacion.TabIndex = 212
        Me.CheckClientesDeFacturacion.Text = "Clientes de Facturación"
        Me.CheckClientesDeFacturacion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckClientesDeFacturacion.UseVisualStyleBackColor = False
        '
        'CheckClientesDeOperacion
        '
        Me.CheckClientesDeOperacion.BackColor = System.Drawing.Color.Transparent
        Me.CheckClientesDeOperacion.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckClientesDeOperacion.Checked = True
        Me.CheckClientesDeOperacion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckClientesDeOperacion.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckClientesDeOperacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckClientesDeOperacion.Location = New System.Drawing.Point(619, 4)
        Me.CheckClientesDeOperacion.Name = "CheckClientesDeOperacion"
        Me.CheckClientesDeOperacion.Size = New System.Drawing.Size(78, 30)
        Me.CheckClientesDeOperacion.TabIndex = 211
        Me.CheckClientesDeOperacion.Text = "Clientes de Operación"
        Me.CheckClientesDeOperacion.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckClientesDeOperacion.UseVisualStyleBackColor = False
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(316, 6)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(174, 21)
        Me.ComboAlias.TabIndex = 210
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(281, 10)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(29, 13)
        Me.Label5.TabIndex = 209
        Me.Label5.Text = "Alias"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCliente
        '
        Me.ComboCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCliente.FormattingEnabled = True
        Me.ComboCliente.Location = New System.Drawing.Point(52, 5)
        Me.ComboCliente.Name = "ComboCliente"
        Me.ComboCliente.Size = New System.Drawing.Size(223, 21)
        Me.ComboCliente.TabIndex = 208
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(5, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 207
        Me.Label3.Text = "Cliente"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(923, 7)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(106, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(7, 646)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 108
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(40, 646)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 110
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(73, 646)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 111
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(107, 646)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 107
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LemonChiffon
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Clave, Me.Nombre, Me.AliasW, Me.Cuit, Me.Calle, Me.Numero, Me.Localidad, Me.Provincia, Me.Pais, Me.DeOperacion, Me.CanalVenta, Me.CanalDistribucion})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.EnableHeadersVisualStyles = False
        Me.Grid.Location = New System.Drawing.Point(7, 47)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.RowHeadersWidth = 30
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Grid.Size = New System.Drawing.Size(1121, 595)
        Me.Grid.TabIndex = 106
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(492, 647)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(151, 32)
        Me.ButtonExportarExcel.TabIndex = 160
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'Clave
        '
        Me.Clave.DataPropertyName = "Clave"
        Me.Clave.HeaderText = "Clave"
        Me.Clave.Name = "Clave"
        Me.Clave.ReadOnly = True
        Me.Clave.Visible = False
        '
        'Nombre
        '
        Me.Nombre.DataPropertyName = "Nombre"
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nombre.DefaultCellStyle = DataGridViewCellStyle2
        Me.Nombre.HeaderText = "Razon Social"
        Me.Nombre.MinimumWidth = 190
        Me.Nombre.Name = "Nombre"
        Me.Nombre.ReadOnly = True
        Me.Nombre.Width = 190
        '
        'AliasW
        '
        Me.AliasW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.AliasW.DataPropertyName = "Alias"
        Me.AliasW.HeaderText = "Alias"
        Me.AliasW.MinimumWidth = 90
        Me.AliasW.Name = "AliasW"
        Me.AliasW.ReadOnly = True
        Me.AliasW.Width = 90
        '
        'Cuit
        '
        Me.Cuit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.Cuit.DataPropertyName = "Cuit"
        Me.Cuit.HeaderText = "Cuit"
        Me.Cuit.MinimumWidth = 80
        Me.Cuit.Name = "Cuit"
        Me.Cuit.ReadOnly = True
        Me.Cuit.Width = 80
        '
        'Calle
        '
        Me.Calle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Calle.DataPropertyName = "Calle"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Calle.DefaultCellStyle = DataGridViewCellStyle3
        Me.Calle.HeaderText = "Calle"
        Me.Calle.MinimumWidth = 130
        Me.Calle.Name = "Calle"
        Me.Calle.ReadOnly = True
        Me.Calle.Width = 130
        '
        'Numero
        '
        Me.Numero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Numero.DataPropertyName = "Numero"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Numero.DefaultCellStyle = DataGridViewCellStyle4
        Me.Numero.HeaderText = "Nro."
        Me.Numero.MinimumWidth = 60
        Me.Numero.Name = "Numero"
        Me.Numero.ReadOnly = True
        Me.Numero.Width = 60
        '
        'Localidad
        '
        Me.Localidad.DataPropertyName = "Localidad"
        Me.Localidad.HeaderText = "Localidad"
        Me.Localidad.MinimumWidth = 100
        Me.Localidad.Name = "Localidad"
        Me.Localidad.ReadOnly = True
        '
        'Provincia
        '
        Me.Provincia.DataPropertyName = "Provincia"
        Me.Provincia.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Provincia.HeaderText = "Provincia"
        Me.Provincia.MinimumWidth = 90
        Me.Provincia.Name = "Provincia"
        Me.Provincia.ReadOnly = True
        Me.Provincia.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Provincia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Provincia.Width = 90
        '
        'Pais
        '
        Me.Pais.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Pais.DataPropertyName = "Pais"
        Me.Pais.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Pais.HeaderText = "Pais"
        Me.Pais.MinimumWidth = 80
        Me.Pais.Name = "Pais"
        Me.Pais.ReadOnly = True
        Me.Pais.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Pais.Width = 80
        '
        'DeOperacion
        '
        Me.DeOperacion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DeOperacion.DataPropertyName = "DeOperacion"
        Me.DeOperacion.HeaderText = "De Operación"
        Me.DeOperacion.MinimumWidth = 60
        Me.DeOperacion.Name = "DeOperacion"
        Me.DeOperacion.ReadOnly = True
        Me.DeOperacion.Width = 60
        '
        'CanalVenta
        '
        Me.CanalVenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CanalVenta.DataPropertyName = "CanalVenta"
        Me.CanalVenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.CanalVenta.HeaderText = "Canal De Venta"
        Me.CanalVenta.Name = "CanalVenta"
        Me.CanalVenta.ReadOnly = True
        Me.CanalVenta.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CanalVenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'CanalDistribucion
        '
        Me.CanalDistribucion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CanalDistribucion.DataPropertyName = "CanalDistribucion"
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
        Me.CanalDistribucion.DefaultCellStyle = DataGridViewCellStyle5
        Me.CanalDistribucion.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.CanalDistribucion.HeaderText = "Canal de Distribución"
        Me.CanalDistribucion.MinimumWidth = 90
        Me.CanalDistribucion.Name = "CanalDistribucion"
        Me.CanalDistribucion.ReadOnly = True
        Me.CanalDistribucion.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.CanalDistribucion.Width = 90
        '
        'ListaClientes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ClientSize = New System.Drawing.Size(1135, 683)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Panel1)
        Me.KeyPreview = True
        Me.Name = "ListaClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Listado Clientes."
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboCliente As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents CheckClientesDeFacturacion As System.Windows.Forms.CheckBox
    Friend WithEvents CheckClientesDeOperacion As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents Clave As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AliasW As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Calle As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Numero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Localidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Provincia As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Pais As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents DeOperacion As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents CanalVenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CanalDistribucion As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
