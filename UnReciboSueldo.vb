Imports System.Drawing.Printing
Imports System.IO
Imports System.Globalization
Public Class UnReciboSueldo
    Public PRecibo As Integer
    Public PLegajo As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    Private WithEvents bs As New BindingSource
    '
    Dim DtRecibosCabeza As DataTable
    Dim DtRecibosDetalle As DataTable
    Dim DtGrid1 As DataTable
    Dim DtTablaConceptos As DataTable
    ' 
    '    Dim DirectorioDatos As String = "C:\ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "Datos.TXT"
    '  Dim DirectorioParametros As String = "C:\ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "Parametros.TXT"
    Dim DirectorioParametros As String = "\\SERVER_RDAPP\" & "ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "Parametros.TXT"
    Dim DirectorioDatos As String = "\\SERVER_RDAPP\" & "ArchivosImportados\" + Format(CInt(GClaveEmpresa), "000") + "Datos.TXT"

    '
    Dim RowCabezaAnt As DataRow
    Dim cc As ComboBox
    Dim ConexionRecibo As String
    Dim ImporteW As Double
    ' Para impresion.        If GCuitEmpresa = GPatagonia Then

    Dim LineasParaImpresion As Integer = 0
    Dim ErrorImpresion As Boolean
    Dim NombreArchivo As String = ""
    Dim CUIL As Decimal
    Dim DtLegajo As New DataTable
    Dim FechaReciboTXT As String
    Private Sub UnReciboSueldo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not PermisoEscritura(9) Then PBloqueaFunciones = True

        Me.Top = 50

        Grid1.AutoGenerateColumns = False

        If GCuitEmpresa = GPatagonia Then
            ButtonImportacion.Visible = True
            Panel1.Visible = True

            Grid1.Columns("Unidades").Visible = True
            Grid1.Size = New Size(460, Grid1.Height)
            Grid1.Left = PanelContenedor.Width / 2 - Grid1.Width / 2

            ButtonEliminarLineaConcepto.Left = Grid1.Left
            TextImporte.Left = Grid1.Left + Grid1.Width - TextImporte.Width
        Else
            ButtonDesdeParametros.Visible = False
        End If

        If PRecibo = 0 Then
            OpcionLegajos.ShowDialog()
            PLegajo = OpcionLegajos.PLegajo
            PAbierto = OpcionLegajos.PAbierto
            TextNombre.Text = OpcionLegajos.PNombre
            OpcionLegajos.Dispose()
            If PLegajo = 0 Then Me.Close() : Exit Sub
        Else
            ButtonImportacion.Enabled = True
        End If

        ArmaTablaConceptos()

        LlenaCombosGrid()

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        If PAbierto Then
            ConexionRecibo = Conexion
        Else : ConexionRecibo = ConexionN
        End If

        If PAbierto Then PictureCandado.Image = UnRecibo.ImageList1.Images.Item("Abierto")
        If Not PAbierto Then PictureCandado.Image = UnRecibo.ImageList1.Images.Item("Cerrado")
        If Not PermisoTotal Then PictureCandado.Visible = False

        If Not ArmaArchivos() Then Me.Close() : Exit Sub

        GModificacionOk = False

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtGrid1.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        Grid1.EndEdit()
        MiEnlazador.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtRecibosDetalleAux As DataTable = DtRecibosDetalle.Copy
        If DtRecibosCabezaAux.Rows(0).Item("FechaContable") <> CDate(TextFechaContable.Text) Then
            DtRecibosCabezaAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
        End If

        If PRecibo = 0 Then If Not ActualizaArchivos(DtRecibosCabezaAux, DtRecibosDetalleAux) Then Exit Sub

        If IsNothing(DtRecibosDetalleAux.GetChanges) And IsNothing(DtRecibosCabezaAux.GetChanges) Then
            MsgBox("No hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If PRecibo = 0 Then
            If HacerAlta(DtRecibosCabezaAux, DtRecibosDetalleAux) Then
                ArmaArchivos()
            End If
        Else
            If HacerModificacion(DtRecibosCabezaAux, DtRecibosDetalleAux) Then
                ArmaArchivos()
            End If
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonBaja_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonBaja.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PRecibo = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("Recibo esta ANULADO. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Dim DtRecibosCabezaAux As DataTable = DtRecibosCabeza.Copy
        Dim DtRecibosDetalleAux As DataTable = DtRecibosDetalle.Copy

        If Not IsNothing(DtRecibosDetalleAux.GetChanges) Then
            MsgBox("Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        DtRecibosCabeza.Rows(0).Item("Saldo") = Trunca(DtRecibosCabeza.Rows(0).Item("Saldo")) 'Arreglo para solucionar redondeo.

        If DtRecibosCabeza.Rows(0).Item("Saldo") <> DtRecibosCabeza.Rows(0).Item("Importe") Then
            MsgBox("Recibo Tiene Pago. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable
        ' 
        If GGeneraAsiento Then
            If Not HallaAsientosCabeza(4000, DtRecibosCabezaAux.Rows(0).Item("Recibo"), DtAsientoCabeza, ConexionRecibo) Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Recibo se Dara de Baja. Desea CONTINUAR la Operación?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Me.Cursor = System.Windows.Forms.Cursors.Default
            Exit Sub
        End If

        DtRecibosCabezaAux.Rows(0).Item("Estado") = 3

        Dim resul As Integer = ActualizaMovi("B", DtRecibosCabezaAux, DtRecibosDetalleAux, DtAsientoCabeza, DtAsientoDetalle, ConexionRecibo)
        If resul < 0 Then
            MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul = 0 Then
            MsgBox("Otro usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
        End If
        If resul > 0 Then
            MsgBox("Movimiento Fue Dado de Baja Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PRecibo = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 4000
        If PAbierto Then
            ListaAsientos.PDocumentoB = PRecibo
        Else
            ListaAsientos.PDocumentoN = PRecibo
        End If
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        DtLegajo.Rows.Clear()
        PRecibo = 0
        UnReciboSueldo_Load(Nothing, Nothing)

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueDeposito_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaquePeriodo.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaPeriodo.Text = ""
        Else : TextFechaPeriodo.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub ButtonEliminarLineaConcepto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLineaConcepto.Click

        If Grid1.Rows.Count = 1 Then Exit Sub

        Dim Row As DataRow
        Row = DtGrid1.Rows.Item(Grid1.CurrentRow.Index)
        Row.Delete()

        CalculaTotales()

    End Sub
    Private Sub ButtonDesdeParametros_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDesdeParametros.Click

        HallaDatosDeParametros()

    End Sub
    Private Sub TextMes_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMes.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnio_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAnio.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextMesDeposito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextMesDeposito.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub TextAnioDeposito_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextAnioDeposito.KeyPress

        If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Function ArmaArchivos() As Boolean

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        CreaDtGrid1()

        Dim Sql As String

        DtRecibosCabeza = New DataTable
        Sql = "SELECT * FROM RecibosSueldosCabeza WHERE  Recibo = " & PRecibo & ";"
        If Not Tablas.Read(Sql, ConexionRecibo, DtRecibosCabeza) Then Return False
        If PRecibo <> 0 And DtRecibosCabeza.Rows.Count = 0 Then
            MsgBox("Recibo de Sueldo No Existe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If PRecibo = 0 Then
            Dim Row As DataRow = DtRecibosCabeza.NewRow
            Row("Legajo") = PLegajo
            Row("Recibo") = 0
            Row("Mes") = 0
            Row("Anio") = 0
            Row("Importe") = 0
            Row("Fecha") = Now
            Row("FechaContable") = "01/01/1800"
            Row("Caja") = GCaja
            Row("Comentario") = ""
            Row("Estado") = 1
            Row("Saldo") = 0
            DtRecibosCabeza.Rows.Add(Row)
        End If

        RowCabezaAnt = DtRecibosCabeza.Rows(0)

        MuestraCabeza()

        DtRecibosDetalle = New DataTable
        Sql = "SELECT * FROM RecibosSueldosDetalle WHERE Recibo = " & DtRecibosCabeza.Rows(0).Item("Recibo") & ";"
        If Not Tablas.Read(Sql, ConexionRecibo, DtRecibosDetalle) Then Return False

        For Each Row As DataRow In DtRecibosDetalle.Rows
            Dim Row1 As DataRow = DtGrid1.NewRow
            Row1("Item") = Row("Item")
            Row1("Recibo") = Row("Recibo")
            Row1("Concepto") = Row("Concepto")
            Row1("Importe") = Row("Importe")
            Row1("Unidades") = Row("Unidades")
            Row1("CodigoExterno") = 0
            Dim RowBusqueda() As DataRow = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowBusqueda.Length > 0 Then Row1("CodigoExterno") = RowBusqueda(0).Item("UltimoNumero")

            DtGrid1.Rows.Add(Row1)
        Next

        If PRecibo = 0 Then   'muestra conceptos habituales.
            For Each Row As DataRow In DtTablaConceptos.Rows
                If Row("Clave") <> 0 Then
                    If Row("Activo4") Then
                        Dim Row1 As DataRow = DtGrid1.NewRow
                        Row1("Legajo") = PLegajo
                        Row1("Item") = 0
                        Row1("Recibo") = 0
                        Row1("CodigoExterno") = Row("UltimoNumero")
                        Row1("Concepto") = Row("Clave")
                        Row1("Importe") = 0
                        Row1("Unidades") = 0
                        DtGrid1.Rows.Add(Row1)
                    End If
                End If
            Next
        End If

        Grid1.DataSource = bs
        bs.DataSource = DtGrid1

        LeerLegajo()

        If ComboEstado.SelectedValue = 3 Then
            Grid1.ReadOnly = True
            ButtonEliminarLineaConcepto.Enabled = False
        End If

        If PRecibo <> 0 Then ProtejeGrid(Grid1)

        CalculaTotales()

        ImporteW = DtRecibosCabeza.Rows(0).Item("Importe") 'lo guardo por si hubo cambio.

        If PRecibo = 0 Then
            ButtonEliminarLineaConcepto.Enabled = True
            Grid1.ReadOnly = False
        Else
            ButtonEliminarLineaConcepto.Enabled = False
            Grid1.ReadOnly = True
        End If

        AddHandler DtGrid1.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid1_ColumnChanging)
        AddHandler DtGrid1.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGrid1_NewRow)
        AddHandler DtGrid1.RowChanged, New DataRowChangeEventHandler(AddressOf Dtgrid1_RowChanged)

        Me.Cursor = System.Windows.Forms.Cursors.Default

        Return True

    End Function
    Private Sub MuestraCabeza()

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtRecibosCabeza

        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "Recibo")
        AddHandler Enlace.Format, AddressOf FormatRecibo
        TextRecibo.DataBindings.Clear()
        TextRecibo.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Mes")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextMes.DataBindings.Clear()
        TextMes.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Anio")
        AddHandler Enlace.Format, AddressOf FormatEntero
        TextAnio.DataBindings.Clear()
        TextAnio.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Importe")
        AddHandler Enlace.Format, AddressOf FormatImporte
        TextImporte.DataBindings.Clear()
        TextImporte.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else
            TextFechaContable.Text = Format(DtRecibosCabeza.Rows(0).Item("FechaContable"), "dd/MM/yyyy")
        End If

    End Sub
    Private Sub FormatImporte(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, GDecimales)
        End If

    End Sub
    Private Sub FormatEntero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        End If

    End Sub
    Private Sub FormatRecibo(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = 0 Then
            Numero.Value = Format(Numero.Value, "#")
        Else : Numero.Value = FormatNumber(Numero.Value, 0)
        End If

    End Sub
    Private Sub ParseCero(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        If Numero.Value = "" Then Numero.Value = 0

    End Sub
    Private Function ActualizaArchivos(ByVal DtRecibosCabezaAux As DataTable, ByRef DtRecibosDetalleAux As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        DtRecibosCabezaAux.Rows(0).Item("Saldo") = DtRecibosCabezaAux.Rows(0).Item("Importe")

        'Actualizo Conceptos.
        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtRecibosDetalleAux.Select("Item = " & Row("Item"))
            If RowsBusqueda.Length = 0 Then
                Dim Row1 As DataRow = DtRecibosDetalleAux.NewRow
                Row1("Recibo") = 0
                Row1("Concepto") = Row("Concepto")
                Row1("Importe") = Row("Importe")
                Row1("Unidades") = Row("Unidades")
                DtRecibosDetalleAux.Rows.Add(Row1)
            End If
        Next

        Return True

    End Function
    Private Sub CreaDtGrid1()

        DtGrid1 = New DataTable

        Dim Legajo As New DataColumn("Legajo")
        Legajo.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Legajo)

        Dim Recibo As New DataColumn("Recibo")
        Recibo.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Recibo)

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Item)

        Dim CodigoExterno As New DataColumn("CodigoExterno")
        CodigoExterno.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(CodigoExterno)

        Dim Concepto As New DataColumn("Concepto")
        Concepto.DataType = System.Type.GetType("System.Int32")
        DtGrid1.Columns.Add(Concepto)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtGrid1.Columns.Add(Importe)

        Dim Unidades As New DataColumn("Unidades")
        Unidades.DataType = System.Type.GetType("System.Double")
        DtGrid1.Columns.Add(Unidades)

    End Sub
    Private Function HacerAlta(ByVal DtReciboCabezaAux As DataTable, ByVal DtReciboDetalleAux As DataTable) As Boolean

        'Arma Asientos.
        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If GGeneraAsiento Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Return False
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalle) Then Return False
            If Not ArmaArchivosAsiento("A", DtReciboCabezaAux, DtAsientoCabeza, DtAsientoDetalle) Then Return False
        End If

        Dim NumeroMovi As Double
        Dim Resul As Double
        Dim NumeroAsiento As Double

        For i As Integer = 1 To 50
            'Halla Ultima numeracion.
            NumeroMovi = UltimaNumeracion(ConexionRecibo)
            If NumeroMovi < 0 Then
                MsgBox("Error Base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
            '
            DtReciboCabezaAux.Rows(0).Item("Recibo") = NumeroMovi
            For Each Row As DataRow In DtReciboDetalleAux.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row("Recibo") = NumeroMovi
                End If
            Next
            ' 
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabeza.Rows.Count <> 0 Then
                NumeroAsiento = UltimaNumeracionAsiento(ConexionRecibo)
                If NumeroAsiento < 0 Then
                    MsgBox("ERROR Base de datos al leer tabla de Asientos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Return False
                End If
            End If
            '
            If DtAsientoCabeza.Rows.Count <> 0 Then
                DtAsientoCabeza.Rows(0).Item("Asiento") = NumeroAsiento
                DtAsientoCabeza.Rows(0).Item("Documento") = NumeroMovi
                For Each Row As DataRow In DtAsientoDetalle.Rows
                    Row("Asiento") = NumeroAsiento
                Next
            End If

            Resul = ActualizaMovi("A", DtReciboCabezaAux, DtReciboDetalleAux, DtAsientoCabeza, DtAsientoDetalle, ConexionRecibo)

            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = -3 Then
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PRecibo = NumeroMovi
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function HacerModificacion(ByVal DtReciboCabezaAux As DataTable, ByVal DtReciboDetalleAux As DataTable) As Boolean

        Dim DtAsientoCabeza As New DataTable
        Dim DtAsientoDetalle As New DataTable

        If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabeza) Then Return False

        If RowCabezaAnt("FechaContable") <> DtReciboCabezaAux.Rows(0).Item("FechaContable") Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE TipoDocumento = " & 4000 & " AND Documento = " & PRecibo & ";", ConexionRecibo, DtAsientoCabeza) Then Return False
            Dim IntFecha As Integer = DtReciboCabezaAux.Rows(0).Item("FechaContable").Year & Format(DtReciboCabezaAux.Rows(0).Item("FechaContable").Month, "00") & Format(DtReciboCabezaAux.Rows(0).Item("FechaContable").Day, "00")
            If DtAsientoCabeza.Rows.Count <> 0 Then DtAsientoCabeza.Rows(0).Item("IntFecha") = IntFecha
        End If

        Dim Resul As Decimal = ActualizaMovi("M", DtReciboCabezaAux, DtReciboDetalleAux, DtAsientoCabeza, DtAsientoDetalle, ConexionRecibo)

        If Resul = -2 Or Resul <> 0 And Resul <> 1000 Then
            MsgBox("Error Base de Dato. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If
        If Resul = 1000 Then
            MsgBox("Modificación Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            Return True
        End If

    End Function
    Private Function ActualizaMovi(ByVal Funcion As String, ByVal DtReciboCabezaAux As DataTable, ByVal DtReciboDetalleAux As DataTable, ByVal DtAsientoCabezaAux As DataTable, ByVal DtAsientoDetalleAux As DataTable, ByVal ConexionStr As String) As Double

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    If Not IsNothing(DtReciboCabezaAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RecibosSueldosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtReciboCabezaAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtReciboDetalleAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM RecibosSueldosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtReciboDetalleAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoCabezaAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosCabeza;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoCabezaAux.GetChanges)
                        End Using
                    End If
                    '
                    If Not IsNothing(DtAsientoDetalleAux.GetChanges) Then
                        Using DaP As New OleDb.OleDbDataAdapter("SELECT * FROM AsientosDetalle;", MiConexion)
                            Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                            DaP.SelectCommand.Transaction = Trans
                            DaP.Update(DtAsientoDetalleAux.GetChanges)
                        End Using
                    End If
                    '
                    Trans.Commit()
                    Return 1000
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    If ex.ErrorCode = GAltaExiste Then
                        Return -1
                    Else
                        Return -2
                    End If
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Return 0
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Return -2
            End Try
        End Using

    End Function
    Public Sub LeerLegajo()

        Dim ConexionLegajo As String

        If DtRecibosCabeza.Rows(0).Item("Legajo") < 5000 Then
            ConexionLegajo = Conexion
        Else : ConexionLegajo = ConexionN
        End If

        If Not Tablas.Read("SELECT Nombres,Apellidos,Bruto,Legajo,CUIL,Categoria,Funcion,Banco,FechaAlta,ObraSocial FROM Empleados WHERE Legajo = " & DtRecibosCabeza.Rows(0).Item("Legajo") & ";", ConexionLegajo, DtLegajo) Then End
        If DtLegajo.Rows.Count <> 0 Then
            TextBruto.Text = FormatNumber(DtLegajo.Rows(0).Item("Bruto"), GDecimales)
            TextNombre.Text = Format(DtLegajo.Rows(0).Item("Legajo"), "0000") & " - " & DtLegajo.Rows(0).Item("Nombres") & " " & DtLegajo.Rows(0).Item("Apellidos")
            CUIL = DtLegajo.Rows(0).Item("CUIL")
        End If

        DtLegajo.Dispose()

    End Sub
    Public Function UltimaNumeracion(ByVal ConexionStr As String) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Recibo) FROM RecibosSueldosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Sub ArmaTablaConceptos()

        DtTablaConceptos = New DataTable
        DtTablaConceptos = Tablas.Leer("SELECT Clave,Nombre,Operador,Activo,Activo2,Activo3,Activo4,UltimoNumero FROM Tablas WHERE Tipo = 34 ORDER BY Nombre;")

    End Sub
    Private Sub LlenaCombosGrid()

        Concepto1.DataSource = DtTablaConceptos
        Dim Row As DataRow = Concepto1.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Row("Operador") = 0
        Concepto1.DataSource.Rows.Add(Row)
        Concepto1.DisplayMember = "Nombre"
        Concepto1.ValueMember = "Clave"

    End Sub
    Private Sub CalculaTotales()

        Dim Total As Double = 0
        Dim RowsBusqueda() As DataRow

        bs.EndEdit()

        For Each Row As DataRow In DtGrid1.Rows
            RowsBusqueda = DtTablaConceptos.Select("Clave = " & Row("Concepto"))
            If RowsBusqueda(0).Item("Operador") = 2 Then
                If Row("Importe") > 0 Then     'Si esta ya en negativo no lo haga positivo por multiplicar por -1.
                    Row("Importe") = -1 * Row("Importe")
                End If
            End If
            Total = Total + Row("Importe")
        Next

        TextImporte.Text = FormatNumber(Total, GDecimales)

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByVal DtReciboCabezaAux As DataTable, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable) As Boolean

        Dim ListaLotesParaAsientoAux As New List(Of ItemLotesParaAsientos)
        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)

        Dim Item As New ItemListaConceptosAsientos

        For Each Row As DataRow In DtGrid1.Rows
            Item = New ItemListaConceptosAsientos
            Item.Legajo = DtReciboCabezaAux.Rows(0).Item("Legajo")
            Item.Clave = Row("Concepto")
            Item.Importe = Row("Importe")
            If Item.Importe < 0 Then Item.Importe = -1 * Item.Importe
            ListaConceptos.Add(Item)
        Next
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = CDbl(TextImporte.Text)
        ListaConceptos.Add(Item)

        If Funcion = "A" Then
            If Not Asiento(4000, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function UltimaNumeracionAsiento(ByVal ConexionStr) As Integer

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Asiento) FROM AsientosCabeza;", Miconexion)
                    Dim Ultimo = Cmd.ExecuteScalar()
                    If Not IsDBNull(Ultimo) Then
                        Return CDbl(Ultimo) + 1
                    Else : Return 1
                    End If
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If Grid1.Rows.Count = 1 Then
            MsgBox("Falta Informar Conceptos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid1.Focus()
            Return False
        End If

        Dim Row As DataGridViewRow

        For i As Integer = 0 To Grid1.Rows.Count - 2
            Row = Grid1.Rows(i)
            If Row.Cells("Concepto1").Value = 0 Then
                MsgBox("Debe Informar Concepto en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Concepto1")
                Grid1.BeginEdit(True)
                Return False
            End If
            If Row.Cells("Importe1").Value = 0 Then
                MsgBox("Debe Informar Importe en la linea " & i + 1, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid1.CurrentCell = Grid1.Rows(i).Cells("Importe1")
                Grid1.BeginEdit(True)
                Return False
            End If
        Next

        If CDbl(TextImporte.Text) <= 0 Then
            MsgBox("Importe Recibo debe ser Mayor a Cero.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid1.BeginEdit(True)
            Return False
        End If

        If TextMes.Text = "" Then
            MsgBox("Falta Mes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMes.Focus()
            Return False
        End If
        If CInt(TextMes.Text) < 1 Or CInt(TextMes.Text) > 12 Then
            MsgBox("Mes Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextMes.Focus()
            Return False
        End If
        If TextAnio.Text = "" Then
            MsgBox("Falta Año.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextAnio.Focus()
            Return False
        End If
        If CInt(TextAnio.Text) < 2000 Then
            MsgBox("Año Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextAnio.Focus()
            Return False
        End If

        If TextMesDeposito.Text <> "" Then
            If CInt(TextMes.Text) < 1 Or CInt(TextMes.Text) > 12 Then
                MsgBox("Mes Deposito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextMesDeposito.Focus()
                Return False
            End If
        End If
        If TextAnioDeposito.Text <> "" Then
            If CInt(TextAnioDeposito.Text) < 2000 Then
                MsgBox("Año Deposito Incorrecto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                TextAnio.Focus()
                Return False
            End If
        End If

        If Not ConsisteFecha(TextFechaContable.Text) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If DiferenciaDias(DateTime1.Value, TextFechaContable.Text) < -365 Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If DiferenciaDias(DateTime1.Value, TextFechaContable.Text) > 0 Then
            MsgBox("Fecha Contable Mayor que la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If

        Return True

    End Function
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PRecibo = 0 Then MsgBox("DEBE GRABAR EL RECIBO ANTES DE IMPRIMIR!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub

        If TextMesDeposito.Text = "" Or TextAnioDeposito.Text = "" Or TextFechaPeriodo.Text = "" Or TextBancoDeposito.Text = "" Or TextNombrePeriodo.Text = "" Then
            If GCuitEmpresa = GPatagonia Then
                MsgBox("Debe informar Fecha Deposito S.S., Fecha Periodo S.S., Banco o Nombre del Periodo.") : Exit Sub
            End If
        End If

        PlantillaReciboSueldo.IniciaImpresion()

        Dim DTDetalle As DataTable
        PlantillaReciboSueldo.CreaDTDetalle(DTDetalle)

        PlantillaReciboSueldo.TipoReciboStr = TextNombrePeriodo.Text
        If FechaReciboTXT <> "" Then PlantillaReciboSueldo.FechaRecibo = FechaReciboTXT

        Dim FechaDeposito As String = ""
        If TextMesDeposito.Text <> "" And TextAnioDeposito.Text <> "" Then FechaDeposito = TextMesDeposito.Text.PadLeft(2, "00") & "/" & TextAnioDeposito.Text

        Dim Periodo As String = ""
        If TextFechaPeriodo.Text <> "" Then Periodo = TextFechaPeriodo.Text.Substring(0, 5)

        PlantillaReciboSueldo.VectorCabeza(0) = ""
        PlantillaReciboSueldo.VectorCabeza(1) = Format(DtLegajo.Rows(0).Item("Legajo"), "0000")
        PlantillaReciboSueldo.VectorCabeza(2) = DtLegajo.Rows(0).Item("Apellidos") & " " & DtLegajo.Rows(0).Item("Nombres")
        PlantillaReciboSueldo.VectorCabeza(3) = Format(CUIL, "##-########-#")
        PlantillaReciboSueldo.VectorCabeza(4) = Format(DtLegajo.Rows(0).Item("FechaAlta"), "dd/MM/yyyy")
        PlantillaReciboSueldo.VectorCabeza(5) = ""
        PlantillaReciboSueldo.VectorCabeza(6) = "$ " & TextBruto.Text
        PlantillaReciboSueldo.VectorCabeza(7) = Periodo
        PlantillaReciboSueldo.VectorCabeza(8) = FechaDeposito
        PlantillaReciboSueldo.VectorCabeza(9) = TextBancoDeposito.Text
        PlantillaReciboSueldo.VectorCabeza(10) = DtLegajo.Rows(0).Item("Categoria")
        PlantillaReciboSueldo.VectorCabeza(11) = DtLegajo.Rows(0).Item("Funcion")

        Dim Fila As DataRow
        Dim CodigoExterno As Integer

        For Each Row As DataGridViewRow In Grid1.Rows
            If Row.Cells("Concepto1").Value = 0 Then Exit For

            Fila = DTDetalle.NewRow()
            Fila.Item("Cod") = Row.Cells("CodigoExterno").Value
            Fila.Item("Concepto") = Row.Cells("Concepto1").FormattedValue
            Fila.Item("Unidades") = Row.Cells("Unidades").Value
            Fila.Item("HaberCDesc") = 0
            Fila.Item("HaberSDesc") = 0
            Fila.Item("Deducciones") = 0

            Dim RowBusqueda() As DataRow = DtTablaConceptos.Select("Clave = " & Row.Cells("Concepto1").Value)
            If RowBusqueda.Length > 0 Then
                If RowBusqueda(0).Item("Activo") Then Fila.Item("Deducciones") = (-1) * Row.Cells("Importe1").Value
                If RowBusqueda(0).Item("Activo2") Then Fila.Item("HaberSDesc") = Row.Cells("Importe1").Value
                If Not RowBusqueda(0).Item("Activo") And Not RowBusqueda(0).Item("Activo2") Then Fila.Item("HaberCDesc") = Row.Cells("Importe1").Value
            End If

            DTDetalle.Rows.Add(Fila)

            PlantillaReciboSueldo.VectorTotales(0) += Fila.Item("HaberCDesc")
            PlantillaReciboSueldo.VectorTotales(1) += Fila.Item("HaberSDesc")
            PlantillaReciboSueldo.VectorTotales(2) += Fila.Item("Deducciones")
        Next

        PlantillaReciboSueldo.VectorTotales(3) = PlantillaReciboSueldo.VectorTotales(0) + PlantillaReciboSueldo.VectorTotales(1) - PlantillaReciboSueldo.VectorTotales(2)

        PlantillaReciboSueldo.DTDetalle = DTDetalle
        PlantillaReciboSueldo.ObraSocial = NombreObraSocial(DtLegajo.Rows(0).Item("ObraSocial"))

        PlantillaReciboSueldo.Imprime()

    End Sub
    Private Function NombreObraSocial(ByRef ObraSocial As Integer) As String

        Dim Dt As New DataTable

        Try
            If Not Tablas.Read("SELECT Nombre FROM Tablas WHERE Clave = " & ObraSocial & " AND Tipo = 46;", Conexion, Dt) Then Return ""
            If Dt.Rows.Count <> 0 Then
                Return Dt.Rows(0).Item(0)
            Else : Return ""
            End If
        Catch ex As Exception
            Return ""
        Finally
            Dt.Dispose()
        End Try

    End Function
    Private Sub ButtonImportacion_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportacion.Click

        If Not HallaDatosDeParametros() Then Exit Sub

        If DirectorioDatos = "" Then MsgBox("No se Encontro Archivo de Importación") : Exit Sub

        Grid1.EndEdit()
        DtGrid1.Clear()

        Dim Linea As String
        Dim CuilTXT As Decimal
        Dim CodigoConcepto As Integer
        Dim CodigoInterno As Integer
        Dim NombreConcepto As String
        Dim Unidades As Decimal
        Dim Importe As Decimal
        Dim VectorImporte(3) As Decimal
        Dim FilaDetalle As DataRow
        Const ThrowError As String = "NO SE PUDO CONVERTIR IMPORTE AL TIPO DECIMAL!! "

        Dim provider As New System.Globalization.NumberFormatInfo
        provider.NumberDecimalSeparator = "."

        provider.NumberGroupSeparator = ","

        'Lee Datos.
        Try
            Using reader As StreamReader = New StreamReader(DirectorioDatos)
                Linea = reader.ReadLine

                While Not reader.EndOfStream
                    CuilTXT = Linea.Substring(433, 11)
                    FechaReciboTXT = Linea.Substring(183, 8)

                    If FechaReciboTXT.Substring(2, 2) <> TextMes.Text.PadLeft(2, "0") Or CuilTXT <> CUIL Then Linea = reader.ReadLine : Continue While
                    If Linea.Substring(8, 4).Trim = "" Then Exit While

                    CodigoConcepto = Linea.Substring(8, 4)
                    Dim RowBusqueda() As DataRow = DtTablaConceptos.Select("UltimoNumero = " & CodigoConcepto)
                    If RowBusqueda.Length > 0 Then CodigoInterno = RowBusqueda(0).Item("Clave")

                    NombreConcepto = HallaNombreConcepto(CodigoConcepto)
                    If NombreConcepto = "" Then MsgBox("CONCEPTO CON CODIGO " & CodigoConcepto & " NO ESTA DEFINIDO EN TABLAS!", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub

                    Decimal.TryParse(Linea.Substring(52, 16), System.Globalization.NumberStyles.Float, provider, Unidades) ' DIAS 
                    If Unidades = 0 Then
                        Decimal.TryParse(Linea.Substring(49, 5), System.Globalization.NumberStyles.Float, provider, Unidades) ' PORCENTAJE
                    End If

                    Decimal.TryParse(Linea.Substring(71, 17), System.Globalization.NumberStyles.Float, provider, Importe)
                    If Importe = 0 Then Throw New Exception(ThrowError)

                    FilaDetalle = DtGrid1.NewRow
                    FilaDetalle("Legajo") = PLegajo
                    FilaDetalle("Recibo") = PRecibo
                    FilaDetalle("Item") = 0
                    FilaDetalle("CodigoExterno") = CodigoConcepto
                    FilaDetalle("Concepto") = CodigoInterno
                    FilaDetalle("Importe") = Importe
                    FilaDetalle("Unidades") = Unidades
                    DtGrid1.Rows.Add(FilaDetalle)

                    Linea = reader.ReadLine
                End While
                If DtGrid1.Rows.Count = 0 Then MsgBox("NO SE ENCONTRO EMPLEADO EN EL ARCHIVO DE IMPORTACION!", MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly, "ATENCION!")
            End Using
        Catch exIO As IOException
            DtGrid1.Rows.Clear()
            MsgBox("ERROR EN LA IMPORTACION DEL ARCHIVO: ---> " & exIO.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        Catch ex As Exception
            DtGrid1.Rows.Clear()
            MsgBox("ERROR EN LA IMPORTACION DEL ARCHIVO: ---> " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "ERROR!") : Exit Sub
        End Try

        Grid1.DataSource = bs
        bs.DataSource = DtGrid1

        CalculaTotales()

    End Sub
    Private Function HallaDatosDeParametros() As Boolean

        Dim Linea As String

        Try
            Using reader As StreamReader = New StreamReader(DirectorioParametros)
                Linea = reader.ReadLine
                If Val(TextMes.Text) <> 0 And Val(Linea.Substring(0, 2)) <> Val(TextMes.Text) Then MsgBox("Mes Informado no Coincide con la informada en archivo Importado.") : Return False
                If Val(TextAnio.Text) <> 0 And Val(Linea.Substring(2, 4)) <> Val(TextAnio.Text) Then MsgBox("Año Informado no Coincide con la informada en archivo Importado.") : Return False
                TextMes.Text = Linea.Substring(0, 2)
                TextAnio.Text = Linea.Substring(2, 4)
                TextMesDeposito.Text = Linea.Substring(6, 2)
                TextAnioDeposito.Text = Linea.Substring(8, 4)
                TextBancoDeposito.Text = Trim(Linea.Substring(12, 20))
                TextFechaPeriodo.Text = Linea.Substring(32, 10)          '27,19    
                TextFechaContable.Text = Linea.Substring(42, 10)
                TextNombrePeriodo.Text = Linea.Substring(52, Linea.Length - 52)
            End Using
        Catch ex As Exception
            MsgBox("No se pudo leer Archivo Importado.  " + ex.Message) : Return False
        End Try

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID1.                 --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid1_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEnter

        If Not Grid1.Columns(e.ColumnIndex).ReadOnly Then
            Grid1.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid1_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellLeave

        'para manejo del autocoplete de conceptos.
        If Grid1.Columns(e.ColumnIndex).Name = "Concepto1" Then
            If Not cc Is Nothing Then
                cc.SelectedIndex = cc.FindStringExact(cc.Text)
                If cc.SelectedIndex < 0 Then cc.SelectedValue = 0
            Else
                Exit Sub
            End If
        End If

    End Sub
    Private Sub Grid1_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid1.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cc = e.Control
            cc.DropDownStyle = ComboBoxStyle.DropDown
            cc.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cc.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim columna As Integer = Grid1.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey1_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged1_TextChanged

    End Sub
    Private Sub ValidaKey1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Unidades" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Importe1" Or Grid1.Columns.Item(Grid1.CurrentCell.ColumnIndex).Name = "Unidades" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), GDecimales)
            End If
        End If

    End Sub
    Private Sub Grid1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid1.CellEndEdit

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Then
            CalculaTotales()
        End If

    End Sub
    Private Sub Grid1_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid1.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid1.Columns(e.ColumnIndex).Name = "Importe1" Or Grid1.Columns(e.ColumnIndex).Name = "Unidades" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#.##")
                Else : e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

    End Sub
    Private Sub Grid1_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid1.DataError
        Exit Sub
    End Sub
    Private Sub DtGrid1_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        Dim Row As DataRow = e.Row

        Row("Recibo") = DtRecibosCabeza.Rows(0).Item("Recibo")
        Row("Legajo") = DtRecibosCabeza.Rows(0).Item("Legajo")
        Row("Item") = 0
        Row("CodigoExterno") = 0
        Row("Concepto") = 0
        Row("Importe") = 0
        Row("Unidades") = 0

    End Sub
    Private Sub DtGrid1_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Importe") Then
            If IsDBNull(e.Row("Importe")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("Unidades") Then
            If IsDBNull(e.Row("Unidades")) Then Exit Sub
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If

    End Sub
    Private Sub Dtgrid1_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If GCuitEmpresa <> GPatagonia Then Exit Sub

        e.Row.RowError = ""

        If e.Row.Item("Unidades") = 0 Then
            If DtTablaConceptos.Select("ACTIVO3 = 1 AND CLAVE = " & e.Row.Item("Concepto")).Length > 0 Then
                MsgBox("Debe Informar Unidades.")
                e.Row.RowError = "Error."
            End If
        End If
        Grid1.Refresh()


    End Sub

    
End Class