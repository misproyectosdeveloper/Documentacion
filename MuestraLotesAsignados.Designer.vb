<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MuestraLotesAsignados
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Operacion = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.candado = New System.Windows.Forms.DataGridViewImageColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Deposito = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.PermisoImp = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.AllowUserToAddRows = False
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.LightGray
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Operacion, Me.Lote, Me.Secuencia, Me.candado, Me.LoteYSecuencia, Me.Articulo, Me.Deposito, Me.PermisoImp, Me.Cantidad})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.Location = New System.Drawing.Point(25, 40)
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        Me.Grid.Size = New System.Drawing.Size(706, 373)
        Me.Grid.TabIndex = 17
        '
        'Operacion
        '
        Me.Operacion.DataPropertyName = "Operacion"
        Me.Operacion.HeaderText = "Operacion"
        Me.Operacion.Name = "Operacion"
        Me.Operacion.Visible = False
        '
        'Lote
        '
        Me.Lote.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Lote.DataPropertyName = "Lote"
        Me.Lote.HeaderText = "Lote"
        Me.Lote.MinimumWidth = 70
        Me.Lote.Name = "Lote"
        Me.Lote.Visible = False
        Me.Lote.Width = 70
        '
        'Secuencia
        '
        Me.Secuencia.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.MinimumWidth = 70
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        Me.Secuencia.Width = 70
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
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle3
        Me.LoteYSecuencia.HeaderText = "Lote"
        Me.LoteYSecuencia.MinimumWidth = 80
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        Me.LoteYSecuencia.Width = 80
        '
        'Articulo
        '
        Me.Articulo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Articulo.DataPropertyName = "Articulo"
        Me.Articulo.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.MinimumWidth = 200
        Me.Articulo.Name = "Articulo"
        Me.Articulo.ReadOnly = True
        Me.Articulo.Width = 200
        '
        'Deposito
        '
        Me.Deposito.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Deposito.DataPropertyName = "Deposito"
        Me.Deposito.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.[Nothing]
        Me.Deposito.HeaderText = "Deposito"
        Me.Deposito.MinimumWidth = 90
        Me.Deposito.Name = "Deposito"
        Me.Deposito.ReadOnly = True
        Me.Deposito.Width = 90
        '
        'PermisoImp
        '
        Me.PermisoImp.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.PermisoImp.DataPropertyName = "PermisoImp"
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.PermisoImp.DefaultCellStyle = DataGridViewCellStyle4
        Me.PermisoImp.HeaderText = "Permiso Impor."
        Me.PermisoImp.MinimumWidth = 120
        Me.PermisoImp.Name = "PermisoImp"
        Me.PermisoImp.ReadOnly = True
        Me.PermisoImp.Width = 120
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle5
        Me.Cantidad.HeaderText = "Asignado"
        Me.Cantidad.MaxInputLength = 8
        Me.Cantidad.MinimumWidth = 80
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.Width = 80
        '
        'MuestraLotesAsignados
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(759, 467)
        Me.Controls.Add(Me.Grid)
        Me.Name = "MuestraLotesAsignados"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lotes Asignados "
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Operacion As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents candado As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Deposito As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents PermisoImp As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
