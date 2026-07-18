import type { CargarDocumentosCliente } from './cargar_documentos_cliente';

export interface CargarDocumentosClienteFormDraftState {
  key: string;
  value: Partial<CargarDocumentosCliente>;
}

export interface CargarDocumentosClienteAccordionState {
  standard: number[];
  temporal: number[];
}
