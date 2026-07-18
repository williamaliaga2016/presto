import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RealizarRevisionPrevioFirmaBanco } from '../models/realizar_revision_previo_firma_banco';
import { realizarRevisionPrevioFirmaBancoService } from '../api/realizarRevisionPrevioFirmaBancoService';

export function useRealizarRevisionPrevioFirmaBanco(id_expediente: number) {
  return useQuery<ApiResponse<RealizarRevisionPrevioFirmaBanco | null>>({
    queryKey: ['realizar_revision_previo_firma_banco', id_expediente],
    queryFn: () => realizarRevisionPrevioFirmaBancoService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
