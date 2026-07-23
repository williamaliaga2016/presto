import type { Auditoria } from '@/models/Auditoria';

export interface FirmarRepLegal extends Auditoria {
  id: number;
  id_expediente: number;
  id_actividad: string;
  concepto_firma: string | null;
  tipologia: string | null;
  casuistica: string | null;
  observaciones: string | null;
}

export const EMPTY_FIRMAR_REP_LEGAL = (
  id_expediente: number,
): FirmarRepLegal => ({
  id: 0,
  id_expediente,
  id_actividad: 'BBVA_ESCRITURACION_FIRMAR_REP_LEGAL',
  concepto_firma: null,
  tipologia: null,
  casuistica: null,
  observaciones: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});
