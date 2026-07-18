import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import { SelectButton } from 'primereact/selectbutton';
import { Toast } from 'primereact/toast';

import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosTitularSection from '@/features/actividades/validar_informacion/components/DatosTitularSection';
import DatosCreditoSection from '@/features/actividades/validar_informacion/components/DatosCreditoSection';
import CondicionesFinancierasSection from '@/features/actividades/validar_informacion/components/CondicionesFinancierasSection';
import DatosInmuebleSection from '@/features/actividades/validar_informacion/components/DatosInmuebleSection';
import RegistroContactoSection from '@/features/actividades/validar_informacion/components/RegistroContactoSection';
import { useValidarInformacion } from '@/features/actividades/validar_informacion/hooks/useValidarInformacion';
import { useControlesValidarInformacion } from '@/features/actividades/validar_informacion/hooks/useControlesValidarInformacion';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '@/features/actividades/validar_informacion/models/catalogo';
import { EMPTY_VALIDAR_INFORMACION } from '@/features/actividades/validar_informacion/models/validar_informacion';
import type { RevisarDocumentosInmueble } from '../models/revisar_documentos_inmueble';
import {
  buildInitialState,
  normalizeRevisarDocumentosInmueble,
  validateAvanzarFields,
} from '../models/revisar_documentos_inmueble.form';
import { useRevisarDocumentosInmueble } from '../hooks/useRevisarDocumentosInmueble';
import { useUpsertRevisarDocumentosInmueble } from '../hooks/useUpsertRevisarDocumentosInmueble';
import { useAvanzarRevisarDocumentosInmueble } from '../hooks/useAvanzarRevisarDocumentosInmueble';
import { useControlesRevisarDocumentosInmueble } from '../hooks/useControlesRevisarDocumentosInmueble';

const ACTIVIDAD_ID = 'ACT_REVISAR_DOCS';

const DOCUMENTOS_CORRECTOS_OPTIONS = [
  { label: 'Sí', value: true },
  { label: 'No', value: false },
];

export default function RevisarDocumentosInmueblePage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<RevisarDocumentosInmueble>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState('');
  const [invalidFields, setInvalidFields] = useState<Set<string>>(new Set());
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data: validarInfoData } = useValidarInformacion(id_expediente);
  const { data: controlesData } = useControlesValidarInformacion(id_expediente);
  const { data, isLoading } = useRevisarDocumentosInmueble(id_expediente);
  const { data: controlesRdiData } = useControlesRevisarDocumentosInmueble(id_expediente);

  const saveMutation = useUpsertRevisarDocumentosInmueble();
  const avanzarMutation = useAvanzarRevisarDocumentosInmueble();

  const validarInfo = validarInfoData?.status ? (validarInfoData.detail ?? EMPTY_VALIDAR_INFORMACION(id_expediente)) : EMPTY_VALIDAR_INFORMACION(id_expediente);
  const controles = controlesData?.status ? (controlesData.detail ?? EMPTY_CONTROLES_VALIDAR_INFORMACION) : EMPTY_CONTROLES_VALIDAR_INFORMACION;
  const motivoDevolucionOptions = controlesRdiData?.status
    ? (controlesRdiData.detail?.motivo_devolucion ?? [])
    : [];

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setErrorMessage('');
      setInvalidFields(new Set());
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      setForm(normalizeRevisarDocumentosInmueble(data.detail, id_expediente));
      setIsDisabled(true);
      setCanAdvance(Boolean(data.detail.id && data.detail.id > 0));
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
      setForm(normalizeRevisarDocumentosInmueble({ id_expediente }, id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const clearInvalidField = (field: string) => {
    setInvalidFields((prev) => {
      if (!prev.has(field)) return prev;
      const next = new Set(prev);
      next.delete(field);
      return next;
    });
  };

  const updateField = <K extends keyof RevisarDocumentosInmueble>(
    field: K,
    value: RevisarDocumentosInmueble[K],
  ) => {
    clearInvalidField(field as string);
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleEditar = () => {
    setErrorMessage('');
    setInvalidFields(new Set());
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    setErrorMessage('');

    if (!form.id_expediente || form.id_expediente <= 0) {
      toast.current?.show({ severity: 'warn', summary: 'Validación', detail: 'No existe un id_expediente válido.', life: 3000 });
      return;
    }

    try {
      setIsBusy(true);
      const response = await saveMutation.mutateAsync(form);

      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Revisar Documentos Inmueble guardado correctamente', life: 3000 });
        setForm(normalizeRevisarDocumentosInmueble(response.detail ?? form, id_expediente));
        setIsDisabled(true);
        setCanAdvance(true);
        hasHydratedRef.current = true;
      } else {
        toast.current?.show({ severity: 'warn', summary: 'Atención', detail: response.message || 'No se pudo guardar', life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: 'error', summary: 'Error', detail: 'Ocurrió un error al guardar', life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage('');

    const missing = validateAvanzarFields(form);
    if (missing.size > 0) {
      setInvalidFields(missing);
      toast.current?.show({ severity: 'warn', summary: 'Datos Obligatorios Faltantes', detail: 'Complete los campos requeridos antes de avanzar.', life: 4000 });
      return;
    }

    setInvalidFields(new Set());

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(form.id_expediente);

      if (response.status) {
        toast.current?.show({ severity: 'success', summary: 'Éxito', detail: 'Actividad avanzada correctamente', life: 2000 });
        navigate('/home/bandeja');
      } else {
        const msg = response.message || 'No se pudo avanzar la actividad.';
        setErrorMessage(msg);
        toast.current?.show({ severity: 'warn', summary: 'Atención', detail: msg, life: 4000 });
      }
    } catch {
      const msg = 'Ocurrió un error al avanzar.';
      setErrorMessage(msg);
      toast.current?.show({ severity: 'error', summary: 'Error', detail: msg, life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate('/home/bandeja');
  const isInvalid = (field: string) => invalidFields.has(field);
  const noop = () => {};

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Revisar Documentos Inmueble
      </h2>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        {/* Columna izquierda 2/3 */}
        <div className="lg:col-span-2 flex flex-col gap-4">
          <Accordion multiple activeIndex={[0, 1, 2, 3]}>
            <AccordionTab header="Datos Titular" disabled={!id_expediente}>
              <DatosTitularSection
                data={validarInfo}
                controles={controles}
                isEditing={false}
                onChange={noop as never}
              />
            </AccordionTab>

            <AccordionTab header="Datos del Crédito" disabled={!id_expediente}>
              <DatosCreditoSection
                data={validarInfo}
                controles={controles}
                isEditing={false}
                onChange={noop as never}
              />
            </AccordionTab>

            <AccordionTab header="Condiciones Financieras" disabled={!id_expediente}>
              <CondicionesFinancierasSection
                data={validarInfo}
                controles={controles}
                isEditing={false}
                onChange={noop as never}
              />
            </AccordionTab>

            <AccordionTab header="Datos del Inmueble" disabled={!id_expediente}>
              <DatosInmuebleSection
                data={validarInfo}
                controles={controles}
                isEditing={false}
                onChange={noop as never}
              />
            </AccordionTab>

            <AccordionTab header="Registro Contacto" disabled={!id_expediente}>
              <RegistroContactoSection
                id_expediente={id_expediente}
                id_actividad={ACTIVIDAD_ID}
                controles={controles}
              />
            </AccordionTab>
          </Accordion>

          <Card className="w-full shadow-md card-presto-form">
            <h3 className="text-lg font-semibold mb-4">Revisar Documentos Inmueble</h3>

            {isLoading && id_expediente > 0 && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            {errorMessage && (
              <div className="mb-4 rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
                {errorMessage}
              </div>
            )}

            <div className="flex flex-col gap-4">
              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">¿Documentos Correctos?</label>
                <SelectButton
                  value={form.documentos_correctos}
                  options={DOCUMENTOS_CORRECTOS_OPTIONS}
                  onChange={(e) => {
                    updateField('documentos_correctos', e.value as boolean | null);
                    if (e.value !== false) updateField('motivo_devolucion', null);
                  }}
                  disabled={isDisabled}
                  className={isInvalid('documentos_correctos') ? 'p-invalid' : ''}
                />
                {isInvalid('documentos_correctos') && (
                  <small className="text-red-600">Campo obligatorio</small>
                )}
              </div>

              {form.documentos_correctos === false && (
                <div className="flex flex-col gap-1">
                  <label className="font-semibold text-sm">Motivo de Devolución</label>
                  <Dropdown
                    value={form.motivo_devolucion}
                    options={motivoDevolucionOptions}
                    optionLabel="description"
                    optionValue="code"
                    onChange={(e) => updateField('motivo_devolucion', e.value as string)}
                    placeholder="Seleccione un motivo..."
                    className={`form-dropdown-presto w-full${isInvalid('motivo_devolucion') ? ' p-invalid' : ''}`}
                    disabled={isDisabled}
                  />
                  {isInvalid('motivo_devolucion') && (
                    <small className="text-red-600">Campo obligatorio cuando los documentos no son correctos</small>
                  )}
                </div>
              )}

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Observaciones</label>
                <InputTextarea
                  value={form.observaciones ?? ''}
                  onChange={(e) => updateField('observaciones', e.target.value)}
                  rows={5}
                  autoResize
                  className={`form-textarea-presto w-full${isInvalid('observaciones') ? ' p-invalid' : ''}`}
                  disabled={isDisabled}
                  placeholder="Ingrese observaciones"
                />
                {isInvalid('observaciones') && (
                  <small className="text-red-600">Campo obligatorio</small>
                )}
              </div>
            </div>

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
        </div>

        {/* Columna derecha 1/3 */}
        <div className="lg:col-span-1">
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVIDAD_ID}
            filter_by_activity={false}
            show_carta_aprobacion={true}
          />
        </div>
      </div>
    </>
  );
}
