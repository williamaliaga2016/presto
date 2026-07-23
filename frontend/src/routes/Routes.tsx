import { BrowserRouter, Navigate, Route, Routes } from "react-router-dom";
import { PermissionDeniedListener } from "@/shared/components/security/PermissionDeniedListener";

import LoginPage from "@/features/auth/pages/Login";
import AccesoTemporalPage from "@/features/acceso_temporal/pages/AccesoTemporalPage";
import HomePage from "@/features/home/pages/Home";
import BandejaActividades from "@/features/bandeja_actividades/pages/BandejaActividades";
import ReportesPage from "@/features/reportes/pages/Reportes";
import { ProtectedRoute } from "@/app/router/ProtectedRoute";
import { AdminRoute } from "@/app/router/AdminRoute";
import NotFound from "@/shared/pages/NotFound";
import { useAuth } from "@/app/providers/AuthProvider";
import ConsultActivityPage from "@/features/consult_activity/page/ConsultActivity";
import RecepcionCargaFabricaPage from "@/features/actividades/recepcion_carga_fabrica/pages/recepcion_carga_fabrica_page";
import CorregirReparoFabricaPage from "@/features/actividades/corregir_reparo_fabrica/pages/corregir_reparo_fabrica_page";
import CorregirReparoTasacionPage from "@/features/actividades/corregir_reparo_tasacion/pages/corregir_reparo_tasacion_page";
import GenerarMemoEscrituraPage from "@/features/actividades/generar_memo_escritura/pages/generar_memo_escritura_page";
import CorregirReparoCalculoDocPage from "@/features/actividades/corregir_reparo_calculo_doc/pages/corregir_reparo_calculo_doc_page";
import CorregirReparoDatosOperacionPage from "@/features/actividades/corregir_reparo_datos_operacion/pages/corregir_reparo_datos_operacion_page";
import GenerarPreFiniquitoPage from "@/features/actividades/generar_preFiniquito/pages/generar_preFiniquito_page";
import CorregirReparoPrefiniquitoPage from "@/features/actividades/corregir_reparo_prefiniquito/pages/corregir_reparo_prefiniquito_page";
import CargaOperacionBancoPage from "@/features/actividades/carga_operacion_banco/pages/carga_operacion_banco_page";
import AsignarEscrituraPage from "@/features/actividades/asignar_escritura/pages/AsignarEscrituraPage";
import AsignarEstudioTitulosPage from "@/features/actividades/asignar_estudio_titulos/pages/AsignarEstudioTitulosPage";
import AdministracionUsuarios from "@/features/administracion/usuarios/pages/AdministracionUsuarios";
import DatosOperacionPage from "@/features/actividades/datos_operacion/pages/DatosOperacion";
import RevisarDatosOperacionPage from "@/features/actividades/revisar_datos_operacion/pages/RevisarDatosOperacion";
import ReasignacionDesestimacionPage from "@/shared/components/ReasignacionDesestimacionPage";
import RevisarIngresoDatosCreditoPage from "@/features/actividades/revisar_ingreso_datos_credito/pages/revisar_ingreso_datos_credito_page";
import VerificarReparoEstudioTituloPage from "@/features/actividades/verificar_reparo_estudio_titulo/pages/verificar_reparo_estudio_titulo_page";
import CalculoGeneracionDocumentoPage from "@/features/actividades/calculo_generacion_documento/pages/calculo_generacion_documento_page";
import RegistrarTasacionPage from "@/features/actividades/registrar_tasacion/pages/registrar_tasacion_page";
import GenerarEstudioTitulosPage from "@/features/actividades/generar_estudio_titulos/pages/generar_estudio_titulos_page";
import GenerarBorradorEscritura from "@/features/actividades/generar_borrador_escritura/pages/generar_borrador_escritura";

import CorregirReparoVisadoPage from "@/features/actividades/corregir_reparo_visado/pages/corregir_reparo_visado_page";
import CorregirCartaResguardoPage from "@/features/actividades/corregir_carta_resguardo/pages/corregir_carta_resguardo_page";
import CorregirReparoEstudioTitulosPage from "@/features/actividades/corregir_reparo_estudio_titulos/pages/corregir_reparo_estudio_titulos_page";
import CorregirReparoGenerarMemoEscrituraPage from "@/features/actividades/corregir_reparo_generar_memo_escritura/pages/corregir_reparo_generar_memo_escritura_page";
import CorregirReparoGenerarBorradorEscrituraPage from "@/features/actividades/corregir_reparo_generar_borrador_escritura/pages/corregir_reparo_generar_borrador_escritura_page";
import VerificarReparoIngresarDatosOperacionPage from "@/features/actividades/verificar_reparo_ingresar_datos_operacion/pages/verificar_reparo_ingresar_datos_operacion_page";
import RealizarControlCreditoPage from "@/features/actividades/realizar_control_credito/pages/realizar_control_credito";
import CorregirReparoControlCreditoPage from "@/features/actividades/corregir_reparo_control_credito/pages/corregir_reparo_control_credito_page";
import AprobacionComercialLegalCdRPage from "@/features/actividades/aprobacion_comercial_legal_cdr/pages/aprobacion_comercial_legal_cdr_page";
import VisarOperacionPage from "@/features/actividades/visar_operacion/pages/visar_operacion_page";
import RealizarRevisionPrevioFirmaBancoPage from "@/features/actividades/realizar_revision_previo_firma_banco/pages/realizar_revision_previo_firma_banco_page";
import RegistrarEscrituraCbrPage from "@/features/actividades/registrar_escritura_cbr/pages/registrar_escritura_cbr_page";
import RecepcionarMatrizPage from "@/features/actividades/recepcionar_matriz/pages/recepcionar_matriz_page";
import GenerarRecursosPagosCbrPage from "@/features/actividades/generar_recursos_pagos_cbr/pages/generar_recursos_pagos_cbr_page";
import GenerarCartaResguardoPage from "@/features/actividades/generar_carta_resguardo/pages/generar_carta_resguardo_page";
import CorregirNotariaReparoAbogadosPage from "@/features/actividades/corregir_notaria_reparo_abogados/pages/corregir_notaria_reparo_abogados_page";
import RegistrarFirmaCompradorPage from "@/features/actividades/registrar_firma_comprador/pages/RegistrarFirmaComprador";
import RegistrarFirmaVendedorPage from "@/features/actividades/registrar_firma_vendedor/pages/RegistrarFirmaVendedor";
import RegistrarFirmaBancoAcreedorCGPage from "@/features/actividades/registrar_firma_banco_acreedor_cg/pages/registrar_firma_banco_acreedor_cg_page";
import CorregirReparoCdrPage from "@/features/actividades/corregir_reparo_cdr/pages/corregir_reparo_cdr_page";
import CorregirReparoCopiasEscriturasPage from "@/features/actividades/corregir_reparo_copias_escrituras/pages/corregir_reparo_copias_escritura_page";
import CierreCopiasNotariaPage from "@/features/actividades/cierre_copias_notaria/pages/cierre_copias_notaria_page";
import VerificarCorreccionEscrituraPage from "@/features/actividades/verificar_correccion_escritura/pages/verificar_correccion_escritura_page";
import CorregirReparoCierreCopiasNotariaPage from "@/features/actividades/corregir_reparo_cierre_copias_notaria/pages/corregir_reparo_cierre_copias_notaria_page";
import RegistrarFirmaApoderadoBancoPage from "@/features/actividades/registrar_firma_apoderado_banco/pages/registrar_firma_apoderado_banco_page";
import GenerarFiniquitoPage from "@/features/actividades/generar_finiquito/pages/generar_finiquito_page";
import VerificarReparoCbrPage from "@/features/actividades/verificar_reparo_cbr/pages/verificar_reparo_cbr_page";
import RecibirInstruccionPagoPage from "@/features/actividades/recibir_instruccion_pago/pages/recibir_instruccion_pago_page";
import EntregarCarpetaPage from "@/features/actividades/entregar_carpeta/pages/entregar_carpeta_page";
import CorregirReparoInstPagoPage from "@/features/actividades/corregir_reparo_inst_pago/pages/corregir_reparo_inst_pago_page";
import CorregirReparosGestorPage from "@/features/actividades/corregir_reparos_gestor/pages/corregir_reparos_gestor_page";
import CorregirReparoEntregarCarpetaPage from "@/features/actividades/corregir_reparo_entregar_carpeta/pages/corregir_reparo_entregar_carpeta_page";
import CorregirRevisarInscripcionCbrPage from "@/features/actividades/revisar_inscripcion_cbr/pages/revisar_inscripcion_cbr_page";
import ReingresarEscrituraCbrPage from "@/features/actividades/reingresar_escritura_cbr/pages/reingresar_escritura_cbr_page";
import CorregirReparoLiquidacionPage from "@/features/actividades/corregir_reparo_liquidacion/pages/corregir_reparo_liquidacion_page";
import RevisarLiquidacionPage from "@/features/actividades/revisar_liquidacion/pages/revisar_liquidacion_page";
import RevisarDesembolsoPage from "@/features/actividades/revisar_desembolso/pages/revisar_desembolso_page";
import ReparoFormularioPage from "@/features/actividades/reparo_formulario/pages/reparo_formulario_page";
import RectificatoriaFirmaPage from "@/features/actividades/rectificatoria_firma/pages/rectificatoria_firma_page";
import ControlEscrituraPage from "@/features/actividades/control_escritura/pages/control_escritura_page";
import CorregirControlEscrituraPage from "@/features/actividades/corregir_control_escritura/pages/corregir_control_escritura_page";
import ValidacionRectificatoriaLegalPage from "@/features/actividades/validacion_rectificatoria_legal/pages/validacion_rectificatoria_legal_page";
import GestionRectificatoriaPage from "@/features/actividades/gestion_rectificatoria/pages/gestion_rectificatoria_page";
import GestionRectificatoriaSolucionReparoPage from "@/features/actividades/gestion_rectificatoria_solucion_reparo/pages/gestion_rectificatoria_solucion_reparo_page";
import GestionReparoPage from "@/features/actividades/gestion_reparo/pages/gestion_reparo_page";
import RectificatoriaAnalisisDerivacionReparoPostventaPage from "@/features/actividades/rectificatoria_analisis_derivacion_reparo_postventa/pages/rectificatoria_analisis_derivacion_reparo_postventa_page";
import RectificatoriaPostventaSolucionReparoPage from "@/features/actividades/rectificatoria_postventa_solucion_reparo/pages/rectificatoria_postventa_solucion_reparo_page";
import ValorizarCbrPage from "@/features/actividades/valorizar_cbr/pages/valorizar_cbr_page";
import RegistrarFechaRegistroCbrPage from "@/features/actividades/registrar_fecha_registro_cbr/pages/registrar_fecha_registro_cbr_page";
import ValidacionRectificatoriaLegalPostventaPage from "@/features/actividades/validacion_rectificatoria_legal_postventa/pages/validacion_rectificatoria_legal_postventa_page";
import GestionRectificatoriaEscrituraFirmadaPage from "@/features/actividades/gestion_rectificatoria_escritura_firmada/pages/gestion_rectificatoria_escritura_firmada_page";
import RectificatoriaLegalCartaResguardoPage from "@/features/actividades/rectificatoria_legal_carta_resguardo/pages/rectificatoria_legal_carta_resguardo_page";
import RectificatoriaLegalFirmaAlzantePage from "@/features/actividades/rectificatoria_legal_firma_alzante/pages/rectificatoria_legal_firma_alzante_page";
import RectificatoriaLegalCierreCopiasPage from "@/features/actividades/rectificatoria_legal_cierre_copias/pages/rectificatoria_legal_cierre_copias_page";
import RectificatoriaLegalCierreCopiasPostventaPage from "@/features/actividades/rectificatoria_legal_cierre_copias_postventa/pages/rectificatoria_legal_cierre_copias_postventa_page";
import GestionRectificatoriaEscrituraFirmadaPostventaPage from "@/features/actividades/gestion_rectificatoria_escritura_firmada_postventa/pages/gestion_rectificatoria_escritura_firmada_postventa_page";
import RectificatoriaFirmaPostVentaPage from "@/features/actividades/rectificatoria_firma_post_venta/pages/rectificatoria_firma_post_venta_page";
import RevisarCopiasEscriturasPage from "@/features/actividades/revisar_copias_escrituras/pages/revisar_copias_escritura_page";
import CargarDocumentosConstructoraPage from "@/features/actividades/cargar_documentos_constructora/pages/cargar_documentos_constructora_page";
import RevisarDocumentosInmueblePage from "@/features/actividades/revisar_documentos_inmueble/pages/revisar_documentos_inmueble_page";
import AdministracionRolesPage from "@/features/administracion/roles/pages/administracion_roles";
import ValidarInformacionPage from
  "@/features/actividades/validar_informacion/pages/validar_informacion_page";
import CargarDocumentosClientePage from
  "@/features/actividades/cargar_documentos_cliente/pages/cargar_documentos_cliente_page";
import AsignarFirmasPage from
  "@/features/actividades/asignar_firmas_peritos_abogados/pages/AsignarFirmasPage";
import AsignarFirmasAccesoPage from
  "@/features/actividades/asignar_firmas_peritos_abogados/pages/AsignarFirmasAccesoPage";
import ValidarIntegracionDocumentosPage from "@/features/actividades/validar_integracion_documentos/pages/validar_integracion_documentos_page";
import DevolucionVbComercialPage from "@/features/actividades/devolucion_vb_comercial/pages/devolucion_vb_comercial_page";

import DefinirInmueblePage from "@/features/actividades/definir_inmueble/pages/definir_inmueble_page";
import CargarSoportesPagoPage from "@/features/actividades/cargar_soportes_pago/pages/cargar_soportes_pago_page";
import GestionarFirmaPage from "@/features/actividades/gestionar_firma/pages/gestionar_firma_page";
import GestionarFirmaFisicaPage from "@/features/actividades/gestionar_firma_fisica/pages/gestionar_firma_fisica_page";
import FirmarEscrituraClientePage from "@/features/actividades/firmar_escritura_cliente/pages/firmar_escritura_cliente_page";
import FirmarRepLegalPage from "@/features/actividades/firmar_rep_legal/pages/firmar_rep_legal_page";

export default function AppRouter() {
  const { isAuthenticated } = useAuth();

  return (
    <BrowserRouter>
      <PermissionDeniedListener />
      <Routes>
        <Route
          path="/"
          element={<Navigate to={isAuthenticated ? "/home/bandeja" : "/login"} replace />}
        />

        <Route
          path="/login"
          element={isAuthenticated ? <Navigate to="/home/bandeja" replace /> : <LoginPage />}
        />

        <Route path="/acceso-temporal" element={<AccesoTemporalPage />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/home" element={<HomePage />}>
            <Route element={<AdminRoute />}>
              <Route path="administracion_usuarios" element={<AdministracionUsuarios />} />
              <Route path="administracion_roles" element={<AdministracionRolesPage />} />
              <Route path="reasignacion-desestimacion" element={<ReasignacionDesestimacionPage />} />
            </Route>
            <Route path="bandeja" element={<BandejaActividades />} />
            <Route path="reportes" element={<ReportesPage />} />
            <Route path="consulta_actividades" element={<ConsultActivityPage />} />
            <Route path="carga_operacion_banco" element={<CargaOperacionBancoPage />} />
            <Route path="carga_operacion_banco/:id_expediente" element={<CargaOperacionBancoPage />} />
            <Route
              path="validar_informacion/:id_expediente"
              element={<ValidarInformacionPage />}
            />
            <Route
              path="cargar_documentos_cliente/:id_expediente"
              element={<CargarDocumentosClientePage />}
            />
            <Route
              path="asignar_firmas_peritos_abogados/:id_expediente"
              element={<AsignarFirmasPage />}
            />
            <Route
              path="asignar_firmas/:id_expediente"
              element={<AsignarFirmasPage />}
            />
            <Route path="asignar_firmas" element={<AsignarFirmasAccesoPage />} />
            <Route path="recepcion_carga_fabrica/:id_expediente" element={<RecepcionCargaFabricaPage />} />
            <Route path="corregir_reparo_fabrica/:id_expediente" element={<CorregirReparoFabricaPage />} />
            <Route path="corregir_reparo_tasacion/:id_expediente" element={<CorregirReparoTasacionPage />} />
            <Route path="generar_memo_escritura/:id_expediente" element={<GenerarMemoEscrituraPage />} />
            <Route path="corregir_reparo_generar_memo_escritura/:id_expediente" element={<CorregirReparoGenerarMemoEscrituraPage />} />
            <Route path="corregir_reparo_generar_borrador_escritura/:id_expediente" element={<CorregirReparoGenerarBorradorEscrituraPage />} />
            <Route path="corregir_reparo_calculo_generacion_documento/:id_expediente" element={<CorregirReparoCalculoDocPage />} />
            <Route path="corregir_reparo_datos_operacion/:id_expediente" element={<CorregirReparoDatosOperacionPage />} />
            <Route path="datos_operacion/:id_expediente" element={<DatosOperacionPage />} />
            <Route path="revisar_datos_operacion/:id_expediente" element={<RevisarDatosOperacionPage />} />
            <Route path="asignar_escritura/:id_expediente" element={<AsignarEscrituraPage />} />
            <Route path="asignar_estudio_titulos/:id_expediente" element={<AsignarEstudioTitulosPage />} />
            <Route path="calculo_generacion_documento/:id_expediente" element={<CalculoGeneracionDocumentoPage />} />
            <Route path="registrar_tasacion/:id_expediente" element={<RegistrarTasacionPage />} />
            <Route path="corregir_reparo_estudio_titulos/:id_expediente" element={<CorregirReparoEstudioTitulosPage />} />
            {/* <Route path="alta_solicitud" element={<AltaSolicitudPage />} />
            <Route path="alta_solicitud/:id_expediente" element={<AltaSolicitudPage />} /> */}
            <Route path="generar_prefiniquito/:id_expediente" element={<GenerarPreFiniquitoPage />} />
            <Route path="corregir_reparo_prefiniquito/:id_expediente" element={<CorregirReparoPrefiniquitoPage />} />
            <Route path="revisar_ingreso_datos_credito/:id_expediente" element={<RevisarIngresoDatosCreditoPage />} />
            <Route path="verificar_reparo_estudio_titulos/:id_expediente" element={<VerificarReparoEstudioTituloPage />} />
            <Route path="generar_estudio_titulos/:id_expediente" element={<GenerarEstudioTitulosPage />} />
            <Route path="generar_borrador_escritura/:id_expediente" element={<GenerarBorradorEscritura />} />
            <Route path="corregir_reparo_visado/:id_expediente" element={<CorregirReparoVisadoPage />} />
            <Route path="corregir_carta_resguardo/:id_expediente" element={<CorregirCartaResguardoPage />} />
            <Route path="verificar_reparo_ingreso_datos_operacion/:id_expediente" element={<VerificarReparoIngresarDatosOperacionPage />} />
            <Route path="realizar_control_credito/:id_expediente" element={<RealizarControlCreditoPage />} />
            <Route path="corregir_reparo_control_credito/:id_expediente" element={<CorregirReparoControlCreditoPage />} />
            <Route path="aprobacion_comercial_legal_cdr/:id_expediente" element={<AprobacionComercialLegalCdRPage />} />
            <Route path="realizar_revision_previo_firma_banco/:id_expediente" element={<RealizarRevisionPrevioFirmaBancoPage />} />
            <Route path="registrar_escritura_cbr/:id_expediente" element={<RegistrarEscrituraCbrPage />} />
            <Route path="visar_operacion/:id_expediente" element={<VisarOperacionPage />} />
            <Route path="recepcionar_matriz/:id_expediente" element={<RecepcionarMatrizPage />} />
            <Route path="generar_recursos_pagos_cbr/:id_expediente" element={<GenerarRecursosPagosCbrPage />} />
            <Route path="generar_carta_resguardo/:id_expediente" element={<GenerarCartaResguardoPage />} />
            <Route path="corregir_notaria_reparo_abogados/:id_expediente" element={<CorregirNotariaReparoAbogadosPage />} />
            <Route path="registrar_firma_comprador/:id_expediente" element={<RegistrarFirmaCompradorPage />} />
            <Route path="registrar_firma_vendedor/:id_expediente" element={<RegistrarFirmaVendedorPage />} />
            <Route path="registrar_firma_banco_acreedor_cg/:id_expediente" element={<RegistrarFirmaBancoAcreedorCGPage />} />
            <Route path="corregir_reparo_cdr/:id_expediente" element={<CorregirReparoCdrPage />} />
            <Route path="corregir_reparo_copias_escrituras/:id_expediente" element={<CorregirReparoCopiasEscriturasPage />} />
            <Route path="cierre_copias_notaria/:id_expediente" element={<CierreCopiasNotariaPage />} />
            <Route path="verificar_correccion_escritura/:id_expediente" element={<VerificarCorreccionEscrituraPage />} />
            <Route path="corregir_reparo_cierre_copias_notaria/:id_expediente" element={<CorregirReparoCierreCopiasNotariaPage />} />
            <Route path="verificar_reparo_cbr/:id_expediente" element={<VerificarReparoCbrPage />} />
            <Route path="recibir_instruccion_pago/:id_expediente" element={<RecibirInstruccionPagoPage />} />
            <Route path="entrega_carpeta/:id_expediente" element={<EntregarCarpetaPage />} />
            <Route path="corregir_reparo_inst_pago/:id_expediente" element={<CorregirReparoInstPagoPage />} />
            <Route path="corregir_reparos_gestor/:id_expediente" element={<CorregirReparosGestorPage />} />
            <Route path="corregir_reparo_entregar_carpeta/:id_expediente" element={<CorregirReparoEntregarCarpetaPage />} />
            <Route path="revisar_inscripcion_cbr/:id_expediente" element={<CorregirRevisarInscripcionCbrPage />} />
            <Route path="reingresar_escritura_cbr/:id_expediente" element={<ReingresarEscrituraCbrPage />} />
            <Route path="corregir_reparo_liquidacion/:id_expediente" element={<CorregirReparoLiquidacionPage />} />
            <Route path="revisar_liquidacion/:id_expediente" element={<RevisarLiquidacionPage />} />
            <Route path="revisar_desembolso/:id_expediente" element={<RevisarDesembolsoPage />} />
            <Route path="control_escritura/:id_expediente" element={<ControlEscrituraPage />} />
            <Route path="corregir_control_escritura/:id_expediente" element={<CorregirControlEscrituraPage />} />
            <Route path="generar_finiquito/:id_expediente" element={<GenerarFiniquitoPage />} />
            <Route path="registrar_firma_apoderado_banco/:id_expediente" element={<RegistrarFirmaApoderadoBancoPage />} />
            <Route path="reparo_formulario/:id_expediente" element={<ReparoFormularioPage />} />
            <Route path="rectificatoria_firma/:id_expediente" element={<RectificatoriaFirmaPage />} />
            <Route path="validacion_rectificatoria_legal/:id_expediente" element={<ValidacionRectificatoriaLegalPage />} />
            <Route path="gestion_rectificatoria/:id_expediente" element={<GestionRectificatoriaPage />} />
            <Route path="gestion_rectificatoria_solucion_reparo/:id_expediente" element={<GestionRectificatoriaSolucionReparoPage />} />
            <Route path="gestion_reparo/:id_expediente" element={<GestionReparoPage />} />
            <Route path="valorizar_cbr/:id_expediente" element={<ValorizarCbrPage />} />
            <Route path="registrar_fecha_registro_cbr/:id_expediente" element={<RegistrarFechaRegistroCbrPage />} />
            <Route path="rectificatoria_analisis_derivacion_reparo_postventa/:id_expediente" element={<RectificatoriaAnalisisDerivacionReparoPostventaPage />} />
            <Route path="rectificatoria_postventa_solucion_reparo/:id_expediente" element={<RectificatoriaPostventaSolucionReparoPage />} />
            <Route path="validacion_rectificatoria_legal_postventa/:id_expediente" element={<ValidacionRectificatoriaLegalPostventaPage />} />
            <Route path="gestion_rectificatoria_escritura_firmada/:id_expediente" element={<GestionRectificatoriaEscrituraFirmadaPage />}/> 
            <Route path="rectificatoria_legal_carta_resguardo/:id_expediente" element={<RectificatoriaLegalCartaResguardoPage />}/>
            <Route path="rectificatoria_legal_firma_alzante/:id_expediente" element={<RectificatoriaLegalFirmaAlzantePage />}/>
            <Route path="rectificatoria_legal_cierre_copias/:id_expediente" element={<RectificatoriaLegalCierreCopiasPage />}/>
            <Route path="rectificatoria_legal_cierre_copias_postventa/:id_expediente" element={<RectificatoriaLegalCierreCopiasPostventaPage />}/>
            <Route path="gestion_rectificatoria_escritura_firmada_postventa/:id_expediente" element={<GestionRectificatoriaEscrituraFirmadaPostventaPage />}/>
            <Route path="rectificatoria_firma_post_venta/:id_expediente" element={<RectificatoriaFirmaPostVentaPage />}/>
            <Route path="revisar_copias_escrituras/:id_expediente" element={<RevisarCopiasEscriturasPage />}/>
            <Route path="cargar_documentos_constructora/:id_expediente" element={<CargarDocumentosConstructoraPage />}/>
            <Route path="revisar_documentos_inmueble/:id_expediente" element={<RevisarDocumentosInmueblePage />}/>
            <Route path="validar_integracion_documentos/:id_expediente" element={<ValidarIntegracionDocumentosPage />} />
            <Route path="devolucion_vb_comercial/:id_expediente" element={<DevolucionVbComercialPage />} />

            <Route path="definir_inmueble/:id_expediente" element={<DefinirInmueblePage />} />
            <Route path="cargar_soportes_pago/:id_expediente" element={<CargarSoportesPagoPage />} />
            <Route path="gestionar_firma/:id_expediente" element={<GestionarFirmaPage />} />
            <Route path="gestionar_firmas_fisica/:id_expediente" element={<GestionarFirmaFisicaPage />} />
            <Route path="firmar_escritura_cliente/:id_expediente" element={<FirmarEscrituraClientePage />} />
            <Route path="firmar_rep_legal/:id_expediente" element={<FirmarRepLegalPage />} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}
