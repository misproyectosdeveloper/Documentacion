<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionDocumentosNoImputados
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
        Me.RadioPendientes = New System.Windows.Forms.RadioButton
        Me.RadioFacturados = New System.Windows.Forms.RadioButton
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.RadioPendientes)
        Me.Panel2.Controls.Add(Me.RadioFacturados)
        Me.Panel2.Location = New System.Drawing.Point(159, 47)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(392, 71)
        Me.Panel2.TabIndex = 206
        '
        'RadioPendientes
        '
        Me.RadioPendientes.AutoSize = True
        Me.RadioPendientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPendientes.Location = New System.Drawing.Point(246, 25)
        Me.RadioPendientes.Name = "RadioPendientes"
        Me.RadioPendientes.Size = New System.Drawing.Size(88, 17)
        Me.RadioPendientes.TabIndex = 206
        Me.RadioPendientes.TabStop = True
        Me.RadioPendientes.Text = "Pendientes"
        Me.RadioPendientes.UseVisualStyleBackColor = True
        '
        'RadioFacturados
        '
        Me.RadioFacturados.AutoSize = True
        Me.RadioFacturados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFacturados.Location = New System.Drawing.Point(35, 25)
        Me.RadioFacturados.Name = "RadioFacturados"
        Me.RadioFacturados.Size = New System.Drawing.Size(155, 17)
        Me.RadioFacturados.TabIndex = 205
        Me.RadioFacturados.TabStop = True
        Me.RadioFacturados.Text = "Facturados/Liquidados"
        Me.RadioFacturados.UseVisualStyleBackColor = True
        '
        'OpcionDocumentosNoImputados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(710, 489)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "OpcionDocumentosNoImputados"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioPendientes As System.Windows.Forms.RadioButton
    Friend WithEvents RadioFacturados As System.Windows.Forms.RadioButton
End Class
