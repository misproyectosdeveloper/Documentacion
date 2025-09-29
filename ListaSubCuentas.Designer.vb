<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaSubCuentas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaSubCuentas))
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.ButtonNuevaSubCuenta = New System.Windows.Forms.Button
        Me.ButtonNuevaCuenta = New System.Windows.Forms.Button
        Me.PanelCuenta = New System.Windows.Forms.Panel
        Me.ButtonBorrar = New System.Windows.Forms.Button
        Me.ButtonGraba = New System.Windows.Forms.Button
        Me.PanelSubCuenta = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.TextNombreSubCuenta = New System.Windows.Forms.TextBox
        Me.TextSubCuenta = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextNombreCuenta = New System.Windows.Forms.TextBox
        Me.TextCuenta = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel5.SuspendLayout()
        Me.PanelCuenta.SuspendLayout()
        Me.PanelSubCuenta.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.BackColor = System.Drawing.Color.White
        Me.TreeView1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView1.ForeColor = System.Drawing.Color.OliveDrab
        Me.TreeView1.FullRowSelect = True
        Me.TreeView1.Indent = 60
        Me.TreeView1.Location = New System.Drawing.Point(7, 6)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(519, 664)
        Me.TreeView1.TabIndex = 173
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.ButtonNuevaSubCuenta)
        Me.Panel5.Controls.Add(Me.ButtonNuevaCuenta)
        Me.Panel5.Controls.Add(Me.PanelCuenta)
        Me.Panel5.Location = New System.Drawing.Point(534, 9)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(432, 348)
        Me.Panel5.TabIndex = 174
        '
        'ButtonNuevaSubCuenta
        '
        Me.ButtonNuevaSubCuenta.FlatAppearance.BorderSize = 0
        Me.ButtonNuevaSubCuenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevaSubCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaSubCuenta.Image = CType(resources.GetObject("ButtonNuevaSubCuenta.Image"), System.Drawing.Image)
        Me.ButtonNuevaSubCuenta.Location = New System.Drawing.Point(220, 6)
        Me.ButtonNuevaSubCuenta.Name = "ButtonNuevaSubCuenta"
        Me.ButtonNuevaSubCuenta.Size = New System.Drawing.Size(200, 20)
        Me.ButtonNuevaSubCuenta.TabIndex = 178
        Me.ButtonNuevaSubCuenta.Text = "Nueva Sub-Cuenta"
        Me.ButtonNuevaSubCuenta.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaSubCuenta.UseVisualStyleBackColor = True
        '
        'ButtonNuevaCuenta
        '
        Me.ButtonNuevaCuenta.FlatAppearance.BorderSize = 0
        Me.ButtonNuevaCuenta.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevaCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaCuenta.Image = CType(resources.GetObject("ButtonNuevaCuenta.Image"), System.Drawing.Image)
        Me.ButtonNuevaCuenta.Location = New System.Drawing.Point(6, 6)
        Me.ButtonNuevaCuenta.Name = "ButtonNuevaCuenta"
        Me.ButtonNuevaCuenta.Size = New System.Drawing.Size(200, 20)
        Me.ButtonNuevaCuenta.TabIndex = 177
        Me.ButtonNuevaCuenta.Text = "Nueva Cuenta"
        Me.ButtonNuevaCuenta.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaCuenta.UseVisualStyleBackColor = True
        '
        'PanelCuenta
        '
        Me.PanelCuenta.Controls.Add(Me.ButtonBorrar)
        Me.PanelCuenta.Controls.Add(Me.ButtonGraba)
        Me.PanelCuenta.Controls.Add(Me.PanelSubCuenta)
        Me.PanelCuenta.Controls.Add(Me.Label6)
        Me.PanelCuenta.Controls.Add(Me.Label1)
        Me.PanelCuenta.Controls.Add(Me.TextNombreCuenta)
        Me.PanelCuenta.Controls.Add(Me.TextCuenta)
        Me.PanelCuenta.Controls.Add(Me.Label3)
        Me.PanelCuenta.Location = New System.Drawing.Point(18, 33)
        Me.PanelCuenta.Name = "PanelCuenta"
        Me.PanelCuenta.Size = New System.Drawing.Size(409, 296)
        Me.PanelCuenta.TabIndex = 0
        '
        'ButtonBorrar
        '
        Me.ButtonBorrar.FlatAppearance.BorderSize = 0
        Me.ButtonBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBorrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBorrar.Image = Global.ScomerV01.My.Resources.Resources.Basurero
        Me.ButtonBorrar.Location = New System.Drawing.Point(32, 255)
        Me.ButtonBorrar.Name = "ButtonBorrar"
        Me.ButtonBorrar.Size = New System.Drawing.Size(136, 29)
        Me.ButtonBorrar.TabIndex = 257
        Me.ButtonBorrar.Text = "Borra Cuenta"
        Me.ButtonBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBorrar.UseVisualStyleBackColor = True
        '
        'ButtonGraba
        '
        Me.ButtonGraba.FlatAppearance.BorderSize = 0
        Me.ButtonGraba.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonGraba.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonGraba.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonGraba.Location = New System.Drawing.Point(234, 254)
        Me.ButtonGraba.Name = "ButtonGraba"
        Me.ButtonGraba.Size = New System.Drawing.Size(157, 29)
        Me.ButtonGraba.TabIndex = 185
        Me.ButtonGraba.Text = "Graba Cambios"
        Me.ButtonGraba.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonGraba.UseVisualStyleBackColor = True
        '
        'PanelSubCuenta
        '
        Me.PanelSubCuenta.Controls.Add(Me.Label5)
        Me.PanelSubCuenta.Controls.Add(Me.TextComentario)
        Me.PanelSubCuenta.Controls.Add(Me.TextNombreSubCuenta)
        Me.PanelSubCuenta.Controls.Add(Me.TextSubCuenta)
        Me.PanelSubCuenta.Controls.Add(Me.Label4)
        Me.PanelSubCuenta.Location = New System.Drawing.Point(29, 65)
        Me.PanelSubCuenta.Name = "PanelSubCuenta"
        Me.PanelSubCuenta.Size = New System.Drawing.Size(350, 110)
        Me.PanelSubCuenta.TabIndex = 184
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(9, 45)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(60, 13)
        Me.Label5.TabIndex = 178
        Me.Label5.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(87, 42)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(234, 20)
        Me.TextComentario.TabIndex = 177
        '
        'TextNombreSubCuenta
        '
        Me.TextNombreSubCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreSubCuenta.Location = New System.Drawing.Point(168, 7)
        Me.TextNombreSubCuenta.MaxLength = 20
        Me.TextNombreSubCuenta.Name = "TextNombreSubCuenta"
        Me.TextNombreSubCuenta.Size = New System.Drawing.Size(153, 20)
        Me.TextNombreSubCuenta.TabIndex = 10
        '
        'TextSubCuenta
        '
        Me.TextSubCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSubCuenta.Location = New System.Drawing.Point(85, 7)
        Me.TextSubCuenta.MaxLength = 2
        Me.TextSubCuenta.Name = "TextSubCuenta"
        Me.TextSubCuenta.Size = New System.Drawing.Size(74, 20)
        Me.TextSubCuenta.TabIndex = 9
        Me.TextSubCuenta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(7, 10)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Sub-Cuenta"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(127, 14)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 183
        Me.Label6.Text = "Codigo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(235, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 182
        Me.Label1.Text = "Nombre"
        '
        'TextNombreCuenta
        '
        Me.TextNombreCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreCuenta.Location = New System.Drawing.Point(196, 36)
        Me.TextNombreCuenta.MaxLength = 20
        Me.TextNombreCuenta.Name = "TextNombreCuenta"
        Me.TextNombreCuenta.Size = New System.Drawing.Size(153, 20)
        Me.TextNombreCuenta.TabIndex = 178
        '
        'TextCuenta
        '
        Me.TextCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuenta.Location = New System.Drawing.Point(113, 35)
        Me.TextCuenta.MaxLength = 6
        Me.TextCuenta.Name = "TextCuenta"
        Me.TextCuenta.Size = New System.Drawing.Size(74, 20)
        Me.TextCuenta.TabIndex = 177
        Me.TextCuenta.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(34, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 179
        Me.Label3.Text = "Cuenta"
        '
        'ListaSubCuentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(978, 682)
        Me.Controls.Add(Me.TreeView1)
        Me.Controls.Add(Me.Panel5)
        Me.Name = "ListaSubCuentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cuentas Y Sub-Cuentas"
        Me.Panel5.ResumeLayout(False)
        Me.PanelCuenta.ResumeLayout(False)
        Me.PanelCuenta.PerformLayout()
        Me.PanelSubCuenta.ResumeLayout(False)
        Me.PanelSubCuenta.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents PanelCuenta As System.Windows.Forms.Panel
    Friend WithEvents PanelSubCuenta As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents TextNombreSubCuenta As System.Windows.Forms.TextBox
    Friend WithEvents TextSubCuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextNombreCuenta As System.Windows.Forms.TextBox
    Friend WithEvents TextCuenta As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevaCuenta As System.Windows.Forms.Button
    Friend WithEvents ButtonGraba As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevaSubCuenta As System.Windows.Forms.Button
    Friend WithEvents ButtonBorrar As System.Windows.Forms.Button
End Class
