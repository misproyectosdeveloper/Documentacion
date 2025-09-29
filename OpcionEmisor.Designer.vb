<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionEmisor
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
        Me.PanelBeneficiario = New System.Windows.Forms.Panel
        Me.ComboAliasACuenta = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboACuenta = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioListaDiaria = New System.Windows.Forms.RadioButton
        Me.RadioSemanaCompleta = New System.Windows.Forms.RadioButton
        Me.PanelBeneficiario.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(171, 487)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 8
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PanelBeneficiario
        '
        Me.PanelBeneficiario.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelBeneficiario.Controls.Add(Me.ComboAliasACuenta)
        Me.PanelBeneficiario.Controls.Add(Me.Label3)
        Me.PanelBeneficiario.Controls.Add(Me.ComboACuenta)
        Me.PanelBeneficiario.Controls.Add(Me.Label1)
        Me.PanelBeneficiario.Location = New System.Drawing.Point(35, 96)
        Me.PanelBeneficiario.Name = "PanelBeneficiario"
        Me.PanelBeneficiario.Size = New System.Drawing.Size(402, 90)
        Me.PanelBeneficiario.TabIndex = 4
        Me.PanelBeneficiario.Visible = False
        '
        'ComboAliasACuenta
        '
        Me.ComboAliasACuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAliasACuenta.FormattingEnabled = True
        Me.ComboAliasACuenta.Location = New System.Drawing.Point(102, 44)
        Me.ComboAliasACuenta.Name = "ComboAliasACuenta"
        Me.ComboAliasACuenta.Size = New System.Drawing.Size(263, 28)
        Me.ComboAliasACuenta.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 50)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 153
        Me.Label3.Text = "Alias"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboACuenta
        '
        Me.ComboACuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboACuenta.FormattingEnabled = True
        Me.ComboACuenta.Location = New System.Drawing.Point(103, 4)
        Me.ComboACuenta.Name = "ComboACuenta"
        Me.ComboACuenta.Size = New System.Drawing.Size(263, 28)
        Me.ComboACuenta.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(88, 13)
        Me.Label1.TabIndex = 147
        Me.Label1.Text = "Por Cuenta de"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(222, 387)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 147
        Me.PictureCandado.TabStop = False
        Me.PictureCandado.Visible = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboEmisor)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Location = New System.Drawing.Point(35, 8)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(402, 82)
        Me.Panel1.TabIndex = 152
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(102, 41)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 153
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 46)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 155
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(103, 4)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmisor.TabIndex = 152
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(11, 7)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(65, 13)
        Me.LabelEmisor.TabIndex = 154
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.TextNumero)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(36, 201)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(402, 39)
        Me.Panel2.TabIndex = 153
        Me.Panel2.Visible = False
        '
        'TextNumero
        '
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(166, 5)
        Me.TextNumero.MaxLength = 7
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(126, 26)
        Me.TextNumero.TabIndex = 211
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(62, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 13)
        Me.Label4.TabIndex = 155
        Me.Label4.Text = "Lists De Precio "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioListaDiaria)
        Me.Panel3.Controls.Add(Me.RadioSemanaCompleta)
        Me.Panel3.Location = New System.Drawing.Point(36, 243)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(402, 48)
        Me.Panel3.TabIndex = 154
        Me.Panel3.Visible = False
        '
        'RadioListaDiaria
        '
        Me.RadioListaDiaria.AutoSize = True
        Me.RadioListaDiaria.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioListaDiaria.Location = New System.Drawing.Point(15, 12)
        Me.RadioListaDiaria.Name = "RadioListaDiaria"
        Me.RadioListaDiaria.Size = New System.Drawing.Size(111, 22)
        Me.RadioListaDiaria.TabIndex = 1
        Me.RadioListaDiaria.TabStop = True
        Me.RadioListaDiaria.Text = "Lista Diaria"
        Me.RadioListaDiaria.UseVisualStyleBackColor = True
        '
        'RadioSemanaCompleta
        '
        Me.RadioSemanaCompleta.AutoSize = True
        Me.RadioSemanaCompleta.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSemanaCompleta.Location = New System.Drawing.Point(190, 12)
        Me.RadioSemanaCompleta.Name = "RadioSemanaCompleta"
        Me.RadioSemanaCompleta.Size = New System.Drawing.Size(205, 22)
        Me.RadioSemanaCompleta.TabIndex = 0
        Me.RadioSemanaCompleta.TabStop = True
        Me.RadioSemanaCompleta.Text = "Lista Semana Completa"
        Me.RadioSemanaCompleta.UseVisualStyleBackColor = True
        '
        'OpcionEmisor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(501, 574)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PanelBeneficiario)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionEmisor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        Me.PanelBeneficiario.ResumeLayout(False)
        Me.PanelBeneficiario.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PanelBeneficiario As System.Windows.Forms.Panel
    Friend WithEvents ComboACuenta As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboAliasACuenta As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioListaDiaria As System.Windows.Forms.RadioButton
    Friend WithEvents RadioSemanaCompleta As System.Windows.Forms.RadioButton
End Class
