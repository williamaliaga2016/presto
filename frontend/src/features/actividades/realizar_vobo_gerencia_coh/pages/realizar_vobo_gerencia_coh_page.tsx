/**
 * RealizarVoboGerenciaCohPage
 *
 * Página principal de la actividad "Realizar Vobo Gerencia COH".
 * Orquesta la estructura de acordeón con tres tabs:
 *   1. "Información General" — expandido por defecto, EncabezadoActividad en solo lectura
 *   2. "Funciones Transversales" — colapsado por defecto
 *   3. "Vobo Gerencia COH" — expandido por defecto, DatosHeredados + SeccionDictamen
 *
 * Requisitos cubiertos: 1.1, 1.2, 1.3, 1.4, 1.5, 1.6, 2.1, 2.4, 5.7
 */
import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { ProgressSpinner } from 'primereact/progressspinner';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredados from '../components/DatosHeredados';
import SeccionDictamen from '../components/SeccionDictamen';

import type {
  VoboGerenciaCoh,
  VoboGerenciaCohHerencia,
  VoboGerenciaCohResponse,
} from '../models/vobo_gerencia_coh';
import { EMPTY_VOBO_GERENCIA_COH, esObservacionesObligatoria } from '../models/vobo_gerencia_coh';
import { useVoboGerenciaCoh } from '../hooks/useVoboGerenciaCoh';
import { useUpsertVoboGerenciaCoh } from '../hooks/useUpsertVoboGerenciaCoh';
import { useAvanzarVoboGerenciaCoh } from '../hooks/useAvanzarVoboGerenciaCoh';

// ─── Constantes ────────────────────────────────────────────────────────────────

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_VOBO_GERENCIA_COH';

// ─── Componente ────────────────────────────────────────────────────────────────

export default function RealizarVoboGerenciaCohPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idParam } = useParams();

  const id_expediente = Number(idParam ?? 0);

  // ── Estado del formulario ─────────────────────────────────────────────────
  const [form, setForm] = useState<VoboGerenciaCoh>(
    EMPTY_VOBO_GERENCIA_COH(id_expediente),
  );
  const [herencia, setHerencia] = useState<VoboGerenciaCohHerencia | null>(null);

  // ── Estado de errores de carga (Req 1.6 vs Req 2.4) ──────────────────────
  /**
   * loadError: fallo grave de carga (timeout, error de red, expediente inválido).
   * → Oculta la sección, deshabilita AMBOS botones (Req 1.6).
   */
  const [loadError, setLoadError] = useState(false);
  /**
   * herenciaError: backend no pudo proporcionar los datos heredados.
   * → Muestra mensaje de error con botón de reintento, deshabilita SOLO Avanzar,
   *   Guardar sigue activo (Req 2.4).
   */
  const [herenciaError, setHerenciaError] = useState(false);

  // ── Estado de operaciones ─────────────────────────────────────────────────
  const [isBusy, setIsBusy] = useState(false);
  const [observacionesError, setObservacionesError] = useState(false);

  // ── Refs internos ─────────────────────────────────────────────────────────
  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  // ── Queries / Mutations ───────────────────────────────────────────────────
  const { data, isLoading, isError, refetch } = useVoboGerenciaCoh(id_expediente);
  const saveMutation = useUpsertVoboGerenciaCoh();
  const avanzarMutation = useAvanzarVoboGerenciaCoh();

  // ── Reset cuando cambia el expediente ────────────────────────────────────
  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_VOBO_GERENCIA_COH(id_expediente));
      setHerencia(null);
      setLoadError(false);
      setHerenciaError(false);
      setObservacionesError(false);
    }
  }, [id_expediente]);

  // ── Hydration: poblar formulario cuando llegan los datos ─────────────────
  useEffect(() => {
    if (hasHydratedRef.current) return;

    // Error de red/API
    if (isError) {
      setLoadError(true);
      hasHydratedRef.current = true;
      return;
    }

    // Todavía cargando — esperar
    if (!data) return;

    // Backend retornó status false → error grave de carga
    if (!data.status) {
      setLoadError(true);
      hasHydratedRef.current = true;
      return;
    }

    const payload = data.detail as VoboGerenciaCohResponse | null;
    if (payload) {
      // Verificar herencia (Req 2.4)
      if (!payload.herencia) {
        setHerenciaError(true);
      } else {
        setHerencia(payload.herencia);
        setHerenciaError(false);
      }

      // Precarga: siempre se toma el formulario que retorna el backend
      // (incluye created_date ya resuelto por el servidor, aun para
      // expedientes sin registro previo — Req 5.7).
      if (payload.formulario) {
        setForm({
          ...EMPTY_VOBO_GERENCIA_COH(id_expediente),
          ...payload.formulario,
          id_expediente,
        });
      }
    }

    hasHydratedRef.current = true;
  }, [data, isError, id_expediente]);

  // ── Helpers de actualización de campos ───────────────────────────────────
  const updateField = <K extends keyof VoboGerenciaCoh>(
    field: K,
    value: VoboGerenciaCoh[K],
  ) => setForm((prev) => ({ ...prev, [field]: value }));

  // ── Retry herencia (Req 2.4) ──────────────────────────────────────────────
  const handleRetryHerencia = async () => {
    hasHydratedRef.current = false;
    await refetch();
  };

  // ── Guardar (lógica completa en task 11.3) ────────────────────────────────
  const handleGuardar = async () => {
    if (!id_expediente || id_expediente <= 0) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: 'No existe un id_expediente válido.',
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const payload: VoboGerenciaCoh = {
        ...form,
        id: Number(form.id ?? 0),
        id_expediente,
        id_actividad: ACTIVITY_ID,
        is_active: true,
        row_status: true,
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Información guardada correctamente',
          life: 3000,
        });
        if (response.detail) {
          setForm({
            ...EMPTY_VOBO_GERENCIA_COH(id_expediente),
            ...response.detail,
            id_expediente,
          });
        }
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No fue posible guardar la información.',
          life: 4000,
        });
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'No fue posible guardar la información.',
        life: 4000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  // ── Avanzar (lógica de validación completa en task 11.4) ─────────────────
  const handleAvanzar = async () => {
    // Validación frontend (Req 5.3)
    if (!form.concepto_vobo) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Campo obligatorio',
        detail: 'Debe seleccionar un Concepto para avanzar.',
        life: 4000,
      });
      return;
    }

    // Validación: observaciones requeridas si concepto es "No Favorable" (Req 5.4)
    if (
      esObservacionesObligatoria(form.concepto_vobo) &&
      !form.observaciones?.trim()
    ) {
      setObservacionesError(true);
      toast.current?.show({
        severity: 'warn',
        summary: 'Campo obligatorio',
        detail: 'Las observaciones son obligatorias cuando el concepto es "No Favorable".',
        life: 4000,
      });
      return;
    }

    setObservacionesError(false);

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        const destino =
          response.detail?.actividad_destino ?? 'actividad siguiente';
        toast.current?.show({
          severity: 'success',
          summary: 'Actividad avanzada',
          detail: `El trámite fue enviado al Comercial: ${destino}`,
          life: 4000,
        });
        setTimeout(() => navigate('/home/bandeja'), 4500);
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail:
            response.message ||
            'No fue posible completar la transición del flujo.',
          life: 5000,
        });
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'No fue posible completar el avance.',
        life: 4000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate('/home/bandeja');

  // ── Derivados de estado para botones (Req 1.4, 1.6, 2.4) ─────────────────
  /**
   * Guardar se deshabilita cuando:
   * - isBusy (operación en curso — Req 1.4)
   * - loadError grave (Req 1.6)
   * Guardar se mantiene HABILITADO cuando solo hay herenciaError (Req 2.4)
   */
  const guardarDisabled = isBusy || loadError;

  /**
   * Avanzar se deshabilita cuando:
   * - isBusy (operación en curso — Req 1.4)
   * - loadError grave (Req 1.6)
   * - herenciaError (no se pudieron cargar los datos heredados — Req 2.4)
   */
  const avanzarDisabled = isBusy || loadError || herenciaError;

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Realizar Vobo Gerencia COH
      </h2>

      {/* Req 1.1: Accordion con tres tabs.
          "Información General" (índice 0) y "Vobo Gerencia COH" (índice 2)
          expandidos por defecto; "Funciones Transversales" (índice 1) colapsado. */}
      <Accordion activeIndex={[0, 2]} multiple>

        {/* ── Tab 1: Información General (expandido, Req 1.1, 1.2) ─────── */}
        <AccordionTab
          header="Información General"
          disabled={!id_expediente || id_expediente <= 0}
        >
          {/* Req 1.5: spinner mientras carga */}
          {isLoading && id_expediente > 0 && (
            <div className="flex items-center gap-3 py-4 text-sm text-blue-600">
              <ProgressSpinner
                style={{ width: '1.5rem', height: '1.5rem' }}
                strokeWidth="4"
              />
              <span>Cargando información del expediente...</span>
            </div>
          )}

          {/* Req 1.6: error de carga — ocultar encabezado */}
          {loadError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
              No fue posible obtener la información del encabezado. Verifique
              el identificador del expediente e intente nuevamente.
            </div>
          )}

          {/* Encabezado: solo si no carga y no hay error grave */}
          {!isLoading && !loadError && (
            <EncabezadoActividad
              idExpediente={id_expediente}
              activityID={ACTIVITY_ID}
            />
          )}
        </AccordionTab>

        {/* ── Tab 2: Funciones Transversales (colapsado, Req 1.3) ──────── */}
        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          {/* Req 1.3: Expediente Digital y Trazabilidad/Bitácora */}
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVITY_ID}
            show_registro_contacto={false}
          />
        </AccordionTab>

        {/* ── Tab 3: Vobo Gerencia COH (expandido, Req 2.1) ────────────── */}
        <AccordionTab header="Vobo Gerencia COH">
          <Card className="w-full shadow-md card-presto-form mb-6">

            {/* Req 1.5: spinner durante carga */}
            {isLoading && id_expediente > 0 && (
              <div className="flex items-center gap-3 mb-4 text-sm text-blue-600">
                <ProgressSpinner
                  style={{ width: '1.5rem', height: '1.5rem' }}
                  strokeWidth="4"
                />
                <span>Cargando información...</span>
              </div>
            )}

            {/* Req 1.6: error grave — ocultar formulario */}
            {loadError && (
              <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
                No fue posible obtener la información del expediente. Verifique
                el identificador e intente nuevamente.
              </div>
            )}

            {/* Contenido principal (oculto si hay error grave de carga) */}
            {!loadError && (
              <>
                {/* Req 2.1: Datos heredados en solo lectura */}
                <div className="mb-6">
                  <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                    Datos Heredados
                  </h3>

                  {/* Req 2.4: error de herencia con botón de reintento */}
                  {herenciaError ? (
                    <div className="p-3 bg-yellow-50 border border-yellow-200 rounded text-sm text-yellow-800 flex items-center justify-between gap-4">
                      <span>
                        No fue posible cargar la información heredada. Puede guardar
                        parcialmente, pero no podrá avanzar hasta que se carguen los datos.
                      </span>
                      <Button
                        type="button"
                        label="Reintentar"
                        icon="pi pi-refresh"
                        severity="warning"
                        size="small"
                        onClick={handleRetryHerencia}
                      />
                    </div>
                  ) : (
                    <DatosHeredados herencia={herencia} />
                  )}
                </div>

                <div className="border-t border-slate-200 my-4" />

                {/* Req 3.1–3.7: Dictamen del Gerente COH */}
                <SeccionDictamen
                  concepto_vobo={form.concepto_vobo}
                  observaciones={form.observaciones}
                  onConceptoChange={(v) => {
                    updateField('concepto_vobo', v);
                    if (!esObservacionesObligatoria(v)) setObservacionesError(false);
                  }}
                  onObservacionesChange={(v) => {
                    updateField('observaciones', v);
                    if (v?.trim()) setObservacionesError(false);
                  }}
                  disabled={isBusy}
                  observacionesError={observacionesError}
                />

                {/* Req 1.4: Botones de acción */}
                <div className="form-actions">
                  <Button
                    type="button"
                    label={saveMutation.isPending ? 'Guardando...' : 'Guardar'}
                    icon="pi pi-save"
                    severity="success"
                    onClick={handleGuardar}
                    disabled={guardarDisabled}
                    className="btn-responsive"
                  />

                  <Button
                    type="button"
                    label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                    icon="pi pi-arrow-right"
                    severity="warning"
                    onClick={handleAvanzar}
                    disabled={avanzarDisabled}
                    className="btn-responsive"
                  />

                  <Button
                    type="button"
                    label="Salir"
                    icon="pi pi-sign-out"
                    severity="secondary"
                    outlined
                    onClick={handleSalir}
                    disabled={isBusy}
                    className="btn-responsive"
                  />
                </div>
              </>
            )}

            {/* Mostrar botón Salir incluso cuando hay loadError */}
            {loadError && (
              <div className="form-actions">
                <Button
                  type="button"
                  label="Salir"
                  icon="pi pi-sign-out"
                  severity="secondary"
                  outlined
                  onClick={handleSalir}
                  className="btn-responsive"
                />
              </div>
            )}
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
