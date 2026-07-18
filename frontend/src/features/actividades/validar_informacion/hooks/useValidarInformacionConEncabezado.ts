import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ValidarInformacionConEncabezadoDTO } from
  '../models/encabezado_validar_informacion';
import { validarInformacionService } from '../api/validarInformacionService';

export function useValidarInformacionConEncabezado(id_expediente: number) {
  return useQuery<ApiResponse<ValidarInformacionConEncabezadoDTO>>({
    queryKey: ['validar-informacion-con-encabezado', id_expediente],
    queryFn: () => validarInformacionService.getConEncabezado(id_expediente),
    enabled: id_expediente > 0,
  });
}
