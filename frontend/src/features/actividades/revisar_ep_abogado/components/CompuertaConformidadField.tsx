import SwitchForm from '@/shared/components/SwitchForm';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

interface Props {
  form: RevisarEpAbogado;
  updateField: <K extends keyof RevisarEpAbogado>(
    field: K,
    value: RevisarEpAbogado[K],
  ) => void;
  onConformidadChange?: (value: RevisarEpAbogado['ep_conforme']) => void;
  disabled?: boolean;
}

/**
 * Campo obligatorio "¿Escritura Pública Conforme?" (SI/NO) como Switch.
 * Corresponde al requerimiento CA04.
 */
export default function CompuertaConformidadField({
  form,
  updateField,
  onConformidadChange,
  disabled = false,
}: Props) {
  const handleChange = (value: boolean) => {
    const newValue: RevisarEpAbogado['ep_conforme'] = value ? 'SI' : 'NO';
    updateField('ep_conforme', newValue);

    // Cuando cambia a "SI", limpiar campos condicionales de novedades
    if (newValue === 'SI') {
      updateField('tipologia', null);
      updateField('casuistica', null);
      updateField('observaciones_legales', null);
    }

    onConformidadChange?.(newValue);
  };

  return (
    <SwitchForm
      label="¿Escritura Pública Conforme?"
      value={form.ep_conforme === 'SI'}
      onChange={handleChange}
      disabled={disabled}
      required
    />
  );
}
