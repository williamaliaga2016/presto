export interface ValidarIntegracionEncabezadoDTO {
  scoring?: string | null;
  id_tipo_sub_producto?: string | null;
  monto_otorgado_original?: number | null;
  plazo_meses?: number | null;
  tasa?: number | null;
  conditions_organismo_decisor?: string | null;
  codigo_oficina?: string | null;
  descripcion_oficina?: string | null;
  codigo_asesor?: string | null;
  correo_declarativo_original?: string | null;
  telefono_declarativo_original?: string | null;
}
