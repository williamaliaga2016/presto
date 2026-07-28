import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export interface ControlesRevisionMarcacionCobertura {
  tipo_documento: ControlBaseDTO[];  // TIPO_DOCUMENTO_ID
  tipo_vivienda: ControlBaseDTO[];   // TIPO_VIVIENDA_BBV107
  estado_proceso: ControlBaseDTO[];  // ESTADO_PROCESO_BBV107
}

export const EMPTY_CONTROLES_REVISION_MARCACION_COBERTURA: ControlesRevisionMarcacionCobertura = {
  tipo_documento: [],
  tipo_vivienda: [],
  estado_proceso: [],
};
