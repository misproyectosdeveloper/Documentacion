<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AnalisisResultadosLotesCosteo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AnalisisResultadosLotesCosteo))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle20 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle21 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
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
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle19 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.MaskedLote = New System.Windows.Forms.MaskedTextBox
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.CheckSinStock = New System.Windows.Forms.CheckBox
        Me.CheckConStock = New System.Windows.Forms.CheckBox
        Me.ComboArticulo = New System.Windows.Forms.ComboBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.RadioImporteTotal = New System.Windows.Forms.RadioButton
        Me.RadioImporteSinIva = New System.Windows.Forms.RadioButton
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label3 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cerrado = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Especie = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Variedad = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Facturado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Remitido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ImporteSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GastoComercial = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.GastoComercialSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoAsignado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoAsignadoSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoProduccion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CostoProduccionSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Resultado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ResultadoSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrResultado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrResultadoSinIva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(6, 719)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 23)
        Me.ButtonPrimero.TabIndex = 151
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(40, 719)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 23)
        Me.ButtonAnterior.TabIndex = 152
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(72, 719)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 23)
        Me.ButtonPosterior.TabIndex = 149
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(104, 719)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 23)
        Me.ButtonUltimo.TabIndex = 150
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
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
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.MaskedLote)
        Me.Panel1.Controls.Add(Me.ComboCosteo)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.ComboVariedad)
        Me.Panel1.Controls.Add(Me.ComboEspecie)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.CheckSinStock)
        Me.Panel1.Controls.Add(Me.CheckConStock)
        Me.Panel1.Controls.Add(Me.ComboArticulo)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Location = New System.Drawing.Point(5, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1324, 55)
        Me.Panel1.TabIndex = 148
        '
        'MaskedLote
        '
        Me.MaskedLote.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaskedLote.InsertKeyMode = System.Windows.Forms.InsertKeyMode.Overwrite
        Me.MaskedLote.Location = New System.Drawing.Point(60, 27)
        Me.MaskedLote.Mask = "0000000/000"
        Me.MaskedLote.Name = "MaskedLote"
        Me.MaskedLote.Size = New System.Drawing.Size(109, 20)
        Me.MaskedLote.TabIndex = 277
        Me.MaskedLote.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.MaskedLote.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(591, 1)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(154, 21)
        Me.ComboCosteo.TabIndex = 209
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(540, 4)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(45, 15)
        Me.Label9.TabIndex = 210
        Me.Label9.Text = "Costeo"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(1032, 1)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(123, 21)
        Me.ComboVariedad.TabIndex = 142
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(841, 1)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(123, 21)
        Me.ComboEspecie.TabIndex = 141
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(977, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(49, 13)
        Me.Label6.TabIndex = 140
        Me.Label6.Text = "Variedad"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(790, 6)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(45, 13)
        Me.Label5.TabIndex = 139
        Me.Label5.Text = "Especie"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckSinStock
        '
        Me.CheckSinStock.BackColor = System.Drawing.Color.LightGray
        Me.CheckSinStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSinStock.Checked = True
        Me.CheckSinStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckSinStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSinStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSinStock.Location = New System.Drawing.Point(706, 25)
        Me.CheckSinStock.Name = "CheckSinStock"
        Me.CheckSinStock.Size = New System.Drawing.Size(83, 30)
        Me.CheckSinStock.TabIndex = 136
        Me.CheckSinStock.Text = "Sin Stock"
        Me.CheckSinStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckSinStock.UseVisualStyleBackColor = False
        '
        'CheckConStock
        '
        Me.CheckConStock.BackColor = System.Drawing.Color.LightGray
        Me.CheckConStock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckConStock.Checked = True
        Me.CheckConStock.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckConStock.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckConStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckConStock.Location = New System.Drawing.Point(622, 24)
        Me.CheckConStock.Name = "CheckConStock"
        Me.CheckConStock.Size = New System.Drawing.Size(78, 30)
        Me.CheckConStock.TabIndex = 135
        Me.CheckConStock.Text = "Con Stock"
        Me.CheckConStock.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckConStock.UseVisualStyleBackColor = False
        '
        'ComboArticulo
        '
        Me.ComboArticulo.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboArticulo.FormattingEnabled = True
        Me.ComboArticulo.Location = New System.Drawing.Point(61, 3)
        Me.ComboArticulo.Name = "ComboArticulo"
        Me.ComboArticulo.Size = New System.Drawing.Size(242, 22)
        Me.ComboArticulo.TabIndex = 132
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.LightGray
        Me.Panel3.Controls.Add(Me.RadioImporteTotal)
        Me.Panel3.Controls.Add(Me.RadioImporteSinIva)
        Me.Panel3.Location = New System.Drawing.Point(804, 26)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(253, 28)
        Me.Panel3.TabIndex = 131
        '
        'RadioImporteTotal
        '
        Me.RadioImporteTotal.AutoSize = True
        Me.RadioImporteTotal.Checked = True
        Me.RadioImporteTotal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioImporteTotal.ForeColor = System.Drawing.Color.Red
        Me.RadioImporteTotal.Location = New System.Drawing.Point(31, 6)
        Me.RadioImporteTotal.Name = "RadioImporteTotal"
        Me.RadioImporteTotal.Size = New System.Drawing.Size(87, 17)
        Me.RadioImporteTotal.TabIndex = 3
        Me.RadioImporteTotal.TabStop = True
        Me.RadioImporteTotal.Text = "Importe Total"
        Me.RadioImporteTotal.UseVisualStyleBackColor = True
        '
        'RadioImporteSinIva
        '
        Me.RadioImporteSinIva.AutoSize = True
        Me.RadioImporteSinIva.BackColor = System.Drawing.Color.LightGray
        Me.RadioImporteSinIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioImporteSinIva.Location = New System.Drawing.Point(147, 5)
        Me.RadioImporteSinIva.Name = "RadioImporteSinIva"
        Me.RadioImporteSinIva.Size = New System.Drawing.Size(96, 17)
        Me.RadioImporteSinIva.TabIndex = 0
        Me.RadioImporteSinIva.Text = "Importe Sin Iva"
        Me.RadioImporteSinIva.UseVisualStyleBackColor = False
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(368, 2)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(154, 21)
        Me.ComboProveedor.TabIndex = 97
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(314, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(47, 13)
        Me.Label1.TabIndex = 96
        Me.Label1.Text = "Negocio"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(28, 13)
        Me.Label2.TabIndex = 91
        Me.Label2.Text = "Lote"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1169, 21)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(129, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(437, 32)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(35, 13)
        Me.Label7.TabIndex = 34
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(248, 32)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 13)
        Me.Label4.TabIndex = 33
        Me.Label4.Text = "Ingreso Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(477, 28)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeHasta.TabIndex = 32
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(329, 28)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(95, 20)
        Me.DateTimeDesde.TabIndex = 30
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(7, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Articulos"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Lote, Me.Secuencia, Me.Candado, Me.LoteYSecuencia, Me.Proveedor, Me.Cerrado, Me.Articulo, Me.Especie, Me.Variedad, Me.Fecha, Me.Cantidad, Me.Stock, Me.Facturado, Me.Remitido, Me.Importe, Me.ImporteSinIva, Me.GastoComercial, Me.GastoComercialSinIva, Me.CostoAsignado, Me.CostoAsignadoSinIva, Me.CostoProduccion, Me.CostoProduccionSinIva, Me.Resultado, Me.ResultadoSinIva, Me.PrResultado, Me.PrResultadoSinIva})
        DataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle20.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle20
        Me.Grid.Location = New System.Drawing.Point(5, 60)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle21.BackColor = System.Drawing.Color.AntiqueWhite
        DataGridViewCellStyle21.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle21
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(1324, 653)
        Me.Grid.TabIndex = 147
        Me.Grid.TabStop = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        Me.Operacion.Width = 62
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
        Me.Secuencia.Width = 64
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
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle3
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 80
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 80
        '
        'Proveedor
        '
        Me.Proveedor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Proveedor.DataPropertyName = "Proveedor"
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Negocio"
        Me.Proveedor.MinimumWidth = 100
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        '
        'Cerrado
        '
        Me.Cerrado.DataPropertyName = "Cerrado"
        Me.Cerrado.HeaderText = "Cerrado"
        Me.Cerrado.Name = "Cerrado"
        Me.Cerrado.ReadOnly = True
        Me.Cerrado.Width = 50
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 100
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        '
        'Especie
        '
        Me.Especie.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Especie.DataPropertyName = "Especie"
        Me.Especie.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Especie.HeaderText = "Especie"
        Me.Especie.MinimumWidth = 60
        Me.Especie.Name = "Especie"
        Me.Especie.ReadOnly = True
        Me.Especie.Width = 60
        '
        'Variedad
        '
        Me.Variedad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Variedad.DataPropertyName = "Variedad"
        Me.Variedad.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Variedad.HeaderText = "Variedad"
        Me.Variedad.MinimumWidth = 60
        Me.Variedad.Name = "Variedad"
        Me.Variedad.ReadOnly = True
        Me.Variedad.Width = 60
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.Width = 70
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle4
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MinimumWidth = 60
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 60
        '
        'Stock
        '
        Me.Stock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Stock.DataPropertyName = "Stock"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle5
        Me.Stock.HeaderText = "Stock"
        Me.Stock.MinimumWidth = 60
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Width = 60
        '
        'Facturado
        '
        Me.Facturado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Facturado.DataPropertyName = "Facturado"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Facturado.DefaultCellStyle = DataGridViewCellStyle6
        Me.Facturado.HeaderText = "Facturado"
        Me.Facturado.MinimumWidth = 60
        Me.Facturado.Name = "Facturado"
        Me.Facturado.ReadOnly = True
        Me.Facturado.Width = 60
        '
        'Remitido
        '
        Me.Remitido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Remitido.DataPropertyName = "Remitido"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Remitido.DefaultCellStyle = DataGridViewCellStyle7
        Me.Remitido.HeaderText = "Remitido"
        Me.Remitido.MinimumWidth = 60
        Me.Remitido.Name = "Remitido"
        Me.Remitido.ReadOnly = True
        Me.Remitido.Width = 60
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle8
        Me.Importe.HeaderText = "Imp. Venta"
        Me.Importe.MaxInputLength = 8
        Me.Importe.MinimumWidth = 90
        Me.Importe.Name = "Importe"
        Me.Importe.ReadOnly = True
        Me.Importe.Width = 90
        '
        'ImporteSinIva
        '
        Me.ImporteSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ImporteSinIva.DataPropertyName = "ImporteSinIva"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ImporteSinIva.DefaultCellStyle = DataGridViewCellStyle9
        Me.ImporteSinIva.HeaderText = "Imp. Venta"
        Me.ImporteSinIva.MinimumWidth = 90
        Me.ImporteSinIva.Name = "ImporteSinIva"
        Me.ImporteSinIva.ReadOnly = True
        Me.ImporteSinIva.Visible = False
        Me.ImporteSinIva.Width = 90
        '
        'GastoComercial
        '
        Me.GastoComercial.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.GastoComercial.DataPropertyName = "GastoComercial"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.GastoComercial.DefaultCellStyle = DataGridViewCellStyle10
        Me.GastoComercial.HeaderText = "Gas.Comer."
        Me.GastoComercial.MinimumWidth = 80
        Me.GastoComercial.Name = "GastoComercial"
        Me.GastoComercial.ReadOnly = True
        Me.GastoComercial.Width = 80
        '
        'GastoComercialSinIva
        '
        Me.GastoComercialSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.GastoComercialSinIva.DataPropertyName = "GastoComercialSinIva"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.GastoComercialSinIva.DefaultCellStyle = DataGridViewCellStyle11
        Me.GastoComercialSinIva.HeaderText = "Gas.Comer. "
        Me.GastoComercialSinIva.MinimumWidth = 80
        Me.GastoComercialSinIva.Name = "GastoComercialSinIva"
        Me.GastoComercialSinIva.ReadOnly = True
        Me.GastoComercialSinIva.Visible = False
        Me.GastoComercialSinIva.Width = 80
        '
        'CostoAsignado
        '
        Me.CostoAsignado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoAsignado.DataPropertyName = "CostoAsignado"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoAsignado.DefaultCellStyle = DataGridViewCellStyle12
        Me.CostoAsignado.HeaderText = "Costo Asig."
        Me.CostoAsignado.MinimumWidth = 90
        Me.CostoAsignado.Name = "CostoAsignado"
        Me.CostoAsignado.ReadOnly = True
        Me.CostoAsignado.Width = 90
        '
        'CostoAsignadoSinIva
        '
        Me.CostoAsignadoSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoAsignadoSinIva.DataPropertyName = "CostoAsignadoSinIva"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoAsignadoSinIva.DefaultCellStyle = DataGridViewCellStyle13
        Me.CostoAsignadoSinIva.HeaderText = "Costo Asig."
        Me.CostoAsignadoSinIva.MaxInputLength = 6
        Me.CostoAsignadoSinIva.MinimumWidth = 90
        Me.CostoAsignadoSinIva.Name = "CostoAsignadoSinIva"
        Me.CostoAsignadoSinIva.ReadOnly = True
        Me.CostoAsignadoSinIva.Visible = False
        Me.CostoAsignadoSinIva.Width = 90
        '
        'CostoProduccion
        '
        Me.CostoProduccion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoProduccion.DataPropertyName = "CostoProduccion"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoProduccion.DefaultCellStyle = DataGridViewCellStyle14
        Me.CostoProduccion.HeaderText = "Costo Pr."
        Me.CostoProduccion.MinimumWidth = 80
        Me.CostoProduccion.Name = "CostoProduccion"
        Me.CostoProduccion.ReadOnly = True
        Me.CostoProduccion.Width = 80
        '
        'CostoProduccionSinIva
        '
        Me.CostoProduccionSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CostoProduccionSinIva.DataPropertyName = "CostoProduccionSinIva"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CostoProduccionSinIva.DefaultCellStyle = DataGridViewCellStyle15
        Me.CostoProduccionSinIva.HeaderText = "Costo Pr."
        Me.CostoProduccionSinIva.MinimumWidth = 80
        Me.CostoProduccionSinIva.Name = "CostoProduccionSinIva"
        Me.CostoProduccionSinIva.ReadOnly = True
        Me.CostoProduccionSinIva.Visible = False
        Me.CostoProduccionSinIva.Width = 80
        '
        'Resultado
        '
        Me.Resultado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Resultado.DataPropertyName = "Resultado"
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Resultado.DefaultCellStyle = DataGridViewCellStyle16
        Me.Resultado.HeaderText = "Resultado"
        Me.Resultado.MinimumWidth = 80
        Me.Resultado.Name = "Resultado"
        Me.Resultado.ReadOnly = True
        Me.Resultado.Width = 80
        '
        'ResultadoSinIva
        '
        Me.ResultadoSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ResultadoSinIva.DataPropertyName = "ResultadoSinIva"
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ResultadoSinIva.DefaultCellStyle = DataGridViewCellStyle17
        Me.ResultadoSinIva.HeaderText = "Resultado"
        Me.ResultadoSinIva.MinimumWidth = 80
        Me.ResultadoSinIva.Name = "ResultadoSinIva"
        Me.ResultadoSinIva.ReadOnly = True
        Me.ResultadoSinIva.Visible = False
        Me.ResultadoSinIva.Width = 80
        '
        'PrResultado
        '
        Me.PrResultado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrResultado.DataPropertyName = "PrResultado"
        DataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrResultado.DefaultCellStyle = DataGridViewCellStyle18
        Me.PrResultado.HeaderText = "Resul.Pro."
        Me.PrResultado.MinimumWidth = 70
        Me.PrResultado.Name = "PrResultado"
        Me.PrResultado.ReadOnly = True
        Me.PrResultado.Width = 70
        '
        'PrResultadoSinIva
        '
        Me.PrResultadoSinIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrResultadoSinIva.DataPropertyName = "PrResultadoSinIva"
        DataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrResultadoSinIva.DefaultCellStyle = DataGridViewCellStyle19
        Me.PrResultadoSinIva.HeaderText = "Resul.Pro."
        Me.PrResultadoSinIva.MinimumWidth = 70
        Me.PrResultadoSinIva.Name = "PrResultadoSinIva"
        Me.PrResultadoSinIva.ReadOnly = True
        Me.PrResultadoSinIva.Visible = False
        Me.PrResultadoSinIva.Width = 70
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(609, 717)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(143, 28)
        Me.ButtonExportarExcel.TabIndex = 165
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.TextAlign = System.Drawing.ContentAlignment.TopLeft
        Me.ButtonExportarExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'AnalisisResultadosLotesCosteo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Lavender
        Me.ClientSize = New System.Drawing.Size(1336, 753)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "AnalisisResultadosLotesCosteo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Análisis De Resultado Lotes Costeo"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CheckSinStock As System.Windows.Forms.CheckBox
    Friend WithEvents CheckConStock As System.Windows.Forms.CheckBox
    Friend WithEvents ComboArticulo As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents RadioImporteTotal As System.Windows.Forms.RadioButton
    Friend WithEvents RadioImporteSinIva As System.Windows.Forms.RadioButton
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents MaskedLote As System.Windows.Forms.MaskedTextBox
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cerrado As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Especie As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Variedad As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Facturado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Remitido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ImporteSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GastoComercial As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GastoComercialSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoAsignado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoAsignadoSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoProduccion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CostoProduccionSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Resultado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ResultadoSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrResultado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrResultadoSinIva As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
