<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionTransferencia
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
        Me.LabelOrigen = New System.Windows.Forms.Label
        Me.ComboDepositoOrigen = New System.Windows.Forms.ComboBox
        Me.LabelDestino = New System.Windows.Forms.Label
        Me.ComboDepositoDestino = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelOrigen
        '
        Me.LabelOrigen.AutoSize = True
        Me.LabelOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOrigen.Location = New System.Drawing.Point(149, 58)
        Me.LabelOrigen.Name = "LabelOrigen"
        Me.LabelOrigen.Size = New System.Drawing.Size(98, 13)
        Me.LabelOrigen.TabIndex = 132
        Me.LabelOrigen.Text = "Deposito Origen"
        Me.LabelOrigen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDepositoOrigen
        '
        Me.ComboDepositoOrigen.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDepositoOrigen.FormattingEnabled = True
        Me.ComboDepositoOrigen.Location = New System.Drawing.Point(263, 54)
        Me.ComboDepositoOrigen.Name = "ComboDepositoOrigen"
        Me.ComboDepositoOrigen.Size = New System.Drawing.Size(175, 21)
        Me.ComboDepositoOrigen.TabIndex = 131
        '
        'LabelDestino
        '
        Me.LabelDestino.AutoSize = True
        Me.LabelDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDestino.Location = New System.Drawing.Point(149, 111)
        Me.LabelDestino.Name = "LabelDestino"
        Me.LabelDestino.Size = New System.Drawing.Size(104, 13)
        Me.LabelDestino.TabIndex = 134
        Me.LabelDestino.Text = "Deposito Destino"
        Me.LabelDestino.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDepositoDestino
        '
        Me.ComboDepositoDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDepositoDestino.FormattingEnabled = True
        Me.ComboDepositoDestino.Location = New System.Drawing.Point(264, 107)
        Me.ComboDepositoDestino.Name = "ComboDepositoDestino"
        Me.ComboDepositoDestino.Size = New System.Drawing.Size(174, 21)
        Me.ComboDepositoDestino.TabIndex = 133
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(220, 371)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 135
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(271, 215)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 136
        Me.PictureCandado.TabStop = False
        '
        'OpcionTransferencia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(605, 469)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.LabelDestino)
        Me.Controls.Add(Me.ComboDepositoDestino)
        Me.Controls.Add(Me.LabelOrigen)
        Me.Controls.Add(Me.ComboDepositoOrigen)
        Me.Name = "OpcionTransferencia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelOrigen As System.Windows.Forms.Label
    Friend WithEvents ComboDepositoOrigen As System.Windows.Forms.ComboBox
    Friend WithEvents LabelDestino As System.Windows.Forms.Label
    Friend WithEvents ComboDepositoDestino As System.Windows.Forms.ComboBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
