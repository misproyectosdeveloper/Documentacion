<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaBancos
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.ButtonNuevaSucursal = New System.Windows.Forms.Button
        Me.ButtonModificarSucursal = New System.Windows.Forms.Button
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextNombre = New System.Windows.Forms.TextBox
        Me.TextSucursal = New System.Windows.Forms.TextBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ButtonNuevaCuenta = New System.Windows.Forms.Button
        Me.ButtonModificaCuenta = New System.Windows.Forms.Button
        Me.CheckLiquidaDivisa = New System.Windows.Forms.CheckBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextSaldoInicial = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextNumeracionFinal = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextNumeracionInicial = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextUltimoNumero = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextUltimaSerie = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.CheckTieneChequera = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextCbu = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboTipo = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboBancos = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.ButtonElegir = New System.Windows.Forms.Button
        Me.ButtonAgenda = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.ComboBancos)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(551, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(406, 446)
        Me.Panel1.TabIndex = 174
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(16, 48)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 13)
        Me.Label3.TabIndex = 186
        Me.Label3.Text = "Sucursal"
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.ButtonNuevaSucursal)
        Me.Panel4.Controls.Add(Me.ButtonModificarSucursal)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label1)
        Me.Panel4.Controls.Add(Me.TextNombre)
        Me.Panel4.Controls.Add(Me.TextSucursal)
        Me.Panel4.Location = New System.Drawing.Point(17, 64)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(371, 81)
        Me.Panel4.TabIndex = 185
        '
        'ButtonNuevaSucursal
        '
        Me.ButtonNuevaSucursal.BackColor = System.Drawing.Color.Yellow
        Me.ButtonNuevaSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaSucursal.Location = New System.Drawing.Point(230, 54)
        Me.ButtonNuevaSucursal.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonNuevaSucursal.Name = "ButtonNuevaSucursal"
        Me.ButtonNuevaSucursal.Size = New System.Drawing.Size(130, 20)
        Me.ButtonNuevaSucursal.TabIndex = 35
        Me.ButtonNuevaSucursal.TabStop = False
        Me.ButtonNuevaSucursal.Text = "Nueva Sucursal"
        Me.ButtonNuevaSucursal.UseVisualStyleBackColor = False
        '
        'ButtonModificarSucursal
        '
        Me.ButtonModificarSucursal.BackColor = System.Drawing.Color.Yellow
        Me.ButtonModificarSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonModificarSucursal.Location = New System.Drawing.Point(16, 54)
        Me.ButtonModificarSucursal.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonModificarSucursal.Name = "ButtonModificarSucursal"
        Me.ButtonModificarSucursal.Size = New System.Drawing.Size(128, 20)
        Me.ButtonModificarSucursal.TabIndex = 34
        Me.ButtonModificarSucursal.TabStop = False
        Me.ButtonModificarSucursal.Text = "Modificar Sucursal"
        Me.ButtonModificarSucursal.UseVisualStyleBackColor = False
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(8, 13)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "Codigo"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(160, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 24
        Me.Label1.Text = "Nombre"
        '
        'TextNombre
        '
        Me.TextNombre.BackColor = System.Drawing.Color.White
        Me.TextNombre.Enabled = False
        Me.TextNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombre.Location = New System.Drawing.Point(217, 13)
        Me.TextNombre.MaxLength = 15
        Me.TextNombre.Name = "TextNombre"
        Me.TextNombre.Size = New System.Drawing.Size(133, 20)
        Me.TextNombre.TabIndex = 22
        '
        'TextSucursal
        '
        Me.TextSucursal.BackColor = System.Drawing.Color.White
        Me.TextSucursal.Enabled = False
        Me.TextSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSucursal.Location = New System.Drawing.Point(61, 11)
        Me.TextSucursal.MaxLength = 3
        Me.TextSucursal.Name = "TextSucursal"
        Me.TextSucursal.Size = New System.Drawing.Size(74, 20)
        Me.TextSucursal.TabIndex = 21
        Me.TextSucursal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.ButtonNuevaCuenta)
        Me.Panel2.Controls.Add(Me.ButtonModificaCuenta)
        Me.Panel2.Controls.Add(Me.CheckLiquidaDivisa)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.TextSaldoInicial)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.ComboMoneda)
        Me.Panel2.Controls.Add(Me.CheckTieneChequera)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.TextCbu)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.ComboTipo)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.TextNumero)
        Me.Panel2.Location = New System.Drawing.Point(15, 166)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(373, 252)
        Me.Panel2.TabIndex = 6
        '
        'ButtonNuevaCuenta
        '
        Me.ButtonNuevaCuenta.BackColor = System.Drawing.Color.Yellow
        Me.ButtonNuevaCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevaCuenta.Location = New System.Drawing.Point(232, 224)
        Me.ButtonNuevaCuenta.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonNuevaCuenta.Name = "ButtonNuevaCuenta"
        Me.ButtonNuevaCuenta.Size = New System.Drawing.Size(130, 20)
        Me.ButtonNuevaCuenta.TabIndex = 201
        Me.ButtonNuevaCuenta.TabStop = False
        Me.ButtonNuevaCuenta.Text = "Nueva Cuenta"
        Me.ButtonNuevaCuenta.UseVisualStyleBackColor = False
        '
        'ButtonModificaCuenta
        '
        Me.ButtonModificaCuenta.BackColor = System.Drawing.Color.Yellow
        Me.ButtonModificaCuenta.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonModificaCuenta.Location = New System.Drawing.Point(18, 224)
        Me.ButtonModificaCuenta.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonModificaCuenta.Name = "ButtonModificaCuenta"
        Me.ButtonModificaCuenta.Size = New System.Drawing.Size(119, 20)
        Me.ButtonModificaCuenta.TabIndex = 200
        Me.ButtonModificaCuenta.TabStop = False
        Me.ButtonModificaCuenta.Text = "Modifica Cuenta"
        Me.ButtonModificaCuenta.UseVisualStyleBackColor = False
        '
        'CheckLiquidaDivisa
        '
        Me.CheckLiquidaDivisa.AutoSize = True
        Me.CheckLiquidaDivisa.BackColor = System.Drawing.Color.Transparent
        Me.CheckLiquidaDivisa.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckLiquidaDivisa.Enabled = False
        Me.CheckLiquidaDivisa.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.CheckLiquidaDivisa.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckLiquidaDivisa.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckLiquidaDivisa.Location = New System.Drawing.Point(236, 106)
        Me.CheckLiquidaDivisa.Name = "CheckLiquidaDivisa"
        Me.CheckLiquidaDivisa.Size = New System.Drawing.Size(109, 20)
        Me.CheckLiquidaDivisa.TabIndex = 199
        Me.CheckLiquidaDivisa.Text = "Liquida Divisa"
        Me.CheckLiquidaDivisa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckLiquidaDivisa.UseVisualStyleBackColor = False
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(5, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(64, 13)
        Me.Label12.TabIndex = 198
        Me.Label12.Text = "Saldo Inicial"
        '
        'TextSaldoInicial
        '
        Me.TextSaldoInicial.BackColor = System.Drawing.Color.White
        Me.TextSaldoInicial.Enabled = False
        Me.TextSaldoInicial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldoInicial.Location = New System.Drawing.Point(79, 47)
        Me.TextSaldoInicial.MaxLength = 10
        Me.TextSaldoInicial.Name = "TextSaldoInicial"
        Me.TextSaldoInicial.Size = New System.Drawing.Size(112, 20)
        Me.TextSaldoInicial.TabIndex = 9
        Me.TextSaldoInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextNumeracionFinal)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.TextNumeracionInicial)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.TextUltimoNumero)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.TextUltimaSerie)
        Me.Panel3.Enabled = False
        Me.Panel3.Location = New System.Drawing.Point(9, 136)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(359, 67)
        Me.Panel3.TabIndex = 196
        Me.Panel3.Visible = False
        '
        'TextNumeracionFinal
        '
        Me.TextNumeracionFinal.BackColor = System.Drawing.Color.White
        Me.TextNumeracionFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeracionFinal.Location = New System.Drawing.Point(252, 37)
        Me.TextNumeracionFinal.MaxLength = 10
        Me.TextNumeracionFinal.Name = "TextNumeracionFinal"
        Me.TextNumeracionFinal.Size = New System.Drawing.Size(90, 20)
        Me.TextNumeracionFinal.TabIndex = 32
        Me.TextNumeracionFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(219, 40)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(29, 13)
        Me.Label14.TabIndex = 200
        Me.Label14.Text = "Final"
        '
        'TextNumeracionInicial
        '
        Me.TextNumeracionInicial.BackColor = System.Drawing.Color.White
        Me.TextNumeracionInicial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeracionInicial.Location = New System.Drawing.Point(117, 36)
        Me.TextNumeracionInicial.MaxLength = 10
        Me.TextNumeracionInicial.Name = "TextNumeracionInicial"
        Me.TextNumeracionInicial.Size = New System.Drawing.Size(90, 20)
        Me.TextNumeracionInicial.TabIndex = 30
        Me.TextNumeracionInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(7, 40)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(103, 13)
        Me.Label13.TabIndex = 198
        Me.Label13.Text = "Numeración :  Inicial"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(127, 11)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(113, 13)
        Me.Label10.TabIndex = 197
        Me.Label10.Text = "Ultimo Numero Emitido"
        '
        'TextUltimoNumero
        '
        Me.TextUltimoNumero.BackColor = System.Drawing.Color.White
        Me.TextUltimoNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUltimoNumero.Location = New System.Drawing.Point(252, 7)
        Me.TextUltimoNumero.MaxLength = 10
        Me.TextUltimoNumero.Name = "TextUltimoNumero"
        Me.TextUltimoNumero.Size = New System.Drawing.Size(90, 20)
        Me.TextUltimoNumero.TabIndex = 26
        Me.TextUltimoNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(5, 11)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(63, 13)
        Me.Label9.TabIndex = 196
        Me.Label9.Text = "Ultima Serie"
        '
        'TextUltimaSerie
        '
        Me.TextUltimaSerie.BackColor = System.Drawing.Color.White
        Me.TextUltimaSerie.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextUltimaSerie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUltimaSerie.Location = New System.Drawing.Point(73, 7)
        Me.TextUltimaSerie.MaxLength = 1
        Me.TextUltimaSerie.Name = "TextUltimaSerie"
        Me.TextUltimaSerie.Size = New System.Drawing.Size(31, 20)
        Me.TextUltimaSerie.TabIndex = 24
        Me.TextUltimaSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(201, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(49, 13)
        Me.Label11.TabIndex = 195
        Me.Label11.Text = "Moneda "
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(252, 46)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(96, 21)
        Me.ComboMoneda.TabIndex = 20
        '
        'CheckTieneChequera
        '
        Me.CheckTieneChequera.AutoSize = True
        Me.CheckTieneChequera.BackColor = System.Drawing.Color.Transparent
        Me.CheckTieneChequera.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckTieneChequera.Enabled = False
        Me.CheckTieneChequera.FlatAppearance.CheckedBackColor = System.Drawing.Color.White
        Me.CheckTieneChequera.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.CheckTieneChequera.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckTieneChequera.ForeColor = System.Drawing.Color.Black
        Me.CheckTieneChequera.Location = New System.Drawing.Point(2, 106)
        Me.CheckTieneChequera.Name = "CheckTieneChequera"
        Me.CheckTieneChequera.Size = New System.Drawing.Size(121, 20)
        Me.CheckTieneChequera.TabIndex = 22
        Me.CheckTieneChequera.Text = "Tiene Chequera"
        Me.CheckTieneChequera.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckTieneChequera.UseVisualStyleBackColor = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(38, 13)
        Me.Label5.TabIndex = 188
        Me.Label5.Text = "C.B.U."
        '
        'TextCbu
        '
        Me.TextCbu.BackColor = System.Drawing.Color.White
        Me.TextCbu.Enabled = False
        Me.TextCbu.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCbu.Location = New System.Drawing.Point(80, 76)
        Me.TextCbu.MaxLength = 22
        Me.TextCbu.Name = "TextCbu"
        Me.TextCbu.Size = New System.Drawing.Size(165, 20)
        Me.TextCbu.TabIndex = 21
        Me.TextCbu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 13)
        Me.Label8.TabIndex = 182
        Me.Label8.Text = "Numero Cta."
        '
        'ComboTipo
        '
        Me.ComboTipo.BackColor = System.Drawing.Color.White
        Me.ComboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTipo.Enabled = False
        Me.ComboTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipo.FormattingEnabled = True
        Me.ComboTipo.Location = New System.Drawing.Point(296, 14)
        Me.ComboTipo.Name = "ComboTipo"
        Me.ComboTipo.Size = New System.Drawing.Size(50, 21)
        Me.ComboTipo.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(215, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 13)
        Me.Label7.TabIndex = 181
        Me.Label7.Text = "Tipo Cuenta"
        '
        'TextNumero
        '
        Me.TextNumero.BackColor = System.Drawing.Color.White
        Me.TextNumero.Enabled = False
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(79, 15)
        Me.TextNumero.MaxLength = 10
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(112, 20)
        Me.TextNumero.TabIndex = 6
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 148)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 13)
        Me.Label4.TabIndex = 184
        Me.Label4.Text = "Cuenta"
        '
        'ComboBancos
        '
        Me.ComboBancos.BackColor = System.Drawing.Color.White
        Me.ComboBancos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBancos.Enabled = False
        Me.ComboBancos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancos.FormattingEnabled = True
        Me.ComboBancos.Location = New System.Drawing.Point(129, 15)
        Me.ComboBancos.Name = "ComboBancos"
        Me.ComboBancos.Size = New System.Drawing.Size(229, 24)
        Me.ComboBancos.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(54, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Banco"
        '
        'TreeView1
        '
        Me.TreeView1.BackColor = System.Drawing.Color.White
        Me.TreeView1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView1.ForeColor = System.Drawing.Color.OliveDrab
        Me.TreeView1.FullRowSelect = True
        Me.TreeView1.Indent = 60
        Me.TreeView1.Location = New System.Drawing.Point(24, 11)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(519, 447)
        Me.TreeView1.TabIndex = 1
        '
        'ButtonElegir
        '
        Me.ButtonElegir.BackColor = System.Drawing.Color.White
        Me.ButtonElegir.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonElegir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonElegir.Location = New System.Drawing.Point(470, 475)
        Me.ButtonElegir.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonElegir.Name = "ButtonElegir"
        Me.ButtonElegir.Size = New System.Drawing.Size(114, 30)
        Me.ButtonElegir.TabIndex = 175
        Me.ButtonElegir.Text = "Aceptar "
        Me.ButtonElegir.UseVisualStyleBackColor = False
        Me.ButtonElegir.Visible = False
        '
        'ButtonAgenda
        '
        Me.ButtonAgenda.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ButtonAgenda.FlatAppearance.BorderSize = 0
        Me.ButtonAgenda.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAgenda.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAgenda.Location = New System.Drawing.Point(861, 469)
        Me.ButtonAgenda.Name = "ButtonAgenda"
        Me.ButtonAgenda.Size = New System.Drawing.Size(97, 26)
        Me.ButtonAgenda.TabIndex = 604
        Me.ButtonAgenda.Text = "Agenda"
        Me.ButtonAgenda.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAgenda.UseVisualStyleBackColor = False
        '
        'ListaBancos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(982, 514)
        Me.Controls.Add(Me.ButtonAgenda)
        Me.Controls.Add(Me.ButtonElegir)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.TreeView1)
        Me.Name = "ListaBancos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ListaBancos1"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ComboBancos As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextCbu As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents CheckTieneChequera As System.Windows.Forms.CheckBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextUltimoNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextUltimaSerie As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextSaldoInicial As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextNumeracionFinal As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextNumeracionInicial As System.Windows.Forms.TextBox
    Friend WithEvents ButtonElegir As System.Windows.Forms.Button
    Friend WithEvents ButtonAgenda As System.Windows.Forms.Button
    Friend WithEvents CheckLiquidaDivisa As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextNombre As System.Windows.Forms.TextBox
    Friend WithEvents TextSucursal As System.Windows.Forms.TextBox
    Friend WithEvents ButtonNuevaSucursal As System.Windows.Forms.Button
    Friend WithEvents ButtonModificarSucursal As System.Windows.Forms.Button
    Friend WithEvents ButtonNuevaCuenta As System.Windows.Forms.Button
    Friend WithEvents ButtonModificaCuenta As System.Windows.Forms.Button
End Class
