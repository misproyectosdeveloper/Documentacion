<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnAsiento
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnAsiento))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel4 = New System.Windows.Forms.Panel
        Me.CheckBoxCredito = New System.Windows.Forms.CheckBox
        Me.CheckBoxDebito = New System.Windows.Forms.CheckBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.Label5 = New System.Windows.Forms.Label
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.ComboCosteo = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.PaneLotes = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.ButtonImportePorLotes = New System.Windows.Forms.Button
        Me.ButtonLotesAImputar = New System.Windows.Forms.Button
        Me.TextAsiento = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label18 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.CuentaP = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CuentaStr = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lupa = New System.Windows.Forms.DataGridViewImageColumn
        Me.Centro = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Cuenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.SubCuenta = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Debe = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Haber = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextSaldo = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextTotalHaber = New System.Windows.Forms.TextBox
        Me.TextTotalDebe = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.ButtonAnula = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.datetime1 = New System.Windows.Forms.DateTimePicker
        Me.Panel1.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.PaneLotes.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightGray
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.datetime1)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.PaneLotes)
        Me.Panel1.Controls.Add(Me.TextAsiento)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label18)
        Me.Panel1.Controls.Add(Me.TextComentario)
        Me.Panel1.Controls.Add(Me.PictureCandado)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(49, 39)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(750, 159)
        Me.Panel1.TabIndex = 1
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.CheckBoxCredito)
        Me.Panel4.Controls.Add(Me.CheckBoxDebito)
        Me.Panel4.Controls.Add(Me.Label8)
        Me.Panel4.Location = New System.Drawing.Point(386, 119)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(268, 28)
        Me.Panel4.TabIndex = 347
        '
        'CheckBoxCredito
        '
        Me.CheckBoxCredito.AutoSize = True
        Me.CheckBoxCredito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxCredito.Location = New System.Drawing.Point(176, 6)
        Me.CheckBoxCredito.Name = "CheckBoxCredito"
        Me.CheckBoxCredito.Size = New System.Drawing.Size(66, 17)
        Me.CheckBoxCredito.TabIndex = 1016
        Me.CheckBoxCredito.Text = "Crédito"
        Me.CheckBoxCredito.UseVisualStyleBackColor = True
        '
        'CheckBoxDebito
        '
        Me.CheckBoxDebito.AutoSize = True
        Me.CheckBoxDebito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckBoxDebito.Location = New System.Drawing.Point(81, 6)
        Me.CheckBoxDebito.Name = "CheckBoxDebito"
        Me.CheckBoxDebito.Size = New System.Drawing.Size(63, 17)
        Me.CheckBoxDebito.TabIndex = 1015
        Me.CheckBoxDebito.Text = "Debito"
        Me.CheckBoxDebito.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(5, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(49, 13)
        Me.Label8.TabIndex = 1014
        Me.Label8.Text = "Asiento"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label5)
        Me.Panel3.Controls.Add(Me.ComboNegocio)
        Me.Panel3.Controls.Add(Me.Label6)
        Me.Panel3.Controls.Add(Me.ComboCosteo)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Location = New System.Drawing.Point(13, 78)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(641, 35)
        Me.Panel3.TabIndex = 346
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(119, 13)
        Me.Label5.TabIndex = 1015
        Me.Label5.Text = "Negocio a Imputar :"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(202, 5)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(170, 21)
        Me.ComboNegocio.TabIndex = 1011
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(142, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(54, 13)
        Me.Label6.TabIndex = 1014
        Me.Label6.Text = "Negocio"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboCosteo
        '
        Me.ComboCosteo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboCosteo.FormattingEnabled = True
        Me.ComboCosteo.Location = New System.Drawing.Point(433, 5)
        Me.ComboCosteo.Name = "ComboCosteo"
        Me.ComboCosteo.Size = New System.Drawing.Size(198, 21)
        Me.ComboCosteo.TabIndex = 1012
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(381, 9)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(46, 13)
        Me.Label7.TabIndex = 1013
        Me.Label7.Text = "Costeo"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'PaneLotes
        '
        Me.PaneLotes.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PaneLotes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PaneLotes.Controls.Add(Me.Label4)
        Me.PaneLotes.Controls.Add(Me.ButtonImportePorLotes)
        Me.PaneLotes.Controls.Add(Me.ButtonLotesAImputar)
        Me.PaneLotes.Location = New System.Drawing.Point(13, 40)
        Me.PaneLotes.Name = "PaneLotes"
        Me.PaneLotes.Size = New System.Drawing.Size(641, 36)
        Me.PaneLotes.TabIndex = 345
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(6, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 13)
        Me.Label4.TabIndex = 1011
        Me.Label4.Text = "Lotes a Imputar :"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonImportePorLotes
        '
        Me.ButtonImportePorLotes.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonImportePorLotes.FlatAppearance.BorderSize = 0
        Me.ButtonImportePorLotes.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonImportePorLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportePorLotes.Location = New System.Drawing.Point(474, 7)
        Me.ButtonImportePorLotes.Name = "ButtonImportePorLotes"
        Me.ButtonImportePorLotes.Size = New System.Drawing.Size(151, 20)
        Me.ButtonImportePorLotes.TabIndex = 1010
        Me.ButtonImportePorLotes.TabStop = False
        Me.ButtonImportePorLotes.Text = "Importe Por Lotes "
        Me.ButtonImportePorLotes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImportePorLotes.UseVisualStyleBackColor = False
        '
        'ButtonLotesAImputar
        '
        Me.ButtonLotesAImputar.BackColor = System.Drawing.Color.LightGray
        Me.ButtonLotesAImputar.FlatAppearance.BorderSize = 0
        Me.ButtonLotesAImputar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonLotesAImputar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonLotesAImputar.Location = New System.Drawing.Point(131, 6)
        Me.ButtonLotesAImputar.Name = "ButtonLotesAImputar"
        Me.ButtonLotesAImputar.Size = New System.Drawing.Size(163, 22)
        Me.ButtonLotesAImputar.TabIndex = 1009
        Me.ButtonLotesAImputar.TabStop = False
        Me.ButtonLotesAImputar.Text = "Lotes"
        Me.ButtonLotesAImputar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonLotesAImputar.UseVisualStyleBackColor = False
        '
        'TextAsiento
        '
        Me.TextAsiento.BackColor = System.Drawing.Color.White
        Me.TextAsiento.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAsiento.Location = New System.Drawing.Point(99, 4)
        Me.TextAsiento.MaxLength = 12
        Me.TextAsiento.Name = "TextAsiento"
        Me.TextAsiento.ReadOnly = True
        Me.TextAsiento.Size = New System.Drawing.Size(116, 20)
        Me.TextAsiento.TabIndex = 274
        Me.TextAsiento.TabStop = False
        Me.TextAsiento.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 9)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(49, 13)
        Me.Label9.TabIndex = 275
        Me.Label9.Text = "Asiento"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label18.Location = New System.Drawing.Point(10, 129)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(81, 15)
        Me.Label18.TabIndex = 129
        Me.Label18.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(98, 128)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(237, 20)
        Me.TextComentario.TabIndex = 1
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(681, 48)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(50, 54)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 125
        Me.PictureCandado.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(516, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 13)
        Me.Label1.TabIndex = 33
        Me.Label1.Text = "Fecha Contable"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(13, 370)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(98, 20)
        Me.ButtonEliminarLinea.TabIndex = 104
        Me.ButtonEliminarLinea.TabStop = False
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.CuentaP, Me.CuentaStr, Me.Lupa, Me.Centro, Me.Cuenta, Me.SubCuenta, Me.Debe, Me.Haber})
        DataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle15
        Me.Grid.Location = New System.Drawing.Point(14, 10)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle16.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle16.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle16
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(721, 357)
        Me.Grid.TabIndex = 4
        '
        'CuentaP
        '
        Me.CuentaP.DataPropertyName = "CuentaP"
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.CuentaP.DefaultCellStyle = DataGridViewCellStyle10
        Me.CuentaP.HeaderText = "CuentaP"
        Me.CuentaP.MinimumWidth = 80
        Me.CuentaP.Name = "CuentaP"
        Me.CuentaP.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.CuentaP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.CuentaP.Visible = False
        Me.CuentaP.Width = 80
        '
        'CuentaStr
        '
        Me.CuentaStr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.CuentaStr.DataPropertyName = "CuentaStr"
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CuentaStr.DefaultCellStyle = DataGridViewCellStyle11
        Me.CuentaStr.HeaderText = "Cuenta"
        Me.CuentaStr.MinimumWidth = 100
        Me.CuentaStr.Name = "CuentaStr"
        Me.CuentaStr.ReadOnly = True
        '
        'Lupa
        '
        Me.Lupa.HeaderText = ""
        Me.Lupa.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.Lupa.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Lupa.Name = "Lupa"
        Me.Lupa.Width = 21
        '
        'Centro
        '
        Me.Centro.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Centro.DataPropertyName = "Centro"
        DataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Centro.DefaultCellStyle = DataGridViewCellStyle12
        Me.Centro.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Centro.HeaderText = "Centro"
        Me.Centro.MinimumWidth = 100
        Me.Centro.Name = "Centro"
        Me.Centro.ReadOnly = True
        Me.Centro.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        '
        'Cuenta
        '
        Me.Cuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuenta.DataPropertyName = "Cuenta"
        Me.Cuenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Cuenta.HeaderText = "Cuenta"
        Me.Cuenta.MinimumWidth = 120
        Me.Cuenta.Name = "Cuenta"
        Me.Cuenta.ReadOnly = True
        Me.Cuenta.Width = 120
        '
        'SubCuenta
        '
        Me.SubCuenta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.SubCuenta.DataPropertyName = "SubCuenta"
        Me.SubCuenta.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.SubCuenta.HeaderText = "SubCuenta"
        Me.SubCuenta.MinimumWidth = 120
        Me.SubCuenta.Name = "SubCuenta"
        Me.SubCuenta.ReadOnly = True
        Me.SubCuenta.Width = 120
        '
        'Debe
        '
        Me.Debe.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Debe.DataPropertyName = "Debe"
        DataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Debe.DefaultCellStyle = DataGridViewCellStyle13
        Me.Debe.HeaderText = "Debe"
        Me.Debe.MinimumWidth = 90
        Me.Debe.Name = "Debe"
        Me.Debe.Width = 90
        '
        'Haber
        '
        Me.Haber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Haber.DataPropertyName = "Haber"
        DataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Haber.DefaultCellStyle = DataGridViewCellStyle14
        Me.Haber.HeaderText = "Haber"
        Me.Haber.MinimumWidth = 90
        Me.Haber.Name = "Haber"
        Me.Haber.Width = 90
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightGray
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.TextSaldo)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.Label10)
        Me.Panel2.Controls.Add(Me.TextTotalHaber)
        Me.Panel2.Controls.Add(Me.TextTotalDebe)
        Me.Panel2.Controls.Add(Me.ButtonEliminarLinea)
        Me.Panel2.Controls.Add(Me.Grid)
        Me.Panel2.Location = New System.Drawing.Point(49, 204)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(750, 412)
        Me.Panel2.TabIndex = 4
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(575, 382)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(39, 13)
        Me.Label12.TabIndex = 279
        Me.Label12.Text = "Saldo"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextSaldo
        '
        Me.TextSaldo.BackColor = System.Drawing.Color.White
        Me.TextSaldo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldo.Location = New System.Drawing.Point(622, 378)
        Me.TextSaldo.MaxLength = 30
        Me.TextSaldo.Name = "TextSaldo"
        Me.TextSaldo.ReadOnly = True
        Me.TextSaldo.Size = New System.Drawing.Size(113, 20)
        Me.TextSaldo.TabIndex = 278
        Me.TextSaldo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(364, 382)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 13)
        Me.Label11.TabIndex = 277
        Me.Label11.Text = "Total Haber"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(148, 381)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(70, 13)
        Me.Label10.TabIndex = 276
        Me.Label10.Text = "Total Debe"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextTotalHaber
        '
        Me.TextTotalHaber.BackColor = System.Drawing.Color.White
        Me.TextTotalHaber.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalHaber.Location = New System.Drawing.Point(451, 378)
        Me.TextTotalHaber.MaxLength = 30
        Me.TextTotalHaber.Name = "TextTotalHaber"
        Me.TextTotalHaber.ReadOnly = True
        Me.TextTotalHaber.Size = New System.Drawing.Size(113, 20)
        Me.TextTotalHaber.TabIndex = 106
        Me.TextTotalHaber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextTotalDebe
        '
        Me.TextTotalDebe.BackColor = System.Drawing.Color.White
        Me.TextTotalDebe.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextTotalDebe.Location = New System.Drawing.Point(229, 378)
        Me.TextTotalDebe.MaxLength = 30
        Me.TextTotalDebe.Name = "TextTotalDebe"
        Me.TextTotalDebe.ReadOnly = True
        Me.TextTotalDebe.Size = New System.Drawing.Size(113, 20)
        Me.TextTotalDebe.TabIndex = 105
        Me.TextTotalDebe.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(641, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 13)
        Me.Label2.TabIndex = 280
        Me.Label2.Text = "Estado"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(51, 642)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(135, 29)
        Me.ButtonNuevo.TabIndex = 281
        Me.ButtonNuevo.Text = "Nuevo Asiento"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(696, 14)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(99, 21)
        Me.ComboEstado.TabIndex = 279
        Me.ComboEstado.TabStop = False
        '
        'ButtonAnula
        '
        Me.ButtonAnula.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnula.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnula.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonAnula.Location = New System.Drawing.Point(366, 641)
        Me.ButtonAnula.Name = "ButtonAnula"
        Me.ButtonAnula.Size = New System.Drawing.Size(135, 29)
        Me.ButtonAnula.TabIndex = 275
        Me.ButtonAnula.Text = "Anular Asiento"
        Me.ButtonAnula.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAnula.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(661, 639)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(136, 29)
        Me.ButtonAceptar.TabIndex = 274
        Me.ButtonAceptar.Text = "Graba Cambios"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboBox1.Enabled = False
        Me.ComboBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.Location = New System.Drawing.Point(955, 9)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(99, 21)
        Me.ComboBox1.TabIndex = 279
        Me.ComboBox1.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(900, 14)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 13)
        Me.Label3.TabIndex = 280
        Me.Label3.Text = "Estado"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'datetime1
        '
        Me.datetime1.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.datetime1.CustomFormat = "dd/MM/yyyy"
        Me.datetime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.datetime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.datetime1.Location = New System.Drawing.Point(634, 5)
        Me.datetime1.Name = "datetime1"
        Me.datetime1.Size = New System.Drawing.Size(95, 20)
        Me.datetime1.TabIndex = 348
        '
        'UnAsiento
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(851, 698)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboBox1)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.ButtonAnula)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "UnAsiento"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asiento"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.PaneLotes.ResumeLayout(False)
        Me.PaneLotes.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAnula As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents TextAsiento As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents PaneLotes As System.Windows.Forms.Panel
    Friend WithEvents ButtonImportePorLotes As System.Windows.Forms.Button
    Friend WithEvents ButtonLotesAImputar As System.Windows.Forms.Button
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents ComboCosteo As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Panel4 As System.Windows.Forms.Panel
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxCredito As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxDebito As System.Windows.Forms.CheckBox
    Friend WithEvents CuentaP As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CuentaStr As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lupa As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Centro As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Cuenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents SubCuenta As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Debe As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Haber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextTotalHaber As System.Windows.Forms.TextBox
    Friend WithEvents TextTotalDebe As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextSaldo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents datetime1 As System.Windows.Forms.DateTimePicker
End Class
