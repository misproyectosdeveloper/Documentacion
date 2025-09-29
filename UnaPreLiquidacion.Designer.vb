<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaPreLiquidacion
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
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaPreLiquidacion))
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Descarga1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.RemitoGuia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Senia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Iva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Ingresados = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Merma = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Medida = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Precio = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioF = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Total = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextUnidades = New System.Windows.Forms.TextBox
        Me.TextBruto = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextComision = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextImporteComision = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextDescarga = New System.Windows.Forms.TextBox
        Me.TextNeto = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label6 = New System.Windows.Forms.Label
        Me.PanelCandado = New System.Windows.Forms.Panel
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.TextAutorizar = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.TextDirecto = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextImporteXBulto = New System.Windows.Forms.TextBox
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextTotalSenia = New System.Windows.Forms.TextBox
        Me.TextSeniaXBulto = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextTotal = New System.Windows.Forms.TextBox
        Me.ButtonIgualProveedor = New System.Windows.Forms.Button
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextIvaComision = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.ComboIVA = New System.Windows.Forms.ComboBox
        Me.LabelIVA = New System.Windows.Forms.Label
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelCandado.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Descarga1, Me.Lote, Me.Secuencia, Me.Fecha, Me.Sel, Me.Candado, Me.LoteYSecuencia, Me.Articulo, Me.RemitoGuia, Me.Senia, Me.Iva, Me.Ingresados, Me.Merma, Me.Cantidad, Me.Medida, Me.Precio, Me.PrecioF, Me.Total})
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.Location = New System.Drawing.Point(19, 55)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle13
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(949, 422)
        Me.Grid.TabIndex = 102
        Me.Grid.TabStop = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        Me.Operacion.Width = 81
        '
        'Descarga1
        '
        Me.Descarga1.DataPropertyName = "Descarga"
        Me.Descarga1.HeaderText = "Descarga"
        Me.Descarga1.Name = "Descarga1"
        Me.Descarga1.Visible = False
        Me.Descarga1.Width = 78
        '
        'Lote
        '
        Me.Lote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle2
        Me.Lote.HeaderText = "Lotes"
        Me.Lote.MinimumWidth = 70
        Me.Lote.Name = "Lote"
        Me.Lote.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Lote.Visible = False
        Me.Lote.Width = 70
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        Me.Secuencia.Width = 83
        '
        'Fecha
        '
        Me.Fecha.DataPropertyName = "Fecha"
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.Name = "Fecha"
        Me.Fecha.Visible = False
        Me.Fecha.Width = 62
        '
        'Sel
        '
        Me.Sel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 25
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 25
        '
        'Candado
        '
        Me.Candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.Width = 30
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 53
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 150
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Width = 150
        '
        'RemitoGuia
        '
        Me.RemitoGuia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.RemitoGuia.DataPropertyName = "RemitoGuia"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.RemitoGuia.DefaultCellStyle = DataGridViewCellStyle3
        Me.RemitoGuia.HeaderText = "Remito/Guia"
        Me.RemitoGuia.MinimumWidth = 90
        Me.RemitoGuia.Name = "RemitoGuia"
        Me.RemitoGuia.ReadOnly = True
        Me.RemitoGuia.Width = 90
        '
        'Senia
        '
        Me.Senia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Senia.DataPropertyName = "Senia"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Senia.DefaultCellStyle = DataGridViewCellStyle4
        Me.Senia.HeaderText = "Seña"
        Me.Senia.MinimumWidth = 70
        Me.Senia.Name = "Senia"
        Me.Senia.ReadOnly = True
        Me.Senia.Width = 70
        '
        'Iva
        '
        Me.Iva.DataPropertyName = "Iva"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Iva.DefaultCellStyle = DataGridViewCellStyle5
        Me.Iva.HeaderText = "% Iva"
        Me.Iva.Name = "Iva"
        Me.Iva.ReadOnly = True
        Me.Iva.Width = 58
        '
        'Ingresados
        '
        Me.Ingresados.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Ingresados.DataPropertyName = "Ingresados"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Ingresados.DefaultCellStyle = DataGridViewCellStyle6
        Me.Ingresados.HeaderText = "Cant.Inicial"
        Me.Ingresados.MaxInputLength = 8
        Me.Ingresados.MinimumWidth = 70
        Me.Ingresados.Name = "Ingresados"
        Me.Ingresados.ReadOnly = True
        Me.Ingresados.Width = 70
        '
        'Merma
        '
        Me.Merma.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Merma.DataPropertyName = "Merma"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Merma.DefaultCellStyle = DataGridViewCellStyle7
        Me.Merma.HeaderText = "Merma"
        Me.Merma.MinimumWidth = 70
        Me.Merma.Name = "Merma"
        Me.Merma.ReadOnly = True
        Me.Merma.Width = 70
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle8
        Me.Cantidad.HeaderText = "A Liquidar"
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
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
        'Precio
        '
        Me.Precio.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Precio.DataPropertyName = "Precio"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Precio.DefaultCellStyle = DataGridViewCellStyle9
        Me.Precio.HeaderText = "Precio S."
        Me.Precio.MaxInputLength = 6
        Me.Precio.MinimumWidth = 75
        Me.Precio.Name = "Precio"
        Me.Precio.ReadOnly = True
        Me.Precio.Visible = False
        Me.Precio.Width = 75
        '
        'PrecioF
        '
        Me.PrecioF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrecioF.DataPropertyName = "PrecioF"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrecioF.DefaultCellStyle = DataGridViewCellStyle10
        Me.PrecioF.HeaderText = "Precio "
        Me.PrecioF.MinimumWidth = 75
        Me.PrecioF.Name = "PrecioF"
        Me.PrecioF.ReadOnly = True
        Me.PrecioF.Width = 75
        '
        'Total
        '
        Me.Total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Total.DataPropertyName = "Total"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Total.DefaultCellStyle = DataGridViewCellStyle11
        Me.Total.HeaderText = "Neto a Liq."
        Me.Total.MinimumWidth = 90
        Me.Total.Name = "Total"
        Me.Total.ReadOnly = True
        Me.Total.Width = 90
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(74, 485)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(104, 15)
        Me.Label3.TabIndex = 104
        Me.Label3.Text = "Total Unidades"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextUnidades
        '
        Me.TextUnidades.BackColor = System.Drawing.Color.PowderBlue
        Me.TextUnidades.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextUnidades.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUnidades.Location = New System.Drawing.Point(212, 485)
        Me.TextUnidades.Name = "TextUnidades"
        Me.TextUnidades.ReadOnly = True
        Me.TextUnidades.Size = New System.Drawing.Size(112, 20)
        Me.TextUnidades.TabIndex = 105
        Me.TextUnidades.TabStop = False
        Me.TextUnidades.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextBruto
        '
        Me.TextBruto.BackColor = System.Drawing.Color.PowderBlue
        Me.TextBruto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBruto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBruto.Location = New System.Drawing.Point(461, 484)
        Me.TextBruto.Name = "TextBruto"
        Me.TextBruto.ReadOnly = True
        Me.TextBruto.Size = New System.Drawing.Size(136, 20)
        Me.TextBruto.TabIndex = 107
        Me.TextBruto.TabStop = False
        Me.TextBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(367, 485)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 15)
        Me.Label1.TabIndex = 106
        Me.Label1.Text = "Venta Bruta"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextComision
        '
        Me.TextComision.BackColor = System.Drawing.Color.White
        Me.TextComision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextComision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComision.Location = New System.Drawing.Point(246, 510)
        Me.TextComision.Name = "TextComision"
        Me.TextComision.Size = New System.Drawing.Size(78, 20)
        Me.TextComision.TabIndex = 1
        Me.TextComision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(73, 510)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(67, 15)
        Me.Label2.TabIndex = 108
        Me.Label2.Text = "Comisión"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImporteComision
        '
        Me.TextImporteComision.BackColor = System.Drawing.Color.PowderBlue
        Me.TextImporteComision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextImporteComision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporteComision.Location = New System.Drawing.Point(461, 508)
        Me.TextImporteComision.Name = "TextImporteComision"
        Me.TextImporteComision.ReadOnly = True
        Me.TextImporteComision.Size = New System.Drawing.Size(136, 20)
        Me.TextImporteComision.TabIndex = 110
        Me.TextImporteComision.TabStop = False
        Me.TextImporteComision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(74, 556)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(68, 15)
        Me.Label5.TabIndex = 113
        Me.Label5.Text = "Descarga"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextDescarga
        '
        Me.TextDescarga.BackColor = System.Drawing.Color.PowderBlue
        Me.TextDescarga.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextDescarga.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDescarga.Location = New System.Drawing.Point(461, 555)
        Me.TextDescarga.Name = "TextDescarga"
        Me.TextDescarga.ReadOnly = True
        Me.TextDescarga.Size = New System.Drawing.Size(136, 20)
        Me.TextDescarga.TabIndex = 114
        Me.TextDescarga.TabStop = False
        Me.TextDescarga.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextNeto
        '
        Me.TextNeto.BackColor = System.Drawing.Color.LightCyan
        Me.TextNeto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextNeto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNeto.Location = New System.Drawing.Point(461, 579)
        Me.TextNeto.Name = "TextNeto"
        Me.TextNeto.ReadOnly = True
        Me.TextNeto.Size = New System.Drawing.Size(136, 20)
        Me.TextNeto.TabIndex = 116
        Me.TextNeto.TabStop = False
        Me.TextNeto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(19, 17)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(65, 13)
        Me.Label8.TabIndex = 120
        Me.Label8.Text = "Proveedor"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(341, 583)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(107, 15)
        Me.Label9.TabIndex = 122
        Me.Label9.Text = "Sub-Total  Neto"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(808, 7)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(42, 13)
        Me.Label10.TabIndex = 124
        Me.Label10.Text = "Fecha"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(855, 4)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(113, 20)
        Me.DateTime1.TabIndex = 123
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(223, 513)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(19, 15)
        Me.Label6.TabIndex = 231
        Me.Label6.Text = "%"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelCandado
        '
        Me.PanelCandado.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelCandado.Controls.Add(Me.PictureBox1)
        Me.PanelCandado.Controls.Add(Me.PictureBox2)
        Me.PanelCandado.Controls.Add(Me.Label19)
        Me.PanelCandado.Controls.Add(Me.TextAutorizar)
        Me.PanelCandado.Controls.Add(Me.Label20)
        Me.PanelCandado.Controls.Add(Me.TextDirecto)
        Me.PanelCandado.Location = New System.Drawing.Point(715, 483)
        Me.PanelCandado.Name = "PanelCandado"
        Me.PanelCandado.Size = New System.Drawing.Size(119, 50)
        Me.PanelCandado.TabIndex = 3
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(-39, 25)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(15, 20)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 126
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.InitialImage = CType(resources.GetObject("PictureBox2.InitialImage"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(-39, 3)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(15, 20)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 125
        Me.PictureBox2.TabStop = False
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label19.Location = New System.Drawing.Point(82, 27)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(15, 13)
        Me.Label19.TabIndex = 21
        Me.Label19.Text = "%"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextAutorizar
        '
        Me.TextAutorizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAutorizar.Location = New System.Drawing.Point(24, 24)
        Me.TextAutorizar.MaxLength = 6
        Me.TextAutorizar.Name = "TextAutorizar"
        Me.TextAutorizar.ReadOnly = True
        Me.TextAutorizar.Size = New System.Drawing.Size(50, 20)
        Me.TextAutorizar.TabIndex = 20
        Me.TextAutorizar.TabStop = False
        Me.TextAutorizar.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label20.Location = New System.Drawing.Point(82, 7)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(15, 13)
        Me.Label20.TabIndex = 19
        Me.Label20.Text = "%"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextDirecto
        '
        Me.TextDirecto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextDirecto.Location = New System.Drawing.Point(24, 2)
        Me.TextDirecto.MaxLength = 7
        Me.TextDirecto.Name = "TextDirecto"
        Me.TextDirecto.Size = New System.Drawing.Size(50, 20)
        Me.TextDirecto.TabIndex = 3
        Me.TextDirecto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(175, 558)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(65, 15)
        Me.Label4.TabIndex = 238
        Me.Label4.Text = "$ X Bulto"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImporteXBulto
        '
        Me.TextImporteXBulto.BackColor = System.Drawing.Color.White
        Me.TextImporteXBulto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextImporteXBulto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporteXBulto.Location = New System.Drawing.Point(246, 556)
        Me.TextImporteXBulto.Name = "TextImporteXBulto"
        Me.TextImporteXBulto.Size = New System.Drawing.Size(78, 20)
        Me.TextImporteXBulto.TabIndex = 2
        Me.TextImporteXBulto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboProveedor
        '
        Me.ComboProveedor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboProveedor.Enabled = False
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(97, 12)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(263, 21)
        Me.ComboProveedor.TabIndex = 239
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(74, 605)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 15)
        Me.Label7.TabIndex = 274
        Me.Label7.Text = "Seña"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotalSenia
        '
        Me.TextTotalSenia.BackColor = System.Drawing.Color.PowderBlue
        Me.TextTotalSenia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotalSenia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalSenia.Location = New System.Drawing.Point(461, 603)
        Me.TextTotalSenia.Name = "TextTotalSenia"
        Me.TextTotalSenia.ReadOnly = True
        Me.TextTotalSenia.Size = New System.Drawing.Size(136, 20)
        Me.TextTotalSenia.TabIndex = 275
        Me.TextTotalSenia.TabStop = False
        Me.TextTotalSenia.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextSeniaXBulto
        '
        Me.TextSeniaXBulto.BackColor = System.Drawing.Color.White
        Me.TextSeniaXBulto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextSeniaXBulto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSeniaXBulto.Location = New System.Drawing.Point(246, 602)
        Me.TextSeniaXBulto.Name = "TextSeniaXBulto"
        Me.TextSeniaXBulto.Size = New System.Drawing.Size(78, 20)
        Me.TextSeniaXBulto.TabIndex = 277
        Me.TextSeniaXBulto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(407, 629)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(39, 15)
        Me.Label12.TabIndex = 279
        Me.Label12.Text = "Total"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotal
        '
        Me.TextTotal.BackColor = System.Drawing.Color.LightCyan
        Me.TextTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotal.Location = New System.Drawing.Point(461, 627)
        Me.TextTotal.Name = "TextTotal"
        Me.TextTotal.ReadOnly = True
        Me.TextTotal.Size = New System.Drawing.Size(136, 20)
        Me.TextTotal.TabIndex = 280
        Me.TextTotal.TabStop = False
        Me.TextTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonIgualProveedor
        '
        Me.ButtonIgualProveedor.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonIgualProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonIgualProveedor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonIgualProveedor.Location = New System.Drawing.Point(203, 656)
        Me.ButtonIgualProveedor.Name = "ButtonIgualProveedor"
        Me.ButtonIgualProveedor.Size = New System.Drawing.Size(212, 37)
        Me.ButtonIgualProveedor.TabIndex = 273
        Me.ButtonIgualProveedor.TabStop = False
        Me.ButtonIgualProveedor.Text = "Nueva Pre-Liquidación Igual prov."
        Me.ButtonIgualProveedor.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonIgualProveedor.UseVisualStyleBackColor = True
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(21, 656)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(152, 37)
        Me.ButtonNuevo.TabIndex = 272
        Me.ButtonNuevo.TabStop = False
        Me.ButtonNuevo.Text = "Nueva Pre-Liquidación"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.ButtonAceptar.Location = New System.Drawing.Point(813, 656)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(152, 37)
        Me.ButtonAceptar.TabIndex = 5
        Me.ButtonAceptar.Text = "Ver Liquidación"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(223, 536)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(19, 15)
        Me.Label11.TabIndex = 283
        Me.Label11.Text = "%"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextIvaComision
        '
        Me.TextIvaComision.BackColor = System.Drawing.Color.White
        Me.TextIvaComision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextIvaComision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextIvaComision.Location = New System.Drawing.Point(246, 533)
        Me.TextIvaComision.MaxLength = 5
        Me.TextIvaComision.Name = "TextIvaComision"
        Me.TextIvaComision.Size = New System.Drawing.Size(78, 20)
        Me.TextIvaComision.TabIndex = 281
        Me.TextIvaComision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(74, 533)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(91, 15)
        Me.Label13.TabIndex = 282
        Me.Label13.Text = "IVA Comisión"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(422, 17)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(52, 13)
        Me.Label14.TabIndex = 284
        Me.Label14.Text = "Label14"
        '
        'ComboIVA
        '
        Me.ComboIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboIVA.FormattingEnabled = True
        Me.ComboIVA.Location = New System.Drawing.Point(855, 30)
        Me.ComboIVA.Name = "ComboIVA"
        Me.ComboIVA.Size = New System.Drawing.Size(61, 21)
        Me.ComboIVA.TabIndex = 285
        '
        'LabelIVA
        '
        Me.LabelIVA.AutoSize = True
        Me.LabelIVA.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelIVA.Location = New System.Drawing.Point(708, 33)
        Me.LabelIVA.Name = "LabelIVA"
        Me.LabelIVA.Size = New System.Drawing.Size(135, 13)
        Me.LabelIVA.TabIndex = 286
        Me.LabelIVA.Text = "Filtra Lotes con I.V.A.:"
        '
        'UnaPreLiquidacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.PowderBlue
        Me.ClientSize = New System.Drawing.Size(988, 702)
        Me.Controls.Add(Me.LabelIVA)
        Me.Controls.Add(Me.ComboIVA)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.TextIvaComision)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.TextTotal)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.TextSeniaXBulto)
        Me.Controls.Add(Me.TextTotalSenia)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.ButtonIgualProveedor)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ComboProveedor)
        Me.Controls.Add(Me.PanelCandado)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.TextNeto)
        Me.Controls.Add(Me.TextDescarga)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TextImporteComision)
        Me.Controls.Add(Me.TextComision)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextBruto)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TextUnidades)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.TextImporteXBulto)
        Me.Name = "UnaPreLiquidacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pre- Liquidación Proveedores."
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelCandado.ResumeLayout(False)
        Me.PanelCandado.PerformLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextUnidades As System.Windows.Forms.TextBox
    Friend WithEvents TextBruto As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextComision As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TextImporteComision As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextDescarga As System.Windows.Forms.TextBox
    Friend WithEvents TextNeto As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Descarga As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents PanelCandado As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents TextAutorizar As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents TextDirecto As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextImporteXBulto As System.Windows.Forms.TextBox
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonIgualProveedor As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextTotalSenia As System.Windows.Forms.TextBox
    Friend WithEvents TextSeniaXBulto As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextTotal As System.Windows.Forms.TextBox
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Descarga1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents RemitoGuia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Senia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Iva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ingresados As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Merma As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Medida As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Precio As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioF As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Total As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextIvaComision As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ComboIVA As System.Windows.Forms.ComboBox
    Friend WithEvents LabelIVA As System.Windows.Forms.Label
End Class
