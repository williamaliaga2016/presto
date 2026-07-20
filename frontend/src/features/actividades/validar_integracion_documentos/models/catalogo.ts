import { ControlesValidarInformacion } from "../../validar_informacion/models/catalogo";

export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

// Grupo de catalogos
export interface ValidarIntegracionCatalogos extends ControlesValidarInformacion {
  motivo_devolucion: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDAR_INTEGRACION: ValidarIntegracionCatalogos = {
  motivo_devolucion: [],
  tipo_documento_id: [],
  estatus_general: [],
  tipo_inmueble: [],
  departamento: [],
  municipio: [],
  situacion_laboral: [],
  canal_contacto: [],
  resultado_contacto: [],
  tipo_credito: []
};

// Controles/catalogos agrupados por componente hijo
export interface ValidarIntegracionFormControles {
  motivo_devolucion: CatalogoOption[];
}