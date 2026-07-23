import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';

export interface ControlesEPRegistradas {
  tipologias_garantias: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_EP_REGISTRADAS: ControlesEPRegistradas = {
  tipologias_garantias: [],
};
