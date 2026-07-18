export interface ReporteDTO {
  id_reporte: number;
  nombre: string;
  descripcion?: string | null;
  report_path: string;
  template?: string | null;
  extension?: string | null;
  is_active: boolean;
  row_status: boolean;
}

export interface ReportViewerConfig {
  report_service_url: string;
  report_path: string;
  parameters?: { name: string; values: string[] }[];
}
