<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionCesion
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboDestino = New System.Windows.Forms.ComboBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioBanco = New System.Windows.Forms.RadioButton
        Me.RadioCliente = New System.Windows.Forms.RadioButton
        Me.RadioProveedor = New System.Windows.Forms.RadioButton
        Me.Label6 = New System.Windows.Forms.Label
        Me.DateFechaDesde = New System.Windows.Forms.DateTimePicker
        Me.CheckSoloPendientes = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.DateFechaHasta = New System.Windows.Forms.DateTimePicker
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(165, 116)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 18)
        Me.Label1.TabIndex = 162
        Me.Label1.Text = "Cedido A:"
        '
        'ComboDestino
        '
        Me.ComboDestino.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDestino.FormattingEnabled = True
        Me.ComboDestino.Location = New System.Drawing.Point(253, 111)
        Me.ComboDestino.Name = "ComboDestino"
        Me.ComboDestino.Size = New System.Drawing.Size(263, 28)
        Me.ComboDestino.TabIndex = 161
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.RadioBanco)
        Me.Panel3.Controls.Add(Me.RadioCliente)
        Me.Panel3.Controls.Add(Me.RadioProveedor)
        Me.Panel3.Location = New System.Drawing.Point(152, 44)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(402, 48)
        Me.Panel3.TabIndex = 160
        '
        'RadioBanco
        '
        Me.RadioBanco.AutoSize = True
        Me.RadioBanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioBanco.Location = New System.Drawing.Point(291, 12)
        Me.RadioBanco.Name = "RadioBanco"
        Me.RadioBanco.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RadioBanco.Size = New System.Drawing.Size(74, 22)
        Me.RadioBanco.TabIndex = 3
        Me.RadioBanco.Text = "Banco"
        Me.RadioBanco.UseVisualStyleBackColor = True
        '
        'RadioCliente
        '
        Me.RadioCliente.AutoSize = True
        Me.RadioCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioCliente.Location = New System.Drawing.Point(159, 12)
        Me.RadioCliente.Name = "RadioCliente"
        Me.RadioCliente.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RadioCliente.Size = New System.Drawing.Size(78, 22)
        Me.RadioCliente.TabIndex = 2
        Me.RadioCliente.Text = "Cliente"
        Me.RadioCliente.UseVisualStyleBackColor = True
        '
        'RadioProveedor
        '
        Me.RadioProveedor.AutoSize = True
        Me.RadioProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioProveedor.Location = New System.Drawing.Point(15, 12)
        Me.RadioProveedor.Name = "RadioProveedor"
        Me.RadioProveedor.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.RadioProveedor.Size = New System.Drawing.Size(104, 22)
        Me.RadioProveedor.TabIndex = 1
        Me.RadioProveedor.Text = "Proveedor"
        Me.RadioProveedor.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(101, 198)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(180, 18)
        Me.Label6.TabIndex = 172
        Me.Label6.Text = "Fecha Cesión:   Desde"
        '
        'DateFechaDesde
        '
        Me.DateFechaDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFechaDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateFechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateFechaDesde.Location = New System.Drawing.Point(294, 198)
        Me.DateFechaDesde.Name = "DateFechaDesde"
        Me.DateFechaDesde.Size = New System.Drawing.Size(108, 22)
        Me.DateFechaDesde.TabIndex = 171
        '
        'CheckSoloPendientes
        '
        Me.CheckSoloPendientes.AutoSize = True
        Me.CheckSoloPendientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSoloPendientes.Location = New System.Drawing.Point(446, 245)
        Me.CheckSoloPendientes.Name = "CheckSoloPendientes"
        Me.CheckSoloPendientes.Size = New System.Drawing.Size(141, 20)
        Me.CheckSoloPendientes.TabIndex = 173
        Me.CheckSoloPendientes.Text = "Solo Pendientes"
        Me.CheckSoloPendientes.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(288, 334)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 174
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(433, 198)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 18)
        Me.Label2.TabIndex = 176
        Me.Label2.Text = "Hasta"
        '
        'DateFechaHasta
        '
        Me.DateFechaHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFechaHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateFechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateFechaHasta.Location = New System.Drawing.Point(493, 198)
        Me.DateFechaHasta.Name = "DateFechaHasta"
        Me.DateFechaHasta.Size = New System.Drawing.Size(108, 22)
        Me.DateFechaHasta.TabIndex = 175
        '
        'OpcionCesion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(707, 408)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.DateFechaHasta)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.CheckSoloPendientes)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.DateFechaDesde)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboDestino)
        Me.Controls.Add(Me.Panel3)
        Me.Name = "OpcionCesion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Cesión"
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboDestino As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioBanco As System.Windows.Forms.RadioButton
    Friend WithEvents RadioCliente As System.Windows.Forms.RadioButton
    Friend WithEvents RadioProveedor As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents DateFechaDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents CheckSoloPendientes As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DateFechaHasta As System.Windows.Forms.DateTimePicker
End Class
