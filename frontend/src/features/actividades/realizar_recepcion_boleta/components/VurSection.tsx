import { InputText } from 'primereact/inputtext';
import { Calendar } from 'primereact/calendar';
import { Button } from 'primereact/button';
import { Message } from 'primereact/message';
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
    <div className="space-y-4">
      {/* Botón Ejecutar VUR + Alerta */}
      <div className="flex items-center gap-3 mb-4">
        <div className="flex items-center gap-2">
          <i className="pi pi-android text-blue-600 text-lg" />
          <span className="text-sm font-bold text-blue-700 uppercase">Ejecución VUR</span>
        </div>
        <div className="ml-auto">
          <Button
            type="button"
            label={isVurLoading ? 'Consultando VUR...' : 'Ejecutar VUR'}
            icon="pi pi-caret-right"
            onClick={onEjecutarVUR}
            disabled={isDisabled || isVurLoading}
            className="p-button-raised"
            style={{ backgroundColor: '#004481', borderColor: '#004481', color: '#fff' }}
          />
        </div>
      </div>

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
        <div className="flex flex-col gap-1">
          <label className="text-xs font-medium text-gray-700">Número de Boleta (Radicado) *</label>
          <InputText
            value={form.numero_boleta ?? ''}
            onChange={(e) => updateField('numero_boleta', e.target.value || null)}
            disabled={isDisabled}
            placeholder="Número de radicado"
          />
        </div>

        <div className="flex flex-col gap-1">
          <label className="text-xs font-medium text-gray-700">Fecha de Boleta *</label>
          <Calendar
            value={form.fecha_boleta ? new Date(form.fecha_boleta) : null}
            onChange={(e) => updateField('fecha_boleta', e.value ? (e.value as Date).toISOString().split('T')[0] : null)}
            disabled={isDisabled}
            maxDate={new Date()}
            dateFormat="yy-mm-dd"
            showIcon
            placeholder="AAAA-MM-DD"
          />
        </div>

        <div className="flex flex-col gap-1">
          <label className="text-xs font-medium text-gray-700">Número de Matrícula *</label>
          <InputText
            value={form.numero_matricula ?? ''}
            onChange={(e) => updateField('numero_matricula', e.target.value || null)}
            disabled={isDisabled}
            placeholder="Número(s) de matrícula"
          />
        </div>
      </div>
    </div>
  );
}
