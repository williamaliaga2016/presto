import DropdownForm from '@/shared/components/DropdownForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

interface Props {
  form: RevisarEpAbogado;
  updateField: <K extends keyof RevisarEpAbogado>(
    field: K,
    value: RevisarEpAbogado[K],
  ) => void;
  tipologias: CatalogoOption[];
  casuisticas: CatalogoOption[];
  disabled?: boolean;
}

/**
 * Sección "Novedades" – se muestra condicionalmente cuando ep_conforme = "NO".
 * Contiene los campos obligatorios: Tipología (Parametría L39),
 * Casuística (Parametría L40) y Observaciones Legales.
 * Corresponde a los requerimientos CA05 y CA08.
 */
export default function NovedadesSection({
  form,
  updateField,
  tipologias,
  casuisticas,
  disabled = false,
}: Props) {
  if (form.ep_conforme !== 'NO') {
    return null;
  }

  return (
    <div className="flex flex-col gap-4">
      <DropdownForm
        label="Tipología"
        value={form.tipologia}
        options={tipologias}
        onChange={(val) => updateField('tipologia', val)}
        placeholder="Seleccionar tipología..."
        disabled={disabled}
        required
        filter
        filterPlaceholder="Buscar tipología..."
      />

      <DropdownForm
        label="Casuística"
        value={form.casuistica}
        options={casuisticas}
        onChange={(val) => updateField('casuistica', val)}
        placeholder="Seleccionar casuística..."
        disabled={disabled}
        required
        filter
        filterPlaceholder="Buscar casuística..."
      />

      <InputTextAreaForm
        label="Observaciones Legales"
        value={form.observaciones_legales ?? ''}
        onChange={(val) => updateField('observaciones_legales', val)}
        placeholder="Ingrese las observaciones legales..."
        disabled={disabled}
        required
      />
    </div>
  );
}
