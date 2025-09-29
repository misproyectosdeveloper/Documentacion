<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaTabla
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle17 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle18 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnaTabla))
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Clave = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CodigoMonedaAfip = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LetraFactura = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TipoIva = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.TipoPago = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.SumaResta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Iva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Activo = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Operador = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Activo2 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Activo3 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Activo4 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Activo5 = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.TopeMes = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.AlicuotaRetencion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Centro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LupaVenta = New System.Windows.Forms.DataGridViewImageColumn
        Me.Cuenta2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LupaCompra = New System.Windows.Forms.DataGridViewImageColumn
        Me.AlicuotaRetIngBruto = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OperadorInt = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CodProvinciaIIBB = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OrigenPercepcion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CodigoAfipElectronico = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UltimoNumero = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.LabelTitulo = New System.Windows.Forms.Label
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Mensaje = New System.Windows.Forms.Label
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Clave, Me.Nombre, Me.CodigoMonedaAfip, Me.LetraFactura, Me.TipoIva, Me.TipoPago, Me.SumaResta, Me.Iva, Me.Activo, Me.Operador, Me.Activo2, Me.Activo3, Me.Activo4, Me.Activo5, Me.TopeMes, Me.AlicuotaRetencion, Me.Cuenta, Me.Centro, Me.LupaVenta, Me.Cuenta2, Me.LupaCompra, Me.AlicuotaRetIngBruto, Me.OperadorInt, Me.CodProvinciaIIBB, Me.OrigenPercepcion, Me.CodigoAfipElectronico, Me.Comentario, Me.UltimoNumero, Me.Cuit})
        DataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle16.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle16.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.Location = New System.Drawing.Point(29, 66)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle17.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle17
        DataGridViewCellStyle18.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle18.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle18
        Me.Grid.Size = New System.Drawing.Size(610, 413)
        Me.Grid.TabIndex = 1
        '
        'Clave
        '
        Me.Clave.DataPropertyName = "Clave"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Clave.DefaultCellStyle = DataGridViewCellStyle3
        Me.Clave.HeaderText = "Clave"
        Me.Clave.MaxInputLength = 11
        Me.Clave.Name = "Clave"
        Me.Clave.Visible = False
        Me.Clave.Width = 59
        '
        'Nombre
        '
        Me.Nombre.DataPropertyName = "Nombre"
        Me.Nombre.HeaderText = "Nombre"
        Me.Nombre.MaxInputLength = 20
        Me.Nombre.MinimumWidth = 130
        Me.Nombre.Name = "Nombre"
        Me.Nombre.Width = 130
        '
        'CodigoMonedaAfip
        '
        Me.CodigoMonedaAfip.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CodigoMonedaAfip.DataPropertyName = "CodigoMonedaAfip"
        Me.CodigoMonedaAfip.HeaderText = "Codigo Afip"
        Me.CodigoMonedaAfip.MaxInputLength = 3
        Me.CodigoMonedaAfip.MinimumWidth = 70
        Me.CodigoMonedaAfip.Name = "CodigoMonedaAfip"
        Me.CodigoMonedaAfip.Visible = False
        Me.CodigoMonedaAfip.Width = 70
        '
        'LetraFactura
        '
        Me.LetraFactura.DataPropertyName = "LetraFactura"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LetraFactura.DefaultCellStyle = DataGridViewCellStyle4
        Me.LetraFactura.HeaderText = "Letra Factura"
        Me.LetraFactura.Name = "LetraFactura"
        Me.LetraFactura.Visible = False
        Me.LetraFactura.Width = 95
        '
        'TipoIva
        '
        Me.TipoIva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoIva.DataPropertyName = "TipoIva"
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.White
        Me.TipoIva.DefaultCellStyle = DataGridViewCellStyle5
        Me.TipoIva.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TipoIva.HeaderText = "Tipo Iva"
        Me.TipoIva.MinimumWidth = 120
        Me.TipoIva.Name = "TipoIva"
        Me.TipoIva.Visible = False
        Me.TipoIva.Width = 120
        '
        'TipoPago
        '
        Me.TipoPago.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TipoPago.DataPropertyName = "TipoPago"
        Me.TipoPago.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.TipoPago.HeaderText = "Tipo Pago"
        Me.TipoPago.MinimumWidth = 150
        Me.TipoPago.Name = "TipoPago"
        Me.TipoPago.Visible = False
        Me.TipoPago.Width = 150
        '
        'SumaResta
        '
        Me.SumaResta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.SumaResta.DataPropertyName = "Operador"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        Me.SumaResta.DefaultCellStyle = DataGridViewCellStyle6
        Me.SumaResta.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.SumaResta.HeaderText = "Operador"
        Me.SumaResta.MinimumWidth = 90
        Me.SumaResta.Name = "SumaResta"
        Me.SumaResta.Visible = False
        Me.SumaResta.Width = 90
        '
        'Iva
        '
        Me.Iva.DataPropertyName = "Iva"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Iva.DefaultCellStyle = DataGridViewCellStyle7
        Me.Iva.HeaderText = "Iva"
        Me.Iva.Name = "Iva"
        Me.Iva.Width = 47
        '
        'Activo
        '
        Me.Activo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Activo.DataPropertyName = "Activo"
        Me.Activo.HeaderText = "Activo"
        Me.Activo.MinimumWidth = 60
        Me.Activo.Name = "Activo"
        Me.Activo.Visible = False
        Me.Activo.Width = 60
        '
        'Operador
        '
        Me.Operador.DataPropertyName = "Operador"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Operador.DefaultCellStyle = DataGridViewCellStyle8
        Me.Operador.HeaderText = "Clave Para Remito (COT-ARBA) "
        Me.Operador.MaxInputLength = 6
        Me.Operador.MinimumWidth = 100
        Me.Operador.Name = "Operador"
        Me.Operador.Visible = False
        Me.Operador.Width = 186
        '
        'Activo2
        '
        Me.Activo2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Activo2.DataPropertyName = "Activo2"
        Me.Activo2.HeaderText = "Activo2"
        Me.Activo2.MinimumWidth = 60
        Me.Activo2.Name = "Activo2"
        Me.Activo2.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Activo2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Activo2.Visible = False
        Me.Activo2.Width = 60
        '
        'Activo3
        '
        Me.Activo3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Activo3.DataPropertyName = "Activo3"
        Me.Activo3.HeaderText = "Activo3"
        Me.Activo3.MinimumWidth = 60
        Me.Activo3.Name = "Activo3"
        Me.Activo3.Visible = False
        Me.Activo3.Width = 60
        '
        'Activo4
        '
        Me.Activo4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Activo4.DataPropertyName = "Activo4"
        Me.Activo4.HeaderText = "Activo4"
        Me.Activo4.MinimumWidth = 60
        Me.Activo4.Name = "Activo4"
        Me.Activo4.Visible = False
        Me.Activo4.Width = 60
        '
        'Activo5
        '
        Me.Activo5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Activo5.DataPropertyName = "Activo5"
        Me.Activo5.HeaderText = "Activo5"
        Me.Activo5.MinimumWidth = 60
        Me.Activo5.Name = "Activo5"
        Me.Activo5.Visible = False
        Me.Activo5.Width = 60
        '
        'TopeMes
        '
        Me.TopeMes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.TopeMes.DataPropertyName = "TopeMes"
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.TopeMes.DefaultCellStyle = DataGridViewCellStyle9
        Me.TopeMes.HeaderText = "Tope Mes"
        Me.TopeMes.MaxInputLength = 8
        Me.TopeMes.MinimumWidth = 70
        Me.TopeMes.Name = "TopeMes"
        Me.TopeMes.Visible = False
        Me.TopeMes.Width = 70
        '
        'AlicuotaRetencion
        '
        Me.AlicuotaRetencion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.AlicuotaRetencion.DataPropertyName = "AlicuotaRetencion"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.AlicuotaRetencion.DefaultCellStyle = DataGridViewCellStyle10
        Me.AlicuotaRetencion.FillWeight = 1.0!
        Me.AlicuotaRetencion.HeaderText = "Alícuota"
        Me.AlicuotaRetencion.MaxInputLength = 5
        Me.AlicuotaRetencion.MinimumWidth = 50
        Me.AlicuotaRetencion.Name = "AlicuotaRetencion"
        Me.AlicuotaRetencion.Visible = False
        Me.AlicuotaRetencion.Width = 50
        '
        'Cuenta
        '
        Me.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuenta.DataPropertyName = "Cuenta"
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MaxInputLength = 0
        Me.Cuenta.MinimumWidth = 90
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        Me.Cuenta.Visible = False
        Me.Cuenta.Width = 90
        '
        'Centro
        '
        Me.Centro.DataPropertyName = "Centro"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Centro.DefaultCellStyle = DataGridViewCellStyle11
        Me.Centro.HeaderText = "Centro"
        Me.Centro.Name = "Centro"
        Me.Centro.ReadOnly = True
        Me.Centro.Visible = False
        Me.Centro.Width = 63
        '
        'LupaVenta
        '
        Me.LupaVenta.HeaderText = ""
        Me.LupaVenta.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.LupaVenta.MinimumWidth = 30
        Me.LupaVenta.Name = "LupaVenta"
        Me.LupaVenta.Visible = False
        Me.LupaVenta.Width = 30
        '
        'Cuenta2
        '
        Me.Cuenta2.DataPropertyName = "Cuenta2"
        Me.Cuenta2.HeaderText = "Cuenta"
        Me.Cuenta2.MaxInputLength = 0
        Me.Cuenta2.MinimumWidth = 90
        Me.Cuenta2.Name = "Cuenta2"
        Me.Cuenta2.ReadOnly = True
        Me.Cuenta2.Visible = False
        Me.Cuenta2.Width = 90
        '
        'LupaCompra
        '
        Me.LupaCompra.HeaderText = ""
        Me.LupaCompra.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.LupaCompra.MinimumWidth = 30
        Me.LupaCompra.Name = "LupaCompra"
        Me.LupaCompra.Visible = False
        Me.LupaCompra.Width = 30
        '
        'AlicuotaRetIngBruto
        '
        Me.AlicuotaRetIngBruto.DataPropertyName = "AlicuotaRetIngBruto"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.AlicuotaRetIngBruto.DefaultCellStyle = DataGridViewCellStyle12
        Me.AlicuotaRetIngBruto.HeaderText = "Alicuota Retención Ing. Bruto"
        Me.AlicuotaRetIngBruto.Name = "AlicuotaRetIngBruto"
        Me.AlicuotaRetIngBruto.Visible = False
        Me.AlicuotaRetIngBruto.Width = 171
        '
        'OperadorInt
        '
        Me.OperadorInt.DataPropertyName = "Operador"
        Me.OperadorInt.HeaderText = "Jurisdiccion IIBB Covenio Multilateral"
        Me.OperadorInt.Name = "OperadorInt"
        Me.OperadorInt.Visible = False
        Me.OperadorInt.Width = 205
        '
        'CodProvinciaIIBB
        '
        Me.CodProvinciaIIBB.DataPropertyName = "CodigoProvinciaIIBB"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CodProvinciaIIBB.DefaultCellStyle = DataGridViewCellStyle13
        Me.CodProvinciaIIBB.HeaderText = "Cod.Provincia IIBB"
        Me.CodProvinciaIIBB.MinimumWidth = 90
        Me.CodProvinciaIIBB.Name = "CodProvinciaIIBB"
        Me.CodProvinciaIIBB.Visible = False
        Me.CodProvinciaIIBB.Width = 121
        '
        'OrigenPercepcion
        '
        Me.OrigenPercepcion.DataPropertyName = "OrigenPercepcion"
        Me.OrigenPercepcion.HeaderText = "OrigenPercepcion"
        Me.OrigenPercepcion.Name = "OrigenPercepcion"
        Me.OrigenPercepcion.Visible = False
        Me.OrigenPercepcion.Width = 117
        '
        'CodigoAfipElectronico
        '
        Me.CodigoAfipElectronico.DataPropertyName = "CodigoAfipElectronico"
        Me.CodigoAfipElectronico.HeaderText = "Codigos Afip Electronicos"
        Me.CodigoAfipElectronico.Name = "CodigoAfipElectronico"
        Me.CodigoAfipElectronico.Visible = False
        Me.CodigoAfipElectronico.Width = 152
        '
        'Comentario
        '
        Me.Comentario.DataPropertyName = "Comentario"
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.MaxInputLength = 50
        Me.Comentario.MinimumWidth = 200
        Me.Comentario.Name = "Comentario"
        Me.Comentario.Width = 200
        '
        'UltimoNumero
        '
        Me.UltimoNumero.DataPropertyName = "UltimoNumero"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.UltimoNumero.DefaultCellStyle = DataGridViewCellStyle14
        Me.UltimoNumero.HeaderText = "Ultimo Numero"
        Me.UltimoNumero.MaxInputLength = 12
        Me.UltimoNumero.Name = "UltimoNumero"
        Me.UltimoNumero.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.UltimoNumero.Visible = False
        Me.UltimoNumero.Width = 82
        '
        'Cuit
        '
        Me.Cuit.DataPropertyName = "Cuit"
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cuit.DefaultCellStyle = DataGridViewCellStyle15
        Me.Cuit.HeaderText = "Cuit"
        Me.Cuit.MaxInputLength = 11
        Me.Cuit.MinimumWidth = 100
        Me.Cuit.Name = "Cuit"
        Me.Cuit.Visible = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(525, 482)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonAceptar.TabIndex = 7
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Location = New System.Drawing.Point(366, 482)
        Me.ButtonEliminar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonEliminar.TabIndex = 6
        Me.ButtonEliminar.Text = "Borrar Linea"
        Me.ButtonEliminar.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(29, 483)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 2
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(63, 483)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 3
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'LabelTitulo
        '
        Me.LabelTitulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTitulo.Location = New System.Drawing.Point(46, 29)
        Me.LabelTitulo.Name = "LabelTitulo"
        Me.LabelTitulo.Size = New System.Drawing.Size(573, 24)
        Me.LabelTitulo.TabIndex = 152
        Me.LabelTitulo.Text = "Titulo"
        Me.LabelTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(95, 483)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 4
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(127, 483)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 5
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 550)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(213, 13)
        Me.Label1.TabIndex = 154
        Me.Label1.Text = "En Borrar linea testear si esta siendo usado."
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'Mensaje
        '
        Me.Mensaje.AutoSize = True
        Me.Mensaje.Location = New System.Drawing.Point(30, 521)
        Me.Mensaje.Name = "Mensaje"
        Me.Mensaje.Size = New System.Drawing.Size(39, 13)
        Me.Mensaje.TabIndex = 155
        Me.Mensaje.Text = "Label2"
        Me.Mensaje.Visible = False
        '
        'UnaTabla
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(704, 562)
        Me.Controls.Add(Me.Mensaje)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.LabelTitulo)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Name = "UnaTabla"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tablas"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents LabelTitulo As System.Windows.Forms.Label
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Mensaje As System.Windows.Forms.Label
    Friend WithEvents Clave As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodigoMonedaAfip As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LetraFactura As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TipoIva As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents TipoPago As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents SumaResta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Iva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Activo As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Operador As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Activo2 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Activo3 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Activo4 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Activo5 As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents TopeMes As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AlicuotaRetencion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Centro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LupaVenta As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Cuenta2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LupaCompra As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents AlicuotaRetIngBruto As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OperadorInt As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodProvinciaIIBB As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OrigenPercepcion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodigoAfipElectronico As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UltimoNumero As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuit As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
