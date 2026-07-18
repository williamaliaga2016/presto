export interface RevisarIngresoDatosCredito {
  id_revisar_ingreso_datos_credito: number;
  id_datos_operacion: number;
  id_expediente: number;

  ubicacion?: string | null;
  tipo_operacion?: string | null;
  fines_generales?: boolean | null;
  nombre_proyecto?: string | null;
  credito_segunda_vivienda?: boolean | null;
  inmobiliaria?: string | null;
  vivienda_social?: boolean | null;
  dfl2?: boolean | null;
  propietario_dfl2?: boolean | null;
  recepcion_final_mayor_2_anios?: boolean | null;
  porcentaje_impuesto?: number | null;
  monto_credito_afecto_impuesto?: number | null;
  impuesto_a_pagar?: number | null;
  enviar_a_reparo?: boolean | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}