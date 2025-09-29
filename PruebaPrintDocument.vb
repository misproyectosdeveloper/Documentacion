Public Class PruebaPrintDocument
    'captuta imagen datagridview en visual b. 6.0
    'http://www.recursosvisualbasic.com.ar/htm/trucos-codigofuente-visual-basic/360-imprimir-datagrid.htm
    'ver http://www.elguille.info/net/dotnet/imprimir_visual_basic_net.aspx 
    'http://www.solovb.net/index.php/2009/02/14/imprimir-utilizando-printdocument-y-printdialog/
    'http://social.msdn.microsoft.com/Forums/es/vbes/thread/f943792e-de62-4b4a-bff6-928bee406b67
    ' http://www.chilecomparte.cl/topic/1003654-imprimir-datagridview-vbnet/
    ' http://es.debugmodeon.com/articulo/impresion-con-visual-basic-net
    Dim ElForm As Bitmap
    Private Sub PruebaPrintDocument_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.Rows.Add(1000199, "KK", 5)
        Grid.Rows.Add(10.9, "jjj", 7)
        Grid.Rows.Add(44444, "jjjh", 7)

    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        '     If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
        ' PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
        ' PrintDocument1.Print()
        ' End If

    End Sub
    Private Sub ImprimeGrid(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)

        Dim printFont As System.Drawing.Font = New Font("Arial", 10)
        Dim topMargin As Double = e.MarginBounds.Top
        Dim yPos As Double = 0
        Dim linesPerPage As Double = 0
        Dim count As Integer = 0
        Dim texto As String = ""
        Dim row As System.Windows.Forms.DataGridViewRow

        topMargin = Y
        ' Calculamos el número de líneas que caben en cada página
        linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics)

        ' Imprimimos las cabeceras
        Dim header As DataGridViewHeaderCell
        For Each column As DataGridViewColumn In Grid.Columns
            header = column.HeaderCell
            texto += vbTab + header.FormattedValue.ToString()
        Next

        yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
        e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, X, yPos)
        ' Dejamos una línea de separación
        count += 2

        ' Recorremos las filas del DataGridView hasta que llegemos
        ' a las líneas que nos caben en cada página o al final del grid.
        Dim i As Integer
        While count < linesPerPage AndAlso i < Grid.Rows.Count
            row = Grid.Rows(i)
            texto = ""
            For Each celda As System.Windows.Forms.DataGridViewCell In row.Cells
                'Comprobamos que la celda tenga algún valor, en caso de 
                'permitir añadir filas esto es muy importante
                If celda.Value IsNot Nothing Then
                    texto += vbTab + celda.Value.ToString()
                End If
            Next
            ' Calculamos la posición en la que se escribe la línea
            yPos = topMargin + (count * printFont.GetHeight(e.Graphics))

            ' Escribimos la línea con el objeto Graphics
            e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, X, yPos)
            ' Incrementamos los contadores
            count += 1
            i += 1
        End While
    End Sub
    Private Sub ImprimeGrid2(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)
        ' http://www.chilecomparte.cl/topic/1003654-imprimir-datagridview-vbnet/

        Dim printFont As System.Drawing.Font = New Font("Arial", 10)
        Dim topMargin As Double = e.MarginBounds.Top
        Dim yPos As Double = 0
        Dim linesPerPage As Double = 0
        Dim count As Integer = 0
        Dim texto As String = ""
        Dim row As System.Windows.Forms.DataGridViewRow

        topMargin = Y
        ' Calculamos el número de líneas que caben en cada página
        linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics)
        ' Imprimimos las cabeceras
        Dim header As DataGridViewHeaderCell
        Dim PosX As Integer = X
        For Each column As DataGridViewColumn In Grid.Columns
            header = column.HeaderCell
            texto = header.FormattedValue.ToString()
            PosX = PosX + header.Size.Width + 2
            yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
            e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, PosX, yPos)
        Next
        ' Dejamos una línea de separación
        count += 2

        ' Recorremos las filas del DataGridView hasta que llegemos
        ' a las líneas que nos caben en cada página o al final del grid.
        Dim i As Integer
        While count < linesPerPage AndAlso i < Grid.Rows.Count
            row = Grid.Rows(i)
            texto = ""
            PosX = X
            For Each celda As System.Windows.Forms.DataGridViewCell In row.Cells
                'Comprobamos que la celda tenga algún valor, en caso de 
                'permitir añadir filas esto es muy importante
                If celda.Value IsNot Nothing Then
                    texto = celda.Value.ToString()
                    PosX = PosX + celda.Size.Width + 2
                    yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
                    e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, PosX, yPos)
                End If
            Next
            count += 1
            i += 1
        End While

    End Sub
    Private Sub ImprimeGrid3(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)
        ' http://www.chilecomparte.cl/topic/1003654-imprimir-datagridview-vbnet/

        Dim printFont As System.Drawing.Font = New Font("Arial", 10)
        Dim topMargin As Double = e.MarginBounds.Top
        Dim yPos As Double = 0
        Dim linesPerPage As Double = 0
        Dim count As Integer = 0
        Dim texto As String = ""
        Dim row As System.Windows.Forms.DataGridViewRow

        topMargin = Y
        ' Calculamos el número de líneas que caben en cada página
        linesPerPage = e.MarginBounds.Height / printFont.GetHeight(e.Graphics)

        ' Imprimimos las cabeceras
        Dim header As DataGridViewHeaderCell
        For Each column As DataGridViewColumn In Grid.Columns
            header = column.HeaderCell
            Dim texto1 As String = header.FormattedValue.ToString
            While texto1.Length >= header.Size.Width
                texto1 = texto1 & " "
            End While
            texto += vbTab + texto1
        Next

        yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
        e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, X, yPos)
        ' Dejamos una línea de separación
        count += 2

        ' Recorremos las filas del DataGridView hasta que llegemos
        ' a las líneas que nos caben en cada página o al final del grid.
        Dim i As Integer
        While count < linesPerPage AndAlso i < Grid.Rows.Count
            row = Grid.Rows(i)
            texto = ""
            For Each celda As System.Windows.Forms.DataGridViewCell In row.Cells
                'Comprobamos que la celda tenga algún valor, en caso de 
                'permitir añadir filas esto es muy importante
                If celda.Value IsNot Nothing Then
                    Dim texto1 As String = celda.Value.ToString()
                    While texto1.Length >= celda.Size.Width
                        texto1 = texto1 & " "
                    End While
                    texto += vbTab + texto1
                End If
            Next
            ' Calculamos la posición en la que se escribe la línea
            yPos = topMargin + (count * printFont.GetHeight(e.Graphics))

            ' Escribimos la línea con el objeto Graphics
            e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, X, yPos)
            ' Incrementamos los contadores
            count += 1
            i += 1
        End While
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim Preview As Boolean = True

        Dim Pd As New PrintDialog
        If Pd.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim MyPrint As New PrintPantalla(Me)
            MyPrint.PrinterSettings = Pd.PrinterSettings
            If Preview Then
                Dim Dlg As New PrintPreviewDialog
                Dlg.Document = MyPrint     'PrintPreviewDialog associate with PrintDocument.
                Dlg.ShowDialog() 'open the print preview
            Else
                MyPrint.Print()
            End If
        End If
        Exit Sub

    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim pa As New Print2Pantalla(Me)
        pa.Print()
        Exit Sub

    End Sub
    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        e.Graphics.DrawImage(ElForm, 0, 0)

    End Sub

End Class

