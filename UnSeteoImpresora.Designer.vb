<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnSeteoImpresora
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.TextSizePapel = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextImprsoraPredeterminada = New System.Windows.Forms.TextBox
        Me.TextNombrePc = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.LabelEmisor = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Impresora = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MargenIzquierdo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.MargenSuperior = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Predeterminada = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Panel1.SuspendLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.Salmon
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(235, 584)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(228, 53)
        Me.ButtonAceptar.TabIndex = 162
        Me.ButtonAceptar.Text = "GRABAR"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.TextSizePapel)
        Me.Panel1.Controls.Add(Me.Label6)
        Me.Panel1.Controls.Add(Me.TextImprsoraPredeterminada)
        Me.Panel1.Controls.Add(Me.TextNombrePc)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.LabelEmisor)
        Me.Panel1.Location = New System.Drawing.Point(56, 68)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(599, 87)
        Me.Panel1.TabIndex = 172
        '
        'TextSizePapel
        '
        Me.TextSizePapel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextSizePapel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSizePapel.Location = New System.Drawing.Point(311, 54)
        Me.TextSizePapel.MaxLength = 4
        Me.TextSizePapel.Name = "TextSizePapel"
        Me.TextSizePapel.ReadOnly = True
        Me.TextSizePapel.Size = New System.Drawing.Size(260, 21)
        Me.TextSizePapel.TabIndex = 177
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(28, 57)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 15)
        Me.Label6.TabIndex = 176
        Me.Label6.Text = "Medida Papel"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextImprsoraPredeterminada
        '
        Me.TextImprsoraPredeterminada.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextImprsoraPredeterminada.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImprsoraPredeterminada.Location = New System.Drawing.Point(311, 30)
        Me.TextImprsoraPredeterminada.MaxLength = 4
        Me.TextImprsoraPredeterminada.Name = "TextImprsoraPredeterminada"
        Me.TextImprsoraPredeterminada.ReadOnly = True
        Me.TextImprsoraPredeterminada.Size = New System.Drawing.Size(260, 21)
        Me.TextImprsoraPredeterminada.TabIndex = 175
        '
        'TextNombrePc
        '
        Me.TextNombrePc.BackColor = System.Drawing.Color.WhiteSmoke
        Me.TextNombrePc.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombrePc.Location = New System.Drawing.Point(311, 8)
        Me.TextNombrePc.MaxLength = 4
        Me.TextNombrePc.Name = "TextNombrePc"
        Me.TextNombrePc.ReadOnly = True
        Me.TextNombrePc.Size = New System.Drawing.Size(260, 21)
        Me.TextNombrePc.TabIndex = 174
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(28, 11)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(84, 15)
        Me.Label2.TabIndex = 173
        Me.Label2.Text = "Nombre PC."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelEmisor
        '
        Me.LabelEmisor.AutoSize = True
        Me.LabelEmisor.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelEmisor.Location = New System.Drawing.Point(26, 33)
        Me.LabelEmisor.Name = "LabelEmisor"
        Me.LabelEmisor.Size = New System.Drawing.Size(222, 15)
        Me.LabelEmisor.TabIndex = 172
        Me.LabelEmisor.Text = "Impresora Predeterminada Actual"
        Me.LabelEmisor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(54, 36)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(601, 30)
        Me.Label7.TabIndex = 174
        Me.Label7.Text = "Valores Actuales de Impresión establecidos en panel de Impresoras"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(56, 168)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(599, 43)
        Me.Label8.TabIndex = 176
        Me.Label8.Text = "Modifica márgenes de impresión. Solo para Facturas, Remitos,Liquidaciones, Notas " & _
            "debito/crédito. "
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Grid
        '
        Me.Grid.BackgroundColor = System.Drawing.Color.White
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Impresora, Me.MargenIzquierdo, Me.MargenSuperior, Me.Predeterminada})
        Me.Grid.Location = New System.Drawing.Point(57, 217)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(598, 237)
        Me.Grid.TabIndex = 177
        '
        'Impresora
        '
        Me.Impresora.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Impresora.HeaderText = "Impresora"
        Me.Impresora.MinimumWidth = 230
        Me.Impresora.Name = "Impresora"
        Me.Impresora.ReadOnly = True
        Me.Impresora.Width = 230
        '
        'MargenIzquierdo
        '
        Me.MargenIzquierdo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.MargenIzquierdo.DefaultCellStyle = DataGridViewCellStyle3
        Me.MargenIzquierdo.HeaderText = "Margen Izquierdo (mm)"
        Me.MargenIzquierdo.MaxInputLength = 4
        Me.MargenIzquierdo.MinimumWidth = 100
        Me.MargenIzquierdo.Name = "MargenIzquierdo"
        '
        'MargenSuperior
        '
        Me.MargenSuperior.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.MargenSuperior.DefaultCellStyle = DataGridViewCellStyle4
        Me.MargenSuperior.HeaderText = "Margen Superior  (mm)"
        Me.MargenSuperior.MaxInputLength = 4
        Me.MargenSuperior.MinimumWidth = 100
        Me.MargenSuperior.Name = "MargenSuperior"
        '
        'Predeterminada
        '
        Me.Predeterminada.HeaderText = "Seleccionada"
        Me.Predeterminada.Name = "Predeterminada"
        Me.Predeterminada.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Predeterminada.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(54, 460)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(599, 24)
        Me.Label1.TabIndex = 178
        Me.Label1.Text = "Margenes Izquierdo y Superior con espacios toma valores predeterminado por el sis" & _
            "tema."
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(53, 482)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(610, 34)
        Me.Label3.TabIndex = 179
        Me.Label3.Text = "Seleccionada: es la impresora por donde se Imprimirá. Si no se especifica toma la" & _
            " impresora pre-determinada en panel de impresoras."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(67, 4)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(601, 28)
        Me.Label5.TabIndex = 181
        Me.Label5.Text = "Parámetros  para Impresión"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(54, 516)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(599, 18)
        Me.Label4.TabIndex = 182
        Me.Label4.Text = "El sistema asigna la Hoja A4 Automáticamente a la Impresora."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'UnSeteoImpresora
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(734, 654)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Grid)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Name = "UnSeteoImpresora"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Seteo Impresora"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextSizePapel As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextImprsoraPredeterminada As System.Windows.Forms.TextBox
    Friend WithEvents TextNombrePc As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LabelEmisor As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Impresora As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MargenIzquierdo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MargenSuperior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Predeterminada As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
