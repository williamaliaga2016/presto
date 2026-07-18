export interface RectificatoriaFirmaPostVentaDetalle {
  id_rectificatoria_firma_post_venta_detalle: number;
  id_rectificatoria_firma_post_venta: number;
  id_expediente: number;
  relacion_titular?: string;
  rol_comparecencia: string;
  rut?: string | null;
  nombres?: string;
  apellido_paterno?: string;
  apellido_materno?: string;
  fecha_firma?: string | null;
}
