<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionIngreso
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionIngreso))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ComboEmisor = New System.Windows.Forms.ComboBox
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboDeposito = New System.Windows.Forms.ComboBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Sel = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Orden = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ComboNegocio = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboAlias = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.PanelSucursal = New System.Windows.Forms.Panel
        Me.ButtonOrdenCompra = New System.Windows.Forms.Button
        Me.ButtonImportarExcel = New System.Windows.Forms.Button
        Me.ButtonImportar = New System.Windows.Forms.Button
        Me.Label5 = New System.Windows.Forms.Label
        Me.FechaIngreso = New System.Windows.Forms.DateTimePicker
        Me.ComboSucursales = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.TextBox1 = New System.Windows.Forms.TextBox
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelSucursal.SuspendLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ComboEmisor
        '
        Me.ComboEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEmisor.FormattingEnabled = True
        Me.ComboEmisor.Location = New System.Drawing.Point(143, 13)
        Me.ComboEmisor.Name = "ComboEmisor"
        Me.ComboEmisor.Size = New System.Drawing.Size(263, 28)
        Me.ComboEmisor.TabIndex = 1
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(40, 17)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(95, 20)
        Me.LabelEmisor.TabIndex = 137
        Me.LabelEmisor.Text = "Proveedor "
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(423, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 20)
        Me.Label3.TabIndex = 136
        Me.Label3.Text = "Deposito "
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboDeposito
        '
        Me.ComboDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboDeposito.FormattingEnabled = True
        Me.ComboDeposito.Location = New System.Drawing.Point(522, 13)
        Me.ComboDeposito.Name = "ComboDeposito"
        Me.ComboDeposito.Size = New System.Drawing.Size(142, 28)
        Me.ComboDeposito.TabIndex = 2
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(298, 617)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 30)
        Me.ButtonAceptar.TabIndex = 10
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Sel, Me.Orden, Me.Fecha})
        Me.Grid.Location = New System.Drawing.Point(234, 302)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 20
        Me.Grid.Size = New System.Drawing.Size(278, 155)
        Me.Grid.TabIndex = 6
        Me.Grid.Visible = False
        '
        'Sel
        '
        Me.Sel.HeaderText = ""
        Me.Sel.MinimumWidth = 30
        Me.Sel.Name = "Sel"
        Me.Sel.Width = 30
        '
        'Orden
        '
        Me.Orden.DataPropertyName = "Orden"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Orden.DefaultCellStyle = DataGridViewCellStyle3
        Me.Orden.HeaderText = "Orden Compra"
        Me.Orden.Name = "Orden"
        Me.Orden.ReadOnly = True
        '
        'Fecha
        '
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle4
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'ComboNegocio
        '
        Me.ComboNegocio.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboNegocio.FormattingEnabled = True
        Me.ComboNegocio.Location = New System.Drawing.Point(143, 86)
        Me.ComboNegocio.Name = "ComboNegocio"
        Me.ComboNegocio.Size = New System.Drawing.Size(263, 28)
        Me.ComboNegocio.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(40, 90)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 20)
        Me.Label1.TabIndex = 144
        Me.Label1.Text = "Negocio"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ComboAlias
        '
        Me.ComboAlias.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboAlias.FormattingEnabled = True
        Me.ComboAlias.Location = New System.Drawing.Point(143, 45)
        Me.ComboAlias.Name = "ComboAlias"
        Me.ComboAlias.Size = New System.Drawing.Size(263, 28)
        Me.ComboAlias.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(40, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(48, 20)
        Me.Label2.TabIndex = 153
        Me.Label2.Text = "Alias"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PanelSucursal
        '
        Me.PanelSucursal.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelSucursal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelSucursal.Controls.Add(Me.TextBox1)
        Me.PanelSucursal.Controls.Add(Me.ButtonOrdenCompra)
        Me.PanelSucursal.Controls.Add(Me.ButtonImportarExcel)
        Me.PanelSucursal.Controls.Add(Me.ButtonImportar)
        Me.PanelSucursal.Controls.Add(Me.Label5)
        Me.PanelSucursal.Controls.Add(Me.FechaIngreso)
        Me.PanelSucursal.Controls.Add(Me.ComboSucursales)
        Me.PanelSucursal.Controls.Add(Me.Label4)
        Me.PanelSucursal.Location = New System.Drawing.Point(129, 119)
        Me.PanelSucursal.Name = "PanelSucursal"
        Me.PanelSucursal.Size = New System.Drawing.Size(474, 425)
        Me.PanelSucursal.TabIndex = 303
        Me.PanelSucursal.Visible = False
        '
        'ButtonOrdenCompra
        '
        Me.ButtonOrdenCompra.BackColor = System.Drawing.Color.MistyRose
        Me.ButtonOrdenCompra.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonOrdenCompra.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonOrdenCompra.Location = New System.Drawing.Point(227, 155)
        Me.ButtonOrdenCompra.Name = "ButtonOrdenCompra"
        Me.ButtonOrdenCompra.Size = New System.Drawing.Size(156, 26)
        Me.ButtonOrdenCompra.TabIndex = 321
        Me.ButtonOrdenCompra.Text = "Ver Orden Compra"
        Me.ButtonOrdenCompra.UseVisualStyleBackColor = False
        '
        'ButtonImportarExcel
        '
        Me.ButtonImportarExcel.BackColor = System.Drawing.Color.MistyRose
        Me.ButtonImportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportarExcel.Location = New System.Drawing.Point(12, 387)
        Me.ButtonImportarExcel.Name = "ButtonImportarExcel"
        Me.ButtonImportarExcel.Size = New System.Drawing.Size(451, 26)
        Me.ButtonImportarExcel.TabIndex = 312
        Me.ButtonImportarExcel.Text = "Importar de un Remito Empresa Externa Desde Excel"
        Me.ButtonImportarExcel.UseVisualStyleBackColor = False
        Me.ButtonImportarExcel.Visible = False
        '
        'ButtonImportar
        '
        Me.ButtonImportar.BackColor = System.Drawing.Color.MistyRose
        Me.ButtonImportar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImportar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportar.Location = New System.Drawing.Point(13, 350)
        Me.ButtonImportar.Name = "ButtonImportar"
        Me.ButtonImportar.Size = New System.Drawing.Size(450, 26)
        Me.ButtonImportar.TabIndex = 311
        Me.ButtonImportar.Text = "Importar de un Remito Empresa Asociada"
        Me.ButtonImportar.UseVisualStyleBackColor = False
        Me.ButtonImportar.Visible = False
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(101, 117)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(125, 20)
        Me.Label5.TabIndex = 310
        Me.Label5.Text = "Fecha Ingreso"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'FechaIngreso
        '
        Me.FechaIngreso.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaIngreso.CustomFormat = "dd/MM/yyyy"
        Me.FechaIngreso.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.FechaIngreso.Location = New System.Drawing.Point(238, 118)
        Me.FechaIngreso.Name = "FechaIngreso"
        Me.FechaIngreso.Size = New System.Drawing.Size(108, 20)
        Me.FechaIngreso.TabIndex = 309
        '
        'ComboSucursales
        '
        Me.ComboSucursales.BackColor = System.Drawing.Color.White
        Me.ComboSucursales.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboSucursales.FormattingEnabled = True
        Me.ComboSucursales.Location = New System.Drawing.Point(115, 76)
        Me.ComboSucursales.Name = "ComboSucursales"
        Me.ComboSucursales.Size = New System.Drawing.Size(263, 28)
        Me.ComboSucursales.TabIndex = 307
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(25, 80)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 20)
        Me.Label4.TabIndex = 308
        Me.Label4.Text = "Sucursal"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(350, 556)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(54, 50)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 138
        Me.PictureCandado.TabStop = False
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.Location = New System.Drawing.Point(6, 4)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(460, 57)
        Me.TextBox1.TabIndex = 322
        Me.TextBox1.Text = "Si tiene Lista de Precios debe Informar Fecha-Ingreso y Sucursal (Si tiene) antes" & _
            " de informar los articulos del Remito o Guia. Estos se Controlara con la lista d" & _
            "e precios. "
        '
        'OpcionIngreso
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.AntiqueWhite
        Me.ClientSize = New System.Drawing.Size(704, 666)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.PanelSucursal)
        Me.Controls.Add(Me.ComboAlias)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ComboNegocio)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PictureCandado)
        Me.Controls.Add(Me.ComboEmisor)
        Me.Controls.Add(Me.LabelEmisor)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.ComboDeposito)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "OpcionIngreso"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Opcion Ingreso"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelSucursal.ResumeLayout(False)
        Me.PanelSucursal.PerformLayout()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ComboEmisor As System.Windows.Forms.ComboBox
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboDeposito As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ComboNegocio As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboAlias As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents PanelSucursal As System.Windows.Forms.Panel
    Friend WithEvents ComboSucursales As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents FechaIngreso As System.Windows.Forms.DateTimePicker
    Friend WithEvents ButtonImportarExcel As System.Windows.Forms.Button
    Friend WithEvents ButtonImportar As System.Windows.Forms.Button
    Friend WithEvents Sel As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Orden As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonOrdenCompra As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
End Class
