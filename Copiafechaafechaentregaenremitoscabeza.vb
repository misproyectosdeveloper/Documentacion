Imports System.Transactions
Public Class Copiafechaafechaentregaenremitoscabeza

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Dim DtB As New DataTable
        Dim Contador As Integer = 0
        Dim Contador2 As Integer = 0

        If Not Tablas.Read("SELECT * From Lotes WHERE SecuenciaOrigen > 99 ORDER by loteorigen,secuenciaorigen;", ConexionN, DtB) Then End
        If DtB.Rows.Count = 0 Then
            MsgBox("Termino 2 ")
            Exit Sub
        End If

        For Each Row As DataRow In DtB.Rows
            Dim Dt As New DataTable
            Dim Sql As String = "SELECT LoteOrigen,SecuenciaOrigen FROM Lotes WHERE Lote = " & Row("LoteOrigen") & " AND Secuencia = " & Row("SecuenciaOrigen") & ";"
            If Not Tablas.Read(Sql, ConexionN, Dt) Then End
            If Dt.Rows(0).Item("SecuenciaOrigen") > 99 Then
                Contador = Contador + 1
            Else
                Row("SecuenciaOrigen") = Dt.Rows(0).Item("SecuenciaOrigen")
                Contador2 = Contador2 + 1
            End If
        Next

        MsgBox(Contador2)


        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                '
                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "Lotes", ConexionN)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Termino")


    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Dim DtB As New DataTable
        Dim DocuRetencion As New DataTable

        If Not Tablas.Read("SELECT * From DocuRetenciones;", Conexion, DocuRetencion) Then End
        If Not Tablas.Read("SELECT * From Tablas WHERE Tipo = 25;", Conexion, DtB) Then End
        For Each Row As DataRow In DtB.Rows
            Row("Iva") = 20170101
            If Row("Codigoretencion") = 2 Then  'percepcion.
                Row("Activo2") = False
                Row("Activo3") = False
                Row("Activo4") = False
                Row("TipoIva") = 0
                Row("Formula") = 0
                Row("TopeMes") = 0
                Row("AlicuotaRetencion") = 0
            End If
        Next

        Dim RowsBusqueda() As DataRow

        For Each Row As DataRow In DtB.Rows
            If Row("Codigoretencion") = 1 Then  'retencion.
                If Row("Activo4") Then
                    If Row("Formula") = 1 Then
                        Dim RowA As DataRow = DocuRetencion.NewRow
                        RowA("Clave") = Row("Clave")
                        RowA("TipoDocumento") = 600
                        DocuRetencion.Rows.Add(RowA)
                    End If
                    If Row("Formula") = 2 Then
                        Dim RowA As DataRow = DocuRetencion.NewRow
                        RowA("Clave") = Row("Clave")
                        RowA("TipoDocumento") = 10
                        DocuRetencion.Rows.Add(RowA)
                    End If
                End If
                Row("Activo2") = False
                Row("Activo3") = False
                Row("Activo4") = False
                Row("TipoIva") = 0
            End If
        Next

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                '
                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "Tablas", Conexion)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                If Not IsNothing(DocuRetencion.GetChanges) Then
                    Resul = GrabaTabla(DocuRetencion.GetChanges, "DocuRetenciones", Conexion)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Termino")

    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click

        Dim DtB As New DataTable

        If Not Tablas.Read("SELECT * From NotasCreditoCabeza;", Conexion, DtB) Then End
        For Each Row As DataRow In DtB.Rows
            If Format(Row("FechaContable"), "yyyyMMdd") = "18000101" Then
                Row("FechaContable") = Row("Fecha")
            End If
        Next

        Dim DtN As New DataTable

        If Not Tablas.Read("SELECT * From NotasCreditoCabeza;", ConexionN, DtN) Then End
        For Each Row As DataRow In DtN.Rows
            If Format(Row("FechaContable"), "yyyyMMdd") = "18000101" Then
                Row("FechaContable") = Row("Fecha")
            End If
        Next

        'Factura....................................................
        Dim DtFB As New DataTable

        If Not Tablas.Read("SELECT * From FacturasCabeza;", Conexion, DtFB) Then End
        For Each Row As DataRow In DtFB.Rows
            If Format(Row("FechaContable"), "yyyyMMdd") = "18000101" Then
                Row("FechaContable") = Row("Fecha")
            End If
        Next

        Dim DtFN As New DataTable

        If Not Tablas.Read("SELECT * From FacturasCabeza;", ConexionN, DtFN) Then End
        For Each Row As DataRow In DtFN.Rows
            If Format(Row("FechaContable"), "yyyyMMdd") = "18000101" Then
                Row("FechaContable") = Row("Fecha")
            End If
        Next

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                '
                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "NotasCreditoCabeza", Conexion)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                If Not IsNothing(DtN.GetChanges) Then
                    Resul = GrabaTabla(DtN.GetChanges, "NotasCreditoCabeza", ConexionN)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                If Not IsNothing(DtFB.GetChanges) Then
                    Resul = GrabaTabla(DtFB.GetChanges, "FacturasCabeza", Conexion)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                If Not IsNothing(DtFN.GetChanges) Then
                    Resul = GrabaTabla(DtFN.GetChanges, "FacturasCabeza", ConexionN)
                    If Resul <= 0 Then Exit Sub
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Termino")


    End Sub
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click

        Dim DtB As New DataTable

        TextBox1.Clear()
        If Not Tablas.Read("SELECT Remito,Fecha From RemitosCabeza;", Conexion, DtB) Then End
        For Each Row As DataRow In DtB.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosCabeza WHERE TipoDocumento = 6060 and documento = " & Row("Remito") & ";", Conexion, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & Format(Row("Remito"), "0000-00000000") & "  " & Row("Fecha")
            End If
        Next

        Dim DtN As New DataTable

        If Not Tablas.Read("SELECT Remito,Fecha From RemitosCabeza;", ConexionN, DtN) Then End
        For Each Row As DataRow In DtN.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosCabeza WHERE TipoDocumento = 6060 and documento = " & Row("Remito") & ";", ConexionN, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & Format(Row("Remito"), "0000-00000000") & "  " & Row("Fecha")
            End If
        Next

        DtB.Dispose()
        DtN.Dispose()

        MsgBox("Fin")

    End Sub
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click

        Dim DtB As New DataTable

        TextBox1.Clear()
        If Not Tablas.Read("SELECT  Asiento From AsientosDetalle;", Conexion, DtB) Then End
        For Each Row As DataRow In DtB.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosCabeza WHERE Asiento = " & Row("Asiento") & ";", Conexion, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & "B" & Row("Asiento")
            End If
        Next

        Dim DtN As New DataTable

        If Not Tablas.Read("SELECT  Asiento From AsientosDetalle;", ConexionN, DtN) Then End
        For Each Row As DataRow In DtN.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Asiento FROM AsientosCabeza WHERE Asiento = " & Row("Asiento") & ";", ConexionN, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & "N" & Row("Asiento")
            End If
        Next

        DtB.Dispose()
        DtN.Dispose()

        MsgBox("Fin")

    End Sub
    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click

        Dim DtB As New DataTable

        TextBox1.Clear()
        If Not Tablas.Read("SELECT Remito,Fecha From RemitosCabeza;", Conexion, DtB) Then End
        For Each Row As DataRow In DtB.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Remito FROM RemitosDetalle WHERE Remito = " & Row("Remito") & ";", Conexion, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & Format(Row("Remito"), "0000-00000000") & "  " & Row("Fecha")
            End If
        Next

        Dim DtN As New DataTable

        If Not Tablas.Read("SELECT Remito,Fecha From RemitosCabeza;", ConexionN, DtN) Then End
        For Each Row As DataRow In DtN.Rows
            Dim Dt As New DataTable
            If Not Tablas.Read("SELECT Remito FROM RemitosDetalle WHERE Remito = " & Row("Remito") & ";", ConexionN, Dt) Then End
            If Dt.Rows.Count = 0 Then
                TextBox1.Text = TextBox1.Text & vbCrLf & Format(Row("Remito"), "0000-00000000") & "  " & Row("Fecha")
            End If
        Next

        DtB.Dispose()
        DtN.Dispose()

        MsgBox("Fin")
    End Sub
    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click

        Dim DtB As New DataTable

        If Not Tablas.Read("SELECT * From Tablas;", Conexion, DtB) Then End

        Dim Row As DataRow

        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Impuestos nacionales"
        Row("CodigoAfipElectronico") = 1
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Impuestos provincial"
        Row("CodigoAfipElectronico") = 2
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Impu. municipales"
        Row("CodigoAfipElectronico") = 3
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Impuestos internos"
        Row("CodigoAfipElectronico") = 4
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "IIBB"
        Row("CodigoAfipElectronico") = 5
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Percepción de IVA"
        Row("CodigoAfipElectronico") = 6
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Percepción IIBB"
        Row("CodigoAfipElectronico") = 7
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Perc. Impu. municip."
        Row("CodigoAfipElectronico") = 8
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Otras percepciones"
        Row("CodigoAfipElectronico") = 9
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Perc.Iva no categ."
        Row("CodigoAfipElectronico") = 13
        DtB.Rows.Add(Row)
        '
        Row = DtB.NewRow
        AceraRow(Row)
        Row("Tipo") = 41
        Row("Nombre") = "Otro"
        Row("CodigoAfipElectronico") = 99
        DtB.Rows.Add(Row)

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtB.GetChanges) Then
                    Resul = GrabaTabla(DtB.GetChanges, "Tablas", Conexion)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Sub
                    End If
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        DtB.Dispose()

        MsgBox("Fin")

    End Sub
    Private Sub AceraRow(ByRef Row As DataRow)

        Row("Clave") = 0
        Row("Tipo") = 0
        Row("Nombre") = ""
        Row("CodigoMonedaAfip") = ""
        Row("LetraFactura") = ""
        Row("Iva") = 0
        Row("CodigoRetencion") = 0
        Row("Operador") = 0
        Row("Activo") = 0
        Row("Activo2") = 0
        Row("Activo3") = 0
        Row("Activo4") = 0
        Row("Activo5") = 0
        Row("TipoIva") = 0
        Row("TipoPago") = 0
        Row("TopeMes") = 0
        Row("Cuit") = 0
        Row("AlicuotaRetencion") = 0
        Row("UltimoNumero") = 0
        Row("Cuenta") = 0
        Row("Cuenta2") = 0
        Row("EsPorProvincia") = 0
        Row("Formula") = 0
        Row("AlicuotaRetIngBruto") = 0
        Row("OrigenPercepcion") = 0
        Row("Comentario") = ""
        Row("CodigoAfipElectronico") = 0

    End Sub
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click

        Dim DtProvincia As New DataTable
        Dim HayError As Boolean

        If Not Tablas.Read("SELECT * FROM RecibosRetenciones WHERE Comprobante = 0 AND (TipoNota = 50 or TipoNota = 60 or TipoNota = 70 or TipoNota = 500 or TipoNota = 700);", Conexion, DtProvincia) Then End
        For Each Row As DataRow In DtProvincia.Rows
            Dim Comprobante As Integer = HallaComprobanteRecibo(Row("TipoNota"), Row("Nota"), Row("Retencion"))
            If Comprobante > 0 Then
                Row("Comprobante") = Comprobante
            Else
                HayError = True
            End If
        Next

        If HayError Then
            If MsgBox("Hay errores. Quiere Continuar?", MsgBoxStyle.YesNo) = MsgBoxResult.No Then Exit Sub
        End If

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtProvincia.GetChanges) Then
                    Resul = GrabaTabla(DtProvincia.GetChanges, "RecibosRetenciones", Conexion)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Sub
                    End If
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Termino")

    End Sub
    Private Function HallaComprobanteRecibo(ByVal TipoNota As Integer, ByVal Nota As Integer, ByVal MedioPago As Integer) As Integer

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT Comprobante FROM RecibosDetallePago WHERE TipoNota = " & TipoNota & " AND Nota = " & Nota & " AND MedioPago = " & MedioPago & ";", Conexion, Dt) Then
            MsgBox("Error Lectura Tipo Nota " & TipoNota & " Nota " & Nota & " MedioPago " & MedioPago)
            Return -1000
        End If

        If Dt.Rows.Count = 0 Then
            MsgBox("No se encuentra  Tipo Nota " & TipoNota & " Nota " & Nota & " MedioPago " & MedioPago)
            Return -1000
        End If

        Dim Comprobante As Integer = Dt.Rows(0).Item("Comprobante")

        Dt.Dispose()

        Return Comprobante

    End Function
    Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click

        Dim Dt As New DataTable
        If Not Tablas.Read("SELECT * FROM OtrosProveedores;", Conexion, Dt) Then Exit Sub

        Dim DtProveedores As New DataTable
        If Not Tablas.Read("SELECT * FROM Proveedores;", Conexion, DtProveedores) Then Exit Sub

        For Each Row As DataRow In Dt.Rows
            Dim Row2 As DataRow = DtProveedores.NewRow
            AceraRegistroProveedor(Row2)
            Row2("Nombre") = Row("Nombre")
            Row2("Calle") = Row("Calle")
            Row2("Localidad") = Row("Localidad")
            Row2("Provincia") = Row("Provincia")
            Row2("Pais") = Row("Pais")
            Row2("Telefonos") = Row("Telefonos")
            Row2("Faxes") = Row("Faxes")
            Row2("Cuit") = Row("Cuit")
            Row2("TipoIva") = Row("TipoIva")
            Row2("Estado") = Row("Estado")
            Row2("ExentoRetencion") = Row("ExentoRetencion")
            Row2("SaldoInicial") = Row("SaldoInicial")
            DtProveedores.Rows.Add(Row2)
        Next

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtProveedores.GetChanges) Then
                    Resul = GrabaTabla(DtProveedores.GetChanges, "Proveedores", Conexion)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Sub
                    End If
                End If
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        MsgBox("Proceso Terminado.")

    End Sub
    Private Sub AceraRegistroProveedor(ByRef Row As DataRow)

        Row("Nombre") = ""
        Row("Calle") = ""
        Row("Localidad") = ""
        Row("Provincia") = 0
        Row("Pais") = 0
        Row("CodPostal") = 0
        Row("Telefonos") = ""
        Row("Faxes") = ""
        Row("Cuit") = 0
        Row("Producto") = 0
        Row("TipoOperacion") = 0
        Row("EsCliente") = False
        Row("Directo") = 100
        Row("CondicionPago") = 0
        Row("TipoIva") = 0
        Row("ExentoRetencion") = False
        Row("Comision") = 0
        Row("ComisionAdicional") = 0
        Row("SaldoInicial") = 0
        Row("Cambio") = 1
        Row("Alias") = ""
        Row("Centro") = 0
        Row("Moneda") = 1
        Row("IngresoBruto") = 0
        Row("EsDelGrupo") = False
        Row("Estado") = 0
        Row("ListaDePrecios") = False
        Row("ListaDePreciosPorZona") = False
        Row("EsEgresoCaja") = False
        Row("Opr") = True

    End Sub
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button10.Click

        If Not AgregaNCredito(Conexion) Then End
        If Not AgregaNCredito(ConexionN) Then End

    End Sub
    Private Function AgregaNCredito(ByVal conexionStr As String) As Boolean

        Dim DtB As New DataTable
        Dim DtR As New DataTable
        Dim Sql As String
        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * From RecibosDetalle WHERE TipoNota = 0;", conexionStr, DtR) Then Return False

        If Not Tablas.Read("SELECT * From NotasCreditoCabeza WHERE Estado <> 3;", conexionStr, DtB) Then Return False
        For Each Row As DataRow In DtB.Rows
            Sql = "SELECT * FROM RecibosDetalle WHERE TipoNota = 4 AND Nota = " & Row("NotaCredito") & ";"
            If Not Tablas.Read(Sql, conexionStr, Dt) Then Return False
            If Dt.Rows.Count = 0 Then
                Dim RowA As DataRow = DtR.NewRow
                RowA("TipoNota") = 4
                RowA("Nota") = Row("NotaCredito")
                RowA("TipoComprobante") = 2
                RowA("Comprobante") = Row("Factura")
                RowA("Importe") = Row("Importe") + Row("Percepciones")
                DtR.Rows.Add(RowA)
            End If
        Next

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtR.GetChanges) Then
                    Resul = GrabaTabla(DtR.GetChanges, "RecibosDetalle", conexionStr)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Function
                    End If
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Function
        End Try

        MsgBox("Termino   " & conexionStr)

        Return True

    End Function
    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click

        Dim DtProveedor As New DataTable
        Dim DtTabla As New DataTable

        If Not Tablas.Read("SELECT Clave,Nombre From Proveedores WHERE Producto = 8;", Conexion, DtProveedor) Then End
        If Not Tablas.Read("SELECT * From Tablas;", Conexion, DtTabla) Then End

        Dim RowW As DataRow = DtTabla.NewRow
        AceraRow(RowW)
        RowW("Tipo") = 43
        RowW("Nombre") = "Propio"
        DtTabla.Rows.Add(RowW)

        For Each row As DataRow In DtProveedor.Rows
            RowW = DtTabla.NewRow
            AceraRow(RowW)
            RowW("Tipo") = 43
            RowW("Nombre") = row("nombre")
            RowW("Operador") = row("Clave")
            DtTabla.Rows.Add(RowW)
        Next
        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(DtTabla.GetChanges) Then
                    Resul = GrabaTabla(DtTabla.GetChanges, "Tablas", Conexion)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Sub
                    End If
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Sub
        End Try

        DtTabla.Dispose()
        DtProveedor.Dispose()

        MsgBox("Fin")

    End Sub
    Private Sub ButtonArreglaMerma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonArreglaMerma.Click

        If Not Arreglamerma(Conexion) Then End
        If Not ArreglaMerma(ConexionN) Then End

    End Sub
    Private Function ArreglaMerma(ByVal conexionStr As String) As Boolean

        Dim Dt As New DataTable

        If Not Tablas.Read("SELECT * From Lotes;", conexionStr, Dt) Then Return False
        For Each Row As DataRow In Dt.Rows
            If Row("Secuencia") < 100 Then
                If Row("MermaTr") = 0 Then Row("MermaTr") = -100
            End If
        Next

        Dim Resul As Double = 0

        Dim MitransactionOptions As New TransactionOptions
        'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
        MitransactionOptions.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        MitransactionOptions.Timeout = New TimeSpan(0, 3, 0)
        Try
            Using Scope As New TransactionScope(TransactionScopeOption.Required, MitransactionOptions)
                '
                If Not IsNothing(Dt.GetChanges) Then
                    Resul = GrabaTabla(Dt.GetChanges, "Lotes", conexionStr)
                    If Resul <= 0 Then
                        MsgBox("Error " & Resul)
                        Exit Function
                    End If
                End If
                '
                Scope.Complete()
            End Using
        Catch ex As TransactionException
            MsgBox(ex.Message)
            Exit Function
        End Try

        MsgBox("Termino   " & conexionStr)

        Return True

    End Function
End Class