<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaSICORE
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.Label7 = New System.Windows.Forms.Label
        Me.ButtonEXCELComprasComprobantes = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptarCompras = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ButtonCITIVentas = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonEXCELComprasComprobantesVentas = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Controls.Add(Me.ButtonEXCELComprasComprobantes)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.ButtonAceptarCompras)
        Me.Panel2.Location = New System.Drawing.Point(142, 99)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(723, 202)
        Me.Panel2.TabIndex = 182
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(98, 113)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(553, 30)
        Me.Label2.TabIndex = 167
        Me.Label2.Text = "Se genera los Archivos : REGINFO_CV_COMPRAS_CBTE.TXT  y  REGINFO_CV_COMPRAS_ALICU" & _
            "OTAS.TXT"
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.Label7)
        Me.Panel5.Location = New System.Drawing.Point(89, 156)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(579, 43)
        Me.Panel5.TabIndex = 166
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(4, 8)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(553, 30)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Procesa los Comprobantes: Facturas Proveedores, Recibos Debito/Creidito Proveedor" & _
            "es, N.V.L.P., Liquidaciones, Gastos Bancarios, Pago de Prestamos.  NO PROCESA OP" & _
            "ERACIONES DE IMPORTACION."
        '
        'ButtonEXCELComprasComprobantes
        '
        Me.ButtonEXCELComprasComprobantes.BackColor = System.Drawing.Color.DarkKhaki
        Me.ButtonEXCELComprasComprobantes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEXCELComprasComprobantes.Location = New System.Drawing.Point(396, 48)
        Me.ButtonEXCELComprasComprobantes.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEXCELComprasComprobantes.Name = "ButtonEXCELComprasComprobantes"
        Me.ButtonEXCELComprasComprobantes.Size = New System.Drawing.Size(125, 38)
        Me.ButtonEXCELComprasComprobantes.TabIndex = 165
        Me.ButtonEXCELComprasComprobantes.Text = "Exportar a EXCEL (Solo para control)"
        Me.ButtonEXCELComprasComprobantes.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(298, 4)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(144, 20)
        Me.Label1.TabIndex = 164
        Me.Label1.Text = "C.I.T.I. Compras "
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptarCompras
        '
        Me.ButtonAceptarCompras.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptarCompras.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptarCompras.Location = New System.Drawing.Point(225, 48)
        Me.ButtonAceptarCompras.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptarCompras.Name = "ButtonAceptarCompras"
        Me.ButtonAceptarCompras.Size = New System.Drawing.Size(132, 35)
        Me.ButtonAceptarCompras.TabIndex = 84
        Me.ButtonAceptarCompras.Text = "Generar Archivo"
        Me.ButtonAceptarCompras.UseVisualStyleBackColor = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.DateTimeHasta)
        Me.Panel3.Controls.Add(Me.DateTimeDesde)
        Me.Panel3.Location = New System.Drawing.Point(143, 40)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(722, 52)
        Me.Panel3.TabIndex = 183
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(83, 18)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(107, 13)
        Me.Label5.TabIndex = 164
        Me.Label5.Text = "Fecha a Procesar"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(393, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(40, 13)
        Me.Label3.TabIndex = 163
        Me.Label3.Text = "Hasta"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(209, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 162
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(439, 15)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(106, 20)
        Me.DateTimeHasta.TabIndex = 161
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(259, 15)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(97, 20)
        Me.DateTimeDesde.TabIndex = 160
        '
        'ButtonCITIVentas
        '
        Me.ButtonCITIVentas.BackColor = System.Drawing.Color.Yellow
        Me.ButtonCITIVentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCITIVentas.Location = New System.Drawing.Point(223, 69)
        Me.ButtonCITIVentas.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonCITIVentas.Name = "ButtonCITIVentas"
        Me.ButtonCITIVentas.Size = New System.Drawing.Size(132, 35)
        Me.ButtonCITIVentas.TabIndex = 84
        Me.ButtonCITIVentas.Text = "Generar Archivo"
        Me.ButtonCITIVentas.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(303, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(125, 20)
        Me.Label6.TabIndex = 164
        Me.Label6.Text = "C.I.T.I  Ventas"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonEXCELComprasComprobantesVentas
        '
        Me.ButtonEXCELComprasComprobantesVentas.BackColor = System.Drawing.Color.DarkKhaki
        Me.ButtonEXCELComprasComprobantesVentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEXCELComprasComprobantesVentas.Location = New System.Drawing.Point(393, 69)
        Me.ButtonEXCELComprasComprobantesVentas.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEXCELComprasComprobantesVentas.Name = "ButtonEXCELComprasComprobantesVentas"
        Me.ButtonEXCELComprasComprobantesVentas.Size = New System.Drawing.Size(132, 35)
        Me.ButtonEXCELComprasComprobantesVentas.TabIndex = 165
        Me.ButtonEXCELComprasComprobantesVentas.Text = "Exportar a EXCEL (Solo para control)"
        Me.ButtonEXCELComprasComprobantesVentas.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.Panel6)
        Me.Panel4.Controls.Add(Me.ButtonEXCELComprasComprobantesVentas)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.ButtonCITIVentas)
        Me.Panel4.Location = New System.Drawing.Point(142, 307)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(723, 229)
        Me.Panel4.TabIndex = 184
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(105, 138)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(553, 30)
        Me.Label9.TabIndex = 168
        Me.Label9.Text = "Se genera los Archivos : REGINFO_CV_VENTAS_CBTE.TXT  y  REGINFO_CV_VENTAS_ALICUOT" & _
            "AS.TXT"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Location = New System.Drawing.Point(87, 182)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(579, 43)
        Me.Panel6.TabIndex = 167
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(4, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(553, 30)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Procesa los Comprobantes: Facturas Clientes, Nota de Creditos,Recibos Debito/Crei" & _
            "dito Clientes, N.V.L.P., Liquidaciones.  NO PROCESA OPERACIONES DE IMPORTACION."
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(398, 556)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(63, 16)
        Me.Label10.TabIndex = 185
        Me.Label10.Text = "Label10"
        '
        'ListaSICORE
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(1006, 592)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "ListaSICORE"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "CITI Ventas/Compras"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptarCompras As System.Windows.Forms.Button
    Friend WithEvents ButtonEXCELComprasComprobantes As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ButtonCITIVentas As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ButtonEXCELComprasComprobantesVentas As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
End Class
