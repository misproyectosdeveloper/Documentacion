Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Public Class ItemPaseDeProyectos
    Private StrServidor As String
    Private IntClaveEmpresa As Integer
    Private IntClaveUsuario As Integer
    Private BolPermisoTotal As Boolean
    Private StrConexion As String
    Private StrConexionN As String
    Public Sub New()
    End Sub
    Public Property Servidor() As String
        Get
            Return StrServidor
        End Get
        Set(ByVal value As String)
            StrServidor = value
        End Set
    End Property
    Public Property ClaveEmpresa() As Integer
        Get
            Return IntClaveEmpresa
        End Get
        Set(ByVal value As Integer)
            IntClaveEmpresa = value
        End Set
    End Property
    Public Property ClaveUsuario() As Integer
        Get
            Return IntClaveUsuario
        End Get
        Set(ByVal value As Integer)
            IntClaveUsuario = value
        End Set
    End Property
    Public Property PermisoTotal() As Boolean
        Get
            Return BolPermisoTotal
        End Get
        Set(ByVal value As Boolean)
            BolPermisoTotal = value
        End Set
    End Property
    Public Property Conexion() As String
        Get
            Return StrConexion
        End Get
        Set(ByVal value As String)
            StrConexion = value
        End Set
    End Property
    Public Property ConexionN() As String
        Get
            Return StrConexionN
        End Get
        Set(ByVal value As String)
            StrConexionN = value
        End Set
    End Property
End Class
Public Class ItemRetenciones
    Private IntClave As Integer
    Private StrNombre As String
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return StrNombre
        End Get
        Set(ByVal value As String)
            StrNombre = value
        End Set
    End Property
End Class
Public Class ItemIvaReten
    Private IntClave As Integer
    Private IntCodigoAfip As Integer
    Private IntCodigoAfipElectronico As Integer
    Private StrNombre As String
    Private DecAlicuota As Decimal
    Private DecImporte As Decimal
    Private DecBase As Decimal
    Private IntFormula As Integer
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property CodigoAfip() As Integer
        Get
            Return IntCodigoAfip
        End Get
        Set(ByVal value As Integer)
            IntCodigoAfip = value
        End Set
    End Property
    Public Property CodigoAfipElectronico() As Integer
        Get
            Return IntCodigoAfipElectronico
        End Get
        Set(ByVal value As Integer)
            IntCodigoAfipElectronico = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return StrNombre
        End Get
        Set(ByVal value As String)
            StrNombre = value
        End Set
    End Property
    Public Property Alicuota() As Decimal
        Get
            Return DecAlicuota
        End Get
        Set(ByVal value As Decimal)
            DecAlicuota = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property Base() As Decimal
        Get
            Return DecBase
        End Get
        Set(ByVal value As Decimal)
            DecBase = value
        End Set
    End Property
    Public Property Formula() As Integer
        Get
            Return IntFormula
        End Get
        Set(ByVal value As Integer)
            IntFormula = value
        End Set
    End Property

End Class
Public Class ItemListaConceptosAsientos
    Private IntClave As Integer
    Private DecImporte As Decimal
    Private IntTipoIva As Integer
    Private IntLegajo As Integer
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property TipoIva() As Integer
        Get
            Return IntTipoIva
        End Get
        Set(ByVal value As Integer)
            IntTipoIva = value
        End Set
    End Property
    Public Property Legajo() As Integer
        Get
            Return IntLegajo
        End Get
        Set(ByVal value As Integer)
            IntLegajo = value
        End Set
    End Property

End Class
Public Class ItemCuentasAsientos
    Private DblCuenta As Double
    Private IntClave As Integer
    Private DecImporte As Decimal
    Private DecImporteB As Decimal
    Private DecImporteN As Decimal
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property Cuenta() As Double
        Get
            Return DblCuenta
        End Get
        Set(ByVal value As Double)
            DblCuenta = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property ImporteB() As Decimal
        Get
            Return DecImporteB
        End Get
        Set(ByVal value As Decimal)
            DecImporteB = value
        End Set
    End Property
    Public Property ImporteN() As Decimal
        Get
            Return DecImporteN
        End Get
        Set(ByVal value As Decimal)
            DecImporteN = value
        End Set
    End Property
End Class
Public Class ItemLotesParaAsientos
    Private IntClave As Integer
    Private IntTipoOperacion As Integer
    Private IntCentro As Integer
    Private DecMontoNeto As Decimal
    Private DecMontoNetoReventa As Decimal
    Private DecMontoNetoConsignacion As Decimal
    Private DecMontoNetoCosteo As Decimal
    Private DecMontoNetoReventaMG As Decimal
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property TipoOperacion() As Integer
        Get
            Return IntTipoOperacion
        End Get
        Set(ByVal value As Integer)
            IntTipoOperacion = value
        End Set
    End Property
    Public Property Centro() As Integer
        Get
            Return IntCentro
        End Get
        Set(ByVal value As Integer)
            IntCentro = value
        End Set
    End Property
    Public Property MontoNeto() As Decimal
        Get
            Return DecMontoNeto
        End Get
        Set(ByVal value As Decimal)
            DecMontoNeto = value
        End Set
    End Property
    Public Property MontoNetoReventa() As Decimal
        Get
            Return DecMontoNetoReventa
        End Get
        Set(ByVal value As Decimal)
            DecMontoNetoReventa = value
        End Set
    End Property
    Public Property MontoNetoConsignacion() As Decimal
        Get
            Return DecMontoNetoConsignacion
        End Get
        Set(ByVal value As Decimal)
            DecMontoNetoConsignacion = value
        End Set
    End Property
    Public Property MontoNetoCosteo() As Decimal
        Get
            Return DecMontoNetoCosteo
        End Get
        Set(ByVal value As Decimal)
            DecMontoNetoCosteo = value
        End Set
    End Property
    Public Property MontoNetoReventaMG() As Decimal
        Get
            Return DecMontoNetoReventaMG
        End Get
        Set(ByVal value As Decimal)
            DecMontoNetoReventaMG = value
        End Set
    End Property

End Class
Public Class ItemListaDePrecios
    Private IntArticulo As Integer
    Private DblPrecio As Double
    Public Sub New()
    End Sub
    Public Property Articulo() As Integer
        Get
            Return IntArticulo
        End Get
        Set(ByVal value As Integer)
            IntArticulo = value
        End Set
    End Property
    Public Property Precio() As Double
        Get
            Return DblPrecio
        End Get
        Set(ByVal value As Double)
            DblPrecio = value
        End Set
    End Property
End Class
Public Class ItemIva
    Private DblIva As Double
    Private DecImporte As Decimal
    Public Sub New()
    End Sub
    Public Property Iva() As Double
        Get
            Return DblIva
        End Get
        Set(ByVal value As Double)
            DblIva = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
End Class
Public Class ItemCheque
    Private IntOperacion As Integer
    Private IntMedioPago As Integer
    Private IntBanco As Integer
    Private DblCuenta As Double
    Private StrSerie As String
    Private IntNumero As Integer
    Private DblImporte As Double
    Private DateFecha As Date
    Private DateFechaDeposito As Date
    Private StrEmisorCheque As String
    Private IntCaja As Integer
    Private IntTipoOrigen As Integer
    Private DblCompOrigen As Double
    Private IntTipoDestino As Integer
    Private DblCompDestino As Double
    Private DblAfectado As Double
    Private IntClaveCheque As Integer
    Private IntEstado As Integer
    Private BoleCheq As Boolean
    Public Sub New()
    End Sub
    Public Property MedioPago() As Integer
        Get
            Return IntMedioPago
        End Get
        Set(ByVal value As Integer)
            IntMedioPago = value
        End Set
    End Property
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
        End Set
    End Property
    Public Property Banco() As Integer
        Get
            Return IntBanco
        End Get
        Set(ByVal value As Integer)
            IntBanco = value
        End Set
    End Property
    Public Property Cuenta() As Double
        Get
            Return DblCuenta
        End Get
        Set(ByVal value As Double)
            DblCuenta = value
        End Set
    End Property
    Public Property Serie() As String
        Get
            Return StrSerie
        End Get
        Set(ByVal value As String)
            StrSerie = value
        End Set
    End Property
    Public Property Numero() As Integer
        Get
            Return IntNumero
        End Get
        Set(ByVal value As Integer)
            IntNumero = value
        End Set
    End Property
    Public Property Importe() As Double
        Get
            Return DblImporte
        End Get
        Set(ByVal value As Double)
            DblImporte = value
        End Set
    End Property
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
    Public Property FechaDeposito() As Date
        Get
            Return DateFechaDeposito
        End Get
        Set(ByVal value As Date)
            DateFechaDeposito = value
        End Set
    End Property
    Public Property EmisorCheque() As String
        Get
            Return StrEmisorCheque
        End Get
        Set(ByVal value As String)
            StrEmisorCheque = value
        End Set
    End Property
    Public Property Caja() As Integer
        Get
            Return IntCaja
        End Get
        Set(ByVal value As Integer)
            IntCaja = value
        End Set
    End Property
    Public Property TipoOrigen() As Integer
        Get
            Return IntTipoOrigen
        End Get
        Set(ByVal value As Integer)
            IntTipoOrigen = value
        End Set
    End Property
    Public Property CompOrigen() As Double
        Get
            Return DblCompOrigen
        End Get
        Set(ByVal value As Double)
            DblCompOrigen = value
        End Set
    End Property
    Public Property TipoDestino() As Integer
        Get
            Return IntTipoDestino
        End Get
        Set(ByVal value As Integer)
            IntTipoDestino = value
        End Set
    End Property
    Public Property CompDestino() As Double
        Get
            Return DblCompDestino
        End Get
        Set(ByVal value As Double)
            DblCompDestino = value
        End Set
    End Property
    Public Property Afectado() As Double
        Get
            Return DblAfectado
        End Get
        Set(ByVal value As Double)
            DblAfectado = value
        End Set
    End Property
    Public Property ClaveCheque() As Integer
        Get
            Return IntClaveCheque
        End Get
        Set(ByVal value As Integer)
            IntClaveCheque = value
        End Set
    End Property
    Public Property Estado() As Integer
        Get
            Return IntEstado
        End Get
        Set(ByVal value As Integer)
            IntEstado = value
        End Set
    End Property
    Public Property echeq() As Boolean
        Get
            Return BoleCheq
        End Get
        Set(ByVal value As Boolean)
            BoleCheq = value
        End Set
    End Property
End Class
Public Class ItemRecuperoSenia
    Private IntOperacion As Integer
    Private IntNota As Integer
    Private DblImporte As Double
    Private DblVale As Double
    Private DateFecha As Date
    Public Sub New()
    End Sub
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
        End Set
    End Property
    Public Property Nota() As Integer
        Get
            Return IntNota
        End Get
        Set(ByVal value As Integer)
            IntNota = value
        End Set
    End Property
    Public Property Vale() As Double
        Get
            Return DblVale
        End Get
        Set(ByVal value As Double)
            DblVale = value
        End Set
    End Property
    Public Property Importe() As Double
        Get
            Return DblImporte
        End Get
        Set(ByVal value As Double)
            DblImporte = value
        End Set
    End Property
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
End Class
Public Class Vigencia
    Private DateFecha As Date
    Private DblValor As Double
    Private DblAlicuota As Double
    Public Sub New()
    End Sub
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
    Public Property Valor() As Double
        Get
            Return DblValor
        End Get
        Set(ByVal value As Double)
            DblValor = value
        End Set
    End Property
    Public Property Alicuota() As Double
        Get
            Return DblAlicuota
        End Get
        Set(ByVal value As Double)
            DblAlicuota = value
        End Set
    End Property
End Class
Public Class FilaReciboDetalle

    Private DblRecibo As Double
    Private IntLote As Integer
    Private intSecuencia As Integer
    Private intDeposito As Integer
    Private DateFecha As DateTime
    Private DblCantidad As Double
    Public Sub New()
    End Sub
    Public Property Recibo() As Double
        Get
            Return DblRecibo
        End Get
        Set(ByVal value As Double)
            DblRecibo = value
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
            Return intSecuencia
        End Get
        Set(ByVal value As Integer)
            intSecuencia = value
        End Set
    End Property
    Public Property Deposito() As Integer
        Get
            Return intDeposito
        End Get
        Set(ByVal value As Integer)
            intDeposito = value
        End Set
    End Property
    Public Property Fecha() As DateTime
        Get
            Return DateFecha
        End Get
        Set(ByVal value As DateTime)
            DateFecha = value
        End Set
    End Property
    Public Property Cantidad() As Double
        Get
            Return DblCantidad
        End Get
        Set(ByVal value As Double)
            DblCantidad = value
        End Set
    End Property
End Class
Public Class FilaComprobanteFactura

    Private IntOperacion As Integer
    Private IntOperacionRemito As Integer
    Private IntIndice As Integer
    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private IntArticulo As Integer
    Private IntDeposito As Integer
    Private DecCantidad As Decimal
    Private DblOrdenCompra As Double
    Private DblRemito As Double
    Private DecIngreso As Decimal
    Private DecImporte As Decimal
    Private DecImporteB As Decimal
    Private DecImporteN As Decimal
    Private DecSenia As Decimal
    Private DateFecha As Date
    Public Sub New()
    End Sub
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
        End Set
    End Property
    Public Property OperacionRemito() As Integer
        Get
            Return IntOperacionRemito
        End Get
        Set(ByVal value As Integer)
            IntOperacionRemito = value
        End Set
    End Property
    Public Property Indice() As Integer
        Get
            Return IntIndice
        End Get
        Set(ByVal value As Integer)
            IntIndice = value
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
    Public Property Deposito() As Integer
        Get
            Return IntDeposito
        End Get
        Set(ByVal value As Integer)
            IntDeposito = value
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
    Public Property OrdenCompra() As Double
        Get
            Return DblOrdenCompra
        End Get
        Set(ByVal value As Double)
            DblOrdenCompra = value
        End Set
    End Property
    Public Property Ingreso() As Decimal
        Get
            Return DecIngreso
        End Get
        Set(ByVal value As Decimal)
            DecIngreso = value
        End Set
    End Property
    Public Property Remito() As Double
        Get
            Return DblRemito
        End Get
        Set(ByVal value As Double)
            DblRemito = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property ImporteB() As Decimal
        Get
            Return DecImporteB
        End Get
        Set(ByVal value As Decimal)
            DecImporteB = value
        End Set
    End Property
    Public Property ImporteN() As Decimal
        Get
            Return DecImporteN
        End Get
        Set(ByVal value As Decimal)
            DecImporteN = value
        End Set
    End Property
    Public Property Senia() As Decimal
        Get
            Return DecSenia
        End Get
        Set(ByVal value As Decimal)
            DecSenia = value
        End Set
    End Property
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
End Class
Public Class FilaAsignacion

    Private IntIndice As Integer
    Private IntOperacion As Integer
    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private intDeposito As Integer
    Private DecAsignado As Decimal
    Private DecDevolucion As Decimal
    Private DecImporte As Decimal
    Private DecImporteSinIva As Decimal
    Private DecImporte2 As Decimal
    Private IntLoteOrigen As Integer
    Private IntSecuenciaOrigen As Integer
    Private IntDepositoOrigen As Integer
    Private StrPermisoImp As String
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
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
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
    Public Property Deposito() As Integer
        Get
            Return intDeposito
        End Get
        Set(ByVal value As Integer)
            intDeposito = value
        End Set
    End Property
    Public Property Asignado() As Decimal
        Get
            Return DecAsignado
        End Get
        Set(ByVal value As Decimal)
            DecAsignado = value
        End Set
    End Property
    Public Property Devolucion() As Decimal
        Get
            Return DecDevolucion
        End Get
        Set(ByVal value As Decimal)
            DecDevolucion = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property ImporteSinIva() As Decimal
        Get
            Return DecImporteSinIva
        End Get
        Set(ByVal value As Decimal)
            DecImporteSinIva = value
        End Set
    End Property
    Public Property Importe2() As Decimal
        Get
            Return DecImporte2
        End Get
        Set(ByVal value As Decimal)
            DecImporte2 = value
        End Set
    End Property
    Public Property LoteOrigen() As Integer
        Get
            Return IntLoteOrigen
        End Get
        Set(ByVal value As Integer)
            IntLoteOrigen = value
        End Set
    End Property
    Public Property SecuenciaOrigen() As Integer
        Get
            Return IntSecuenciaOrigen
        End Get
        Set(ByVal value As Integer)
            IntSecuenciaOrigen = value
        End Set
    End Property
    Public Property DepositoOrigen() As Integer
        Get
            Return IntDepositoOrigen
        End Get
        Set(ByVal value As Integer)
            IntDepositoOrigen = value
        End Set
    End Property
    Public Property PermisoImp() As String
        Get
            Return StrPermisoImp
        End Get
        Set(ByVal value As String)
            StrPermisoImp = value
        End Set
    End Property

End Class
Public Class FilaLiquidacion

    Private IntOperacion As Integer
    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private IntDeposito As Integer
    Private IntProveedor As Integer
    Private IntArticulo As Integer
    Private decIniciales As Decimal
    Private DecAlicuota As Decimal
    Private DecBaja As Decimal
    Private DecMerma As Decimal
    Private decALiquidar As Decimal
    Private DecImporte As Decimal
    Private DecPrecioS As Decimal
    Private DecPrecioF As Decimal
    Private DblRemitoGuia As Double
    Private DateFecha As Date
    Private StrPermisoImp As String
    Private StrMedida As String
    Public Sub New()
    End Sub
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
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
    Public Property Deposito() As Integer
        Get
            Return IntDeposito
        End Get
        Set(ByVal value As Integer)
            IntDeposito = value
        End Set
    End Property
    Public Property Proveedor() As Integer
        Get
            Return IntProveedor
        End Get
        Set(ByVal value As Integer)
            IntProveedor = value
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
    Public Property Iniciales() As Decimal
        Get
            Return decIniciales
        End Get
        Set(ByVal value As Decimal)
            decIniciales = value
        End Set
    End Property
    Public Property Alicuota() As Decimal
        Get
            Return DecAlicuota
        End Get
        Set(ByVal value As Decimal)
            DecAlicuota = value
        End Set
    End Property
    Public Property Baja() As Decimal
        Get
            Return DecBaja
        End Get
        Set(ByVal value As Decimal)
            DecBaja = value
        End Set
    End Property
    Public Property Merma() As Decimal
        Get
            Return DecMerma
        End Get
        Set(ByVal value As Decimal)
            DecMerma = value
        End Set
    End Property
    Public Property Aliquidar() As Decimal
        Get
            Return decALiquidar
        End Get
        Set(ByVal value As Decimal)
            decALiquidar = value
        End Set
    End Property
    Public Property Importe() As Decimal
        Get
            Return DecImporte
        End Get
        Set(ByVal value As Decimal)
            DecImporte = value
        End Set
    End Property
    Public Property PrecioS() As Decimal
        Get
            Return DecPrecioS
        End Get
        Set(ByVal value As Decimal)
            DecPrecioS = value
        End Set
    End Property
    Public Property PrecioF() As Decimal
        Get
            Return DecPrecioF
        End Get
        Set(ByVal value As Decimal)
            DecPrecioF = value
        End Set
    End Property
    Public Property RemitoGuia() As Double
        Get
            Return DblRemitoGuia
        End Get
        Set(ByVal value As Double)
            DblRemitoGuia = value
        End Set
    End Property
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
    Public Property PermisoImp() As String
        Get
            Return StrPermisoImp
        End Get
        Set(ByVal value As String)
            StrPermisoImp = value
        End Set
    End Property
    Public Property Medida() As String
        Get
            Return StrMedida
        End Get
        Set(ByVal value As String)
            StrMedida = value
        End Set
    End Property

End Class
Public Class FilaFactura

    Private IntIndice As Integer
    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private intDeposito As Integer
    Private intArticulo As Integer
    Private intkilosXUnidad As Integer
    Private DblIva As Double
    Private DblUnitario As Double
    Private intCantidad As Integer
    Private DblTotalArticulo As Double
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
    Public Property Articulo() As Integer
        Get
            Return intArticulo
        End Get
        Set(ByVal value As Integer)
            intArticulo = value
        End Set
    End Property
    Public Property KilosXUnidad() As Integer
        Get
            Return intkilosXUnidad
        End Get
        Set(ByVal value As Integer)
            intkilosXUnidad = value
        End Set
    End Property
    Public Property Iva() As Double
        Get
            Return DblIva
        End Get
        Set(ByVal value As Double)
            DblIva = value
        End Set
    End Property
    Public Property Unitario() As Double
        Get
            Return DblUnitario
        End Get
        Set(ByVal value As Double)
            DblUnitario = value
        End Set
    End Property
    Public Property Cantidad() As Integer
        Get
            Return intCantidad
        End Get
        Set(ByVal value As Integer)
            intCantidad = value
        End Set
    End Property
    Public Property TotalArticulo() As Double
        Get
            Return DblTotalArticulo
        End Get
        Set(ByVal value As Double)
            DblTotalArticulo = value
        End Set
    End Property
    
End Class
Public Class FilaItemsRecibo

    Private IntItem As Integer
    Private IntTipo As Integer
    Private IntMonedaBanco As Integer
    Private DblCambioCheque As Double
    Private StrEmisor As String
    Private DblImporte As Double
    Private DateFechaCheque As DateTime
    Private DateFecha As DateTime
    Public Sub New()
    End Sub
    Public Property Item() As Integer
        Get
            Return IntItem
        End Get
        Set(ByVal value As Integer)
            IntItem = value
        End Set
    End Property
    Public Property Tipo() As Integer
        Get
            Return IntTipo
        End Get
        Set(ByVal value As Integer)
            IntTipo = value
        End Set
    End Property
    Public Property MonedaBanco() As Integer
        Get
            Return IntMonedaBanco
        End Get
        Set(ByVal value As Integer)
            IntMonedaBanco = value
        End Set
    End Property
    Public Property CambioCheque() As Double
        Get
            Return DblCambioCheque
        End Get
        Set(ByVal value As Double)
            DblCambioCheque = value
        End Set
    End Property
    Public Property Importe() As Double
        Get
            Return DblImporte
        End Get
        Set(ByVal value As Double)
            DblImporte = value
        End Set
    End Property
    Public Property Emisor() As String
        Get
            Return StrEmisor
        End Get
        Set(ByVal value As String)
            StrEmisor = value
        End Set
    End Property
    Public Property FechaCheque() As DateTime
        Get
            Return DateFechaCheque
        End Get
        Set(ByVal value As DateTime)
            DateFechaCheque = value
        End Set
    End Property
    Public Property Fecha() As DateTime
        Get
            Return DateFecha
        End Get
        Set(ByVal value As DateTime)
            DateFecha = value
        End Set
    End Property

End Class
Public Class FilaItemsRetencion

    Private IntClave As Integer
    Private IntNumero As Integer
    Private IntTipoIva As Integer
    Private StrNombre As String
    Private DblTopeMes As Double
    Private DblAlicuotaRetencion As Double
    Private DblImporte As Double
    Private DblRetencionCobrada As Double
    Private BolActivo As Boolean
    Public Sub New()
    End Sub
    Public Property Clave() As Integer
        Get
            Return IntClave
        End Get
        Set(ByVal value As Integer)
            IntClave = value
        End Set
    End Property
    Public Property Numero() As Integer
        Get
            Return IntNumero
        End Get
        Set(ByVal value As Integer)
            IntNumero = value
        End Set
    End Property
    Public Property TipoIva() As Integer
        Get
            Return IntTipoIva
        End Get
        Set(ByVal value As Integer)
            IntTipoIva = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return StrNombre
        End Get
        Set(ByVal value As String)
            StrNombre = value
        End Set
    End Property
    Public Property TopeMes() As Double
        Get
            Return DblTopeMes
        End Get
        Set(ByVal value As Double)
            DblTopeMes = value
        End Set
    End Property
    Public Property AlicuotaRetencion() As Double
        Get
            Return DblAlicuotaRetencion
        End Get
        Set(ByVal value As Double)
            DblAlicuotaRetencion = value
        End Set
    End Property
    Public Property Importe() As Double
        Get
            Return DblImporte
        End Get
        Set(ByVal value As Double)
            DblImporte = value
        End Set
    End Property
    Public Property RetencionCobrada() As Double
        Get
            Return DblRetencionCobrada
        End Get
        Set(ByVal value As Double)
            DblRetencionCobrada = value
        End Set
    End Property
    Public Property Activo() As Boolean
        Get
            Return BolActivo
        End Get
        Set(ByVal value As Boolean)
            BolActivo = value
        End Set
    End Property

End Class
Public Class FilaMedioPago

    Private IntMedioPago As Integer
    Private IntBanco As Integer
    Private DblCuenta As Double
    Private StrSerie As String
    Private DblNumero As Double
    Private StrEmisor As String
    Private StrFecha As String
    Private DblCambio As Double
    Private DblImporte As Double
    Private IntClaveCheque As Integer
    Public Sub New()
    End Sub
    Public Property MedioPago() As Integer
        Get
            Return IntMedioPago
        End Get
        Set(ByVal value As Integer)
            IntMedioPago = value
        End Set
    End Property
    Public Property Banco() As Integer
        Get
            Return IntBanco
        End Get
        Set(ByVal value As Integer)
            IntBanco = value
        End Set
    End Property
    Public Property Cuenta() As Double
        Get
            Return DblCuenta
        End Get
        Set(ByVal value As Double)
            DblCuenta = value
        End Set
    End Property
    Public Property Serie() As String
        Get
            Return StrSerie
        End Get
        Set(ByVal value As String)
            StrSerie = value
        End Set
    End Property
    Public Property Numero() As Double
        Get
            Return DblNumero
        End Get
        Set(ByVal value As Double)
            DblNumero = value
        End Set
    End Property
    Public Property Emisor() As String
        Get
            Return StrEmisor
        End Get
        Set(ByVal value As String)
            StrEmisor = value
        End Set
    End Property
    Public Property Fecha() As String
        Get
            Return StrFecha
        End Get
        Set(ByVal value As String)
            StrFecha = value
        End Set
    End Property
    Public Property Cambio() As Double
        Get
            Return DblCambio
        End Get
        Set(ByVal value As Double)
            DblCambio = value
        End Set
    End Property
    Public Property Importe() As Double
        Get
            Return DblImporte
        End Get
        Set(ByVal value As Double)
            DblImporte = value
        End Set
    End Property
    Public Property ClaveCheque() As Integer
        Get
            Return IntClaveCheque
        End Get
        Set(ByVal value As Integer)
            IntClaveCheque = value
        End Set
    End Property

End Class
Public Class ItemCostosAsignados
    Private IntOperacion As Integer
    Private StrNombre As String
    Private StrComprobante As String
    Private IntTipoComprobante As Integer
    Private DecNroComprobante As Decimal
    Private DecImporteConIva As Decimal
    Private DecImporteSinIva As Decimal
    Private DecCantidad As Decimal
    Public Sub New()
    End Sub
    Public Property Operacion() As Integer
        Get
            Return IntOperacion
        End Get
        Set(ByVal value As Integer)
            IntOperacion = value
        End Set
    End Property
    Public Property Nombre() As String
        Get
            Return StrNombre
        End Get
        Set(ByVal value As String)
            StrNombre = value
        End Set
    End Property
    Public Property Comprobante() As String
        Get
            Return StrComprobante
        End Get
        Set(ByVal value As String)
            StrComprobante = value
        End Set
    End Property
    Public Property TipoComprobante() As Integer
        Get
            Return IntTipoComprobante
        End Get
        Set(ByVal value As Integer)
            IntTipoComprobante = value
        End Set
    End Property
    Public Property NroComprobante() As Decimal
        Get
            Return DecNroComprobante
        End Get
        Set(ByVal value As Decimal)
            DecNroComprobante = value
        End Set
    End Property
    Public Property ImporteConIva() As Decimal
        Get
            Return DecImporteConIva
        End Get
        Set(ByVal value As Decimal)
            DecImporteConIva = value
        End Set
    End Property
    Public Property ImporteSinIva() As Decimal
        Get
            Return DecImporteSinIva
        End Get
        Set(ByVal value As Decimal)
            DecImporteSinIva = value
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
End Class
Public Class ItemResumenCheques
    Private IntSemana As Integer
    Private DblEmitido As Double
    Private DblDepositado As Double
    Private DblAcumuladoSinDepositar As Double
    Private DateDesde As Date
    Private DateHasta As Date
    Public Sub New()
    End Sub
    Public Property Semana() As Integer
        Get
            Return IntSemana
        End Get
        Set(ByVal value As Integer)
            IntSemana = value
        End Set
    End Property
    Public Property Emitido() As Double
        Get
            Return DblEmitido
        End Get
        Set(ByVal value As Double)
            DblEmitido = value
        End Set
    End Property
    Public Property Deposito() As Double
        Get
            Return DblDepositado
        End Get
        Set(ByVal value As Double)
            DblDepositado = value
        End Set
    End Property
    Public Property AmunuladoSinDepositar() As Double
        Get
            Return DblAcumuladoSinDepositar
        End Get
        Set(ByVal value As Double)
            DblAcumuladoSinDepositar = value
        End Set
    End Property
    Public Property Desde() As Date
        Get
            Return DateDesde
        End Get
        Set(ByVal value As Date)
            DateDesde = value
        End Set
    End Property
    Public Property Hasta() As Date
        Get
            Return DateHasta
        End Get
        Set(ByVal value As Date)
            DateHasta = value
        End Set
    End Property

End Class
Public Class ItemRemito
    Private DblRemito As Double
    Private BolAbierto As Boolean
    Private IntEstado As Integer
    Private DateFecha As Date
    Public Sub New()
    End Sub
    Public Property Remito() As Double
        Get
            Return DblRemito
        End Get
        Set(ByVal value As Double)
            DblRemito = value
        End Set
    End Property
    Public Property Abierto() As Boolean
        Get
            Return BolAbierto
        End Get
        Set(ByVal value As Boolean)
            BolAbierto = value
        End Set
    End Property
    Public Property Fecha() As Date
        Get
            Return DateFecha
        End Get
        Set(ByVal value As Date)
            DateFecha = value
        End Set
    End Property
    Public Property Estado() As Integer
        Get
            Return IntEstado
        End Get
        Set(ByVal value As Integer)
            IntEstado = value
        End Set
    End Property
End Class
Public Class ItemPedido
    Private IntArticulo As Integer
    Private DecCantidad As Decimal
    Private DecPrecio As Decimal
    Public Sub New()
    End Sub
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
    Public Property Precio() As Decimal
        Get
            Return DecPrecio
        End Get
        Set(ByVal value As Decimal)
            DecPrecio = value
        End Set
    End Property
End Class
Public Class ItemPorOrdenCompra
    Private DecOrdenCompra As Decimal
    Private DecCantidad As Decimal
    Private DecImporteConIva As Decimal
    Private DecImporteSinIva As Decimal
    Public Sub New()
    End Sub
    Public Property OrdenCompra() As Decimal
        Get
            Return DecOrdenCompra
        End Get
        Set(ByVal value As Decimal)
            DecOrdenCompra = value
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
    Public Property ImporteConIva() As Decimal
        Get
            Return DecImporteConIva
        End Get
        Set(ByVal value As Decimal)
            DecImporteConIva = value
        End Set
    End Property
    Public Property ImporteSinIva() As Decimal
        Get
            Return DecImporteSinIva
        End Get
        Set(ByVal value As Decimal)
            DecImporteSinIva = value
        End Set
    End Property

End Class
Public Class ItemListado
    Private IntArticulo As Integer
    Private DecIva As Decimal
    Private DecPrecio As Decimal
    Private DecCantidad As Decimal
    Private DecKilosXUnidad As Decimal
    Private StrCodigoCliente As String
    Private DecNeto As Decimal
    Private DecTotalItem As Decimal
    Private StrMedida As String
    Private StrUMedida As String
    Private DecPrecioLista As Decimal
    Private IntTipoPrecio As Integer
    Public Sub New()
    End Sub
    Public Property Articulo() As Integer
        Get
            Return IntArticulo
        End Get
        Set(ByVal value As Integer)
            IntArticulo = value
        End Set
    End Property
    Public Property Iva() As Decimal
        Get
            Return DecIva
        End Get
        Set(ByVal value As Decimal)
            DecIva = value
        End Set
    End Property
    Public Property Precio() As Decimal
        Get
            Return DecPrecio
        End Get
        Set(ByVal value As Decimal)
            DecPrecio = value
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
    Public Property KilosXUnidad() As Decimal
        Get
            Return DecKilosXUnidad
        End Get
        Set(ByVal value As Decimal)
            DecKilosXUnidad = value
        End Set
    End Property
    Public Property CodigoCliente() As String
        Get
            Return StrCodigoCliente
        End Get
        Set(ByVal value As String)
            StrCodigoCliente = value
        End Set
    End Property
    Public Property Neto() As Decimal
        Get
            Return DecNeto
        End Get
        Set(ByVal value As Decimal)
            DecNeto = value
        End Set
    End Property
    Public Property TotalItem() As Decimal
        Get
            Return DecTotalItem
        End Get
        Set(ByVal value As Decimal)
            DecTotalItem = value
        End Set
    End Property
    Public Property Medida() As String
        Get
            Return StrMedida
        End Get
        Set(ByVal value As String)
            StrMedida = value
        End Set
    End Property
    Public Property UMedida() As String
        Get
            Return StrUMedida
        End Get
        Set(ByVal value As String)
            StrUMedida = value
        End Set
    End Property
    Public Property PrecioLista() As Decimal
        Get
            Return DecPrecioLista
        End Get
        Set(ByVal value As Decimal)
            DecPrecioLista = value
        End Set
    End Property
    Public Property TipoPrecio() As Integer
        Get
            Return IntTipoPrecio
        End Get
        Set(ByVal value As Integer)
            IntTipoPrecio = value
        End Set
    End Property

End Class
Public Class ItemRemitosCantidad
    Private DecRemito As Decimal
    Private DecCantidad As Decimal
    Public Sub New()
    End Sub
    Public Property Remito() As Decimal
        Get
            Return DecRemito
        End Get
        Set(ByVal value As Decimal)
            DecRemito = value
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
 
End Class
Public Class ItemReproceso
    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private IntArticulo As Integer
    Private DecBaja As Decimal
    Private DecAlta As Decimal
    Private DecKilosXUnidad As Decimal
    Private DecTotalKilos As Decimal
    Private DecPorcentaje As Decimal
    Private DecMerma As Decimal
    Private IntCentro As Integer
    Private IntTipoOperacion As Integer
    Private DecPrecio As Decimal
    Private IntSecuenciaReproceso As Integer
    Public Sub New()
    End Sub
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
    Public Property Baja() As Decimal
        Get
            Return DecBaja
        End Get
        Set(ByVal value As Decimal)
            DecBaja = value
        End Set
    End Property
    Public Property Alta() As Decimal
        Get
            Return DecAlta
        End Get
        Set(ByVal value As Decimal)
            DecAlta = value
        End Set
    End Property
    Public Property KilosXUnidad() As Decimal
        Get
            Return DecKilosXUnidad
        End Get
        Set(ByVal value As Decimal)
            DecKilosXUnidad = value
        End Set
    End Property
    Public Property TotalKilos() As Decimal
        Get
            Return DecTotalKilos
        End Get
        Set(ByVal value As Decimal)
            DecTotalKilos = value
        End Set
    End Property
    Public Property Porcentaje() As Decimal
        Get
            Return DecPorcentaje
        End Get
        Set(ByVal value As Decimal)
            DecPorcentaje = value
        End Set
    End Property
    Public Property Merma() As Decimal
        Get
            Return DecMerma
        End Get
        Set(ByVal value As Decimal)
            DecMerma = value
        End Set
    End Property
    Public Property SecuenciaReproceso() As Integer
        Get
            Return IntSecuenciaReproceso
        End Get
        Set(ByVal value As Integer)
            IntSecuenciaReproceso = value
        End Set
    End Property
    Public Property Centro() As Integer
        Get
            Return IntCentro
        End Get
        Set(ByVal value As Integer)
            IntCentro = value
        End Set
    End Property
    Public Property TipoOperacion() As Integer
        Get
            Return IntTipoOperacion
        End Get
        Set(ByVal value As Integer)
            IntTipoOperacion = value
        End Set
    End Property
    Public Property Precio() As Decimal
        Get
            Return DecPrecio
        End Get
        Set(ByVal value As Decimal)
            DecPrecio = value
        End Set
    End Property
End Class
Public Class FilaGenerica

    Private IntLote As Integer
    Private IntSecuencia As Integer
    Private DecImporte1 As Decimal
    Private DecImporte2 As Decimal
    Private DecImporte3 As Decimal
    Private DecImporte4 As Decimal
    Private DecImporte5 As Decimal
    Private DecImporte6 As Decimal
    Private StrString1 As String
    Public Sub New()
    End Sub
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
    Public Property Importe1() As Decimal
        Get
            Return DecImporte1
        End Get
        Set(ByVal value As Decimal)
            DecImporte1 = value
        End Set
    End Property
    Public Property Importe2() As Decimal
        Get
            Return DecImporte2
        End Get
        Set(ByVal value As Decimal)
            DecImporte2 = value
        End Set
    End Property
    Public Property Importe3() As Decimal
        Get
            Return DecImporte3
        End Get
        Set(ByVal value As Decimal)
            DecImporte3 = value
        End Set
    End Property
    Public Property Importe4() As Decimal
        Get
            Return DecImporte4
        End Get
        Set(ByVal value As Decimal)
            DecImporte4 = value
        End Set
    End Property
    Public Property Importe5() As Decimal
        Get
            Return DecImporte5
        End Get
        Set(ByVal value As Decimal)
            DecImporte5 = value
        End Set
    End Property
    Public Property Importe6() As Decimal
        Get
            Return DecImporte6
        End Get
        Set(ByVal value As Decimal)
            DecImporte6 = value
        End Set
    End Property
    Public Property String1() As String
        Get
            Return StrString1
        End Get
        Set(ByVal value As String)
            StrString1 = value
        End Set
    End Property
End Class
Public Class ItemCtaCte
    Private StrFecha As String
    Private StrTipo As String
    Private IntTipoComprobante As Integer
    Private StrComprobante As String
    Private StrLote As String
    Private StrArticulo As String
    Private DblCantidad As Decimal
    Private DblPrecio As Decimal
    Private DblTotal As Decimal
    Private DblImpuesto As Decimal
    Private DblSenia As Decimal
    Private DblDebe As Decimal
    Private DblHaber As Decimal
    Private DblSaldoPeriodo As Decimal
    Private DblSaldoGeneral As Decimal
    Private StrConexion As String
    Private StrEstado As String
    Public Sub New()
    End Sub
    Public Property Fecha() As String
        Get
            Return StrFecha
        End Get
        Set(ByVal value As String)
            StrFecha = value
        End Set
    End Property
    Public Property Tipo() As String
        Get
            Return StrTipo
        End Get
        Set(ByVal value As String)
            StrTipo = value
        End Set
    End Property
    Public Property TipoComprobante() As Integer
        Get
            Return IntTipoComprobante
        End Get
        Set(ByVal value As Integer)
            IntTipoComprobante = value
        End Set
    End Property
    Public Property Comprobante() As String
        Get
            Return StrComprobante
        End Get
        Set(ByVal value As String)
            StrComprobante = value
        End Set
    End Property
    Public Property Lote() As String
        Get
            Return StrLote
        End Get
        Set(ByVal value As String)
            StrLote = value
        End Set
    End Property
    Public Property Articulo() As String
        Get
            Return StrArticulo
        End Get
        Set(ByVal value As String)
            StrArticulo = value
        End Set
    End Property
    Public Property Cantidad() As Decimal
        Get
            Return DblCantidad
        End Get
        Set(ByVal value As Decimal)
            DblCantidad = value
        End Set
    End Property
    Public Property Precio() As Decimal
        Get
            Return DblPrecio
        End Get
        Set(ByVal value As Decimal)
            DblPrecio = value
        End Set
    End Property
    Public Property Total() As Decimal
        Get
            Return DblTotal
        End Get
        Set(ByVal value As Decimal)
            DblTotal = value
        End Set
    End Property
    Public Property Impuesto() As Decimal
        Get
            Return DblImpuesto
        End Get
        Set(ByVal value As Decimal)
            DblImpuesto = value
        End Set
    End Property
    Public Property Senia() As Decimal
        Get
            Return DblSenia
        End Get
        Set(ByVal value As Decimal)
            DblSenia = value
        End Set
    End Property
    Public Property Debe() As Decimal
        Get
            Return DblDebe
        End Get
        Set(ByVal value As Decimal)
            DblDebe = value
        End Set
    End Property
    Public Property Haber() As Decimal
        Get
            Return DblHaber
        End Get
        Set(ByVal value As Decimal)
            DblHaber = value
        End Set
    End Property
    Public Property SaldoPeriodo() As Decimal
        Get
            Return DblSaldoPeriodo
        End Get
        Set(ByVal value As Decimal)
            DblSaldoPeriodo = value
        End Set
    End Property
    Public Property SaldoGeneral() As Decimal
        Get
            Return DblSaldoGeneral
        End Get
        Set(ByVal value As Decimal)
            DblSaldoGeneral = value
        End Set
    End Property
    Public Property Estado() As String
        Get
            Return StrEstado
        End Get
        Set(ByVal value As String)
            StrEstado = value
        End Set
    End Property
End Class
Public Class Tablas
    Public Shared Function Leer(ByVal SqlStr As String) As DataTable

        Dim cnn As OleDbConnection = New OleDbConnection(Conexion)

        cnn.Open()
        Dim cmd As OleDbDataAdapter = New OleDbDataAdapter(SqlStr, cnn)
        Dim dt As DataTable = New DataTable

        Try
            cmd.Fill(dt)
            cmd = Nothing
            Return dt
        Catch exSql As OleDb.OleDbException
            MsgBox(exSql.Message.ToString)
        Catch ex As Exception
        Finally
            If cnn.State = ConnectionState.Open Then cnn.Close()
        End Try

    End Function
    Public Shared Function Read(ByVal SqlStr As String, ByVal Conexion As String, ByRef Dt As DataTable) As Boolean

        Dim cnn As OleDbConnection = New OleDbConnection(Conexion)

        Try
            cnn.Open()
        Catch ex As Exception
            MsgBox("Error al Abrir Base de Datos. " & ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

        Dim cmd As OleDbDataAdapter = New OleDbDataAdapter(SqlStr, cnn)

        Try
            cmd.Fill(Dt)
            cmd.Dispose()
            Return True
        Catch ex As Exception
            MsgBox("ERROR De lectura Base de Datos." & ex.Message, MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        Finally
            If cnn.State = ConnectionState.Open Then cnn.Close()
        End Try

    End Function
    Public Shared Function Grabar(ByVal Dt As DataTable, ByVal Archivo As String, ByVal ConexionStr As String) As String

        If IsNothing(Dt.GetChanges) Then Return ""

        Dim Sql As String = "SELECT * FROM " & Archivo & ";"

        Using MiConexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Try
                    Using DaP As New OleDb.OleDbDataAdapter(Sql, MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.Update(Dt.GetChanges)
                    End Using
                    Return ""
                Catch ex As OleDb.OleDbException
                    If ex.ErrorCode = GAltaExiste Then
                        Return "Alta Ya Existe."
                    Else
                        Return "Error Base de Datos en " & Archivo + vbCrLf + ex.Message
                    End If
                Catch ex As DBConcurrencyException
                    Return "Error Otro usuario modifico datos en " & Archivo + vbCrLf + ex.Message
                Finally
                End Try
            Catch ex As Exception
                Return "Error de Base de datos en " & Archivo + vbCrLf + ex.Message
            End Try
        End Using

    End Function
    Public Shared Function GrabarOleDb(ByVal Dt As DataTable, ByVal Sql As String, ByVal ConexionStr As String) As String

        Dim Mensaje As String
        GModificacionOk = False

        Dim Trans As OleDb.OleDbTransaction

        Using MiConexion As New OleDb.OleDbConnection(ConexionStr)
            Try
                MiConexion.Open()
                'con la opcion en Sql:  Server ALTER DATABASE Scomer SET READ_COMMITTED_SNAPSHOT ON para bloqueo de fila.
                Trans = MiConexion.BeginTransaction(IsolationLevel.ReadCommitted)
                '
                Try
                    Using DaP As New OleDb.OleDbDataAdapter(Sql, MiConexion)
                        Dim SqlCommandBuilder As New OleDb.OleDbCommandBuilder(DaP)
                        DaP.SelectCommand.Transaction = Trans
                        DaP.Update(Dt.GetChanges)
                    End Using
                    Trans.Commit()
                    Mensaje = "Cambios Realizados Exitosamente."
                    GModificacionOk = True
                Catch ex As OleDb.OleDbException
                    Trans.Rollback()
                    Mensaje = "Error de Base de datos. Operación se CANCELA." + vbCrLf + ex.Message
                Catch ex As DBConcurrencyException
                    Trans.Rollback()
                    Mensaje = "Error,Otro Usuario modifico Datos. Operación se CANCELA." + vbCrLf + ex.Message
                Finally
                    Trans = Nothing
                End Try
            Catch ex As Exception
                Mensaje = "Error de Base de datos. Operación se CANCELA." + vbCrLf + ex.Message
            End Try
        End Using

        Return Mensaje

    End Function
    Public Shared Function Insertar(ByVal Sql As String) As Integer

        Try
            Dim cmd As OleDbCommand = _
            New OleDbCommand(Sql, New OleDbConnection(Conexion))
            cmd.Connection.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Return i
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
    Public Shared Function Funcion(ByVal Sql As String) As Integer

        Try
            Dim cmd As OleDbCommand = _
            New OleDbCommand(Sql, New OleDbConnection(Conexion))
            cmd.Connection.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Return i
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
    Public Shared Function Borrar(ByVal Sql As String) As Integer

        Try
            Dim cmd As OleDbCommand = _
            New OleDbCommand(Sql, New OleDbConnection(Conexion))
            cmd.Connection.Open()
            Dim i As Integer = cmd.ExecuteNonQuery()
            cmd.Connection.Close()
            Return i
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.Critical)
        End Try

    End Function
End Class
Public Class PermisoImportacion
    Public Shared Function Valida(ByVal Permiso As String) As Boolean

        If Permiso.Length <> 16 Then
            MsgBox("Longitud Permiso Importación debe ser de 16 posiciones. (Ej: 12345CV12345678F).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If Not IsNumeric(Mid(Permiso, 1, 5)) Or Not IsNumeric(Mid(Permiso, 8, 8)) Then
            MsgBox("Permiso Importación Erroneo. (Ej: 12345CV12345678F).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        If IsNumeric(Mid(Permiso, 6, 1)) Or IsNumeric(Mid(Permiso, 7, 1)) Or IsNumeric(Mid(Permiso, 16, 1)) Then
            MsgBox("Permiso Importación Erroneo. (Ej: 12345CV12345678F).", MsgBoxStyle.Critical + MsgBoxStyle.DefaultButton3)
            Return False
        End If

        Return True

    End Function
End Class
Public Class EsNumericoGridBox

    Public Shared Sub Valida(ByRef Box As TextBox, ByVal Decimales As Integer)

        Dim CuentaComas As Integer = 0
        For Each C As Char In Box.Text
            If C = "," Then CuentaComas = CuentaComas + 1
        Next
        If CuentaComas = 2 And Strings.Right(Box.Text, 1) = "," Then
            Box.Text = Strings.Left(Box.Text, Box.Text.Length - 1)
            Box.SelectionStart = Box.Text.Length
            Exit Sub
        End If

        If Not IsNumeric(Box.Text) Then
            MsgBox("Dato No Numérico.")
            Box.Text = ""
            Exit Sub
        End If

        Dim MiArray() As String

        MiArray = Split(Box.Text, ",")
        If UBound(MiArray) > 1 Then
            MsgBox("Dato No Numérico.")
            Box.Text = ""
            Exit Sub
        End If

        If UBound(MiArray) = 1 Then
            If MiArray(1).ToString.Length > Decimales Then
                Box.Text = Strings.Left(Box.Text, Box.Text.Length - 1)
                Box.SelectionStart = Box.Text.Length
            End If
        End If

    End Sub
    Public Shared Sub ValidaConSigno(ByRef Box As TextBox, ByVal Decimales As Integer)

        Dim BoxAux As New TextBox
        BoxAux.Text = Strings.Right(Box.Text, Box.Text.Length - 1)
        If BoxAux.Text = "" Then Exit Sub

        Dim Signo As String
        Signo = Strings.Left(Box.Text, 1)

        Dim CuentaSigno As Integer = 0
        For Each C As Char In Box.Text
            If C = "-" Then CuentaSigno = CuentaSigno + 1
        Next
        If Signo <> "-" And CuentaSigno <> 0 Then
            Box.Text = Strings.Left(Box.Text, Box.Text.Length - 1)
            Box.SelectionStart = Box.Text.Length
            Exit Sub
        End If
        If Signo = "-" And CuentaSigno > 1 Then
            Box.Text = Strings.Left(Box.Text, Box.Text.Length - 1)
            Box.SelectionStart = Box.Text.Length
            Exit Sub
        End If

        EsNumericoGridBox.Valida(BoxAux, Decimales)
        Box.Text = Signo & BoxAux.Text

    End Sub

End Class
Public Class EsPorcentajeGridBox

    Public Shared Sub Valida(ByRef Box As TextBox)

        EsNumericoGridBox.Valida(Box, 2)
        If Box.Text = "" Then Exit Sub
        If CDec(Box.Text) > 100 Then
            MsgBox("Dato Supera 100%.")
            Box.Text = ""
        End If

    End Sub
   
End Class
Public Class InicializaRegistros

    Public Shared Sub ArmaNuevaFactura(ByRef Row As DataRow)

        Row("Factura") = 0
        Row("Cliente") = 0
        Row("ClienteOperacion") = 0
        Row("Remito") = 0
        Row("Deposito") = 0
        Row("Sucursal") = 0
        Row("FormaPago") = 0
        Row("EsFCE") = False
        Row("AgenteDeposito") = ""
        Row("FechaPago") = "01/01/1800"
        Row("TipoIva") = 0
        Row("Fecha") = Date.Now
        Row("Estado") = 2
        Row("Recibo") = 0
        Row("Rel") = False
        Row("Relacionada") = 0
        Row("EsServicios") = 0
        Row("Final") = 1
        Row("PorUnidad") = 1
        Row("Comentario") = ""
        Row("Importe") = 0
        Row("Percepciones") = 0
        Row("ImporteDev") = 0
        Row("Saldo") = 0
        Row("Senia") = 0
        Row("Bultos") = 0
        Row("Vendedor") = 0
        Row("Moneda") = 1
        Row("Cambio") = 1
        Row("FechaElectronica") = "01/01/1800"
        Row("FechaContable") = "01/01/1800"
        Row("EsExterior") = False
        Row("EsSecos") = False
        Row("EsElectronica") = False
        Row("IncoTerm") = 0
        Row("Pedido") = 0
        Row("FechaEntrega") = Date.Now
        Row("PedidoCliente") = ""
        Row("Confirmado") = False
        Row("Descuento") = 0
        Row("Cae") = 0
        Row("FechaCae") = 0
        Row("Tr") = False
        Row("EsZ") = False
        Row("ComprobanteDesde") = 0
        Row("ComprobanteHasta") = 0

    End Sub
    Public Shared Sub ArmaNuevaFacturaDetalle(ByRef Row As DataRow)

        Row("Indice") = 0
        Row("Factura") = 0
        Row("Articulo") = 0
        Row("TipoPrecio") = 0
        Row("KilosXUnidad") = 0
        Row("Iva") = 0
        Row("Precio") = 0
        Row("PrecioLista") = 0
        Row("Cantidad") = 0
        Row("TotalArticulo") = 0
        Row("Devueltas") = 0
        Row("Descuento") = 0
        Row("Remito") = 0
        Row("Senia") = 0

    End Sub
    Public Shared Sub ArmaArchivoMediosPago(ByRef DtGrid As DataTable)

        DtGrid = New DataTable

        Dim Item As New DataColumn("Item")
        Item.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Item)

        Dim MedioPago As New DataColumn("MedioPago")
        MedioPago.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(MedioPago)

        Dim Detalle As New DataColumn("Detalle")
        Detalle.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Detalle)

        Dim Neto As New DataColumn("Neto")
        Neto.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Neto)

        Dim Alicuota As New DataColumn("Alicuota")
        Alicuota.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Alicuota)

        Dim ImporteIva As New DataColumn("ImporteIva")
        ImporteIva.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(ImporteIva)

        Dim Cambio As New DataColumn("Cambio")
        Cambio.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Cambio)

        Dim Importe As New DataColumn("Importe")
        Importe.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Importe)

        Dim Bultos As New DataColumn("Bultos")
        Bultos.DataType = System.Type.GetType("System.Decimal")
        DtGrid.Columns.Add(Bultos)

        Dim Banco As New DataColumn("Banco")
        Banco.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Banco)

        Dim Cuenta As New DataColumn("Cuenta")
        Cuenta.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Cuenta)

        Dim Serie As New DataColumn("Serie")
        Serie.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(Serie)

        Dim Numero As New DataColumn("Numero")
        Numero.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(Numero)

        Dim Fecha As New DataColumn("Fecha")
        Fecha.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(Fecha)

        Dim EmisorCheque As New DataColumn("EmisorCheque")
        EmisorCheque.DataType = System.Type.GetType("System.String")
        DtGrid.Columns.Add(EmisorCheque)

        Dim Comprobante As New DataColumn("Comprobante")
        Comprobante.DataType = System.Type.GetType("System.Double")
        DtGrid.Columns.Add(Comprobante)

        Dim FechaComprobante As New DataColumn("FechaComprobante")
        FechaComprobante.DataType = System.Type.GetType("System.DateTime")
        DtGrid.Columns.Add(FechaComprobante)

        Dim ClaveCheque As New DataColumn("ClaveCheque")
        ClaveCheque.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveCheque)

        Dim ClaveInterna As New DataColumn("ClaveInterna")
        ClaveInterna.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveInterna)

        Dim ClaveChequeVisual As New DataColumn("ClaveChequeVisual")
        ClaveChequeVisual.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ClaveChequeVisual)

        Dim NumeracionInicial As New DataColumn("NumeracionInicial")
        NumeracionInicial.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionInicial)

        Dim NumeracionFinal As New DataColumn("NumeracionFinal")
        NumeracionFinal.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(NumeracionFinal)

        Dim TieneLupa As New DataColumn("TieneLupa")
        TieneLupa.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(TieneLupa)

        Dim ID As New DataColumn("ID")
        ID.DataType = System.Type.GetType("System.Int32")
        DtGrid.Columns.Add(ID)

        Dim eCheq As New DataColumn("eCheq")
        eCheq.DataType = System.Type.GetType("System.Boolean")
        DtGrid.Columns.Add(eCheq)

    End Sub

End Class
Public Class MascaraImpresion

    Public Shared Sub ImprimeMascara(ByVal NombreEmpresa As String, ByVal CuitEmpresa As String, ByVal TipoComprobante As Integer, ByVal Direccion1 As String, ByVal Direccion2 As String, ByVal Direccion3 As String, ByVal LetraComprobante As String, ByVal CondicionIva As String, ByVal IngBruto As String, ByVal FechaInicio As String, ByVal DirectorioXML As String, ByRef e As System.Drawing.Printing.PrintPageEventArgs, ByVal EsFCE As Boolean, ByVal TipoNota As Integer)
        'TipoNota solo para saber codigo afip.

        Dim CodigoW As Integer

        Select Case TipoComprobante
            Case 1, 2, 3
                If Not EsFCE Then
                    CodigoW = HallaTipoSegunAfip(HallaNumeroLetra(LetraComprobante), TipoNota)
                Else
                    CodigoW = HallaTipoSegunAfipFCE(HallaNumeroLetra(LetraComprobante), TipoNota)
                End If
        End Select

        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim PrintFont As System.Drawing.Font

        Dim MargenSuperior As Integer = 7
        Dim MargenDerecho As Integer = 4

        Dim xI As Integer = MargenDerecho
        Dim YI As Integer = MargenSuperior
        Dim xD As Integer = 0
        Dim YD As Integer = 0
        Dim Ancho As Integer = 197
        Dim Alto As Integer = 279
        Dim Texto As String
        Dim Codigo As String
        Dim TextoComprobante

        Dim Letra As String = LetraComprobante
        If TipoComprobante = 1 Then
            Codigo = "Cod." & Format(CodigoW, "000")
            TextoComprobante = "F A C T U R A"
        End If
        If TipoComprobante = 2 Then
            Codigo = "Cod." & Format(CodigoW, "000")
            TextoComprobante = "NOTA DE DEBITO"
        End If
        If TipoComprobante = 3 Then
            Codigo = "Cod." & Format(CodigoW, "000")
            TextoComprobante = "NOTA DE CREDITO"
        End If
        If TipoComprobante = 4 Then
            TextoComprobante = "G U I A "
        End If
        If TipoComprobante = 5 Then
            TextoComprobante = "R E M I T O"
        End If
        If TipoComprobante = 12 Then    'internas.
            TextoComprobante = "NOTA DE DEBITO"
        End If
        If TipoComprobante = 13 Then
            TextoComprobante = "NOTA DE CREDITO"  'internas.
        End If
        If TipoComprobante = 50 Then
            TextoComprobante = "ORDEN DE COMPRA"
        End If

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), xI, YI, Ancho, Alto)

        xI = MargenDerecho : YI = MargenSuperior + 55
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto - 20
        xI = MargenDerecho : YI = MargenSuperior + Alto - 24
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto - 21
        xI = MargenDerecho : YI = MargenSuperior + Alto - 25
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        'rectangulo en la letra
        xI = MargenDerecho + 95 : YI = MargenSuperior + 4
        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), xI, YI, 17, 18)
        '
        'Para letra
        PrintFont = New Font("Arial", 36)
        xI = MargenDerecho + 97 : YI = MargenSuperior + 5
        e.Graphics.DrawString(Letra, PrintFont, Brushes.Black, xI, YI)
        Texto = Codigo
        PrintFont = New Font("Arial", 8)
        xI = MargenDerecho + 99 : YI = MargenSuperior + 18
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Tipo Comprbante
        Texto = TextoComprobante
        PrintFont = New Font("Arial", 18)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Select Case TipoComprobante
            Case 12, 13
                Texto = "INTERNA"
                PrintFont = New Font("Arial", 18)
                xI = MargenDerecho + 135 : YI = MargenSuperior + 11
                e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        End Select
        'Nro.
        Texto = "Nro."
        PrintFont = New Font("Arial", 18)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 18
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Fecha
        Texto = "FECHA"
        PrintFont = New Font("Arial", 14)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 30
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Razon Social
        Texto = NombreEmpresa
        If Texto.Length <= 28 Then
            PrintFont = New Font("Arial", 18)
        Else
            PrintFont = New Font("Arial", 16)
        End If
        '        xI = MargenDerecho + 5 : YI = MargenSuperior + 25
        xI = MargenDerecho + 5 : YI = MargenSuperior + 28
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Direccion
        PrintFont = New Font("Arial", 9)
        Texto = Direccion1
        xI = MargenDerecho + 3 : YI = MargenSuperior + 38
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = Direccion2
        xI = MargenDerecho + 3 : YI = MargenSuperior + 41
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = Direccion3
        xI = MargenDerecho + 3 : YI = MargenSuperior + 44
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        PrintFont = New Font("Arial", 10)
        Texto = "Condición frente IVA " & CondicionIva
        xI = MargenDerecho + 3 : YI = MargenSuperior + 49
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'CUIT
        PrintFont = New Font("Arial", 8)
        Texto = "Ing. Bruto " & IngBruto
        xI = MargenDerecho + 120 : YI = MargenSuperior + 48
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = "C.U.I.T. Nª " & CuitEmpresa
        xI = MargenDerecho + 120 : YI = MargenSuperior + 45
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = "Fecha Inic. " & FechaInicio
        xI = MargenDerecho + 120 : YI = MargenSuperior + 51
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)

        Try
            Dim Direccionlogo As String = DirectorioXML & "LOGO" & Strings.Left(CuitEmpresa, 2) & Strings.Mid(CuitEmpresa, 4, 8) & Strings.Right(CuitEmpresa, 1) & ".jpg"
            Dim newImage As Image = Image.FromFile(Direccionlogo)
            xI = MargenDerecho : YI = MargenSuperior + 3
            '            e.Graphics.DrawImage(newImage, MargenDerecho, MargenSuperior, 90, 20)
            e.Graphics.DrawImage(newImage, MargenDerecho, MargenSuperior, 90, 28)
        Catch ex As Exception
        End Try

    End Sub
End Class
Public Class MascaraImpresionRemitosFactura

    Public Shared Sub ImprimeMascara(ByVal NombreEmpresa As String, ByVal CuitEmpresa As String, ByVal TipoComprobante As Integer, ByVal Direccion1 As String, ByVal Direccion2 As String, ByVal Direccion3 As String, ByVal LetraComprobante As String, ByVal CondicionIva As String, ByVal IngBruto As String, ByVal FechaInicio As String, ByVal DirectorioXML As String, ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim PrintFont As System.Drawing.Font

        Dim MargenSuperior As Integer = 7
        Dim MargenDerecho As Integer = 4

        Dim xI As Integer = MargenDerecho
        Dim YI As Integer = MargenSuperior
        Dim xD As Integer = 0
        Dim YD As Integer = 0
        Dim Ancho As Integer = 195
        Dim Alto As Integer = 275
        Dim Texto As String
        Dim Codigo As String
        Dim TextoComprobante

        Dim Letra As String = "X"
        TextoComprobante = "REMITOS FACTURA"

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), xI, YI, Ancho, Alto)
        xI = MargenDerecho : YI = MargenSuperior + 55
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto - 20
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        xI = MargenDerecho : YI = MargenSuperior + Alto - 21
        e.Graphics.DrawLine(New Pen(Color.Black, 0.3), xI, YI, xI + Ancho, YI)
        'rectangulo en la letra
        xI = MargenDerecho + 95 : YI = MargenSuperior + 4
        e.Graphics.DrawRectangle(New Pen(Color.Black, 0.3), xI, YI, 17, 18)
        '
        'Para letra
        PrintFont = New Font("Arial", 36)
        xI = MargenDerecho + 97 : YI = MargenSuperior + 5
        e.Graphics.DrawString(Letra, PrintFont, Brushes.Black, xI, YI)
        Texto = Codigo
        PrintFont = New Font("Arial", 8)
        xI = MargenDerecho + 99 : YI = MargenSuperior + 18
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Tipo Comprbante
        Texto = TextoComprobante
        PrintFont = New Font("Arial", 18)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Nro.
        Texto = "Nro."
        PrintFont = New Font("Arial", 18)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 18
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Fecha
        Texto = "FECHA"
        PrintFont = New Font("Arial", 14)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 30
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Razon Social
        Texto = NombreEmpresa
        If Texto.Length <= 28 Then
            PrintFont = New Font("Arial", 18)
        Else
            PrintFont = New Font("Arial", 16)
        End If
        xI = MargenDerecho + 5 : YI = MargenSuperior + 25
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Direccion
        PrintFont = New Font("Arial", 9)
        Texto = Direccion1
        xI = MargenDerecho + 3 : YI = MargenSuperior + 38
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = Direccion2
        xI = MargenDerecho + 3 : YI = MargenSuperior + 41
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = Direccion3
        xI = MargenDerecho + 3 : YI = MargenSuperior + 44
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        PrintFont = New Font("Arial", 10)
        Texto = "Condición frente IVA " & CondicionIva
        xI = MargenDerecho + 3 : YI = MargenSuperior + 49
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'CUIT
        PrintFont = New Font("Arial", 8)
        Texto = "Ing. Bruto " & IngBruto
        xI = MargenDerecho + 120 : YI = MargenSuperior + 48
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = "C.U.I.T. Nª " & CuitEmpresa
        xI = MargenDerecho + 120 : YI = MargenSuperior + 45
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = "Fecha Inic. " & FechaInicio
        xI = MargenDerecho + 120 : YI = MargenSuperior + 51
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)

        Try
            Dim Direccionlogo As String = DirectorioXML & "LOGO" & Strings.Left(CuitEmpresa, 2) & Strings.Mid(CuitEmpresa, 4, 8) & Strings.Right(CuitEmpresa, 1) & ".jpg"
            Dim newImage As Image = Image.FromFile(Direccionlogo)
            xI = MargenDerecho : YI = MargenSuperior + 3
            e.Graphics.DrawImage(newImage, MargenDerecho, MargenSuperior, 90, 20)
        Catch ex As Exception
        End Try

    End Sub
End Class
Public Class MascaraImpresionRemitosN

    Public Shared Sub ImprimeMascara(ByVal NombreEmpresa As String, ByVal MaskedRemito As String, ByRef e As System.Drawing.Printing.PrintPageEventArgs)

        '100 puntos sobre x = 2,50 cm.
        '100 puntos sobre y = 2,50 cm.

        Dim PrintFont As System.Drawing.Font

        Dim MargenSuperior As Integer = 7
        Dim MargenDerecho As Integer = 4

        Dim xI As Integer = MargenDerecho
        Dim YI As Integer = MargenSuperior
        Dim xD As Integer = 0
        Dim YD As Integer = 0
        Dim Ancho As Integer = 195
        Dim Alto As Integer = 275
        Dim Texto As String
        Dim Codigo As String
        Dim TextoComprobante

        Dim Letra As String = " "

        TextoComprobante = "GUIA "

        e.Graphics.PageUnit = GraphicsUnit.Millimeter

        'Tipo Comprbante
        Texto = TextoComprobante
        PrintFont = New Font("Courier New", 18, FontStyle.Regular)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 5
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Nro.
        Texto = "Nro."
        PrintFont = New Font("Courier New", 13, FontStyle.Regular)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 18
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        Texto = NumeroEditado(MaskedRemito)
        PrintFont = New Font("Courier New", 13, FontStyle.Regular)
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI + 10, YI)
        'Fecha
        Texto = "FECHA"
        PrintFont = New Font("Courier New", 12, FontStyle.Regular)
        xI = MargenDerecho + 120 : YI = MargenSuperior + 30
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI, YI)
        'Razon Social
        Texto = NombreEmpresa
        If Texto.Length <= 28 Then
            PrintFont = New Font("Courier New", 13, FontStyle.Regular)
        Else
            PrintFont = New Font("Courier New", 11, FontStyle.Regular)
        End If
        xI = MargenDerecho + 5 : YI = MargenSuperior + 25
        e.Graphics.DrawString(Texto, PrintFont, Brushes.Black, xI + 20, YI)

    End Sub
End Class





