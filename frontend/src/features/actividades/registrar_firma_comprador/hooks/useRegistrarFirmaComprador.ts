import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmaComprador } from '../models/registrar_firma_comprador';
import type { FirmaCompradorDetalle } from '../models/registrar_firma_comprador_detalle';
import { registrarFirmaCompradorService } from '../api/registrarFirmaCompradorService';

export interface FirmaCompradorRequest extends FirmaComprador {
  firma_comprador_detalle: FirmaCompradorDetalle[];
}

export function useRegistrarFirmaComprador(id_expediente: number) {
  return useQuery<ApiResponse<FirmaCompradorRequest | null>>({
    queryKey: ['firma_comprador', id_expediente],
    queryFn: () =>
      registrarFirmaCompradorService.getByIdExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}