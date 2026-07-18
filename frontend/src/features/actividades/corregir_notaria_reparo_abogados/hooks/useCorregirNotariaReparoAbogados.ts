import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { CorregirNotariaReparoAbogados } from '../models/corregir_notaria_reparo_abogados';
import { corregirNotariaReparoAbogadosService } from '../api/corregirNotariaReparoAbogadosService';

export function useCorregirNotariaReparoAbogados(id_expediente: number) {
  return useQuery<ApiResponse<CorregirNotariaReparoAbogados | null>>({
    queryKey: ['corregir_notaria_reparo_abogados', id_expediente],
    queryFn: () => corregirNotariaReparoAbogadosService.getByExpediente(id_expediente),
    enabled: !!id_expediente,
  });
}
