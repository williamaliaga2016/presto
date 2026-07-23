import { Auditoria } from '@/models/Auditoria';
import { CatalogoOption } from '@/models/CatalogoOption';

export interface RevisarEpAbogado extends Auditoria{
  id: number;
  id_expediente: number;
  id_actividad: string;

  // Datos heredados (solo lectura en UI)
  notaria: string | null;
  fecha_notaria: string | null;
  numero_notaria: number | null;
  ciudad_notaria: string | null;
  numero_escritura: string | null;
  fecha_escritura: string | null;

  // Campo editable
  representante_legal: string | null;

  // Compuerta de Conformidad
  ep_conforme: 'SI' | 'NO' | null;

  // Campos condicionales (cuando ep_conforme = "NO")
  tipologia: string | null;
  casuistica: string | null;
  observaciones_legales: string | null;
}

export interface ControlesRevisarEp {
  representantes_legales?: CatalogoOption[];
  tipologias?: CatalogoOption[];
  casuisticas?: CatalogoOption[];
  [key: string]: CatalogoOption[] | undefined;
}

export const EMPTY_CONTROLES_REVISAR_EP: ControlesRevisarEp = {
  representantes_legales: [],
  tipologias: [],
  casuisticas: [],
};
