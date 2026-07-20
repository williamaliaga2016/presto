import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Card } from "primereact/card";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { confirmDialog, ConfirmDialog } from "primereact/confirmdialog";
import { InputTextarea } from "primereact/inputtextarea";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import DevolucionVbComercialForm from "../components/DevolucionVbComercialForm";
import type { DevolucionVbComercialForm as DevolucionVbComercialFormModel } from "../models/devolucion_vb_comercial";
import {
  buildInitialDevolucionVbComercial,
  DEVOLUCION_VB_COMERCIAL_ACTIVITY_ID,
  normalizeDevolucionVbComercial,
  toDevolucionVbComercialSavePayload,
} from "../models/devolucion_vb_comercial.form";
import { getErrorMessage } from "@/core/errors/getErrorMessage";
import { useDevolucionVbComercial } from "../hooks/useDevolucionVbComercial";
import { useUpsertDevolucionVbComercial } from "../hooks/useUpsertDevolucionVbComercial";
import { useAvanzarDevolucionVbComercial } from "../hooks/useAvanzarDevolucionVbComercial";
import FormActions from "@/shared/components/FormActions";
import { useControlesDevolucionVbComercial } from "../hooks/useControlesDevolucionVbComercial";
import { DevolucionVbComercialFormControles, EMPTY_CONTROLES_DEVOLUCION_VB_COMERCIAL } from "../models/catalogos";
import CondicionesFinancierasSection from "../components/CondicionesFinancierasSection";
import DatosInmuebleSection from "../components/DatosInmuebleSection";
import DatosTitularSection from "../components/DatosTitularSection";
import RegistroContactoSection from "../components/RegistroContactoSection";
import DatosCreditoSection from "../components/DatosCreditoSection";

const ACTIVITY_ID = DEVOLUCION_VB_COMERCIAL_ACTIVITY_ID;

export default function DevolucionVbComercialPage() {
  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  const [form, setForm] = useState<DevolucionVbComercialFormModel>(buildInitialDevolucionVbComercial(id_expediente));
  const [errorMessage, setErrorMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  const { data, isLoading } = useDevolucionVbComercial(id_expediente);

  const { data: controlesData } = useControlesDevolucionVbComercial();
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_DEVOLUCION_VB_COMERCIAL;

  const controlesDevolucionForm: DevolucionVbComercialFormControles = {
    motivo_cierre: controles.motivo_cierre ?? []
  };

  const saveMutation = useUpsertDevolucionVbComercial();
  const avanzarMutation = useAvanzarDevolucionVbComercial();

  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialDevolucionVbComercial(id_expediente));
      setErrorMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  useEffect(() => {
    if (hasHydratedRef.current || isLoading) return;

    // Existe registro en BD
    if (data?.status && data.detail) {
      const loadedEntity = normalizeDevolucionVbComercial(
        data.detail,
        id_expediente
      );

      setForm(loadedEntity);
      setIsDisabled(loadedEntity.actividad.id > 0);
      setCanAdvance(loadedEntity.actividad.id > 0);
    } else {
      // No existe registro en BD
      setForm(buildInitialDevolucionVbComercial(id_expediente));
      setIsDisabled(false);
      setCanAdvance(false);
    }

    hasHydratedRef.current = true;
  }, [data, id_expediente, isLoading]);

  const updateFormCoreField = <K extends keyof typeof form.actividad>(
    field: K,
    value: (typeof form.actividad)[K]
  ) => {
    setForm((prev) => ({
      ...prev,
      actividad: {
        ...prev.actividad,
        [field]: value,
        ...(field === "clienteDesiste" && value === false ? { motivoCierre: "" } : {})
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
    if (core.clienteDesiste === null) {
      return "Debe seleccionar si el cliente desiste o no.";
    }
    if (core.clienteDesiste === true && !core.motivoCierre) {
      return "El motivo de cierre es requerido cuando el cliente desiste.";
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
      toast.current?.show({
        severity: "warn",
        summary: "Validación",
        detail: validationMessage,
        life: 3000
      });
      return;
    }

    try {
      setIsBusy(true);

      const savePayload = toDevolucionVbComercialSavePayload(form, id_expediente);

      const response = await saveMutation.mutateAsync(savePayload);

      if (response.status) {
        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Devolución comercial guardada correctamente.",
          life: 3000
        });

        const savedEntity = normalizeDevolucionVbComercial(
          response.detail ?? savePayload,
          id_expediente
        );

        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
      } else {
        toast.current?.show({
          severity: "warn",
          summary: "Atención",
          detail: response.message || "No se pudo guardar",
          life: 3000
        });
      }
    } catch {
      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "Ocurrió un error al guardar",
        life: 3000
      });
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzarProceso = async (confirmadoBackend: boolean) => {
    try {
      setIsBusy(true);
      const res = await avanzarMutation.mutateAsync({
        id_expediente,
        confirmarCierre: confirmadoBackend
      });
      if (res.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Actividad avanzada correctamente", life: 2000 });
        navigate("/home/bandeja");
      } else {
        setErrorMessage(res.message || "No se pudo avanzar la actividad.");
      }
    } catch (error: unknown) {
      setErrorMessage(getErrorMessage(error));
    } finally {
      setIsBusy(false);
    }
  };

  const handleAvanzarClick = () => {
    setErrorMessage("");
    const validationMessage = validateForm();
    if (validationMessage) {
      setErrorMessage(validationMessage);
      return;
    }

    if (form.actividad.clienteDesiste === true) {
      confirmDialog({
        group: "devolucion-vb-comercial",
        message: `¿Estás seguro de dar cierre al Folio [${id_expediente}]? Ten en cuenta que el avance del Folio no podrá ser recuperado.`,
        header: 'Confirmación de Cierre Irreversible',
        icon: 'pi pi-exclamation-triangle',
        acceptLabel: 'SÍ',
        rejectLabel: 'NO',
        accept: () => handleAvanzarProceso(true),
        reject: () => { }
      });
    } else {
      handleAvanzarProceso(false);
    }
  };

  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>
      <Toast ref={toast} />
      <ConfirmDialog group="devolucion-vb-comercial"/>

      <div className="w-full mb-6">
        <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="md:col-span-2 flex flex-col">

          {isLoading && (
            <div className="text-sm text-blue-600 mb-2">Cargando información...</div>
          )}

          {errorMessage && (
            <div className="rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700 mb-2">
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
              <DatosCreditoSection />
            </AccordionTab>

            <AccordionTab header="Condiciones Financieras">
              <CondicionesFinancierasSection />
            </AccordionTab>

            {/* Accordion Editable según HU: Registro Contacto Comercial */}
            <AccordionTab header="Registro Contacto">
              <RegistroContactoSection />
            </AccordionTab>

            {/* Sección Dumb inyectada en la última pestaña unificada */}
            <AccordionTab header="Realizar Devolución Pendiente VB Comercial">
              <DevolucionVbComercialForm
                form={form.actividad}
                isDisabled={isDisabled}
                updateField={updateFormCoreField}
                controles={controlesDevolucionForm}
              />
            </AccordionTab>
          </Accordion>

          {/* Acciones unificadas inferiores de formulario */}
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
                onAvanzar={handleAvanzarClick}
                onSalir={handleSalir}
              />
            </div>
          </div>
        </div>

        <div className="md:col-span-1">
          <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
        </div>
      </div>
    </>
  );
}