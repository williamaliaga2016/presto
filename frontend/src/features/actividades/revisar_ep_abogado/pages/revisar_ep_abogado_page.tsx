import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosHeredadosSection from '../components/DatosHeredadosSection';
import RevisionLegalSection from '../components/RevisionLegalSection';
import CompuertaConformidadField from '../components/CompuertaConformidadField';
import NovedadesSection from '../components/NovedadesSection';
import type { RevisarEpAbogado } from '../models/revisar_ep_abogado';
import { EMPTY_CONTROLES_REVISAR_EP } from '../models/revisar_ep_abogado';
import { useRevisarEpAbogado } from '../hooks/useRevisarEpAbogado';
import { useControlesRevisarEp } from '../hooks/useControlesRevisarEp';
import { useUpsertRevisarEp } from '../hooks/useUpsertRevisarEp';
import { useAvanzarRevisarEp } from '../hooks/useAvanzarRevisarEp';

const ACTIVITY_ID = 'BBVA_ESCRITURACION_REVISAR_EP_ABOGADO';

const buildInitialState = (id_expediente: number): RevisarEpAbogado => ({
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
  ep_conforme: null,
  tipologia: null,
  casuistica: null,
  observaciones_legales: null,
  is_active: false,
  row_status: false,
  created_by: 0,
  created_date: '',
  modified_by: null,
  modified_date: null,
});

const normalizeRevisarEp = (
  source: Partial<RevisarEpAbogado> | null | undefined,
  id_expediente_fallback: number,
): RevisarEpAbogado => ({
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
  ep_conforme: source?.ep_conforme ?? null,
  tipologia: source?.tipologia ?? null,
  casuistica: source?.casuistica ?? null,
  observaciones_legales: source?.observaciones_legales ?? null,
  is_active: source?.is_active ?? true,
  row_status: source?.row_status ?? true,
  created_by: source?.created_by ?? 0,
  created_date: source?.created_date ?? '',
  modified_by: source?.modified_by ?? null,
  modified_date: source?.modified_date ?? null,
});

const extractFormularioFromDetail = (
  detail: unknown,
): Partial<RevisarEpAbogado> | null => {
  if (!detail || typeof detail !== 'object') {
    return null;
  }

  const detailRecord = detail as Record<string, unknown>;
  const nestedFormulario = detailRecord.formulario;

  if (nestedFormulario && typeof nestedFormulario === 'object') {
    return nestedFormulario as Partial<RevisarEpAbogado>;
  }

  return detail as Partial<RevisarEpAbogado>;
};

export default function RevisarEpAbogadoPage() {
  const toast = useRef<Toast>(null);

  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RevisarEpAbogado>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState('');
  const [successMessage, setSuccessMessage] = useState('');
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRevisarEpAbogado(id_expediente);
  const { data: controlesData } = useControlesRevisarEp(id_expediente);
  const saveMutation = useUpsertRevisarEp();
  const avanzarMutation = useAvanzarRevisarEp();

  const controles = controlesData?.detail ?? EMPTY_CONTROLES_REVISAR_EP;
  const representantes = controles.representantes_legales ?? [];
  const tipologias = controles.tipologias ?? [];
  const casuisticas = controles.casuisticas ?? [];

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
      const loadedEntity = normalizeRevisarEp(formulario, id_expediente);

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
        normalizeRevisarEp(
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

  const updateField = <K extends keyof RevisarEpAbogado>(
    field: K,
    value: RevisarEpAbogado[K],
  ) => {
    setForm((prev) => ({
      ...prev,
      [field]: value,
    }));
  };

  // --- Validation ---

  const validateAvanzar = (): string[] => {
    const missing: string[] = [];

    if (!form.representante_legal) missing.push('Representante Legal');
    if (!form.ep_conforme) missing.push('¿Escritura Pública Conforme?');

    if (form.ep_conforme === 'NO') {
      if (!form.tipologia) missing.push('Tipología');
      if (!form.casuistica) missing.push('Casuística');
      if (!form.observaciones_legales?.trim()) missing.push('Observaciones Legales');
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

      const payload: RevisarEpAbogado = normalizeRevisarEp(
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
        const savedEntity = normalizeRevisarEp(formularioGuardado, payload.id_expediente);

        // Preservar campos heredados (no se persisten en tabla propia,
        // el backend no los devuelve en la respuesta del guardado)
        savedEntity.notaria = savedEntity.notaria ?? form.notaria;
        savedEntity.fecha_notaria = savedEntity.fecha_notaria ?? form.fecha_notaria;
        savedEntity.numero_notaria = savedEntity.numero_notaria ?? form.numero_notaria;
        savedEntity.ciudad_notaria = savedEntity.ciudad_notaria ?? form.ciudad_notaria;
        savedEntity.numero_escritura = savedEntity.numero_escritura ?? form.numero_escritura;
        savedEntity.fecha_escritura = savedEntity.fecha_escritura ?? form.fecha_escritura;

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
      console.error('ERROR GUARDAR REVISAR EP ABOGADO', error);

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

    try {
      setIsBusy(true);

      const response = await avanzarMutation.mutateAsync(form);

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
      console.error('ERROR AVANZAR REVISAR EP ABOGADO', error);

      const errorMsg =
        error instanceof Error ? error.message : 'Ocurrió un error al avanzar.';

      setErrorMessage(errorMsg);
      setSuccessMessage('');

      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: errorMsg,
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate('/home/bandeja');
  };

  // --- Render ---

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Revisar EP Abogado
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
        </AccordionTab>

        <AccordionTab header="Revisar EP Abogado">
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
              {/* Datos Heredados (solo lectura) */}
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Datos Heredados
                </h3>
              </div>

              <DatosHeredadosSection form={form} />

              {/* Revisión Legal */}
              <div className="md:col-span-3 border-t border-slate-200 my-1" />
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Revisión Legal
                </h3>
              </div>

              <div className="md:col-span-3">
                <RevisionLegalSection
                  form={form}
                  updateField={updateField}
                  representantes={representantes}
                  disabled={isDisabled}
                />
              </div>

              {/* Compuerta de Conformidad */}
              <div className="md:col-span-3 border-t border-slate-200 my-1" />
              <div className="md:col-span-3">
                <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                  Conformidad EP
                </h3>
              </div>

              <div className="md:col-span-3">
                <CompuertaConformidadField
                  form={form}
                  updateField={updateField}
                  disabled={isDisabled}
                />
              </div>

              {/* Novedades (condicional: ep_conforme = "NO") */}
              {form.ep_conforme === 'NO' && (
                <>
                  <div className="md:col-span-3 border-t border-slate-200 my-1" />
                  <div className="md:col-span-3">
                    <h3 className="text-sm font-bold text-slate-800 uppercase tracking-wide mb-3">
                      Novedades
                    </h3>
                  </div>

                  <div className="md:col-span-3">
                    <NovedadesSection
                      form={form}
                      updateField={updateField}
                      tipologias={tipologias}
                      casuisticas={casuisticas}
                      disabled={isDisabled}
                    />
                  </div>
                </>
              )}
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
    </>
  );
}
