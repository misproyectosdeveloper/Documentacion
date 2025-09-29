Public Class ListaRecibosFaltantes
    Public PTipo As Integer
    '
    Private WithEvents bs As New BindingSource
    Dim DtGrid As DataTable
    ' 
    Dim Sql As String
    Private Sub ListaRecibosFaltantes_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not GAdministrador Then
            MsgBox("Usuario Debe Ser Administrador.")
            Exit Sub
        End If

        Me.Top = 50

        If PTipo = 60 Or PTipo = 1000 Then
            TextLetra.Visible = False
        End If

        Grid.AutoGenerateColumns = False

        ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ListaRecibosFaltantes_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        Dim Desde As Double
        Dim Hasta As Double

        If Val(MaskedComprobante.Text) <> 0 Then
            Select Case PTipo
                Case 60, 1000
                    Desde = Val(MaskedComprobante.Text)
                    Hasta = CDbl(Strings.Left(MaskedComprobante.Text, 4) & "99999999")
                Case Else
                    Desde = CDbl(HallaNumeroLetra(TextLetra.Text) & Format(Val(MaskedComprobante.Text), "000000000000"))
                    Hasta = CDbl(HallaNumeroLetra(TextLetra.Text) & Mid(MaskedComprobante.Text, 1, 4) & "99999999")
            End Select
        End If

        Dim SqlComprobante As String
        Dim SqlComprobante2 As String

        Select Case PTipo
            Case 2
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "WHERE Factura BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT Factura AS Comprobante FROM FacturasCabeza " & SqlComprobante & ";"
                Label1.Text = ("Numeros de Facturas Faltantes")
            Case 5
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "AND Nota BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT Nota AS Comprobante FROM RecibosCabeza WHERE TipoNota = 5 " & SqlComprobante & ";"
                Label1.Text = ("Numeros de Notas Debitos Faltantes")
            Case 7
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "WHERE NotaCredito BETWEEN " & Desde & " AND " & Hasta
                    SqlComprobante2 = "AND Nota BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT NotaCredito AS Comprobante FROM NotasCreditoCabeza " & SqlComprobante & _
                      " UNION ALL " & _
                      "SELECT Nota AS Comprobante FROM RecibosCabeza WHERE TipoNota = 7 " & SqlComprobante2 & ";"
                Label1.Text = ("Numeros de Notas Creditos Faltantes")
            Case 60
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "AND ReciboOficial BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT ReciboOficial AS Comprobante FROM RecibosCabeza WHERE TipoNota = 60 AND ReciboOficial <> 0 " & SqlComprobante & ";"
                Label1.Text = ("Numeros de Cobranzas Manuales Faltantes")
            Case 1000
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "WHERE Remito BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT Remito AS Comprobante FROM RemitosCabeza " & SqlComprobante & ";"
                Label1.Text = ("Numeros de Remitos Faltantes")
            Case 2000
                If Val(MaskedComprobante.Text) <> 0 Then
                    SqlComprobante = "WHERE Liquidacion BETWEEN " & Desde & " AND " & Hasta
                End If
                Sql = "SELECT Liquidacion AS Comprobante FROM LiquidacionCabeza " & SqlComprobante & ";"
                Label1.Text = ("Numeros de Liquidaciones Faltantes")
        End Select

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid()

        Select Case PTipo
            Case 60, 1000
                LLenaGridCobranza()
            Case Else
                LLenaGrid()
        End Select

        Me.Cursor = System.Windows.Forms.Cursors.Default

        If DtGrid.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

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

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Comprobante"

        Dim ComprobanteAnt As Double
        Dim LetraAnt As Integer
        Dim PuntoDeVentaAnt As Integer

        For Each Row As DataRowView In View
            If Strings.Left(Row("Comprobante").ToString, 1) <> LetraAnt Then
                ComprobanteAnt = Row("Comprobante")
                LetraAnt = Strings.Left(Row("Comprobante").ToString, 1)
                PuntoDeVentaAnt = Mid(Row("Comprobante").ToString, 2, 4)
            Else
                If Mid(Row("Comprobante").ToString, 2, 4) <> PuntoDeVentaAnt Then
                    ComprobanteAnt = Row("Comprobante")
                    PuntoDeVentaAnt = Mid(Row("Comprobante").ToString, 2, 4)
                Else
                    If Row("Comprobante") <> ComprobanteAnt + 1 Then
                        Dim Row1 As DataRow = DtGrid.NewRow
                        Row1("Desde") = ComprobanteAnt + 1
                        Row1("Hasta") = Row("Comprobante") - 1
                        DtGrid.Rows.Add(Row1)
                    End If
                    ComprobanteAnt = Row("Comprobante")
                End If
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub LLenaGridCobranza()

        Dim Dt As New DataTable

        If Not Tablas.Read(Sql, Conexion, Dt) Then Me.Close() : Exit Sub

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Comprobante"

        Dim ComprobanteAnt As Double
        Dim PuntoDeVentaAnt As Integer

        For Each Row As DataRowView In View
            If Strings.Left(Row("Comprobante").ToString, 1) <> PuntoDeVentaAnt Then
                ComprobanteAnt = Row("Comprobante")
                PuntoDeVentaAnt = Strings.Left(Row("Comprobante").ToString, 1)
            Else
                If Row("Comprobante") <> ComprobanteAnt + 1 Then
                    If Row("Comprobante") <> ComprobanteAnt + 1 Then
                        Dim Row1 As DataRow = DtGrid.NewRow
                        Row1("Desde") = ComprobanteAnt + 1
                        Row1("Hasta") = Row("Comprobante") - 1
                        DtGrid.Rows.Add(Row1)
                    End If
                End If
                ComprobanteAnt = Row("Comprobante")
            End If
        Next

        Grid.DataSource = bs
        bs.DataSource = DtGrid

        Dt.Dispose()

    End Sub
    Private Sub CreaDtGrid()

        DtGrid = New DataTable

        Dim Desde As DataColumn = New DataColumn("Desde")
        Desde.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Desde)

        Dim Hasta As DataColumn = New DataColumn("Hasta")
        Hasta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Hasta)

    End Sub
    Private Function Valida() As Boolean

        If TextLetra.Text <> "" Or Val(MaskedComprobante.Text) <> 0 Then
            Select Case PTipo
                Case 60, 1000
                Case Else
                    If TextLetra.Text = "" Then
                        MsgBox("Falta Informar Letra del Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        TextLetra.Focus()
                        Return False
                    End If
                    If HallaNumeroLetra(TextLetra.Text) = 0 Then
                        MsgBox("Letra Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        TextLetra.Focus()
                        Return False
                    End If
            End Select
            If Val(MaskedComprobante.Text) = 0 Then
                MsgBox("Falta Informar Comprobante.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedComprobante.Focus()
                Return False
            End If
            If Not MaskedOK(MaskedComprobante) Then
                MsgBox("Comprobante Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedComprobante.Focus()
                Return False
            End If
            Dim Dta As New DataTable
            Select Case PTipo
                Case 2
                    Dim Numero As Double = CDbl(HallaNumeroLetra(TextLetra.Text) & Format(Val(MaskedComprobante.Text), "000000000000"))
                    If Not Tablas.Read("SELECT Factura FROM FacturasCabeza WHERE Factura = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count = 0 Then
                        MsgBox("Comprobante no Existe, Debe informar un Comprobante Existente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Dta.Dispose()
                        Return False
                    End If
                    Dta.Dispose()
                Case 5
                    Dim Numero As Double = CDbl(HallaNumeroLetra(TextLetra.Text) & Format(Val(MaskedComprobante.Text), "000000000000"))
                    If Not Tablas.Read("SELECT Nota FROM RecibosCabeza WHERE TipoNota = 5 AND Nota = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count = 0 Then
                        MsgBox("Comprobante no Existe, Debe informar un Comprobante Existente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Dta.Dispose()
                        Return False
                    End If
                    Dta.Dispose()
                Case 7
                    Dim Numero As Double = CDbl(HallaNumeroLetra(TextLetra.Text) & Format(Val(MaskedComprobante.Text), "000000000000"))
                    If Not Tablas.Read("SELECT NotaCredito FROM NotasCreditoCabeza WHERE NotaCredito = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count <> 0 Then
                        Dta.Dispose()
                        Return True
                    End If
                    If Not Tablas.Read("SELECT Nota FROM RecibosCabeza WHERE TipoNota = 7 AND Nota = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count <> 0 Then
                        Dta.Dispose()
                        Return True
                    End If
                    MsgBox("Comprobante no Existe, Debe informar un Comprobante Existente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Dta.Dispose()
                    Return False
                Case 60
                    Dim Numero As Double = Val(MaskedComprobante.Text)
                    If Not Tablas.Read("SELECT Nota FROM RecibosCabeza WHERE TipoNota = 60 AND Nota = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count = 0 Then
                        MsgBox("Comprobante no Existe, Debe informar un Comprobante Existente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Dta.Dispose()
                        Return False
                    End If
                    Dta.Dispose()
                Case 1000
                    Dim Numero As Double = Val(MaskedComprobante.Text)
                    If Not Tablas.Read("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Numero & ";", Conexion, Dta) Then Dta.Dispose() : Return False
                    If Dta.Rows.Count = 0 Then
                        MsgBox("Comprobante no Existe, Debe informar un Comprobante Existente.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Dta.Dispose()
                        Return False
                    End If
                    Dta.Dispose()
            End Select
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Desde" Or Grid.Columns(e.ColumnIndex).Name = "Hasta" Then
            e.Value = NumeroEditado(e.Value)
        End If

    End Sub
 
End Class