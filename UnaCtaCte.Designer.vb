<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaCtaCte
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
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle22 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle23 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle24 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaCtaCte))
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TipoOrigen = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Emisor1 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Tipo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Mensaje = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ReciboOficial = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Debito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Credito = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SaldoCta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.CanalVenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.CanalDistribucion = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.LabelCanalVenta = New System.Windows.Forms.Label
        Me.ComboBoxCanalVenta = New System.Windows.Forms.ComboBox
        Me.CheckYaAsignados = New System.Windows.Forms.CheckBox
        Me.CheckSoloExterior = New System.Windows.Forms.CheckBox
        Me.CheckSoloDomesticos = New System.Windows.Forms.CheckBox
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.CheckSinRemitos = New System.Windows.Forms.CheckBox
        Me.CheckSinAsignar = New System.Windows.Forms.CheckBox
        Me.CheckConAnuladas = New System.Windows.Forms.CheckBox
        Me.PanelMoneda = New System.Windows.Forms.Panel
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.CheckDetalle = New System.Windows.Forms.CheckBox
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.CheckConBalanceo = New System.Windows.Forms.CheckBox
        Me.CheckSinBalanceo = New System.Windows.Forms.CheckBox
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextSaldoCta = New System.Windows.Forms.TextBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.LabelClienteProveedor = New System.Windows.Forms.Label
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.ButtonSoloTotales = New System.Windows.Forms.Button
        Me.ButtonNoImputados = New System.Windows.Forms.Button
        Me.ButtonCompQueLoImputan = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.CheckIncluirConSaldoCero = New System.Windows.Forms.CheckBox
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.PanelMoneda.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.LemonChiffon
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.TipoOrigen, Me.Candado, Me.Emisor1, Me.Fecha, Me.Tipo, Me.Mensaje, Me.Comprobante, Me.ReciboOficial, Me.Comentario, Me.Saldo, Me.Debito, Me.Credito, Me.SaldoCta, Me.Estado, Me.CanalVenta, Me.CanalDistribucion})
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle28.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle28.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle28.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle28.SelectionBackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle28.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle28.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle28
        Me.Grid.Location = New System.Drawing.Point(7, 91)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        Me.Grid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle29.BackColor = System.Drawing.Color.WhiteSmoke
        DataGridViewCellStyle29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle29.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle29.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        DataGridViewCellStyle29.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle29
        Me.Grid.RowHeadersWidth = 30
        DataGridViewCellStyle30.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle30.SelectionBackColor = System.Drawing.Color.LightSeaGreen
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle30
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1183, 526)
        Me.Grid.TabIndex = 129
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        Me.Operacion.Width = 81
        '
        'TipoOrigen
        '
        Me.TipoOrigen.DataPropertyName = "TipoOrigen"
        Me.TipoOrigen.HeaderText = "TipoOrigen"
        Me.TipoOrigen.Name = "TipoOrigen"
        Me.TipoOrigen.ReadOnly = True
        Me.TipoOrigen.Visible = False
        Me.TipoOrigen.Width = 84
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
        'Emisor1
        '
        Me.Emisor1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Emisor1.DataPropertyName = "Emisor"
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Emisor1.DefaultCellStyle = DataGridViewCellStyle17
        Me.Emisor1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Emisor1.HeaderText = ""
        Me.Emisor1.MinimumWidth = 150
        Me.Emisor1.Name = "Emisor1"
        Me.Emisor1.ReadOnly = True
        Me.Emisor1.Width = 150
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle18
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'Tipo
        '
        Me.Tipo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Tipo.DataPropertyName = "Tipo"
        Me.Tipo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Tipo.HeaderText = ""
        Me.Tipo.MinimumWidth = 110
        Me.Tipo.Name = "Tipo"
        Me.Tipo.ReadOnly = True
        Me.Tipo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Tipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Tipo.Width = 110
        '
        'Mensaje
        '
        Me.Mensaje.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Mensaje.DataPropertyName = "Mensaje"
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle19.ForeColor = System.Drawing.Color.OliveDrab
        Me.Mensaje.DefaultCellStyle = DataGridViewCellStyle19
        Me.Mensaje.HeaderText = ""
        Me.Mensaje.MinimumWidth = 60
        Me.Mensaje.Name = "Mensaje"
        Me.Mensaje.ReadOnly = True
        Me.Mensaje.Width = 60
        '
        'Comprobante
        '
        Me.Comprobante.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comprobante.DataPropertyName = "Comprobante"
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Comprobante.DefaultCellStyle = DataGridViewCellStyle20
        Me.Comprobante.HeaderText = "Comprobante"
        Me.Comprobante.MinimumWidth = 90
        Me.Comprobante.Name = "Comprobante"
        Me.Comprobante.ReadOnly = True
        Me.Comprobante.Width = 90
        '
        'ReciboOficial
        '
        Me.ReciboOficial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ReciboOficial.DataPropertyName = "ReciboOficial"
        DataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ReciboOficial.DefaultCellStyle = DataGridViewCellStyle21
        Me.ReciboOficial.HeaderText = "Recibo Oficial"
        Me.ReciboOficial.MinimumWidth = 98
        Me.ReciboOficial.Name = "ReciboOficial"
        Me.ReciboOficial.ReadOnly = True
        Me.ReciboOficial.Width = 98
        '
        'Comentario
        '
        Me.Comentario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comentario.DataPropertyName = "Comentario"
        DataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Comentario.DefaultCellStyle = DataGridViewCellStyle22
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.Name = "Comentario"
        Me.Comentario.ReadOnly = True
        Me.Comentario.Width = 50
        '
        'Saldo
        '
        Me.Saldo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle23
        Me.Saldo.HeaderText = "Sin Asignar"
        Me.Saldo.MinimumWidth = 100
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        '
        'Debito
        '
        Me.Debito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Debito.DataPropertyName = "Debito"
        DataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Debito.DefaultCellStyle = DataGridViewCellStyle24
        Me.Debito.HeaderText = "Debito"
        Me.Debito.MinimumWidth = 100
        Me.Debito.Name = "Debito"
        Me.Debito.ReadOnly = True
        '
        'Credito
        '
        Me.Credito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Credito.DataPropertyName = "Credito"
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Credito.DefaultCellStyle = DataGridViewCellStyle25
        Me.Credito.HeaderText = "Credito"
        Me.Credito.MinimumWidth = 100
        Me.Credito.Name = "Credito"
        Me.Credito.ReadOnly = True
        '
        'SaldoCta
        '
        Me.SaldoCta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.SaldoCta.DataPropertyName = "SaldoCta"
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle26.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaldoCta.DefaultCellStyle = DataGridViewCellStyle26
        Me.SaldoCta.HeaderText = "Saldo "
        Me.SaldoCta.MinimumWidth = 100
        Me.SaldoCta.Name = "SaldoCta"
        Me.SaldoCta.ReadOnly = True
        '
        'Estado
        '
        Me.Estado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle27.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle27.ForeColor = System.Drawing.Color.Red
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle27
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 70
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.Width = 70
        '
        'CanalVenta
        '
        Me.CanalVenta.DataPropertyName = "CanalVenta"
        Me.CanalVenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.CanalVenta.HeaderText = "CanalVenta"
        Me.CanalVenta.Name = "CanalVenta"
        Me.CanalVenta.ReadOnly = True
        Me.CanalVenta.Visible = False
        Me.CanalVenta.Width = 68
        '
        'CanalDistribucion
        '
        Me.CanalDistribucion.DataPropertyName = "CanalDistribucion"
        Me.CanalDistribucion.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.CanalDistribucion.HeaderText = "CanalDistribucion"
        Me.CanalDistribucion.Name = "CanalDistribucion"
        Me.CanalDistribucion.ReadOnly = True
        Me.CanalDistribucion.Visible = False
        Me.CanalDistribucion.Width = 95
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckIncluirConSaldoCero)
        Me.Panel1.Controls.Add(Me.LabelCanalVenta)
        Me.Panel1.Controls.Add(Me.ComboBoxCanalVenta)
        Me.Panel1.Controls.Add(Me.CheckYaAsignados)
        Me.Panel1.Controls.Add(Me.CheckSoloExterior)
        Me.Panel1.Controls.Add(Me.CheckSoloDomesticos)
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.CheckSinRemitos)
        Me.Panel1.Controls.Add(Me.CheckSinAsignar)
        Me.Panel1.Controls.Add(Me.CheckConAnuladas)
        Me.Panel1.Controls.Add(Me.PanelMoneda)
        Me.Panel1.Controls.Add(Me.CheckDetalle)
        Me.Panel1.Controls.Add(Me.ComboEmisor)
        Me.Panel1.Controls.Add(Me.CheckConBalanceo)
        Me.Panel1.Controls.Add(Me.CheckSinBalanceo)
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.TextSaldoCta)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.LabelClienteProveedor)
        Me.Panel1.Location = New System.Drawing.Point(8, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1182, 84)
        Me.Panel1.TabIndex = 132
        '
        'LabelCanalVenta
        '
        Me.LabelCanalVenta.AutoSize = True
        Me.LabelCanalVenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCanalVenta.Location = New System.Drawing.Point(6, 59)
        Me.LabelCanalVenta.Name = "LabelCanalVenta"
        Me.LabelCanalVenta.Size = New System.Drawing.Size(116, 16)
        Me.LabelCanalVenta.TabIndex = 1043
        Me.LabelCanalVenta.Text = "Canal De Venta"
        Me.LabelCanalVenta.Visible = False
        '
        'ComboBoxCanalVenta
        '
        Me.ComboBoxCanalVenta.FormattingEnabled = True
        Me.ComboBoxCanalVenta.Location = New System.Drawing.Point(128, 58)
        Me.ComboBoxCanalVenta.Name = "ComboBoxCanalVenta"
        Me.ComboBoxCanalVenta.Size = New System.Drawing.Size(177, 21)
        Me.ComboBoxCanalVenta.TabIndex = 1042
        Me.ComboBoxCanalVenta.Visible = False
        '
        'CheckYaAsignados
        '
        Me.CheckYaAsignados.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckYaAsignados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckYaAsignados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckYaAsignados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckYaAsignados.Location = New System.Drawing.Point(565, 57)
        Me.CheckYaAsignados.Name = "CheckYaAsignados"
        Me.CheckYaAsignados.Size = New System.Drawing.Size(122, 22)
        Me.CheckYaAsignados.TabIndex = 1040
        Me.CheckYaAsignados.Text = "Solo Ya Asignados"
        Me.CheckYaAsignados.UseVisualStyleBackColor = False
        '
        'CheckSoloExterior
        '
        Me.CheckSoloExterior.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSoloExterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSoloExterior.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSoloExterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSoloExterior.Location = New System.Drawing.Point(777, 40)
        Me.CheckSoloExterior.Name = "CheckSoloExterior"
        Me.CheckSoloExterior.Size = New System.Drawing.Size(106, 15)
        Me.CheckSoloExterior.TabIndex = 1039
        Me.CheckSoloExterior.Text = "Solo Del Exterior"
        Me.CheckSoloExterior.UseVisualStyleBackColor = False
        '
        'CheckSoloDomesticos
        '
        Me.CheckSoloDomesticos.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSoloDomesticos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSoloDomesticos.Checked = True
        Me.CheckSoloDomesticos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckSoloDomesticos.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSoloDomesticos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSoloDomesticos.Location = New System.Drawing.Point(665, 40)
        Me.CheckSoloDomesticos.Name = "CheckSoloDomesticos"
        Me.CheckSoloDomesticos.Size = New System.Drawing.Size(106, 15)
        Me.CheckSoloDomesticos.TabIndex = 1038
        Me.CheckSoloDomesticos.Text = "Solo Domésticos"
        Me.CheckSoloDomesticos.UseVisualStyleBackColor = False
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(86, 30)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(138, 21)
        Me.ComboAlias.TabIndex = 1037
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(43, 16)
        Me.Label3.TabIndex = 1036
        Me.Label3.Text = "Alias"
        '
        'CheckSinRemitos
        '
        Me.CheckSinRemitos.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSinRemitos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinRemitos.Checked = True
        Me.CheckSinRemitos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckSinRemitos.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinRemitos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinRemitos.Location = New System.Drawing.Point(892, 40)
        Me.CheckSinRemitos.Name = "CheckSinRemitos"
        Me.CheckSinRemitos.Size = New System.Drawing.Size(138, 20)
        Me.CheckSinRemitos.TabIndex = 1033
        Me.CheckSinRemitos.Text = "Sin Remitos Valorizados"
        Me.CheckSinRemitos.UseVisualStyleBackColor = False
        '
        'CheckSinAsignar
        '
        Me.CheckSinAsignar.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSinAsignar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinAsignar.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinAsignar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinAsignar.Location = New System.Drawing.Point(457, 57)
        Me.CheckSinAsignar.Name = "CheckSinAsignar"
        Me.CheckSinAsignar.Size = New System.Drawing.Size(106, 22)
        Me.CheckSinAsignar.TabIndex = 1032
        Me.CheckSinAsignar.Text = "Solo Sin Asignar"
        Me.CheckSinAsignar.UseVisualStyleBackColor = False
        '
        'CheckConAnuladas
        '
        Me.CheckConAnuladas.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckConAnuladas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConAnuladas.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConAnuladas.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConAnuladas.Location = New System.Drawing.Point(892, 53)
        Me.CheckConAnuladas.Name = "CheckConAnuladas"
        Me.CheckConAnuladas.Size = New System.Drawing.Size(93, 30)
        Me.CheckConAnuladas.TabIndex = 1031
        Me.CheckConAnuladas.Text = "Con Anuladas"
        Me.CheckConAnuladas.UseVisualStyleBackColor = False
        '
        'PanelMoneda
        '
        Me.PanelMoneda.BackColor = System.Drawing.Color.Plum
        Me.PanelMoneda.Controls.Add(Me.ComboMoneda)
        Me.PanelMoneda.Controls.Add(Me.Label1)
        Me.PanelMoneda.Location = New System.Drawing.Point(448, 4)
        Me.PanelMoneda.Name = "PanelMoneda"
        Me.PanelMoneda.Size = New System.Drawing.Size(202, 30)
        Me.PanelMoneda.TabIndex = 1030
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.ForeColor = System.Drawing.Color.Black
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(79, 3)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(115, 23)
        Me.ComboMoneda.TabIndex = 144
        Me.ComboMoneda.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(0, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 15)
        Me.Label1.TabIndex = 143
        Me.Label1.Text = "Importes  en"
        '
        'CheckDetalle
        '
        Me.CheckDetalle.BackColor = System.Drawing.Color.Transparent
        Me.CheckDetalle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckDetalle.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckDetalle.ForeColor = System.Drawing.Color.Purple
        Me.CheckDetalle.Location = New System.Drawing.Point(313, 1)
        Me.CheckDetalle.Name = "CheckDetalle"
        Me.CheckDetalle.Size = New System.Drawing.Size(131, 30)
        Me.CheckDetalle.TabIndex = 1029
        Me.CheckDetalle.Text = "Vista Detalle"
        Me.CheckDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckDetalle.UseVisualStyleBackColor = False
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(86, 3)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(216, 21)
        Me.ComboEmisor.TabIndex = 271
        '
        'CheckConBalanceo
        '
        Me.CheckConBalanceo.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckConBalanceo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConBalanceo.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConBalanceo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConBalanceo.Location = New System.Drawing.Point(791, 53)
        Me.CheckConBalanceo.Name = "CheckConBalanceo"
        Me.CheckConBalanceo.Size = New System.Drawing.Size(107, 30)
        Me.CheckConBalanceo.TabIndex = 270
        Me.CheckConBalanceo.Text = "Solo Contables"
        Me.CheckConBalanceo.UseVisualStyleBackColor = False
        '
        'CheckSinBalanceo
        '
        Me.CheckSinBalanceo.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSinBalanceo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinBalanceo.Checked = True
        Me.CheckSinBalanceo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckSinBalanceo.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinBalanceo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinBalanceo.Location = New System.Drawing.Point(693, 53)
        Me.CheckSinBalanceo.Name = "CheckSinBalanceo"
        Me.CheckSinBalanceo.Size = New System.Drawing.Size(90, 30)
        Me.CheckSinBalanceo.TabIndex = 269
        Me.CheckSinBalanceo.Text = "Sin Contables"
        Me.CheckSinBalanceo.UseVisualStyleBackColor = False
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = Global.ScomerV01.My.Resources.Resources.Ccerrado
        Me.CheckCerrado.Location = New System.Drawing.Point(1074, 7)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 30)
        Me.CheckCerrado.TabIndex = 268
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = Global.ScomerV01.My.Resources.Resources.CAbierto
        Me.CheckAbierto.Location = New System.Drawing.Point(1032, 7)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 30)
        Me.CheckAbierto.TabIndex = 267
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(229, 36)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 13)
        Me.Label4.TabIndex = 128
        Me.Label4.Text = "Total Saldo"
        '
        'TextSaldoCta
        '
        Me.TextSaldoCta.BackColor = System.Drawing.Color.White
        Me.TextSaldoCta.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextSaldoCta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldoCta.Location = New System.Drawing.Point(309, 33)
        Me.TextSaldoCta.MaxLength = 10
        Me.TextSaldoCta.Name = "TextSaldoCta"
        Me.TextSaldoCta.ReadOnly = True
        Me.TextSaldoCta.Size = New System.Drawing.Size(130, 20)
        Me.TextSaldoCta.TabIndex = 0
        Me.TextSaldoCta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1049, 51)
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
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(875, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 125
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(670, 16)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 13)
        Me.Label2.TabIndex = 124
        Me.Label2.Text = "F.Contable Desde"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(915, 13)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(96, 20)
        Me.DateTimeHasta.TabIndex = 3
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(767, 13)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(98, 20)
        Me.DateTimeDesde.TabIndex = 2
        '
        'LabelClienteProveedor
        '
        Me.LabelClienteProveedor.AutoSize = True
        Me.LabelClienteProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelClienteProveedor.Location = New System.Drawing.Point(2, 7)
        Me.LabelClienteProveedor.Name = "LabelClienteProveedor"
        Me.LabelClienteProveedor.Size = New System.Drawing.Size(81, 16)
        Me.LabelClienteProveedor.TabIndex = 120
        Me.LabelClienteProveedor.Text = "Proveedor"
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(8, 625)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 134
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(41, 625)
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(73, 625)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 22)
        Me.ButtonPosterior.TabIndex = 136
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(105, 625)
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
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(350, 626)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(154, 35)
        Me.ButtonExportarExcel.TabIndex = 148
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ButtonSoloTotales
        '
        Me.ButtonSoloTotales.BackColor = System.Drawing.Color.Transparent
        Me.ButtonSoloTotales.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonSoloTotales.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonSoloTotales.Location = New System.Drawing.Point(160, 626)
        Me.ButtonSoloTotales.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonSoloTotales.Name = "ButtonSoloTotales"
        Me.ButtonSoloTotales.Size = New System.Drawing.Size(154, 35)
        Me.ButtonSoloTotales.TabIndex = 149
        Me.ButtonSoloTotales.Text = "Muestra Solo Totales"
        Me.ButtonSoloTotales.UseVisualStyleBackColor = False
        '
        'ButtonNoImputados
        '
        Me.ButtonNoImputados.AutoEllipsis = True
        Me.ButtonNoImputados.BackColor = System.Drawing.Color.Transparent
        Me.ButtonNoImputados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonNoImputados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNoImputados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNoImputados.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNoImputados.Location = New System.Drawing.Point(538, 626)
        Me.ButtonNoImputados.Name = "ButtonNoImputados"
        Me.ButtonNoImputados.Size = New System.Drawing.Size(154, 35)
        Me.ButtonNoImputados.TabIndex = 150
        Me.ButtonNoImputados.Text = "Comp. No Imputados"
        Me.ButtonNoImputados.UseVisualStyleBackColor = False
        '
        'ButtonCompQueLoImputan
        '
        Me.ButtonCompQueLoImputan.AutoEllipsis = True
        Me.ButtonCompQueLoImputan.BackColor = System.Drawing.Color.Transparent
        Me.ButtonCompQueLoImputan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonCompQueLoImputan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCompQueLoImputan.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCompQueLoImputan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonCompQueLoImputan.Location = New System.Drawing.Point(725, 626)
        Me.ButtonCompQueLoImputan.Name = "ButtonCompQueLoImputan"
        Me.ButtonCompQueLoImputan.Size = New System.Drawing.Size(161, 35)
        Me.ButtonCompQueLoImputan.TabIndex = 152
        Me.ButtonCompQueLoImputan.Text = "Comp. Que lo Imputan"
        Me.ButtonCompQueLoImputan.UseVisualStyleBackColor = False
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(924, 625)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(264, 35)
        Me.ButtonImprimir.TabIndex = 1014
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime Con Detalle Factura/Remito"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'CheckIncluirConSaldoCero
        '
        Me.CheckIncluirConSaldoCero.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckIncluirConSaldoCero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckIncluirConSaldoCero.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckIncluirConSaldoCero.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckIncluirConSaldoCero.Location = New System.Drawing.Point(321, 59)
        Me.CheckIncluirConSaldoCero.Name = "CheckIncluirConSaldoCero"
        Me.CheckIncluirConSaldoCero.Size = New System.Drawing.Size(130, 22)
        Me.CheckIncluirConSaldoCero.TabIndex = 1044
        Me.CheckIncluirConSaldoCero.Text = "Incluir con Saldo Cero"
        Me.CheckIncluirConSaldoCero.UseVisualStyleBackColor = False
        '
        'UnaCtaCte
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LemonChiffon
        Me.ClientSize = New System.Drawing.Size(1199, 676)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonCompQueLoImputan)
        Me.Controls.Add(Me.ButtonNoImputados)
        Me.Controls.Add(Me.ButtonSoloTotales)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "UnaCtaCte"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cuenta Corriente."
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelMoneda.ResumeLayout(False)
        Me.PanelMoneda.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents LabelClienteProveedor As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextSaldoCta As System.Windows.Forms.TextBox
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents CheckSinBalanceo As System.Windows.Forms.CheckBox
    Friend WithEvents CheckConBalanceo As System.Windows.Forms.CheckBox
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents CheckDetalle As System.Windows.Forms.CheckBox
    Friend WithEvents PanelMoneda As System.Windows.Forms.Panel
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents CheckConAnuladas As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonSoloTotales As System.Windows.Forms.Button
    Friend WithEvents ButtonNoImputados As System.Windows.Forms.Button
    Friend WithEvents CheckSinAsignar As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonCompQueLoImputan As System.Windows.Forms.Button
    Friend WithEvents CheckSinRemitos As System.Windows.Forms.CheckBox
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents CheckSoloDomesticos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckSoloExterior As System.Windows.Forms.CheckBox
    Friend WithEvents CheckYaAsignados As System.Windows.Forms.CheckBox
    Friend WithEvents LabelCanalVenta As System.Windows.Forms.Label
    Friend WithEvents ComboBoxCanalVenta As System.Windows.Forms.ComboBox
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TipoOrigen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Emisor1 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Tipo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Mensaje As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ReciboOficial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Debito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Credito As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SaldoCta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CanalVenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CanalDistribucion As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents CheckIncluirConSaldoCero As System.Windows.Forms.CheckBox
End Class
