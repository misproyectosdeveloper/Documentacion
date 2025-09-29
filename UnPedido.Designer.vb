<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnPedido
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.ButtonImportar = New System.Windows.Forms.Button
        Me.ButtonBorrar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.FechaDesde = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboSucursal = New System.Windows.Forms.ComboBox
        Me.ComboCliente = New System.Windows.Forms.ComboBox
        Me.FechaHasta = New System.Windows.Forms.DateTimePicker
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextPedidoCliente = New System.Windows.Forms.TextBox
        Me.TextPedido = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Entregada = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TipoPrecio = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Armador = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fletero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.PanelPrecio = New System.Windows.Forms.Panel
        Me.ComboTipoPrecio = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelIva = New System.Windows.Forms.Panel
        Me.RadioSinIva = New System.Windows.Forms.RadioButton
        Me.RadioFinal = New System.Windows.Forms.RadioButton
        Me.ButtonNuevaIgualCliente = New System.Windows.Forms.Button
        Me.ButtonExportarExcelCorto = New System.Windows.Forms.Button
        Me.Label10 = New System.Windows.Forms.Label
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ButtonImportacionKrikos = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelPrecio.SuspendLayout()
        Me.PanelIva.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(303, 624)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(132, 23)
        Me.ButtonExportarExcel.TabIndex = 295
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'ButtonImportar
        '
        Me.ButtonImportar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonImportar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportar.Location = New System.Drawing.Point(20, 76)
        Me.ButtonImportar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonImportar.Name = "ButtonImportar"
        Me.ButtonImportar.Size = New System.Drawing.Size(110, 23)
        Me.ButtonImportar.TabIndex = 294
        Me.ButtonImportar.Text = "Importar Pedido"
        Me.ButtonImportar.UseVisualStyleBackColor = False
        '
        'ButtonBorrar
        '
        Me.ButtonBorrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBorrar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBorrar.Image = Global.ScomerV01.My.Resources.Resources.Basurero
        Me.ButtonBorrar.Location = New System.Drawing.Point(459, 624)
        Me.ButtonBorrar.Name = "ButtonBorrar"
        Me.ButtonBorrar.Size = New System.Drawing.Size(130, 23)
        Me.ButtonBorrar.TabIndex = 293
        Me.ButtonBorrar.Text = "Borra Pedido"
        Me.ButtonBorrar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBorrar.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(605, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(42, 13)
        Me.Label7.TabIndex = 289
        Me.Label7.Text = "Fecha"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(656, 5)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(89, 20)
        Me.DateTime1.TabIndex = 287
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Location = New System.Drawing.Point(165, 617)
        Me.ButtonEliminar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonEliminar.TabIndex = 283
        Me.ButtonEliminar.Text = "Borrar Linea"
        Me.ButtonEliminar.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonAceptar.Location = New System.Drawing.Point(609, 625)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(130, 23)
        Me.ButtonAceptar.TabIndex = 284
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(22, 617)
        Me.ButtonPrimero.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(32, 22)
        Me.ButtonPrimero.TabIndex = 280
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(55, 617)
        Me.ButtonAnterior.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(32, 22)
        Me.ButtonAnterior.TabIndex = 281
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(122, 617)
        Me.ButtonUltimo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(32, 22)
        Me.ButtonUltimo.TabIndex = 279
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(89, 617)
        Me.ButtonPosterior.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(32, 22)
        Me.ButtonPosterior.TabIndex = 282
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 296
        Me.Label3.Text = "Sucursal"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(21, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(146, 13)
        Me.Label1.TabIndex = 300
        Me.Label1.Text = "Fecha Entrega:    Desde"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaDesde
        '
        Me.FechaDesde.CustomFormat = "dd/MM/yyyy"
        Me.FechaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaDesde.Location = New System.Drawing.Point(173, 50)
        Me.FechaDesde.Name = "FechaDesde"
        Me.FechaDesde.Size = New System.Drawing.Size(108, 20)
        Me.FechaDesde.TabIndex = 299
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(238, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 302
        Me.Label4.Text = "Cliente"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboSucursal
        '
        Me.ComboSucursal.BackColor = System.Drawing.Color.White
        Me.ComboSucursal.Enabled = False
        Me.ComboSucursal.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.ComboSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursal.FormattingEnabled = True
        Me.ComboSucursal.Location = New System.Drawing.Point(83, 27)
        Me.ComboSucursal.Name = "ComboSucursal"
        Me.ComboSucursal.Size = New System.Drawing.Size(155, 21)
        Me.ComboSucursal.TabIndex = 303
        '
        'ComboCliente
        '
        Me.ComboCliente.BackColor = System.Drawing.Color.White
        Me.ComboCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboCliente.Enabled = False
        Me.ComboCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCliente.FormattingEnabled = True
        Me.ComboCliente.Location = New System.Drawing.Point(294, 6)
        Me.ComboCliente.Name = "ComboCliente"
        Me.ComboCliente.Size = New System.Drawing.Size(235, 21)
        Me.ComboCliente.TabIndex = 304
        Me.ComboCliente.TabStop = False
        '
        'FechaHasta
        '
        Me.FechaHasta.CustomFormat = "dd/MM/yyyy"
        Me.FechaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaHasta.Location = New System.Drawing.Point(337, 50)
        Me.FechaHasta.Name = "FechaHasta"
        Me.FechaHasta.Size = New System.Drawing.Size(108, 20)
        Me.FechaHasta.TabIndex = 305
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(291, 54)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 13)
        Me.Label5.TabIndex = 306
        Me.Label5.Text = "Hasta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(263, 33)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(89, 13)
        Me.Label6.TabIndex = 307
        Me.Label6.Text = "Pedido Cliente"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextPedidoCliente
        '
        Me.TextPedidoCliente.BackColor = System.Drawing.Color.White
        Me.TextPedidoCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPedidoCliente.Location = New System.Drawing.Point(358, 29)
        Me.TextPedidoCliente.Name = "TextPedidoCliente"
        Me.TextPedidoCliente.Size = New System.Drawing.Size(211, 20)
        Me.TextPedidoCliente.TabIndex = 308
        '
        'TextPedido
        '
        Me.TextPedido.BackColor = System.Drawing.Color.White
        Me.TextPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPedido.Location = New System.Drawing.Point(83, 6)
        Me.TextPedido.Name = "TextPedido"
        Me.TextPedido.ReadOnly = True
        Me.TextPedido.Size = New System.Drawing.Size(133, 20)
        Me.TextPedido.TabIndex = 310
        Me.TextPedido.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 10)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 309
        Me.Label8.Text = "Pedido"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(153, 82)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(134, 13)
        Me.Label9.TabIndex = 311
        Me.Label9.Text = "Tiene Lista de Precios"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Articulo, Me.Cantidad, Me.Entregada, Me.Saldo, Me.TipoPrecio, Me.Precio, Me.Armador, Me.Fletero})
        Me.Grid.Location = New System.Drawing.Point(21, 99)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(724, 515)
        Me.Grid.TabIndex = 312
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 250
        Me.Articulo.Name = "Articulo"
        Me.Articulo.Width = 250
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle1
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MaxInputLength = 10
        Me.Cantidad.MinimumWidth = 90
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Width = 90
        '
        'Entregada
        '
        Me.Entregada.DataPropertyName = "Entregada"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Entregada.DefaultCellStyle = DataGridViewCellStyle2
        Me.Entregada.HeaderText = "Entregado"
        Me.Entregada.Name = "Entregada"
        Me.Entregada.ReadOnly = True
        Me.Entregada.Width = 90
        '
        'Saldo
        '
        Me.Saldo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle3
        Me.Saldo.HeaderText = "Saldo"
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Width = 90
        '
        'TipoPrecio
        '
        Me.TipoPrecio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoPrecio.DataPropertyName = "TipoPrecio"
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        Me.TipoPrecio.DefaultCellStyle = DataGridViewCellStyle4
        Me.TipoPrecio.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TipoPrecio.HeaderText = "Tipo Precio"
        Me.TipoPrecio.MinimumWidth = 50
        Me.TipoPrecio.Name = "TipoPrecio"
        Me.TipoPrecio.Width = 50
        '
        'Precio
        '
        Me.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle5
        Me.Precio.HeaderText = "Precio"
        Me.Precio.MaxInputLength = 10
        Me.Precio.Name = "Precio"
        Me.Precio.Width = 90
        '
        'Armador
        '
        Me.Armador.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Armador.HeaderText = "Armador"
        Me.Armador.MinimumWidth = 50
        Me.Armador.Name = "Armador"
        Me.Armador.ReadOnly = True
        Me.Armador.Visible = False
        Me.Armador.Width = 50
        '
        'Fletero
        '
        Me.Fletero.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fletero.HeaderText = "Fletero"
        Me.Fletero.MinimumWidth = 50
        Me.Fletero.Name = "Fletero"
        Me.Fletero.ReadOnly = True
        Me.Fletero.Visible = False
        Me.Fletero.Width = 50
        '
        'CheckCerrado
        '
        Me.CheckCerrado.AutoSize = True
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckCerrado.Location = New System.Drawing.Point(589, 36)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(104, 28)
        Me.CheckCerrado.TabIndex = 313
        Me.CheckCerrado.Text = "Cerrado"
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'PanelPrecio
        '
        Me.PanelPrecio.Controls.Add(Me.ComboTipoPrecio)
        Me.PanelPrecio.Controls.Add(Me.Label2)
        Me.PanelPrecio.Controls.Add(Me.PanelIva)
        Me.PanelPrecio.Location = New System.Drawing.Point(352, 70)
        Me.PanelPrecio.Name = "PanelPrecio"
        Me.PanelPrecio.Size = New System.Drawing.Size(393, 28)
        Me.PanelPrecio.TabIndex = 314
        '
        'ComboTipoPrecio
        '
        Me.ComboTipoPrecio.FormattingEnabled = True
        Me.ComboTipoPrecio.Items.AddRange(New Object() {"", "."})
        Me.ComboTipoPrecio.Location = New System.Drawing.Point(129, 4)
        Me.ComboTipoPrecio.Name = "ComboTipoPrecio"
        Me.ComboTipoPrecio.Size = New System.Drawing.Size(66, 21)
        Me.ComboTipoPrecio.TabIndex = 327
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 326
        Me.Label2.Text = "Precio Prederminado"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelIva
        '
        Me.PanelIva.BackColor = System.Drawing.Color.White
        Me.PanelIva.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelIva.Controls.Add(Me.RadioSinIva)
        Me.PanelIva.Controls.Add(Me.RadioFinal)
        Me.PanelIva.Location = New System.Drawing.Point(258, 1)
        Me.PanelIva.Name = "PanelIva"
        Me.PanelIva.Size = New System.Drawing.Size(130, 24)
        Me.PanelIva.TabIndex = 295
        '
        'RadioSinIva
        '
        Me.RadioSinIva.AutoSize = True
        Me.RadioSinIva.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RadioSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSinIva.Location = New System.Drawing.Point(6, 3)
        Me.RadioSinIva.Name = "RadioSinIva"
        Me.RadioSinIva.Size = New System.Drawing.Size(64, 17)
        Me.RadioSinIva.TabIndex = 12
        Me.RadioSinIva.Text = "Sin Iva"
        Me.RadioSinIva.UseVisualStyleBackColor = True
        '
        'RadioFinal
        '
        Me.RadioFinal.AutoSize = True
        Me.RadioFinal.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.RadioFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFinal.Location = New System.Drawing.Point(74, 3)
        Me.RadioFinal.Name = "RadioFinal"
        Me.RadioFinal.Size = New System.Drawing.Size(51, 17)
        Me.RadioFinal.TabIndex = 11
        Me.RadioFinal.Text = "Final"
        Me.RadioFinal.UseVisualStyleBackColor = True
        '
        'ButtonNuevaIgualCliente
        '
        Me.ButtonNuevaIgualCliente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevaIgualCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaIgualCliente.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevaIgualCliente.Location = New System.Drawing.Point(35, 655)
        Me.ButtonNuevaIgualCliente.Name = "ButtonNuevaIgualCliente"
        Me.ButtonNuevaIgualCliente.Size = New System.Drawing.Size(177, 25)
        Me.ButtonNuevaIgualCliente.TabIndex = 315
        Me.ButtonNuevaIgualCliente.TabStop = False
        Me.ButtonNuevaIgualCliente.Text = "Nuevo pedido  Igual Cliente"
        Me.ButtonNuevaIgualCliente.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevaIgualCliente.UseVisualStyleBackColor = True
        '
        'ButtonExportarExcelCorto
        '
        Me.ButtonExportarExcelCorto.AutoEllipsis = True
        Me.ButtonExportarExcelCorto.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcelCorto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcelCorto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcelCorto.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcelCorto.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcelCorto.Location = New System.Drawing.Point(219, 654)
        Me.ButtonExportarExcelCorto.Name = "ButtonExportarExcelCorto"
        Me.ButtonExportarExcelCorto.Size = New System.Drawing.Size(199, 25)
        Me.ButtonExportarExcelCorto.TabIndex = 316
        Me.ButtonExportarExcelCorto.Text = "Exportar a EXCEL Corto"
        Me.ButtonExportarExcelCorto.UseVisualStyleBackColor = False
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(18, 673)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(647, 29)
        Me.Label10.TabIndex = 317
        Me.Label10.Text = "Si el Cliente tiene lista de precios no se podra definir precios en el pedido."
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(459, 655)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(130, 30)
        Me.ButtonImprimir.TabIndex = 323
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonImportacionKrikos
        '
        Me.ButtonImportacionKrikos.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ButtonImportacionKrikos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImportacionKrikos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportacionKrikos.Location = New System.Drawing.Point(609, 664)
        Me.ButtonImportacionKrikos.Name = "ButtonImportacionKrikos"
        Me.ButtonImportacionKrikos.Size = New System.Drawing.Size(130, 25)
        Me.ButtonImportacionKrikos.TabIndex = 324
        Me.ButtonImportacionKrikos.Text = "Importacion Krikos"
        Me.ButtonImportacionKrikos.UseVisualStyleBackColor = False
        Me.ButtonImportacionKrikos.Visible = False
        '
        'UnPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(767, 703)
        Me.Controls.Add(Me.ButtonImportacionKrikos)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonExportarExcelCorto)
        Me.Controls.Add(Me.ButtonNuevaIgualCliente)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.PanelPrecio)
        Me.Controls.Add(Me.CheckCerrado)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.TextPedido)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TextPedidoCliente)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.FechaHasta)
        Me.Controls.Add(Me.ComboSucursal)
        Me.Controls.Add(Me.ComboCliente)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.FechaDesde)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonImportar)
        Me.Controls.Add(Me.ButtonBorrar)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.KeyPreview = True
        Me.Name = "UnPedido"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pedidos"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelPrecio.ResumeLayout(False)
        Me.PanelPrecio.PerformLayout()
        Me.PanelIva.ResumeLayout(False)
        Me.PanelIva.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents ButtonImportar As System.Windows.Forms.Button
    Friend WithEvents ButtonBorrar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FechaDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboSucursal As System.Windows.Forms.ComboBox
    Friend WithEvents ComboCliente As System.Windows.Forms.ComboBox
    Friend WithEvents FechaHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextPedidoCliente As System.Windows.Forms.TextBox
    Friend WithEvents TextPedido As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents PanelPrecio As System.Windows.Forms.Panel
    Friend WithEvents PanelIva As System.Windows.Forms.Panel
    Friend WithEvents RadioSinIva As System.Windows.Forms.RadioButton
    Friend WithEvents RadioFinal As System.Windows.Forms.RadioButton
    Friend WithEvents ButtonNuevaIgualCliente As System.Windows.Forms.Button
    Friend WithEvents ButtonExportarExcelCorto As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ComboTipoPrecio As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonImportacionKrikos As System.Windows.Forms.Button
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Entregada As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TipoPrecio As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Armador As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fletero As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
