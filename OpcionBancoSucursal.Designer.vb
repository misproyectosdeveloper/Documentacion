<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionBancoSucursal
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
        Me.ComboBancos = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.ComboSucursal = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'ComboBancos
        '
        Me.ComboBancos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancos.FormattingEnabled = True
        Me.ComboBancos.Location = New System.Drawing.Point(133, 112)
        Me.ComboBancos.Name = "ComboBancos"
        Me.ComboBancos.Size = New System.Drawing.Size(167, 24)
        Me.ComboBancos.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(70, 116)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 16)
        Me.Label7.TabIndex = 10
        Me.Label7.Text = "Banco"
        '
        'ComboSucursal
        '
        Me.ComboSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursal.FormattingEnabled = True
        Me.ComboSucursal.Location = New System.Drawing.Point(417, 111)
        Me.ComboSucursal.Name = "ComboSucursal"
        Me.ComboSucursal.Size = New System.Drawing.Size(180, 24)
        Me.ComboSucursal.TabIndex = 273
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(336, 116)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(68, 16)
        Me.Label13.TabIndex = 272
        Me.Label13.Text = "Sucursal"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(269, 269)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 274
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'OpcionBancoSucursal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(673, 493)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboSucursal)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.ComboBancos)
        Me.Controls.Add(Me.Label7)
        Me.KeyPreview = True
        Me.Name = "OpcionBancoSucursal"
        Me.Text = "OpcionBancoSucursal"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBancos As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboSucursal As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
