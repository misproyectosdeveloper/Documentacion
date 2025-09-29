<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnaCuentaBancaria
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
        Me.ComboBancos = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.CheckLiquidaDivisa = New System.Windows.Forms.CheckBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.TextSaldoInicial = New System.Windows.Forms.TextBox
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.TextNumeracionFinal = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.TextNumeracionInicial = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.TextUltimoNumero = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.TextUltimaSerie = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.ComboMoneda = New System.Windows.Forms.ComboBox
        Me.CheckTieneChequera = New System.Windows.Forms.CheckBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextCbu = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ComboTipo = New System.Windows.Forms.ComboBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextNumero = New System.Windows.Forms.TextBox
        Me.PanelSucursal = New System.Windows.Forms.Panel
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextNombreSucursal = New System.Windows.Forms.TextBox
        Me.TextSucursal = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.ButtonEliminar = New System.Windows.Forms.Button
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.PanelSucursal.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBancos
        '
        Me.ComboBancos.BackColor = System.Drawing.Color.White
        Me.ComboBancos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBancos.Enabled = False
        Me.ComboBancos.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBancos.FormattingEnabled = True
        Me.ComboBancos.Location = New System.Drawing.Point(110, 25)
        Me.ComboBancos.Name = "ComboBancos"
        Me.ComboBancos.Size = New System.Drawing.Size(229, 24)
        Me.ComboBancos.TabIndex = 7
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(44, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Banco"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.LightBlue
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.CheckLiquidaDivisa)
        Me.Panel2.Controls.Add(Me.Label12)
        Me.Panel2.Controls.Add(Me.TextSaldoInicial)
        Me.Panel2.Controls.Add(Me.Panel3)
        Me.Panel2.Controls.Add(Me.Label11)
        Me.Panel2.Controls.Add(Me.ComboMoneda)
        Me.Panel2.Controls.Add(Me.CheckTieneChequera)
        Me.Panel2.Controls.Add(Me.Label5)
        Me.Panel2.Controls.Add(Me.TextCbu)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.ComboTipo)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.TextNumero)
        Me.Panel2.Location = New System.Drawing.Point(48, 146)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(647, 231)
        Me.Panel2.TabIndex = 11
        '
        'CheckLiquidaDivisa
        '
        Me.CheckLiquidaDivisa.AutoSize = True
        Me.CheckLiquidaDivisa.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckLiquidaDivisa.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckLiquidaDivisa.Location = New System.Drawing.Point(398, 106)
        Me.CheckLiquidaDivisa.Name = "CheckLiquidaDivisa"
        Me.CheckLiquidaDivisa.Size = New System.Drawing.Size(112, 20)
        Me.CheckLiquidaDivisa.TabIndex = 199
        Me.CheckLiquidaDivisa.Text = "Liquida Divisa"
        Me.CheckLiquidaDivisa.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckLiquidaDivisa.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(5, 50)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(74, 15)
        Me.Label12.TabIndex = 198
        Me.Label12.Text = "Saldo Inicial"
        '
        'TextSaldoInicial
        '
        Me.TextSaldoInicial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSaldoInicial.Location = New System.Drawing.Point(87, 47)
        Me.TextSaldoInicial.MaxLength = 10
        Me.TextSaldoInicial.Name = "TextSaldoInicial"
        Me.TextSaldoInicial.Size = New System.Drawing.Size(112, 22)
        Me.TextSaldoInicial.TabIndex = 9
        Me.TextSaldoInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.TextNumeracionFinal)
        Me.Panel3.Controls.Add(Me.Label14)
        Me.Panel3.Controls.Add(Me.TextNumeracionInicial)
        Me.Panel3.Controls.Add(Me.Label13)
        Me.Panel3.Controls.Add(Me.Label10)
        Me.Panel3.Controls.Add(Me.TextUltimoNumero)
        Me.Panel3.Controls.Add(Me.Label9)
        Me.Panel3.Controls.Add(Me.TextUltimaSerie)
        Me.Panel3.Location = New System.Drawing.Point(122, 147)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(434, 67)
        Me.Panel3.TabIndex = 196
        Me.Panel3.Visible = False
        '
        'TextNumeracionFinal
        '
        Me.TextNumeracionFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeracionFinal.Location = New System.Drawing.Point(319, 37)
        Me.TextNumeracionFinal.MaxLength = 10
        Me.TextNumeracionFinal.Name = "TextNumeracionFinal"
        Me.TextNumeracionFinal.Size = New System.Drawing.Size(90, 22)
        Me.TextNumeracionFinal.TabIndex = 32
        Me.TextNumeracionFinal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(272, 40)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(34, 15)
        Me.Label14.TabIndex = 200
        Me.Label14.Text = "Final"
        '
        'TextNumeracionInicial
        '
        Me.TextNumeracionInicial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumeracionInicial.Location = New System.Drawing.Point(134, 36)
        Me.TextNumeracionInicial.MaxLength = 10
        Me.TextNumeracionInicial.Name = "TextNumeracionInicial"
        Me.TextNumeracionInicial.Size = New System.Drawing.Size(90, 22)
        Me.TextNumeracionInicial.TabIndex = 30
        Me.TextNumeracionInicial.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(7, 40)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(119, 15)
        Me.Label13.TabIndex = 198
        Me.Label13.Text = "Numeración :  Inicial"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(176, 11)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(136, 15)
        Me.Label10.TabIndex = 197
        Me.Label10.Text = "Ultimo Numero Emitido"
        '
        'TextUltimoNumero
        '
        Me.TextUltimoNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUltimoNumero.Location = New System.Drawing.Point(321, 7)
        Me.TextUltimoNumero.MaxLength = 10
        Me.TextUltimoNumero.Name = "TextUltimoNumero"
        Me.TextUltimoNumero.Size = New System.Drawing.Size(90, 22)
        Me.TextUltimoNumero.TabIndex = 26
        Me.TextUltimoNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(5, 11)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(75, 15)
        Me.Label9.TabIndex = 196
        Me.Label9.Text = "Ultima Serie"
        '
        'TextUltimaSerie
        '
        Me.TextUltimaSerie.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me.TextUltimaSerie.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextUltimaSerie.Location = New System.Drawing.Point(90, 7)
        Me.TextUltimaSerie.MaxLength = 1
        Me.TextUltimaSerie.Name = "TextUltimaSerie"
        Me.TextUltimaSerie.Size = New System.Drawing.Size(31, 22)
        Me.TextUltimaSerie.TabIndex = 24
        Me.TextUltimaSerie.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(396, 50)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(56, 15)
        Me.Label11.TabIndex = 195
        Me.Label11.Text = "Moneda "
        '
        'ComboMoneda
        '
        Me.ComboMoneda.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboMoneda.FormattingEnabled = True
        Me.ComboMoneda.Location = New System.Drawing.Point(468, 46)
        Me.ComboMoneda.Name = "ComboMoneda"
        Me.ComboMoneda.Size = New System.Drawing.Size(119, 24)
        Me.ComboMoneda.TabIndex = 20
        '
        'CheckTieneChequera
        '
        Me.CheckTieneChequera.AutoSize = True
        Me.CheckTieneChequera.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.CheckTieneChequera.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckTieneChequera.Location = New System.Drawing.Point(2, 106)
        Me.CheckTieneChequera.Name = "CheckTieneChequera"
        Me.CheckTieneChequera.Size = New System.Drawing.Size(124, 20)
        Me.CheckTieneChequera.TabIndex = 22
        Me.CheckTieneChequera.Text = "Tiene Chequera"
        Me.CheckTieneChequera.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        Me.CheckTieneChequera.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(5, 80)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(41, 15)
        Me.Label5.TabIndex = 188
        Me.Label5.Text = "C.B.U."
        '
        'TextCbu
        '
        Me.TextCbu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextCbu.Location = New System.Drawing.Point(88, 76)
        Me.TextCbu.MaxLength = 22
        Me.TextCbu.Name = "TextCbu"
        Me.TextCbu.Size = New System.Drawing.Size(165, 22)
        Me.TextCbu.TabIndex = 21
        Me.TextCbu.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 19)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(76, 15)
        Me.Label8.TabIndex = 182
        Me.Label8.Text = "Numero Cta."
        '
        'ComboTipo
        '
        Me.ComboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboTipo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboTipo.FormattingEnabled = True
        Me.ComboTipo.Location = New System.Drawing.Point(533, 14)
        Me.ComboTipo.Name = "ComboTipo"
        Me.ComboTipo.Size = New System.Drawing.Size(50, 24)
        Me.ComboTipo.TabIndex = 8
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(394, 18)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 15)
        Me.Label7.TabIndex = 181
        Me.Label7.Text = "Tipo Cuenta"
        '
        'TextNumero
        '
        Me.TextNumero.BackColor = System.Drawing.Color.White
        Me.TextNumero.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNumero.Location = New System.Drawing.Point(87, 15)
        Me.TextNumero.MaxLength = 10
        Me.TextNumero.Name = "TextNumero"
        Me.TextNumero.Size = New System.Drawing.Size(112, 22)
        Me.TextNumero.TabIndex = 6
        Me.TextNumero.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PanelSucursal
        '
        Me.PanelSucursal.BackColor = System.Drawing.Color.LightBlue
        Me.PanelSucursal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelSucursal.Controls.Add(Me.Label4)
        Me.PanelSucursal.Controls.Add(Me.TextNombreSucursal)
        Me.PanelSucursal.Controls.Add(Me.TextSucursal)
        Me.PanelSucursal.Controls.Add(Me.Label3)
        Me.PanelSucursal.Location = New System.Drawing.Point(47, 72)
        Me.PanelSucursal.Name = "PanelSucursal"
        Me.PanelSucursal.Size = New System.Drawing.Size(647, 58)
        Me.PanelSucursal.TabIndex = 12
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(304, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Nombre Sucursal"
        '
        'TextNombreSucursal
        '
        Me.TextNombreSucursal.BackColor = System.Drawing.Color.White
        Me.TextNombreSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombreSucursal.Location = New System.Drawing.Point(438, 21)
        Me.TextNombreSucursal.MaxLength = 15
        Me.TextNombreSucursal.Name = "TextNombreSucursal"
        Me.TextNombreSucursal.Size = New System.Drawing.Size(133, 22)
        Me.TextNombreSucursal.TabIndex = 9
        '
        'TextSucursal
        '
        Me.TextSucursal.BackColor = System.Drawing.Color.White
        Me.TextSucursal.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextSucursal.Location = New System.Drawing.Point(168, 21)
        Me.TextSucursal.MaxLength = 3
        Me.TextSucursal.Name = "TextSucursal"
        Me.TextSucursal.Size = New System.Drawing.Size(74, 22)
        Me.TextSucursal.TabIndex = 8
        Me.TextSucursal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(41, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(99, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Código Sucursal"
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.BackColor = System.Drawing.Color.LightGreen
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Location = New System.Drawing.Point(580, 395)
        Me.ButtonAceptar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(114, 31)
        Me.ButtonAceptar.TabIndex = 42
        Me.ButtonAceptar.Text = "Aceptar Cambios"
        Me.ButtonAceptar.UseVisualStyleBackColor = False
        '
        'ButtonEliminar
        '
        Me.ButtonEliminar.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminar.Location = New System.Drawing.Point(48, 395)
        Me.ButtonEliminar.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminar.Name = "ButtonEliminar"
        Me.ButtonEliminar.Size = New System.Drawing.Size(128, 31)
        Me.ButtonEliminar.TabIndex = 43
        Me.ButtonEliminar.TabStop = False
        Me.ButtonEliminar.Text = "Borrar "
        Me.ButtonEliminar.UseVisualStyleBackColor = False
        '
        'UnaCuentaBancaria
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.WhiteSmoke
        Me.ClientSize = New System.Drawing.Size(779, 456)
        Me.Controls.Add(Me.ButtonEliminar)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.PanelSucursal)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ComboBancos)
        Me.Controls.Add(Me.Label2)
        Me.Name = "UnaCuentaBancaria"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Cuenta Bancaria"
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.PanelSucursal.ResumeLayout(False)
        Me.PanelSucursal.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBancos As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents CheckLiquidaDivisa As System.Windows.Forms.CheckBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents TextSaldoInicial As System.Windows.Forms.TextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextNumeracionFinal As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents TextNumeracionInicial As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents TextUltimoNumero As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextUltimaSerie As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ComboMoneda As System.Windows.Forms.ComboBox
    Friend WithEvents CheckTieneChequera As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextCbu As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboTipo As System.Windows.Forms.ComboBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextNumero As System.Windows.Forms.TextBox
    Friend WithEvents PanelSucursal As System.Windows.Forms.Panel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextNombreSucursal As System.Windows.Forms.TextBox
    Friend WithEvents TextSucursal As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents ButtonEliminar As System.Windows.Forms.Button
End Class
