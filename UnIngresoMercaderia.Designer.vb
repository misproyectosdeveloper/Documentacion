<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnIngresoMercaderia
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnIngresoMercaderia))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.AGranel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CantidadOriginal = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Medida = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Calibre = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Senia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PermisoImp = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextOrdenCompra = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.TextRemitoCliente = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboSucursal = New System.Windows.Forms.ComboBox
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextEmisor = New System.Windows.Forms.TextBox
        Me.ComboProvincia = New System.Windows.Forms.ComboBox
        Me.ComboTipoOperacion = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.MaskedRemito = New System.Windows.Forms.MaskedTextBox
        Me.TextMinuto = New System.Windows.Forms.TextBox
        Me.TextHora = New System.Windows.Forms.TextBox
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextGuia = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonRemitoCliente = New System.Windows.Forms.Button
        Me.ButtonNuevaIgualProveedor = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonRefrescar = New System.Windows.Forms.Button
        Me.ButtonEtiquetado = New System.Windows.Forms.Button
        Me.Panel2.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.ButtonEtiquetado)
        Me.Panel2.Controls.Add(Me.ButtonEliminarLinea)
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Location = New System.Drawing.Point(54, 108)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(971, 512)
        Me.Panel2.TabIndex = 100
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(44, 484)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(113, 20)
        Me.ButtonEliminarLinea.TabIndex = 999
        Me.ButtonEliminarLinea.TabStop = False
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.LightYellow
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.AGranel, Me.Stock, Me.CantidadOriginal, Me.Lote, Me.Secuencia, Me.LoteYSecuencia, Me.Articulo, Me.Cantidad, Me.Medida, Me.Calibre, Me.Senia, Me.PermisoImp})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle15
        Me.Grid.Location = New System.Drawing.Point(45, 1)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(887, 479)
        Me.Grid.TabIndex = 10
        '
        'AGranel
        '
        Me.AGranel.DataPropertyName = "AGranel"
        Me.AGranel.HeaderText = "AGranel"
        Me.AGranel.Name = "AGranel"
        Me.AGranel.Visible = False
        Me.AGranel.Width = 70
        '
        'Stock
        '
        Me.Stock.DataPropertyName = "Stock"
        Me.Stock.HeaderText = "Stock"
        Me.Stock.Name = "Stock"
        Me.Stock.Visible = False
        Me.Stock.Width = 60
        '
        'CantidadOriginal
        '
        Me.CantidadOriginal.DataPropertyName = "CantidadOriginal"
        Me.CantidadOriginal.HeaderText = "CantidadOriginal"
        Me.CantidadOriginal.Name = "CantidadOriginal"
        Me.CantidadOriginal.Visible = False
        Me.CantidadOriginal.Width = 109
        '
        'Lote
        '
        Me.Lote.DataPropertyName = "Lote"
        Me.Lote.HeaderText = "Lote"
        Me.Lote.Name = "Lote"
        Me.Lote.Visible = False
        Me.Lote.Width = 53
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        Me.Secuencia.Width = 83
        '
        'LoteYSecuencia
        '
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle10
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 70
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 70
        '
        'Articulo
        '
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle11
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 370
        Me.Articulo.Name = "Articulo"
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Articulo.Width = 370
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle12
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Width = 80
        '
        'Medida
        '
        Me.Medida.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Medida.DataPropertyName = "Medida"
        Me.Medida.HeaderText = ""
        Me.Medida.MinimumWidth = 25
        Me.Medida.Name = "Medida"
        Me.Medida.ReadOnly = True
        Me.Medida.Width = 25
        '
        'Calibre
        '
        Me.Calibre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Calibre.DataPropertyName = "Calibre"
        Me.Calibre.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Calibre.HeaderText = "Calibre"
        Me.Calibre.MinimumWidth = 80
        Me.Calibre.Name = "Calibre"
        Me.Calibre.Width = 80
        '
        'Senia
        '
        Me.Senia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Senia.DataPropertyName = "Senia"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Senia.DefaultCellStyle = DataGridViewCellStyle13
        Me.Senia.HeaderText = "Senia"
        Me.Senia.MaxInputLength = 8
        Me.Senia.MinimumWidth = 70
        Me.Senia.Name = "Senia"
        Me.Senia.Width = 70
        '
        'PermisoImp
        '
        Me.PermisoImp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PermisoImp.DataPropertyName = "PermisoImp"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PermisoImp.DefaultCellStyle = DataGridViewCellStyle14
        Me.PermisoImp.HeaderText = "Permiso Imp."
        Me.PermisoImp.MaxInputLength = 16
        Me.PermisoImp.MinimumWidth = 130
        Me.PermisoImp.Name = "PermisoImp"
        Me.PermisoImp.Width = 130
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 76)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 15)
        Me.Label7.TabIndex = 115
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Location = New System.Drawing.Point(84, 75)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(334, 20)
        Me.TextComentario.TabIndex = 8
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextOrdenCompra)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Controls.Add(Me.TextRemitoCliente)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.ComboSucursal)
        Me.Panel1.Controls.Add(Me.ComboCosteo)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.ComboMoneda)
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.TextEmisor)
        Me.Panel1.Controls.Add(Me.ComboProvincia)
        Me.Panel1.Controls.Add(Me.ComboTipoOperacion)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.MaskedRemito)
        Me.Panel1.Controls.Add(Me.TextMinuto)
        Me.Panel1.Controls.Add(Me.TextHora)
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TextGuia)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Location = New System.Drawing.Point(55, 5)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(969, 102)
        Me.Panel1.TabIndex = 88
        '
        'TextOrdenCompra
        '
        Me.TextOrdenCompra.BackColor = System.Drawing.Color.White
        Me.TextOrdenCompra.Enabled = False
        Me.TextOrdenCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextOrdenCompra.Location = New System.Drawing.Point(447, 26)
        Me.TextOrdenCompra.MaxLength = 12
        Me.TextOrdenCompra.Name = "TextOrdenCompra"
        Me.TextOrdenCompra.Size = New System.Drawing.Size(101, 20)
        Me.TextOrdenCompra.TabIndex = 211
        Me.TextOrdenCompra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(365, 30)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(75, 13)
        Me.Label14.TabIndex = 212
        Me.Label14.Text = "Orden Compra"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(850, 2)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(99, 20)
        Me.DateTime1.TabIndex = 210
        Me.DateTime1.TabStop = False
        '
        'TextRemitoCliente
        '
        Me.TextRemitoCliente.BackColor = System.Drawing.Color.White
        Me.TextRemitoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRemitoCliente.Location = New System.Drawing.Point(566, 73)
        Me.TextRemitoCliente.MaxLength = 12
        Me.TextRemitoCliente.Name = "TextRemitoCliente"
        Me.TextRemitoCliente.ReadOnly = True
        Me.TextRemitoCliente.Size = New System.Drawing.Size(119, 20)
        Me.TextRemitoCliente.TabIndex = 208
        Me.TextRemitoCliente.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(482, 77)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(75, 13)
        Me.Label13.TabIndex = 209
        Me.Label13.Text = "Remito Cliente"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(527, 6)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 13)
        Me.Label11.TabIndex = 207
        Me.Label11.Text = "Sucursal"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboSucursal
        '
        Me.ComboSucursal.BackColor = System.Drawing.Color.White
        Me.ComboSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursal.FormattingEnabled = True
        Me.ComboSucursal.Location = New System.Drawing.Point(584, 2)
        Me.ComboSucursal.Name = "ComboSucursal"
        Me.ComboSucursal.Size = New System.Drawing.Size(166, 21)
        Me.ComboSucursal.TabIndex = 206
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(496, 50)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(202, 21)
        Me.ComboCosteo.TabIndex = 204
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(447, 54)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(40, 13)
        Me.Label12.TabIndex = 205
        Me.Label12.Text = "Costeo"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(773, 49)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(104, 21)
        Me.ComboMoneda.TabIndex = 200
        Me.ComboMoneda.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(720, 53)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 201
        Me.Label10.Text = "Moneda"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextEmisor
        '
        Me.TextEmisor.BackColor = System.Drawing.Color.White
        Me.TextEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextEmisor.Location = New System.Drawing.Point(86, 4)
        Me.TextEmisor.MaxLength = 30
        Me.TextEmisor.Name = "TextEmisor"
        Me.TextEmisor.ReadOnly = True
        Me.TextEmisor.Size = New System.Drawing.Size(246, 20)
        Me.TextEmisor.TabIndex = 199
        '
        'ComboProvincia
        '
        Me.ComboProvincia.BackColor = System.Drawing.Color.White
        Me.ComboProvincia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProvincia.FormattingEnabled = True
        Me.ComboProvincia.Location = New System.Drawing.Point(86, 48)
        Me.ComboProvincia.Name = "ComboProvincia"
        Me.ComboProvincia.Size = New System.Drawing.Size(134, 21)
        Me.ComboProvincia.TabIndex = 198
        '
        'ComboTipoOperacion
        '
        Me.ComboTipoOperacion.BackColor = System.Drawing.Color.White
        Me.ComboTipoOperacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoOperacion.FormattingEnabled = True
        Me.ComboTipoOperacion.Location = New System.Drawing.Point(316, 50)
        Me.ComboTipoOperacion.Name = "ComboTipoOperacion"
        Me.ComboTipoOperacion.Size = New System.Drawing.Size(121, 21)
        Me.ComboTipoOperacion.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(231, 54)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(80, 13)
        Me.Label9.TabIndex = 197
        Me.Label9.Text = "Tipo Operación"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'MaskedRemito
        '
        Me.MaskedRemito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedRemito.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedRemito.Location = New System.Drawing.Point(86, 27)
        Me.MaskedRemito.Mask = "0000-00000000"
        Me.MaskedRemito.Name = "MaskedRemito"
        Me.MaskedRemito.Size = New System.Drawing.Size(97, 20)
        Me.MaskedRemito.TabIndex = 1
        Me.MaskedRemito.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'TextMinuto
        '
        Me.TextMinuto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMinuto.Location = New System.Drawing.Point(719, 27)
        Me.TextMinuto.MaxLength = 12
        Me.TextMinuto.Name = "TextMinuto"
        Me.TextMinuto.Size = New System.Drawing.Size(33, 20)
        Me.TextMinuto.TabIndex = 7
        Me.TextMinuto.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TextHora
        '
        Me.TextHora.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextHora.Location = New System.Drawing.Point(679, 27)
        Me.TextHora.MaxLength = 12
        Me.TextHora.Name = "TextHora"
        Me.TextHora.Size = New System.Drawing.Size(33, 20)
        Me.TextHora.TabIndex = 6
        Me.TextHora.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(911, 54)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 117
        Me.PictureCandado.TabStop = False
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(397, 2)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(123, 21)
        Me.ComboDeposito.TabIndex = 0
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(340, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "Deposito"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(602, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "Hora Ingreso"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(795, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Fecha "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(11, 52)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(51, 13)
        Me.Label3.TabIndex = 22
        Me.Label3.Text = "Provincia"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextGuia
        '
        Me.TextGuia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextGuia.Location = New System.Drawing.Point(243, 27)
        Me.TextGuia.MaxLength = 12
        Me.TextGuia.Name = "TextGuia"
        Me.TextGuia.Size = New System.Drawing.Size(101, 20)
        Me.TextGuia.TabIndex = 2
        Me.TextGuia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(198, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 20
        Me.Label2.Text = "Guia"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(10, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 13)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Remito"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(56, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Proveedor"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonRemitoCliente
        '
        Me.ButtonRemitoCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonRemitoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRemitoCliente.Location = New System.Drawing.Point(431, 627)
        Me.ButtonRemitoCliente.Name = "ButtonRemitoCliente"
        Me.ButtonRemitoCliente.Size = New System.Drawing.Size(138, 37)
        Me.ButtonRemitoCliente.TabIndex = 1027
        Me.ButtonRemitoCliente.TabStop = False
        Me.ButtonRemitoCliente.Text = "Remitir a un  Cliente"
        Me.ButtonRemitoCliente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonRemitoCliente.UseVisualStyleBackColor = True
        '
        'ButtonNuevaIgualProveedor
        '
        Me.ButtonNuevaIgualProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevaIgualProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaIgualProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevaIgualProveedor.Location = New System.Drawing.Point(173, 628)
        Me.ButtonNuevaIgualProveedor.Name = "ButtonNuevaIgualProveedor"
        Me.ButtonNuevaIgualProveedor.Size = New System.Drawing.Size(138, 37)
        Me.ButtonNuevaIgualProveedor.TabIndex = 1025
        Me.ButtonNuevaIgualProveedor.TabStop = False
        Me.ButtonNuevaIgualProveedor.Text = "Nuevo Ingr. Igual Proveedor"
        Me.ButtonNuevaIgualProveedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaIgualProveedor.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(578, 627)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(137, 37)
        Me.ButtonAsientoContable.TabIndex = 1023
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.Location = New System.Drawing.Point(53, 628)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(108, 37)
        Me.ButtonNuevo.TabIndex = 23
        Me.ButtonNuevo.Text = "Nuevo Ingreso "
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(324, 627)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(101, 37)
        Me.ButtonImprimir.TabIndex = 1026
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatAppearance.BorderSize = 0
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(551, 595)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(135, 30)
        Me.ButtonAnula.TabIndex = 1024
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anular Ingreso"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        Me.ButtonAnula.Visible = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(885, 626)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(138, 37)
        Me.ButtonAceptar.TabIndex = 20
        Me.ButtonAceptar.Text = "Graba Ingreso "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonRefrescar
        '
        Me.ButtonRefrescar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonRefrescar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRefrescar.Location = New System.Drawing.Point(726, 626)
        Me.ButtonRefrescar.Name = "ButtonRefrescar"
        Me.ButtonRefrescar.Size = New System.Drawing.Size(152, 37)
        Me.ButtonRefrescar.TabIndex = 1028
        Me.ButtonRefrescar.TabStop = False
        Me.ButtonRefrescar.Text = "Refresca Lista de Precios"
        Me.ButtonRefrescar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonRefrescar.UseVisualStyleBackColor = True
        '
        'ButtonEtiquetado
        '
        Me.ButtonEtiquetado.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ButtonEtiquetado.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonEtiquetado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEtiquetado.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonEtiquetado.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonEtiquetado.Location = New System.Drawing.Point(783, 481)
        Me.ButtonEtiquetado.Name = "ButtonEtiquetado"
        Me.ButtonEtiquetado.Size = New System.Drawing.Size(149, 28)
        Me.ButtonEtiquetado.TabIndex = 1029
        Me.ButtonEtiquetado.Text = "ETIQUETA LOTE"
        Me.ButtonEtiquetado.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonEtiquetado.UseVisualStyleBackColor = False
        '
        'UnIngresoMercaderia
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(1074, 676)
        Me.Controls.Add(Me.ButtonRefrescar)
        Me.Controls.Add(Me.ButtonRemitoCliente)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonNuevaIgualProveedor)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Name = "UnIngresoMercaderia"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ingreso de Mercaderia"
        Me.Panel2.ResumeLayout(False)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextGuia As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents TextMinuto As System.Windows.Forms.TextBox
    Friend WithEvents TextHora As System.Windows.Forms.TextBox
    Friend WithEvents MaskedRemito As System.Windows.Forms.MaskedTextBox
    Friend WithEvents ComboTipoOperacion As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboProvincia As System.Windows.Forms.ComboBox
    Friend WithEvents TextEmisor As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevaIgualProveedor As System.Windows.Forms.Button
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents AGranel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CantidadOriginal As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Medida As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Calibre As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Senia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PermisoImp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboSucursal As System.Windows.Forms.ComboBox
    Friend WithEvents TextRemitoCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ButtonRemitoCliente As System.Windows.Forms.Button
    Friend WithEvents ButtonRefrescar As System.Windows.Forms.Button
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents TextOrdenCompra As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ButtonEtiquetado As System.Windows.Forms.Button
End Class
