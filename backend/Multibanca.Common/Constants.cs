namespace Multibanca.Common
{
    public class Constants
    {
        public struct WorkFlowMultibanca
        {
            public const string WorkFlowID = "WP_BBVA_CONTACTO_CLIENTE";
        }

        public struct EndPointServiceWF
        {
            //// DESARROLLO ////
            public const string UrlService = "http://localhost:81/WorkFlowServices.asmx";

            //// PRODUCCION ////
            //public const string UrlService = "https://www.cibergestionperu.com/Presto.Workflow.Service/WorkFlowServices.asmx";              
        }

        #region ActividadesId
        public struct Actividades
        {
            public const string CargaOperacionBanco = "_4X3k8H6kEeeGvo3jOJ1wYw";
            public const string RecepcionCargaFabrica = "_qBTPVMV-EeypJfcPB6uWpg";
            public const string CorregirReparoFabrica = "_ab12CD34EfGhIjKlMnOpQr";
            public const string DatosOperacion = "_LCDxkE_wEeWmubI992vDXg";
            public const string DatosDelVendedor = "";
            public const string CorregirReparoIngresarDatosOperacion = "_Zm7QxN4pEeabKf2_L9tR6w";
            public const string RegistrarTasacion = "_C8vLpS3mEecJYh5_X1nD9q";
            public const string AsignarEstudioTitulos = "_R2kWdF8sEedMu7_Z6pT3x";
            public const string CorregirReparoTasacion = "_H9qBnV1tEeePw4_Y8mK2c";
            public const string AsignarEscritura = "_X7mCpR9yEeqHa6_Z1tN4s";
            public const string GenerarPreFiniquito = "_K1qZdF5uEerMb7_Y9wL3x";
            public const string CorregirReparoCalculoDoc = "_J6wLpT4zEelNy2_Y1mD7x";
            public const string CorregirReparoPrefiniquito = "_Bp7Z_W1gQebYz2_G2dX9m";
            public const string CorregirReparoDatosOperacion = "_Zm7QxN4pEeabKf2_L9tR6w";
            public const string CalculoGeneracionDocumento = "_G2tHwN6yEekRf1_Z3vC8q";
            public const string GenerarEstudioTitulos = "_T5xJrC6yEefLa9_W3vN7d";
            public const string CorregirReparoEstudioTitulos = "_M4pKsZ2uEegQn8_X7bR1h";
            public const string VerificarReparoEstudioTitulos = "_D7nYtL5wEehCv3_Z2qM8k";
            public const string RevisarIngresoDatosOperacion = "_P1rFxU9vEeiJs6_Y4tW0n";
            public const string VerificarReparoIngresarDatosOperacion = "_V8mQcK3xEejBd7_X9pL5s";
            public const string CorregirReparoCalculoGenDoc = "_J6wLpT4zEelNy2_Y1mD7x";
            public const string GenerarMemoEscritura = "_B3qRsV8uEemKg5_X6tP9w";
            public const string CorregirReparoGenerarMemoEscritura = "_N9xFdC1vEenZh4_Z8rL2m";
            public const string VisarOperacion = "_F5kTyM7wEeoJq8_Y2nV6p";
            public const string CorregirReparoVisado = "_S4vBnL2xEepWu3_X5qK1d";
            public const string CorregirReparoCdr = "_Jr6P_L3mZebGt2_P8xH5q";
            public const string CorregirReparoCopiasEscrituras = "_Qc6N_R2vFeaNt8_V3sK7z";
            public const string GenerarCartaResguardo = "_Fm4K_N9rWeaBh7_L2tP6z";
            public const string CorregirCartaResguardo = "_Gq7V_M2pXebEr8_M5wJ1c";
            public const string GenerarBorradorEscritura = "_Y8tLsN4vEesPc2_X3mR7q";
            public const string CorregirReparoGenBorradorEscritura = "_W6pRnK8xEetDf4_Z7mQ2v";
            public const string RevisarLiquidacion = "_Cq2A_X4hReaZa6_H5eY3n";
            public const string RevisarDatosOperacion = "_P1rFxU9vEeiJs6_Y4tW0n";
            public const string RegistrarFirmaBancoAcreedorCG = "_Kt9N_V7qAeaHv6_Q1mL4y";
            public const string RealizarAprobacionComercialLegalCdR = "_Hn1D_Q8sYeaFu4_N6kR9v";
            public const string GenerarFiniquito = "_Ym6W_T2dNebVw9_D1aU8h";
            public const string VerificarCorreccionEscritura = "_Ny8T_P4tDebLr2_T6qH1x";
            public const string RegistrarFirmaApoderadoBanco = "_Pa3M_Q7uEebMs5_U9rJ4y";
            //POSTFIRMA
            public const string RealizarControlCredito = "_Cy5N_R7pUebKd9_G3mL8t";
            public const string CorregirReparoControlCredito = "_Dh2Q_X6nPeaJr1_H7vK4y";
            public const string CorregirReparosGestor = "_Ly2I_F4qZebHi6_R5pG3w";
            public const string ControlEscritura = "_Hv9F_C3mWeaEf5_N4kD2t";
            public const string CorregirControlEscritura = "_Jw4G_D6nXebFg8_P7mE5u"; 
            public const string GestionRectificatoria = "_Mz5J_G7rAeaIj9_S8qH6x";
            public const string GestionRectificatoriaSolucionReparo = "_Na8K_H2sBebJk3_T3rI1y";
            public const string GestionReparo = "_Pb3L_I5tCeaKl7_U6sJ4z";
            public const string RectificatoriaLegalCartaResguardo = "_Vj5R_O7zIebQr9_A8yQ6f";
            public const string RectificatoriaLegalFirmaAlzante = "_Wk8S_P2aJeaRs3_B2zR1g";
            public const string RectificatoriaLegalCierreCopias = "_Xm3T_Q5bKebSt7_C5aS4h";
            public const string RectificatoriaLegalCierreCopiasPostventa = "_Ds5Z_W7hQebYz9_J7gY6q";
            public const string GestionRectificatoriaEscrituraFirmada = "_Uh2Q_N4yHeaPq6_Z5xP3e";
            public const string GestionRectificatoriaEscrituraFirmadaPostventa = "_Cr2Y_V4gPeaXy6_H4fX3p";
            public const string RectificatoriaAnalisisDerivacionReparoPostventa = "_Yn6U_R8cLeaTu1_D8bT7j";
            public const string RectificatoriaPostventaSolucionReparo = "_Zo9V_S3dMebUv5_E3cU2k";
            public const string ValidacionRectificatoriaLegalPostventa = "_Ap4W_T6eNeaVw8_F6dV5m";
            public const string RectificatoriaFirmaPostventa = "_Bq7X_U1fOebWx2_G9eW8n";
            public const string CorregirReparoLiquidacion = "_Dr5B_Y7iSebAb9_J9fZ6p";
            public const string RegistrarFirmaComprador = "_Ak3L_P8qTebYv7_D2nH5x";
            public const string RegistrarFirmaVendedor = "_Br9T_K1mQeaWp4_F8cJ2s";
            public const string RealizarRevisionPrevioFirmaBanco = "_Lw2Q_M8rBebJp3_R7nD5t";
            public const string CorregirNotariaReparoAbogados = "_Mx5V_N1sCeaKq9_S2pF8w";
            public const string RealizarCierreCopiasNotaria = "_Hz2B_Y6xTebUc5_N9fZ2u";
            public const string CorregirReparoCierreCopiasNotaria = "_Gy7A_X3wSebTb1_M6eY8t";
            public const string RevisarCopiasEscrituras = "_Dv6X_U2tPeaQy9_J5bV7q";
            public const string RevisarInscripcionCbr = "_Wj8U_R4bLebTt2_B3yS1f";
            public const string ReingresarEscrituraCbr = "_Xk3V_S7cMeaUv6_C7zT4g";
            public const string RegistrarFechaRegistroCbr = "_Vh5T_Q1aKeaSy8_A9xR6e";
            public const string ValorizarCbr = "_Rd1P_T9wGebOu4_W5tL2a";
            public const string GenerarRecursosPagosCbr = "_Se4Q_M3xHebPv7_X8uN5b";
            public const string RegistrarEscrituraCbr = "_Tf7R_N6yIeaQw1_Y2vP9c";
            public const string VerificarReparoCbr = "_Ug2S_P8zJebRx5_Z6wQ3d";
            public const string RecibirInstruccionPago = "_Zn9X_U5eOebWx3_E4bV2j";
            public const string CorregirReparoInstPago = "_Ao4Y_V8fPeaXy7_F8cW5k";
            public const string RecepcionarMatriz = "_Ep8L_T3kVebCf5_J9nD2q";

            public const string EntregarCarpeta = "_Ft3D_A5kUeaCd7_L6hB4r";
            public const string CorregirReparoEntregarCarpeta = "_Gu6E_B8lVebDe1_M1jC8s";
            public const string RevisarDesembolso = "_Es8C_Z2jTebBc3_K3gA1q";
            public const string ValidacionRectificatoriaLegal = "_Qd6M_J8uDebLm1_V9tK7a";
            public const string ReparoFormulario = "_Sf4O_L6wFeaNo8_X7vM5c";
            public const string RectificatoriaFirma = "_Tg7P_M1xGebOp2_Y1wN9d";
        }
        #endregion

        #region Transiciones
        public struct Transiciones
        {
            public const string CierreCopiasEntregarCarpeta = "";
            public const string CierreCopiasVerificarReparoCbr = "";
            public const string CierreCopiasPostventaEntregarCarpeta = "";
            public const string CierreCopiasPostventaVerificarReparoCbr = "";
            public const string GestionRectificatoriaEscrituraFirmada = "gestion_rectificatoria_rscritura_firmada";
            public const string DefinirInmuebleAvanzar = "TR_006";
            public const string DocsClienteAvanzar = "TR_009";
            public const string SoportesPagoAvanzar = "TR_017";
        }
        #endregion

        #region Documentos
        public struct Documentos
        {
            public const int MemorandumEscrituracionDefault = 99;
            public const string MemorandumEscrituracionDescripcion = "Memorandum de escrituración";
            public const string MemorandumEscrituracionTipo = "Documento generado por el sistema";
            public const string RecepcionCargaFabrica = "_4X3k8H6kEeeGvo3jOJ1wYw";
            public const string CorregirReparoFabrica = "_qBTPVMV-EeypJfcPB6uWpg";
            public const string CalculoGeneracionDocumento = "_G2tHwN6yEekRf1_Z3vC8q";
            public const string CorregirReparoCalculoGeneracionDocumento = "_J6wLpT4zEelNy2_Y1mD7x";
            public const string GenerarMemoEscritura = "_B3qRsV8uEemKg5_X6tP9w";
        }
        #endregion

        #region Config_AWS3
        public struct AWS3
        {
            public const string BucketConfig = "";
        }
        #endregion

        #region StorageLocal
        public struct StorageLocal
        {
            public const string filePath = "C:\\FilesExpDig";
        }
        #endregion

        public struct Path
        {
            public const string templates = "Templates";
        }

        public struct PathFolder
        {
            public const string expediente_digital = "ExpedienteDigital";
        }

        public struct Catalogo
        {
            public const string TipoDocumento = "TIPODOCUMENTO";
            public const string TipoUtilidades = "UTILIDADES";
            public const string TipoMoneda = "TIPOMONEDA";
            public const string TipoDetalleError = "TIPODETALLEERROR";
            public const string TipoDocumentoValido = "TIPODOCUMENTOVALIDO";
            public const string TipoPerfiles = "PERFILES";
            public const string TipoRegion = "TIPOREGION";
            public const string TipoComuna = "TIPOCOMUNA";
            public const string Perfiles = "PERFILES";
            public const string InstitucionesBancoAcreedor = "INSTITUCIONES_BANCO_ACREEDOR";

            /* TipoBusquedaConsultaAct */
            public const string TipoBusquedaPrestoContabilidad = "BUSQUEDACONSULTASOLICITUD";

            /* Multibanca */
            public const string TipoOperacion = "TIPO_OPERACION";
            public const string Segmento = "SEGMENTO";
            public const string CanalVenta = "CANAL_VENTA";
            public const string ModeloOperacion = "MODELO_OPERACION";
            public const string TipoCarpeta = "TIPO_CARPETA";
            public const string Inmobiliaria = "INMOBILIARIA";
            public const string GlosaProducto = "GLOSA_PRODUCTO";
            public const string CodigoProductoComercial = "CODIGO_PRODUCTO_COMERCIAL";
            public const string BancoAlzante = "BANCO_ALZANTE";

            public const string TipoComprador = "TIPO_COMPRADOR";
            public const string TipoVendedor = "TIPO_VENDEDOR";
            public const string Genero = "GENERO";
            public const string EstadoCivil = "ESTADO_CIVIL";
            public const string RelacionTitular = "RELACION_TITULAR";
            public const string Region = "REGION";
            public const string Comuna = "COMUNA";
            public const string Nacionalidad = "NACIONALIDAD";

            public const string TipoPrestamo = "TIPO_PRESTAMO";
            public const string DestinoCredito = "DESTINO_CREDITO";
            public const string TipoTasa = "TIPO_TASA";
            public const string Moneda = "MONEDA";
            public const string TipoFinanciamiento = "TIPO_FINANCIAMIENTO";
            public const string TipoHipoteca = "TIPO_HIPOTECA";
            public const string NombreAbogado = "NOMBREABOGADO";
            public const string TipoReparoVerificarCbr = "TIPO_REPARO_VERIFICAR_CBR";
            public const string TipoReparoRectificatoriaPostVenta = "TIPO_REPARO_RECTIFICATORIA_POSTVENTA";
            public const string CondicionDesembolso = "CONDICION_DESEMBOLSO";
            public const string TipoCarta = "TIPO_CARTA";
            public const string tipoRequerimientoDocumentacion = "TIPO_REQUERIMIENTO_DOCUMENTACION";
            public const string realizaPago = "REALIZA_PAGO";

            // ============================================================
            // DATOS OPERACIÓN 5.4
            // ============================================================

            public const string DatosOperacionSiNo = "DATOS_OPERACION_SI_NO";
            public const string DatosOperacionTipoOperacion = "DATOS_OPERACION_TIPO_OPERACION";
            public const string DatosOperacionTipoDireccionDividendo = "DATOS_OPERACION_TIPO_DIRECCION_DIVIDENDO";
            public const string DatosOperacionTipoVendedor = "DATOS_OPERACION_TIPO_VENDEDOR";
            public const string DatosOperacionGenero = "DATOS_OPERACION_GENERO";
            public const string DatosOperacionEstadoCivil = "DATOS_OPERACION_ESTADO_CIVIL";
            public const string DatosOperacionRegion = "DATOS_OPERACION_REGION";
            public const string DatosOperacionComuna = "DATOS_OPERACION_COMUNA";
            public const string DatosOperacionNacionalidad = "DATOS_OPERACION_NACIONALIDAD";
            public const string DatosOperacionBancoAcreedorInstitucion = "DATOS_OPERACION_BANCO_ACREEDOR_INSTITUCION";
            public const string DatosOperacionTipoPropiedad = "DATOS_OPERACION_TIPO_PROPIEDAD";
            public const string DatosOperacionEstadoPropiedad = "DATOS_OPERACION_ESTADO_PROPIEDAD";
            public const string DatosOperacionTipoVenta = "DATOS_OPERACION_TIPO_VENTA";
            public const string DatosOperacionTipoConstruccion = "DATOS_OPERACION_TIPO_CONSTRUCCION";
            public const string DatosOperacionTipoDireccion = "DATOS_OPERACION_TIPO_DIRECCION";
            public const string DatosOperacionExisteRolAvaluo = "DATOS_OPERACION_EXISTE_ROL_AVALUO";

            // ============================================================
            // DATOS GENERAR BORRADOR ESCRITURA 5.20
            // ============================================================
            public const string Notaria = "NOTARIA";
            public const string RolComparecencia = "ROLCOMPARECENCIA";

            // ============================================================
            // Gestion Rectificatoria 6.62
            // ============================================================
            public const string GestionRectificatoriaTipoReparo = "GESTION_RECTIFICATORIA_TIPO_REPARO";

            // ============================================================
            // Gestion GestionRectificatoriaEscrituraFirmada 6.67
            // ============================================================
            public const string GestionRectificatoriaEscrituraFirmadaTipoReparo = "GR_ESCRITURA_FIRMADA_TIPO_REPARO";

            // ============================================================
            // Gestion GestionRectificatoriaEscriturFirmadaPostventa 7.103
            // ============================================================
            public const string GestionRectificatoriaEscriturFirmadaPostventaTipoReparo = "GR_ESCRITURA_FIRMADA_POSTVENTA_TIPO_REPARO";

            /* VALOR UF ACTUAL */
            public const string ValorUFActual = "VALOR_UF_ACTUAL";

            // ============================================================
            // BBVA COLOMBIA — tipos de catálogo nuevos
            // ============================================================
            public const string TipoDocumentoId     = "TIPO_DOCUMENTO_ID";
            public const string TipoSubproducto     = "TIPO_SUBPRODUCTO";
            public const string ModeloCarta         = "MODELO_CARTA";
            public const string SubproductoModelo   = "SUBPRODUCTO_MODELO";
            public const string EstatusGeneral      = "ESTATUS_GENERAL";
            public const string MotivoDevolucion    = "MOTIVO_DEVOLUCION";
            public const string SegmentoBbva        = "SEGMENTO";
            public const string CanalOriginacion    = "CANAL_ORIGINACION";
            public const string TipoInmueble        = "TIPO_INMUEBLE";
            public const string EstadoInmueble      = "ESTADO_INMUEBLE";
            public const string SituacionLaboral    = "SITUACION_LABORAL";
            public const string Departamento        = "DEPARTAMENTO";
            public const string Municipio           = "MUNICIPIO";
            public const string CanalContacto       = "CANAL_CONTACTO";
            public const string ResultadoContacto   = "RESULTADO_CONTACTO";
            public const string DetalleContacto     = "DETALLE_CONTACTO";
            public const string TipoFirma           = "TIPO_FIRMA";
            public const string TipoCredito         = "TIPO_CREDITO";
            public const string EscrituracionTiposCredito = "ESCRITURACION_TIPO_CREDITO";
            public const string TipoCreditoLeasing  = "ESCRITURACION_TIPO_CREDITO_LEASING";
            public const string TipoCreditoCXI      = "ESCRITURACION_TIPO_CREDITO_CXI";
            public const string MotivoCierre        = "MOTIVO_CIERRE";
            public const string CodigoOficina       = "CODIGO_OFICINA";
            public const string RepresentanteLegal_L38 = "L38_REPRESENTANTE_LEGAL";
            public const string TipologiaCorreccionEP_L39 = "L39_TIPOLOGIA_DEVOLUCION_EP_ABOGADO";
            public const string CasuisticaCorreccionEP_L40 = "L40_CASUISTICA_DEVOLUCION_EP_ABOGADO";
            public const string TipologiaEscalamiento = "TIPOLOGIA_ESCALAMIENTO";
            public const string TipoBoleta = "TIPO_BOLETA";
            public const string OficinaRegistro = "OFICINA_REGISTRO";
        }

        // ============================================================
        // BBVA COLOMBIA — IDs de actividades Presto Legalización
        // Deben coincidir exactamente con los id_actividad en cat_actividades_ws
        // ============================================================
        public static class ActividadesBBVA
        {
            public const string CargarCreditos = "ACT_CARGAR_CREDITOS";
            public const string CartaAprobacion = "ACT_CARTA_APROBACION";
            public const string RadicarCredito = "ACT_RADICAR_CREDITO";
            public const string ValidarInformacion = "BBVA_CONTACTO_VALIDAR_INFORMACION_8DAA7A61";
            public const string DefinirInmueble = "BBVA_CONTACTO_DEFINIR_INMUEBLE_EA84B803";
            public const string DocsConstructora = "BBVA_CONTACTO_CARGAR_DOCUMENTOS_CONSTRUCTORA_45B34EE0";
            public const string DocsCliente = "BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738";
            public const string RevisarDocs = "BBVA_CONTACTO_REVISAR_DOCUMENTOS_INMUEBLE_810753B8";
            public const string AsignarFirmas = "BBVA_CONTACTO_ASIGNAR_FIRMAS_PERITOS_Y_ABOGADOS_BF7D9447";
            public const string SoportesPago = "BBVA_CONTACTO_CARGAR_SOPORTES_DE_PAGO_899F408B";
            public const string GestionarFirma = "BBVA_CONTACTO_GESTIONAR_FIRMA_A55478C8";
            public const string FirmaFisica = "BBVA_CONTACTO_GESTIONAR_FIRMAS_FISICA_A256C783";
            public const string ValidarIntegracion = "BBVA_CONTACTO_VALIDAR_INTEGRACION_DE_DOCUMENTOS_2B87EDAF";
            public const string DevolucionVbComercial = "BBVA_CONTACTO_REALIZAR_DEVOLUCION_PENDIENTE_VB_COMERCIAL_C4C97D10";

            // Actividades destino para verificar conceptos previos (enrutamiento)
            public const string EscrituracionFirmarEscrituraCliente = "BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F";
            public const string EscrituracionFirmarRepLegal = "BBVA_ESCRITURACION_FIRMAR_REP_LEGAL";
            public const string EscrituracionRealizarEntregaEpFirmada = "BBVA_ESCRITURACION_REALIZAR_ENTREGA_EP_FIRMADA";
            public const string EscrituracionRevisarEPAbogado = "ACT_REVISAR_EP_ABOGADO";
            public const string EscrituracionVBProrrata = "ACT_VB_PRORRATA";
            public const string EscrituracionRealizarCausacion = "ACT_REALIZAR_CAUSACION";
            public const string EscrituracionRealizarRecepcionBoleta = "BBVA_ESCRITURACION_REALIZAR_RECEPCION_BOLETA";
            public const string EscrituracionRealizarEPRegistradas = "BBVA_ESCRITURACION_REALIZAR_EP_REGISTRADAS";
            public const string EscrituracionRealizarVBFinalAbogado = "BBVA_ESCRITURACION_REALIZAR_VB_FINAL_ABOGADO";
        }

        // ============================================================
        // BBVA COLOMBIA — IDs de transiciones del Expediente Digital
        // ============================================================
        public static class TransicionesBBVA
        {
            // BBVA Escrituración — Firmar Escritura Cliente
            // TODO: Reemplazar con los nombres reales de transición del XPDL
            public const string FirmarEscClienteAEscalamientoComercial = "TR_FIRMAR_ESC_ESCALAMIENTO_COMERCIAL";
            public const string FirmarEscClienteARevisarEP = "TR_FIRMAR_ESC_REVISAR_EP";
            public const string FirmarEscClienteAVBProrrata = "TR_FIRMAR_ESC_VB_PRORRATA";
            public const string FirmarEscClienteACausacion = "TR_FIRMAR_ESC_CAUSACION";

            // BBVA Escrituración — Firmar Rep. Legal
            // TODO: Reemplazar con los nombres reales de transición del XPDL
            public const string FirmarRepLegalEntregaEP = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_ENTREGA_EP";
            public const string FirmarRepLegalDevolucion = "BBVA_ESCRITURACION_TR_FIRMAR_REP_LEGAL_DEVOLUCION";

            // BBVA Escrituración — Realizar Entrega EP Firmada
            public const string EntregaEpFirmadaRecepcionBoleta = "BBVA_ESCRITURACION_TR_ENTREGA_EP_RECEPCION_BOLETA";
            public const string EntregaEpFirmadaExcepcionDesembolso = "BBVA_ESCRITURACION_TR_ENTREGA_EP_EXCEPCION_DESEMBOLSO";

            // BBVA Escrituración — Realizar Recepción Boleta
            public const string RecepcionBoletaEPRegistradas = "BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EP_REGISTRADAS";
            public const string RecepcionBoletaExcepcionDesembolso = "BBVA_ESCRITURACION_TR_RECEPCION_BOLETA_EXCEPCION_DESEMBOLSO";

            // BBVA Escrituración — Realizar EP Registradas
            public const string EPRegistradasVBFinalAbogado = "BBVA_ESCRITURACION_TR_EP_REGISTRADAS_VB_FINAL_ABOGADO";
            // BBVA Escrituración — Revisar EP Abogado
            public const string RevisarEPAFirmarRepLegal = "TR_REVISAR_EP_FIRMAR_REP_LEGAL";
            public const string RevisarEPADevolucion = "TR_REVISAR_EP_DEVOLUCION";
        }

        // ============================================================
        // BBVA COLOMBIA — IDs de documentos del Expediente Digital
        // Deben coincidir con cat_expediente_digital_documentos
        // ============================================================
        public static class DocumentosBBVA
        {
            public const int PromesaCompraventa = 1001;
            public const int CartaAprobacion    = 1002;
        }

        // ============================================================
        // Mensajes API compartidos
        // ============================================================
        public static class ApiMessages
        {
            public const string ErrorInterno = "Ocurrio un error interno.";
        }

        // ============================================================
        // Acceso temporal - codigos funcionales de validacion
        // ============================================================
        public static class AccesoTemporal
        {
            public const string TokenInvalido = "TOKEN_INVALIDO";
            public const string TokenUsado = "TOKEN_USADO";
            public const string TokenExpirado = "TOKEN_EXPIRADO";
            public const string ClaimToken = "token_acceso_temporal";
            public const string ClaimTipoAcceso = "tipo_acceso";
            public const string TipoAccesoTemporal = "temporal";
            public const string RolCliente = "CLIENTE";
        }
    }
}
