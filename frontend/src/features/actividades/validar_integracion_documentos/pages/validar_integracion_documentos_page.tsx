import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { Dialog } from "primereact/dialog";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import ValidarIntegracionForm from "../components/ValidarIntegracionForm";
import FormActions from "../../../../shared/components/FormActions"; // Importación del nuevo componente
import type { ValidarIntegracionDocumentosForm } from "../models/validar_integracion_documentos";
import {
  buildInitialValidarIntegracion,
  normalizeValidarIntegracion,
  toValidarIntegracionSavePayload,
  VALIDAR_INTEGRACION_ACTIVITY_ID,
} from "../models/validar_integracion_documentos.form";
import { useValidarIntegracionDocumentos } from "../hooks/useValidarIntegracionDocumentos";
import { useUpsertValidarIntegracionDocumentos } from "../hooks/useUpsertValidarIntegracionDocumentos";
import { useAvanzarValidarIntegracionDocumentos } from "../hooks/useAvanzarValidarIntegracionDocumentos";
import DatosCreditoSection from "@/features/actividades/validar_integracion_documentos/components/DatosCreditoSection";
import { useControlesValidarIntegracion } from "../hooks/useControlesValidarIntegracion";
import { DatosCreditoControles, EMPTY_CONTROLES_VALIDAR_INTEGRACION, ValidarIntegracionFormControles } from "../models/catalogo";
import CondicionesFinancierasSection from "../components/CondicionesFinancierasSection";
import DatosInmuebleSection from "../components/DatosInmuebleSection";
import DatosTitularSection from "../components/DatosTitularSection";
import RegistroContactoSection from "../components/RegistroContactoSection";
import { getErrorMessage } from "@/core/errors/getErrorMessage";

const ACTIVITY_ID = VALIDAR_INTEGRACION_ACTIVITY_ID;

export default function ValidarIntegracionDocumentosPage() {
  
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<ValidarIntegracionDocumentosForm>(buildInitialValidarIntegracion(id_expediente));
  const [errorMessage, setErrorMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const [showIntervinienteModal, setShowIntervinienteModal] = useState(false);
  const [intervinienteForm, setIntervinienteForm] = useState({
    tipo_id_t1: "RFC-TITULAR-T1",
    nombre_completo_t1: "Titular Principal Resguardado",
    nombre_interviniente: "",
    parentesco_auxiliar: ""
  });

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useValidarIntegracionDocumentos(id_expediente);

  const { data: controlesData } = useControlesValidarIntegracion(id_expediente);
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VALIDAR_INTEGRACION;

  const controlesValidarInformacionForm: ValidarIntegracionFormControles = {
    motivo_devolucion: controles.motivo_devolucion ?? []
  }

  const controlesDatosCreditoForm: DatosCreditoControles = {
    tipo_credito: controles.motivo_devolucion ?? []
  }

  const saveMutation = useUpsertValidarIntegracionDocumentos();
  const avanzarMutation = useAvanzarValidarIntegracionDocumentos();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialValidarIntegracion(id_expediente));
      setErrorMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current) return;

    if (data?.status && data.detail) {
      const loadedEntity = normalizeValidarIntegracion(data.detail, id_expediente);
      setForm(loadedEntity);
      setIsDisabled(Number(loadedEntity.actividad.id) > 0);
      setCanAdvance(Number(loadedEntity.actividad.id) > 0);
      hasHydratedRef.current = true;
      return;
    }

    if (!id_expediente || id_expediente <= 0) {
      setForm(buildInitialValidarIntegracion(0));
      setIsDisabled(false);
      setCanAdvance(false);
      hasHydratedRef.current = true;
      return;
    }

    if (data) {
      setForm((prev) => normalizeValidarIntegracion({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  const updateFormCoreField = <K extends keyof typeof form.actividad>(
    field: K,
    value: (typeof form.actividad)[K]
  ) => {
    setForm((prev) => ({
      ...prev,
      actividad: {
        ...prev.actividad,
        [field]: value,
        ...(field === "documentosCorrectos" && value === true ? { motivo_devolucion: "" } : {})
      }
    }));
  };

  const updateSharedSectionField = (section: 'datosCredito', field: string, value: unknown) => {
    setForm((prev) => ({
      ...prev,
      [section]: {
        ...prev[section],
        [field]: value
      }
    }));
  };

  const handleEditar = () => {
    setErrorMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  const validateForm = () => {
    const core = form.actividad;
    if (!core.idExpediente || core.idExpediente <= 0) {
      return "No existe un id_expediente válido.";
    }
    if (core.documentosCorrectos === null) {
      return "Debe seleccionar si los documentos son correctos.";
    }
    if (core.documentosCorrectos === false && !core.motivoDevolucion) {
      return "El motivo de devolución es requerido cuando los documentos no son correctos.";
    }
    if (!core.observaciones || core.observaciones.trim() === "") {
      return "Las observaciones son obligatorias.";
    }
    return "";
  };

  const handleGuardar = async () => {
    setErrorMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      toast.current?.show({ severity: "warn", summary: "Validación", detail: validationMessage, life: 3000 });
      return;
    }

    try {
      setIsBusy(true);
      // Aquí el payload toma todo el estado 'form' unificado (con datos de crédito incluidos)
      const savePayload = toValidarIntegracionSavePayload(
        form,
        id_expediente,
      );

      const response = await saveMutation.mutateAsync(savePayload);

      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Validación de integración guardada correctamente.", life: 3000 });
        const savedEntity = normalizeValidarIntegracion(response.detail ?? savePayload, savePayload.idExpediente);
        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
      } else {
        toast.current?.show({ severity: "warn", summary: "Atención", detail: response.message || "No se pudo guardar", life: 3000 });
      }
    } catch {
      toast.current?.show({ severity: "error", summary: "Error", detail: "Ocurrió un error al guardar", life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzar = async () => {
    setErrorMessage("");

    const expedienteId = Number(form.actividad.idExpediente ?? 0);

    if (!expedienteId || expedienteId < 0) {
      const msg = "No existe un id_expediente válido para avanzar.";
      setErrorMessage(msg);
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: msg,
        life: 3000,
      });
      return;
    }

    const validationMessage = validateForm();
    if (validationMessage) {
      setErrorMessage(validationMessage);
      return;
    }

    try {
      setIsBusy(true);
      const response = await avanzarMutation.mutateAsync(Number(form.actividad.idExpediente));

      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Actividad avanzada correctamente", life: 2000 });
        navigate("/home/bandeja");
      } else {
        const warningMsg = response.message || "No se pudo avanzar la actividad.";
        setErrorMessage(warningMsg);
        toast.current?.show({ severity: "warn", summary: "Atención", detail: warningMsg, life: 3000 });
      }
    } catch (error: unknown) {
      const errorMsg = getErrorMessage(error);
      setErrorMessage(errorMsg);
      toast.current?.show({ severity: "error", summary: "Error", detail: errorMsg, life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  const handleGuardarInterviniente = () => {
    toast.current?.show({ severity: "success", summary: "Éxito", detail: "Participante auxiliar adicionado correctamente.", life: 3000 });
    setShowIntervinienteModal(false);
    setIntervinienteForm(prev => ({ ...prev, nombre_interviniente: "", parentesco_auxiliar: "" }));
  };

  return (
    <>
      <Toast ref={toast} />

      <div className="w-full mb-6">
        <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="md:col-span-2 flex flex-col">
          
          {isLoading && (
            <div className="text-sm text-blue-600">Cargando información...</div>
          )}

          {errorMessage && (
            <div className="rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
              {errorMessage}
            </div>
          )}

          <Accordion activeIndex={[0, 5]} multiple>
            <AccordionTab header="Datos Titular">
              <DatosTitularSection />
            </AccordionTab>

            <AccordionTab header="Datos Inmueble">
              <DatosInmuebleSection />
            </AccordionTab>

            <AccordionTab header="Datos Crédito">
              <DatosCreditoSection
                data={form.datos_credito}
                encabezado={encabezado}
                controles={controlesDatosCreditoForm}
                isEditing={!isDisabled}
                onChange={(field, val) => updateSharedSectionField('datosCredito', field, val)}
              />
            </AccordionTab>

            <AccordionTab header="Condiciones Financieras">
              <CondicionesFinancierasSection />
            </AccordionTab>

            <AccordionTab header="Registro Contacto">
              <RegistroContactoSection />
            </AccordionTab>

            <AccordionTab header="Validar Integración de Documentos">
              <ValidarIntegracionForm
                  form={form.actividad}
                  isDisabled={isDisabled}
                  updateField={updateFormCoreField}
                  onOpenModal={() => setShowIntervinienteModal(true)}
                  controles={controlesValidarInformacionForm}
                />
            </AccordionTab>
          </Accordion>

          {/* Se incorporan las acciones unificadas debajo de todo el grupo de pestañas */}
          <div className="p-card shadow-none border-none border-0 bg-transparent">
            <div className="p-card-body p-0">
              <FormActions 
                isDisabled={isDisabled}
                canAdvance={canAdvance}
                isBusy={isBusy}
                saveIsPending={saveMutation.isPending}
                avanzarIsPending={avanzarMutation.isPending}
                onEditar={handleEditar}
                onGuardar={handleGuardar}
                onAvanzar={handleAvanzar}
                onSalir={handleSalir}
              />
            </div>
          </div>
        </div>

        <div className="md:col-span-1">
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </div>
      </div>

      {/* Modal / Dialog de Interviniente */}
      <Dialog
        header="Adicionar Interviniente / Apoderado"
        visible={showIntervinienteModal}
        style={{ width: '450px' }}
        modal
        onHide={() => setShowIntervinienteModal(false)}
        footer={
          <div>
            <Button label="Cancelar" icon="pi pi-times" className="p-button-text text-gray-500" onClick={() => setShowIntervinienteModal(false)} />
            <Button label="Registrar" icon="pi pi-check" severity="success" onClick={handleGuardarInterviniente} />
          </div>
        }
      >
        <div className="flex flex-col gap-4 mt-2">
          <div className="flex flex-col gap-1">
            <label className="text-xs font-semibold text-gray-500">ID Titular Principal (T1)</label>
            <InputText value={intervinienteForm.tipo_id_t1} disabled className="bg-gray-100" />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-xs font-semibold text-gray-500">Nombre Completo Titular (T1)</label>
            <InputText value={intervinienteForm.nombre_completo_t1} disabled className="bg-gray-100" />
          </div>

          <hr className="border-gray-200" />

          <div className="flex flex-col gap-1">
            <label className="text-sm font-semibold">Nombre del Participante Auxiliar *</label>
            <InputText
              value={intervinienteForm.nombre_interviniente}
              onChange={(e) => setIntervinienteForm({ ...intervinienteForm, nombre_interviniente: e.target.value })}
              placeholder="Nombre completo"
            />
          </div>
          <div className="flex flex-col gap-1">
            <label className="text-sm font-semibold">Parentesco / Relación *</label>
            <InputText
              value={intervinienteForm.parentesco_auxiliar}
              onChange={(e) => setIntervinienteForm({ ...intervinienteForm, parentesco_auxiliar: e.target.value })}
              placeholder="Ej. Apoderado legal, Aval"
            />
          </div>
        </div>
      </Dialog>
    </>
  );
}