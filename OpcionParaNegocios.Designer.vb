<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionParaNegocios
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionParaNegocios))
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.RadioMercaderias = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(153, 65)
        Me.ComboNegocio.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(289, 24)
        Me.ComboNegocio.TabIndex = 145
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 71)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 16)
        Me.Label1.TabIndex = 146
        Me.Label1.Text = "Negocio"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(153, 109)
        Me.ComboCosteo.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(289, 24)
        Me.ComboCosteo.TabIndex = 147
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 115)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 16)
        Me.Label2.TabIndex = 148
        Me.Label2.Text = "Costeo"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Location = New System.Drawing.Point(646, 140)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(151, 44)
        Me.Panel1.TabIndex = 233
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(80, 8)
        Me.CheckCerrado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
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
        Me.CheckAbierto.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(17, 8)
        Me.CheckAbierto.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(63, 30)
        Me.CheckAbierto.TabIndex = 229
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(714, 68)
        Me.DateTimeHasta.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(123, 22)
        Me.DateTimeHasta.TabIndex = 237
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(525, 69)
        Me.DateTimeDesde.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(123, 22)
        Me.DateTimeDesde.TabIndex = 236
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(658, 71)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 235
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(465, 72)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(54, 16)
        Me.Label8.TabIndex = 234
        Me.Label8.Text = "Desde"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(607, 40)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 18)
        Me.Label3.TabIndex = 238
        Me.Label3.Text = "Fecha Facturas"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(325, 266)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(251, 40)
        Me.ButtonAceptar.TabIndex = 239
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(14, 161)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(121, 16)
        Me.Label6.TabIndex = 241
        Me.Label6.Text = "Estado Facturas"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(153, 153)
        Me.ComboEstado.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(144, 24)
        Me.ComboEstado.TabIndex = 240
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.RadioMercaderias)
        Me.Panel2.Location = New System.Drawing.Point(484, 140)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(151, 44)
        Me.Panel2.TabIndex = 242
        '
        'RadioMercaderias
        '
        Me.RadioMercaderias.AutoSize = True
        Me.RadioMercaderias.Checked = True
        Me.RadioMercaderias.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioMercaderias.Location = New System.Drawing.Point(16, 12)
        Me.RadioMercaderias.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.RadioMercaderias.Name = "RadioMercaderias"
        Me.RadioMercaderias.Size = New System.Drawing.Size(119, 22)
        Me.RadioMercaderias.TabIndex = 231
        Me.RadioMercaderias.TabStop = True
        Me.RadioMercaderias.Text = "Mercaderias"
        Me.RadioMercaderias.UseVisualStyleBackColor = True
        '
        'OpcionParaNegocios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(10.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(861, 357)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.DateTimeHasta)
        Me.Controls.Add(Me.DateTimeDesde)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboCosteo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboNegocio)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "OpcionParaNegocios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones Para Negocios"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioMercaderias As System.Windows.Forms.RadioButton
End Class
