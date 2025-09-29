<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CuilNoValidos
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
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Proceso = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Nombre = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Cuit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.ButtonExportarExcel = New System.Windows.Forms.Button
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Proceso, Me.Nombre, Me.Cuit})
        Me.Grid.Location = New System.Drawing.Point(111, 14)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(578, 527)
        Me.Grid.TabIndex = 0
        '
        'Proceso
        '
        Me.Proceso.HeaderText = "Proceso"
        Me.Proceso.MinimumWidth = 10
        Me.Proceso.Name = "Proceso"
        Me.Proceso.ReadOnly = True
        '
        'Nombre
        '
        Me.Nombre.HeaderText = "Nombre"
        Me.Nombre.Name = "Nombre"
        Me.Nombre.ReadOnly = True
        Me.Nombre.Width = 200
        '
        'Cuit
        '
        Me.Cuit.HeaderText = "Cuit"
        Me.Cuit.Name = "Cuit"
        Me.Cuit.ReadOnly = True
        '
        'ButtonExportarExcel
        '
        Me.ButtonExportarExcel.AutoEllipsis = True
        Me.ButtonExportarExcel.BackColor = System.Drawing.Color.Transparent
        Me.ButtonExportarExcel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ButtonExportarExcel.FlatAppearance.BorderSize = 0
        Me.ButtonExportarExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonExportarExcel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonExportarExcel.Image = Global.ScomerV01.My.Resources.Resources.Excel
        Me.ButtonExportarExcel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonExportarExcel.Location = New System.Drawing.Point(291, 547)
        Me.ButtonExportarExcel.Name = "ButtonExportarExcel"
        Me.ButtonExportarExcel.Size = New System.Drawing.Size(191, 32)
        Me.ButtonExportarExcel.TabIndex = 160
        Me.ButtonExportarExcel.Text = "Exportar a EXCEL"
        Me.ButtonExportarExcel.UseVisualStyleBackColor = False
        '
        'CuilNoValidos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(773, 586)
        Me.Controls.Add(Me.ButtonExportarExcel)
        Me.Controls.Add(Me.Grid)
        Me.Name = "CuilNoValidos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cuit  No Validos"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Proceso As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Nombre As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Cuit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents ButtonExportarExcel As System.Windows.Forms.Button
End Class
