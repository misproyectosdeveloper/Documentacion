<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Lotes
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
        Me.Grid = New System.Windows.Forms.DataGridView
        Me.Ds = New System.Data.DataSet
        Me.Dt = New System.Data.DataTable
        Me.Lote = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Secuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Especie = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Variedad = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Marca = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Categoria = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Envase = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Articulo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.LoteSecuencia = New System.Windows.Forms.DataGridViewTextBoxColumn
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Ds, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Dt, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Grid
        '
        Me.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.Grid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Lote, Me.Secuencia, Me.Especie, Me.Variedad, Me.Marca, Me.Categoria, Me.Envase, Me.Articulo, Me.LoteSecuencia})
        Me.Grid.Location = New System.Drawing.Point(12, 35)
        Me.Grid.Name = "Grid"
        Me.Grid.Size = New System.Drawing.Size(900, 478)
        Me.Grid.TabIndex = 0
        '
        'Ds
        '
        Me.Ds.DataSetName = "DataSet"
        Me.Ds.Tables.AddRange(New System.Data.DataTable() {Me.Dt})
        '
        'Dt
        '
        Me.Dt.TableName = "Dt"
        '
        'Lote
        '
        Me.Lote.DataPropertyName = "Lote"
        Me.Lote.HeaderText = "Lote"
        Me.Lote.Name = "Lote"
        Me.Lote.Visible = False
        '
        'Secuencia
        '
        Me.Secuencia.DataPropertyName = "Secuencia"
        Me.Secuencia.HeaderText = "Secuencia"
        Me.Secuencia.Name = "Secuencia"
        Me.Secuencia.Visible = False
        '
        'Especie
        '
        Me.Especie.DataPropertyName = "NombreEspecie"
        Me.Especie.HeaderText = "Especie"
        Me.Especie.Name = "Especie"
        '
        'Variedad
        '
        Me.Variedad.DataPropertyName = "NombreVariedad"
        Me.Variedad.HeaderText = "Variedad"
        Me.Variedad.Name = "Variedad"
        '
        'Marca
        '
        Me.Marca.DataPropertyName = "NombreMarca"
        Me.Marca.HeaderText = "Marca"
        Me.Marca.Name = "Marca"
        '
        'Categoria
        '
        Me.Categoria.DataPropertyName = "NombreCategoria"
        Me.Categoria.HeaderText = "Categoria"
        Me.Categoria.Name = "Categoria"
        '
        'Envase
        '
        Me.Envase.DataPropertyName = "NombreEnvase"
        Me.Envase.HeaderText = "Envase"
        Me.Envase.Name = "Envase"
        '
        'Articulo
        '
        Me.Articulo.DataPropertyName = "NombreArticulo"
        Me.Articulo.HeaderText = "Articulo"
        Me.Articulo.Name = "Articulo"
        '
        'LoteSecuencia
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.LoteSecuencia.DefaultCellStyle = DataGridViewCellStyle1
        Me.LoteSecuencia.HeaderText = "LoteSecuencia"
        Me.LoteSecuencia.Name = "LoteSecuencia"
        '
        'Lotes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(955, 555)
        Me.Controls.Add(Me.Grid)
        Me.Name = "Lotes"
        Me.Text = "Lotes"
        CType(Me.Grid, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Ds, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Dt, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Ds As System.Data.DataSet
    Friend WithEvents Dt As System.Data.DataTable
    Friend WithEvents Grid As System.Windows.Forms.DataGridView
    Friend WithEvents Lote As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Secuencia As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Especie As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Variedad As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Marca As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Categoria As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Envase As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Articulo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents LoteSecuencia As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
