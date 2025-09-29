<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeleccionaArticulosPedido
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Entregada = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.TextCliente = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.FechaEntrega = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.Grid2 = New System.Windows.Forms.DataGridView
        Me.ButtonPrimero2 = New System.Windows.Forms.Button
        Me.ButtonAnterior2 = New System.Windows.Forms.Button
        Me.ButtonUltimo2 = New System.Windows.Forms.Button
        Me.ButtonPosterior2 = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.PanelMarcarTodos = New System.Windows.Forms.Panel
        Me.ButtonDesmarcarTodos = New System.Windows.Forms.Button
        Me.ButtonMarcarTodos = New System.Windows.Forms.Button
        Me.ButtonImportar = New System.Windows.Forms.Button
        Me.Sel2 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Pedido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PedidoCliente = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Sucursal = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.FechaEnt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaHas = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelMarcarTodos.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Sel, Me.Articulo, Me.Cantidad, Me.Entregada, Me.Saldo, Me.Precio})
        Me.Grid.Location = New System.Drawing.Point(97, 281)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Size = New System.Drawing.Size(697, 381)
        Me.Grid.TabIndex = 317
        '
        'Sel
        '
        Me.Sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 25
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 25
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 250
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Width = 250
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle2
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MaxInputLength = 10
        Me.Cantidad.MinimumWidth = 90
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Width = 90
        '
        'Entregada
        '
        Me.Entregada.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Entregada.DataPropertyName = "Entregada"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Entregada.DefaultCellStyle = DataGridViewCellStyle3
        Me.Entregada.HeaderText = "Entregado"
        Me.Entregada.MinimumWidth = 90
        Me.Entregada.Name = "Entregada"
        Me.Entregada.ReadOnly = True
        Me.Entregada.Width = 90
        '
        'Saldo
        '
        Me.Saldo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle4
        Me.Saldo.HeaderText = "Saldo"
        Me.Saldo.MinimumWidth = 90
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        Me.Saldo.Width = 90
        '
        'Precio
        '
        Me.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle5
        Me.Precio.HeaderText = "Precio"
        Me.Precio.MaxInputLength = 10
        Me.Precio.MinimumWidth = 90
        Me.Precio.Name = "Precio"
        Me.Precio.Width = 90
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(97, 669)
        Me.ButtonPrimero.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(32, 22)
        Me.ButtonPrimero.TabIndex = 314
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(130, 669)
        Me.ButtonAnterior.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(32, 22)
        Me.ButtonAnterior.TabIndex = 315
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(197, 669)
        Me.ButtonUltimo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(32, 22)
        Me.ButtonUltimo.TabIndex = 313
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(164, 669)
        Me.ButtonPosterior.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(32, 22)
        Me.ButtonPosterior.TabIndex = 316
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(380, 673)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 318
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'TextCliente
        '
        Me.TextCliente.BackColor = System.Drawing.Color.White
        Me.TextCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCliente.Location = New System.Drawing.Point(156, 4)
        Me.TextCliente.Name = "TextCliente"
        Me.TextCliente.ReadOnly = True
        Me.TextCliente.Size = New System.Drawing.Size(297, 20)
        Me.TextCliente.TabIndex = 319
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(94, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 320
        Me.Label1.Text = "Cliente"
        '
        'FechaEntrega
        '
        Me.FechaEntrega.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntrega.CustomFormat = "dd/MM/yyyy"
        Me.FechaEntrega.Enabled = False
        Me.FechaEntrega.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntrega.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaEntrega.Location = New System.Drawing.Point(699, 4)
        Me.FechaEntrega.Name = "FechaEntrega"
        Me.FechaEntrega.Size = New System.Drawing.Size(92, 20)
        Me.FechaEntrega.TabIndex = 321
        Me.FechaEntrega.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(602, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(90, 13)
        Me.Label2.TabIndex = 322
        Me.Label2.Text = "Fecha Entrega"
        '
        'Grid2
        '
        Me.Grid2.AllowUserToAddRows = False
        Me.Grid2.AllowUserToDeleteRows = False
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Grid2.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle7
        Me.Grid2.BackgroundColor = System.Drawing.Color.White
        Me.Grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Sel2, Me.Pedido, Me.PedidoCliente, Me.Sucursal, Me.FechaEnt, Me.FechaHas})
        Me.Grid2.Location = New System.Drawing.Point(97, 32)
        Me.Grid2.Name = "Grid2"
        Me.Grid2.RowHeadersWidth = 20
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Grid2.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid2.Size = New System.Drawing.Size(697, 177)
        Me.Grid2.TabIndex = 323
        '
        'ButtonPrimero2
        '
        Me.ButtonPrimero2.AutoEllipsis = True
        Me.ButtonPrimero2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero2.Location = New System.Drawing.Point(98, 213)
        Me.ButtonPrimero2.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPrimero2.Name = "ButtonPrimero2"
        Me.ButtonPrimero2.Size = New System.Drawing.Size(32, 20)
        Me.ButtonPrimero2.TabIndex = 325
        Me.ButtonPrimero2.Text = "<<"
        Me.ButtonPrimero2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero2.UseVisualStyleBackColor = False
        '
        'ButtonAnterior2
        '
        Me.ButtonAnterior2.AutoEllipsis = True
        Me.ButtonAnterior2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior2.Location = New System.Drawing.Point(131, 213)
        Me.ButtonAnterior2.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAnterior2.Name = "ButtonAnterior2"
        Me.ButtonAnterior2.Size = New System.Drawing.Size(32, 20)
        Me.ButtonAnterior2.TabIndex = 326
        Me.ButtonAnterior2.Text = "<"
        Me.ButtonAnterior2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior2.UseVisualStyleBackColor = False
        '
        'ButtonUltimo2
        '
        Me.ButtonUltimo2.AutoEllipsis = True
        Me.ButtonUltimo2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo2.Location = New System.Drawing.Point(198, 213)
        Me.ButtonUltimo2.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonUltimo2.Name = "ButtonUltimo2"
        Me.ButtonUltimo2.Size = New System.Drawing.Size(32, 20)
        Me.ButtonUltimo2.TabIndex = 324
        Me.ButtonUltimo2.Text = ">>"
        Me.ButtonUltimo2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonUltimo2.UseVisualStyleBackColor = False
        '
        'ButtonPosterior2
        '
        Me.ButtonPosterior2.AutoEllipsis = True
        Me.ButtonPosterior2.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior2.Location = New System.Drawing.Point(165, 213)
        Me.ButtonPosterior2.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPosterior2.Name = "ButtonPosterior2"
        Me.ButtonPosterior2.Size = New System.Drawing.Size(32, 20)
        Me.ButtonPosterior2.TabIndex = 327
        Me.ButtonPosterior2.Text = ">"
        Me.ButtonPosterior2.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.ButtonPosterior2.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(423, 262)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(111, 13)
        Me.Label3.TabIndex = 328
        Me.Label3.Text = "Detalle del Pedido"
        '
        'PanelMarcarTodos
        '
        Me.PanelMarcarTodos.Controls.Add(Me.ButtonDesmarcarTodos)
        Me.PanelMarcarTodos.Controls.Add(Me.ButtonMarcarTodos)
        Me.PanelMarcarTodos.Location = New System.Drawing.Point(98, 246)
        Me.PanelMarcarTodos.Name = "PanelMarcarTodos"
        Me.PanelMarcarTodos.Size = New System.Drawing.Size(219, 33)
        Me.PanelMarcarTodos.TabIndex = 329
        '
        'ButtonDesmarcarTodos
        '
        Me.ButtonDesmarcarTodos.Location = New System.Drawing.Point(109, 6)
        Me.ButtonDesmarcarTodos.Name = "ButtonDesmarcarTodos"
        Me.ButtonDesmarcarTodos.Size = New System.Drawing.Size(102, 21)
        Me.ButtonDesmarcarTodos.TabIndex = 140
        Me.ButtonDesmarcarTodos.Text = "Des-Marcar"
        Me.ButtonDesmarcarTodos.UseVisualStyleBackColor = True
        '
        'ButtonMarcarTodos
        '
        Me.ButtonMarcarTodos.Location = New System.Drawing.Point(-2, 6)
        Me.ButtonMarcarTodos.Name = "ButtonMarcarTodos"
        Me.ButtonMarcarTodos.Size = New System.Drawing.Size(102, 21)
        Me.ButtonMarcarTodos.TabIndex = 139
        Me.ButtonMarcarTodos.Text = "Marcar Todos"
        Me.ButtonMarcarTodos.UseVisualStyleBackColor = True
        '
        'ButtonImportar
        '
        Me.ButtonImportar.BackColor = System.Drawing.Color.GreenYellow
        Me.ButtonImportar.Location = New System.Drawing.Point(644, 250)
        Me.ButtonImportar.Name = "ButtonImportar"
        Me.ButtonImportar.Size = New System.Drawing.Size(147, 25)
        Me.ButtonImportar.TabIndex = 330
        Me.ButtonImportar.Text = "Importar Art. de un Remito"
        Me.ButtonImportar.UseVisualStyleBackColor = False
        '
        'Sel2
        '
        Me.Sel2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel2.HeaderText = ""
        Me.Sel2.MinimumWidth = 25
        Me.Sel2.Name = "Sel2"
        Me.Sel2.Width = 25
        '
        'Pedido
        '
        Me.Pedido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Pedido.DataPropertyName = "Pedido"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Pedido.DefaultCellStyle = DataGridViewCellStyle8
        Me.Pedido.HeaderText = "Pedido"
        Me.Pedido.MinimumWidth = 100
        Me.Pedido.Name = "Pedido"
        Me.Pedido.ReadOnly = True
        '
        'PedidoCliente
        '
        Me.PedidoCliente.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PedidoCliente.DataPropertyName = "PedidoCliente"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.PedidoCliente.DefaultCellStyle = DataGridViewCellStyle9
        Me.PedidoCliente.HeaderText = "Pedido Cliente"
        Me.PedidoCliente.MinimumWidth = 200
        Me.PedidoCliente.Name = "PedidoCliente"
        Me.PedidoCliente.ReadOnly = True
        Me.PedidoCliente.Width = 200
        '
        'Sucursal
        '
        Me.Sucursal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sucursal.DataPropertyName = "Sucursal"
        Me.Sucursal.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Sucursal.HeaderText = "Sucursal"
        Me.Sucursal.MinimumWidth = 100
        Me.Sucursal.Name = "Sucursal"
        Me.Sucursal.ReadOnly = True
        '
        'FechaEnt
        '
        Me.FechaEnt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.FechaEnt.DataPropertyName = "FechaEntregaDesde"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaEnt.DefaultCellStyle = DataGridViewCellStyle10
        Me.FechaEnt.HeaderText = "Entrega Desde"
        Me.FechaEnt.MinimumWidth = 110
        Me.FechaEnt.Name = "FechaEnt"
        Me.FechaEnt.ReadOnly = True
        Me.FechaEnt.Width = 110
        '
        'FechaHas
        '
        Me.FechaHas.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.FechaHas.DataPropertyName = "FechaEntregaHasta"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaHas.DefaultCellStyle = DataGridViewCellStyle11
        Me.FechaHas.HeaderText = "Entrega Hasta"
        Me.FechaHas.MinimumWidth = 110
        Me.FechaHas.Name = "FechaHas"
        Me.FechaHas.ReadOnly = True
        Me.FechaHas.Width = 110
        '
        'SeleccionaArticulosPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(873, 706)
        Me.Controls.Add(Me.ButtonImportar)
        Me.Controls.Add(Me.PanelMarcarTodos)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ButtonPrimero2)
        Me.Controls.Add(Me.ButtonAnterior2)
        Me.Controls.Add(Me.ButtonUltimo2)
        Me.Controls.Add(Me.ButtonPosterior2)
        Me.Controls.Add(Me.Grid2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.FechaEntrega)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextCliente)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Name = "SeleccionaArticulosPedido"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Seleccionar Articulos Pedido"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelMarcarTodos.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents FechaEntrega As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Grid2 As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonPrimero2 As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior2 As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo2 As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior2 As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Entregada As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PanelMarcarTodos As System.Windows.Forms.Panel
    Friend WithEvents ButtonDesmarcarTodos As System.Windows.Forms.Button
    Friend WithEvents ButtonMarcarTodos As System.Windows.Forms.Button
    Friend WithEvents ButtonImportar As System.Windows.Forms.Button
    Friend WithEvents Sel2 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Pedido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PedidoCliente As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Sucursal As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents FechaEnt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaHas As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
