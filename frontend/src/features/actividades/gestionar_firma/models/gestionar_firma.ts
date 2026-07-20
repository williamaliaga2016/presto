export interface GestionarFirma {
  id: number;
  id_expediente: number;
  id_actividad: string;
  requiere_firma_electronica: boolean;
  firma_electronica_realizada: boolean;
  nombre_cliente_firma?: string | null;
  nombre_solicitante_firma?: string | null;
  franja_horaria?: string | null;
  direccion_firma?: string | null;
  descripcion_tramite?: string | null;
  fecha_programacion?: string | null;
  ciudad_cliente?: string | null;
  tipo_credito_firma?: string | null;
  observaciones?: string | null;
}
