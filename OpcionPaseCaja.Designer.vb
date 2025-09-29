<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionPaseCaja
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionPaseCaja))
        Me.ComboCaja = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboCaja
        '
        Me.ComboCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCaja.FormattingEnabled = True
        Me.ComboCaja.Location = New System.Drawing.Point(328, 59)
        Me.ComboCaja.Name = "ComboCaja"
        Me.ComboCaja.Size = New System.Drawing.Size(95, 21)
        Me.ComboCaja.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(195, 60)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(105, 18)
        Me.LabelEmisor.TabIndex = 146
        Me.LabelEmisor.Text = "Caja Destino"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(250, 283)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 3
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(303, 151)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 147
        Me.PictureCandado.TabStop = False
        '
        'OpcionPaseCaja
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(629, 335)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ComboCaja)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionPaseCaja"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pase de Caja"
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ComboCaja As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
