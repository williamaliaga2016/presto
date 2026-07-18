import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import { datosDelVendedorService } from '../api/datosDelVendedorService';
import type { DatosOperacion } from '@/features/actividades/datos_operacion/models/datos_operacion';

export function useDatosDelVendedor(id_expediente: number) {
  return useQuery<ApiResponse<DatosOperacion | null>>({
    queryKey: ['datos_del_vendedor_5_11_3', id_expediente],
    queryFn: () => datosDelVendedorService.getFullByExpediente(id_expediente),
    enabled: !!id_expediente && id_expediente > 0,
  });
}
