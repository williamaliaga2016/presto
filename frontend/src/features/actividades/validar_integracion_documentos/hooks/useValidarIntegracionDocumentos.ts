import { useQuery } from '@tanstack/react-query';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';

export const validarIntegracionQueryKeys = {
  actividad: (id_expediente: number) => ['validar-integracion-documentos', id_expediente] as const,
  controles: (id_expediente: number) => ['validar-integracion-documentos-controles', id_expediente] as const,
};

export function useValidarIntegracionDocumentos(id_expediente: number) {
  return useQuery({
    queryKey: validarIntegracionQueryKeys.actividad(id_expediente),
    queryFn: () => validarIntegracionDocumentosService.getByExpediente(id_expediente),
    enabled: id_expediente > 0,
  });
}
