<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaCentrosDeCosto
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
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ListaCentrosDeCosto))
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.ButtonEliminarLinea = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Centro = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Comentario = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Lupa = New System.Windows.Forms.DataGridViewImageColumn
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.DataGridViewImageColumn1 = New System.Windows.Forms.DataGridViewImageColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(520, 645)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 20)
        Me.ButtonAceptar.TabIndex = 245
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.Transparent
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.Location = New System.Drawing.Point(196, 645)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 21)
        Me.ButtonUltimo.TabIndex = 240
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'ButtonEliminarLinea
        '
        Me.ButtonEliminarLinea.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLinea.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLinea.Location = New System.Drawing.Point(393, 644)
        Me.ButtonEliminarLinea.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLinea.Name = "ButtonEliminarLinea"
        Me.ButtonEliminarLinea.Size = New System.Drawing.Size(114, 20)
        Me.ButtonEliminarLinea.TabIndex = 244
        Me.ButtonEliminarLinea.Text = "Borrar Linea"
        Me.ButtonEliminarLinea.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPosterior.Location = New System.Drawing.Point(165, 645)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(29, 21)
        Me.ButtonPosterior.TabIndex = 243
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.Transparent
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior.Location = New System.Drawing.Point(135, 645)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(29, 21)
        Me.ButtonAnterior.TabIndex = 242
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.Transparent
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.Location = New System.Drawing.Point(102, 645)
        Me.ButtonPrimero.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 21)
        Me.ButtonPrimero.TabIndex = 241
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'Grid
        '
        Me.Grid.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGray
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Centro, Me.Nombre, Me.Comentario, Me.Lupa})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle4
        Me.Grid.Location = New System.Drawing.Point(103, 7)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.Transparent
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.LightBlue
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Black
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle6
        Me.Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid.Size = New System.Drawing.Size(535, 632)
        Me.Grid.TabIndex = 239
        '
        'Centro
        '
        Me.Centro.DataPropertyName = "Centro"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Centro.DefaultCellStyle = DataGridViewCellStyle2
        Me.Centro.HeaderText = "Centro"
        Me.Centro.MaxInputLength = 3
        Me.Centro.MinimumWidth = 90
        Me.Centro.Name = "Centro"
        Me.Centro.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Centro.Width = 90
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
        'Comentario
        '
        Me.Comentario.DataPropertyName = "Comentario"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.Comentario.DefaultCellStyle = DataGridViewCellStyle3
        Me.Comentario.HeaderText = "Comentario"
        Me.Comentario.MaxInputLength = 30
        Me.Comentario.MinimumWidth = 150
        Me.Comentario.Name = "Comentario"
        Me.Comentario.Width = 150
        '
        'Lupa
        '
        Me.Lupa.HeaderText = "Cuentas Asociadas"
        Me.Lupa.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.Lupa.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.Lupa.Name = "Lupa"
        Me.Lupa.ReadOnly = True
        Me.Lupa.Width = 94
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "Abierto")
        Me.ImageList1.Images.SetKeyName(1, "Cerrado")
        Me.ImageList1.Images.SetKeyName(2, "Lupa")
        '
        'DataGridViewImageColumn1
        '
        Me.DataGridViewImageColumn1.HeaderText = "Cuentas Asociadas"
        Me.DataGridViewImageColumn1.Image = Global.ScomerV01.My.Resources.Resources.icono176
        Me.DataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom
        Me.DataGridViewImageColumn1.Name = "DataGridViewImageColumn1"
        Me.DataGridViewImageColumn1.ReadOnly = True
        Me.DataGridViewImageColumn1.Width = 94
        '
        'ListaCentrosDeCosto
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Thistle
        Me.ClientSize = New System.Drawing.Size(761, 676)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.ButtonEliminarLinea)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.Grid)
        Me.Name = "ListaCentrosDeCosto"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Centros De Costo"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminarLinea As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents Centro As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Comentario As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Lupa As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents DataGridViewImageColumn1 As System.Windows.Forms.DataGridViewImageColumn
End Class
