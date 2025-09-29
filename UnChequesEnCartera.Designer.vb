<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnChequesEnCartera
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnChequesEnCartera))
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonActualizar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckSoloeCheq = New System.Windows.Forms.CheckBox
        Me.CheckSoloRechazados = New System.Windows.Forms.CheckBox
        Me.ComboFondoFijo = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.CheckAfectado = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextCaja = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.ComboCliente = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBanco = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboProveedor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.CheckEntregados = New System.Windows.Forms.CheckBox
        Me.CheckEnCartera = New System.Windows.Forms.CheckBox
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonExcel = New System.Windows.Forms.Button
        Me.Estado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OrdAnt = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.ColorTexto = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.VencimientoAnt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EstadoCh = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Color = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.Sel2 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Vencimiento = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Banco = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.eCheq = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.eCheqAnt = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Numero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Moneda = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.FechaRecibido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EmisorRecibido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FechaEntregado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Entregado = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EmisorCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ORD = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.ClaveCheque = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cartel = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Caja = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Estado, Me.OrdAnt, Me.ColorTexto, Me.VencimientoAnt, Me.EstadoCh, Me.Color, Me.Operacion, Me.Candado, Me.Sel2, Me.Vencimiento, Me.Banco, Me.eCheq, Me.eCheqAnt, Me.Importe, Me.Numero, Me.Moneda, Me.FechaRecibido, Me.EmisorRecibido, Me.FechaEntregado, Me.Entregado, Me.EmisorCheque, Me.ORD, Me.ClaveCheque, Me.Cartel, Me.Caja})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle11
        Me.Grid.Location = New System.Drawing.Point(5, 64)
        Me.Grid.Margin = New System.Windows.Forms.Padding(0)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.RowHeadersWidth = 20
        DataGridViewCellStyle13.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle13.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle13.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle13.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle13
        Me.Grid.Size = New System.Drawing.Size(1245, 626)
        Me.Grid.TabIndex = 260
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Wheat
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(4, 697)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 21)
        Me.ButtonPrimero.TabIndex = 269
        Me.ButtonPrimero.TabStop = False
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Wheat
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(38, 697)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 21)
        Me.ButtonAnterior.TabIndex = 270
        Me.ButtonAnterior.TabStop = False
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Wheat
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(106, 697)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 21)
        Me.ButtonUltimo.TabIndex = 268
        Me.ButtonUltimo.TabStop = False
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Wheat
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(72, 697)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 21)
        Me.ButtonPosterior.TabIndex = 271
        Me.ButtonPosterior.TabStop = False
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonActualizar
        '
        Me.ButtonActualizar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonActualizar.Location = New System.Drawing.Point(1095, 695)
        Me.ButtonActualizar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonActualizar.Name = "ButtonActualizar"
        Me.ButtonActualizar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonActualizar.TabIndex = 274
        Me.ButtonActualizar.Text = "Aceptar Cambios"
        Me.ButtonActualizar.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckSoloeCheq)
        Me.Panel1.Controls.Add(Me.CheckSoloRechazados)
        Me.Panel1.Controls.Add(Me.ComboFondoFijo)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.CheckAfectado)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.TextCaja)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.ComboCliente)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboBanco)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboProveedor)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Controls.Add(Me.CheckEntregados)
        Me.Panel1.Controls.Add(Me.CheckEnCartera)
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Location = New System.Drawing.Point(5, 4)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1245, 55)
        Me.Panel1.TabIndex = 275
        '
        'CheckSoloeCheq
        '
        Me.CheckSoloeCheq.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSoloeCheq.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSoloeCheq.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSoloeCheq.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSoloeCheq.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSoloeCheq.Location = New System.Drawing.Point(790, 20)
        Me.CheckSoloeCheq.Name = "CheckSoloeCheq"
        Me.CheckSoloeCheq.Size = New System.Drawing.Size(102, 30)
        Me.CheckSoloeCheq.TabIndex = 306
        Me.CheckSoloeCheq.Text = "Solo eCheq"
        Me.CheckSoloeCheq.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckSoloeCheq.UseVisualStyleBackColor = False
        '
        'CheckSoloRechazados
        '
        Me.CheckSoloRechazados.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckSoloRechazados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSoloRechazados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSoloRechazados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSoloRechazados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSoloRechazados.Location = New System.Drawing.Point(522, 21)
        Me.CheckSoloRechazados.Name = "CheckSoloRechazados"
        Me.CheckSoloRechazados.Size = New System.Drawing.Size(127, 30)
        Me.CheckSoloRechazados.TabIndex = 305
        Me.CheckSoloRechazados.Text = "Solo Rechazados"
        Me.CheckSoloRechazados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckSoloRechazados.UseVisualStyleBackColor = False
        '
        'ComboFondoFijo
        '
        Me.ComboFondoFijo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboFondoFijo.FormattingEnabled = True
        Me.ComboFondoFijo.Location = New System.Drawing.Point(692, 2)
        Me.ComboFondoFijo.Name = "ComboFondoFijo"
        Me.ComboFondoFijo.Size = New System.Drawing.Size(151, 21)
        Me.ComboFondoFijo.TabIndex = 303
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(616, 6)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(66, 13)
        Me.Label6.TabIndex = 304
        Me.Label6.Text = "Fondo Fijo"
        '
        'CheckAfectado
        '
        Me.CheckAfectado.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckAfectado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAfectado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAfectado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAfectado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckAfectado.Location = New System.Drawing.Point(655, 20)
        Me.CheckAfectado.Name = "CheckAfectado"
        Me.CheckAfectado.Size = New System.Drawing.Size(117, 30)
        Me.CheckAfectado.TabIndex = 302
        Me.CheckAfectado.Text = "Ver Afectados"
        Me.CheckAfectado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAfectado.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(125, 28)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(152, 20)
        Me.Label5.TabIndex = 301
        Me.Label5.Text = "(999 muestra todas las cajas)"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(0, 31)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 300
        Me.Label3.Text = "Caja"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCaja
        '
        Me.TextCaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCaja.Location = New System.Drawing.Point(64, 28)
        Me.TextCaja.MaxLength = 3
        Me.TextCaja.Name = "TextCaja"
        Me.TextCaja.Size = New System.Drawing.Size(55, 20)
        Me.TextCaja.TabIndex = 299
        Me.TextCaja.TabStop = False
        Me.TextCaja.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(1026, 6)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 13)
        Me.Label7.TabIndex = 298
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(849, 6)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 297
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(1073, 3)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(105, 20)
        Me.DateTimeHasta.TabIndex = 296
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(899, 2)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(112, 20)
        Me.DateTimeDesde.TabIndex = 295
        '
        'ComboCliente
        '
        Me.ComboCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCliente.FormattingEnabled = True
        Me.ComboCliente.Location = New System.Drawing.Point(63, 2)
        Me.ComboCliente.Name = "ComboCliente"
        Me.ComboCliente.Size = New System.Drawing.Size(138, 21)
        Me.ComboCliente.TabIndex = 283
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(-1, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(63, 13)
        Me.Label1.TabIndex = 284
        Me.Label1.Text = "Cliente"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboBanco
        '
        Me.ComboBanco.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBanco.FormattingEnabled = True
        Me.ComboBanco.Location = New System.Drawing.Point(476, 2)
        Me.ComboBanco.Name = "ComboBanco"
        Me.ComboBanco.Size = New System.Drawing.Size(123, 21)
        Me.ComboBanco.TabIndex = 281
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(427, 6)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 13)
        Me.Label2.TabIndex = 282
        Me.Label2.Text = "Banco"
        '
        'ComboProveedor
        '
        Me.ComboProveedor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboProveedor.FormattingEnabled = True
        Me.ComboProveedor.Location = New System.Drawing.Point(277, 2)
        Me.ComboProveedor.Name = "ComboProveedor"
        Me.ComboProveedor.Size = New System.Drawing.Size(138, 21)
        Me.ComboProveedor.TabIndex = 279
        '
        'LabelEmisor
        '
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(205, 6)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(70, 13)
        Me.LabelEmisor.TabIndex = 280
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CheckEntregados
        '
        Me.CheckEntregados.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckEntregados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckEntregados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckEntregados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckEntregados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEntregados.Location = New System.Drawing.Point(421, 21)
        Me.CheckEntregados.Name = "CheckEntregados"
        Me.CheckEntregados.Size = New System.Drawing.Size(89, 30)
        Me.CheckEntregados.TabIndex = 278
        Me.CheckEntregados.Text = "Entregados"
        Me.CheckEntregados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckEntregados.UseVisualStyleBackColor = False
        '
        'CheckEnCartera
        '
        Me.CheckEnCartera.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckEnCartera.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckEnCartera.Checked = True
        Me.CheckEnCartera.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckEnCartera.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckEnCartera.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckEnCartera.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckEnCartera.Location = New System.Drawing.Point(309, 21)
        Me.CheckEnCartera.Name = "CheckEnCartera"
        Me.CheckEnCartera.Size = New System.Drawing.Size(84, 30)
        Me.CheckEnCartera.TabIndex = 277
        Me.CheckEnCartera.Text = "En Cartera"
        Me.CheckEnCartera.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckEnCartera.UseVisualStyleBackColor = False
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(999, 20)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(36, 30)
        Me.CheckCerrado.TabIndex = 276
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Gainsboro
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(957, 20)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(35, 30)
        Me.CheckAbierto.TabIndex = 275
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1075, 29)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(117, 22)
        Me.ButtonAceptar.TabIndex = 274
        Me.ButtonAceptar.TabStop = False
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonExcel
        '
        Me.ButtonExcel.AutoEllipsis = True
        Me.ButtonExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExcel.Location = New System.Drawing.Point(504, 698)
        Me.ButtonExcel.Name = "ButtonExcel"
        Me.ButtonExcel.Size = New System.Drawing.Size(191, 32)
        Me.ButtonExcel.TabIndex = 276
        Me.ButtonExcel.Text = "Exportar a EXCEL"
        Me.ButtonExcel.UseVisualStyleBackColor = False
        '
        'Estado
        '
        Me.Estado.DataPropertyName = "Estado"
        Me.Estado.HeaderText = "Estado"
        Me.Estado.Name = "Estado"
        Me.Estado.Visible = False
        '
        'OrdAnt
        '
        Me.OrdAnt.DataPropertyName = "OrdAnt"
        Me.OrdAnt.HeaderText = "OrdAnt"
        Me.OrdAnt.Name = "OrdAnt"
        Me.OrdAnt.Visible = False
        '
        'ColorTexto
        '
        Me.ColorTexto.DataPropertyName = "ColorTexto"
        Me.ColorTexto.HeaderText = "Colortexto"
        Me.ColorTexto.Name = "ColorTexto"
        Me.ColorTexto.Visible = False
        '
        'VencimientoAnt
        '
        Me.VencimientoAnt.DataPropertyName = "VencimientoAnt"
        Me.VencimientoAnt.HeaderText = "VencimientoAnt"
        Me.VencimientoAnt.Name = "VencimientoAnt"
        Me.VencimientoAnt.Visible = False
        '
        'EstadoCh
        '
        Me.EstadoCh.DataPropertyName = "EstadoCh"
        Me.EstadoCh.HeaderText = "Estado"
        Me.EstadoCh.Name = "EstadoCh"
        Me.EstadoCh.Visible = False
        '
        'Color
        '
        Me.Color.DataPropertyName = "Color"
        Me.Color.HeaderText = "Color"
        Me.Color.Name = "Color"
        Me.Color.Visible = False
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        '
        'Candado
        '
        Me.Candado.HeaderText = ""
        Me.Candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Candado.MinimumWidth = 30
        Me.Candado.Name = "Candado"
        Me.Candado.Width = 30
        '
        'Sel2
        '
        Me.Sel2.HeaderText = ""
        Me.Sel2.MinimumWidth = 30
        Me.Sel2.Name = "Sel2"
        Me.Sel2.Width = 30
        '
        'Vencimiento
        '
        Me.Vencimiento.DataPropertyName = "Vencimiento"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Vencimiento.DefaultCellStyle = DataGridViewCellStyle1
        Me.Vencimiento.HeaderText = "Fecha"
        Me.Vencimiento.MinimumWidth = 80
        Me.Vencimiento.Name = "Vencimiento"
        Me.Vencimiento.ReadOnly = True
        Me.Vencimiento.Width = 80
        '
        'Banco
        '
        Me.Banco.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Banco.DataPropertyName = "Banco"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Banco.DefaultCellStyle = DataGridViewCellStyle2
        Me.Banco.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Banco.HeaderText = "Banco"
        Me.Banco.MinimumWidth = 100
        Me.Banco.Name = "Banco"
        Me.Banco.ReadOnly = True
        '
        'eCheq
        '
        Me.eCheq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.eCheq.DataPropertyName = "eCheq"
        Me.eCheq.HeaderText = "eCheq"
        Me.eCheq.MinimumWidth = 40
        Me.eCheq.Name = "eCheq"
        Me.eCheq.ReadOnly = True
        Me.eCheq.Width = 40
        '
        'eCheqAnt
        '
        Me.eCheqAnt.DataPropertyName = "eCheqAnt"
        Me.eCheqAnt.HeaderText = "eCheqAnt"
        Me.eCheqAnt.Name = "eCheqAnt"
        Me.eCheqAnt.ReadOnly = True
        Me.eCheqAnt.Visible = False
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle3
        Me.Importe.HeaderText = "Importe"
        Me.Importe.MaxInputLength = 12
        Me.Importe.MinimumWidth = 100
        Me.Importe.Name = "Importe"
        Me.Importe.ReadOnly = True
        '
        'Numero
        '
        Me.Numero.DataPropertyName = "Numero"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Numero.DefaultCellStyle = DataGridViewCellStyle4
        Me.Numero.HeaderText = "Numero"
        Me.Numero.MaxInputLength = 10
        Me.Numero.MinimumWidth = 90
        Me.Numero.Name = "Numero"
        Me.Numero.ReadOnly = True
        Me.Numero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Numero.Width = 90
        '
        'Moneda
        '
        Me.Moneda.DataPropertyName = "Moneda"
        Me.Moneda.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Moneda.HeaderText = "Moneda"
        Me.Moneda.MinimumWidth = 70
        Me.Moneda.Name = "Moneda"
        Me.Moneda.ReadOnly = True
        Me.Moneda.Visible = False
        Me.Moneda.Width = 70
        '
        'FechaRecibido
        '
        Me.FechaRecibido.DataPropertyName = "FechaRecibido"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.FechaRecibido.DefaultCellStyle = DataGridViewCellStyle5
        Me.FechaRecibido.HeaderText = "Recibido"
        Me.FechaRecibido.MinimumWidth = 80
        Me.FechaRecibido.Name = "FechaRecibido"
        Me.FechaRecibido.ReadOnly = True
        Me.FechaRecibido.Width = 80
        '
        'EmisorRecibido
        '
        Me.EmisorRecibido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.EmisorRecibido.DataPropertyName = "EmisorRecibido"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EmisorRecibido.DefaultCellStyle = DataGridViewCellStyle6
        Me.EmisorRecibido.HeaderText = "Recibido de"
        Me.EmisorRecibido.MinimumWidth = 140
        Me.EmisorRecibido.Name = "EmisorRecibido"
        Me.EmisorRecibido.ReadOnly = True
        Me.EmisorRecibido.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.EmisorRecibido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.EmisorRecibido.Width = 140
        '
        'FechaEntregado
        '
        Me.FechaEntregado.DataPropertyName = "FechaEntregado"
        Me.FechaEntregado.HeaderText = "Entregado"
        Me.FechaEntregado.MinimumWidth = 80
        Me.FechaEntregado.Name = "FechaEntregado"
        Me.FechaEntregado.ReadOnly = True
        Me.FechaEntregado.Width = 80
        '
        'Entregado
        '
        Me.Entregado.DataPropertyName = "EmisorEntregado"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Entregado.DefaultCellStyle = DataGridViewCellStyle7
        Me.Entregado.HeaderText = "Entregado A"
        Me.Entregado.MinimumWidth = 140
        Me.Entregado.Name = "Entregado"
        Me.Entregado.ReadOnly = True
        Me.Entregado.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Entregado.Width = 140
        '
        'EmisorCheque
        '
        Me.EmisorCheque.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.EmisorCheque.DataPropertyName = "EmisorCheque"
        Me.EmisorCheque.HeaderText = "Emisor"
        Me.EmisorCheque.MinimumWidth = 20
        Me.EmisorCheque.Name = "EmisorCheque"
        Me.EmisorCheque.ReadOnly = True
        Me.EmisorCheque.Width = 20
        '
        'ORD
        '
        Me.ORD.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.ORD.DataPropertyName = "Ord"
        Me.ORD.HeaderText = "OR."
        Me.ORD.MinimumWidth = 30
        Me.ORD.Name = "ORD"
        Me.ORD.Width = 30
        '
        'ClaveCheque
        '
        Me.ClaveCheque.DataPropertyName = "ClaveCheque"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.ClaveCheque.DefaultCellStyle = DataGridViewCellStyle8
        Me.ClaveCheque.HeaderText = "Clave"
        Me.ClaveCheque.MinimumWidth = 70
        Me.ClaveCheque.Name = "ClaveCheque"
        Me.ClaveCheque.ReadOnly = True
        Me.ClaveCheque.Width = 70
        '
        'Cartel
        '
        Me.Cartel.DataPropertyName = "Cartel"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Cartel.DefaultCellStyle = DataGridViewCellStyle9
        Me.Cartel.HeaderText = ""
        Me.Cartel.MinimumWidth = 130
        Me.Cartel.Name = "Cartel"
        Me.Cartel.Width = 130
        '
        'Caja
        '
        Me.Caja.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Caja.DataPropertyName = "Caja"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomCenter
        Me.Caja.DefaultCellStyle = DataGridViewCellStyle10
        Me.Caja.HeaderText = "Caja"
        Me.Caja.MinimumWidth = 40
        Me.Caja.Name = "Caja"
        Me.Caja.Width = 40
        '
        'UnChequesEnCartera
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(1254, 736)
        Me.Controls.Add(Me.ButtonExcel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonActualizar)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "UnChequesEnCartera"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cheques En Cartera"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonActualizar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents CheckEnCartera As System.Windows.Forms.CheckBox
    Friend WithEvents CheckEntregados As System.Windows.Forms.CheckBox
    Friend WithEvents ComboProveedor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ComboBanco As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboCliente As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextCaja As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ButtonExcel As System.Windows.Forms.Button
    Friend WithEvents CheckAfectado As System.Windows.Forms.CheckBox
    Friend WithEvents ComboFondoFijo As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents CheckSoloRechazados As System.Windows.Forms.CheckBox
    Friend WithEvents CheckSoloeCheq As System.Windows.Forms.CheckBox
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrdAnt As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ColorTexto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents VencimientoAnt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EstadoCh As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Color As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Sel2 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Vencimiento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Banco As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents eCheq As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents eCheqAnt As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Numero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Moneda As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents FechaRecibido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmisorRecibido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FechaEntregado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Entregado As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EmisorCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ORD As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ClaveCheque As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cartel As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Caja As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
