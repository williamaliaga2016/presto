import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export const validarInformacionQueryKeys = {
  actividad: (id_expediente: number) => ['validar-informacion', id_expediente] as const,
  controles: (id_expediente: number) => ['validar-informacion-controles', id_expediente] as const,
  registroContacto: (id_expediente: number, id_actividad: string) =>
    ['validar-informacion-registro-contacto', id_expediente, id_actividad] as const,
};

export function useValidarInformacion(id_expediente: number) {
  return useQuery<ApiResponse<ValidarInformacionBBVA | null>>({
    queryKey: validarInformacionQueryKeys.actividad(id_expediente),
    queryFn: () => validarInformacionService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
