export interface Interviniente {
  id?: number;
  id_expediente?: number;
  nombre_completo: string;
  tipo_identificacion: string;
  tipo_identificacion_descripcion?: string;
  numero_identificacion: string;
  telefono?: string;
  correo_electronico?: string;
}