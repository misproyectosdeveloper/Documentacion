<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnCanjeCheque
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnCanjeCheque))
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.PanelCheque = New System.Windows.Forms.Panel
        Me.TextRecibidoDe = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextComprobante = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.TextCuenta = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.PanelAfectado = New System.Windows.Forms.Panel
        Me.TextAfectado = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.LabelEstado = New System.Windows.Forms.Label
        Me.ButtonReemplazar = New System.Windows.Forms.Button
        Me.TextEntregadoA = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.DateTime3 = New System.Windows.Forms.DateTimePicker
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextSerie = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboBanco = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextNumeroCheque = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextClaveCheque = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ButtonBuscar = New System.Windows.Forms.Button
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.PanelCheque.SuspendLayout()
        Me.PanelAfectado.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(523, 14)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 1022
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(571, 11)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(105, 20)
        Me.DateTime1.TabIndex = 1023
        Me.DateTime1.TabStop = False
        '
        'PanelCheque
        '
        Me.PanelCheque.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelCheque.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelCheque.Controls.Add(Me.TextRecibidoDe)
        Me.PanelCheque.Controls.Add(Me.Label4)
        Me.PanelCheque.Controls.Add(Me.TextComprobante)
        Me.PanelCheque.Controls.Add(Me.Label17)
        Me.PanelCheque.Controls.Add(Me.TextCuenta)
        Me.PanelCheque.Controls.Add(Me.Label12)
        Me.PanelCheque.Controls.Add(Me.PanelAfectado)
        Me.PanelCheque.Controls.Add(Me.LabelEstado)
        Me.PanelCheque.Controls.Add(Me.ButtonReemplazar)
        Me.PanelCheque.Controls.Add(Me.TextEntregadoA)
        Me.PanelCheque.Controls.Add(Me.Label13)
        Me.PanelCheque.Controls.Add(Me.TextImporte)
        Me.PanelCheque.Controls.Add(Me.Label5)
        Me.PanelCheque.Controls.Add(Me.DateTime3)
        Me.PanelCheque.Controls.Add(Me.Label3)
        Me.PanelCheque.Controls.Add(Me.TextSerie)
        Me.PanelCheque.Controls.Add(Me.Label9)
        Me.PanelCheque.Location = New System.Drawing.Point(42, 203)
        Me.PanelCheque.Name = "PanelCheque"
        Me.PanelCheque.Size = New System.Drawing.Size(639, 156)
        Me.PanelCheque.TabIndex = 1021
        Me.PanelCheque.TabStop = True
        Me.PanelCheque.Visible = False
        '
        'TextRecibidoDe
        '
        Me.TextRecibidoDe.BackColor = System.Drawing.Color.White
        Me.TextRecibidoDe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRecibidoDe.Location = New System.Drawing.Point(88, 47)
        Me.TextRecibidoDe.MaxLength = 12
        Me.TextRecibidoDe.Name = "TextRecibidoDe"
        Me.TextRecibidoDe.ReadOnly = True
        Me.TextRecibidoDe.Size = New System.Drawing.Size(186, 20)
        Me.TextRecibidoDe.TabIndex = 275
        Me.TextRecibidoDe.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(11, 50)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(77, 13)
        Me.Label4.TabIndex = 276
        Me.Label4.Text = "Recibido De"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextComprobante
        '
        Me.TextComprobante.BackColor = System.Drawing.Color.White
        Me.TextComprobante.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComprobante.Location = New System.Drawing.Point(480, 81)
        Me.TextComprobante.MaxLength = 9
        Me.TextComprobante.Name = "TextComprobante"
        Me.TextComprobante.ReadOnly = True
        Me.TextComprobante.Size = New System.Drawing.Size(122, 20)
        Me.TextComprobante.TabIndex = 272
        Me.TextComprobante.TabStop = False
        Me.TextComprobante.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(393, 84)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(81, 13)
        Me.Label17.TabIndex = 273
        Me.Label17.Text = "Comprobante"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextCuenta
        '
        Me.TextCuenta.BackColor = System.Drawing.Color.White
        Me.TextCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuenta.Location = New System.Drawing.Point(80, 11)
        Me.TextCuenta.MaxLength = 12
        Me.TextCuenta.Name = "TextCuenta"
        Me.TextCuenta.ReadOnly = True
        Me.TextCuenta.Size = New System.Drawing.Size(96, 20)
        Me.TextCuenta.TabIndex = 271
        Me.TextCuenta.TabStop = False
        Me.TextCuenta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(12, 15)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(47, 13)
        Me.Label12.TabIndex = 270
        Me.Label12.Text = "Cuenta"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelAfectado
        '
        Me.PanelAfectado.Controls.Add(Me.TextAfectado)
        Me.PanelAfectado.Controls.Add(Me.Label14)
        Me.PanelAfectado.Location = New System.Drawing.Point(4, 107)
        Me.PanelAfectado.Name = "PanelAfectado"
        Me.PanelAfectado.Size = New System.Drawing.Size(316, 34)
        Me.PanelAfectado.TabIndex = 151
        '
        'TextAfectado
        '
        Me.TextAfectado.BackColor = System.Drawing.Color.White
        Me.TextAfectado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAfectado.Location = New System.Drawing.Point(156, 6)
        Me.TextAfectado.MaxLength = 12
        Me.TextAfectado.Name = "TextAfectado"
        Me.TextAfectado.ReadOnly = True
        Me.TextAfectado.Size = New System.Drawing.Size(134, 20)
        Me.TextAfectado.TabIndex = 147
        Me.TextAfectado.TabStop = False
        Me.TextAfectado.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(3, 10)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(144, 13)
        Me.Label14.TabIndex = 145
        Me.Label14.Text = "Comprobante Afectado :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelEstado
        '
        Me.LabelEstado.AutoSize = True
        Me.LabelEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEstado.ForeColor = System.Drawing.Color.Red
        Me.LabelEstado.Location = New System.Drawing.Point(8, 86)
        Me.LabelEstado.Name = "LabelEstado"
        Me.LabelEstado.Size = New System.Drawing.Size(74, 13)
        Me.LabelEstado.TabIndex = 149
        Me.LabelEstado.Text = "Estado       "
        Me.LabelEstado.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonReemplazar
        '
        Me.ButtonReemplazar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonReemplazar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonReemplazar.Location = New System.Drawing.Point(340, 114)
        Me.ButtonReemplazar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonReemplazar.Name = "ButtonReemplazar"
        Me.ButtonReemplazar.Size = New System.Drawing.Size(254, 23)
        Me.ButtonReemplazar.TabIndex = 13
        Me.ButtonReemplazar.Text = "Reemplazar"
        Me.ButtonReemplazar.UseVisualStyleBackColor = False
        '
        'TextEntregadoA
        '
        Me.TextEntregadoA.BackColor = System.Drawing.Color.White
        Me.TextEntregadoA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEntregadoA.Location = New System.Drawing.Point(417, 48)
        Me.TextEntregadoA.MaxLength = 12
        Me.TextEntregadoA.Name = "TextEntregadoA"
        Me.TextEntregadoA.ReadOnly = True
        Me.TextEntregadoA.Size = New System.Drawing.Size(186, 20)
        Me.TextEntregadoA.TabIndex = 146
        Me.TextEntregadoA.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(338, 51)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(77, 13)
        Me.Label13.TabIndex = 147
        Me.Label13.Text = "Entregado A"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(324, 12)
        Me.TextImporte.MaxLength = 12
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.ReadOnly = True
        Me.TextImporte.Size = New System.Drawing.Size(96, 20)
        Me.TextImporte.TabIndex = 135
        Me.TextImporte.TabStop = False
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(271, 15)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(49, 13)
        Me.Label5.TabIndex = 136
        Me.Label5.Text = "Importe"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'DateTime3
        '
        Me.DateTime3.CustomFormat = "dd/MM/yyyy"
        Me.DateTime3.Enabled = False
        Me.DateTime3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime3.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime3.Location = New System.Drawing.Point(514, 11)
        Me.DateTime3.Name = "DateTime3"
        Me.DateTime3.Size = New System.Drawing.Size(91, 20)
        Me.DateTime3.TabIndex = 20
        Me.DateTime3.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(467, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(42, 13)
        Me.Label3.TabIndex = 134
        Me.Label3.Text = "Fecha"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextSerie
        '
        Me.TextSerie.BackColor = System.Drawing.Color.White
        Me.TextSerie.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextSerie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSerie.Location = New System.Drawing.Point(225, 12)
        Me.TextSerie.MaxLength = 12
        Me.TextSerie.Name = "TextSerie"
        Me.TextSerie.ReadOnly = True
        Me.TextSerie.Size = New System.Drawing.Size(38, 20)
        Me.TextSerie.TabIndex = 12
        Me.TextSerie.TabStop = False
        Me.TextSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(185, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Serie"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboBanco)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.TextNumeroCheque)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(41, 42)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(511, 74)
        Me.Panel1.TabIndex = 1020
        '
        'ComboBanco
        '
        Me.ComboBanco.BackColor = System.Drawing.Color.White
        Me.ComboBanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBanco.FormattingEnabled = True
        Me.ComboBanco.Location = New System.Drawing.Point(111, 38)
        Me.ComboBanco.Name = "ComboBanco"
        Me.ComboBanco.Size = New System.Drawing.Size(127, 21)
        Me.ComboBanco.TabIndex = 1
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(63, 42)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 262
        Me.Label6.Text = "Banco"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextNumeroCheque
        '
        Me.TextNumeroCheque.BackColor = System.Drawing.Color.White
        Me.TextNumeroCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeroCheque.Location = New System.Drawing.Point(298, 39)
        Me.TextNumeroCheque.MaxLength = 12
        Me.TextNumeroCheque.Name = "TextNumeroCheque"
        Me.TextNumeroCheque.Size = New System.Drawing.Size(107, 20)
        Me.TextNumeroCheque.TabIndex = 3
        Me.TextNumeroCheque.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(245, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(50, 13)
        Me.Label7.TabIndex = 261
        Me.Label7.Text = "Numero"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(179, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(279, 13)
        Me.Label2.TabIndex = 258
        Me.Label2.Text = "Busqueda cheque PROPIO por Banco y Numero"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.TextClaveCheque)
        Me.Panel2.Location = New System.Drawing.Point(41, 115)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(511, 74)
        Me.Panel2.TabIndex = 1024
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(143, 11)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(321, 13)
        Me.Label1.TabIndex = 258
        Me.Label1.Text = "Busqueda cheque de TERCEROS por Clave de Cheque"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextClaveCheque
        '
        Me.TextClaveCheque.BackColor = System.Drawing.Color.White
        Me.TextClaveCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextClaveCheque.Location = New System.Drawing.Point(202, 41)
        Me.TextClaveCheque.MaxLength = 7
        Me.TextClaveCheque.Name = "TextClaveCheque"
        Me.TextClaveCheque.Size = New System.Drawing.Size(154, 22)
        Me.TextClaveCheque.TabIndex = 1
        Me.TextClaveCheque.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ButtonBuscar)
        Me.Panel3.Controls.Add(Me.PictureCandado)
        Me.Panel3.Location = New System.Drawing.Point(551, 42)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(130, 147)
        Me.Panel3.TabIndex = 1025
        '
        'ButtonBuscar
        '
        Me.ButtonBuscar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonBuscar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBuscar.Location = New System.Drawing.Point(7, 98)
        Me.ButtonBuscar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonBuscar.Name = "ButtonBuscar"
        Me.ButtonBuscar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonBuscar.TabIndex = 265
        Me.ButtonBuscar.Text = "Buscar"
        Me.ButtonBuscar.UseVisualStyleBackColor = False
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(43, 34)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(44, 39)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 264
        Me.PictureCandado.TabStop = False
        '
        'UnCanjeCheque
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(724, 429)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.PanelCheque)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UnCanjeCheque"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Canje de Cheque"
        Me.PanelCheque.ResumeLayout(False)
        Me.PanelCheque.PerformLayout()
        Me.PanelAfectado.ResumeLayout(False)
        Me.PanelAfectado.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents PanelCheque As System.Windows.Forms.Panel
    Friend WithEvents TextComprobante As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextCuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents PanelAfectado As System.Windows.Forms.Panel
    Friend WithEvents TextAfectado As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents LabelEstado As System.Windows.Forms.Label
    Friend WithEvents ButtonReemplazar As System.Windows.Forms.Button
    Friend WithEvents TextEntregadoA As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents DateTime3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextSerie As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboBanco As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextNumeroCheque As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextClaveCheque As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonBuscar As System.Windows.Forms.Button
    Friend WithEvents TextRecibidoDe As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
