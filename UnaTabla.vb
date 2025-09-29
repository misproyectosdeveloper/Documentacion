Imports ClassPassWord
Public Class UnaTabla
    Public Ptipo As Integer
    Public PBloqueaFunciones As Boolean
    ' 
    Dim Dt As DataTable
    Private WithEvents bs As New BindingSource
    '
    Dim TablaIva(0) As Double
    Dim EsTabla As Boolean = True
    Private Sub UnaTabla_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Grid.AutoGenerateColumns = False

        Grid.Columns("LupaVenta").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")
        Grid.Columns("LupaCompra").DefaultCellStyle.NullValue = ImageList1.Images.Item("Lupa")

        LlenaCombosGrid()

        If Ptipo = 1 Then
            LabelTitulo.Text = "Especies"
            Grid.Columns("Iva").Visible = True
            Grid.Columns("Operador").Visible = True
            Grid.Columns("Operador").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Grid.Columns("Operador").MinimumWidth = 100
        End If
        If Ptipo = 2 Then
            LabelTitulo.Text = "Variedades"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 3 Then
            LabelTitulo.Text = "Marcas"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 4 Then
            LabelTitulo.Text = "Categorias"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 5 Then
            LabelTitulo.Text = "Calibres"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 6 Then
            LabelTitulo.Text = "Artículos Logísticos"
            Grid.Columns("Iva").Visible = True
        End If
        If Ptipo = 19 Then
            LabelTitulo.Text = "Depositos"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 20 Then
            LabelTitulo.Text = "Depositos de Insumos"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 22 Then
            LabelTitulo.Text = "IVAs. del Sistema"
            Grid.Columns("Iva").Visible = True
            Grid.Columns("Iva").HeaderText = "Alícuota"
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Activo").Visible = True
            Grid.Columns("Activo").HeaderText = "Activo Factura Proveedor"
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("Cuenta").HeaderText = "Cta.Venta"
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Cuenta2").Visible = True
            Grid.Columns("Cuenta2").HeaderText = "Cta.Compra"
            Grid.Columns("LupaCompra").Visible = True
            '-------Es para Codigo impuesto afip--------------- 
            Grid.Columns("OperadorInt").Visible = True
            Grid.Columns("OperadorInt").HeaderText = "Código AFIP"
            Grid.Columns("OperadorInt").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Grid.Columns("OperadorInt").MinimumWidth = 50
            Grid.Columns("OperadorInt").Width = 50
            DirectCast(Grid.Columns("OperadorInt"), DataGridViewTextBoxColumn).MaxInputLength = 3
            DirectCast(Grid.Columns("OperadorInt"), DataGridViewTextBoxColumn).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '-----------------------------------------------------
        End If
        If Ptipo = 23 Then
            LabelTitulo.Text = "Canal de Venta"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 25 Then
            LabelTitulo.Text = "Tabla Retenciones"  ' Tratada en UnaTablaRetenciones.
        End If
        If Ptipo = 26 Then
            LabelTitulo.Text = "Bancos"
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Cuit").HeaderText = "Cuit"
            Grid.Columns("Cuit").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Grid.Columns("Cuit").Visible = True
            Grid.Columns("Iva").Visible = False
            If PermisoTotal Then
                Grid.Columns("Activo2").Visible = True
                Grid.Columns("Activo2").HeaderText = "Cerrado"
            End If
        End If
        If Ptipo = 27 Then
            LabelTitulo.Text = "Monedas Extranjeras"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("CodigoMonedaAfip").Visible = True
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
        End If
        If Ptipo = 28 Then
            LabelTitulo.Text = "Paises"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Cuit").Visible = True
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Cuit").HeaderText = "Cuit Pais"
        End If
        If Ptipo = 29 Then
            LabelTitulo.Text = "Conceptos de Gastos Factura Proveedor"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 30 Then
            LabelTitulo.Text = "Unidades de Negocio"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Nombre").MinimumWidth = 150
            Dim col As DataGridViewTextBoxColumn
            col = Grid.Columns("Nombre")
            col.MaxInputLength = 30
            Grid.Columns("Centro").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Comentario").Visible = False
            EsTabla = False
        End If
        If Ptipo = 31 Then
            LabelTitulo.Text = "Provincias Y Datos Retención Ingreso Bruto"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("AlicuotaretIngBruto").Visible = True
            DirectCast(Grid.Columns("AlicuotaretIngBruto"), DataGridViewTextBoxColumn).MaxInputLength = 6
            DirectCast(Grid.Columns("AlicuotaretIngBruto"), DataGridViewTextBoxColumn).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '-------Es para jurisdiccion Ing.Bruto.--------------- 
            Grid.Columns("OperadorInt").Visible = True
            Grid.Columns("OperadorInt").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Grid.Columns("OperadorInt").MinimumWidth = 90
            Grid.Columns("CodProvinciaIIBB").Width = 90
            DirectCast(Grid.Columns("OperadorInt"), DataGridViewTextBoxColumn).MaxInputLength = 3
            DirectCast(Grid.Columns("OperadorInt"), DataGridViewTextBoxColumn).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Grid.Columns("CodProvinciaIIBB").Visible = True
            Grid.Columns("CodProvinciaIIBB").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            Grid.Columns("CodProvinciaIIBB").MinimumWidth = 90
            Grid.Columns("CodProvinciaIIBB").Width = 90
            DirectCast(Grid.Columns("CodProvinciaIIBB"), DataGridViewTextBoxColumn).MaxInputLength = 3
            DirectCast(Grid.Columns("CodProvinciaIIBB"), DataGridViewTextBoxColumn).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            '-----------------------------------------------------
            Grid.Columns("Comentario").Visible = False
        End If
        If Ptipo = 32 Then
            LabelTitulo.Text = "Conceptos Gastos de Prestamos"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Activo2").Visible = True
            Grid.Columns("Activo2").HeaderText = "Gravado"
        End If
        If Ptipo = 33 Then
            LabelTitulo.Text = "Gastos Bancarios"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Activo2").Visible = True
            Grid.Columns("Activo2").HeaderText = "Gravado"
            Grid.Columns("Activo3").Visible = True
            Grid.Columns("Activo3").HeaderText = "No Incluir en Libro IVA-Compra"
            Grid.Columns("Activo3").MinimumWidth = 90
            Grid.Columns("Activo3").Width = 90
            Grid.Columns("SumaResta").Visible = True
        End If
        If Ptipo = 34 Then
            LabelTitulo.Text = "Conceptos Netos Para Sueldos"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("SumaResta").Visible = True
            Grid.Columns("SumaResta").DisplayIndex = 2   'cambia orden en el grid.
            DirectCast(Grid.Columns("SumaResta"), DataGridViewComboBoxColumn).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            DirectCast(Grid.Columns("SumaResta"), DataGridViewComboBoxColumn).MinimumWidth = 30
            DirectCast(Grid.Columns("SumaResta"), DataGridViewComboBoxColumn).Width = 60
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Activo5").Visible = True
            Grid.Columns("Activo5").HeaderText = "Remu."
            Grid.Columns("Activo5").DisplayIndex = 3   'cambia orden en el grid.
            Grid.Columns("Activo").Visible = True
            Grid.Columns("Activo").HeaderText = "Descuento"
            Grid.Columns("Activo2").Visible = True
            Grid.Columns("Activo2").HeaderText = "No Remu."
            Grid.Columns("Activo2").DisplayIndex = 4   'cambia orden en el grid.
            Grid.Columns("Activo4").Visible = True
            Grid.Columns("Activo4").HeaderText = "Habitual"
            Grid.Columns("Activo3").Visible = True
            Grid.Columns("Activo3").HeaderText = "Contiene Unidades"
            Grid.Columns("UltimoNumero").Visible = True
            Grid.Columns("UltimoNumero").HeaderText = "Codigo Externo"
            DirectCast(Grid.Columns("UltimoNumero"), DataGridViewTextBoxColumn).AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            DirectCast(Grid.Columns("UltimoNumero"), DataGridViewTextBoxColumn).Width = 50
            DirectCast(Grid.Columns("UltimoNumero"), DataGridViewTextBoxColumn).MaxInputLength = 4
            Mensaje.Text = "Habitual: Aparece Automáticamente en Recibos de Sueldos. Remu.: Remunerativo. NO-Remu.: No Remunerativo. "
            Mensaje.Visible = True
        End If
        If Ptipo = 36 Then
            LabelTitulo.Text = "Conceptos Facturas Otros Proveedores"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("SumaResta").Visible = True
            Grid.Columns("Comentario").Visible = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("TipoPago").Visible = True
        End If
        If Ptipo = 37 Then
            LabelTitulo.Text = "Vendedores"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Clave").Visible = True
            Grid.Columns("Clave").ReadOnly = True
        End If
        If Ptipo = 38 Then
            LabelTitulo.Text = "INCOTERM"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 39 Then
            LabelTitulo.Text = "Tipo Pago Otros Proveedores"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
        End If
        If Ptipo = 40 Then
            LabelTitulo.Text = "Zonas para Lista de Precios"
            Grid.Columns("Iva").Visible = False
            Dim col As DataGridViewTextBoxColumn
            col = Grid.Columns("Nombre")
            col.MaxInputLength = 12
        End If
        If Ptipo = 41 Then
            LabelTitulo.Text = "Códigos AFIP Reten/Perc. Para Comp. Electrónicos"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("CodigoAfipElectronico").Visible = True
            Grid.Columns("CodigoAfipElectronico").HeaderText = "Codigo AFIP"
        End If
        If Ptipo = 43 Then
            LabelTitulo.Text = "Transporte"  ' Tratada en UnaTablaTransporte complementos del sistema.
        End If
        If Ptipo = 44 Then
            LabelTitulo.Text = "Usuarios Remito electronicos"  ' Tratada en UnaTablaUsuarios complementos del sistema.
        End If
        If Ptipo = 45 Then
            LabelTitulo.Text = "Canal De Distribución"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 46 Then
            LabelTitulo.Text = "Obras Sociales"
            Grid.Columns("Iva").Visible = False
        End If
        If Ptipo = 1000 Then
            LabelTitulo.Text = "Insumos"
            Me.BackColor = Color.Thistle
            EsTabla = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
        End If
        If Ptipo = 2000 Then
            LabelTitulo.Text = "CENTROS DE COSTOS DE TIPOS DE OPERACIONES"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Centro").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Comentario").Visible = False
            ButtonEliminar.Visible = False
            Grid.Columns("Nombre").ReadOnly = True
            Grid.AllowUserToAddRows = False
            EsTabla = False
        End If
        If Ptipo = 3000 Then
            LabelTitulo.Text = "CUENTAS DE MEDIOS DE PAGO"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Cuenta").Visible = True
            Grid.Columns("LupaVenta").Visible = True
            Grid.Columns("Comentario").Visible = False
            ButtonEliminar.Visible = False
            Grid.Columns("Nombre").ReadOnly = True
            Grid.AllowUserToAddRows = False
            EsTabla = False
        End If
        If Ptipo = 4000 Then
            LabelTitulo.Text = "MAESTRO DE FONDOS FIJO"
            Grid.Columns("Iva").Visible = False
            Grid.Columns("Comentario").Visible = False
            EsTabla = False
        End If

        LLenaGrid()

        ArmaTablaIva(TablaIva)

        If bs.Count > 0 Then Grid.FirstDisplayedScrollingRowIndex = bs.Position

        AddHandler Dt.ColumnChanging, New DataColumnChangeEventHandler(AddressOf Dt_ColumnChanging)
        AddHandler Dt.RowChanging, New DataRowChangeEventHandler(AddressOf Dt_RowChanging)
        AddHandler Dt.TableNewRow, New DataTableNewRowEventHandler(AddressOf Dt_NewRow)
        AddHandler Dt.RowDeleting, New DataRowChangeEventHandler(AddressOf Dt_Deleting)
        AddHandler Dt.RowChanged, New DataRowChangeEventHandler(AddressOf Dt_RowChanged)

    End Sub
    Private Sub UnaTabla_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If Not IsNothing(Dt.GetChanges) Then
            If MsgBox("Los cambios no fueron Actualizados. Quiere Actualizarlos?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.Yes Then
                ButtonAceptar_Click(Nothing, Nothing)
                If Not GModificacionOk Then e.Cancel = True : Exit Sub
            End If
        End If

        Me.Dispose()

    End Sub
    Private Sub ButtonAceptar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAceptar.Click

        If PBloqueaFunciones Then
            MsgBox("Función Bloqueda. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Exit Sub
        End If

        bs.EndEdit()

        If IsNothing(Dt.GetChanges) Then Exit Sub

        If Dt.HasErrors Then
            MsgBox("Debe Corregir errores antes de Realizar los Cambios.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Exit Sub
        End If

        If GGeneraAsiento And Grid.Columns("Cuenta").Visible Then
            For Each Row As DataRow In Dt.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row.RowError = ""
                    If Row("Cuenta") = 0 Then
                        Row.RowError = "Error"
                    End If
                End If
            Next
            If Dt.HasErrors Then
                MsgBox("Falta Informar Cuenta.")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Cuenta2").Visible Then
            For Each Row As DataRow In Dt.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row.RowError = ""
                    If Row("Cuenta") = 0 Then
                        Row.RowError = "Error"
                    End If
                End If
            Next
            If Dt.HasErrors Then
                MsgBox("Falta Informar Cuenta.")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Centro").Visible Then
            For Each Row As DataRow In Dt.Rows
                If Row.RowState <> DataRowState.Deleted Then
                    Row.RowError = ""
                    If Row("Centro") = 0 Then
                        Row.RowError = "Error"
                    End If
                End If
            Next
            If Dt.HasErrors Then
                MsgBox("Falta Informar Centro de Costo.")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        For Each Row As DataRow In Dt.Rows
            If Row.RowState = DataRowState.Added Then
                'If IsDBNull(Row("Comentario")) Then Row("Comentario") = ""
                'If IsDBNull(Row("Iva")) Then Row("Iva") = 0
                If Ptipo <> 1000 And Ptipo <> 30 And Ptipo <> 4000 Then Row("Tipo") = Ptipo
            End If
        Next

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Dim Trans As OleDb.OleDbTransaction

        GModificacionOk = False

        Using MiConexion As New OleDb.OleDbConnection(Conexion)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Dim Sql1 As String
                    Select Case Ptipo
                        Case 30
                            Sql1 = "SELECT * FROM Proveedores;"
                        Case 1000
                            Sql1 = "SELECT * FROM Insumos;"
                        Case 2000
                            Sql1 = "SELECT * FROM Miselaneas;"
                        Case 3000
                            Sql1 = "SELECT * FROM Miselaneas;"
                        Case 4000
                            Sql1 = "SELECT * FROM MaestroFondoFijo;"
                        Case Else
                            Sql1 = "SELECT * FROM Tablas;"
                    End Select
                    Using DaP As New OleDb.OleDbDataAdapter(Sql1, MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(Dt.GetChanges)
                    End Using
                    Trans.Commit()
                    MsgBox("Cambios Realizados Exitosamente.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                Catch ex As DBConcurrencyException
                    MsgBox("Error,Otro Usuario modifico Datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Trans.Rollback()
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                MsgBox("Error de Base de datos. Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            End Try
        End Using

        Me.Cursor = System.Windows.Forms.Cursors.Default

        UnaTabla_Load(Nothing, Nothing)

    End Sub
    Private Sub ButtonEliminar_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonEliminar.Click

        If bs.Current Is Nothing Then
            MsgBox("No hay registro actual para eliminar", MsgBoxStyle.Exclamation, "Eliminar")
            Exit Sub
        End If

        Dim Row As DataRowView = bs.Current

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        If Row("Clave") <> 0 Then
            If Ptipo = 1 Then
                If EspecieUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 2 Then
                If VariedadUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 3 Then
                If MarcaUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 4 Then
                If CategoriaUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 5 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If CalibreUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If CalibreUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
            End If
            If Ptipo = 19 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If DepositoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If DepositoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 20 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If DepositoInsumoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If DepositoInsumoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 22 Then
                If IvaUsado(Grid.CurrentRow.Cells("Clave").Value, Grid.CurrentRow.Cells("IVA").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 23 Then
                If CanalDeVentaUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 26 Then
                If BancoEnCuentaUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If BancoEnCuentaUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If BancoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If BancoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 27 Then
                If MonedaUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usada. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 28 Then
                If PaisUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 29 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If GastoFacturaProveedorUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If GastoFacturaProveedorUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 30 Then
                If UnidadNegocioUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 31 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If ProvinciaIngresoMercaderiaUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If ProvinciaIngresoMercaderiaUsada(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If ProvinciaUsada(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 32 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If GastoPrestamoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If GastoPrestamoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 33 Then
                If GastoBancarioUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 34 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If ConceptoSueldoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If ConceptoSueldoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 36 Then                           'revisado.
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If ConceptoOtrosProveedorUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If ConceptoOtrosProveedorUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 37 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If VendedorUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If VendedorUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 38 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If IncotermUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If IncotermUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 39 Then                        'revisado.
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If TipoOtrosPagoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If TipoOtrosPagoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Item esta siendo usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 40 Then
                If ZonaUsada(Grid.CurrentRow.Cells("Clave").Value) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 41 Then
                If CodigoAFIPUsado(Grid.CurrentRow.Cells("CodigoAfipElectronico").Value) Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
            If Ptipo = 45 Then
                If CanalDistribucionUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Item  esta siendo Usado. Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If

            If Ptipo = 1000 Then
                If GUsaNegra And Not PermisoTotal Then
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    MsgBox("ERROR Usuario no esta Autorizado para esta Funcion(1000). Operación se CANCELA.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                    Exit Sub
                End If
                If InsumoUsado(Grid.CurrentRow.Cells("Clave").Value, Conexion) Then
                    MsgBox("El Insumo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
                If InsumoUsado(Grid.CurrentRow.Cells("Clave").Value, ConexionN) Then
                    MsgBox("El Insumo esta siendo usado.Operación se CANCELA.", MsgBoxStyle.Exclamation + MsgBoxStyle.DefaultButton3)
                    Me.Cursor = System.Windows.Forms.Cursors.Default
                    Exit Sub
                End If
            End If
        End If

        If Row("Clave") <> 0 Then
            If MsgBox("El Item se eliminara definitivamente. ¿Desea eliminarlo?.", MsgBoxStyle.Question + MsgBoxStyle.YesNo + MsgBoxStyle.DefaultButton3) = MsgBoxResult.No Then
                Me.Cursor = System.Windows.Forms.Cursors.Default
                Exit Sub
            End If
        End If

        bs.RemoveCurrent()

        Me.Cursor = System.Windows.Forms.Cursors.Default

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

        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor

        Select Case Ptipo
            Case 30
                If Not Tablas.Read("SELECT * FROM Proveedores WHERE TipoOperacion = 4 ORDER BY Nombre;", Conexion, Dt) Then End : Exit Sub
            Case 1000
                If Not Tablas.Read("SELECT * FROM Insumos ORDER BY Nombre;", Conexion, Dt) Then End : Exit Sub
            Case 2000
                If Not Tablas.Read("SELECT * FROM Miselaneas WHERE Codigo = 1 ORDER BY Nombre;", Conexion, Dt) Then End : Exit Sub
            Case 3000
                If Not Tablas.Read("SELECT * FROM Miselaneas WHERE Codigo = 2 ORDER BY Nombre;", Conexion, Dt) Then End : Exit Sub
            Case 4000
                If Not Tablas.Read("SELECT * FROM MaestroFondoFijo ORDER BY Nombre;", Conexion, Dt) Then End : Exit Sub
            Case Else
                If Not Tablas.Read("SELECT * FROM Tablas WHERE Tipo = " & Ptipo & ";", Conexion, Dt) Then End : Exit Sub
        End Select

        Grid.DataSource = bs
        bs.DataSource = Dt

        Me.Cursor = System.Windows.Forms.Cursors.Default

    End Sub
    Private Function InsumoUsado(ByVal Insumo As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Articulo) FROM OrdenCompraDetalle WHERE Articulo = " & Insumo & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function EspecieUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Articulos WHERE Especie = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function VariedadUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Articulos WHERE Variedad = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function MarcaUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Articulos WHERE Marca = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function CalibreUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Calibre) FROM Lotes WHERE Calibre = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function DepositoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM Lotes WHERE Deposito = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function DepositoInsumoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Ingreso) FROM IngresoInsumoCabeza WHERE Deposito = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function CategoriaUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Articulos WHERE Categoria = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function IvaUsado(ByVal Clave As Integer, ByVal Iva As Double, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(CodigoIva) FROM RecibosCabeza WHERE CodigoIva = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alicuota) FROM RecibosDetallePago WHERE Alicuota = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Iva) FROM Articulos WHERE Iva = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Iva) FROM ArticulosServicios WHERE IVA = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Iva) FROM Insumos WHERE Iva = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alicuota) FROM PrestamosMovimientoPago WHERE Alicuota = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alicuota) FROM RecibosDetallePago WHERE Alicuota = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM GastosBancarioDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alicuota) FROM LiquidacionCabeza WHERE Alicuota = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(AlicuotaComision) FROM LiquidacionCabeza WHERE AlicuotaComision = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(AlicuotaDescarga) FROM LiquidacionCabeza WHERE AlicuotaDescarga = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Alicuota) FROM NVLPDetalle WHERE (Impuesto = 3 or Impuesto = 6 or Impuesto = 8 or Impuesto = 11 or Impuesto = 12) AND Alicuota = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Iva) FROM OrdenCompraDetalle WHERE Iva = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Iva) FROM Tablas WHERE Tipo = 1 AND Iva = " & Iva & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function CanalDeVentaUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(CanalVenta) FROM Clientes WHERE CanalVenta = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function CanalDistribucionUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(CanalDistribucion) FROM Clientes WHERE CanalDistribucion = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function BancoEnCuentaUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                If ConexionStr = Conexion Then
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM CuentasBancarias WHERE Banco = " & Clave & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                End If
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM MovimientosBancarioCabeza WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM GastosBancarioCabeza WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function BancoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM RecibosDetallePago WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM SueldosMovimientoPago WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM PrestamosMovimientoPago WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM PrestamosDetalle WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Banco) FROM OtrosPagosPago WHERE Banco = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function MonedaUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Moneda) FROM Proveedores WHERE Moneda = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Moneda) FROM Clientes WHERE Moneda = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function PaisUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Proveedores WHERE Pais = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Clientes WHERE Pais = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
                Return False
            End Try
        End Using

    End Function
    Private Function GastoBancarioUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM GastosBancarioDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ConceptoSueldoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM RecibosSueldosDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ConceptoOtrosProveedorUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM OtrasFacturasDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function VendedorUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Vendedor) FROM FacturasCabeza WHERE Vendedor = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function IncotermUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(IncoTerm) FROM FacturasProveedorCabeza WHERE IncoTerm = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(IncoTerm) FROM FacturasCabeza WHERE IncoTerm = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function TipoOtrosPagoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                If ConexionStr = Conexion Then
                    Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(TipoPago) FROM Tablas WHERE Tipo = 36 AND TipoPago = " & Clave & ";", Miconexion)
                        If Cmd.ExecuteScalar() <> 0 Then Return True
                    End Using
                End If
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(TipoPago) FROM OtrasFacturasCabeza WHERE TipoPago = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(TipoPago) FROM OtrosPagosCabeza WHERE TipoPago = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox(ex.Message)
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function UnidadNegocioUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Negocio) FROM Costeos WHERE Negocio = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function GastoFacturaProveedorUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(ConceptoGasto) FROM FacturasProveedorCabeza WHERE ConceptoGasto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ProvinciaIngresoMercaderiaUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lote) FROM IngresoMercaderiasCabeza WHERE Provincia = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ProvinciaUsada(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Provincia) FROM Clientes WHERE Provincia = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Provincia) FROM Proveedores WHERE Provincia = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function GastoPrestamoUsado(ByVal Clave As Integer, ByVal ConexionStr As String) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Concepto) FROM PrestamosMovimientoDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ConceptoOtrasFacturasUsado(ByVal Clave As Integer) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM OtrasFacturasDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

        Using Miconexion As New OleDb.OleDbConnection(ConexionN)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Factura) FROM OtrasFacturasDetalle WHERE Concepto = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then Return True
                End Using
                Return False
            Catch ex2 As Exception
                MsgBox("Error, Base de Datos.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Function ZonaUsada(ByVal Clave As Integer) As Boolean


        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM SucursalesClientes WHERE Zona = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then MsgBox("El Item esta siendo usado por sucursal de Cliente. Operación se CANCELA.") : Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: SucursalesClientes.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using
        '
        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM SucursalesProveedores WHERE Zona = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then MsgBox("El Item esta siendo usado por sucursal de proveedor. Operación se CANCELA.") : Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: SucursalesProveedores.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using
        '
        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lista) FROM ListaDePreciosCabeza WHERE Zona = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then MsgBox("El Item esta siendo usado por lista de precios Cliente. Operación se CANCELA.") : Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: ListaDePreciosCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using
        '
        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Lista) FROM ListaDePreciosProveedoresCabeza WHERE Zona = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then MsgBox("El Item esta siendo usado por lista de precios Proveedor. Operación se CANCELA.") : Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: ListaDePreciosProveedoresCabeza.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using


        Return False

    End Function
    Private Function CodigoAFIPUsado(ByVal Clave As Integer) As Boolean

        Using Miconexion As New OleDb.OleDbConnection(Conexion)
            Try
                Miconexion.Open()
                Using Cmd As New OleDb.OleDbCommand("SELECT COUNT(Clave) FROM Tablas WHERE Tipo = 25 AND CodigoAfipElectronico = " & Clave & ";", Miconexion)
                    If Cmd.ExecuteScalar() <> 0 Then MsgBox("Código AFIP esta siendo usado por una Percepción o retención. Operación se CANCELA.") : Return True
                End Using
            Catch ex As Exception
                MsgBox("Error, Base de Datos. Tabla: Tablas.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
                End
            End Try
        End Using

    End Function
    Private Sub LlenaCombosGrid()

        Dim Row As DataRow
        Dim Dt As New DataTable

        Dt.Columns.Add("Clave", Type.GetType("System.Int32"))
        Dt.Columns.Add("Nombre", Type.GetType("System.String"))

        Row = Dt.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 1
        Row("Nombre") = "Suma"
        Dt.Rows.Add(Row)
        ' 
        Row = Dt.NewRow
        Row("Clave") = 2
        Row("Nombre") = "Resta"
        Dt.Rows.Add(Row)
        '
        SumaResta.DataSource = Dt
        SumaResta.DisplayMember = "Nombre"
        SumaResta.ValueMember = "Clave"

        TipoPago.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 39;")
        Row = TipoPago.DataSource.NewRow
        Row("Clave") = 0
        Row("Nombre") = " "
        TipoPago.DataSource.Rows.Add(Row)
        TipoPago.DisplayMember = "Nombre"
        TipoPago.ValueMember = "Clave"

    End Sub
    ' ---------------------------------------------------------------------------------------------------------------------------------
    ' -------------------------              MANEJO DEL GRID.                  --------------------------------------------------------
    ' ---------------------------------------------------------------------------------------------------------------------------------
    Private Sub Grid_CellEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellEnter

        If Not Grid.Columns(e.ColumnIndex).ReadOnly Then
            Grid.BeginEdit(True)
        End If

    End Sub
    Private Sub Grid_CellFormatting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellFormattingEventArgs) Handles Grid.CellFormatting

        If e.ColumnIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "Iva" Or Grid.Columns(e.ColumnIndex).Name = "AlicuotaRetIngBruto" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, GDecimales)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Clave" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = FormatNumber(e.Value, 0)
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" And Ptipo <> 28 Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "00-00000000-0")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuit" And Ptipo = 28 Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then e.Value = Format(e.Value, "#")
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "CodigoAfipElectronico" And Ptipo = 31 Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Cuenta" Or Grid.Columns(e.ColumnIndex).Name = "Cuenta2" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "000-000000-00")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Centro" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "000")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "Operador" Or Grid.Columns(e.ColumnIndex).Name = "OperadorInt" Then
            If Not IsDBNull(e.Value) Then
                If e.Value = 0 Then
                    e.Value = Format(e.Value, "#")
                Else
                    e.Value = Format(e.Value, "000")
                End If
            End If
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaVenta" Or Grid.Columns(e.ColumnIndex).Name = "LupaCompra" Then
            e.Value = Nothing
        End If

    End Sub
    Private Sub Grid_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles Grid.CellDoubleClick

        If e.ColumnIndex < 0 Or e.RowIndex < 0 Then Exit Sub

        If Grid.Columns(e.ColumnIndex).Name = "LupaVenta" And (Ptipo = 30 Or Ptipo = 2000) Then
            SeleccionarVarios.PCentro = Grid.Rows(e.RowIndex).Cells("Centro").Value
            SeleccionarVarios.PEsCentroCosto = True
            SeleccionarVarios.ShowDialog()
            If SeleccionarVarios.PCentro <> Grid.Rows(e.RowIndex).Cells("Centro").Value Then
                Dim Row As DataRowView = bs.Current
                Row("Centro") = SeleccionarVarios.PCentro
                Grid.Refresh()
            End If
            SeleccionarVarios.Dispose()
            Exit Sub
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaVenta" And Ptipo <> 30 Then
            SeleccionarCuenta.PCentro = 0
            SeleccionarCuenta.ShowDialog()
            If SeleccionarCuenta.PCuenta <> 0 Then
                Dim Row As DataRowView = bs.Current
                Row("Cuenta") = SeleccionarCuenta.PCuenta
                Grid.Refresh()
            End If
            SeleccionarCuenta.Dispose()
        End If

        If Grid.Columns(e.ColumnIndex).Name = "LupaCompra" Then
            SeleccionarCuenta.PCentro = 0
            SeleccionarCuenta.ShowDialog()
            If SeleccionarCuenta.PCuenta <> 0 Then
                Dim Row As DataRowView = bs.Current
                Row("Cuenta2") = SeleccionarCuenta.PCuenta
                Grid.Refresh()
            End If
            SeleccionarCuenta.Dispose()
        End If

    End Sub
    Private Sub Grid_EditingControlShowing(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewEditingControlShowingEventArgs) Handles Grid.EditingControlShowing

        Dim columna As Integer = Grid.CurrentCell.ColumnIndex

        If Grid.Columns(columna).Name = "TipoIva" Or Grid.Columns(columna).Name = "SumaResta" Or Grid.Columns(columna).Name = "TipoPago" _
                                     Or Grid.Columns(columna).Name = "Operador" Then Exit Sub 'Se saltea ComboBox.(para que no cancele).

        Dim Texto As TextBox = CType(e.Control, TextBox)
        AddHandler Texto.KeyPress, AddressOf ValidaKey_KeyPress
        AddHandler Texto.TextChanged, AddressOf ValidaChanged_TextChanged

    End Sub
    Private Sub ValidaKey_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Iva" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "AlicuotaRetIngBruto" Then
            If e.KeyChar = "." Then e.KeyChar = ","
            If InStr("0123456789," & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Clave" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Cuit" Or _
            Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "OperadorInt" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "CodProvinciaIIBB" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Ptipo = 34 And Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "UltimoNumero" Then
            If InStr("0123456789" & Chr(8), e.KeyChar) = 0 Then e.KeyChar = ""
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Nombre" And (Ptipo = 1 Or Ptipo = 2 Or Ptipo = 3 Or Ptipo = 4 Or Ptipo = 5) Then
            e.KeyChar = e.KeyChar.ToString.ToUpper()
        End If

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "LetraFactura" Then
            e.KeyChar = e.KeyChar.ToString.ToUpper()
        End If

    End Sub
    Private Sub ValidaChanged_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        If Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "Iva" Or Grid.Columns.Item(Grid.CurrentCell.ColumnIndex).Name = "AlicuotaRetIngBruto" Then
            If CType(sender, TextBox).Text <> "" Then
                If Not IsNumeric(CType(sender, TextBox).Text) Then
                    CType(sender, TextBox).Text = ""
                    CType(sender, TextBox).Focus()
                End If
            End If
        End If

    End Sub
    Private Sub Grid_DataError(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewDataErrorEventArgs) Handles Grid.DataError
        Exit Sub
    End Sub
    Private Sub Dt_NewRow(ByVal sender As Object, ByVal e As DataTableNewRowEventArgs)

        e.Row("Clave") = 0
        e.Row("Nombre") = ""
        Select Case Ptipo
            Case 30
                ArmaNuevoProveedor(e.Row)
                e.Row("TipoOperacion") = 4
                e.Row("Producto") = Fruta
            Case 1000
                e.Row("Iva") = 0
                e.Row("Cuenta") = 0
                e.Row("Comentario") = ""
            Case 4000
                e.Row("Nombre") = ""
            Case Else
                AceraRowTablas(e.Row)
        End Select

    End Sub
    Private Sub Dt_ColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        e.Row.ClearErrors()

        If e.Column.ColumnName.Equals("Nombre") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            e.ProposedValue = e.ProposedValue.ToString.Trim
            If e.ProposedValue = "" Then Exit Sub
            Dim RowsBusqueda() As DataRow
            If EsTabla Then
                RowsBusqueda = Dt.Select("Tipo = " & Ptipo & " AND Nombre = '" & e.ProposedValue & "'")
            Else : RowsBusqueda = Dt.Select("Nombre = '" & e.ProposedValue & "'")
            End If
            If RowsBusqueda.Length > 0 Then
                MsgBox("Nombre Repetido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Nombre")
            End If
        End If

        If e.Column.ColumnName.Equals("Iva") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue > 100 Then
                MsgBox("Iva Invalido.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("Iva")
            End If
        End If

        If e.Column.ColumnName.Equals("AlicuotaRetIngBruto") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue > 100 Then
                MsgBox("Alicuota Retención Ing. Bruto.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("AlicuotaRetIngBruto")
            End If
        End If

        If e.Column.ColumnName.Equals("LetraFactura") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = ""
            If e.ProposedValue = "" Then Exit Sub
            If e.ProposedValue <> "A" And e.ProposedValue <> "B" And e.ProposedValue <> "C" And e.ProposedValue <> "E" Then
                MsgBox("Letra para Facturas Invalida.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                e.ProposedValue = e.Row("LetraFactura")
            End If
        End If

        If e.Column.ColumnName.Equals("CodigoMonedaAfip") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

        If e.Column.ColumnName.Equals("Centro") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cuenta") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cuenta2") Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Cuit") And Ptipo <> 28 Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
            If e.ProposedValue <> 0 Then
                Dim aa As New DllVarias
                If Not aa.ValidaCuiT(e.ProposedValue.ToString) Then
                    MsgBox("Cuit Erroneo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    e.ProposedValue = e.Row("Cuit")
                End If
            End If
        End If

        If e.Column.ColumnName.Equals("Cuit") And Ptipo = 28 Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("CodigoProvinciaIIBB") And Ptipo = 31 Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
            If e.ProposedValue <> "" Then
                e.ProposedValue = RellenarCeros(e.ProposedValue, 3)
            End If
        End If

        If e.Column.ColumnName.Equals("UltimoNumero") And Ptipo = 34 Then
            If IsDBNull(e.ProposedValue) Then e.ProposedValue = 0
        End If

        If e.Column.ColumnName.Equals("Comentario") Then
            If IsNothing(e.ProposedValue) Then e.ProposedValue = ""
        End If

    End Sub
    Private Sub Dt_RowChanging(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        If e.Row.HasErrors Then Exit Sub

        e.Row.RowError = ""

        If e.Row("Nombre") = "" Then
            e.Row.RowError = "Error."
            MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
            Grid.Refresh()
            Exit Sub
        End If

        If Ptipo = 22 Then
            If e.Row("Iva") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Alícuota.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Ptipo = 24 Then
            If e.Row("LetraFactura").ToString.Trim = "" Then
                e.Row.RowError = "Error."
                MsgBox("Falta Letra Factura.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Grid.Columns("Iva").Visible And Ptipo <> 22 Then
            For Each Item As Double In TablaIva
                If Item = e.Row("Iva") Then Exit Sub
            Next
            e.Row.RowError = "Error."
            MsgBox("Alicuota no Existe.")
            Grid.Refresh()
            Exit Sub
        End If

        If Ptipo = 26 Then
            If ValidaStringNombres(e.Row("Nombre")) <> "" Then
                e.Row.RowError = "Error."
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Ptipo = 27 Then
            If e.Row("CodigoMonedaAfip").ToString.Trim <> "" Then
                If e.Row("CodigoMonedaAfip").ToString.Trim = "" Then
                    e.Row.RowError = "Error."
                    MsgBox("Falta Codigo Moneda Afip.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Grid.Refresh()
                    Exit Sub
                End If
                If e.Row("CodigoMonedaAfip").ToString.Length <> 3 Then
                    e.Row.RowError = "Error."
                    MsgBox("Codigo Moneda Afip debe tener 3 posiciones.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                    Grid.Refresh()
                    Exit Sub
                End If
            End If
        End If

        If Ptipo = 34 Or Ptipo = 36 Or Ptipo = 33 Then
            If e.Row("Operador") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Suma o Resta en Operador.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Ptipo = 36 Then
            If e.Row("TipoPago") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Tipo Pago.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Ptipo = 41 Then
            If e.Row("CodigoAfipElectronico") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Código AFIP.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
            If e.Row("Nombre") = "" Then
                e.Row.RowError = "Error."
                MsgBox("Falta Nombre.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If Grid.Columns("Cuit").Visible Then
            If e.Row("Cuit") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Cuit.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Centro").Visible Then
            If e.Row("Centro") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Centro de Costo.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Cuenta").Visible Then
            If e.Row("Cuenta") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Cuenta2").Visible Then
            If e.Row("Cuenta2") = 0 Then
                e.Row.RowError = "Error."
                MsgBox("Falta Cuenta.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

        If GGeneraAsiento And Grid.Columns("Cuenta").Visible And Grid.Columns("Cuenta2").Visible Then
            If e.Row("Cuenta") = e.Row("Cuenta2") Then
                e.Row.RowError = "Error."
                MsgBox("Cuentas NO Deben ser Iguales.", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3, "Error")
                Grid.Refresh()
                Exit Sub
            End If
        End If

    End Sub
    Private Sub Dt_RowChanged(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        Dim I As Integer
        e.Row.RowError = ""

        If Ptipo = 41 Then
            Grid.Select()

            I = 0
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("Nombre").Value = e.Row("Nombre") Then
                    I = I + 1
                End If
            Next
            If I > 1 Then e.Row.RowError = "Error." : MsgBox("Nombre ya Existe.")

            I = 0
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("CodigoAfipElectronico").Value = e.Row("CodigoAfipElectronico") Then
                    I = I + 1
                End If
            Next
            If I > 1 Then e.Row.RowError = "Error." : MsgBox("Código AFIP ya Existe.")
        End If

        If Ptipo = 31 Then
            Grid.Select()

            I = 0
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("OperadorInt").Value <> 0 Then
                    If Row.Cells("OperadorInt").Value = e.Row("Operador") Then
                        I = I + 1
                    End If
                End If
            Next
            If I > 1 Then e.Row.RowError = "Error." : MsgBox("Jurisdicción IIBB Conv. Multilateral ya Existe.")

            I = 0
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("CodProvinciaIIBB").Value <> "" Then
                    If Row.Cells("CodProvinciaIIBB").Value = e.Row("CodigoProvinciaIIBB") Then
                        I = I + 1
                    End If
                End If
            Next
            If I > 1 Then e.Row.RowError = "Error." : MsgBox("Codigo Provincia IIBB ya Existe.")
        End If

        If Ptipo = 34 Then
            Grid.Select()

            I = 0
            For Each Row As DataGridViewRow In Grid.Rows
                If Row.Cells("UltimoNumero").Value <> 0 Then
                    If Row.Cells("UltimoNumero").Value = e.Row("UltimoNumero") Then
                        I = I + 1
                    End If
                End If
            Next
            If I > 1 Then e.Row.RowError = "Error." : MsgBox("Codigo Concepto ya Existe.")

            Dim Contador As Integer = 0
            If e.Row("Activo") <> 0 Then contador = contador + 1
            If e.Row("Activo2") <> 0 Then contador = contador + 1
            If e.Row("Activo5") <> 0 Then Contador = Contador + 1
            Select Case Contador
                Case 0
                    e.Row.RowError = "Error." : MsgBox("Debe Informar: Remunerativo, No Remunerativo o Descuento.")
                Case 2, 3
                    e.Row.RowError = "Error." : MsgBox("Solo Debe Informar: Remunerativo , No Remunerativo o Descuento.")
            End Select
        End If

    End Sub
    Private Sub Dt_Deleting(ByVal sender As Object, ByVal e As System.Data.DataRowChangeEventArgs)

        e.Row.RowError = ""

    End Sub

End Class