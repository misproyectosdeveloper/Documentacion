<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaFamiliares
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Clave = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Apellido = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Parentesco = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Masculino = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Nacimiento = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuil = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonPrimero = New System.Windows.Forms.Button
        Me.ButtonAnterior = New System.Windows.Forms.Button
        Me.ButtonPosterior = New System.Windows.Forms.Button
        Me.ButtonUltimo = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Tan
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(656, 372)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonAceptar.TabIndex = 9
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Location = New System.Drawing.Point(504, 372)
        Me.ButtonEliminar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(114, 23)
        Me.ButtonEliminar.TabIndex = 8
        Me.ButtonEliminar.TabStop = False
        Me.ButtonEliminar.Text = "Borrar Linea"
        Me.ButtonEliminar.UseVisualStyleBackColor = False
        '
        'Grid
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Clave, Me.Nombre, Me.Apellido, Me.Parentesco, Me.Masculino, Me.Nacimiento, Me.Cuil})
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid.DefaultCellStyle = DataGridViewCellStyle7
        Me.Grid.Location = New System.Drawing.Point(74, 46)
        Me.Grid.Name = "Grid"
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Grid.RowHeadersDefaultCellStyle = DataGridViewCellStyle8
        DataGridViewCellStyle9.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.RowsDefaultCellStyle = DataGridViewCellStyle9
        Me.Grid.Size = New System.Drawing.Size(697, 322)
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
        Me.Nombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Nombre.DataPropertyName = "Nombre"
        Me.Nombre.HeaderText = "Nombre"
        Me.Nombre.MaxInputLength = 30
        Me.Nombre.MinimumWidth = 130
        Me.Nombre.Name = "Nombre"
        Me.Nombre.Width = 130
        '
        'Apellido
        '
        Me.Apellido.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Apellido.DataPropertyName = "Apellido"
        Me.Apellido.HeaderText = "Apellido"
        Me.Apellido.MaxInputLength = 30
        Me.Apellido.MinimumWidth = 130
        Me.Apellido.Name = "Apellido"
        Me.Apellido.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Apellido.Width = 130
        '
        'Parentesco
        '
        Me.Parentesco.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Parentesco.DataPropertyName = "Parentesco"
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        Me.Parentesco.DefaultCellStyle = DataGridViewCellStyle4
        Me.Parentesco.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.Parentesco.HeaderText = "Parentesco"
        Me.Parentesco.MinimumWidth = 120
        Me.Parentesco.Name = "Parentesco"
        Me.Parentesco.Width = 120
        '
        'Masculino
        '
        Me.Masculino.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Masculino.DataPropertyName = "Masculino"
        Me.Masculino.HeaderText = "Masculino"
        Me.Masculino.MinimumWidth = 60
        Me.Masculino.Name = "Masculino"
        Me.Masculino.Width = 60
        '
        'Nacimiento
        '
        Me.Nacimiento.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Nacimiento.DataPropertyName = "FechaNacimiento"
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Nacimiento.DefaultCellStyle = DataGridViewCellStyle5
        Me.Nacimiento.HeaderText = "Nacimiento"
        Me.Nacimiento.MaxInputLength = 8
        Me.Nacimiento.MinimumWidth = 80
        Me.Nacimiento.Name = "Nacimiento"
        Me.Nacimiento.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Nacimiento.Width = 80
        '
        'Cuil
        '
        Me.Cuil.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Cuil.DataPropertyName = "Cuil"
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Cuil.DefaultCellStyle = DataGridViewCellStyle6
        Me.Cuil.HeaderText = "Cuil"
        Me.Cuil.MaxInputLength = 11
        Me.Cuil.MinimumWidth = 110
        Me.Cuil.Name = "Cuil"
        Me.Cuil.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Cuil.Width = 110
        '
        'ButtonPrimero
        '
        Me.ButtonPrimero.AutoEllipsis = True
        Me.ButtonPrimero.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonPrimero.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPrimero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPrimero.Location = New System.Drawing.Point(76, 370)
        Me.ButtonPrimero.Name = "ButtonPrimero"
        Me.ButtonPrimero.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPrimero.TabIndex = 158
        Me.ButtonPrimero.TabStop = False
        Me.ButtonPrimero.Text = "<<"
        Me.ButtonPrimero.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPrimero.UseVisualStyleBackColor = False
        '
        'ButtonAnterior
        '
        Me.ButtonAnterior.AutoEllipsis = True
        Me.ButtonAnterior.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAnterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnterior.Location = New System.Drawing.Point(109, 370)
        Me.ButtonAnterior.Name = "ButtonAnterior"
        Me.ButtonAnterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonAnterior.TabIndex = 159
        Me.ButtonAnterior.TabStop = False
        Me.ButtonAnterior.Text = "<"
        Me.ButtonAnterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonAnterior.UseVisualStyleBackColor = False
        '
        'ButtonPosterior
        '
        Me.ButtonPosterior.AutoEllipsis = True
        Me.ButtonPosterior.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonPosterior.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonPosterior.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonPosterior.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPosterior.Location = New System.Drawing.Point(142, 370)
        Me.ButtonPosterior.Name = "ButtonPosterior"
        Me.ButtonPosterior.Size = New System.Drawing.Size(31, 22)
        Me.ButtonPosterior.TabIndex = 160
        Me.ButtonPosterior.TabStop = False
        Me.ButtonPosterior.Text = ">"
        Me.ButtonPosterior.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonPosterior.UseVisualStyleBackColor = False
        '
        'ButtonUltimo
        '
        Me.ButtonUltimo.AutoEllipsis = True
        Me.ButtonUltimo.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ButtonUltimo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonUltimo.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonUltimo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonUltimo.Location = New System.Drawing.Point(175, 370)
        Me.ButtonUltimo.Name = "ButtonUltimo"
        Me.ButtonUltimo.Size = New System.Drawing.Size(31, 22)
        Me.ButtonUltimo.TabIndex = 161
        Me.ButtonUltimo.TabStop = False
        Me.ButtonUltimo.Text = ">>"
        Me.ButtonUltimo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ButtonUltimo.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(74, 451)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(646, 13)
        Me.Label1.TabIndex = 162
        Me.Label1.Text = "NACIMIENTO y CUIL deben ingresarse con dígitos numéricos sin barra o guiones. (Ej" & _
            "emplos.   Fecha:  01032012   Cuil:  01124456781 "
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(235, 13)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(395, 26)
        Me.Label2.TabIndex = 163
        Me.Label2.Text = "Nombre"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ListaFamiliares
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(852, 480)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonPrimero)
        Me.Controls.Add(Me.ButtonAnterior)
        Me.Controls.Add(Me.ButtonPosterior)
        Me.Controls.Add(Me.ButtonUltimo)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Name = "ListaFamiliares"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Familiares"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonPrimero As System.Windows.Forms.Button
    Friend WithEvents ButtonAnterior As System.Windows.Forms.Button
    Friend WithEvents ButtonPosterior As System.Windows.Forms.Button
    Friend WithEvents ButtonUltimo As System.Windows.Forms.Button
    Friend WithEvents Sexo As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Clave As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Apellido As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Parentesco As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Masculino As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Nacimiento As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuil As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
