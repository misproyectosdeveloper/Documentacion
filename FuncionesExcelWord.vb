Imports System.Data
Imports Microsoft.Office.Interop.Word
Imports System.Data.OleDb
Imports Microsoft.Office.Interop
Imports Microsoft.Reporting.WinForms
Module FuncionesExcelWord
    '   http://support.microsoft.com/kb/306682/es
    Dim Doc As Documents
    Public Sub Word()


        Dim app As Application = New Application
        Dim doc As Document

        Try
            doc = app.Documents.Open("C:\wordPrueba.docx")
        Catch ex As Exception
            doc = app.Documents.Add
        End Try
        app.Visible = True
        app.Activate()

        '     Quit the application.
        app.Quit()


    End Sub
    Public Sub MuestraExcel1()

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBooks As Excel.Workbooks = Nothing
        Dim xlsWB As Excel.Workbook = Nothing
        '    Me.Application.ActiveWorkbook.Protect(getPasswordFromUser)

        Try

            xlsApp = New Excel.Application
            xlsApp.Visible = True
            xlsWorkBooks = xlsApp.Workbooks
            xlsWB = xlsWorkBooks.Open("C:\ExcelPrueba.xlsx")
            xlsWorkBooks.Application.ActiveWorkbook.Protect() 'protege para no ser modificado.
        Catch ex As Exception
        Finally
            'xlsWB.Close()
            ' xlsWB = Nothing
            '   xlsApp.Quit()
            ' xlsApp = Nothing
        End Try

    End Sub
    Public Sub MuestraExcel(ByVal Ruta As String)

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBooks As Excel.Workbooks = Nothing
        Dim xlsWB As Excel.Workbook = Nothing

        Try
            xlsApp = New Excel.Application
            xlsApp.Visible = True
            xlsWorkBooks = xlsApp.Workbooks
            xlsWB = xlsWorkBooks.Open(Ruta)
            xlsWorkBooks.Application.ActiveWorkbook.Protect() 'protege para no ser modificado.
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            '  xlsWB.Close()
            xlsWB = Nothing
            '  xlsApp.Quit()
            xlsApp = Nothing
        End Try

    End Sub
    Function GridAExcel(ByVal ElGrid As DataGridView, ByVal Titulo1 As String, ByVal Titulo2 As String, ByVal Titulo3 As String) As Boolean

        'ver tambien usuando datatable http://www.dotnetcr.com/recurso/Exportar-datos-a-Excel
        'http://www.gemboxsoftware.com/support/articles/import-export-datagrid-xls-xlsx-ods-csv-html-net

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Try
            'Añadimos el Libro al programa, y la hoja al libro
            exLibro = exApp.Workbooks.Add
            exHoja = exLibro.Worksheets(1)

            ' ¿Cuantas columnas y cuantas filas?
            Dim NCol As Integer = ElGrid.ColumnCount
            Dim NRow As Integer = ElGrid.RowCount
            Dim LineaTitulo As Integer = 7  '4
            Dim LineaComiensoExcel As Integer = 9 ' 6
            Dim ColExxel As Integer = 0

            'Aqui muestra nombres de las columnas.
            For i As Integer = 1 To NCol
                If ElGrid.Columns(i - 1).Visible And ElGrid.Columns(i - 1).Name <> "Candado" And ElGrid.Columns(i - 1).Name <> "Lupa" Then
                    ColExxel = ColExxel + 1
                    'Muestra titulos.
                    exHoja.Cells.Item(LineaTitulo, ColExxel) = ElGrid.Columns(i - 1).HeaderText 'ElGrid.Columns(i - 1).Name.ToString
                End If
                exHoja.Rows.Item(LineaTitulo).Font.Bold = True
            Next

            'Si es un dato boolan muestra si/No.
            ColExxel = 0
            For Fila As Integer = 0 To NRow - 1
                ColExxel = 0
                For Col As Integer = 0 To NCol - 1
                    If ElGrid.Columns(Col).Visible And ElGrid.Columns(Col).Name <> "Candado" And ElGrid.Columns(Col).Name <> "Lupa" Then
                        ColExxel = ColExxel + 1
                        If ElGrid.Columns(Col).GetType Is GetType(DataGridViewCheckBoxColumn) Then
                            If IsDBNull(ElGrid.Rows(Fila).Cells(Col).Value) Then
                                exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = ""
                                Continue For
                            End If
                            If ElGrid.Rows(Fila).Cells(Col).Value = True Then
                                exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = "SI"
                            Else
                                exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = ""
                            End If
                            Continue For
                        End If
                        If IsNumeric(ElGrid.Rows(Fila).Cells(Col).FormattedValue) Then
                            exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = ElGrid.Rows(Fila).Cells(Col).Value
                            Dim Rango2 As Microsoft.Office.Interop.Excel.Range
                            Rango2 = exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel)
                            Rango2.NumberFormat = "#0.00"
                            Continue For
                        End If
                        If IsDate(ElGrid.Rows(Fila).Cells(Col).FormattedValue) Then
                            exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = ElGrid.Rows(Fila).Cells(Col).Value
                            Dim Rango2 As Microsoft.Office.Interop.Excel.Range
                            Rango2 = exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel)
                            Rango2.NumberFormat = "dd/MM/yyyy"
                            Continue For
                        End If
                        exHoja.Cells.Item(LineaComiensoExcel + Fila, ColExxel) = ElGrid.Rows(Fila).Cells(Col).FormattedValue
                    End If
                Next
            Next

            'Formatea ancho, Alineacion de las columnas.
            'http://stackoverflow.com/questions/9918484/merge-cells-with-same-values-in-microsoft-office-interop-excel
            ColExxel = 0
            Dim Rango As Microsoft.Office.Interop.Excel.Range
            For ColumnaGrid As Integer = 0 To NCol - 1
                If ElGrid.Columns(ColumnaGrid).Visible And ElGrid.Columns(ColumnaGrid).Name <> "Candado" And ElGrid.Columns(ColumnaGrid).Name <> "Lupa" Then
                    Dim AA As String = Chr(65 + ColExxel) & LineaComiensoExcel
                    Dim BB As String = Chr(65 + ColExxel) & LineaComiensoExcel + ElGrid.Rows.Count
                    Rango = Nothing
                    Rango = exHoja.Range(AA, BB)
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopCenter Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomLeft Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
                    If ElGrid.Columns(ColumnaGrid).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight Then Rango.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight
                    Rango.EntireColumn.AutoFit()
                    ColExxel = ColExxel + 1
                End If
            Next
            exHoja.Cells.Item(2, 1) = Titulo1 : exHoja.Rows.Item(2).Font.Bold = True
            exHoja.Cells.Item(3, 1) = Titulo2 : exHoja.Rows.Item(3).Font.Bold = True
            exHoja.Cells.Item(4, 1) = Titulo3 : exHoja.Rows.Item(4).Font.Bold = True

            'Aplicación visible
            exApp.Application.Visible = True
            '
            exHoja = Nothing
            exLibro = Nothing
            exApp = Nothing
            '
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al exportar a Excel")
            Return False
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = OldCulture
        End Try

        Return True

    End Function
    Function TablaAExcel(ByVal Dt As System.Data.DataTable, ByVal Titulo1 As String, ByVal Titulo2 As String, ByVal Titulo3 As String) As Boolean
        'ver tambien usuando datatable http://www.dotnetcr.com/recurso/Exportar-datos-a-Excel
        'http://www.gemboxsoftware.com/support/articles/import-export-datagrid-xls-xlsx-ods-csv-html-net

        If Dt.Rows.Count = 0 Then
            MsgBox("No Existe Datos.")
            Exit Function
        End If

        'Creamos las variables
        Dim exApp As New Microsoft.Office.Interop.Excel.Application
        Dim exLibro As Microsoft.Office.Interop.Excel.Workbook
        Dim exHoja As Microsoft.Office.Interop.Excel.Worksheet

        'para adaptarlo si el excel esta en otro idioma que el del sistema operativo.
        'http://www.add-in-express.com/creating-addins-blog/2009/02/13/old-format-invalid-type-library/
        Dim newCulture As System.Globalization.CultureInfo
        Dim OldCulture As System.Globalization.CultureInfo
        OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture
        newCulture = New System.Globalization.CultureInfo(exApp.LanguageSettings.LanguageID(Microsoft.Office.Core.MsoAppLanguageID.msoLanguageIDUI))
        System.Threading.Thread.CurrentThread.CurrentCulture = newCulture
        '----------------------------------------------------------------------------------------------

        Try
            'Añadimos el Libro al programa, y la hoja al libro
            exLibro = exApp.Workbooks.Add
            exHoja = exLibro.Worksheets(1)

            exHoja.Cells.Item(2, 1) = Titulo1 : exHoja.Rows.Item(2).Font.Bold = True
            exHoja.Cells.Item(3, 1) = Titulo2 : exHoja.Rows.Item(3).Font.Bold = True
            exHoja.Cells.Item(4, 1) = Titulo3 : exHoja.Rows.Item(4).Font.Bold = True

            ' ¿Cuantas columnas y cuantas filas?
            Dim NCol As Integer = Dt.Columns.Count
            Dim NRow As Integer = Dt.Rows.Count
            Dim LineaTitulo As Integer = 7  '4
            Dim LineaGrid As Integer = 9 ' 6
            Dim ColExxel As Integer = 0
            Dim Row As DataRow

            'Aqui recorremos todas las filas, y por cada fila todas las columnas y vamos escribiendo.
            For i As Integer = 1 To NCol
                ColExxel = ColExxel + 1
                'Muestra titulos.
                exHoja.Cells.Item(LineaTitulo, ColExxel) = Dt.Columns(i - 1).ColumnName
                ' exHoja.Cells.Item(1, i).HorizontalAlignment = 3
                ' exHoja.Cells.Item(1, i).HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft no funciona con formattedvalue
            Next

            ColExxel = 0
            For Fila As Integer = 0 To NRow - 1
                ColExxel = 0
                For Col As Integer = 0 To NCol - 1
                    ColExxel = ColExxel + 1
                    exHoja.Cells.Item(LineaGrid + Fila, ColExxel) = Dt.Rows(Fila).Item(Col)
                    If Dt.Columns(Col).ColumnName.Contains("Fecha") Then exHoja.Cells.Item(LineaGrid + Fila, ColExxel) = Replace(exHoja.Cells.Item(LineaGrid + Fila, ColExxel).Text, "/", "/")
                Next
            Next

            'Formateo celdas a numerico.

            'Titulo en negrita, Alineado al centro y que el tamaño de la columna se ajuste al texto
            exHoja.Rows.Item(LineaTitulo).Font.Bold = True
            exHoja.Rows.Item(LineaTitulo).HorizontalAlignment = 3
            ''            exHoja.Columns.AutoFit()

            'Aplicación visible
            exApp.Application.Visible = True
            '
            exHoja = Nothing
            exLibro = Nothing
            exApp = Nothing
            '
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical, "Error al exportar a Excel")
            Return False
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = OldCulture
        End Try

        Return True

    End Function
    Private Function TieneDecimales(ByVal Numero As Double) As Integer

        Dim MiArray() As String

        MiArray = Split(Numero.ToString, ",")
        If UBound(MiArray) = 0 Then Return 0
        If UBound(MiArray) = 1 Then Return MiArray(1).Length + 1

    End Function

End Module
