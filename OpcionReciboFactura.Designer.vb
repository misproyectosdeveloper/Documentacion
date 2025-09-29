<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionReciboFactura
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
        Me.RadioContadoEfectivo = New System.Windows.Forms.RadioButton
        Me.RadioMixto = New System.Windows.Forms.RadioButton
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.RadioMixto)
        Me.Panel1.Controls.Add(Me.RadioContadoEfectivo)
        Me.Panel1.Location = New System.Drawing.Point(72, 75)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(576, 93)
        Me.Panel1.TabIndex = 0
        '
        'RadioContadoEfectivo
        '
        Me.RadioContadoEfectivo.AutoSize = True
        Me.RadioContadoEfectivo.BackColor = System.Drawing.Color.Transparent
        Me.RadioContadoEfectivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioContadoEfectivo.Location = New System.Drawing.Point(76, 24)
        Me.RadioContadoEfectivo.Name = "RadioContadoEfectivo"
        Me.RadioContadoEfectivo.Size = New System.Drawing.Size(258, 28)
        Me.RadioContadoEfectivo.TabIndex = 0
        Me.RadioContadoEfectivo.TabStop = True
        Me.RadioContadoEfectivo.Text = "Contado Efectivo o Seña"
        Me.RadioContadoEfectivo.UseVisualStyleBackColor = False
        '
        'RadioMixto
        '
        Me.RadioMixto.AutoSize = True
        Me.RadioMixto.BackColor = System.Drawing.Color.Transparent
        Me.RadioMixto.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioMixto.Location = New System.Drawing.Point(454, 26)
        Me.RadioMixto.Name = "RadioMixto"
        Me.RadioMixto.Size = New System.Drawing.Size(78, 28)
        Me.RadioMixto.TabIndex = 1
        Me.RadioMixto.TabStop = True
        Me.RadioMixto.Text = "Mixto"
        Me.RadioMixto.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(70, 49)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(124, 18)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Forma de Pago"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(292, 239)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(168, 48)
        Me.ButtonAceptar.TabIndex = 273
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'OpcionReciboFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(736, 324)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "OpcionReciboFactura"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Recibo-Factura"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RadioContadoEfectivo As System.Windows.Forms.RadioButton
    Friend WithEvents RadioMixto As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
