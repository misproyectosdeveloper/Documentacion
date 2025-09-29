<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaCompraDivisas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaCompraDivisas))
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureAlmanaqueContable = New System.Windows.Forms.PictureBox
        Me.TextFechaContable = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.PanelEmisor = New System.Windows.Forms.Panel
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioCliente = New System.Windows.Forms.RadioButton
        Me.RadioBancario = New System.Windows.Forms.RadioButton
        Me.RadioProveedor = New System.Windows.Forms.RadioButton
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextMovimiento = New System.Windows.Forms.TextBox
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextTotalPesos = New System.Windows.Forms.TextBox
        Me.ButtonEliminarTodo = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextTotalMediosPago = New System.Windows.Forms.TextBox
        Me.ButtonMediosDePago = New System.Windows.Forms.Button
        Me.PanelMoneda = New System.Windows.Forms.Panel
        Me.TextCambio = New System.Windows.Forms.TextBox
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.ButtonEliminarTodo1 = New System.Windows.Forms.Button
        Me.ButtonMediosDePago1 = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextTotalMediosPago1 = New System.Windows.Forms.TextBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.LabelCaja = New System.Windows.Forms.Label
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelEmisor.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.PanelMoneda.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(675, 13)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 188
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(624, 17)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 187
        Me.Label15.Text = "Estado"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureAlmanaqueContable)
        Me.Panel1.Controls.Add(Me.TextFechaContable)
        Me.Panel1.Controls.Add(Me.Label22)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.PanelEmisor)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Location = New System.Drawing.Point(14, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(769, 101)
        Me.Panel1.TabIndex = 182
        '
        'PictureAlmanaqueContable
        '
        Me.PictureAlmanaqueContable.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueContable.Location = New System.Drawing.Point(607, 5)
        Me.PictureAlmanaqueContable.Name = "PictureAlmanaqueContable"
        Me.PictureAlmanaqueContable.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueContable.TabIndex = 1012
        Me.PictureAlmanaqueContable.TabStop = False
        '
        'TextFechaContable
        '
        Me.TextFechaContable.BackColor = System.Drawing.Color.White
        Me.TextFechaContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaContable.Location = New System.Drawing.Point(519, 5)
        Me.TextFechaContable.MaxLength = 10
        Me.TextFechaContable.Name = "TextFechaContable"
        Me.TextFechaContable.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaContable.TabIndex = 1011
        Me.TextFechaContable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(416, 8)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(96, 13)
        Me.Label22.TabIndex = 1010
        Me.Label22.Text = "Fecha Contable"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(427, 75)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 20)
        Me.Label7.TabIndex = 188
        Me.Label7.Text = "Estado"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(689, 6)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(44, 48)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 138
        Me.PictureCandado.TabStop = False
        '
        'PanelEmisor
        '
        Me.PanelEmisor.BackColor = System.Drawing.Color.Gainsboro
        Me.PanelEmisor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelEmisor.Controls.Add(Me.ComboEmisor)
        Me.PanelEmisor.Controls.Add(Me.LabelEmisor)
        Me.PanelEmisor.Location = New System.Drawing.Point(340, 35)
        Me.PanelEmisor.Name = "PanelEmisor"
        Me.PanelEmisor.Size = New System.Drawing.Size(248, 28)
        Me.PanelEmisor.TabIndex = 4
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(91, 2)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(147, 21)
        Me.ComboEmisor.TabIndex = 5
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(4, 5)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(65, 13)
        Me.LabelEmisor.TabIndex = 8
        Me.LabelEmisor.Text = "Proveedor"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(18, 78)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(60, 13)
        Me.Label9.TabIndex = 132
        Me.Label9.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(86, 75)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(253, 20)
        Me.TextComentario.TabIndex = 16
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(127, 13)
        Me.Label1.TabIndex = 121
        Me.Label1.Text = "Comprador/Vendedor"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioCliente)
        Me.Panel3.Controls.Add(Me.RadioBancario)
        Me.Panel3.Controls.Add(Me.RadioProveedor)
        Me.Panel3.Location = New System.Drawing.Point(21, 36)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(298, 28)
        Me.Panel3.TabIndex = 1
        '
        'RadioCliente
        '
        Me.RadioCliente.AutoSize = True
        Me.RadioCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioCliente.Location = New System.Drawing.Point(209, 4)
        Me.RadioCliente.Name = "RadioCliente"
        Me.RadioCliente.Size = New System.Drawing.Size(64, 17)
        Me.RadioCliente.TabIndex = 3
        Me.RadioCliente.Text = "Cliente"
        Me.RadioCliente.UseVisualStyleBackColor = True
        '
        'RadioBancario
        '
        Me.RadioBancario.AutoSize = True
        Me.RadioBancario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioBancario.Location = New System.Drawing.Point(3, 5)
        Me.RadioBancario.Name = "RadioBancario"
        Me.RadioBancario.Size = New System.Drawing.Size(75, 17)
        Me.RadioBancario.TabIndex = 1
        Me.RadioBancario.Text = "Bancario"
        Me.RadioBancario.UseVisualStyleBackColor = True
        '
        'RadioProveedor
        '
        Me.RadioProveedor.AutoSize = True
        Me.RadioProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioProveedor.Location = New System.Drawing.Point(104, 4)
        Me.RadioProveedor.Name = "RadioProveedor"
        Me.RadioProveedor.Size = New System.Drawing.Size(83, 17)
        Me.RadioProveedor.TabIndex = 2
        Me.RadioProveedor.Text = "Proveedor"
        Me.RadioProveedor.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(433, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 186
        Me.Label10.Text = "Fecha "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(14, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 185
        Me.Label6.Text = "Comprobante"
        '
        'TextMovimiento
        '
        Me.TextMovimiento.BackColor = System.Drawing.Color.White
        Me.TextMovimiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMovimiento.Location = New System.Drawing.Point(105, 15)
        Me.TextMovimiento.MaxLength = 8
        Me.TextMovimiento.Name = "TextMovimiento"
        Me.TextMovimiento.ReadOnly = True
        Me.TextMovimiento.Size = New System.Drawing.Size(130, 20)
        Me.TextMovimiento.TabIndex = 184
        Me.TextMovimiento.TabStop = False
        Me.TextMovimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(482, 13)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 183
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.Label2)
        Me.Panel5.Controls.Add(Me.TextTotalPesos)
        Me.Panel5.Controls.Add(Me.ButtonEliminarTodo)
        Me.Panel5.Controls.Add(Me.Label4)
        Me.Panel5.Controls.Add(Me.TextTotalMediosPago)
        Me.Panel5.Controls.Add(Me.ButtonMediosDePago)
        Me.Panel5.Controls.Add(Me.PanelMoneda)
        Me.Panel5.Location = New System.Drawing.Point(15, 162)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(768, 179)
        Me.Panel5.TabIndex = 1008
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(396, 142)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(82, 15)
        Me.Label2.TabIndex = 1013
        Me.Label2.Text = "Total Pesos"
        '
        'TextTotalPesos
        '
        Me.TextTotalPesos.BackColor = System.Drawing.Color.White
        Me.TextTotalPesos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalPesos.Location = New System.Drawing.Point(486, 139)
        Me.TextTotalPesos.MaxLength = 5
        Me.TextTotalPesos.Name = "TextTotalPesos"
        Me.TextTotalPesos.ReadOnly = True
        Me.TextTotalPesos.Size = New System.Drawing.Size(101, 21)
        Me.TextTotalPesos.TabIndex = 1012
        Me.TextTotalPesos.TabStop = False
        Me.TextTotalPesos.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonEliminarTodo
        '
        Me.ButtonEliminarTodo.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarTodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodo.Location = New System.Drawing.Point(153, 100)
        Me.ButtonEliminarTodo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodo.Name = "ButtonEliminarTodo"
        Me.ButtonEliminarTodo.Size = New System.Drawing.Size(113, 22)
        Me.ButtonEliminarTodo.TabIndex = 1011
        Me.ButtonEliminarTodo.TabStop = False
        Me.ButtonEliminarTodo.Text = "Borrar Todo"
        Me.ButtonEliminarTodo.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(196, 143)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(82, 15)
        Me.Label4.TabIndex = 1010
        Me.Label4.Text = "Total Divisa"
        '
        'TextTotalMediosPago
        '
        Me.TextTotalMediosPago.BackColor = System.Drawing.Color.White
        Me.TextTotalMediosPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalMediosPago.Location = New System.Drawing.Point(285, 140)
        Me.TextTotalMediosPago.MaxLength = 5
        Me.TextTotalMediosPago.Name = "TextTotalMediosPago"
        Me.TextTotalMediosPago.ReadOnly = True
        Me.TextTotalMediosPago.Size = New System.Drawing.Size(101, 21)
        Me.TextTotalMediosPago.TabIndex = 1009
        Me.TextTotalMediosPago.TabStop = False
        Me.TextTotalMediosPago.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonMediosDePago
        '
        Me.ButtonMediosDePago.BackColor = System.Drawing.Color.LightGray
        Me.ButtonMediosDePago.FlatAppearance.BorderSize = 0
        Me.ButtonMediosDePago.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMediosDePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMediosDePago.Location = New System.Drawing.Point(157, 48)
        Me.ButtonMediosDePago.Name = "ButtonMediosDePago"
        Me.ButtonMediosDePago.Size = New System.Drawing.Size(459, 51)
        Me.ButtonMediosDePago.TabIndex = 1008
        Me.ButtonMediosDePago.TabStop = False
        Me.ButtonMediosDePago.Text = "Informar o Ver Conceptos"
        Me.ButtonMediosDePago.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonMediosDePago.UseVisualStyleBackColor = False
        '
        'PanelMoneda
        '
        Me.PanelMoneda.BackColor = System.Drawing.Color.Gainsboro
        Me.PanelMoneda.Controls.Add(Me.TextCambio)
        Me.PanelMoneda.Controls.Add(Me.ComboMoneda)
        Me.PanelMoneda.Controls.Add(Me.Label17)
        Me.PanelMoneda.Controls.Add(Me.Label18)
        Me.PanelMoneda.Location = New System.Drawing.Point(221, 8)
        Me.PanelMoneda.Name = "PanelMoneda"
        Me.PanelMoneda.Size = New System.Drawing.Size(333, 31)
        Me.PanelMoneda.TabIndex = 1007
        '
        'TextCambio
        '
        Me.TextCambio.BackColor = System.Drawing.Color.White
        Me.TextCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCambio.Location = New System.Drawing.Point(233, 4)
        Me.TextCambio.MaxLength = 10
        Me.TextCambio.Name = "TextCambio"
        Me.TextCambio.Size = New System.Drawing.Size(59, 21)
        Me.TextCambio.TabIndex = 4
        Me.TextCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.ForeColor = System.Drawing.Color.Black
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(79, 3)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(96, 23)
        Me.ComboMoneda.TabIndex = 144
        Me.ComboMoneda.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(16, 8)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(53, 15)
        Me.Label17.TabIndex = 143
        Me.Label17.Text = "Moneda"
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(178, 7)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(50, 15)
        Me.Label18.TabIndex = 142
        Me.Label18.Text = "Cambio"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.ButtonEliminarTodo1)
        Me.Panel6.Controls.Add(Me.ButtonMediosDePago1)
        Me.Panel6.Controls.Add(Me.Label5)
        Me.Panel6.Controls.Add(Me.TextTotalMediosPago1)
        Me.Panel6.Location = New System.Drawing.Point(15, 372)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(768, 156)
        Me.Panel6.TabIndex = 1009
        '
        'ButtonEliminarTodo1
        '
        Me.ButtonEliminarTodo1.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarTodo1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodo1.Location = New System.Drawing.Point(153, 64)
        Me.ButtonEliminarTodo1.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodo1.Name = "ButtonEliminarTodo1"
        Me.ButtonEliminarTodo1.Size = New System.Drawing.Size(113, 22)
        Me.ButtonEliminarTodo1.TabIndex = 1014
        Me.ButtonEliminarTodo1.TabStop = False
        Me.ButtonEliminarTodo1.Text = "Borrar Todo"
        Me.ButtonEliminarTodo1.UseVisualStyleBackColor = False
        '
        'ButtonMediosDePago1
        '
        Me.ButtonMediosDePago1.BackColor = System.Drawing.Color.LightGray
        Me.ButtonMediosDePago1.FlatAppearance.BorderSize = 0
        Me.ButtonMediosDePago1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMediosDePago1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMediosDePago1.Location = New System.Drawing.Point(156, 12)
        Me.ButtonMediosDePago1.Name = "ButtonMediosDePago1"
        Me.ButtonMediosDePago1.Size = New System.Drawing.Size(459, 51)
        Me.ButtonMediosDePago1.TabIndex = 1013
        Me.ButtonMediosDePago1.TabStop = False
        Me.ButtonMediosDePago1.Text = "Informar o Ver Conceptos"
        Me.ButtonMediosDePago1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonMediosDePago1.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(287, 98)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(82, 15)
        Me.Label5.TabIndex = 1012
        Me.Label5.Text = "Total Pesos"
        '
        'TextTotalMediosPago1
        '
        Me.TextTotalMediosPago1.BackColor = System.Drawing.Color.White
        Me.TextTotalMediosPago1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalMediosPago1.Location = New System.Drawing.Point(377, 95)
        Me.TextTotalMediosPago1.MaxLength = 5
        Me.TextTotalMediosPago1.Name = "TextTotalMediosPago1"
        Me.TextTotalMediosPago1.ReadOnly = True
        Me.TextTotalMediosPago1.Size = New System.Drawing.Size(125, 21)
        Me.TextTotalMediosPago1.TabIndex = 1011
        Me.TextTotalMediosPago1.TabStop = False
        Me.TextTotalMediosPago1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(616, 578)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(167, 36)
        Me.ButtonAceptar.TabIndex = 1010
        Me.ButtonAceptar.Text = "Graba Comprobante"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(275, 578)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(155, 36)
        Me.ButtonAnula.TabIndex = 1011
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anula Comprobante"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(276, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 15)
        Me.Label3.TabIndex = 1012
        Me.Label3.Text = "Caja "
        '
        'LabelCaja
        '
        Me.LabelCaja.AutoSize = True
        Me.LabelCaja.BackColor = System.Drawing.Color.White
        Me.LabelCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaja.Location = New System.Drawing.Point(330, 16)
        Me.LabelCaja.Name = "LabelCaja"
        Me.LabelCaja.Size = New System.Drawing.Size(40, 15)
        Me.LabelCaja.TabIndex = 1013
        Me.LabelCaja.Text = "Caja "
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(440, 579)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(167, 36)
        Me.ButtonAsientoContable.TabIndex = 1014
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(151, 577)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(115, 36)
        Me.ButtonImprimir.TabIndex = 1015
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.Location = New System.Drawing.Point(17, 577)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(115, 36)
        Me.ButtonNuevo.TabIndex = 1016
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nueva Compra/Venta"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'UnaCompraDivisas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(805, 635)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.LabelCaja)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextMovimiento)
        Me.Controls.Add(Me.DateTime1)
        Me.Name = "UnaCompraDivisas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Compra/Venta de Divisas"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelEmisor.ResumeLayout(False)
        Me.PanelEmisor.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.PanelMoneda.ResumeLayout(False)
        Me.PanelMoneda.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents PanelEmisor As System.Windows.Forms.Panel
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioCliente As System.Windows.Forms.RadioButton
    Friend WithEvents RadioBancario As System.Windows.Forms.RadioButton
    Friend WithEvents RadioProveedor As System.Windows.Forms.RadioButton
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextMovimiento As System.Windows.Forms.TextBox
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents PanelMoneda As System.Windows.Forms.Panel
    Friend WithEvents TextCambio As System.Windows.Forms.TextBox
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents ButtonMediosDePago As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextTotalMediosPago As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextTotalMediosPago1 As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonMediosDePago1 As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminarTodo As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminarTodo1 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextTotalPesos As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LabelCaja As System.Windows.Forms.Label
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents PictureAlmanaqueContable As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaContable As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
End Class
