import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { Toast } from "primereact/toast";
import { Dialog } from "primereact/dialog";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import ValidarIntegracionForm from "../components/ValidarIntegracionForm";
import FormActions from "../../../../shared/components/FormActions"; // Importación del nuevo componente
import type { GuardarValidarIntegracionDocumentos, ValidarIntegracionDocumentos } from "../models/validar_integracion_documentos";
import { useValidarIntegracionDocumentos } from "../hooks/useValidarIntegracionDocumentos";
import { useUpsertValidarIntegracionDocumentos } from "../hooks/useUpsertValidarIntegracionDocumentos";
import { useAvanzarValidarIntegracionDocumentos } from "../hooks/useAvanzarValidarIntegracionDocumentos";
import { EMPTY_ENCABEZADO_VALIDAR_INFORMACION } from "../../validar_informacion/models/encabezado_validar_informacion";
import { useControlesValidarIntegracion } from "../hooks/useControlesValidarIntegracion";
import { EMPTY_CONTROLES_VALIDAR_INTEGRACION } from "../models/catalogo";
import DatosTitularSection from "../../validar_informacion/components/DatosTitularSection";
import DatosInmuebleSection from "../../validar_informacion/components/DatosInmuebleSection";
import CondicionesFinancierasSection from "../../validar_informacion/components/CondicionesFinancierasSection";
import DatosCreditoSection from "../../validar_informacion/components/DatosCreditoSection";
import { useAuth } from "@/app/providers/AuthProvider";
import IntervinientesSection from "../components/IntervinientesSection";
import { useIntervinientes } from "../hooks/useIntervinientes";
import { useGuardarInterviniente } from "../hooks/useGuardarInterviniente";
import { Interviniente } from "../models/interviniente";

const ACTIVITY_ID = "ACT_VALIDAR_INTEGRACION";

const buildInitialState = (id_expediente: number): ValidarIntegracionDocumentos => ({
  id_expediente: id_expediente,
  encabezado: {},
  formulario: {
    id: 0,
    id_expediente: id_expediente,
    id_actividad: ACTIVITY_ID,
    documentos_correctos: null,
    credito_condicionado: false,
    motivo_devolucion: "",
    observaciones: "",
  },
  validar_informacion_data: {
    id_expediente: id_expediente,
    tipo_credito: "",
    tiene_garantia: false,
    monto_otorgado_vi: 0,
    id_actividad: "" // REVISAR
  }
});

const normalizeValidarIntegracion = (
  source: ValidarIntegracionDocumentos | null | undefined,
  id_expediente_fallback: number,
): ValidarIntegracionDocumentos => {
  if (!source) return buildInitialState(id_expediente_fallback);

  return {
    id_expediente: source.id_expediente || id_expediente_fallback || 0,
    encabezado: source.encabezado ?? {},
    formulario: {
      id: Number(source.formulario?.id ?? 0),
      id_expediente: Number(source.formulario?.id_expediente || id_expediente_fallback),
      id_actividad: source.formulario?.id_actividad ?? ACTIVITY_ID,
      documentos_correctos: source.formulario?.documentos_correctos ?? true,
      credito_condicionado: source.formulario?.credito_condicionado ?? false,
      motivo_devolucion: source.formulario?.motivo_devolucion ?? "",
      observaciones: source.formulario?.observaciones ?? "",
    },
    validar_informacion_data: source.validar_informacion_data ?? { id_expediente: id_expediente_fallback }
  };
};

export default function ValidarIntegracionDocumentosPage() {

  // CONTROL DE SESION
  const { user } = useAuth();
  const role_name = user?.role_name;

  const toast = useRef<Toast>(null);
  const navigate = useNavigate();
  const { id_expediente: idExpedienteParam } = useParams();
  const id_expediente = Number(idExpedienteParam ?? 0);

  // FORMULARIO
  const [form, setForm] = useState<ValidarIntegracionDocumentos>(buildInitialState(id_expediente));
  const [errorMessage, setErrorMessage] = useState("");
  const [isDisabled, setIsDisabled] = useState(true);
  const [canAdvance, setCanAdvance] = useState(false);
  const [isBusy, setIsBusy] = useState(false);

  // FLAGS
  const isAnalistaViviendaII = ["ANALISTA VIVIENDA II", "ADMINISTRADOR"].includes(user?.role_name?.trim().toUpperCase() || ""); // ADMIN TAMBIEN PUEDE MODIFICAR

  // DATA
  const hasHydratedRef = useRef(false);
  const currentExpedienteRef = useRef<number>(id_expediente);

  // INTERVINIENTES
  const { data: intervinientesData, refetch: refetchIntervinientes } = useIntervinientes(id_expediente);
  const intervinientes = intervinientesData?.detail ?? [];

  // FORMULARIO
  const { data, isLoading } = useValidarIntegracionDocumentos(id_expediente);
  const encabezado = data?.detail?.encabezado ?? EMPTY_ENCABEZADO_VALIDAR_INFORMACION;

  // CONTROLES
  const { data: controlesData } = useControlesValidarIntegracion();
  const controles = controlesData?.detail ?? EMPTY_CONTROLES_VALIDAR_INTEGRACION;

  // MUTACIONES
  const saveMutation = useUpsertValidarIntegracionDocumentos();
  const avanzarMutation = useAvanzarValidarIntegracionDocumentos();
  const guardarIntervinienteMutation = useGuardarInterviniente();

  // CONTROL DE HIDRATACION
  useEffect(() => {
    if (currentExpedienteRef.current !== id_expediente) {
      currentExpedienteRef.current = id_expediente;
      hasHydratedRef.current = false;
      setForm(buildInitialState(id_expediente));
      setErrorMessage("");
      setIsDisabled(true);
      setCanAdvance(false);
    }
  }, [id_expediente]);

  // INICIALIZACION DE FORMULARIO
  useEffect(() => {
    if (hasHydratedRef.current || isLoading) return;

    if (data?.status && data.detail) {
      const loadedEntity = normalizeValidarIntegracion(data.detail, id_expediente);
      setForm(loadedEntity);
      setIsDisabled(Number(loadedEntity.formulario.id) > 0);
      setCanAdvance(Number(loadedEntity.formulario.id) > 0);
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
      setForm((prev) => normalizeValidarIntegracion({ ...prev, id_expediente }, id_expediente));
      setIsDisabled(true);
      setCanAdvance(false);
      hasHydratedRef.current = true;
    }
  }, [data, id_expediente]);

  // ACTUALIZACION DE CAMPO FORMULARIO PRINCIPAL
  const updateFormCoreField = <K extends keyof typeof form.formulario>(
    field: K,
    value: (typeof form.formulario)[K]
  ) => {
    setForm((prev) => ({
      ...prev,
      formulario: {
        ...prev.formulario,
        [field]: value,
        ...(field === "documentos_correctos" && value === true ? { motivo_devolucion: "" } : {})
      }
    }));
  };

  // ACTUALIZACION DE CAMPO FORMULARIO VALIDAR INFORMACION
  const updateValidarInformacionField = <K extends keyof typeof form.validar_informacion_data>(
    field: K,
    value: any
  ) => {
    console.log("field", field)
    console.log("value", value)
    setForm((prev) => ({
      ...prev,
      validar_informacion_data: {
        ...prev.validar_informacion_data,
        [field]: value
      }
    }));
  };

  // MODO EDICION
  const handleEditar = () => {
    setErrorMessage("");
    setIsDisabled(false);
    setCanAdvance(false);
  };

  // VALIDAR FORMULARIO
  const validateForm = () => {
    const core = form.formulario;
    console.log(form);
    console.log(core);
    if (!core.id_expediente || core.id_expediente <= 0) {
      return "No existe un id_expediente válido.";
    }
    if (core.documentos_correctos === null) {
      return "Debe seleccionar si los documentos son correctos.";
    }
    if (core.documentos_correctos === false && !core.motivo_devolucion) {
      return "El motivo de devolución es requerido cuando los documentos no son correctos.";
    }
    if (!core.observaciones || core.observaciones.trim() === "") {
      return "Las observaciones son obligatorias.";
    }
    return "";
  };

  // GUARDAR INTERVINIENTE
  const handleGuardarInterviniente = async (
    interviniente: Interviniente
  ) => {

    try {

      const response = await guardarIntervinienteMutation.mutateAsync({
        id_expediente,
        data: interviniente
      });


      if (response.status) {

        toast.current?.show({
          severity: "success",
          summary: "Éxito",
          detail: "Interviniente registrado correctamente.",
          life: 3000
        });

        await refetchIntervinientes();

      }

    } catch (error) {

      toast.current?.show({
        severity: "error",
        summary: "Error",
        detail: "No fue posible registrar el interviniente.",
        life: 3000
      });

    }
  };

  // GUARDAR DATOS ACTIVIDAD
  const handleGuardar = async () => {
    setErrorMessage("");

    const validationMessage = validateForm();
    if (validationMessage) {
      toast.current?.show({ severity: "warn", summary: "Validación", detail: validationMessage, life: 3000 });
      return;
    }

    try {
      setIsBusy(true);

      const payload = normalizeValidarIntegracion({ ...form, id_expediente }, id_expediente);

      const model: GuardarValidarIntegracionDocumentos = {
        formulario: payload.formulario,
        validar_informacion_data: payload.validar_informacion_data
      }

      const response = await saveMutation.mutateAsync(model);

      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Validación de integración guardada correctamente.", life: 3000 });
        const savedEntity = normalizeValidarIntegracion(response.detail, payload.formulario.id_expediente);
        setForm(savedEntity);
        setIsDisabled(true);
        setCanAdvance(true);
      } else {
        toast.current?.show({ severity: "warn", summary: "Atención", detail: response.message || "No se pudo guardar", life: 3000 });
      }
    } catch (error) {
      console.error("ERROR GUARDAR VALIDAR INTEGRACION", error);
      toast.current?.show({ severity: "error", summary: "Error", detail: "Ocurrió un error al guardar", life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  // AVANZAR ACTIVIDAD
  const handleAvanzar = async () => {
    setErrorMessage("");

    const expedienteId = Number(form.formulario.id_expediente ?? 0);

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
      const response = await avanzarMutation.mutateAsync(Number(form.formulario.id_expediente));

      if (response.status) {
        toast.current?.show({ severity: "success", summary: "Éxito", detail: "Actividad avanzada correctamente", life: 2000 });
        navigate("/home/bandeja");
      } else {
        const warningMsg = response.message || "No se pudo avanzar la actividad.";
        setErrorMessage(warningMsg);
        toast.current?.show({ severity: "warn", summary: "Atención", detail: warningMsg, life: 3000 });
      }
    } catch (error: any) {
      console.error("ERROR AVANZAR VALIDAR INTEGRACION", error);

      const errorMsg = error?.response?.data?.message || "Ocurrió un error al avanzar.";
      setErrorMessage(errorMsg);
      toast.current?.show({ severity: "error", summary: "Error", detail: errorMsg, life: 3000 });
    } finally {
      setIsBusy(false);
    }
  };

  // SALIR A BANDEJA
  const handleSalir = () => {
    navigate("/home/bandeja");
  };

  return (
    <>
      <Toast ref={toast} />

      <h2 className="text-2xl font-bold text-gray-900 mb-6">
        Validar Integración de Documentos
      </h2>

      <div className="grid grid-cols-1 gap-6 mt-6">
        <div className="flex flex-col">

          {isLoading && (
            <div className="text-sm text-blue-600">Cargando información...</div>
          )}

          {errorMessage && (
            <div className="rounded-md border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
              {errorMessage}
            </div>
          )}

          <Accordion activeIndex={[0]} multiple>
            <AccordionTab header="Información General">
              <EncabezadoActividad idExpediente={id_expediente} activityID={ACTIVITY_ID} />
            </AccordionTab>
            <AccordionTab header="Funciones Transversales">
              <FuncionesTransversales idExpediente={id_expediente} idActividad={ACTIVITY_ID} />
            </AccordionTab>
            <AccordionTab header="Validar Informacion">
              <div className="grid grid-cols-1 gap-2 mb-5">
                <h4 className="text-blue-800 font-bold text-corp-primary tracking-wider mb-3 border-b pb-1">Datos del Titular</h4>
                <DatosTitularSection data={form.validar_informacion_data} controles={controles} isEditing={false} onChange={() => undefined} />
              </div>
              <div className="grid grid-cols-1 gap-2 mb-5">
                <h4 className="text-blue-800 font-bold text-corp-primary tracking-wider mb-3 border-b">Datos de Crédito</h4>
                <DatosCreditoSection data={form.validar_informacion_data} encabezado={encabezado} controles={controles} isEditing={!isDisabled && isAnalistaViviendaII} onChange={updateValidarInformacionField} />
              </div>
              <div className="grid grid-cols-1 gap-2 mb-5">
                <h4 className="text-blue-800 font-bold text-corp-primary tracking-wider mb-3 border-b pb-1">Datos del Inmueble</h4>
                <DatosInmuebleSection data={form.validar_informacion_data} controles={controles} isEditing={false} onChange={() => undefined} />
              </div>
              <div className="grid grid-cols-1 gap-2 mb-5">
                <h4 className="text-blue-800 font-bold text-corp-primary tracking-wider mb-3 border-b pb-1">Condiciones Financieras</h4>
                <CondicionesFinancierasSection data={form.validar_informacion_data} encabezado={encabezado} isEditing={!isDisabled && isAnalistaViviendaII} onChange={updateValidarInformacionField} />
              </div>
            </AccordionTab>

            <AccordionTab header="Validar Integración de Documentos">
              <ValidarIntegracionForm
                form={form.formulario}
                isDisabled={isDisabled}
                updateField={updateFormCoreField}
                controles={controles}
              />

              <div className="mt-5">
                <IntervinientesSection
                  data={intervinientes}
                  disabled={!isAnalistaViviendaII || isDisabled}
                  onSave={handleGuardarInterviniente}
                  controles={controles}
                />
              </div>
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

      </div>
    </>
  );
}
