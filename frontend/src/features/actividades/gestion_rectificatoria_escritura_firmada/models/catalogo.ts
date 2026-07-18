export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesGestionRectificatoriaEscrituraFirmada {
  tipo_reparo: CatalogoOption[];
}



export const EMPTY_CONTROLES_GESTION_RECTIFICATORIA_ESCRITURA_FIRMADA: ControlesGestionRectificatoriaEscrituraFirmada = {
  tipo_reparo: [],
};

