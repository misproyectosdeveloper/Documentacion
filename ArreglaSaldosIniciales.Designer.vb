<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ArreglaSaldosIniciales
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
        Me.Button1 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.Button3 = New System.Windows.Forms.Button
        Me.ButtonArreglaPrecios = New System.Windows.Forms.Button
        Me.ButtonFacturasB = New System.Windows.Forms.Button
        Me.ButtonFacturasN = New System.Windows.Forms.Button
        Me.ButtonRemitosB = New System.Windows.Forms.Button
        Me.ButtonRemitosN = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(235, 215)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(355, 76)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(235, 309)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(355, 76)
        Me.Button2.TabIndex = 2
        Me.Button2.Text = "Cambia Codigos Clientes"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(235, 403)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(355, 76)
        Me.Button3.TabIndex = 3
        Me.Button3.Text = "Arregla Remitos valor para Pradan"
        Me.Button3.UseVisualStyleBackColor = True
        Me.Button3.Visible = False
        '
        'ButtonArreglaPrecios
        '
        Me.ButtonArreglaPrecios.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonArreglaPrecios.Location = New System.Drawing.Point(316, 12)
        Me.ButtonArreglaPrecios.Name = "ButtonArreglaPrecios"
        Me.ButtonArreglaPrecios.Size = New System.Drawing.Size(231, 56)
        Me.ButtonArreglaPrecios.TabIndex = 4
        Me.ButtonArreglaPrecios.Text = "Arregla ""Tipo Precio"""
        Me.ButtonArreglaPrecios.UseVisualStyleBackColor = True
        '
        'ButtonFacturasB
        '
        Me.ButtonFacturasB.Location = New System.Drawing.Point(151, 87)
        Me.ButtonFacturasB.Name = "ButtonFacturasB"
        Me.ButtonFacturasB.Size = New System.Drawing.Size(159, 40)
        Me.ButtonFacturasB.TabIndex = 5
        Me.ButtonFacturasB.Text = "FACTURAS B"
        Me.ButtonFacturasB.UseVisualStyleBackColor = True
        '
        'ButtonFacturasN
        '
        Me.ButtonFacturasN.Location = New System.Drawing.Point(151, 144)
        Me.ButtonFacturasN.Name = "ButtonFacturasN"
        Me.ButtonFacturasN.Size = New System.Drawing.Size(159, 40)
        Me.ButtonFacturasN.TabIndex = 6
        Me.ButtonFacturasN.Text = "FACTURAS N"
        Me.ButtonFacturasN.UseVisualStyleBackColor = True
        '
        'ButtonRemitosB
        '
        Me.ButtonRemitosB.Location = New System.Drawing.Point(356, 87)
        Me.ButtonRemitosB.Name = "ButtonRemitosB"
        Me.ButtonRemitosB.Size = New System.Drawing.Size(159, 40)
        Me.ButtonRemitosB.TabIndex = 7
        Me.ButtonRemitosB.Text = "REMITOS B"
        Me.ButtonRemitosB.UseVisualStyleBackColor = True
        '
        'ButtonRemitosN
        '
        Me.ButtonRemitosN.Location = New System.Drawing.Point(354, 144)
        Me.ButtonRemitosN.Name = "ButtonRemitosN"
        Me.ButtonRemitosN.Size = New System.Drawing.Size(159, 40)
        Me.ButtonRemitosN.TabIndex = 8
        Me.ButtonRemitosN.Text = "REMITOS N"
        Me.ButtonRemitosN.UseVisualStyleBackColor = True
        '
        'ArreglaSaldosIniciales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(833, 491)
        Me.Controls.Add(Me.ButtonRemitosN)
        Me.Controls.Add(Me.ButtonRemitosB)
        Me.Controls.Add(Me.ButtonFacturasN)
        Me.Controls.Add(Me.ButtonFacturasB)
        Me.Controls.Add(Me.ButtonArreglaPrecios)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Name = "ArreglaSaldosIniciales"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ArreglaSaldosIniciales"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ButtonArreglaPrecios As System.Windows.Forms.Button
    Friend WithEvents ButtonFacturasB As System.Windows.Forms.Button
    Friend WithEvents ButtonFacturasN As System.Windows.Forms.Button
    Friend WithEvents ButtonRemitosB As System.Windows.Forms.Button
    Friend WithEvents ButtonRemitosN As System.Windows.Forms.Button
End Class
