import { useQuery } from '@tanstack/react-query';
import type { ApiResponse } from '@/core/api/models/ApiResponse';
import type { RevisarDocumentosInmueble } from '../models/revisar_documentos_inmueble';
import { revisarDocumentosInmuebleService } from '../api/revisarDocumentosInmuebleService';

export const revisarDocumentosInmuebleQueryKeys = {
  actividad: (id_expediente: number) =>
    ['revisar-documentos-inmueble', id_expediente] as const,
  controles: (id_expediente: number) =>
    ['revisar-documentos-inmueble-controles', id_expediente] as const,
};

export function useRevisarDocumentosInmueble(id_expediente: number) {
  return useQuery<ApiResponse<RevisarDocumentosInmueble | null>>({
    queryKey: revisarDocumentosInmuebleQueryKeys.actividad(id_expediente),
    queryFn: () =>
      revisarDocumentosInmuebleService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
