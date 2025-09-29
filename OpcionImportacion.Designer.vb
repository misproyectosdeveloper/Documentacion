<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionImportacion
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
        Me.ComboEmpresa = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboEmpresa
        '
        Me.ComboEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmpresa.FormattingEnabled = True
        Me.ComboEmpresa.Location = New System.Drawing.Point(266, 80)
        Me.ComboEmpresa.Name = "ComboEmpresa"
        Me.ComboEmpresa.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmpresa.TabIndex = 138
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(87, 84)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(159, 20)
        Me.LabelEmisor.TabIndex = 139
        Me.LabelEmisor.Text = "Empresa Asociada"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedRemito
        '
        Me.MaskedRemito.BackColor = System.Drawing.Color.White
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(265, 140)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(145, 26)
        Me.MaskedRemito.TabIndex = 198
        Me.MaskedRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(87, 144)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(66, 20)
        Me.Label9.TabIndex = 197
        Me.Label9.Text = "Remito"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(447, 130)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 199
        Me.PictureCandado.TabStop = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(266, 362)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 36)
        Me.ButtonAceptar.TabIndex = 200
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'OpcionImportacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(650, 463)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.MaskedRemito)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ComboEmpresa)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Name = "OpcionImportacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Importación"
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboEmpresa As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
