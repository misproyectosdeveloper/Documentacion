<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnSaldoInicial
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
        Me.Label10 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextImputableB = New System.Windows.Forms.TextBox
        Me.Label42 = New System.Windows.Forms.Label
        Me.TextCambio = New System.Windows.Forms.TextBox
        Me.PictureCandadoB = New System.Windows.Forms.PictureBox
        Me.Label29 = New System.Windows.Forms.Label
        Me.TextSaldoInicialB = New System.Windows.Forms.TextBox
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextImputableN = New System.Windows.Forms.TextBox
        Me.PictureBox4 = New System.Windows.Forms.PictureBox
        Me.Label35 = New System.Windows.Forms.Label
        Me.TextSaldoInicialN = New System.Windows.Forms.TextBox
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label31 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        CType(Me.PictureCandadoB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel7.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(473, 340)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(125, 29)
        Me.ButtonAceptar.TabIndex = 10
        Me.ButtonAceptar.Text = "Graba Cambios "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.TextImputableB)
        Me.Panel1.Controls.Add(Me.Label42)
        Me.Panel1.Controls.Add(Me.TextCambio)
        Me.Panel1.Controls.Add(Me.PictureCandadoB)
        Me.Panel1.Controls.Add(Me.Label29)
        Me.Panel1.Controls.Add(Me.TextSaldoInicialB)
        Me.Panel1.Controls.Add(Me.Panel7)
        Me.Panel1.Location = New System.Drawing.Point(13, 61)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(585, 202)
        Me.Panel1.TabIndex = 632
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(376, 23)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 640
        Me.Label10.Text = "Fecha "
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(425, 19)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 639
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(351, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 13)
        Me.Label1.TabIndex = 638
        Me.Label1.Text = "Por Imputar"
        '
        'TextImputableB
        '
        Me.TextImputableB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImputableB.Location = New System.Drawing.Point(431, 62)
        Me.TextImputableB.MaxLength = 10
        Me.TextImputableB.Name = "TextImputableB"
        Me.TextImputableB.ReadOnly = True
        Me.TextImputableB.Size = New System.Drawing.Size(104, 20)
        Me.TextImputableB.TabIndex = 637
        Me.TextImputableB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label42.Location = New System.Drawing.Point(20, 25)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(48, 13)
        Me.Label42.TabIndex = 636
        Me.Label42.Text = "Cambio"
        '
        'TextCambio
        '
        Me.TextCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCambio.Location = New System.Drawing.Point(73, 22)
        Me.TextCambio.MaxLength = 10
        Me.TextCambio.Name = "TextCambio"
        Me.TextCambio.Size = New System.Drawing.Size(58, 20)
        Me.TextCambio.TabIndex = 3
        Me.TextCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PictureCandadoB
        '
        Me.PictureCandadoB.Image = Global.ScomerV01.My.Resources.Resources.CAbierto
        Me.PictureCandadoB.InitialImage = Global.ScomerV01.My.Resources.Resources.Ccerrado
        Me.PictureCandadoB.Location = New System.Drawing.Point(302, 53)
        Me.PictureCandadoB.Name = "PictureCandadoB"
        Me.PictureCandadoB.Size = New System.Drawing.Size(30, 30)
        Me.PictureCandadoB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandadoB.TabIndex = 634
        Me.PictureCandadoB.TabStop = False
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(21, 64)
        Me.Label29.Name = "Label29"
        Me.Label29.Size = New System.Drawing.Size(127, 13)
        Me.Label29.TabIndex = 633
        Me.Label29.Text = "Saldo Inicial Cta.Cte."
        '
        'TextSaldoInicialB
        '
        Me.TextSaldoInicialB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldoInicialB.Location = New System.Drawing.Point(176, 61)
        Me.TextSaldoInicialB.MaxLength = 10
        Me.TextSaldoInicialB.Name = "TextSaldoInicialB"
        Me.TextSaldoInicialB.Size = New System.Drawing.Size(112, 20)
        Me.TextSaldoInicialB.TabIndex = 1
        Me.TextSaldoInicialB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.Label2)
        Me.Panel7.Controls.Add(Me.TextImputableN)
        Me.Panel7.Controls.Add(Me.PictureBox4)
        Me.Panel7.Controls.Add(Me.Label35)
        Me.Panel7.Controls.Add(Me.TextSaldoInicialN)
        Me.Panel7.Location = New System.Drawing.Point(16, 108)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(529, 61)
        Me.Panel7.TabIndex = 631
        Me.Panel7.Visible = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(335, 17)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 13)
        Me.Label2.TabIndex = 640
        Me.Label2.Text = "Por Imputar"
        '
        'TextImputableN
        '
        Me.TextImputableN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImputableN.Location = New System.Drawing.Point(415, 14)
        Me.TextImputableN.MaxLength = 10
        Me.TextImputableN.Name = "TextImputableN"
        Me.TextImputableN.ReadOnly = True
        Me.TextImputableN.Size = New System.Drawing.Size(104, 20)
        Me.TextImputableN.TabIndex = 639
        Me.TextImputableN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = Global.ScomerV01.My.Resources.Resources.Ccerrado
        Me.PictureBox4.InitialImage = Nothing
        Me.PictureBox4.Location = New System.Drawing.Point(285, 3)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(30, 30)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 616
        Me.PictureBox4.TabStop = False
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(5, 18)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(127, 13)
        Me.Label35.TabIndex = 615
        Me.Label35.Text = "Saldo Inicial Cta.Cte."
        '
        'TextSaldoInicialN
        '
        Me.TextSaldoInicialN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldoInicialN.Location = New System.Drawing.Point(160, 13)
        Me.TextSaldoInicialN.MaxLength = 10
        Me.TextSaldoInicialN.Name = "TextSaldoInicialN"
        Me.TextSaldoInicialN.Size = New System.Drawing.Size(112, 20)
        Me.TextSaldoInicialN.TabIndex = 4
        Me.TextSaldoInicialN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboMoneda
        '
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(153, 38)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(115, 21)
        Me.ComboMoneda.TabIndex = 634
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(16, 41)
        Me.Label31.Name = "Label31"
        Me.Label31.Size = New System.Drawing.Size(131, 15)
        Me.Label31.TabIndex = 633
        Me.Label31.Text = "Moneda en que Opera"
        '
        'UnSaldoInicial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGray
        Me.ClientSize = New System.Drawing.Size(619, 386)
        Me.Controls.Add(Me.ComboMoneda)
        Me.Controls.Add(Me.Label31)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "UnSaldoInicial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Saldo Inicial"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureCandadoB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label42 As System.Windows.Forms.Label
    Friend WithEvents TextCambio As System.Windows.Forms.TextBox
    Friend WithEvents PictureCandadoB As System.Windows.Forms.PictureBox
    Friend WithEvents Label29 As System.Windows.Forms.Label
    Friend WithEvents TextSaldoInicialB As System.Windows.Forms.TextBox
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents Label35 As System.Windows.Forms.Label
    Friend WithEvents TextSaldoInicialN As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextImputableB As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextImputableN As System.Windows.Forms.TextBox
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label31 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
End Class
