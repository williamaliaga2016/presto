export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesRegistrarEscrituraCbr {
  conservador: CatalogoOption[];
}
export const EMPTY_CONTROLES_REGISTRAR_ESCRITURA_CBR: ControlesRegistrarEscrituraCbr = {
  conservador: [],
};
