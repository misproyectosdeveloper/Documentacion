<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnMovimientoFondoFijo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnMovimientoFondoFijo))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.RadioAumenta = New System.Windows.Forms.RadioButton
        Me.RadioDisminuye = New System.Windows.Forms.RadioButton
        Me.TextNombreFondoFijo = New System.Windows.Forms.TextBox
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.LabelImporteOrden = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextMovimiento = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ButtonEliminarTodo = New System.Windows.Forms.Button
        Me.ButtonMediosDePago = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextTotalRecibo = New System.Windows.Forms.TextBox
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.TextNombreFondoFijo)
        Me.Panel1.Controls.Add(Me.TextNumero)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Controls.Add(Me.Label19)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Location = New System.Drawing.Point(23, 39)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(769, 101)
        Me.Panel1.TabIndex = 183
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(708, 41)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 203
        Me.PictureCandado.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.RadioAumenta)
        Me.Panel4.Controls.Add(Me.RadioDisminuye)
        Me.Panel4.Location = New System.Drawing.Point(373, 41)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(266, 35)
        Me.Panel4.TabIndex = 202
        '
        'RadioAumenta
        '
        Me.RadioAumenta.AutoSize = True
        Me.RadioAumenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioAumenta.Location = New System.Drawing.Point(22, 6)
        Me.RadioAumenta.Name = "RadioAumenta"
        Me.RadioAumenta.Size = New System.Drawing.Size(99, 24)
        Me.RadioAumenta.TabIndex = 200
        Me.RadioAumenta.Text = "Aumenta"
        Me.RadioAumenta.UseVisualStyleBackColor = True
        '
        'RadioDisminuye
        '
        Me.RadioDisminuye.AutoSize = True
        Me.RadioDisminuye.BackColor = System.Drawing.Color.Transparent
        Me.RadioDisminuye.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioDisminuye.Location = New System.Drawing.Point(134, 5)
        Me.RadioDisminuye.Name = "RadioDisminuye"
        Me.RadioDisminuye.Size = New System.Drawing.Size(109, 24)
        Me.RadioDisminuye.TabIndex = 201
        Me.RadioDisminuye.Text = "Disminuye"
        Me.RadioDisminuye.UseVisualStyleBackColor = False
        '
        'TextNombreFondoFijo
        '
        Me.TextNombreFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreFondoFijo.Location = New System.Drawing.Point(142, 9)
        Me.TextNombreFondoFijo.MaxLength = 30
        Me.TextNombreFondoFijo.Name = "TextNombreFondoFijo"
        Me.TextNombreFondoFijo.ReadOnly = True
        Me.TextNombreFondoFijo.Size = New System.Drawing.Size(253, 20)
        Me.TextNombreFondoFijo.TabIndex = 197
        Me.TextNombreFondoFijo.TabStop = False
        '
        'TextNumero
        '
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(142, 39)
        Me.TextNumero.MaxLength = 7
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.ReadOnly = True
        Me.TextNumero.Size = New System.Drawing.Size(102, 20)
        Me.TextNumero.TabIndex = 194
        Me.TextNumero.TabStop = False
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 43)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(113, 13)
        Me.Label1.TabIndex = 193
        Me.Label1.Text = "Numero Fondo Fijo"
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(16, 12)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(66, 13)
        Me.LabelEmisor.TabIndex = 192
        Me.LabelEmisor.Text = "Fondo Fijo"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(579, 11)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(42, 13)
        Me.Label19.TabIndex = 190
        Me.Label19.Text = "Fecha"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(629, 8)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(110, 20)
        Me.DateTime1.TabIndex = 189
        Me.DateTime1.TabStop = False
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
        Me.TextComentario.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(634, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 314
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
        Me.ComboEstado.Location = New System.Drawing.Point(689, 14)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 313
        Me.ComboEstado.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Thistle
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextImporte)
        Me.Panel3.Controls.Add(Me.LabelImporteOrden)
        Me.Panel3.Location = New System.Drawing.Point(22, 148)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(770, 28)
        Me.Panel3.TabIndex = 315
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(362, 2)
        Me.TextImporte.MaxLength = 10
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.Size = New System.Drawing.Size(111, 20)
        Me.TextImporte.TabIndex = 7
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelImporteOrden
        '
        Me.LabelImporteOrden.AutoSize = True
        Me.LabelImporteOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelImporteOrden.Location = New System.Drawing.Point(304, 7)
        Me.LabelImporteOrden.Name = "LabelImporteOrden"
        Me.LabelImporteOrden.Size = New System.Drawing.Size(49, 13)
        Me.LabelImporteOrden.TabIndex = 244
        Me.LabelImporteOrden.Text = "Importe"
        Me.LabelImporteOrden.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(21, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 317
        Me.Label6.Text = "Comprobante"
        '
        'TextMovimiento
        '
        Me.TextMovimiento.BackColor = System.Drawing.Color.White
        Me.TextMovimiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMovimiento.Location = New System.Drawing.Point(112, 15)
        Me.TextMovimiento.MaxLength = 8
        Me.TextMovimiento.Name = "TextMovimiento"
        Me.TextMovimiento.ReadOnly = True
        Me.TextMovimiento.Size = New System.Drawing.Size(130, 20)
        Me.TextMovimiento.TabIndex = 316
        Me.TextMovimiento.TabStop = False
        Me.TextMovimiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Thistle
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.ButtonEliminarTodo)
        Me.Panel2.Controls.Add(Me.ButtonMediosDePago)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.TextTotalRecibo)
        Me.Panel2.Location = New System.Drawing.Point(21, 184)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(770, 176)
        Me.Panel2.TabIndex = 318
        '
        'ButtonEliminarTodo
        '
        Me.ButtonEliminarTodo.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarTodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodo.Location = New System.Drawing.Point(199, 52)
        Me.ButtonEliminarTodo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodo.Name = "ButtonEliminarTodo"
        Me.ButtonEliminarTodo.Size = New System.Drawing.Size(113, 20)
        Me.ButtonEliminarTodo.TabIndex = 1012
        Me.ButtonEliminarTodo.TabStop = False
        Me.ButtonEliminarTodo.Text = "Borrar Conceptos"
        Me.ButtonEliminarTodo.UseVisualStyleBackColor = False
        '
        'ButtonMediosDePago
        '
        Me.ButtonMediosDePago.BackColor = System.Drawing.Color.Gainsboro
        Me.ButtonMediosDePago.FlatAppearance.BorderSize = 0
        Me.ButtonMediosDePago.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMediosDePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMediosDePago.Location = New System.Drawing.Point(202, 14)
        Me.ButtonMediosDePago.Name = "ButtonMediosDePago"
        Me.ButtonMediosDePago.Size = New System.Drawing.Size(414, 36)
        Me.ButtonMediosDePago.TabIndex = 10
        Me.ButtonMediosDePago.TabStop = False
        Me.ButtonMediosDePago.Text = "Conceptos de Pagos/Cobranza"
        Me.ButtonMediosDePago.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonMediosDePago.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(287, 130)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 13)
        Me.Label3.TabIndex = 244
        Me.Label3.Text = "Total Conceptos"
        '
        'TextTotalRecibo
        '
        Me.TextTotalRecibo.BackColor = System.Drawing.Color.White
        Me.TextTotalRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalRecibo.Location = New System.Drawing.Point(393, 126)
        Me.TextTotalRecibo.MaxLength = 20
        Me.TextTotalRecibo.Name = "TextTotalRecibo"
        Me.TextTotalRecibo.ReadOnly = True
        Me.TextTotalRecibo.Size = New System.Drawing.Size(111, 20)
        Me.TextTotalRecibo.TabIndex = 0
        Me.TextTotalRecibo.TabStop = False
        Me.TextTotalRecibo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(18, 413)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(155, 35)
        Me.ButtonAnula.TabIndex = 321
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anula Ajuste"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(422, 413)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 35)
        Me.ButtonAsientoContable.TabIndex = 320
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(634, 413)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(155, 35)
        Me.ButtonAceptar.TabIndex = 20
        Me.ButtonAceptar.Text = "Graba Ajuste "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(222, 413)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(155, 35)
        Me.ButtonImprimir.TabIndex = 322
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'UnMovimientoFondoFijo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(814, 480)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextMovimiento)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UnMovimientoFondoFijo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ajuste Fondo Fijo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents LabelImporteOrden As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextMovimiento As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarTodo As System.Windows.Forms.Button
    Friend WithEvents ButtonMediosDePago As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextTotalRecibo As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextNombreFondoFijo As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents RadioAumenta As System.Windows.Forms.RadioButton
    Friend WithEvents RadioDisminuye As System.Windows.Forms.RadioButton
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
End Class
