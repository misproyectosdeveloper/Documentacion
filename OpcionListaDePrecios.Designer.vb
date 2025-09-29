<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionListaDePrecios
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
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboZona = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioListaDiaria = New System.Windows.Forms.RadioButton
        Me.RadioSemanaCompleta = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboVendedor = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.ComboZona)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Location = New System.Drawing.Point(101, 135)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(407, 40)
        Me.Panel4.TabIndex = 188
        '
        'ComboZona
        '
        Me.ComboZona.BackColor = System.Drawing.Color.White
        Me.ComboZona.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboZona.FormattingEnabled = True
        Me.ComboZona.Location = New System.Drawing.Point(138, 5)
        Me.ComboZona.Name = "ComboZona"
        Me.ComboZona.Size = New System.Drawing.Size(263, 28)
        Me.ComboZona.TabIndex = 177
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(50, 20)
        Me.Label1.TabIndex = 178
        Me.Label1.Text = "Zona"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(247, 418)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 186
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ComboAlias
        '
        Me.ComboAlias.BackColor = System.Drawing.Color.White
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(238, 88)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 183
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(106, 93)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 185
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.BackColor = System.Drawing.Color.White
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(238, 53)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmisor.TabIndex = 182
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(107, 57)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(65, 20)
        Me.LabelEmisor.TabIndex = 184
        Me.LabelEmisor.Text = "Cliente"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioListaDiaria)
        Me.Panel3.Controls.Add(Me.RadioSemanaCompleta)
        Me.Panel3.Location = New System.Drawing.Point(124, 299)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(402, 48)
        Me.Panel3.TabIndex = 189
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
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboVendedor)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(104, 181)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(437, 89)
        Me.Panel1.TabIndex = 193
        '
        'ComboVendedor
        '
        Me.ComboVendedor.BackColor = System.Drawing.Color.White
        Me.ComboVendedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVendedor.FormattingEnabled = True
        Me.ComboVendedor.Location = New System.Drawing.Point(137, 47)
        Me.ComboVendedor.Name = "ComboVendedor"
        Me.ComboVendedor.Size = New System.Drawing.Size(263, 28)
        Me.ComboVendedor.TabIndex = 192
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 51)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(87, 20)
        Me.Label3.TabIndex = 193
        Me.Label3.Text = "Vendedor"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(139, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(260, 20)
        Me.Label4.TabIndex = 194
        Me.Label4.Text = "Para Lista de Precios Por Vendedor"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpcionListaDePrecios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(651, 507)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Name = "OpcionListaDePrecios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opción Lista De Precios"
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboZona As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioListaDiaria As System.Windows.Forms.RadioButton
    Friend WithEvents RadioSemanaCompleta As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboVendedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
