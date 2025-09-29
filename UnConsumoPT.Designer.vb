<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnConsumoPT
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnConsumoPT))
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.LabelCuentas = New System.Windows.Forms.Label
        Me.PictureLupaCuenta = New System.Windows.Forms.PictureBox
        Me.ListCuentas = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Importe1 = New System.Windows.Forms.ColumnHeader
        Me.Importe2 = New System.Windows.Forms.ColumnHeader
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonTextoRecibo = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextTotalConsumo = New System.Windows.Forms.TextBox
        Me.ButtonNetoPorLotes = New System.Windows.Forms.Button
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label15 = New System.Windows.Forms.Label
        Me.RadioPorKilo = New System.Windows.Forms.RadioButton
        Me.RadioPorUnidad = New System.Windows.Forms.RadioButton
        Me.TextConsumo = New System.Windows.Forms.TextBox
        Me.AGranel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Indice = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LupaLotes = New System.Windows.Forms.DataGridViewImageColumn
        Me.Medida = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioLista = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.KilosXUnidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Neto = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TotalArticulo = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AGranel, Me.Indice, Me.LoteYSecuencia, Me.Articulo, Me.Cantidad, Me.LupaLotes, Me.Medida, Me.PrecioLista, Me.Precio, Me.KilosXUnidad, Me.Neto, Me.TotalArticulo})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle10
        Me.Grid.Location = New System.Drawing.Point(74, 25)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle11
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(917, 408)
        Me.Grid.TabIndex = 15
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.LabelCuentas)
        Me.Panel1.Controls.Add(Me.PictureLupaCuenta)
        Me.Panel1.Controls.Add(Me.ListCuentas)
        Me.Panel1.Controls.Add(Me.ComboCosteo)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboNegocio)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Location = New System.Drawing.Point(13, 38)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1080, 85)
        Me.Panel1.TabIndex = 307
        '
        'LabelCuentas
        '
        Me.LabelCuentas.AutoSize = True
        Me.LabelCuentas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCuentas.Location = New System.Drawing.Point(821, 36)
        Me.LabelCuentas.Name = "LabelCuentas"
        Me.LabelCuentas.Size = New System.Drawing.Size(104, 13)
        Me.LabelCuentas.TabIndex = 1006
        Me.LabelCuentas.Text = "Imputación Contable"
        Me.LabelCuentas.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureLupaCuenta
        '
        Me.PictureLupaCuenta.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupaCuenta.InitialImage = Nothing
        Me.PictureLupaCuenta.Location = New System.Drawing.Point(942, 50)
        Me.PictureLupaCuenta.Name = "PictureLupaCuenta"
        Me.PictureLupaCuenta.Size = New System.Drawing.Size(31, 29)
        Me.PictureLupaCuenta.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupaCuenta.TabIndex = 1005
        Me.PictureLupaCuenta.TabStop = False
        '
        'ListCuentas
        '
        Me.ListCuentas.BackColor = System.Drawing.Color.White
        Me.ListCuentas.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.Importe1, Me.Importe2})
        Me.ListCuentas.Location = New System.Drawing.Point(812, 50)
        Me.ListCuentas.Name = "ListCuentas"
        Me.ListCuentas.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ListCuentas.Size = New System.Drawing.Size(121, 28)
        Me.ListCuentas.TabIndex = 1004
        Me.ListCuentas.TileSize = New System.Drawing.Size(90, 15)
        Me.ListCuentas.UseCompatibleStateImageBehavior = False
        Me.ListCuentas.View = System.Windows.Forms.View.Tile
        '
        'ComboCosteo
        '
        Me.ComboCosteo.BackColor = System.Drawing.Color.White
        Me.ComboCosteo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboCosteo.Enabled = False
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(329, 5)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(217, 21)
        Me.ComboCosteo.TabIndex = 288
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(271, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 289
        Me.Label4.Text = "Costeo"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboNegocio
        '
        Me.ComboNegocio.BackColor = System.Drawing.Color.White
        Me.ComboNegocio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboNegocio.Enabled = False
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(74, 6)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(189, 21)
        Me.ComboNegocio.TabIndex = 286
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(8, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(54, 13)
        Me.Label11.TabIndex = 287
        Me.Label11.Text = "Negocio"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(1001, 7)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(53, 57)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 124
        Me.PictureCandado.TabStop = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(7, 37)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(71, 15)
        Me.Label18.TabIndex = 122
        Me.Label18.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Location = New System.Drawing.Point(82, 36)
        Me.TextComentario.MaxLength = 15
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(216, 20)
        Me.TextComentario.TabIndex = 121
        Me.TextComentario.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(575, 10)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(57, 13)
        Me.Label3.TabIndex = 37
        Me.Label3.Text = "Deposito"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(647, 7)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(161, 21)
        Me.ComboDeposito.TabIndex = 36
        Me.ComboDeposito.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(829, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(42, 13)
        Me.Label1.TabIndex = 33
        Me.Label1.Text = "Fecha"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(876, 7)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(93, 20)
        Me.DateTime1.TabIndex = 133
        Me.DateTime1.TabStop = False
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(682, 667)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(139, 32)
        Me.ButtonAsientoContable.TabIndex = 320
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
        Me.ButtonAnula.Location = New System.Drawing.Point(441, 666)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(131, 33)
        Me.ButtonAnula.TabIndex = 319
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anular Consumo"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(13, 666)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(117, 33)
        Me.ButtonNuevo.TabIndex = 311
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nuevo Consumo"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(947, 666)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(146, 33)
        Me.ButtonAceptar.TabIndex = 309
        Me.ButtonAceptar.Text = "Graba Consumo "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = CType(resources.GetObject("ButtonImprimir.Image"), System.Drawing.Image)
        Me.ButtonImprimir.Location = New System.Drawing.Point(219, 667)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(116, 33)
        Me.ButtonImprimir.TabIndex = 310
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime "
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(12, 15)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(58, 13)
        Me.Label9.TabIndex = 314
        Me.Label9.Text = "Consumo"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.White
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(984, 8)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 312
        Me.ComboEstado.TabStop = False
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(929, 14)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 313
        Me.Label2.Text = "Estado"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonTextoRecibo
        '
        Me.ButtonTextoRecibo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonTextoRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonTextoRecibo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonTextoRecibo.Location = New System.Drawing.Point(78, 479)
        Me.ButtonTextoRecibo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonTextoRecibo.Name = "ButtonTextoRecibo"
        Me.ButtonTextoRecibo.Size = New System.Drawing.Size(140, 30)
        Me.ButtonTextoRecibo.TabIndex = 308
        Me.ButtonTextoRecibo.TabStop = False
        Me.ButtonTextoRecibo.Text = "Texto Para Impresión"
        Me.ButtonTextoRecibo.UseVisualStyleBackColor = False
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Transparent
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.TextTotalConsumo)
        Me.Panel2.Controls.Add(Me.ButtonNetoPorLotes)
        Me.Panel2.Controls.Add(Me.ButtonTextoRecibo)
        Me.Panel2.Controls.Add(Me.ButtonEliminarLinea)
        Me.Panel2.Controls.Add(Me.Panel4)
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(13, 125)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1080, 514)
        Me.Panel2.TabIndex = 308
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(837, 443)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(34, 13)
        Me.Label5.TabIndex = 327
        Me.Label5.Text = "Total "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotalConsumo
        '
        Me.TextTotalConsumo.BackColor = System.Drawing.Color.White
        Me.TextTotalConsumo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalConsumo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalConsumo.Location = New System.Drawing.Point(880, 439)
        Me.TextTotalConsumo.MaxLength = 20
        Me.TextTotalConsumo.Name = "TextTotalConsumo"
        Me.TextTotalConsumo.ReadOnly = True
        Me.TextTotalConsumo.Size = New System.Drawing.Size(111, 20)
        Me.TextTotalConsumo.TabIndex = 326
        Me.TextTotalConsumo.TabStop = False
        Me.TextTotalConsumo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonNetoPorLotes
        '
        Me.ButtonNetoPorLotes.BackColor = System.Drawing.Color.Gainsboro
        Me.ButtonNetoPorLotes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNetoPorLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNetoPorLotes.Location = New System.Drawing.Point(249, 482)
        Me.ButtonNetoPorLotes.Name = "ButtonNetoPorLotes"
        Me.ButtonNetoPorLotes.Size = New System.Drawing.Size(143, 26)
        Me.ButtonNetoPorLotes.TabIndex = 325
        Me.ButtonNetoPorLotes.Text = "Ver Importe Por Lotes"
        Me.ButtonNetoPorLotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNetoPorLotes.UseVisualStyleBackColor = False
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(72, 438)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(98, 20)
        Me.ButtonEliminarLinea.TabIndex = 140
        Me.ButtonEliminarLinea.TabStop = False
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.RadioPorKilo)
        Me.Panel4.Controls.Add(Me.RadioPorUnidad)
        Me.Panel4.Location = New System.Drawing.Point(462, -1)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(215, 24)
        Me.Panel4.TabIndex = 7
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(4, 4)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(48, 15)
        Me.Label15.TabIndex = 137
        Me.Label15.Text = "Precio: "
        '
        'RadioPorKilo
        '
        Me.RadioPorKilo.AutoSize = True
        Me.RadioPorKilo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPorKilo.Location = New System.Drawing.Point(148, 4)
        Me.RadioPorKilo.Name = "RadioPorKilo"
        Me.RadioPorKilo.Size = New System.Drawing.Size(61, 17)
        Me.RadioPorKilo.TabIndex = 9
        Me.RadioPorKilo.Text = "Por Kilo"
        Me.RadioPorKilo.UseVisualStyleBackColor = True
        '
        'RadioPorUnidad
        '
        Me.RadioPorUnidad.AutoSize = True
        Me.RadioPorUnidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPorUnidad.Location = New System.Drawing.Point(60, 4)
        Me.RadioPorUnidad.Name = "RadioPorUnidad"
        Me.RadioPorUnidad.Size = New System.Drawing.Size(78, 17)
        Me.RadioPorUnidad.TabIndex = 8
        Me.RadioPorUnidad.Text = "Por Unidad"
        Me.RadioPorUnidad.UseVisualStyleBackColor = True
        '
        'TextConsumo
        '
        Me.TextConsumo.BackColor = System.Drawing.Color.White
        Me.TextConsumo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextConsumo.Location = New System.Drawing.Point(89, 9)
        Me.TextConsumo.MaxLength = 15
        Me.TextConsumo.Name = "TextConsumo"
        Me.TextConsumo.ReadOnly = True
        Me.TextConsumo.Size = New System.Drawing.Size(135, 20)
        Me.TextConsumo.TabIndex = 323
        Me.TextConsumo.TabStop = False
        Me.TextConsumo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'AGranel
        '
        Me.AGranel.HeaderText = "AGranel"
        Me.AGranel.Name = "AGranel"
        Me.AGranel.Visible = False
        Me.AGranel.Width = 70
        '
        'Indice
        '
        Me.Indice.DataPropertyName = "indice"
        Me.Indice.HeaderText = "Indice"
        Me.Indice.Name = "Indice"
        Me.Indice.Visible = False
        Me.Indice.Width = 61
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle2
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 70
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.LoteYSecuencia.Width = 70
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle3
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 300
        Me.Articulo.Name = "Articulo"
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.Width = 300
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle4
        Me.Cantidad.HeaderText = "Cantidad xUni"
        Me.Cantidad.MaxInputLength = 10
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cantidad.Width = 80
        '
        'LupaLotes
        '
        Me.LupaLotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.LupaLotes.HeaderText = "Lote"
        Me.LupaLotes.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.LupaLotes.MinimumWidth = 40
        Me.LupaLotes.Name = "LupaLotes"
        Me.LupaLotes.Width = 40
        '
        'Medida
        '
        Me.Medida.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Medida.HeaderText = ""
        Me.Medida.MinimumWidth = 25
        Me.Medida.Name = "Medida"
        Me.Medida.ReadOnly = True
        Me.Medida.Width = 25
        '
        'PrecioLista
        '
        Me.PrecioLista.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrecioLista.DefaultCellStyle = DataGridViewCellStyle5
        Me.PrecioLista.HeaderText = "Precio"
        Me.PrecioLista.MaxInputLength = 10
        Me.PrecioLista.MinimumWidth = 100
        Me.PrecioLista.Name = "PrecioLista"
        '
        'Precio
        '
        Me.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Gainsboro
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle6
        Me.Precio.HeaderText = "Precio  "
        Me.Precio.MaxInputLength = 10
        Me.Precio.MinimumWidth = 84
        Me.Precio.Name = "Precio"
        Me.Precio.ReadOnly = True
        Me.Precio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Precio.Width = 84
        '
        'KilosXUnidad
        '
        Me.KilosXUnidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.KilosXUnidad.DataPropertyName = "KilosXUnidad"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.Gainsboro
        Me.KilosXUnidad.DefaultCellStyle = DataGridViewCellStyle7
        Me.KilosXUnidad.HeaderText = "Kilos xUni"
        Me.KilosXUnidad.MaxInputLength = 8
        Me.KilosXUnidad.MinimumWidth = 60
        Me.KilosXUnidad.Name = "KilosXUnidad"
        Me.KilosXUnidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.KilosXUnidad.Width = 60
        '
        'Neto
        '
        Me.Neto.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.Gainsboro
        Me.Neto.DefaultCellStyle = DataGridViewCellStyle8
        Me.Neto.HeaderText = "Neto"
        Me.Neto.MinimumWidth = 90
        Me.Neto.Name = "Neto"
        Me.Neto.ReadOnly = True
        Me.Neto.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Neto.Visible = False
        Me.Neto.Width = 90
        '
        'TotalArticulo
        '
        Me.TotalArticulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TotalArticulo.DataPropertyName = "TotalArticulo"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.Gainsboro
        Me.TotalArticulo.DefaultCellStyle = DataGridViewCellStyle9
        Me.TotalArticulo.HeaderText = "Total"
        Me.TotalArticulo.MaxInputLength = 8
        Me.TotalArticulo.MinimumWidth = 90
        Me.TotalArticulo.Name = "TotalArticulo"
        Me.TotalArticulo.ReadOnly = True
        Me.TotalArticulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.TotalArticulo.Width = 90
        '
        'UnConsumoPT
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(1110, 712)
        Me.Controls.Add(Me.TextConsumo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Panel2)
        Me.Name = "UnConsumoPT"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Consumo Terminado"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureLupaCuenta, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonTextoRecibo As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents RadioPorKilo As System.Windows.Forms.RadioButton
    Friend WithEvents RadioPorUnidad As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonNetoPorLotes As System.Windows.Forms.Button
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextConsumo As System.Windows.Forms.TextBox
    Friend WithEvents TextTotalConsumo As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents LabelCuentas As System.Windows.Forms.Label
    Friend WithEvents PictureLupaCuenta As System.Windows.Forms.PictureBox
    Friend WithEvents ListCuentas As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Importe1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Importe2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents AGranel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Indice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LupaLotes As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Medida As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioLista As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KilosXUnidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Neto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalArticulo As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
