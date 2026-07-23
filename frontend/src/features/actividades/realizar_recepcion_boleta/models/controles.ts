import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';

export interface ControlesRecepcionBoleta {
  tipo_boleta: ControlBaseDTO[];
  oficina_registro: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_RECEPCION_BOLETA: ControlesRecepcionBoleta = {
  tipo_boleta: [],
  oficina_registro: [],
};
