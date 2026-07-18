export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesVerificarReparoCbr {
  tiporeparo: CatalogoOption[];
}
export const EMPTY_CONTROLES_VERIFICAR_REPARO_CBR: ControlesVerificarReparoCbr = {
  tiporeparo: [],
};
