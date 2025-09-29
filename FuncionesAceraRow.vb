Module FuncionesAceraRow
    Public Sub AceraRowTablas(ByRef Row As DataRow)

        'Operador se utiliza en tabla de tipo: 22,25,31,33,34,36,43,44

        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Tipo") = 0
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
        Row("CodigoProvinciaIIBB") = ""
        Row("OrigenPercepcion") = 0
        Row("Comentario") = ""
        Row("CodigoAfipElectronico") = 0

    End Sub
    Public Sub ArmaNuevoProveedor(ByRef Row As DataRow)

        Row("Clave") = 0
        Row("Nombre") = ""
        Row("Calle") = ""
        Row("Numero") = 0
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
End Module
