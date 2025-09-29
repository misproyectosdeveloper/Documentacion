Public Class OpcionImportacionExcel
    Public PDtImportacion As DataTable
    Public PRemito As Decimal
    Public PFechaRemito As Date
    Public PAbierto As Boolean
    Public PProveedor As Integer
    '
    Dim NombreArchivo As String = ""
    Private Sub OpcionImportacionExcel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PideExcel() Then Me.Close() : Exit Sub

    End Sub
    Private Function PideExcel() As Boolean

        Dim openFileDialog1 As New OpenFileDialog()

        openFileDialog1.InitialDirectory = "C:\"
        openFileDialog1.Filter = "Excel files (*.xls)|*.xls|All files (*.*)|*.*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True
        If openFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If IsNothing(openFileDialog1.FileName) Then Exit Function
        End If
        If openFileDialog1.FileName = "" Then Exit Function

        NombreArchivo = openFileDialog1.FileName

        Return True

    End Function
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If MaskedRemito.Text = "" Or MaskedRemito.Text = "000000000000" Then
            MsgBox("Falta Ingresar Remito a Procesar.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If Not IsNumeric(Strings.Left(MaskedRemito.Text, 4)) Or Not IsNumeric(Strings.Right(MaskedRemito.Text, 8)) Then
            MsgBox("Remito a Procesar No Numerico.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If CDec(Strings.Left(MaskedRemito.Text, 4)) = 0 Then
            MsgBox("Falta Ingresar Punto de Venta del Remito a Procesar.", MsgBoxStyle.Information)
            Exit Sub
        End If
        If CDec(Strings.Right(MaskedRemito.Text, 8)) = 0 Then
            MsgBox("Falta Ingresar Numero del Remito a Procesar.", MsgBoxStyle.Information)
            Exit Sub
        End If

        If Not ValidaExcel() Then Exit Sub

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Open(NombreArchivo)
        exHoja = exLibro.Worksheets(1)

        Dim I As Integer
        Dim Dt2 As New DataTable
        Dim Cantidad As Decimal = 0

        If Not Tablas.Read("SELECT Articulo,Cantidad FROM RemitosDetalle WHERE Remito = 0;", Conexion, Dt2) Then Exit Sub

        Try
            For I = 1 To exLibro.ActiveSheet.UsedRange.Rows.Count
                If exHoja.Cells(I, 9).value = "D" Then
                    If exHoja.Cells(I, 2).value = MaskedRemito.Text Then
                        Dim Row As DataRow = Dt2.NewRow
                        Row("Articulo") = exHoja.Cells(I, 3).value
                        Row("Cantidad") = exHoja.Cells(I, 5).value
                        Cantidad = Cantidad + Row("Cantidad")
                        Dt2.Rows.Add(Row)
                        If exHoja.Cells(I, 1).value = "X" Then
                            PAbierto = False
                        Else
                            PAbierto = True
                        End If
                        PFechaRemito = Strings.Right(exHoja.Cells(I, 10).value, 4) & "/" & Strings.Mid(exHoja.Cells(I, 10).value, 4, 2) & "/" & Strings.Left(exHoja.Cells(I, 10).value, 2)
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox("Error en la Linea:   " & I)
            Exit Sub
        Finally
        End Try

        If cantidad = 0 Then
            MsgBox("Nio hay Cantidades procesadas. Operación se CANCELA.", MsgBoxStyle.Critical)
            Exit Sub
        End If

        exLibro.Close(Nothing, Nothing, Nothing)
        exApp.Workbooks.Close()
        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing
        GC.Collect()

        PDtImportacion = Dt2.Copy
        PRemito = Val(MaskedRemito.Text)

        Dt2.Dispose()

        Me.Close()

    End Sub
    Private Function ValidaExcel() As Boolean

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        exLibro = exApp.Workbooks.Open(NombreArchivo)
        exHoja = exLibro.Worksheets(1)

        Dim I As Integer

        If exHoja.Cells(1, 20).value <> "IMPEXC" Then
            MsgBox("Excel no corresponde a una Exportación de Remito.", MsgBoxStyle.Critical) : Exit Function
        End If

        Dim Cantidad As Decimal = 0
        Dim HayErrores As Boolean
        Dim Mensaje As String = ""

        Try
            For I = 1 To exLibro.ActiveSheet.UsedRange.Rows.Count
                Mensaje = ""
                If exHoja.Cells(I, 9).value = "D" Then
                    If Not IsNumeric(exHoja.Cells(I, 2).value) Then
                        Mensaje = "Numero Remito No Numerico"
                        exHoja.Cells.Item(I, 8) = Mensaje
                        If Mensaje <> "" Then HayErrores = True
                        Exit For
                    End If
                    If exHoja.Cells(I, 2).value = MaskedRemito.Text Then
                        If Not IsNumeric(exHoja.Cells(I, 3).value) Then
                            Mensaje = Mensaje & " Articulo No Numerico"
                        Else
                            If NombreArticuloYEstado(exHoja.Cells(I, 3).value, Conexion) = "" Then
                                Mensaje = Mensaje & " Articulo No Existe en la Empresa Destino o esta Deshabilitado"
                            End If
                        End If
                        If Not IsNumeric(exHoja.Cells(I, 5).value) Then
                            Mensaje = Mensaje & " Cantidad No Numerica"
                        Else
                            Cantidad = Cantidad + exHoja.Cells(I, 5).value
                        End If
                        If Not IsDate(Strings.Right(exHoja.Cells(I, 10).value, 4) & "/" & Strings.Mid(exHoja.Cells(I, 10).value, 4, 2) & "/" & Strings.Left(exHoja.Cells(I, 10).value, 2)) Then
                            Mensaje = Mensaje & " Fecha Remito Incorrecta"
                        End If
                        exHoja.Cells.Item(I, 8) = Mensaje
                        If Mensaje <> "" Then HayErrores = True
                    End If
                End If
            Next
        Catch ex As Exception
            MsgBox("Error en la Linea:   " & I)
            HayErrores = True
        Finally
        End Try

        If Cantidad = 0 Then
            MsgBox("No hay cantidades de articulos informado. Operación se CANCELA.", MsgBoxStyle.Critical)
            HayErrores = True
        End If
        If HayErrores Then
            MsgBox("Se encontraron Errores en el EXCEL a Importar. Operación se CANCELA.", MsgBoxStyle.Critical)
            exApp.Application.Visible = True
        End If

        exHoja = Nothing
        exLibro = Nothing
        exApp = Nothing

        Return True

    End Function
   

End Class