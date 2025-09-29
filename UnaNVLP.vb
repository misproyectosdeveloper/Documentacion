Imports System.Transactions
Imports System.Drawing.Printing
Imports System.IO
Public Class UnaNVLP
    Public PLiquidacion As Double
    Public PCliente As Integer
    Public PAbierto As Boolean
    Public PBloqueaFunciones As Boolean
    Public PtipoFactura As Integer
    '
    Private MiEnlazador As New BindingSource
    '
    Dim ListaDeLotes As List(Of FilaComprobanteFactura)
    '
    Dim DtLiquidacionCabezaB As DataTable
    Dim DtLiquidacionDetalleB As DataTable
    Dim DtLiquidacionLotesB As DataTable
    Dim DtLiquidacionCabezaN As DataTable
    Dim DtLiquidacionDetalleN As DataTable
    Dim DtLiquidacionLotesN As DataTable
    Dim DtGrid As DataTable
    Dim DtGridCompro As DataTable
    Dim DtRetencionProvincia As DataTable
    Dim DtRetencionProvinciaAux As DataTable
    Dim RegCabezaAnt As DataRow
    Dim ClienteOpr As Boolean
    '
    Dim TablaIva(0) As Double
    Dim IvaW As Double
    Dim ConexionLiquidacion As String
    Dim UltimaFechaContableW As DateTime
    Dim ReciboOficialAnt As Decimal
    'Para impresion.
    Dim Paginas As Integer = 0
    Dim Copias As Integer = 0
    Dim HayFilasErroneas As Boolean
    Public Class ItemLotesDelRemito
        Private IntIndice As Integer
        Private IntOperacionLote As Integer
        Private IntLote As Integer
        Private IntSecuencia As Integer
        Private IntArticulo As Integer
        Private DecCantidad As Decimal
        Private DecPrecioB As Decimal
        Private DecPrecioN As Decimal
        Private DecRemito As Decimal
        Private DecLiquidado As Decimal
        Private IntDeposito As Integer
        Public Sub New()
        End Sub
        Public Property Indice() As Integer
            Get
                Return IntIndice
            End Get
            Set(ByVal value As Integer)
                IntIndice = value
            End Set
        End Property
        Public Property OperacionLote() As Integer
            Get
                Return IntOperacionLote
            End Get
            Set(ByVal value As Integer)
                IntOperacionLote = value
            End Set
        End Property
        Public Property Lote() As Integer
            Get
                Return IntLote
            End Get
            Set(ByVal value As Integer)
                IntLote = value
            End Set
        End Property
        Public Property Secuencia() As Integer
            Get
                Return IntSecuencia
            End Get
            Set(ByVal value As Integer)
                IntSecuencia = value
            End Set
        End Property
        Public Property Articulo() As Integer
            Get
                Return IntArticulo
            End Get
            Set(ByVal value As Integer)
                IntArticulo = value
            End Set
        End Property
        Public Property Cantidad() As Decimal
            Get
                Return DecCantidad
            End Get
            Set(ByVal value As Decimal)
                DecCantidad = value
            End Set
        End Property
        Public Property PrecioB() As Decimal
            Get
                Return DecPrecioB
            End Get
            Set(ByVal value As Decimal)
                DecPrecioB = value
            End Set
        End Property
        Public Property PrecioN() As Decimal
            Get
                Return DecPrecioN
            End Get
            Set(ByVal value As Decimal)
                DecPrecioN = value
            End Set
        End Property
        Public Property Remito() As Decimal
            Get
                Return DecRemito
            End Get
            Set(ByVal value As Decimal)
                DecRemito = value
            End Set
        End Property
        Public Property Liquidado() As Decimal
            Get
                Return DecLiquidado
            End Get
            Set(ByVal value As Decimal)
                DecLiquidado = value
            End Set
        End Property
        Public Property Deposito() As Integer
            Get
                Return IntDeposito
            End Get
            Set(ByVal value As Integer)
                IntDeposito = value
            End Set
        End Property
    End Class
    Private Sub UnaNVLP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Me.Top = 50

        If Not PermisoEscritura(6) Then PBloqueaFunciones = True

        If PAbierto And EsContable(PLiquidacion) Then
            UnaNVLPContable.PLiquidacion = PLiquidacion
            UnaNVLPContable.ShowDialog()
            Me.Close()
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            Opciones()
            If PCliente = 0 Then Me.Close() : Exit Sub
        End If

        GridCompro.Columns("Candado").DefaultCellStyle.NullValue = Nothing
        GridCompro.Columns("CandadoRemito").DefaultCellStyle.NullValue = Nothing

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

        If PAbierto Then
            ConexionLiquidacion = Conexion
        Else
            ConexionLiquidacion = ConexionN
        End If

        LlenaCombosGrid()
        ArmaTablaIva(TablaIva)

        ArmaArchivos()

        GModificacionOk = False

        If Not PermisoTotal Then
            Grid.Columns("ImporteN").Visible = False
            TextTotalN.Visible = False
            GridCompro.Columns("Precio2").Visible = False
            GridCompro.Columns("Importe2").Visible = False
        End If

        UltimaFechaContableW = UltimaFechacontable(Conexion, 1)
        If UltimaFechaContableW = "2/1/1000" Then
            MsgBox("Error Base de Datos al Leer Ultima Fecha Contable.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Me.Close()
        End If

    End Sub
    Private Sub UnaNVLP_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        If PLiquidacion <> 0 Then GridCompro.Focus()

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
        Dim DtLiquidacionLotesBAux As DataTable = DtLiquidacionLotesB.Copy
        Dim DtLiquidacionCabezaNAux As DataTable = DtLiquidacionCabezaN.Copy
        Dim DtLiquidacionDetalleNAux As DataTable = DtLiquidacionDetalleN.Copy
        Dim DtLiquidacionLotesNAux As DataTable = DtLiquidacionLotesN.Copy
        Dim DtRemitosB As New DataTable
        Dim DtRemitosN As New DataTable
        Dim DtLotesB As New DataTable
        Dim DtLotesN As New DataTable

        If PLiquidacion = 0 Then
            If Not ActualizaArchivos("A", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtRemitosB, DtRemitosN, DtLotesB, DtLotesN) Then Exit Sub
        Else
            If Not ActualizaArchivos("M", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtRemitosB, DtRemitosN, DtLotesB, DtLotesN) Then Exit Sub
        End If

        'Arma Asientos.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable
        '
        If GGeneraAsiento And PLiquidacion = 0 Then
            If Not Tablas.Read("SELECT * FROM AsientosCabeza WHERE Asiento = 0;", Conexion, DtAsientoCabezaB) Then Exit Sub
            If Not Tablas.Read("SELECT * FROM AsientosDetalle WHERE Asiento = 0;", Conexion, DtAsientoDetalleB) Then Exit Sub
            DtAsientoCabezaN = DtAsientoCabezaB.Clone
            DtAsientoDetalleN = DtAsientoDetalleB.Clone
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaB, DtAsientoDetalleB, 1) Then Exit Sub
            End If
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                If Not ArmaArchivosAsiento("A", DtAsientoCabezaN, DtAsientoDetalleN, 2) Then Exit Sub
            End If
        End If
        If PLiquidacion <> 0 Then
            If Format(RegCabezaAnt.Item("FechaContable"), "dd/MM/yyyy") <> TextFechaContable.Text Then
                If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                    If Not HallaAsientosCabeza(800, DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaB, Conexion) Then Exit Sub
                    If DtAsientoCabezaB.Rows.Count <> 0 Then
                        Dim IntFecha As Integer = DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Year & Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Month, "00") & Format(DtLiquidacionCabezaBAux.Rows(0).Item("FechaContable").Day, "00")
                        DtAsientoCabezaB.Rows(0).Item("IntFecha") = IntFecha
                    End If
                End If
                If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                    If Not HallaAsientosCabeza(800, DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaN, ConexionN) Then Exit Sub
                    If DtAsientoCabezaN.Rows.Count <> 0 Then
                        Dim IntFecha As Integer = DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable").Year & Format(DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable").Month, "00") & Format(DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable").Day, "00")
                        DtAsientoCabezaN.Rows(0).Item("IntFecha") = IntFecha
                    End If
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
                Row1("Provincia") = Row("Provincia")
                Row1("Retencion") = Row("Retencion")
                Row1("Comprobante") = Row("Comprobante")
                Row1("Importe") = Row("Importe")
                DtRetencionProvinciaWW.Rows.Add(Row1)
            Next
        End If

        If IsNothing(DtLiquidacionCabezaBAux.GetChanges) And IsNothing(DtLiquidacionCabezaNAux.GetChanges) And _
           IsNothing(DtLiquidacionDetalleBAux.GetChanges) And IsNothing(DtLiquidacionDetalleNAux.GetChanges) And _
           IsNothing(DtLiquidacionLotesBAux.GetChanges) And IsNothing(DtLiquidacionLotesNAux.GetChanges) Then
            MsgBox("NO Hay cambios. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        If PLiquidacion = 0 Then
            HacerAlta(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtLotesB, DtLotesN, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtRetencionProvinciaWW)
        Else
            Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtLotesB, DtLotesN, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtRetencionProvinciaWW)
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

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Exit Sub
            End If
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
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then
            If DtLiquidacionCabezaN.Rows(0).Item("Saldo") <> DtLiquidacionCabezaN.Rows(0).Item("Importe") Then
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
        Dim DtLiquidacionLotesBAux As DataTable = DtLiquidacionLotesB.Copy
        Dim DtLiquidacionCabezaNAux As DataTable = DtLiquidacionCabezaN.Copy
        Dim DtLiquidacionDetalleNAux As DataTable = DtLiquidacionDetalleN.Copy
        Dim DtLiquidacionLotesNAux As DataTable = DtLiquidacionLotesN.Copy
        Dim DtRemitosB As New DataTable
        Dim DtRemitosN As New DataTable
        Dim DtLotesB As New DataTable
        Dim DtLotesN As New DataTable

        'Anula asiento.
        Dim DtAsientoCabezaB As New DataTable
        Dim DtAsientoDetalleB As New DataTable
        Dim DtAsientoCabezaN As New DataTable
        Dim DtAsientoDetalleN As New DataTable

        If GGeneraAsiento Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(800, DtLiquidacionCabezaBAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaB, Conexion) Then Exit Sub
            End If
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                If Not HallaAsientosCabeza(800, DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion"), DtAsientoCabezaN, ConexionN) Then Exit Sub
            End If
            If DtAsientoCabezaB.Rows.Count <> 0 Then DtAsientoCabezaB.Rows(0).Item("Estado") = 3
            If DtAsientoCabezaN.Rows.Count <> 0 Then DtAsientoCabezaN.Rows(0).Item("Estado") = 3
        End If

        If MsgBox("Liquidación se Anulara. ¿Desea Anularla?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
            Exit Sub
        End If

        If Not ActualizaArchivos("B", DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtRemitosB, DtRemitosN, DtLotesB, DtLotesN) Then Exit Sub

        Dim DtRetencionProvinciaWW As DataTable = DtRetencionProvincia.Clone

        Dim Resul As Double = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionLotesNAux, DtLotesB, DtLotesN, DtRemitosB, DtRemitosN, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtRetencionProvinciaWW)
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
        UnaNVLP_Load(Nothing, Nothing)

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

        ListaAsientos.PTipoDocumento = 800
        If DtLiquidacionCabezaB.Rows.Count <> 0 Then ListaAsientos.PDocumentoB = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then ListaAsientos.PDocumentoN = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")
        ListaAsientos.Show()

    End Sub
    Private Sub ButtonNetoPorLotes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonNetoPorLotes.Click

        If PLiquidacion = 0 Then
            MsgBox("Opcion Invalida. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        SeleccionarVarios.PEsNetoPorLotesNVLP = True
        SeleccionarVarios.PLiquidacion = PLiquidacion
        SeleccionarVarios.PAbierto = PAbierto
        SeleccionarVarios.ShowDialog()
        SeleccionarVarios.Dispose()

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
    Private Sub PictureLupa_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureLupa.Click

        ListaDeLotes = New List(Of FilaComprobanteFactura)

        For Each Row As DataRow In DtGridCompro.Rows
            Dim Item As New FilaComprobanteFactura
            Item.Operacion = Row("Operacion")
            Item.Indice = Row("Indice")
            Item.Lote = Row("Lote")
            Item.Secuencia = Row("Secuencia")
            Item.Deposito = Row("Deposito")
            Item.Remito = Row("Remito")
            ListaDeLotes.Add(Item)
        Next

        SeleccionarVarios.PEsNVLP = True
        SeleccionarVarios.PEmisor = PCliente
        SeleccionarVarios.PListaDeLotes = ListaDeLotes
        SeleccionarVarios.ShowDialog()

        AgregaLotesDelGrid(SeleccionarVarios.PListaDeLotes, False)

        SeleccionarVarios.Dispose()

    End Sub
    Private Sub ButtonImportar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImportar.Click

        If PLiquidacion <> 0 Then
            MsgBox("Error. NVLP ya esta Grabada. OPERACION SE CANCELA.") : Exit Sub
        End If

        OpcionDirectorio.ShowDialog()
        If OpcionDirectorio.PRegresar Then OpcionDirectorio.Dispose() : Exit Sub
        Dim Path As String = OpcionDirectorio.PPath
        Dim Archivo As String = OpcionDirectorio.PFile
        Dim Extencion As String = OpcionDirectorio.PExtencion
        OpcionDirectorio.Dispose()

        Dim Dt As DataTable = DtGridCompro.Copy

        Dim Lista As New List(Of FilaComprobanteFactura)
        Dim Linea As String
        Dim MiArray() As String

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Using reader As StreamReader = New StreamReader(Path)
            Linea = reader.ReadLine
            MiArray = Split(Linea, ";")

            If UBound(MiArray) = 0 Then
                MsgBox("No Se Encontro Datos en Archivo.", MsgBoxStyle.Critical) : Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
            If MiArray(0) = "C" Then
                MaskedReciboOficial.Text = Strings.Right(MiArray(1), 12)
                TextFechaNVLP.Text = MiArray(2)
                If CuitNumerico(GCuitEmpresa) <> MiArray(3) Then
                    MsgBox("Cuit Empresa no se Corresponde con archivo de Importación.") : Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
                If MiArray(10) <> CuitNumerico(TextCuit.Text) Then
                    MsgBox("Cuit Cliente no se Corresponde con archivo de Importación.") : Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                End If
            Else
                MsgBox("Error al leer archivo .txt o no corresponde a una Importación.") : Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
            End If
            Dim RowsBusqueda2() As DataRow
            RowsBusqueda2 = DtGrid.Select("Clave = 5")
            RowsBusqueda2(0).Item("ImporteB") = MiArray(4)
            RowsBusqueda2(0).Item("ImporteN") = MiArray(5)
            RowsBusqueda2 = DtGrid.Select("Clave = 6")
            RowsBusqueda2(0).Item("Iva") = MiArray(6)
            RowsBusqueda2 = DtGrid.Select("Clave = 7")
            RowsBusqueda2(0).Item("ImporteB") = MiArray(7)
            RowsBusqueda2(0).Item("ImporteN") = MiArray(8)
            RowsBusqueda2 = DtGrid.Select("Clave = 8")
            RowsBusqueda2(0).Item("Iva") = MiArray(9)
        End Using

        Dim ConexionRemito As String = ""
        Dim OperacionRemito As String = 0
        Dim RemitoAnt As Decimal = 0
        Dim DtRemitoDetalle As New DataTable

        Using reader As StreamReader = New StreamReader(Path)
            Linea = reader.ReadLine
            Do While (Not Linea Is Nothing)
                MiArray = Split(Linea, ";")
                If MiArray(0) = "C" Then Linea = reader.ReadLine : Continue Do
                'Halla conexion Remito.
                If MiArray(3) <> RemitoAnt Then
                    HallaDatosRemito(MiArray(3), ComboEmisor.SelectedValue, ConexionRemito, OperacionRemito, DtRemitoDetalle)
                    If OperacionRemito = 0 Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                    RemitoAnt = MiArray(3)
                End If
                'Halla datos de Lotes en el remito.
                Dim ListaLotes As New List(Of ItemLotesDelRemito)
                If Not ProcesaDetalleImportacion(MiArray(1), MiArray(3), MiArray(2), MiArray(5), MiArray(6), ListaLotes, ConexionRemito, OperacionRemito, _
                               DtRemitoDetalle, HayFilasErroneas) Then Me.Cursor = System.Windows.Forms.Cursors.Default : Exit Sub
                ' Agrega a la fila.
                For Each Fila2 As ItemLotesDelRemito In ListaLotes
                    Dim Fila As New FilaComprobanteFactura
                    Fila.Operacion = Fila2.OperacionLote
                    Fila.OperacionRemito = OperacionRemito
                    Fila.Indice = Fila2.Indice
                    Fila.Lote = Fila2.Lote
                    Fila.Secuencia = Fila2.Secuencia
                    Fila.Articulo = Fila2.Articulo
                    Fila.Deposito = Fila2.Deposito
                    Fila.Remito = Fila2.Remito
                    Fila.Cantidad = Fila2.Liquidado
                    Fila.ImporteB = Fila2.PrecioB
                    Fila.ImporteN = Fila2.PrecioN
                    Fila.Ingreso = Fila2.Cantidad  'Aqui pongo la cantidad liquidada en empresa asociada. 
                    If HayFilasErroneas Then Fila.Ingreso = 0 'Pongo 0 para que salga cartel de error y se colore la grilla de rojo en el remito con errores.
                    Lista.Add(Fila)
                Next
                Linea = reader.ReadLine
            Loop
            reader.Dispose()
        End Using

        AgregaLotesDelGrid(Lista, True)

        Dim RowsBusqueda() As DataRow

        For Each Fila As FilaComprobanteFactura In Lista
            RowsBusqueda = DtGridCompro.Select("Remito = " & Fila.Remito & " AND Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia)
            RowsBusqueda(0).Item("PrecioB") = Fila.ImporteB
            RowsBusqueda(0).Item("PrecioN") = Fila.ImporteN
        Next

        Lista = Nothing

        CalculaTotal()

        Me.Cursor = System.Windows.Forms.Cursors.Default

        GridCompro.Refresh()

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
    Private Sub ArmaArchivos()                    'ArmaArchivos

        CreaDtRetencionProvinciaAux()

        ArmaGrid()

        CreaDtGridCompro()

        DtLiquidacionCabezaB = New DataTable
        DtLiquidacionCabezaN = New DataTable
        DtLiquidacionDetalleB = New DataTable
        DtLiquidacionDetalleN = New DataTable
        DtLiquidacionLotesB = New DataTable
        DtLiquidacionLotesN = New DataTable

        If PLiquidacion <> 0 Then
            If PAbierto Then
                If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaB.Rows.Count = 0 Then
                    MsgBox("Liquidacion No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
                If Not Tablas.Read("SELECT * FROM NVLPLotes WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionLotesB) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaB.Rows(0).Item("Rel") And PermisoTotal Then
                    If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Nrel = " & PLiquidacion & ";", ConexionN, DtLiquidacionCabezaN) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion= " & DtLiquidacionCabezaN.Rows(0).Item("Liquidacion") & ";", ConexionN, DtLiquidacionDetalleN) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM NVLPLotes WHERE Liquidacion= " & DtLiquidacionCabezaN.Rows(0).Item("Liquidacion") & ";", ConexionN, DtLiquidacionLotesN) Then Me.Close() : Exit Sub
                Else
                    DtLiquidacionCabezaN = DtLiquidacionCabezaB.Clone
                    DtLiquidacionDetalleN = DtLiquidacionDetalleB.Clone
                    DtLiquidacionLotesN = DtLiquidacionLotesB.Clone
                End If
                RegCabezaAnt = DtLiquidacionCabezaB.Rows(0)
            Else
                If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionCabezaN) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaN.Rows.Count = 0 Then
                    MsgBox("Liquidacion No Existe En Base De Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Me.Close() : Exit Sub
                End If
                If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionDetalleN) Then Me.Close() : Exit Sub
                If Not Tablas.Read("SELECT * FROM NVLPLotes WHERE Liquidacion = " & PLiquidacion & ";", ConexionN, DtLiquidacionLotesN) Then Me.Close() : Exit Sub
                If DtLiquidacionCabezaN.Rows(0).Item("Rel") Then
                    If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Liquidacion = " & DtLiquidacionCabezaN.Rows(0).Item("NRel") & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion = " & DtLiquidacionCabezaB.Rows(0).Item("Liquidacion") & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
                    If Not Tablas.Read("SELECT * FROM NVLPLotes WHERE Liquidacion = " & DtLiquidacionCabezaB.Rows(0).Item("Liquidacion") & ";", Conexion, DtLiquidacionLotesB) Then Me.Close() : Exit Sub
                Else
                    DtLiquidacionCabezaB = DtLiquidacionCabezaN.Clone
                    DtLiquidacionDetalleB = DtLiquidacionDetalleN.Clone
                    DtLiquidacionLotesB = DtLiquidacionLotesN.Clone
                End If
                RegCabezaAnt = DtLiquidacionCabezaN.Rows(0)
            End If
        End If

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            PCliente = DtLiquidacionCabezaB.Rows(0).Item("Cliente")
        Else
            If DtLiquidacionCabezaN.Rows.Count <> 0 Then
                PCliente = DtLiquidacionCabezaN.Rows(0).Item("Cliente")
            End If
        End If

        If Not LlenaDatosCliente(PCliente) Then Me.Close() : Exit Sub

        If PLiquidacion = 0 Then
            If Not Tablas.Read("SELECT * FROM NVLPCabeza WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionCabezaB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM NVLPDetalle WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionDetalleB) Then Me.Close() : Exit Sub
            If Not Tablas.Read("SELECT * FROM NVLPLotes WHERE Liquidacion = " & PLiquidacion & ";", Conexion, DtLiquidacionLotesB) Then Me.Close() : Exit Sub
            DtLiquidacionCabezaN = DtLiquidacionCabezaB.Clone
            DtLiquidacionDetalleN = DtLiquidacionDetalleB.Clone
            DtLiquidacionLotesN = DtLiquidacionLotesB.Clone
            '
            Dim Row As DataRow = DtLiquidacionCabezaB.NewRow
            ArmaNuevaNVLP(Row)
            Row("ReciboOficial") = CDbl(PtipoFactura & Format("000000000000"))
            Row("Cliente") = PCliente
            Row("Fecha") = Now.Date
            DtLiquidacionCabezaB.Rows.Add(Row)
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
        For Each Row As DataRow In DtLiquidacionDetalleN.Rows
            RowsBusqueda = DtGrid.Select("Clave = " & Row("Impuesto"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("ImporteN") = Row("Importe")
                If RowsBusqueda(0).Item("Iva") = 0 Then RowsBusqueda(0).Item("Iva") = Row("Alicuota")
            End If
        Next

        For Each Row As DataRow In DtLiquidacionLotesB.Rows
            Dim Row1 As DataRow = DtGridCompro.NewRow
            Row1("Indice") = Row("Indice")
            Row1("Lote") = Row("Lote")
            Row1("Secuencia") = Row("Secuencia")
            Row1("Deposito") = Row("Deposito")
            Row1("Operacion") = Row("Operacion")
            Row1("Remito") = Row("Remito")
            Row1("OperacionRemito") = Row("OPR")
            Row1("Remitido") = Row("Remitido")
            Row1("Merma") = Row("Merma")
            Row1("Cantidad") = Row("Cantidad")
            If Row("MermaAux") < 0 Then                  'Muestra La merma y cantidad ficticia.
                Row1("MermaFicticia") = Row("MermaAux")
                Row1("CantidadFicticia") = Trunca(Row("Cantidad") - Row("MermaAux"))
            Else
                Row1("MermaFicticia") = Row("Merma")
                Row1("CantidadFicticia") = Row("Cantidad")
            End If
            Row1("PrecioB") = Row("Precio")
            Row1("ImporteB") = Row("Importe")
            Row1("PrecioN") = 0
            Row1("ImporteN") = 0
            If Row("Operacion") = 1 Then
                Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
            Else : Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
            End If
            Dim RowsBusquedaA() As DataRow
            RowsBusquedaA = Articulo.DataSource.select("Clave = " & Row1("Articulo"))
            Row1("NombreArticulo") = RowsBusquedaA(0).Item("Nombre")
            Row1("Iva") = HallaIva(Row1("Articulo"))
            Dim AGranel As Boolean
            Row1("Medida") = ""
            HallaAGranelYMedida(Row1("Articulo"), AGranel, Row1("Medida"))
            DtGridCompro.Rows.Add(Row1)
        Next
        For Each Row As DataRow In DtLiquidacionLotesN.Rows
            RowsBusqueda = DtGridCompro.Select("Indice = " & Row("Indice") & "AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & " AND Remito = " & Row("Remito"))
            If RowsBusqueda.Length <> 0 Then
                RowsBusqueda(0).Item("PrecioN") = Row("Precio")
                RowsBusqueda(0).Item("ImporteN") = Row("Importe")
            Else
                Dim Row1 As DataRow = DtGridCompro.NewRow
                Row1("Indice") = Row("Indice")
                Row1("Lote") = Row("Lote")
                Row1("Secuencia") = Row("Secuencia")
                Row1("Deposito") = Row("Deposito")
                Row1("Operacion") = Row("Operacion")
                Row1("Remito") = Row("Remito")
                Row1("OperacionRemito") = Row("OPR")
                Row1("Remitido") = Row("Remitido")
                Row1("Merma") = Row("Merma")
                Row1("Cantidad") = Row("Cantidad")
                If Row("MermaAux") < 0 Then
                    Row1("MermaFicticia") = Row("MermaAux")
                    Row1("CantidadFicticia") = Trunca(Row("Cantidad") - Row("MermaAux"))
                Else
                    Row1("MermaFicticia") = Row("Merma")
                    Row1("CantidadFicticia") = Row("Cantidad")
                End If
                Row1("PrecioB") = 0
                Row1("ImporteB") = 0
                Row1("PrecioN") = Row("Precio")
                Row1("ImporteN") = Row("Importe")
                If Row("Operacion") = 1 Then
                    Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), Conexion)
                Else : Row1("Articulo") = HallaArticulo(Row("Lote"), Row("Secuencia"), ConexionN)
                End If
                Row1("NombreArticulo") = NombreArticulo(Row1("Articulo"))
                Row1("Iva") = HallaIva(Row1("Articulo"))
                Dim AGranel As Boolean
                Row1("Medida") = ""
                HallaAGranelYMedida(Row1("Articulo"), AGranel, Row1("Medida"))
                DtGridCompro.Rows.Add(Row1)
            End If
        Next

        GridCompro.DataSource = DtGridCompro

        If PLiquidacion = 0 Then
            PictureLupa.Enabled = True
            GridCompro.ReadOnly = False
            Grid.ReadOnly = False
            MaskedLiquidacionN.Visible = False
            MaskedLiquidacion.Text = Nothing
        Else
            If DtLiquidacionCabezaB.Rows.Count <> 0 And DtLiquidacionCabezaN.Rows.Count <> 0 Then
                MaskedLiquidacion.Text = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
                MaskedLiquidacionN.Text = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")
                MaskedLiquidacionN.Visible = True
            End If
            If DtLiquidacionCabezaB.Rows.Count <> 0 And DtLiquidacionCabezaN.Rows.Count = 0 Then
                MaskedLiquidacion.Text = DtLiquidacionCabezaB.Rows(0).Item("Liquidacion")
                MaskedLiquidacionN.Visible = False
                Grid.Columns("ImporteN").ReadOnly = True
            End If
            If DtLiquidacionCabezaB.Rows.Count = 0 And DtLiquidacionCabezaN.Rows.Count <> 0 Then
                MaskedLiquidacion.Text = DtLiquidacionCabezaN.Rows(0).Item("Liquidacion")
                MaskedLiquidacionN.Visible = False
                Grid.Columns("ImporteB").ReadOnly = True
            End If
            PictureLupa.Enabled = False
            GridCompro.ReadOnly = True
            Grid.ReadOnly = True
        End If

        ComboEmisor.SelectedValue = PCliente

        MuestraTotales()

        AddHandler DtGrid.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGrid_ColumnChanging)
        AddHandler DtGridCompro.ColumnChanging, New DataColumnChangeEventHandler(AddressOf DtGridCompro_ColumnChanging)
        AddHandler DtGrid.RowChanged, New DataRowChangeEventHandler(AddressOf DtGrid_RowChanged)

    End Sub
    Private Function MuestraCabeza() As Boolean

        MiEnlazador = New BindingSource
        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            MiEnlazador.DataSource = DtLiquidacionCabezaB
        Else : MiEnlazador.DataSource = DtLiquidacionCabezaN
        End If

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
        PtipoFactura = Strings.Left(Numero.Value, 1)

        Numero.Value = Strings.Right(Numero.Value, 12)

    End Sub
    Private Sub FormatParseReciboOficial(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = CDbl(PtipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000"))

    End Sub
    Private Sub FormatText(ByVal sender As Object, ByVal Numero As ConvertEventArgs)

        Numero.Value = Numero.Value.ToString.Trim

    End Sub
    Private Function ActualizaArchivos(ByVal Funcion As String, ByRef DtLiquidacionCabezaBAux As DataTable, ByRef DtLiquidacionCabezaNAux As DataTable, ByRef DtLiquidacionDetalleBAux As DataTable, ByRef DtLiquidacionDetalleNAux As DataTable, ByRef DtLiquidacionLotesBAux As DataTable, ByRef DtLiquidacionlotesNAux As DataTable, ByRef DtRemitosB As DataTable, ByRef DtRemitosN As DataTable, ByRef DtLotesB As DataTable, ByRef DtLotesN As DataTable) As Boolean

        Dim RowsBusqueda() As DataRow

        If Funcion = "A" And CDbl(TextTotalN.Text) <> 0 Then DtLiquidacionCabezaNAux = DtLiquidacionCabezaBAux.Copy
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
                DtLiquidacionCabezaBAux.Rows(0).Item("Importe") = CDbl(TextTotalB.Text)
                DtLiquidacionCabezaBAux.Rows(0).Item("Saldo") = CDbl(TextTotalB.Text) - Gastado
            End If
            If PLiquidacion = 0 Then
                If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                    DtLiquidacionCabezaBAux.Rows(0).Item("Rel") = True
                Else
                    DtLiquidacionCabezaBAux.Rows(0).Item("Rel") = False
                End If
            End If
        End If

        If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
            RowsBusqueda = DtGrid.Select("Clave = 2")
            ImporteBruto = RowsBusqueda(0).Item("ImporteN")
            If Format(DtLiquidacionCabezaNAux.Rows(0).Item("FechaLiquidacion"), "dd/MM/yyyy") <> CDate(TextFechaNVLP.Text) Then DtLiquidacionCabezaNAux.Rows(0).Item("FechaLiquidacion") = CDate(TextFechaNVLP.Text)
            If Format(DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable"), "dd/MM/yyyy") <> CDate(TextFechaContable.Text) Then DtLiquidacionCabezaNAux.Rows(0).Item("FechaContable") = CDate(TextFechaContable.Text)
            If CDbl(TextTotalN.Text) <> DtLiquidacionCabezaNAux.Rows(0).Item("Importe") Or ImporteBruto <> DtLiquidacionCabezaNAux.Rows(0).Item("ImporteBruto") Then
                Dim Gastado As Decimal = DtLiquidacionCabezaNAux.Rows(0).Item("Importe") - DtLiquidacionCabezaNAux.Rows(0).Item("Saldo")
                DtLiquidacionCabezaNAux.Rows(0).Item("ImporteBruto") = ImporteBruto
                DtLiquidacionCabezaNAux.Rows(0).Item("Importe") = CDbl(TextTotalN.Text)
                DtLiquidacionCabezaNAux.Rows(0).Item("Saldo") = CDec(TextTotalN.Text) - Gastado
            End If
            If PLiquidacion = 0 Then
                If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                    DtLiquidacionCabezaNAux.Rows(0).Item("Rel") = True
                Else
                    DtLiquidacionCabezaNAux.Rows(0).Item("Rel") = False
                End If
            End If
        End If
        If Funcion = "B" Then
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then DtLiquidacionCabezaBAux.Rows(0).Item("Estado") = 3
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then DtLiquidacionCabezaNAux.Rows(0).Item("Estado") = 3
        End If

        'Actualizo Lotes Liquidados.
        'leo lotes liquidados por el cliente.
        DtLotesB = New DataTable
        DtLotesN = New DataTable
        Dim Esta As Boolean
        For Each Row As DataRow In DtGridCompro.Rows
            Esta = False
            If Row("Operacion") = 1 Then
                If DtLotesB.Rows.Count <> 0 Then
                    RowsBusqueda = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                    If RowsBusqueda.Length <> 0 Then Esta = True
                End If
                If Not Esta Then
                    If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & ";", Conexion, DtLotesB) Then Me.Close() : Exit Function
                End If
            Else
                If PermisoTotal Then
                    If DtLotesN.Rows.Count <> 0 Then
                        RowsBusqueda = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                        If RowsBusqueda.Length <> 0 Then Esta = True
                    End If
                    If Not Esta Then
                        If Not Tablas.Read("SELECT * FROM Lotes WHERE Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & ";", ConexionN, DtLotesN) Then Me.Close() : Exit Function
                    End If
                End If
            End If
        Next
        If DtLotesB.Rows.Count = 0 Then DtLotesB = DtLotesN.Clone
        If DtLotesN.Rows.Count = 0 Then DtLotesN = DtLotesB.Clone
        '

        RowsBusqueda = DtGrid.Select("Clave = 1")
        Dim Bruto As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 2")
        Bruto = Bruto + RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Dim Comision As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Dim Descarga As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        Dim FleteTerrestre As Decimal = RowsBusqueda(0).Item("ImporteB")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        Dim OtrsoConceptos As Decimal = RowsBusqueda(0).Item("ImporteB")
        Dim IndiceCorreccionCIvaB As Decimal = 0
        Dim IndiceCorreccionSIvaB As Decimal = 0
        If Bruto <> 0 Then IndiceCorreccionCIvaB = CDec(TextTotalB.Text) / Bruto
        If Bruto <> 0 Then IndiceCorreccionSIvaB = (Bruto - Comision - Descarga - FleteTerrestre - OtrsoConceptos) / Bruto
        '
        RowsBusqueda = DtGrid.Select("Clave = 2")
        Bruto = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 5")
        Comision = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 7")
        Descarga = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 9")
        FleteTerrestre = RowsBusqueda(0).Item("ImporteN")
        RowsBusqueda = DtGrid.Select("Clave = 10")
        OtrsoConceptos = RowsBusqueda(0).Item("ImporteN")
        Dim IndiceCorreccionCIvaN As Decimal = 0
        Dim IndiceCorreccionSIvaN As Decimal = 0
        If Bruto <> 0 Then IndiceCorreccionCIvaN = CDec(TextTotalN.Text) / Bruto
        If Bruto <> 0 Then IndiceCorreccionSIvaN = (Bruto - Comision - Descarga - FleteTerrestre - OtrsoConceptos) / Bruto
        '
        Dim NetoConIva As Double
        Dim NetoSinIva As Double
        '
        For Each Row As DataRow In DtGridCompro.Rows
            NetoConIva = Trunca(Row("ImporteB") * IndiceCorreccionCIvaB)
            NetoSinIva = Trunca(Row("ImporteB") * IndiceCorreccionSIvaB)
            RowsBusqueda = DtLiquidacionLotesBAux.Select("Indice = " & Row("Indice") & " AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & " AND Remito = " & Row("Remito"))
            If RowsBusqueda.Length = 0 Then
                If Row("ImporteB") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionLotesBAux.NewRow
                    Row1("Liquidacion") = 0
                    Row1("Indice") = Row("Indice")
                    Row1("Lote") = Row("Lote")
                    Row1("Secuencia") = Row("Secuencia")
                    Row1("Deposito") = Row("Deposito")
                    Row1("Operacion") = Row("Operacion")
                    Row1("Remitido") = Row("Remitido")
                    Row1("Merma") = Row("Merma")
                    Row1("MermaAux") = Row("MermaFicticia")
                    Row1("Cantidad") = Row("Cantidad")
                    Row1("Precio") = Row("PrecioB")
                    Row1("Importe") = Row("ImporteB")
                    Row1("NetoConIva") = NetoConIva
                    Row1("NetoSinIva") = NetoSinIva
                    Row1("Remito") = Row("Remito")
                    Row1("OPR") = Row("OperacionRemito")
                    DtLiquidacionLotesBAux.Rows.Add(Row1)
                    Dim RowsBusqueda1() As DataRow          'Actualiza merma.
                    If Row("Operacion") = 1 Then
                        RowsBusqueda1 = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                    Else
                        RowsBusqueda1 = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                    End If
                    RowsBusqueda1(0).Item("Merma") = CDec(RowsBusqueda1(0).Item("Merma")) + CDec(Row("Merma"))
                End If
            Else
                If Row("Merma") <> RowsBusqueda(0).Item("Merma") Then
                    Dim RowsBusqueda1() As DataRow        'Actualiza merma.
                    If Row("Operacion") = 1 Then
                        RowsBusqueda1 = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                    Else
                        RowsBusqueda1 = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                    End If
                    RowsBusqueda1(0).Item("Merma") = CDec(RowsBusqueda1(0).Item("Merma")) + Row("Merma") - CDec(RowsBusqueda(0).Item("Merma"))
                    RowsBusqueda(0).Item("Merma") = Row("Merma")
                    RowsBusqueda(0).Item("MermaAux") = Row("MermaFicticia")
                End If
                If Row("Cantidad") <> RowsBusqueda(0).Item("Cantidad") Then RowsBusqueda(0).Item("Cantidad") = Row("Cantidad")
                If Row("PrecioB") <> RowsBusqueda(0).Item("Precio") Then RowsBusqueda(0).Item("Precio") = Row("PrecioB")
                If Row("ImporteB") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("ImporteB")
                If NetoConIva <> RowsBusqueda(0).Item("NetoConIva") Then RowsBusqueda(0).Item("NetoConIva") = NetoConIva
                If NetoSinIva <> RowsBusqueda(0).Item("NetoSinIva") Then RowsBusqueda(0).Item("NetoSinIva") = NetoSinIva
            End If
            'Para cerrado.
            NetoConIva = Trunca(Row("ImporteN") * IndiceCorreccionCIvaN)
            NetoSinIva = Trunca(Row("ImporteN") * IndiceCorreccionSIvaN)
            RowsBusqueda = DtLiquidacionlotesNAux.Select("Indice = " & Row("Indice") & " AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito") & " AND Remito = " & Row("Remito"))
            If RowsBusqueda.Length = 0 Then
                If Row("ImporteN") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionlotesNAux.NewRow
                    Row1("Liquidacion") = 0
                    Row1("Indice") = Row("Indice")
                    Row1("Lote") = Row("Lote")
                    Row1("Secuencia") = Row("Secuencia")
                    Row1("Deposito") = Row("Deposito")
                    Row1("Operacion") = Row("Operacion")
                    Row1("Remitido") = Row("Remitido")
                    Row1("Merma") = Row("Merma")
                    Row1("MermaAux") = Row("MermaFicticia")
                    Row1("Cantidad") = Row("Cantidad")
                    Row1("Precio") = Row("PrecioN")
                    Row1("Importe") = Row("ImporteN")
                    Row1("NetoConIva") = NetoConIva
                    Row1("NetoSinIva") = NetoSinIva
                    Row1("OPR") = Row("OperacionRemito")
                    Row1("Remito") = Row("Remito")
                    DtLiquidacionlotesNAux.Rows.Add(Row1)
                    If Row("ImporteB") = 0 Then   'Actualiza merma.
                        Dim RowsBusqueda1() As DataRow
                        If Row("Operacion") = 1 Then
                            RowsBusqueda1 = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                        Else
                            RowsBusqueda1 = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                        End If
                        RowsBusqueda1(0).Item("Merma") = CDec(RowsBusqueda1(0).Item("Merma")) + Row("Merma")
                    End If
                End If
            Else
                If Row("Merma") <> RowsBusqueda(0).Item("Merma") Then
                    If Row("ImporteB") = 0 Then         'Actualiza merma.
                        Dim RowsBusqueda1() As DataRow
                        If Row("Operacion") = 1 Then
                            RowsBusqueda1 = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                        Else
                            RowsBusqueda1 = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                        End If
                        RowsBusqueda1(0).Item("Merma") = CDec(RowsBusqueda1(0).Item("Merma")) + Row("Merma") - CDec(RowsBusqueda(0).Item("Merma"))
                    End If
                    RowsBusqueda(0).Item("Merma") = Row("Merma")
                    RowsBusqueda(0).Item("MermaAux") = Row("MermaFicticia")
                End If
                If Row("Cantidad") <> RowsBusqueda(0).Item("Cantidad") Then RowsBusqueda(0).Item("Cantidad") = Row("Cantidad")
                If Row("PrecioN") <> RowsBusqueda(0).Item("Precio") Then RowsBusqueda(0).Item("Precio") = Row("PrecioN")
                If Row("ImporteN") <> RowsBusqueda(0).Item("Importe") Then RowsBusqueda(0).Item("Importe") = Row("ImporteN")
                If NetoConIva <> RowsBusqueda(0).Item("NetoConIva") Then RowsBusqueda(0).Item("NetoConIva") = NetoConIva
                If NetoSinIva <> RowsBusqueda(0).Item("NetoSinIva") Then RowsBusqueda(0).Item("NetoSinIva") = NetoSinIva
            End If
        Next
        If Funcion = "B" Then
            For Each Row As DataRow In DtGridCompro.Rows
                Dim RowsBusqueda1() As DataRow          'Actualiza merma.
                If Row("Operacion") = 1 Then
                    RowsBusqueda1 = DtLotesB.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                Else
                    RowsBusqueda1 = DtLotesN.Select("Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & " AND Deposito = " & Row("Deposito"))
                End If
                RowsBusqueda1(0).Item("Merma") = CDec(RowsBusqueda1(0).Item("Merma")) - Row("Merma")
            Next
        End If

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
        For Each Row As DataRow In DtGrid.Rows
            RowsBusqueda = DtLiquidacionDetalleNAux.Select("Impuesto = " & Row("Clave"))
            If RowsBusqueda.Length = 0 Then
                If Row("ImporteN") <> 0 Then
                    Dim Row1 As DataRow = DtLiquidacionDetalleNAux.NewRow
                    Row1("Liquidacion") = DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion")
                    Row1("Impuesto") = Row("Clave")
                    Row1("Alicuota") = Row("Iva")
                    Row1("Importe") = Row("ImporteN")
                    DtLiquidacionDetalleNAux.Rows.Add(Row1)
                End If
            Else
                If Row("ImporteN") <> RowsBusqueda(0).Item("Importe") Or Row("Iva") <> RowsBusqueda(0).Item("Alicuota") Then
                    If Row("ImporteN") <> 0 Then
                        RowsBusqueda(0).Item("Importe") = Row("ImporteN")
                        RowsBusqueda(0).Item("Alicuota") = Row("Iva")
                    Else : RowsBusqueda(0).Delete()
                    End If
                End If
            End If
        Next

        'Actualizo el campo "liquidado" en los lotes del remito en AsignacionLotes.
        If Funcion = "A" Or Funcion = "B" Then
            DtRemitosB = New DataTable
            DtRemitosN = New DataTable
            Dim Sql As String
            For Each Row As DataRow In DtGridCompro.Rows
                Sql = "SELECT * FROM AsignacionLotes WHERE TipoComprobante = 1 AND Comprobante = " & Row("Remito") & " AND Indice = " & Row("Indice") & " AND Lote = " & Row("Lote") & " AND Secuencia = " & Row("Secuencia") & ";"
                If Row("OperacionRemito") = 1 Then
                    If Not Tablas.Read(Sql, Conexion, DtRemitosB) Then Return False
                End If
                If Row("OperacionRemito") = 2 Then
                    If Not Tablas.Read(Sql, ConexionN, DtRemitosN) Then Return False
                End If
                If Row("OperacionRemito") < 0 Then
                    MsgBox("Error Operacion remito. Operación se CANCELA.")
                    Return False
                End If
            Next
            For Each Row As DataRow In DtRemitosB.Rows
                If Funcion = "A" Then
                    Row("Liquidado") = True
                Else : Row("Liquidado") = False
                End If
            Next
            For Each Row As DataRow In DtRemitosN.Rows
                If Funcion = "A" Then
                    Row("Liquidado") = True
                Else : Row("Liquidado") = False
                End If
            Next
        End If
        If DtRemitosB.Rows.Count = 0 Then DtRemitosB = DtRemitosN.Clone
        If DtRemitosN.Rows.Count = 0 Then DtRemitosN = DtRemitosB.Clone

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
                Case 5, 7, 9, 10
                    Row1.Cells("ImporteN").ReadOnly = False
                Case Else
                    Row1.Cells("ImporteN").ReadOnly = True
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
    Private Sub HacerAlta(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionCabezaNAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionDetalleNAux As DataTable, ByVal DtLiquidacionLotesBAux As DataTable, ByVal DtLiquidacionlotesNAux As DataTable, ByVal DtLotesBAux As DataTable, ByVal DtLotesNAux As DataTable, ByVal DtRemitosBAux As DataTable, ByVal DtRemitosNAux As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtRetencionProvinciaWW As DataTable)

        'Graba Factura.
        Dim NumeroB As Double = 0
        Dim NumeroN As Double = 0
        Dim NumeroAsientoB As Double = 0
        Dim NumeroAsientoN As Double = 0

        Dim Resul As Double = 0

        For i As Integer = 1 To 50
            If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then
                NumeroB = UltimaNumeracion(Conexion)
                If NumeroB < 0 Then
                    MsgBox("ERROR Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                NumeroN = UltimaNumeracion(ConexionN)
                If NumeroN < 0 Then
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
                For Each Row As DataRow In DtLiquidacionLotesBAux.Rows
                    Row("Liquidacion") = NumeroB
                Next
            End If
            '
            If DtLiquidacionCabezaNAux.Rows.Count <> 0 Then
                DtLiquidacionCabezaNAux.Rows(0).Item("Liquidacion") = NumeroN
                If DtLiquidacionCabezaBAux.Rows.Count <> 0 Then DtLiquidacionCabezaNAux.Rows(0).Item("Nrel") = NumeroB
                For Each Row As DataRow In DtLiquidacionDetalleNAux.Rows
                    Row("Liquidacion") = NumeroN
                Next
                For Each Row As DataRow In DtLiquidacionlotesNAux.Rows
                    Row("Liquidacion") = NumeroN
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
            'Halla Ultima numeracion Asiento B
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                NumeroAsientoN = UltimaNumeracionAsiento(ConexionN)
                If NumeroAsientoN < 0 Then
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
            If DtAsientoCabezaN.Rows.Count <> 0 Then
                DtAsientoCabezaN.Rows(0).Item("Asiento") = NumeroAsientoN
                DtAsientoCabezaN.Rows(0).Item("Documento") = NumeroN
                For Each Row As DataRow In DtAsientoDetalleN.Rows
                    Row("Asiento") = NumeroAsientoN
                Next
            End If
            '
            For Each Row As DataRow In DtRetencionProvinciaWW.Rows
                Row("Nota") = NumeroB
            Next
            '
            Resul = ActualizaFactura(DtLiquidacionCabezaBAux, DtLiquidacionCabezaNAux, DtLiquidacionDetalleBAux, DtLiquidacionDetalleNAux, DtLiquidacionLotesBAux, DtLiquidacionlotesNAux, DtLotesBAux, DtLotesNAux, DtRemitosBAux, DtRemitosNAux, DtAsientoCabezaB, DtAsientoDetalleB, DtAsientoCabezaN, DtAsientoDetalleN, DtRetencionProvinciaWW)
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
            If NumeroB <> 0 Then
                PLiquidacion = NumeroB
                PAbierto = True
            Else : PLiquidacion = NumeroN
                PAbierto = False
            End If
            GModificacionOk = True
            ArmaArchivos()
        End If

    End Sub
    Private Function ActualizaFactura(ByVal DtLiquidacionCabezaBAux As DataTable, ByVal DtLiquidacionCabezaNAux As DataTable, ByVal DtLiquidacionDetalleBAux As DataTable, ByVal DtLiquidacionDetalleNAux As DataTable, ByVal DtLiquidacionLotesBAux As DataTable, ByVal DtLiquidacionlotesNAux As DataTable, ByVal DtLotesBW As DataTable, ByVal DtLotesNW As DataTable, ByVal DtRemitosBW As DataTable, ByVal DtRemitosNW As DataTable, ByVal DtAsientoCabezaB As DataTable, ByVal DtAsientoDetalleB As DataTable, ByVal DtAsientoCabezaN As DataTable, ByVal DtAsientoDetalleN As DataTable, ByVal DtRetencionProvinciaWW As DataTable) As Double

        'CUIDADO: en GrabaTabla siempre poner getChange de la tabla para que tome los cambio cuando pase por segunda ves.

        Dim DtLotesBAux As DataTable = DtLotesBW
        Dim DtLotesNAux As DataTable = DtLotesNW
        Dim DtRemitosBAux As DataTable = DtRemitosBW
        Dim DtRemitosNAux As DataTable = DtRemitosNW

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
                If Not IsNothing(DtLiquidacionCabezaNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionCabezaNAux.GetChanges, "NVLPCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionDetalleBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionDetalleBAux.GetChanges, "NVLPDetalle", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionDetalleNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionDetalleNAux.GetChanges, "NVLPDetalle", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionLotesBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionLotesBAux.GetChanges, "NVLPLotes", Conexion)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLiquidacionlotesNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLiquidacionlotesNAux.GetChanges, "NVLPLotes", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                '
                If Not IsNothing(DtLotesBAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesBAux.GetChanges, "Lotes", Conexion)
                    If Resul <= 0 Then Return -2
                End If
                '
                If Not IsNothing(DtLotesNAux.GetChanges) Then
                    Resul = GrabaTabla(DtLotesNAux.GetChanges, "Lotes", ConexionN)
                    If Resul <= 0 Then Return -2
                End If
                '
                If Not IsNothing(DtRemitosBAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitosBAux.GetChanges, "AsignacionLotes", Conexion)
                    If Resul <= 0 Then Return -2
                End If
                '
                If Not IsNothing(DtRemitosNAux.GetChanges) Then
                    Resul = GrabaTabla(DtRemitosNAux.GetChanges, "AsignacionLotes", ConexionN)
                    If Resul <= 0 Then Return -2
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
                If Not IsNothing(DtAsientoCabezaN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoCabezaN.GetChanges, "AsientosCabeza", ConexionN)
                    If Resul <= 0 Then Return Resul
                End If
                If Not IsNothing(DtAsientoDetalleN.GetChanges) Then
                    Resul = GrabaTabla(DtAsientoDetalleN.GetChanges, "AsientosDetalle", ConexionN)
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
        ClienteOpr = Dta.Rows(0).Item("Opr")

        Dta.Dispose()

        Return True

    End Function
    Private Sub CalculaTotal()

        Dim TotalB As Decimal = 0
        Dim TotalN As Decimal = 0
        Dim TotalBG As Decimal = 0
        Dim TotalBE As Decimal = 0
        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtGridCompro.Rows
            TotalB = TotalB + Row("ImporteB")
            TotalN = TotalN + Row("ImporteN")
        Next

        If PLiquidacion = 0 Then
            For Each Row As DataRow In DtGridCompro.Rows
                If Row("Iva") <> 0 Then
                    TotalBG = TotalBG + Row("ImporteB")
                Else
                    TotalBE = TotalBE + Row("ImporteB")
                End If
            Next
            RowsBusqueda = DtGrid.Select("Clave = 1")
            RowsBusqueda(0).Item("ImporteB") = TotalBG
            RowsBusqueda = DtGrid.Select("Clave = 2")
            RowsBusqueda(0).Item("ImporteB") = TotalBE
        End If

        RowsBusqueda = DtGrid.Select("Clave = 2")
        RowsBusqueda(0).Item("ImporteN") = TotalN

        TotalB = 0
        TotalN = 0

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
        Dim TotalN As Decimal = 0

        For Each Row As DataRow In DtGrid.Rows
            If Row("Clave") = 1 Or Row("Clave") = 2 Or Row("Clave") = 3 Then
                TotalB = TotalB + Row("ImporteB")
                TotalN = TotalN + Row("ImporteN")
            Else
                TotalB = TotalB - Row("ImporteB")
                TotalN = TotalN - Row("ImporteN")
            End If
        Next

        TextTotalB.Text = FormatNumber(TotalB, GDecimales)
        TextTotalN.Text = FormatNumber(TotalN, GDecimales)

    End Sub
    Private Sub CreaDtGridCompro()

        DtGridCompro = New DataTable

        Dim Medida As New DataColumn("Medida")
        Medida.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(Medida)

        Dim OperacionRemito As New DataColumn("OperacionRemito")
        OperacionRemito.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(OperacionRemito)

        Dim Operacion As New DataColumn("Operacion")
        Operacion.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Operacion)

        Dim Indice As New DataColumn("Indice")
        Indice.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Indice)

        Dim Lote As New DataColumn("Lote")
        Lote.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Lote)

        Dim Secuencia As New DataColumn("Secuencia")
        Secuencia.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Secuencia)

        Dim Deposito As New DataColumn("Deposito")
        Deposito.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Deposito)

        Dim Remito As New DataColumn("Remito")
        Remito.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Remito)

        Dim Articulo As New DataColumn("Articulo")
        Articulo.DataType = System.Type.GetType("System.Int32")
        DtGridCompro.Columns.Add(Articulo)

        Dim NombreArticulo As New DataColumn("NombreArticulo")
        NombreArticulo.DataType = System.Type.GetType("System.String")
        DtGridCompro.Columns.Add(NombreArticulo)

        Dim Remitido As New DataColumn("Remitido")
        Remitido.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Remitido)

        Dim Merma As New DataColumn("Merma")
        Merma.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(Merma)

        Dim MermaFicticia As New DataColumn("MermaFicticia")
        MermaFicticia.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(MermaFicticia)

        Dim Cantidad As New DataColumn("Cantidad")
        Cantidad.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Cantidad)

        Dim CantidadFicticia As New DataColumn("CantidadFicticia")
        CantidadFicticia.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(CantidadFicticia)

        Dim PrecioB As New DataColumn("PrecioB")
        PrecioB.DataType = System.Type.GetType("System.Double")
        DtGridCompro.Columns.Add(PrecioB)

        Dim Iva As New DataColumn("Iva")
        Iva.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(Iva)

        Dim ImporteB As New DataColumn("ImporteB")
        ImporteB.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(ImporteB)

        Dim PrecioN As New DataColumn("PrecioN")
        PrecioN.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(PrecioN)

        Dim ImporteN As New DataColumn("ImporteN")
        ImporteN.DataType = System.Type.GetType("System.Decimal")
        DtGridCompro.Columns.Add(ImporteN)

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

        Articulo.DataSource = TodosLosArticulos()
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
        PtipoFactura = OpcionLetra.PNumeroLetra
        If PtipoFactura = 3 Then
            MsgBox("N.V.L.P Incorrecta para Letra C. Operación se CANCELA.")
            PCliente = 0
        End If
        If EsClienteDelGupo(PCliente) Then
            ButtonImportar.Visible = True
        Else
            ButtonImportar.Visible = False
        End If
        '       PtipoFactura = LetrasPermitidasCliente(HallaTipoIvaCliente(PCliente), 800)
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

        If Not ArmaListaParaTipoOperacionesLotes(ListaLotesParaAsiento, Operacion) Then Return False
        '
        Dim Item As New ItemListaConceptosAsientos
        Item.Clave = 213
        If Operacion = 1 Then
            Item.Importe = CDbl(TextTotalB.Text)
        Else
            Item.Importe = CDbl(TextTotalN.Text)
        End If
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
            If Not Asiento(800, 0, ListaConceptos, DtCabeza, DtDetalle, ListaCuentas, ListaLotesParaAsiento, ListaIVA, ListaRetenciones, CDate(TextFechaContable.Text), 0) Then Return False
        End If

        Return True

    End Function
    Private Function ArmaListaParaTipoOperacionesLotes(ByRef Lista As List(Of ItemLotesParaAsientos), ByVal Operacion As Integer) As Boolean

        Dim Neto As Decimal
        Dim Cantidad As Decimal
        Dim RowsBusqueda() As DataRow
        Dim ComisionW As Decimal
        Dim Comision As Decimal
        Dim Descarga As Decimal
        Dim FleteTerrestre As Decimal
        Dim OtrosConceptos As Decimal
        '
        Dim TotalComision As Decimal
        Dim TotalDescarga As Decimal
        Dim TotalFleteTerrestre As Decimal
        Dim TotalOtrosConceptos As Decimal

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

        For Each Row As DataRow In DtGridCompro.Rows
            Cantidad = Cantidad + Row("Cantidad")
        Next

        Dim Fila2 As New ItemLotesParaAsientos

        For Each Row As DataRow In DtGridCompro.Rows
            Dim Tipo As Integer
            Dim Centro As Integer
            If Row("Operacion") = 1 Then
                HallaCentroTipoOperacion(Row("Lote"), Row("Secuencia"), Conexion, Tipo, Centro)
            Else
                HallaCentroTipoOperacion(Row("Lote"), Row("Secuencia"), ConexionN, Tipo, Centro)
            End If
            If Centro <= 0 Then
                MsgBox("Error en Tipo Operacion en Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000"))
                Return False
            End If
            '
            Fila2 = New ItemLotesParaAsientos
            Fila2.TipoOperacion = Tipo
            Fila2.Centro = Centro
            If Operacion = 1 Then
                Fila2.MontoNeto = Row("ImporteB")
            Else
                Fila2.MontoNeto = Row("ImporteN")
            End If
            If Tipo = 1 Then Fila2.Clave = 301 'consignacion
            If Tipo = 2 Then Fila2.Clave = 300 'reventa
            If Tipo = 3 Then Fila2.Clave = 303 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 302 'costeo
            If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)
            '      Prorrotea Comision. 
            Fila2 = New ItemLotesParaAsientos
            Fila2.TipoOperacion = Tipo
            Fila2.Centro = Centro
            If Operacion = 1 Then
                Fila2.MontoNeto = Trunca(Row("ImporteB") * Comision / 100)
            Else
                Fila2.MontoNeto = Trunca(Row("ImporteN") * Comision / 100)
            End If
            If Tipo = 1 Then Fila2.Clave = 401 'consignacion
            If Tipo = 2 Then Fila2.Clave = 400 'reventa
            If Tipo = 3 Then Fila2.Clave = 403 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 402 'costeo
            TotalComision = TotalComision + Fila2.MontoNeto
            If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)
            '     Prorrotea Descarga. 
            Fila2 = New ItemLotesParaAsientos
            Fila2.TipoOperacion = Tipo
            Fila2.Centro = Centro
            Fila2.MontoNeto = Trunca(Descarga * Row("Cantidad") / Cantidad)
            If Tipo = 1 Then Fila2.Clave = 601 'consignacion
            If Tipo = 2 Then Fila2.Clave = 600 'reventa
            If Tipo = 3 Then Fila2.Clave = 603 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 602 'costeo
            TotalDescarga = TotalDescarga + Fila2.MontoNeto
            If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)
            '     Prorrotea Flete Terrestre. 
            Fila2 = New ItemLotesParaAsientos
            Fila2.TipoOperacion = Tipo
            Fila2.Centro = Centro
            Fila2.MontoNeto = Trunca(FleteTerrestre * Row("Cantidad") / Cantidad)
            If Tipo = 1 Then Fila2.Clave = 411 'consignacion
            If Tipo = 2 Then Fila2.Clave = 410 'reventa
            If Tipo = 3 Then Fila2.Clave = 413 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 412 'costeo
            TotalFleteTerrestre = TotalFleteTerrestre + Fila2.MontoNeto
            If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)
            '     Prorrotea Otros Conceptos. 
            Fila2 = New ItemLotesParaAsientos
            Fila2.TipoOperacion = Tipo
            Fila2.Centro = Centro
            Fila2.MontoNeto = Trunca(OtrosConceptos * Row("Cantidad") / Cantidad)
            If Tipo = 1 Then Fila2.Clave = 421 'consignacion
            If Tipo = 2 Then Fila2.Clave = 420 'reventa
            If Tipo = 3 Then Fila2.Clave = 423 'reventa MG
            If Tipo = 4 Then Fila2.Clave = 422 'costeo
            TotalOtrosConceptos = TotalOtrosConceptos + Fila2.MontoNeto
            If Fila2.MontoNeto <> 0 Then Lista.Add(Fila2)
        Next

        'Balancea los redondeos.
        For Each Fila As ItemLotesParaAsientos In Lista
            Select Case Fila.Clave
                Case 400, 401, 402, 403
                    Fila.MontoNeto = Fila.MontoNeto + ComisionW - TotalComision
                    Exit For
            End Select
        Next
        For Each Fila As ItemLotesParaAsientos In Lista
            Select Case Fila.Clave
                Case 600, 601, 602, 603
                    Fila.MontoNeto = Fila.MontoNeto + Descarga - TotalDescarga
                    Exit For
            End Select
        Next
        For Each Fila As ItemLotesParaAsientos In Lista
            Select Case Fila.Clave
                Case 410, 411, 412, 413
                    Fila.MontoNeto = Fila.MontoNeto + FleteTerrestre - TotalFleteTerrestre
                    Exit For
            End Select
        Next
        For Each Fila As ItemLotesParaAsientos In Lista
            Select Case Fila.Clave
                Case 420, 421, 422, 423
                    Fila.MontoNeto = Fila.MontoNeto + OtrosConceptos - TotalOtrosConceptos
                    Exit For
            End Select
        Next

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
            Texto = "Nro. Interno :  " & Format(Val(MaskedLiquidacion.Text), "00000000") & " " & Format(Val(MaskedLiquidacionN.Text), "00000000")
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
            If PermisoTotal Then
                Texto = "PRECIO2"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaPrecio2 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
                Texto = "IMPORTE2"
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporte2 - Longi - 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, y + 2)
            End If
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
                Texto = Row.Cells("Compro").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaLote - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Remito.
                Texto = Row.Cells("Remito").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Articulo.
                Texto = Mid(Row.Cells("Articulo").FormattedValue, 1, 20)
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemito + 1
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Remitido.
                Texto = Row.Cells("Remitido").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaRemitido - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime merma.
                Texto = Row.Cells("MermaFicticia").FormattedValue
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaMerma - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                'Imprime Cantidad.
                Texto = Row.Cells("CantidadFicticia").FormattedValue
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
                If PermisoTotal Then
                    'Imprime precio2.
                    Texto = Row.Cells("Precio2").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width()
                    Xq = LineaPrecio2 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    'Imprime importe2.
                    Texto = Row.Cells("Importe2").FormattedValue
                    Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                    Xq = LineaImporte2 - Longi
                    e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                End If
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
                If Row.Cells("ImporteB").Value <> 0 Or Row.Cells("ImporteN").Value <> 0 Then
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
                    If PermisoTotal Then
                        'Imprime importen.
                        Texto = Row.Cells("ImporteN").FormattedValue
                        Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                        Xq = LineaImporteC2 - Longi
                        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
                    End If
                End If
            Next

            Yq = MTop + 237

            PrintFont = New Font("Courier New", 10)
            Texto = TextTotalB.Text
            Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
            Xq = LineaImporteC - Longi
            e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            If PermisoTotal Then
                Texto = TextTotalN.Text
                Longi = e.Graphics.MeasureString(Texto, PrintFont).Width
                Xq = LineaImporteC2 - Longi
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, Xq, Yq)
            End If

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
    Private Function EsContable(ByVal NVLP As Double) As Boolean

        Try
            Using Miconexion As New OleDb.OleDbConnection(Conexion)
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT Tr FROM NVLPCabeza WHERE Estado = 1 AND Liquidacion = " & NVLP & ";", Miconexion)
                    Return CDbl(Cmd.ExecuteScalar())
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error al leer Tabla: NVLPCabeza.", MsgBoxStyle.Critical)
            End
        End Try

    End Function
    Private Sub AgregaLotesDelGrid(ByVal Lista As List(Of FilaComprobanteFactura), ByVal EsImportacion As Boolean)

        Dim RowsBusqueda() As DataRow
        HayFilasErroneas = False

        If Lista.Count <> 0 Then
            Dim Dt As DataTable = DtGridCompro.Copy
            For Each Fila As FilaComprobanteFactura In Lista
                RowsBusqueda = Dt.Select("Indice = " & Fila.Indice & " AND Lote = " & Fila.Lote & " AND Secuencia = " & Fila.Secuencia & " AND Deposito = " & Fila.Deposito & " AND Remito = " & Fila.Remito)
                If RowsBusqueda.Length = 0 Then
                    Dim Row As DataRow = Dt.NewRow
                    Row("Operacion") = Fila.Operacion   'Operacion Lote del remito (asignacionLote).
                    Row("OperacionRemito") = Fila.OperacionRemito   'Operacion Remito.
                    Row("Indice") = Fila.Indice
                    Row("Lote") = Fila.Lote
                    Row("Secuencia") = Fila.Secuencia
                    Row("Articulo") = Fila.Articulo
                    Dim RowsBusquedaA() As DataRow
                    RowsBusquedaA = Articulo.DataSource.select("Clave = " & Row("Articulo"))
                    Row("NombreArticulo") = RowsBusquedaA(0).Item("Nombre")
                    Row("Deposito") = Fila.Deposito
                    Row("Remito") = Fila.Remito
                    Row("Remitido") = Fila.Cantidad
                    Row("Merma") = 0
                    Row("Cantidad") = Fila.Cantidad
                    If EsImportacion Then Row("Cantidad") = Fila.Ingreso
                    Row("MermaFicticia") = 0
                    Row("CantidadFicticia") = Fila.Cantidad
                    If EsImportacion Then Row("CantidadFicticia") = Fila.Ingreso
                    Row("PrecioB") = 0
                    Row("Iva") = HallaIva(Fila.Articulo)
                    Row("ImporteB") = 0
                    Row("PrecioN") = 0
                    Row("ImporteN") = 0
                    Row("Medida") = ""
                    Dim AGranel As Boolean
                    HallaAGranelYMedida(Fila.Articulo, AGranel, Row("Medida"))
                    If Row("Remitido") <> Row("Cantidad") Then Row.RowError = "Lote Ajustado" : HayFilasErroneas = True
                    Dt.Rows.Add(Row)
                End If
            Next
            For I As Integer = Dt.Rows.Count - 1 To 0 Step -1
                Dim Row As DataRow = Dt.Rows(I)
                Dim Esta As Boolean = False
                For Each Fila As FilaComprobanteFactura In Lista
                    If Fila.Indice = Row("Indice") And Fila.Remito = Row("Remito") And Fila.Lote = Row("Lote") And Fila.Secuencia = Row("Secuencia") And Fila.Deposito = Row("Deposito") Then
                        Esta = True
                        Exit For
                    End If
                Next
                If Esta = False Then Row.Delete()
            Next
            DtGridCompro.Clear()
            For Each Row As DataRow In Dt.Rows
                DtGridCompro.ImportRow(Row)
            Next
            Dt.Dispose()
        End If

        If DtGridCompro.Rows.Count = 0 Then IvaW = 0
        If IvaW = 0 And DtGridCompro.Rows.Count <> 0 Then
            IvaW = DtGridCompro.Rows(0).Item("Iva")
        End If

        RowsBusqueda = DtGrid.Select("Clave = 3")
        RowsBusqueda(0).Item("Iva") = Format(IvaW, "0.00")

        If GTipoIva <> 2 Then
            If DtGridCompro.Rows.Count <> 0 Then
                For I As Integer = DtGridCompro.Rows.Count - 1 To 0 Step -1
                    Dim Row As DataRow = DtGridCompro.Rows(I)
                    If IvaW <> Row("Iva") Then
                        MsgBox("Iva del Articulo " & NombreArticulo(Row("Articulo")) & " difiere de los Anteriores.")
                        Row.Delete()
                    End If
                Next
            End If
        End If

        CalculaTotal()

        If HayFilasErroneas Then
            CajaTextoAjusteLotes.Visible = True
            ButtonAceptar.Enabled = False
        Else
            CajaTextoAjusteLotes.Visible = False
            ButtonAceptar.Enabled = True
        End If
        HayFilasErroneas = False

    End Sub
    Private Function ProcesaDetalleImportacion(ByVal CantidadConDevolucionEnLiquidacion As Decimal, ByVal Remito As Decimal, ByVal ArticuloEnLiq As Integer, ByVal PrecioEnLiqB As Decimal, _
                         ByVal PrecioEnLiqN As Decimal, ByRef ListaLotes As List(Of ItemLotesDelRemito), ByVal ConexionStr As String, ByVal OperacionRemito As Integer, ByVal Dt As DataTable, ByRef HayFilasErroneas As Boolean) As Boolean

        Dim RowsBusqueda() As DataRow
        HayFilasErroneas = False

        Dim ArticuloDestino As Decimal = HallaArticuloDestino(ArticuloEnLiq, ComboEmisor.SelectedValue)
        If ArticuloDestino = -1000 Then Return False

        RowsBusqueda = Dt.Select("Articulo = " & ArticuloDestino)
        If RowsBusqueda.Length = 0 Then
            MsgBox("Articulo: " & NombreArticulo(ArticuloDestino) & " No existe en el Remito: " & NumeroEditado(Remito)) : Return True  'false
        End If
        '
        RowsBusqueda = Dt.Select("Articulo = " & ArticuloDestino & " AND (Cantidad - Devueltas) = " & CantidadConDevolucionEnLiquidacion.ToString.Replace(",", "."))
        If RowsBusqueda.Length = 0 Then
            MsgBox("Cantidad Articulo: " & NombreArticulo(ArticuloDestino) & " en Remito: " & NumeroEditado(Remito) & " No Coincide con la Cantidad Ingresada en el Cliente.")
            HayFilasErroneas = True
            RowsBusqueda = Dt.Select("Articulo = " & ArticuloDestino)  'Para que lo muestre en la NVLP con error.
        End If

        If RowsBusqueda.Length > 1 Then   'Caso en que hay más de un indice para el mismo articulo = ArticuloDestino. En ese caso se pone precios = 0 para que lo complete el Operador. 
            PrecioEnLiqB = 0 : PrecioEnLiqN = 0
        End If

        For I As Integer = 0 To RowsBusqueda.Length - 1
            If Not LeeAsignacionLotesDelRemito(ConexionStr, Remito, RowsBusqueda(I).Item("Indice"), ArticuloDestino, PrecioEnLiqB, PrecioEnLiqN, ListaLotes) Then Return False
        Next

        Return True

    End Function
    Private Function LeeAsignacionLotesDelRemito(ByVal ConexionStr As String, ByVal Remito As Decimal, ByVal Indice As Integer, ByVal ArticuloDestino As Integer, ByVal PrecioB As Decimal, ByVal PrecioN As Decimal, ByRef Lista As List(Of ItemLotesDelRemito)) As Boolean

        Dim Dt As New DataTable

        Dim Nombre As String = NombreArticulo(ArticuloDestino)
        If Not Tablas.Read("SELECT * FROM AsignacionLotes WHERE TipoComprobante= 1 AND Comprobante = " & Remito & " AND Indice = " & Indice & " AND Cantidad <> 0;", ConexionStr, Dt) Then
            MsgBox("Error al leer Tablas: AsignacionLotes.", MsgBoxStyle.Critical) : Dt.Dispose() : Return False
        End If
        If Dt.Rows.Count = 0 Then
            MsgBox("Lotes del Remito " & Remito & " No Encontrado.", MsgBoxStyle.Critical) : Dt.Dispose() : Return False
        End If
        For Each Row As DataRow In Dt.Rows
            If Row("Liquidado") Then
                If MsgBox("Lote " & Row("Lote") & "/" & Format(Row("Secuencia"), "000") & " Remito " & NumeroEditado(Remito) & " Ya tiene Liquidación. Quiere continuar -SIN- Agregarlo a la NVLP?.", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    Continue For
                Else
                    Dt.Dispose() : Return False
                End If
            End If
            Dim Fila As New ItemLotesDelRemito
            Fila.Remito = Remito
            Fila.Indice = Row("Indice")
            Fila.Lote = Row("Lote")
            Fila.Secuencia = Row("Secuencia")
            Fila.OperacionLote = Row("Operacion")
            Fila.Cantidad = Row("Cantidad")
            Fila.Liquidado = Row("Cantidad")
            Fila.Articulo = ArticuloDestino
            Fila.PrecioB = PrecioB
            Fila.PrecioN = PrecioN
            Fila.Deposito = Row("Deposito")
            Lista.Add(Fila)
        Next

        Dt.Dispose()
        Return True

    End Function
    Private Function HallaArticuloDestino(ByVal ArticuloOrigen As Integer, ByVal ClienteDestino As Integer) As Integer

        Dim ArticuloDestino As Integer

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT Articulo FROM CodigosCliente WHERE Cliente = " & ClienteDestino & " AND CodigoDeCliente = '" & ArticuloOrigen & "';", Conexion, Dt) Then
            MsgBox("Error al leer Tabla: CodigosCliente.") : Return -1000
        End If
        If Dt.Rows.Count = 0 Then
            MsgBox("Articulo: " & ArticuloOrigen & " No encontrado en Código Cliente en Empresa Proveedora.", MsgBoxStyle.Critical) : Return -1000
        Else
            ArticuloDestino = Dt.Rows(0).Item("Articulo")
        End If

        Dt.Dispose()
        Return ArticuloDestino

    End Function
    Private Sub HallaDatosRemito(ByVal Remito As Decimal, ByVal Cliente As Integer, ByRef ConexionRemito As String, ByRef OperacionRemito As Integer, ByRef DtDetalleRemito As DataTable)

        ConexionRemito = "" : OperacionRemito = 0
        Dim Dt As New DataTable
        DtDetalleRemito.Clear()

        If Not Tablas.Read("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & " AND Cliente = " & Cliente & ";", Conexion, Dt) Then
            MsgBox("Error al Leer Tablas: RemitosCabeza.") : Exit Sub
        End If
        If Dt.Rows.Count <> 0 Then
            ConexionRemito = Conexion : OperacionRemito = 1
            If Not Tablas.Read("SELECT * FROM RemitosDetalle WHERE Remito = " & Remito & ";", Conexion, DtDetalleRemito) Then
                MsgBox("Error al Leer Tablas: RemitosDetalle.") : Exit Sub
            End If
            Dt.Dispose() : Exit Sub
        End If

        'Si no esta en Conexion.
        If Not Tablas.Read("SELECT Remito FROM RemitosCabeza WHERE Remito = " & Remito & " AND Cliente = " & Cliente & ";", ConexionN, Dt) Then
            MsgBox("Error al Leer Tablas: RemitosCabeza(N).") : Exit Sub
        End If
        If Dt.Rows.Count <> 0 Then
            ConexionRemito = ConexionN : OperacionRemito = 2
            If Not Tablas.Read("SELECT * FROM RemitosDetalle WHERE Remito = " & Remito & ";", ConexionN, DtDetalleRemito) Then
                MsgBox("Error al Leer Tablas: RemitosDetalle(N).") : Exit Sub
            End If
            Dt.Dispose() : Exit Sub
        End If

        MsgBox("Remito: " & NumeroEditado(Remito) & " No Encontrado.")
        Dt.Dispose()

    End Sub
    Private Function Valida() As Boolean

        If DtLiquidacionCabezaB.Rows.Count <> 0 Then
            If DtLiquidacionCabezaB.Rows(0).Item("Rel") And Not PermisoTotal Then
                MsgBox("Error, en este momento no puede modificar(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        End If

        If Not ClienteOpr And CDbl(TextTotalB.Text) <> 0 Then
            If MsgBox("Cliente Solo Opera a Candado Cerrado. Debe Cambiar Candado en Cliente. Quiere Continuar de Todos Modos?(S/N)", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Return False
        End If

        If PLiquidacion <> 0 Then
            If CDbl(TextTotalB.Text) <> 0 And DtLiquidacionCabezaB.Rows.Count = 0 Or CDbl(TextTotalB.Text) = 0 And DtLiquidacionCabezaB.Rows.Count <> 0 Then
                MsgBox("Para Realizar Esta Modificacion Debe Dar de Baja Esta Factura y ReHacerla. Operación se CANCELA.")
                Return False
            End If
            If CDbl(TextTotalN.Text) <> 0 And DtLiquidacionCabezaN.Rows.Count = 0 Or CDbl(TextTotalN.Text) = 0 And DtLiquidacionCabezaN.Rows.Count <> 0 Then
                MsgBox("Debe Anular esta N.V.L.P. y Rehacerla. Operación se CANCELA.")
                Return False
            End If
        End If

        If GridCompro.Rows.Count = 0 Then
            MsgBox("Falta Informar Items a Liquidar.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If CDbl(TextTotalB.Text) < 0 Or CDbl(TextTotalN.Text) < 0 Then
            MsgBox("Total Negativo", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Cantidad").Value = 0 Then
                MsgBox("Falta Cantidad en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("Precio").Value = 0 And Row.Cells("Precio2").Value = 0 Then
                MsgBox("Falta Precio en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("Importe").Value = 0 And Row.Cells("Importe2").Value = 0 Then
                MsgBox("Falta Importe en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("Precio").Value <> 0 And Row.Cells("Importe").Value = 0 Then
                MsgBox("Falta Importe en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("Precio2").Value <> 0 And Row.Cells("Importe2").Value = 0 Then
                MsgBox("Falta Importe(2) en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("Precio").Value = 0 And Row.Cells("OperacionRemito").Value = 1 Then
                MsgBox("Falta Precio en Lote " & Row.Cells("Lote").Value & "/" & Format(Row.Cells("Secuencia").Value, "000"))
                Return False
            End If
            If Row.Cells("MermaFicticia").Value < 0 Then
                If MsgBox("Existe Merma Positiva. Desea Continuar?.", MsgBoxStyle.Information + MsgBoxStyle.YesNo) = MsgBoxResult.No Then
                    Return False
                End If
            End If
        Next

        If CDbl(TextTotalB.Text) = 0 And CDbl(TextTotalN.Text) = 0 Then
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
                    If Grid.Rows(i).Cells("Alicuota").Value <> 0 And (Grid.Rows(i).Cells("ImporteB").Value = 0 Or Grid.Rows(i).Cells("ImporteN").Value = 0) Then
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
            If Val(PtipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000")) <> ReciboOficialAnt Then
                Select Case ExisteNVLP(Val(PtipoFactura & Format(Val(MaskedReciboOficial.Text), "000000000000")), PCliente)
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
        If DtLiquidacionCabezaN.Rows.Count <> 0 Then
            Dim Imputado As Double = DtLiquidacionCabezaN.Rows(0).Item("Importe") - DtLiquidacionCabezaN.Rows(0).Item("Saldo")
            If CDbl(TextTotalN.Text) < Imputado Then
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

        Dim HayImporteB As Boolean = False
        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Importe").Value <> 0 Then HayImporteB = True
        Next
        For Each Row As DataGridViewRow In GridCompro.Rows
            If Row.Cells("Importe").Value = 0 And Row.Cells("Importe2").Value <> 0 And HayImporteB Then
                MsgBox("No Debe Existir un Lote con Solo Importe(2), si Hay Lotes con Importe. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Return False
            End If
        Next

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

        If Grid.Columns(e.ColumnIndex).Name = "ImporteB" Or Grid.Columns(e.ColumnIndex).Name = "ImporteN" Then
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

        If e.Column.ColumnName.Equals("ImporteN") Then
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
    ' -------------------------              MANEJO DEL GRID de comprobantes.  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub GridCompro_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEnter

        If Not GridCompro.Columns(e.ColumnIndex).ReadOnly Then
            GridCompro.BeginEdit(True)
        End If

    End Sub
    Private Sub Gridcompro_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles GridCompro.CellEndEdit

        CalculaTotal()

    End Sub
    Private Sub GridCompro_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles GridCompro.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If GridCompro.Columns(e.ColumnIndex).Name = "Compro" Then
            If PermisoTotal Then
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Abierto")
                If GridCompro.Rows(e.RowIndex).Cells("Operacion").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("Candado").Value = ImageList1.Images.Item("Cerrado")
            End If
            If GridCompro.Rows(e.RowIndex).Cells("Lote").Value <> 0 Then
                e.Value = GridCompro.Rows(e.RowIndex).Cells("Lote").Value & "/" & Format(GridCompro.Rows(e.RowIndex).Cells("Secuencia").Value, "000")
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Remito" Then
            If Not IsDBNull(e.Value) Then
                If PermisoTotal Then
                    If GridCompro.Rows(e.RowIndex).Cells("OperacionRemito").Value = 1 Then GridCompro.Rows(e.RowIndex).Cells("CandadoRemito").Value = ImageList1.Images.Item("Abierto")
                    If GridCompro.Rows(e.RowIndex).Cells("OperacionRemito").Value = 2 Then GridCompro.Rows(e.RowIndex).Cells("CandadoRemito").Value = ImageList1.Images.Item("Cerrado")
                End If
                e.Value = NumeroEditado(e.Value)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Importe" Or GridCompro.Columns(e.ColumnIndex).Name = "Importe2" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                e.Value = FormatNumber(e.Value, GDecimales)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "Precio" Or GridCompro.Columns(e.ColumnIndex).Name = "Precio2" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            Else
                e.Value = FormatNumber(e.Value, 5)
            End If
        End If

        If GridCompro.Columns(e.ColumnIndex).Name = "MermaFicticia" Or GridCompro.Columns(e.ColumnIndex).Name = "CantidadFicticia" Then
            If IsDBNull(e.Value) Then e.Value = 0
            If e.Value = 0 Then
                e.Value = Format(e.Value, "#")
            End If
        End If

        If GridCompro.Rows(e.RowIndex).ErrorText = "Lote Ajustado" Then
            GridCompro.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.IndianRed
            GridCompro.Rows(e.RowIndex).ErrorText = ""
        End If

    End Sub
    Private Sub GridCompro_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles GridCompro.EditingControlShowing

        Dim columna As Integer = GridCompro.CurrentCell.ColumnIndex

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPressCompro
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChangedCompro

    End Sub
    Private Sub ValidaKey_KeyPressCompro(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio2" Or _
           GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Importe" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Importe2" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "CantidadFicticia" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "MermaFicticia" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("-0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

    End Sub
    Private Sub ValidaChanged_TextChangedCompro(ByVal sender As Object, ByVal e As System.EventArgs)

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "Precio2" Then
            If CType(sender, TextBox).Text <> "" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 5)
            End If
        End If

        If GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "CantidadFicticia" Or GridCompro.Columns.Item(GridCompro.CurrentCell.ColumnIndex).Name = "MermaFicticia" Then
            If CType(sender, TextBox).Text <> "" And CType(sender, TextBox).Text <> "-" Then
                EsNumericoGridBox.Valida(CType(sender, TextBox), 2)
            End If
        End If

    End Sub
    Private Sub DtGridCompro_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        If PLiquidacion <> 0 Then Exit Sub

        If (e.Column.ColumnName.Equals("PrecioB")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca5(e.ProposedValue)
            If e.ProposedValue > 0 And e.Row("OperacionRemito") = 2 Then
                MsgBox("Remito solo puede tener Importe(2).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = 0
            End If
            e.Row("ImporteB") = CalculaNeto(e.Row("CantidadFicticia"), e.ProposedValue)
        End If

        If (e.Column.ColumnName.Equals("PrecioN")) Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca5(e.ProposedValue)
            e.Row("ImporteN") = CalculaNeto(e.Row("CantidadFicticia"), e.ProposedValue)
        End If

        If e.Column.ColumnName.Equals("MermaFicticia") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            e.ProposedValue = Trunca(e.ProposedValue)
            If e.ProposedValue >= 0 Then
                If e.ProposedValue > e.Row("Remitido") Then
                    MsgBox("Merma Supera Cantidad.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    e.ProposedValue = e.Row("MermaFicticia")
                End If
                e.Row("Merma") = e.ProposedValue
                e.Row("Cantidad") = Trunca(e.Row("Remitido") - e.ProposedValue)
            Else
                e.Row("Merma") = 0
                e.Row("Cantidad") = e.Row("Remitido")
            End If
            e.Row("CantidadFicticia") = Trunca(e.Row("Remitido") - e.ProposedValue)
            e.Row("ImporteB") = CalculaNeto(e.Row("CantidadFicticia"), e.Row("PrecioB"))
            e.Row("ImporteN") = CalculaNeto(e.Row("CantidadFicticia"), e.Row("PrecioN"))
        End If

        GridCompro.Refresh()

    End Sub
    Private Sub Gridcompro_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles GridCompro.DataError
        Exit Sub
    End Sub
    Private Sub Grid_RowValidating(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles Grid.RowValidating

        If IsDBNull(Grid.Rows(e.RowIndex)) Or IsNothing(Grid.Rows(e.RowIndex)) Then Exit Sub

        If Grid.Rows(e.RowIndex).GetErrorText(e.RowIndex) = "Lote Ajustado" Then
            Grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.IndianRed
            Grid.Rows(e.RowIndex).ErrorText = ""
        End If

    End Sub
   
End Class