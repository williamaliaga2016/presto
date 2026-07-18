export interface RevisarDatosOperacionCredito {
  id_revisar_datos_operacion_credito: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;

  santiago?: boolean | null;
  regiones?: boolean | null;
  tipo_operacion?: string | null;
  fines_generales?: boolean | null;
  nombre_proyecto?: string | null;
  credito_segunda_vivienda?: boolean | null;
  inmobiliaria?: string | null;
  vivienda_social?: boolean | null;
  dfl2?: boolean | null;
  propietario_0_1_dfl2?: boolean | null;
  recepcion_final_mayor_2?: boolean | null;
  porcentaje_impuesto?: number | null;
  monto_credito_afecto?: number | null;
  impuesto_a_pagar?: number | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDatosOperacionPropiedad {
  id_revisar_datos_operacion_propiedad: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;

  tipo_propiedad?: string | null;
  estado?: string | null;
  tipo_venta?: string | null;
  tipo_construccion?: string | null;
  tipo_direccion?: string | null;
  direccion?: string | null;
  villa_condominio?: string | null;
  numero?: string | null;
  numero_casa_habitantes?: string | null;
  conjunto?: string | null;
  manzana?: string | null;
  lote?: string | null;
  region?: string | null;
  comuna?: string | null;
  existe_rol_avaluo?: string | null;
  rol_avaluo_1?: string | null;
  rol_avaluo_2?: string | null;
  valor_avaluo_pesos?: number | null;
  enviar_reparo?: string | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDatosOperacionBanco {
  id_revisar_datos_operacion_banco: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;

  cuenta_carta_resguardo?: boolean | null;
  institucion?: string | null;
  rut_banco_acreedor?: string | null;
  enviar_a_reparo?: boolean | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDatosOperacionVendedor {
  id_revisar_datos_operacion_vendedor: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;

  rut?: string | null;
  tipo_persona?: string | null;
  razon_social?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  fecha_nacimiento?: string | null;
  genero?: string | null;
  estado_civil?: string | null;
  nacionalidad?: string | null;
  profesion?: string | null;
  relacion_titular?: string | null;
  direccion?: string | null;
  direccion_env_esc?: string | null;
  region?: string | null;
  region_env_esc?: string | null;
  comuna?: string | null;
  comuna_env_esc?: string | null;
  direccion_env_div?: string | null;
  region_env_div?: string | null;
  comuna_env_div?: string | null;
  tipo_dir_dividendo?: number | null;
  telefono?: string | null;
  telefono_comercial?: string | null;
  telefono_movil?: string | null;
  email?: string | null;
  email2?: string | null;
  enviar_reparo?: string | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDatosOperacionComprador {
  id_revisar_datos_operacion_comprador: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;

  rut?: string | null;
  tipo_persona?: string | null;
  razon_social?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  fecha_nacimiento?: string | null;
  genero?: string | null;
  estado_civil?: string | null;
  nacionalidad?: string | null;
  profesion?: string | null;
  relacion_titular?: string | null;
  direccion?: string | null;
  direccion_env_esc?: string | null;
  region?: string | null;
  region_env_esc?: string | null;
  comuna?: string | null;
  comuna_env_esc?: string | null;
  direccion_env_div?: string | null;
  region_env_div?: string | null;
  comuna_env_div?: string | null;
  tipo_dir_dividendo?: number | null;
  telefono?: string | null;
  telefono_comercial?: string | null;
  telefono_movil?: string | null;
  email?: string | null;
  email2?: string | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface RevisarDatosOperacionFiadorGarante {
  id_revisar_datos_operacion_fiador_garante: number;
  id_revisar_datos_operacion: number;
  id_expediente: number;
  rut?: string | null;
  nombres?: string | null;
  apellido_paterno?: string | null;
  apellido_materno?: string | null;
  fecha_nacimiento?: string | null;
  genero?: string | null;
  estado_civil?: string | null;
  nacionalidad?: string | null;
  profesion?: string | null;
  direccion?: string | null;
  region?: string | null;
  comuna?: string | null;
  telefono_fijo?: string | null;
  telefono_movil?: string | null;
  email?: string | null;
  relacion_titular?: string | null;
  observaciones?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export const buildFiadorGaranteEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionFiadorGarante => ({
  id_revisar_datos_operacion_fiador_garante: 0,
  id_revisar_datos_operacion,
  id_expediente,
  rut: '',
  nombres: '',
  apellido_paterno: '',
  apellido_materno: '',
  fecha_nacimiento: null,
  genero: null,
  estado_civil: null,
  nacionalidad: null,
  profesion: '',
  direccion: '',
  region: null,
  comuna: null,
  telefono_fijo: '',
  telefono_movil: '',
  email: '',
  relacion_titular: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export interface RevisarDatosOperacion {
  id_revisar_datos_operacion: number;
  id_expediente: number;

  enviar_reparo?: boolean | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;

  credito?: RevisarDatosOperacionCredito | null;
  propiedad?: RevisarDatosOperacionPropiedad | null;
  revisar_datos_operacion_banco?: RevisarDatosOperacionBanco | null;
  compradores: RevisarDatosOperacionComprador[];
  vendedores: RevisarDatosOperacionVendedor[];
  fiadores_garantes?: RevisarDatosOperacionFiadorGarante[];
}

export type DatosOperacionPropiedadPrecarga = {
  id_datos_operacion_propiedad?: number | null;
  id_datos_operacion?: number | null;
  id_expediente?: number | null;
  tipo_propiedad?: string | null;
  estado?: string | null;
  tipo_venta?: string | null;
  tipo_construccion?: string | null;
  tipo_direccion?: string | null;
  direccion?: string | null;
  villa_condominio?: string | null;
  numero?: string | null;
  numero_casa_habitantes?: string | null;
  conjunto?: string | null;
  manzana?: string | null;
  lote?: string | null;
  region?: string | null;
  comuna?: string | null;
  existe_rol_avaluo?: string | null;
  rol_avaluo_1?: string | null;
  rol_avaluo_2?: string | null;
  valor_avaluo_pesos?: number | null;
};

export const now = () => new Date().toISOString();

export const buildCreditoEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionCredito => ({
  id_revisar_datos_operacion_credito: 0,
  id_revisar_datos_operacion,
  id_expediente,
  santiago: null,
  regiones: null,
  tipo_operacion: null,
  fines_generales: null,
  nombre_proyecto: null,
  credito_segunda_vivienda: null,
  inmobiliaria: null,
  vivienda_social: null,
  dfl2: null,
  propietario_0_1_dfl2: null,
  recepcion_final_mayor_2: null,
  porcentaje_impuesto: null,
  monto_credito_afecto: null,
  impuesto_a_pagar: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildPropiedadEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionPropiedad => ({
  id_revisar_datos_operacion_propiedad: 0,
  id_revisar_datos_operacion,
  id_expediente,
  tipo_propiedad: null,
  estado: null,
  tipo_venta: null,
  tipo_construccion: null,
  tipo_direccion: null,
  direccion: '',
  villa_condominio: '',
  numero: '',
  numero_casa_habitantes: '',
  conjunto: '',
  manzana: '',
  lote: '',
  region: null,
  comuna: null,
  existe_rol_avaluo: null,
  rol_avaluo_1: '',
  rol_avaluo_2: '',
  valor_avaluo_pesos: null,
  enviar_reparo: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildRevisarDatosOperacionBancoEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionBanco => ({
  id_revisar_datos_operacion_banco: 0,
  id_revisar_datos_operacion,
  id_expediente,
  cuenta_carta_resguardo: null,
  institucion: null,
  rut_banco_acreedor: '',
  enviar_a_reparo: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildVendedorEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionVendedor => ({
  id_revisar_datos_operacion_vendedor: 0,
  id_revisar_datos_operacion,
  id_expediente,
  rut: '',
  tipo_persona: null,
  razon_social: '',
  nombres: '',
  apellido_paterno: '',
  apellido_materno: '',
  fecha_nacimiento: null,
  genero: null,
  estado_civil: null,
  nacionalidad: null,
  profesion: '',
  relacion_titular: null,
  direccion: '',
  direccion_env_esc: '',
  region: null,
  region_env_esc: '',
  comuna: null,
  comuna_env_esc: '',
  direccion_env_div: '',
  region_env_div: '',
  comuna_env_div: '',
  tipo_dir_dividendo: null,
  telefono: '',
  telefono_comercial: '',
  telefono_movil: '',
  email: '',
  email2: '',
  enviar_reparo: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildCompradorEmpty = (
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionComprador => ({
  id_revisar_datos_operacion_comprador: 0,
  id_revisar_datos_operacion,
  id_expediente,
  rut: '',
  tipo_persona: null,
  razon_social: '',
  nombres: '',
  apellido_paterno: '',
  apellido_materno: '',
  fecha_nacimiento: null,
  genero: null,
  estado_civil: null,
  nacionalidad: null,
  profesion: '',
  relacion_titular: null,
  direccion: '',
  direccion_env_esc: '',
  region: null,
  region_env_esc: '',
  comuna: null,
  comuna_env_esc: '',
  direccion_env_div: '',
  region_env_div: '',
  comuna_env_div: '',
  tipo_dir_dividendo: null,
  telefono: '',
  telefono_comercial: '',
  telefono_movil: '',
  email: '',
  email2: '',
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildDatosOperacionEmpty = (
  id_expediente: number,
): RevisarDatosOperacion => ({
  id_revisar_datos_operacion: 0,
  id_expediente,
  enviar_reparo: null,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
  credito: buildCreditoEmpty(id_expediente),
  propiedad: buildPropiedadEmpty(id_expediente),
  revisar_datos_operacion_banco: buildRevisarDatosOperacionBancoEmpty(id_expediente),
  compradores: [],
  vendedores: [],
  fiadores_garantes: [],
});

export const buildPropiedadFromDatosOperacion = (
  source: DatosOperacionPropiedadPrecarga | null | undefined,
  id_expediente: number,
  id_revisar_datos_operacion = 0,
): RevisarDatosOperacionPropiedad => ({
  ...buildPropiedadEmpty(id_expediente, id_revisar_datos_operacion),
  tipo_propiedad: source?.tipo_propiedad ?? null,
  estado: source?.estado ?? null,
  tipo_venta: source?.tipo_venta ?? null,
  tipo_construccion: source?.tipo_construccion ?? null,
  tipo_direccion: source?.tipo_direccion ?? null,
  direccion: source?.direccion ?? '',
  villa_condominio: source?.villa_condominio ?? '',
  numero: source?.numero ?? '',
  numero_casa_habitantes: source?.numero_casa_habitantes ?? '',
  conjunto: source?.conjunto ?? '',
  manzana: source?.manzana ?? '',
  lote: source?.lote ?? '',
  region: source?.region ?? null,
  comuna: source?.comuna ?? null,
  existe_rol_avaluo: source?.existe_rol_avaluo ?? null,
  rol_avaluo_1: source?.rol_avaluo_1 ?? '',
  rol_avaluo_2: source?.rol_avaluo_2 ?? '',
  valor_avaluo_pesos: source?.valor_avaluo_pesos ?? null,
  enviar_reparo: null,
  observaciones: '',
});
