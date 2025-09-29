<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnTxtARBA
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
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioPercepcion = New System.Windows.Forms.RadioButton
        Me.RadioRetencion = New System.Windows.Forms.RadioButton
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonRetencion = New System.Windows.Forms.Button
        Me.ButtonPercepcion = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextDesde = New System.Windows.Forms.TextBox
        Me.TextHasta = New System.Windows.Forms.TextBox
        Me.ButtonRencionGanancias = New System.Windows.Forms.Button
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioPercepcion)
        Me.Panel3.Controls.Add(Me.RadioRetencion)
        Me.Panel3.Enabled = False
        Me.Panel3.Location = New System.Drawing.Point(148, 11)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(344, 48)
        Me.Panel3.TabIndex = 155
        '
        'RadioPercepcion
        '
        Me.RadioPercepcion.AutoSize = True
        Me.RadioPercepcion.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.RadioPercepcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPercepcion.Location = New System.Drawing.Point(15, 12)
        Me.RadioPercepcion.Name = "RadioPercepcion"
        Me.RadioPercepcion.Size = New System.Drawing.Size(111, 22)
        Me.RadioPercepcion.TabIndex = 1
        Me.RadioPercepcion.TabStop = True
        Me.RadioPercepcion.Text = "Percepción"
        Me.RadioPercepcion.UseVisualStyleBackColor = True
        '
        'RadioRetencion
        '
        Me.RadioRetencion.AutoSize = True
        Me.RadioRetencion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioRetencion.Location = New System.Drawing.Point(221, 12)
        Me.RadioRetencion.Name = "RadioRetencion"
        Me.RadioRetencion.Size = New System.Drawing.Size(102, 22)
        Me.RadioRetencion.TabIndex = 0
        Me.RadioRetencion.TabStop = True
        Me.RadioRetencion.Text = "Retención"
        Me.RadioRetencion.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextNumero)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ButtonRetencion)
        Me.Panel1.Location = New System.Drawing.Point(91, 171)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(467, 88)
        Me.Panel1.TabIndex = 158
        Me.Panel1.Visible = False
        '
        'TextNumero
        '
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(92, 31)
        Me.TextNumero.MaxLength = 7
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(49, 26)
        Me.TextNumero.TabIndex = 212
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 16)
        Me.Label1.TabIndex = 159
        Me.Label1.Text = "Quincena:"
        '
        'ButtonRetencion
        '
        Me.ButtonRetencion.BackColor = System.Drawing.Color.Salmon
        Me.ButtonRetencion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRetencion.Location = New System.Drawing.Point(156, 15)
        Me.ButtonRetencion.Name = "ButtonRetencion"
        Me.ButtonRetencion.Size = New System.Drawing.Size(289, 51)
        Me.ButtonRetencion.TabIndex = 158
        Me.ButtonRetencion.Text = "Generar TXT Importación Retención Actividad 6"
        Me.ButtonRetencion.UseVisualStyleBackColor = False
        '
        'ButtonPercepcion
        '
        Me.ButtonPercepcion.BackColor = System.Drawing.Color.Salmon
        Me.ButtonPercepcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPercepcion.Location = New System.Drawing.Point(138, 235)
        Me.ButtonPercepcion.Name = "ButtonPercepcion"
        Me.ButtonPercepcion.Size = New System.Drawing.Size(356, 45)
        Me.ButtonPercepcion.TabIndex = 161
        Me.ButtonPercepcion.Text = "Generar TXT Importación Percepción Actividad 7"
        Me.ButtonPercepcion.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 404)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(652, 18)
        Me.Label4.TabIndex = 163
        Me.Label4.Text = "Label4"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(94, 90)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 13)
        Me.Label5.TabIndex = 169
        Me.Label5.Text = "Fecha a Procesar"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(375, 91)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 168
        Me.Label3.Text = "Hasta"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(206, 91)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 167
        Me.Label2.Text = "Desde"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextDesde
        '
        Me.TextDesde.BackColor = System.Drawing.Color.White
        Me.TextDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDesde.Location = New System.Drawing.Point(260, 87)
        Me.TextDesde.MaxLength = 7
        Me.TextDesde.Name = "TextDesde"
        Me.TextDesde.ReadOnly = True
        Me.TextDesde.Size = New System.Drawing.Size(99, 21)
        Me.TextDesde.TabIndex = 213
        Me.TextDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextHasta
        '
        Me.TextHasta.BackColor = System.Drawing.Color.White
        Me.TextHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextHasta.Location = New System.Drawing.Point(427, 87)
        Me.TextHasta.MaxLength = 7
        Me.TextHasta.Name = "TextHasta"
        Me.TextHasta.ReadOnly = True
        Me.TextHasta.Size = New System.Drawing.Size(99, 21)
        Me.TextHasta.TabIndex = 214
        Me.TextHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ButtonRencionGanancias
        '
        Me.ButtonRencionGanancias.BackColor = System.Drawing.Color.Salmon
        Me.ButtonRencionGanancias.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRencionGanancias.Location = New System.Drawing.Point(189, 279)
        Me.ButtonRencionGanancias.Name = "ButtonRencionGanancias"
        Me.ButtonRencionGanancias.Size = New System.Drawing.Size(289, 51)
        Me.ButtonRencionGanancias.TabIndex = 215
        Me.ButtonRencionGanancias.Text = "Generar TXT Importación Retención Ganancias"
        Me.ButtonRencionGanancias.UseVisualStyleBackColor = False
        Me.ButtonRencionGanancias.Visible = False
        '
        'UnTxtARBA
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(667, 448)
        Me.Controls.Add(Me.ButtonRencionGanancias)
        Me.Controls.Add(Me.TextHasta)
        Me.Controls.Add(Me.TextDesde)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.ButtonPercepcion)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel3)
        Me.Name = "UnTxtARBA"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TXT Importación  para ARBA"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioPercepcion As System.Windows.Forms.RadioButton
    Friend WithEvents RadioRetencion As System.Windows.Forms.RadioButton
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonPercepcion As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonRetencion As System.Windows.Forms.Button
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextDesde As System.Windows.Forms.TextBox
    Friend WithEvents TextHasta As System.Windows.Forms.TextBox
    Friend WithEvents ButtonRencionGanancias As System.Windows.Forms.Button
End Class
