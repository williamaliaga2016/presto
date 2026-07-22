import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export interface ControlesFirmarRepLegal {
  concepto_firma: ControlBaseDTO[];   // L41 — CONCEPTO_FIRMA_REP_LEGAL
  tipologia: ControlBaseDTO[];        // L42 — TIPOLOGIA_REP_LEGAL
  casuistica: ControlBaseDTO[];       // L43 — CASUISTICA_REP_LEGAL (filtrar por parent_code)
}

export const EMPTY_CONTROLES_FIRMAR_REP_LEGAL: ControlesFirmarRepLegal = {
  concepto_firma: [],
  tipologia: [],
  casuistica: [],
};
