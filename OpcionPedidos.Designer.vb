<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionPedidos
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
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextPedido = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextFechaHasta = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueHasta = New System.Windows.Forms.PictureBox
        Me.TextFechaDesde = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueDesde = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextPedidoCliente = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.CheckConSaldoPositivo = New System.Windows.Forms.CheckBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.RadioSinCantidades = New System.Windows.Forms.RadioButton
        Me.RadioConCantidad = New System.Windows.Forms.RadioButton
        Me.Label8 = New System.Windows.Forms.Label
        Me.CheckAbiertos = New System.Windows.Forms.CheckBox
        Me.CheckCerrados = New System.Windows.Forms.CheckBox
        Me.CheckConRepeticion = New System.Windows.Forms.CheckBox
        Me.CheckImportacionKrikos = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(323, 452)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(176, 26)
        Me.ButtonAceptar.TabIndex = 174
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ComboAlias
        '
        Me.ComboAlias.BackColor = System.Drawing.Color.White
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(286, 58)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(306, 28)
        Me.ComboAlias.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(132, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 173
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.BackColor = System.Drawing.Color.White
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(286, 23)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(306, 28)
        Me.ComboEmisor.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(133, 27)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(65, 20)
        Me.LabelEmisor.TabIndex = 172
        Me.LabelEmisor.Text = "Cliente"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TextPedido)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(125, 132)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(475, 40)
        Me.Panel1.TabIndex = 178
        '
        'TextPedido
        '
        Me.TextPedido.BackColor = System.Drawing.Color.White
        Me.TextPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPedido.Location = New System.Drawing.Point(163, 5)
        Me.TextPedido.MaxLength = 10
        Me.TextPedido.Name = "TextPedido"
        Me.TextPedido.Size = New System.Drawing.Size(146, 26)
        Me.TextPedido.TabIndex = 1009
        Me.TextPedido.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(8, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(64, 20)
        Me.Label4.TabIndex = 178
        Me.Label4.Text = "Pedido"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.TextFechaHasta)
        Me.Panel2.Controls.Add(Me.PictureAlmanaqueHasta)
        Me.Panel2.Controls.Add(Me.TextFechaDesde)
        Me.Panel2.Controls.Add(Me.PictureAlmanaqueDesde)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(43, 175)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(689, 67)
        Me.Panel2.TabIndex = 179
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(430, 43)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(221, 15)
        Me.Label7.TabIndex = 1013
        Me.Label7.Text = "(Si no se Informa asume Fecha Desde)"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(420, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(57, 20)
        Me.Label5.TabIndex = 1012
        Me.Label5.Text = "Hasta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextFechaHasta
        '
        Me.TextFechaHasta.BackColor = System.Drawing.Color.White
        Me.TextFechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaHasta.Location = New System.Drawing.Point(492, 7)
        Me.TextFechaHasta.MaxLength = 10
        Me.TextFechaHasta.Name = "TextFechaHasta"
        Me.TextFechaHasta.Size = New System.Drawing.Size(109, 26)
        Me.TextFechaHasta.TabIndex = 5
        Me.TextFechaHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueHasta
        '
        Me.PictureAlmanaqueHasta.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueHasta.Location = New System.Drawing.Point(604, 4)
        Me.PictureAlmanaqueHasta.Name = "PictureAlmanaqueHasta"
        Me.PictureAlmanaqueHasta.Size = New System.Drawing.Size(42, 34)
        Me.PictureAlmanaqueHasta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueHasta.TabIndex = 1011
        Me.PictureAlmanaqueHasta.TabStop = False
        '
        'TextFechaDesde
        '
        Me.TextFechaDesde.BackColor = System.Drawing.Color.White
        Me.TextFechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaDesde.Location = New System.Drawing.Point(246, 6)
        Me.TextFechaDesde.MaxLength = 10
        Me.TextFechaDesde.Name = "TextFechaDesde"
        Me.TextFechaDesde.Size = New System.Drawing.Size(111, 26)
        Me.TextFechaDesde.TabIndex = 4
        Me.TextFechaDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueDesde
        '
        Me.PictureAlmanaqueDesde.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueDesde.Location = New System.Drawing.Point(360, 3)
        Me.PictureAlmanaqueDesde.Name = "PictureAlmanaqueDesde"
        Me.PictureAlmanaqueDesde.Size = New System.Drawing.Size(42, 34)
        Me.PictureAlmanaqueDesde.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueDesde.TabIndex = 1009
        Me.PictureAlmanaqueDesde.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(10, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(195, 20)
        Me.Label3.TabIndex = 178
        Me.Label3.Text = "Fecha Entrega:  Desde"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.TextPedidoCliente)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(127, 248)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(475, 38)
        Me.Panel3.TabIndex = 180
        '
        'TextPedidoCliente
        '
        Me.TextPedidoCliente.BackColor = System.Drawing.Color.White
        Me.TextPedidoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPedidoCliente.Location = New System.Drawing.Point(145, 4)
        Me.TextPedidoCliente.MaxLength = 30
        Me.TextPedidoCliente.Name = "TextPedidoCliente"
        Me.TextPedidoCliente.Size = New System.Drawing.Size(320, 26)
        Me.TextPedidoCliente.TabIndex = 6
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(125, 20)
        Me.Label6.TabIndex = 178
        Me.Label6.Text = "Pedido Cliente"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.ComboSucursales)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Location = New System.Drawing.Point(126, 91)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(475, 40)
        Me.Panel4.TabIndex = 181
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(161, 5)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(306, 28)
        Me.ComboSucursales.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(7, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 20)
        Me.Label1.TabIndex = 178
        Me.Label1.Text = "Sucursal"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckConSaldoPositivo
        '
        Me.CheckConSaldoPositivo.AutoSize = True
        Me.CheckConSaldoPositivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckConSaldoPositivo.Location = New System.Drawing.Point(161, 324)
        Me.CheckConSaldoPositivo.Name = "CheckConSaldoPositivo"
        Me.CheckConSaldoPositivo.Size = New System.Drawing.Size(172, 22)
        Me.CheckConSaldoPositivo.TabIndex = 182
        Me.CheckConSaldoPositivo.Text = "Con Saldo Positivo"
        Me.CheckConSaldoPositivo.UseVisualStyleBackColor = True
        Me.CheckConSaldoPositivo.Visible = False
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.RadioSinCantidades)
        Me.Panel5.Controls.Add(Me.RadioConCantidad)
        Me.Panel5.Location = New System.Drawing.Point(235, 369)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(364, 32)
        Me.Panel5.TabIndex = 183
        '
        'RadioSinCantidades
        '
        Me.RadioSinCantidades.AutoSize = True
        Me.RadioSinCantidades.Checked = True
        Me.RadioSinCantidades.Location = New System.Drawing.Point(16, 7)
        Me.RadioSinCantidades.Name = "RadioSinCantidades"
        Me.RadioSinCantidades.Size = New System.Drawing.Size(103, 17)
        Me.RadioSinCantidades.TabIndex = 1
        Me.RadioSinCantidades.TabStop = True
        Me.RadioSinCantidades.Text = "Sin Cantiddes"
        Me.RadioSinCantidades.UseVisualStyleBackColor = True
        '
        'RadioConCantidad
        '
        Me.RadioConCantidad.AutoSize = True
        Me.RadioConCantidad.Location = New System.Drawing.Point(159, 6)
        Me.RadioConCantidad.Name = "RadioConCantidad"
        Me.RadioConCantidad.Size = New System.Drawing.Size(165, 17)
        Me.RadioConCantidad.TabIndex = 0
        Me.RadioConCantidad.Text = "Con Cantidades Iniciales"
        Me.RadioConCantidad.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(47, 500)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(647, 45)
        Me.Label8.TabIndex = 184
        Me.Label8.Text = "Alta de Pedido: Si el cliente esta definido para operar con lista de precios y no" & _
            " esta definida para la fecha del pedido, no se podra realizar el alta."
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckAbiertos
        '
        Me.CheckAbiertos.AutoSize = True
        Me.CheckAbiertos.Checked = True
        Me.CheckAbiertos.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbiertos.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAbiertos.Location = New System.Drawing.Point(353, 324)
        Me.CheckAbiertos.Name = "CheckAbiertos"
        Me.CheckAbiertos.Size = New System.Drawing.Size(89, 22)
        Me.CheckAbiertos.TabIndex = 185
        Me.CheckAbiertos.Text = "Abiertos"
        Me.CheckAbiertos.UseVisualStyleBackColor = True
        Me.CheckAbiertos.Visible = False
        '
        'CheckCerrados
        '
        Me.CheckCerrados.AutoSize = True
        Me.CheckCerrados.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrados.Location = New System.Drawing.Point(461, 324)
        Me.CheckCerrados.Name = "CheckCerrados"
        Me.CheckCerrados.Size = New System.Drawing.Size(97, 22)
        Me.CheckCerrados.TabIndex = 186
        Me.CheckCerrados.Text = "Cerrados"
        Me.CheckCerrados.UseVisualStyleBackColor = True
        Me.CheckCerrados.Visible = False
        '
        'CheckConRepeticion
        '
        Me.CheckConRepeticion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckConRepeticion.Location = New System.Drawing.Point(570, 309)
        Me.CheckConRepeticion.Name = "CheckConRepeticion"
        Me.CheckConRepeticion.Size = New System.Drawing.Size(143, 55)
        Me.CheckConRepeticion.TabIndex = 187
        Me.CheckConRepeticion.Text = "Repite Pedidio y Sucursal en todas las lineas"
        Me.CheckConRepeticion.UseVisualStyleBackColor = True
        Me.CheckConRepeticion.Visible = False
        '
        'CheckImportacionKrikos
        '
        Me.CheckImportacionKrikos.BackColor = System.Drawing.Color.LightSteelBlue
        Me.CheckImportacionKrikos.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckImportacionKrikos.FlatAppearance.BorderSize = 3
        Me.CheckImportacionKrikos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckImportacionKrikos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckImportacionKrikos.Location = New System.Drawing.Point(279, 290)
        Me.CheckImportacionKrikos.Name = "CheckImportacionKrikos"
        Me.CheckImportacionKrikos.Size = New System.Drawing.Size(317, 32)
        Me.CheckImportacionKrikos.TabIndex = 7
        Me.CheckImportacionKrikos.Text = "Importación Ordenes de Compra Krikos"
        Me.CheckImportacionKrikos.UseVisualStyleBackColor = False
        Me.CheckImportacionKrikos.Visible = False
        '
        'OpcionPedidos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(775, 572)
        Me.Controls.Add(Me.CheckImportacionKrikos)
        Me.Controls.Add(Me.CheckConRepeticion)
        Me.Controls.Add(Me.CheckCerrados)
        Me.Controls.Add(Me.CheckAbiertos)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.CheckConSaldoPositivo)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "OpcionPedidos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opción Pedidos"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextFechaHasta As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueHasta As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaDesde As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueDesde As System.Windows.Forms.PictureBox
    Friend WithEvents TextPedido As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextPedidoCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents CheckConSaldoPositivo As System.Windows.Forms.CheckBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents RadioSinCantidades As System.Windows.Forms.RadioButton
    Friend WithEvents RadioConCantidad As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CheckAbiertos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckCerrados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckConRepeticion As System.Windows.Forms.CheckBox
    Friend WithEvents CheckImportacionKrikos As System.Windows.Forms.CheckBox
End Class
