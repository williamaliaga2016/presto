import { Auditoria } from '@/models/Auditoria';
import { CatalogoOption } from '@/models/CatalogoOption';

export interface FirmarEscrituraCliente extends Auditoria{
  id: number;
  id_expediente: number;
  id_actividad: string;

  // Información de Notaría
  notaria: string | null;
  fecha_notaria: string | null;
  numero_notaria: number | null;
  ciudad_notaria: string | null;

  // Formalización de Escritura
  numero_escritura: string | null;
  fecha_escritura: string | null;
  representante_legal: string | null;

  // Decisiones de Enrutamiento
  requiere_escalamiento_comercial: 'SI' | 'NO' | null;
  tipologia: string | null;
  requiere_causar: 'SI' | 'NO' | null;

  // Campos adicionales
  observaciones: string | null;
  tipo_credito: string | null;
}

export interface ControlesFirmarEscritura {
  representantes_legales?: CatalogoOption[];
  tipologias?: CatalogoOption[];
  [key: string]: CatalogoOption[] | undefined;
}

export const EMPTY_CONTROLES_FIRMAR_ESCRITURA: ControlesFirmarEscritura = {
  representantes_legales: [],
  tipologias: [],
};
