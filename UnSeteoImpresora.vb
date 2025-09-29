Imports System.Drawing.Printing
Imports System.IO
Public Class UnSeteoImpresora
    Public Class ItemImpresora
        Private IntClave As Integer
        Private StrNombrePC As String
        Private StrImpresora As String
        Private StrMargenIzquierdo As String
        Private StrMargenSuperior As String
        Private StrPredeterminada As String
        Public Sub New()
        End Sub
        Public Property Clave() As Integer
            Get
                Return IntClave
            End Get
            Set(ByVal value As Integer)
                IntClave = value
            End Set
        End Property
        Public Property NombrePC() As String
            Get
                Return StrNombrePC
            End Get
            Set(ByVal value As String)
                StrNombrePC = value
            End Set
        End Property
        Public Property Impresora() As String
            Get
                Return StrImpresora
            End Get
            Set(ByVal value As String)
                StrImpresora = value
            End Set
        End Property
        Public Property MargenIzquierdo() As String
            Get
                Return StrMargenIzquierdo
            End Get
            Set(ByVal value As String)
                StrMargenIzquierdo = value
            End Set
        End Property
        Public Property MargenSuperior() As String
            Get
                Return StrMargenSuperior
            End Get
            Set(ByVal value As String)
                StrMargenSuperior = value
            End Set
        End Property
        Public Property Predeterminada() As String
            Get
                Return StrPredeterminada
            End Get
            Set(ByVal value As String)
                StrPredeterminada = value
            End Set
        End Property
    End Class
    Dim Path As String
    Dim PathEntrada As String
    Dim ListaDeImpresoras As List(Of ItemImpresora)
    Private Sub UnSeteoImpresora_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Path = GeneraCarpetaEnDiscoSistema("XML Afip")
        PathEntrada = Path & "\SeteoImpresion.txt"

        'Arma de con valores de impresion del usuario.
        ListaDeImpresoras = New List(Of ItemImpresora)
        If File.Exists(PathEntrada) Then
            If Not ArmaListaDeImpresoras() Then
                Me.Close() : Exit Sub
            End If
        End If

        'Arma grid con Impresoras de S.O.
        Grid.Rows.Clear()
        For Each printerName As String In System.Drawing.Printing.PrinterSettings.InstalledPrinters()
            Grid.Rows.Add(printerName, "", "", "")
        Next

        For Each Row As DataGridViewRow In Grid.Rows
            Dim Izquierdo As String, Superior As String, Preferencia As String
            TraeDatosImpresora(Row.Cells(0).Value, Izquierdo, Superior, Preferencia)
            Row.Cells(1).Value = Izquierdo
            Row.Cells(2).Value = Superior
            Row.Cells(3).Value = False
            If Preferencia = "1" Then Row.Cells(3).Value = True
        Next

        'mustra datos de la impresora Predertimana. 
        Dim instance As New Printing.PrinterSettings
        Dim impresosaPredt As String = instance.PrinterName
        TextImprsoraPredeterminada.Text = impresosaPredt
        TextNombrePc.Text = My.Computer.Name

        Dim print_document As New PrintDocument
        print_document.PrinterSettings.PrinterName = impresosaPredt
        TextSizePapel.Text = print_document.DefaultPageSettings.PaperSize.PaperName
        print_document.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        Dim Contador As Integer = 0
        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells(3).Value = True Then Contador = Contador + 1
        Next
        If Contador > 1 Then
            MsgBox("ERROR: No debe haber mas de una Impresora Selecionada.", MsgBoxStyle.Information)
            Exit Sub
        End If

        Dim Linea As String
        File.Delete(PathEntrada)

        For Each Row As DataGridViewRow In Grid.Rows
            Dim Preferencia As String = ""
            If Row.Cells(3).Value = True Then Preferencia = "1"
            Linea = Row.Cells(0).Value & ";" & Row.Cells(1).Value & ";" & Row.Cells(2).Value & ";" & Preferencia
            Using writer As StreamWriter = New StreamWriter(PathEntrada, True)
                writer.WriteLine(Linea)
                writer.Close()
            End Using
        Next

        MsgBox("Datos Grabados con Exito.", MsgBoxStyle.Information)
        UnSeteoImpresora_Load(Nothing, Nothing)

    End Sub
    Private Function ArmaListaDeImpresoras() As Boolean

        ListaDeImpresoras.Clear()

        Try
            Using reader As StreamReader = New StreamReader(PathEntrada)
                Dim Linea As String = reader.ReadLine
                Do While (Not Linea Is Nothing)
                    Dim MiArray() As String
                    MiArray = Split(Linea, ";")
                    If MiArray.Length <> 0 Then
                        Dim Fila As New ItemImpresora
                        Fila.Impresora = MiArray(0)
                        Fila.MargenIzquierdo = MiArray(1)
                        Fila.MargenSuperior = MiArray(2)
                        Fila.Predeterminada = MiArray(3)
                        ListaDeImpresoras.Add(Fila)
                    End If
                    Linea = reader.ReadLine
                Loop
                reader.Dispose()
            End Using
        Catch ex As Exception
            MsgBox("No se pudo leer valores de Impresora.  " & ex.Message, MsgBoxStyle.Critical) : Return False
        End Try

        Return True

    End Function
    Private Sub TraeDatosImpresora(ByVal Impresora As String, ByRef Izquierdo As String, ByRef Superior As String, ByRef Predeterminada As String)

        Izquierdo = ""
        Superior = ""
        Predeterminada = ""

        For Each Linea As ItemImpresora In ListaDeImpresoras
            If Linea.Impresora = Impresora Then
                Izquierdo = Linea.MargenIzquierdo
                Superior = Linea.MargenSuperior
                Predeterminada = Linea.Predeterminada
                Exit For
            End If
        Next

    End Sub
    Private Function ArmaListaDeImpresoras(ByVal PathEntrada As String) As List(Of ItemImpresora)

        Dim ListaDeImpresoras As New List(Of ItemImpresora)

        Try
            Using reader As StreamReader = New StreamReader(PathEntrada)
                Dim Linea As String = reader.ReadLine
                Do While (Not Linea Is Nothing)
                    Dim MiArray() As String
                    MiArray = Split(Linea, ";")
                    If MiArray.Length <> 0 Then
                        Dim Fila As New ItemImpresora
                        Fila.Impresora = MiArray(0)
                        Fila.MargenIzquierdo = MiArray(1)
                        Fila.MargenSuperior = MiArray(2)
                        Fila.Predeterminada = MiArray(3)
                        ListaDeImpresoras.Add(Fila)
                    End If
                    Linea = reader.ReadLine
                Loop
                reader.Dispose()
            End Using
        Catch ex As Exception
            ListaDeImpresoras.Clear()
        End Try

        Return ListaDeImpresoras

    End Function
    Public Function GeneraCarpetaEnDiscoSistema(ByVal Carpeta As String) As String

        Dim PathCliente As String

        'Halla si esta carpeta Carpeta y si no esta la crea.
        Try
            'Halla letra del disco donde esta el sistema Operativo.
            Dim DiscoDelSistema As String = HallaDiscoDelSistema()
            ' Determine whether the directory exists.
            PathCliente = DiscoDelSistema & Carpeta
            If Not Directory.Exists(PathCliente) Then
                Directory.CreateDirectory(PathCliente)
            End If
            Return PathCliente
        Catch ex As Exception
            MsgBox("Error al Crear carpeta " & Carpeta & ". Operación se CANCELA. " & ex.Message)
        End Try

    End Function
    Public Function HallaDiscoDelSistema() As String

        Try
            Return Strings.Left(System.Environment.SystemDirectory, 3)
        Catch ex As Exception
            Return ""
        End Try

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              Setea Margenes de la impresion    --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Public Function SeteaImpresion(ByVal print_document As PrintDocument)

        Dim Impresora As String = ""
        Dim Seleccion As String = ""
        Dim MargenIzquierdo As String
        Dim MargenSuperior As String

        HalloDatosImpresion(Impresora, MargenIzquierdo, MargenSuperior, Seleccion)

        If Impresora <> "" Then
            If Seleccion = "1" And Impresora <> "" Then
                print_document.PrinterSettings.PrinterName = Impresora
                For i As Integer = 0 To print_document.PrinterSettings.PaperSizes.Count - 1
                    If print_document.PrinterSettings.PaperSizes.Item(i).Kind = PaperKind.A4 Then
                        print_document.DefaultPageSettings.PaperSize = print_document.DefaultPageSettings.PrinterSettings.PaperSizes.Item(i)
                        Exit For
                    End If
                Next
            End If
            '   Dim margins As Margins = New Margins(100, 100, 0, 0)
            '   print_document.DefaultPageSettings.Margins = margins
            If MargenIzquierdo <> "" Or MargenSuperior <> "" Then
                print_document.OriginAtMargins = True
                print_document.DefaultPageSettings.Margins.Left = print_document.DefaultPageSettings.PrintableArea.Left
                print_document.DefaultPageSettings.Margins.Top = print_document.DefaultPageSettings.PrintableArea.Top
                If MargenIzquierdo <> "" Then
                    print_document.DefaultPageSettings.Margins.Left = CInt(MargenIzquierdo)
                End If
                If MargenSuperior <> "" Then
                    print_document.DefaultPageSettings.Margins.Top = CInt(MargenSuperior)
                End If
            End If
        End If

    End Function
    Private Sub HalloDatosImpresion(ByRef Impresora As String, ByRef Izquierdo As String, ByRef Superior As String, ByRef Seleccionada As String)

        Impresora = ""
        Izquierdo = ""
        Superior = ""
        Seleccionada = ""

        Dim Path As String = HallaDiscoDelSistema()
        Dim PathEntrada = Path & "XML Afip\SeteoImpresion.txt"

        'mustra datos de la impresora Predertimana. 
        Dim instance As New Printing.PrinterSettings
        Impresora = instance.PrinterName

        If Not File.Exists(PathEntrada) Then
            Exit Sub
        End If

        Dim ListaDeImpresoras As List(Of ItemImpresora)
        ListaDeImpresoras = ArmaListaDeImpresoras(PathEntrada)
        For Each Fila As ItemImpresora In ListaDeImpresoras
            If Fila.Predeterminada = "1" Then
                Impresora = Fila.Impresora
                Seleccionada = "1"
                Exit For
            End If
        Next
        For Each Fila As ItemImpresora In ListaDeImpresoras
            If Fila.Impresora = Impresora Then
                Izquierdo = Fila.MargenIzquierdo
                Superior = Fila.MargenSuperior
                Exit For
            End If
        Next

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID DE COMPROBANTES.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "MargenIzquierdo" Or Grid.Columns(e.ColumnIndex).Name = "MargenSuperior" Then
            If IsNumeric(e.Value) Then
                '       If e.Value = 0 Then
                'e.Value = ""
                '   End If
            End If
        End If

    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MargenIzquierdo" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MargenSuperior" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MargenIzquierdo" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "MargenSuperior" Then
            If CType(sender, TextBox).Text <> "" Then
                If CType(sender, TextBox).Text = 0 Then
                    CType(sender, TextBox).Text = ""
                End If
            End If
        End If
    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub

  
End Class