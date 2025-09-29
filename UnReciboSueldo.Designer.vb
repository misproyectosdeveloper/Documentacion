<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnReciboSueldo
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnReciboSueldo))
        Me.PanelContenedor = New System.Windows.Forms.Panel
        Me.ButtonImportacion = New System.Windows.Forms.Button
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.ButtonDesdeParametros = New System.Windows.Forms.Button
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.PictureAlmanaquePeriodo = New System.Windows.Forms.PictureBox
        Me.TextFechaPeriodo = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.TextBancoDeposito = New System.Windows.Forms.TextBox
        Me.TextNombrePeriodo = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.TextAnioDeposito = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.TextMesDeposito = New System.Windows.Forms.TextBox
        Me.MesDeposito = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        Me.PictureAlmanaqueContable = New System.Windows.Forms.PictureBox
        Me.TextFechaContable = New System.Windows.Forms.TextBox
        Me.Label24 = New System.Windows.Forms.Label
        Me.PictureCandado = New System.Windows.Forms.PictureBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.TextComentario = New System.Windows.Forms.TextBox
        Me.TextAnio = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.TextMes = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBruto = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.TextNombre = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.ButtonEliminarLineaConcepto = New System.Windows.Forms.Button
        Me.TextImporte = New System.Windows.Forms.TextBox
        Me.Grid1 = New System.Windows.Forms.DataGridView
        Me.Item = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CodigoExterno = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Concepto1 = New System.Windows.Forms.DataGridViewComboBoxColumn
        Me.Unidades = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Importe1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DateTime1 = New System.Windows.Forms.DateTimePicker
        Me.ComboEstado = New System.Windows.Forms.ComboBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.TextRecibo = New System.Windows.Forms.TextBox
        Me.ButtonNuevo = New System.Windows.Forms.Button
        Me.ButtonAsientoContable = New System.Windows.Forms.Button
        Me.ButtonImprimir = New System.Windows.Forms.Button
        Me.ButtonBaja = New System.Windows.Forms.Button
        Me.ButtonAceptar = New System.Windows.Forms.Button
        Me.PanelContenedor.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureAlmanaquePeriodo, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PanelContenedor
        '
        Me.PanelContenedor.BackColor = System.Drawing.Color.Thistle
        Me.PanelContenedor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PanelContenedor.Controls.Add(Me.ButtonImportacion)
        Me.PanelContenedor.Controls.Add(Me.Panel3)
        Me.PanelContenedor.Controls.Add(Me.ButtonEliminarLineaConcepto)
        Me.PanelContenedor.Controls.Add(Me.TextImporte)
        Me.PanelContenedor.Controls.Add(Me.Grid1)
        Me.PanelContenedor.Location = New System.Drawing.Point(18, 34)
        Me.PanelContenedor.Name = "PanelContenedor"
        Me.PanelContenedor.Size = New System.Drawing.Size(808, 573)
        Me.PanelContenedor.TabIndex = 2
        Me.PanelContenedor.TabStop = True
        '
        'ButtonImportacion
        '
        Me.ButtonImportacion.BackColor = System.Drawing.Color.Khaki
        Me.ButtonImportacion.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImportacion.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImportacion.Location = New System.Drawing.Point(657, 100)
        Me.ButtonImportacion.Name = "ButtonImportacion"
        Me.ButtonImportacion.Size = New System.Drawing.Size(110, 28)
        Me.ButtonImportacion.TabIndex = 0
        Me.ButtonImportacion.TabStop = False
        Me.ButtonImportacion.Text = "Importar"
        Me.ButtonImportacion.UseVisualStyleBackColor = False
        Me.ButtonImportacion.Visible = False
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.Color.LightGreen
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.ButtonDesdeParametros)
        Me.Panel3.Controls.Add(Me.Panel1)
        Me.Panel3.Controls.Add(Me.Panel2)
        Me.Panel3.Controls.Add(Me.PictureCandado)
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.TextComentario)
        Me.Panel3.Controls.Add(Me.TextAnio)
        Me.Panel3.Controls.Add(Me.Label4)
        Me.Panel3.Controls.Add(Me.TextMes)
        Me.Panel3.Controls.Add(Me.Label3)
        Me.Panel3.Controls.Add(Me.TextBruto)
        Me.Panel3.Controls.Add(Me.Label2)
        Me.Panel3.Controls.Add(Me.TextNombre)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(8, 2)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(795, 92)
        Me.Panel3.TabIndex = 15
        '
        'ButtonDesdeParametros
        '
        Me.ButtonDesdeParametros.BackColor = System.Drawing.Color.Gainsboro
        Me.ButtonDesdeParametros.Location = New System.Drawing.Point(707, 42)
        Me.ButtonDesdeParametros.Name = "ButtonDesdeParametros"
        Me.ButtonDesdeParametros.Size = New System.Drawing.Size(78, 22)
        Me.ButtonDesdeParametros.TabIndex = 0
        Me.ButtonDesdeParametros.TabStop = False
        Me.ButtonDesdeParametros.Text = "Parametro"
        Me.ButtonDesdeParametros.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Gainsboro
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.PictureAlmanaquePeriodo)
        Me.Panel1.Controls.Add(Me.TextFechaPeriodo)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.TextBancoDeposito)
        Me.Panel1.Controls.Add(Me.TextNombrePeriodo)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.TextAnioDeposito)
        Me.Panel1.Controls.Add(Me.Label5)
        Me.Panel1.Controls.Add(Me.TextMesDeposito)
        Me.Panel1.Controls.Add(Me.MesDeposito)
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Location = New System.Drawing.Point(5, 61)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(779, 33)
        Me.Panel1.TabIndex = 16
        '
        'PictureAlmanaquePeriodo
        '
        Me.PictureAlmanaquePeriodo.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaquePeriodo.Location = New System.Drawing.Point(305, 3)
        Me.PictureAlmanaquePeriodo.Name = "PictureAlmanaquePeriodo"
        Me.PictureAlmanaquePeriodo.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaquePeriodo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaquePeriodo.TabIndex = 1021
        Me.PictureAlmanaquePeriodo.TabStop = False
        '
        'TextFechaPeriodo
        '
        Me.TextFechaPeriodo.BackColor = System.Drawing.Color.White
        Me.TextFechaPeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaPeriodo.Location = New System.Drawing.Point(223, 3)
        Me.TextFechaPeriodo.MaxLength = 10
        Me.TextFechaPeriodo.Name = "TextFechaPeriodo"
        Me.TextFechaPeriodo.ReadOnly = True
        Me.TextFechaPeriodo.Size = New System.Drawing.Size(78, 21)
        Me.TextFechaPeriodo.TabIndex = 0
        Me.TextFechaPeriodo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(153, 7)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(69, 13)
        Me.Label11.TabIndex = 1019
        Me.Label11.Text = "Periodo Dep."
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBancoDeposito
        '
        Me.TextBancoDeposito.BackColor = System.Drawing.Color.White
        Me.TextBancoDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBancoDeposito.Location = New System.Drawing.Point(410, 4)
        Me.TextBancoDeposito.MaxLength = 18
        Me.TextBancoDeposito.Name = "TextBancoDeposito"
        Me.TextBancoDeposito.Size = New System.Drawing.Size(125, 20)
        Me.TextBancoDeposito.TabIndex = 22
        '
        'TextNombrePeriodo
        '
        Me.TextNombrePeriodo.BackColor = System.Drawing.Color.White
        Me.TextNombrePeriodo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombrePeriodo.Location = New System.Drawing.Point(627, 3)
        Me.TextNombrePeriodo.MaxLength = 18
        Me.TextNombrePeriodo.Name = "TextNombrePeriodo"
        Me.TextNombrePeriodo.Size = New System.Drawing.Size(145, 20)
        Me.TextNombrePeriodo.TabIndex = 30
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(541, 7)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(83, 13)
        Me.Label8.TabIndex = 291
        Me.Label8.Text = "Nombre Periódo"
        '
        'TextAnioDeposito
        '
        Me.TextAnioDeposito.BackColor = System.Drawing.Color.White
        Me.TextAnioDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAnioDeposito.Location = New System.Drawing.Point(106, 4)
        Me.TextAnioDeposito.MaxLength = 4
        Me.TextAnioDeposito.Name = "TextAnioDeposito"
        Me.TextAnioDeposito.Size = New System.Drawing.Size(40, 20)
        Me.TextAnioDeposito.TabIndex = 18
        Me.TextAnioDeposito.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(92, 9)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(13, 16)
        Me.Label5.TabIndex = 289
        Me.Label5.Text = "/"
        '
        'TextMesDeposito
        '
        Me.TextMesDeposito.BackColor = System.Drawing.Color.White
        Me.TextMesDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMesDeposito.Location = New System.Drawing.Point(67, 5)
        Me.TextMesDeposito.MaxLength = 2
        Me.TextMesDeposito.Name = "TextMesDeposito"
        Me.TextMesDeposito.Size = New System.Drawing.Size(24, 20)
        Me.TextMesDeposito.TabIndex = 17
        Me.TextMesDeposito.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'MesDeposito
        '
        Me.MesDeposito.AutoSize = True
        Me.MesDeposito.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MesDeposito.Location = New System.Drawing.Point(-1, 9)
        Me.MesDeposito.Name = "MesDeposito"
        Me.MesDeposito.Size = New System.Drawing.Size(63, 13)
        Me.MesDeposito.TabIndex = 287
        Me.MesDeposito.Text = "Ult.Dep.S.S"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(343, 7)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(64, 13)
        Me.Label9.TabIndex = 294
        Me.Label9.Text = "Banco Dep."
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.PictureAlmanaqueContable)
        Me.Panel2.Controls.Add(Me.TextFechaContable)
        Me.Panel2.Controls.Add(Me.Label24)
        Me.Panel2.Location = New System.Drawing.Point(448, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(210, 30)
        Me.Panel2.TabIndex = 0
        '
        'PictureAlmanaqueContable
        '
        Me.PictureAlmanaqueContable.Image = Global.ScomerV01.My.Resources.Resources.Almanaque
        Me.PictureAlmanaqueContable.Location = New System.Drawing.Point(176, 3)
        Me.PictureAlmanaqueContable.Name = "PictureAlmanaqueContable"
        Me.PictureAlmanaqueContable.Size = New System.Drawing.Size(26, 24)
        Me.PictureAlmanaqueContable.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureAlmanaqueContable.TabIndex = 1018
        Me.PictureAlmanaqueContable.TabStop = False
        '
        'TextFechaContable
        '
        Me.TextFechaContable.BackColor = System.Drawing.Color.White
        Me.TextFechaContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextFechaContable.Location = New System.Drawing.Point(88, 3)
        Me.TextFechaContable.MaxLength = 10
        Me.TextFechaContable.Name = "TextFechaContable"
        Me.TextFechaContable.Size = New System.Drawing.Size(85, 21)
        Me.TextFechaContable.TabIndex = 0
        Me.TextFechaContable.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(1, 9)
        Me.Label24.Name = "Label24"
        Me.Label24.Size = New System.Drawing.Size(82, 13)
        Me.Label24.TabIndex = 1016
        Me.Label24.Text = "Fecha Contable"
        Me.Label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureCandado
        '
        Me.PictureCandado.Location = New System.Drawing.Point(666, 3)
        Me.PictureCandado.Name = "PictureCandado"
        Me.PictureCandado.Size = New System.Drawing.Size(37, 37)
        Me.PictureCandado.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureCandado.TabIndex = 289
        Me.PictureCandado.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 40)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(60, 13)
        Me.Label7.TabIndex = 288
        Me.Label7.Text = "Comentario"
        '
        'TextComentario
        '
        Me.TextComentario.BackColor = System.Drawing.Color.White
        Me.TextComentario.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextComentario.Location = New System.Drawing.Point(70, 36)
        Me.TextComentario.MaxLength = 30
        Me.TextComentario.Name = "TextComentario"
        Me.TextComentario.Size = New System.Drawing.Size(224, 20)
        Me.TextComentario.TabIndex = 287
        Me.TextComentario.TabStop = False
        '
        'TextAnio
        '
        Me.TextAnio.BackColor = System.Drawing.Color.White
        Me.TextAnio.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextAnio.Location = New System.Drawing.Point(433, 35)
        Me.TextAnio.MaxLength = 4
        Me.TextAnio.Name = "TextAnio"
        Me.TextAnio.Size = New System.Drawing.Size(73, 20)
        Me.TextAnio.TabIndex = 14
        Me.TextAnio.TabStop = False
        Me.TextAnio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(402, 39)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 13)
        Me.Label4.TabIndex = 285
        Me.Label4.Text = "Año"
        '
        'TextMes
        '
        Me.TextMes.BackColor = System.Drawing.Color.White
        Me.TextMes.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextMes.Location = New System.Drawing.Point(339, 36)
        Me.TextMes.MaxLength = 2
        Me.TextMes.Name = "TextMes"
        Me.TextMes.Size = New System.Drawing.Size(50, 20)
        Me.TextMes.TabIndex = 12
        Me.TextMes.TabStop = False
        Me.TextMes.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(308, 40)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(27, 13)
        Me.Label3.TabIndex = 283
        Me.Label3.Text = "Mes"
        '
        'TextBruto
        '
        Me.TextBruto.BackColor = System.Drawing.Color.White
        Me.TextBruto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBruto.Location = New System.Drawing.Point(349, 2)
        Me.TextBruto.MaxLength = 8
        Me.TextBruto.Name = "TextBruto"
        Me.TextBruto.ReadOnly = True
        Me.TextBruto.Size = New System.Drawing.Size(91, 20)
        Me.TextBruto.TabIndex = 282
        Me.TextBruto.TabStop = False
        Me.TextBruto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(302, 7)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 13)
        Me.Label2.TabIndex = 281
        Me.Label2.Text = "Sueldo"
        '
        'TextNombre
        '
        Me.TextNombre.BackColor = System.Drawing.Color.White
        Me.TextNombre.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextNombre.Location = New System.Drawing.Point(70, 3)
        Me.TextNombre.MaxLength = 8
        Me.TextNombre.Name = "TextNombre"
        Me.TextNombre.ReadOnly = True
        Me.TextNombre.Size = New System.Drawing.Size(224, 20)
        Me.TextNombre.TabIndex = 279
        Me.TextNombre.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 7)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(45, 13)
        Me.Label1.TabIndex = 277
        Me.Label1.Text = "Legajo"
        '
        'ButtonEliminarLineaConcepto
        '
        Me.ButtonEliminarLineaConcepto.BackColor = System.Drawing.Color.PaleVioletRed
        Me.ButtonEliminarLineaConcepto.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonEliminarLineaConcepto.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonEliminarLineaConcepto.Location = New System.Drawing.Point(208, 541)
        Me.ButtonEliminarLineaConcepto.Margin = New System.Windows.Forms.Padding(0)
        Me.ButtonEliminarLineaConcepto.Name = "ButtonEliminarLineaConcepto"
        Me.ButtonEliminarLineaConcepto.Size = New System.Drawing.Size(98, 21)
        Me.ButtonEliminarLineaConcepto.TabIndex = 261
        Me.ButtonEliminarLineaConcepto.TabStop = False
        Me.ButtonEliminarLineaConcepto.Text = "Borrar Linea"
        Me.ButtonEliminarLineaConcepto.UseVisualStyleBackColor = False
        '
        'TextImporte
        '
        Me.TextImporte.BackColor = System.Drawing.Color.White
        Me.TextImporte.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextImporte.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextImporte.Location = New System.Drawing.Point(474, 542)
        Me.TextImporte.MaxLength = 20
        Me.TextImporte.Name = "TextImporte"
        Me.TextImporte.ReadOnly = True
        Me.TextImporte.Size = New System.Drawing.Size(119, 20)
        Me.TextImporte.TabIndex = 260
        Me.TextImporte.TabStop = False
        Me.TextImporte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Grid1
        '
        Me.Grid1.AllowUserToDeleteRows = False
        Me.Grid1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.Grid1.BackgroundColor = System.Drawing.Color.White
        Me.Grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Me.Grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Item, Me.CodigoExterno, Me.Concepto1, Me.Unidades, Me.Importe1})
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Grid1.DefaultCellStyle = DataGridViewCellStyle4
        Me.Grid1.Location = New System.Drawing.Point(206, 100)
        Me.Grid1.Name = "Grid1"
        Me.Grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.Grid1.Size = New System.Drawing.Size(388, 435)
        Me.Grid1.TabIndex = 40
        '
        'Item
        '
        Me.Item.DataPropertyName = "Item"
        Me.Item.HeaderText = "Item"
        Me.Item.Name = "Item"
        Me.Item.Visible = False
        Me.Item.Width = 52
        '
        'CodigoExterno
        '
        Me.CodigoExterno.DataPropertyName = "CodigoExterno"
        Me.CodigoExterno.HeaderText = "CodigoExterno"
        Me.CodigoExterno.Name = "CodigoExterno"
        Me.CodigoExterno.ReadOnly = True
        Me.CodigoExterno.Visible = False
        Me.CodigoExterno.Width = 101
        '
        'Concepto1
        '
        Me.Concepto1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Concepto1.DataPropertyName = "Concepto"
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White
        Me.Concepto1.DefaultCellStyle = DataGridViewCellStyle1
        Me.Concepto1.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox
        Me.Concepto1.HeaderText = "Concepto"
        Me.Concepto1.MinimumWidth = 140
        Me.Concepto1.Name = "Concepto1"
        Me.Concepto1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Concepto1.Width = 200
        '
        'Unidades
        '
        Me.Unidades.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Unidades.DataPropertyName = "Unidades"
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.Format = "N2"
        DataGridViewCellStyle2.NullValue = Nothing
        Me.Unidades.DefaultCellStyle = DataGridViewCellStyle2
        Me.Unidades.HeaderText = "Unidades"
        Me.Unidades.MaxInputLength = 5
        Me.Unidades.MinimumWidth = 80
        Me.Unidades.Name = "Unidades"
        Me.Unidades.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Unidades.Visible = False
        Me.Unidades.Width = 80
        '
        'Importe1
        '
        Me.Importe1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
        Me.Importe1.DataPropertyName = "Importe"
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopRight
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Blue
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White
        Me.Importe1.DefaultCellStyle = DataGridViewCellStyle3
        Me.Importe1.HeaderText = "Importe"
        Me.Importe1.MaxInputLength = 10
        Me.Importe1.MinimumWidth = 120
        Me.Importe1.Name = "Importe1"
        Me.Importe1.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Importe1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Importe1.Width = 120
        '
        'DateTime1
        '
        Me.DateTime1.CustomFormat = "dd/MM/yyyy"
        Me.DateTime1.Enabled = False
        Me.DateTime1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTime1.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.DateTime1.Location = New System.Drawing.Point(395, 9)
        Me.DateTime1.Name = "DateTime1"
        Me.DateTime1.Size = New System.Drawing.Size(108, 20)
        Me.DateTime1.TabIndex = 215
        Me.DateTime1.TabStop = False
        '
        'ComboEstado
        '
        Me.ComboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple
        Me.ComboEstado.Enabled = False
        Me.ComboEstado.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboEstado.FormattingEnabled = True
        Me.ComboEstado.Location = New System.Drawing.Point(711, 8)
        Me.ComboEstado.Name = "ComboEstado"
        Me.ComboEstado.Size = New System.Drawing.Size(108, 21)
        Me.ComboEstado.TabIndex = 220
        Me.ComboEstado.TabStop = False
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.Location = New System.Drawing.Point(660, 12)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(46, 13)
        Me.Label15.TabIndex = 219
        Me.Label15.Text = "Estado"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(346, 13)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 13)
        Me.Label10.TabIndex = 218
        Me.Label10.Text = "Fecha "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(20, 13)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 13)
        Me.Label6.TabIndex = 217
        Me.Label6.Text = "Recibo"
        '
        'TextRecibo
        '
        Me.TextRecibo.BackColor = System.Drawing.Color.White
        Me.TextRecibo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextRecibo.Location = New System.Drawing.Point(75, 10)
        Me.TextRecibo.MaxLength = 8
        Me.TextRecibo.Name = "TextRecibo"
        Me.TextRecibo.ReadOnly = True
        Me.TextRecibo.Size = New System.Drawing.Size(130, 20)
        Me.TextRecibo.TabIndex = 216
        Me.TextRecibo.TabStop = False
        Me.TextRecibo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonNuevo
        '
        Me.ButtonNuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonNuevo.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonNuevo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonNuevo.Location = New System.Drawing.Point(22, 621)
        Me.ButtonNuevo.Name = "ButtonNuevo"
        Me.ButtonNuevo.Size = New System.Drawing.Size(116, 35)
        Me.ButtonNuevo.TabIndex = 0
        Me.ButtonNuevo.Text = "Nuevo Recibo"
        Me.ButtonNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonNuevo.UseVisualStyleBackColor = True
        '
        'ButtonAsientoContable
        '
        Me.ButtonAsientoContable.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAsientoContable.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAsientoContable.Location = New System.Drawing.Point(508, 620)
        Me.ButtonAsientoContable.Name = "ButtonAsientoContable"
        Me.ButtonAsientoContable.Size = New System.Drawing.Size(140, 35)
        Me.ButtonAsientoContable.TabIndex = 0
        Me.ButtonAsientoContable.TabStop = False
        Me.ButtonAsientoContable.Text = "Ver Asiento Contable"
        Me.ButtonAsientoContable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAsientoContable.UseVisualStyleBackColor = True
        '
        'ButtonImprimir
        '
        Me.ButtonImprimir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonImprimir.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonImprimir.Image = CType(resources.GetObject("ButtonImprimir.Image"), System.Drawing.Image)
        Me.ButtonImprimir.Location = New System.Drawing.Point(184, 620)
        Me.ButtonImprimir.Name = "ButtonImprimir"
        Me.ButtonImprimir.Size = New System.Drawing.Size(116, 35)
        Me.ButtonImprimir.TabIndex = 47
        Me.ButtonImprimir.TabStop = False
        Me.ButtonImprimir.Text = "Imprime "
        Me.ButtonImprimir.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonImprimir.UseVisualStyleBackColor = True
        '
        'ButtonBaja
        '
        Me.ButtonBaja.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonBaja.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonBaja.Image = Global.ScomerV01.My.Resources.Resources.eliminar
        Me.ButtonBaja.Location = New System.Drawing.Point(346, 621)
        Me.ButtonBaja.Name = "ButtonBaja"
        Me.ButtonBaja.Size = New System.Drawing.Size(116, 35)
        Me.ButtonBaja.TabIndex = 0
        Me.ButtonBaja.TabStop = False
        Me.ButtonBaja.Text = "Baja Recibo"
        Me.ButtonBaja.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonBaja.UseVisualStyleBackColor = True
        '
        'ButtonAceptar
        '
        Me.ButtonAceptar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ButtonAceptar.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonAceptar.Image = Global.ScomerV01.My.Resources.Resources.Grabar
        Me.ButtonAceptar.Location = New System.Drawing.Point(706, 620)
        Me.ButtonAceptar.Name = "ButtonAceptar"
        Me.ButtonAceptar.Size = New System.Drawing.Size(116, 35)
        Me.ButtonAceptar.TabIndex = 44
        Me.ButtonAceptar.Text = "Graba Recibo"
        Me.ButtonAceptar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ButtonAceptar.UseVisualStyleBackColor = True
        '
        'UnReciboSueldo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightGreen
        Me.ClientSize = New System.Drawing.Size(849, 676)
        Me.Controls.Add(Me.ButtonImprimir)
        Me.Controls.Add(Me.ButtonAsientoContable)
        Me.Controls.Add(Me.ButtonNuevo)
        Me.Controls.Add(Me.ButtonBaja)
        Me.Controls.Add(Me.ButtonAceptar)
        Me.Controls.Add(Me.DateTime1)
        Me.Controls.Add(Me.ComboEstado)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TextRecibo)
        Me.Controls.Add(Me.PanelContenedor)
        Me.Name = "UnReciboSueldo"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Recibo de Sueldo"
        Me.PanelContenedor.ResumeLayout(False)
        Me.PanelContenedor.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureAlmanaquePeriodo, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        CType(Me.PictureAlmanaqueContable, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureCandado, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Grid1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PanelContenedor As System.Windows.Forms.Panel
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents TextNombre As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ButtonEliminarLineaConcepto As System.Windows.Forms.Button
    Friend WithEvents TextImporte As System.Windows.Forms.TextBox
    Friend WithEvents Grid1 As System.Windows.Forms.DataGridView
    Friend WithEvents ButtonBaja As System.Windows.Forms.Button
    Friend WithEvents ButtonAceptar As System.Windows.Forms.Button
    Friend WithEvents DateTime1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboEstado As System.Windows.Forms.ComboBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TextRecibo As System.Windows.Forms.TextBox
    Friend WithEvents TextAnio As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents TextMes As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBruto As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TextComentario As System.Windows.Forms.TextBox
    Friend WithEvents PictureCandado As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonNuevo As System.Windows.Forms.Button
    Friend WithEvents ButtonAsientoContable As System.Windows.Forms.Button
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents PictureAlmanaqueContable As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaContable As System.Windows.Forms.TextBox
    Friend WithEvents Label24 As System.Windows.Forms.Label
    Friend WithEvents ButtonImprimir As System.Windows.Forms.Button
    Friend WithEvents ButtonImportacion As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents TextAnioDeposito As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents TextMesDeposito As System.Windows.Forms.TextBox
    Friend WithEvents MesDeposito As System.Windows.Forms.Label
    Friend WithEvents TextNombrePeriodo As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Item As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CodigoExterno As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Concepto1 As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents Unidades As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Importe1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TextBancoDeposito As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents ButtonDesdeParametros As System.Windows.Forms.Button
    Friend WithEvents PictureAlmanaquePeriodo As System.Windows.Forms.PictureBox
    Friend WithEvents TextFechaPeriodo As System.Windows.Forms.TextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
End Class
