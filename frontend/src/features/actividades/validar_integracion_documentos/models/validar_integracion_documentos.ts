import { ValidarInformacionBBVA } from "../../validar_informacion/models/validar_informacion";

export interface ValidarIntegracionDocumentos {
  id_expediente: number;
  encabezado: ValidarIntegracionEncabezado;
  formulario: ValidarIntegracionDocumentosFormulario;
  validar_informacion_data: ValidarInformacionBBVA
}

export interface GuardarValidarIntegracionDocumentos {
  formulario: ValidarIntegracionDocumentosFormulario;
  validar_informacion_data: ValidarInformacionBBVA
}

export interface ValidarIntegracionDocumentosFormulario {
  id: number;
  id_expediente: number;
  id_actividad: string;
  documentos_correctos: boolean | null;
  credito_condicionado: boolean;
  motivo_devolucion: string;
  observaciones: string;
}

export interface ValidarIntegracionEncabezado {
  scoring?: string | null;
  id_tipo_sub_producto?: string | null;
  monto_otorgado_original?: number | null;
  plazo_meses?: number | null;
  tasa?: number | null;
  conditions_organismo_decisor?: string | null;
  codigo_oficina?: string | null;
  descripcion_oficina?: string | null;
  codigo_asesor?: string | null;
  correo_declarativo_original?: string | null;
  telefono_declarativo_original?: string | null;
}

