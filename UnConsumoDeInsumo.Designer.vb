<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnConsumoDeInsumo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnConsumoDeInsumo))
        Me.LabelCuentas = New System.Windows.Forms.Label
        Me.Cuenta = New System.Windows.Forms.ColumnHeader
        Me.PictureLupaCuenta = New System.Windows.Forms.PictureBox
        Me.ButtonVerDetalle = New System.Windows.Forms.Button
        Me.ListCuentas = New System.Windows.Forms.ListView
        Me.Importe1 = New System.Windows.Forms.ColumnHeader
        Me.Importe2 = New System.Windows.Forms.ColumnHeader
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.ComboInsumo = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.LabelTitulo = New System.Windows.Forms.Label
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextConsumo = New System.Windows.Forms.TextBox
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.TextCantidad = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label19 = New System.Windows.Forms.Label
        Me.Label17 = New System.Windows.Forms.Label
        Me.TextCostoSinIva = New System.Windows.Forms.TextBox
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.TextCosto = New System.Windows.Forms.TextBox
        Me.ButtonCalculaCosto = New System.Windows.Forms.Button
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PictureLupaLote = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.ButtonVerDetalleLotes = New System.Windows.Forms.Button
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureLupaLote, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LabelCuentas
        '
        Me.LabelCuentas.AutoSize = True
        Me.LabelCuentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCuentas.Location = New System.Drawing.Point(489, 86)
        Me.LabelCuentas.Name = "LabelCuentas"
        Me.LabelCuentas.Size = New System.Drawing.Size(104, 13)
        Me.LabelCuentas.TabIndex = 299
        Me.LabelCuentas.Text = "Imputación Contable"
        Me.LabelCuentas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureLupaCuenta
        '
        Me.PictureLupaCuenta.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupaCuenta.InitialImage = Nothing
        Me.PictureLupaCuenta.Location = New System.Drawing.Point(732, 79)
        Me.PictureLupaCuenta.Name = "PictureLupaCuenta"
        Me.PictureLupaCuenta.Size = New System.Drawing.Size(31, 29)
        Me.PictureLupaCuenta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupaCuenta.TabIndex = 298
        Me.PictureLupaCuenta.TabStop = False
        '
        'ButtonVerDetalle
        '
        Me.ButtonVerDetalle.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonVerDetalle.FlatAppearance.BorderSize = 0
        Me.ButtonVerDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonVerDetalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerDetalle.Location = New System.Drawing.Point(715, 455)
        Me.ButtonVerDetalle.Name = "ButtonVerDetalle"
        Me.ButtonVerDetalle.Size = New System.Drawing.Size(175, 20)
        Me.ButtonVerDetalle.TabIndex = 311
        Me.ButtonVerDetalle.Text = "Detalle Ordenes de Compra"
        Me.ButtonVerDetalle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerDetalle.UseVisualStyleBackColor = False
        '
        'ListCuentas
        '
        Me.ListCuentas.BackColor = System.Drawing.Color.White
        Me.ListCuentas.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Cuenta, Me.Importe1, Me.Importe2})
        Me.ListCuentas.Location = New System.Drawing.Point(598, 79)
        Me.ListCuentas.Name = "ListCuentas"
        Me.ListCuentas.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListCuentas.Size = New System.Drawing.Size(121, 28)
        Me.ListCuentas.TabIndex = 297
        Me.ListCuentas.TileSize = New System.Drawing.Size(90, 15)
        Me.ListCuentas.UseCompatibleStateImageBehavior = False
        Me.ListCuentas.View = System.Windows.Forms.View.Tile
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.LabelCuentas)
        Me.Panel3.Controls.Add(Me.PictureLupaCuenta)
        Me.Panel3.Controls.Add(Me.ListCuentas)
        Me.Panel3.Controls.Add(Me.Label16)
        Me.Panel3.Controls.Add(Me.TextComentario)
        Me.Panel3.Controls.Add(Me.ComboInsumo)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.DateTime1)
        Me.Panel3.Controls.Add(Me.PictureCandado)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Controls.Add(Me.ComboDeposito)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Location = New System.Drawing.Point(82, 121)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(808, 121)
        Me.Panel3.TabIndex = 309
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(22, 58)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(60, 13)
        Me.Label16.TabIndex = 182
        Me.Label16.Text = "Comentario"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(90, 53)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(294, 20)
        Me.TextComentario.TabIndex = 181
        Me.TextComentario.TabStop = False
        '
        'ComboInsumo
        '
        Me.ComboInsumo.BackColor = System.Drawing.Color.White
        Me.ComboInsumo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboInsumo.Enabled = False
        Me.ComboInsumo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboInsumo.FormattingEnabled = True
        Me.ComboInsumo.Location = New System.Drawing.Point(91, 16)
        Me.ComboInsumo.Name = "ComboInsumo"
        Me.ComboInsumo.Size = New System.Drawing.Size(220, 21)
        Me.ComboInsumo.TabIndex = 180
        Me.ComboInsumo.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(337, 19)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(42, 13)
        Me.Label14.TabIndex = 178
        Me.Label14.Text = "Fecha"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(384, 16)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(113, 20)
        Me.DateTime1.TabIndex = 179
        Me.DateTime1.TabStop = False
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(732, 16)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(50, 45)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 177
        Me.PictureCandado.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(24, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 176
        Me.Label1.Text = "Insumo"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(576, 16)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(139, 21)
        Me.ComboDeposito.TabIndex = 174
        Me.ComboDeposito.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(515, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 13)
        Me.Label2.TabIndex = 175
        Me.Label2.Text = "Deposito"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelTitulo
        '
        Me.LabelTitulo.AutoSize = True
        Me.LabelTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTitulo.Location = New System.Drawing.Point(398, 59)
        Me.LabelTitulo.Name = "LabelTitulo"
        Me.LabelTitulo.Size = New System.Drawing.Size(43, 20)
        Me.LabelTitulo.TabIndex = 308
        Me.LabelTitulo.Text = "Tipo"
        Me.LabelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.Location = New System.Drawing.Point(82, 516)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(126, 29)
        Me.ButtonNuevo.TabIndex = 302
        Me.ButtonNuevo.Text = "Nuevo Ingreso"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(742, 516)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(144, 29)
        Me.ButtonAceptar.TabIndex = 301
        Me.ButtonAceptar.Text = "Graba Consumo"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextConsumo
        '
        Me.TextConsumo.BackColor = System.Drawing.Color.White
        Me.TextConsumo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextConsumo.Location = New System.Drawing.Point(173, 93)
        Me.TextConsumo.MaxLength = 8
        Me.TextConsumo.Name = "TextConsumo"
        Me.TextConsumo.ReadOnly = True
        Me.TextConsumo.Size = New System.Drawing.Size(111, 20)
        Me.TextConsumo.TabIndex = 306
        Me.TextConsumo.TabStop = False
        Me.TextConsumo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(503, 516)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(155, 29)
        Me.ButtonAsientoContable.TabIndex = 310
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(293, 516)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(136, 29)
        Me.ButtonAnula.TabIndex = 307
        Me.ButtonAnula.Text = "Anular Ingreso"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'TextCantidad
        '
        Me.TextCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCantidad.Location = New System.Drawing.Point(76, 13)
        Me.TextCantidad.MaxLength = 8
        Me.TextCantidad.Name = "TextCantidad"
        Me.TextCantidad.Size = New System.Drawing.Size(111, 20)
        Me.TextCantidad.TabIndex = 23
        Me.TextCantidad.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(83, 96)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(81, 13)
        Me.Label15.TabIndex = 305
        Me.Label15.Text = "Comprobante"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(14, 16)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(57, 13)
        Me.Label11.TabIndex = 183
        Me.Label11.Text = "Cantidad"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.LightGreen
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ComboNegocio)
        Me.Panel4.Controls.Add(Me.ComboCosteo)
        Me.Panel4.Controls.Add(Me.Label7)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Location = New System.Drawing.Point(192, 26)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(483, 44)
        Me.Panel4.TabIndex = 10
        Me.Panel4.Visible = False
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(73, 11)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(168, 21)
        Me.ComboNegocio.TabIndex = 10
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(312, 11)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(154, 21)
        Me.ComboCosteo.TabIndex = 11
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(258, 15)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 162
        Me.Label7.Text = "Costeo"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(9, 15)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(54, 13)
        Me.Label8.TabIndex = 160
        Me.Label8.Text = "Negocio"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(537, 17)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(62, 13)
        Me.Label19.TabIndex = 181
        Me.Label19.Text = "$ Con Iva"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(340, 17)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(58, 13)
        Me.Label17.TabIndex = 180
        Me.Label17.Text = "$ Sin Iva"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TextCostoSinIva
        '
        Me.TextCostoSinIva.BackColor = System.Drawing.Color.White
        Me.TextCostoSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCostoSinIva.Location = New System.Drawing.Point(405, 13)
        Me.TextCostoSinIva.MaxLength = 8
        Me.TextCostoSinIva.Name = "TextCostoSinIva"
        Me.TextCostoSinIva.ReadOnly = True
        Me.TextCostoSinIva.Size = New System.Drawing.Size(121, 20)
        Me.TextCostoSinIva.TabIndex = 179
        Me.TextCostoSinIva.TabStop = False
        Me.TextCostoSinIva.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.LightGreen
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.TextCantidad)
        Me.Panel6.Controls.Add(Me.Label11)
        Me.Panel6.Controls.Add(Me.Label19)
        Me.Panel6.Controls.Add(Me.Label17)
        Me.Panel6.Controls.Add(Me.TextCostoSinIva)
        Me.Panel6.Controls.Add(Me.TextCosto)
        Me.Panel6.Controls.Add(Me.ButtonCalculaCosto)
        Me.Panel6.Location = New System.Drawing.Point(37, 119)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(741, 52)
        Me.Panel6.TabIndex = 22
        '
        'TextCosto
        '
        Me.TextCosto.BackColor = System.Drawing.Color.White
        Me.TextCosto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCosto.Location = New System.Drawing.Point(605, 13)
        Me.TextCosto.MaxLength = 8
        Me.TextCosto.Name = "TextCosto"
        Me.TextCosto.ReadOnly = True
        Me.TextCosto.Size = New System.Drawing.Size(121, 20)
        Me.TextCosto.TabIndex = 177
        Me.TextCosto.TabStop = False
        Me.TextCosto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonCalculaCosto
        '
        Me.ButtonCalculaCosto.BackColor = System.Drawing.Color.Thistle
        Me.ButtonCalculaCosto.FlatAppearance.BorderSize = 0
        Me.ButtonCalculaCosto.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonCalculaCosto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonCalculaCosto.Location = New System.Drawing.Point(207, 9)
        Me.ButtonCalculaCosto.Name = "ButtonCalculaCosto"
        Me.ButtonCalculaCosto.Size = New System.Drawing.Size(119, 29)
        Me.ButtonCalculaCosto.TabIndex = 24
        Me.ButtonCalculaCosto.Text = "Calcula Costo --->"
        Me.ButtonCalculaCosto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonCalculaCosto.UseVisualStyleBackColor = False
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.White
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(781, 92)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 303
        Me.ComboEstado.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Panel6)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Location = New System.Drawing.Point(81, 248)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(809, 203)
        Me.Panel1.TabIndex = 300
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightGreen
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.PictureLupaLote)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Location = New System.Drawing.Point(37, 15)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(206, 62)
        Me.Panel2.TabIndex = 2
        Me.Panel2.Visible = False
        '
        'PictureLupaLote
        '
        Me.PictureLupaLote.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupaLote.InitialImage = Nothing
        Me.PictureLupaLote.Location = New System.Drawing.Point(141, 10)
        Me.PictureLupaLote.Name = "PictureLupaLote"
        Me.PictureLupaLote.Size = New System.Drawing.Size(52, 51)
        Me.PictureLupaLote.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupaLote.TabIndex = 270
        Me.PictureLupaLote.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 25)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(124, 16)
        Me.Label3.TabIndex = 156
        Me.Label3.Text = "Lotes Afectados "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(726, 98)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(46, 13)
        Me.Label13.TabIndex = 304
        Me.Label13.Text = "Estado"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonVerDetalleLotes
        '
        Me.ButtonVerDetalleLotes.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonVerDetalleLotes.FlatAppearance.BorderSize = 0
        Me.ButtonVerDetalleLotes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonVerDetalleLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonVerDetalleLotes.Location = New System.Drawing.Point(436, 455)
        Me.ButtonVerDetalleLotes.Name = "ButtonVerDetalleLotes"
        Me.ButtonVerDetalleLotes.Size = New System.Drawing.Size(210, 20)
        Me.ButtonVerDetalleLotes.TabIndex = 313
        Me.ButtonVerDetalleLotes.Text = "Detalle Lotes Afectados"
        Me.ButtonVerDetalleLotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonVerDetalleLotes.UseVisualStyleBackColor = False
        '
        'UnConsumoDeInsumo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(971, 604)
        Me.Controls.Add(Me.ButtonVerDetalleLotes)
        Me.Controls.Add(Me.ButtonVerDetalle)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.LabelTitulo)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.TextConsumo)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label13)
        Me.Name = "UnConsumoDeInsumo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Consumo de Insumo"
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureLupaLote, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LabelCuentas As System.Windows.Forms.Label
    Friend WithEvents Cuenta As System.Windows.Forms.ColumnHeader
    Friend WithEvents PictureLupaCuenta As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonVerDetalle As System.Windows.Forms.Button
    Friend WithEvents ListCuentas As System.Windows.Forms.ListView
    Friend WithEvents Importe1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Importe2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents ComboInsumo As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelTitulo As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextConsumo As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents TextCantidad As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents TextCostoSinIva As System.Windows.Forms.TextBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents TextCosto As System.Windows.Forms.TextBox
    Friend WithEvents ButtonCalculaCosto As System.Windows.Forms.Button
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PictureLupaLote As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ButtonVerDetalleLotes As System.Windows.Forms.Button
End Class
