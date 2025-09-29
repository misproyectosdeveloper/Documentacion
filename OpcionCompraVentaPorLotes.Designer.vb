<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionCompraVentaPorLotes
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
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckNoFacturados = New System.Windows.Forms.CheckBox
        Me.CheckFacturados = New System.Windows.Forms.CheckBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.CheckSinStock = New System.Windows.Forms.CheckBox
        Me.CheckConStock = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(132, 52)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 26)
        Me.ComboAlias.TabIndex = 157
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(40, 56)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(44, 18)
        Me.Label2.TabIndex = 159
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(133, 21)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 26)
        Me.ComboEmisor.TabIndex = 156
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(39, 24)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(86, 18)
        Me.LabelEmisor.TabIndex = 158
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(418, 15)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(378, 71)
        Me.Panel1.TabIndex = 205
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(253, 21)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(108, 24)
        Me.DateTimeHasta.TabIndex = 163
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(64, 21)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(108, 24)
        Me.DateTimeDesde.TabIndex = 162
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(189, 23)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(52, 18)
        Me.Label7.TabIndex = 161
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 18)
        Me.Label4.TabIndex = 160
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckNoFacturados)
        Me.Panel2.Controls.Add(Me.CheckFacturados)
        Me.Panel2.Location = New System.Drawing.Point(43, 103)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(278, 40)
        Me.Panel2.TabIndex = 206
        '
        'CheckNoFacturados
        '
        Me.CheckNoFacturados.BackColor = System.Drawing.Color.Transparent
        Me.CheckNoFacturados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckNoFacturados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckNoFacturados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckNoFacturados.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckNoFacturados.Location = New System.Drawing.Point(129, 4)
        Me.CheckNoFacturados.Name = "CheckNoFacturados"
        Me.CheckNoFacturados.Size = New System.Drawing.Size(144, 30)
        Me.CheckNoFacturados.TabIndex = 232
        Me.CheckNoFacturados.Text = "No Facturados"
        Me.CheckNoFacturados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckNoFacturados.UseVisualStyleBackColor = False
        '
        'CheckFacturados
        '
        Me.CheckFacturados.BackColor = System.Drawing.Color.Transparent
        Me.CheckFacturados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckFacturados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckFacturados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckFacturados.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckFacturados.Location = New System.Drawing.Point(13, 4)
        Me.CheckFacturados.Name = "CheckFacturados"
        Me.CheckFacturados.Size = New System.Drawing.Size(110, 30)
        Me.CheckFacturados.TabIndex = 231
        Me.CheckFacturados.Text = "Facturados"
        Me.CheckFacturados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckFacturados.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(600, 94)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(183, 19)
        Me.Label10.TabIndex = 222
        Me.Label10.Text = "Especie"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(598, 116)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(187, 26)
        Me.ComboEspecie.TabIndex = 221
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(361, 257)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 224
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.CheckSinStock)
        Me.Panel4.Controls.Add(Me.CheckConStock)
        Me.Panel4.Location = New System.Drawing.Point(340, 103)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(238, 40)
        Me.Panel4.TabIndex = 251
        '
        'CheckSinStock
        '
        Me.CheckSinStock.BackColor = System.Drawing.Color.Transparent
        Me.CheckSinStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSinStock.Location = New System.Drawing.Point(123, 5)
        Me.CheckSinStock.Name = "CheckSinStock"
        Me.CheckSinStock.Size = New System.Drawing.Size(110, 30)
        Me.CheckSinStock.TabIndex = 230
        Me.CheckSinStock.Text = "Sin Stock"
        Me.CheckSinStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckSinStock.UseVisualStyleBackColor = False
        '
        'CheckConStock
        '
        Me.CheckConStock.BackColor = System.Drawing.Color.Transparent
        Me.CheckConStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConStock.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckConStock.Location = New System.Drawing.Point(10, 5)
        Me.CheckConStock.Name = "CheckConStock"
        Me.CheckConStock.Size = New System.Drawing.Size(110, 30)
        Me.CheckConStock.TabIndex = 229
        Me.CheckConStock.Text = "Con Stock"
        Me.CheckConStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckConStock.UseVisualStyleBackColor = False
        '
        'OpcionCompraVentaPorLotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(832, 320)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ComboEspecie)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Name = "OpcionCompraVentaPorLotes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents CheckConStock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckSinStock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckNoFacturados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckFacturados As System.Windows.Forms.CheckBox
End Class
