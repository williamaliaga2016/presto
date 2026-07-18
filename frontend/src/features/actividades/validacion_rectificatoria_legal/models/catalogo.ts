export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesValidacionRectificatoriaLegal {
  rol_comparecencia: CatalogoOption[];
    genero: CatalogoOption[];
    estado_civil: CatalogoOption[];
    relacion_titular: CatalogoOption[];
    region: CatalogoOption[];
    comuna: CatalogoOption[];
    nacionalidad: CatalogoOption[];
    tipo_requerimiento_documentacion: CatalogoOption[];
    realiza_pago: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDACION_RECTIFICATORIA_LEGAL: ControlesValidacionRectificatoriaLegal = {
  rol_comparecencia: [],
    genero: [],
    estado_civil: [],
    relacion_titular: [],
    region: [],
    comuna: [],
    nacionalidad: [],
    tipo_requerimiento_documentacion: [],
    realiza_pago: [],
};
