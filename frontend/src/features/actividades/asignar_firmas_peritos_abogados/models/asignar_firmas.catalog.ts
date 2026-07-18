import type { ControlesValidarInformacion } from '../../validar_informacion/models/catalogo';

export interface DocumentoSolicitar {
  id: string;
  nombre: string;
}

export interface AsignarFirmasControles extends ControlesValidarInformacion {
  documentos_solicitar: DocumentoSolicitar[];
}
