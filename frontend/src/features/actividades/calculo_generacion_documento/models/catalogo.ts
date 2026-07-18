export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesPropiedad {
  tipo_propiedad: CatalogoOption[];
  estado_propiedad: CatalogoOption[];
  tipo_venta: CatalogoOption[];
  tipo_construccion: CatalogoOption[];
  tipo_direccion: CatalogoOption[];
  existe_rol_avaluo: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  si_no: CatalogoOption[];
}

export const EMPTY_CONTROLES_PROPIEDAD: ControlesPropiedad = {
  tipo_propiedad: [],
  estado_propiedad: [],
  tipo_venta: [],
  tipo_construccion: [],
  tipo_direccion: [],
  existe_rol_avaluo: [],
  region: [],
  comuna: [],
  si_no: [],
};
