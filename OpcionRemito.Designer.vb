<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionRemito
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionRemito))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.FechaEntrega = New System.Windows.Forms.DateTimePicker
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckArticulosLoteados = New System.Windows.Forms.CheckBox
        Me.ButtonVerPedido = New System.Windows.Forms.Button
        Me.CheckPedidos = New System.Windows.Forms.CheckBox
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ComboPuntoDeVenta = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.CheckRemitoManual = New System.Windows.Forms.CheckBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.ButtonPorCuentaYOrden = New System.Windows.Forms.Button
        Me.TextSucursalPorCuentaYOrden = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.TextPorCuentaYOrden = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.ButtonVerClientes = New System.Windows.Forms.Button
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(252, 555)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 20
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(419, 23)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 39
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(484, 19)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(142, 21)
        Me.ComboDeposito.TabIndex = 3
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(75, 20)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 21)
        Me.ComboEmisor.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(11, 24)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(46, 13)
        Me.LabelEmisor.TabIndex = 41
        Me.LabelEmisor.Text = "Cliente"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(299, 469)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 126
        Me.PictureCandado.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(75, 54)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 21)
        Me.ComboAlias.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 58)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 147
        Me.Label1.Text = "Alias"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.FechaEntrega)
        Me.Panel1.Location = New System.Drawing.Point(92, 204)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(453, 61)
        Me.Panel1.TabIndex = 6
        Me.Panel1.TabStop = True
        '
        'Label2
        '
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(272, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(164, 46)
        Me.Label2.TabIndex = 296
        Me.Label2.Text = "Fecha Entrega: Utilizada para encontrar la lista de precios del clienete"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(27, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(109, 16)
        Me.Label5.TabIndex = 295
        Me.Label5.Text = "Fecha Entrega"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaEntrega
        '
        Me.FechaEntrega.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntrega.CustomFormat = "dd/MM/yyyy"
        Me.FechaEntrega.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaEntrega.Location = New System.Drawing.Point(151, 17)
        Me.FechaEntrega.Name = "FechaEntrega"
        Me.FechaEntrega.Size = New System.Drawing.Size(95, 20)
        Me.FechaEntrega.TabIndex = 6
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Yellow
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckArticulosLoteados)
        Me.Panel2.Controls.Add(Me.ButtonVerPedido)
        Me.Panel2.Controls.Add(Me.CheckPedidos)
        Me.Panel2.Location = New System.Drawing.Point(208, 283)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(245, 90)
        Me.Panel2.TabIndex = 7
        Me.Panel2.TabStop = True
        '
        'CheckArticulosLoteados
        '
        Me.CheckArticulosLoteados.AutoSize = True
        Me.CheckArticulosLoteados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckArticulosLoteados.Location = New System.Drawing.Point(26, 53)
        Me.CheckArticulosLoteados.Name = "CheckArticulosLoteados"
        Me.CheckArticulosLoteados.Size = New System.Drawing.Size(187, 20)
        Me.CheckArticulosLoteados.TabIndex = 129
        Me.CheckArticulosLoteados.Text = "Solo Articulos en Stock"
        Me.CheckArticulosLoteados.UseVisualStyleBackColor = True
        '
        'ButtonVerPedido
        '
        Me.ButtonVerPedido.BackColor = System.Drawing.Color.Transparent
        Me.ButtonVerPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonVerPedido.FlatAppearance.BorderSize = 0
        Me.ButtonVerPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonVerPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerPedido.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonVerPedido.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonVerPedido.Location = New System.Drawing.Point(168, 6)
        Me.ButtonVerPedido.Name = "ButtonVerPedido"
        Me.ButtonVerPedido.Size = New System.Drawing.Size(45, 41)
        Me.ButtonVerPedido.TabIndex = 8
        Me.ButtonVerPedido.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerPedido.UseVisualStyleBackColor = False
        Me.ButtonVerPedido.Visible = False
        '
        'CheckPedidos
        '
        Me.CheckPedidos.AutoSize = True
        Me.CheckPedidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckPedidos.Location = New System.Drawing.Point(27, 15)
        Me.CheckPedidos.Name = "CheckPedidos"
        Me.CheckPedidos.Size = New System.Drawing.Size(123, 20)
        Me.CheckPedidos.TabIndex = 7
        Me.CheckPedidos.Text = "Sobre Pedido"
        Me.CheckPedidos.UseVisualStyleBackColor = True
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(239, 89)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(263, 28)
        Me.ComboSucursales.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(149, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 20)
        Me.Label4.TabIndex = 298
        Me.Label4.Text = "Sucursal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.White
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ComboPuntoDeVenta)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.CheckRemitoManual)
        Me.Panel3.Location = New System.Drawing.Point(20, 415)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(609, 39)
        Me.Panel3.TabIndex = 9
        Me.Panel3.TabStop = True
        '
        'ComboPuntoDeVenta
        '
        Me.ComboPuntoDeVenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPuntoDeVenta.FormattingEnabled = True
        Me.ComboPuntoDeVenta.Location = New System.Drawing.Point(522, 10)
        Me.ComboPuntoDeVenta.Name = "ComboPuntoDeVenta"
        Me.ComboPuntoDeVenta.Size = New System.Drawing.Size(68, 21)
        Me.ComboPuntoDeVenta.TabIndex = 10
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(342, 11)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(167, 16)
        Me.Label6.TabIndex = 303
        Me.Label6.Text = "Punto de Venta Manual"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckRemitoManual
        '
        Me.CheckRemitoManual.AutoSize = True
        Me.CheckRemitoManual.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckRemitoManual.Location = New System.Drawing.Point(21, 9)
        Me.CheckRemitoManual.Name = "CheckRemitoManual"
        Me.CheckRemitoManual.Size = New System.Drawing.Size(130, 20)
        Me.CheckRemitoManual.TabIndex = 9
        Me.CheckRemitoManual.Text = "Remito Manual"
        Me.CheckRemitoManual.UseVisualStyleBackColor = True
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.White
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.ButtonPorCuentaYOrden)
        Me.Panel5.Controls.Add(Me.TextSucursalPorCuentaYOrden)
        Me.Panel5.Controls.Add(Me.Label21)
        Me.Panel5.Controls.Add(Me.TextPorCuentaYOrden)
        Me.Panel5.Controls.Add(Me.Label12)
        Me.Panel5.Controls.Add(Me.ButtonVerClientes)
        Me.Panel5.Location = New System.Drawing.Point(19, 132)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(610, 37)
        Me.Panel5.TabIndex = 5
        Me.Panel5.TabStop = True
        '
        'ButtonPorCuentaYOrden
        '
        Me.ButtonPorCuentaYOrden.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPorCuentaYOrden.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPorCuentaYOrden.FlatAppearance.BorderSize = 0
        Me.ButtonPorCuentaYOrden.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonPorCuentaYOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPorCuentaYOrden.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonPorCuentaYOrden.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPorCuentaYOrden.Location = New System.Drawing.Point(568, -2)
        Me.ButtonPorCuentaYOrden.Name = "ButtonPorCuentaYOrden"
        Me.ButtonPorCuentaYOrden.Size = New System.Drawing.Size(34, 36)
        Me.ButtonPorCuentaYOrden.TabIndex = 5
        Me.ButtonPorCuentaYOrden.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonPorCuentaYOrden.UseVisualStyleBackColor = False
        '
        'TextSucursalPorCuentaYOrden
        '
        Me.TextSucursalPorCuentaYOrden.BackColor = System.Drawing.Color.White
        Me.TextSucursalPorCuentaYOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSucursalPorCuentaYOrden.Location = New System.Drawing.Point(417, 7)
        Me.TextSucursalPorCuentaYOrden.MaxLength = 30
        Me.TextSucursalPorCuentaYOrden.Name = "TextSucursalPorCuentaYOrden"
        Me.TextSucursalPorCuentaYOrden.ReadOnly = True
        Me.TextSucursalPorCuentaYOrden.Size = New System.Drawing.Size(145, 20)
        Me.TextSucursalPorCuentaYOrden.TabIndex = 279
        Me.TextSucursalPorCuentaYOrden.TabStop = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(351, 11)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(60, 13)
        Me.Label21.TabIndex = 278
        Me.Label21.Text = "Sucursal:"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextPorCuentaYOrden
        '
        Me.TextPorCuentaYOrden.BackColor = System.Drawing.Color.White
        Me.TextPorCuentaYOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPorCuentaYOrden.Location = New System.Drawing.Point(148, 8)
        Me.TextPorCuentaYOrden.MaxLength = 30
        Me.TextPorCuentaYOrden.Name = "TextPorCuentaYOrden"
        Me.TextPorCuentaYOrden.ReadOnly = True
        Me.TextPorCuentaYOrden.Size = New System.Drawing.Size(199, 20)
        Me.TextPorCuentaYOrden.TabIndex = 126
        Me.TextPorCuentaYOrden.TabStop = False
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(11, 4)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(137, 30)
        Me.Label12.TabIndex = 125
        Me.Label12.Text = "Por Cuenta Y Orden del Cliente:"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonVerClientes
        '
        Me.ButtonVerClientes.BackColor = System.Drawing.Color.Transparent
        Me.ButtonVerClientes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonVerClientes.FlatAppearance.BorderSize = 0
        Me.ButtonVerClientes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonVerClientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerClientes.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonVerClientes.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonVerClientes.Location = New System.Drawing.Point(649, -6)
        Me.ButtonVerClientes.Name = "ButtonVerClientes"
        Me.ButtonVerClientes.Size = New System.Drawing.Size(45, 41)
        Me.ButtonVerClientes.TabIndex = 124
        Me.ButtonVerClientes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerClientes.UseVisualStyleBackColor = False
        '
        'OpcionRemito
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(653, 606)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ComboSucursales)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.KeyPreview = True
        Me.Name = "OpcionRemito"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones "
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FechaEntrega As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CheckPedidos As System.Windows.Forms.CheckBox
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerPedido As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboPuntoDeVenta As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CheckRemitoManual As System.Windows.Forms.CheckBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents TextSucursalPorCuentaYOrden As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TextPorCuentaYOrden As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerClientes As System.Windows.Forms.Button
    Friend WithEvents ButtonPorCuentaYOrden As System.Windows.Forms.Button
    Friend WithEvents CheckArticulosLoteados As System.Windows.Forms.CheckBox
End Class
