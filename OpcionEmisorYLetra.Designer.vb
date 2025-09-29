<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionEmisorYLetra
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
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.TextTipoFactura = New System.Windows.Forms.TextBox
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(218, 77)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(299, 28)
        Me.ComboAlias.TabIndex = 151
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(109, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 16)
        Me.Label2.TabIndex = 157
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(316, 179)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 156
        Me.Label1.Text = "Letra"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(310, 271)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 155
        Me.PictureCandado.TabStop = False
        Me.PictureCandado.Visible = False
        '
        'TextTipoFactura
        '
        Me.TextTipoFactura.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextTipoFactura.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTipoFactura.Location = New System.Drawing.Point(311, 200)
        Me.TextTipoFactura.Margin = New System.Windows.Forms.Padding(0)
        Me.TextTipoFactura.MaxLength = 1
        Me.TextTipoFactura.Name = "TextTipoFactura"
        Me.TextTipoFactura.Size = New System.Drawing.Size(54, 29)
        Me.TextTipoFactura.TabIndex = 152
        Me.TextTipoFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(220, 42)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(297, 28)
        Me.ComboEmisor.TabIndex = 150
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(110, 45)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(100, 16)
        Me.LabelEmisor.TabIndex = 154
        Me.LabelEmisor.Text = "Razon Social"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(263, 376)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 153
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Location = New System.Drawing.Point(107, 116)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(423, 57)
        Me.Panel1.TabIndex = 160
        Me.Panel1.Visible = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 21)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(202, 13)
        Me.Label3.TabIndex = 161
        Me.Label3.Text = "Solo Lotes del Deposito (Optativo)"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ComboDeposito.Location = New System.Drawing.Point(244, 14)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(165, 28)
        Me.ComboDeposito.TabIndex = 160
        '
        'OpcionEmisorYLetra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(644, 480)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.TextTipoFactura)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionEmisorYLetra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents TextTipoFactura As System.Windows.Forms.TextBox
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
End Class
