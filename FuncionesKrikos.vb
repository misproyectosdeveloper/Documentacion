Imports System.IO
Imports System.Xml
Module FuncionesKrikos
    Public PServidorKrikos As String   '= "Odetti"
    Dim Archivo As String = ""
    Dim Path As String = "K:\ORDERS\Documentos\DownLoad\Orders\" 'TENGO QUE Generar una Unidad de red con Letra K a la carpeta krikosDMS. En las maquinas que importen Ordenes.  
    Dim PathBackUp As String = "K:\ORDERS\Documentos\DownLoad\Orders\Backup\"
    Dim di As DirectoryInfo
    Dim Linea As String = ""
    Public Function LeeArchivoTXT(ByVal Cliente As Integer, ByRef Sucursal As Integer, ByVal FechaEntrega As Date, ByRef PedidoCliente As String) As Boolean

        Dim SucursalW As Integer
        SucursalW = Sucursal

        Dim ArrayFileName() As String

        Try
            di = New DirectoryInfo(Path)

            For Each fi As Object In di.GetFiles()
                Archivo = fi.Name
                ArrayFileName = Split(Archivo, "_")

                If Not ValidaEmpresaYCliente(ArrayFileName(3), ArrayFileName(2), Cliente) Then Continue For

                If Not System.IO.File.Exists(Path & Archivo) Then Continue For

                Using reader As StreamReader = New StreamReader(Path & Archivo)
                    Linea = reader.ReadLine
                    Dim Fecha As Date = FormateaFecha(Trim(Linea.Substring(267, 10)))
                    If Fecha <> Format(FechaEntrega, "dd/MM/yyyy") Then Continue For

                    Dim OrdenCompraEnTxt As String = Trim(Linea.Substring(476, 20))

                    If Sucursal <> 0 Then
                        If Not BuscaSucursalCliente(Cliente, Trim(Linea.Substring(30, 13)), Sucursal) Then Continue For
                    Else
                        Sucursal = BuscaSucursal(Trim(Linea.Substring(30, 13)), Cliente, OrdenCompraEnTxt)
                        If Sucursal = 0 Then MsgBox("Debe Informar Sucursal Para Continuar") : Return False
                    End If

                    PedidoCliente = OrdenCompraEnTxt      ' Trim(Linea.Substring(47, 10)) : se usaba anteriormente

                    reader.Dispose() : Exit For
                End Using
            Next
            If PedidoCliente = "" Then MsgBox("NO HAY ORDENES DE COMPRA!", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, "ATENCION!") : Return False
            Return True
        Catch ex As Exception
            MsgBox(ex.Message + vbCrLf + "Debe definir Unidad de red a la carpeta de krikos con Letra K:", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Return False
        Finally
            Linea = ""
        End Try

    End Function
    Public Function LeeDetalleTXT(ByVal Cliente As Integer, ByVal Lista As Integer, ByVal OrdenCompra As String, ByVal Sucursal As Decimal, ByRef DetallePedido As DataTable) As Boolean

        Dim RowDetalle As DataRow
        Dim Articulo As Integer = 0

        Try
            di = New DirectoryInfo(Path)
            If Not BuscaArchivo(di, OrdenCompra) Then MsgBox("NO SE ENCONTRO EL ARCHIVO!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Return False

            Using reader As StreamReader = New StreamReader(Path & Archivo)
                Linea = reader.ReadLine
                Do While (Not Linea Is Nothing)
                    If Linea.Contains("HEAD") Then Linea = reader.ReadLine : Continue Do
                    If Linea.Contains("SPLIT") Then
                        MsgBox("EL ARCHIVO CONTIENE UNA ETIQUETA SPLIT. NO SE PUEDE PROCESAR. AVISAR A INFORMATICA!") : Return False
                    End If

                    If Not BuscaIDArticulo(Cliente, Lista, Articulo, Linea.Substring(10, 14), Linea.Substring(44, 35), Linea.Substring(261, 11)) Then Linea = reader.ReadLine : Continue Do 'Return False

                    RowDetalle = DetallePedido.NewRow
                    RowDetalle("Pedido") = 0
                    RowDetalle("Sucursal") = Sucursal
                    RowDetalle("Articulo") = Articulo
                    Dim ar As String = NombreArticulo(Articulo)
                    Dim MiArray() As String
                    MiArray = Split(Trim(Linea.Substring(261, 11)), ".")
                    If UBound(MiArray) = 0 Then
                        RowDetalle("Cantidad") = MiArray(0)
                    Else
                        RowDetalle("Cantidad") = MiArray(0) & "," & MiArray(1)
                    End If
                    RowDetalle("Entregada") = 0
                    RowDetalle("Precio") = 0
                    RowDetalle("TipoPrecio") = 0
                    DetallePedido.Rows.Add(RowDetalle)

                    Linea = reader.ReadLine
                Loop
                reader.Dispose()
            End Using

            Return True
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!")
            DetallePedido.Rows.Clear()
            Return False
        Finally
            Linea = ""
        End Try

    End Function
    Private Function ValidaEmpresaYCliente(ByVal GLNEmpresa As String, ByVal GLNCliente As String, ByVal Cliente As Integer) As Boolean

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT * FROM ClientesCodigos WHERE Sucursal = 0", Conexion, DT) Then Return False

        If DT.Rows.Count < 2 Then Return False

        If DT.Select("CLIENTE = 0 AND GLN = " & GLNEmpresa & "").Length = 0 Then Return False
        If DT.Select("CLIENTE = " & Cliente & " AND GLN = " & GLNCliente & "").Length = 0 Then Return False

        Return True

    End Function
    Private Function BuscaSucursalCliente(ByVal Cliente As Integer, ByVal GLNSucursal As String, ByVal Sucursal As Integer) As Boolean

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT * FROM ClientesCodigos WHERE Sucursal <> 0 AND Cliente = " & Cliente, Conexion, DT) Then Return False

        If DT.Rows.Count = 0 Or DT.Select("GLN = " & CDec(GLNSucursal)).Length = 0 Then Return False

        If Sucursal <> DT.Select("GLN = " & CDec(GLNSucursal))(0).Item("Sucursal") Then Return False

        Return True

    End Function
    Private Function BuscaIDArticulo(ByVal Cliente As Integer, ByVal Lista As Integer, ByRef Articulo As Integer, ByVal EANArticulo As String, ByVal ArticuloCliente As String, ByVal Unidades As String) As Boolean

        Dim DT As New DataTable
        If Not Tablas.Read("SELECT Articulo FROM CodigosCliente WHERE Cliente = " & Cliente & " AND EAN = " & CDec(EANArticulo), Conexion, DT) Then Return False

        If DT.Rows.Count = 0 Then MsgBox("EL EAN: " & EANArticulo & " Del Art. " & ArticuloCliente & "de Uni: " & Trim(Unidades) & ". NO ESTÁ DEFINIDO EN CODIGO ARTICULO CLIENTE!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Return False
        If Lista <> 0 Then
            If Not EstaEnListaDePrecios(Lista, DT.Rows(0).Item(0)) Then
                MsgBox("Articulo del Cliente " & NombreArticulo(DT.Rows(0).Item(0)) & "de Uni: " & Trim(Unidades) & " No esta en Lista de Precios: " & Lista, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Return False
            End If
        End If

        Articulo = DT.Rows(0).Item(0)

        Return True

    End Function
    Private Function EstaEnListaDePrecios(ByVal Lista As Integer, ByVal Articulo As Integer) As Boolean

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Articulo FROM ListaDePreciosDetalle WHERE Lista = " & Lista & " AND Articulo = " & Articulo & ";", Conexion, Dt) Then
            MsgBox("Error 11: Al leer Tabla ListaDePrecios. Avisar a Sistemas.", MsgBoxStyle.Information) : Return False
        End If
        If Dt.Rows.Count = 0 Then
            Dt.Dispose() : Return False
        Else
            Dt.Dispose() : Return True
        End If

    End Function
    Public Sub ArchivoCambiaDirectorio(ByVal OrdenCompra As String, ByVal EsBackUp As Boolean)

        Dim DirectorioOrigen As String = Path
        Dim DirectorioDestino As String = PathBackUp

        If EsBackUp Then
            DirectorioOrigen = PathBackUp
            DirectorioDestino = Path
        End If

        Try
            di = New DirectoryInfo(DirectorioOrigen)
            If Not BuscaArchivo(di, Trim(OrdenCompra)) Then MsgBox("NO SE ENCONTRO EL ARCHIVO!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub

            File.Copy(DirectorioOrigen & Archivo, DirectorioDestino & Archivo)
        Catch ex As Exception
            MsgBox("Error al pasar OrdenCompra a la Carpeta BACKUP (Carpeta no existe)")
        End Try

    End Sub
    Private Function FormateaFecha(ByVal Fecha As String) As Date

        Dim Dia, Mes, Anio As String

        Anio = Fecha.Substring(0, 4)
        Mes = Fecha.Substring(4, 2)
        Dia = Fecha.Substring(6, 2)

        Return CDate(Dia & "/" & Mes & "/" & Anio)

    End Function
    Private Function BuscaArchivo(ByVal di As DirectoryInfo, ByVal OrdenCompra As String) As Boolean

        Dim NombreArchivo As String = ""
        Dim ArrayFileName() As String

        For Each fi As Object In di.GetFiles()
            ArrayFileName = Split(fi.Name, "_")
            If ArrayFileName(1) = OrdenCompra Then NombreArchivo = fi.Name : Exit For
        Next

        If NombreArchivo = "" Then Return False

        Archivo = NombreArchivo : Return True

    End Function
    Public Function BorraArchivoW(ByVal Archivo As String) As Boolean

        Dim ArrayFileName() As String

        For Each fi As Object In di.GetFiles()
            Dim ArchivoEnTXT = fi.Name
            ArrayFileName = Split(ArchivoEnTXT, "_")
            If ArrayFileName(1) = Archivo Then
                File.Delete(Path & ArchivoEnTXT) : Return True
            End If
        Next

    End Function
    Private Function BuscaSucursal(ByVal GLNSucursal As Decimal, ByVal Cliente As Integer, ByVal Orden As String) As Integer

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Sucursal FROM ClientesCodigos WHERE GLN = " & GLNSucursal & " AND Cliente = " & Cliente & ";", Conexion, Dt) Then
            MsgBox("Error al leer Tabla: ClientesCodigos") : Return 0
        End If
        If Dt.Rows.Count = 0 Then
            Dt.Dispose() : MsgBox("Sucursal con GNL en Krikos: " & GLNSucursal & " No existe en Cliente. Orden: " & Orden) : Return 0
        Else
            Dt.Dispose() : Return Dt.Rows(0).Item("Sucursal")
        End If

    End Function
    Public Function ExisteUnidadRedKrikos() As Boolean

        di = New DirectoryInfo(Path)
        If Not di.Exists Then Return False
        Return True

    End Function
    Public Sub BorraOrdenesVencidas()

        Dim Directorio As String = Path

        Try
            For I As Integer = 0 To 1
                di = New DirectoryInfo(Directorio)

                For Each fi As Object In di.GetFiles()
                    If DiferenciaDias(fi.creationtime, Today.Date) > 30 Then File.Delete(Directorio & fi.Name)
                Next

                Directorio = PathBackUp
            Next
        Catch ex As Exception
            MsgBox("Error al leer Backp :" & ex.Message & " Avisar al Administrador del sistema.", MsgBoxStyle.Information)
        End Try

    End Sub

End Module
