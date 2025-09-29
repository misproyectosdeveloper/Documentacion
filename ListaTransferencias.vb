Public Class ListaTransferencias
    Private WithEvents bs As New BindingSource
    '
    Dim Dt As DataTable
    '
    Dim SqlB As String
    Dim SqlN As String

    Private Sub ListaTransferencias_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.Dispose()
    End Sub
    Private Sub ListaTransferencias_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Top = 50

        Grid.AutoGenerateColumns = False
        Grid.Columns("Candado").DefaultCellStyle.NullValue = Nothing

        LlenaComboTablas(ComboDeposito, 19)
        ComboDeposito.SelectedValue = 0
        With ComboDeposito
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        GeneraCombosGrid()

        If PermisoTotal Then
            Grid.Columns("Candado").Visible = True
        Else : Grid.Columns("Candado").Visible = False
        End If


    End Sub
    Private Sub ListaTransferencias_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Grid.Focus()

    End Sub
    Protected Overrides Sub OnKeyUp(ByVal e As KeyEventArgs) 'Establecer a True el valor de la propiedad KeyPreview del formulario.

        If e.KeyData = 13 Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If Not Valida() Then Exit Sub

        SqlB = "SELECT 1 as Operacion,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,* FROM TransCabeza "
        SqlN = "SELECT 2 as Operacion,CAST(FLOOR(CAST(Fecha AS FLOAT)) AS DATETIME) AS Fecha,* FROM TransCabeza "

        Dim SqlComprobante As String = ""
        If TextComprobante.Text <> "" Then
            SqlComprobante = "WHERE Transferencia = " & TextComprobante.Text & " "
        Else : SqlComprobante = "WHERE Transferencia LIKE '%' "
        End If

        Dim SqlDeposito As String = ""
        If ComboDeposito.SelectedValue <> 0 Then
            SqlDeposito = "AND Origen = " & ComboDeposito.SelectedValue & " "
        End If

        Dim SqlFecha As String = ""
        SqlFecha = "AND Fecha >='" & Format(DateTimeDesde.Value, "yyyyMMdd") & "' AND Fecha < DATEADD(dd,1,'" & Format(DateTimeHasta.Value, "yyyyMMdd") & "') "

        SqlB = SqlB & SqlComprobante & SqlDeposito & SqlFecha
        SqlN = SqlN & SqlComprobante & SqlDeposito & SqlFecha

        LLenaGrid()

        If Dt.Rows.Count = 0 Then
            MsgBox("No existe Movimientos.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If

        Grid.Focus()

    End Sub
    Private Sub ComboDeposito_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ComboDeposito.Validating

        If IsNothing(ComboDeposito.SelectedValue) Then ComboDeposito.SelectedValue = 0

    End Sub
    Private Sub Textcomprobante_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextComprobante.KeyPress

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
    Private Sub LLenaGrid()

        Dt = New DataTable
        If Not Tablas.Read(SqlB, Conexion, Dt) Then Me.Close() : Exit Sub
        If PermisoTotal Then
            Tablas.Read(SqlN, ConexionN, Dt)
        End If

        Dim View As New DataView
        View = Dt.DefaultView
        View.Sort = "Transferencia"

        Grid.DataSource = bs
        bs.DataSource = View

    End Sub
    Private Sub GeneraCombosGrid()

        Origen.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 19 & " ORDER BY Nombre;")
        Origen.DisplayMember = "Nombre"
        Origen.ValueMember = "Clave"

        Destino.DataSource = Tablas.Leer("Select Clave,Nombre From Tablas WHERE Tipo = " & 19 & " ORDER BY Nombre;")
        Destino.DisplayMember = "Nombre"
        Destino.ValueMember = "Clave"

        Estado.DataSource = DtEstadoAfectaStockYBaja()
        Estado.DisplayMember = "Nombre"
        Estado.ValueMember = "Clave"

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

        If Grid.Columns(e.ColumnIndex).Name = "Comprobante" Then
            If Not IsNothing(e.Value) Then
                If PermisoTotal Then
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Abierto")
                    If Grid.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then Grid.Rows(e.RowIndex).Cells("Candado").Value = UnRemito.ImageList1.Images.Item("Cerrado")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Fecha" Then
            If Not IsDBNull(e.Value) Then e.Value = Format(e.Value, "dd/MM/yyyy")
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Rows.Count = 0 Then Exit Sub

        GModificacionOk = False

        UnaTransferencia.PTrans = Grid.CurrentRow.Cells("Comprobante").Value
        If Grid.CurrentRow.Cells("Operacion").Value = 1 Then
            UnaTransferencia.PAbierto = True
        Else : UnaTransferencia.PAbierto = False
        End If
        UnaTransferencia.ShowDialog()
        UnaTransferencia.Dispose()
        If GModificacionOk Then ButtonAceptar_Click(Nothing, Nothing)

    End Sub

    Private Sub ButtonExcel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonExcel.Click

        If Grid.Rows.Count = 0 Then MsgBox("No hay transferencias en el listado!", MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "ERROR!") : Exit Sub

        Dim DTExcel As New DataTable

        CreaDT(DTExcel)

        For Each Row As DataGridViewRow In Grid.Rows
            Dim StrConexion As String
            Dim DTTransferencia As New DataTable
            Dim Operacion As String = ""
            Dim UnidadMedida As String = ""

            StrConexion = Conexion
            Operacion = "Abierto"
            If Row.Cells("Operacion").Value = 2 Then
                StrConexion = ConexionN
                Operacion = "Cerrado"
            End If

            If Not Tablas.Read("SELECT DISTINCT TD.Lote, TD.Secuencia, TD.Cantidad, L.Articulo FROM TransDetalle TD, Lotes L WHERE Transferencia = " & Row.Cells("Comprobante").Value & " AND L.Lote = TD.Lote AND L.Secuencia = TD.Secuencia;", StrConexion, DTTransferencia) Then End

            For Each FilaTrans As DataRow In DTTransferencia.Rows
                Dim Fila As DataRow
                Fila = DTExcel.NewRow()
                Fila("Operacion") = Operacion
                Fila("Transferencia") = Row.Cells("Comprobante").Value
                Fila("Fecha") = Row.Cells("Fecha").FormattedValue
                Fila("Origen") = Row.Cells("Origen").FormattedValue
                Fila("Destino") = Row.Cells("Destino").FormattedValue
                Fila("Estado") = Row.Cells("Estado").FormattedValue
                Fila("Lote") = FilaTrans.Item("Lote") & "/" & FilaTrans.Item("Secuencia")
                Fila("Articulo") = NombreArticulo(FilaTrans.Item("Articulo"))
                Fila("Cantidad") = FilaTrans.Item("Cantidad")

                HallaAGranelYMedida(FilaTrans.Item("Articulo"), 0, UnidadMedida)
                Fila("Unidad") = UnidadMedida
                DTExcel.Rows.Add(Fila)
            Next
            DTExcel.Rows.Add()
        Next

        TablaAExcel(DTExcel, "Listado De Transferencias de Mercaderia", DateTimeDesde.Value, DateTimeHasta.Value)

    End Sub
    Private Sub CreaDT(ByRef DT As DataTable)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Operacion)

        Dim Transferencia As New DataColumn("Transferencia")
        Transferencia.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Transferencia)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Fecha)

        Dim Origen As New DataColumn("Origen")
        Origen.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Origen)

        Dim Destino As New DataColumn("Destino")
        Destino.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Destino)

        Dim Estado As New DataColumn("Estado")
        Estado.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Estado)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Lote)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Cantidad)

        Dim Unidad As New DataColumn("Unidad")
        Unidad.DataType = System.Type.GetType("System.String")
        DT.Columns.Add(Unidad)

    End Sub
End Class