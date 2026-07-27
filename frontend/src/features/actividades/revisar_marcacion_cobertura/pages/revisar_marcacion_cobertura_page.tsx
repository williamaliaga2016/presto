/**
 * RevisarMarcacionCoberturaPage
 *
 * Página principal de la actividad "Revisar Marcación de Cobertura" (BBV-107).
 * Orquesta la estructura de acordeón con tres tabs:
 *   1. "Información General" — expandido por defecto, EncabezadoActividad en solo lectura
 *   2. "Funciones Transversales" — colapsado por defecto
 *   3. "Revisar Marcación de Cobertura" — expandido por defecto, DatosHeredados +
 *      SeccionMarcacionCobertura + SeccionNotificacionColocaciones
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
import { isValidEmail } from '@/features/actividades/carga_operacion_banco/utils/inputFilters';
import DatosHeredados from '../components/DatosHeredados';
import SeccionMarcacionCobertura from '../components/SeccionMarcacionCobertura';
import SeccionNotificacionColocaciones from '../components/SeccionNotificacionColocaciones';

import type {
  RevisionMarcacionCobertura,
  RevisionMarcacionCoberturaHerencia,
  RevisionMarcacionCoberturaResponse,
} from '../models/revision_marcacion_cobertura';
import {
  EMPTY_REVISION_MARCACION_COBERTURA,
  camposObligatoriosFaltantes,
  emailColocacionesInvalido,
} from '../models/revision_marcacion_cobertura';
import { useRevisionMarcacionCobertura } from '../hooks/useRevisionMarcacionCobertura';
import { useUpsertRevisionMarcacionCobertura } from '../hooks/useUpsertRevisionMarcacionCobertura';
import { useAvanzarRevisionMarcacionCobertura } from '../hooks/useAvanzarRevisionMarcacionCobertura';

// ─── Constantes ────────────────────────────────────────────────────────────────

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REVISAR_MARCACION_COBERTURA';

// ─── Componente ────────────────────────────────────────────────────────────────

export default function RevisarMarcacionCoberturaPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idParam } = useParams();

  const id_expediente = Number(idParam ?? 0);

  // ── Estado del formulario ─────────────────────────────────────────────────
  const [form, setForm] = useState<RevisionMarcacionCobertura>(
    EMPTY_REVISION_MARCACION_COBERTURA(id_expediente),
  );
  const [herencia, setHerencia] = useState<RevisionMarcacionCoberturaHerencia | null>(null);

  // ── Estado de errores de carga ────────────────────────────────────────────
  /**
   * loadError: fallo grave de carga (timeout, error de red, expediente inválido).
   * → Oculta la sección, deshabilita AMBOS botones.
   */
  const [loadError, setLoadError] = useState(false);
  /**
   * herenciaError: backend no pudo proporcionar los datos del encabezado (CA02).
   * → Muestra mensaje de error con botón de reintento, deshabilita SOLO Avanzar,
   *   Guardar sigue activo.
   */
  const [herenciaError, setHerenciaError] = useState(false);

  // ── Estado de operaciones ─────────────────────────────────────────────────
  const [isBusy, setIsBusy] = useState(false);
  const [emailError, setEmailError] = useState(false);

  // ── Refs internos ─────────────────────────────────────────────────────────
  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  // ── Queries / Mutations ───────────────────────────────────────────────────
  const { data, isLoading, isError, refetch } = useRevisionMarcacionCobertura(id_expediente);
  const saveMutation = useUpsertRevisionMarcacionCobertura();
  const avanzarMutation = useAvanzarRevisionMarcacionCobertura();

  // ── Reset cuando cambia el expediente ────────────────────────────────────
  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_REVISION_MARCACION_COBERTURA(id_expediente));
      setHerencia(null);
      setLoadError(false);
      setHerenciaError(false);
      setEmailError(false);
    }
  }, [id_expediente]);

  // ── Hydration: poblar formulario cuando llegan los datos ─────────────────
  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (isError) {
      setLoadError(true);
      hasHydratedRef.current = true;
      return;
    }

    if (!data) return;

    if (!data.status) {
      setLoadError(true);
      hasHydratedRef.current = true;
      return;
    }

    const payload = data.detail as RevisionMarcacionCoberturaResponse | null;
    if (payload) {
      if (!payload.herencia) {
        setHerenciaError(true);
      } else {
        setHerencia(payload.herencia);
        setHerenciaError(false);
      }

      if (payload.formulario) {
        if (payload.formulario.id > 0) {
          setForm({
            ...EMPTY_REVISION_MARCACION_COBERTURA(id_expediente),
            ...payload.formulario,
            id_expediente,
          });
        } else {
          setForm(EMPTY_REVISION_MARCACION_COBERTURA(id_expediente));
        }
      }
    }

    hasHydratedRef.current = true;
  }, [data, isError, id_expediente]);

  // ── Helpers de actualización de campos ───────────────────────────────────
  const updateField = <K extends keyof RevisionMarcacionCobertura>(
    field: K,
    value: RevisionMarcacionCobertura[K],
  ) => setForm((prev) => ({ ...prev, [field]: value }));

  // ── Retry herencia ─────────────────────────────────────────────────────────
  const handleRetryHerencia = async () => {
    hasHydratedRef.current = false;
    await refetch();
  };

  // ── Guardar ─────────────────────────────────────────────────────────────
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

      const payload: RevisionMarcacionCobertura = {
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
            ...EMPTY_REVISION_MARCACION_COBERTURA(id_expediente),
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

  // ── Avanzar ────────────────────────────────────────────────────────────────
  const handleAvanzar = async () => {
    // CA04: validación de campos obligatorios
    const faltantes = camposObligatoriosFaltantes(form);
    if (faltantes.length > 0) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Campos obligatorios',
        detail: `Faltan: ${faltantes.join(', ')}.`,
        life: 5000,
      });
      return;
    }

    // CA08: bloqueo por correo faltante o inválido
    if (emailColocacionesInvalido(form.email_area_colocaciones, isValidEmail)) {
      setEmailError(true);
      toast.current?.show({
        severity: 'warn',
        summary: 'Correo obligatorio',
        detail: 'El correo del Área de Colocaciones es obligatorio y debe tener un formato válido.',
        life: 4000,
      });
      return;
    }

    setEmailError(false);

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        const destino = response.detail?.actividad_destino ?? 'actividad siguiente';
        toast.current?.show({
          severity: 'success',
          summary: 'Actividad avanzada',
          detail: `El trámite fue enviado a: ${destino}`,
          life: 4000,
        });
        setTimeout(() => navigate('/home/bandeja'), 4500);
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No fue posible completar la transición del flujo.',
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

  // ── Derivados de estado para botones ─────────────────────────────────────
  const guardarDisabled = isBusy || loadError;
  const avanzarDisabled = isBusy || loadError || herenciaError;

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Revisar Marcación de Cobertura
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>

        {/* ── Tab 1: Información General ────────────────────────────────── */}
        <AccordionTab
          header="Información General"
          disabled={!id_expediente || id_expediente <= 0}
        >
          {isLoading && id_expediente > 0 && (
            <div className="flex items-center gap-3 py-4 text-sm text-blue-600">
              <ProgressSpinner style={{ width: '1.5rem', height: '1.5rem' }} strokeWidth="4" />
              <span>Cargando información del expediente...</span>
            </div>
          )}

          {loadError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
              No fue posible obtener la información del encabezado. Verifique
              el identificador del expediente e intente nuevamente.
            </div>
          )}

          {!isLoading && !loadError && (
            <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
          )}
        </AccordionTab>

        {/* ── Tab 2: Funciones Transversales ────────────────────────────── */}
        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVITY_ID}
            show_registro_contacto={false}
          />
        </AccordionTab>

        {/* ── Tab 3: Revisar Marcación de Cobertura ─────────────────────── */}
        <AccordionTab header="Revisar Marcación de Cobertura">
          <Card className="w-full shadow-md card-presto-form mb-6">

            {isLoading && id_expediente > 0 && (
              <div className="flex items-center gap-3 mb-4 text-sm text-blue-600">
                <ProgressSpinner style={{ width: '1.5rem', height: '1.5rem' }} strokeWidth="4" />
                <span>Cargando información...</span>
              </div>
            )}

            {loadError && (
              <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
                No fue posible obtener la información del expediente. Verifique
                el identificador e intente nuevamente.
              </div>
            )}

            {!loadError && (
              <>
                <div className="mb-6">
                  <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                    Datos Heredados
                  </h3>

                  {herenciaError ? (
                    <div className="p-3 bg-yellow-50 border border-yellow-200 rounded text-sm text-yellow-800 flex items-center justify-between gap-4">
                      <span>
                        No fue posible cargar la información del encabezado. Puede guardar
                        parcialmente, pero no podrá avanzar hasta que se cargue la información.
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

                <SeccionMarcacionCobertura
                  form={form}
                  onChange={updateField}
                  disabled={isBusy}
                />

                <div className="border-t border-slate-200 my-4" />

                <SeccionNotificacionColocaciones
                  email={form.email_area_colocaciones}
                  onChange={(v) => {
                    updateField('email_area_colocaciones', v);
                    if (v) setEmailError(false);
                  }}
                  disabled={isBusy}
                  invalid={emailError}
                />

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
