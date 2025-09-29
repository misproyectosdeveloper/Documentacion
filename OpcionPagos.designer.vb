<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionPagos
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
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboEmpleados = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboAliasEmisor = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ComboTipo = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.DateTimeHasta)
        Me.Panel2.Controls.Add(Me.DateTimeDesde)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Location = New System.Drawing.Point(378, 61)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(387, 62)
        Me.Panel2.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 19)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 16)
        Me.Label4.TabIndex = 42
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(260, 16)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(114, 22)
        Me.DateTimeHasta.TabIndex = 4
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(74, 17)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(114, 22)
        Me.DateTimeDesde.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(203, 19)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 38
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.ComboEmpleados)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.ComboAliasEmisor)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboEmisor)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Location = New System.Drawing.Point(11, 137)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(403, 138)
        Me.Panel1.TabIndex = 7
        '
        'ComboEmpleados
        '
        Me.ComboEmpleados.BackColor = System.Drawing.Color.White
        Me.ComboEmpleados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmpleados.FormattingEnabled = True
        Me.ComboEmpleados.Location = New System.Drawing.Point(114, 88)
        Me.ComboEmpleados.Name = "ComboEmpleados"
        Me.ComboEmpleados.Size = New System.Drawing.Size(263, 23)
        Me.ComboEmpleados.TabIndex = 11
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(19, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(63, 20)
        Me.Label6.TabIndex = 297
        Me.Label6.Text = "Legajo"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboAliasEmisor
        '
        Me.ComboAliasEmisor.BackColor = System.Drawing.Color.White
        Me.ComboAliasEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAliasEmisor.FormattingEnabled = True
        Me.ComboAliasEmisor.Location = New System.Drawing.Point(114, 48)
        Me.ComboAliasEmisor.Name = "ComboAliasEmisor"
        Me.ComboAliasEmisor.Size = New System.Drawing.Size(263, 23)
        Me.ComboAliasEmisor.TabIndex = 9
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 53)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 20)
        Me.Label1.TabIndex = 295
        Me.Label1.Text = "Alias"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.BackColor = System.Drawing.Color.White
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(114, 13)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 23)
        Me.ComboEmisor.TabIndex = 7
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(12, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(90, 20)
        Me.Label5.TabIndex = 294
        Me.Label5.Text = "Proveedor"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ComboTipo)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Location = New System.Drawing.Point(6, 61)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(366, 62)
        Me.Panel3.TabIndex = 1
        '
        'ComboTipo
        '
        Me.ComboTipo.BackColor = System.Drawing.Color.White
        Me.ComboTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipo.FormattingEnabled = True
        Me.ComboTipo.Location = New System.Drawing.Point(153, 16)
        Me.ComboTipo.Name = "ComboTipo"
        Me.ComboTipo.Size = New System.Drawing.Size(196, 23)
        Me.ComboTipo.TabIndex = 1
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(137, 16)
        Me.Label3.TabIndex = 38
        Me.Label3.Text = "Tipo Comprobante"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(634, 155)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(119, 23)
        Me.ComboEstado.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(572, 157)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 16)
        Me.Label2.TabIndex = 292
        Me.Label2.Text = "Estado"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(336, 347)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(117, 31)
        Me.ButtonAceptar.TabIndex = 293
        Me.ButtonAceptar.Text = "Aceptar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'OpcionPagos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(772, 400)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "OpcionPagos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion "
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboAliasEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmpleados As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
End Class
