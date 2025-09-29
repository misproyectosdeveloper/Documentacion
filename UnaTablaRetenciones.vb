Public Class UnaTablaRetenciones
    'CodigoRetencion:  1- Retencion
    '                  2- Percepcion.
    Dim Dt As DataTable
    Dim DtDocuRetenciones As DataTable
    Private WithEvents bs As New BindingSource
    Private Sub UnaTablaRetenciones_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        LlenaCombosGrid()

        LLenaGrid()

    End Sub
    Private Sub UnaTabla_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAlta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAlta.Click

        GModificacionOk = False

        UnaRetencion.PClaveRetencion = 0
        UnaRetencion.ShowDialog()
        If GModificacionOk Then
            LLenaGrid()
        End If

    End Sub
    Private Sub ButtonUltimo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonUltimo.Click

        bs.MoveLast()

    End Sub
    Private Sub ButtonAnterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnterior.Click

        bs.MovePrevious()

    End Sub
    Private Sub ButtonPosterior_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPosterior.Click

        bs.MoveNext()

    End Sub
    Private Sub ButtonPrimero_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPrimero.Click

        bs.MoveFirst()

    End Sub
    Private Sub LLenaGrid()

        Dt = New DataTable

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Not Tablas.Read("SELECT * FROM Tablas WHERE Tipo = " & 25 & ";", Conexion, Dt) Then End : Exit Sub

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Retención"
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Percepción"
        Dt.Rows.Add(Row)
        '
        CodigoRetencion.DataSource = Dt
        CodigoRetencion.DisplayMember = "Nombre"
        CodigoRetencion.ValueMember = "Clave"

        Dim Dt2 As New DataTable

        Dt2.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt2.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt2.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Imp.IVA"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Imp.Ing. Bruto"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 3
        Row("Nombre") = "Otros Imp.Nacionales"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 4
        Row("Nombre") = "Imp.Municipales"
        Dt2.Rows.Add(Row)
        ' 
        Row = Dt2.NewRow
        Row("Clave") = 5
        Row("Nombre") = "Imp.Internos"
        Dt2.Rows.Add(Row)
        '
        OrigenPercepcion.DataSource = Dt2
        OrigenPercepcion.DisplayMember = "Nombre"
        OrigenPercepcion.ValueMember = "Clave"


    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta2" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "000-000000-00")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Formula" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "UltimoNumero" Then
            If Not IsDBNull(e.Value) Then
                e.Value = FormatNumber(e.Value, 0)
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        GModificacionOk = False

        UnaRetencion.PClaveRetencion = Grid.CurrentRow.Cells("Clave").Value
        UnaRetencion.ShowDialog()
        If GModificacionOk Then
            LLenaGrid()
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
   
    
End Class