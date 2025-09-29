<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnArticulo
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextCantidadPrimarios = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.ComboSecundario = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboEnvase = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboCategoria = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboMarca = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.TextNombre = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextIva = New System.Windows.Forms.TextBox
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PictureCuentaContable = New System.Windows.Forms.PictureBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextCuentaContable = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.ButtonDeshabilitar = New System.Windows.Forms.Button
        Me.ButtonActiva = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextInterno = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextEAN = New System.Windows.Forms.TextBox
        Me.CheckExento = New System.Windows.Forms.CheckBox
        Me.CheckNoGrabado = New System.Windows.Forms.CheckBox
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureCuentaContable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGreen
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextCantidadPrimarios)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.ComboSecundario)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.ComboEnvase)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboCategoria)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboMarca)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboVariedad)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboEspecie)
        Me.Panel1.Location = New System.Drawing.Point(35, 179)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(701, 214)
        Me.Panel1.TabIndex = 20
        '
        'TextCantidadPrimarios
        '
        Me.TextCantidadPrimarios.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCantidadPrimarios.Location = New System.Drawing.Point(359, 179)
        Me.TextCantidadPrimarios.MaxLength = 7
        Me.TextCantidadPrimarios.Name = "TextCantidadPrimarios"
        Me.TextCantidadPrimarios.Size = New System.Drawing.Size(73, 22)
        Me.TextCantidadPrimarios.TabIndex = 46
        Me.TextCantidadPrimarios.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(9, 180)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(340, 19)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Cantidad Envases Primarios que contine el Secundario"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(4, 149)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(140, 19)
        Me.Label7.TabIndex = 21
        Me.Label7.Text = "Envase Secundario"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboSecundario
        '
        Me.ComboSecundario.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSecundario.FormattingEnabled = True
        Me.ComboSecundario.Location = New System.Drawing.Point(148, 144)
        Me.ComboSecundario.Name = "ComboSecundario"
        Me.ComboSecundario.Size = New System.Drawing.Size(361, 24)
        Me.ComboSecundario.TabIndex = 40
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(358, 59)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(145, 19)
        Me.Label5.TabIndex = 19
        Me.Label5.Text = "Envase"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEnvase
        '
        Me.ComboEnvase.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEnvase.FormattingEnabled = True
        Me.ComboEnvase.Location = New System.Drawing.Point(281, 82)
        Me.ComboEnvase.Name = "ComboEnvase"
        Me.ComboEnvase.Size = New System.Drawing.Size(294, 24)
        Me.ComboEnvase.TabIndex = 37
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(91, 58)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(161, 19)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Categoria"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCategoria
        '
        Me.ComboCategoria.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCategoria.FormattingEnabled = True
        Me.ComboCategoria.Location = New System.Drawing.Point(85, 81)
        Me.ComboCategoria.Name = "ComboCategoria"
        Me.ComboCategoria.Size = New System.Drawing.Size(173, 24)
        Me.ComboCategoria.TabIndex = 33
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(438, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(167, 19)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Marca"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboMarca
        '
        Me.ComboMarca.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMarca.FormattingEnabled = True
        Me.ComboMarca.Location = New System.Drawing.Point(438, 23)
        Me.ComboMarca.Name = "ComboMarca"
        Me.ComboMarca.Size = New System.Drawing.Size(173, 24)
        Me.ComboMarca.TabIndex = 30
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(232, 1)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(177, 19)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Variedad"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(232, 25)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(179, 24)
        Me.ComboVariedad.TabIndex = 27
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(19, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(183, 19)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Especie"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(16, 26)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(187, 24)
        Me.ComboEspecie.TabIndex = 24
        '
        'TextNombre
        '
        Me.TextNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombre.Location = New System.Drawing.Point(153, 16)
        Me.TextNombre.MaxLength = 30
        Me.TextNombre.Name = "TextNombre"
        Me.TextNombre.Size = New System.Drawing.Size(363, 22)
        Me.TextNombre.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(37, 17)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(65, 18)
        Me.Label6.TabIndex = 12
        Me.Label6.Text = "Articulo"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(34, 79)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 19)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "% IVA. "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextIva
        '
        Me.TextIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextIva.Location = New System.Drawing.Point(153, 76)
        Me.TextIva.MaxLength = 7
        Me.TextIva.Name = "TextIva"
        Me.TextIva.Size = New System.Drawing.Size(73, 22)
        Me.TextIva.TabIndex = 7
        Me.TextIva.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Image = Global.ScomerV01.My.Resources.Resources.Basurero
        Me.ButtonEliminar.Location = New System.Drawing.Point(32, 444)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(135, 29)
        Me.ButtonEliminar.TabIndex = 200
        Me.ButtonEliminar.Text = "Borra Articulo"
        Me.ButtonEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonEliminar.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(597, 444)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(136, 29)
        Me.ButtonAceptar.TabIndex = 50
        Me.ButtonAceptar.Text = "Graba Cambios"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.PictureCuentaContable)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.TextCuentaContable)
        Me.Panel2.Location = New System.Drawing.Point(469, 48)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(264, 40)
        Me.Panel2.TabIndex = 158
        '
        'PictureCuentaContable
        '
        Me.PictureCuentaContable.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureCuentaContable.InitialImage = Nothing
        Me.PictureCuentaContable.Location = New System.Drawing.Point(220, 5)
        Me.PictureCuentaContable.Name = "PictureCuentaContable"
        Me.PictureCuentaContable.Size = New System.Drawing.Size(32, 31)
        Me.PictureCuentaContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCuentaContable.TabIndex = 275
        Me.PictureCuentaContable.TabStop = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(4, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 13)
        Me.Label12.TabIndex = 274
        Me.Label12.Text = "Cuenta "
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuentaContable
        '
        Me.TextCuentaContable.BackColor = System.Drawing.Color.White
        Me.TextCuentaContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuentaContable.Location = New System.Drawing.Point(61, 9)
        Me.TextCuentaContable.MaxLength = 10
        Me.TextCuentaContable.Name = "TextCuentaContable"
        Me.TextCuentaContable.ReadOnly = True
        Me.TextCuentaContable.Size = New System.Drawing.Size(143, 20)
        Me.TextCuentaContable.TabIndex = 273
        Me.TextCuentaContable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold)
        Me.Label10.Location = New System.Drawing.Point(522, 17)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(61, 18)
        Me.Label10.TabIndex = 162
        Me.Label10.Text = "Estado"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(589, 16)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(144, 21)
        Me.ComboEstado.TabIndex = 161
        Me.ComboEstado.TabStop = False
        '
        'ButtonDeshabilitar
        '
        Me.ButtonDeshabilitar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonDeshabilitar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDeshabilitar.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonDeshabilitar.Location = New System.Drawing.Point(213, 446)
        Me.ButtonDeshabilitar.Name = "ButtonDeshabilitar"
        Me.ButtonDeshabilitar.Size = New System.Drawing.Size(136, 29)
        Me.ButtonDeshabilitar.TabIndex = 60
        Me.ButtonDeshabilitar.Text = "Deshabilitar"
        Me.ButtonDeshabilitar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonDeshabilitar.UseVisualStyleBackColor = True
        '
        'ButtonActiva
        '
        Me.ButtonActiva.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonActiva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonActiva.Image = Global.ScomerV01.My.Resources.Resources.CHECKMRK
        Me.ButtonActiva.Location = New System.Drawing.Point(409, 446)
        Me.ButtonActiva.Name = "ButtonActiva"
        Me.ButtonActiva.Size = New System.Drawing.Size(136, 29)
        Me.ButtonActiva.TabIndex = 55
        Me.ButtonActiva.Text = "Activar"
        Me.ButtonActiva.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonActiva.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(37, 48)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(93, 18)
        Me.Label11.TabIndex = 165
        Me.Label11.Text = "Nro.Interno"
        '
        'TextInterno
        '
        Me.TextInterno.BackColor = System.Drawing.Color.White
        Me.TextInterno.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextInterno.Location = New System.Drawing.Point(153, 47)
        Me.TextInterno.MaxLength = 7
        Me.TextInterno.Name = "TextInterno"
        Me.TextInterno.ReadOnly = True
        Me.TextInterno.Size = New System.Drawing.Size(138, 22)
        Me.TextInterno.TabIndex = 166
        Me.TextInterno.TabStop = False
        Me.TextInterno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(36, 107)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(66, 19)
        Me.Label13.TabIndex = 167
        Me.Label13.Text = "EAN-13"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextEAN
        '
        Me.TextEAN.BackColor = System.Drawing.Color.White
        Me.TextEAN.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEAN.Location = New System.Drawing.Point(153, 107)
        Me.TextEAN.MaxLength = 13
        Me.TextEAN.Name = "TextEAN"
        Me.TextEAN.Size = New System.Drawing.Size(157, 22)
        Me.TextEAN.TabIndex = 9
        '
        'CheckExento
        '
        Me.CheckExento.AutoSize = True
        Me.CheckExento.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckExento.Location = New System.Drawing.Point(327, 107)
        Me.CheckExento.Name = "CheckExento"
        Me.CheckExento.Size = New System.Drawing.Size(74, 20)
        Me.CheckExento.TabIndex = 10
        Me.CheckExento.Text = "Exento"
        Me.CheckExento.UseVisualStyleBackColor = True
        '
        'CheckNoGrabado
        '
        Me.CheckNoGrabado.AutoSize = True
        Me.CheckNoGrabado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckNoGrabado.Location = New System.Drawing.Point(421, 108)
        Me.CheckNoGrabado.Name = "CheckNoGrabado"
        Me.CheckNoGrabado.Size = New System.Drawing.Size(112, 20)
        Me.CheckNoGrabado.TabIndex = 13
        Me.CheckNoGrabado.Text = "No Grabado"
        Me.CheckNoGrabado.UseVisualStyleBackColor = True
        '
        'UnArticulo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(785, 503)
        Me.Controls.Add(Me.CheckNoGrabado)
        Me.Controls.Add(Me.CheckExento)
        Me.Controls.Add(Me.TextEAN)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.TextInterno)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.ButtonActiva)
        Me.Controls.Add(Me.ButtonDeshabilitar)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextIva)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextNombre)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UnArticulo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Articulo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureCuentaContable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboEnvase As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboCategoria As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboMarca As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextNombre As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents TextIva As System.Windows.Forms.TextBox
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ComboSecundario As System.Windows.Forms.ComboBox
    Friend WithEvents TextCantidadPrimarios As System.Windows.Forms.TextBox
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PictureCuentaContable As System.Windows.Forms.PictureBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextCuentaContable As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonDeshabilitar As System.Windows.Forms.Button
    Friend WithEvents ButtonActiva As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextInterno As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextEAN As System.Windows.Forms.TextBox
    Friend WithEvents CheckExento As System.Windows.Forms.CheckBox
    Friend WithEvents CheckNoGrabado As System.Windows.Forms.CheckBox
End Class
