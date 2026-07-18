export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesGestionRectificatoriaEscrituraFirmadaPostventa {
  tipo_reparo: CatalogoOption[];
}



export const EMPTY_CONTROLES_GESTION_RECTIFICATORIA_ESCRITURA_FIRMADA: ControlesGestionRectificatoriaEscrituraFirmadaPostventa = {
  tipo_reparo: [],
};

