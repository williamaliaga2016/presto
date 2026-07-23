import DropdownForm from '@/shared/components/DropdownForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';

interface Props {
  form: RevisarEpAbogado;
  updateField: <K extends keyof RevisarEpAbogado>(
    field: K,
    value: RevisarEpAbogado[K],
  ) => void;
  representantes: CatalogoOption[];
  disabled?: boolean;
}

/**
 * Sección "Revisión Legal" – campo Representante Legal.
 * El valor se precarga/hereda de la actividad "Firmar Escritura Cliente"
 * y es editable mediante un dropdown con opciones de Parametría L38.
 * Corresponde al requerimiento CA03.
 */
export default function RevisionLegalSection({
  form,
  updateField,
  representantes,
  disabled = false,
}: Props) {
  return (
    <DropdownForm
      label="Representante Legal"
      value={form.representante_legal}
      options={representantes}
      onChange={(val) => updateField('representante_legal', val)}
      placeholder="Seleccionar representante..."
      disabled={disabled}
      required
      filter
      filterPlaceholder="Buscar representante..."
    />
  );
}
