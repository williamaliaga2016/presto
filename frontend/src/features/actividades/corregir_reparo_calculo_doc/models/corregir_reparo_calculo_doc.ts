export interface CorregirReparoCalculoDoc {
  id_corregir_reparo_calculo_doc: number;
  id_expediente: number;
  id_usuario_solicitante: number;
  is_subsanar: boolean;
  observaciones: string | null;

  existe_rol_avaluo: string | null;
  rol_avaluo_editado: string | null;
  valor_avaluo_pesos: number | null;

  /**
   * Campos autocompletados desde 5.13 Cálculo y Generación Documento
   * y datos_operacion_propiedad. Vienen del backend con [NotMapped]
   * y son de sólo lectura en la UI.
   */
  solicitante?: string | null;
  observaciones_reparo?: string | null;
  fecha_ingreso?: string | null;
  tipo_propiedad?: string | null;
  tipo_direccion?: string | null;
  direccion?: string | null;
  region?: string | null;
  comuna?: string | null;
  rol_avaluo?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by: number | null;
  modified_date: string | null;
}
