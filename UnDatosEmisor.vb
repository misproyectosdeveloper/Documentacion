Public Class UnDatosEmisor
    Public PEsCliente As Boolean
    Public PEsProveedor As Boolean
    Public PEsOtroProveedor As Boolean
    Public PEsAduana As Boolean
    Public PEmisor As Integer
    Public PConexion As String
    Private Sub UnDatosEmpresa_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If PConexion <> "" Then Conexion = PConexion 'Paso la conexion desde SExpotV01 que esta referenciado.

        ArmaTipoIva(ComboTipoIva)

        LlenaComboTablas(ComboPais, 28)

        ComboMoneda.DataSource = Nothing
        ComboMoneda.DataSource = Tablas.Leer("SELECT Clave,Nombre FROM Tablas WHERE Tipo = 27;")
        ComboMoneda.DisplayMember = "Nombre"
        ComboMoneda.ValueMember = "Clave"
        With ComboMoneda
            .AutoCompleteMode = AutoCompleteMode.SuggestAppend
            .AutoCompleteSource = AutoCompleteSource.ListItems
        End With

        Dim Dta As New DataTable

        If PEsCliente Then
            If Not Tablas.Read("SELECT * FROM Clientes WHERE Clave = " & PEmisor & ";", Conexion, Dta) Then Me.Close()
        End If

        If PEsAduana Then
            If Not Tablas.Read("SELECT * FROM ClientesAduana WHERE Clave = " & PEmisor & ";", Conexion, Dta) Then Me.Close()
        End If

        If PEsProveedor Then
            If Not Tablas.Read("SELECT * FROM Proveedores WHERE Clave = " & PEmisor & ";", Conexion, Dta) Then Me.Close()
        End If

        If PEsOtroProveedor Then
            If Not Tablas.Read("SELECT * FROM OtrosProveedores WHERE Clave = " & PEmisor & ";", Conexion, Dta) Then Me.Close()
        End If

        If PEsAduana Then
            LabelEmpresa.Text = Dta.Rows(0).Item("Nombre")
            ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
            TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
        Else
            LabelEmpresa.Text = Dta.Rows(0).Item("Nombre")
            TextCalle.Text = Dta.Rows(0).Item("Calle")
            TextLocalidad.Text = Dta.Rows(0).Item("Localidad")
            TextProvincia.Text = HallaNombreProvincia(Dta.Rows(0).Item("Provincia"))
            ComboPais.SelectedValue = Dta.Rows(0).Item("Pais")
            TextTelefonos.Text = Dta.Rows(0).Item("Telefonos")
            TextFaxes.Text = Dta.Rows(0).Item("Faxes")
            TextCuit.Text = Format(Dta.Rows(0).Item("Cuit"), "00-00000000-0")
            ComboTipoIva.SelectedValue = Dta.Rows(0).Item("TipoIva")
            If PEsCliente Then CheckCodigoCliente.Checked = Dta.Rows(0).Item("TieneCodigoCliente")
            ComboMoneda.SelectedValue = Dta.Rows(0).Item("Moneda")
        End If

    End Sub
    Private Sub UnDatosEmpresa_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Dispose()

    End Sub
End Class