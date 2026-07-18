import { useEffect, useMemo, useRef, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Accordion, AccordionTab } from 'primereact/accordion';
import { Button } from 'primereact/button';
import { Card } from 'primereact/card';
import { Checkbox } from 'primereact/checkbox';
import { Dialog } from 'primereact/dialog';
import { InputNumber } from 'primereact/inputnumber';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { RadioButton } from 'primereact/radiobutton';
import { Toast } from 'primereact/toast';
import type { Toast as ToastRef } from 'primereact/toast';
import FuncionesTransversales from '@/features/funciones_transversales/pages/FuncionesTransversales';
import DatosTitularSection from '../../validar_informacion/components/DatosTitularSection';
import DatosInmuebleSection from '../../validar_informacion/components/DatosInmuebleSection';
import DatosCreditoSection from '../../validar_informacion/components/DatosCreditoSection';
import CondicionesFinancierasSection from '../../validar_informacion/components/CondicionesFinancierasSection';
import RegistroContactoSection from '../../validar_informacion/components/RegistroContactoSection';
import { EMPTY_CONTROLES_VALIDAR_INFORMACION } from '../../validar_informacion/models/catalogo';
import {
  EMPTY_ENCABEZADO_VALIDAR_INFORMACION,
  type EncabezadoValidarInformacion,
} from '../../validar_informacion/models/encabezado_validar_informacion';
import {
  EMPTY_VALIDAR_INFORMACION,
  type ValidarInformacionBBVA,
} from '../../validar_informacion/models/validar_informacion';
import {
  useAsignarFirmas,
  useAvanzarAsignarFirmas,
  useCalcularAsignacion,
  useControlesAsignarFirmas,
  useGuardarAsignarFirmas,
} from '../hooks/useAsignarFirmas';
import {
  EMPTY_ASIGNAR_FIRMAS,
  RESULTADO_FIELDS,
  type AsignarFirmasForm,
  type DatosFolioAsignacion,
} from '../models/asignarFirmas';

const ACTIVIDAD_ID = 'ACT_ASIGNAR_FIRMAS';
const MANUAL_FIELDS: (keyof AsignarFirmasForm)[] = [
  'tipo_cliente', 'codigo_ejecutivo_solicitante', 'oficina_solicitante',
  'tipo_solicitud_avaluo', 'tipo_tramite_eett',
];
const inputClass = 'form-input-presto w-full';
const textareaClass = 'form-textarea-presto w-full';

const folioLabels: [keyof DatosFolioAsignacion, string][] = [
  ['fecha_asignacion', 'Fecha de Asignación'],
  ['tipo_inmueble', 'Tipo de Inmueble'],
  ['constructora', 'Constructora'],
  ['proyecto', 'Proyecto'],
  ['identificacion_cliente', 'Identificación Cliente'],
  ['nombre_cliente', 'Nombre Cliente'],
  ['departamento_predio', 'Departamento del Predio'],
  ['ciudad_predio', 'Ciudad del Predio'],
  ['direccion_predio', 'Dirección del Predio'],
  ['ubicacion_predio', 'Ubicación del Predio'],
  ['valor_comercial_predio', 'Valor Comercial del Predio'],
  ['usuario_solicitante', 'Usuario Solicitante'],
];

const resultGroups: { title: string; fields: [keyof AsignarFirmasForm, string, boolean?][] }[] = [
  {
    title: 'Firma / Perito',
    fields: [
      ['nombre_firma_supervisor', 'Nombre Firma / Supervisor'], ['telefono_firma', 'Teléfono'],
      ['email_firma', 'Email'], ['valor_avaluo', 'Valor Avalúo', true],
      ['valor_total_consignar', 'Valor Total a Consignar', true],
      ['opciones_recaudo', 'Opciones de Recaudo'], ['numero_recaudo', 'Número de Recaudo'],
      ['banco', 'Banco'],
    ],
  },
  {
    title: 'Abogado',
    fields: [
      ['nombre_abogado', 'Nombre Abogado'], ['telefono_abogado', 'Teléfono'],
      ['valor_estudio_titulos', 'Valor Estudio de Títulos', true],
      ['tipo_cuenta_abogado', 'Tipo de Cuenta'], ['numero_cuenta_abogado', 'Número de Cuenta'],
    ],
  },
];

function displayValue(value: unknown, key: keyof DatosFolioAsignacion) {
  if (value === null || value === undefined || value === '') return '—';
  if (key === 'valor_comercial_predio' && typeof value === 'number') {
    return value.toLocaleString('es-CO', { style: 'currency', currency: 'COP' });
  }
  if (key === 'fecha_asignacion') return new Date(String(value)).toLocaleDateString('es-CO');
  return String(value);
}

const normalizeHeaderValue = (value: unknown) => {
  if (value === null || value === undefined || String(value).trim() === '') return '-';
  return String(value);
};

const formatHeaderDate = (value?: string | null) => {
  if (!value) return '-';
  const date = new Date(value);
  if (Number.isNaN(date.getTime())) return value;
  return date.toLocaleDateString('es-CO');
};

const formatHeaderMoney = (value?: number | null) => {
  if (value === null || value === undefined) return '-';
  return value.toLocaleString('es-CO', { style: 'currency', currency: 'COP' });
};

function HeaderItem({ label, value }: { label: string; value?: unknown }) {
  return (
    <div className="flex flex-col gap-1">
      <span className="text-xs font-semibold uppercase tracking-wide text-gray-500">
        {label}
      </span>
      <span className="min-h-[20px] text-sm font-medium text-gray-800">
        {normalizeHeaderValue(value)}
      </span>
    </div>
  );
}

function AsignarFirmasHeader({
  id,
  form,
  heredados,
  encabezado,
  datosFolio,
}: {
  id: number;
  form: AsignarFirmasForm;
  heredados: ValidarInformacionBBVA;
  encabezado: EncabezadoValidarInformacion;
  datosFolio?: DatosFolioAsignacion | null;
}) {
  const usuarioAsignado = datosFolio?.usuario_solicitante
    || encabezado.codigo_asesor
    || form.codigo_ejecutivo_solicitante;
  const proyecto = datosFolio?.proyecto || heredados.descripcion_proyecto;
  const constructoraProyecto = [datosFolio?.constructora || heredados.constructora, proyecto]
    .filter(Boolean)
    .join(' / ');

  return (
    <Card className="w-full shadow-md card-presto-form mb-6">
      <div className="mb-4">
        <p className="text-sm text-gray-500">
          Datos informativos de la actividad actual.
        </p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-5">
        <HeaderItem label="Id Expediente" value={id} />
        <HeaderItem label="Actividad" value="Asignar Firmas, Peritos y Abogados" />
        <HeaderItem label="Estado" value={form.id ? 'En progreso' : 'Nueva'} />
        <HeaderItem label="Usuario Asignado" value={usuarioAsignado} />

        <HeaderItem label="Fecha Alta" value={formatHeaderDate(heredados.created_date)} />
        <HeaderItem label="Fecha Asignación" value={formatHeaderDate(datosFolio?.fecha_asignacion)} />
        <HeaderItem label="N° Identificación" value={datosFolio?.identificacion_cliente || heredados.numero_id_t1} />
        <HeaderItem label="ID Scoring" value={encabezado.scoring} />

        <HeaderItem label="Tipo de Crédito" value={heredados.tipo_credito} />
        <HeaderItem label="Nro. Registro" value="0" />
        <HeaderItem label="Segmento" value={encabezado.id_tipo_sub_producto} />
        <HeaderItem label="Canal de Originación" value={heredados.estatus_general} />

        <HeaderItem label="Constructora / Proyecto" value={constructoraProyecto} />
        <HeaderItem label="Propietario" value={datosFolio?.nombre_cliente || heredados.nombre_completo_t1} />
        <HeaderItem label="Código Subproducto" value={encabezado.id_tipo_sub_producto} />
        <HeaderItem label="Subproducto BBVA" value={encabezado.id_tipo_sub_producto} />

        <HeaderItem label="Tipo de Tasa" value={encabezado.condiciones_organismo_decisor} />
        <HeaderItem label="Tasa" value={encabezado.tasa} />
        <HeaderItem label="Moneda" value="COP" />
        <HeaderItem label="Variabilidad Tasa" value={encabezado.condiciones_organismo_decisor} />

        <HeaderItem
          label="Plazo"
          value={encabezado.plazo_meses !== null && encabezado.plazo_meses !== undefined
            ? `${encabezado.plazo_meses} meses`
            : null}
        />
        <HeaderItem label="Comisión" value="0" />
        <HeaderItem label="Monto Nominal" value={formatHeaderMoney(encabezado.monto_otorgado_original)} />
        <HeaderItem label="Monto Solicitado" value={formatHeaderMoney(heredados.monto_otorgado_vi)} />

        <HeaderItem label="Banco Alzante" value={null} />
        <HeaderItem label="N° Op. Cartera" value="0" />
        <HeaderItem label="Modelo de Operación" value={null} />
        <HeaderItem label="Tipo Carpeta" value={null} />

        <HeaderItem label="Fecha Inicio" value={null} />
        <HeaderItem label="Monto Residual" value="0" />
        <HeaderItem label="Tasa 2° Periodo" value="0" />
        <HeaderItem label="Código Producto Cartera" value={null} />

        <HeaderItem label="Plazo 1° Periodo" value="0" />
        <HeaderItem label="Tasa 1° Periodo" value="0" />
        <HeaderItem label="Tipo Tasa Mixta Prod. Com." value={null} />
        <HeaderItem label="Tasa Máxima" value="0" />

        <HeaderItem label="Periodo Gracia" value="0" />
        <HeaderItem label="Tipo Tasa Aplic. Contab." value={null} />
        <HeaderItem label="Plazo 2° Periodo" value="0" />
        <HeaderItem label="Destino Crédito" value={null} />

        <HeaderItem label="Indicador 2° Vivienda" value={null} />
        <HeaderItem label="Precio Venta Moneda Original" value={null} />
        <HeaderItem label="Tipo de Financiamiento" value={null} />
        <HeaderItem label="Precio Venta en Pesos" value="0" />

        <HeaderItem label="Cantidad Meses sin Vencimiento" value="0" />
        <HeaderItem label="Indicador PAC" value={null} />
        <HeaderItem label="Indicador Cred. Comp" value="0" />
        <HeaderItem label="Número Cuenta Gastos" value="0" />

        <HeaderItem label="Nombre de Proyecto" value={proyecto} />
        <HeaderItem label="Préstamo Máximo" value="0" />
        <HeaderItem label="Nro Piloto" value="0" />
        <HeaderItem label="Valor Comercial Predio" value={formatHeaderMoney(datosFolio?.valor_comercial_predio)} />

        <HeaderItem label="Departamento Predio" value={datosFolio?.departamento_predio || heredados.departamento_inmueble} />
        <HeaderItem label="Ciudad Predio" value={datosFolio?.ciudad_predio || heredados.municipio_inmueble} />
        <HeaderItem label="Ubicación Predio" value={datosFolio?.ubicacion_predio || heredados.estado_inmueble} />
        <HeaderItem label="Tipo Inmueble" value={datosFolio?.tipo_inmueble || heredados.tipo_inmueble} />
      </div>
    </Card>
  );
}

export default function AsignarFirmasPage() {
  const { id_expediente: param } = useParams();
  const id = Number(param ?? 0);
  const toast = useRef<ToastRef>(null);
  const [form, setForm] = useState<AsignarFirmasForm>(EMPTY_ASIGNAR_FIRMAS(id));
  const [dirty, setDirty] = useState(false);
  const [invalid, setInvalid] = useState<Set<string>>(new Set());
  const [accessLink, setAccessLink] = useState<string | null>(null);
  const { data, isLoading, error } = useAsignarFirmas(id);
  const { data: controlesData } = useControlesAsignarFirmas(id);
  const guardar = useGuardarAsignarFirmas();
  const calcular = useCalcularAsignacion(id);
  const avanzar = useAvanzarAsignarFirmas();

  const detail = data?.detail;
  const heredados = detail?.datos_heredados ?? EMPTY_VALIDAR_INFORMACION(id);
  const encabezado = detail?.encabezado ?? EMPTY_ENCABEZADO_VALIDAR_INFORMACION;
  const controles = controlesData?.detail ?? {
    ...EMPTY_CONTROLES_VALIDAR_INFORMACION,
    documentos_solicitar: [],
  };

  useEffect(() => {
    if (!detail) return;
    const incoming = detail.formulario ?? EMPTY_ASIGNAR_FIRMAS(id);
    setForm({
      ...incoming,
      checklist_documentos_solicitar: incoming.checklist_documentos_solicitar ?? [],
      codigo_ejecutivo_solicitante:
        incoming.codigo_ejecutivo_solicitante || detail.encabezado?.codigo_asesor || '',
      oficina_solicitante:
        incoming.oficina_solicitante || detail.encabezado?.codigo_oficina || '',
      tipo_solicitud_avaluo:
        incoming.tipo_solicitud_avaluo || detail.datos_heredados?.tipo_credito || '',
      tipo_tramite_eett:
        incoming.tipo_tramite_eett || detail.datos_heredados?.tipo_credito || '',
    });
    setDirty(false);
  }, [detail, id]);

  const hasResults = useMemo(
    () => Boolean(form.nombre_firma_supervisor && form.nombre_abogado),
    [form.nombre_firma_supervisor, form.nombre_abogado],
  );
  const canCalculate = MANUAL_FIELDS.every((field) => String(form[field] ?? '').trim());

  const update = (field: keyof AsignarFirmasForm, value: unknown) => {
    setForm((current) => {
      const next = { ...current, [field]: value };
      if (MANUAL_FIELDS.includes(field) && RESULTADO_FIELDS.some((key) => current[key] != null)) {
        RESULTADO_FIELDS.forEach((key) => { next[key] = null; });
      }
      if (field === 'requiere_envio_notificacion' && value === false) {
        next.checklist_documentos_solicitar = [];
      }
      return next;
    });
    setDirty(true);
    setInvalid((current) => {
      const next = new Set(current);
      next.delete(String(field));
      return next;
    });
  };

  const showError = (summary: string, caught: Error) => toast.current?.show({
    severity: 'error', summary, detail: caught.message, life: 5000,
  });

  const handleCalcular = () => {
    calcular.mutate({
      tipo_cliente: form.tipo_cliente!,
      codigo_ejecutivo: form.codigo_ejecutivo_solicitante!,
      oficina: form.oficina_solicitante!,
      tipo_solicitud_avaluo: form.tipo_solicitud_avaluo!,
      tipo_tramite_eett: form.tipo_tramite_eett!,
      id_expediente: id,
    }, {
      onSuccess: (response) => {
        setForm((current) => ({ ...current, ...response.detail }));
        setDirty(true);
      },
      onError: (caught) => showError('Error al calcular asignación', caught),
    });
  };

  const handleGuardar = () => guardar.mutate(form, {
    onSuccess: () => {
      setDirty(false);
      toast.current?.show({ severity: 'success', summary: 'Guardado', detail: 'Información guardada correctamente', life: 3000 });
    },
    onError: (caught) => showError('Error al guardar', caught),
  });

  const handleAvanzar = () => {
    const missing = new Set<string>();
    MANUAL_FIELDS.forEach((field) => { if (!String(form[field] ?? '').trim()) missing.add(String(field)); });
    if (!hasResults) missing.add('resultado_asignacion');
    if (form.requiere_envio_notificacion === null || form.requiere_envio_notificacion === undefined) {
      missing.add('requiere_envio_notificacion');
    }
    if (form.requiere_envio_notificacion && form.checklist_documentos_solicitar.length === 0) {
      missing.add('checklist_documentos_solicitar');
    }
    if (!form.observaciones?.trim()) missing.add('observaciones');
    if (missing.size) {
      setInvalid(missing);
      toast.current?.show({ severity: 'warn', summary: 'Datos Obligatorios Faltantes', detail: !hasResults ? 'Debe calcular la asignación antes de avanzar.' : 'Complete los campos resaltados.', life: 5000 });
      return;
    }
    avanzar.mutate(id, {
      onSuccess: (response) => {
        const temporalUrl = response.detail?.acceso_temporal?.url;
        toast.current?.show({
          severity: 'success',
          summary: 'Grabado correctamente',
          detail: 'La actividad avanzó correctamente.',
          life: 3000,
        });
        if (temporalUrl) setAccessLink(temporalUrl);
      },
      onError: (caught) => showError('Error al avanzar', caught),
    });
  };

  const handleCopyAccessLink = async () => {
    if (!accessLink) return;

    try {
      await navigator.clipboard.writeText(accessLink);
      toast.current?.show({
        severity: 'success',
        summary: 'Link copiado',
        detail: 'El link de acceso temporal fue copiado.',
        life: 3000,
      });
    } catch {
      toast.current?.show({
        severity: 'warn',
        summary: 'No fue posible copiar',
        detail: 'Selecciona el link y copialo manualmente.',
        life: 5000,
      });
    }
  };

  const toggleDocumento = (documentoId: string, checked: boolean) => {
    const selected = new Set(form.checklist_documentos_solicitar);
    if (checked) selected.add(documentoId); else selected.delete(documentoId);
    update('checklist_documentos_solicitar', [...selected]);
  };

  if (isLoading) return <div className="p-4">Cargando...</div>;
  if (error) return <div className="p-4 text-red-600">No fue posible cargar la actividad: {error.message}</div>;

  return (
    <div className="flex flex-col gap-4 p-4">
      <Toast ref={toast} />

      <Dialog
        header="Link de acceso temporal"
        visible={Boolean(accessLink)}
        modal
        className="w-[92vw] max-w-2xl"
        onHide={() => setAccessLink(null)}
        footer={(
          <div className="flex justify-end gap-2">
            <Button
              label="Copiar link"
              icon="pi pi-copy"
              onClick={handleCopyAccessLink}
            />
            <Button
              label="Cerrar"
              icon="pi pi-times"
              severity="secondary"
              onClick={() => setAccessLink(null)}
            />
          </div>
        )}
      >
        <div className="flex flex-col gap-3">
          <p className="m-0 text-sm text-gray-700">
            Este es el link de acceso temporal, compartelo con el usuario externo
            para proceder con la carga de documentos y continuar con el proceso.
          </p>
          <InputText
            value={accessLink ?? ''}
            readOnly
            className="w-full"
            onFocus={(event) => event.currentTarget.select()}
          />
        </div>
      </Dialog>

      <Card>
        <Accordion multiple activeIndex={[0]}>
          <AccordionTab header="Información del Expediente">
            <AsignarFirmasHeader
              id={id}
              form={form}
              heredados={heredados}
              encabezado={encabezado}
              datosFolio={detail?.datos_folio}
            />
          </AccordionTab>
        </Accordion>
      </Card>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-4">
        <div className="lg:col-span-2">
          <Card className="w-full shadow-md card-presto-form">
            <Accordion multiple>
              <AccordionTab header="Datos Titular">
                <DatosTitularSection data={heredados} controles={controles} isEditing={false} onChange={() => undefined} />
              </AccordionTab>
              <AccordionTab header="Datos Inmueble">
                <DatosInmuebleSection data={heredados} controles={controles} isEditing={false} onChange={() => undefined} />
              </AccordionTab>
              <AccordionTab header="Datos Crédito">
                <DatosCreditoSection data={heredados} encabezado={encabezado} controles={controles} isEditing={false} onChange={() => undefined} />
              </AccordionTab>
              <AccordionTab header="Condiciones Financieras">
                <CondicionesFinancierasSection data={heredados} encabezado={encabezado} isEditing={false} onChange={() => undefined} />
              </AccordionTab>
              <AccordionTab header="Registro Contacto">
                <RegistroContactoSection id_expediente={id} id_actividad={ACTIVIDAD_ID} controles={controles} />
              </AccordionTab>
              <AccordionTab header="Asignar Firmas, Peritos y Abogados">
                <section className="space-y-6">
                  <fieldset className="border border-gray-200 rounded-md p-4">
                    <legend className="px-2 text-sm font-semibold text-gray-700">
                      DATOS DEL FOLIO
                    </legend>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
                      {folioLabels.map(([key, label]) => (
                        <div key={key} className="rounded border bg-gray-50 px-3 py-2">
                          <div className="text-xs text-gray-500">{label}</div>
                          <div className="text-sm font-medium">{displayValue(detail?.datos_folio?.[key], key)}</div>
                        </div>
                      ))}
                    </div>
                  </fieldset>
                  <fieldset className="border border-gray-200 rounded-md p-4">
                    <legend className="px-2 text-sm font-semibold text-gray-700">
                      DATOS PARA EL MOTOR ASIGNADOR
                    </legend>
                    <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                      {([
                        ['tipo_cliente', 'Tipo de Cliente'], ['codigo_ejecutivo_solicitante', 'Código Ejecutivo Solicitante'],
                        ['oficina_solicitante', 'Oficina Solicitante'], ['tipo_solicitud_avaluo', 'Tipo de Solicitud Avalúo'],
                        ['tipo_tramite_eett', 'Tipo de Trámite EETT'],
                      ] as [keyof AsignarFirmasForm, string][]).map(([field, label]) => (
                        <div key={field} className="flex flex-col gap-1">
                          <label className="font-semibold text-sm">{label} *</label>
                          <InputText
                            value={String(form[field] ?? '')}
                            onChange={(e) => update(field, e.target.value)}
                            className={`${inputClass} ${invalid.has(String(field)) ? 'p-invalid' : ''}`}
                          />
                        </div>
                      ))}
                    </div>
                    <div className="flex justify-end mt-4">
                      <Button label="Calcular Asignación" icon="pi pi-calculator" disabled={!canCalculate} loading={calcular.isPending} onClick={handleCalcular} />
                    </div>
                  </fieldset>
                  {resultGroups.map((group) => (
                    <fieldset key={group.title} className={`border rounded-md p-4 ${invalid.has('resultado_asignacion') ? 'border-red-500' : 'border-gray-200'}`}>
                      <legend className="px-2 text-sm font-semibold text-gray-700">
                        {group.title.toUpperCase()}
                      </legend>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-5">
                        {group.fields.map(([field, label, currency]) => (
                          <div key={field} className="flex flex-col gap-1">
                            <label className="font-semibold text-sm">{label}</label>
                            {currency ? (
                              <InputNumber
                                value={(form[field] as number | null) ?? null}
                                mode="currency"
                                currency="COP"
                                locale="es-CO"
                                disabled
                                className="w-full"
                                inputClassName={inputClass}
                              />
                            ) : (
                              <InputText value={String(form[field] ?? '')} disabled className={inputClass} />
                            )}
                          </div>
                        ))}
                      </div>
                    </fieldset>
                  ))}
                </section>
              </AccordionTab>
              <AccordionTab header="Estatus General">
                <div className="space-y-5">
                  <fieldset className={`border rounded-md p-4 ${invalid.has('requiere_envio_notificacion') ? 'border-red-500' : 'border-gray-200'}`}>
                    <legend className="px-2 text-sm font-semibold text-gray-700">
                      ESTATUS GENERAL
                    </legend>
                    <label className="text-sm font-medium">¿Requiere Envío de Notificación? *</label>
                    <div className="flex gap-5 mt-2">
                      {[true, false].map((value) => <div key={String(value)} className="flex items-center gap-2"><RadioButton className="form-radio-presto" inputId={`notifica-${value}`} checked={form.requiere_envio_notificacion === value} onChange={() => update('requiere_envio_notificacion', value)} /><label htmlFor={`notifica-${value}`}>{value ? 'Sí' : 'No'}</label></div>)}
                    </div>
                  </fieldset>
                  {form.requiere_envio_notificacion && (
                    <fieldset className={`border rounded-md p-4 ${invalid.has('checklist_documentos_solicitar') ? 'border-red-500' : 'border-gray-200'}`}>
                      <legend className="px-2 text-sm font-semibold text-gray-700">
                        DOCUMENTOS A SOLICITAR
                      </legend>
                      <label className="font-semibold text-sm">CheckList Documentos a Solicitar *</label>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-2 mt-2">
                        {controles.documentos_solicitar.map((doc) => <div key={doc.id} className="flex items-center gap-2"><Checkbox className="form-checkbox-presto" inputId={`doc-${doc.id}`} checked={form.checklist_documentos_solicitar.includes(doc.id)} onChange={(e) => toggleDocumento(doc.id, e.checked ?? false)} /><label htmlFor={`doc-${doc.id}`}>{doc.nombre}</label></div>)}
                        {!controles.documentos_solicitar.length && <span className="text-sm text-amber-700">El backend no devolvió documentos configurados.</span>}
                      </div>
                    </fieldset>
                  )}
                  <div className="flex flex-col gap-1">
                    <label className="font-semibold text-sm">Observaciones *</label>
                    <InputTextarea rows={4} value={form.observaciones ?? ''} onChange={(e) => update('observaciones', e.target.value)} className={`${textareaClass} ${invalid.has('observaciones') ? 'p-invalid' : ''}`} />
                  </div>
                </div>
              </AccordionTab>
            </Accordion>
            <div className="form-actions pt-4 border-t">
              <Button label="Guardar" icon="pi pi-save" severity="secondary" loading={guardar.isPending} disabled={!dirty} onClick={handleGuardar} className="btn-responsive" />
              <Button label="Avanzar" icon="pi pi-arrow-right" loading={avanzar.isPending} disabled={dirty} onClick={handleAvanzar} className="btn-responsive" />
            </div>
          </Card>
        </div>
        <div className="lg:col-span-1">
          <FuncionesTransversales idExpediente={id} idActividad={ACTIVIDAD_ID} />
        </div>
      </div>
    </div>
  );
}
