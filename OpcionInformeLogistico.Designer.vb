<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionInformeLogistico
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionInformeLogistico))
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(121, 44)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 272
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(29, 48)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(48, 20)
        Me.Label13.TabIndex = 274
        Me.Label13.Text = "Alias"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(121, 11)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmisor.TabIndex = 271
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(27, 14)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(90, 20)
        Me.LabelEmisor.TabIndex = 273
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(31, 142)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(86, 20)
        Me.Label9.TabIndex = 270
        Me.Label9.Text = "Deposito "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ComboDeposito.Location = New System.Drawing.Point(121, 138)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(141, 28)
        Me.ComboDeposito.TabIndex = 269
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Location = New System.Drawing.Point(571, 92)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(137, 40)
        Me.Panel1.TabIndex = 268
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckCerrado.Checked = True
        Me.CheckCerrado.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(67, 5)
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
        Me.CheckAbierto.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(10, 5)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(54, 30)
        Me.CheckAbierto.TabIndex = 229
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(284, 141)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 20)
        Me.Label6.TabIndex = 267
        Me.Label6.Text = "Estado"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(363, 138)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(114, 28)
        Me.ComboEstado.TabIndex = 266
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(672, 15)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(128, 26)
        Me.DateTimeHasta.TabIndex = 265
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(475, 14)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(129, 26)
        Me.DateTimeDesde.TabIndex = 264
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(608, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(57, 20)
        Me.Label7.TabIndex = 263
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(404, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(61, 20)
        Me.Label8.TabIndex = 262
        Me.Label8.Text = "Desde"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboArticulo
        '
        Me.ComboArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(120, 188)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(263, 28)
        Me.ComboArticulo.TabIndex = 275
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(34, 191)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 20)
        Me.Label1.TabIndex = 276
        Me.Label1.Text = "Articulo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(374, 341)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 31)
        Me.ButtonAceptar.TabIndex = 277
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.ComboSucursales)
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Location = New System.Drawing.Point(21, 77)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(407, 40)
        Me.Panel4.TabIndex = 278
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(100, 5)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(263, 28)
        Me.ComboSucursales.TabIndex = 177
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 20)
        Me.Label2.TabIndex = 178
        Me.Label2.Text = "Sucursal"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpcionInformeLogistico
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(869, 399)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboArticulo)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.DateTimeHasta)
        Me.Controls.Add(Me.DateTimeDesde)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label8)
        Me.Name = "OpcionInformeLogistico"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        Me.Panel1.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
