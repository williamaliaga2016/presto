import { useMutation } from '@tanstack/react-query';
import {
  registrarFirmaVendedorService,
  type FirmaVendedorRequest,
} from '../api/registrarFirmaVendedorService';

export function useUpsertRegistrarFirmaVendedor() {
  return useMutation({
    mutationFn: (payload: FirmaVendedorRequest) =>
      registrarFirmaVendedorService.save(payload),
  });
}