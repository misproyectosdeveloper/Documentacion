<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionFondoFijo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionFondoFijo))
        Me.ComboFondoFijo = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ComboFondoFijo
        '
        Me.ComboFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboFondoFijo.FormattingEnabled = True
        Me.ComboFondoFijo.Location = New System.Drawing.Point(316, 33)
        Me.ComboFondoFijo.Name = "ComboFondoFijo"
        Me.ComboFondoFijo.Size = New System.Drawing.Size(219, 28)
        Me.ComboFondoFijo.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(116, 36)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(180, 20)
        Me.LabelEmisor.TabIndex = 197
        Me.LabelEmisor.Text = "Proveedor Fondo Fijo"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(263, 347)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 4
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(316, 180)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 209
        Me.PictureCandado.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'TextNumero
        '
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(315, 91)
        Me.TextNumero.MaxLength = 7
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(102, 26)
        Me.TextNumero.TabIndex = 210
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(116, 95)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(161, 20)
        Me.Label1.TabIndex = 211
        Me.Label1.Text = "Numero Fondo Fijo"
        '
        'OpcionFondoFijo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(651, 426)
        Me.Controls.Add(Me.TextNumero)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboFondoFijo)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Name = "OpcionFondoFijo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboFondoFijo As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
