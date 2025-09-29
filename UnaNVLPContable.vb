Imports System.Transactions
Imports System.Drawing.Printing
Public Class UnaNVLPContable
    Public PLiquidacion As Double
    Public PAbierto As Boolean
    Public PCliente As Integer
    Public PBloqueaFunciones As Boolean
    '
    Private MiEnlazador As New BindingSource
    '
    Dim ListaDeLotes As List(Of FilaComprobanteFactura)
    '
    Dim DtLiquidacionCabezaB As DataTable
    Dim DtLiquidacionDetalleB As DataTable
    Dim DtLiquidacionArticulosB As DataTable
    Dim DtGrid As DataTable
    Dim DtGridCompro As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    '
    Dim IndiceW As Integer
    Dim TipoFactura As Integer
    Dim cb As ComboBox
    Dim TablaIva(0) As Double
    Dim IvaW As Double
    Dim ConexionLiquidacion As String
    Dim UltimaFechaContableW As DateTime
    Dim RegCabezaAnt As DataRow
    Dim ReciboOficialAnt As Decimal
    'Para Impresion.
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Private Sub UnaNVLPContable_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 50

        If Not PermisoEscritura(6) Then PBloqueaFunciones = True

        If PLiquidacion = 0 Then
            Opciones()
            If PCliente = 0 Then Me.Close() : Exit Sub
        End If

        Grid.AutoGenerateColumns = False
        Grid.Columns("Lupa").DefaultCellStyle.NullValue = Nothing

        GridCompro.AutoGenerateColumns = False

        LlenaCombo(ComboEmisor, "", "Clientes")
        ComboEmisor.SelectedValue = PCliente

        ArmaTipoIva(ComboTipoIva)
        LlenaComboTablas(ComboPais, 28)

        ComboEstado.DataSource = DtEstadoActivoYBaja()
        ComboEstado.DisplayMember = "Nombre"
        ComboEstado.ValueMember = "Clave"

        ConexionLiquidacion = Conexion

        LlenaCombosGrid()

        ArmaTablaIva(TablaIva)

        ArmaArchivos()

        GModificacionOk = False

        If Not PermisoTotal Then LabelTr.Visible = False

        UltimaFechaContableW = UltimaFechacontable(Conexion, 1)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnaNVLPContable_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PLiquidacion <> 0 Then GridCompro.Focus()

    End Sub
    Private Sub UnaNVLPContable_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

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

        MiEnlazador.EndEdit()
        Grid.EndEdit()
        GridCompro.EndEdit()

        If Not Valida() Then Exit Sub

        Dim DtLiquidacionCabezaBAux As DataTable = DtLiquidacionCabezaB.Copy
        Dim DtLiquidacionDetalleBAux As DataTable = DtLiquidacionDetalleB.Copy
        Dim DtLiquidacionArticulosBAux As DataTable = DtLiquidacionArticulosB.Copy

        If PLiquidacion = 0 Then
            If Not ActualizaArchivos("A", DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux) Then Exit Sub
        Else
            If Not ActualizaArchivos("M", DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux) Then Exit Sub
        End If

        'Arma Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        '
        If GGeneraAsiento And PLiquidacion = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Exit Sub
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaB, DtAsientoDetalleB, 1) Then Exit Sub
            End If
        End If
        If PLiquidacion <> 0 Then
            If RegCabezaAnt.Item("FechaContable") <> DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable") Then
                If Not HallaAsientosCabeza(801, PLiquidacion, DtAsientoCabezaB, Conexion) Then Exit Sub
                If DtAsientoCabezaB.Rows.Count <> 0 Then
                    Dim IntFecha As Integer = DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Year & Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Month, "00") & Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Day, "00")
                    DtAsientoCabezaB.Rows(0).Item("IntFecha") = IntFecha
                End If
            End If
        End If

        'Actualiza Archivo de distribucion retenciones por provincia.
        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone
        If PLiquidacion = 0 Then
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                Dim Row1 As DataRow = DtRetencionProvinciaWW.NewRow
                Row1("TipoNota") = 800
                Row1("Nota") = 0
                Row1("Comprobante") = Row("Comprobante")
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        If IsNothing(DtLiquidacionCabezaBAux.GetChanges) And _
           IsNothing(DtLiquidacionDetalleBAux.GetChanges) And _
           IsNothing(DtLiquidacionArticulosBAux.GetChanges) Then
            MsgBox("NO Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            HacerAlta(DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux, DtAsientoCabezaB, DtAsientoDetalleB, DtRetencionProvinciaWW)
        Else
            Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux, DtAsientoCabezaB, DtAsientoDetalleB, DtRetencionProvinciaWW)
            If Resul = 0 Then
                MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
            If Resul < 0 Then
                MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End If
            If Resul > 0 Then
                MsgBox("Modificacion Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                GModificacionOk = True
                ArmaArchivos()
            End If
        End If

    End Sub
    Private Sub ButtonAnula_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnula.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If ComboEstado.SelectedValue = 3 Then
            MsgBox("ERROR, Liquidación Ya esta Anulada. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Saldo") <> DtLiquidacionCabezaB.Rows(0).Item("Importe") Then
                MsgBox("Liquidación fue Imputada en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
        End If

        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Exit Sub
        End If

        Dim DtLiquidacionCabezaBAux As DataTable = DtLiquidacionCabezaB.Copy
        Dim DtLiquidacionDetalleBAux As DataTable = DtLiquidacionDetalleB.Copy
        Dim DtLiquidacionArticulosBAux As DataTable = DtLiquidacionArticulosB.Copy

        'Anula asiento.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable

        If GGeneraAsiento Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(801, DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaB, Conexion) Then Exit Sub
            End If
            If DtAsientoCabezaB.Rows.Count <> 0 Then DtAsientoCabezaB.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Liquidación se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        If Not ActualizaArchivos("B", DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux) Then Exit Sub

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux, DtAsientoCabezaB, DtAsientoDetalleB, DtRetencionProvinciaWW)
        '
        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("N.V.L.P. Anulada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Sub ButtonNuevo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevo.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLiquidacion = 0
        UnaNVLPContable_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonNuevaIgualCliente_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNuevaIgualCliente.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        PLiquidacion = 0
        ArmaArchivos()

    End Sub
    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. N.V.L.P. debe ser Grabada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        Paginas = 0
        Copias = 1

        Dim print_document As New PrintDocument
        AddHandler print_document.PrintPage, AddressOf Print_Print

        print_document.Print()

    End Sub
    Private Sub ButtonAsientoContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAsientoContable.Click

        If PLiquidacion = 0 Then Exit Sub

        ListaAsientos.PTipoDocumento = 801
        ListaAsientos.PDocumentoB = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
        ListaAsientos.PDocumentoN = 0
        ListaAsientos.Show()

    End Sub
    Private Sub PictureAlmanaqueFactura_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueNVLP.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaNVLP.Text = ""
        Else : TextFechaNVLP.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub PictureAlmanaqueContable_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureAlmanaqueContable.Click

        Calendario.ShowDialog()
        If Calendario.PFecha = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Calendario.PFecha, "dd/MM/yyyy")
        End If
        Calendario.Dispose()

    End Sub
    Private Sub MaskedReciboOficial_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedReciboOficial.Validating

        If Val(MaskedReciboOficial.Text) = 0 Then Exit Sub

        If Not MaskedOK(MaskedReciboOficial) Then
            MsgBox("N.V.L.P. Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedReciboOficial.Text = "000000000000"
            e.Cancel = True
            Exit Sub
        End If

    End Sub
    Private Sub ArmaArchivos()                            'ArmaArchivos

        CreaDtRetencionProvinciaAux()

        ArmaGrid()

        CreaDtGridCompro()

        DtLiquidacionCabezaB = New DataTable
        DtLiquidacionDetalleB = New DataTable
        DtLiquidacionArticulosB = New DataTable

        If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
        If PLiquidacion <> 0 And DtLiquidacionCabezaB.Rows.Count = 0 Then
            MsgBox("Liquidacion No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close() : Exit Sub
        End If
        If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
        If Not Tablas.Read("SELECT * FROM NVLPArticulos WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionArticulosB) Then Me.Close() : Exit Sub

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            PCliente = DtLiquidacionCabezaB.Rows(0).Item("Cliente")
        End If

        If Not LlenaDatosCliente(PCliente) Then Me.Close() : Exit Sub

        If PLiquidacion = 0 Then
            Dim Row As DataRow = DtLiquidacionCabezaB.NewRow
            ArmaNuevaNVLP(Row)
            Row("ReciboOficial") = CDbl(TipoFactura & Format("000000000000"))
            Row("Cliente") = PCliente
            Row("Fecha") = Now.Date
            Row("Tr") = True
            DtLiquidacionCabezaB.Rows.Add(Row)
        End If
        If PLiquidacion <> 0 Then
            RegCabezaAnt = DtLiquidacionCabezaB.Rows(0)
        End If

        DtRetencionProvincia = New DataTable
        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE TipoNota = " & 800 & " AND Nota = " & PLiquidacion & ";", Conexion, DtRetencionProvincia) Then Me.Close() : Exit Sub
        For Each Row As DataRow In DtRetencionProvincia.Rows
            Dim Row1 As DataRow = DtRetencionProvinciaAux.NewRow
            Row1("Retencion") = Row("Retencion")
            Row1("Provincia") = Row("Provincia")
            Row1("Comprobante") = Row("Comprobante")
            Row1("Importe") = Row("Importe")
            DtRetencionProvinciaAux.Rows.Add(Row1)
        Next

        MuestraCabeza()

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtLiquidacionDetalleB.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("ImporteB") = Row("Importe")
                RowsBusqueda(0).Item("Iva") = Row("Alicuota")
            End If
        Next

        For Each Row As DataRow In DtLiquidacionArticulosB.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow
            Row1("Indice") = Row("Indice")
            Row1("Cantidad") = Row("Cantidad")
            Row1("Precio") = Row("Precio")
            Row1("Articulo") = Row("Articulo")
            Row1("Iva") = Row("Iva")
            Row1("Importe") = Row("Importe")
            Dim AGranel As Boolean
            Row1("Medida") = ""
            HallaAGranelYMedida(Row1("Articulo"), AGranel, Row1("Medida"))
            DtGridCompro.Rows.Add(Row1)
        Next

        IndiceW = 0
        GridCompro.DataSource = DtGridCompro

        If PLiquidacion = 0 Then
            GridCompro.ReadOnly = False
            ButtonEliminarLinea.Enabled = True
            Grid.ReadOnly = False
            '   MaskedReciboOficial.ReadOnly = False
            MaskedLiquidacion.Text = Nothing
        Else
            MaskedLiquidacion.Text = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
            GridCompro.ReadOnly = True
            ButtonEliminarLinea.Enabled = False
            Grid.ReadOnly = True
            '   MaskedReciboOficial.ReadOnly = True
            ComboEmisor.Enabled = False
            If ComboEstado.SelectedValue = 3 Then
                Panel4.Enabled = False
            End If
        End If

        ComboEmisor.SelectedValue = PCliente

        MuestraTotales()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)
        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)
        AddHandler DtGridCompro.TableNewRow, New DataTableNewRowEventHandler(AddressOf DtGridCompro_NewRow)
        AddHandler DtGridCompro.RowChanged, New DataRowChangeEventHandler(AddressOf DtGridCompro_RowChanged)

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        MiEnlazador.DataSource = DtLiquidacionCabezaB
        Dim Enlace As Binding

        Enlace = New Binding("Text", MiEnlazador, "ReciboOficial")
        AddHandler Enlace.Format, AddressOf FormatReciboOficial
        AddHandler Enlace.Parse, AddressOf FormatParseReciboOficial
        MaskedReciboOficial.DataBindings.Clear()
        MaskedReciboOficial.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Cliente")
        ComboEmisor.DataBindings.Clear()
        ComboEmisor.DataBindings.Add(Enlace)

        Enlace = New Binding("SelectedValue", MiEnlazador, "Estado")
        ComboEstado.DataBindings.Clear()
        ComboEstado.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Fecha")
        DateTime1.DataBindings.Clear()
        DateTime1.DataBindings.Add(Enlace)

        Enlace = New Binding("Text", MiEnlazador, "Comentario")
        AddHandler Enlace.Parse, AddressOf FormatText
        TextComentario.DataBindings.Clear()
        TextComentario.DataBindings.Add(Enlace)

        Dim Row As DataRowView = MiEnlazador.Current

        If Row("FechaLiquidacion") = "01/01/1800" Then
            TextFechaNVLP.Text = ""
        Else : TextFechaNVLP.Text = Format(Row("FechaLiquidacion"), "dd/MM/yyyy")
        End If

        If Row("FechaContable") = "01/01/1800" Then
            TextFechaContable.Text = ""
        Else : TextFechaContable.Text = Format(Row("FechaContable"), "dd/MM/yyyy")
        End If

        ReciboOficialAnt = Row("ReciboOficial")

    End Function
    Private Sub FormatReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        TextTipoFactura.Text = LetraTipoIva(Strings.Left(Numero.Value, 1))
        TipoFactura = Strings.Left(Numero.Value, 1)

        Numero.Value = Strings.Right(Numero.Value, 12)

    End Sub
    Private Sub FormatParseReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(TipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000"))

    End Sub
    Private Sub FormatText(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function ActualizaArchivos(ByVal Funcion As String, ByRef DtLiquidacionCabezaBAux As DataTable, ByRef DtLiquidacionDetalleBAux As DataTable, ByRef DtLiquidacionArticulosBAux As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        If Funcion = "A" And CDbl(TextTotalB.Text) = 0 Then DtLiquidacionCabezaBAux.Clear()

        'Actualizo Cabeza de Liquidacion.
        Dim ImporteBruto As Decimal
        If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
            RowsBusqueda = DtGrid.Select("Clave = 1")
            ImporteBruto = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 2")
            ImporteBruto = ImporteBruto + RowsBusqueda(0).Item("ImporteB")
            If Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaLiquidacion"), "dd/MM/yyyy") <> CDate(TextFechaNVLP.Text) Then DtLiquidacionCabezaBAux.Rows(0).Item("FechaLiquidacion") = CDate(TextFechaNVLP.Text)
            If Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If CDbl(TextTotalB.Text) <> DtLiquidacionCabezaBAux.Rows(0).Item("Importe") Or ImporteBruto <> DtLiquidacionCabezaBAux.Rows(0).Item("ImporteBruto") Then
                Dim Gastado As Decimal = DtLiquidacionCabezaBAux.Rows(0).Item("Importe") - DtLiquidacionCabezaBAux.Rows(0).Item("Saldo")
                DtLiquidacionCabezaBAux.Rows(0).Item("ImporteBruto") = ImporteBruto
                DtLiquidacionCabezaBAux.Rows(0).Item("Importe") = CDec(TextTotalB.Text)
                DtLiquidacionCabezaBAux.Rows(0).Item("Saldo") = CDec(TextTotalB.Text) - Gastado
            End If
            If PLiquidacion = 0 Then
                DtLiquidacionCabezaBAux.Rows(0).Item("Rel") = False
            End If
        End If

        If Funcion = "B" Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then DtLiquidacionCabezaBAux.Rows(0).Item("Estado") = 3
        End If

        'Genero Archivos Articulos.
        For Each Row As DataRow In DtGridCompro.Rows
            RowsBusqueda = DtLiquidacionArticulosBAux.Select("Indice = " & Row("Indice"))
            If RowsBusqueda.Length = 0 Then
                If Row("Importe") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionArticulosBAux.NewRow
                    Row1("Liquidacion") = 0
                    Row1("Indice") = Row("Indice")
                    Row1("Articulo") = Row("Articulo")
                    Row1("Cantidad") = Row("Cantidad")
                    Row1("Precio") = Row("Precio")
                    Row1("Iva") = Row("Iva")
                    Row1("Importe") = Row("Importe")
                    DtLiquidacionArticulosBAux.Rows.Add(Row1)
                End If
            Else
                If Row("Cantidad") <> RowsBusqueda(0).Item("Cantidad") Then RowsBusqueda(0).Item("Cantidad") = Row("Cantidad")
                If Row("Precio") <> RowsBusqueda(0).Item("Precio") Then RowsBusqueda(0).Item("Precio") = Row("Precio")
                If Row("Importe") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("Importe")
                If Row("Iva") <> RowsBusqueda(0).Item("Iva") Then RowsBusqueda(0).Item("Iva") = Row("Iva")
            End If
        Next

        'Actualiza detalle NVLO Impuestos.
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtLiquidacionDetalleBAux.Select("Impuesto = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                If Row("ImporteB") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionDetalleBAux.NewRow
                    Row1("Liquidacion") = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
                    Row1("Impuesto") = Row("Clave")
                    Row1("Alicuota") = Row("Iva")
                    Row1("Importe") = Row("ImporteB")
                    DtLiquidacionDetalleBAux.Rows.Add(Row1)
                End If
            Else
                If Row("ImporteB") <> RowsBusqueda(0).Item("Importe") Or Row("Iva") <> RowsBusqueda(0).Item("Alicuota") Then
                    If Row("ImporteB") <> 0 Then
                        RowsBusqueda(0).Item("Alicuota") = Row("Iva")
                        RowsBusqueda(0).Item("Importe") = Row("ImporteB")
                    Else : RowsBusqueda(0).Delete()
                    End If
                End If
            End If
        Next

        Return True

    End Function
    Private Sub ArmaGrid()

        Dim RowsBusqueda() As DataRow

        DtGrid = ArmaConceptosNVLP(PLiquidacion)

        Grid.DataSource = DtGrid

        '   Grid.Sort(Grid.Columns("Clave"), System.ComponentModel.ListSortDirection.Ascending)

        Grid.Columns("Alicuota").ReadOnly = True
        Grid.Columns("Sel").ReadOnly = True

        For Each Row1 As DataGridViewRow In Grid.Rows
            Select Case Row1.Cells("Clave").Value
                Case 1, 2
                    Row1.Cells("ImporteB").ReadOnly = True
                Case 3, 6, 8, 11, 12
                    Row1.Cells("ImporteB").ReadOnly = True
            End Select
            Select Case Row1.Cells("Clave").Value
                Case 3
                    Row1.Cells("Alicuota").ReadOnly = True
                Case 6, 8, 11, 12
                    If GTipoIva = 2 Then
                        Row1.Cells("Alicuota").ReadOnly = True
                    Else
                        Row1.Cells("Alicuota").ReadOnly = False
                    End If
            End Select
            ' 
            RowsBusqueda = DtGrid.Select("Clave = " & Row1.Cells("Clave").Value)
            If RowsBusqueda(0).Item("Tipo") = 4 Then
                If EsRetencionPorProvincia(Row1.Cells("Clave").Value) Then
                    Row1.Cells("TieneLupa").Value = True
                Else : Row1.Cells("TieneLupa").Value = False
                End If
            End If
        Next

    End Sub
    Private Sub HacerAlta(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionArticulosBAux As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtRetencionProvinciaWW As DataTable)

        'Graba Factura.
        Dim NumeroB As Double = 0
        Dim NumeroAsientoB As Double = 0

        Dim Resul As Double = 0

        For i As Integer = 1 To 50
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                NumeroB = UltimaNumeracion(Conexion)
                If NumeroB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            '
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion") = NumeroB
                For Each Row As DataRow In DtLiquidacionDetalleBAux.Rows
                    Row("Liquidacion") = NumeroB
                Next
                For Each Row As DataRow In DtLiquidacionArticulosBAux.Rows
                    Row("Liquidacion") = NumeroB
                Next
            End If
            '
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                NumeroAsientoB = UltimaNumeracionAsiento(Conexion)
                If NumeroAsientoB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            'Actualiza Asientos.
            If DtAsientoCabezaB.Rows.Count <> 0 Then
                DtAsientoCabezaB.Rows(0).Item("Asiento") = NumeroAsientoB
                DtAsientoCabezaB.Rows(0).Item("Documento") = NumeroB
                For Each Row As DataRow In DtAsientoDetalleB.Rows
                    Row("Asiento") = NumeroAsientoB
                Next
            End If
            '
            For Each Row As DataRow In DtRetencionProvinciaWW.Rows
                Row("Nota") = NumeroB
            Next
            '
            Resul = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionDetalleBAux, DtLiquidacionArticulosBAux, DtAsientoCabezaB, DtAsientoDetalleB, DtRetencionProvinciaWW)
            '
            If Resul >= 0 Then Exit For
            If Resul = -2 Then Exit For
            If Resul = -1 And i = 50 Then Exit For
        Next

        If Resul = -1 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = -2 Then
            MsgBox("ERROR de base De Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul = 0 Then
            MsgBox("Otro Usuario modifico datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
        End If
        If Resul > 0 Then
            MsgBox("Alta Realizada Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            PLiquidacion = NumeroB
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaFactura(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionArticulosBAux As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim Resul As Double

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)

        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)

                If Not IsNothing(DtLiquidacionCabezaBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionCabezaBAux.GetChanges, "NVLPCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionDetalleBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionDetalleBAux.GetChanges, "NVLPDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionArticulosBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionArticulosBAux.GetChanges, "NVLPArticulos", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                ' graba Asiento B.
                If Not IsNothing(DtAsientoCabezaB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaB.GetChanges, "AsientosCabeza", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleB.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleB.GetChanges, "AsientosDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtRetencionProvinciaWW.GetChanges) Then
                    Resul = GrabaTabla(DtRetencionProvinciaWW.GetChanges, "RecibosRetenciones", Conexion)
                    If Resul <= 0 Then Return 0
                End If
                '
                Scope.Complete()
                Return 1000
            End Using
        Catch ex As TransactionException
            Return 0
        Finally
        End Try

    End Function
    Private Function LlenaDatosCliente(ByVal Cliente As Integer) As Boolean

        Dim Dta As New DataTable

        Dim Sql As String = "SELECT * FROM Clientes WHERE Clave = " & Cliente & ";"
        Dta = Tablas.Leer(Sql)
        If Dta.Rows.Count = 0 Then
            MsgBox("ERROR, el Cliente ya no existe o error en la Base de datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Dta.Dispose()
            Return False
        End If

        TextCalle.Text = Dta.Rows(0).Item("Calle")
        TextLocalidad.Text = Dta.Rows(0).Item("Localidad")
        TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
        ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
        TextTelefonos.Text = Dta.Rows(0).Item("Telefonos")
        TextFaxes.Text = Dta.Rows(0).Item("Faxes")
        TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
        ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
        If Not Dta.Rows(0).Item("Opr") And PLiquidacion = 0 Then
            If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        Dta.Dispose()

        Return True

    End Function
    Private Sub CalculaTotal()

        Dim TotalBG As Decimal = 0
        Dim TotalBE As Decimal = 0
        Dim RowsBusqueda() As DataRow
        Dim IvaW As Decimal = 0

        If GridCompro.Rows.Count <> 0 Then
            IvaW = GridCompro.Rows(0).Cells("Iva").Value
        End If
        RowsBusqueda = DtGrid.Select("Clave = 3")
        RowsBusqueda(0).Item("Iva") = Format(IvaW, "0.00")

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Iva").Value <> 0 Then
                TotalBG = TotalBG + Row.Cells("Importe").Value
            Else
                TotalBE = TotalBE + Row.Cells("Importe").Value
            End If
        Next
        RowsBusqueda = DtGrid.Select("Clave = 1")
        RowsBusqueda(0).Item("ImporteB") = TotalBG
        RowsBusqueda = DtGrid.Select("Clave = 2")
        RowsBusqueda(0).Item("ImporteB") = TotalBE

        RowsBusqueda = DtGrid.Select("Clave = 1")
        Dim Grabado As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Dim Comision As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Dim Descarga As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        Dim FleteTerrestre As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        Dim OtrosConceptos As Decimal = RowsBusqueda(0).Item("ImporteB")

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 3 Then
                Row("ImporteB") = Trunca(Grabado * Row("Iva") / 100)
            End If
            If Row("Clave") = 6 Then
                Row("ImporteB") = Trunca(Comision * Row("Iva") / 100)
            End If
            If Row("Clave") = 8 Then
                Row("ImporteB") = Trunca(Descarga * Row("Iva") / 100)
            End If
            If Row("Clave") = 11 Then
                Row("ImporteB") = Trunca(FleteTerrestre * Row("Iva") / 100)
            End If
            If Row("Clave") = 12 Then
                Row("ImporteB") = Trunca(OtrosConceptos * Row("Iva") / 100)
            End If
        Next

        MuestraTotales()

    End Sub
    Private Sub MuestraTotales()

        Dim TotalB As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Then
                TotalB = TotalB + Row("ImporteB")
            Else
                TotalB = TotalB - Row("ImporteB")
            End If
        Next

        TextTotalB.Text = FormatNumber(TotalB, GDecimales)

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Medida)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Indice)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Articulo)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Cantidad)

        Dim Precio As New DataColumn("Precio")
        Precio.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Precio)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Iva)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Importe)

    End Sub
    Private Sub CreaDtRetencionProvinciaAux()

        DtRetencionProvinciaAux = New DataTable

        Dim Retencion As New DataColumn("Retencion")
        Retencion.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Retencion)

        Dim Provincia As New DataColumn("Provincia")
        Provincia.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Provincia)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Int32")
        DtRetencionProvinciaAux.Columns.Add(Comprobante)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Double")
        DtRetencionProvinciaAux.Columns.Add(Importe)

    End Sub
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow

        If PLiquidacion = 0 Then
            Articulo.DataSource = ArticulosActivos()
        Else
            Articulo.DataSource = TodosLosArticulos()
        End If
        Row = Articulo.DataSource.NewRow()
        Row("Clave") = 0
        Row("Nombre") = " "
        Articulo.DataSource.Rows.Add(Row)
        Articulo.DisplayMember = "Nombre"
        Articulo.ValueMember = "Clave"

    End Sub
    Private Function UltimaNumeracion(ByVal ConexionStr) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT MAX(Liquidacion) FROM NVLPCabeza;", Miconexion)
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
    Private Sub Opciones()

        OpcionLetra.BackColor = Color.LightGreen
        OpcionLetra.PEmisor = PCliente
        OpcionLetra.PictureCandado.Visible = False
        OpcionLetra.PEsNVLP = True
        OpcionLetra.ShowDialog()
        PCliente = OpcionLetra.PEmisor
        TipoFactura = OpcionLetra.PNumeroLetra
        OpcionLetra.Dispose()

    End Sub
    Private Function ArmaArchivosAsiento(ByVal Funcion As String, ByRef DtCabeza As DataTable, ByRef DtDetalle As DataTable, ByVal Operacion As Integer) As Boolean

        Dim ListaLotesParaAsientoAux As New List(Of ItemLotesParaAsientos)
        Dim ListaLotesParaAsiento As New List(Of ItemLotesParaAsientos)
        Dim ListaConceptos As New List(Of ItemListaConceptosAsientos)
        Dim ListaCuentas As New List(Of ItemCuentasAsientos)
        Dim ListaIVA As New List(Of ItemListaConceptosAsientos)
        Dim ListaRetenciones As New List(Of ItemListaConceptosAsientos)
        Dim RowsBusqueda() As DataRow

        If Not ArmaListaParaNetoComisionDescargaEtc(ListaConceptos, Operacion) Then Return False
        '
        Dim Item As New ItemListaConceptosAsientos
        Item.Clave = 213
        Item.Importe = CDbl(TextTotalB.Text)
        ListaConceptos.Add(Item)
        '
        RowsBusqueda = DtGrid.Select("Clave = 3")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 6
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 6")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 8")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 11")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        End If
        '
        RowsBusqueda = DtGrid.Select("Clave = 12")
        If Operacion = 1 Then
            If RowsBusqueda(0).Item("ImporteB") <> 0 Then
                Item = New ItemListaConceptosAsientos
                Item.Clave = HallaClaveIva(RowsBusqueda(0).Item("Iva"))
                Item.Importe = RowsBusqueda(0).Item("ImporteB")
                Item.TipoIva = 5
                ListaIVA.Add(Item)
            End If
        End If
        '
        If Operacion = 1 Then
            For Each Row As DataRow In DtGrid.Rows
                RowsBusqueda = DtGrid.Select("Clave = " & Row("Clave"))
                If Row("Tipo") = 4 Then
                    Item = New ItemListaConceptosAsientos
                    Item.Clave = Row("Clave")
                    Item.Importe = Row("ImporteB")
                    Item.TipoIva = 9
                    If Item.Importe <> 0 Then ListaRetenciones.Add(Item)
                End If
            Next
        End If

        If Funcion = "A" Then
            If Not Asiento(801, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaListaParaNetoComisionDescargaEtc(ByRef ListaConceptos As List(Of ItemListaConceptosAsientos), ByVal Operacion As Integer) As Boolean

        Dim Neto As Decimal
        Dim ComisionW As Decimal
        Dim Comision As Decimal
        Dim Descarga As Decimal
        Dim FleteTerrestre As Decimal
        Dim OtrosConceptos As Decimal
        Dim RowsBusqueda() As DataRow

        If Operacion = 1 Then
            RowsBusqueda = DtGrid.Select("Clave = 1")
            Neto = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 2")
            Neto = Neto + RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 5")
            ComisionW = RowsBusqueda(0).Item("ImporteB")
            If Neto <> 0 Then Comision = RowsBusqueda(0).Item("ImporteB") * 100 / Neto
            RowsBusqueda = DtGrid.Select("Clave = 7")
            Descarga = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 9")
            FleteTerrestre = RowsBusqueda(0).Item("ImporteB")
            RowsBusqueda = DtGrid.Select("Clave = 10")
            OtrosConceptos = RowsBusqueda(0).Item("ImporteB")
        Else
            RowsBusqueda = DtGrid.Select("Clave = 2")
            Neto = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 5")
            ComisionW = RowsBusqueda(0).Item("ImporteN")
            If Neto <> 0 Then Comision = RowsBusqueda(0).Item("ImporteN") * 100 / Neto
            RowsBusqueda = DtGrid.Select("Clave = 7")
            Descarga = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 9")
            FleteTerrestre = RowsBusqueda(0).Item("ImporteN")
            RowsBusqueda = DtGrid.Select("Clave = 10")
            OtrosConceptos = RowsBusqueda(0).Item("ImporteN")
        End If

        Dim Item As New ItemListaConceptosAsientos
        Item.Clave = 202
        Item.Importe = Neto
        ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 208
        Item.Importe = ComisionW
        ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 207
        Item.Importe = Descarga
        ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 210
        Item.Importe = FleteTerrestre
        ListaConceptos.Add(Item)
        '
        Item = New ItemListaConceptosAsientos
        Item.Clave = 211
        Item.Importe = OtrosConceptos
        ListaConceptos.Add(Item)

        Return True

    End Function
    Private Sub Print_Print(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        'Not GridCompro.Columns(i).GetType().Equals(GetType(DataGridViewImageColumn)) para saber type de columna. puede ser util.

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 8
        Dim MTop As Integer = 20
        Dim Longi As Integer
        Dim Xq As Integer
        Dim Yq As Integer
        Dim Ancho As Integer
        Dim Alto As Integer
        Dim PrintFont As System.Drawing.Font = New Font("Courier New", 9, FontStyle.Regular)

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            PrintFont = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "N.V.L.P"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 160, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. Interno :  " & Format(Val(MaskedLiquidacion.Text), "00000000")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 15)
            Texto = "Fecha:  " & TextFechaContable.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 150, MTop + 15)
            '
            Texto = "Nro. N.V.L.P.:  " & Format(Val(MaskedReciboOficial.Text), "0000-00000000")
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 25)
            Texto = "Fecha N.V.L.P.:  " & TextFechaNVLP.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 90, MTop + 25)
            Texto = "Cliente : " & ComboEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 30)

            'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------
            x = MIzq
            y = MTop + 50
            Ancho = 195
            Alto = 95
            PrintFont = New Font("Courier New", 9)

            Dim LineaLote As Integer = x + 20
            Dim LineaRemito As Integer = x + 43
            Dim LineaArticulo As Integer = x + 84
            Dim LineaRemitido As Integer = x + 100
            Dim LineaMerma As Integer = x + 113
            Dim LineaCantidad As Integer = x + 125
            Dim LineaPrecio As Integer = x + 140
            Dim LineaImporte As Integer = x + 160
            Dim LineaPrecio2 As Integer = x + 175
            Dim LineaImporte2 As Integer = x + Ancho

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaLote, y, LineaLote, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaRemito, y, LineaRemito, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaArticulo, y, LineaArticulo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaRemitido, y, LineaRemitido, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaMerma, y, LineaMerma, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCantidad, y, LineaCantidad, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio, y, LineaPrecio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaPrecio2, y, LineaPrecio2, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, y + Alto)
            'Titulos de descripcion.
            Texto = "LOTE"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 5, y + 2)
            Texto = "REMITO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaRemito - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "ARTICULO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaArticulo - Longi - 15
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "REMIT."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaRemitido - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "MERMA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaMerma - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "CANTI."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCantidad - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "PRECIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaPrecio - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 2
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion de los lotes.
            Yq = y - SaltoLinea
            PrintFont = New Font("Courier New", 8)
            For Each Row As DataGridViewRow In GridCompro.Rows
                Yq = Yq + SaltoLinea
                'Imprime lote.
                Texto = "0/000"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaLote - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Remito.
                Texto = "0"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Articulo.
                Texto = Mid(Row.Cells("Articulo").FormattedValue, 1, 20)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Remitido.
                Texto = Row.Cells("Cantidad").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemitido - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime merma.
                Texto = "0"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaMerma - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Cantidad.
                Texto = Row.Cells("Cantidad").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaCantidad - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime precio.
                Texto = Row.Cells("Precio").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaPrecio - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime importe.
                Texto = Row.Cells("Importe").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            Next

            'Grafica -Rectangulo Conceptos. ----------------------------------------------------------------------
            x = MIzq + 40
            y = MTop + 155
            Ancho = 120
            Alto = 80
            PrintFont = New Font("Courier New", 9)

            Dim LineaConcepto As Integer = x + 40
            Dim LineaAlicuota As Integer = x + 60
            Dim LineaImporteC As Integer = x + 90
            Dim LineaImporteC2 As Integer = x + Ancho

            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaConcepto, y, LineaConcepto, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaAlicuota, y, LineaAlicuota, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporteC, y, LineaImporteC, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporteC2, y, LineaImporteC2, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, y + Alto)
            'Titulos de descripcion.
            Texto = "CONCEPTO"
            Xq = x
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq + 15, y + 2)
            Texto = "ALICUOTA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaAlicuota - Longi - 1
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporteC - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            If PermisoTotal Then
                Texto = "IMPORTE2"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporteC2 - Longi - 10
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            End If
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion de los concepto de cobro.
            Yq = y - SaltoLinea
            PrintFont = New Font("Courier New", 8)
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("ImporteB").Value <> 0 Then
                    Yq = Yq + SaltoLinea
                    'Imprime concepto.
                    Texto = Row.Cells("Nombre").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = x
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime alicuota.
                    Texto = Row.Cells("Alicuota").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaAlicuota - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime importeb.
                    Texto = Row.Cells("ImporteB").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporteC - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next

            Yq = MTop + 237

            PrintFont = New Font("Courier New", 10)
            Texto = TextTotalB.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporteC - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)

            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.HasMorePages = False
        End Try

    End Sub
    Private Sub Print_PrintCopia(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)

        Dim x As Integer
        Dim y As Integer
        Dim Texto As String
        Dim SaltoLinea As Integer = 4
        Dim MIzq As Integer = 10
        Dim MTop As Integer = 20

        Try
            e.Graphics.PageUnit = GraphicsUnit.Millimeter
            'Encabezado.
            Dim PrintFont As System.Drawing.Font = New Font("Courier New", 13, FontStyle.Bold)
            Texto = GNombreEmpresa
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop)
            Texto = "N.V.L.P"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 100, MTop)
            ' 
            PrintFont = New Font("Courier New", 12)
            Texto = "Nro. N.V.L.P.:  " & MaskedReciboOficial.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 20)
            Texto = "Fecha:  " & DateTime1.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 130, MTop + 20)
            Texto = "Nombre: " & ComboEmisor.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, MTop + 30)
            Texto = "Importe      : " & TextTotalB.Text
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 60, MTop + 40)

            Dim Longi As Integer
            Dim Xq As Integer
            Dim Yq As Integer
            Dim RowsBusqueda() As DataRow
            Dim Ancho As Integer
            Dim Alto As Integer

            'Grafica -Rectangulo medios de pagos. ----------------------------------------------------------------------
            x = MIzq
            y = MTop + 50
            Ancho = 180
            Alto = 95
            PrintFont = New Font("Courier New", 10)
            Dim LineaDescripcion As Integer = x + 40
            Dim LineaCambio As Integer = x + 55
            Dim LineaImporte As Integer = x + 85
            Dim LineaBanco As Integer = x + 125
            Dim LineaNumero As Integer = x + 154
            Dim LineaVencimiento As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaDescripcion, y, LineaDescripcion, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaCambio, y, LineaCambio, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte, y, LineaImporte, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaBanco, y, LineaBanco, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaNumero, y, LineaNumero, y + Alto)
            'Titulos de descripcion.
            Texto = "DESCRIPCION DEL PAGO"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "DESCRIPCION"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "CAMBIO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaCambio - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "BANCO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaBanco - Longi - 20
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "NUMERO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaNumero - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "VTO."
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaVencimiento - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            '------------------------------------------------------------------------------------------------------------
            'Descripcion del pago.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In Grid.Rows
                If IsNothing(Row.Cells("Concepto").Value) Then Exit For
                ''''''          RowsBusqueda = DtFormasPago.Select("Clave = " & Row.Cells("Concepto").Value)
                Yq = Yq + SaltoLinea
                'Imprime Detalle.
                Texto = Row.Cells("Concepto").FormattedValue
                Xq = x
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                If Row.Cells("Cambio").Value <> 0 Then
                    'Imprime Cambio.
                    Texto = Row.Cells("Cambio").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaCambio - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = FormatNumber(Trunca(Row.Cells("Cambio").Value * Row.Cells("Importe").Value), GDecimales)
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                Else
                    'Imprime Importe.
                    Texto = Row.Cells("Importe").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime Banco.
                Texto = Row.Cells("Banco").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Numero.
                Texto = Row.Cells("Numero").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaNumero - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Vencimeinto.
                Texto = Row.Cells("Fecha").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaVencimiento - Longi - 2
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Comprobante.
                If Row.Cells("Comprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("Comprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaNumero - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
                'Imprime FechaComprobante.
                If Row.Cells("FechaComprobante").FormattedValue <> "" Then
                    Texto = Row.Cells("FechaComprobante").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaVencimiento - Longi - 2
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Grafica -Rectangulo Imputacion. ----------------------------------------------------------------------
            y = MTop + 150
            Ancho = 183
            Alto = 50
            Dim LineaTipo As Integer = x + 35
            Dim LineaComprobante As Integer = x + 69
            Dim LineaFecha As Integer = x + 94
            Dim LineaImporte1 As Integer = x + 125
            Dim LineaSaldo As Integer = x + 155
            Dim LineaImporte2 As Integer = x + Ancho
            e.Graphics.DrawRectangle(New Pen(Color.Black, 0.1), x, y, Ancho, Alto)
            'Lineas vertical.
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaTipo, y, LineaTipo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaComprobante, y, LineaComprobante, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaFecha, y, LineaFecha, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte1, y, LineaImporte1, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaSaldo, y, LineaSaldo, y + Alto)
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), LineaImporte2, y, LineaImporte2, y + Alto)
            'Titulos de descripcion.
            Texto = "ASIGNACIONES"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, y - 4)
            Texto = "TIPO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq + 2, y + 2)
            Texto = "COMPROBANTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaComprobante - Longi - 6
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "FECHA"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaFecha - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPORTE"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte1 - Longi - 10
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "SALDO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaSaldo - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            Texto = "IMPUTADO"
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporte2 - Longi - 5
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            'linea horizontal.
            y = y + 2 * SaltoLinea
            e.Graphics.DrawLine(New Pen(Color.Black, 0.1), x, y, x + Ancho, y)
            'Descripcion Imputacion.
            Yq = y - SaltoLinea
            For Each Row As DataGridViewRow In GridCompro.Rows
                If Row.Cells("Asignado").Value <> 0 Then
                    Yq = Yq + SaltoLinea
                    'Imprime Tipo.
                    Texto = Row.Cells("Tipo").FormattedValue
                    Xq = x
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Comprobante.
                    Texto = Row.Cells("Recibo").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaComprobante - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Fecha.
                    Texto = Row.Cells("FechaCompro").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaFecha - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Importe.
                    Texto = Row.Cells("ImporteCompro").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte1 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime saldo.
                    Texto = Row.Cells("Saldo").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaSaldo - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime Asignado.
                    Texto = Row.Cells("Asignado").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte2 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
            Next
            'Final.
            Yq = MTop + 215
            PrintFont = New Font("Courier New", 10)
            Texto = "RECIBI LOS VALORES DESCRIPTOS PRECEDENTEMENTE  ........................... "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + SaltoLinea
            Texto = "                                                       F I R M A           "
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 2 * SaltoLinea
            Texto = "ACLARACION APELLIDO Y NOMBRE :............................................."
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 2 * SaltoLinea
            Texto = "DOCUMENTO  TIPO :...........  Nro.:........................................"
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Yq = Yq + 3 * SaltoLinea
            Texto = "A U T O R I Z O : ............................."
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, MIzq, Yq)
            Paginas = Paginas + 1

            If Paginas < Copias Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
            End If
        Catch ex As Exception
            MsgBox("Error " & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            e.HasMorePages = False
        End Try

    End Sub
    Private Function ExisteNVLP(ByVal NVLP As Double, ByVal Cliente As Integer) As Double

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(ReciboOficial) FROM NVLPCabeza WHERE Estado = 1 AND ReciboOficial = " & NVLP & " AND Cliente = " & Cliente & ";", Miconexion)
                    Return CDbl(Cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            Return -1
        End Try

    End Function
    Private Function Valida() As Boolean

        If PLiquidacion <> 0 Then
            If CDbl(TextTotalB.Text) <> 0 And DtLiquidacionCabezaB.Rows.Count = 0 Or CDbl(TextTotalB.Text) = 0 And DtLiquidacionCabezaB.Rows.Count <> 0 Then
                MsgBox("Para Realizar Esta Modificacion Debe Dar de Baja Esta Factura y Re-Hacerla. Operación se CANCELA.")
                Return False
            End If
        End If

        If GridCompro.Rows.Count = 0 Then
            MsgBox("Falta Informar Items a Liquidar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextTotalB.Text) < 0 Then
            MsgBox("Total Negativo", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row As DataGridViewRow In GridCompro.Rows
            If IsNothing(Row.Cells("Articulo").Value) Then Exit For
            If Row.Cells("Cantidad").Value = 0 Then
                MsgBox("Falta Cantidad en " & NombreArticulo(Row.Cells("Articulo").Value))
                Return False
            End If
            If Row.Cells("Precio").Value = 0 Then
                MsgBox("Falta Precio en " & NombreArticulo(Row.Cells("Articulo").Value))
                Return False
            End If
            If TieneDecimales(Row.Cells("Cantidad").Value) And Not Row.Cells("AGranel").Value Then
                MsgBox("Cantidad no debe tener Decimales en Articulo " & Row.Cells("Cantidad").FormattedValue, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

        If CDbl(TextTotalB.Text) = 0 Then
            MsgBox("Falta Informar Neto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        For i As Integer = 0 To Grid.Rows.Count - 1
            Dim ImporteB As Double = 0
            Dim ComisionB As Double = 0
            Dim DescargaB As Double = 0
            Dim FleteTerrestreB As Double = 0
            Dim OtrosConceptosB As Double = 0
            If Grid.Rows(i).Cells("Clave").Value = 1 Then ImporteB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 5 Then ComisionB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 7 Then DescargaB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 9 Then FleteTerrestreB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 10 Then OtrosConceptosB = Grid.Rows(i).Cells("ImporteB").Value
            If Grid.Rows(i).Cells("Clave").Value = 3 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And ImporteB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And ImporteB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 6 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And ComisionB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A. Comisión.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And ComisionB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Comisión.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 8 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And DescargaB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A. Descarga.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And DescargaB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Descarga.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 11 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And FleteTerrestreB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A. Flete Terrestre.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And FleteTerrestreB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Flete Terrestre.")
                    Return False
                End If
            End If
            If Grid.Rows(i).Cells("Clave").Value = 12 Then
                If Grid.Rows(i).Cells("Alicuota").Value <> 0 And Grid.Rows(i).Cells("ImporteB").Value = 0 And OtrosConceptosB <> 0 Then
                    MsgBox("Falta Informar Importe en I.V.A.Otros Conceptos.")
                    Return False
                End If
                If Grid.Rows(i).Cells("Alicuota").Value = 0 And Grid.Rows(i).Cells("ImporteB").Value <> 0 And OtrosConceptosB <> 0 Then
                    MsgBox("Falta Informar Alicuota en I.V.A. Otros Conceptos.")
                    Return False
                End If
            End If
            Select Case Grid.Rows(i).Cells("Clave").Value
                Case 5, 7, 9, 10
                    If Grid.Rows(i).Cells("Alicuota").Value <> 0 And (Grid.Rows(i).Cells("ImporteB").Value = 0) Then
                        MsgBox("Falta Informar Importe correspondiente al concepto de este IVA.")
                        Return False
                    End If
            End Select
        Next

        If Grid.Rows(0).Cells("ImporteB").Value = 0 And Grid.Rows(1).Cells("ImporteB").Value = 0 And CDbl(TextTotalB.Text) <> 0 Then
            MsgBox("Falta Informar Neto en Importe.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Grid.Focus()
            Return False
        End If

        If Val(MaskedReciboOficial.Text) = 0 Then
            MsgBox("Falta Informar numero de la N.V.L.P.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            MaskedReciboOficial.Focus()
            Return False
        End If

        If TextFechaNVLP.Text = "" Then
            MsgBox("Falta Informar Fecha N.V.L.P.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaNVLP.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaNVLP.Text) Then
            MsgBox("Fecha N.V.L.P. Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaNVLP.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaNVLP.Text, Date.Now) < 0 Then
            MsgBox("Fecha Factura Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaNVLP.Focus()
            Return False
        End If
        If TextFechaContable.Text = "" Then
            MsgBox("Falta Informar Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If Not ConsisteFecha(TextFechaContable.Text) Then
            MsgBox("Fecha Contable Incorrecta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If DiferenciaDias(TextFechaContable.Text, Date.Now) < 0 Then
            MsgBox("Fecha Contable Mayor a la Actual.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If
        If CierreContableCerrado(CDate(TextFechaContable.Text).Month, CDate(TextFechaContable.Text).Year) Then
            MsgBox("Fecha Contable en un Periodo Contable Cerrado.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            TextFechaContable.Focus()
            Return False
        End If

        If CDbl(TextTotalB.Text) <> 0 Then
            If Val(MaskedReciboOficial.Text) = 0 Then
                MsgBox("Falta Informar N.V.L.P. Proveedor. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                MaskedReciboOficial.Focus()
                Return False
            End If
            If Val(TipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000")) <> ReciboOficialAnt Then
                Select Case ExisteNVLP(Val(TipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000")), PCliente)
                    Case Is < 0
                        MsgBox("ERROR, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        MaskedReciboOficial.Focus()
                        Return False
                    Case Is > 0
                        MsgBox("N.V.L.P. Ya Exsite.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                        MaskedReciboOficial.Focus()
                        Return False
                End Select
            End If
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            Dim Imputado As Double = DtLiquidacionCabezaB.Rows(0).Item("Importe") - DtLiquidacionCabezaB.Rows(0).Item("Saldo")
            If CDbl(TextTotalB.Text) < Imputado Then
                MsgBox("Nuevo Importe de de la N.V.L.P. Supera importe Imputado en la Cuenta Corriente. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        Dim ImporteRetencion As Double = 0
        For Each Row1 As DataGridViewRow In Grid.Rows
            If Not IsNothing(Row1.Cells("Clave").Value) Then
                If Row1.Cells("TieneLupa").Value Then
                    ImporteRetencion = ImporteRetencion + Row1.Cells("ImporteB").Value
                End If
            End If
        Next
        If ImporteRetencion <> 0 Then
            If DtRetencionProvinciaAux.Rows.Count = 0 Then
                MsgBox("Falta Distribuir Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
            Dim ImporteDistribuido As Double
            For Each Row As DataRow In DtRetencionProvinciaAux.Rows
                ImporteDistribuido = ImporteDistribuido + Row("Importe")
            Next
            If ImporteDistribuido <> ImporteRetencion Then
                MsgBox("Incorrecta Distribución Retención Por Provincia. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Grid.Focus()
                Return False
            End If
        End If

        Return True

    End Function
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not Grid.CurrentRow.Cells("TieneLupa").Value Then Exit Sub
            If Grid.CurrentRow.Cells("ImporteB").Value = 0 Then
                MsgBox("Debe Informar Importe")
                Exit Sub
            End If
            If PLiquidacion <> 0 Then SeleccionarRetencionesProvincias.PFuncionBloqueada = True
            SeleccionarRetencionesProvincias.PDtGrid = DtRetencionProvinciaAux
            SeleccionarRetencionesProvincias.PImporte = Grid.CurrentRow.Cells("ImporteB").Value
            SeleccionarRetencionesProvincias.PRetencion = Grid.CurrentRow.Cells("Clave").Value
            SeleccionarRetencionesProvincias.ShowDialog()
            SeleccionarRetencionesProvincias.Dispose()
        End If

    End Sub
    Private Sub Grid_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                If Grid.Rows(e.RowIndex).Cells("Clave").Value = 1 Or Grid.Rows(e.RowIndex).Cells("Clave").Value = 2 Or Grid.Rows(e.RowIndex).Cells("Clave").Value = 3 Then
                    e.Value = FormatNumber(e.Value, GDecimales)
                Else
                    e.Value = "-" & FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Alicuota" Then
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else : e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Lupa" Then
            If Not IsDBNull(Grid.Rows(e.RowIndex).Cells("TieneLupa").Value) Then
                If Grid.Rows(e.RowIndex).Cells("TieneLupa").Value Then
                    e.Value = ImageList1.Images.Item("Lupa")
                Else : e.Value = Nothing
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If e.KeyChar = "." Then e.KeyChar = ","
        If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Nombre" Then Exit Sub

        If CType(sender, TextBox).Text <> "" Then
            EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
        End If

    End Sub
    Private Sub DtGrid_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If e.Column.ColumnName.Equals("Iva") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue > 100 Then e.ProposedValue = 0
            For Each Item As Double In TablaIva
                If Item = e.ProposedValue Then Exit Sub
            Next
            MsgBox("Alicuota no Existe.")
            e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("ImporteB") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
        End If


    End Sub
    Private Sub DtGrid_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row("ImporteB") = 0 Then
            For I As Integer = DtRetencionProvinciaAux.Rows.Count - 1 To 0 Step -1
                If DtRetencionProvinciaAux.Rows(I).Item("Retencion") = e.Row("Clave") Then DtRetencionProvinciaAux.Rows(I).Delete()
            Next
        End If

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRIDCOMPRO de comprobantes.  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub ButtonEliminarLinea_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminarLinea.Click

        If GridCompro.Rows.Count = 1 Then
            MsgBox("No hay un registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        GridCompro.Rows.Remove(GridCompro.CurrentRow)

        CalculaTotal()

    End Sub
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        'para manejo del autocoplete de articulos.
        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub GridCompro_CellLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellLeave

        'para manejo del autocoplete de articulos.
        If GridCompro.Columns(e.ColumnIndex).Name = "Articulo" Then
            If Not cb Is Nothing Then
                cb.SelectedIndex = cb.FindStringExact(cb.Text)
            End If
        End If

    End Sub
    Private Sub GridCompro_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles GridCompro.CellBeginEdit

        If GridCompro.Columns(e.ColumnIndex).Name = "Precio" Or GridCompro.Columns(e.ColumnIndex).Name = "Cantidad" Then
            If Not IsDBNull(GridCompro.Rows(e.RowIndex).Cells("Articulo").Value) Then
                If GridCompro.CurrentRow.Cells("Articulo").Value = 0 Then e.Cancel = True
            End If
        End If

    End Sub
    Private Sub GridCompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        Dim Row As DataGridViewRow = GridCompro.CurrentRow

        If GridCompro.Columns(e.ColumnIndex).Name = "Articulo" Then            'Completa el grid con datos para ser utilizados en el Recalculo de cantidad.
            HallaAGranelYMedida(Row.Cells("Articulo").Value, Row.Cells("AGranel").Value, Row.Cells("Medida").Value)
        End If

        CalculaTotal()

    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        'para manejo del autocoplete de articulos.
        If TypeOf e.Control Is ComboBox Then
            cb = e.Control
            cb.DropDownStyle = ComboBoxStyle.DropDown
            cb.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cb.AutoCompleteSource = AutoCompleteSource.ListItems
            Exit Sub
        End If

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKeyCompro_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChangedCompro_TextChanged

    End Sub
    Private Sub ValidaKeyCompro_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Cantidad" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Iva" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChangedCompro_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Cantidad" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 3)
            End If
        End If

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Iva" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If IsNumeric(e.Value) Then
            If e.Value = 0 Then e.Value = Format(0, "#") : Exit Sub
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Iva" Then
            If Not IsDBNull(GridCompro.Rows(e.RowIndex).Cells("Iva").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Precio" Then
            If Not IsDBNull(GridCompro.Rows(e.RowIndex).Cells("Precio").Value) Then
                e.Value = FormatNumber(e.Value, 3)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Importe" Then
            If Not IsDBNull(GridCompro.Rows(e.RowIndex).Cells("Importe").Value) Then
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

    End Sub
    Private Sub DtGridCompro_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        IndiceW = IndiceW + 1
        e.Row("Indice") = IndiceW
        e.Row("Articulo") = 0
        e.Row("Iva") = 0
        e.Row("Importe") = 0
        e.Row("Precio") = 0
        e.Row("Cantidad") = 0

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If (e.Column.ColumnName.Equals("Articulo")) Then
            If Not IsDBNull(e.Row("Articulo")) Then
                Dim Kilos As Double
                Dim Iva As Double
                HallaKilosIva(e.ProposedValue, Kilos, Iva)
                e.Row("Iva") = Iva
            End If
        End If

        If (e.Column.ColumnName.Equals("Cantidad")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If Not IsDBNull(e.Row("Precio")) Then
                e.Row("Importe") = CalculaNeto(e.Row("Precio"), e.ProposedValue)
            End If
        End If

        If (e.Column.ColumnName.Equals("Precio")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca3(e.ProposedValue)
            If Not IsDBNull(e.Row("Cantidad")) Then
                e.Row("Importe") = CalculaNeto(e.Row("Cantidad"), e.ProposedValue)
            End If
        End If

        If e.Column.ColumnName.Equals("Iva") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue > 100 Then e.ProposedValue = 0
            For Each Item As Double In TablaIva
                If Item = e.ProposedValue Then Exit Sub
            Next
            MsgBox("Alicuota no Existe.")
            e.ProposedValue = 0
        End If

        GridCompro.Refresh()

    End Sub
    Private Sub DtGridCompro_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        'Borra las lineas en blancos que aparecen cuando hago click en el ultimo renglon y sin informar nada regreso a algun renglon anterior. 

        If GridCompro.Rows.Count > 1 Then
            For Each Row As DataRow In DtGridCompro.Rows
                If Row("Iva") <> e.Row("Iva") Then
                    MsgBox("%IVA de los Articulos deben ser iguales.", MsgBoxStyle.Information)
                    e.Row.Delete()
                    Exit Sub
                End If
            Next
        End If

        If e.Row("Articulo") = 0 And e.Row("Cantidad") = 0 And e.Row("Precio") = 0 Then e.Row.Delete()

    End Sub
    Private Sub Gridcompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs)
        Exit Sub
    End Sub

End Class