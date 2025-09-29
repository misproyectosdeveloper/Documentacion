<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionVentas
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionVentas))
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.CheckCerrado = New System.Windows.Forms.CheckBox
        Me.CheckAbierto = New System.Windows.Forms.CheckBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckPendientesFacturar = New System.Windows.Forms.CheckBox
        Me.CheckFacturados = New System.Windows.Forms.CheckBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboCanalVenta = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboVendedor = New System.Windows.Forms.ComboBox
        Me.CheckMuestraLotes = New System.Windows.Forms.CheckBox
        Me.CheckRepetirDatos = New System.Windows.Forms.CheckBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.Panel7 = New System.Windows.Forms.Panel
        Me.RadioFechaEntrega = New System.Windows.Forms.RadioButton
        Me.RadioFechaEmision = New System.Windows.Forms.RadioButton
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.PanelRemitos = New System.Windows.Forms.Panel
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextRemitos = New System.Windows.Forms.TextBox
        Me.Panel9 = New System.Windows.Forms.Panel
        Me.RadioTodas = New System.Windows.Forms.RadioButton
        Me.RadioSoloContables = New System.Windows.Forms.RadioButton
        Me.RadioSinContables = New System.Windows.Forms.RadioButton
        Me.CheckMuestraPedido = New System.Windows.Forms.CheckBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.ComboMarca = New System.Windows.Forms.ComboBox
        Me.CheckSoloConfirmados = New System.Windows.Forms.CheckBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.ComboCategoria = New System.Windows.Forms.ComboBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.ComboEnvase = New System.Windows.Forms.ComboBox
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.PanelRemitos.SuspendLayout()
        Me.Panel9.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(204, 269)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(65, 19)
        Me.Label2.TabIndex = 212
        Me.Label2.Text = "Variedad"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(274, 266)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(96, 24)
        Me.ComboVariedad.TabIndex = 211
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(2, 268)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 19)
        Me.Label1.TabIndex = 210
        Me.Label1.Text = "Especie"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(73, 265)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(125, 24)
        Me.ComboEspecie.TabIndex = 209
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(75, 175)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(167, 21)
        Me.ComboEstado.TabIndex = 230
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(13, 178)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 13)
        Me.Label6.TabIndex = 231
        Me.Label6.Text = "Estado"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.CheckCerrado)
        Me.Panel1.Controls.Add(Me.CheckAbierto)
        Me.Panel1.Location = New System.Drawing.Point(345, 112)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(137, 40)
        Me.Panel1.TabIndex = 232
        '
        'CheckCerrado
        '
        Me.CheckCerrado.BackColor = System.Drawing.Color.Transparent
        Me.CheckCerrado.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckCerrado.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckCerrado.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckCerrado.Image = CType(resources.GetObject("CheckCerrado.Image"), System.Drawing.Image)
        Me.CheckCerrado.Location = New System.Drawing.Point(67, 5)
        Me.CheckCerrado.Name = "CheckCerrado"
        Me.CheckCerrado.Size = New System.Drawing.Size(57, 30)
        Me.CheckCerrado.TabIndex = 230
        Me.CheckCerrado.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckCerrado.UseVisualStyleBackColor = False
        '
        'CheckAbierto
        '
        Me.CheckAbierto.BackColor = System.Drawing.Color.Transparent
        Me.CheckAbierto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckAbierto.Checked = True
        Me.CheckAbierto.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckAbierto.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckAbierto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckAbierto.Image = CType(resources.GetObject("CheckAbierto.Image"), System.Drawing.Image)
        Me.CheckAbierto.Location = New System.Drawing.Point(10, 5)
        Me.CheckAbierto.Name = "CheckAbierto"
        Me.CheckAbierto.Size = New System.Drawing.Size(54, 30)
        Me.CheckAbierto.TabIndex = 229
        Me.CheckAbierto.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckAbierto.UseVisualStyleBackColor = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(407, 496)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 233
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckPendientesFacturar)
        Me.Panel2.Controls.Add(Me.CheckFacturados)
        Me.Panel2.Location = New System.Drawing.Point(324, 165)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(251, 40)
        Me.Panel2.TabIndex = 234
        Me.Panel2.Visible = False
        '
        'CheckPendientesFacturar
        '
        Me.CheckPendientesFacturar.BackColor = System.Drawing.Color.Transparent
        Me.CheckPendientesFacturar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckPendientesFacturar.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckPendientesFacturar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckPendientesFacturar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckPendientesFacturar.Location = New System.Drawing.Point(99, 5)
        Me.CheckPendientesFacturar.Name = "CheckPendientesFacturar"
        Me.CheckPendientesFacturar.Size = New System.Drawing.Size(140, 30)
        Me.CheckPendientesFacturar.TabIndex = 230
        Me.CheckPendientesFacturar.Text = "Pendiente de Fact."
        Me.CheckPendientesFacturar.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckPendientesFacturar.UseVisualStyleBackColor = False
        '
        'CheckFacturados
        '
        Me.CheckFacturados.BackColor = System.Drawing.Color.Transparent
        Me.CheckFacturados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckFacturados.Checked = True
        Me.CheckFacturados.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CheckFacturados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckFacturados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckFacturados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckFacturados.Location = New System.Drawing.Point(10, 5)
        Me.CheckFacturados.Name = "CheckFacturados"
        Me.CheckFacturados.Size = New System.Drawing.Size(85, 30)
        Me.CheckFacturados.TabIndex = 229
        Me.CheckFacturados.Text = "Facturado"
        Me.CheckFacturados.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckFacturados.UseVisualStyleBackColor = False
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(14, 122)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(61, 13)
        Me.Label9.TabIndex = 236
        Me.Label9.Text = "Deposito "
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.BackColor = System.Drawing.Color.White
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.ComboDeposito.Location = New System.Drawing.Point(80, 118)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(141, 21)
        Me.ComboDeposito.TabIndex = 235
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.ComboAlias)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.ComboEmisor)
        Me.Panel3.Controls.Add(Me.LabelEmisor)
        Me.Panel3.Location = New System.Drawing.Point(8, 19)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(337, 70)
        Me.Panel3.TabIndex = 237
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(65, 39)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 21)
        Me.ComboAlias.TabIndex = 223
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 43)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(34, 13)
        Me.Label3.TabIndex = 225
        Me.Label3.Text = "Alias"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(66, 8)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 21)
        Me.ComboEmisor.TabIndex = 222
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(5, 11)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(46, 13)
        Me.LabelEmisor.TabIndex = 224
        Me.LabelEmisor.Text = "Cliente"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel4
        '
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.ComboCanalVenta)
        Me.Panel4.Controls.Add(Me.Label4)
        Me.Panel4.Controls.Add(Me.ComboVendedor)
        Me.Panel4.Location = New System.Drawing.Point(3, 298)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(628, 42)
        Me.Panel4.TabIndex = 238
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(299, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(102, 19)
        Me.Label5.TabIndex = 229
        Me.Label5.Text = "Canal de Venta"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label5.Visible = False
        '
        'ComboCanalVenta
        '
        Me.ComboCanalVenta.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCanalVenta.FormattingEnabled = True
        Me.ComboCanalVenta.Location = New System.Drawing.Point(406, 9)
        Me.ComboCanalVenta.Name = "ComboCanalVenta"
        Me.ComboCanalVenta.Size = New System.Drawing.Size(188, 24)
        Me.ComboCanalVenta.TabIndex = 228
        Me.ComboCanalVenta.Visible = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(2, 11)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(74, 19)
        Me.Label4.TabIndex = 227
        Me.Label4.Text = "Vendedor"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboVendedor
        '
        Me.ComboVendedor.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVendedor.FormattingEnabled = True
        Me.ComboVendedor.Location = New System.Drawing.Point(92, 10)
        Me.ComboVendedor.Name = "ComboVendedor"
        Me.ComboVendedor.Size = New System.Drawing.Size(188, 24)
        Me.ComboVendedor.TabIndex = 226
        '
        'CheckMuestraLotes
        '
        Me.CheckMuestraLotes.BackColor = System.Drawing.Color.Transparent
        Me.CheckMuestraLotes.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckMuestraLotes.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckMuestraLotes.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckMuestraLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckMuestraLotes.Location = New System.Drawing.Point(16, 214)
        Me.CheckMuestraLotes.Name = "CheckMuestraLotes"
        Me.CheckMuestraLotes.Size = New System.Drawing.Size(113, 30)
        Me.CheckMuestraLotes.TabIndex = 239
        Me.CheckMuestraLotes.Text = "Muestra Lotes"
        Me.CheckMuestraLotes.UseVisualStyleBackColor = False
        '
        'CheckRepetirDatos
        '
        Me.CheckRepetirDatos.BackColor = System.Drawing.Color.Transparent
        Me.CheckRepetirDatos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckRepetirDatos.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckRepetirDatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckRepetirDatos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckRepetirDatos.Location = New System.Drawing.Point(327, 215)
        Me.CheckRepetirDatos.Name = "CheckRepetirDatos"
        Me.CheckRepetirDatos.Size = New System.Drawing.Size(223, 30)
        Me.CheckRepetirDatos.TabIndex = 240
        Me.CheckRepetirDatos.Text = "Repetir Datos en Todas las Lineas"
        Me.CheckRepetirDatos.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckRepetirDatos.UseVisualStyleBackColor = False
        '
        'Panel5
        '
        Me.Panel5.Controls.Add(Me.ComboSucursales)
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Location = New System.Drawing.Point(536, 112)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(296, 40)
        Me.Panel5.TabIndex = 241
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(72, 8)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(188, 21)
        Me.ComboSucursales.TabIndex = 177
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(9, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 13)
        Me.Label10.TabIndex = 178
        Me.Label10.Text = "Sucursal"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel6
        '
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.Panel7)
        Me.Panel6.Controls.Add(Me.DateTimeHasta)
        Me.Panel6.Controls.Add(Me.DateTimeDesde)
        Me.Panel6.Controls.Add(Me.Label7)
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Location = New System.Drawing.Point(364, 22)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(471, 66)
        Me.Panel6.TabIndex = 242
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.RadioFechaEntrega)
        Me.Panel7.Controls.Add(Me.RadioFechaEmision)
        Me.Panel7.Location = New System.Drawing.Point(4, 3)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(124, 58)
        Me.Panel7.TabIndex = 233
        '
        'RadioFechaEntrega
        '
        Me.RadioFechaEntrega.AutoSize = True
        Me.RadioFechaEntrega.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFechaEntrega.Location = New System.Drawing.Point(13, 33)
        Me.RadioFechaEntrega.Name = "RadioFechaEntrega"
        Me.RadioFechaEntrega.Size = New System.Drawing.Size(108, 17)
        Me.RadioFechaEntrega.TabIndex = 225
        Me.RadioFechaEntrega.Text = "Fecha Entrega"
        Me.RadioFechaEntrega.UseVisualStyleBackColor = True
        '
        'RadioFechaEmision
        '
        Me.RadioFechaEmision.AutoSize = True
        Me.RadioFechaEmision.Checked = True
        Me.RadioFechaEmision.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFechaEmision.Location = New System.Drawing.Point(13, 7)
        Me.RadioFechaEmision.Name = "RadioFechaEmision"
        Me.RadioFechaEmision.Size = New System.Drawing.Size(107, 17)
        Me.RadioFechaEmision.TabIndex = 224
        Me.RadioFechaEmision.TabStop = True
        Me.RadioFechaEmision.Text = "Fecha Emision"
        Me.RadioFechaEmision.UseVisualStyleBackColor = True
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(353, 21)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(108, 20)
        Me.DateTimeHasta.TabIndex = 221
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(187, 21)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(108, 20)
        Me.DateTimeDesde.TabIndex = 220
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(306, 25)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 13)
        Me.Label7.TabIndex = 219
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(136, 24)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(43, 13)
        Me.Label8.TabIndex = 218
        Me.Label8.Text = "Desde"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelRemitos
        '
        Me.PanelRemitos.Controls.Add(Me.Label13)
        Me.PanelRemitos.Controls.Add(Me.Label12)
        Me.PanelRemitos.Controls.Add(Me.Label11)
        Me.PanelRemitos.Controls.Add(Me.TextRemitos)
        Me.PanelRemitos.Location = New System.Drawing.Point(622, 300)
        Me.PanelRemitos.Name = "PanelRemitos"
        Me.PanelRemitos.Size = New System.Drawing.Size(199, 144)
        Me.PanelRemitos.TabIndex = 248
        Me.PanelRemitos.Visible = False
        '
        'Label13
        '
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(69, 6)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(65, 19)
        Me.Label13.TabIndex = 251
        Me.Label13.Text = "Remitos"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(46, 125)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(121, 13)
        Me.Label12.TabIndex = 250
        Me.Label12.Text = "     Guias:           105785"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(41, 108)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(126, 13)
        Me.Label11.TabIndex = 249
        Me.Label11.Text = "Ej.: Remitos: 1-00000789"
        '
        'TextRemitos
        '
        Me.TextRemitos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRemitos.Location = New System.Drawing.Point(43, 28)
        Me.TextRemitos.Multiline = True
        Me.TextRemitos.Name = "TextRemitos"
        Me.TextRemitos.Size = New System.Drawing.Size(112, 73)
        Me.TextRemitos.TabIndex = 248
        '
        'Panel9
        '
        Me.Panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel9.Controls.Add(Me.RadioTodas)
        Me.Panel9.Controls.Add(Me.RadioSoloContables)
        Me.Panel9.Controls.Add(Me.RadioSinContables)
        Me.Panel9.Location = New System.Drawing.Point(294, 359)
        Me.Panel9.Name = "Panel9"
        Me.Panel9.Size = New System.Drawing.Size(317, 40)
        Me.Panel9.TabIndex = 250
        '
        'RadioTodas
        '
        Me.RadioTodas.AutoSize = True
        Me.RadioTodas.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioTodas.Location = New System.Drawing.Point(231, 10)
        Me.RadioTodas.Name = "RadioTodas"
        Me.RadioTodas.Size = New System.Drawing.Size(60, 17)
        Me.RadioTodas.TabIndex = 2
        Me.RadioTodas.Text = "Todas"
        Me.RadioTodas.UseVisualStyleBackColor = True
        '
        'RadioSoloContables
        '
        Me.RadioSoloContables.AutoSize = True
        Me.RadioSoloContables.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSoloContables.Location = New System.Drawing.Point(112, 10)
        Me.RadioSoloContables.Name = "RadioSoloContables"
        Me.RadioSoloContables.Size = New System.Drawing.Size(104, 17)
        Me.RadioSoloContables.TabIndex = 1
        Me.RadioSoloContables.Text = "Solo Contable"
        Me.RadioSoloContables.UseVisualStyleBackColor = True
        '
        'RadioSinContables
        '
        Me.RadioSinContables.AutoSize = True
        Me.RadioSinContables.Checked = True
        Me.RadioSinContables.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioSinContables.Location = New System.Drawing.Point(8, 9)
        Me.RadioSinContables.Name = "RadioSinContables"
        Me.RadioSinContables.Size = New System.Drawing.Size(97, 17)
        Me.RadioSinContables.TabIndex = 0
        Me.RadioSinContables.TabStop = True
        Me.RadioSinContables.Text = "Sin Contable"
        Me.RadioSinContables.UseVisualStyleBackColor = True
        '
        'CheckMuestraPedido
        '
        Me.CheckMuestraPedido.BackColor = System.Drawing.Color.Transparent
        Me.CheckMuestraPedido.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckMuestraPedido.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckMuestraPedido.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckMuestraPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckMuestraPedido.Location = New System.Drawing.Point(137, 215)
        Me.CheckMuestraPedido.Name = "CheckMuestraPedido"
        Me.CheckMuestraPedido.Size = New System.Drawing.Size(156, 30)
        Me.CheckMuestraPedido.TabIndex = 251
        Me.CheckMuestraPedido.Text = "Muestra Pedido Cliente"
        Me.CheckMuestraPedido.UseVisualStyleBackColor = False
        Me.CheckMuestraPedido.Visible = False
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(380, 269)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(49, 19)
        Me.Label14.TabIndex = 253
        Me.Label14.Text = "Marca"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboMarca
        '
        Me.ComboMarca.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMarca.FormattingEnabled = True
        Me.ComboMarca.Location = New System.Drawing.Point(431, 266)
        Me.ComboMarca.Name = "ComboMarca"
        Me.ComboMarca.Size = New System.Drawing.Size(96, 24)
        Me.ComboMarca.TabIndex = 252
        '
        'CheckSoloConfirmados
        '
        Me.CheckSoloConfirmados.BackColor = System.Drawing.Color.Transparent
        Me.CheckSoloConfirmados.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.CheckSoloConfirmados.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.CheckSoloConfirmados.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckSoloConfirmados.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckSoloConfirmados.Location = New System.Drawing.Point(589, 215)
        Me.CheckSoloConfirmados.Name = "CheckSoloConfirmados"
        Me.CheckSoloConfirmados.Size = New System.Drawing.Size(136, 30)
        Me.CheckSoloConfirmados.TabIndex = 254
        Me.CheckSoloConfirmados.Text = "Solo Confirmados"
        Me.CheckSoloConfirmados.UseVisualStyleBackColor = False
        '
        'Label15
        '
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(534, 269)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(65, 19)
        Me.Label15.TabIndex = 256
        Me.Label15.Text = "Categoria"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCategoria
        '
        Me.ComboCategoria.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCategoria.FormattingEnabled = True
        Me.ComboCategoria.Location = New System.Drawing.Point(606, 266)
        Me.ComboCategoria.Name = "ComboCategoria"
        Me.ComboCategoria.Size = New System.Drawing.Size(96, 24)
        Me.ComboCategoria.TabIndex = 255
        '
        'Label16
        '
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(712, 269)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(52, 19)
        Me.Label16.TabIndex = 258
        Me.Label16.Text = "Envase"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ComboEnvase
        '
        Me.ComboEnvase.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEnvase.FormattingEnabled = True
        Me.ComboEnvase.Location = New System.Drawing.Point(768, 266)
        Me.ComboEnvase.Name = "ComboEnvase"
        Me.ComboEnvase.Size = New System.Drawing.Size(96, 24)
        Me.ComboEnvase.TabIndex = 257
        '
        'OpcionVentas
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.ClientSize = New System.Drawing.Size(885, 551)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.ComboEnvase)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.ComboCategoria)
        Me.Controls.Add(Me.CheckSoloConfirmados)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.ComboMarca)
        Me.Controls.Add(Me.CheckMuestraPedido)
        Me.Controls.Add(Me.Panel9)
        Me.Controls.Add(Me.PanelRemitos)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.CheckRepetirDatos)
        Me.Controls.Add(Me.CheckMuestraLotes)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboVariedad)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboEspecie)
        Me.Name = "OpcionVentas"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opciones"
        Me.Panel1.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.PanelRemitos.ResumeLayout(False)
        Me.PanelRemitos.PerformLayout()
        Me.Panel9.ResumeLayout(False)
        Me.Panel9.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents CheckCerrado As System.Windows.Forms.CheckBox
    Friend WithEvents CheckAbierto As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CheckPendientesFacturar As System.Windows.Forms.CheckBox
    Friend WithEvents CheckFacturados As System.Windows.Forms.CheckBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboCanalVenta As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboVendedor As System.Windows.Forms.ComboBox
    Friend WithEvents CheckMuestraLotes As System.Windows.Forms.CheckBox
    Friend WithEvents CheckRepetirDatos As System.Windows.Forms.CheckBox
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Panel7 As System.Windows.Forms.Panel
    Friend WithEvents RadioFechaEntrega As System.Windows.Forms.RadioButton
    Friend WithEvents RadioFechaEmision As System.Windows.Forms.RadioButton
    Friend WithEvents PanelRemitos As System.Windows.Forms.Panel
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents TextRemitos As System.Windows.Forms.TextBox
    Friend WithEvents Panel9 As System.Windows.Forms.Panel
    Friend WithEvents RadioTodas As System.Windows.Forms.RadioButton
    Friend WithEvents RadioSoloContables As System.Windows.Forms.RadioButton
    Friend WithEvents RadioSinContables As System.Windows.Forms.RadioButton
    Friend WithEvents CheckMuestraPedido As System.Windows.Forms.CheckBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents ComboMarca As System.Windows.Forms.ComboBox
    Friend WithEvents CheckSoloConfirmados As System.Windows.Forms.CheckBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents ComboCategoria As System.Windows.Forms.ComboBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents ComboEnvase As System.Windows.Forms.ComboBox
End Class
