import { useMutation } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { Interviniente } from '../models/interviniente';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';


interface SaveIntervinienteRequest {
  id_expediente: number;
  data: Interviniente;
}


export function useGuardarInterviniente() {

  return useMutation<
    ApiResponse<Interviniente>,
    unknown,
    SaveIntervinienteRequest
  >({

    mutationFn: ({
      id_expediente,
      data
    }) =>
      validarIntegracionDocumentosService.saveInterviniente(
        id_expediente,
        data
      ),

  });
}