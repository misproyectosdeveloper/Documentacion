<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnMovimientoBancario
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
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnMovimientoBancario))
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.MaskedMovimiento = New System.Windows.Forms.MaskedTextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PanelMoneda = New System.Windows.Forms.Panel
        Me.TextCambio = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.PanelGrid = New System.Windows.Forms.Panel
        Me.PanelOperacionCheque = New System.Windows.Forms.Panel
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.PanelRetiro = New System.Windows.Forms.Panel
        Me.LabelMonedaRetiro = New System.Windows.Forms.Label
        Me.ButtonLupaCuentaRetiro = New System.Windows.Forms.Button
        Me.TextCuentaRetiro = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.ComboBancoRetiro = New System.Windows.Forms.ComboBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.TextTotalComprobante = New System.Windows.Forms.TextBox
        Me.LabelImporte = New System.Windows.Forms.Label
        Me.PanelDeposito = New System.Windows.Forms.Panel
        Me.LabelMonedaDeposito = New System.Windows.Forms.Label
        Me.ButtonLupaCuentaDeposito = New System.Windows.Forms.Button
        Me.TextCuentaDeposito = New System.Windows.Forms.TextBox
        Me.LabelCuenta = New System.Windows.Forms.Label
        Me.LabelBanco = New System.Windows.Forms.Label
        Me.ComboBancoDeposito = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextComprobante = New System.Windows.Forms.TextBox
        Me.DateTime2 = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.LabelCaja = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.NumeracionInicial = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.eCheq = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.NumeracionFinal = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Banco = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.LupaCuenta = New System.Windows.Forms.DataGridViewImageColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Serie = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Numero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EmisorCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ClaveCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel2.SuspendLayout()
        Me.PanelMoneda.SuspendLayout()
        Me.PanelGrid.SuspendLayout()
        Me.PanelOperacionCheque.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelRetiro.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.PanelDeposito.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MaskedMovimiento
        '
        Me.MaskedMovimiento.BackColor = System.Drawing.Color.White
        Me.MaskedMovimiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedMovimiento.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedMovimiento.Location = New System.Drawing.Point(146, 60)
        Me.MaskedMovimiento.Mask = "0000-00000000"
        Me.MaskedMovimiento.Name = "MaskedMovimiento"
        Me.MaskedMovimiento.ReadOnly = True
        Me.MaskedMovimiento.Size = New System.Drawing.Size(114, 21)
        Me.MaskedMovimiento.TabIndex = 1017
        Me.MaskedMovimiento.TabStop = False
        Me.MaskedMovimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedMovimiento.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(648, 61)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 1016
        Me.Label8.Text = "Estado"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.Wheat
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(702, 58)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 1015
        Me.ComboEstado.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(67, 64)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(71, 13)
        Me.Label2.TabIndex = 1014
        Me.Label2.Text = "Movimiento"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(445, 62)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 1012
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(493, 59)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(105, 20)
        Me.DateTime1.TabIndex = 1013
        Me.DateTime1.TabStop = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(158, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(574, 18)
        Me.Label1.TabIndex = 1011
        Me.Label1.Text = "Label1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightGreen
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.PanelMoneda)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.TextComentario)
        Me.Panel2.Controls.Add(Me.PanelGrid)
        Me.Panel2.Controls.Add(Me.PanelRetiro)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.PanelDeposito)
        Me.Panel2.Controls.Add(Me.Panel1)
        Me.Panel2.Location = New System.Drawing.Point(65, 85)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(767, 479)
        Me.Panel2.TabIndex = 1
        Me.Panel2.TabStop = True
        '
        'PanelMoneda
        '
        Me.PanelMoneda.BackColor = System.Drawing.Color.Plum
        Me.PanelMoneda.Controls.Add(Me.TextCambio)
        Me.PanelMoneda.Controls.Add(Me.Label18)
        Me.PanelMoneda.Location = New System.Drawing.Point(265, 85)
        Me.PanelMoneda.Name = "PanelMoneda"
        Me.PanelMoneda.Size = New System.Drawing.Size(259, 31)
        Me.PanelMoneda.TabIndex = 1007
        '
        'TextCambio
        '
        Me.TextCambio.BackColor = System.Drawing.Color.White
        Me.TextCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCambio.Location = New System.Drawing.Point(130, 4)
        Me.TextCambio.MaxLength = 10
        Me.TextCambio.Name = "TextCambio"
        Me.TextCambio.Size = New System.Drawing.Size(59, 21)
        Me.TextCambio.TabIndex = 4
        Me.TextCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(66, 7)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(56, 15)
        Me.Label18.TabIndex = 142
        Me.Label18.Text = "Cambio"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(215, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 13)
        Me.Label10.TabIndex = 27
        Me.Label10.Text = "Comentario"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(286, 9)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(282, 20)
        Me.TextComentario.TabIndex = 26
        Me.TextComentario.TabStop = False
        '
        'PanelGrid
        '
        Me.PanelGrid.Controls.Add(Me.PanelOperacionCheque)
        Me.PanelGrid.Controls.Add(Me.ButtonEliminarLinea)
        Me.PanelGrid.Controls.Add(Me.Grid)
        Me.PanelGrid.Location = New System.Drawing.Point(6, 227)
        Me.PanelGrid.Name = "PanelGrid"
        Me.PanelGrid.Size = New System.Drawing.Size(756, 190)
        Me.PanelGrid.TabIndex = 22
        Me.PanelGrid.Visible = False
        '
        'PanelOperacionCheque
        '
        Me.PanelOperacionCheque.Controls.Add(Me.PictureCandado)
        Me.PanelOperacionCheque.Controls.Add(Me.Label5)
        Me.PanelOperacionCheque.Location = New System.Drawing.Point(521, 6)
        Me.PanelOperacionCheque.Name = "PanelOperacionCheque"
        Me.PanelOperacionCheque.Size = New System.Drawing.Size(212, 31)
        Me.PanelOperacionCheque.TabIndex = 100005
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(170, 2)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(25, 25)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 126
        Me.PictureCandado.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(61, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(101, 13)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Operación Cheques"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(8, 166)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(98, 20)
        Me.ButtonEliminarLinea.TabIndex = 100003
        Me.ButtonEliminarLinea.TabStop = False
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.NumeracionInicial, Me.eCheq, Me.NumeracionFinal, Me.Operacion, Me.Importe, Me.Banco, Me.LupaCuenta, Me.Cuenta, Me.Serie, Me.Numero, Me.EmisorCheque, Me.Fecha, Me.ClaveCheque})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle15
        Me.Grid.Location = New System.Drawing.Point(5, 39)
        Me.Grid.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid.Name = "Grid"
        Me.Grid.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle16.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Grid.Size = New System.Drawing.Size(740, 124)
        Me.Grid.TabIndex = 24
        '
        'PanelRetiro
        '
        Me.PanelRetiro.BackColor = System.Drawing.Color.White
        Me.PanelRetiro.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelRetiro.Controls.Add(Me.LabelMonedaRetiro)
        Me.PanelRetiro.Controls.Add(Me.ButtonLupaCuentaRetiro)
        Me.PanelRetiro.Controls.Add(Me.TextCuentaRetiro)
        Me.PanelRetiro.Controls.Add(Me.Label11)
        Me.PanelRetiro.Controls.Add(Me.Label12)
        Me.PanelRetiro.Controls.Add(Me.ComboBancoRetiro)
        Me.PanelRetiro.Location = New System.Drawing.Point(112, 178)
        Me.PanelRetiro.Name = "PanelRetiro"
        Me.PanelRetiro.Size = New System.Drawing.Size(542, 44)
        Me.PanelRetiro.TabIndex = 8
        Me.PanelRetiro.Visible = False
        '
        'LabelMonedaRetiro
        '
        Me.LabelMonedaRetiro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMonedaRetiro.Location = New System.Drawing.Point(284, 24)
        Me.LabelMonedaRetiro.Name = "LabelMonedaRetiro"
        Me.LabelMonedaRetiro.Size = New System.Drawing.Size(89, 11)
        Me.LabelMonedaRetiro.TabIndex = 132
        Me.LabelMonedaRetiro.Text = "( Moneda )"
        Me.LabelMonedaRetiro.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonLupaCuentaRetiro
        '
        Me.ButtonLupaCuentaRetiro.BackColor = System.Drawing.Color.White
        Me.ButtonLupaCuentaRetiro.FlatAppearance.BorderSize = 0
        Me.ButtonLupaCuentaRetiro.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonLupaCuentaRetiro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonLupaCuentaRetiro.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonLupaCuentaRetiro.Location = New System.Drawing.Point(482, 1)
        Me.ButtonLupaCuentaRetiro.Name = "ButtonLupaCuentaRetiro"
        Me.ButtonLupaCuentaRetiro.Size = New System.Drawing.Size(30, 30)
        Me.ButtonLupaCuentaRetiro.TabIndex = 10
        Me.ButtonLupaCuentaRetiro.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonLupaCuentaRetiro.UseVisualStyleBackColor = False
        '
        'TextCuentaRetiro
        '
        Me.TextCuentaRetiro.BackColor = System.Drawing.Color.White
        Me.TextCuentaRetiro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuentaRetiro.Location = New System.Drawing.Point(379, 7)
        Me.TextCuentaRetiro.MaxLength = 12
        Me.TextCuentaRetiro.Name = "TextCuentaRetiro"
        Me.TextCuentaRetiro.ReadOnly = True
        Me.TextCuentaRetiro.Size = New System.Drawing.Size(94, 20)
        Me.TextCuentaRetiro.TabIndex = 7
        Me.TextCuentaRetiro.TabStop = False
        Me.TextCuentaRetiro.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(301, 10)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(47, 13)
        Me.Label11.TabIndex = 131
        Me.Label11.Text = "Cuenta"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(7, 10)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(81, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Banco Retiro"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboBancoRetiro
        '
        Me.ComboBancoRetiro.BackColor = System.Drawing.Color.White
        Me.ComboBancoRetiro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBancoRetiro.Enabled = False
        Me.ComboBancoRetiro.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancoRetiro.FormattingEnabled = True
        Me.ComboBancoRetiro.Location = New System.Drawing.Point(142, 6)
        Me.ComboBancoRetiro.Name = "ComboBancoRetiro"
        Me.ComboBancoRetiro.Size = New System.Drawing.Size(127, 21)
        Me.ComboBancoRetiro.TabIndex = 9
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.White
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.TextTotalComprobante)
        Me.Panel4.Controls.Add(Me.LabelImporte)
        Me.Panel4.Location = New System.Drawing.Point(251, 422)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(282, 29)
        Me.Panel4.TabIndex = 25
        '
        'TextTotalComprobante
        '
        Me.TextTotalComprobante.BackColor = System.Drawing.Color.White
        Me.TextTotalComprobante.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalComprobante.Location = New System.Drawing.Point(141, 4)
        Me.TextTotalComprobante.MaxLength = 12
        Me.TextTotalComprobante.Name = "TextTotalComprobante"
        Me.TextTotalComprobante.Size = New System.Drawing.Size(111, 20)
        Me.TextTotalComprobante.TabIndex = 26
        Me.TextTotalComprobante.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelImporte
        '
        Me.LabelImporte.AutoSize = True
        Me.LabelImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelImporte.Location = New System.Drawing.Point(3, 7)
        Me.LabelImporte.Name = "LabelImporte"
        Me.LabelImporte.Size = New System.Drawing.Size(114, 13)
        Me.LabelImporte.TabIndex = 100006
        Me.LabelImporte.Text = "Total Comprobante"
        Me.LabelImporte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelDeposito
        '
        Me.PanelDeposito.BackColor = System.Drawing.Color.White
        Me.PanelDeposito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelDeposito.Controls.Add(Me.LabelMonedaDeposito)
        Me.PanelDeposito.Controls.Add(Me.ButtonLupaCuentaDeposito)
        Me.PanelDeposito.Controls.Add(Me.TextCuentaDeposito)
        Me.PanelDeposito.Controls.Add(Me.LabelCuenta)
        Me.PanelDeposito.Controls.Add(Me.LabelBanco)
        Me.PanelDeposito.Controls.Add(Me.ComboBancoDeposito)
        Me.PanelDeposito.Location = New System.Drawing.Point(112, 129)
        Me.PanelDeposito.Name = "PanelDeposito"
        Me.PanelDeposito.Size = New System.Drawing.Size(542, 44)
        Me.PanelDeposito.TabIndex = 5
        Me.PanelDeposito.TabStop = True
        Me.PanelDeposito.Visible = False
        '
        'LabelMonedaDeposito
        '
        Me.LabelMonedaDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMonedaDeposito.Location = New System.Drawing.Point(284, 23)
        Me.LabelMonedaDeposito.Name = "LabelMonedaDeposito"
        Me.LabelMonedaDeposito.Size = New System.Drawing.Size(89, 11)
        Me.LabelMonedaDeposito.TabIndex = 132
        Me.LabelMonedaDeposito.Text = "( Moneda )"
        Me.LabelMonedaDeposito.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonLupaCuentaDeposito
        '
        Me.ButtonLupaCuentaDeposito.BackColor = System.Drawing.Color.White
        Me.ButtonLupaCuentaDeposito.FlatAppearance.BorderSize = 0
        Me.ButtonLupaCuentaDeposito.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonLupaCuentaDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonLupaCuentaDeposito.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonLupaCuentaDeposito.Location = New System.Drawing.Point(482, 1)
        Me.ButtonLupaCuentaDeposito.Name = "ButtonLupaCuentaDeposito"
        Me.ButtonLupaCuentaDeposito.Size = New System.Drawing.Size(30, 30)
        Me.ButtonLupaCuentaDeposito.TabIndex = 7
        Me.ButtonLupaCuentaDeposito.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonLupaCuentaDeposito.UseVisualStyleBackColor = False
        '
        'TextCuentaDeposito
        '
        Me.TextCuentaDeposito.BackColor = System.Drawing.Color.White
        Me.TextCuentaDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuentaDeposito.Location = New System.Drawing.Point(379, 6)
        Me.TextCuentaDeposito.MaxLength = 12
        Me.TextCuentaDeposito.Name = "TextCuentaDeposito"
        Me.TextCuentaDeposito.ReadOnly = True
        Me.TextCuentaDeposito.Size = New System.Drawing.Size(94, 20)
        Me.TextCuentaDeposito.TabIndex = 6
        Me.TextCuentaDeposito.TabStop = False
        Me.TextCuentaDeposito.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelCuenta
        '
        Me.LabelCuenta.AutoSize = True
        Me.LabelCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCuenta.Location = New System.Drawing.Point(300, 9)
        Me.LabelCuenta.Name = "LabelCuenta"
        Me.LabelCuenta.Size = New System.Drawing.Size(51, 13)
        Me.LabelCuenta.TabIndex = 131
        Me.LabelCuenta.Text = "Cuenta "
        Me.LabelCuenta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelBanco
        '
        Me.LabelBanco.AutoSize = True
        Me.LabelBanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelBanco.Location = New System.Drawing.Point(7, 9)
        Me.LabelBanco.Name = "LabelBanco"
        Me.LabelBanco.Size = New System.Drawing.Size(97, 13)
        Me.LabelBanco.TabIndex = 4
        Me.LabelBanco.Text = "Banco Deposito"
        Me.LabelBanco.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboBancoDeposito
        '
        Me.ComboBancoDeposito.BackColor = System.Drawing.Color.White
        Me.ComboBancoDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBancoDeposito.Enabled = False
        Me.ComboBancoDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancoDeposito.FormattingEnabled = True
        Me.ComboBancoDeposito.Location = New System.Drawing.Point(142, 5)
        Me.ComboBancoDeposito.Name = "ComboBancoDeposito"
        Me.ComboBancoDeposito.Size = New System.Drawing.Size(127, 21)
        Me.ComboBancoDeposito.TabIndex = 5
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TextComprobante)
        Me.Panel1.Controls.Add(Me.DateTime2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(90, 49)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(590, 26)
        Me.Panel1.TabIndex = 1
        '
        'TextComprobante
        '
        Me.TextComprobante.BackColor = System.Drawing.Color.White
        Me.TextComprobante.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComprobante.Location = New System.Drawing.Point(169, 4)
        Me.TextComprobante.MaxLength = 9
        Me.TextComprobante.Name = "TextComprobante"
        Me.TextComprobante.Size = New System.Drawing.Size(128, 20)
        Me.TextComprobante.TabIndex = 2
        Me.TextComprobante.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'DateTime2
        '
        Me.DateTime2.CustomFormat = "dd/MM/yyyy"
        Me.DateTime2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime2.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime2.Location = New System.Drawing.Point(459, 4)
        Me.DateTime2.Name = "DateTime2"
        Me.DateTime2.Size = New System.Drawing.Size(107, 20)
        Me.DateTime2.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(324, 7)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(120, 13)
        Me.Label4.TabIndex = 1011
        Me.Label4.Text = "Fecha Comprobante"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(30, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 13)
        Me.Label3.TabIndex = 1
        Me.Label3.Text = "Numero Comprobante"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = ""
        Me.DataGridViewImageColumn1.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.MinimumWidth = 30
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 30
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(60, 607)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(135, 35)
        Me.ButtonNuevo.TabIndex = 45
        Me.ButtonNuevo.Text = "Nuevo Movimiento"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(364, 607)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(135, 35)
        Me.ButtonAnula.TabIndex = 1021
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anular Nota"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(667, 607)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(135, 35)
        Me.ButtonAceptar.TabIndex = 40
        Me.ButtonAceptar.Text = "Graba Movimiento "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(512, 607)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(140, 35)
        Me.ButtonAsientoContable.TabIndex = 1022
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(211, 607)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(135, 35)
        Me.ButtonImprimir.TabIndex = 1023
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'LabelCaja
        '
        Me.LabelCaja.AutoSize = True
        Me.LabelCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaja.Location = New System.Drawing.Point(350, 59)
        Me.LabelCaja.Name = "LabelCaja"
        Me.LabelCaja.Size = New System.Drawing.Size(47, 18)
        Me.LabelCaja.TabIndex = 1025
        Me.LabelCaja.Text = "Caja "
        Me.LabelCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(299, 59)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 15)
        Me.Label13.TabIndex = 1024
        Me.Label13.Text = "Caja "
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'NumeracionInicial
        '
        Me.NumeracionInicial.DataPropertyName = "NumeracionInicial"
        Me.NumeracionInicial.HeaderText = "NumeracioInicial"
        Me.NumeracionInicial.Name = "NumeracionInicial"
        Me.NumeracionInicial.Visible = False
        '
        'eCheq
        '
        Me.eCheq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.eCheq.DataPropertyName = "eCheq"
        Me.eCheq.HeaderText = "eCheq"
        Me.eCheq.MinimumWidth = 40
        Me.eCheq.Name = "eCheq"
        Me.eCheq.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.eCheq.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.eCheq.Width = 40
        '
        'NumeracionFinal
        '
        Me.NumeracionFinal.DataPropertyName = "NumeracionFinal"
        Me.NumeracionFinal.HeaderText = "NumeracioFinal"
        Me.NumeracionFinal.Name = "NumeracionFinal"
        Me.NumeracionFinal.Visible = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle9
        Me.Importe.HeaderText = "Importe "
        Me.Importe.MaxInputLength = 12
        Me.Importe.MinimumWidth = 80
        Me.Importe.Name = "Importe"
        Me.Importe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Importe.Width = 80
        '
        'Banco
        '
        Me.Banco.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Banco.DataPropertyName = "Banco"
        Me.Banco.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Banco.HeaderText = "Banco"
        Me.Banco.MinimumWidth = 90
        Me.Banco.Name = "Banco"
        Me.Banco.ReadOnly = True
        Me.Banco.Width = 90
        '
        'LupaCuenta
        '
        Me.LupaCuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.LupaCuenta.HeaderText = ""
        Me.LupaCuenta.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.LupaCuenta.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.LupaCuenta.MinimumWidth = 30
        Me.LupaCuenta.Name = "LupaCuenta"
        Me.LupaCuenta.ReadOnly = True
        Me.LupaCuenta.Width = 30
        '
        'Cuenta
        '
        Me.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuenta.DataPropertyName = "Cuenta"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cuenta.DefaultCellStyle = DataGridViewCellStyle10
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MaxInputLength = 8
        Me.Cuenta.MinimumWidth = 100
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Cuenta.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Serie
        '
        Me.Serie.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Serie.DataPropertyName = "Serie"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Serie.DefaultCellStyle = DataGridViewCellStyle11
        Me.Serie.HeaderText = "Serie"
        Me.Serie.MaxInputLength = 1
        Me.Serie.MinimumWidth = 35
        Me.Serie.Name = "Serie"
        Me.Serie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Serie.Width = 35
        '
        'Numero
        '
        Me.Numero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Numero.DataPropertyName = "Numero"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Numero.DefaultCellStyle = DataGridViewCellStyle12
        Me.Numero.HeaderText = "Numero"
        Me.Numero.MaxInputLength = 10
        Me.Numero.MinimumWidth = 80
        Me.Numero.Name = "Numero"
        Me.Numero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Numero.Width = 80
        '
        'EmisorCheque
        '
        Me.EmisorCheque.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.EmisorCheque.DataPropertyName = "EmisorCheque"
        Me.EmisorCheque.HeaderText = "Emisor"
        Me.EmisorCheque.MaxInputLength = 30
        Me.EmisorCheque.MinimumWidth = 100
        Me.EmisorCheque.Name = "EmisorCheque"
        Me.EmisorCheque.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle13
        Me.Fecha.HeaderText = "Vencimiento"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Fecha.Width = 70
        '
        'ClaveCheque
        '
        Me.ClaveCheque.DataPropertyName = "ClaveCheque"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle14.ForeColor = System.Drawing.Color.Red
        Me.ClaveCheque.DefaultCellStyle = DataGridViewCellStyle14
        Me.ClaveCheque.HeaderText = "Clave Cheque"
        Me.ClaveCheque.MinimumWidth = 100
        Me.ClaveCheque.Name = "ClaveCheque"
        Me.ClaveCheque.ReadOnly = True
        '
        'UnMovimientoBancario
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(868, 653)
        Me.Controls.Add(Me.LabelCaja)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.MaskedMovimiento)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.Label1)
        Me.Name = "UnMovimientoBancario"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Movimiento Bancario"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.PanelMoneda.ResumeLayout(False)
        Me.PanelMoneda.PerformLayout()
        Me.PanelGrid.ResumeLayout(False)
        Me.PanelOperacionCheque.ResumeLayout(False)
        Me.PanelOperacionCheque.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelRetiro.ResumeLayout(False)
        Me.PanelRetiro.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.PanelDeposito.ResumeLayout(False)
        Me.PanelDeposito.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MaskedMovimiento As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents LabelImporte As System.Windows.Forms.Label
    Friend WithEvents TextTotalComprobante As System.Windows.Forms.TextBox
    Friend WithEvents PanelDeposito As System.Windows.Forms.Panel
    Friend WithEvents LabelMonedaDeposito As System.Windows.Forms.Label
    Friend WithEvents ButtonLupaCuentaDeposito As System.Windows.Forms.Button
    Friend WithEvents TextCuentaDeposito As System.Windows.Forms.TextBox
    Friend WithEvents LabelCuenta As System.Windows.Forms.Label
    Friend WithEvents LabelBanco As System.Windows.Forms.Label
    Friend WithEvents ComboBancoDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextComprobante As System.Windows.Forms.TextBox
    Friend WithEvents DateTime2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PanelRetiro As System.Windows.Forms.Panel
    Friend WithEvents LabelMonedaRetiro As System.Windows.Forms.Label
    Friend WithEvents ButtonLupaCuentaRetiro As System.Windows.Forms.Button
    Friend WithEvents TextCuentaRetiro As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ComboBancoRetiro As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PanelGrid As System.Windows.Forms.Panel
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents PanelOperacionCheque As System.Windows.Forms.Panel
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents PanelMoneda As System.Windows.Forms.Panel
    Friend WithEvents TextCambio As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents LabelCaja As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents NumeracionInicial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents eCheq As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents NumeracionFinal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Banco As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents LupaCuenta As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Serie As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Numero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmisorCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ClaveCheque As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
