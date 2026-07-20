export interface CatalogoOption {
  code?: string | null;
  description?: string | null;
}

export interface ControlesGestionarFirmaFisica {
  resultado_gestoria?: CatalogoOption[];
  [key: string]: CatalogoOption[] | undefined;
}

export const DEFAULT_RESULTADO_GESTORIA_OPTIONS: CatalogoOption[] = [
  { code: 'EFECTIVO', description: 'Efectivo' },
  { code: 'CANCELADO', description: 'Cancelado' },
  { code: 'REPROGRAMAR', description: 'Reprogramar' },
];

export const EMPTY_CONTROLES_GESTIONAR_FIRMA_FISICA: ControlesGestionarFirmaFisica = {
  resultado_gestoria: DEFAULT_RESULTADO_GESTORIA_OPTIONS,
};
