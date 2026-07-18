import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { expedienteDigitalService } from '../api/expedienteDigitalService';
import type { ControlBaseDTO } from '@/shared/models/ControlBaseDTO';

export function useDocumentosExpedienteDigital(
  id_expediente: number,
  id_expediente_digital: number,
) {
  return useQuery<ApiResponse<ControlBaseDTO[]>>({
    queryKey: [
      'expediente_digital',
      'documentos',
      id_expediente,
      id_expediente_digital,
    ],
    queryFn: () =>
      expedienteDigitalService.getCatalogoDocumentos(
        id_expediente,
        id_expediente_digital,
      ),
    enabled: id_expediente > 0 && id_expediente_digital > 0,
  });
}
