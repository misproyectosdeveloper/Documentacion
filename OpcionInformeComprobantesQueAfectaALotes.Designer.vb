<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionInformeComprobantesQueAfectaALotes
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionInformeComprobantesQueAfectaALotes))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.MaskedLote = New System.Windows.Forms.MaskedTextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioConIva = New System.Windows.Forms.RadioButton
        Me.RadioSinIva = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboEmisor)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Location = New System.Drawing.Point(24, 36)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(402, 76)
        Me.Panel1.TabIndex = 153
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(121, 35)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 153
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 155
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(122, 4)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmisor.TabIndex = 152
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(11, 7)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(90, 20)
        Me.LabelEmisor.TabIndex = 154
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(712, 41)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(128, 26)
        Me.DateTimeHasta.TabIndex = 269
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(515, 40)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(129, 26)
        Me.DateTimeDesde.TabIndex = 268
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(648, 44)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 20)
        Me.Label7.TabIndex = 267
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(444, 43)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 20)
        Me.Label8.TabIndex = 266
        Me.Label8.Text = "Desde"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(145, 132)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(217, 28)
        Me.ComboCosteo.TabIndex = 271
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(35, 136)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(66, 20)
        Me.Label11.TabIndex = 272
        Me.Label11.Text = "Costeo"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MaskedLote
        '
        Me.MaskedLote.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedLote.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedLote.Location = New System.Drawing.Point(507, 139)
        Me.MaskedLote.Mask = "0000000/000"
        Me.MaskedLote.Name = "MaskedLote"
        Me.MaskedLote.Size = New System.Drawing.Size(126, 26)
        Me.MaskedLote.TabIndex = 277
        Me.MaskedLote.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedLote.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(445, 143)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 20)
        Me.Label1.TabIndex = 276
        Me.Label1.Text = "Lote"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(378, 279)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 278
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckCerrado)
        Me.Panel2.Controls.Add(Me.CheckAbierto)
        Me.Panel2.Location = New System.Drawing.Point(709, 131)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(129, 43)
        Me.Panel2.TabIndex = 279
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(67, 5)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(57, 30)
        Me.CheckCerrado.TabIndex = 230
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Transparent
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(10, 5)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(54, 30)
        Me.CheckAbierto.TabIndex = 229
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(190, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(117, 20)
        Me.Label3.TabIndex = 280
        Me.Label3.Text = "Proveedor Lote"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(610, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(148, 20)
        Me.Label4.TabIndex = 281
        Me.Label4.Text = "Fecha Ingreso Lote"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioSinIva)
        Me.Panel3.Controls.Add(Me.RadioConIva)
        Me.Panel3.Location = New System.Drawing.Point(27, 189)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(335, 43)
        Me.Panel3.TabIndex = 282
        '
        'RadioConIva
        '
        Me.RadioConIva.AutoSize = True
        Me.RadioConIva.Checked = True
        Me.RadioConIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioConIva.Location = New System.Drawing.Point(15, 8)
        Me.RadioConIva.Name = "RadioConIva"
        Me.RadioConIva.Size = New System.Drawing.Size(135, 24)
        Me.RadioConIva.TabIndex = 0
        Me.RadioConIva.TabStop = True
        Me.RadioConIva.Text = "Importe C/Iva"
        Me.RadioConIva.UseVisualStyleBackColor = True
        '
        'RadioSinIva
        '
        Me.RadioSinIva.AutoSize = True
        Me.RadioSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSinIva.Location = New System.Drawing.Point(170, 8)
        Me.RadioSinIva.Name = "RadioSinIva"
        Me.RadioSinIva.Size = New System.Drawing.Size(135, 24)
        Me.RadioSinIva.TabIndex = 1
        Me.RadioSinIva.Text = "Importe S/Iva"
        Me.RadioSinIva.UseVisualStyleBackColor = True
        '
        'OpcionInformeComprobantesQueAfectaALotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(856, 345)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.MaskedLote)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboCosteo)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.DateTimeHasta)
        Me.Controls.Add(Me.DateTimeDesde)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "OpcionInformeComprobantesQueAfectaALotes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents MaskedLote As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioSinIva As System.Windows.Forms.RadioButton
    Friend WithEvents RadioConIva As System.Windows.Forms.RadioButton
End Class
