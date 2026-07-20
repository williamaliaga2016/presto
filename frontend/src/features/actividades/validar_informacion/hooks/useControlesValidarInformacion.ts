import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { ControlesValidarInformacion } from '../models/catalogo';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../models/catalogo';
import { validarInformacionService } from '../api/validarInformacionService';

export function useControlesValidarInformacion(id_expediente: number) {
  return useQuery<ApiResponse<ControlesValidarInformacion>>({
    queryKey: ['controles-validar-informacion', id_expediente],
    queryFn: () => validarInformacionService.getControles(id_expediente),
    enabled: id_expediente > 0,
    placeholderData: {
      status: true,
      detail: EMPTY_CONTROLES_VALIDAR_INFORMACION,
      message: '',
    },
  });
}
