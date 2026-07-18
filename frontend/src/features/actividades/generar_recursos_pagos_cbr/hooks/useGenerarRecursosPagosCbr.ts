import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { GenerarRecursosPagosCbr } from '../models/generar_recursos_pagos_cbr';
import { generarRecursosPagosCbrService } from '../api/generarRecursosPagosCbrService';

export function useGenerarRecursosPagosCbr(id_expediente: number) {
  return useQuery<ApiResponse<GenerarRecursosPagosCbr | null>>({
    queryKey: ['generar_recursos_pagos_cbr', id_expediente],
    queryFn: () => generarRecursosPagosCbrService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
