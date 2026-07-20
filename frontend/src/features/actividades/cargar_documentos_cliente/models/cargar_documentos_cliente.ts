/**
 * Contrato frontend/backend del registro de confirmacion documental de Cargar Documentos Cliente.
 */
export interface CargarDocumentosCliente {
  id?: number;
  id_expediente: number;
  id_actividad: string;
  documentos_adjuntos: boolean;
  observaciones?: string | null;
  is_active?: boolean;
  row_status?: boolean;
}

/**
 * Informacion general que la pantalla externa de Cargar Documentos Cliente puede mostrar enmascarada.
 */
export interface CargarDocumentosClienteInfo {
  /** Identificador del expediente presentado como folio Presto. */
  id_expediente: number;
  /** Nombre del cliente obtenido desde la informacion validada. */
  nombre_completo_t1?: string | null;
  nombre_cliente?: string | null;
  correo_declarativo?: string | null;
  telefono_declarativo?: string | null;
  /** Usuario responsable del workflow presentado como analista de vivienda. */
  nombre_analista?: string | null;
}

/**
 * Crea el estado inicial del formulario de confirmacion documental para un expediente.
 *
 * @param id_expediente Identificador del expediente en Presto.
 * @returns Modelo inicial de Cargar Documentos Cliente con la actividad fija `ACT_DOCS_CLIENTE`.
 */
export const EMPTY_CARGAR_DOCUMENTOS_CLIENTE = (
  id_expediente: number,
): CargarDocumentosCliente => ({
  id_expediente,
  id_actividad: 'BBVA_CONTACTO_CARGAR_DOCUMENTOS_CLIENTE_CBF7A738',
  documentos_adjuntos: false,
  observaciones: '',
  is_active: true,
  row_status: true,
});
