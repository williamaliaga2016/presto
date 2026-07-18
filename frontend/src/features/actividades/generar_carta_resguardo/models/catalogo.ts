export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesGenerarCartaResguardo {
  tipocarta: CatalogoOption[];
}
export const EMPTY_CONTROLES_GENERAR_cARTA_RESGUARDO: ControlesGenerarCartaResguardo = {
  tipocarta: [],
};
