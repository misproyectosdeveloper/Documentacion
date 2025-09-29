Public Class ListaAsientos
    Public PTipoDocumento As Integer
    Public PDocumentoB As Double
    Public PDocumentoN As Double
    Public PTipoComprobante As Integer
    Public PConexion As String
    Public PConexionN As String
    Public PPaseDeProyectos As ItemPaseDeProyectos
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String
    Private Sub ListaAsientos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        '----------------- Paso la conexion desde otros proyectos ------------------------
        If Not IsNothing(PPaseDeProyectos) Then
            Conexion = PPaseDeProyectos.Conexion
            ConexionN = PPaseDeProyectos.ConexionN
            PermisoTotal = PPaseDeProyectos.PermisoTotal
        End If
        '----------------------------------------------------------------------------------

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        CreaDtGrid()

        LlenaCombosGrid()

        If PTipoDocumento <> 0 Then
            ButtonAceptar_Click(Nothing, Nothing)
        End If

    End Sub
    Private Sub ListaAsientos_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar.Focus() : ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim SqlFecha As String
        If PTipoDocumento = 0 Then
            SqlFecha = "C.IntFecha >= " & Format(DateTimeDesde.Value, "yyyyMMdd") & " AND C.IntFecha <= " & Format(DateTimeHasta.Value, "yyyyMMdd")
        End If

        Dim SqlAsiento As String = ""
        If TextAsiento.Text <> "" Then
            SqlAsiento = " AND C.Asiento = " & Val(TextAsiento.Text)
        End If

        SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE " & SqlFecha & SqlAsiento & ";"
        SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE " & SqlFecha & SqlAsiento & ";"

        If PTipoDocumento <> 0 Then
            Dim StrTipoComprobante As String = ""
            Select Case PTipoComprobante
                Case 5, 6, 7, 8, 13005, 13006, 13007, 13008
                    If PTipoComprobante <> 0 Then StrTipoComprobante = " AND TipoComprobante = " & PTipoComprobante
            End Select
            '
            If PTipoDocumento = 2 Then
                SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = " & PTipoDocumento & " or C.TipoDocumento = 6070) AND C.Documento = " & PDocumentoB & ";"
                SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = " & PTipoDocumento & " Or C.TipoDocumento = 6070) AND C.Documento = " & PDocumentoN & ";"
            Else
                If PTipoDocumento = 7006 Then
                    SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = " & PTipoDocumento & " or C.TipoDocumento = 6071) AND C.Documento = " & PDocumentoB & ";"
                    SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = " & PTipoDocumento & " Or C.TipoDocumento = 6071) AND C.Documento = " & PDocumentoN & ";"
                Else
                    If PTipoDocumento = 4 Then
                        SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 4 or C.TipoDocumento = 6072) AND C.Documento = " & PDocumentoB & ";"
                        SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 4 or C.TipoDocumento = 6072) AND C.Documento = " & PDocumentoN & ";"
                    Else
                        If PTipoDocumento = 41 Then
                            SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 41 or C.TipoDocumento = 6073) AND C.Documento = " & PDocumentoB & ";"
                            SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 41 or C.TipoDocumento = 6073) AND C.Documento = " & PDocumentoN & ";"
                        Else
                            If PTipoDocumento = 61000 Then
                                SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 61000 or C.TipoDocumento = 61001) AND C.Documento = " & PDocumentoB & ";"
                                SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE (C.TipoDocumento = 61000 or C.TipoDocumento = 61001) AND C.Documento = " & PDocumentoN & ";"
                            Else
                                SqlB = "SELECT 1 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = " & PTipoDocumento & " AND C.Documento = " & PDocumentoB & StrTipoComprobante & ";"
                                SqlN = "SELECT 2 as Operacion,C.*,D.* FROM AsientosCabeza AS C INNER JOIN AsientosDetalle AS D ON C.Asiento = D.Asiento WHERE C.TipoDocumento = " & PTipoDocumento & " AND C.Documento = " & PDocumentoN & StrTipoComprobante & ";"
                            End If
                        End If
                    End If
                End If
            End If
            Panel1.Enabled = False
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        LLenaGrid()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

    End Sub
    Private Sub TextAsiento_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAsiento.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

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
    Private Sub ButtonExportarExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExportarExcel.Click

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        GridAExcel(Grid, Date.Now, "Asientos Desde el " & DateTimeDesde.Text & "  Hasta el " & DateTimeHasta.Text, "")

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub LLenaGrid()

        DtGrid.Clear()

        If Not CheckAbierto.Checked And Not CheckCerrado.Checked Then
            CheckAbierto.Checked = True
            CheckCerrado.Checked = True
        End If

        Dim Dt As New DataTable

        If CheckAbierto.Checked Then
            If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        End If
        If CheckCerrado.Checked = True Then
            If Not Tablas.Read(SqlN, ConexionN, Dt) Then Me.Close() : Exit Sub
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "IntFecha,Asiento,Operacion,Cuenta"

        Dim AsientoAnt As Integer
        Dim OperacionAnt As Integer
        Dim TotalDebe As Double
        Dim TotalHaber As Double
        Dim Row1 As DataRow
        Dim Centro As Integer
        Dim Cuenta As Integer
        Dim SubCuenta As Integer

        For Each Row As DataRowView In View
            Row1 = DtGrid.NewRow
            If Row("Asiento") <> AsientoAnt Or Row("Operacion") <> OperacionAnt Then
                Row1("Operacion") = Row("Operacion")
                Row1("Asiento") = Row("Asiento")
                Row1("TipoDocumento") = Row("TipoDocumento")
                Row1("TipoComprobante") = Row("TipoComprobante")
                Row1("Documento") = Row("Documento")
                Row1("Fecha") = Row("IntFecha").ToString.Substring(6, 2) & "/" & Row("IntFecha").ToString.Substring(4, 2) & "/" & Row("IntFecha").ToString.Substring(0, 4)
                Row1("Estado") = 0
                If Row("Estado") <> 1 Then Row1("Estado") = Row("Estado")
                AsientoAnt = Row("Asiento")
                OperacionAnt = Row("Operacion")
            End If
            Row1("Cuenta") = Row("Cuenta")
            HallaPartesCuenta(Row("Cuenta"), Centro, Cuenta, SubCuenta)
            Row1("NombreCuenta") = Cuenta
            Row1("NombreSubCuenta") = Cuenta & Format(SubCuenta, "00")
            Row1("Debe") = Row("Debe")
            Row1("Haber") = Row("Haber")
            If Row("Estado") <> 3 Then
                TotalDebe = TotalDebe + Row("Debe")
                TotalHaber = TotalHaber + Row("Haber")
            End If
            Row1("Asiento2") = Row("Asiento")
            DtGrid.Rows.Add(Row1)
        Next
        '
        Row1 = DtGrid.NewRow
        Row1("Debe") = TotalDebe
        Row1("Haber") = TotalHaber
        DtGrid.Rows.Add(Row1)

        Dt.Dispose()

        Grid.DataSource = bs
        bs.DataSource = DtGrid

    End Sub
    Private Sub LlenaCombosGrid()

        TipoDocumento.DataSource = ArmaDocumentosAsientos()
        TipoDocumento.DisplayMember = "Nombre"
        TipoDocumento.ValueMember = "Codigo"

        NombreCuenta.DataSource = Tablas.Leer("SELECT Cuenta,Nombre FROM Cuentas;")
        NombreCuenta.DisplayMember = "Nombre"
        NombreCuenta.ValueMember = "Cuenta"

        NombreSubCuenta.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM SubCuentas;")
        NombreSubCuenta.DisplayMember = "Nombre"
        NombreSubCuenta.ValueMember = "Clave"

        Estado.DataSource = DtEstadoActivoYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Operacion)

        Dim Prestamo As New DataColumn("Asiento")
        Prestamo.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Prestamo)

        Dim TipoDocumento As New DataColumn("TipoDocumento")
        TipoDocumento.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoDocumento)

        Dim TipoComprobante As New DataColumn("TipoComprobante")
        TipoComprobante.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(TipoComprobante)

        Dim Documento As New DataColumn("Documento")
        Documento.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Documento)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Debe As New DataColumn("Debe")
        Debe.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Debe)

        Dim Haber As New DataColumn("Haber")
        Haber.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Haber)

        Dim NombreCuenta As New DataColumn("NombreCuenta")
        NombreCuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NombreCuenta)

        Dim NombreSubCuenta As New DataColumn("NombreSubCuenta")
        NombreSubCuenta.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NombreSubCuenta)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Estado)

        Dim Asiento2 As New DataColumn("Asiento2")
        Asiento2.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Asiento2)

    End Sub
    Private Function Valida() As Boolean

        If DiferenciaDias(DateTimeDesde.Value, DateTimeHasta.Value) < 0 Then
            MsgBox("Fecha Invalida.", MsgBoxStyle.Information + MsgBoxStyle.DefaultButton3)
            Exit Function
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Asiento" Then
            If Not IsDBNull(e.Value) Then
                e.Value = NumeroEditado(e.Value)
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Documento" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = NumeroEditado(e.Value)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Then
            If Not IsDBNull(e.Value) Then
                If e.Value <> 0 Then
                    e.Value = Format(e.Value, "000-000000-00")
                Else : e.Value = ""
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Debe" Or Grid.Columns(e.ColumnIndex).Name = "Haber" Then
            If e.Value <> 0 Then
                e.Value = FormatNumber(e.Value, GDecimales)
            Else : e.Value = Format(0, "#")
            End If
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.RowIndex < 0 Or e.ColumnIndex < 0 Then Exit Sub

        If IsDBNull(Grid.Rows(e.RowIndex).Cells("Asiento").Value) Then Exit Sub

        Dim Abierto As Boolean
        If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then
            Abierto = True
        Else
            Abierto = False
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Asiento" Then
            If Abierto Then
                UnAsiento.PAbierto = True
            Else : UnAsiento.PAbierto = False
            End If
            UnAsiento.PAsiento = Grid.Rows(e.RowIndex).Cells("Asiento").Value
            UnAsiento.ShowDialog()
            If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Documento" Then
            If Grid.Columns(e.ColumnIndex).Name = "Documento" Then
                MuestraComprobanteDelTipoAsiento(Grid.Rows(e.RowIndex).Cells("TipoDocumento").Value, Grid.Rows(e.RowIndex).Cells("TipoComprobante").Value, Grid.Rows(e.RowIndex).Cells("Documento").Value, Abierto)
            End If
        End If

    End Sub

End Class