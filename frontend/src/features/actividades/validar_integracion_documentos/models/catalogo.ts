export interface CatalogoOption {
    code?: string | null;
    description?: string | null;
}

// Grupo de catalogos
export interface ValidarIntegracionCatalogos {
    motivo_devolucion: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDAR_INTEGRACION: ValidarIntegracionCatalogos = {
    motivo_devolucion: []
};

// Controles/catalogos agrupados por componente hijo
export interface ValidarIntegracionFormControles {
    motivo_devolucion: CatalogoOption[];
}

export interface DatosCreditoControles {
    tipo_credito: CatalogoOption[];
}