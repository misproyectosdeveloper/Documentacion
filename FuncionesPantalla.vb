Imports System.Drawing.Printing
Imports System.Drawing
Imports System
'
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Module FuncionesPantalla
    Public WithEvents printDoc As PrintDocument
    Dim Formu As Form
    Public Function ImprimePantalla(ByVal Formulario As Form) As Boolean

        Formu = Formulario


        Dim PrintPreviewDialog2 As New PrintPreviewDialog


        PrintPreviewDialog2.Document = printDoc 'PrintPreviewDialog associate with PrintDocument.
        PrintPreviewDialog2.ShowDialog() 'open the print preview




        ''''     Dim WithEvents printDoc As PrintDocument
        ' asignamos el método de evento para cada página a imprimir
        ''''''''  AddHandler printDoc.PrintPage, AddressOf print_PrintPage()

        printDoc.Print()

    End Function
    Private Sub printDoc_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim Document As PrintDocument = DirectCast(sender, PrintDocument)
        RecorrerControl(Formu, e, 0, 0)

    End Sub
    Public Sub RecorrerControl(ByRef oVControls As Object, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal OX As Integer, ByVal OY As Integer)

        For Each XControl As Control In oVControls.Controls
            Dim Texto As String
            Dim X As Integer
            Dim Y As Integer
            If TypeOf (XControl) Is TextBox Or TypeOf (XControl) Is ComboBox Then
                Texto = XControl.Text
                X = OX + XControl.Location.X
                Y = OY + XControl.Location.Y
                e.Graphics.DrawString(Texto, New Font(XControl.Font.Name, XControl.Font.Size * 2, FontStyle.Regular), Brushes.Black, X, Y)
            End If
            If TypeOf (XControl) Is Panel Then
                RecorrerControl(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
            If TypeOf (XControl) Is DataGridView Then
                ImprimeGrid2(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
        Next

    End Sub
    Private Sub ImprimeGrid(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)
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
End Module
