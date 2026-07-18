export interface GestionRectificatoriaEscrituraFirmada {

  id_gestion_rectificatoria_escritura_firmada?: number | null;
  id_rectificatoria_firma_comprado_vendedor?: number | null;
  id_expediente?: number | null;
  enviar_tipo_reparo?: string | null;
  vb_solicitado_fiscalia?: boolean | null;
  id_usuario_solicitante?: number | null;
  subsanar?: boolean | null;
  observaciones?: string | null;
  

  id_solicitante?: number | null;
  id_solicitud?: number | null;
  solicitante?: string | null;
  observaciones_reparo?: string | null;
  fecha_ingreso?: string | null;
  
  is_active?: boolean | null;
  row_status?: boolean | null;
  created_by?: number | null;
  created_date?: string | null;
  modified_by?: number | null;
  modified_date?: string | null;
}