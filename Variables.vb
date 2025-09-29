Module Variables
    'Tener en cuenta para leer desde Bases de Datos distintas: http://msdn.microsoft.com/es-es/library/bh8kx08z(VS.80).aspx
    '' Public Const Conexion As String = "Provider=SQLOLEDB.1;" & _
    ''              "Integrated Security=SSPI; " & _
    ''             "Persist Security Info=False;" & _
    ''            "User ID=sa;Password=Patagonia1;" & _
    ''           "Initial Catalog=SComer;" & _
    ''          "Data Source=SERVIDOR\SQLEXPRESS"
    ''Public Const ConexionN As String = "Provider=SQLOLEDB.1;" & _
    ''             "Integrated Security=SSPI; " & _
    ''           "Persist Security Info=False;" & _
    ''             "User ID=sa;Password=Patagonia1;" & _
    ''             "Initial Catalog=SCOpr;" & _
    ''            "Data Source=SERVIDOR\SQLEXPRESS"
    ''    Public Const Conexion As String = "Provider=SQLOLEDB.1;" & _
    ''               "Integrated Security=SSPI; " & _
    ''               "Persist Security Info=False;" & _
    ''             "User ID=sa;" & _
    ''           "Initial Catalog=SComer;" & _
    ''             "Data Source=(local)\SQLEXPRESS"
    ''   Public Const ConexionN As String = "Provider=SQLOLEDB.1;" & _
    ''                  "Integrated Security=SSPI; " & _
    ''                "Persist Security Info=False;" & _
    ''              "User ID=sa;" & _
    ''            "Initial Catalog=SCOpr;" & _
    ''          "Data Source=(local)\SQLEXPRESS"
    '    Public Const ConexionW As String = "Provider=SQLOLEDB.1;" & _
    Public ConexionEmpresa As String = "Provider=SQLOLEDB.1;" & _
        "User ID=User;Password=Egle2000;" & _
        "Initial Catalog=SCEmpresas;" & _
        "Connect Timeout=120;" & _
        "Data Source=YYYY;"
    Public Const ConexionW As String = "Provider=SQLOLEDB.1;" & _
            "Integrated Security=SSPI; " & _
            "Persist Security Info=False;" & _
            "User ID=sa;Password=Egle2000;" & _
            "Initial Catalog=SComer;" & _
            "Connect Timeout=120;"
    Public Const ConexionNW As String = "Provider=SQLOLEDB.1;" & _
            "Integrated Security=SSPI; " & _
            "Persist Security Info=False;" & _
            "User ID=sa;Password=Egle2000;" & _
            "Initial Catalog=SCOpr;" & _
            "Connect Timeout=120;"
    Public Const ConexionW1 As String = "Provider=SQLOLEDB.1;" & _
            "User ID=User;Password=Egle2000;" & _
            "Initial Catalog=XXXX;" & _
            "Connect Timeout=120;" & _
            "Data Source=YYYY;"
    Public Const ConexionNW1 As String = "Provider=SQLOLEDB.1;" & _
            "User ID=User;Password=Egle2000;" & _
            "Initial Catalog=XXXX;" & _
            "Connect Timeout=120;" & _
            "Data Source=YYYY;"

    Public Conexion As String
    Public ConexionN As String
    Public GArticulo As Integer
    Public GEspecie As Integer
    Public GVariedad As Integer
    Public GMarca As Integer
    Public GCategoria As Integer
    Public GEnvase As Integer
    Public GModificacionOk As Boolean
    Public GDecimales As Integer = 2
    Public GRemito As Integer
    Public PermisoTotal As Boolean
    Public GCaja As Integer
    Public GTipoIva As Integer
    Public GCondicionIvaEmpresa As String
    Public GPuntoDeVenta As Integer
    Public GPuntoDeVentaResponsableInsc As Integer
    Public GPuntoDeVentaResponsableNoInsc As Integer
    Public GPuntoDeVentaConsumidorFinal As Integer
    Public GPuntoDeVentaExportacion As Integer
    Public GPuntoDeVentaRemitos As Integer
    Public GPuntoDeVentaRecibos As Integer
    Public GPuntoDeVentaDebResponsableInsc As Integer
    Public GPuntoDeVentaDebResponsableNoInsc As Integer
    Public GPuntoDeVentaDebConsumidorFinal As Integer
    Public GPuntoDeVentaDebExportacion As Integer
    Public GPuntoDeVentaCredResponsableInsc As Integer
    Public GPuntoDeVentaCredResponsableNoInsc As Integer
    Public GPuntoDeVentaCredConsumidorFinal As Integer
    Public GPuntoDeVentaCredExportacion As Integer
    Public GPuntoDeVentaLiqResponsableInsc As Integer
    Public GPuntoDeVentaLiqResponsableNoInsc As Integer
    Public GPuntoDeVentaLiqConsumidorFinal As Integer
    Public GFechaVencimiento As Date

    Public GForm As Form
    '--------------------------------------------------
    Public GDtPerfil As DataTable
    Public GLectura As Boolean
    Public GEscritura As Boolean
    Public GEspecial1 As Boolean
    Public GEspecial2 As Boolean
    Public GEspecial3 As Boolean
    '
    Public GServidor As String
    Public GFaltaDatosEmpresa As Boolean
    Public GlobalClave As Long = 0
    Public GNombreEmpresa As String = ""
    Public GClaveEmpresa As String = ""
    Public GCuitEmpresa As String
    Public GlobalFechaStr As String
    Public GlobalAccesoOk As List(Of Integer)
    Public GlobalAccesoDenegado As List(Of Integer)
    Public ListaDeErrores As New List(Of String)
    Public GlobalDt As DataTable
    Public GAltaExiste As String = -2147217873
    Public GCajaTotal As Boolean
    Public GAdministrador As Boolean
    Public GClaveUsuario As Integer
    Public GUsaNegra As Boolean = True
    Public GLineasFacturas As Integer = 28
    Public GLineasRemitos As Integer = 28
    Public GLineasPreLiquidacion As Integer = 28
    Public GLineasPagoRecibos As Integer = 20
    Public GLineasImutacionRecibos As Integer = 10
    Public GLineasConceptosPrestamos As Integer = 10
    Public GCancelaSinAsiento As Boolean = True
    Public GTolerancia As Decimal
    Public GBusqueda() As DataRow
    Public Const GGeneraAsiento As Boolean = True
    Public GConListaDeCostos As Boolean
    Public GConListaDeVentas As Boolean
    Public GTieneGestionExportacion As Boolean
    Public GListaCostosAsignados As List(Of ItemCostosAsignados)
    Public GListaCostosInsumos As List(Of ItemCostosAsignados)
    Public GListaLotesDeRecibos As List(Of ItemCostosAsignados)
    Public GListaLotesDeReintegros As List(Of ItemCostosAsignados)
    Public GListaLotesDeOtrasFacturas As List(Of ItemCostosAsignados)
    Public GListaLotesDeAsientosManuales As List(Of ItemCostosAsignados)
    Public GListaMermaPositivasNVLP As List(Of ItemCostosAsignados)
    Public GListaLotesDeConsumosTerminados As List(Of ItemCostosAsignados)
    Public GTipoIvaEmpresa As Integer
    Public GDireccion1 As String
    Public GDireccion2 As String
    Public GDireccion3 As String
    Public GIngBruto As String
    Public GFechaInicio As String
    Public GExcepcion As OleDb.OleDbException
    Public GTipoLicencia As String = ""
    Public GProveedorEgresoCaja As Integer = 0
    Public GLimiteParaConsumidorFinal As Decimal
    'Int32 = Integer ==> entre -2.147.483.648 a +2.147.483.647 
    Public Const Insumo As Integer = 7
    Public Const Fruta As Integer = 5
    Public Const ProveedorInsumos As Integer = 7
    Public Const ProveedorTransporte As Integer = 8
    'Tipo Tipo Iva.
    Public Const Exterior As Integer = 4
    'Codigo paises.
    Public Const Argentina As Integer = 1
    'Para Asientos factura venta.
    Public Const ClaveMontoNeto As Integer = 2
    Public Const ClaveIva As Integer = 3
    Public Const ClaveSenia As Integer = 4
    Public Const ClaveMontoFinal As Integer = 5
    Public Const Bulto As Integer = 4
    Public Const MedioBulto As Integer = 102
    Public Const Unidad As Integer = 103
    Public Const Kilo As Integer = 104
    Public Const MercadoCentral As String = "30-54665974-3"
    Public Const Fruticola As String = "30-71044543-1"
    Public Const MecaFront As String = "30-71087758-7"
    Public Const GPatagonia As String = "30-70908844-7"
    Public Const GPatagoniaFresh As String = "30-71115973-4"
    Public Const GCuadroNorte As String = "30-71499881-8"
    Public Const GPremiumFruit As String = "30-71010225-9"
    Public Const GPradan As String = "30-71151527-1"
    Public Const GTux As String = "30-71434715-9"
    Public Const GPruebaEnTux As String = "30-11111111-8"
    Public Const GARD As String = "30-71082112-3"
    Public Const GQUORE As String = "30-71768502-0"
    Public Const GEdeal As String = "30-71524555-4"
    Public Const GPedidosYaTrucho As String = "30-00000001-5"

    'Referencia para Asientos de Costos.
    Public Const Refe As Decimal = 1

End Module
