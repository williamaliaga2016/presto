import { useQuery } from '@tanstack/react-query';
import { validarIntegracionDocumentosService } from '../api/validarIntegracionDocumentosService';
import { validarIntegracionQueryKeys } from './useValidarIntegracionDocumentos';

export function useControlesValidarIntegracion(id_expediente: number) {
  return useQuery({
    queryKey: validarIntegracionQueryKeys.controles(id_expediente),
    queryFn: () => validarIntegracionDocumentosService.getControles(id_expediente),
    enabled: id_expediente > 0,
  });
}
