import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CalculoGeneracionDocumento } from '../models/calculo_generacion_documento';
import { calculoGeneracionDocumentoService } from '../api/calculoGeneracionDocumentoService';

export function useCalculoGeneracionDocumento(id_expediente: number) {
  return useQuery<ApiResponse<CalculoGeneracionDocumento | null>>({
    queryKey: ['calculo_generacion_documento', id_expediente],
    queryFn: () => calculoGeneracionDocumentoService.getByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
