export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesDatosCredito {
  si_no: CatalogoOption[];
  tipo_operacion: CatalogoOption[];
  tipo_direccion_dividendo: CatalogoOption[];
}

export const EMPTY_CONTROLES_DATOS_CREDITO: ControlesDatosCredito = {
  si_no: [],
  tipo_operacion: [],
  tipo_direccion_dividendo: [],
};

