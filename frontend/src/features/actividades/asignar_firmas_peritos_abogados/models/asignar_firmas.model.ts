export interface ResultadoAsignacion {
  nombre_firma_supervisor?: string | null;
  telefono_firma?: string | null;
  email_firma?: string | null;
  valor_avaluo?: number | null;
  valor_total_consignar?: number | null;
  opciones_recaudo?: string | null;
  numero_recaudo?: string | null;
  banco?: string | null;
  nombre_abogado?: string | null;
  telefono_abogado?: string | null;
  valor_estudio_titulos?: number | null;
  tipo_cuenta_abogado?: string | null;
  numero_cuenta_abogado?: string | null;
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
