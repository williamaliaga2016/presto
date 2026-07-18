import type { DevolucionVbComercialData } from './devolucion_vb_comercial';

export type DevolucionVbComercialApiDetail = DevolucionVbComercialData;

export interface AvanzarDevolucionVbComercialRequest {
  id_expediente: number;
  confirmarCierre: boolean;
}

export interface AvanzarDevolucionVbComercialResponse {
  workflow?: unknown;
  cierre_confirmado?: boolean;
}
