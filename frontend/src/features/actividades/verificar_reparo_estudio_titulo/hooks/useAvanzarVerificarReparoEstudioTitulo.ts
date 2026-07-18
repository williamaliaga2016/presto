import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { verificarReparoEstudioTituloService } from '../api/verificarReparoEstudioTituloService';

export function useAvanzarVerificarReparoEstudioTitulo() {
  return useMutation<ApiResponse<boolean>, unknown, number>({
    mutationFn: (id_expediente) => verificarReparoEstudioTituloService.avanzar(id_expediente),
  });
}
