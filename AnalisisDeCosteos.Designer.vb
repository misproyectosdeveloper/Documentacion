<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AnalisisDeCosteos
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
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Negocio = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Costeo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Kilos = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaDesde = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaHasta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GastosConIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GastosSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoEstConIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoEstSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoRealConIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoRealSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cerrado = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.RadioImporteTotal = New System.Windows.Forms.RadioButton
        Me.RadioImporteSinIva = New System.Windows.Forms.RadioButton
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextTotalGastos = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(13, 650)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 151
        Me.ButtonPrimero.TabStop = False
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(47, 650)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonAnterior.TabIndex = 152
        Me.ButtonAnterior.TabStop = False
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(79, 650)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonPosterior.TabIndex = 149
        Me.ButtonPosterior.TabStop = False
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(111, 650)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 150
        Me.ButtonUltimo.TabStop = False
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LemonChiffon
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Negocio, Me.Costeo, Me.Kilos, Me.FechaDesde, Me.FechaHasta, Me.GastosConIva, Me.GastosSinIva, Me.CostoEstConIva, Me.CostoEstSinIva, Me.CostoRealConIva, Me.CostoRealSinIva, Me.Cerrado})
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle13
        Me.Grid.Location = New System.Drawing.Point(16, 64)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        Me.Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle14.BackColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle14.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        DataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle14
        Me.Grid.RowHeadersWidth = 30
        DataGridViewCellStyle15.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle15
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(995, 578)
        Me.Grid.TabIndex = 3
        '
        'Negocio
        '
        Me.Negocio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Negocio.DataPropertyName = "Negocio"
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Negocio.DefaultCellStyle = DataGridViewCellStyle2
        Me.Negocio.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Negocio.HeaderText = "Negocio"
        Me.Negocio.MinimumWidth = 150
        Me.Negocio.Name = "Negocio"
        Me.Negocio.ReadOnly = True
        Me.Negocio.Width = 150
        '
        'Costeo
        '
        Me.Costeo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Costeo.DataPropertyName = "Costeo"
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Costeo.DefaultCellStyle = DataGridViewCellStyle3
        Me.Costeo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Costeo.HeaderText = "Costeo"
        Me.Costeo.MinimumWidth = 150
        Me.Costeo.Name = "Costeo"
        Me.Costeo.ReadOnly = True
        Me.Costeo.Width = 150
        '
        'Kilos
        '
        Me.Kilos.DataPropertyName = "Kilos"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Kilos.DefaultCellStyle = DataGridViewCellStyle4
        Me.Kilos.HeaderText = "Kilos"
        Me.Kilos.MinimumWidth = 80
        Me.Kilos.Name = "Kilos"
        Me.Kilos.ReadOnly = True
        Me.Kilos.Width = 80
        '
        'FechaDesde
        '
        Me.FechaDesde.DataPropertyName = "FechaDesde"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaDesde.DefaultCellStyle = DataGridViewCellStyle5
        Me.FechaDesde.HeaderText = "Fecha Inicio"
        Me.FechaDesde.MinimumWidth = 90
        Me.FechaDesde.Name = "FechaDesde"
        Me.FechaDesde.ReadOnly = True
        Me.FechaDesde.Width = 90
        '
        'FechaHasta
        '
        Me.FechaHasta.DataPropertyName = "FechaHasta"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaHasta.DefaultCellStyle = DataGridViewCellStyle6
        Me.FechaHasta.HeaderText = "Fecha Final"
        Me.FechaHasta.MinimumWidth = 90
        Me.FechaHasta.Name = "FechaHasta"
        Me.FechaHasta.ReadOnly = True
        Me.FechaHasta.Width = 90
        '
        'GastosConIva
        '
        Me.GastosConIva.DataPropertyName = "GastosConIva"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.GastosConIva.DefaultCellStyle = DataGridViewCellStyle7
        Me.GastosConIva.HeaderText = "Gastos "
        Me.GastosConIva.MinimumWidth = 100
        Me.GastosConIva.Name = "GastosConIva"
        Me.GastosConIva.ReadOnly = True
        '
        'GastosSinIva
        '
        Me.GastosSinIva.DataPropertyName = "GastosSinIva"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.GastosSinIva.DefaultCellStyle = DataGridViewCellStyle8
        Me.GastosSinIva.HeaderText = "Gastos "
        Me.GastosSinIva.MinimumWidth = 100
        Me.GastosSinIva.Name = "GastosSinIva"
        Me.GastosSinIva.ReadOnly = True
        Me.GastosSinIva.Visible = False
        '
        'CostoEstConIva
        '
        Me.CostoEstConIva.DataPropertyName = "CostoEstConIva"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoEstConIva.DefaultCellStyle = DataGridViewCellStyle9
        Me.CostoEstConIva.HeaderText = "Costo Est."
        Me.CostoEstConIva.MinimumWidth = 100
        Me.CostoEstConIva.Name = "CostoEstConIva"
        Me.CostoEstConIva.ReadOnly = True
        '
        'CostoEstSinIva
        '
        Me.CostoEstSinIva.DataPropertyName = "CostoEstSinIva"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoEstSinIva.DefaultCellStyle = DataGridViewCellStyle10
        Me.CostoEstSinIva.HeaderText = "Costo Est."
        Me.CostoEstSinIva.MinimumWidth = 100
        Me.CostoEstSinIva.Name = "CostoEstSinIva"
        Me.CostoEstSinIva.ReadOnly = True
        Me.CostoEstSinIva.Visible = False
        '
        'CostoRealConIva
        '
        Me.CostoRealConIva.DataPropertyName = "CostoRealConIva"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoRealConIva.DefaultCellStyle = DataGridViewCellStyle11
        Me.CostoRealConIva.HeaderText = "Costo Real"
        Me.CostoRealConIva.MinimumWidth = 120
        Me.CostoRealConIva.Name = "CostoRealConIva"
        Me.CostoRealConIva.ReadOnly = True
        Me.CostoRealConIva.Width = 120
        '
        'CostoRealSinIva
        '
        Me.CostoRealSinIva.DataPropertyName = "CostoRealSinIva"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoRealSinIva.DefaultCellStyle = DataGridViewCellStyle12
        Me.CostoRealSinIva.HeaderText = "Costo Real"
        Me.CostoRealSinIva.MinimumWidth = 120
        Me.CostoRealSinIva.Name = "CostoRealSinIva"
        Me.CostoRealSinIva.ReadOnly = True
        Me.CostoRealSinIva.Visible = False
        Me.CostoRealSinIva.Width = 120
        '
        'Cerrado
        '
        Me.Cerrado.DataPropertyName = "Cerrado"
        Me.Cerrado.HeaderText = "Cerrado"
        Me.Cerrado.MinimumWidth = 50
        Me.Cerrado.Name = "Cerrado"
        Me.Cerrado.ReadOnly = True
        Me.Cerrado.Width = 50
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.ComboCosteo)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.ComboNegocio)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Location = New System.Drawing.Point(17, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(994, 60)
        Me.Panel1.TabIndex = 1
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Transparent
        Me.Panel4.Controls.Add(Me.RadioImporteTotal)
        Me.Panel4.Controls.Add(Me.RadioImporteSinIva)
        Me.Panel4.Location = New System.Drawing.Point(509, 29)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(212, 25)
        Me.Panel4.TabIndex = 165
        '
        'RadioImporteTotal
        '
        Me.RadioImporteTotal.AutoSize = True
        Me.RadioImporteTotal.Checked = True
        Me.RadioImporteTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioImporteTotal.ForeColor = System.Drawing.Color.Red
        Me.RadioImporteTotal.Location = New System.Drawing.Point(5, 4)
        Me.RadioImporteTotal.Name = "RadioImporteTotal"
        Me.RadioImporteTotal.Size = New System.Drawing.Size(87, 17)
        Me.RadioImporteTotal.TabIndex = 3
        Me.RadioImporteTotal.TabStop = True
        Me.RadioImporteTotal.Text = "Importe Total"
        Me.RadioImporteTotal.UseVisualStyleBackColor = True
        '
        'RadioImporteSinIva
        '
        Me.RadioImporteSinIva.AutoSize = True
        Me.RadioImporteSinIva.BackColor = System.Drawing.Color.Transparent
        Me.RadioImporteSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioImporteSinIva.Location = New System.Drawing.Point(108, 4)
        Me.RadioImporteSinIva.Name = "RadioImporteSinIva"
        Me.RadioImporteSinIva.Size = New System.Drawing.Size(96, 17)
        Me.RadioImporteSinIva.TabIndex = 0
        Me.RadioImporteSinIva.Text = "Importe Sin Iva"
        Me.RadioImporteSinIva.UseVisualStyleBackColor = False
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(330, 2)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(154, 21)
        Me.ComboCosteo.TabIndex = 163
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(282, 5)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 15)
        Me.Label5.TabIndex = 164
        Me.Label5.Text = "Costeo"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(102, 3)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(168, 21)
        Me.ComboNegocio.TabIndex = 161
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(2, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(95, 15)
        Me.Label8.TabIndex = 162
        Me.Label8.Text = "Uni. de Negocio"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(850, 32)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(117, 22)
        Me.ButtonAceptar.TabIndex = 5
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(741, 7)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 15)
        Me.Label7.TabIndex = 125
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(499, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(118, 15)
        Me.Label2.TabIndex = 124
        Me.Label2.Text = "Fecha Inicio:  Desde"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(784, 5)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(105, 20)
        Me.DateTimeHasta.TabIndex = 3
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(625, 4)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(112, 20)
        Me.DateTimeDesde.TabIndex = 2
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.TextTotalGastos)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Location = New System.Drawing.Point(725, 647)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(248, 30)
        Me.Panel3.TabIndex = 163
        '
        'TextTotalGastos
        '
        Me.TextTotalGastos.BackColor = System.Drawing.Color.Thistle
        Me.TextTotalGastos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalGastos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalGastos.Location = New System.Drawing.Point(131, 4)
        Me.TextTotalGastos.MaxLength = 9
        Me.TextTotalGastos.Name = "TextTotalGastos"
        Me.TextTotalGastos.Size = New System.Drawing.Size(114, 20)
        Me.TextTotalGastos.TabIndex = 128
        Me.TextTotalGastos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(3, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(124, 15)
        Me.Label4.TabIndex = 121
        Me.Label4.Text = "Total Gastos C/IVA"
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(439, 647)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(191, 35)
        Me.ButtonExportarExcel.TabIndex = 164
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'AnalisisDeCosteos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1032, 685)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.KeyPreview = True
        Me.Name = "AnalisisDeCosteos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Análisis De Costeos"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextTotalGastos As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents RadioImporteTotal As System.Windows.Forms.RadioButton
    Friend WithEvents RadioImporteSinIva As System.Windows.Forms.RadioButton
    Friend WithEvents CostoEst As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents Negocio As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Costeo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Kilos As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaDesde As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaHasta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GastosConIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GastosSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoEstConIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoEstSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoRealConIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoRealSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cerrado As System.Windows.Forms.DataGridViewCheckBoxColumn
End Class
