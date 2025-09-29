<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionInformeLotesReventasFacturados
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
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.DateTimeHasta = New System.Windows.Forms.DateTimePicker
        Me.DateTimeDesde = New System.Windows.Forms.DateTimePicker
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.RadioPendientes = New System.Windows.Forms.RadioButton
        Me.RadioFacturados = New System.Windows.Forms.RadioButton
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.CheckBoxTodos = New System.Windows.Forms.CheckBox
        Me.CheckBoxConsignacion = New System.Windows.Forms.CheckBox
        Me.CheckBoxReventa = New System.Windows.Forms.CheckBox
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.Label12 = New System.Windows.Forms.Label
        Me.ComboDuenio = New System.Windows.Forms.ComboBox
        Me.ComboMarca = New System.Windows.Forms.ComboBox
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboEnvase = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboCategoria = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.Panel5 = New System.Windows.Forms.Panel
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.PanelRemitos = New System.Windows.Forms.Panel
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextRemitos = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.PanelRemitos.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(235, 43)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 24)
        Me.ComboAlias.TabIndex = 153
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(143, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 16)
        Me.Label2.TabIndex = 155
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(236, 15)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 24)
        Me.ComboEmisor.TabIndex = 152
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(142, 18)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(81, 16)
        Me.LabelEmisor.TabIndex = 154
        Me.LabelEmisor.Text = "Proveedor"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(266, 614)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 203
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.DateTimeHasta)
        Me.Panel1.Controls.Add(Me.DateTimeDesde)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(104, 160)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(476, 45)
        Me.Panel1.TabIndex = 204
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 16)
        Me.Label1.TabIndex = 164
        Me.Label1.Text = "Ingreso"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DateTimeHasta
        '
        Me.DateTimeHasta.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeHasta.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeHasta.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeHasta.Location = New System.Drawing.Point(339, 11)
        Me.DateTimeHasta.Name = "DateTimeHasta"
        Me.DateTimeHasta.Size = New System.Drawing.Size(117, 22)
        Me.DateTimeHasta.TabIndex = 163
        '
        'DateTimeDesde
        '
        Me.DateTimeDesde.CustomFormat = "dd/MM/yyyy"
        Me.DateTimeDesde.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeDesde.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTimeDesde.Location = New System.Drawing.Point(149, 10)
        Me.DateTimeDesde.Name = "DateTimeDesde"
        Me.DateTimeDesde.Size = New System.Drawing.Size(114, 22)
        Me.DateTimeDesde.TabIndex = 162
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(282, 14)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(49, 16)
        Me.Label7.TabIndex = 161
        Me.Label7.Text = "Hasta"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(85, 13)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(54, 16)
        Me.Label4.TabIndex = 160
        Me.Label4.Text = "Desde"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.RadioPendientes)
        Me.Panel2.Controls.Add(Me.RadioFacturados)
        Me.Panel2.Location = New System.Drawing.Point(148, 256)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(392, 40)
        Me.Panel2.TabIndex = 205
        '
        'RadioPendientes
        '
        Me.RadioPendientes.AutoSize = True
        Me.RadioPendientes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioPendientes.Location = New System.Drawing.Point(246, 8)
        Me.RadioPendientes.Name = "RadioPendientes"
        Me.RadioPendientes.Size = New System.Drawing.Size(104, 20)
        Me.RadioPendientes.TabIndex = 206
        Me.RadioPendientes.TabStop = True
        Me.RadioPendientes.Text = "Pendientes"
        Me.RadioPendientes.UseVisualStyleBackColor = True
        '
        'RadioFacturados
        '
        Me.RadioFacturados.AutoSize = True
        Me.RadioFacturados.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RadioFacturados.Location = New System.Drawing.Point(35, 8)
        Me.RadioFacturados.Name = "RadioFacturados"
        Me.RadioFacturados.Size = New System.Drawing.Size(186, 20)
        Me.RadioFacturados.TabIndex = 205
        Me.RadioFacturados.TabStop = True
        Me.RadioFacturados.Text = "Facturados/Liquidados"
        Me.RadioFacturados.UseVisualStyleBackColor = True
        '
        'Panel3
        '
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.CheckBoxTodos)
        Me.Panel3.Controls.Add(Me.CheckBoxConsignacion)
        Me.Panel3.Controls.Add(Me.CheckBoxReventa)
        Me.Panel3.Location = New System.Drawing.Point(147, 213)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(392, 37)
        Me.Panel3.TabIndex = 206
        '
        'CheckBoxTodos
        '
        Me.CheckBoxTodos.AutoSize = True
        Me.CheckBoxTodos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxTodos.Location = New System.Drawing.Point(286, 5)
        Me.CheckBoxTodos.Name = "CheckBoxTodos"
        Me.CheckBoxTodos.Size = New System.Drawing.Size(72, 20)
        Me.CheckBoxTodos.TabIndex = 2
        Me.CheckBoxTodos.Text = "Todos"
        Me.CheckBoxTodos.UseVisualStyleBackColor = True
        '
        'CheckBoxConsignacion
        '
        Me.CheckBoxConsignacion.AutoSize = True
        Me.CheckBoxConsignacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxConsignacion.Location = New System.Drawing.Point(142, 6)
        Me.CheckBoxConsignacion.Name = "CheckBoxConsignacion"
        Me.CheckBoxConsignacion.Size = New System.Drawing.Size(121, 20)
        Me.CheckBoxConsignacion.TabIndex = 1
        Me.CheckBoxConsignacion.Text = "Consignación"
        Me.CheckBoxConsignacion.UseVisualStyleBackColor = True
        '
        'CheckBoxReventa
        '
        Me.CheckBoxReventa.AutoSize = True
        Me.CheckBoxReventa.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxReventa.Location = New System.Drawing.Point(12, 5)
        Me.CheckBoxReventa.Name = "CheckBoxReventa"
        Me.CheckBoxReventa.Size = New System.Drawing.Size(85, 20)
        Me.CheckBoxReventa.TabIndex = 0
        Me.CheckBoxReventa.Text = "Reventa"
        Me.CheckBoxReventa.UseVisualStyleBackColor = True
        '
        'Panel4
        '
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label12)
        Me.Panel4.Controls.Add(Me.ComboDuenio)
        Me.Panel4.Controls.Add(Me.ComboMarca)
        Me.Panel4.Controls.Add(Me.ComboDeposito)
        Me.Panel4.Controls.Add(Me.Label6)
        Me.Panel4.Controls.Add(Me.Label5)
        Me.Panel4.Controls.Add(Me.ComboEnvase)
        Me.Panel4.Controls.Add(Me.Label3)
        Me.Panel4.Controls.Add(Me.ComboCategoria)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Controls.Add(Me.Label9)
        Me.Panel4.Controls.Add(Me.ComboVariedad)
        Me.Panel4.Controls.Add(Me.Label10)
        Me.Panel4.Controls.Add(Me.ComboEspecie)
        Me.Panel4.Location = New System.Drawing.Point(33, 446)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(609, 151)
        Me.Panel4.TabIndex = 219
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(424, 99)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(145, 19)
        Me.Label12.TabIndex = 232
        Me.Label12.Text = "Dueño Envase"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDuenio
        '
        Me.ComboDuenio.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDuenio.FormattingEnabled = True
        Me.ComboDuenio.Location = New System.Drawing.Point(402, 118)
        Me.ComboDuenio.Name = "ComboDuenio"
        Me.ComboDuenio.Size = New System.Drawing.Size(187, 24)
        Me.ComboDuenio.TabIndex = 231
        '
        'ComboMarca
        '
        Me.ComboMarca.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMarca.FormattingEnabled = True
        Me.ComboMarca.Location = New System.Drawing.Point(422, 25)
        Me.ComboMarca.Name = "ComboMarca"
        Me.ComboMarca.Size = New System.Drawing.Size(173, 24)
        Me.ComboMarca.TabIndex = 230
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(8, 121)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(184, 21)
        Me.ComboDeposito.TabIndex = 229
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(66, 103)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(57, 13)
        Me.Label6.TabIndex = 228
        Me.Label6.Text = "Deposito"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(291, 52)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(145, 19)
        Me.Label5.TabIndex = 227
        Me.Label5.Text = "Envase"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEnvase
        '
        Me.ComboEnvase.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEnvase.FormattingEnabled = True
        Me.ComboEnvase.Location = New System.Drawing.Point(214, 73)
        Me.ComboEnvase.Name = "ComboEnvase"
        Me.ComboEnvase.Size = New System.Drawing.Size(294, 24)
        Me.ComboEnvase.TabIndex = 226
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(18, 53)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(161, 19)
        Me.Label3.TabIndex = 225
        Me.Label3.Text = "Categoria"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCategoria
        '
        Me.ComboCategoria.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCategoria.FormattingEnabled = True
        Me.ComboCategoria.Location = New System.Drawing.Point(6, 74)
        Me.ComboCategoria.Name = "ComboCategoria"
        Me.ComboCategoria.Size = New System.Drawing.Size(186, 24)
        Me.ComboCategoria.TabIndex = 224
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(421, 3)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(167, 19)
        Me.Label8.TabIndex = 223
        Me.Label8.Text = "Marca"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(215, 3)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(177, 19)
        Me.Label9.TabIndex = 222
        Me.Label9.Text = "Variedad"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(215, 24)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(179, 24)
        Me.ComboVariedad.TabIndex = 221
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(8, 3)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(183, 19)
        Me.Label10.TabIndex = 220
        Me.Label10.Text = "Especie"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(6, 24)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(187, 24)
        Me.ComboEspecie.TabIndex = 219
        '
        'Panel5
        '
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.ComboCosteo)
        Me.Panel5.Controls.Add(Me.Label11)
        Me.Panel5.Location = New System.Drawing.Point(134, 117)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(391, 36)
        Me.Panel5.TabIndex = 222
        Me.Panel5.Visible = False
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(100, 6)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(263, 24)
        Me.ComboCosteo.TabIndex = 222
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(19, 10)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(57, 16)
        Me.Label11.TabIndex = 223
        Me.Label11.Text = "Costeo"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel6
        '
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.ComboSucursales)
        Me.Panel6.Controls.Add(Me.Label13)
        Me.Panel6.Location = New System.Drawing.Point(134, 74)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(391, 36)
        Me.Panel6.TabIndex = 303
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(100, 4)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(263, 28)
        Me.ComboSucursales.TabIndex = 303
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(14, 11)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(79, 20)
        Me.Label13.TabIndex = 304
        Me.Label13.Text = "Sucursal"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelRemitos
        '
        Me.PanelRemitos.Controls.Add(Me.Label15)
        Me.PanelRemitos.Controls.Add(Me.Label16)
        Me.PanelRemitos.Controls.Add(Me.Label14)
        Me.PanelRemitos.Controls.Add(Me.TextRemitos)
        Me.PanelRemitos.Location = New System.Drawing.Point(289, 308)
        Me.PanelRemitos.Name = "PanelRemitos"
        Me.PanelRemitos.Size = New System.Drawing.Size(151, 129)
        Me.PanelRemitos.TabIndex = 304
        Me.PanelRemitos.Visible = False
        '
        'Label14
        '
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(15, 3)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(124, 19)
        Me.Label14.TabIndex = 251
        Me.Label14.Text = "Remitos/Guias Prov."
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextRemitos
        '
        Me.TextRemitos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRemitos.Location = New System.Drawing.Point(18, 23)
        Me.TextRemitos.Multiline = True
        Me.TextRemitos.Name = "TextRemitos"
        Me.TextRemitos.Size = New System.Drawing.Size(112, 63)
        Me.TextRemitos.TabIndex = 248
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(16, 113)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(121, 13)
        Me.Label15.TabIndex = 253
        Me.Label15.Text = "     Guias:           105785"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(12, 96)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(126, 13)
        Me.Label16.TabIndex = 252
        Me.Label16.Text = "Ej.: Remitos: 1-00000789"
        '
        'OpcionInformeLotesReventasFacturados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(676, 650)
        Me.Controls.Add(Me.PanelRemitos)
        Me.Controls.Add(Me.Panel6)
        Me.Controls.Add(Me.Panel5)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Name = "OpcionInformeLotesReventasFacturados"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.PanelRemitos.ResumeLayout(False)
        Me.PanelRemitos.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DateTimeHasta As System.Windows.Forms.DateTimePicker
    Friend WithEvents DateTimeDesde As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents RadioPendientes As System.Windows.Forms.RadioButton
    Friend WithEvents RadioFacturados As System.Windows.Forms.RadioButton
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents CheckBoxTodos As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxConsignacion As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxReventa As System.Windows.Forms.CheckBox
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents ComboMarca As System.Windows.Forms.ComboBox
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboEnvase As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboCategoria As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel5 As System.Windows.Forms.Panel
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents ComboDuenio As System.Windows.Forms.ComboBox
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents PanelRemitos As System.Windows.Forms.Panel
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextRemitos As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
End Class
