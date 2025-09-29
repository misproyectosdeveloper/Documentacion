<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeleccionaLotesAAsignar
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SeleccionaLotesAAsignar))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Label3 = New System.Windows.Forms.Label
        Me.LabCantidad = New System.Windows.Forms.Label
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.LabelArticulo = New System.Windows.Forms.Label
        Me.LabelDeposito = New System.Windows.Forms.Label
        Me.LabelUnidad = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.LabelFaltaAsignar = New System.Windows.Forms.Label
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.OtrasAsignaciones = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteOrigen = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DepositoOrigen = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SecuenciaOrigen = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.KilosXUnidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Proveedor = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Fecha = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Calibre = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Stock = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PermisoImp = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Asignado = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(434, 65)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 15)
        Me.Label2.TabIndex = 19
        Me.Label2.Text = "Deposito"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(63, 63)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(55, 15)
        Me.Label1.TabIndex = 18
        Me.Label1.Text = "Articulo"
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.OtrasAsignaciones, Me.LoteOrigen, Me.DepositoOrigen, Me.SecuenciaOrigen, Me.Lote, Me.Secuencia, Me.KilosXUnidad, Me.candado, Me.LoteYSecuencia, Me.Proveedor, Me.Fecha, Me.Calibre, Me.Stock, Me.PermisoImp, Me.Asignado})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.Location = New System.Drawing.Point(39, 126)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        Me.Grid.Size = New System.Drawing.Size(707, 349)
        Me.Grid.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(231, 100)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(128, 15)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "Cantidad A Asignar"
        '
        'LabCantidad
        '
        Me.LabCantidad.BackColor = System.Drawing.Color.White
        Me.LabCantidad.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabCantidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabCantidad.Location = New System.Drawing.Point(375, 98)
        Me.LabCantidad.Name = "LabCantidad"
        Me.LabCantidad.Size = New System.Drawing.Size(115, 19)
        Me.LabCantidad.TabIndex = 22
        Me.LabCantidad.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(329, 543)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 24
        Me.ButtonAceptar.Text = "Aceptar Asignación"
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'LabelArticulo
        '
        Me.LabelArticulo.BackColor = System.Drawing.Color.White
        Me.LabelArticulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelArticulo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelArticulo.Location = New System.Drawing.Point(129, 63)
        Me.LabelArticulo.Name = "LabelArticulo"
        Me.LabelArticulo.Size = New System.Drawing.Size(286, 19)
        Me.LabelArticulo.TabIndex = 25
        Me.LabelArticulo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelDeposito
        '
        Me.LabelDeposito.BackColor = System.Drawing.Color.White
        Me.LabelDeposito.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelDeposito.Location = New System.Drawing.Point(503, 63)
        Me.LabelDeposito.Name = "LabelDeposito"
        Me.LabelDeposito.Size = New System.Drawing.Size(166, 19)
        Me.LabelDeposito.TabIndex = 26
        Me.LabelDeposito.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LabelUnidad
        '
        Me.LabelUnidad.AutoSize = True
        Me.LabelUnidad.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelUnidad.Location = New System.Drawing.Point(499, 99)
        Me.LabelUnidad.Name = "LabelUnidad"
        Me.LabelUnidad.Size = New System.Drawing.Size(19, 15)
        Me.LabelUnidad.TabIndex = 27
        Me.LabelUnidad.Text = "   "
        Me.LabelUnidad.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(268, 483)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 15)
        Me.Label4.TabIndex = 29
        Me.Label4.Text = "Falta Asignar"
        '
        'LabelFaltaAsignar
        '
        Me.LabelFaltaAsignar.BackColor = System.Drawing.Color.White
        Me.LabelFaltaAsignar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LabelFaltaAsignar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelFaltaAsignar.Location = New System.Drawing.Point(374, 481)
        Me.LabelFaltaAsignar.Name = "LabelFaltaAsignar"
        Me.LabelFaltaAsignar.Size = New System.Drawing.Size(115, 19)
        Me.LabelFaltaAsignar.TabIndex = 28
        Me.LabelFaltaAsignar.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        '
        'OtrasAsignaciones
        '
        Me.OtrasAsignaciones.DataPropertyName = "OtrasAsignaciones"
        Me.OtrasAsignaciones.HeaderText = "OtrasAsignaciones"
        Me.OtrasAsignaciones.Name = "OtrasAsignaciones"
        Me.OtrasAsignaciones.Visible = False
        '
        'LoteOrigen
        '
        Me.LoteOrigen.DataPropertyName = "LoteOrigen"
        Me.LoteOrigen.HeaderText = "LoteOrigen"
        Me.LoteOrigen.Name = "LoteOrigen"
        Me.LoteOrigen.Visible = False
        '
        'DepositoOrigen
        '
        Me.DepositoOrigen.DataPropertyName = "DepositoOrigen"
        Me.DepositoOrigen.HeaderText = "Deposito Origen"
        Me.DepositoOrigen.Name = "DepositoOrigen"
        Me.DepositoOrigen.Visible = False
        '
        'SecuenciaOrigen
        '
        Me.SecuenciaOrigen.DataPropertyName = "SecuenciaOrigen"
        Me.SecuenciaOrigen.HeaderText = "SecuenciaOrigen"
        Me.SecuenciaOrigen.Name = "SecuenciaOrigen"
        Me.SecuenciaOrigen.Visible = False
        '
        'Lote
        '
        Me.Lote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lote.DataPropertyName = "Lote"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Lote.DefaultCellStyle = DataGridViewCellStyle3
        Me.Lote.HeaderText = "Lote"
        Me.Lote.MinimumWidth = 80
        Me.Lote.Name = "Lote"
        Me.Lote.Visible = False
        Me.Lote.Width = 80
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        '
        'KilosXUnidad
        '
        Me.KilosXUnidad.DataPropertyName = "kilosXUnidad"
        Me.KilosXUnidad.HeaderText = "KilosXUnidad"
        Me.KilosXUnidad.Name = "KilosXUnidad"
        Me.KilosXUnidad.Visible = False
        '
        'candado
        '
        Me.candado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.candado.HeaderText = ""
        Me.candado.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.candado.MinimumWidth = 30
        Me.candado.Name = "candado"
        Me.candado.Width = 30
        '
        'LoteYSecuencia
        '
        Me.LoteYSecuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle4
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
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Proveedor.DefaultCellStyle = DataGridViewCellStyle5
        Me.Proveedor.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Proveedor.HeaderText = "Proveedor"
        Me.Proveedor.MinimumWidth = 140
        Me.Proveedor.Name = "Proveedor"
        Me.Proveedor.ReadOnly = True
        Me.Proveedor.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Proveedor.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Proveedor.Width = 140
        '
        'Fecha
        '
        Me.Fecha.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Fecha.DataPropertyName = "Fecha"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Fecha.DefaultCellStyle = DataGridViewCellStyle6
        Me.Fecha.HeaderText = "Fecha"
        Me.Fecha.MinimumWidth = 70
        Me.Fecha.Name = "Fecha"
        Me.Fecha.ReadOnly = True
        Me.Fecha.Width = 70
        '
        'Calibre
        '
        Me.Calibre.DataPropertyName = "Calibre"
        Me.Calibre.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Calibre.HeaderText = "Calibre"
        Me.Calibre.MinimumWidth = 70
        Me.Calibre.Name = "Calibre"
        Me.Calibre.ReadOnly = True
        Me.Calibre.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Calibre.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.Calibre.Width = 70
        '
        'Stock
        '
        Me.Stock.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Stock.DataPropertyName = "Stock"
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Stock.DefaultCellStyle = DataGridViewCellStyle7
        Me.Stock.HeaderText = "Stock"
        Me.Stock.MinimumWidth = 70
        Me.Stock.Name = "Stock"
        Me.Stock.ReadOnly = True
        Me.Stock.Width = 70
        '
        'PermisoImp
        '
        Me.PermisoImp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PermisoImp.DataPropertyName = "PermisoImp"
        Me.PermisoImp.HeaderText = "Permiso Imp."
        Me.PermisoImp.MinimumWidth = 110
        Me.PermisoImp.Name = "PermisoImp"
        Me.PermisoImp.ReadOnly = True
        Me.PermisoImp.Width = 110
        '
        'Asignado
        '
        Me.Asignado.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Asignado.DataPropertyName = "Asignado"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Asignado.DefaultCellStyle = DataGridViewCellStyle8
        Me.Asignado.HeaderText = "Asignar"
        Me.Asignado.MaxInputLength = 12
        Me.Asignado.MinimumWidth = 70
        Me.Asignado.Name = "Asignado"
        Me.Asignado.Width = 70
        '
        'SeleccionaLotesAAsignar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 580)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.LabelFaltaAsignar)
        Me.Controls.Add(Me.LabelUnidad)
        Me.Controls.Add(Me.LabelDeposito)
        Me.Controls.Add(Me.LabelArticulo)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.LabCantidad)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.KeyPreview = True
        Me.Name = "SeleccionaLotesAAsignar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Selecciona lotes"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LabCantidad As System.Windows.Forms.Label
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents LabelArticulo As System.Windows.Forms.Label
    Friend WithEvents LabelDeposito As System.Windows.Forms.Label
    Friend WithEvents LabelUnidad As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents LabelFaltaAsignar As System.Windows.Forms.Label
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents OtrasAsignaciones As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteOrigen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DepositoOrigen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SecuenciaOrigen As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents KilosXUnidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Proveedor As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Fecha As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Calibre As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Stock As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PermisoImp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Asignado As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
