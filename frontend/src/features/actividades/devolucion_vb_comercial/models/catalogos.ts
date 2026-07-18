export interface CatalogoOption {
    code?: string | null;
    description?: string | null;
}

// Grupo de catalogos
export interface DevolucionVbComercialCatalogos {
    motivo_cierre: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDAR_INTEGRACION: DevolucionVbComercialCatalogos = {
    motivo_cierre: []
};

export interface DevolucionVbComercialFormControles {
  motivo_cierre: CatalogoOption[];
}

export const EMPTY_CONTROLES_DEVOLUCION_VB_COMERCIAL : DevolucionVbComercialFormControles = {
  motivo_cierre: []
};