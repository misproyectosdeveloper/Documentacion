<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaArticulosSecuencial
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Clave = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Especie = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Variedad = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Marca = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Categoria = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Envase = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Secundario = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Iva = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CantidadPri = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.EAN = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Estado = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Label10 = New System.Windows.Forms.Label
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.ComboSecundario = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboEnvase = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboCategoria = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ComboMarca = New System.Windows.Forms.ComboBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboEspecie = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboVariedad = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonExportaExxel = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LemonChiffon
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Clave, Me.Nombre, Me.Especie, Me.Variedad, Me.Marca, Me.Categoria, Me.Envase, Me.Secundario, Me.Iva, Me.Cuenta, Me.CantidadPri, Me.EAN, Me.Estado})
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle8
        Me.Grid.EnableHeadersVisualStyles = False
        Me.Grid.Location = New System.Drawing.Point(12, 58)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.ReadOnly = True
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.RowHeadersWidth = 30
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Grid.Size = New System.Drawing.Size(1214, 580)
        Me.Grid.TabIndex = 107
        '
        'Clave
        '
        Me.Clave.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Clave.DataPropertyName = "Clave"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Clave.DefaultCellStyle = DataGridViewCellStyle2
        Me.Clave.HeaderText = "Nro.Interno"
        Me.Clave.MinimumWidth = 70
        Me.Clave.Name = "Clave"
        Me.Clave.ReadOnly = True
        Me.Clave.Width = 70
        '
        'Nombre
        '
        Me.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Nombre.DataPropertyName = "Nombre"
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nombre.DefaultCellStyle = DataGridViewCellStyle3
        Me.Nombre.HeaderText = "Nombre"
        Me.Nombre.MinimumWidth = 200
        Me.Nombre.Name = "Nombre"
        Me.Nombre.ReadOnly = True
        Me.Nombre.Width = 200
        '
        'Especie
        '
        Me.Especie.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Especie.DataPropertyName = "Especie"
        Me.Especie.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Especie.HeaderText = "Especie"
        Me.Especie.MinimumWidth = 80
        Me.Especie.Name = "Especie"
        Me.Especie.ReadOnly = True
        Me.Especie.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Especie.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Especie.Width = 80
        '
        'Variedad
        '
        Me.Variedad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Variedad.DataPropertyName = "Variedad"
        Me.Variedad.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Variedad.HeaderText = "Variedad"
        Me.Variedad.MinimumWidth = 80
        Me.Variedad.Name = "Variedad"
        Me.Variedad.ReadOnly = True
        Me.Variedad.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Variedad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Variedad.Width = 80
        '
        'Marca
        '
        Me.Marca.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Marca.DataPropertyName = "Marca"
        Me.Marca.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Marca.HeaderText = "Marca"
        Me.Marca.MinimumWidth = 80
        Me.Marca.Name = "Marca"
        Me.Marca.ReadOnly = True
        Me.Marca.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Marca.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Marca.Width = 80
        '
        'Categoria
        '
        Me.Categoria.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Categoria.DataPropertyName = "Categoria"
        Me.Categoria.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Categoria.HeaderText = "Categoria"
        Me.Categoria.MinimumWidth = 80
        Me.Categoria.Name = "Categoria"
        Me.Categoria.ReadOnly = True
        Me.Categoria.Width = 80
        '
        'Envase
        '
        Me.Envase.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Envase.DataPropertyName = "Envase"
        Me.Envase.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Envase.HeaderText = "Envase"
        Me.Envase.MinimumWidth = 80
        Me.Envase.Name = "Envase"
        Me.Envase.ReadOnly = True
        Me.Envase.Width = 80
        '
        'Secundario
        '
        Me.Secundario.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Secundario.DataPropertyName = "Secundario"
        Me.Secundario.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Secundario.FillWeight = 1.0!
        Me.Secundario.HeaderText = "Secundario"
        Me.Secundario.MinimumWidth = 80
        Me.Secundario.Name = "Secundario"
        Me.Secundario.ReadOnly = True
        Me.Secundario.Width = 80
        '
        'Iva
        '
        Me.Iva.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Iva.DataPropertyName = "Iva"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Iva.DefaultCellStyle = DataGridViewCellStyle4
        Me.Iva.HeaderText = "Iva"
        Me.Iva.MinimumWidth = 50
        Me.Iva.Name = "Iva"
        Me.Iva.ReadOnly = True
        Me.Iva.Width = 50
        '
        'Cuenta
        '
        Me.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuenta.DataPropertyName = "Cuenta"
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MinimumWidth = 100
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        '
        'CantidadPri
        '
        Me.CantidadPri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CantidadPri.DataPropertyName = "CantidadPrimarios"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.CantidadPri.DefaultCellStyle = DataGridViewCellStyle5
        Me.CantidadPri.HeaderText = "Cant. Primario"
        Me.CantidadPri.MinimumWidth = 70
        Me.CantidadPri.Name = "CantidadPri"
        Me.CantidadPri.ReadOnly = True
        Me.CantidadPri.Width = 70
        '
        'EAN
        '
        Me.EAN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.EAN.DataPropertyName = "EAN"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.EAN.DefaultCellStyle = DataGridViewCellStyle6
        Me.EAN.HeaderText = "EAN-13"
        Me.EAN.MinimumWidth = 100
        Me.EAN.Name = "EAN"
        Me.EAN.ReadOnly = True
        '
        'Estado
        '
        Me.Estado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Estado.DataPropertyName = "Estado"
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Red
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Red
        Me.Estado.DefaultCellStyle = DataGridViewCellStyle7
        Me.Estado.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Estado.HeaderText = "Estado"
        Me.Estado.MinimumWidth = 90
        Me.Estado.Name = "Estado"
        Me.Estado.ReadOnly = True
        Me.Estado.Width = 90
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label10)
        Me.Panel1.Controls.Add(Me.ComboEstado)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Controls.Add(Me.ButtonAceptar)
        Me.Panel1.Location = New System.Drawing.Point(13, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1213, 56)
        Me.Panel1.TabIndex = 108
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(649, 10)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(40, 13)
        Me.Label10.TabIndex = 219
        Me.Label10.Text = "Estado"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEstado
        '
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(695, 6)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(111, 21)
        Me.ComboEstado.TabIndex = 218
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.ComboSecundario)
        Me.Panel2.Controls.Add(Me.Label6)
        Me.Panel2.Controls.Add(Me.ComboEnvase)
        Me.Panel2.Controls.Add(Me.Label4)
        Me.Panel2.Controls.Add(Me.ComboCategoria)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.ComboMarca)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.ComboEspecie)
        Me.Panel2.Controls.Add(Me.Label3)
        Me.Panel2.Controls.Add(Me.ComboVariedad)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(5, 2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(661, 48)
        Me.Panel2.TabIndex = 217
        '
        'ComboSecundario
        '
        Me.ComboSecundario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSecundario.FormattingEnabled = True
        Me.ComboSecundario.Location = New System.Drawing.Point(296, 27)
        Me.ComboSecundario.Name = "ComboSecundario"
        Me.ComboSecundario.Size = New System.Drawing.Size(135, 21)
        Me.ComboSecundario.TabIndex = 228
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(233, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 227
        Me.Label6.Text = "Secundario"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEnvase
        '
        Me.ComboEnvase.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEnvase.FormattingEnabled = True
        Me.ComboEnvase.Location = New System.Drawing.Point(96, 26)
        Me.ComboEnvase.Name = "ComboEnvase"
        Me.ComboEnvase.Size = New System.Drawing.Size(124, 21)
        Me.ComboEnvase.TabIndex = 226
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(38, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(43, 13)
        Me.Label4.TabIndex = 225
        Me.Label4.Text = "Envase"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboCategoria
        '
        Me.ComboCategoria.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCategoria.FormattingEnabled = True
        Me.ComboCategoria.Location = New System.Drawing.Point(499, 27)
        Me.ComboCategoria.Name = "ComboCategoria"
        Me.ComboCategoria.Size = New System.Drawing.Size(124, 21)
        Me.ComboCategoria.TabIndex = 224
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(439, 31)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 223
        Me.Label2.Text = "Categoria"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboMarca
        '
        Me.ComboMarca.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMarca.FormattingEnabled = True
        Me.ComboMarca.Location = New System.Drawing.Point(499, 4)
        Me.ComboMarca.Name = "ComboMarca"
        Me.ComboMarca.Size = New System.Drawing.Size(124, 21)
        Me.ComboMarca.TabIndex = 222
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(443, 8)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 13)
        Me.Label5.TabIndex = 221
        Me.Label5.Text = "Marca"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboEspecie
        '
        Me.ComboEspecie.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEspecie.FormattingEnabled = True
        Me.ComboEspecie.Location = New System.Drawing.Point(96, 3)
        Me.ComboEspecie.Name = "ComboEspecie"
        Me.ComboEspecie.Size = New System.Drawing.Size(125, 21)
        Me.ComboEspecie.TabIndex = 220
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(38, 8)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(45, 13)
        Me.Label3.TabIndex = 219
        Me.Label3.Text = "Especie"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboVariedad
        '
        Me.ComboVariedad.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboVariedad.FormattingEnabled = True
        Me.ComboVariedad.Location = New System.Drawing.Point(295, 3)
        Me.ComboVariedad.Name = "ComboVariedad"
        Me.ComboVariedad.Size = New System.Drawing.Size(136, 21)
        Me.ComboVariedad.TabIndex = 218
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(233, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(49, 13)
        Me.Label1.TabIndex = 217
        Me.Label1.Text = "Variedad"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Yellow
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(1033, 28)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(132, 22)
        Me.ButtonAceptar.TabIndex = 84
        Me.ButtonAceptar.Text = "Seleccionar"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonExportaExxel
        '
        Me.ButtonExportaExxel.AutoEllipsis = True
        Me.ButtonExportaExxel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportaExxel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportaExxel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportaExxel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportaExxel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportaExxel.Location = New System.Drawing.Point(593, 642)
        Me.ButtonExportaExxel.Name = "ButtonExportaExxel"
        Me.ButtonExportaExxel.Size = New System.Drawing.Size(191, 32)
        Me.ButtonExportaExxel.TabIndex = 148
        Me.ButtonExportaExxel.Text = "Exportar a EXCEL"
        Me.ButtonExportaExxel.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(12, 642)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 150
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(45, 642)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 151
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
        Me.ButtonPosterior.Location = New System.Drawing.Point(78, 642)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 152
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
        Me.ButtonUltimo.Location = New System.Drawing.Point(112, 642)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 149
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ListaArticulosSecuencial
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1246, 676)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonExportaExxel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "ListaArticulosSecuencial"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Articulos"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonExportaExxel As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents ComboSecundario As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboEnvase As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboCategoria As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboMarca As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ComboEspecie As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboVariedad As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Clave As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Especie As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Variedad As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Marca As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Categoria As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Envase As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Secundario As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Iva As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CantidadPri As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents EAN As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Estado As System.Windows.Forms.DataGridViewComboBoxColumn
End Class
