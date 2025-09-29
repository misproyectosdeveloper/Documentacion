<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionImportacionExcel
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
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(327, 339)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 36)
        Me.ButtonAceptar.TabIndex = 204
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'MaskedRemito
        '
        Me.MaskedRemito.BackColor = System.Drawing.Color.White
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(391, 93)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(145, 26)
        Me.MaskedRemito.TabIndex = 1
        Me.MaskedRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(298, 95)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(66, 20)
        Me.Label9.TabIndex = 205
        Me.Label9.Text = "Remito"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpcionImportacionExcel
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(816, 569)
        Me.Controls.Add(Me.MaskedRemito)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionImportacionExcel"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Importacion desde Excel"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
