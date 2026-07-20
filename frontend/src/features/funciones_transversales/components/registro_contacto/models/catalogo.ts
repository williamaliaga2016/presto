import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export interface ControlesRegistroContacto {
  canal_contacto: ControlBaseDTO[];
  resultado_contacto: ControlBaseDTO[];
  detalle_contacto: ControlBaseDTO[];
}

export const EMPTY_CONTROLES_REGISTRO_CONTACTO: ControlesRegistroContacto = {
  canal_contacto: [],
  resultado_contacto: [],
  detalle_contacto: [],
};
