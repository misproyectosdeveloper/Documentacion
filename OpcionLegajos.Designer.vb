<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionLegajos
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionLegajos))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PanelB = New System.Windows.Forms.Panel
        Me.ComboLegajos = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ComboNombre = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PanelB.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(219, 368)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 10
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PanelB
        '
        Me.PanelB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelB.Controls.Add(Me.ComboLegajos)
        Me.PanelB.Controls.Add(Me.LabelEmisor)
        Me.PanelB.Location = New System.Drawing.Point(95, 16)
        Me.PanelB.Name = "PanelB"
        Me.PanelB.Size = New System.Drawing.Size(418, 54)
        Me.PanelB.TabIndex = 2
        '
        'ComboLegajos
        '
        Me.ComboLegajos.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboLegajos.FormattingEnabled = True
        Me.ComboLegajos.Location = New System.Drawing.Point(94, 14)
        Me.ComboLegajos.Name = "ComboLegajos"
        Me.ComboLegajos.Size = New System.Drawing.Size(276, 24)
        Me.ComboLegajos.TabIndex = 3
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(38, 20)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(45, 13)
        Me.LabelEmisor.TabIndex = 147
        Me.LabelEmisor.Text = "Legajo"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
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
        Me.PictureCandado.Image = Global.ScomerV01.My.Resources.Resources.CAbierto
        Me.PictureCandado.Location = New System.Drawing.Point(272, 279)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 1012
        Me.PictureCandado.TabStop = False
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.ComboNombre)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Location = New System.Drawing.Point(95, 75)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(418, 54)
        Me.Panel2.TabIndex = 1013
        '
        'ComboNombre
        '
        Me.ComboNombre.Font = New System.Drawing.Font("Courier New", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNombre.FormattingEnabled = True
        Me.ComboNombre.Location = New System.Drawing.Point(94, 14)
        Me.ComboNombre.Name = "ComboNombre"
        Me.ComboNombre.Size = New System.Drawing.Size(276, 24)
        Me.ComboNombre.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(38, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 147
        Me.Label4.Text = "Nombre"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'OpcionLegajos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(582, 410)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.PanelB)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.KeyPreview = True
        Me.Name = "OpcionLegajos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Legajos"
        Me.PanelB.ResumeLayout(False)
        Me.PanelB.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PanelB As System.Windows.Forms.Panel
    Friend WithEvents ComboLegajos As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ComboNombre As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
