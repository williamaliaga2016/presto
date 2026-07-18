export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesRevisarCopiasEscrituras {
  comuna: CatalogoOption[];
}
export const EMPTY_CONTROLES_REVISAR_COPIAS_ESCRITURAS: ControlesRevisarCopiasEscrituras = {
  comuna: [],
};
