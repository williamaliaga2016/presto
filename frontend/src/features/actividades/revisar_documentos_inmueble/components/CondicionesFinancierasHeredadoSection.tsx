import ReadOnlyField from './ReadOnlyField';
import type { CondicionesFinancierasHeredado } from '../models/revisar_documentos_inmueble';

type Props = {
  data: CondicionesFinancierasHeredado;
};

const formatCOP = (value: number | null | undefined) => {
  if (value === null || value === undefined) return null;
  return new Intl.NumberFormat('es-CO', {
    style: 'currency',
    currency: 'COP',
    maximumFractionDigits: 0,
  }).format(value);
};

const formatTasa = (value: number | null | undefined) => {
  if (value === null || value === undefined) return null;
  return `${(value * 100).toFixed(2)}%`;
};

export default function CondicionesFinancierasHeredadoSection({ data }: Props) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
      <ReadOnlyField label="Scoring" value={data.scoring} />
      <ReadOnlyField label="Subproducto" value={data.subproducto} />
      <ReadOnlyField label="Monto Otorgado" value={formatCOP(data.monto_otorgado)} />
      <ReadOnlyField label="Plazo (meses)" value={data.plazo_meses} />
      <ReadOnlyField label="Tasa" value={formatTasa(data.tasa)} />
      <ReadOnlyField
        label="Condiciones Organismo Decisor"
        value={data.condiciones_organismo_decisor}
      />
    </div>
  );
}