<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaActualizacionPedido
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
        Me.PanelPedido = New System.Windows.Forms.Panel
        Me.Label1 = New System.Windows.Forms.Label
        Me.TextPedido = New System.Windows.Forms.TextBox
        Me.TextCliente = New System.Windows.Forms.TextBox
        Me.ButtonActualizar = New System.Windows.Forms.Button
        Me.ButtonAnular = New System.Windows.Forms.Button
        Me.TextPorCuentaYOrden = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.PanelPedido.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelPedido
        '
        Me.PanelPedido.Controls.Add(Me.Label1)
        Me.PanelPedido.Controls.Add(Me.TextPedido)
        Me.PanelPedido.Location = New System.Drawing.Point(8, 152)
        Me.PanelPedido.Margin = New System.Windows.Forms.Padding(4)
        Me.PanelPedido.Name = "PanelPedido"
        Me.PanelPedido.Size = New System.Drawing.Size(533, 50)
        Me.PanelPedido.TabIndex = 274
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(16, 15)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 16)
        Me.Label1.TabIndex = 273
        Me.Label1.Text = "Pedido"
        '
        'TextPedido
        '
        Me.TextPedido.BackColor = System.Drawing.Color.White
        Me.TextPedido.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPedido.Location = New System.Drawing.Point(164, 9)
        Me.TextPedido.Margin = New System.Windows.Forms.Padding(4)
        Me.TextPedido.MaxLength = 8
        Me.TextPedido.Name = "TextPedido"
        Me.TextPedido.Size = New System.Drawing.Size(230, 26)
        Me.TextPedido.TabIndex = 272
        Me.TextPedido.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TextCliente
        '
        Me.TextCliente.BackColor = System.Drawing.Color.White
        Me.TextCliente.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCliente.Location = New System.Drawing.Point(176, 75)
        Me.TextCliente.Margin = New System.Windows.Forms.Padding(4)
        Me.TextCliente.MaxLength = 8
        Me.TextCliente.Name = "TextCliente"
        Me.TextCliente.ReadOnly = True
        Me.TextCliente.Size = New System.Drawing.Size(343, 26)
        Me.TextCliente.TabIndex = 275
        '
        'ButtonActualizar
        '
        Me.ButtonActualizar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonActualizar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonActualizar.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonActualizar.Location = New System.Drawing.Point(65, 271)
        Me.ButtonActualizar.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonActualizar.Name = "ButtonActualizar"
        Me.ButtonActualizar.Size = New System.Drawing.Size(423, 41)
        Me.ButtonActualizar.TabIndex = 276
        Me.ButtonActualizar.Text = "Actualizar Pedido con Remito"
        Me.ButtonActualizar.UseVisualStyleBackColor = False
        '
        'ButtonAnular
        '
        Me.ButtonAnular.BackColor = System.Drawing.Color.Red
        Me.ButtonAnular.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAnular.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAnular.Location = New System.Drawing.Point(65, 292)
        Me.ButtonAnular.Margin = New System.Windows.Forms.Padding(4)
        Me.ButtonAnular.Name = "ButtonAnular"
        Me.ButtonAnular.Size = New System.Drawing.Size(423, 41)
        Me.ButtonAnular.TabIndex = 277
        Me.ButtonAnular.Text = "Resta Remito del Pedido"
        Me.ButtonAnular.UseVisualStyleBackColor = False
        '
        'TextPorCuentaYOrden
        '
        Me.TextPorCuentaYOrden.BackColor = System.Drawing.Color.White
        Me.TextPorCuentaYOrden.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextPorCuentaYOrden.Location = New System.Drawing.Point(176, 107)
        Me.TextPorCuentaYOrden.Margin = New System.Windows.Forms.Padding(4)
        Me.TextPorCuentaYOrden.MaxLength = 8
        Me.TextPorCuentaYOrden.Name = "TextPorCuentaYOrden"
        Me.TextPorCuentaYOrden.ReadOnly = True
        Me.TextPorCuentaYOrden.Size = New System.Drawing.Size(343, 26)
        Me.TextPorCuentaYOrden.TabIndex = 278
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 80)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 16)
        Me.Label2.TabIndex = 279
        Me.Label2.Text = "Cliente"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 112)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(144, 16)
        Me.Label3.TabIndex = 280
        Me.Label3.Text = "Por Cuenta Y Orden"
        '
        'UnaActualizacionPedido
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(554, 582)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TextPorCuentaYOrden)
        Me.Controls.Add(Me.ButtonAnular)
        Me.Controls.Add(Me.ButtonActualizar)
        Me.Controls.Add(Me.TextCliente)
        Me.Controls.Add(Me.PanelPedido)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "UnaActualizacionPedido"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Actualizacion Pedido"
        Me.PanelPedido.ResumeLayout(False)
        Me.PanelPedido.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelPedido As System.Windows.Forms.Panel
    Friend WithEvents TextPedido As System.Windows.Forms.TextBox
    Friend WithEvents TextCliente As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonActualizar As System.Windows.Forms.Button
    Friend WithEvents ButtonAnular As System.Windows.Forms.Button
    Friend WithEvents TextPorCuentaYOrden As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
