import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { corregirNotariaReparoAbogadosService } from '../api/corregirNotariaReparoAbogadosService';

export function useAvanzarCorregirNotariaReparoAbogados() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => corregirNotariaReparoAbogadosService.avanzar(id_expediente),
  });
}
