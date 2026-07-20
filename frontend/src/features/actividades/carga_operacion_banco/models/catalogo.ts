export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
  parent_code?: string | null;
  valor?: string | null;
}

export interface ControlesDatosOperacion {
  canal_originacion: CatalogoOption[];
  tipo_inmueble: CatalogoOption[];
  estado_inmueble: CatalogoOption[];
  codigo_oficina: CatalogoOption[];
}

export interface ControlesAntecedenteComprador {
  tipo_documento_id: CatalogoOption[];
  situacion_laboral: CatalogoOption[];
}

export interface ControlesAntecedenteCredito {
  tipo_subproducto: CatalogoOption[];
}

export const EMPTY_CONTROLES_DATOS_OPERACION: ControlesDatosOperacion = {
  canal_originacion: [],
  tipo_inmueble: [],
  estado_inmueble: [],
  codigo_oficina: [],
};

export const EMPTY_CONTROLES_ANTECEDENTE_COMPRADOR: ControlesAntecedenteComprador = {
  tipo_documento_id: [],
  situacion_laboral: [],
};

export const EMPTY_CONTROLES_ANTECEDENTE_CREDITO: ControlesAntecedenteCredito = {
  tipo_subproducto: [],
};
