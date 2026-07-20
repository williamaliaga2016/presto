import type { ValidarInformacionBBVA } from '../../validar_informacion/models/validar_informacion';
import type { EncabezadoValidarInformacion } from '../../validar_informacion/models/encabezado_validar_informacion';
import type { ControlesValidarInformacion } from '../../validar_informacion/models/catalogo';

export interface ResultadoAsignacion {
  nombre_firma_supervisor?: string | null;
  telefono_firma?: string | null;
  celular_firma?: string | null;
  email_firma?: string | null;
  valor_avaluo?: number | null;
  valor_viatico?: number | null;
  valor_total_consignar?: number | null;
  opciones_recaudo?: string | null;
  numero_recaudo?: string | null;
  banco?: string | null;
  nombre_abogado?: string | null;
  telefono_abogado?: string | null;
  email_abogado?: string | null;
  valor_estudio_titulos?: number | null;
  tipo_cuenta_abogado?: string | null;
  numero_cuenta_abogado?: string | null;
  asignacion_por_proyecto_parametrizado?: boolean | null;
  reutilizado_por_duplicado?: boolean | null;
  advertencias?: string[] | null;
}

export interface AsignarFirmasForm extends ResultadoAsignacion {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  tipo_cliente?: string | null;
  codigo_ejecutivo_solicitante?: string | null;
  oficina_solicitante?: string | null;
  tipo_solicitud_avaluo?: string | null;
  tipo_tramite_eett?: string | null;
  direccion_predio?: string | null;
  ubicacion_predio?: string | null;
  valor_comercial_predio?: number | null;
  homologacion?: boolean | null;
  requiere_actualizacion_avaluo?: boolean | null;
  firma_avaluo_anterior?: string | null;
  asignar_abogado?: boolean | null;
  requiere_envio_notificacion?: boolean | null;
  checklist_documentos_solicitar: string[];
  observaciones?: string | null;
}

export interface DatosFolioAsignacion {
  fecha_asignacion?: string | null;
  tipo_inmueble?: string | null;
  constructora?: string | null;
  proyecto?: string | null;
  identificacion_cliente?: string | null;
  nombre_cliente?: string | null;
  departamento_predio?: string | null;
  ciudad_predio?: string | null;
  direccion_predio?: string | null;
  ubicacion_predio?: string | null;
  valor_comercial_predio?: number | null;
  usuario_solicitante?: string | null;
}

export interface DocumentoSolicitar {
  id: string;
  nombre: string;
}

export interface AsignarFirmasControles extends ControlesValidarInformacion {
  documentos_solicitar: DocumentoSolicitar[];
}

export interface AsignarFirmasConEncabezado {
  formulario: AsignarFirmasForm | null;
  datos_heredados: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  datos_folio: DatosFolioAsignacion;
}

export interface AccesoTemporalGenerado {
  url: string;
  token: string;
  fecha_expiracion: string;
}

export interface AsignarFirmasAvanzarResponse {
  workflow?: unknown;
  acceso_temporal?: AccesoTemporalGenerado | null;
}

export interface CalcularAsignacionRequest {
  id_expediente: number;
  tipo_cliente: string;
  codigo_ejecutivo: string;
  oficina: string;
  tipo_solicitud_avaluo: string;
  tipo_tramite_eett: string;

  numero_identificacion_cliente: string;
  direccion_predio?: string | null;

  tipo_vivienda?: string | null;
  constructora?: string | null;
  codigo_proyecto?: string | null;
  departamento_predio: string;
  ciudad_predio: string;
  ubicacion_predio?: string | null;
  valor_comercial_predio: number;

  homologacion?: boolean | null;
  requiere_actualizacion_avaluo?: boolean | null;
  firma_avaluo_anterior?: string | null;

  asignar_abogado?: boolean | null;
}

export const EMPTY_ASIGNAR_FIRMAS = (idExpediente: number): AsignarFirmasForm => ({
  id_expediente: idExpediente,
  id_actividad: 'ACT_ASIGNAR_FIRMAS',
  checklist_documentos_solicitar: [],
  requiere_envio_notificacion: null,
  homologacion: false,
  requiere_actualizacion_avaluo: false,
  asignar_abogado: true,
});

export const RESULTADO_FIELDS: (keyof ResultadoAsignacion)[] = [
  'nombre_firma_supervisor', 'telefono_firma', 'celular_firma', 'email_firma',
  'valor_avaluo', 'valor_viatico', 'valor_total_consignar', 'opciones_recaudo',
  'numero_recaudo', 'banco',
  'nombre_abogado', 'telefono_abogado', 'email_abogado', 'valor_estudio_titulos',
  'tipo_cuenta_abogado', 'numero_cuenta_abogado',
  'asignacion_por_proyecto_parametrizado', 'reutilizado_por_duplicado', 'advertencias',
];

/**
 * Campos cuyo cambio invalida el resultado ya calculado, obligando a
 * recalcular (el motor no ofrece endpoint de "recalcular" distinto).
 * `firma_avaluo_anterior` queda fuera a propósito: el MD la marca como
 * informativa, no altera el cálculo.
 */
export const RECALC_TRIGGER_FIELDS: (keyof AsignarFirmasForm)[] = [
  'tipo_cliente', 'codigo_ejecutivo_solicitante', 'oficina_solicitante',
  'tipo_solicitud_avaluo', 'tipo_tramite_eett',
  'direccion_predio', 'ubicacion_predio', 'valor_comercial_predio',
  'homologacion', 'requiere_actualizacion_avaluo', 'asignar_abogado',
];
