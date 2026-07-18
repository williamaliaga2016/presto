import type { CatalogoOption } from
  '@/features/actividades/carga_operacion_banco/models/catalogo';

export interface ControlesValidarInformacion {
  tipo_documento_id: CatalogoOption[];
  estatus_general: CatalogoOption[];
  motivo_devolucion: CatalogoOption[];
  tipo_inmueble: CatalogoOption[];
  departamento: CatalogoOption[];
  municipio: CatalogoOption[];
  situacion_laboral: CatalogoOption[];
  canal_contacto: CatalogoOption[];
  resultado_contacto: CatalogoOption[];
  tipo_credito: CatalogoOption[];
}

export const EMPTY_CONTROLES_VALIDAR_INFORMACION: ControlesValidarInformacion = {
  tipo_documento_id:  [],
  estatus_general:    [],
  motivo_devolucion:  [],
  tipo_inmueble:      [],
  departamento:       [],
  municipio:          [],
  situacion_laboral:  [],
  canal_contacto:     [],
  resultado_contacto: [],
  tipo_credito:       [],
};
