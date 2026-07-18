export interface ValidarIntegracionDocumentosData {
  id: number;
  idExpediente: number;
  idActividad: string;
  documentosCorrectos: boolean | null;
  creditoCondicionado: boolean;
  motivoDevolucion?: string | null;
  observaciones?: string | null;
}

export interface DatosCreditoFormData {
  tipo_credito?: string | null;
  tiene_garantia?: boolean | null;
}

export interface ValidarIntegracionDocumentosForm {
  actividad: ValidarIntegracionDocumentosData;
  datos_credito: DatosCreditoFormData;
}
