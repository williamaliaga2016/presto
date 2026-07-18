import type {
  AsignarFirmasForm,
  ResultadoAsignacion,
} from './asignar_firmas.model';

export const EMPTY_ASIGNAR_FIRMAS = (
  id_expediente: number,
): AsignarFirmasForm => ({
  id_expediente,
  id_actividad: 'ACT_ASIGNAR_FIRMAS',
  checklist_documentos_solicitar: [],
  requiere_envio_notificacion: null,
});

export const RESULTADO_FIELDS: (keyof ResultadoAsignacion)[] = [
  'nombre_firma_supervisor',
  'telefono_firma',
  'email_firma',
  'valor_avaluo',
  'valor_total_consignar',
  'opciones_recaudo',
  'numero_recaudo',
  'banco',
  'nombre_abogado',
  'telefono_abogado',
  'valor_estudio_titulos',
  'tipo_cuenta_abogado',
  'numero_cuenta_abogado',
];
