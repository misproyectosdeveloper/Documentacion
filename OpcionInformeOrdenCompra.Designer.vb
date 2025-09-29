<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionInformeOrdenCompra
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.FechaDesde = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextOrdenCompra = New System.Windows.Forms.TextBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.FechaHasta = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.TextPeriodoDesde = New System.Windows.Forms.TextBox
        Me.PictureAlmanaquePeriodoDesde = New System.Windows.Forms.PictureBox
        Me.PictureAlmanaquePeriodoHasta = New System.Windows.Forms.PictureBox
        Me.TextPeriodoHasta = New System.Windows.Forms.TextBox
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaquePeriodoDesde, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureAlmanaquePeriodoHasta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboAlias)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Location = New System.Drawing.Point(42, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(402, 72)
        Me.Panel1.TabIndex = 153
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(120, 37)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 37)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 155
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(121, 4)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(263, 28)
        Me.ComboProveedor.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(11, 7)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(90, 20)
        Me.LabelEmisor.TabIndex = 154
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaDesde
        '
        Me.FechaDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaDesde.CustomFormat = "dd/MM/yyyy"
        Me.FechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaDesde.Location = New System.Drawing.Point(305, 152)
        Me.FechaDesde.Name = "FechaDesde"
        Me.FechaDesde.Size = New System.Drawing.Size(129, 26)
        Me.FechaDesde.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(51, 156)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(246, 20)
        Me.Label1.TabIndex = 218
        Me.Label1.Text = "Fecha O. de Compra:   Desde"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(50, 204)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(246, 20)
        Me.Label3.TabIndex = 220
        Me.Label3.Text = "Periodo Entrega:          Desde"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(457, 204)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(57, 20)
        Me.Label4.TabIndex = 222
        Me.Label4.Text = "Hasta"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(51, 256)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(155, 20)
        Me.Label5.TabIndex = 223
        Me.Label5.Text = "Orden de Compra:"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextOrdenCompra
        '
        Me.TextOrdenCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextOrdenCompra.Location = New System.Drawing.Point(240, 253)
        Me.TextOrdenCompra.MaxLength = 7
        Me.TextOrdenCompra.Name = "TextOrdenCompra"
        Me.TextOrdenCompra.Size = New System.Drawing.Size(127, 26)
        Me.TextOrdenCompra.TabIndex = 8
        Me.TextOrdenCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(284, 370)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 31)
        Me.ButtonAceptar.TabIndex = 225
        Me.ButtonAceptar.TabStop = False
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(458, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 20)
        Me.Label6.TabIndex = 227
        Me.Label6.Text = "Hasta"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaHasta
        '
        Me.FechaHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaHasta.CustomFormat = "dd/MM/yyyy"
        Me.FechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaHasta.Location = New System.Drawing.Point(528, 152)
        Me.FechaHasta.Name = "FechaHasta"
        Me.FechaHasta.Size = New System.Drawing.Size(129, 26)
        Me.FechaHasta.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(453, 47)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(66, 20)
        Me.Label7.TabIndex = 251
        Me.Label7.Text = "Estado"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(529, 44)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(114, 28)
        Me.ComboEstado.TabIndex = 3
        '
        'TextPeriodoDesde
        '
        Me.TextPeriodoDesde.BackColor = System.Drawing.Color.White
        Me.TextPeriodoDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPeriodoDesde.Location = New System.Drawing.Point(304, 201)
        Me.TextPeriodoDesde.MaxLength = 10
        Me.TextPeriodoDesde.Name = "TextPeriodoDesde"
        Me.TextPeriodoDesde.Size = New System.Drawing.Size(98, 26)
        Me.TextPeriodoDesde.TabIndex = 6
        Me.TextPeriodoDesde.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureAlmanaquePeriodoDesde
        '
        Me.PictureAlmanaquePeriodoDesde.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaquePeriodoDesde.Location = New System.Drawing.Point(408, 202)
        Me.PictureAlmanaquePeriodoDesde.Name = "PictureAlmanaquePeriodoDesde"
        Me.PictureAlmanaquePeriodoDesde.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaquePeriodoDesde.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaquePeriodoDesde.TabIndex = 1007
        Me.PictureAlmanaquePeriodoDesde.TabStop = False
        '
        'PictureAlmanaquePeriodoHasta
        '
        Me.PictureAlmanaquePeriodoHasta.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaquePeriodoHasta.Location = New System.Drawing.Point(633, 201)
        Me.PictureAlmanaquePeriodoHasta.Name = "PictureAlmanaquePeriodoHasta"
        Me.PictureAlmanaquePeriodoHasta.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaquePeriodoHasta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaquePeriodoHasta.TabIndex = 1009
        Me.PictureAlmanaquePeriodoHasta.TabStop = False
        '
        'TextPeriodoHasta
        '
        Me.TextPeriodoHasta.BackColor = System.Drawing.Color.White
        Me.TextPeriodoHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPeriodoHasta.Location = New System.Drawing.Point(528, 201)
        Me.TextPeriodoHasta.MaxLength = 10
        Me.TextPeriodoHasta.Name = "TextPeriodoHasta"
        Me.TextPeriodoHasta.Size = New System.Drawing.Size(101, 26)
        Me.TextPeriodoHasta.TabIndex = 7
        Me.TextPeriodoHasta.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'OpcionInformeOrdenCompra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 20.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(698, 440)
        Me.Controls.Add(Me.PictureAlmanaquePeriodoHasta)
        Me.Controls.Add(Me.TextPeriodoHasta)
        Me.Controls.Add(Me.PictureAlmanaquePeriodoDesde)
        Me.Controls.Add(Me.TextPeriodoDesde)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.FechaHasta)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextOrdenCompra)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FechaDesde)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "OpcionInformeOrdenCompra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaquePeriodoDesde, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureAlmanaquePeriodoHasta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents FechaDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextOrdenCompra As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents FechaHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents TextPeriodoDesde As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaquePeriodoDesde As System.Windows.Forms.PictureBox
    Friend WithEvents PictureAlmanaquePeriodoHasta As System.Windows.Forms.PictureBox
    Friend WithEvents TextPeriodoHasta As System.Windows.Forms.TextBox
End Class
