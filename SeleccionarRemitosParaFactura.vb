Public Class SeleccionarRemitosParaFactura
    Public PCliente As Integer
    Public PDeposito As Integer
    Public PSucursal As Integer
    Public PListaRemitos As List(Of ItemRemito)
    '
    Dim DtGrid As DataTable
    Private Sub SeleccionarRemitoParaFactura_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        GeneraCombosGrid()

        Dim SqlCliente As String = "AND (Cliente = " & PCliente & " AND PorCuentaYOrden = 0 OR Cliente <> " & PCliente & " AND PorCuentaYOrden = " & PCliente & ")"

        Dim SqlB As String = "SELECT 1 as Operacion,Remito,Cliente,FechaEntrega AS Fecha,Estado,Sucursal,Pedido,SucursalPorCuentaYOrden,Ingreso,'' As NombreProveedor,PorCuentaYOrden,'' AS NombreCliente,'' AS NombreSucursal,'' AS NombreZona,Confirmado FROM RemitosCabeza WHERE Estado <> 3 " & SqlCliente & _
                         " AND Factura = 0 AND Deposito = " & PDeposito & " ORDER BY Remito;"
        Dim SqlN As String = "SELECT 2 as Operacion,Remito,Cliente,FechaEntrega As Fecha,Estado,Sucursal,Pedido,SucursalPorCuentaYOrden,Ingreso,'' As NombreProveedor,PorCuentaYOrden,'' AS NombreCliente,'' AS NombreSucursal,'' AS NombreZona,Confirmado FROM RemitosCabeza WHERE Estado <> 3 " & SqlCliente & _
                         " AND Factura = 0 AND Deposito = " & PDeposito & " ORDER BY Remito;"

        DtGrid = New DataTable

        If Not Tablas.Read(SqlB, Conexion, DtGrid) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            If Not Tablas.Read(SqlN, ConexionN, DtGrid) Then Me.Close() : Exit Sub
        End If

        For Each Row As DataRow In DtGrid.Rows
            If Row("Ingreso") <> 0 Then
                Row("NombreProveedor") = HallaNombreProveedor(Row("Ingreso"), Row("Operacion"))
            End If
            If Row("PorCuentaYOrden") <> 0 Then
                Row("NombreCliente") = NombreCliente(Row("Cliente"))
                Row("NombreSucursal") = NombreSucursalCliente(Row("PorCuentaYOrden"), Row("SucursalPorCuentaYOrden"))
                Row("NombreZona") = NombreZona(Row("PorCuentaYOrden"), Row("SucursalPorCuentaYOrden"))
                If PSucursal <> 0 And PSucursal <> Row("SucursalPorCuentaYOrden") Then Row.Delete()
            Else
                Row("NombreSucursal") = NombreSucursalCliente(PCliente, Row("Sucursal"))
                Row("NombreZona") = NombreZona(PCliente, Row("Sucursal"))
                If PSucursal <> 0 And PSucursal <> Row("Sucursal") Then Row.Delete()
            End If
            If Row.RowState <> DataRowState.Deleted Then
                If RemitoTieneNVLP(Row("Remito"), Row("Operacion")) Then Row.Delete()
            End If
        Next

        Grid.DataSource = DtGrid

        For Each item As ItemRemito In PListaRemitos
            For Each Row As DataGridViewRow In Grid.Rows
                Dim Operacion As Integer
                If item.Abierto Then
                    Operacion = 1
                Else : Operacion = 2
                End If
                If Row.Cells("Remito").Value = item.Remito And Row.Cells("Operacion").Value = Operacion Then
                    Row.Cells("Sel").Value = True
                End If
            Next
        Next

        If Not PermisoTotal Then Grid.Columns("Candado").Visible = False

    End Sub
    Private Function HallaNombreProveedor(ByVal Ingreso As Integer, ByVal Operacion As Integer) As String

        Dim StrConexion As String

        If Operacion = 1 Then
            StrConexion = Conexion
        Else : StrConexion = ConexionN
        End If

        Try
            Using Miconexion As New OleDb.OleDbConnection(StrConexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Proveedor FROM IngresoMercaderiasCabeza WHERE Lote = " & Ingreso & ";", Miconexion)
                    Return NombreProveedor(Cmd.ExecuteScalar)
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Function NombreZona(ByVal Cliente As Integer, ByVal Sucursal As Integer) As String

        If Sucursal = 0 Then Return ""

        Dim Zona As Integer = 0

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Zona FROM SucursalesClientes WHERE Cliente = " & Cliente & " AND Clave = " & Sucursal & ";", Miconexion)
                    Zona = Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

        If Zona = 0 Then Return ""

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Nombre FROM Tablas WHERE Tipo = 40 AND Clave = " & Zona & ";", Miconexion)
                    Return Cmd.ExecuteScalar
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error Base de Datos al leer Tabla: IngresoMercaderiasCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Sub GeneraCombosGrid()

        Estado.DataSource = DtAfectaPendienteAnulada()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Codigo"

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Validar() Then Exit Sub

        PListaRemitos.Clear()

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value Then
                Dim Item As New ItemRemito
                Item.Remito = Row.Cells("Remito").Value
                If Row.Cells("Operacion").Value = 1 Then
                    Item.Abierto = True
                Else : Item.Abierto = False
                End If
                Item.Fecha = Row.Cells("Fecha").Value
                Item.Estado = Row.Cells("Estado").Value
                PListaRemitos.Add(Item)
            End If
        Next

        DtGrid.Dispose()

        Me.Close()

    End Sub
    Private Function Validar() As Boolean

        Dim Con As Integer = 0
        Dim Operacion As Integer = 0
        Dim Ingreso As Integer = -1
        Dim Estado As Integer = 0
        Dim PedidoAnt As Integer = -1

        For Each Row As DataGridViewRow In Grid.Rows
            If Row.Cells("Sel").Value = True Then
                Con = Con + 1
                If Operacion = 0 Then
                    Operacion = Row.Cells("Operacion").Value
                Else
                    If Operacion <> Row.Cells("Operacion").Value Then
                        MsgBox("Todos los Remitos Deben tener Igual Candado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Focus()
                        Return False
                    End If
                End If
                If Estado = 0 Then
                    Estado = Row.Cells("estado").Value
                Else
                    If Estado <> Row.Cells("Estado").Value Then
                        MsgBox("Todos los Remitos Deben tener Igual Estado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Focus()
                        Return False
                    End If
                End If
                If Ingreso = -1 Then
                    Ingreso = Row.Cells("Ingreso").Value
                Else
                    If (Ingreso = 0 And Row.Cells("Ingreso").Value <> 0) Or (Ingreso <> 0 And Row.Cells("Ingreso").Value = 0) Then
                        MsgBox("Se debe Informar Remitos Todos con Ingresos o Todos Sin Ingresos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        Grid.Focus()
                        Return False
                    End If
                End If
            End If
        Next

        If Con = 0 Then
            MsgBox("Falta Informar Remito a Facturar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsNothing(e.Value) Then
                e.Value = Format(e.Value, "0000-00000000")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Ingreso" Then
            If e.Value = 0 Then e.Value = ""
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Pedido" Then
            If e.Value = 0 Then e.Value = ""
        End If

    End Sub
End Class