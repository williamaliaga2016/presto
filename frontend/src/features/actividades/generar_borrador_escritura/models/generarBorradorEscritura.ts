// src/features/generar_borrador_escritura/models/generarBorradorEscritura.ts

export interface GenerarBorradorEscrituraDetalle {
  id_generar_borrador_escritura_detalle_entity: number;
  id_generar_borrador_escritura: number;
  id_datos_operacion_fiador_garante: number;
  id_expediente: number;
  id_rol_comparecencia: number;
  requiere_firma: boolean;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}

export interface GenerarBorradorEscritura {
  id_generar_borrador_escritura: number;
  id_expediente: number;

  existe_alzamiento: boolean;
  seguro_cesantia: boolean;
  mandato_judicial: boolean;
  beneficios?: string | null;
  id_notaria?: number | null;
  enviar_reparo: boolean;
  observaciones?: string | null;

  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;

  detalle?: GenerarBorradorEscrituraDetalle[] | null;
}

// Fiador/Garante (viene de actividad anterior)
export interface FiadorGarante {
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

// Tipo extendido para la tabla con campos de selección
export interface FiadorGaranteWithSelection extends FiadorGarante {
  id_rol_comparecencia: number;
  requiere_firma: boolean;
  id_generar_borrador_escritura_detalle_entity?: number;
}

// Opciones de roles
export const rolesOptions = [
  { label: "Titular", value: 1 },
  { label: "Aval", value: 2 },
  { label: "Codeudor", value: 3 },
];