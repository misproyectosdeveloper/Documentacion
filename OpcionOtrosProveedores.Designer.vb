<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionOtrosProveedores
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
        Me.ComboTipoPago = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboTipoPago
        '
        Me.ComboTipoPago.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoPago.FormattingEnabled = True
        Me.ComboTipoPago.Location = New System.Drawing.Point(236, 106)
        Me.ComboTipoPago.Name = "ComboTipoPago"
        Me.ComboTipoPago.Size = New System.Drawing.Size(263, 21)
        Me.ComboTipoPago.TabIndex = 153
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(126, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 156
        Me.Label2.Text = "Tipo de Pago"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(237, 55)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 21)
        Me.ComboEmisor.TabIndex = 152
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(125, 58)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(93, 13)
        Me.LabelEmisor.TabIndex = 155
        Me.LabelEmisor.Text = "Otro Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(270, 374)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 154
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(320, 202)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 157
        Me.PictureCandado.TabStop = False
        '
        'OpcionOtrosProveedores
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(642, 454)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ComboTipoPago)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionOtrosProveedores"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Otros Proveedores"
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboTipoPago As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
End Class
