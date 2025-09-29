<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionNumero
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
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextAnio = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.CheckTodo = New System.Windows.Forms.CheckBox
        Me.CheckPrecios = New System.Windows.Forms.CheckBox
        Me.CheckCantidad = New System.Windows.Forms.CheckBox
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.MaskedNumero = New System.Windows.Forms.MaskedTextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(229, 376)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 272
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.TextAnio)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(46, 27)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(490, 83)
        Me.Panel1.TabIndex = 273
        '
        'TextAnio
        '
        Me.TextAnio.BackColor = System.Drawing.Color.White
        Me.TextAnio.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAnio.Location = New System.Drawing.Point(205, 50)
        Me.TextAnio.MaxLength = 4
        Me.TextAnio.Name = "TextAnio"
        Me.TextAnio.Size = New System.Drawing.Size(98, 26)
        Me.TextAnio.TabIndex = 273
        Me.TextAnio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(41, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(428, 45)
        Me.Label1.TabIndex = 272
        Me.Label1.Text = "Label1"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.MaskedRemito)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.PictureCandado)
        Me.Panel2.Location = New System.Drawing.Point(51, 125)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(484, 128)
        Me.Panel2.TabIndex = 274
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.CheckTodo)
        Me.Panel3.Controls.Add(Me.CheckPrecios)
        Me.Panel3.Controls.Add(Me.CheckCantidad)
        Me.Panel3.Location = New System.Drawing.Point(10, 75)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(468, 38)
        Me.Panel3.TabIndex = 202
        '
        'CheckTodo
        '
        Me.CheckTodo.AutoSize = True
        Me.CheckTodo.Location = New System.Drawing.Point(398, 10)
        Me.CheckTodo.Name = "CheckTodo"
        Me.CheckTodo.Size = New System.Drawing.Size(51, 17)
        Me.CheckTodo.TabIndex = 204
        Me.CheckTodo.Text = "Todo"
        Me.CheckTodo.UseVisualStyleBackColor = True
        '
        'CheckPrecios
        '
        Me.CheckPrecios.AutoSize = True
        Me.CheckPrecios.Location = New System.Drawing.Point(256, 11)
        Me.CheckPrecios.Name = "CheckPrecios"
        Me.CheckPrecios.Size = New System.Drawing.Size(83, 17)
        Me.CheckPrecios.TabIndex = 203
        Me.CheckPrecios.Text = "Con Precios"
        Me.CheckPrecios.UseVisualStyleBackColor = True
        '
        'CheckCantidad
        '
        Me.CheckCantidad.AutoSize = True
        Me.CheckCantidad.Location = New System.Drawing.Point(20, 10)
        Me.CheckCantidad.Name = "CheckCantidad"
        Me.CheckCantidad.Size = New System.Drawing.Size(190, 17)
        Me.CheckCantidad.TabIndex = 202
        Me.CheckCantidad.Text = "Con Cantidad menos devoluciones"
        Me.CheckCantidad.UseVisualStyleBackColor = True
        '
        'MaskedRemito
        '
        Me.MaskedRemito.BackColor = System.Drawing.Color.White
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(207, 26)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(152, 26)
        Me.MaskedRemito.TabIndex = 198
        Me.MaskedRemito.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(137, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 20)
        Me.Label2.TabIndex = 197
        Me.Label2.Text = "Remito"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(57, 10)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 127
        Me.PictureCandado.TabStop = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.MaskedNumero)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Location = New System.Drawing.Point(50, 266)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(484, 78)
        Me.Panel4.TabIndex = 275
        '
        'MaskedNumero
        '
        Me.MaskedNumero.BackColor = System.Drawing.Color.White
        Me.MaskedNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedNumero.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedNumero.Location = New System.Drawing.Point(232, 26)
        Me.MaskedNumero.Mask = "0000-00000000"
        Me.MaskedNumero.Name = "MaskedNumero"
        Me.MaskedNumero.Size = New System.Drawing.Size(152, 26)
        Me.MaskedNumero.TabIndex = 198
        Me.MaskedNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedNumero.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(39, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(164, 20)
        Me.Label3.TabIndex = 197
        Me.Label3.Text = "Ingrese Nro.Prestamo"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OpcionNumero
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(576, 483)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionNumero"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextAnio As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CheckTodo As System.Windows.Forms.CheckBox
    Friend WithEvents CheckPrecios As System.Windows.Forms.CheckBox
    Friend WithEvents CheckCantidad As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents MaskedNumero As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
