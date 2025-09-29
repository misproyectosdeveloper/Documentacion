<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnReciboReposicion
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
        Dim DataGridViewCellStyle25 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle26 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle27 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle28 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle29 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle30 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle31 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle32 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnReciboReposicion))
        Me.MaskedNota = New System.Windows.Forms.MaskedTextBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ButtonComprobantesAImputar = New System.Windows.Forms.Button
        Me.Panel9 = New System.Windows.Forms.Panel
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextSaldo = New System.Windows.Forms.TextBox
        Me.TextTotalFacturas = New System.Windows.Forms.TextBox
        Me.GridCompro = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Tipo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.TipoVisible = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Comprobante1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Recibo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaCompro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImporteCompro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Moneda = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Saldo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Asignado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonTextoRecibo = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.LabelPuntoDeVenta = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextTotalOrden = New System.Windows.Forms.TextBox
        Me.LabelImporteOrden = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.LabelTipoNota = New System.Windows.Forms.Label
        Me.Reposicion = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.LabelCaja = New System.Windows.Forms.Label
        Me.Label13 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ButtonEliminarTodo = New System.Windows.Forms.Button
        Me.ButtonMediosDePago = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextTotalRecibo = New System.Windows.Forms.TextBox
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.CheckSecos = New System.Windows.Forms.CheckBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.TextNombreFondoFijo = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1.SuspendLayout()
        Me.Panel9.SuspendLayout()
        CType(Me.GridCompro, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'MaskedNota
        '
        Me.MaskedNota.BackColor = System.Drawing.Color.White
        Me.MaskedNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedNota.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedNota.Location = New System.Drawing.Point(88, 3)
        Me.MaskedNota.Mask = "0000-00000000"
        Me.MaskedNota.Name = "MaskedNota"
        Me.MaskedNota.ReadOnly = True
        Me.MaskedNota.Size = New System.Drawing.Size(114, 21)
        Me.MaskedNota.TabIndex = 1
        Me.MaskedNota.TabStop = False
        Me.MaskedNota.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGreen
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ButtonComprobantesAImputar)
        Me.Panel1.Controls.Add(Me.Panel9)
        Me.Panel1.Controls.Add(Me.GridCompro)
        Me.Panel1.Controls.Add(Me.ButtonTextoRecibo)
        Me.Panel1.Location = New System.Drawing.Point(23, 192)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1093, 336)
        Me.Panel1.TabIndex = 328
        '
        'ButtonComprobantesAImputar
        '
        Me.ButtonComprobantesAImputar.BackColor = System.Drawing.Color.LightGray
        Me.ButtonComprobantesAImputar.FlatAppearance.BorderSize = 0
        Me.ButtonComprobantesAImputar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonComprobantesAImputar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonComprobantesAImputar.Location = New System.Drawing.Point(431, 25)
        Me.ButtonComprobantesAImputar.Name = "ButtonComprobantesAImputar"
        Me.ButtonComprobantesAImputar.Size = New System.Drawing.Size(258, 27)
        Me.ButtonComprobantesAImputar.TabIndex = 1018
        Me.ButtonComprobantesAImputar.TabStop = False
        Me.ButtonComprobantesAImputar.Text = "Informar Comprobantes A Imputar"
        Me.ButtonComprobantesAImputar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonComprobantesAImputar.UseVisualStyleBackColor = False
        '
        'Panel9
        '
        Me.Panel9.Controls.Add(Me.Label2)
        Me.Panel9.Controls.Add(Me.Label5)
        Me.Panel9.Controls.Add(Me.TextSaldo)
        Me.Panel9.Controls.Add(Me.TextTotalFacturas)
        Me.Panel9.Location = New System.Drawing.Point(216, 250)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(754, 39)
        Me.Panel9.TabIndex = 1017
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(485, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(112, 16)
        Me.Label2.TabIndex = 1014
        Me.Label2.Text = "Total Imputado"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(129, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 16)
        Me.Label5.TabIndex = 1013
        Me.Label5.Text = "Total a Cuenta Corriente"
        '
        'TextSaldo
        '
        Me.TextSaldo.BackColor = System.Drawing.Color.White
        Me.TextSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldo.Location = New System.Drawing.Point(323, 9)
        Me.TextSaldo.MaxLength = 20
        Me.TextSaldo.Name = "TextSaldo"
        Me.TextSaldo.ReadOnly = True
        Me.TextSaldo.Size = New System.Drawing.Size(108, 20)
        Me.TextSaldo.TabIndex = 244
        Me.TextSaldo.TabStop = False
        Me.TextSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextTotalFacturas
        '
        Me.TextTotalFacturas.BackColor = System.Drawing.Color.White
        Me.TextTotalFacturas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalFacturas.Location = New System.Drawing.Point(608, 9)
        Me.TextTotalFacturas.MaxLength = 20
        Me.TextTotalFacturas.Name = "TextTotalFacturas"
        Me.TextTotalFacturas.ReadOnly = True
        Me.TextTotalFacturas.Size = New System.Drawing.Size(104, 20)
        Me.TextTotalFacturas.TabIndex = 0
        Me.TextTotalFacturas.TabStop = False
        Me.TextTotalFacturas.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GridCompro
        '
        Me.GridCompro.AllowUserToAddRows = False
        Me.GridCompro.AllowUserToDeleteRows = False
        Me.GridCompro.BackgroundColor = System.Drawing.Color.White
        Me.GridCompro.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.GridCompro.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Candado, Me.Tipo, Me.TipoVisible, Me.Comprobante1, Me.Recibo, Me.Comentario, Me.FechaCompro, Me.ImporteCompro, Me.Moneda, Me.Saldo, Me.Asignado})
        Me.GridCompro.Location = New System.Drawing.Point(155, 59)
        Me.GridCompro.Name = "GridCompro"
        Me.GridCompro.RowHeadersWidth = 20
        Me.GridCompro.Size = New System.Drawing.Size(813, 188)
        Me.GridCompro.TabIndex = 1016
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.ReadOnly = True
        Me.Operacion.Visible = False
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.ReadOnly = True
        Me.Candado.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Candado.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Candado.Width = 30
        '
        'Tipo
        '
        Me.Tipo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Tipo.DataPropertyName = "Tipo"
        DataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Tipo.DefaultCellStyle = DataGridViewCellStyle25
        Me.Tipo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Tipo.HeaderText = "Tipo No Visible"
        Me.Tipo.MinimumWidth = 85
        Me.Tipo.Name = "Tipo"
        Me.Tipo.ReadOnly = True
        Me.Tipo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Tipo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Tipo.Visible = False
        Me.Tipo.Width = 85
        '
        'TipoVisible
        '
        Me.TipoVisible.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoVisible.DataPropertyName = "TipoVisible"
        DataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft
        Me.TipoVisible.DefaultCellStyle = DataGridViewCellStyle26
        Me.TipoVisible.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.TipoVisible.HeaderText = "Tipo"
        Me.TipoVisible.MinimumWidth = 85
        Me.TipoVisible.Name = "TipoVisible"
        Me.TipoVisible.ReadOnly = True
        Me.TipoVisible.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.TipoVisible.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.TipoVisible.Width = 85
        '
        'Comprobante1
        '
        Me.Comprobante1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comprobante1.DataPropertyName = "Comprobante"
        DataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Comprobante1.DefaultCellStyle = DataGridViewCellStyle27
        Me.Comprobante1.HeaderText = "Comprobante"
        Me.Comprobante1.MinimumWidth = 90
        Me.Comprobante1.Name = "Comprobante1"
        Me.Comprobante1.ReadOnly = True
        Me.Comprobante1.Visible = False
        Me.Comprobante1.Width = 90
        '
        'Recibo
        '
        Me.Recibo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Recibo.DataPropertyName = "Recibo"
        Me.Recibo.HeaderText = "Comprobante"
        Me.Recibo.MinimumWidth = 90
        Me.Recibo.Name = "Recibo"
        Me.Recibo.ReadOnly = True
        Me.Recibo.Width = 90
        '
        'Comentario
        '
        Me.Comentario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Comentario.DataPropertyName = "Comentario"
        DataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Comentario.DefaultCellStyle = DataGridViewCellStyle28
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.MinimumWidth = 90
        Me.Comentario.Name = "Comentario"
        Me.Comentario.ReadOnly = True
        Me.Comentario.Width = 90
        '
        'FechaCompro
        '
        Me.FechaCompro.DataPropertyName = "Fecha"
        DataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaCompro.DefaultCellStyle = DataGridViewCellStyle29
        Me.FechaCompro.HeaderText = "Fecha"
        Me.FechaCompro.MinimumWidth = 70
        Me.FechaCompro.Name = "FechaCompro"
        Me.FechaCompro.ReadOnly = True
        Me.FechaCompro.Width = 70
        '
        'ImporteCompro
        '
        Me.ImporteCompro.DataPropertyName = "Importe"
        DataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ImporteCompro.DefaultCellStyle = DataGridViewCellStyle30
        Me.ImporteCompro.HeaderText = "Importe"
        Me.ImporteCompro.Name = "ImporteCompro"
        Me.ImporteCompro.ReadOnly = True
        '
        'Moneda
        '
        Me.Moneda.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Moneda.DataPropertyName = "Moneda"
        Me.Moneda.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Moneda.HeaderText = "Moneda"
        Me.Moneda.MinimumWidth = 80
        Me.Moneda.Name = "Moneda"
        Me.Moneda.ReadOnly = True
        Me.Moneda.Width = 80
        '
        'Saldo
        '
        Me.Saldo.DataPropertyName = "Saldo"
        DataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Saldo.DefaultCellStyle = DataGridViewCellStyle31
        Me.Saldo.HeaderText = "Saldo"
        Me.Saldo.Name = "Saldo"
        Me.Saldo.ReadOnly = True
        '
        'Asignado
        '
        Me.Asignado.DataPropertyName = "Asignado"
        DataGridViewCellStyle32.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Asignado.DefaultCellStyle = DataGridViewCellStyle32
        Me.Asignado.HeaderText = "Imp. Asignado"
        Me.Asignado.MaxInputLength = 8
        Me.Asignado.Name = "Asignado"
        '
        'ButtonTextoRecibo
        '
        Me.ButtonTextoRecibo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonTextoRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonTextoRecibo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonTextoRecibo.Location = New System.Drawing.Point(2, 293)
        Me.ButtonTextoRecibo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonTextoRecibo.Name = "ButtonTextoRecibo"
        Me.ButtonTextoRecibo.Size = New System.Drawing.Size(140, 30)
        Me.ButtonTextoRecibo.TabIndex = 246
        Me.ButtonTextoRecibo.TabStop = False
        Me.ButtonTextoRecibo.Text = "Texto Para Impresión"
        Me.ButtonTextoRecibo.UseVisualStyleBackColor = False
        Me.ButtonTextoRecibo.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 34)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 13)
        Me.Label1.TabIndex = 238
        Me.Label1.Text = "Comentario"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelPuntoDeVenta
        '
        Me.LabelPuntoDeVenta.AutoSize = True
        Me.LabelPuntoDeVenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelPuntoDeVenta.Location = New System.Drawing.Point(653, 20)
        Me.LabelPuntoDeVenta.Name = "LabelPuntoDeVenta"
        Me.LabelPuntoDeVenta.Size = New System.Drawing.Size(104, 15)
        Me.LabelPuntoDeVenta.TabIndex = 340
        Me.LabelPuntoDeVenta.Text = "Punto de Venta"
        Me.LabelPuntoDeVenta.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.LightGreen
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextTotalOrden)
        Me.Panel3.Controls.Add(Me.LabelImporteOrden)
        Me.Panel3.Location = New System.Drawing.Point(23, 96)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1093, 28)
        Me.Panel3.TabIndex = 326
        '
        'TextTotalOrden
        '
        Me.TextTotalOrden.BackColor = System.Drawing.Color.White
        Me.TextTotalOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalOrden.Location = New System.Drawing.Point(528, 2)
        Me.TextTotalOrden.MaxLength = 10
        Me.TextTotalOrden.Name = "TextTotalOrden"
        Me.TextTotalOrden.Size = New System.Drawing.Size(95, 20)
        Me.TextTotalOrden.TabIndex = 7
        Me.TextTotalOrden.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelImporteOrden
        '
        Me.LabelImporteOrden.AutoSize = True
        Me.LabelImporteOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelImporteOrden.Location = New System.Drawing.Point(408, 7)
        Me.LabelImporteOrden.Name = "LabelImporteOrden"
        Me.LabelImporteOrden.Size = New System.Drawing.Size(116, 13)
        Me.LabelImporteOrden.TabIndex = 244
        Me.LabelImporteOrden.Text = "Importe Reposición"
        Me.LabelImporteOrden.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(1041, 6)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 137
        Me.PictureCandado.TabStop = False
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(88, 28)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(292, 20)
        Me.TextComentario.TabIndex = 237
        Me.TextComentario.TabStop = False
        '
        'LabelTipoNota
        '
        Me.LabelTipoNota.AutoSize = True
        Me.LabelTipoNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTipoNota.Location = New System.Drawing.Point(21, 22)
        Me.LabelTipoNota.Name = "LabelTipoNota"
        Me.LabelTipoNota.Size = New System.Drawing.Size(63, 13)
        Me.LabelTipoNota.TabIndex = 335
        Me.LabelTipoNota.Text = "Tipo Nota"
        Me.LabelTipoNota.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Reposicion
        '
        Me.Reposicion.AutoSize = True
        Me.Reposicion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Reposicion.Location = New System.Drawing.Point(6, 8)
        Me.Reposicion.Name = "Reposicion"
        Me.Reposicion.Size = New System.Drawing.Size(70, 13)
        Me.Reposicion.TabIndex = 244
        Me.Reposicion.Text = "Reposición"
        Me.Reposicion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(954, 20)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 333
        Me.Label8.Text = "Estado"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(23, 542)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(149, 36)
        Me.ButtonNuevo.TabIndex = 342
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nueva Nota"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprimir.Location = New System.Drawing.Point(258, 542)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(149, 36)
        Me.ButtonImprimir.TabIndex = 341
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = ""
        Me.DataGridViewImageColumn1.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.MinimumWidth = 30
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 30
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(740, 543)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(149, 36)
        Me.ButtonAsientoContable.TabIndex = 338
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(965, 543)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(149, 36)
        Me.ButtonAceptar.TabIndex = 329
        Me.ButtonAceptar.Text = "Graba Nota "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(507, 543)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(149, 36)
        Me.ButtonAnula.TabIndex = 334
        Me.ButtonAnula.TabStop = False
        Me.ButtonAnula.Text = "Anular Nota"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'LabelCaja
        '
        Me.LabelCaja.AutoSize = True
        Me.LabelCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelCaja.Location = New System.Drawing.Point(385, 20)
        Me.LabelCaja.Name = "LabelCaja"
        Me.LabelCaja.Size = New System.Drawing.Size(47, 18)
        Me.LabelCaja.TabIndex = 331
        Me.LabelCaja.Text = "Caja "
        Me.LabelCaja.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(334, 20)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 15)
        Me.Label13.TabIndex = 330
        Me.Label13.Text = "Caja "
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightGreen
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.ButtonEliminarTodo)
        Me.Panel2.Controls.Add(Me.ButtonMediosDePago)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.TextTotalRecibo)
        Me.Panel2.Location = New System.Drawing.Point(23, 125)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1093, 66)
        Me.Panel2.TabIndex = 327
        '
        'ButtonEliminarTodo
        '
        Me.ButtonEliminarTodo.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarTodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarTodo.Location = New System.Drawing.Point(347, 40)
        Me.ButtonEliminarTodo.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarTodo.Name = "ButtonEliminarTodo"
        Me.ButtonEliminarTodo.Size = New System.Drawing.Size(120, 20)
        Me.ButtonEliminarTodo.TabIndex = 1012
        Me.ButtonEliminarTodo.TabStop = False
        Me.ButtonEliminarTodo.Text = "Borrar Conceptos"
        Me.ButtonEliminarTodo.UseVisualStyleBackColor = False
        '
        'ButtonMediosDePago
        '
        Me.ButtonMediosDePago.BackColor = System.Drawing.Color.LightGray
        Me.ButtonMediosDePago.FlatAppearance.BorderSize = 0
        Me.ButtonMediosDePago.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonMediosDePago.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonMediosDePago.Location = New System.Drawing.Point(347, 3)
        Me.ButtonMediosDePago.Name = "ButtonMediosDePago"
        Me.ButtonMediosDePago.Size = New System.Drawing.Size(414, 36)
        Me.ButtonMediosDePago.TabIndex = 1009
        Me.ButtonMediosDePago.TabStop = False
        Me.ButtonMediosDePago.Text = "Conceptos de Pagos/Cobranza"
        Me.ButtonMediosDePago.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonMediosDePago.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(562, 46)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(80, 13)
        Me.Label3.TabIndex = 244
        Me.Label3.Text = "Total Recibo"
        '
        'TextTotalRecibo
        '
        Me.TextTotalRecibo.BackColor = System.Drawing.Color.White
        Me.TextTotalRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalRecibo.Location = New System.Drawing.Point(647, 42)
        Me.TextTotalRecibo.MaxLength = 20
        Me.TextTotalRecibo.Name = "TextTotalRecibo"
        Me.TextTotalRecibo.ReadOnly = True
        Me.TextTotalRecibo.Size = New System.Drawing.Size(111, 20)
        Me.TextTotalRecibo.TabIndex = 0
        Me.TextTotalRecibo.TabStop = False
        Me.TextTotalRecibo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboEstado
        '
        Me.ComboEstado.BackColor = System.Drawing.Color.White
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(1016, 15)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 332
        Me.ComboEstado.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(861, 8)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(42, 13)
        Me.Label15.TabIndex = 33
        Me.Label15.Text = "Fecha"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(910, 4)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(103, 20)
        Me.DateTime1.TabIndex = 2
        '
        'CheckSecos
        '
        Me.CheckSecos.AutoSize = True
        Me.CheckSecos.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckSecos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSecos.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckSecos.Location = New System.Drawing.Point(475, 28)
        Me.CheckSecos.Name = "CheckSecos"
        Me.CheckSecos.Size = New System.Drawing.Size(85, 19)
        Me.CheckSecos.TabIndex = 1008
        Me.CheckSecos.TabStop = False
        Me.CheckSecos.Text = "Es Secos"
        Me.CheckSecos.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.LightGreen
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.TextNombreFondoFijo)
        Me.Panel4.Controls.Add(Me.Label18)
        Me.Panel4.Controls.Add(Me.TextNumero)
        Me.Panel4.Controls.Add(Me.Label23)
        Me.Panel4.Controls.Add(Me.CheckSecos)
        Me.Panel4.Controls.Add(Me.Reposicion)
        Me.Panel4.Controls.Add(Me.MaskedNota)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.TextComentario)
        Me.Panel4.Controls.Add(Me.PictureCandado)
        Me.Panel4.Controls.Add(Me.Label15)
        Me.Panel4.Controls.Add(Me.DateTime1)
        Me.Panel4.Location = New System.Drawing.Point(24, 42)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1092, 53)
        Me.Panel4.TabIndex = 325
        '
        'TextNombreFondoFijo
        '
        Me.TextNombreFondoFijo.BackColor = System.Drawing.Color.White
        Me.TextNombreFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreFondoFijo.Location = New System.Drawing.Point(436, 4)
        Me.TextNombreFondoFijo.MaxLength = 30
        Me.TextNombreFondoFijo.Name = "TextNombreFondoFijo"
        Me.TextNombreFondoFijo.ReadOnly = True
        Me.TextNombreFondoFijo.Size = New System.Drawing.Size(188, 20)
        Me.TextNombreFondoFijo.TabIndex = 1021
        Me.TextNombreFondoFijo.TabStop = False
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(649, 8)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(50, 13)
        Me.Label18.TabIndex = 1020
        Me.Label18.Text = "Numero"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TextNumero
        '
        Me.TextNumero.BackColor = System.Drawing.Color.White
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(711, 4)
        Me.TextNumero.MaxLength = 5
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.ReadOnly = True
        Me.TextNumero.Size = New System.Drawing.Size(103, 20)
        Me.TextNumero.TabIndex = 1019
        Me.TextNumero.TabStop = False
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label23
        '
        Me.Label23.AutoSize = True
        Me.Label23.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label23.Location = New System.Drawing.Point(295, 8)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(128, 13)
        Me.Label23.TabIndex = 1018
        Me.Label23.Text = "Proveedor Fondo Fijo"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'UnReciboReposicion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(1133, 596)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LabelPuntoDeVenta)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.LabelTipoNota)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.LabelCaja)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Panel4)
        Me.Name = "UnReciboReposicion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Reposición"
        Me.Panel1.ResumeLayout(False)
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        CType(Me.GridCompro, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MaskedNota As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonTextoRecibo As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelPuntoDeVenta As System.Windows.Forms.Label
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextTotalOrden As System.Windows.Forms.TextBox
    Friend WithEvents LabelImporteOrden As System.Windows.Forms.Label
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents LabelTipoNota As System.Windows.Forms.Label
    Friend WithEvents Reposicion As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents LabelCaja As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarTodo As System.Windows.Forms.Button
    Friend WithEvents ButtonMediosDePago As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextTotalRecibo As System.Windows.Forms.TextBox
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents CheckSecos As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextNombreFondoFijo As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents GridCompro As System.Windows.Forms.DataGridView
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Tipo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents TipoVisible As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Comprobante1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Recibo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaCompro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImporteCompro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Moneda As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Saldo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Asignado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextSaldo As System.Windows.Forms.TextBox
    Friend WithEvents TextTotalFacturas As System.Windows.Forms.TextBox
    Friend WithEvents ButtonComprobantesAImputar As System.Windows.Forms.Button
End Class
