export interface RegistrarFirmaApoderadoBanco {
  id_registrar_firma_apoderado_banco: number;
  fecha_firma: string;  // DateTime en C# se convierte a string en TypeScript
  id_expediente: number;
  observaciones?: string | null;
  is_active: boolean;
  row_status: boolean;
  created_by: number;
  created_date: string;
  modified_by?: number | null;
  modified_date?: string | null;
}