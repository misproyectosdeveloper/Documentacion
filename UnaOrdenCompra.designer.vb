<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaOrdenCompra
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
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaOrdenCompra))
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.TextTotal = New System.Windows.Forms.TextBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.RadioSinIva = New System.Windows.Forms.RadioButton
        Me.RadioFinal = New System.Windows.Forms.RadioButton
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.PrecioB = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Iva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Neto = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MontoIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TotalArticulo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Recibido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Consumido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonExcel = New System.Windows.Forms.Button
        Me.PanelCabeza = New System.Windows.Forms.Panel
        Me.TextSaldo = New System.Windows.Forms.TextBox
        Me.Comboproveedor = New System.Windows.Forms.ComboBox
        Me.Label25 = New System.Windows.Forms.Label
        Me.TextImporteWW = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.FechaEntregaHasta = New System.Windows.Forms.DateTimePicker
        Me.FechaEntregaDesde = New System.Windows.Forms.DateTimePicker
        Me.Label6 = New System.Windows.Forms.Label
        Me.ButtonDatosEmisor = New System.Windows.Forms.Button
        Me.ComboPais = New System.Windows.Forms.ComboBox
        Me.ComboTipoIva = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextCuit = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextProvincia = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextPlazoPago = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.FechaEmision = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.TextOrden = New System.Windows.Forms.TextBox
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonAgregar = New System.Windows.Forms.Button
        Me.LabelOrdenFacturada = New System.Windows.Forms.Label
        Me.ButtonImprime = New System.Windows.Forms.Button
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelCabeza.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.TextTotal)
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Controls.Add(Me.ButtonEliminarLinea)
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Location = New System.Drawing.Point(36, 130)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(887, 472)
        Me.Panel2.TabIndex = 173
        '
        'TextTotal
        '
        Me.TextTotal.BackColor = System.Drawing.Color.White
        Me.TextTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotal.Location = New System.Drawing.Point(728, 446)
        Me.TextTotal.MaxLength = 20
        Me.TextTotal.Name = "TextTotal"
        Me.TextTotal.ReadOnly = True
        Me.TextTotal.Size = New System.Drawing.Size(121, 20)
        Me.TextTotal.TabIndex = 140
        Me.TextTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.RadioSinIva)
        Me.Panel5.Controls.Add(Me.RadioFinal)
        Me.Panel5.Location = New System.Drawing.Point(15, -4)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(183, 24)
        Me.Panel5.TabIndex = 139
        '
        'RadioSinIva
        '
        Me.RadioSinIva.AutoSize = True
        Me.RadioSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSinIva.Location = New System.Drawing.Point(69, 4)
        Me.RadioSinIva.Name = "RadioSinIva"
        Me.RadioSinIva.Size = New System.Drawing.Size(58, 17)
        Me.RadioSinIva.TabIndex = 4
        Me.RadioSinIva.Text = "Sin Iva"
        Me.RadioSinIva.UseVisualStyleBackColor = True
        '
        'RadioFinal
        '
        Me.RadioFinal.AutoSize = True
        Me.RadioFinal.Checked = True
        Me.RadioFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFinal.Location = New System.Drawing.Point(6, 4)
        Me.RadioFinal.Name = "RadioFinal"
        Me.RadioFinal.Size = New System.Drawing.Size(47, 17)
        Me.RadioFinal.TabIndex = 3
        Me.RadioFinal.TabStop = True
        Me.RadioFinal.Text = "Final"
        Me.RadioFinal.UseVisualStyleBackColor = True
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(12, 446)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(141, 20)
        Me.ButtonEliminarLinea.TabIndex = 0
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PrecioB, Me.Articulo, Me.Cantidad, Me.Precio, Me.Iva, Me.Neto, Me.MontoIva, Me.TotalArticulo, Me.Recibido, Me.Consumido})
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.DodgerBlue
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle10
        Me.Grid.Location = New System.Drawing.Point(14, 21)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle11
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(857, 422)
        Me.Grid.TabIndex = 10
        '
        'PrecioB
        '
        Me.PrecioB.DataPropertyName = "PrecioB"
        Me.PrecioB.HeaderText = "PrecioB"
        Me.PrecioB.Name = "PrecioB"
        Me.PrecioB.Visible = False
        Me.PrecioB.Width = 69
        '
        'Articulo
        '
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle2
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 250
        Me.Articulo.Name = "Articulo"
        Me.Articulo.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Articulo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Articulo.Width = 250
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle3
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.Width = 80
        '
        'Precio
        '
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle4
        Me.Precio.HeaderText = "Precio"
        Me.Precio.Name = "Precio"
        Me.Precio.Width = 62
        '
        'Iva
        '
        Me.Iva.DataPropertyName = "Iva"
        Me.Iva.HeaderText = "Iva"
        Me.Iva.Name = "Iva"
        Me.Iva.ReadOnly = True
        Me.Iva.Visible = False
        Me.Iva.Width = 47
        '
        'Neto
        '
        Me.Neto.DataPropertyName = "Neto"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Neto.DefaultCellStyle = DataGridViewCellStyle5
        Me.Neto.HeaderText = "Neto"
        Me.Neto.MinimumWidth = 80
        Me.Neto.Name = "Neto"
        Me.Neto.ReadOnly = True
        Me.Neto.Width = 80
        '
        'MontoIva
        '
        Me.MontoIva.DataPropertyName = "MontoIva"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.MontoIva.DefaultCellStyle = DataGridViewCellStyle6
        Me.MontoIva.HeaderText = "Monto Iva"
        Me.MontoIva.MinimumWidth = 80
        Me.MontoIva.Name = "MontoIva"
        Me.MontoIva.ReadOnly = True
        Me.MontoIva.Width = 80
        '
        'TotalArticulo
        '
        Me.TotalArticulo.DataPropertyName = "TotalArticulo"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.TotalArticulo.DefaultCellStyle = DataGridViewCellStyle7
        Me.TotalArticulo.HeaderText = "Total "
        Me.TotalArticulo.MinimumWidth = 80
        Me.TotalArticulo.Name = "TotalArticulo"
        Me.TotalArticulo.ReadOnly = True
        Me.TotalArticulo.Width = 80
        '
        'Recibido
        '
        Me.Recibido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Recibido.DataPropertyName = "Recibido"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.MistyRose
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.Recibido.DefaultCellStyle = DataGridViewCellStyle8
        Me.Recibido.HeaderText = "Recibido"
        Me.Recibido.MinimumWidth = 80
        Me.Recibido.Name = "Recibido"
        Me.Recibido.ReadOnly = True
        Me.Recibido.Width = 80
        '
        'Consumido
        '
        Me.Consumido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Consumido.DataPropertyName = "Consumido"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Consumido.DefaultCellStyle = DataGridViewCellStyle9
        Me.Consumido.HeaderText = "Consumido"
        Me.Consumido.MinimumWidth = 80
        Me.Consumido.Name = "Consumido"
        Me.Consumido.ReadOnly = True
        Me.Consumido.Width = 80
        '
        'ButtonExcel
        '
        Me.ButtonExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExcel.Image = Global.ScomerV01.My.Resources.Resources.Excel
        Me.ButtonExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExcel.Location = New System.Drawing.Point(480, 621)
        Me.ButtonExcel.Name = "ButtonExcel"
        Me.ButtonExcel.Size = New System.Drawing.Size(166, 33)
        Me.ButtonExcel.TabIndex = 203
        Me.ButtonExcel.Text = "Facturas de La Orden"
        Me.ButtonExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonExcel.UseVisualStyleBackColor = True
        '
        'PanelCabeza
        '
        Me.PanelCabeza.BackColor = System.Drawing.Color.Gainsboro
        Me.PanelCabeza.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelCabeza.Controls.Add(Me.TextSaldo)
        Me.PanelCabeza.Controls.Add(Me.Comboproveedor)
        Me.PanelCabeza.Controls.Add(Me.Label25)
        Me.PanelCabeza.Controls.Add(Me.TextImporteWW)
        Me.PanelCabeza.Controls.Add(Me.Label11)
        Me.PanelCabeza.Controls.Add(Me.FechaEntregaHasta)
        Me.PanelCabeza.Controls.Add(Me.FechaEntregaDesde)
        Me.PanelCabeza.Controls.Add(Me.Label6)
        Me.PanelCabeza.Controls.Add(Me.ButtonDatosEmisor)
        Me.PanelCabeza.Controls.Add(Me.ComboPais)
        Me.PanelCabeza.Controls.Add(Me.ComboTipoIva)
        Me.PanelCabeza.Controls.Add(Me.Label14)
        Me.PanelCabeza.Controls.Add(Me.TextCuit)
        Me.PanelCabeza.Controls.Add(Me.Label10)
        Me.PanelCabeza.Controls.Add(Me.Label5)
        Me.PanelCabeza.Controls.Add(Me.TextProvincia)
        Me.PanelCabeza.Controls.Add(Me.Label13)
        Me.PanelCabeza.Controls.Add(Me.TextPlazoPago)
        Me.PanelCabeza.Controls.Add(Me.Label1)
        Me.PanelCabeza.Controls.Add(Me.Label2)
        Me.PanelCabeza.Controls.Add(Me.Label7)
        Me.PanelCabeza.Controls.Add(Me.TextComentario)
        Me.PanelCabeza.Controls.Add(Me.FechaEmision)
        Me.PanelCabeza.Controls.Add(Me.Label4)
        Me.PanelCabeza.Controls.Add(Me.Label8)
        Me.PanelCabeza.Location = New System.Drawing.Point(37, 30)
        Me.PanelCabeza.Name = "PanelCabeza"
        Me.PanelCabeza.Size = New System.Drawing.Size(886, 99)
        Me.PanelCabeza.TabIndex = 1
        '
        'TextSaldo
        '
        Me.TextSaldo.BackColor = System.Drawing.Color.White
        Me.TextSaldo.Enabled = False
        Me.TextSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldo.Location = New System.Drawing.Point(685, 73)
        Me.TextSaldo.MaxLength = 20
        Me.TextSaldo.Name = "TextSaldo"
        Me.TextSaldo.Size = New System.Drawing.Size(113, 20)
        Me.TextSaldo.TabIndex = 1019
        Me.TextSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Comboproveedor
        '
        Me.Comboproveedor.BackColor = System.Drawing.Color.White
        Me.Comboproveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.Comboproveedor.Enabled = False
        Me.Comboproveedor.FormattingEnabled = True
        Me.Comboproveedor.Location = New System.Drawing.Point(84, 2)
        Me.Comboproveedor.Name = "Comboproveedor"
        Me.Comboproveedor.Size = New System.Drawing.Size(265, 19)
        Me.Comboproveedor.TabIndex = 1020
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(641, 76)
        Me.Label25.Name = "Label25"
        Me.Label25.Size = New System.Drawing.Size(39, 13)
        Me.Label25.TabIndex = 1018
        Me.Label25.Text = "Saldo"
        Me.Label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImporteWW
        '
        Me.TextImporteWW.BackColor = System.Drawing.Color.White
        Me.TextImporteWW.Enabled = False
        Me.TextImporteWW.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporteWW.Location = New System.Drawing.Point(501, 73)
        Me.TextImporteWW.MaxLength = 20
        Me.TextImporteWW.Name = "TextImporteWW"
        Me.TextImporteWW.Size = New System.Drawing.Size(113, 20)
        Me.TextImporteWW.TabIndex = 1017
        Me.TextImporteWW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(446, 76)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 13)
        Me.Label11.TabIndex = 1016
        Me.Label11.Text = "Importe"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaEntregaHasta
        '
        Me.FechaEntregaHasta.CustomFormat = "dd/MM/yyyy"
        Me.FechaEntregaHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntregaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaEntregaHasta.Location = New System.Drawing.Point(304, 26)
        Me.FechaEntregaHasta.Name = "FechaEntregaHasta"
        Me.FechaEntregaHasta.Size = New System.Drawing.Size(107, 20)
        Me.FechaEntregaHasta.TabIndex = 1015
        '
        'FechaEntregaDesde
        '
        Me.FechaEntregaDesde.CustomFormat = "dd/MM/yyyy"
        Me.FechaEntregaDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEntregaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaEntregaDesde.Location = New System.Drawing.Point(138, 26)
        Me.FechaEntregaDesde.Name = "FechaEntregaDesde"
        Me.FechaEntregaDesde.Size = New System.Drawing.Size(107, 20)
        Me.FechaEntregaDesde.TabIndex = 1014
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(260, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(35, 13)
        Me.Label6.TabIndex = 1013
        Me.Label6.Text = "Hasta"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonDatosEmisor
        '
        Me.ButtonDatosEmisor.BackColor = System.Drawing.Color.PaleGoldenrod
        Me.ButtonDatosEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonDatosEmisor.Location = New System.Drawing.Point(738, 38)
        Me.ButtonDatosEmisor.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonDatosEmisor.Name = "ButtonDatosEmisor"
        Me.ButtonDatosEmisor.Size = New System.Drawing.Size(120, 20)
        Me.ButtonDatosEmisor.TabIndex = 1012
        Me.ButtonDatosEmisor.TabStop = False
        Me.ButtonDatosEmisor.Text = "Datos Proveedor"
        Me.ButtonDatosEmisor.UseVisualStyleBackColor = False
        '
        'ComboPais
        '
        Me.ComboPais.BackColor = System.Drawing.Color.White
        Me.ComboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboPais.Enabled = False
        Me.ComboPais.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPais.FormattingEnabled = True
        Me.ComboPais.Location = New System.Drawing.Point(207, 50)
        Me.ComboPais.Name = "ComboPais"
        Me.ComboPais.Size = New System.Drawing.Size(107, 21)
        Me.ComboPais.TabIndex = 218
        '
        'ComboTipoIva
        '
        Me.ComboTipoIva.BackColor = System.Drawing.Color.White
        Me.ComboTipoIva.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboTipoIva.Enabled = False
        Me.ComboTipoIva.FormattingEnabled = True
        Me.ComboTipoIva.Location = New System.Drawing.Point(489, 47)
        Me.ComboTipoIva.Name = "ComboTipoIva"
        Me.ComboTipoIva.Size = New System.Drawing.Size(152, 21)
        Me.ComboTipoIva.TabIndex = 217
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(459, 51)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(24, 13)
        Me.Label14.TabIndex = 216
        Me.Label14.Text = "IVA"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuit
        '
        Me.TextCuit.BackColor = System.Drawing.Color.White
        Me.TextCuit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuit.Location = New System.Drawing.Point(350, 49)
        Me.TextCuit.MaxLength = 20
        Me.TextCuit.Name = "TextCuit"
        Me.TextCuit.ReadOnly = True
        Me.TextCuit.Size = New System.Drawing.Size(101, 20)
        Me.TextCuit.TabIndex = 215
        Me.TextCuit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(176, 53)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(27, 13)
        Me.Label10.TabIndex = 213
        Me.Label10.Text = "Pais"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(318, 53)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(25, 13)
        Me.Label5.TabIndex = 212
        Me.Label5.Text = "Cuit"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextProvincia
        '
        Me.TextProvincia.BackColor = System.Drawing.Color.White
        Me.TextProvincia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextProvincia.Location = New System.Drawing.Point(66, 50)
        Me.TextProvincia.MaxLength = 20
        Me.TextProvincia.Name = "TextProvincia"
        Me.TextProvincia.ReadOnly = True
        Me.TextProvincia.Size = New System.Drawing.Size(101, 20)
        Me.TextProvincia.TabIndex = 211
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(10, 53)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(51, 13)
        Me.Label13.TabIndex = 202
        Me.Label13.Text = "Provincia"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextPlazoPago
        '
        Me.TextPlazoPago.Location = New System.Drawing.Point(517, 25)
        Me.TextPlazoPago.MaxLength = 20
        Me.TextPlazoPago.Name = "TextPlazoPago"
        Me.TextPlazoPago.Size = New System.Drawing.Size(143, 20)
        Me.TextPlazoPago.TabIndex = 200
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(435, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 13)
        Me.Label1.TabIndex = 199
        Me.Label1.Text = "Plazo de Pago"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(11, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(117, 13)
        Me.Label2.TabIndex = 197
        Me.Label2.Text = "Fecha Entrega   Desde"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 74)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 15)
        Me.Label7.TabIndex = 115
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Location = New System.Drawing.Point(84, 73)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(334, 20)
        Me.TextComentario.TabIndex = 8
        '
        'FechaEmision
        '
        Me.FechaEmision.CustomFormat = "dd/MM/yyyy"
        Me.FechaEmision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaEmision.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaEmision.Location = New System.Drawing.Point(757, 3)
        Me.FechaEmision.Name = "FechaEmision"
        Me.FechaEmision.Size = New System.Drawing.Size(107, 20)
        Me.FechaEmision.TabIndex = 4
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(705, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "Fecha "
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 5)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 13)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Proveedor"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(36, 12)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(87, 13)
        Me.Label9.TabIndex = 177
        Me.Label9.Text = "Orden Compra"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(758, 11)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 175
        Me.Label3.Text = "Estado"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(813, 7)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 179
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(307, 621)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(164, 33)
        Me.ButtonAnula.TabIndex = 180
        Me.ButtonAnula.Text = "Anula Orden Compra"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(794, 621)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(129, 33)
        Me.ButtonAceptar.TabIndex = 171
        Me.ButtonAceptar.Text = "Graba Cambios "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.Location = New System.Drawing.Point(148, 621)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(149, 33)
        Me.ButtonNuevo.TabIndex = 170
        Me.ButtonNuevo.Text = "Nueva Orden Compra"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'TextOrden
        '
        Me.TextOrden.BackColor = System.Drawing.Color.White
        Me.TextOrden.Enabled = False
        Me.TextOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextOrden.Location = New System.Drawing.Point(135, 9)
        Me.TextOrden.MaxLength = 20
        Me.TextOrden.Name = "TextOrden"
        Me.TextOrden.Size = New System.Drawing.Size(99, 20)
        Me.TextOrden.TabIndex = 201
        Me.TextOrden.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'ButtonAgregar
        '
        Me.ButtonAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAgregar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAgregar.Location = New System.Drawing.Point(654, 621)
        Me.ButtonAgregar.Name = "ButtonAgregar"
        Me.ButtonAgregar.Size = New System.Drawing.Size(132, 33)
        Me.ButtonAgregar.TabIndex = 202
        Me.ButtonAgregar.Text = "Agregar Articulos"
        Me.ButtonAgregar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAgregar.UseVisualStyleBackColor = True
        '
        'LabelOrdenFacturada
        '
        Me.LabelOrdenFacturada.AutoSize = True
        Me.LabelOrdenFacturada.BackColor = System.Drawing.Color.White
        Me.LabelOrdenFacturada.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOrdenFacturada.ForeColor = System.Drawing.Color.Red
        Me.LabelOrdenFacturada.Location = New System.Drawing.Point(442, 6)
        Me.LabelOrdenFacturada.Name = "LabelOrdenFacturada"
        Me.LabelOrdenFacturada.Size = New System.Drawing.Size(138, 18)
        Me.LabelOrdenFacturada.TabIndex = 204
        Me.LabelOrdenFacturada.Text = "Orden ya Facturada"
        Me.LabelOrdenFacturada.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonImprime
        '
        Me.ButtonImprime.Enabled = False
        Me.ButtonImprime.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprime.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprime.Image = Global.ScomerV01.My.Resources.Resources.Impresora
        Me.ButtonImprime.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonImprime.Location = New System.Drawing.Point(36, 620)
        Me.ButtonImprime.Name = "ButtonImprime"
        Me.ButtonImprime.Size = New System.Drawing.Size(103, 34)
        Me.ButtonImprime.TabIndex = 205
        Me.ButtonImprime.Text = "Imprimir"
        Me.ButtonImprime.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprime.UseVisualStyleBackColor = True
        '
        'UnaOrdenCompra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(945, 676)
        Me.Controls.Add(Me.ButtonImprime)
        Me.Controls.Add(Me.LabelOrdenFacturada)
        Me.Controls.Add(Me.ButtonExcel)
        Me.Controls.Add(Me.ButtonAgregar)
        Me.Controls.Add(Me.TextOrden)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.PanelCabeza)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Name = "UnaOrdenCompra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Orden de Compra"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelCabeza.ResumeLayout(False)
        Me.PanelCabeza.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PanelCabeza As System.Windows.Forms.Panel
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents TextPlazoPago As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents RadioSinIva As System.Windows.Forms.RadioButton
    Friend WithEvents RadioFinal As System.Windows.Forms.RadioButton
    Friend WithEvents Total As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextTotal As System.Windows.Forms.TextBox
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents TextCuit As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextProvincia As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents ComboTipoIva As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextOrden As System.Windows.Forms.TextBox
    Friend WithEvents ComboPais As System.Windows.Forms.ComboBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonDatosEmisor As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents FechaEntregaHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents FechaEntregaDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAgregar As System.Windows.Forms.Button
    Friend WithEvents ButtonExcel As System.Windows.Forms.Button
    Friend WithEvents TextImporteWW As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextSaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label25 As System.Windows.Forms.Label
    Friend WithEvents LabelOrdenFacturada As System.Windows.Forms.Label
    Friend WithEvents Comboproveedor As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonImprime As System.Windows.Forms.Button
    Friend WithEvents FechaEmision As System.Windows.Forms.DateTimePicker
    Friend WithEvents PrecioB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Iva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Neto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MontoIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TotalArticulo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Recibido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Consumido As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
