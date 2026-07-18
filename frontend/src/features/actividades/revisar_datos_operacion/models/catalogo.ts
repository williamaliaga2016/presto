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

export interface ControlesRevisarDatosOperacionBanco {
  banco_acreedor_institucion: CatalogoOption[];
  si_no: CatalogoOption[];
}

export const EMPTY_CONTROLES_REVISAR_DATOS_OPERACION_BANCO: ControlesRevisarDatosOperacionBanco = {
  banco_acreedor_institucion: [],
  si_no: [],
};

export interface ControlesVendedor {
  si_no: CatalogoOption[];
  tipo_vendedor: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  relacion_titular: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export const EMPTY_CONTROLES_VENDEDOR: ControlesVendedor = {
  si_no: [],
  tipo_vendedor: [],
  genero: [],
  estado_civil: [],
  relacion_titular: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  tipo_direccion_dividendo: [],
};

export interface ControlesCredito {
  tipo_operacion: CatalogoOption[];
  si_no: CatalogoOption[];
}

export const EMPTY_CONTROLES_CREDITO: ControlesCredito = {
  tipo_operacion: [],
  si_no: [],
};

export interface ControlesComprador {
  si_no: CatalogoOption[];
  tipo_comprador: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  relacion_titular: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export const EMPTY_CONTROLES_COMPRADOR: ControlesComprador = {
  si_no: [],
  tipo_comprador: [],
  genero: [],
  estado_civil: [],
  relacion_titular: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  tipo_direccion_dividendo: [],
};

export interface ControlesFiadorGarante {
  relacion_titular: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  si_no: CatalogoOption[];
}

export const EMPTY_CONTROLES_FIADOR_GARANTE: ControlesFiadorGarante = {
  relacion_titular: [],
  genero: [],
  estado_civil: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  si_no: [],
};
