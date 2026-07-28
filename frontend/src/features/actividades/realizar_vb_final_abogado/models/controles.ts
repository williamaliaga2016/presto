import type { ControlBaseDTO } from '@/core/api/models/ControlBaseDTO';

export interface ControlesVBFinalAbogado {
  tipologia: ControlBaseDTO[];
  casuistica: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_VB_FINAL: ControlesVBFinalAbogado = {
  tipologia: [],
  casuistica: [],
};
