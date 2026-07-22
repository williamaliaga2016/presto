import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import InputTextAreaForm from '@/shared/components/InputTextAreaForm';
import InformacionNotariaSection from '../components/InformacionNotariaSection';
import FormalizacionEscrituraSection from '../components/FormalizacionEscrituraSection';
import DecisionesEnrutamientoSection from '../components/DecisionesEnrutamientoSection';
import ConfirmacionCausarModal from '../components/ConfirmacionCausarModal';
import RegistroContactoSection from '../components/RegistroContactoSection';
import type { FirmarEscrituraCliente } from '../models/firmar_escritura_cliente';
import { EMPTY_CONTROLES_FIRMAR_ESCRITURA } from '../models/firmar_escritura_cliente';
import { useFirmarEscrituraCliente } from '../hooks/useFirmarEscrituraCliente';
import { useUpsertFirmarEscritura } from '../hooks/useUpsertFirmarEscritura';
import { useAvanzarFirmarEscritura } from '../hooks/useAvanzarFirmarEscritura';
import { useControlesFirmarEscritura } from '../hooks/useControlesFirmarEscritura';

/**
 * TODO:
 * Reemplazar este valor por el id real de la actividad en el motor/BPMN
 * cuando ya esté registrado el catálogo de actividades.
 */
const ACTIVITY_ID = 'BBVA_ESCRITURACION_FIRMAR_ESCRITURA_CLIENTE_CE5FAC2F';

const buildInitialState = (id_expediente: number): FirmarEscrituraCliente => ({
  id: 0,
  id_expediente,
  id_actividad: ACTIVITY_ID,
  notaria: null,
  fecha_notaria: null,
  numero_notaria: null,
  ciudad_notaria: null,
  numero_escritura: null,
  fecha_escritura: null,
  representante_legal: null,
  requiere_escalamiento_comercial: null,
  tipologia: null,
  requiere_causar: null,
  observaciones: null,
  tipo_credito: null,
  is_active: false,
  row_status: false,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null
});

const normalizeFirmarEscritura = (
  source: Partial<FirmarEscrituraCliente> | null | undefined,
  id_expediente_fallback: number,
): FirmarEscrituraCliente => ({
  id: Number(source?.id ?? 0),
  id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
  id_actividad: source?.id_actividad ?? ACTIVITY_ID,
  notaria: source?.notaria ?? null,
  fecha_notaria: source?.fecha_notaria ?? null,
  numero_notaria: source?.numero_notaria ?? null,
  ciudad_notaria: source?.ciudad_notaria ?? null,
  numero_escritura: source?.numero_escritura ?? null,
  fecha_escritura: source?.fecha_escritura ?? null,
  representante_legal: source?.representante_legal ?? null,
  requiere_escalamiento_comercial: source?.requiere_escalamiento_comercial ?? null,
  tipologia: source?.tipologia ?? null,
  requiere_causar: source?.requiere_causar ?? null,
  observaciones: source?.observaciones ?? null,
  tipo_credito: source?.tipo_credito ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: source?.created_by ?? 0,
  created_date: source?.created_date ?? '',
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const extractFormularioFromDetail = (
  detail: unknown,
): Partial<FirmarEscrituraCliente> | null => {
  if (!detail || typeof detail !== 'object') {
    return null;
  }

  const detailRecord = detail as Record<string, unknown>;
  const nestedFormulario = detailRecord.formulario;

  if (nestedFormulario && typeof nestedFormulario === 'object') {
    return nestedFormulario as Partial<FirmarEscrituraCliente>;
  }

  return detail as Partial<FirmarEscrituraCliente>;
};

export default function FirmarEscrituraClientePage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<FirmarEscrituraCliente>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);
  const [showCausarModal, setShowCausarModal] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useFirmarEscrituraCliente(id_expediente);
  const { data: controlesData } = useControlesFirmarEscritura();
  const saveMutation = useUpsertFirmarEscritura();
  const avanzarMutation = useAvanzarFirmarEscritura();

  const controles = controlesData?.detail ?? EMPTY_CONTROLES_FIRMAR_ESCRITURA;
  const tiposLeasing = controles.tipos_leasing ?? [];
  const tiposEscrituracion = controles.tipos_escrituracion ?? [];

  // --- Hydration Effects ---

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;

      setForm(buildInitialState(id_expediente));
      setErrorMessage('');
      setSuccessMessage('');
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const formulario = extractFormularioFromDetail(data.detail);
      const loadedEntity = normalizeFirmarEscritura(formulario, id_expediente);

      setForm(loadedEntity);
      setIsDisabled(Number(loadedEntity.id) > 0);
      setCanAdvance(false);

      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) =>
        normalizeFirmarEscritura(
          {
            ...prev,
            id_expediente,
          },
          id_expediente,
        ),
      );
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  // --- Helpers ---

  const updateField = <K extends keyof FirmarEscrituraCliente>(
    field: K,
    value: FirmarEscrituraCliente[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  const requiereEscrituracion = (): boolean => {
    if (!form.tipo_credito) return true; // Si aún no carga, no bloqueamos
    if (tiposEscrituracion.length === 0) return true; // Si no hay catálogo aún, no bloqueamos
    return tiposEscrituracion.some(t => t.code === form.tipo_credito);
  };

  const isLeasing = tiposLeasing.some(t => t.code === form.tipo_credito);

  // --- Validation ---

  const validateAvanzar = (): string[] => {
    const missing: string[] = [];

    if (!form.notaria?.trim()) missing.push('Notaría');
    if (!form.fecha_notaria) missing.push('Fecha Notaría');
    if (!form.numero_notaria) missing.push('Número Notaría');
    if (!form.ciudad_notaria?.trim()) missing.push('Ciudad Notaría');
    if (!form.requiere_escalamiento_comercial) missing.push('¿Requiere Escalamiento Comercial?');

    if (form.requiere_escalamiento_comercial === 'NO') {
      if (!form.numero_escritura?.trim()) missing.push('Número Escritura');
      if (!form.fecha_escritura) missing.push('Fecha Escritura');
    }

    if (form.requiere_escalamiento_comercial === 'SI') {
      if (!form.tipologia?.trim()) missing.push('Tipología');
    }

    if (isLeasing) {
      if (!form.requiere_causar) missing.push('¿Requiere Causar?');
    }

    return missing;
  };

  // --- Handlers ---

  const handleEditar = () => {
    setErrorMessage('');
    setSuccessMessage('');
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    // Guardar NO valida campos obligatorios (Req 9.1)
    if (!form.id_expediente || form.id_expediente <= 0) {
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

      const payload: FirmarEscrituraCliente = normalizeFirmarEscritura(
        {
          ...form,
          id: Number(form.id ?? 0),
          id_expediente: Number(form.id_expediente || id_expediente || 0),
        },
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Información guardada correctamente',
          life: 3000,
        });

        const formularioGuardado = extractFormularioFromDetail(response.detail ?? payload);
        const savedEntity = normalizeFirmarEscritura(formularioGuardado, payload.id_expediente);

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo guardar',
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR GUARDAR FIRMAR ESCRITURA CLIENTE', error);

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al guardar',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage('');
    setSuccessMessage('');

    // Validar campos obligatorios (Req 9.2, 9.3)
    const camposFaltantes = validateAvanzar();
    if (camposFaltantes.length > 0) {
      const msg = `Campos obligatorios faltantes: ${camposFaltantes.join(', ')}`;
      setErrorMessage(msg);
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: msg,
        life: 5000,
      });
      return;
    }

    // Si es Leasing y requiere_causar = "SI", mostrar modal de confirmación
    if (isLeasing && form.requiere_causar === 'SI') {
      setShowCausarModal(true);
      return;
    }

    await ejecutarAvanzar();
  };

  const ejecutarAvanzar = async () => {
    const expedienteId = Number(form.id_expediente ?? 0);

    if (!expedienteId || expedienteId <= 0) {
      const msg = 'No existe un id_expediente válido para avanzar.';
      setErrorMessage(msg);
      toast.current?.show({
        severity: 'warn',
        summary: 'Validación',
        detail: msg,
        life: 3000,
      });
      return;
    }

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(expedienteId);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: 'Actividad avanzada correctamente',
          life: 2000,
        });

        navigate('/home/bandeja');
      } else {
        const msg = response.message || 'No se pudo avanzar la actividad.';

        setErrorMessage(msg);
        setSuccessMessage('');

        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: msg,
          life: 3000,
        });
      }
    } catch (error) {
      console.error('ERROR AVANZAR FIRMAR ESCRITURA CLIENTE', error);
      const msg = 'Ocurrió un error al avanzar.';

      setErrorMessage(msg);
      setSuccessMessage('');

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: msg,
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleConfirmarCausar = async () => {
    setShowCausarModal(false);
    await ejecutarAvanzar();
  };

  const handleCancelarCausar = () => {
    setShowCausarModal(false);
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  // --- Guard: tipo de crédito no requiere escrituración ---

  if (
    hasHydratedRef.current &&
    form.tipo_credito &&
    !requiereEscrituracion()
  ) {
    return (
      <>
        <Toast ref={toast} />
        <Card className="w-full shadow-md">
          <div className="flex flex-col items-center justify-center gap-4 py-8">
            <i className="pi pi-ban text-5xl text-red-500" />
            <h2 className="text-xl font-bold text-gray-800">Acceso Denegado</h2>
            <p className="text-sm text-gray-600 text-center max-w-md">
              El tipo de crédito <strong>{form.tipo_credito}</strong> no requiere
              escrituración. Esta actividad no aplica para este expediente.
            </p>
            <Button
              label="Volver a Bandeja"
              icon="pi pi-arrow-left"
              severity="secondary"
              outlined
              onClick={handleSalir}
            />
          </div>
        </Card>
      </>
    );
  }

  // --- Render ---

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Firmar Escritura Cliente
      </h2>

      <Accordion activeIndex={[0, 2]} multiple>
        <AccordionTab
          disabled={!id_expediente || id_expediente <= 0}
          header="Información del Expediente"
        >
          <EncabezadoActividad
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            activityID={ACTIVITY_ID}
          />
        </AccordionTab>

        <AccordionTab
          header="Funciones Transversales"
          disabled={!id_expediente || id_expediente <= 0}
        >
          <FuncionesTransversales
            idExpediente={Number(form.id_expediente || id_expediente || 0)}
            idActividad={ACTIVITY_ID}
            show_registro_contacto={false}
          />
          <div className="mt-4">
            <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
              Registro de Contacto
            </h3>
            <RegistroContactoSection
              id_expediente={Number(form.id_expediente || id_expediente || 0)}
              id_actividad={ACTIVITY_ID}
            />
          </div>
        </AccordionTab>

        <AccordionTab header="Firmar Escritura Cliente">
          <Card className="w-full shadow-md card-presto-form mb-6">
            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">
                Cargando información...
              </div>
            )}

            {errorMessage && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMessage}
              </div>
            )}

            {successMessage && (
              <div className="mb-4 rounded-md border border-green-200 bg-green-50 px-4 py-3 text-sm text-green-700">
                {successMessage}
              </div>
            )}

            <div className="grid grid-cols-1 md:grid-cols-3 gap-x-4 gap-y-4">
              {/* Información de Notaría */}
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Información de Notaría
                </h3>
              </div>

              <InformacionNotariaSection
                form={form}
                isDisabled={isDisabled}
                updateField={updateField}
              />

              {/* Formalización de Escritura */}
              <div className="md:col-span-3 border-t border-slate-200 my-1" />
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Formalización de Escritura
                </h3>
              </div>

              <FormalizacionEscrituraSection
                form={form}
                isDisabled={isDisabled}
                updateField={updateField}
                representantesLegales={controles.representantes_legales ?? []}
              />

              {/* Decisiones de Enrutamiento */}
              <div className="md:col-span-3 border-t border-slate-200 my-1" />
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Decisiones de Enrutamiento
                </h3>
              </div>

              <DecisionesEnrutamientoSection
                form={form}
                isDisabled={isDisabled}
                updateField={updateField}
                tipologias={controles.tipologias ?? []}
                tiposLeasing={tiposLeasing}
              />

              {/* Observaciones */}
              <div className="md:col-span-3 border-t border-slate-200 my-1" />
              <div className="md:col-span-3 flex flex-col gap-1.5">
                <InputTextAreaForm
                  label="Observaciones"
                  value={form.observaciones ?? ''}
                  onChange={(val) => updateField('observaciones', val || null)}
                  maxLength={1000}
                  rows={4}
                  placeholder="Máximo 1000 caracteres"
                  disabled={isDisabled}
                />
              </div>
            </div>

            {/* Botones de acción */}
            <div className="form-actions">
              <Button
                type="button"
                label="Editar"
                icon="pi pi-pencil"
                severity="info"
                outlined
                onClick={handleEditar}
                disabled={isBusy || !isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={saveMutation.isPending ? 'Guardando...' : 'Guardar'}
                icon="pi pi-save"
                severity="success"
                onClick={handleGuardar}
                disabled={isBusy || isDisabled}
                className="btn-responsive"
              />

              <Button
                type="button"
                label={avanzarMutation.isPending ? 'Avanzando...' : 'Avanzar'}
                icon="pi pi-arrow-right"
                severity="warning"
                onClick={handleAvanzar}
                disabled={isBusy || !canAdvance}
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

      {/* Modal de confirmación de causación (Leasing + requiere_causar = "SI") */}
      <ConfirmacionCausarModal
        visible={showCausarModal}
        onConfirm={handleConfirmarCausar}
        onCancel={handleCancelarCausar}
      />
    </>
  );
}
