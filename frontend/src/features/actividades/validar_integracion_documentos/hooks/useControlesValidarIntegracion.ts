import { useQuery } from '@tanstack/react-query';
import { validarIntegracionDocumentosService } from
  '../api/validarIntegracionDocumentosService';

export function useControlesValidarIntegracion() {
  return useQuery({
    queryKey: ['validar-integracion-controles'],
    queryFn: () =>
      validarIntegracionDocumentosService.getControles(),
  });
}
