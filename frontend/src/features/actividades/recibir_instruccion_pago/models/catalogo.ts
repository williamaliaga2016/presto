export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}


export interface ControlesRecibirInstruccionPago {
  condicion_desembolso: CatalogoOption[];
}
export const EMPTY_CONTROLES_RECIBIR_INSTRUCCION_PAGO: ControlesRecibirInstruccionPago = {
  condicion_desembolso: [],
};
