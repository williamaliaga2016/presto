// Tipos del Memorándum de Escrituración (Actividad 5.15).
// Reflejan los DTOs del backend MemoEscrituraDTO / MemoEscrituraDataDTO.

export interface MemoEscrituraRequest {
  id_expediente: number;
  observaciones: string;
  is_enviar_reparo: boolean;
}

export interface EncabezadoMemo {
  nro_solicitud: string;
  nro_mutuo: string;
  mes_calculo: string;
  monto_prestamo_uf: number;
  tasa_porcentaje: number;
  plazo_anios: number;
  costo_total_credito_uf: number;
  cae_porcentaje: number;
  valor_uf_hoy: number;
  fecha_uf_hoy: string;
  valor_uf_calculo: number;
  fecha_uf_calculo: string;
  oficina_origen: string;
  credito_en_uf: boolean;
  fecha_escritura_texto: string;
}

export interface AntecedentePersonalMemo {
  relacion: string;
  rut: string;
  nombre_razon_social: string;
  estado_civil: string;
  regimen_bienes: string;
}

export interface AntecedentesPrestamoMemo {
  tipo_prestamo: string;
  tipo_prestamo_subproducto: string;
  destino_prestamo: string;
  precio_venta_uf: number;
  precio_venta_clp: number;
  valor_tasacion_uf: number;
  valor_tasacion_clp: number;
  valor_asegurable_uf: number;
  valor_asegurable_clp: number;
  fecha_tasacion: string;
  valor_uf_tasacion: number;
  prestamo_maximo_clp: number;
  monto_solicitado_uf: number;
  monto_solicitado_clp: number;
  dividendo_pagar_uf: number;
  dividendo_pagar_clp: number;
  renta_liquida_ajustada_clp: number;
  tipo_comision_tasa: string;
  meses_sabaticos: string;
  periodo_gracia: string;
}

export interface DeudaCompradoresMemo {
  monto_total_pension_uf: number;
  monto_total_pension_clp: number;
  total_cuotas_impagas: number;
  valor_cuota_pension_uf: number;
  valor_cuota_pension_clp: number;
}

export interface AntecedentesCreditoMemo {
  serie_codigo_bursatil_serie: string;
  serie_codigo_bursatil_codigo: string;
  monto_credito_uf: number;
  monto_credito_clp: number;
  monto_cuota_contado_uf: number;
  monto_cuota_contado_clp: number;
  precio_venta_uf: number;
  precio_venta_clp: number;
  plazo_anios: number;
  tasa_preferencial: number;
  tasa_estandar: number;
}

export interface SeguroMemo {
  descripcion: string;
  monto_uf: number;
}

export interface GastosOperacionalesMemo {
  conservador_bienes_raices: number;
  escrituracion: number;
  estudio_titulos: number;
  gastos_notariales: number;
  servicio_inscripcion_cbr: number;
  tasacion: number;
  total_gastos_operacionales: number;
}

export interface RangoDividendoMemo {
  del: number;
  al: number;
  uf: number;
}

export interface DividendosMemo {
  rangos: RangoDividendoMemo[];
}

export interface MedioPagoPACMemo {
  numero_pac: string;
  tipo_medio_pago: string;
}

export interface AntecedentePropiedadMemo {
  tipo: string;
  direccion: string;
  rol_avaluo: string;
  estado: string;
}

export interface OtrosAntecedentesMemo {
  beneficio_tributario: string;
  beneficiario: string;
  destino_fondos: string;
  hipoteca_mandato: string;
  observaciones: string;
}

export interface ResolucionMemo {
  texto_aprobado: string;
}

export interface MemoEscrituraData {
  id_expediente: number;
  encabezado: EncabezadoMemo;
  antecedentes_personales: AntecedentePersonalMemo[];
  antecedentes_prestamo: AntecedentesPrestamoMemo;
  deuda_compradores: DeudaCompradoresMemo;
  antecedentes_credito: AntecedentesCreditoMemo;
  seguros: SeguroMemo[];
  impuesto_al_mutuo: number;
  gastos_operacionales: GastosOperacionalesMemo;
  dividendos: DividendosMemo;
  medio_pago_pac: MedioPagoPACMemo;
  antecedentes_propiedad: AntecedentePropiedadMemo[];
  otros_antecedentes: OtrosAntecedentesMemo;
  resolucion: ResolucionMemo;
}

// Versión indexada en expediente_digital (lo que devuelve Generar / ListarVersiones).
export interface MemoEscrituraVersion {
  id_archivo: number;
  id_expediente: number;
  id_documento: number;
  id_usuario?: number | null;
  nombre_archivo: string;
  nombre_archivo_original: string;
  extension: string;
  version_archivo: number;
  fecha_alta?: string | null;
  comentarios?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}
