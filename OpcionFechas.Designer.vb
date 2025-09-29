<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Opcionfechas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Opcionfechas))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PrintDialog1 = New System.Windows.Forms.PrintDialog
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.PanelCandado = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureAlmanaque = New System.Windows.Forms.PictureBox
        Me.TextFecha = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Panel1.SuspendLayout()
        Me.PanelCandado.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureAlmanaque, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(246, 373)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 39
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PrintDialog1
        '
        Me.PrintDialog1.AllowPrintToFile = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(49, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(513, 35)
        Me.Label1.TabIndex = 42
        Me.Label1.Text = "Label1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboEmisor)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Location = New System.Drawing.Point(93, 54)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(420, 74)
        Me.Panel1.TabIndex = 158
        Me.Panel1.Visible = False
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(129, 39)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 24)
        Me.ComboAlias.TabIndex = 159
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(5, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 16)
        Me.Label2.TabIndex = 161
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(129, 14)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 24)
        Me.ComboEmisor.TabIndex = 158
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(5, 18)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(115, 16)
        Me.LabelEmisor.TabIndex = 160
        Me.LabelEmisor.Text = "Proveedor Lote"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelCandado
        '
        Me.PanelCandado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelCandado.Controls.Add(Me.CheckCerrado)
        Me.PanelCandado.Controls.Add(Me.CheckAbierto)
        Me.PanelCandado.Location = New System.Drawing.Point(253, 251)
        Me.PanelCandado.Name = "PanelCandado"
        Me.PanelCandado.Size = New System.Drawing.Size(129, 43)
        Me.PanelCandado.TabIndex = 280
        Me.PanelCandado.Visible = False
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
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(214, 17)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(85, 15)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(114, 22)
        Me.DateTimeDesde.TabIndex = 40
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(271, 14)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(114, 22)
        Me.DateTimeHasta.TabIndex = 41
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.DateTimeHasta)
        Me.Panel2.Controls.Add(Me.DateTimeDesde)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Location = New System.Drawing.Point(106, 172)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(403, 62)
        Me.Panel2.TabIndex = 282
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(18, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 16)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureAlmanaque
        '
        Me.PictureAlmanaque.Image = CType(resources.GetObject("PictureAlmanaque.Image"), System.Drawing.Image)
        Me.PictureAlmanaque.Location = New System.Drawing.Point(114, 9)
        Me.PictureAlmanaque.Name = "PictureAlmanaque"
        Me.PictureAlmanaque.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaque.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaque.TabIndex = 1015
        Me.PictureAlmanaque.TabStop = False
        '
        'TextFecha
        '
        Me.TextFecha.BackColor = System.Drawing.Color.White
        Me.TextFecha.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFecha.Location = New System.Drawing.Point(23, 10)
        Me.TextFecha.MaxLength = 10
        Me.TextFecha.Name = "TextFecha"
        Me.TextFecha.Size = New System.Drawing.Size(85, 21)
        Me.TextFecha.TabIndex = 1014
        Me.TextFecha.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.PictureAlmanaque)
        Me.Panel3.Controls.Add(Me.TextFecha)
        Me.Panel3.Location = New System.Drawing.Point(247, 133)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(148, 39)
        Me.Panel3.TabIndex = 1016
        '
        'Opcionfechas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(598, 451)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.PanelCandado)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "Opcionfechas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelCandado.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureAlmanaque, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PrintDialog1 As System.Windows.Forms.PrintDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents PanelCandado As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents PictureAlmanaque As System.Windows.Forms.PictureBox
    Friend WithEvents TextFecha As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
End Class
