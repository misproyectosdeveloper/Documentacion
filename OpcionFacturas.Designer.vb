<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionFacturas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionFacturas))
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckFacturaZ = New System.Windows.Forms.CheckBox
        Me.CheckArticulosLoteados = New System.Windows.Forms.CheckBox
        Me.ButtonVerPedido = New System.Windows.Forms.Button
        Me.ButtonVerRemitos = New System.Windows.Forms.Button
        Me.CheckPedidos = New System.Windows.Forms.CheckBox
        Me.CheckBoxEsSecos = New System.Windows.Forms.CheckBox
        Me.CheckBoxEsServicios = New System.Windows.Forms.CheckBox
        Me.CheckRemitos = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.FechaEntrega = New System.Windows.Forms.DateTimePicker
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextCantidadTicket = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextUltimoTicket = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.RadioTicketBCAM = New System.Windows.Forms.RadioButton
        Me.RadioTicket = New System.Windows.Forms.RadioButton
        Me.TextHasta = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextDesde = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.ComboPuntosDeVentaZ = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(85, 21)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(284, 28)
        Me.ComboEmisor.TabIndex = 1
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(27, 27)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(46, 13)
        Me.LabelEmisor.TabIndex = 132
        Me.LabelEmisor.Text = "Cliente"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(380, 28)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 131
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ComboDeposito.Location = New System.Drawing.Point(449, 21)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(165, 28)
        Me.ComboDeposito.TabIndex = 3
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(272, 579)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 100
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(84, 57)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(284, 28)
        Me.ComboAlias.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(27, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 145
        Me.Label1.Text = "Alias"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Yellow
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckFacturaZ)
        Me.Panel1.Controls.Add(Me.CheckArticulosLoteados)
        Me.Panel1.Controls.Add(Me.ButtonVerPedido)
        Me.Panel1.Controls.Add(Me.ButtonVerRemitos)
        Me.Panel1.Controls.Add(Me.CheckPedidos)
        Me.Panel1.Controls.Add(Me.CheckBoxEsSecos)
        Me.Panel1.Controls.Add(Me.CheckBoxEsServicios)
        Me.Panel1.Controls.Add(Me.CheckRemitos)
        Me.Panel1.Location = New System.Drawing.Point(163, 211)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(364, 168)
        Me.Panel1.TabIndex = 8
        Me.Panel1.TabStop = True
        '
        'CheckFacturaZ
        '
        Me.CheckFacturaZ.AutoSize = True
        Me.CheckFacturaZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckFacturaZ.Location = New System.Drawing.Point(67, 140)
        Me.CheckFacturaZ.Name = "CheckFacturaZ"
        Me.CheckFacturaZ.Size = New System.Drawing.Size(116, 20)
        Me.CheckFacturaZ.TabIndex = 129
        Me.CheckFacturaZ.Text = "Ticket Fiscal"
        Me.CheckFacturaZ.UseVisualStyleBackColor = True
        '
        'CheckArticulosLoteados
        '
        Me.CheckArticulosLoteados.AutoSize = True
        Me.CheckArticulosLoteados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckArticulosLoteados.Location = New System.Drawing.Point(67, 111)
        Me.CheckArticulosLoteados.Name = "CheckArticulosLoteados"
        Me.CheckArticulosLoteados.Size = New System.Drawing.Size(187, 20)
        Me.CheckArticulosLoteados.TabIndex = 128
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
        Me.ButtonVerPedido.Location = New System.Drawing.Point(201, 69)
        Me.ButtonVerPedido.Name = "ButtonVerPedido"
        Me.ButtonVerPedido.Size = New System.Drawing.Size(45, 41)
        Me.ButtonVerPedido.TabIndex = 126
        Me.ButtonVerPedido.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerPedido.UseVisualStyleBackColor = False
        Me.ButtonVerPedido.Visible = False
        '
        'ButtonVerRemitos
        '
        Me.ButtonVerRemitos.BackColor = System.Drawing.Color.Transparent
        Me.ButtonVerRemitos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonVerRemitos.FlatAppearance.BorderSize = 0
        Me.ButtonVerRemitos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonVerRemitos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerRemitos.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonVerRemitos.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonVerRemitos.Location = New System.Drawing.Point(199, 35)
        Me.ButtonVerRemitos.Name = "ButtonVerRemitos"
        Me.ButtonVerRemitos.Size = New System.Drawing.Size(45, 41)
        Me.ButtonVerRemitos.TabIndex = 125
        Me.ButtonVerRemitos.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerRemitos.UseVisualStyleBackColor = False
        Me.ButtonVerRemitos.Visible = False
        '
        'CheckPedidos
        '
        Me.CheckPedidos.AutoSize = True
        Me.CheckPedidos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckPedidos.Location = New System.Drawing.Point(68, 79)
        Me.CheckPedidos.Name = "CheckPedidos"
        Me.CheckPedidos.Size = New System.Drawing.Size(123, 20)
        Me.CheckPedidos.TabIndex = 11
        Me.CheckPedidos.Text = "Sobre Pedido"
        Me.CheckPedidos.UseVisualStyleBackColor = True
        '
        'CheckBoxEsSecos
        '
        Me.CheckBoxEsSecos.AutoSize = True
        Me.CheckBoxEsSecos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxEsSecos.Location = New System.Drawing.Point(67, 130)
        Me.CheckBoxEsSecos.Name = "CheckBoxEsSecos"
        Me.CheckBoxEsSecos.Size = New System.Drawing.Size(164, 20)
        Me.CheckBoxEsSecos.TabIndex = 9
        Me.CheckBoxEsSecos.Text = "Factura Para Secos"
        Me.CheckBoxEsSecos.UseVisualStyleBackColor = True
        Me.CheckBoxEsSecos.Visible = False
        '
        'CheckBoxEsServicios
        '
        Me.CheckBoxEsServicios.AutoSize = True
        Me.CheckBoxEsServicios.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxEsServicios.Location = New System.Drawing.Point(68, 13)
        Me.CheckBoxEsServicios.Name = "CheckBoxEsServicios"
        Me.CheckBoxEsServicios.Size = New System.Drawing.Size(185, 20)
        Me.CheckBoxEsServicios.TabIndex = 8
        Me.CheckBoxEsServicios.Text = "Factura Para Servicios"
        Me.CheckBoxEsServicios.UseVisualStyleBackColor = True
        '
        'CheckRemitos
        '
        Me.CheckRemitos.AutoSize = True
        Me.CheckRemitos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckRemitos.Location = New System.Drawing.Point(68, 44)
        Me.CheckRemitos.Name = "CheckRemitos"
        Me.CheckRemitos.Size = New System.Drawing.Size(122, 20)
        Me.CheckRemitos.TabIndex = 10
        Me.CheckRemitos.Text = "Sobre Remito"
        Me.CheckRemitos.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.FechaEntrega)
        Me.Panel2.Location = New System.Drawing.Point(86, 141)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(453, 61)
        Me.Panel2.TabIndex = 6
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
        Me.FechaEntrega.Location = New System.Drawing.Point(143, 17)
        Me.FechaEntrega.Name = "FechaEntrega"
        Me.FechaEntrega.Size = New System.Drawing.Size(108, 20)
        Me.FechaEntrega.TabIndex = 6
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(213, 96)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(263, 28)
        Me.ComboSucursales.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(123, 100)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 20)
        Me.Label4.TabIndex = 300
        Me.Label4.Text = "Sucursal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Yellow
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextCantidadTicket)
        Me.Panel3.Controls.Add(Me.Label11)
        Me.Panel3.Controls.Add(Me.TextUltimoTicket)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.Panel4)
        Me.Panel3.Controls.Add(Me.TextHasta)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.TextDesde)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.ComboPuntosDeVentaZ)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(163, 383)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(364, 175)
        Me.Panel3.TabIndex = 301
        Me.Panel3.TabStop = True
        Me.Panel3.Visible = False
        '
        'TextCantidadTicket
        '
        Me.TextCantidadTicket.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCantidadTicket.Location = New System.Drawing.Point(274, 42)
        Me.TextCantidadTicket.MaxLength = 4
        Me.TextCantidadTicket.Name = "TextCantidadTicket"
        Me.TextCantidadTicket.Size = New System.Drawing.Size(73, 20)
        Me.TextCantidadTicket.TabIndex = 158
        Me.TextCantidadTicket.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(187, 45)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(81, 13)
        Me.Label11.TabIndex = 157
        Me.Label11.Text = "Cant.Tick.Fisc.:"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextUltimoTicket
        '
        Me.TextUltimoTicket.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUltimoTicket.Location = New System.Drawing.Point(86, 42)
        Me.TextUltimoTicket.MaxLength = 10
        Me.TextUltimoTicket.Name = "TextUltimoTicket"
        Me.TextUltimoTicket.Size = New System.Drawing.Size(88, 20)
        Me.TextUltimoTicket.TabIndex = 156
        Me.TextUltimoTicket.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(12, 45)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(69, 13)
        Me.Label10.TabIndex = 155
        Me.Label10.Text = "Ult.TickFisc.:"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(148, 115)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 154
        Me.Label9.Text = "Tipo de Ticket"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.RadioTicketBCAM)
        Me.Panel4.Controls.Add(Me.RadioTicket)
        Me.Panel4.Location = New System.Drawing.Point(38, 132)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(301, 30)
        Me.Panel4.TabIndex = 153
        '
        'RadioTicketBCAM
        '
        Me.RadioTicketBCAM.AutoSize = True
        Me.RadioTicketBCAM.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioTicketBCAM.Location = New System.Drawing.Point(171, 5)
        Me.RadioTicketBCAM.Name = "RadioTicketBCAM"
        Me.RadioTicketBCAM.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RadioTicketBCAM.Size = New System.Drawing.Size(116, 20)
        Me.RadioTicketBCAM.TabIndex = 9
        Me.RadioTicketBCAM.TabStop = True
        Me.RadioTicketBCAM.Text = "Ticket B/C/A/M"
        Me.RadioTicketBCAM.UseVisualStyleBackColor = True
        '
        'RadioTicket
        '
        Me.RadioTicket.AutoSize = True
        Me.RadioTicket.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioTicket.Location = New System.Drawing.Point(21, 5)
        Me.RadioTicket.Name = "RadioTicket"
        Me.RadioTicket.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RadioTicket.Size = New System.Drawing.Size(63, 20)
        Me.RadioTicket.TabIndex = 5
        Me.RadioTicket.TabStop = True
        Me.RadioTicket.Text = "Ticket"
        Me.RadioTicket.UseVisualStyleBackColor = True
        '
        'TextHasta
        '
        Me.TextHasta.BackColor = System.Drawing.Color.White
        Me.TextHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextHasta.Location = New System.Drawing.Point(263, 78)
        Me.TextHasta.MaxLength = 10
        Me.TextHasta.Name = "TextHasta"
        Me.TextHasta.ReadOnly = True
        Me.TextHasta.Size = New System.Drawing.Size(88, 20)
        Me.TextHasta.TabIndex = 137
        Me.TextHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(213, 82)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 13)
        Me.Label8.TabIndex = 136
        Me.Label8.Text = "Hasta"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextDesde
        '
        Me.TextDesde.BackColor = System.Drawing.Color.White
        Me.TextDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDesde.Location = New System.Drawing.Point(116, 79)
        Me.TextDesde.MaxLength = 10
        Me.TextDesde.Name = "TextDesde"
        Me.TextDesde.ReadOnly = True
        Me.TextDesde.Size = New System.Drawing.Size(88, 20)
        Me.TextDesde.TabIndex = 135
        Me.TextDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 82)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(98, 13)
        Me.Label7.TabIndex = 134
        Me.Label7.Text = "Comprobate Desde"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboPuntosDeVentaZ
        '
        Me.ComboPuntosDeVentaZ.BackColor = System.Drawing.Color.White
        Me.ComboPuntosDeVentaZ.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPuntosDeVentaZ.FormattingEnabled = True
        Me.ComboPuntosDeVentaZ.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ComboPuntosDeVentaZ.Location = New System.Drawing.Point(187, 6)
        Me.ComboPuntosDeVentaZ.Name = "ComboPuntosDeVentaZ"
        Me.ComboPuntosDeVentaZ.Size = New System.Drawing.Size(74, 21)
        Me.ComboPuntosDeVentaZ.TabIndex = 133
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(83, 8)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 132
        Me.Label6.Text = "Punto de Venta"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpcionFacturas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(674, 653)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ComboSucursales)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionFacturas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Facturas"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckBoxEsSecos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxEsServicios As System.Windows.Forms.CheckBox
    Friend WithEvents CheckRemitos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckPedidos As System.Windows.Forms.CheckBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FechaEntrega As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerRemitos As System.Windows.Forms.Button
    Friend WithEvents ButtonVerPedido As System.Windows.Forms.Button
    Friend WithEvents CheckArticulosLoteados As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboPuntosDeVentaZ As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextHasta As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextDesde As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents RadioTicketBCAM As System.Windows.Forms.RadioButton
    Friend WithEvents RadioTicket As System.Windows.Forms.RadioButton
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents CheckFacturaZ As System.Windows.Forms.CheckBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextCantidadTicket As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextUltimoTicket As System.Windows.Forms.TextBox
End Class
