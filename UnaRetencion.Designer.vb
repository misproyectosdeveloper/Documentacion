<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaRetencion
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
        Me.TextNombre = New System.Windows.Forms.TextBox
        Me.TextClaveInterna = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboOrigenPercepcion = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboCodigoRetencion = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.LupaCuentaDebito = New System.Windows.Forms.PictureBox
        Me.LupaCuentaCredito = New System.Windows.Forms.PictureBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.ButtonBorrar = New System.Windows.Forms.Button
        Me.ButtonGarbar = New System.Windows.Forms.Button
        Me.MaskedCuentaDebito = New System.Windows.Forms.MaskedTextBox
        Me.MaskedCuentaCredito = New System.Windows.Forms.MaskedTextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.CheckEsPorProvincia = New System.Windows.Forms.CheckBox
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.CodigoComprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Comprobante = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextJurisdiccion = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.DateVigencia = New System.Windows.Forms.DateTimePicker
        Me.Label14 = New System.Windows.Forms.Label
        Me.ComboCodigoAfipElectronico = New System.Windows.Forms.ComboBox
        Me.ButtonVerClientes = New System.Windows.Forms.Button
        Me.Label13 = New System.Windows.Forms.Label
        Me.GridAuto = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewCheckBoxColumn1 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextAlicuota = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextTopeMes = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextFormula = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.ComboTipoImpositivo = New System.Windows.Forms.ComboBox
        Me.TextCodigoProvinciaIngresoBruto = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        CType(Me.LupaCuentaDebito, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.LupaCuentaCredito, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.GridAuto, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextNombre
        '
        Me.TextNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombre.Location = New System.Drawing.Point(134, 34)
        Me.TextNombre.MaxLength = 20
        Me.TextNombre.Name = "TextNombre"
        Me.TextNombre.Size = New System.Drawing.Size(347, 21)
        Me.TextNombre.TabIndex = 0
        '
        'TextClaveInterna
        '
        Me.TextClaveInterna.BackColor = System.Drawing.Color.White
        Me.TextClaveInterna.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextClaveInterna.Location = New System.Drawing.Point(24, 34)
        Me.TextClaveInterna.MaxLength = 25
        Me.TextClaveInterna.Name = "TextClaveInterna"
        Me.TextClaveInterna.ReadOnly = True
        Me.TextClaveInterna.Size = New System.Drawing.Size(85, 21)
        Me.TextClaveInterna.TabIndex = 1
        Me.TextClaveInterna.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(86, 16)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Clave Interna"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(132, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Nombre"
        '
        'ComboOrigenPercepcion
        '
        Me.ComboOrigenPercepcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboOrigenPercepcion.FormattingEnabled = True
        Me.ComboOrigenPercepcion.Location = New System.Drawing.Point(25, 80)
        Me.ComboOrigenPercepcion.Name = "ComboOrigenPercepcion"
        Me.ComboOrigenPercepcion.Size = New System.Drawing.Size(264, 23)
        Me.ComboOrigenPercepcion.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(22, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(207, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "Código AFIP Perc./Ret.  para CITI"
        '
        'ComboCodigoRetencion
        '
        Me.ComboCodigoRetencion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCodigoRetencion.FormattingEnabled = True
        Me.ComboCodigoRetencion.Location = New System.Drawing.Point(647, 32)
        Me.ComboCodigoRetencion.Name = "ComboCodigoRetencion"
        Me.ComboCodigoRetencion.Size = New System.Drawing.Size(215, 23)
        Me.ComboCodigoRetencion.TabIndex = 15
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(644, 12)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(36, 16)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Tipo"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(300, 60)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(163, 16)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Cuenta para Debito Fiscal"
        '
        'LupaCuentaDebito
        '
        Me.LupaCuentaDebito.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.LupaCuentaDebito.InitialImage = Nothing
        Me.LupaCuentaDebito.Location = New System.Drawing.Point(458, 78)
        Me.LupaCuentaDebito.Name = "LupaCuentaDebito"
        Me.LupaCuentaDebito.Size = New System.Drawing.Size(38, 38)
        Me.LupaCuentaDebito.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.LupaCuentaDebito.TabIndex = 289
        Me.LupaCuentaDebito.TabStop = False
        '
        'LupaCuentaCredito
        '
        Me.LupaCuentaCredito.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.LupaCuentaCredito.InitialImage = Nothing
        Me.LupaCuentaCredito.Location = New System.Drawing.Point(669, 78)
        Me.LupaCuentaCredito.Name = "LupaCuentaCredito"
        Me.LupaCuentaCredito.Size = New System.Drawing.Size(38, 38)
        Me.LupaCuentaCredito.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.LupaCuentaCredito.TabIndex = 292
        Me.LupaCuentaCredito.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(507, 60)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(169, 16)
        Me.Label7.TabIndex = 290
        Me.Label7.Text = "Cuenta para  Crédito Fiscal"
        '
        'ButtonBorrar
        '
        Me.ButtonBorrar.BackColor = System.Drawing.Color.Transparent
        Me.ButtonBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBorrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBorrar.Image = Global.ScomerV01.My.Resources.Resources.Basurero
        Me.ButtonBorrar.Location = New System.Drawing.Point(19, 641)
        Me.ButtonBorrar.Name = "ButtonBorrar"
        Me.ButtonBorrar.Size = New System.Drawing.Size(146, 30)
        Me.ButtonBorrar.TabIndex = 1019
        Me.ButtonBorrar.TabStop = False
        Me.ButtonBorrar.Text = "Borrar  Ret./Perc."
        Me.ButtonBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBorrar.UseVisualStyleBackColor = False
        '
        'ButtonGarbar
        '
        Me.ButtonGarbar.BackColor = System.Drawing.Color.Transparent
        Me.ButtonGarbar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonGarbar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonGarbar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonGarbar.Location = New System.Drawing.Point(721, 640)
        Me.ButtonGarbar.Name = "ButtonGarbar"
        Me.ButtonGarbar.Size = New System.Drawing.Size(152, 33)
        Me.ButtonGarbar.TabIndex = 1018
        Me.ButtonGarbar.Text = "Graba Cambios "
        Me.ButtonGarbar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonGarbar.UseVisualStyleBackColor = False
        '
        'MaskedCuentaDebito
        '
        Me.MaskedCuentaDebito.BackColor = System.Drawing.Color.White
        Me.MaskedCuentaDebito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedCuentaDebito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedCuentaDebito.Location = New System.Drawing.Point(303, 83)
        Me.MaskedCuentaDebito.Mask = "000-000000-00"
        Me.MaskedCuentaDebito.Name = "MaskedCuentaDebito"
        Me.MaskedCuentaDebito.ReadOnly = True
        Me.MaskedCuentaDebito.Size = New System.Drawing.Size(144, 21)
        Me.MaskedCuentaDebito.TabIndex = 1020
        Me.MaskedCuentaDebito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedCuentaDebito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'MaskedCuentaCredito
        '
        Me.MaskedCuentaCredito.BackColor = System.Drawing.Color.White
        Me.MaskedCuentaCredito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedCuentaCredito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedCuentaCredito.Location = New System.Drawing.Point(511, 83)
        Me.MaskedCuentaCredito.Mask = "000-000000-00"
        Me.MaskedCuentaCredito.Name = "MaskedCuentaCredito"
        Me.MaskedCuentaCredito.ReadOnly = True
        Me.MaskedCuentaCredito.Size = New System.Drawing.Size(144, 21)
        Me.MaskedCuentaCredito.TabIndex = 1021
        Me.MaskedCuentaCredito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedCuentaCredito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Khaki
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label15)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.CheckEsPorProvincia)
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Location = New System.Drawing.Point(18, 122)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(855, 243)
        Me.Panel2.TabIndex = 1028
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(258, 5)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(540, 20)
        Me.Label15.TabIndex = 1031
        Me.Label15.Text = "Retenciones/Percepciones que terceros hacen a nuestra Empresa"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(6, 7)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(207, 16)
        Me.Label12.TabIndex = 1026
        Me.Label12.Text = "Comprobantes donde se Percibe"
        '
        'CheckEsPorProvincia
        '
        Me.CheckEsPorProvincia.AutoSize = True
        Me.CheckEsPorProvincia.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEsPorProvincia.Location = New System.Drawing.Point(349, 54)
        Me.CheckEsPorProvincia.Name = "CheckEsPorProvincia"
        Me.CheckEsPorProvincia.Size = New System.Drawing.Size(231, 28)
        Me.CheckEsPorProvincia.TabIndex = 1025
        Me.CheckEsPorProvincia.Text = "Distribuye por Provincia "
        Me.CheckEsPorProvincia.UseVisualStyleBackColor = True
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CodigoComprobante, Me.Sel, Me.Comprobante})
        Me.Grid.Location = New System.Drawing.Point(8, 28)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(253, 210)
        Me.Grid.TabIndex = 1024
        '
        'CodigoComprobante
        '
        Me.CodigoComprobante.DataPropertyName = "Clave"
        Me.CodigoComprobante.HeaderText = ""
        Me.CodigoComprobante.Name = "CodigoComprobante"
        Me.CodigoComprobante.Visible = False
        '
        'Sel
        '
        Me.Sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel.DataPropertyName = "Sel"
        Me.Sel.HeaderText = "Sel"
        Me.Sel.MinimumWidth = 30
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 30
        '
        'Comprobante
        '
        Me.Comprobante.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comprobante.DataPropertyName = "Nombre"
        Me.Comprobante.HeaderText = "Comprobante"
        Me.Comprobante.Name = "Comprobante"
        Me.Comprobante.ReadOnly = True
        Me.Comprobante.Width = 150
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.PaleGreen
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextCodigoProvinciaIngresoBruto)
        Me.Panel3.Controls.Add(Me.Label18)
        Me.Panel3.Controls.Add(Me.TextJurisdiccion)
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.DateVigencia)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.ComboCodigoAfipElectronico)
        Me.Panel3.Controls.Add(Me.ButtonVerClientes)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.GridAuto)
        Me.Panel3.Controls.Add(Me.Panel1)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Location = New System.Drawing.Point(18, 370)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(855, 264)
        Me.Panel3.TabIndex = 1029
        '
        'TextJurisdiccion
        '
        Me.TextJurisdiccion.BackColor = System.Drawing.Color.White
        Me.TextJurisdiccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextJurisdiccion.Location = New System.Drawing.Point(466, 164)
        Me.TextJurisdiccion.MaxLength = 3
        Me.TextJurisdiccion.Name = "TextJurisdiccion"
        Me.TextJurisdiccion.Size = New System.Drawing.Size(50, 26)
        Me.TextJurisdiccion.TabIndex = 1048
        Me.TextJurisdiccion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(295, 163)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(167, 37)
        Me.Label16.TabIndex = 1047
        Me.Label16.Text = "Jurisdicción  Ingreso Bruto Convenio Multilateral"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(512, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 16)
        Me.Label3.TabIndex = 1046
        Me.Label3.Text = "Vigencia a partir de"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateVigencia
        '
        Me.DateVigencia.CustomFormat = "dd/MM/yyyy"
        Me.DateVigencia.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateVigencia.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateVigencia.Location = New System.Drawing.Point(650, 33)
        Me.DateVigencia.Name = "DateVigencia"
        Me.DateVigencia.Size = New System.Drawing.Size(130, 26)
        Me.DateVigencia.TabIndex = 1045
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(294, 66)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(212, 39)
        Me.Label14.TabIndex = 1041
        Me.Label14.Text = "Código AFIP de Percepciones para Comprobantes Electrónicos"
        '
        'ComboCodigoAfipElectronico
        '
        Me.ComboCodigoAfipElectronico.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCodigoAfipElectronico.FormattingEnabled = True
        Me.ComboCodigoAfipElectronico.Location = New System.Drawing.Point(518, 67)
        Me.ComboCodigoAfipElectronico.Name = "ComboCodigoAfipElectronico"
        Me.ComboCodigoAfipElectronico.Size = New System.Drawing.Size(264, 23)
        Me.ComboCodigoAfipElectronico.TabIndex = 1040
        '
        'ButtonVerClientes
        '
        Me.ButtonVerClientes.BackColor = System.Drawing.Color.Transparent
        Me.ButtonVerClientes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ButtonVerClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonVerClientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerClientes.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonVerClientes.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonVerClientes.Location = New System.Drawing.Point(633, 210)
        Me.ButtonVerClientes.Name = "ButtonVerClientes"
        Me.ButtonVerClientes.Size = New System.Drawing.Size(207, 39)
        Me.ButtonVerClientes.TabIndex = 1039
        Me.ButtonVerClientes.Text = "Ayuda para Nro. Formula"
        Me.ButtonVerClientes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerClientes.UseVisualStyleBackColor = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(10, 11)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(213, 16)
        Me.Label13.TabIndex = 1032
        Me.Label13.Text = "Comprobantes donde se Realizan"
        '
        'GridAuto
        '
        Me.GridAuto.AllowUserToAddRows = False
        Me.GridAuto.AllowUserToDeleteRows = False
        Me.GridAuto.BackgroundColor = System.Drawing.Color.White
        Me.GridAuto.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridAuto.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewCheckBoxColumn1, Me.DataGridViewTextBoxColumn2})
        Me.GridAuto.Location = New System.Drawing.Point(11, 32)
        Me.GridAuto.Name = "GridAuto"
        Me.GridAuto.Size = New System.Drawing.Size(253, 149)
        Me.GridAuto.TabIndex = 1031
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "Clave"
        Me.DataGridViewTextBoxColumn1.HeaderText = ""
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.Visible = False
        '
        'DataGridViewCheckBoxColumn1
        '
        Me.DataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.DataGridViewCheckBoxColumn1.DataPropertyName = "Sel"
        Me.DataGridViewCheckBoxColumn1.HeaderText = "Sel"
        Me.DataGridViewCheckBoxColumn1.MinimumWidth = 30
        Me.DataGridViewCheckBoxColumn1.Name = "DataGridViewCheckBoxColumn1"
        Me.DataGridViewCheckBoxColumn1.Width = 30
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "Nombre"
        Me.DataGridViewTextBoxColumn2.HeaderText = "Comprobante"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        Me.DataGridViewTextBoxColumn2.Width = 150
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TextAlicuota)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.TextTopeMes)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.TextFormula)
        Me.Panel1.Location = New System.Drawing.Point(310, 108)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(529, 43)
        Me.Panel1.TabIndex = 1027
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(379, 11)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(71, 16)
        Me.Label10.TabIndex = 304
        Me.Label10.Text = "% Alicuota"
        '
        'TextAlicuota
        '
        Me.TextAlicuota.BackColor = System.Drawing.Color.White
        Me.TextAlicuota.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAlicuota.Location = New System.Drawing.Point(459, 6)
        Me.TextAlicuota.MaxLength = 5
        Me.TextAlicuota.Name = "TextAlicuota"
        Me.TextAlicuota.Size = New System.Drawing.Size(50, 26)
        Me.TextAlicuota.TabIndex = 303
        Me.TextAlicuota.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(161, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(92, 16)
        Me.Label9.TabIndex = 302
        Me.Label9.Text = "Tope del Mes"
        '
        'TextTopeMes
        '
        Me.TextTopeMes.BackColor = System.Drawing.Color.White
        Me.TextTopeMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTopeMes.Location = New System.Drawing.Point(261, 7)
        Me.TextTopeMes.MaxLength = 9
        Me.TextTopeMes.Name = "TextTopeMes"
        Me.TextTopeMes.Size = New System.Drawing.Size(109, 26)
        Me.TextTopeMes.TabIndex = 301
        Me.TextTopeMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(8, 12)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(85, 16)
        Me.Label8.TabIndex = 300
        Me.Label8.Text = "Nro. Formula"
        '
        'TextFormula
        '
        Me.TextFormula.BackColor = System.Drawing.Color.White
        Me.TextFormula.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFormula.Location = New System.Drawing.Point(105, 7)
        Me.TextFormula.MaxLength = 2
        Me.TextFormula.Name = "TextFormula"
        Me.TextFormula.Size = New System.Drawing.Size(44, 26)
        Me.TextFormula.TabIndex = 299
        Me.TextFormula.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(263, 7)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(534, 20)
        Me.Label11.TabIndex = 1023
        Me.Label11.Text = "Retenciones/Percepciones que nuestra Empresa hace a Terceros"
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(712, 62)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(100, 16)
        Me.Label17.TabIndex = 1031
        Me.Label17.Text = "Tipo Impositivo"
        '
        'ComboTipoImpositivo
        '
        Me.ComboTipoImpositivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoImpositivo.FormattingEnabled = True
        Me.ComboTipoImpositivo.Location = New System.Drawing.Point(715, 82)
        Me.ComboTipoImpositivo.Name = "ComboTipoImpositivo"
        Me.ComboTipoImpositivo.Size = New System.Drawing.Size(161, 23)
        Me.ComboTipoImpositivo.TabIndex = 1030
        '
        'TextCodigoProvinciaIngresoBruto
        '
        Me.TextCodigoProvinciaIngresoBruto.BackColor = System.Drawing.Color.White
        Me.TextCodigoProvinciaIngresoBruto.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCodigoProvinciaIngresoBruto.Location = New System.Drawing.Point(696, 163)
        Me.TextCodigoProvinciaIngresoBruto.MaxLength = 3
        Me.TextCodigoProvinciaIngresoBruto.Name = "TextCodigoProvinciaIngresoBruto"
        Me.TextCodigoProvinciaIngresoBruto.Size = New System.Drawing.Size(50, 26)
        Me.TextCodigoProvinciaIngresoBruto.TabIndex = 1050
        Me.TextCodigoProvinciaIngresoBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label18
        '
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(536, 162)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(167, 37)
        Me.Label18.TabIndex = 1049
        Me.Label18.Text = "Jurisdicción  Provincia Ingreso Bruto"
        '
        'UnaRetencion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(890, 682)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.ComboTipoImpositivo)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.MaskedCuentaCredito)
        Me.Controls.Add(Me.MaskedCuentaDebito)
        Me.Controls.Add(Me.ButtonBorrar)
        Me.Controls.Add(Me.ButtonGarbar)
        Me.Controls.Add(Me.LupaCuentaCredito)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.LupaCuentaDebito)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ComboCodigoRetencion)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ComboOrigenPercepcion)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextClaveInterna)
        Me.Controls.Add(Me.TextNombre)
        Me.Name = "UnaRetencion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Retenciones/Precepciones"
        CType(Me.LupaCuentaDebito, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.LupaCuentaCredito, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.GridAuto, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextNombre As System.Windows.Forms.TextBox
    Friend WithEvents TextClaveInterna As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboOrigenPercepcion As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboCodigoRetencion As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents LupaCuentaDebito As System.Windows.Forms.PictureBox
    Friend WithEvents LupaCuentaCredito As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonBorrar As System.Windows.Forms.Button
    Friend WithEvents ButtonGarbar As System.Windows.Forms.Button
    Friend WithEvents MaskedCuentaDebito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents MaskedCuentaCredito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents CheckEsPorProvincia As System.Windows.Forms.CheckBox
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents CodigoComprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Comprobante As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextAlicuota As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextTopeMes As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextFormula As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GridAuto As System.Windows.Forms.DataGridView
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerClientes As System.Windows.Forms.Button
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewCheckBoxColumn1 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ComboCodigoAfipElectronico As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents DateVigencia As System.Windows.Forms.DateTimePicker
    Friend WithEvents TextJurisdiccion As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents ComboTipoImpositivo As System.Windows.Forms.ComboBox
    Friend WithEvents TextCodigoProvinciaIngresoBruto As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
End Class
