import { useEffect, useRef, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Dropdown } from 'primereact/dropdown';
import { InputTextarea } from 'primereact/inputtextarea';
import { SelectButton } from 'primereact/selectbutton';
import { Toast } from 'primereact/toast';

import EncabezadoActividad from '@/features/encabezado/pages/EncabezadoActividad';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';

import DatosTitularHeredadoSection from '../components/DatosTitularHeredadoSection';
import DatosInmuebleHeredadoSection from '../components/DatosInmuebleHeredadoSection';
import DatosCreditoHeredadoSection from '../components/DatosCreditoHeredadoSection';
import CondicionesFinancierasHeredadoSection from '../components/CondicionesFinancierasHeredadoSection';
import DocumentosObligatoriosAlert from '../components/DocumentosObligatoriosAlert';

import {
  ACTIVIDAD_ID_REVISAR_DOCUMENTOS_INMUEBLE,
  EMPTY_DATOS_HEREDADOS_RDI,
  EMPTY_DOCUMENTOS_OBLIGATORIOS_RDI,
  buildInitialState,
  normalizeRevisarDocumentosInmueble,
  validateAvanzarFields,
  type RevisarDocumentosInmuebleFormulario,
} from '../models/revisar_documentos_inmueble';
import { useRevisarDocumentosInmueble } from '../hooks/useRevisarDocumentosInmueble';
import { useUpsertRevisarDocumentosInmueble } from '../hooks/useUpsertRevisarDocumentosInmueble';
import { useAvanzarRevisarDocumentosInmueble } from '../hooks/useAvanzarRevisarDocumentosInmueble';
import { useControlesRevisarDocumentosInmueble } from '../hooks/useControlesRevisarDocumentosInmueble';

const ACTIVIDAD_ID = ACTIVIDAD_ID_REVISAR_DOCUMENTOS_INMUEBLE;

const RESTRICCION_DOCUMENTOS_INCOMPLETOS =
  'No puede avanzar el caso como correcto si existen documentos obligatorios rechazados o faltantes en el expediente.';

export default function RevisarDocumentosInmueblePage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();

  const id_expediente = Number(idExpedienteParam ?? 0);
  const has_valid_expediente = id_expediente > 0;

  const [form, setForm] = useState<RevisarDocumentosInmuebleFormulario>(
    buildInitialState(id_expediente),
  );
  const [invalidFields, setInvalidFields] = useState<Set<string>>(new Set());
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useRevisarDocumentosInmueble(id_expediente);
  const { data: controlesData } = useControlesRevisarDocumentosInmueble(id_expediente);

  const saveMutation = useUpsertRevisarDocumentosInmueble();
  const avanzarMutation = useAvanzarRevisarDocumentosInmueble();

  const detail = data?.status ? data.detail : undefined;
  const datosHeredados = detail?.datos_heredados ?? EMPTY_DATOS_HEREDADOS_RDI;
  const documentosObligatorios = detail?.documentos_obligatorios ?? EMPTY_DOCUMENTOS_OBLIGATORIOS_RDI;
  const motivoDevolucionOptions = controlesData?.status
    ? (controlesData.detail?.motivo_devolucion ?? [])
    : [];

  const documentosCorrectosOptions = [
    { label: 'Sí', value: true, disabled: !documentosObligatorios.completos },
    { label: 'No', value: false, disabled: false },
  ];

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setInvalidFields(new Set());
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    const formulario = detail?.formulario;

    if (formulario) {
      const loaded = normalizeRevisarDocumentosInmueble(formulario, id_expediente);
      setForm(loaded);
      setIsDisabled(Number(formulario.id) > 0);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (!has_valid_expediente) {
      setForm(buildInitialState(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm(buildInitialState(id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, detail, id_expediente, has_valid_expediente]);

  const clearInvalidField = (field: string) => {
    setInvalidFields((prev) => {
      if (!prev.has(field)) return prev;
      const next = new Set(prev);
      next.delete(field);
      return next;
    });
  };

  const updateField = <K extends keyof RevisarDocumentosInmuebleFormulario>(
    field: K,
    value: RevisarDocumentosInmuebleFormulario[K],
  ) => {
    clearInvalidField(field as string);
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const handleDocumentosCorrectosChange = (value: boolean | null) => {
    if (value === true && !documentosObligatorios.completos) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Documentos obligatorios pendientes',
        detail: RESTRICCION_DOCUMENTOS_INCOMPLETOS,
        life: 5000,
      });
      return;
    }

    updateField('documentos_correctos', value);
    if (value !== false) {
      updateField('motivo_devolucion', null);
    }
  };

  const handleEditar = () => {
    setInvalidFields(new Set());
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const handleGuardar = async () => {
    if (form.documentos_correctos === true && !documentosObligatorios.completos) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Documentos obligatorios pendientes',
        detail: RESTRICCION_DOCUMENTOS_INCOMPLETOS,
        life: 5000,
      });
      return;
    }

    try {
      setIsBusy(true);
      const payload = normalizeRevisarDocumentosInmueble({ ...form, id_expediente }, id_expediente);
      const response = await saveMutation.mutateAsync(payload);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: response.message || 'Progreso guardado',
          life: 3000,
        });
        setForm(normalizeRevisarDocumentosInmueble(response.detail ?? payload, id_expediente));
        setIsDisabled(true);
        setCanAdvance(true);
        setInvalidFields(new Set());
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
      console.error('ERROR GUARDAR REVISAR DOCUMENTOS INMUEBLE', error);
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
    const missing = validateAvanzarFields(form);
    if (missing.size > 0) {
      setInvalidFields(missing);
      toast.current?.show({
        severity: 'warn',
        summary: 'Datos Obligatorios Faltantes',
        detail: 'Complete los campos requeridos antes de avanzar.',
        life: 4000,
      });
      return;
    }

    if (form.documentos_correctos === true && !documentosObligatorios.completos) {
      toast.current?.show({
        severity: 'warn',
        summary: 'Documentos obligatorios pendientes',
        detail: RESTRICCION_DOCUMENTOS_INCOMPLETOS,
        life: 5000,
      });
      return;
    }

    setInvalidFields(new Set());

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(id_expediente);

      if (response.status) {
        toast.current?.show({
          severity: 'success',
          summary: 'Éxito',
          detail: response.message || 'Actividad avanzada correctamente',
          life: 2000,
        });
        navigate('/home/bandeja');
      } else {
        toast.current?.show({
          severity: 'warn',
          summary: 'Atención',
          detail: response.message || 'No se pudo avanzar la actividad.',
          life: 4000,
        });
      }
    } catch (error) {
      console.error('ERROR AVANZAR REVISAR DOCUMENTOS INMUEBLE', error);
      toast.current?.show({
        severity: 'error',
        summary: 'Error',
        detail: 'Ocurrió un error al avanzar.',
        life: 3000,
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => navigate('/home/bandeja');
  const isInvalid = (field: string) => invalidFields.has(field);

  return (
    <div>
      <Toast ref={toast} />

      <Accordion multiple activeIndex={[0, 1, 2]}>
        <AccordionTab header="Información General" disabled={!has_valid_expediente}>
          <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVIDAD_ID} />
        </AccordionTab>

        <AccordionTab header="Funciones Transversales" disabled={!has_valid_expediente}>
          <FuncionesTransversales
            idExpediente={id_expediente}
            idActividad={ACTIVIDAD_ID}
            filter_by_activity={false}
            show_bitacora
            show_carta_aprobacion
            show_carta_compromiso={false}
          />
        </AccordionTab>

        <AccordionTab header="Revisar Documentos Inmueble" disabled={!has_valid_expediente}>
          <Accordion multiple activeIndex={[0, 1, 2, 3]}>
            <AccordionTab header="Datos Titular">
              <DatosTitularHeredadoSection data={datosHeredados.datos_titular} />
            </AccordionTab>

            <AccordionTab header="Datos del Crédito">
              <DatosCreditoHeredadoSection data={datosHeredados.datos_credito} />
            </AccordionTab>

            <AccordionTab header="Condiciones Financieras">
              <CondicionesFinancierasHeredadoSection data={datosHeredados.condiciones_financieras} />
            </AccordionTab>

            <AccordionTab header="Datos del Inmueble">
              <DatosInmuebleHeredadoSection data={datosHeredados.datos_inmueble} />
            </AccordionTab>
          </Accordion>

          <Card className="w-full shadow-md card-presto-form mt-4">
            {isLoading && has_valid_expediente && (
              <div className="mb-4 text-sm text-blue-600">Cargando información...</div>
            )}

            <DocumentosObligatoriosAlert data={documentosObligatorios} />

            <div className="flex flex-col gap-4">
              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">¿Documentos Correctos? *</label>
                <SelectButton
                  value={form.documentos_correctos}
                  options={documentosCorrectosOptions}
                  optionLabel="label"
                  optionValue="value"
                  optionDisabled="disabled"
                  onChange={(e) => handleDocumentosCorrectosChange(e.value ?? null)}
                  disabled={isDisabled}
                  className={isInvalid('documentos_correctos') ? 'p-invalid' : ''}
                />
                {isInvalid('documentos_correctos') && (
                  <small className="text-red-600">Campo obligatorio</small>
                )}
                {!documentosObligatorios.completos && (
                  <small className="text-yellow-700">
                    No puede marcar "Sí" mientras falten documentos obligatorios en el expediente.
                  </small>
                )}
              </div>

              {form.documentos_correctos === false && (
                <div className="flex flex-col gap-1">
                  <label className="font-semibold text-sm">Motivo de Devolución *</label>
                  <Dropdown
                    value={form.motivo_devolucion}
                    options={motivoDevolucionOptions}
                    optionLabel="description"
                    optionValue="code"
                    onChange={(e) => updateField('motivo_devolucion', e.value as string)}
                    placeholder="Seleccione un motivo..."
                    className={`form-dropdown-presto w-full${isInvalid('motivo_devolucion') ? ' p-invalid' : ''}`}
                    disabled={isDisabled}
                    showClear
                  />
                  {isInvalid('motivo_devolucion') && (
                    <small className="text-red-600">
                      Campo obligatorio cuando los documentos no son correctos
                    </small>
                  )}
                </div>
              )}

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div className="flex flex-col gap-1">
                  <label className="font-semibold text-sm">¿Requiere Actualización de Avalúos?</label>
                  <Dropdown
                    value={form.requiere_actualizacion_avaluos}
                    options={[
                      { label: 'Sí', value: 'Si' },
                      { label: 'No', value: 'No' },
                    ]}
                    onChange={(e) => updateField('requiere_actualizacion_avaluos', e.value)}
                    placeholder="Seleccione..."
                    className="form-dropdown-presto w-full"
                    disabled={isDisabled}
                    showClear
                  />
                </div>

                <div className="flex flex-col gap-1">
                  <label className="font-semibold text-sm">Homologación</label>
                  <Dropdown
                    value={form.homologacion}
                    options={[
                      { label: 'Sí', value: 'Si' },
                      { label: 'No', value: 'No' },
                    ]}
                    onChange={(e) => updateField('homologacion', e.value)}
                    placeholder="Seleccione..."
                    className="form-dropdown-presto w-full"
                    disabled={isDisabled}
                    showClear
                  />
                </div>
              </div>

              <div className="flex flex-col gap-1">
                <label className="font-semibold text-sm">Observaciones</label>
                <InputTextarea
                  value={form.observaciones ?? ''}
                  onChange={(e) => updateField('observaciones', e.target.value)}
                  rows={5}
                  autoResize
                  className="form-textarea-presto w-full"
                  disabled={isDisabled}
                  placeholder="Ingrese observaciones"
                />
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
        </AccordionTab>
      </Accordion>
    </div>
  );
}