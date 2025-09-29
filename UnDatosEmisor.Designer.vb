<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnDatosEmisor
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
        Me.ComboPais = New System.Windows.Forms.ComboBox
        Me.ComboTipoIva = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextCuit = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextProvincia = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.TextFaxes = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextTelefonos = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextLocalidad = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextCalle = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.LabelEmpresa = New System.Windows.Forms.Label
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.CheckCodigoCliente = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'ComboPais
        '
        Me.ComboPais.BackColor = System.Drawing.Color.White
        Me.ComboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboPais.Enabled = False
        Me.ComboPais.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPais.FormattingEnabled = True
        Me.ComboPais.Location = New System.Drawing.Point(740, 112)
        Me.ComboPais.Name = "ComboPais"
        Me.ComboPais.Size = New System.Drawing.Size(144, 21)
        Me.ComboPais.TabIndex = 154
        '
        'ComboTipoIva
        '
        Me.ComboTipoIva.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboTipoIva.Enabled = False
        Me.ComboTipoIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoIva.FormattingEnabled = True
        Me.ComboTipoIva.Location = New System.Drawing.Point(740, 143)
        Me.ComboTipoIva.Name = "ComboTipoIva"
        Me.ComboTipoIva.Size = New System.Drawing.Size(145, 20)
        Me.ComboTipoIva.TabIndex = 153
        Me.ComboTipoIva.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(700, 145)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(32, 16)
        Me.Label14.TabIndex = 152
        Me.Label14.Text = "IVA"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuit
        '
        Me.TextCuit.BackColor = System.Drawing.Color.White
        Me.TextCuit.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuit.Location = New System.Drawing.Point(535, 142)
        Me.TextCuit.MaxLength = 20
        Me.TextCuit.Name = "TextCuit"
        Me.TextCuit.ReadOnly = True
        Me.TextCuit.Size = New System.Drawing.Size(159, 22)
        Me.TextCuit.TabIndex = 151
        Me.TextCuit.TabStop = False
        Me.TextCuit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(696, 115)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(39, 16)
        Me.Label10.TabIndex = 150
        Me.Label10.Text = "Pais"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(494, 146)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 16)
        Me.Label4.TabIndex = 149
        Me.Label4.Text = "Cuit"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextProvincia
        '
        Me.TextProvincia.BackColor = System.Drawing.Color.White
        Me.TextProvincia.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextProvincia.Location = New System.Drawing.Point(592, 112)
        Me.TextProvincia.MaxLength = 20
        Me.TextProvincia.Name = "TextProvincia"
        Me.TextProvincia.ReadOnly = True
        Me.TextProvincia.Size = New System.Drawing.Size(101, 22)
        Me.TextProvincia.TabIndex = 148
        Me.TextProvincia.TabStop = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(13, 117)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(44, 16)
        Me.Label21.TabIndex = 147
        Me.Label21.Text = "Calle"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextFaxes
        '
        Me.TextFaxes.BackColor = System.Drawing.Color.White
        Me.TextFaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFaxes.Location = New System.Drawing.Point(370, 143)
        Me.TextFaxes.MaxLength = 30
        Me.TextFaxes.Name = "TextFaxes"
        Me.TextFaxes.ReadOnly = True
        Me.TextFaxes.Size = New System.Drawing.Size(117, 22)
        Me.TextFaxes.TabIndex = 146
        Me.TextFaxes.TabStop = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(287, 146)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(50, 16)
        Me.Label6.TabIndex = 145
        Me.Label6.Text = "Faxes"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTelefonos
        '
        Me.TextTelefonos.BackColor = System.Drawing.Color.White
        Me.TextTelefonos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTelefonos.Location = New System.Drawing.Point(98, 143)
        Me.TextTelefonos.MaxLength = 30
        Me.TextTelefonos.Name = "TextTelefonos"
        Me.TextTelefonos.ReadOnly = True
        Me.TextTelefonos.Size = New System.Drawing.Size(174, 22)
        Me.TextTelefonos.TabIndex = 144
        Me.TextTelefonos.TabStop = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 146)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(78, 16)
        Me.Label5.TabIndex = 143
        Me.Label5.Text = "Telefonos"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextLocalidad
        '
        Me.TextLocalidad.BackColor = System.Drawing.Color.White
        Me.TextLocalidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLocalidad.Location = New System.Drawing.Point(369, 112)
        Me.TextLocalidad.MaxLength = 20
        Me.TextLocalidad.Name = "TextLocalidad"
        Me.TextLocalidad.ReadOnly = True
        Me.TextLocalidad.Size = New System.Drawing.Size(118, 22)
        Me.TextLocalidad.TabIndex = 142
        Me.TextLocalidad.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(286, 115)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(77, 16)
        Me.Label11.TabIndex = 141
        Me.Label11.Text = "Localidad"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCalle
        '
        Me.TextCalle.BackColor = System.Drawing.Color.White
        Me.TextCalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCalle.Location = New System.Drawing.Point(98, 112)
        Me.TextCalle.Margin = New System.Windows.Forms.Padding(0)
        Me.TextCalle.MaxLength = 30
        Me.TextCalle.Name = "TextCalle"
        Me.TextCalle.ReadOnly = True
        Me.TextCalle.Size = New System.Drawing.Size(175, 22)
        Me.TextCalle.TabIndex = 140
        Me.TextCalle.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(494, 115)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(73, 16)
        Me.Label12.TabIndex = 139
        Me.Label12.Text = "Provincia"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelEmpresa
        '
        Me.LabelEmpresa.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmpresa.Location = New System.Drawing.Point(212, 46)
        Me.LabelEmpresa.Name = "LabelEmpresa"
        Me.LabelEmpresa.Size = New System.Drawing.Size(512, 35)
        Me.LabelEmpresa.TabIndex = 156
        Me.LabelEmpresa.Text = "EMPRESA"
        Me.LabelEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.ForeColor = System.Drawing.Color.Black
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(184, 220)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(145, 20)
        Me.ComboMoneda.TabIndex = 158
        Me.ComboMoneda.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(14, 222)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(162, 16)
        Me.Label7.TabIndex = 157
        Me.Label7.Text = "Moneda en que Opera"
        '
        'CheckCodigoCliente
        '
        Me.CheckCodigoCliente.AutoSize = True
        Me.CheckCodigoCliente.Enabled = False
        Me.CheckCodigoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCodigoCliente.Location = New System.Drawing.Point(375, 222)
        Me.CheckCodigoCliente.Name = "CheckCodigoCliente"
        Me.CheckCodigoCliente.Size = New System.Drawing.Size(173, 20)
        Me.CheckCodigoCliente.TabIndex = 159
        Me.CheckCodigoCliente.Text = "Tiene Codigo Cliente"
        Me.CheckCodigoCliente.UseVisualStyleBackColor = True
        '
        'UnDatosEmisor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ClientSize = New System.Drawing.Size(914, 302)
        Me.Controls.Add(Me.CheckCodigoCliente)
        Me.Controls.Add(Me.ComboMoneda)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.LabelEmpresa)
        Me.Controls.Add(Me.ComboPais)
        Me.Controls.Add(Me.ComboTipoIva)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.TextCuit)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextProvincia)
        Me.Controls.Add(Me.Label21)
        Me.Controls.Add(Me.TextFaxes)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextTelefonos)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextLocalidad)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TextCalle)
        Me.Controls.Add(Me.Label12)
        Me.Name = "UnDatosEmisor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Datos Empresa"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboPais As System.Windows.Forms.ComboBox
    Friend WithEvents ComboTipoIva As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextCuit As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextProvincia As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TextFaxes As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextTelefonos As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextLocalidad As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextCalle As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents LabelEmpresa As System.Windows.Forms.Label
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents CheckCodigoCliente As System.Windows.Forms.CheckBox
End Class
