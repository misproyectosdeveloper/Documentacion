<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnTXTPerRetPercibida
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
        Me.TextHasta = New System.Windows.Forms.TextBox
        Me.TextDesde = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioPercepcion = New System.Windows.Forms.RadioButton
        Me.RadioRetencion = New System.Windows.Forms.RadioButton
        Me.ButtonProcesar = New System.Windows.Forms.Button
        Me.ComboTipo = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboJurisdiccion = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.LabelImporte = New System.Windows.Forms.Label
        Me.LabelArchivo = New System.Windows.Forms.Label
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'TextHasta
        '
        Me.TextHasta.BackColor = System.Drawing.Color.White
        Me.TextHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextHasta.Location = New System.Drawing.Point(493, 122)
        Me.TextHasta.MaxLength = 7
        Me.TextHasta.Name = "TextHasta"
        Me.TextHasta.ReadOnly = True
        Me.TextHasta.Size = New System.Drawing.Size(99, 21)
        Me.TextHasta.TabIndex = 219
        Me.TextHasta.TabStop = False
        Me.TextHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextDesde
        '
        Me.TextDesde.BackColor = System.Drawing.Color.White
        Me.TextDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDesde.Location = New System.Drawing.Point(326, 122)
        Me.TextDesde.MaxLength = 7
        Me.TextDesde.Name = "TextDesde"
        Me.TextDesde.ReadOnly = True
        Me.TextDesde.Size = New System.Drawing.Size(99, 21)
        Me.TextDesde.TabIndex = 218
        Me.TextDesde.TabStop = False
        Me.TextDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(153, 125)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(115, 16)
        Me.Label5.TabIndex = 217
        Me.Label5.Text = "Fecha a Procesar"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(441, 126)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(44, 16)
        Me.Label3.TabIndex = 216
        Me.Label3.Text = "Hasta"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(272, 126)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 16)
        Me.Label2.TabIndex = 215
        Me.Label2.Text = "Desde"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioPercepcion)
        Me.Panel3.Controls.Add(Me.RadioRetencion)
        Me.Panel3.Enabled = False
        Me.Panel3.Location = New System.Drawing.Point(127, 22)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(503, 48)
        Me.Panel3.TabIndex = 220
        '
        'RadioPercepcion
        '
        Me.RadioPercepcion.AutoSize = True
        Me.RadioPercepcion.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.RadioPercepcion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPercepcion.Location = New System.Drawing.Point(50, 12)
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
        Me.RadioRetencion.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.RadioRetencion.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.RadioRetencion.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioRetencion.Location = New System.Drawing.Point(338, 13)
        Me.RadioRetencion.Name = "RadioRetencion"
        Me.RadioRetencion.Size = New System.Drawing.Size(108, 23)
        Me.RadioRetencion.TabIndex = 0
        Me.RadioRetencion.TabStop = True
        Me.RadioRetencion.Text = "Retención"
        Me.RadioRetencion.UseVisualStyleBackColor = True
        '
        'ButtonProcesar
        '
        Me.ButtonProcesar.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonProcesar.Location = New System.Drawing.Point(208, 264)
        Me.ButtonProcesar.Name = "ButtonProcesar"
        Me.ButtonProcesar.Size = New System.Drawing.Size(322, 43)
        Me.ButtonProcesar.TabIndex = 100
        Me.ButtonProcesar.Text = "PROCESAR"
        Me.ButtonProcesar.UseVisualStyleBackColor = True
        '
        'ComboTipo
        '
        Me.ComboTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipo.FormattingEnabled = True
        Me.ComboTipo.Location = New System.Drawing.Point(244, 200)
        Me.ComboTipo.Name = "ComboTipo"
        Me.ComboTipo.Size = New System.Drawing.Size(130, 21)
        Me.ComboTipo.TabIndex = 227
        Me.ComboTipo.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(124, 202)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(111, 16)
        Me.Label6.TabIndex = 226
        Me.Label6.Text = "Percepción de IB"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboJurisdiccion
        '
        Me.ComboJurisdiccion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboJurisdiccion.FormattingEnabled = True
        Me.ComboJurisdiccion.Location = New System.Drawing.Point(478, 201)
        Me.ComboJurisdiccion.Name = "ComboJurisdiccion"
        Me.ComboJurisdiccion.Size = New System.Drawing.Size(152, 21)
        Me.ComboJurisdiccion.TabIndex = 233
        Me.ComboJurisdiccion.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(390, 202)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(79, 16)
        Me.Label12.TabIndex = 234
        Me.Label12.Text = "Jurisdicción"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelImporte
        '
        Me.LabelImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelImporte.Location = New System.Drawing.Point(188, 359)
        Me.LabelImporte.Name = "LabelImporte"
        Me.LabelImporte.Size = New System.Drawing.Size(382, 29)
        Me.LabelImporte.TabIndex = 235
        Me.LabelImporte.Text = "Importe Procesados: "
        Me.LabelImporte.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LabelImporte.Visible = False
        '
        'LabelArchivo
        '
        Me.LabelArchivo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelArchivo.Location = New System.Drawing.Point(32, 421)
        Me.LabelArchivo.Name = "LabelArchivo"
        Me.LabelArchivo.Size = New System.Drawing.Size(695, 29)
        Me.LabelArchivo.TabIndex = 236
        Me.LabelArchivo.Text = "Archivo Generado: "
        Me.LabelArchivo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.LabelArchivo.Visible = False
        '
        'UnTXTPerRetPercibida
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(752, 546)
        Me.Controls.Add(Me.LabelArchivo)
        Me.Controls.Add(Me.LabelImporte)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.ComboJurisdiccion)
        Me.Controls.Add(Me.ComboTipo)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ButtonProcesar)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.TextHasta)
        Me.Controls.Add(Me.TextDesde)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Name = "UnTXTPerRetPercibida"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "UnTXTPerRetPercibida"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextHasta As System.Windows.Forms.TextBox
    Friend WithEvents TextDesde As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioPercepcion As System.Windows.Forms.RadioButton
    Friend WithEvents RadioRetencion As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonProcesar As System.Windows.Forms.Button
    Friend WithEvents ComboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboJurisdiccion As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents LabelImporte As System.Windows.Forms.Label
    Friend WithEvents LabelArchivo As System.Windows.Forms.Label
End Class
