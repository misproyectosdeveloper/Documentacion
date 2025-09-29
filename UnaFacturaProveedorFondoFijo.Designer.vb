<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaFacturaProveedorFondoFijo
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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaFacturaProveedorFondoFijo))
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.MaskedReciboOficial = New System.Windows.Forms.MaskedTextBox
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboPais = New System.Windows.Forms.ComboBox
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.ComboConceptoGasto = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.CheckSecos = New System.Windows.Forms.CheckBox
        Me.TextFechaContable = New System.Windows.Forms.TextBox
        Me.TextFechaFactura = New System.Windows.Forms.TextBox
        Me.LabelCuentas = New System.Windows.Forms.Label
        Me.ListCuentas = New System.Windows.Forms.ListView
        Me.Cuenta = New System.Windows.Forms.ColumnHeader
        Me.Importe1 = New System.Windows.Forms.ColumnHeader
        Me.Importe2 = New System.Windows.Forms.ColumnHeader
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboTipoIva = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextCuit = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.MaskedFacturaN = New System.Windows.Forms.MaskedTextBox
        Me.MaskedFactura = New System.Windows.Forms.MaskedTextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextTipoFactura = New System.Windows.Forms.TextBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ButtonDatosEmisor = New System.Windows.Forms.Button
        Me.Label20 = New System.Windows.Forms.Label
        Me.TextRendicion = New System.Windows.Forms.TextBox
        Me.TextNombreFondoFijo = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.PictureAlmanaqueContable = New System.Windows.Forms.PictureBox
        Me.PictureAlmanaqueFactura = New System.Windows.Forms.PictureBox
        Me.PictureLupaCuenta = New System.Windows.Forms.PictureBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Clave = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TieneLupa = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Alicuota = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImporteB = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImporteN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lupa = New System.Windows.Forms.DataGridViewImageColumn
        Me.TextTotalN = New System.Windows.Forms.TextBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextTotalB = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.PaneLotes = New System.Windows.Forms.Panel
        Me.ButtonNetoPorLotes = New System.Windows.Forms.Button
        Me.ButtonLotesAImputar = New System.Windows.Forms.Button
        Me.ButtonNuevaIgualProveedor = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.Panel4.SuspendLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueFactura, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PaneLotes.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(78, 84)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(189, 21)
        Me.ComboNegocio.TabIndex = 7
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(6, 87)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(54, 13)
        Me.Label11.TabIndex = 284
        Me.Label11.Text = "Negocio"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(463, 34)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(96, 13)
        Me.Label22.TabIndex = 283
        Me.Label22.Text = "Fecha Contable"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(246, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 275
        Me.Label1.Text = "Fecha Factura"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(7, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(112, 13)
        Me.Label6.TabIndex = 274
        Me.Label6.Text = "Factura Proveedor"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedReciboOficial
        '
        Me.MaskedReciboOficial.BackColor = System.Drawing.Color.White
        Me.MaskedReciboOficial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedReciboOficial.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedReciboOficial.Location = New System.Drawing.Point(125, 28)
        Me.MaskedReciboOficial.Mask = "0000-00000000"
        Me.MaskedReciboOficial.Name = "MaskedReciboOficial"
        Me.MaskedReciboOficial.Size = New System.Drawing.Size(113, 21)
        Me.MaskedReciboOficial.TabIndex = 1
        Me.MaskedReciboOficial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedReciboOficial.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(330, 83)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(217, 21)
        Me.ComboCosteo.TabIndex = 8
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(280, 88)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 202
        Me.Label10.Text = "Costeo"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboPais
        '
        Me.ComboPais.BackColor = System.Drawing.SystemColors.Control
        Me.ComboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboPais.Enabled = False
        Me.ComboPais.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPais.FormattingEnabled = True
        Me.ComboPais.Location = New System.Drawing.Point(322, 112)
        Me.ComboPais.Name = "ComboPais"
        Me.ComboPais.Size = New System.Drawing.Size(143, 21)
        Me.ComboPais.TabIndex = 201
        Me.ComboPais.TabStop = False
        '
        'ComboEmisor
        '
        Me.ComboEmisor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEmisor.Enabled = False
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(86, 3)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 21)
        Me.ComboEmisor.TabIndex = 193
        Me.ComboEmisor.TabStop = False
        '
        'ComboConceptoGasto
        '
        Me.ComboConceptoGasto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboConceptoGasto.FormattingEnabled = True
        Me.ComboConceptoGasto.Location = New System.Drawing.Point(819, 34)
        Me.ComboConceptoGasto.Name = "ComboConceptoGasto"
        Me.ComboConceptoGasto.Size = New System.Drawing.Size(120, 21)
        Me.ComboConceptoGasto.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(691, 36)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(119, 13)
        Me.Label9.TabIndex = 191
        Me.Label9.Text = "Concepto del Gasto"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckSecos
        '
        Me.CheckSecos.AutoSize = True
        Me.CheckSecos.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckSecos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSecos.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckSecos.Location = New System.Drawing.Point(669, 55)
        Me.CheckSecos.Name = "CheckSecos"
        Me.CheckSecos.Size = New System.Drawing.Size(85, 19)
        Me.CheckSecos.TabIndex = 1010
        Me.CheckSecos.TabStop = False
        Me.CheckSecos.Text = "Es Secos"
        Me.CheckSecos.UseVisualStyleBackColor = True
        Me.CheckSecos.Visible = False
        '
        'TextFechaContable
        '
        Me.TextFechaContable.BackColor = System.Drawing.Color.White
        Me.TextFechaContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaContable.Location = New System.Drawing.Point(565, 30)
        Me.TextFechaContable.MaxLength = 10
        Me.TextFechaContable.Name = "TextFechaContable"
        Me.TextFechaContable.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaContable.TabIndex = 1008
        Me.TextFechaContable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextFechaFactura
        '
        Me.TextFechaFactura.BackColor = System.Drawing.Color.White
        Me.TextFechaFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaFactura.Location = New System.Drawing.Point(340, 30)
        Me.TextFechaFactura.MaxLength = 10
        Me.TextFechaFactura.Name = "TextFechaFactura"
        Me.TextFechaFactura.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaFactura.TabIndex = 1006
        Me.TextFechaFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LabelCuentas
        '
        Me.LabelCuentas.AutoSize = True
        Me.LabelCuentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCuentas.Location = New System.Drawing.Point(652, 84)
        Me.LabelCuentas.Name = "LabelCuentas"
        Me.LabelCuentas.Size = New System.Drawing.Size(123, 13)
        Me.LabelCuentas.TabIndex = 296
        Me.LabelCuentas.Text = "Imputación Contable"
        Me.LabelCuentas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ListCuentas
        '
        Me.ListCuentas.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Cuenta, Me.Importe1, Me.Importe2})
        Me.ListCuentas.Location = New System.Drawing.Point(781, 76)
        Me.ListCuentas.Name = "ListCuentas"
        Me.ListCuentas.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListCuentas.Size = New System.Drawing.Size(121, 28)
        Me.ListCuentas.TabIndex = 294
        Me.ListCuentas.TileSize = New System.Drawing.Size(90, 15)
        Me.ListCuentas.UseCompatibleStateImageBehavior = False
        Me.ListCuentas.View = System.Windows.Forms.View.Tile
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(70, 114)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(197, 20)
        Me.TextComentario.TabIndex = 180
        Me.TextComentario.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 117)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(60, 13)
        Me.Label3.TabIndex = 179
        Me.Label3.Text = "Comentario"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboTipoIva
        '
        Me.ComboTipoIva.BackColor = System.Drawing.SystemColors.Control
        Me.ComboTipoIva.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboTipoIva.Enabled = False
        Me.ComboTipoIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoIva.FormattingEnabled = True
        Me.ComboTipoIva.Location = New System.Drawing.Point(645, 112)
        Me.ComboTipoIva.Name = "ComboTipoIva"
        Me.ComboTipoIva.Size = New System.Drawing.Size(144, 21)
        Me.ComboTipoIva.TabIndex = 0
        Me.ComboTipoIva.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(616, 115)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(24, 13)
        Me.Label14.TabIndex = 135
        Me.Label14.Text = "IVA"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuit
        '
        Me.TextCuit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuit.Location = New System.Drawing.Point(508, 112)
        Me.TextCuit.MaxLength = 20
        Me.TextCuit.Name = "TextCuit"
        Me.TextCuit.ReadOnly = True
        Me.TextCuit.Size = New System.Drawing.Size(101, 20)
        Me.TextCuit.TabIndex = 0
        Me.TextCuit.TabStop = False
        Me.TextCuit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(292, 115)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(27, 13)
        Me.Label12.TabIndex = 107
        Me.Label12.Text = "Pais"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(475, 116)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(25, 13)
        Me.Label13.TabIndex = 104
        Me.Label13.Text = "Cuit"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(779, 6)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(42, 13)
        Me.Label19.TabIndex = 33
        Me.Label19.Text = "Fecha"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(826, 3)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(110, 20)
        Me.DateTime1.TabIndex = 32
        Me.DateTime1.TabStop = False
        '
        'MaskedFacturaN
        '
        Me.MaskedFacturaN.BackColor = System.Drawing.Color.White
        Me.MaskedFacturaN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedFacturaN.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedFacturaN.Location = New System.Drawing.Point(202, 15)
        Me.MaskedFacturaN.Mask = "00000000"
        Me.MaskedFacturaN.Name = "MaskedFacturaN"
        Me.MaskedFacturaN.ReadOnly = True
        Me.MaskedFacturaN.Size = New System.Drawing.Size(86, 21)
        Me.MaskedFacturaN.TabIndex = 319
        Me.MaskedFacturaN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedFacturaN.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'MaskedFactura
        '
        Me.MaskedFactura.BackColor = System.Drawing.Color.White
        Me.MaskedFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedFactura.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedFactura.Location = New System.Drawing.Point(100, 15)
        Me.MaskedFactura.Mask = "00000000"
        Me.MaskedFactura.Name = "MaskedFactura"
        Me.MaskedFactura.ReadOnly = True
        Me.MaskedFactura.Size = New System.Drawing.Size(87, 21)
        Me.MaskedFactura.TabIndex = 318
        Me.MaskedFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedFactura.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 317
        Me.Label2.Text = "Nro. Interno"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTipoFactura
        '
        Me.TextTipoFactura.BackColor = System.Drawing.Color.White
        Me.TextTipoFactura.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextTipoFactura.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTipoFactura.Location = New System.Drawing.Point(495, 11)
        Me.TextTipoFactura.Margin = New System.Windows.Forms.Padding(0)
        Me.TextTipoFactura.MaxLength = 1
        Me.TextTipoFactura.Name = "TextTipoFactura"
        Me.TextTipoFactura.ReadOnly = True
        Me.TextTipoFactura.Size = New System.Drawing.Size(39, 29)
        Me.TextTipoFactura.TabIndex = 316
        Me.TextTipoFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ButtonDatosEmisor)
        Me.Panel4.Controls.Add(Me.Label20)
        Me.Panel4.Controls.Add(Me.TextRendicion)
        Me.Panel4.Controls.Add(Me.TextNombreFondoFijo)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.TextNumero)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.CheckSecos)
        Me.Panel4.Controls.Add(Me.PictureAlmanaqueContable)
        Me.Panel4.Controls.Add(Me.TextFechaContable)
        Me.Panel4.Controls.Add(Me.TextFechaFactura)
        Me.Panel4.Controls.Add(Me.PictureAlmanaqueFactura)
        Me.Panel4.Controls.Add(Me.LabelCuentas)
        Me.Panel4.Controls.Add(Me.PictureLupaCuenta)
        Me.Panel4.Controls.Add(Me.ListCuentas)
        Me.Panel4.Controls.Add(Me.ComboNegocio)
        Me.Panel4.Controls.Add(Me.Label11)
        Me.Panel4.Controls.Add(Me.Label22)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.MaskedReciboOficial)
        Me.Panel4.Controls.Add(Me.ComboCosteo)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.ComboPais)
        Me.Panel4.Controls.Add(Me.ComboEmisor)
        Me.Panel4.Controls.Add(Me.ComboConceptoGasto)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.TextComentario)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.ComboTipoIva)
        Me.Panel4.Controls.Add(Me.Label14)
        Me.Panel4.Controls.Add(Me.TextCuit)
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.Label13)
        Me.Panel4.Controls.Add(Me.Label19)
        Me.Panel4.Controls.Add(Me.DateTime1)
        Me.Panel4.Controls.Add(Me.LabelEmisor)
        Me.Panel4.Location = New System.Drawing.Point(14, 42)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(961, 139)
        Me.Panel4.TabIndex = 306
        '
        'ButtonDatosEmisor
        '
        Me.ButtonDatosEmisor.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ButtonDatosEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDatosEmisor.Location = New System.Drawing.Point(802, 112)
        Me.ButtonDatosEmisor.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDatosEmisor.Name = "ButtonDatosEmisor"
        Me.ButtonDatosEmisor.Size = New System.Drawing.Size(120, 20)
        Me.ButtonDatosEmisor.TabIndex = 1018
        Me.ButtonDatosEmisor.TabStop = False
        Me.ButtonDatosEmisor.Text = "Datos Proveedor"
        Me.ButtonDatosEmisor.UseVisualStyleBackColor = False
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(510, 59)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(64, 13)
        Me.Label20.TabIndex = 1017
        Me.Label20.Text = "Rendición"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextRendicion
        '
        Me.TextRendicion.BackColor = System.Drawing.Color.White
        Me.TextRendicion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRendicion.Location = New System.Drawing.Point(581, 55)
        Me.TextRendicion.MaxLength = 5
        Me.TextRendicion.Name = "TextRendicion"
        Me.TextRendicion.ReadOnly = True
        Me.TextRendicion.Size = New System.Drawing.Size(74, 20)
        Me.TextRendicion.TabIndex = 1016
        Me.TextRendicion.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextNombreFondoFijo
        '
        Me.TextNombreFondoFijo.BackColor = System.Drawing.Color.White
        Me.TextNombreFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreFondoFijo.Location = New System.Drawing.Point(149, 56)
        Me.TextNombreFondoFijo.MaxLength = 30
        Me.TextNombreFondoFijo.Name = "TextNombreFondoFijo"
        Me.TextNombreFondoFijo.ReadOnly = True
        Me.TextNombreFondoFijo.Size = New System.Drawing.Size(172, 20)
        Me.TextNombreFondoFijo.TabIndex = 1015
        Me.TextNombreFondoFijo.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(336, 59)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 1014
        Me.Label7.Text = "Numero"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextNumero
        '
        Me.TextNumero.BackColor = System.Drawing.Color.White
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(396, 55)
        Me.TextNumero.MaxLength = 5
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.ReadOnly = True
        Me.TextNumero.Size = New System.Drawing.Size(103, 20)
        Me.TextNumero.TabIndex = 1013
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(7, 61)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(128, 13)
        Me.Label5.TabIndex = 1012
        Me.Label5.Text = "Proveedor Fondo Fijo"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PictureAlmanaqueContable
        '
        Me.PictureAlmanaqueContable.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueContable.Location = New System.Drawing.Point(653, 30)
        Me.PictureAlmanaqueContable.Name = "PictureAlmanaqueContable"
        Me.PictureAlmanaqueContable.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueContable.TabIndex = 1009
        Me.PictureAlmanaqueContable.TabStop = False
        '
        'PictureAlmanaqueFactura
        '
        Me.PictureAlmanaqueFactura.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueFactura.Location = New System.Drawing.Point(426, 29)
        Me.PictureAlmanaqueFactura.Name = "PictureAlmanaqueFactura"
        Me.PictureAlmanaqueFactura.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueFactura.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueFactura.TabIndex = 1007
        Me.PictureAlmanaqueFactura.TabStop = False
        '
        'PictureLupaCuenta
        '
        Me.PictureLupaCuenta.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupaCuenta.InitialImage = Nothing
        Me.PictureLupaCuenta.Location = New System.Drawing.Point(904, 74)
        Me.PictureLupaCuenta.Name = "PictureLupaCuenta"
        Me.PictureLupaCuenta.Size = New System.Drawing.Size(31, 29)
        Me.PictureLupaCuenta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupaCuenta.TabIndex = 295
        Me.PictureLupaCuenta.TabStop = False
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(9, 7)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(65, 13)
        Me.LabelEmisor.TabIndex = 18
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Clave, Me.TieneLupa, Me.Sel, Me.Nombre, Me.Alicuota, Me.ImporteB, Me.ImporteN, Me.Lupa})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle10
        Me.Grid.Location = New System.Drawing.Point(228, 222)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(510, 356)
        Me.Grid.TabIndex = 307
        '
        'Clave
        '
        Me.Clave.DataPropertyName = "Clave"
        Me.Clave.HeaderText = "Clave"
        Me.Clave.Name = "Clave"
        Me.Clave.Visible = False
        Me.Clave.Width = 59
        '
        'TieneLupa
        '
        Me.TieneLupa.DataPropertyName = "TieneLupa"
        Me.TieneLupa.HeaderText = "TieneLupa"
        Me.TieneLupa.Name = "TieneLupa"
        Me.TieneLupa.Visible = False
        Me.TieneLupa.Width = 83
        '
        'Sel
        '
        Me.Sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 20
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 20
        '
        'Nombre
        '
        Me.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Nombre.DataPropertyName = "Nombre"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White
        Me.Nombre.DefaultCellStyle = DataGridViewCellStyle6
        Me.Nombre.HeaderText = "Concepto"
        Me.Nombre.MaxInputLength = 10
        Me.Nombre.MinimumWidth = 120
        Me.Nombre.Name = "Nombre"
        Me.Nombre.ReadOnly = True
        Me.Nombre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Nombre.Width = 120
        '
        'Alicuota
        '
        Me.Alicuota.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Alicuota.DataPropertyName = "Iva"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black
        Me.Alicuota.DefaultCellStyle = DataGridViewCellStyle7
        Me.Alicuota.HeaderText = "Alicuota %"
        Me.Alicuota.MaxInputLength = 10
        Me.Alicuota.MinimumWidth = 70
        Me.Alicuota.Name = "Alicuota"
        Me.Alicuota.ReadOnly = True
        Me.Alicuota.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Alicuota.Width = 70
        '
        'ImporteB
        '
        Me.ImporteB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ImporteB.DataPropertyName = "ImporteB"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White
        Me.ImporteB.DefaultCellStyle = DataGridViewCellStyle8
        Me.ImporteB.HeaderText = "Importe"
        Me.ImporteB.MaxInputLength = 10
        Me.ImporteB.MinimumWidth = 100
        Me.ImporteB.Name = "ImporteB"
        Me.ImporteB.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ImporteB.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'ImporteN
        '
        Me.ImporteN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ImporteN.DataPropertyName = "ImporteN"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White
        Me.ImporteN.DefaultCellStyle = DataGridViewCellStyle9
        Me.ImporteN.HeaderText = "Importe (2)"
        Me.ImporteN.MinimumWidth = 100
        Me.ImporteN.Name = "ImporteN"
        Me.ImporteN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Lupa
        '
        Me.Lupa.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lupa.HeaderText = ""
        Me.Lupa.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Lupa.MinimumWidth = 35
        Me.Lupa.Name = "Lupa"
        Me.Lupa.Width = 35
        '
        'TextTotalN
        '
        Me.TextTotalN.BackColor = System.Drawing.Color.Gainsboro
        Me.TextTotalN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalN.ForeColor = System.Drawing.Color.Black
        Me.TextTotalN.Location = New System.Drawing.Point(566, 584)
        Me.TextTotalN.MaxLength = 30
        Me.TextTotalN.Name = "TextTotalN"
        Me.TextTotalN.ReadOnly = True
        Me.TextTotalN.Size = New System.Drawing.Size(96, 20)
        Me.TextTotalN.TabIndex = 315
        Me.TextTotalN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(839, 16)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 314
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(413, 587)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 13)
        Me.Label4.TabIndex = 311
        Me.Label4.Text = "Totales"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotalB
        '
        Me.TextTotalB.BackColor = System.Drawing.Color.White
        Me.TextTotalB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalB.Location = New System.Drawing.Point(469, 584)
        Me.TextTotalB.MaxLength = 30
        Me.TextTotalB.Name = "TextTotalB"
        Me.TextTotalB.ReadOnly = True
        Me.TextTotalB.Size = New System.Drawing.Size(96, 20)
        Me.TextTotalB.TabIndex = 310
        Me.TextTotalB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(788, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 313
        Me.Label8.Text = "Estado"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PaneLotes
        '
        Me.PaneLotes.BackColor = System.Drawing.Color.Transparent
        Me.PaneLotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PaneLotes.Controls.Add(Me.ButtonNetoPorLotes)
        Me.PaneLotes.Controls.Add(Me.ButtonLotesAImputar)
        Me.PaneLotes.Location = New System.Drawing.Point(14, 186)
        Me.PaneLotes.Name = "PaneLotes"
        Me.PaneLotes.Size = New System.Drawing.Size(961, 32)
        Me.PaneLotes.TabIndex = 327
        '
        'ButtonNetoPorLotes
        '
        Me.ButtonNetoPorLotes.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ButtonNetoPorLotes.FlatAppearance.BorderSize = 0
        Me.ButtonNetoPorLotes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonNetoPorLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNetoPorLotes.Location = New System.Drawing.Point(822, 4)
        Me.ButtonNetoPorLotes.Name = "ButtonNetoPorLotes"
        Me.ButtonNetoPorLotes.Size = New System.Drawing.Size(125, 20)
        Me.ButtonNetoPorLotes.TabIndex = 1010
        Me.ButtonNetoPorLotes.TabStop = False
        Me.ButtonNetoPorLotes.Text = "Importe Por Lotes"
        Me.ButtonNetoPorLotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNetoPorLotes.UseVisualStyleBackColor = False
        '
        'ButtonLotesAImputar
        '
        Me.ButtonLotesAImputar.BackColor = System.Drawing.Color.RosyBrown
        Me.ButtonLotesAImputar.FlatAppearance.BorderSize = 0
        Me.ButtonLotesAImputar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonLotesAImputar.Location = New System.Drawing.Point(343, 2)
        Me.ButtonLotesAImputar.Name = "ButtonLotesAImputar"
        Me.ButtonLotesAImputar.Size = New System.Drawing.Size(259, 27)
        Me.ButtonLotesAImputar.TabIndex = 1009
        Me.ButtonLotesAImputar.TabStop = False
        Me.ButtonLotesAImputar.Text = "Lotes a Imputar"
        Me.ButtonLotesAImputar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonLotesAImputar.UseVisualStyleBackColor = False
        '
        'ButtonNuevaIgualProveedor
        '
        Me.ButtonNuevaIgualProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevaIgualProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaIgualProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevaIgualProveedor.Location = New System.Drawing.Point(212, 647)
        Me.ButtonNuevaIgualProveedor.Name = "ButtonNuevaIgualProveedor"
        Me.ButtonNuevaIgualProveedor.Size = New System.Drawing.Size(187, 29)
        Me.ButtonNuevaIgualProveedor.TabIndex = 326
        Me.ButtonNuevaIgualProveedor.TabStop = False
        Me.ButtonNuevaIgualProveedor.Text = "Nueva Fact. Igual Proveedor"
        Me.ButtonNuevaIgualProveedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaIgualProveedor.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(615, 645)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(167, 29)
        Me.ButtonAsientoContable.TabIndex = 324
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(421, 646)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(167, 29)
        Me.ButtonAnula.TabIndex = 312
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anula Factura"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(808, 644)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(167, 29)
        Me.ButtonAceptar.TabIndex = 308
        Me.ButtonAceptar.Text = "Graba Factura"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(14, 647)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(167, 29)
        Me.ButtonNuevo.TabIndex = 309
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nueva Factura"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'UnaFacturaProveedorFondoFijo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(997, 681)
        Me.Controls.Add(Me.PaneLotes)
        Me.Controls.Add(Me.ButtonNuevaIgualProveedor)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.MaskedFacturaN)
        Me.Controls.Add(Me.MaskedFactura)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextTipoFactura)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.TextTotalN)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextTotalB)
        Me.Controls.Add(Me.Label8)
        Me.Name = "UnaFacturaProveedorFondoFijo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rendicion Fondo Fijo"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueFactura, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PaneLotes.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents MaskedReciboOficial As System.Windows.Forms.MaskedTextBox
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboPais As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents ComboConceptoGasto As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CheckSecos As System.Windows.Forms.CheckBox
    Friend WithEvents PictureAlmanaqueContable As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaContable As System.Windows.Forms.TextBox
    Friend WithEvents TextFechaFactura As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueFactura As System.Windows.Forms.PictureBox
    Friend WithEvents LabelCuentas As System.Windows.Forms.Label
    Friend WithEvents PictureLupaCuenta As System.Windows.Forms.PictureBox
    Friend WithEvents ListCuentas As System.Windows.Forms.ListView
    Friend WithEvents Cuenta As System.Windows.Forms.ColumnHeader
    Friend WithEvents Importe1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Importe2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents ButtonNuevaIgualProveedor As System.Windows.Forms.Button
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ComboTipoIva As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextCuit As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents MaskedFacturaN As System.Windows.Forms.MaskedTextBox
    Friend WithEvents MaskedFactura As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextTipoFactura As System.Windows.Forms.TextBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents TextTotalN As System.Windows.Forms.TextBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextTotalB As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents TextNombreFondoFijo As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents TextRendicion As System.Windows.Forms.TextBox
    Friend WithEvents PaneLotes As System.Windows.Forms.Panel
    Friend WithEvents ButtonNetoPorLotes As System.Windows.Forms.Button
    Friend WithEvents ButtonLotesAImputar As System.Windows.Forms.Button
    Friend WithEvents ButtonDatosEmisor As System.Windows.Forms.Button
    Friend WithEvents Clave As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TieneLupa As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Alicuota As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImporteB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImporteN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lupa As System.Windows.Forms.DataGridViewImageColumn
End Class
