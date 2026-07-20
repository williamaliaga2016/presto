import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionBBVA } from '../models/validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useValidarInformacion(id_expediente: number) {
  return useQuery<ApiResponse<ValidarInformacionBBVA | null>>({
    queryKey: ['validar-informacion', id_expediente],
    queryFn: () => validarInformacionService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
