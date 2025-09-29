<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OpcionLotesAImputar
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OpcionLotesAImputar))
        Me.Panel6 = New System.Windows.Forms.Panel
        Me.ButtonBorraLineaLotes = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.OperacionLote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CandadoLote = New System.Windows.Forms.DataGridViewImageColumn
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteYSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cantidad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PictureLupa = New System.Windows.Forms.PictureBox
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.Panel6.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureLupa, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.Thistle
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.ButtonBorraLineaLotes)
        Me.Panel6.Controls.Add(Me.Grid)
        Me.Panel6.Controls.Add(Me.PictureLupa)
        Me.Panel6.Location = New System.Drawing.Point(98, 37)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(441, 316)
        Me.Panel6.TabIndex = 288
        '
        'ButtonBorraLineaLotes
        '
        Me.ButtonBorraLineaLotes.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonBorraLineaLotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBorraLineaLotes.Location = New System.Drawing.Point(72, 280)
        Me.ButtonBorraLineaLotes.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonBorraLineaLotes.Name = "ButtonBorraLineaLotes"
        Me.ButtonBorraLineaLotes.Size = New System.Drawing.Size(98, 20)
        Me.ButtonBorraLineaLotes.TabIndex = 270
        Me.ButtonBorraLineaLotes.TabStop = False
        Me.ButtonBorraLineaLotes.Text = "Borrar Linea"
        Me.ButtonBorraLineaLotes.UseVisualStyleBackColor = False
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
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.OperacionLote, Me.CandadoLote, Me.Lote, Me.Secuencia, Me.LoteYSecuencia, Me.Cantidad})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle4
        Me.Grid.Location = New System.Drawing.Point(74, 12)
        Me.Grid.MultiSelect = False
        Me.Grid.Name = "Grid"
        Me.Grid.RowHeadersWidth = 25
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Ivory
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.CornflowerBlue
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle5
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(256, 265)
        Me.Grid.TabIndex = 268
        '
        'OperacionLote
        '
        Me.OperacionLote.DataPropertyName = "Operacion"
        Me.OperacionLote.HeaderText = "Operacion"
        Me.OperacionLote.Name = "OperacionLote"
        Me.OperacionLote.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.OperacionLote.Visible = False
        Me.OperacionLote.Width = 62
        '
        'CandadoLote
        '
        Me.CandadoLote.HeaderText = ""
        Me.CandadoLote.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.CandadoLote.MinimumWidth = 30
        Me.CandadoLote.Name = "CandadoLote"
        Me.CandadoLote.ReadOnly = True
        Me.CandadoLote.Width = 30
        '
        'Lote
        '
        Me.Lote.DataPropertyName = "Lote"
        Me.Lote.HeaderText = "Lote"
        Me.Lote.Name = "Lote"
        Me.Lote.Visible = False
        Me.Lote.Width = 53
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        Me.Secuencia.Width = 83
        '
        'LoteYSecuencia
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LoteYSecuencia.DefaultCellStyle = DataGridViewCellStyle2
        Me.LoteYSecuencia.HeaderText = "Lotes"
        Me.LoteYSecuencia.MinimumWidth = 100
        Me.LoteYSecuencia.Name = "LoteYSecuencia"
        Me.LoteYSecuencia.ReadOnly = True
        '
        'Cantidad
        '
        Me.Cantidad.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cantidad.DataPropertyName = "Cantidad"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cantidad.DefaultCellStyle = DataGridViewCellStyle3
        Me.Cantidad.HeaderText = "Cantidad"
        Me.Cantidad.MinimumWidth = 70
        Me.Cantidad.Name = "Cantidad"
        Me.Cantidad.ReadOnly = True
        Me.Cantidad.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cantidad.Width = 70
        '
        'PictureLupa
        '
        Me.PictureLupa.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.PictureLupa.InitialImage = Nothing
        Me.PictureLupa.Location = New System.Drawing.Point(355, 105)
        Me.PictureLupa.Name = "PictureLupa"
        Me.PictureLupa.Size = New System.Drawing.Size(61, 61)
        Me.PictureLupa.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureLupa.TabIndex = 269
        Me.PictureLupa.TabStop = False
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(243, 397)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 289
        Me.ButtonAceptar.Text = "Aceptar "
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
        'OpcionLotesAImputar
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Wheat
        Me.ClientSize = New System.Drawing.Size(637, 449)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Panel6)
        Me.Name = "OpcionLotesAImputar"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Lotes a Imputar"
        Me.Panel6.ResumeLayout(False)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureLupa, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel6 As System.Windows.Forms.Panel
    Friend WithEvents ButtonBorraLineaLotes As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents OperacionLote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CandadoLote As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteYSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cantidad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PictureLupa As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
End Class
