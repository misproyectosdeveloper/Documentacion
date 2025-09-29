<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnBalance
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
        Me.ButtonBorra = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureAlmanaqueHasta = New System.Windows.Forms.PictureBox
        Me.TextHasta = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueDesde = New System.Windows.Forms.PictureBox
        Me.TextDesde = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonBorra
        '
        Me.ButtonBorra.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBorra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBorra.Image = Global.ScomerV01.My.Resources.Resources.Basurero
        Me.ButtonBorra.Location = New System.Drawing.Point(66, 222)
        Me.ButtonBorra.Name = "ButtonBorra"
        Me.ButtonBorra.Size = New System.Drawing.Size(135, 30)
        Me.ButtonBorra.TabIndex = 273
        Me.ButtonBorra.TabStop = False
        Me.ButtonBorra.Text = "Borra Balance"
        Me.ButtonBorra.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBorra.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(538, 222)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(131, 29)
        Me.ButtonAceptar.TabIndex = 272
        Me.ButtonAceptar.Text = "Graba Cambios"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureAlmanaqueHasta)
        Me.Panel1.Controls.Add(Me.TextHasta)
        Me.Panel1.Controls.Add(Me.PictureAlmanaqueDesde)
        Me.Panel1.Controls.Add(Me.TextDesde)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Location = New System.Drawing.Point(66, 25)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(607, 100)
        Me.Panel1.TabIndex = 302
        '
        'PictureAlmanaqueHasta
        '
        Me.PictureAlmanaqueHasta.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueHasta.Location = New System.Drawing.Point(427, 27)
        Me.PictureAlmanaqueHasta.Name = "PictureAlmanaqueHasta"
        Me.PictureAlmanaqueHasta.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueHasta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueHasta.TabIndex = 1016
        Me.PictureAlmanaqueHasta.TabStop = False
        '
        'TextHasta
        '
        Me.TextHasta.BackColor = System.Drawing.Color.White
        Me.TextHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextHasta.Location = New System.Drawing.Point(314, 26)
        Me.TextHasta.MaxLength = 10
        Me.TextHasta.Name = "TextHasta"
        Me.TextHasta.Size = New System.Drawing.Size(107, 26)
        Me.TextHasta.TabIndex = 1015
        Me.TextHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaqueDesde
        '
        Me.PictureAlmanaqueDesde.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueDesde.Location = New System.Drawing.Point(189, 26)
        Me.PictureAlmanaqueDesde.Name = "PictureAlmanaqueDesde"
        Me.PictureAlmanaqueDesde.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueDesde.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueDesde.TabIndex = 1014
        Me.PictureAlmanaqueDesde.TabStop = False
        '
        'TextDesde
        '
        Me.TextDesde.BackColor = System.Drawing.Color.White
        Me.TextDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDesde.Location = New System.Drawing.Point(76, 25)
        Me.TextDesde.MaxLength = 10
        Me.TextDesde.Name = "TextDesde"
        Me.TextDesde.Size = New System.Drawing.Size(107, 26)
        Me.TextDesde.TabIndex = 1013
        Me.TextDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(247, 28)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 20)
        Me.Label7.TabIndex = 225
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 20)
        Me.Label4.TabIndex = 223
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckCerrado
        '
        Me.CheckCerrado.AutoSize = True
        Me.CheckCerrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrado.Location = New System.Drawing.Point(495, 28)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(92, 24)
        Me.CheckCerrado.TabIndex = 221
        Me.CheckCerrado.Text = "Cerrado"
        Me.CheckCerrado.UseVisualStyleBackColor = True
        '
        'UnBalance
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(739, 318)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonBorra)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "UnBalance"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Balance"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaqueHasta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaqueDesde, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonBorra As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents PictureAlmanaqueDesde As System.Windows.Forms.PictureBox
    Friend WithEvents TextDesde As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueHasta As System.Windows.Forms.PictureBox
    Friend WithEvents TextHasta As System.Windows.Forms.TextBox
End Class
