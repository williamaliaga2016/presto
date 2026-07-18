export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesDatosCredito {
  si_no: CatalogoOption[];
  tipo_operacion: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export interface ControlesComprador {
  relacion_titular: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  si_no: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export interface ControlesVendedor {
  tipo_vendedor: CatalogoOption[];
  relacion_titular: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  si_no: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export interface ControlesFiadorGarante {
  relacion_titular: CatalogoOption[];
  genero: CatalogoOption[];
  estado_civil: CatalogoOption[];
  region: CatalogoOption[];
  comuna: CatalogoOption[];
  nacionalidad: CatalogoOption[];
  si_no: CatalogoOption[];
}

export interface ControlesBancoAcreedor {
  banco_acreedor_institucion: CatalogoOption[];
  si_no: CatalogoOption[];
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

export const EMPTY_CONTROLES_DATOS_CREDITO: ControlesDatosCredito = {
  si_no: [],
  tipo_operacion: [],
  tipo_direccion_dividendo: [],
};

export const EMPTY_CONTROLES_COMPRADOR: ControlesComprador = {
  relacion_titular: [],
  genero: [],
  estado_civil: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  si_no: [],
  tipo_direccion_dividendo: [],
};

export const EMPTY_CONTROLES_VENDEDOR: ControlesVendedor = {
  tipo_vendedor: [],
  relacion_titular: [],
  genero: [],
  estado_civil: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  si_no: [],
  tipo_direccion_dividendo: [],
};

export const EMPTY_CONTROLES_FIADOR_GARANTE: ControlesFiadorGarante = {
  relacion_titular: [],
  genero: [],
  estado_civil: [],
  region: [],
  comuna: [],
  nacionalidad: [],
  si_no: [],
};

export const EMPTY_CONTROLES_BANCO_ACREEDOR: ControlesBancoAcreedor = {
  banco_acreedor_institucion: [],
  si_no: [],
};

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
