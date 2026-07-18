export interface DatosOperacionDatosCredito {
  id_datos_operacion_datos_credito: number;
  id_datos_operacion: number;
  id_expediente: number;

  ubicacion?: boolean | null;
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

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface DatosOperacionComprador {
  id_datos_operacion_comprador: number;
  id_datos_operacion: number;
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

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface DatosOperacionVendedor {
  id_datos_operacion_vendedor: number;
  id_datos_operacion: number;
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

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface DatosOperacionFiadorGarante {
  id_datos_operacion_fiador_garante: number;
  id_datos_operacion: number;
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

export interface DatosOperacionBancoAcreedor {
  id_datos_operacion_banco_acreedor: number;
  id_datos_operacion: number;
  id_expediente: number;

  cuenta_carta_resguardo?: boolean | null;
  institucion?: string | null;
  rut_banco_acreedor?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface DatosOperacionPropiedad {
  id_datos_operacion_propiedad: number;
  id_datos_operacion: number;
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

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface DatosOperacion {
  id_datos_operacion: number;
  id_expediente: number;
  enviar_reparo?: boolean | null;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;

  datos_credito?: DatosOperacionDatosCredito | null;
  compradores?: DatosOperacionComprador[];
  vendedores?: DatosOperacionVendedor[];
  fiadores_garantes?: DatosOperacionFiadorGarante[];
  banco_acreedor?: DatosOperacionBancoAcreedor | null;
  propiedad?: DatosOperacionPropiedad | null;

}

export type PersonaTab = 'comprador' | 'vendedor';

export const now = () => new Date().toISOString();

export const buildDatosOperacionEmpty = (
  id_expediente: number,
): DatosOperacion => ({
  id_datos_operacion: 0,
  id_expediente,
  enviar_reparo: false,
  observaciones: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
  datos_credito: buildDatosCreditoEmpty(id_expediente),
  compradores: [],
  vendedores: [],
  fiadores_garantes: [],
  banco_acreedor: buildBancoAcreedorEmpty(id_expediente),
  propiedad: buildPropiedadEmpty(id_expediente),
});

export const buildDatosCreditoEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionDatosCredito => ({
  id_datos_operacion_datos_credito: 0,
  id_datos_operacion,
  id_expediente,
  ubicacion: '',
  tipo_operacion: null,
  fines_generales: null,
  nombre_proyecto: '',
  credito_segunda_vivienda: false,
  inmobiliaria: null,
  vivienda_social: null,
  dfl2: null,
  propietario_dfl2: null,
  recepcion_final_mayor_2_anios: null,
  porcentaje_impuesto: null,
  monto_credito_afecto_impuesto: null,
  impuesto_a_pagar: null,
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildCompradorEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionComprador => ({
  id_datos_operacion_comprador: 0,
  id_datos_operacion,
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
  region_env_esc: null,
  comuna: null,
  comuna_env_esc: null,
  direccion_env_div: '',
  region_env_div: null,
  comuna_env_div: null,
  tipo_dir_dividendo: null,
  telefono: '',
  telefono_comercial: '',
  telefono_movil: '',
  email: '',
  email2: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildVendedorEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionVendedor => ({
  id_datos_operacion_vendedor: 0,
  id_datos_operacion,
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
  region_env_esc: null,
  comuna: null,
  comuna_env_esc: null,
  direccion_env_div: '',
  region_env_div: null,
  comuna_env_div: null,
  tipo_dir_dividendo: null,
  telefono: '',
  telefono_comercial: '',
  telefono_movil: '',
  email: '',
  email2: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildFiadorGaranteEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionFiadorGarante => ({
  id_datos_operacion_fiador_garante: 0,
  id_datos_operacion,
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

export const buildBancoAcreedorEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionBancoAcreedor => ({
  id_datos_operacion_banco_acreedor: 0,
  id_datos_operacion,
  id_expediente,
  cuenta_carta_resguardo: null,
  institucion: null,
  rut_banco_acreedor: '',
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});

export const buildPropiedadEmpty = (
  id_expediente: number,
  id_datos_operacion = 0,
): DatosOperacionPropiedad => ({
  id_datos_operacion_propiedad: 0,
  id_datos_operacion,
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
  is_active: true,
  row_status: true,
  created_by: 0,
  created_date: now(),
  modified_by: null,
  modified_date: null,
});
