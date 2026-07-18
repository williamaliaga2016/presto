import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { FirmaVendedor } from '../models/registrar_firma_vendedor';
import type { FirmaVendedorDetalle } from '../models/registrar_firma_vendedor_detalle';
import { registrarFirmaVendedorService } from '../api/registrarFirmaVendedorService';

export interface FirmaVendedorRequest extends FirmaVendedor {
  firma_vendedor_detalle: FirmaVendedorDetalle[];
}

export function useRegistrarFirmaVendedor(id_expediente: number) {
  return useQuery<ApiResponse<FirmaVendedorRequest | null>>({
    queryKey: ['firma_vendedor', id_expediente],
    queryFn: () =>
      registrarFirmaVendedorService.getByIdExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}