import { Button } from 'primereact/button';
import { Message } from 'primereact/message';
import InputTextForm from '@/shared/components/InputTextForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import type { RealizarRecepcionBoleta } from '../models/realizar_recepcion_boleta';

interface Props {
  form: RealizarRecepcionBoleta;
  isDisabled: boolean;
  updateField: <K extends keyof RealizarRecepcionBoleta>(field: K, value: RealizarRecepcionBoleta[K]) => void;
  onEjecutarVUR: () => void;
  isVurLoading: boolean;
}

export default function VurSection({ form, isDisabled, updateField, onEjecutarVUR, isVurLoading }: Props) {
  return (
    <div className="border border-slate-300 rounded-lg overflow-hidden">
      {/* Header con fondo azul */}
      <div className="flex items-center justify-between px-4 py-2 bg-gradient-to-r from-[#004481] to-[#0066b3]">
        <div className="flex items-center gap-2">
          <i className="pi pi-globe text-white text-lg" />
          <span className="text-sm font-bold text-white uppercase tracking-wide">Ejecución VUR</span>
        </div>
        <Button
          type="button"
          label={isVurLoading ? 'Consultando VUR...' : 'Ejecutar VUR'}
          icon="pi pi-caret-right"
          onClick={onEjecutarVUR}
          disabled={true}
          className="p-button-sm"
          style={{ backgroundColor: '#1a1a2e', borderColor: '#1a1a2e', color: '#fff' }}
        />
      </div>

      {/* Contenido */}
      <div className="p-4 space-y-4">
        {form.vur_ejecutado && !form.vur_exitoso && (
          <Message
            severity="warn"
            text="El VUR ha fallado. Por favor diligencie los datos manualmente."
            className="w-full"
          />
        )}
        {form.vur_ejecutado && form.vur_exitoso && (
          <Message
            severity="success"
            text="Datos extraídos exitosamente por el VUR."
            className="w-full"
          />
        )}

        {/* Campos VUR */}
        <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
          <InputTextForm
            label="Número de la Boleta (Radicado)"
            value={form.numero_boleta ?? ''}
            onChange={(val) => updateField('numero_boleta', val || null)}
            disabled={isDisabled}
            placeholder="Número de radicado"
            required
          />

          <InputCalendarForm
            label="Fecha de la Boleta (Ingreso)"
            value={form.fecha_boleta}
            onChange={(val) => updateField('fecha_boleta', val)}
            disabled={isDisabled}
            required
          />

          <InputTextForm
            label="Número de Matrícula"
            value={form.numero_matricula ?? ''}
            onChange={(val) => updateField('numero_matricula', val || null)}
            disabled={isDisabled}
            placeholder="Número(s) de matrícula"
            required
          />
        </div>
      </div>
    </div>
  );
}
