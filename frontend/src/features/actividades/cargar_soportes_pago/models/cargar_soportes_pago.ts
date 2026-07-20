/**
 * Contrato frontend/backend del registro de confirmacion documental de Cargar Soportes de Pago.
 */
export interface CargarSoportesPago {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  documentos_adjuntos: boolean;
  observaciones?: string | null;
  is_active?: boolean;
  row_status?: boolean;
}

/**
 * Informacion general que la pantalla externa de Cargar Soportes de Pago puede mostrar.
 */
export interface CargarSoportesPagoInfo {
  /** Identificador del expediente presentado como folio Presto. */
  id_expediente: number;
  /** Nombre del cliente obtenido desde la informacion validada. */
  nombre_completo_t1?: string | null;
  nombre_cliente?: string | null;
  /** Usuario responsable del workflow presentado como analista de vivienda. */
  nombre_analista?: string | null;
}

/**
 * Crea el estado inicial del formulario de confirmacion documental para un expediente.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Modelo inicial de Cargar Soportes de Pago con la actividad fija `ACT_SOPORTES_PAGO`.
 */
export const EMPTY_CARGAR_SOPORTES_PAGO = (
  id_expediente: number,
): CargarSoportesPago => ({
  id_expediente,
  id_actividad: 'BBVA_CONTACTO_CARGAR_SOPORTES_DE_PAGO_899F408B',
  documentos_adjuntos: false,
  observaciones: '',
  is_active: true,
  row_status: true,
});
