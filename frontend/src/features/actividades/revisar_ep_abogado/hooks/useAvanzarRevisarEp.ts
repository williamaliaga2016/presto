import { useMutation, useQueryClient } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';
import { revisarEpAbogadoService } from '../api/revisarEpAbogadoService';

function validateAvanzar(data: RevisarEpAbogado): void {
  const camposFaltantes: string[] = [];

  if (!data.representante_legal) {
    camposFaltantes.push('representante_legal');
  }
  if (!data.ep_conforme) {
    camposFaltantes.push('ep_conforme');
  }

  if (data.ep_conforme === 'NO') {
    if (!data.tipologia) {
      camposFaltantes.push('tipologia');
    }
    if (!data.casuistica) {
      camposFaltantes.push('casuistica');
    }
    if (!data.observaciones_legales) {
      camposFaltantes.push('observaciones_legales');
    }
  }

  if (camposFaltantes.length > 0) {
    throw new Error(
      `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`,
    );
  }
}

export function useAvanzarRevisarEp() {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<boolean>, unknown, RevisarEpAbogado>({
    mutationFn: (data) => {
      validateAvanzar(data);
      return revisarEpAbogadoService.avanzar(data.id_expediente);
    },
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: ['revisar_ep_abogado', variables.id_expediente],
      });
    },
  });
}
