<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnComprobanteAsociado
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextFechaHasta = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueHasta = New System.Windows.Forms.PictureBox
        Me.TextFechaDesde = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueDesde = New System.Windows.Forms.PictureBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.RadioProductoServicio = New System.Windows.Forms.RadioButton
        Me.RadioServicio = New System.Windows.Forms.RadioButton
        Me.RadioProducto = New System.Windows.Forms.RadioButton
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextLetra = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.MaskedComprobante = New System.Windows.Forms.MaskedTextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboTipo = New System.Windows.Forms.ComboBox
        Me.LabelTipoNota = New System.Windows.Forms.Label
        Me.ButtonCancelar = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.TextLetra)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.MaskedComprobante)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboTipo)
        Me.Panel1.Controls.Add(Me.LabelTipoNota)
        Me.Panel1.Location = New System.Drawing.Point(83, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(547, 385)
        Me.Panel1.TabIndex = 202
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(57, 103)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(105, 13)
        Me.Label5.TabIndex = 211
        Me.Label5.Text = "Periodo Afectado"
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.TextFechaHasta)
        Me.Panel3.Controls.Add(Me.PictureAlmanaqueHasta)
        Me.Panel3.Controls.Add(Me.TextFechaDesde)
        Me.Panel3.Controls.Add(Me.PictureAlmanaqueDesde)
        Me.Panel3.Controls.Add(Me.Label8)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Location = New System.Drawing.Point(57, 121)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(439, 99)
        Me.Panel3.TabIndex = 210
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(194, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 20)
        Me.Label4.TabIndex = 1017
        Me.Label4.Text = "Hasta"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextFechaHasta
        '
        Me.TextFechaHasta.BackColor = System.Drawing.Color.White
        Me.TextFechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaHasta.Location = New System.Drawing.Point(266, 8)
        Me.TextFechaHasta.MaxLength = 10
        Me.TextFechaHasta.Name = "TextFechaHasta"
        Me.TextFechaHasta.Size = New System.Drawing.Size(109, 26)
        Me.TextFechaHasta.TabIndex = 1015
        Me.TextFechaHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueHasta
        '
        Me.PictureAlmanaqueHasta.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueHasta.Location = New System.Drawing.Point(378, 5)
        Me.PictureAlmanaqueHasta.Name = "PictureAlmanaqueHasta"
        Me.PictureAlmanaqueHasta.Size = New System.Drawing.Size(42, 34)
        Me.PictureAlmanaqueHasta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueHasta.TabIndex = 1016
        Me.PictureAlmanaqueHasta.TabStop = False
        '
        'TextFechaDesde
        '
        Me.TextFechaDesde.BackColor = System.Drawing.Color.White
        Me.TextFechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaDesde.Location = New System.Drawing.Point(20, 7)
        Me.TextFechaDesde.MaxLength = 10
        Me.TextFechaDesde.Name = "TextFechaDesde"
        Me.TextFechaDesde.Size = New System.Drawing.Size(111, 26)
        Me.TextFechaDesde.TabIndex = 1013
        Me.TextFechaDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueDesde
        '
        Me.PictureAlmanaqueDesde.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueDesde.Location = New System.Drawing.Point(134, 4)
        Me.PictureAlmanaqueDesde.Name = "PictureAlmanaqueDesde"
        Me.PictureAlmanaqueDesde.Size = New System.Drawing.Size(42, 34)
        Me.PictureAlmanaqueDesde.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueDesde.TabIndex = 1014
        Me.PictureAlmanaqueDesde.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(15, 66)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(145, 13)
        Me.Label8.TabIndex = 48
        Me.Label8.Text = "informando periodo afectado."
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(16, 47)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(397, 13)
        Me.Label6.TabIndex = 47
        Me.Label6.Text = "De acuerdo RG. 4540 se puede informar Debitos/Credito  sin documento asociado"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(53, 237)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(162, 13)
        Me.Label3.TabIndex = 209
        Me.Label3.Text = "Tipo Conceptos Informados"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.RadioProductoServicio)
        Me.Panel2.Controls.Add(Me.RadioServicio)
        Me.Panel2.Controls.Add(Me.RadioProducto)
        Me.Panel2.Location = New System.Drawing.Point(55, 254)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(439, 47)
        Me.Panel2.TabIndex = 208
        '
        'RadioProductoServicio
        '
        Me.RadioProductoServicio.AutoSize = True
        Me.RadioProductoServicio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioProductoServicio.Location = New System.Drawing.Point(286, 15)
        Me.RadioProductoServicio.Name = "RadioProductoServicio"
        Me.RadioProductoServicio.Size = New System.Drawing.Size(128, 17)
        Me.RadioProductoServicio.TabIndex = 2
        Me.RadioProductoServicio.TabStop = True
        Me.RadioProductoServicio.Text = "Producto/Servicio"
        Me.RadioProductoServicio.UseVisualStyleBackColor = True
        '
        'RadioServicio
        '
        Me.RadioServicio.AutoSize = True
        Me.RadioServicio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioServicio.Location = New System.Drawing.Point(152, 15)
        Me.RadioServicio.Name = "RadioServicio"
        Me.RadioServicio.Size = New System.Drawing.Size(71, 17)
        Me.RadioServicio.TabIndex = 1
        Me.RadioServicio.TabStop = True
        Me.RadioServicio.Text = "Servicio"
        Me.RadioServicio.UseVisualStyleBackColor = True
        '
        'RadioProducto
        '
        Me.RadioProducto.AutoSize = True
        Me.RadioProducto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioProducto.Location = New System.Drawing.Point(14, 15)
        Me.RadioProducto.Name = "RadioProducto"
        Me.RadioProducto.Size = New System.Drawing.Size(76, 17)
        Me.RadioProducto.TabIndex = 0
        Me.RadioProducto.TabStop = True
        Me.RadioProducto.Text = "Producto"
        Me.RadioProducto.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(199, 342)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 207
        Me.ButtonAceptar.Text = "Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextLetra
        '
        Me.TextLetra.BackColor = System.Drawing.Color.White
        Me.TextLetra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextLetra.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLetra.Location = New System.Drawing.Point(172, 67)
        Me.TextLetra.Name = "TextLetra"
        Me.TextLetra.ReadOnly = True
        Me.TextLetra.Size = New System.Drawing.Size(35, 24)
        Me.TextLetra.TabIndex = 206
        Me.TextLetra.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(67, 71)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 205
        Me.Label1.Text = "Letra Comprobante"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedComprobante
        '
        Me.MaskedComprobante.BackColor = System.Drawing.Color.White
        Me.MaskedComprobante.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedComprobante.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedComprobante.Location = New System.Drawing.Point(348, 66)
        Me.MaskedComprobante.Mask = "0000-00000000"
        Me.MaskedComprobante.Name = "MaskedComprobante"
        Me.MaskedComprobante.Size = New System.Drawing.Size(133, 24)
        Me.MaskedComprobante.TabIndex = 201
        Me.MaskedComprobante.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedComprobante.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(245, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(93, 13)
        Me.Label2.TabIndex = 204
        Me.Label2.Text = "Nro. Comprobante"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboTipo
        '
        Me.ComboTipo.BackColor = System.Drawing.Color.White
        Me.ComboTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipo.ForeColor = System.Drawing.Color.Black
        Me.ComboTipo.FormattingEnabled = True
        Me.ComboTipo.ItemHeight = 20
        Me.ComboTipo.Location = New System.Drawing.Point(228, 16)
        Me.ComboTipo.Name = "ComboTipo"
        Me.ComboTipo.Size = New System.Drawing.Size(256, 28)
        Me.ComboTipo.TabIndex = 202
        Me.ComboTipo.TabStop = False
        '
        'LabelTipoNota
        '
        Me.LabelTipoNota.AutoSize = True
        Me.LabelTipoNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTipoNota.Location = New System.Drawing.Point(67, 20)
        Me.LabelTipoNota.Name = "LabelTipoNota"
        Me.LabelTipoNota.Size = New System.Drawing.Size(141, 13)
        Me.LabelTipoNota.TabIndex = 203
        Me.LabelTipoNota.Text = "Tipo Comprobante Asociado"
        '
        'ButtonCancelar
        '
        Me.ButtonCancelar.BackColor = System.Drawing.Color.Red
        Me.ButtonCancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonCancelar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCancelar.Location = New System.Drawing.Point(256, 477)
        Me.ButtonCancelar.Name = "ButtonCancelar"
        Me.ButtonCancelar.Size = New System.Drawing.Size(193, 26)
        Me.ButtonCancelar.TabIndex = 203
        Me.ButtonCancelar.Text = "Cancelar Envio a la AFIP"
        Me.ButtonCancelar.UseVisualStyleBackColor = False
        '
        'UnComprobanteAsociado
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(685, 519)
        Me.Controls.Add(Me.ButtonCancelar)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "UnComprobanteAsociado"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Comprobante Asociado"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextLetra As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents MaskedComprobante As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelTipoNota As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonCancelar As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioProductoServicio As System.Windows.Forms.RadioButton
    Friend WithEvents RadioServicio As System.Windows.Forms.RadioButton
    Friend WithEvents RadioProducto As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextFechaHasta As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueHasta As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaDesde As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueDesde As System.Windows.Forms.PictureBox
End Class
