import { useEffect, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Checkbox, CheckboxChangeEvent } from "primereact/checkbox";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import type { VerificarReparoCbr } from "../models/verificar_reparo_cbr";
import { useAvanzarVerificarReparoCbr } from "../hooks/useAvanzarVerificarReparoCbr";
import { useVerificarReparoCbr } from "../hooks/useVerificarReparoCbr";
import { useUpsertVerificarReparoCbr } from "../hooks/useUpsertVerificarReparoCbr";
import { RadioButton } from "primereact/radiobutton";
import { Dropdown } from "primereact/dropdown";
import { useControlesVerificarReparoCbr } from "../hooks/useControlesVerificarReparoCbr";
import { EMPTY_CONTROLES_VERIFICAR_REPARO_CBR } from "../models/catalogo";

const ACTIVITY_ID = "_Ug2S_P8zJebRx5_Z6wQ3d";

const buildInitialState = (id_expediente: number): VerificarReparoCbr => ({
    id_verificar_reparo_cbr: 0,
    id_expediente,
    enviar_a_reparo: false,
    enviar_reparo_a: "",
    estatus_reparo: false,
    observaciones: "",
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: new Date().toISOString(),
    modified_by: null,
    modified_date: null,
});

const normalizeVerificarReparoCbr = (
    source: Partial<VerificarReparoCbr> | null | undefined,
    id_expediente_fallback: number,
): VerificarReparoCbr => {
    return {
        id_verificar_reparo_cbr: Number(source?.id_verificar_reparo_cbr ?? 0),
        id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
        enviar_a_reparo: Boolean(source?.enviar_a_reparo ?? false),
        enviar_reparo_a: source?.enviar_reparo_a ?? "",
        estatus_reparo: Boolean(source?.estatus_reparo ?? false),
        observaciones: source?.observaciones ?? "",
        is_active: source?.is_active ?? true,
        row_status: source?.row_status ?? true,
        created_by: Number(source?.created_by ?? 0),
        created_date: source?.created_date ?? new Date().toISOString(),
        modified_by: source?.modified_by ?? null,
        modified_date: source?.modified_date ?? null,
    };
};

export default function VerificarReparoCbrPage() {
    const toast = useRef<Toast>(null);

    const navigate = useNavigate();
    const { id_expediente: idExpedienteParam } = useParams();

    const id_expediente = Number(idExpedienteParam ?? 0);

    const [form, setForm] = useState<VerificarReparoCbr>(
        buildInitialState(id_expediente),
    );
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [isDisabled, setIsDisabled] = useState(true);
    const [canAdvance, setCanAdvance] = useState(false);
    const [isBusy, setIsBusy] = useState(false);

    const hasHydratedRef = useRef(false);
    const currentExpedienteRef = useRef<number>(id_expediente);

    const { data, isLoading } = useVerificarReparoCbr(id_expediente);
    const saveMutation = useUpsertVerificarReparoCbr();
    const avanzarMutation = useAvanzarVerificarReparoCbr();
    const controlesVerificarReparoCbrQuery = useControlesVerificarReparoCbr();
    
    const controlesVerificarReparoCbr = controlesVerificarReparoCbrQuery.data?.status
        ? controlesVerificarReparoCbrQuery.data.detail ?? EMPTY_CONTROLES_VERIFICAR_REPARO_CBR
        : EMPTY_CONTROLES_VERIFICAR_REPARO_CBR;
    
    useEffect(() => {
        if (currentExpedienteRef.current !== id_expediente) {
            currentExpedienteRef.current = id_expediente;
            hasHydratedRef.current = false;

            setForm(buildInitialState(id_expediente));
            setErrorMessage("");
            setSuccessMessage("");
            setIsDisabled(true);
            setCanAdvance(false);
        }
    }, [id_expediente]);

    useEffect(() => {
        if (hasHydratedRef.current) return;

        if (data?.status && data.detail) {
            const loadedEntity = normalizeVerificarReparoCbr(
                data.detail,
                id_expediente,
            );

            setForm(loadedEntity);
            setIsDisabled(Number(data.detail.id_verificar_reparo_cbr) > 0);
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
                normalizeVerificarReparoCbr(
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

    const updateField = <K extends keyof VerificarReparoCbr>(
        field: K,
        value: VerificarReparoCbr[K],
    ) => {
        setForm((prev) => ({
            ...prev,
            [field]: value,
        }));
    };

    const handleEnviarAReparoChange = (e: CheckboxChangeEvent) => {
        updateField("enviar_a_reparo", Boolean(e.checked));
    };

    const handleEditar = () => {
        setErrorMessage("");
        setSuccessMessage("");
        setIsDisabled(false);
        setCanAdvance(false);
    };

    const validateForm = () => {
        if (!form.id_expediente || form.id_expediente <= 0) {
            return "No existe un id_expediente válido.";
        }

        return "";
    };

    const handleGuardar = async () => {
        setErrorMessage("");
        setSuccessMessage("");

        const validationMessage = validateForm();
        if (validationMessage) {
            toast.current?.show({
                severity: "warn",
                summary: "Validación",
                detail: validationMessage,
                life: 3000,
            });
            return;
        }

        try {
            setIsBusy(true);

            const payload: VerificarReparoCbr = normalizeVerificarReparoCbr(
                {
                    ...form,
                    id_verificar_reparo_cbr: Number(
                        form.id_verificar_reparo_cbr ?? 0,
                    ),
                    id_expediente: Number(form.id_expediente || id_expediente || 0),
                },
                id_expediente,
            );

            const response = await saveMutation.mutateAsync(payload);

            if (response.status) {
                toast.current?.show({
                    severity: "success",
                    summary: "Éxito",
                    detail: "Actividad Generar Carta Resguardo guardada correctamente",
                    life: 3000,
                });

                const savedEntity = normalizeVerificarReparoCbr(
                    response.detail ?? payload,
                    payload.id_expediente,
                );

                setForm(savedEntity);
                setIsDisabled(true);
                setCanAdvance(true);
                hasHydratedRef.current = true;
            } else {
                toast.current?.show({
                    severity: "warn",
                    summary: "Atención",
                    detail: response.message || "No se pudo guardar",
                    life: 3000,
                });
            }
        } catch (error) {
            console.error("ERROR GUARDAR Generar Carta Resguardo", error);

            toast.current?.show({
                severity: "error",
                summary: "Error",
                detail: "Ocurrió un error al guardar",
                life: 3000,
            });
        } finally {
            setIsBusy(false);
        }
    };

    const handleAvanzar = async () => {
        setErrorMessage("");
        setSuccessMessage("");

        const validationMessage = validateForm();
        if (validationMessage) {
            setErrorMessage(validationMessage);
            toast.current?.show({
                severity: "warn",
                summary: "Validación",
                detail: validationMessage,
                life: 3000,
            });
            return;
        }

        if (form.enviar_a_reparo && !form.enviar_reparo_a?.trim()) {
            const msg = "Debe seleccionar el tipo de reparo para avanzar.";
            setErrorMessage(msg);
            toast.current?.show({
                severity: "warn",
                summary: "Validación",
                detail: msg,
                life: 3000,
            });
            return;
        }

        try {
            setIsBusy(true);

            const response = await avanzarMutation.mutateAsync(Number(form.id_expediente));

            if (response.status) {
                toast.current?.show({
                    severity: "success",
                    summary: "Éxito",
                    detail: "Actividad avanzada correctamente",
                    life: 2000,
                });

                navigate("/home/bandeja");
            } else {
                const msg = response.message || "No se pudo avanzar la actividad.";

                setErrorMessage(msg);
                setSuccessMessage("");

                toast.current?.show({
                    severity: "warn",
                    summary: "Atención",
                    detail: msg,
                    life: 3000,
                });
            }
        } catch (error) {
            console.error("ERROR AVANZAR Generar Carta Resguardo", error);
            const msg = "Ocurrió un error al avanzar.";

            setErrorMessage(msg);
            setSuccessMessage("");

            toast.current?.show({
                severity: "error",
                summary: "Error",
                detail: msg,
                life: 3000,
            });
        } finally {
            setIsBusy(false);
        }
    };

    const handleSalir = () => {
        navigate("/home/bandeja");
    };

    return (
        <>
            <Toast ref={toast} />

            <h2 className="text-2xl font-bold text-gray-900 mb-6">
                Verificar Reparo CBR
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
                    />
                </AccordionTab>

                <AccordionTab header="Generar Carta Resguardo">
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
<div className="grid grid-cols-1 md:grid-cols-3 gap-5 mb-3">                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">Expediente</label>

                                <InputNumber
                                    value={form.id_expediente}
                                    className="form-input-presto w-full"
                                    useGrouping={false}
                                    disabled
                                />
                            </div>
                        </div>
                        <div className="grid grid-cols-1  md:grid-cols-3 gap-5">
                            
                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">¿Enviar a reparo?</label>

                                <div className="flex items-center gap-2 h-11">
                                    <Checkbox
                                        className="form-checkbox-presto"
                                        inputId="enviar_a_reparo"
                                        checked={form.enviar_a_reparo}
                                        onChange={handleEnviarAReparoChange}
                                        disabled={isDisabled}
                                    />
                                    <label htmlFor="enviar_a_reparo" className="text-sm">
                                        ¿Enviar a reparo?
                                    </label>
                                </div>
                            </div>
                            

                            <div className="flex flex-col gap-1">
                                <label className="font-semibold text-sm">Enviar reparo a:</label>
                                <Dropdown
                                    value={form?.enviar_reparo_a ?? null}
                                    options={controlesVerificarReparoCbr.tiporeparo}
                                    optionLabel="description"
                                    optionValue="code"
                                    disabled={isDisabled}
                                    className="form-dropdown-presto w-full"
                                    placeholder="Seleccione"
                                    onChange={(e) => updateField('enviar_reparo_a', e.value)}
                                    showClear
                                />
                            </div>

                            <div className="flex flex-col gap-1 mt-5">
                                <div className="flex items-center gap-6 h-10">
                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="estatus_reparo_si"
                                            checked={form?.estatus_reparo === true}
                                            disabled={isDisabled}
                                            onChange={() => updateField('estatus_reparo', true)}
                                        />

                                        <label
                                            htmlFor="estatus_reparo_si"
                                            className="text-sm text-gray-700"
                                        >
                                            Contribuciones
                                        </label>
                                    </div>

                                    <div className="flex items-center gap-2">
                                        <RadioButton
                                            className="form-radio-presto"
                                            inputId="estatus_reparo_no"
                                            checked={form?.estatus_reparo === false}
                                            disabled={isDisabled}
                                            onChange={() => updateField('estatus_reparo', false)}
                                        />

                                        <label
                                            htmlFor="estatus_reparo_no"
                                            className="text-sm text-gray-700"
                                        >
                                            Error Formulario Notaria
                                        </label>
                                    </div>
                                </div>
                            </div>


                            <div className="flex flex-col gap-1 md:col-span-3">
                                <label className="font-semibold text-sm">
                                    Observaciones
                                </label>
                                <InputTextarea
                                    value={form.observaciones ?? ""}
                                    onChange={(e) => updateField("observaciones", e.target.value)}
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
                                label={saveMutation.isPending ? "Guardando..." : "Guardar"}
                                icon="pi pi-save"
                                severity="success"
                                onClick={handleGuardar}
                                disabled={isBusy || isDisabled}
                                className="btn-responsive"
                            />

                            <Button
                                type="button"
                                label={avanzarMutation.isPending ? "Avanzando..." : "Avanzar"}
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
