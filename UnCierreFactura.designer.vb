<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnCierreFactura
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnCierreFactura))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
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
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboClienteOperacion = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboPais = New System.Windows.Forms.ComboBox
        Me.ComboTipoIva = New System.Windows.Forms.ComboBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextCuit = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextProvincia = New System.Windows.Forms.TextBox
        Me.Label21 = New System.Windows.Forms.Label
        Me.TextFaxes = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.TextTelefonos = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.TextLocalidad = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.TextCalle = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.PanelMoneda = New System.Windows.Forms.Panel
        Me.TextCambio = New System.Windows.Forms.TextBox
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label22 = New System.Windows.Forms.Label
        Me.TextFechafactura = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.LabelFactura = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboCliente = New System.Windows.Forms.ComboBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.LabelNota = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.TextNetoB = New System.Windows.Forms.TextBox
        Me.TextNetoN = New System.Windows.Forms.TextBox
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextNetoCierre = New System.Windows.Forms.TextBox
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Indice = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioB = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NetoB = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NetoN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PrecioCierre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.NetoCierre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        Me.PanelMoneda.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Thistle
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.ComboClienteOperacion)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.ComboPais)
        Me.Panel1.Controls.Add(Me.ComboTipoIva)
        Me.Panel1.Controls.Add(Me.Label14)
        Me.Panel1.Controls.Add(Me.TextCuit)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label12)
        Me.Panel1.Controls.Add(Me.TextProvincia)
        Me.Panel1.Controls.Add(Me.Label21)
        Me.Panel1.Controls.Add(Me.TextFaxes)
        Me.Panel1.Controls.Add(Me.Label13)
        Me.Panel1.Controls.Add(Me.TextTelefonos)
        Me.Panel1.Controls.Add(Me.Label15)
        Me.Panel1.Controls.Add(Me.TextLocalidad)
        Me.Panel1.Controls.Add(Me.Label16)
        Me.Panel1.Controls.Add(Me.TextCalle)
        Me.Panel1.Controls.Add(Me.Label17)
        Me.Panel1.Controls.Add(Me.PanelMoneda)
        Me.Panel1.Controls.Add(Me.TextFechafactura)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.LabelFactura)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.ComboDeposito)
        Me.Panel1.Controls.Add(Me.Label7)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.DateTime1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.ComboCliente)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Location = New System.Drawing.Point(22, 43)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1002, 145)
        Me.Panel1.TabIndex = 304
        '
        'ComboClienteOperacion
        '
        Me.ComboClienteOperacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboClienteOperacion.Enabled = False
        Me.ComboClienteOperacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboClienteOperacion.FormattingEnabled = True
        Me.ComboClienteOperacion.Location = New System.Drawing.Point(504, 5)
        Me.ComboClienteOperacion.Name = "ComboClienteOperacion"
        Me.ComboClienteOperacion.Size = New System.Drawing.Size(226, 21)
        Me.ComboClienteOperacion.TabIndex = 1017
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(379, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(108, 13)
        Me.Label1.TabIndex = 1018
        Me.Label1.Text = "Cliente Operación"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboPais
        '
        Me.ComboPais.BackColor = System.Drawing.SystemColors.Control
        Me.ComboPais.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboPais.Enabled = False
        Me.ComboPais.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboPais.FormattingEnabled = True
        Me.ComboPais.Location = New System.Drawing.Point(706, 60)
        Me.ComboPais.Name = "ComboPais"
        Me.ComboPais.Size = New System.Drawing.Size(137, 21)
        Me.ComboPais.TabIndex = 1016
        '
        'ComboTipoIva
        '
        Me.ComboTipoIva.BackColor = System.Drawing.Color.White
        Me.ComboTipoIva.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboTipoIva.Enabled = False
        Me.ComboTipoIva.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipoIva.FormattingEnabled = True
        Me.ComboTipoIva.Location = New System.Drawing.Point(706, 81)
        Me.ComboTipoIva.Name = "ComboTipoIva"
        Me.ComboTipoIva.Size = New System.Drawing.Size(137, 21)
        Me.ComboTipoIva.TabIndex = 1005
        Me.ComboTipoIva.TabStop = False
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(680, 84)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(24, 13)
        Me.Label14.TabIndex = 1015
        Me.Label14.Text = "IVA"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCuit
        '
        Me.TextCuit.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCuit.Location = New System.Drawing.Point(574, 81)
        Me.TextCuit.MaxLength = 20
        Me.TextCuit.Name = "TextCuit"
        Me.TextCuit.ReadOnly = True
        Me.TextCuit.Size = New System.Drawing.Size(101, 20)
        Me.TextCuit.TabIndex = 1004
        Me.TextCuit.TabStop = False
        Me.TextCuit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(677, 65)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(27, 13)
        Me.Label11.TabIndex = 1014
        Me.Label11.Text = "Pais"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(545, 85)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(25, 13)
        Me.Label12.TabIndex = 1013
        Me.Label12.Text = "Cuit"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextProvincia
        '
        Me.TextProvincia.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextProvincia.Location = New System.Drawing.Point(574, 62)
        Me.TextProvincia.MaxLength = 20
        Me.TextProvincia.Name = "TextProvincia"
        Me.TextProvincia.ReadOnly = True
        Me.TextProvincia.Size = New System.Drawing.Size(101, 20)
        Me.TextProvincia.TabIndex = 1002
        Me.TextProvincia.TabStop = False
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label21.Location = New System.Drawing.Point(95, 67)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(30, 13)
        Me.Label21.TabIndex = 1012
        Me.Label21.Text = "Calle"
        Me.Label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextFaxes
        '
        Me.TextFaxes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFaxes.Location = New System.Drawing.Point(398, 81)
        Me.TextFaxes.MaxLength = 30
        Me.TextFaxes.Name = "TextFaxes"
        Me.TextFaxes.ReadOnly = True
        Me.TextFaxes.Size = New System.Drawing.Size(118, 20)
        Me.TextFaxes.TabIndex = 1003
        Me.TextFaxes.TabStop = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(356, 84)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(35, 13)
        Me.Label13.TabIndex = 1011
        Me.Label13.Text = "Faxes"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTelefonos
        '
        Me.TextTelefonos.BackColor = System.Drawing.SystemColors.Control
        Me.TextTelefonos.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTelefonos.Location = New System.Drawing.Point(163, 82)
        Me.TextTelefonos.MaxLength = 30
        Me.TextTelefonos.Name = "TextTelefonos"
        Me.TextTelefonos.ReadOnly = True
        Me.TextTelefonos.Size = New System.Drawing.Size(174, 20)
        Me.TextTelefonos.TabIndex = 1001
        Me.TextTelefonos.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(92, 85)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(54, 13)
        Me.Label15.TabIndex = 1010
        Me.Label15.Text = "Telefonos"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextLocalidad
        '
        Me.TextLocalidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLocalidad.Location = New System.Drawing.Point(398, 62)
        Me.TextLocalidad.MaxLength = 20
        Me.TextLocalidad.Name = "TextLocalidad"
        Me.TextLocalidad.ReadOnly = True
        Me.TextLocalidad.Size = New System.Drawing.Size(118, 20)
        Me.TextLocalidad.TabIndex = 1006
        Me.TextLocalidad.TabStop = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(339, 65)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(53, 13)
        Me.Label16.TabIndex = 1009
        Me.Label16.Text = "Localidad"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextCalle
        '
        Me.TextCalle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCalle.Location = New System.Drawing.Point(163, 62)
        Me.TextCalle.MaxLength = 30
        Me.TextCalle.Name = "TextCalle"
        Me.TextCalle.ReadOnly = True
        Me.TextCalle.Size = New System.Drawing.Size(175, 20)
        Me.TextCalle.TabIndex = 1007
        Me.TextCalle.TabStop = False
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label17.Location = New System.Drawing.Point(518, 65)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(51, 13)
        Me.Label17.TabIndex = 1008
        Me.Label17.Text = "Provincia"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelMoneda
        '
        Me.PanelMoneda.BackColor = System.Drawing.Color.Plum
        Me.PanelMoneda.Controls.Add(Me.TextCambio)
        Me.PanelMoneda.Controls.Add(Me.ComboMoneda)
        Me.PanelMoneda.Controls.Add(Me.Label10)
        Me.PanelMoneda.Controls.Add(Me.Label22)
        Me.PanelMoneda.Location = New System.Drawing.Point(473, 107)
        Me.PanelMoneda.Name = "PanelMoneda"
        Me.PanelMoneda.Size = New System.Drawing.Size(353, 31)
        Me.PanelMoneda.TabIndex = 143
        '
        'TextCambio
        '
        Me.TextCambio.BackColor = System.Drawing.Color.White
        Me.TextCambio.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCambio.Location = New System.Drawing.Point(256, 4)
        Me.TextCambio.MaxLength = 10
        Me.TextCambio.Name = "TextCambio"
        Me.TextCambio.Size = New System.Drawing.Size(59, 21)
        Me.TextCambio.TabIndex = 4
        Me.TextCambio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ComboMoneda
        '
        Me.ComboMoneda.BackColor = System.Drawing.Color.White
        Me.ComboMoneda.Enabled = False
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.ForeColor = System.Drawing.Color.Black
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(79, 4)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(115, 23)
        Me.ComboMoneda.TabIndex = 144
        Me.ComboMoneda.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(0, 7)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(75, 15)
        Me.Label10.TabIndex = 143
        Me.Label10.Text = "Importes  en"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label22.Location = New System.Drawing.Point(202, 7)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(50, 15)
        Me.Label22.TabIndex = 142
        Me.Label22.Text = "Cambio"
        '
        'TextFechafactura
        '
        Me.TextFechafactura.BackColor = System.Drawing.Color.White
        Me.TextFechafactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechafactura.Location = New System.Drawing.Point(432, 30)
        Me.TextFechafactura.Name = "TextFechafactura"
        Me.TextFechafactura.ReadOnly = True
        Me.TextFechafactura.Size = New System.Drawing.Size(115, 20)
        Me.TextFechafactura.TabIndex = 142
        Me.TextFechafactura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(333, 34)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(89, 13)
        Me.Label9.TabIndex = 141
        Me.Label9.Text = "Fecha Factura"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelFactura
        '
        Me.LabelFactura.AutoSize = True
        Me.LabelFactura.BackColor = System.Drawing.Color.White
        Me.LabelFactura.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelFactura.ForeColor = System.Drawing.Color.Black
        Me.LabelFactura.Location = New System.Drawing.Point(82, 34)
        Me.LabelFactura.Name = "LabelFactura"
        Me.LabelFactura.Size = New System.Drawing.Size(51, 15)
        Me.LabelFactura.TabIndex = 140
        Me.LabelFactura.Text = "factura"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(10, 34)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 139
        Me.Label4.Text = "Factura"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(632, 35)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 138
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboDeposito.Enabled = False
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(697, 31)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(118, 21)
        Me.ComboDeposito.TabIndex = 137
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(8, 113)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 15)
        Me.Label7.TabIndex = 136
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Location = New System.Drawing.Point(92, 112)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(319, 20)
        Me.TextComentario.TabIndex = 135
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(941, 39)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(31, 31)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 134
        Me.PictureCandado.TabStop = False
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(867, 4)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(105, 20)
        Me.DateTime1.TabIndex = 132
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(818, 8)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(42, 13)
        Me.Label2.TabIndex = 133
        Me.Label2.Text = "Fecha"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCliente
        '
        Me.ComboCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboCliente.Enabled = False
        Me.ComboCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCliente.FormattingEnabled = True
        Me.ComboCliente.Location = New System.Drawing.Point(136, 4)
        Me.ComboCliente.Name = "ComboCliente"
        Me.ComboCliente.Size = New System.Drawing.Size(226, 21)
        Me.ComboCliente.TabIndex = 130
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(117, 13)
        Me.Label8.TabIndex = 131
        Me.Label8.Text = "Cliente Facturación"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelNota
        '
        Me.LabelNota.BackColor = System.Drawing.Color.White
        Me.LabelNota.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNota.ForeColor = System.Drawing.Color.Black
        Me.LabelNota.Location = New System.Drawing.Point(118, 19)
        Me.LabelNota.Name = "LabelNota"
        Me.LabelNota.Size = New System.Drawing.Size(102, 16)
        Me.LabelNota.TabIndex = 311
        Me.LabelNota.Text = "factura"
        Me.LabelNota.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(870, 18)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 13)
        Me.Label6.TabIndex = 308
        Me.Label6.Text = "Estado"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(924, 15)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 307
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(19, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(87, 13)
        Me.Label5.TabIndex = 306
        Me.Label5.Text = "Cierre Factura"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'TextNetoB
        '
        Me.TextNetoB.BackColor = System.Drawing.Color.White
        Me.TextNetoB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextNetoB.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNetoB.Location = New System.Drawing.Point(429, 383)
        Me.TextNetoB.MaxLength = 20
        Me.TextNetoB.Name = "TextNetoB"
        Me.TextNetoB.ReadOnly = True
        Me.TextNetoB.Size = New System.Drawing.Size(96, 20)
        Me.TextNetoB.TabIndex = 147
        Me.TextNetoB.TabStop = False
        Me.TextNetoB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextNetoN
        '
        Me.TextNetoN.BackColor = System.Drawing.Color.White
        Me.TextNetoN.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextNetoN.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNetoN.Location = New System.Drawing.Point(592, 384)
        Me.TextNetoN.MaxLength = 20
        Me.TextNetoN.Name = "TextNetoN"
        Me.TextNetoN.ReadOnly = True
        Me.TextNetoN.Size = New System.Drawing.Size(96, 20)
        Me.TextNetoN.TabIndex = 146
        Me.TextNetoN.TabStop = False
        Me.TextNetoN.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(878, 383)
        Me.TextImporte.MaxLength = 20
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.ReadOnly = True
        Me.TextImporte.Size = New System.Drawing.Size(96, 20)
        Me.TextImporte.TabIndex = 145
        Me.TextImporte.TabStop = False
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.Thistle
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextNetoCierre)
        Me.Panel3.Controls.Add(Me.TextNetoB)
        Me.Panel3.Controls.Add(Me.TextNetoN)
        Me.Panel3.Controls.Add(Me.TextImporte)
        Me.Panel3.Controls.Add(Me.Grid)
        Me.Panel3.Location = New System.Drawing.Point(21, 193)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(1003, 413)
        Me.Panel3.TabIndex = 305
        '
        'TextNetoCierre
        '
        Me.TextNetoCierre.BackColor = System.Drawing.Color.White
        Me.TextNetoCierre.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextNetoCierre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNetoCierre.Location = New System.Drawing.Point(781, 383)
        Me.TextNetoCierre.MaxLength = 20
        Me.TextNetoCierre.Name = "TextNetoCierre"
        Me.TextNetoCierre.ReadOnly = True
        Me.TextNetoCierre.Size = New System.Drawing.Size(96, 20)
        Me.TextNetoCierre.TabIndex = 148
        Me.TextNetoCierre.TabStop = False
        Me.TextNetoCierre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.AntiqueWhite
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Indice, Me.Articulo, Me.Cantidad, Me.PrecioB, Me.NetoB, Me.PrecioN, Me.NetoN, Me.PrecioCierre, Me.NetoCierre, Me.Importe})
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle11
        Me.Grid.Location = New System.Drawing.Point(9, 4)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        DataGridViewCellStyle12.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(985, 374)
        Me.Grid.TabIndex = 1
        '
        'Indice
        '
        Me.Indice.DataPropertyName = "Indice"
        Me.Indice.HeaderText = "Indice"
        Me.Indice.Name = "Indice"
        Me.Indice.Visible = False
        Me.Indice.Width = 61
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Articulo.DefaultCellStyle = DataGridViewCellStyle2
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 250
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
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
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 80
        '
        'PrecioB
        '
        Me.PrecioB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrecioB.DataPropertyName = "PrecioB"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrecioB.DefaultCellStyle = DataGridViewCellStyle4
        Me.PrecioB.HeaderText = "Precio"
        Me.PrecioB.MaxInputLength = 8
        Me.PrecioB.MinimumWidth = 80
        Me.PrecioB.Name = "PrecioB"
        Me.PrecioB.ReadOnly = True
        Me.PrecioB.Width = 80
        '
        'NetoB
        '
        Me.NetoB.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NetoB.DataPropertyName = "NetoB"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.NetoB.DefaultCellStyle = DataGridViewCellStyle5
        Me.NetoB.HeaderText = "Neto"
        Me.NetoB.MinimumWidth = 80
        Me.NetoB.Name = "NetoB"
        Me.NetoB.ReadOnly = True
        Me.NetoB.Width = 80
        '
        'PrecioN
        '
        Me.PrecioN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrecioN.DataPropertyName = "PrecioN"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrecioN.DefaultCellStyle = DataGridViewCellStyle6
        Me.PrecioN.HeaderText = "Precio (2)"
        Me.PrecioN.MaxInputLength = 8
        Me.PrecioN.MinimumWidth = 80
        Me.PrecioN.Name = "PrecioN"
        Me.PrecioN.ReadOnly = True
        Me.PrecioN.Width = 80
        '
        'NetoN
        '
        Me.NetoN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NetoN.DataPropertyName = "NetoN"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.NetoN.DefaultCellStyle = DataGridViewCellStyle7
        Me.NetoN.HeaderText = "Neto(2)"
        Me.NetoN.MinimumWidth = 80
        Me.NetoN.Name = "NetoN"
        Me.NetoN.ReadOnly = True
        Me.NetoN.Width = 80
        '
        'PrecioCierre
        '
        Me.PrecioCierre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PrecioCierre.DataPropertyName = "PrecioCierre"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.PrecioCierre.DefaultCellStyle = DataGridViewCellStyle8
        Me.PrecioCierre.HeaderText = "Precio Cierre"
        Me.PrecioCierre.MaxInputLength = 8
        Me.PrecioCierre.MinimumWidth = 90
        Me.PrecioCierre.Name = "PrecioCierre"
        Me.PrecioCierre.Width = 90
        '
        'NetoCierre
        '
        Me.NetoCierre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.NetoCierre.DataPropertyName = "NetoCierre"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.NetoCierre.DefaultCellStyle = DataGridViewCellStyle9
        Me.NetoCierre.HeaderText = "Neto Cierre"
        Me.NetoCierre.MinimumWidth = 100
        Me.NetoCierre.Name = "NetoCierre"
        Me.NetoCierre.ReadOnly = True
        '
        'Importe
        '
        Me.Importe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe.DataPropertyName = "Importe"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Importe.DefaultCellStyle = DataGridViewCellStyle10
        Me.Importe.HeaderText = "Total Importe"
        Me.Importe.MinimumWidth = 100
        Me.Importe.Name = "Importe"
        Me.Importe.ReadOnly = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = CType(resources.GetObject("ButtonImprimir.Image"), System.Drawing.Image)
        Me.ButtonImprimir.Location = New System.Drawing.Point(291, 626)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(164, 39)
        Me.ButtonImprimir.TabIndex = 314
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime"
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(587, 625)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(164, 39)
        Me.ButtonAsientoContable.TabIndex = 312
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = CType(resources.GetObject("ButtonAnula.Image"), System.Drawing.Image)
        Me.ButtonAnula.Location = New System.Drawing.Point(21, 627)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(164, 39)
        Me.ButtonAnula.TabIndex = 303
        Me.ButtonAnula.Text = "Anular Nota Credito"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = CType(resources.GetObject("ButtonAceptar.Image"), System.Drawing.Image)
        Me.ButtonAceptar.Location = New System.Drawing.Point(855, 625)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(164, 39)
        Me.ButtonAceptar.TabIndex = 302
        Me.ButtonAceptar.Text = "Graba Nota de Credito "
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'UnCierreFactura
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(1043, 676)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.LabelNota)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "UnCierreFactura"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cierre Factura Exportacion"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.PanelMoneda.ResumeLayout(False)
        Me.PanelMoneda.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboPais As System.Windows.Forms.ComboBox
    Friend WithEvents ComboTipoIva As System.Windows.Forms.ComboBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextCuit As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextProvincia As System.Windows.Forms.TextBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents TextFaxes As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents TextTelefonos As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents TextLocalidad As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents TextCalle As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents PanelMoneda As System.Windows.Forms.Panel
    Friend WithEvents TextCambio As System.Windows.Forms.TextBox
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents TextFechafactura As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents LabelFactura As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboCliente As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents LabelNota As System.Windows.Forms.Label
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextNetoB As System.Windows.Forms.TextBox
    Friend WithEvents TextNetoN As System.Windows.Forms.TextBox
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents TextNetoCierre As System.Windows.Forms.TextBox
    Friend WithEvents Indice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NetoB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NetoN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PrecioCierre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents NetoCierre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ComboClienteOperacion As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
