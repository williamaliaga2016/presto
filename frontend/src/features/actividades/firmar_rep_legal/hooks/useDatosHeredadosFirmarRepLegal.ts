import { useQuery } from '@tanstack/react-query';
import { firmarEscrituraClienteService } from '@/features/actividades/firmar_escritura_cliente/api/firmarEscrituraClienteService';

export interface DatosHeredadosFirmarRepLegal {
  notaria: string | null;
  numero_notaria: number | null;
  ciudad_notaria: string | null;
  fecha_notaria: string | null;
  numero_escritura: string | null;
  fecha_escritura: string | null;
}

export function useDatosHeredadosFirmarRepLegal(id_expediente: number) {
  return useQuery<DatosHeredadosFirmarRepLegal | null>({
    queryKey: ['datos_heredados_firmar_rep_legal', id_expediente],
    queryFn: async () => {
      const response = await firmarEscrituraClienteService.getByExpediente(id_expediente);
      if (!response.status || !response.detail) return null;

      const d = response.detail;
      return {
        notaria: d.notaria ?? null,
        numero_notaria: d.numero_notaria ?? null,
        ciudad_notaria: d.ciudad_notaria ?? null,
        fecha_notaria: d.fecha_notaria ?? null,
        numero_escritura: d.numero_escritura ?? null,
        fecha_escritura: d.fecha_escritura ?? null,
      };
    },
    enabled: !!id_expediente,
  });
}
