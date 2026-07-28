/**
 * ExcepcionDesembolsoPage
 *
 * Página principal de la actividad "Realizar Excepción Desembolso".
 * Orquesta la carga de datos, el formulario, la validación, el guardado,
 * el avance con modal de certificación y el enrutamiento posterior.
 *
 * Requisitos cubiertos: 1.1–1.6, 2.5, 3.3–3.4, 4.2–4.4, 5.3,
 *                       6.4, 8.1–8.5 (completo)
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
import ModalCertificacion from '../components/ModalCertificacion';
import InformacionHeredada from '../components/InformacionHeredada';
import SeccionDecision from '../components/SeccionDecision';

import type {
  ExcepcionDesembolso,
  ExcepcionDesembolsoHerencia,
  ExcepcionDesembolsoResponse,
} from '../models/excepcion_desembolso';
import { EMPTY_EXCEPCION_DESEMBOLSO } from '../models/excepcion_desembolso';
import { useExcepcionDesembolso } from '../hooks/useExcepcionDesembolso';
import { useUpsertExcepcionDesembolso } from '../hooks/useUpsertExcepcionDesembolso';
import { useAvanzarExcepcionDesembolso } from '../hooks/useAvanzarExcepcionDesembolso';

// ─── Constantes ────────────────────────────────────────────────────────────────

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REALIZAR_EXCEPCION_DESEMBOLSO';

// ─── Componente ────────────────────────────────────────────────────────────────

export default function ExcepcionDesembolsoPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idParam } = useParams();

  const id_expediente = Number(idParam ?? 0);

  // ── Estado del formulario ─────────────────────────────────────────────────
  const [form, setForm] = useState<ExcepcionDesembolso>(
    EMPTY_EXCEPCION_DESEMBOLSO(id_expediente),
  );
  const [herencia, setHerencia] = useState<ExcepcionDesembolsoHerencia | null>(null);
  const [origenCaso, setOrigenCaso] = useState<string|null>(null);

  // ── Estado de errores de carga (dos tipos distintos — Req 1.6 vs Req 2.5) ─
  /**
   * loadError: fallo grave de carga (timeout, error de red, expediente inválido).
   * → Oculta la sección, deshabilita AMBOS botones (Req 1.6).
   */
  const [loadError, setLoadError] = useState(false);
  /**
   * origenError: backend no pudo determinar el origen del trámite.
   * → Muestra mensaje de error, deshabilita SOLO Avanzar, Guardar sigue activo (Req 2.5).
   */
  const [origenError, setOrigenError] = useState(false);

  // ── Estado de operaciones ─────────────────────────────────────────────────
  const [isBusy, setIsBusy] = useState(false);
  const [showCertModal, setShowCertModal] = useState(false);
  const [certModalBusy, setCertModalBusy] = useState(false);

  // ── Estado de validación del checkbox (Req 4.3) ───────────────────────────
  const [checkboxError, setCheckboxError] = useState(false);

  // ── Refs internos ─────────────────────────────────────────────────────────
  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  // ── Queries / Mutations ───────────────────────────────────────────────────
  const { data, isLoading, isError } = useExcepcionDesembolso(id_expediente);
  const saveMutation = useUpsertExcepcionDesembolso();
  const avanzarMutation = useAvanzarExcepcionDesembolso();

  // ── Reset cuando cambia el expediente ────────────────────────────────────
  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(EMPTY_EXCEPCION_DESEMBOLSO(id_expediente));
      setHerencia(null);
      setOrigenCaso(null);
      setLoadError(false);
      setOrigenError(false);
      setCheckboxError(false);
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

    const payload = data.detail as ExcepcionDesembolsoResponse | null;
    if (payload) {
      // Verificar si el origen no pudo ser determinado (Req 2.5)
      const origenNulo = payload.origenCaso === null || payload.origenCaso === undefined;
      if (origenNulo) {
        setOrigenError(true);
      } else {
        setOrigenCaso(payload.origenCaso);
      }

      setHerencia(payload.herencia ?? null);

      // Precargar valores guardados si existen (Req 3.4, 4.4)
      if (payload.formulario) {
        setForm({
          ...EMPTY_EXCEPCION_DESEMBOLSO(id_expediente),
          ...payload.formulario,
          id_expediente,
        });
      }
    }

    hasHydratedRef.current = true;
  }, [data, isError, id_expediente]);

  // ── Helpers ───────────────────────────────────────────────────────────────
  const updateField = <K extends keyof ExcepcionDesembolso>(
    field: K,
    value: ExcepcionDesembolso[K],
  ) => setForm((prev) => ({ ...prev, [field]: value }));

  const handleConfirmacionChange = (checked: boolean) => {
    updateField('confirmacion_excepcion', checked);
    if (checked) setCheckboxError(false); // limpiar error visual al marcar (Req 4.3)
  };

  // ── Validación para Avanzar (Req 8.2, 8.4) ───────────────────────────────
  const validateAvanzar = (): string[] => {
    const missing: string[] = [];
    if (!form.excepcion_autorizada) missing.push('Excepción Autorizada');
    if (!form.requiere_vobo_gerencia) missing.push('¿Requiere VoBo Gerencia?');
    if (!form.confirmacion_excepcion) missing.push('Confirmación de excepción');
    return missing;
  };

  // ── Guardar (Req 8.1, 8.3, 8.5) ─────────────────────────────────────────
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
      setIsBusy(true); // Req 8.5: deshabilita ambos botones

      const payload: ExcepcionDesembolso = {
        ...form,
        id: Number(form.id ?? 0),
        id_expediente,
        id_actividad: ACTIVITY_ID,
        is_active: true,
        row_status: true,
      };

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        // Req 8.1: toast de éxito
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Información guardada correctamente',
          life: 3000,
        });
        // Actualizar el formulario con el registro guardado (preserva id, auditoría)
        if (response.detail) {
          setForm({
            ...EMPTY_EXCEPCION_DESEMBOLSO(id_expediente),
            ...response.detail,
            id_expediente,
          });
        }
        hasHydratedRef.current = true;
      } else {
        // Req 8.3: error de persistencia
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No fue posible guardar la información.',
          life: 4000,
        });
      }
    } catch {
      // Req 8.3: error de persistencia (excepción de red/servidor)
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'No fue posible guardar la información.',
        life: 4000,
      });
    } finally {
      setIsBusy(false); // Req 8.5: re-habilita ambos botones
    }
  };

  // ── Avanzar paso 1: validar y abrir modal (Req 8.2, 8.4) ─────────────────
  const handleAvanzarClick = () => {
    const missing = validateAvanzar();

    if (missing.length > 0) {
      // Req 4.3: resaltar el checkbox si es parte de los campos faltantes
      if (!form.confirmacion_excepcion) {
        setCheckboxError(true);
      }

      // Req 8.4: mostrar lista de campos faltantes, NO abrir modal
      toast.current?.show({
        severity: 'warn',
        summary: 'Campos obligatorios',
        detail: `Campos faltantes: ${missing.join(', ')}`,
        life: 5000,
      });
      return;
    }

    // Limpiar error de checkbox si todo está bien
    setCheckboxError(false);

    // Req 8.2: abrir modal sólo si validación es exitosa
    setShowCertModal(true);
  };

  // ── Avanzar paso 2: confirmar en modal y ejecutar (Req 8.2) ──────────────
  const handleConfirmarAvanzar = async () => {
    try {
      setCertModalBusy(true);
      setIsBusy(true); // Req 8.5: deshabilita ambos botones

      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        // Req 6.4: toast con actividad destino + redirect ≤5s
        const destino =
          response.detail?.actividad_destino ?? 'actividad siguiente';
        toast.current?.show({
          severity: 'success',
          summary: 'Actividad avanzada',
          detail: `El trámite fue enrutado a: ${destino}`,
          life: 4000,
        });
        setShowCertModal(false);
        setTimeout(() => navigate('/home/bandeja'), 4500); // ≤5s (Req 6.4)
      } else {
        // Req 6.3: fallo en transición — mantener estado, mostrar error
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail:
            response.message ||
            'No fue posible completar la transición del flujo.',
          life: 5000,
        });
        setShowCertModal(false);
      }
    } catch {
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'No fue posible completar el avance.',
        life: 4000,
      });
      setShowCertModal(false);
    } finally {
      setCertModalBusy(false);
      setIsBusy(false); // Req 8.5: re-habilita ambos botones
    }
  };

  const handleCancelarModal = () => {
    // Req 7.3: cerrar modal sin ejecutar transición, preservar estado del formulario
    if (!certModalBusy) setShowCertModal(false);
  };

  const handleSalir = () => navigate('/home/bandeja');

  // ── Derivados de estado para botones ─────────────────────────────────────
  /**
   * Guardar se deshabilita cuando:
   * - isBusy (operación en curso — Req 8.5)
   * - loadError grave (Req 1.6)
   * Guardar se mantiene HABILITADO cuando solo hay origenError (Req 2.5)
   */
  const guardarDisabled = isBusy || loadError;

  /**
   * Avanzar se deshabilita cuando:
   * - isBusy (operación en curso — Req 8.5)
   * - loadError grave (Req 1.6)
   * - origenError (no se pudo determinar el origen — Req 2.5)
   */
  const avanzarDisabled = isBusy || loadError || origenError;

  // ── Render ────────────────────────────────────────────────────────────────
  return (
    <>
      <Toast ref={toast} />

      <ModalCertificacion
        visible={showCertModal}
        onConfirmar={handleConfirmarAvanzar}
        onCancelar={handleCancelarModal}
      />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Realizar Excepción Desembolso
      </h2>

      {/* Req 1.1: Accordion con tres tabs; Información General y
          Realizar Excepción Desembolso expandidos por defecto (activeIndex [0,2]) */}
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

          {/* Req 1.6: error de carga en Información General */}
          {loadError && (
            <div className="p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
              No fue posible obtener la información del encabezado. Verifique
              el identificador del expediente e intente nuevamente.
            </div>
          )}

          {/* Encabezado del expediente (datos cliente y notaría en solo lectura) */}
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
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVITY_ID}
            show_registro_contacto={false}
          />
        </AccordionTab>

        {/* ── Tab 3: Realizar Excepción Desembolso (expandido, Req 2.1) ── */}
        <AccordionTab header="Realizar Excepción Desembolso">
          <Card className="w-full shadow-md card-presto-form mb-6">

            {/* Req 1.5: spinner dentro de la sección durante la carga */}
            {isLoading && id_expediente > 0 && (
              <div className="flex items-center gap-3 mb-4 text-sm text-blue-600">
                <ProgressSpinner
                  style={{ width: '1.5rem', height: '1.5rem' }}
                  strokeWidth="4"
                />
                <span>Cargando información...</span>
              </div>
            )}

            {/* Req 1.6: error grave de carga — ocultar sección principal */}
            {loadError && (
              <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded text-sm text-red-700">
                No fue posible obtener la información del expediente. Verifique
                el identificador e intente nuevamente.
              </div>
            )}

            {/* Req 2.5: error de origen — mostrar mensaje, sección visible */}
            {!loadError && origenError && (
              <div className="mb-4 p-3 bg-yellow-50 border border-yellow-200 rounded text-sm text-yellow-800">
                No fue posible identificar el origen del trámite. Puede
                guardar parcialmente, pero no podrá avanzar hasta que el
                sistema resuelva el origen del caso.
              </div>
            )}

            {/* Contenido principal (oculto si hay error grave de carga) */}
            {!loadError && (
              <>
                {/* Información heredada de la actividad anterior (solo lectura) */}
                <div className="mb-6">
                  <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                    Información Heredada
                  </h3>
                  <InformacionHeredada
                    herencia={herencia}
                    origenCaso={origenCaso}
                  />
                </div>

                <div className="border-t border-slate-200 my-4" />

                {/* Sección de decisión: dropdowns, observaciones y checkbox */}
                <SeccionDecision
                  excepcionAutorizada={form.excepcion_autorizada}
                  requiereVoboGerencia={form.requiere_vobo_gerencia}
                  confirmacionExcepcion={form.confirmacion_excepcion}
                  observacionesExcepcion={form.observaciones_excepcion}
                  onExcepcionAutorizadaChange={(v) =>
                    updateField('excepcion_autorizada', v)
                  }
                  onRequiereVoboGerenciaChange={(v) =>
                    updateField('requiere_vobo_gerencia', v)
                  }
                  onConfirmacionExcepcionChange={handleConfirmacionChange}
                  onObservacionesExcepcionChange={(v) =>
                    updateField('observaciones_excepcion', v)
                  }
                  disabled={isBusy}
                  checkboxError={checkboxError}
                />
              </>
            )}

            {/* Botones de acción (Req 1.4, 8.5) */}
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
                onClick={handleAvanzarClick}
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
          </Card>
        </AccordionTab>
      </Accordion>
    </>
  );
}
