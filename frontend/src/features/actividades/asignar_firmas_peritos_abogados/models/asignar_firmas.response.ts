import type { ValidarInformacionBBVA } from '../../validar_informacion/models/validar_informacion';
import type { EncabezadoValidarInformacion } from '../../validar_informacion/models/encabezado_validar_informacion';
import type {
  AsignarFirmasForm,
  DatosFolioAsignacion,
} from './asignar_firmas.model';

export interface AsignarFirmasConEncabezado {
  formulario: AsignarFirmasForm | null;
  datos_heredados: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  datos_folio: DatosFolioAsignacion;
}

export interface AccesoTemporalGenerado {
  url: string;
  token: string;
  fecha_expiracion: string;
}

export interface AsignarFirmasAvanzarResponse {
  workflow?: unknown;
  acceso_temporal?: AccesoTemporalGenerado | null;
}
