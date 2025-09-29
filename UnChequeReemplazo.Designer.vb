<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnChequeReemplazo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnChequeReemplazo))
        Me.LabelPuntoDeVenta = New System.Windows.Forms.Label
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.LabelTipoNota = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextImporteCheque = New System.Windows.Forms.TextBox
        Me.TextBanco = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextCheque = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextNota = New System.Windows.Forms.TextBox
        Me.TextEmisor = New System.Windows.Forms.TextBox
        Me.LabelInterno = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.LabelCaja = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ButtonEliminarTodo = New System.Windows.Forms.Button
        Me.ButtonMediosDePago = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextTotalRecibo = New System.Windows.Forms.TextBox
        Me.Panel4.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelPuntoDeVenta
        '
        Me.LabelPuntoDeVenta.AutoSize = True
        Me.LabelPuntoDeVenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPuntoDeVenta.Location = New System.Drawing.Point(793, 22)
        Me.LabelPuntoDeVenta.Name = "LabelPuntoDeVenta"
        Me.LabelPuntoDeVenta.Size = New System.Drawing.Size(104, 15)
        Me.LabelPuntoDeVenta.TabIndex = 312
        Me.LabelPuntoDeVenta.Text = "Punto de Venta"
        Me.LabelPuntoDeVenta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(669, 511)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(147, 37)
        Me.ButtonAsientoContable.TabIndex = 311
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'LabelTipoNota
        '
        Me.LabelTipoNota.AutoSize = True
        Me.LabelTipoNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTipoNota.Location = New System.Drawing.Point(19, 23)
        Me.LabelTipoNota.Name = "LabelTipoNota"
        Me.LabelTipoNota.Size = New System.Drawing.Size(63, 13)
        Me.LabelTipoNota.TabIndex = 309
        Me.LabelTipoNota.Text = "Tipo Nota"
        Me.LabelTipoNota.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Thistle
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.TextImporteCheque)
        Me.Panel4.Controls.Add(Me.TextBanco)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.TextCheque)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Controls.Add(Me.TextNota)
        Me.Panel4.Controls.Add(Me.TextEmisor)
        Me.Panel4.Controls.Add(Me.LabelInterno)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.TextComentario)
        Me.Panel4.Controls.Add(Me.PictureCandado)
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.DateTime1)
        Me.Panel4.Location = New System.Drawing.Point(22, 43)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1092, 98)
        Me.Panel4.TabIndex = 301
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(735, 35)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 18)
        Me.Label2.TabIndex = 258
        Me.Label2.Text = "Importe "
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImporteCheque
        '
        Me.TextImporteCheque.BackColor = System.Drawing.Color.White
        Me.TextImporteCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporteCheque.Location = New System.Drawing.Point(809, 33)
        Me.TextImporteCheque.MaxLength = 10
        Me.TextImporteCheque.Name = "TextImporteCheque"
        Me.TextImporteCheque.ReadOnly = True
        Me.TextImporteCheque.Size = New System.Drawing.Size(139, 22)
        Me.TextImporteCheque.TabIndex = 257
        Me.TextImporteCheque.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBanco
        '
        Me.TextBanco.BackColor = System.Drawing.Color.White
        Me.TextBanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBanco.Location = New System.Drawing.Point(542, 32)
        Me.TextBanco.MaxLength = 30
        Me.TextBanco.Name = "TextBanco"
        Me.TextBanco.ReadOnly = True
        Me.TextBanco.Size = New System.Drawing.Size(164, 22)
        Me.TextBanco.TabIndex = 256
        Me.TextBanco.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(480, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 18)
        Me.Label5.TabIndex = 255
        Me.Label5.Text = "Banco"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCheque
        '
        Me.TextCheque.BackColor = System.Drawing.Color.White
        Me.TextCheque.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCheque.Location = New System.Drawing.Point(250, 32)
        Me.TextCheque.MaxLength = 30
        Me.TextCheque.Name = "TextCheque"
        Me.TextCheque.ReadOnly = True
        Me.TextCheque.Size = New System.Drawing.Size(219, 22)
        Me.TextCheque.TabIndex = 254
        Me.TextCheque.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(68, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(174, 18)
        Me.Label4.TabIndex = 253
        Me.Label4.Text = "Cheque a Reemplazar"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextNota
        '
        Me.TextNota.BackColor = System.Drawing.Color.White
        Me.TextNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNota.Location = New System.Drawing.Point(99, 4)
        Me.TextNota.MaxLength = 30
        Me.TextNota.Name = "TextNota"
        Me.TextNota.ReadOnly = True
        Me.TextNota.Size = New System.Drawing.Size(131, 22)
        Me.TextNota.TabIndex = 252
        Me.TextNota.TabStop = False
        Me.TextNota.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextEmisor
        '
        Me.TextEmisor.BackColor = System.Drawing.Color.White
        Me.TextEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEmisor.Location = New System.Drawing.Point(420, 4)
        Me.TextEmisor.MaxLength = 30
        Me.TextEmisor.Name = "TextEmisor"
        Me.TextEmisor.Size = New System.Drawing.Size(303, 22)
        Me.TextEmisor.TabIndex = 251
        Me.TextEmisor.TabStop = False
        '
        'LabelInterno
        '
        Me.LabelInterno.AutoSize = True
        Me.LabelInterno.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelInterno.Location = New System.Drawing.Point(6, 8)
        Me.LabelInterno.Name = "LabelInterno"
        Me.LabelInterno.Size = New System.Drawing.Size(81, 13)
        Me.LabelInterno.TabIndex = 244
        Me.LabelInterno.Text = "Comprobante"
        Me.LabelInterno.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 238
        Me.Label1.Text = "Comentario"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(78, 62)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(292, 20)
        Me.TextComentario.TabIndex = 237
        Me.TextComentario.TabStop = False
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(1036, 34)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 137
        Me.PictureCandado.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(919, 8)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(968, 4)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(103, 20)
        Me.DateTime1.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(952, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 307
        Me.Label8.Text = "Estado"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.White
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(1007, 16)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 306
        Me.ComboEstado.TabStop = False
        '
        'LabelCaja
        '
        Me.LabelCaja.AutoSize = True
        Me.LabelCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaja.Location = New System.Drawing.Point(383, 21)
        Me.LabelCaja.Name = "LabelCaja"
        Me.LabelCaja.Size = New System.Drawing.Size(47, 18)
        Me.LabelCaja.TabIndex = 305
        Me.LabelCaja.Text = "Caja "
        Me.LabelCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(332, 21)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 15)
        Me.Label13.TabIndex = 304
        Me.Label13.Text = "Caja "
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(966, 509)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(147, 37)
        Me.ButtonAceptar.TabIndex = 303
        Me.ButtonAceptar.Text = "Graba Reemplazo "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(22, 511)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(147, 37)
        Me.ButtonAnula.TabIndex = 308
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anular Reemplazo"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(353, 511)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(147, 37)
        Me.ButtonImprimir.TabIndex = 313
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ButtonEliminarTodo)
        Me.Panel1.Controls.Add(Me.ButtonMediosDePago)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.TextTotalRecibo)
        Me.Panel1.Location = New System.Drawing.Point(20, 174)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1093, 111)
        Me.Panel1.TabIndex = 314
        '
        'ButtonEliminarTodo
        '
        Me.ButtonEliminarTodo.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarTodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodo.Location = New System.Drawing.Point(319, 70)
        Me.ButtonEliminarTodo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodo.Name = "ButtonEliminarTodo"
        Me.ButtonEliminarTodo.Size = New System.Drawing.Size(120, 20)
        Me.ButtonEliminarTodo.TabIndex = 1012
        Me.ButtonEliminarTodo.TabStop = False
        Me.ButtonEliminarTodo.Text = "Borrar Conceptos"
        Me.ButtonEliminarTodo.UseVisualStyleBackColor = False
        '
        'ButtonMediosDePago
        '
        Me.ButtonMediosDePago.BackColor = System.Drawing.Color.LightGray
        Me.ButtonMediosDePago.FlatAppearance.BorderSize = 0
        Me.ButtonMediosDePago.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMediosDePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMediosDePago.Location = New System.Drawing.Point(323, 21)
        Me.ButtonMediosDePago.Name = "ButtonMediosDePago"
        Me.ButtonMediosDePago.Size = New System.Drawing.Size(475, 41)
        Me.ButtonMediosDePago.TabIndex = 1009
        Me.ButtonMediosDePago.TabStop = False
        Me.ButtonMediosDePago.Text = "Conceptos de Pagos/Cobranza"
        Me.ButtonMediosDePago.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonMediosDePago.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(628, 75)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(80, 13)
        Me.Label6.TabIndex = 244
        Me.Label6.Text = "Total Recibo"
        '
        'TextTotalRecibo
        '
        Me.TextTotalRecibo.BackColor = System.Drawing.Color.White
        Me.TextTotalRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalRecibo.Location = New System.Drawing.Point(714, 71)
        Me.TextTotalRecibo.MaxLength = 20
        Me.TextTotalRecibo.Name = "TextTotalRecibo"
        Me.TextTotalRecibo.ReadOnly = True
        Me.TextTotalRecibo.Size = New System.Drawing.Size(85, 20)
        Me.TextTotalRecibo.TabIndex = 0
        Me.TextTotalRecibo.TabStop = False
        Me.TextTotalRecibo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'UnChequeReemplazo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(1133, 676)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.LabelPuntoDeVenta)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.LabelTipoNota)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.LabelCaja)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Name = "UnChequeReemplazo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reemplazo de Cheque"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelPuntoDeVenta As System.Windows.Forms.Label
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents LabelTipoNota As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents LabelInterno As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents LabelCaja As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextEmisor As System.Windows.Forms.TextBox
    Friend WithEvents TextNota As System.Windows.Forms.TextBox
    Friend WithEvents TextBanco As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextCheque As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextImporteCheque As System.Windows.Forms.TextBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarTodo As System.Windows.Forms.Button
    Friend WithEvents ButtonMediosDePago As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextTotalRecibo As System.Windows.Forms.TextBox
End Class
