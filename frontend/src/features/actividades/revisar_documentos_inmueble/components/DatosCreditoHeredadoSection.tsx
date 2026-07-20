import ReadOnlyField from './ReadOnlyField';
import type { DatosCreditoHeredado } from '../models/revisar_documentos_inmueble';

type Props = {
  data: DatosCreditoHeredado;
};

const formatCOP = (value: number | null | undefined) => {
  if (value === null || value === undefined) return null;
  return new Intl.NumberFormat('es-CO', {
    style: 'currency',
    currency: 'COP',
    maximumFractionDigits: 0,
  }).format(value);
};

export default function DatosCreditoHeredadoSection({ data }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
      <ReadOnlyField label="Tipo de Crédito" value={data.tipo_credito} />
      <ReadOnlyField label="¿Tiene Garantía?" value={data.tiene_garantia} />
      <ReadOnlyField label="Monto Otorgado VI" value={formatCOP(data.monto_otorgado_vi)} />
    </div>
  );
}