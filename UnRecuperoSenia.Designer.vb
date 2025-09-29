<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnRecuperoSenia
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnRecuperoSenia))
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.TextNota = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextCantidad = New System.Windows.Forms.TextBox
        Me.PictureAlmanaqueContable = New System.Windows.Forms.PictureBox
        Me.TextFechaVale = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextVale = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.CheckUsado = New System.Windows.Forms.CheckBox
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonBaja = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonNuevaIgualProveedor = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextCaja = New System.Windows.Forms.TextBox
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(518, 16)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 238
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(567, 13)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 236
        Me.DateTime1.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(685, 16)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 242
        Me.Label8.Text = "Estado"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboProveedor
        '
        Me.ComboProveedor.BackColor = System.Drawing.Color.White
        Me.ComboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboProveedor.Enabled = False
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(289, 12)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(222, 21)
        Me.ComboProveedor.TabIndex = 239
        Me.ComboProveedor.TabStop = False
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.White
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(740, 11)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 241
        Me.ComboEstado.TabStop = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(214, 16)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(65, 13)
        Me.Label16.TabIndex = 237
        Me.Label16.Text = "Proveedor"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(784, 44)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(51, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 240
        Me.PictureCandado.TabStop = False
        '
        'TextNota
        '
        Me.TextNota.BackColor = System.Drawing.Color.White
        Me.TextNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNota.Location = New System.Drawing.Point(93, 12)
        Me.TextNota.Name = "TextNota"
        Me.TextNota.ReadOnly = True
        Me.TextNota.Size = New System.Drawing.Size(112, 20)
        Me.TextNota.TabIndex = 243
        Me.TextNota.TabStop = False
        Me.TextNota.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(34, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 244
        Me.Label1.Text = "Recibo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.TextCantidad)
        Me.Panel1.Controls.Add(Me.PictureAlmanaqueContable)
        Me.Panel1.Controls.Add(Me.TextFechaVale)
        Me.Panel1.Controls.Add(Me.Label24)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TextVale)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.TextImporte)
        Me.Panel1.Location = New System.Drawing.Point(33, 111)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(806, 83)
        Me.Panel1.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(387, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(109, 13)
        Me.Label4.TabIndex = 1017
        Me.Label4.Text = "Cantidad Envases"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCantidad
        '
        Me.TextCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCantidad.Location = New System.Drawing.Point(506, 31)
        Me.TextCantidad.MaxLength = 8
        Me.TextCantidad.Name = "TextCantidad"
        Me.TextCantidad.Size = New System.Drawing.Size(101, 20)
        Me.TextCantidad.TabIndex = 4
        Me.TextCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PictureAlmanaqueContable
        '
        Me.PictureAlmanaqueContable.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueContable.Location = New System.Drawing.Point(353, 29)
        Me.PictureAlmanaqueContable.Name = "PictureAlmanaqueContable"
        Me.PictureAlmanaqueContable.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueContable.TabIndex = 1015
        Me.PictureAlmanaqueContable.TabStop = False
        '
        'TextFechaVale
        '
        Me.TextFechaVale.BackColor = System.Drawing.Color.White
        Me.TextFechaVale.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaVale.Location = New System.Drawing.Point(265, 30)
        Me.TextFechaVale.MaxLength = 10
        Me.TextFechaVale.Name = "TextFechaVale"
        Me.TextFechaVale.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaVale.TabIndex = 2
        Me.TextFechaVale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(182, 34)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(71, 13)
        Me.Label24.TabIndex = 1013
        Me.Label24.Text = "Fecha Vale"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(32, 13)
        Me.Label3.TabIndex = 248
        Me.Label3.Text = "Vale"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextVale
        '
        Me.TextVale.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextVale.Location = New System.Drawing.Point(52, 30)
        Me.TextVale.MaxLength = 8
        Me.TextVale.Name = "TextVale"
        Me.TextVale.Size = New System.Drawing.Size(119, 20)
        Me.TextVale.TabIndex = 1
        Me.TextVale.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(613, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(49, 13)
        Me.Label2.TabIndex = 246
        Me.Label2.Text = "Importe"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImporte
        '
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(667, 31)
        Me.TextImporte.MaxLength = 8
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.Size = New System.Drawing.Size(100, 20)
        Me.TextImporte.TabIndex = 5
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CheckUsado
        '
        Me.CheckUsado.AutoCheck = False
        Me.CheckUsado.AutoSize = True
        Me.CheckUsado.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckUsado.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckUsado.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckUsado.Location = New System.Drawing.Point(340, 71)
        Me.CheckUsado.Name = "CheckUsado"
        Me.CheckUsado.Size = New System.Drawing.Size(203, 19)
        Me.CheckUsado.TabIndex = 1009
        Me.CheckUsado.TabStop = False
        Me.CheckUsado.Text = "Utilizado en Orden de Pago"
        Me.CheckUsado.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.BackColor = System.Drawing.Color.Linen
        Me.ButtonNuevo.FlatAppearance.BorderSize = 0
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(23, 349)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(133, 29)
        Me.ButtonNuevo.TabIndex = 10
        Me.ButtonNuevo.Text = "Nuevo Recibo"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = False
        '
        'ButtonBaja
        '
        Me.ButtonBaja.BackColor = System.Drawing.Color.Linen
        Me.ButtonBaja.FlatAppearance.BorderSize = 0
        Me.ButtonBaja.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBaja.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonBaja.Location = New System.Drawing.Point(537, 347)
        Me.ButtonBaja.Name = "ButtonBaja"
        Me.ButtonBaja.Size = New System.Drawing.Size(141, 29)
        Me.ButtonBaja.TabIndex = 17
        Me.ButtonBaja.Text = "Baja Recibo"
        Me.ButtonBaja.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBaja.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Linen
        Me.ButtonAceptar.FlatAppearance.BorderSize = 0
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(689, 347)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(136, 29)
        Me.ButtonAceptar.TabIndex = 20
        Me.ButtonAceptar.Text = "Graba Cambios"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.BackColor = System.Drawing.Color.Linen
        Me.ButtonAsientoContable.FlatAppearance.BorderSize = 0
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(371, 348)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 29)
        Me.ButtonAsientoContable.TabIndex = 15
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = False
        '
        'ButtonNuevaIgualProveedor
        '
        Me.ButtonNuevaIgualProveedor.BackColor = System.Drawing.Color.Linen
        Me.ButtonNuevaIgualProveedor.FlatAppearance.BorderSize = 0
        Me.ButtonNuevaIgualProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonNuevaIgualProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaIgualProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevaIgualProveedor.Location = New System.Drawing.Point(170, 348)
        Me.ButtonNuevaIgualProveedor.Name = "ButtonNuevaIgualProveedor"
        Me.ButtonNuevaIgualProveedor.Size = New System.Drawing.Size(182, 30)
        Me.ButtonNuevaIgualProveedor.TabIndex = 12
        Me.ButtonNuevaIgualProveedor.Text = "Nuevo Rec. Igual Proveedor"
        Me.ButtonNuevaIgualProveedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaIgualProveedor.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(34, 46)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(32, 13)
        Me.Label5.TabIndex = 1011
        Me.Label5.Text = "Caja"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCaja
        '
        Me.TextCaja.BackColor = System.Drawing.Color.White
        Me.TextCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCaja.Location = New System.Drawing.Point(93, 43)
        Me.TextCaja.Name = "TextCaja"
        Me.TextCaja.ReadOnly = True
        Me.TextCaja.Size = New System.Drawing.Size(38, 20)
        Me.TextCaja.TabIndex = 1010
        Me.TextCaja.TabStop = False
        Me.TextCaja.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'UnRecuperoSenia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(862, 425)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextCaja)
        Me.Controls.Add(Me.ButtonNuevaIgualProveedor)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonBaja)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.CheckUsado)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextNota)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ComboProveedor)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.Label16)
        Me.Name = "UnRecuperoSenia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Recupero de Senia Proveedor"
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextNota As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextVale As System.Windows.Forms.TextBox
    Friend WithEvents PictureAlmanaqueContable As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaVale As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextCantidad As System.Windows.Forms.TextBox
    Friend WithEvents CheckUsado As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonBaja As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevaIgualProveedor As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextCaja As System.Windows.Forms.TextBox
End Class
