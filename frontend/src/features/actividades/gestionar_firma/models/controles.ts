export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesGestionarFirma {
  franjas_horarias?: CatalogoOption[];
  ciudades?: CatalogoOption[];
  [key: string]: CatalogoOption[] | undefined;
}

export const EMPTY_CONTROLES_GESTIONAR_FIRMA: ControlesGestionarFirma = {
  franjas_horarias: [],
  ciudades: [],
};
