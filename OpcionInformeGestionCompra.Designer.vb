<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionInformeGestionCompra
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionInformeGestionCompra))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateIngresoHasta = New System.Windows.Forms.DateTimePicker
        Me.DateIngresoDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.DateFacturaHasta = New System.Windows.Forms.DateTimePicker
        Me.DateFacturaDesde = New System.Windows.Forms.DateTimePicker
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.CheckImportacion = New System.Windows.Forms.CheckBox
        Me.CheckDomesticas = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.DateIngresoHasta)
        Me.Panel1.Controls.Add(Me.DateIngresoDesde)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(161, 28)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(392, 71)
        Me.Panel1.TabIndex = 205
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(153, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(107, 16)
        Me.Label1.TabIndex = 164
        Me.Label1.Text = "Fecha Ingreso"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateIngresoHasta
        '
        Me.DateIngresoHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateIngresoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateIngresoHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateIngresoHasta.Location = New System.Drawing.Point(263, 33)
        Me.DateIngresoHasta.Name = "DateIngresoHasta"
        Me.DateIngresoHasta.Size = New System.Drawing.Size(117, 22)
        Me.DateIngresoHasta.TabIndex = 163
        '
        'DateIngresoDesde
        '
        Me.DateIngresoDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateIngresoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateIngresoDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateIngresoDesde.Location = New System.Drawing.Point(73, 32)
        Me.DateIngresoDesde.Name = "DateIngresoDesde"
        Me.DateIngresoDesde.Size = New System.Drawing.Size(114, 22)
        Me.DateIngresoDesde.TabIndex = 162
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(206, 37)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 161
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(9, 35)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 16)
        Me.Label4.TabIndex = 160
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.DateFacturaHasta)
        Me.Panel2.Controls.Add(Me.DateFacturaDesde)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Location = New System.Drawing.Point(161, 115)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(392, 71)
        Me.Panel2.TabIndex = 206
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(101, 4)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(217, 16)
        Me.Label2.TabIndex = 164
        Me.Label2.Text = "Fecha Facturas/Liquidaciones"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateFacturaHasta
        '
        Me.DateFacturaHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateFacturaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFacturaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateFacturaHasta.Location = New System.Drawing.Point(263, 33)
        Me.DateFacturaHasta.Name = "DateFacturaHasta"
        Me.DateFacturaHasta.Size = New System.Drawing.Size(117, 22)
        Me.DateFacturaHasta.TabIndex = 163
        '
        'DateFacturaDesde
        '
        Me.DateFacturaDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateFacturaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFacturaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateFacturaDesde.Location = New System.Drawing.Point(73, 32)
        Me.DateFacturaDesde.Name = "DateFacturaDesde"
        Me.DateFacturaDesde.Size = New System.Drawing.Size(114, 22)
        Me.DateFacturaDesde.TabIndex = 162
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(206, 37)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(49, 16)
        Me.Label3.TabIndex = 161
        Me.Label3.Text = "Hasta"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(9, 35)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(54, 16)
        Me.Label5.TabIndex = 160
        Me.Label5.Text = "Desde"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.CheckImportacion)
        Me.Panel3.Controls.Add(Me.CheckDomesticas)
        Me.Panel3.Location = New System.Drawing.Point(161, 200)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(392, 71)
        Me.Panel3.TabIndex = 207
        '
        'CheckImportacion
        '
        Me.CheckImportacion.AutoSize = True
        Me.CheckImportacion.Checked = True
        Me.CheckImportacion.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckImportacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckImportacion.Location = New System.Drawing.Point(237, 15)
        Me.CheckImportacion.Name = "CheckImportacion"
        Me.CheckImportacion.Size = New System.Drawing.Size(108, 20)
        Me.CheckImportacion.TabIndex = 166
        Me.CheckImportacion.Text = "Importación"
        Me.CheckImportacion.UseVisualStyleBackColor = True
        '
        'CheckDomesticas
        '
        Me.CheckDomesticas.AutoSize = True
        Me.CheckDomesticas.Checked = True
        Me.CheckDomesticas.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckDomesticas.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckDomesticas.Location = New System.Drawing.Point(59, 15)
        Me.CheckDomesticas.Name = "CheckDomesticas"
        Me.CheckDomesticas.Size = New System.Drawing.Size(109, 20)
        Me.CheckDomesticas.TabIndex = 165
        Me.CheckDomesticas.Text = "Domesticas"
        Me.CheckDomesticas.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.White
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(285, 459)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 208
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.CheckCerrado)
        Me.Panel4.Controls.Add(Me.CheckAbierto)
        Me.Panel4.Location = New System.Drawing.Point(289, 287)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(137, 40)
        Me.Panel4.TabIndex = 251
        Me.Panel4.Visible = False
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(67, 4)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(57, 30)
        Me.CheckCerrado.TabIndex = 230
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Transparent
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(10, 5)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(51, 30)
        Me.CheckAbierto.TabIndex = 229
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'OpcionInformeGestionCompra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(715, 544)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "OpcionInformeGestionCompra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "OpcionGestionCompra"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateIngresoHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateIngresoDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateFacturaHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateFacturaDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CheckDomesticas As System.Windows.Forms.CheckBox
    Friend WithEvents CheckImportacion As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
End Class
