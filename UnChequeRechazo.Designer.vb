<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnChequeRechazo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnChequeRechazo))
        Me.PanelCheque = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label22 = New System.Windows.Forms.Label
        Me.TextComprobanteDestino = New System.Windows.Forms.TextBox
        Me.TextTipoComprobanteDestino = New System.Windows.Forms.TextBox
        Me.ButtonEmitirCreditoInterno = New System.Windows.Forms.Button
        Me.TextACuenta = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.ButtonEmitirCredito = New System.Windows.Forms.Button
        Me.ButtonVerCredito = New System.Windows.Forms.Button
        Me.TextCuenta = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextNumeroFondoFijoDestino = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextPrestamoDestino = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextEntregadoA = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label21 = New System.Windows.Forms.Label
        Me.TextComprobanteOrigen = New System.Windows.Forms.TextBox
        Me.TextTipoComprobanteOrigen = New System.Windows.Forms.TextBox
        Me.ButtonEmitirDebitoInterno = New System.Windows.Forms.Button
        Me.TextNumeroFondoFijoOrigen = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextPrestamoOrigen = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.ButtonEmitirDebito = New System.Windows.Forms.Button
        Me.ButtonVerDebito = New System.Windows.Forms.Button
        Me.TextRecibidoDe = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboBanco1 = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.PanelAfectado = New System.Windows.Forms.Panel
        Me.TextAfectado = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.LabelEstado = New System.Windows.Forms.Label
        Me.TextEmisorCheque = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.DateTime3 = New System.Windows.Forms.DateTimePicker
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextSerie = New System.Windows.Forms.TextBox
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextClaveCheque = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboBanco = New System.Windows.Forms.ComboBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.TextNumeroCheque = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label20 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.ButtonBuscaPorClave = New System.Windows.Forms.Button
        Me.PictureCandado1 = New System.Windows.Forms.PictureBox
        Me.LabelCartel = New System.Windows.Forms.Label
        Me.PanelCheque.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.PanelAfectado.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.PictureCandado1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelCheque
        '
        Me.PanelCheque.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelCheque.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelCheque.Controls.Add(Me.Panel3)
        Me.PanelCheque.Controls.Add(Me.Panel1)
        Me.PanelCheque.Controls.Add(Me.ComboBanco1)
        Me.PanelCheque.Controls.Add(Me.Label6)
        Me.PanelCheque.Controls.Add(Me.PanelAfectado)
        Me.PanelCheque.Controls.Add(Me.LabelEstado)
        Me.PanelCheque.Controls.Add(Me.TextEmisorCheque)
        Me.PanelCheque.Controls.Add(Me.Label10)
        Me.PanelCheque.Controls.Add(Me.TextImporte)
        Me.PanelCheque.Controls.Add(Me.Label5)
        Me.PanelCheque.Controls.Add(Me.DateTime3)
        Me.PanelCheque.Controls.Add(Me.Label3)
        Me.PanelCheque.Controls.Add(Me.TextSerie)
        Me.PanelCheque.Controls.Add(Me.TextNumero)
        Me.PanelCheque.Controls.Add(Me.Label4)
        Me.PanelCheque.Controls.Add(Me.Label9)
        Me.PanelCheque.Location = New System.Drawing.Point(130, 216)
        Me.PanelCheque.Name = "PanelCheque"
        Me.PanelCheque.Size = New System.Drawing.Size(621, 425)
        Me.PanelCheque.TabIndex = 1020
        Me.PanelCheque.TabStop = True
        Me.PanelCheque.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Transparent
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label22)
        Me.Panel3.Controls.Add(Me.TextComprobanteDestino)
        Me.Panel3.Controls.Add(Me.TextTipoComprobanteDestino)
        Me.Panel3.Controls.Add(Me.ButtonEmitirCreditoInterno)
        Me.Panel3.Controls.Add(Me.TextACuenta)
        Me.Panel3.Controls.Add(Me.Label17)
        Me.Panel3.Controls.Add(Me.ButtonEmitirCredito)
        Me.Panel3.Controls.Add(Me.ButtonVerCredito)
        Me.Panel3.Controls.Add(Me.TextCuenta)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.TextNumeroFondoFijoDestino)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.TextPrestamoDestino)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.TextEntregadoA)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Location = New System.Drawing.Point(311, 102)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(286, 262)
        Me.Panel3.TabIndex = 1034
        Me.Panel3.Tag = ""
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(115, 131)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(81, 13)
        Me.Label22.TabIndex = 1049
        Me.Label22.Text = "Comprobante"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextComprobanteDestino
        '
        Me.TextComprobanteDestino.BackColor = System.Drawing.Color.White
        Me.TextComprobanteDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComprobanteDestino.Location = New System.Drawing.Point(165, 146)
        Me.TextComprobanteDestino.MaxLength = 12
        Me.TextComprobanteDestino.Name = "TextComprobanteDestino"
        Me.TextComprobanteDestino.ReadOnly = True
        Me.TextComprobanteDestino.Size = New System.Drawing.Size(109, 20)
        Me.TextComprobanteDestino.TabIndex = 1045
        Me.TextComprobanteDestino.TabStop = False
        Me.TextComprobanteDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextTipoComprobanteDestino
        '
        Me.TextTipoComprobanteDestino.BackColor = System.Drawing.Color.White
        Me.TextTipoComprobanteDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTipoComprobanteDestino.Location = New System.Drawing.Point(9, 147)
        Me.TextTipoComprobanteDestino.MaxLength = 12
        Me.TextTipoComprobanteDestino.Name = "TextTipoComprobanteDestino"
        Me.TextTipoComprobanteDestino.ReadOnly = True
        Me.TextTipoComprobanteDestino.Size = New System.Drawing.Size(144, 20)
        Me.TextTipoComprobanteDestino.TabIndex = 1044
        Me.TextTipoComprobanteDestino.TabStop = False
        Me.TextTipoComprobanteDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonEmitirCreditoInterno
        '
        Me.ButtonEmitirCreditoInterno.BackColor = System.Drawing.Color.Khaki
        Me.ButtonEmitirCreditoInterno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEmitirCreditoInterno.Location = New System.Drawing.Point(67, 219)
        Me.ButtonEmitirCreditoInterno.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEmitirCreditoInterno.Name = "ButtonEmitirCreditoInterno"
        Me.ButtonEmitirCreditoInterno.Size = New System.Drawing.Size(165, 23)
        Me.ButtonEmitirCreditoInterno.TabIndex = 1043
        Me.ButtonEmitirCreditoInterno.Text = "Emitir Nota Crédito Interna"
        Me.ButtonEmitirCreditoInterno.UseVisualStyleBackColor = False
        Me.ButtonEmitirCreditoInterno.Visible = False
        '
        'TextACuenta
        '
        Me.TextACuenta.BackColor = System.Drawing.Color.White
        Me.TextACuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextACuenta.Location = New System.Drawing.Point(89, 28)
        Me.TextACuenta.MaxLength = 12
        Me.TextACuenta.Name = "TextACuenta"
        Me.TextACuenta.ReadOnly = True
        Me.TextACuenta.Size = New System.Drawing.Size(186, 20)
        Me.TextACuenta.TabIndex = 1041
        Me.TextACuenta.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(10, 31)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(59, 13)
        Me.Label17.TabIndex = 1042
        Me.Label17.Text = "A Cuenta"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonEmitirCredito
        '
        Me.ButtonEmitirCredito.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonEmitirCredito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEmitirCredito.Location = New System.Drawing.Point(67, 188)
        Me.ButtonEmitirCredito.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEmitirCredito.Name = "ButtonEmitirCredito"
        Me.ButtonEmitirCredito.Size = New System.Drawing.Size(165, 23)
        Me.ButtonEmitirCredito.TabIndex = 1040
        Me.ButtonEmitirCredito.Text = "Emitir Nota Crédito"
        Me.ButtonEmitirCredito.UseVisualStyleBackColor = False
        Me.ButtonEmitirCredito.Visible = False
        '
        'ButtonVerCredito
        '
        Me.ButtonVerCredito.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonVerCredito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerCredito.Location = New System.Drawing.Point(34, 189)
        Me.ButtonVerCredito.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonVerCredito.Name = "ButtonVerCredito"
        Me.ButtonVerCredito.Size = New System.Drawing.Size(228, 23)
        Me.ButtonVerCredito.TabIndex = 1039
        Me.ButtonVerCredito.Text = "Ver Nota Crédito"
        Me.ButtonVerCredito.UseVisualStyleBackColor = False
        Me.ButtonVerCredito.Visible = False
        '
        'TextCuenta
        '
        Me.TextCuenta.BackColor = System.Drawing.Color.White
        Me.TextCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuenta.Location = New System.Drawing.Point(147, 102)
        Me.TextCuenta.MaxLength = 12
        Me.TextCuenta.Name = "TextCuenta"
        Me.TextCuenta.ReadOnly = True
        Me.TextCuenta.Size = New System.Drawing.Size(126, 20)
        Me.TextCuenta.TabIndex = 1037
        Me.TextCuenta.TabStop = False
        Me.TextCuenta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 105)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(47, 13)
        Me.Label8.TabIndex = 1038
        Me.Label8.Text = "Cuenta"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextNumeroFondoFijoDestino
        '
        Me.TextNumeroFondoFijoDestino.BackColor = System.Drawing.Color.White
        Me.TextNumeroFondoFijoDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeroFondoFijoDestino.Location = New System.Drawing.Point(147, 77)
        Me.TextNumeroFondoFijoDestino.MaxLength = 12
        Me.TextNumeroFondoFijoDestino.Name = "TextNumeroFondoFijoDestino"
        Me.TextNumeroFondoFijoDestino.ReadOnly = True
        Me.TextNumeroFondoFijoDestino.Size = New System.Drawing.Size(126, 20)
        Me.TextNumeroFondoFijoDestino.TabIndex = 1035
        Me.TextNumeroFondoFijoDestino.TabStop = False
        Me.TextNumeroFondoFijoDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(12, 80)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(113, 13)
        Me.Label7.TabIndex = 1036
        Me.Label7.Text = "Numero Fondo Fijo"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextPrestamoDestino
        '
        Me.TextPrestamoDestino.BackColor = System.Drawing.Color.White
        Me.TextPrestamoDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPrestamoDestino.Location = New System.Drawing.Point(147, 53)
        Me.TextPrestamoDestino.MaxLength = 12
        Me.TextPrestamoDestino.Name = "TextPrestamoDestino"
        Me.TextPrestamoDestino.ReadOnly = True
        Me.TextPrestamoDestino.Size = New System.Drawing.Size(126, 20)
        Me.TextPrestamoDestino.TabIndex = 1033
        Me.TextPrestamoDestino.TabStop = False
        Me.TextPrestamoDestino.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 13)
        Me.Label2.TabIndex = 1034
        Me.Label2.Text = "Prestamo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextEntregadoA
        '
        Me.TextEntregadoA.BackColor = System.Drawing.Color.White
        Me.TextEntregadoA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEntregadoA.Location = New System.Drawing.Point(89, 4)
        Me.TextEntregadoA.MaxLength = 12
        Me.TextEntregadoA.Name = "TextEntregadoA"
        Me.TextEntregadoA.ReadOnly = True
        Me.TextEntregadoA.Size = New System.Drawing.Size(186, 20)
        Me.TextEntregadoA.TabIndex = 148
        Me.TextEntregadoA.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(10, 7)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(77, 13)
        Me.Label13.TabIndex = 149
        Me.Label13.Text = "Entregado A"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Transparent
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label21)
        Me.Panel1.Controls.Add(Me.TextComprobanteOrigen)
        Me.Panel1.Controls.Add(Me.TextTipoComprobanteOrigen)
        Me.Panel1.Controls.Add(Me.ButtonEmitirDebitoInterno)
        Me.Panel1.Controls.Add(Me.TextNumeroFondoFijoOrigen)
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.TextPrestamoOrigen)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.ButtonEmitirDebito)
        Me.Panel1.Controls.Add(Me.ButtonVerDebito)
        Me.Panel1.Controls.Add(Me.TextRecibidoDe)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Location = New System.Drawing.Point(23, 102)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(286, 262)
        Me.Panel1.TabIndex = 1033
        Me.Panel1.Tag = ""
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(119, 128)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(81, 13)
        Me.Label21.TabIndex = 1048
        Me.Label21.Text = "Comprobante"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextComprobanteOrigen
        '
        Me.TextComprobanteOrigen.BackColor = System.Drawing.Color.White
        Me.TextComprobanteOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComprobanteOrigen.Location = New System.Drawing.Point(159, 147)
        Me.TextComprobanteOrigen.MaxLength = 12
        Me.TextComprobanteOrigen.Name = "TextComprobanteOrigen"
        Me.TextComprobanteOrigen.ReadOnly = True
        Me.TextComprobanteOrigen.Size = New System.Drawing.Size(109, 20)
        Me.TextComprobanteOrigen.TabIndex = 1047
        Me.TextComprobanteOrigen.TabStop = False
        Me.TextComprobanteOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextTipoComprobanteOrigen
        '
        Me.TextTipoComprobanteOrigen.BackColor = System.Drawing.Color.White
        Me.TextTipoComprobanteOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTipoComprobanteOrigen.Location = New System.Drawing.Point(7, 148)
        Me.TextTipoComprobanteOrigen.MaxLength = 12
        Me.TextTipoComprobanteOrigen.Name = "TextTipoComprobanteOrigen"
        Me.TextTipoComprobanteOrigen.ReadOnly = True
        Me.TextTipoComprobanteOrigen.Size = New System.Drawing.Size(144, 20)
        Me.TextTipoComprobanteOrigen.TabIndex = 1046
        Me.TextTipoComprobanteOrigen.TabStop = False
        Me.TextTipoComprobanteOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonEmitirDebitoInterno
        '
        Me.ButtonEmitirDebitoInterno.BackColor = System.Drawing.Color.Khaki
        Me.ButtonEmitirDebitoInterno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEmitirDebitoInterno.Location = New System.Drawing.Point(68, 219)
        Me.ButtonEmitirDebitoInterno.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEmitirDebitoInterno.Name = "ButtonEmitirDebitoInterno"
        Me.ButtonEmitirDebitoInterno.Size = New System.Drawing.Size(165, 23)
        Me.ButtonEmitirDebitoInterno.TabIndex = 1044
        Me.ButtonEmitirDebitoInterno.Text = "Emitir Nota Debito Interna"
        Me.ButtonEmitirDebitoInterno.UseVisualStyleBackColor = False
        Me.ButtonEmitirDebitoInterno.Visible = False
        '
        'TextNumeroFondoFijoOrigen
        '
        Me.TextNumeroFondoFijoOrigen.BackColor = System.Drawing.Color.White
        Me.TextNumeroFondoFijoOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeroFondoFijoOrigen.Location = New System.Drawing.Point(141, 53)
        Me.TextNumeroFondoFijoOrigen.MaxLength = 12
        Me.TextNumeroFondoFijoOrigen.Name = "TextNumeroFondoFijoOrigen"
        Me.TextNumeroFondoFijoOrigen.ReadOnly = True
        Me.TextNumeroFondoFijoOrigen.Size = New System.Drawing.Size(126, 20)
        Me.TextNumeroFondoFijoOrigen.TabIndex = 1037
        Me.TextNumeroFondoFijoOrigen.TabStop = False
        Me.TextNumeroFondoFijoOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(6, 56)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(113, 13)
        Me.Label16.TabIndex = 1038
        Me.Label16.Text = "Numero Fondo Fijo"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextPrestamoOrigen
        '
        Me.TextPrestamoOrigen.BackColor = System.Drawing.Color.White
        Me.TextPrestamoOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPrestamoOrigen.Location = New System.Drawing.Point(141, 29)
        Me.TextPrestamoOrigen.MaxLength = 12
        Me.TextPrestamoOrigen.Name = "TextPrestamoOrigen"
        Me.TextPrestamoOrigen.ReadOnly = True
        Me.TextPrestamoOrigen.Size = New System.Drawing.Size(126, 20)
        Me.TextPrestamoOrigen.TabIndex = 1035
        Me.TextPrestamoOrigen.TabStop = False
        Me.TextPrestamoOrigen.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(5, 32)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(59, 13)
        Me.Label12.TabIndex = 1036
        Me.Label12.Text = "Prestamo"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ButtonEmitirDebito
        '
        Me.ButtonEmitirDebito.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonEmitirDebito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEmitirDebito.Location = New System.Drawing.Point(67, 187)
        Me.ButtonEmitirDebito.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEmitirDebito.Name = "ButtonEmitirDebito"
        Me.ButtonEmitirDebito.Size = New System.Drawing.Size(165, 23)
        Me.ButtonEmitirDebito.TabIndex = 1031
        Me.ButtonEmitirDebito.Text = "Emitir Nota Debito"
        Me.ButtonEmitirDebito.UseVisualStyleBackColor = False
        Me.ButtonEmitirDebito.Visible = False
        '
        'ButtonVerDebito
        '
        Me.ButtonVerDebito.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonVerDebito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerDebito.Location = New System.Drawing.Point(30, 188)
        Me.ButtonVerDebito.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonVerDebito.Name = "ButtonVerDebito"
        Me.ButtonVerDebito.Size = New System.Drawing.Size(228, 23)
        Me.ButtonVerDebito.TabIndex = 1030
        Me.ButtonVerDebito.Text = "Ver Nota Debito"
        Me.ButtonVerDebito.UseVisualStyleBackColor = False
        Me.ButtonVerDebito.Visible = False
        '
        'TextRecibidoDe
        '
        Me.TextRecibidoDe.BackColor = System.Drawing.Color.White
        Me.TextRecibidoDe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRecibidoDe.Location = New System.Drawing.Point(83, 5)
        Me.TextRecibidoDe.MaxLength = 12
        Me.TextRecibidoDe.Name = "TextRecibidoDe"
        Me.TextRecibidoDe.ReadOnly = True
        Me.TextRecibidoDe.Size = New System.Drawing.Size(186, 20)
        Me.TextRecibidoDe.TabIndex = 145
        Me.TextRecibidoDe.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(4, 8)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(77, 13)
        Me.Label11.TabIndex = 146
        Me.Label11.Text = "Recibido De"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBanco1
        '
        Me.ComboBanco1.BackColor = System.Drawing.Color.White
        Me.ComboBanco1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBanco1.FormattingEnabled = True
        Me.ComboBanco1.Location = New System.Drawing.Point(64, 10)
        Me.ComboBanco1.Name = "ComboBanco1"
        Me.ComboBanco1.Size = New System.Drawing.Size(127, 21)
        Me.ComboBanco1.TabIndex = 263
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(5, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(43, 13)
        Me.Label6.TabIndex = 264
        Me.Label6.Text = "Banco"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelAfectado
        '
        Me.PanelAfectado.Controls.Add(Me.TextAfectado)
        Me.PanelAfectado.Controls.Add(Me.Label14)
        Me.PanelAfectado.Location = New System.Drawing.Point(4, 58)
        Me.PanelAfectado.Name = "PanelAfectado"
        Me.PanelAfectado.Size = New System.Drawing.Size(331, 36)
        Me.PanelAfectado.TabIndex = 151
        '
        'TextAfectado
        '
        Me.TextAfectado.BackColor = System.Drawing.Color.White
        Me.TextAfectado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAfectado.Location = New System.Drawing.Point(157, 5)
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
        Me.Label14.Location = New System.Drawing.Point(3, 9)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(144, 13)
        Me.Label14.TabIndex = 145
        Me.Label14.Text = "Comprobante Afectado :"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LabelEstado
        '
        Me.LabelEstado.AutoSize = True
        Me.LabelEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEstado.ForeColor = System.Drawing.Color.Red
        Me.LabelEstado.Location = New System.Drawing.Point(250, 385)
        Me.LabelEstado.Name = "LabelEstado"
        Me.LabelEstado.Size = New System.Drawing.Size(134, 25)
        Me.LabelEstado.TabIndex = 149
        Me.LabelEstado.Text = "Estado       "
        Me.LabelEstado.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextEmisorCheque
        '
        Me.TextEmisorCheque.BackColor = System.Drawing.Color.White
        Me.TextEmisorCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEmisorCheque.Location = New System.Drawing.Point(283, 35)
        Me.TextEmisorCheque.MaxLength = 12
        Me.TextEmisorCheque.Name = "TextEmisorCheque"
        Me.TextEmisorCheque.ReadOnly = True
        Me.TextEmisorCheque.Size = New System.Drawing.Size(169, 20)
        Me.TextEmisorCheque.TabIndex = 141
        Me.TextEmisorCheque.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(188, 38)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(91, 13)
        Me.Label10.TabIndex = 142
        Me.Label10.Text = "Emisor Cheque"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(64, 37)
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
        Me.Label5.Location = New System.Drawing.Point(6, 40)
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
        Me.DateTime3.Location = New System.Drawing.Point(509, 11)
        Me.DateTime3.Name = "DateTime3"
        Me.DateTime3.Size = New System.Drawing.Size(91, 20)
        Me.DateTime3.TabIndex = 20
        Me.DateTime3.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(460, 14)
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
        Me.TextSerie.Location = New System.Drawing.Point(242, 11)
        Me.TextSerie.MaxLength = 12
        Me.TextSerie.Name = "TextSerie"
        Me.TextSerie.ReadOnly = True
        Me.TextSerie.Size = New System.Drawing.Size(38, 20)
        Me.TextSerie.TabIndex = 12
        Me.TextSerie.TabStop = False
        Me.TextSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextNumero
        '
        Me.TextNumero.BackColor = System.Drawing.Color.White
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(338, 11)
        Me.TextNumero.MaxLength = 12
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.ReadOnly = True
        Me.TextNumero.Size = New System.Drawing.Size(96, 20)
        Me.TextNumero.TabIndex = 13
        Me.TextNumero.TabStop = False
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(285, 14)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 131
        Me.Label4.Text = "Numero"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(202, 14)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Serie"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.TextClaveCheque)
        Me.Panel2.Location = New System.Drawing.Point(131, 132)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(512, 74)
        Me.Panel2.TabIndex = 1019
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(138, 11)
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
        Me.TextClaveCheque.Location = New System.Drawing.Point(195, 37)
        Me.TextClaveCheque.MaxLength = 7
        Me.TextClaveCheque.Name = "TextClaveCheque"
        Me.TextClaveCheque.Size = New System.Drawing.Size(154, 22)
        Me.TextClaveCheque.TabIndex = 1
        Me.TextClaveCheque.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(595, 33)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 1021
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(646, 30)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(105, 20)
        Me.DateTime1.TabIndex = 1022
        Me.DateTime1.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ComboBanco)
        Me.Panel4.Controls.Add(Me.Label18)
        Me.Panel4.Controls.Add(Me.TextNumeroCheque)
        Me.Panel4.Controls.Add(Me.Label19)
        Me.Panel4.Controls.Add(Me.Label20)
        Me.Panel4.Location = New System.Drawing.Point(132, 59)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(511, 74)
        Me.Panel4.TabIndex = 1023
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
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(63, 42)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(43, 13)
        Me.Label18.TabIndex = 262
        Me.Label18.Text = "Banco"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
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
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(245, 42)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(50, 13)
        Me.Label19.TabIndex = 261
        Me.Label19.Text = "Numero"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(168, 11)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(279, 13)
        Me.Label20.TabIndex = 258
        Me.Label20.Text = "Busqueda cheque PROPIO por Banco y Numero"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.ButtonBuscaPorClave)
        Me.Panel5.Controls.Add(Me.PictureCandado1)
        Me.Panel5.Location = New System.Drawing.Point(621, 59)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(130, 147)
        Me.Panel5.TabIndex = 1026
        '
        'ButtonBuscaPorClave
        '
        Me.ButtonBuscaPorClave.BackColor = System.Drawing.Color.Yellow
        Me.ButtonBuscaPorClave.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBuscaPorClave.Location = New System.Drawing.Point(7, 98)
        Me.ButtonBuscaPorClave.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonBuscaPorClave.Name = "ButtonBuscaPorClave"
        Me.ButtonBuscaPorClave.Size = New System.Drawing.Size(114, 23)
        Me.ButtonBuscaPorClave.TabIndex = 265
        Me.ButtonBuscaPorClave.Text = "Buscar"
        Me.ButtonBuscaPorClave.UseVisualStyleBackColor = False
        '
        'PictureCandado1
        '
        Me.PictureCandado1.Location = New System.Drawing.Point(42, 22)
        Me.PictureCandado1.Name = "PictureCandado1"
        Me.PictureCandado1.Size = New System.Drawing.Size(51, 51)
        Me.PictureCandado1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado1.TabIndex = 264
        Me.PictureCandado1.TabStop = False
        '
        'LabelCartel
        '
        Me.LabelCartel.AutoSize = True
        Me.LabelCartel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCartel.Location = New System.Drawing.Point(133, 653)
        Me.LabelCartel.Name = "LabelCartel"
        Me.LabelCartel.Size = New System.Drawing.Size(56, 16)
        Me.LabelCartel.TabIndex = 1027
        Me.LabelCartel.Text = "Label23"
        '
        'UnChequeRechazo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(883, 685)
        Me.Controls.Add(Me.LabelCartel)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.PanelCheque)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.DateTime1)
        Me.Name = "UnChequeRechazo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rechazos Cheques de Terceros"
        Me.PanelCheque.ResumeLayout(False)
        Me.PanelCheque.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelAfectado.ResumeLayout(False)
        Me.PanelAfectado.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        CType(Me.PictureCandado1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelCheque As System.Windows.Forms.Panel
    Friend WithEvents ComboBanco1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PanelAfectado As System.Windows.Forms.Panel
    Friend WithEvents TextAfectado As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents LabelEstado As System.Windows.Forms.Label
    Friend WithEvents TextEmisorCheque As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents DateTime3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextSerie As System.Windows.Forms.TextBox
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextClaveCheque As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextPrestamoDestino As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextEntregadoA As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextRecibidoDe As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextNumeroFondoFijoDestino As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextCuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonEmitirCredito As System.Windows.Forms.Button
    Friend WithEvents ButtonVerCredito As System.Windows.Forms.Button
    Friend WithEvents ButtonEmitirDebito As System.Windows.Forms.Button
    Friend WithEvents ButtonVerDebito As System.Windows.Forms.Button
    Friend WithEvents TextPrestamoOrigen As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextNumeroFondoFijoOrigen As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TextACuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboBanco As System.Windows.Forms.ComboBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextNumeroCheque As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents ButtonBuscaPorClave As System.Windows.Forms.Button
    Friend WithEvents PictureCandado1 As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonEmitirCreditoInterno As System.Windows.Forms.Button
    Friend WithEvents ButtonEmitirDebitoInterno As System.Windows.Forms.Button
    Friend WithEvents TextComprobanteDestino As System.Windows.Forms.TextBox
    Friend WithEvents TextTipoComprobanteDestino As System.Windows.Forms.TextBox
    Friend WithEvents TextComprobanteOrigen As System.Windows.Forms.TextBox
    Friend WithEvents TextTipoComprobanteOrigen As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents LabelCartel As System.Windows.Forms.Label
End Class
