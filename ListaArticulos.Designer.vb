<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ListaArticulos
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
        Me.TreeView1 = New System.Windows.Forms.TreeView
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ButtonListaAlfaveticamentecial = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'TreeView1
        '
        Me.TreeView1.BackColor = System.Drawing.Color.White
        Me.TreeView1.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TreeView1.ForeColor = System.Drawing.Color.OliveDrab
        Me.TreeView1.FullRowSelect = True
        Me.TreeView1.Indent = 25
        Me.TreeView1.Location = New System.Drawing.Point(76, 15)
        Me.TreeView1.Name = "TreeView1"
        Me.TreeView1.Size = New System.Drawing.Size(611, 627)
        Me.TreeView1.TabIndex = 0
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'ButtonListaAlfaveticamentecial
        '
        Me.ButtonListaAlfaveticamentecial.BackColor = System.Drawing.Color.Transparent
        Me.ButtonListaAlfaveticamentecial.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonListaAlfaveticamentecial.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonListaAlfaveticamentecial.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonListaAlfaveticamentecial.Location = New System.Drawing.Point(464, 646)
        Me.ButtonListaAlfaveticamentecial.Name = "ButtonListaAlfaveticamentecial"
        Me.ButtonListaAlfaveticamentecial.Size = New System.Drawing.Size(221, 27)
        Me.ButtonListaAlfaveticamentecial.TabIndex = 1
        Me.ButtonListaAlfaveticamentecial.Text = "Lista Activos y Deshabilitados"
        Me.ButtonListaAlfaveticamentecial.UseVisualStyleBackColor = False
        '
        'ListaArticulos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Gainsboro
        Me.ClientSize = New System.Drawing.Size(784, 676)
        Me.Controls.Add(Me.ButtonListaAlfaveticamentecial)
        Me.Controls.Add(Me.TreeView1)
        Me.Name = "ListaArticulos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Articulos"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TreeView1 As System.Windows.Forms.TreeView
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ButtonListaAlfaveticamentecial As System.Windows.Forms.Button
End Class
