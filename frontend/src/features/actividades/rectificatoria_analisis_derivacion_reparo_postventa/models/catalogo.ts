export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesRectificatoriaAnalisisDerivacionReparoPostventa {
  tiporeparo: CatalogoOption[];
}
export const EMPTY_CONTROLES_RECTIFICATORIA_ANALIS_DERIVACION_REPARO_POSTVENTA: ControlesRectificatoriaAnalisisDerivacionReparoPostventa = {
  tiporeparo: [],
};
