/**
 * SeccionMarcacionCobertura
 *
 * Sección editable de captura de datos SITCAR / subsidio "Mi Techo Propio"
 * (CA04). Componente controlado: todos los valores y su manejador único
 * `onChange` se reciben por props.
 *
 * No existe integración real con SITCAR ni catálogos para Tipo de Documento /
 * Tipo de Vivienda / Estado Proceso (ver sección 1.4 / 9 de
 * contexto/BBV-107_RECETA_IMPLEMENTACION.md): se usan listas fijas hasta que
 * se definan catálogos reutilizables.
 */
import DropdownForm from '@/shared/components/DropdownForm';
import InputTextForm from '@/shared/components/InputTextForm';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import InputNumberForm from '@/shared/components/InputNumberForm';
import InputCalendarForm from '@/shared/components/InputCalendarForm';
import InputTimeForm from '@/shared/components/InputTimeForm';
import type { CatalogoOption } from '@/models/CatalogoOption';
import type { RevisionMarcacionCobertura } from '../models/revision_marcacion_cobertura';

const TIPO_DOCUMENTO_OPTIONS: CatalogoOption[] = [
  { code: 'CC', description: 'Cédula de Ciudadanía' },
  { code: 'CE', description: 'Cédula de Extranjería' },
  { code: 'NIT', description: 'NIT' },
];

const TIPO_VIVIENDA_OPTIONS: CatalogoOption[] = [
  { code: 'VIS', description: 'Vivienda de Interés Social' },
  { code: 'VIP', description: 'Vivienda de Interés Prioritario' },
  { code: 'NO_VIS', description: 'No VIS' },
];

const ESTADO_PROCESO_OPTIONS: CatalogoOption[] = [
  { code: 'EN_TRAMITE', description: 'En Trámite' },
  { code: 'APROBADO', description: 'Aprobado' },
  { code: 'RECHAZADO', description: 'Rechazado' },
];

const MAX_OBSERVACIONES = 1000;

interface SeccionMarcacionCoberturaProps {
  form: RevisionMarcacionCobertura;
  onChange: <K extends keyof RevisionMarcacionCobertura>(
    field: K,
    value: RevisionMarcacionCobertura[K],
  ) => void;
  disabled?: boolean;
}

export default function SeccionMarcacionCobertura({
  form,
  onChange,
  disabled = false,
}: SeccionMarcacionCoberturaProps) {
  return (
    <div className="space-y-6">
      {/* ── Identificación ──────────────────────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Identificación
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputTextForm
            label="Consecutivo"
            value={form.consecutivo ?? ''}
            onChange={() => {}}
            disabled
          />
          <DropdownForm
            label="Tipo de Documento"
            value={form.tipo_documento}
            options={TIPO_DOCUMENTO_OPTIONS}
            onChange={(v) => onChange('tipo_documento', v)}
            required
            disabled={disabled}
          />
          <InputTextForm
            label="C.C (Número)"
            value={form.numero_documento ?? ''}
            onChange={(v) => onChange('numero_documento', v || null)}
            required
            disabled={disabled}
          />
          <InputTextForm
            label="TT (Tipo Trámite)"
            value={form.tipo_tramite ?? ''}
            onChange={(v) => onChange('tipo_tramite', v || null)}
            required
            disabled={disabled}
          />
          <InputTextForm
            label="Nombre"
            value={form.nombre ?? ''}
            onChange={(v) => onChange('nombre', v || null)}
            required
            disabled={disabled}
          />
        </div>
      </div>

      {/* ── Proyecto / Constructora ──────────────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Proyecto / Constructora
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputTextForm
            label="Constructora"
            value={form.constructora ?? ''}
            onChange={(v) => onChange('constructora', v || null)}
            required
            disabled={disabled}
          />
          <InputTextForm
            label="Proyecto"
            value={form.proyecto ?? ''}
            onChange={(v) => onChange('proyecto', v || null)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha de Aceptación Plataforma"
            value={form.fecha_aceptacion_plataforma}
            onChange={(v) => onChange('fecha_aceptacion_plataforma', v)}
            required
            disabled={disabled}
          />
          <DropdownForm
            label="Tipo de Vivienda"
            value={form.tipo_vivienda}
            options={TIPO_VIVIENDA_OPTIONS}
            onChange={(v) => onChange('tipo_vivienda', v)}
            required
            disabled={disabled}
          />
        </div>
      </div>

      {/* ── Valores y Plazo ──────────────────────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Valores y Plazo
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputNumberForm
            label="Valor Subsidio"
            value={form.valor_subsidio}
            onChange={(v) => onChange('valor_subsidio', v)}
            required
            disabled={disabled}
            useGrouping
          />
          <InputTextForm
            label="N° Obligación"
            value={form.numero_obligacion ?? ''}
            onChange={(v) => onChange('numero_obligacion', v || null)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha de Desembolso"
            value={form.fecha_desembolso}
            onChange={(v) => onChange('fecha_desembolso', v)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha Próximo Canon"
            value={form.fecha_proximo_canon}
            onChange={(v) => onChange('fecha_proximo_canon', v)}
            required
            disabled={disabled}
          />
          <InputNumberForm
            label="Valor Desembolso"
            value={form.valor_desembolso}
            onChange={(v) => onChange('valor_desembolso', v)}
            required
            disabled={disabled}
            useGrouping
          />
          <InputNumberForm
            label="Intereses Corrientes"
            value={form.intereses_corrientes}
            onChange={(v) => onChange('intereses_corrientes', v)}
            disabled={disabled}
            useGrouping
          />
          <InputNumberForm
            label="Capital"
            value={form.capital}
            onChange={(v) => onChange('capital', v)}
            disabled={disabled}
            useGrouping
          />
          <InputNumberForm
            label="Seguros"
            value={form.seguros}
            onChange={(v) => onChange('seguros', v)}
            disabled={disabled}
            useGrouping
          />
          <InputNumberForm
            label="Cuota Mensual"
            value={form.cuota_mensual}
            onChange={(v) => onChange('cuota_mensual', v)}
            disabled={disabled}
            useGrouping
          />
          <InputNumberForm
            label="Plazo"
            value={form.plazo}
            onChange={(v) => onChange('plazo', v)}
            disabled={disabled}
          />
        </div>
        <div className="mt-4">
          <InputTextAreaForm
            id="marcacion-cobertura-observacion"
            label="Observación"
            value={form.observacion ?? ''}
            onChange={(v) => onChange('observacion', v || null)}
            rows={3}
            disabled={disabled}
          />
        </div>
      </div>

      {/* ── Solicitud y Respuesta de Marcación ───────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Solicitud y Respuesta de Marcación
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputCalendarForm
            label="Fecha Solicitud Marcación"
            value={form.fecha_solicitud_marcacion}
            onChange={(v) => onChange('fecha_solicitud_marcacion', v)}
            required
            disabled={disabled}
          />
          <InputTimeForm
            label="Hora Solicitud Marcación"
            value={form.hora_solicitud_marcacion}
            onChange={(v) => onChange('hora_solicitud_marcacion', v)}
            required
            disabled={disabled}
          />
          <InputTextForm
            label="Responsable M5"
            value={form.responsable_m5 ?? ''}
            onChange={(v) => onChange('responsable_m5', v || null)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha Respuesta Marcación"
            value={form.fecha_respuesta_marcacion}
            onChange={(v) => onChange('fecha_respuesta_marcacion', v)}
            disabled={disabled}
          />
          <InputTimeForm
            label="Hora Respuesta Marcación"
            value={form.hora_respuesta_marcacion}
            onChange={(v) => onChange('hora_respuesta_marcacion', v)}
            disabled={disabled}
          />
        </div>
      </div>

      {/* ── Resolución ────────────────────────────────────────────────────── */}
      <div>
        <h4 className="text-xs font-semibold text-gray-600 uppercase mb-2">
          Resolución
        </h4>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
          <InputTextForm
            label="No Resolución"
            value={form.no_resolucion ?? ''}
            onChange={(v) => onChange('no_resolucion', v || null)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha de la Resolución"
            value={form.fecha_resolucion}
            onChange={(v) => onChange('fecha_resolucion', v)}
            required
            disabled={disabled}
          />
          <InputCalendarForm
            label="Fecha Envío Resolución"
            value={form.fecha_envio_resolucion}
            onChange={(v) => onChange('fecha_envio_resolucion', v)}
            required
            disabled={disabled}
          />
          <DropdownForm
            label="Estado Proceso"
            value={form.estado_proceso}
            options={ESTADO_PROCESO_OPTIONS}
            onChange={(v) => onChange('estado_proceso', v)}
            required
            disabled={disabled}
          />
        </div>
      </div>

      {/* ── Observaciones (CA07) ─────────────────────────────────────────── */}
      <div>
        <InputTextAreaForm
          id="marcacion-cobertura-observaciones"
          label="Observaciones"
          value={form.observaciones ?? ''}
          onChange={(v) => onChange('observaciones', v || null)}
          rows={4}
          maxLength={MAX_OBSERVACIONES}
          required
          disabled={disabled}
        />
      </div>
    </div>
  );
}
