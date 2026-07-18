export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesGestionRectificatoria {
  tipo_reparo: CatalogoOption[];
}



export const EMPTY_CONTROLES_GESTION_RECTIFICATORIA: ControlesGestionRectificatoria = {
  tipo_reparo: [],
};

