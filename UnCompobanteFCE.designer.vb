<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnCompobanteFCE
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
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextLetra = New System.Windows.Forms.TextBox
        Me.MaskedFactura = New System.Windows.Forms.MaskedTextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckNO = New System.Windows.Forms.CheckBox
        Me.CheckSI = New System.Windows.Forms.CheckBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.RadioSCA = New System.Windows.Forms.RadioButton
        Me.RadioADC = New System.Windows.Forms.RadioButton
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel5.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.Label1)
        Me.Panel5.Controls.Add(Me.TextLetra)
        Me.Panel5.Controls.Add(Me.MaskedFactura)
        Me.Panel5.Controls.Add(Me.Label6)
        Me.Panel5.Controls.Add(Me.Label5)
        Me.Panel5.Location = New System.Drawing.Point(14, 24)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(644, 67)
        Me.Panel5.TabIndex = 304
        Me.Panel5.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(364, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(56, 16)
        Me.Label1.TabIndex = 214
        Me.Label1.Text = "Numero"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextLetra
        '
        Me.TextLetra.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextLetra.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLetra.Location = New System.Drawing.Point(269, 23)
        Me.TextLetra.MaxLength = 1
        Me.TextLetra.Name = "TextLetra"
        Me.TextLetra.Size = New System.Drawing.Size(44, 26)
        Me.TextLetra.TabIndex = 212
        Me.TextLetra.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MaskedFactura
        '
        Me.MaskedFactura.BackColor = System.Drawing.Color.White
        Me.MaskedFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedFactura.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedFactura.Location = New System.Drawing.Point(323, 23)
        Me.MaskedFactura.Mask = "0000-00000000"
        Me.MaskedFactura.Name = "MaskedFactura"
        Me.MaskedFactura.Size = New System.Drawing.Size(145, 26)
        Me.MaskedFactura.TabIndex = 199
        Me.MaskedFactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedFactura.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(270, 3)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(41, 16)
        Me.Label6.TabIndex = 157
        Me.Label6.Text = "Letra "
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(30, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(214, 16)
        Me.Label5.TabIndex = 156
        Me.Label5.Text = "Factura de Crédito que Afecta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckNO)
        Me.Panel1.Controls.Add(Me.CheckSI)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(14, 94)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(644, 163)
        Me.Panel1.TabIndex = 308
        '
        'CheckNO
        '
        Me.CheckNO.AutoSize = True
        Me.CheckNO.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckNO.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckNO.Location = New System.Drawing.Point(327, 52)
        Me.CheckNO.Name = "CheckNO"
        Me.CheckNO.Size = New System.Drawing.Size(52, 22)
        Me.CheckNO.TabIndex = 168
        Me.CheckNO.Text = "NO"
        Me.CheckNO.UseVisualStyleBackColor = True
        '
        'CheckSI
        '
        Me.CheckSI.AutoSize = True
        Me.CheckSI.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckSI.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSI.Location = New System.Drawing.Point(266, 52)
        Me.CheckSI.Name = "CheckSI"
        Me.CheckSI.Size = New System.Drawing.Size(46, 24)
        Me.CheckSI.TabIndex = 167
        Me.CheckSI.Text = "SI"
        Me.CheckSI.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.RadioSCA)
        Me.Panel2.Controls.Add(Me.RadioADC)
        Me.Panel2.Location = New System.Drawing.Point(9, 90)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(610, 56)
        Me.Panel2.TabIndex = 164
        '
        'RadioSCA
        '
        Me.RadioSCA.AutoSize = True
        Me.RadioSCA.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioSCA.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSCA.Location = New System.Drawing.Point(258, 13)
        Me.RadioSCA.Name = "RadioSCA"
        Me.RadioSCA.Size = New System.Drawing.Size(336, 22)
        Me.RadioSCA.TabIndex = 2
        Me.RadioSCA.TabStop = True
        Me.RadioSCA.Text = "Transferencia al Sistema de Circulación Abierta"
        Me.RadioSCA.UseVisualStyleBackColor = True
        '
        'RadioADC
        '
        Me.RadioADC.AutoSize = True
        Me.RadioADC.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.RadioADC.Checked = True
        Me.RadioADC.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioADC.Location = New System.Drawing.Point(3, 13)
        Me.RadioADC.Name = "RadioADC"
        Me.RadioADC.Size = New System.Drawing.Size(221, 22)
        Me.RadioADC.TabIndex = 0
        Me.RadioADC.TabStop = True
        Me.RadioADC.Text = "Agente de Deposito Colectivo"
        Me.RadioADC.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(56, 13)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(235, 18)
        Me.Label3.TabIndex = 159
        Me.Label3.Text = "Importe Factura debe ser mayor a "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(232, 16)
        Me.Label2.TabIndex = 157
        Me.Label2.Text = "Es Para Factura  Crédito (FCE) ?"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(239, 284)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 309
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'UnCompobanteFCE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(674, 335)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel5)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "UnCompobanteFCE"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Comprobante MiPyMEs (FCE)"
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents TextLetra As System.Windows.Forms.TextBox
    Friend WithEvents MaskedFactura As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioADC As System.Windows.Forms.RadioButton
    Friend WithEvents RadioSCA As System.Windows.Forms.RadioButton
    Friend WithEvents CheckNO As System.Windows.Forms.CheckBox
    Friend WithEvents CheckSI As System.Windows.Forms.CheckBox
End Class
