'http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
'captura el grid graficamente.
'http://vb.net-informations.com/datagridview/vb.net_datagridview_printing.htm
Imports System.Drawing.Printing
Imports System.Drawing
Imports System
'
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.IO
Public Class PrintTexto
 
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private fntPrintFont As Font
    Private strText As String
    Public Sub New(ByVal Text As String)

        ' Sets the file stream
        MyBase.New()
        strText = Text

    End Sub
    Public Property Text() As String

        Get
            Return strText
        End Get
        Set(ByVal Value As String)
            strText = Value
        End Set

    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev As Printing.PrintEventArgs)

        ' Run base code
        MyBase.OnBeginPrint(ev)
        ' Sets the default font
        If fntPrintFont Is Nothing Then
            fntPrintFont = New Font("Times New Roman", 12)
        End If

    End Sub
    Public Property Font() As Font

        ' Allows the user to override the default font
        Get
            Return fntPrintFont
        End Get
        Set(ByVal Value As Font)
            fntPrintFont = Value
        End Set

    End Property
    Protected Overrides Sub OnPrintPage(ByVal ev As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)
        ' Variables
        Static intCurrentChar As Integer
        Dim intPrintAreaHeight, intPrintAreaWidth, intMarginLeft, intMarginTop As Integer
        ' Set printing area boundaries and margin coordinates
        With MyBase.DefaultPageSettings
            intPrintAreaHeight = .PaperSize.Height - _
                               .Margins.Top - .Margins.Bottom
            intPrintAreaWidth = .PaperSize.Width - _
                              .Margins.Left - .Margins.Right
            intMarginLeft = .Margins.Left 'X
            intMarginTop = .Margins.Top   'Y
        End With
        ' If Landscape set, swap printing height/width
        If MyBase.DefaultPageSettings.Landscape Then
            Dim intTemp As Integer
            intTemp = intPrintAreaHeight
            intPrintAreaHeight = intPrintAreaWidth
            intPrintAreaWidth = intTemp
        End If
        ' Calculate total number of lines
        Dim intLineCount As Int32 = CInt(intPrintAreaHeight / Font.Height)
        ' Initialise rectangle printing area
        Dim rectPrintingArea As New RectangleF(intMarginLeft, intMarginTop, intPrintAreaWidth, intPrintAreaHeight)
        ' Initialise StringFormat class, for text layout
        Dim objSF As New StringFormat(StringFormatFlags.LineLimit)
        ' Figure out how many lines will fit into rectangle
        Dim intLinesFilled, intCharsFitted As Int32
        ev.Graphics.MeasureString(Mid(strText, _
                    UpgradeZeros(intCurrentChar)), Font, _
                    New SizeF(intPrintAreaWidth, _
                    intPrintAreaHeight), objSF, _
                    intCharsFitted, intLinesFilled)
        ' Print the text to the page
        ev.Graphics.DrawString(Mid(strText, UpgradeZeros(intCurrentChar)), Font, _
                                           Brushes.Black, rectPrintingArea, objSF)
        ' Increase current char count
        intCurrentChar += intCharsFitted
        ' Check whether we need to print more
        If intCurrentChar < strText.Length Then
            ev.HasMorePages = True
        Else
            ev.HasMorePages = False
            intCurrentChar = 0
        End If

    End Sub
    Public Function UpgradeZeros(ByVal Input As Integer) As Integer

        ' Upgrades all zeros to ones
        ' - used as opposed to defunct IIF or messy If statements
        If Input = 0 Then
            Return 1
        Else
            Return Input
        End If

    End Function
End Class
Public Class PrintPantalla
    'http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private fntPrintFont As Font
    Private Formulario As Form
    Public Sub New(ByVal Formu As Form)

        ' Sets the file stream
        MyBase.New()
        Formulario = Formu

    End Sub
    Public Property Formu() As Form

        Get
            Return Formulario
        End Get
        Set(ByVal Value As Form)
            Formulario = Value
        End Set

    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev As Printing.PrintEventArgs)

        ' Run base code
        MyBase.OnBeginPrint(ev)
        ' Sets the default font
        If fntPrintFont Is Nothing Then
            fntPrintFont = New Font("Times New Roman", 12)
        End If

    End Sub
    Public Property Font() As Font

        ' Allows the user to override the default font
        Get
            Return fntPrintFont
        End Get
        Set(ByVal Value As Font)
            fntPrintFont = Value
        End Set

    End Property
    Protected Overrides Sub OnPrintPage(ByVal ev As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)
        ' Variables
        RecorrerControl(Formulario, ev, 0, 0)

    End Sub
    Public Function UpgradeZeros(ByVal Input As Integer) As Integer

        ' Upgrades all zeros to ones
        ' - used as opposed to defunct IIF or messy If statements
        If Input = 0 Then
            Return 1
        Else
            Return Input
        End If

    End Function
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
            yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
            e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, PosX, yPos)
            PosX = PosX + header.Size.Width + 2
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
                Dim Xmodi As Integer = PosX
                'Comprobamos que la celda tenga algún valor, en caso de 
                'permitir añadir filas esto es muy importante
                If celda.Value IsNot Nothing Then
                    texto = celda.Value.ToString()
                    Dim Longi As Integer = e.Graphics.MeasureString(texto, printFont).Width
                    If Grid.Columns(celda.ColumnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight _
                        Or Grid.Columns(celda.ColumnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomRight _
                        Or Grid.Columns(celda.ColumnIndex).DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight Then
                        Xmodi = PosX + celda.Size.Width - Longi
                    End If
                    yPos = topMargin + (count * printFont.GetHeight(e.Graphics))
                    e.Graphics.DrawString(texto, printFont, System.Drawing.Brushes.Black, Xmodi, yPos)
                End If
                PosX = PosX + celda.Size.Width + 2
            Next
            count += 1
            i += 1
        End While

    End Sub
End Class
Public Class PrintFormulario
    'http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private Formulario As Form
    Dim ElForm As Bitmap
    Public Sub New(ByVal Formu As Form)

        ' Sets the file stream
        MyBase.New()
        Formulario = Formu

    End Sub
    Public Property Formu() As Form

        Get
            Return Formulario
        End Get
        Set(ByVal Value As Form)
            Formulario = Value
        End Set

    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev As Printing.PrintEventArgs)

        ' Run base code
        MyBase.OnBeginPrint(ev)
        DefaultPageSettings.Landscape = True
        '  DefaultPageSettings.Color = False

        Formulario.BackColor = Color.White
        SacaColor(Formulario)

        Dim Grphs As Graphics = Formulario.CreateGraphics()
        ElForm = New Bitmap(Formulario.Size.Width, Formulario.Size.Height, Grphs)
        Dim memoryGraphics As Graphics = Graphics.FromImage(ElForm)
        memoryGraphics.CopyFromScreen(Formulario.Location.X, Formulario.Location.Y, 0, 0, Formulario.Size)

    End Sub
    Protected Overrides Sub OnPrintPage(ByVal ev As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)


        ev.Graphics.DrawImage(ElForm, 10, 100)

    End Sub
    Public Sub SacaColor(ByRef oVControls As Object)

        For Each XControl As Control In oVControls.Controls
            If TypeOf (XControl) Is Panel Then
                SacaColor(XControl)
                XControl.BackColor = Color.White
            End If
            If TypeOf (XControl) Is Button Then
                XControl.Visible = False
            End If
        Next

    End Sub

End Class
Public Class Print2Pantalla
    'http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private Formulario As Form
    Dim ElForm As Bitmap
    Dim printFont As System.Drawing.Font = New Font("Arial", 10)

    Public Sub New(ByVal Formu As Form)

        ' Sets the file stream
        MyBase.New()
        Formulario = Formu

    End Sub
    Public Property Formu() As Form

        Get
            Return Formulario
        End Get
        Set(ByVal Value As Form)
            Formulario = Value
        End Set

    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev As Printing.PrintEventArgs)

        ' Run base code
        MyBase.OnBeginPrint(ev)
        DefaultPageSettings.Landscape = True

    End Sub
    Protected Overrides Sub OnPrintPage(ByVal ev As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)
        ' Variables
        RecorrerControl(Formulario, ev, 0, 0)

    End Sub
    Public Sub RecorrerControl(ByRef oVControls As Object, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal OX As Integer, ByVal OY As Integer)

        For Each XControl As Control In oVControls.Controls
            Dim Texto As String
            Dim X As Integer
            Dim Y As Integer
            Dim Xmodi As Integer

            If TypeOf (XControl) Is TextBox Or TypeOf (XControl) Is ComboBox Or TypeOf (XControl) Is Label Then
                printFont = New Font(XControl.Font.Name, XControl.Font.Size)
                Texto = XControl.Text
                X = OX + XControl.Location.X
                Y = OY + XControl.Location.Y
                Xmodi = X
                If TypeOf (XControl) Is TextBox Then
                    Dim TextBoxW As New TextBox
                    TextBoxW = XControl
                    If TextBoxW.TextAlign = HorizontalAlignment.Right Then
                        Dim Longi As Integer = e.Graphics.MeasureString(Texto, printFont).Width
                        Xmodi = X + XControl.Width - Longi
                    End If
                End If
                e.Graphics.DrawString(Texto, New Font(XControl.Font.Name, XControl.Font.Size, FontStyle.Regular), Brushes.Black, Xmodi, Y)
            End If
            If TypeOf (XControl) Is DateTimePicker Then
                Texto = XControl.Text
                X = OX + XControl.Location.X
                Y = OY + XControl.Location.Y
                e.Graphics.DrawString(Texto, New Font(XControl.Font.Name, XControl.Font.Size, FontStyle.Regular), Brushes.Black, X, Y)
            End If
            If TypeOf (XControl) Is Panel Then
                RecorrerControl(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
            If TypeOf (XControl) Is DataGridView Then
                ImprimeGrid(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
        Next

    End Sub
    Private Sub ImprimeGrid(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)

        ElForm = New Bitmap(Grid.Width, Grid.Height)
        Grid.DrawToBitmap(ElForm, New Rectangle(0, 0, Grid.Width, Grid.Height))
        e.Graphics.DrawImage(ElForm, X, Y)

    End Sub
End Class
Public Class PrintGPantalla
    'http://www.developer.com/net/asp/article.php/3102381/A-NET-Text-Printing-Class-That-Works.htm
    ' Inherits all the functionality of a PrintDocument
    Inherits Printing.PrintDocument
    ' Private variables to hold default font and text
    Private Formulario As Form
    Dim ElForm As Bitmap
    Public Sub New(ByVal Formu As Form)

        ' Sets the file stream
        MyBase.New()
        Formulario = Formu

    End Sub
    Public Property Formu() As Form

        Get
            Return Formulario
        End Get
        Set(ByVal Value As Form)
            Formulario = Value
        End Set

    End Property
    Protected Overrides Sub OnBeginPrint(ByVal ev As Printing.PrintEventArgs)

        ' Run base code
        MyBase.OnBeginPrint(ev)
        DefaultPageSettings.Landscape = True

    End Sub
    Protected Overrides Sub OnPrintPage(ByVal ev As Printing.PrintPageEventArgs)
        ' Provides the print logic for our document

        ' Run base code
        MyBase.OnPrintPage(ev)
        ' Variables
        RecorrerControl(Formulario, ev, 20, 50)

    End Sub
    Public Sub RecorrerControl(ByVal oVControls As Object, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal OX As Integer, ByVal OY As Integer)

        For Each XControl As Control In oVControls.Controls
            Dim X As Integer
            Dim Y As Integer

            If TypeOf (XControl) Is TextBox Or TypeOf (XControl) Is ComboBox Or TypeOf (XControl) Is Label Or _
                    TypeOf (XControl) Is DateTimePicker Or TypeOf (XControl) Is MaskedTextBox Or TypeOf (XControl) Is CheckBox Or TypeOf (XControl) Is RadioButton Then
                Dim ColorAnt As Color = XControl.BackColor
                XControl.BackColor = Color.White
                X = OX + XControl.Location.X
                Y = OY + XControl.Location.Y
                ElForm = New Bitmap(XControl.Width, XControl.Height)
                XControl.DrawToBitmap(ElForm, New Rectangle(0, 0, XControl.Width, XControl.Height))
                e.Graphics.DrawImage(ElForm, X, Y)
                XControl.BackColor = ColorAnt
            End If
            If TypeOf (XControl) Is Panel Then
                RecorrerControl(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
            If TypeOf (XControl) Is DataGridView Then
                ImprimeGrid(XControl, e, OX + XControl.Location.X, OY + XControl.Location.Y)
            End If
        Next

    End Sub
    Private Sub ImprimeGrid(ByVal Grid As DataGridView, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal X As Integer, ByVal Y As Integer)

        ElForm = New Bitmap(Grid.Width, Grid.Height)
        Grid.DrawToBitmap(ElForm, New Rectangle(0, 0, Grid.Width, Grid.Height))
        e.Graphics.DrawImage(ElForm, X, Y)

    End Sub
End Class




