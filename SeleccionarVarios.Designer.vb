<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SeleccionarVarios
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
        Dim ImageList1 As System.Windows.Forms.ImageList
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SeleccionarVarios))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PanelMarcarTodos = New System.Windows.Forms.Panel
        Me.ButtonDesmarcarTodos = New System.Windows.Forms.Button
        Me.ButtonMarcarTodos = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelMarcarTodos.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        ImageList1.TransparentColor = System.Drawing.Color.Transparent
        ImageList1.Images.SetKeyName(0, "Abierto")
        ImageList1.Images.SetKeyName(1, "Cerrado")
        '
        'Grid
        '
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White
        Me.Grid.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Location = New System.Drawing.Point(193, 38)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(240, 369)
        Me.Grid.TabIndex = 0
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(236, 439)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(151, 26)
        Me.ButtonAceptar.TabIndex = 136
        Me.ButtonAceptar.Text = "Aceptar "
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'PanelMarcarTodos
        '
        Me.PanelMarcarTodos.Controls.Add(Me.ButtonDesmarcarTodos)
        Me.PanelMarcarTodos.Controls.Add(Me.ButtonMarcarTodos)
        Me.PanelMarcarTodos.Location = New System.Drawing.Point(5, 5)
        Me.PanelMarcarTodos.Name = "PanelMarcarTodos"
        Me.PanelMarcarTodos.Size = New System.Drawing.Size(219, 33)
        Me.PanelMarcarTodos.TabIndex = 139
        Me.PanelMarcarTodos.Visible = False
        '
        'ButtonDesmarcarTodos
        '
        Me.ButtonDesmarcarTodos.Location = New System.Drawing.Point(109, 6)
        Me.ButtonDesmarcarTodos.Name = "ButtonDesmarcarTodos"
        Me.ButtonDesmarcarTodos.Size = New System.Drawing.Size(102, 21)
        Me.ButtonDesmarcarTodos.TabIndex = 140
        Me.ButtonDesmarcarTodos.Text = "Des-Marcar"
        Me.ButtonDesmarcarTodos.UseVisualStyleBackColor = True
        '
        'ButtonMarcarTodos
        '
        Me.ButtonMarcarTodos.Location = New System.Drawing.Point(-2, 6)
        Me.ButtonMarcarTodos.Name = "ButtonMarcarTodos"
        Me.ButtonMarcarTodos.Size = New System.Drawing.Size(102, 21)
        Me.ButtonMarcarTodos.TabIndex = 139
        Me.ButtonMarcarTodos.Text = "Marcar Todos"
        Me.ButtonMarcarTodos.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 482)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(400, 13)
        Me.Label1.TabIndex = 140
        Me.Label1.Text = "Para Ordenar por una columna hacer Click con el mouse sobre titulo de la columna." & _
            ""
        '
        'SeleccionarVarios
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(607, 501)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.PanelMarcarTodos)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.Grid)
        Me.KeyPreview = True
        Me.Name = "SeleccionarVarios"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelMarcarTodos.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents PanelMarcarTodos As System.Windows.Forms.Panel
    Friend WithEvents ButtonDesmarcarTodos As System.Windows.Forms.Button
    Friend WithEvents ButtonMarcarTodos As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
