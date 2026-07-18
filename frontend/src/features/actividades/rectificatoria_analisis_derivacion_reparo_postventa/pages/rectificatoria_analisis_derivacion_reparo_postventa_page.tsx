import { useEffect, useMemo, useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Accordion, AccordionTab } from "primereact/accordion";
import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { InputNumber } from "primereact/inputnumber";
import { InputTextarea } from "primereact/inputtextarea";
import { Toast } from "primereact/toast";

import EncabezadoActividad from "@/features/encabezado/pages/EncabezadoActividad";
import FuncionesTransversales from "@/features/funciones_transversales/pages/FuncionesTransversales";
import type { RectificatoriaAnalisisDerivacionReparoPostventa } from "../models/rectificatoria_analisis_derivacion_reparo_postventa";
import { useAvanzarRectificatoriaAnalisisDerivacionReparoPostventa } from "../hooks/useAvanzarRectificatoriaAnalisisDerivacionReparoPostventa";
import { useRectificatoriaAnalisisDerivacionReparoPostventa } from "../hooks/useRectificatoriaAnalisisDerivacionReparoPostventa";
import { useUpsertRectificatoriaAnalisisDerivacionReparoPostventa } from "../hooks/useUpsertRectificatoriaAnalisisDerivacionReparoPostventa";
import { useControlesRectificatoriaAnalisisDerivacionReparoPostventa } from "../hooks/useControlesRectificatoriaAnalisisDerivacionReparoPostventa";
import { ControlesRectificatoriaAnalisisDerivacionReparoPostventa, EMPTY_CONTROLES_RECTIFICATORIA_ANALIS_DERIVACION_REPARO_POSTVENTA } from "../models/catalogo";
import { RadioButton } from "primereact/radiobutton";

const ACTIVITY_ID = "_F5kTyM7wEeoJq8_Y2nV6p";

const buildInitialState = (id_expediente: number): RectificatoriaAnalisisDerivacionReparoPostventa => ({
    id_rectificatoria_analisis_derivacion_reparo_postventa: 0,
    id_expediente,
    enviar_reparo_a: false,
    observaciones: "",
    is_active: true,
    row_status: true,
    created_by: 0,
    created_date: new Date().toISOString(),
    modified_by: null,
    modified_date: null,
});

const normalizeRectificatoriaAnalisisDerivacionReparoPostventa = (
    source: Partial<RectificatoriaAnalisisDerivacionReparoPostventa> | null | undefined,
    id_expediente_fallback: number,
): RectificatoriaAnalisisDerivacionReparoPostventa => {
    return {
        id_rectificatoria_analisis_derivacion_reparo_postventa: Number(source?.id_rectificatoria_analisis_derivacion_reparo_postventa ?? 0),
        id_expediente: Number(source?.id_expediente ?? id_expediente_fallback ?? 0),
        enviar_reparo_a: Boolean(source?.enviar_reparo_a ?? false),
        observaciones: source?.observaciones ?? "",
        is_active: source?.is_active ?? true,
        row_status: source?.row_status ?? true,
        created_by: Number(source?.created_by ?? 0),
        created_date: source?.created_date ?? new Date().toISOString(),
        modified_by: source?.modified_by ?? null,
        modified_date: source?.modified_date ?? null,
    };
};

export default function RectificatoriaAnalisisDerivacionReparoPostventaPage() {
    const toast = useRef<Toast>(null);

    const navigate = useNavigate();
    const { id_expediente: idExpedienteParam } = useParams();

    const id_expediente = Number(idExpedienteParam ?? 0);

    const [form, setForm] = useState<RectificatoriaAnalisisDerivacionReparoPostventa>(
        buildInitialState(id_expediente),
    );
    const [errorMessage, setErrorMessage] = useState("");
    const [successMessage, setSuccessMessage] = useState("");
    const [isDisabled, setIsDisabled] = useState(true);
    const [canAdvance, setCanAdvance] = useState(false);
    const [isBusy, setIsBusy] = useState(false);

    const hasHydratedRef = useRef(false);
    const currentExpedienteRef = useRef<number>(id_expediente);

    const { data, isLoading } = useRectificatoriaAnalisisDerivacionReparoPostventa(id_expediente);
    const saveMutation = useUpsertRectificatoriaAnalisisDerivacionReparoPostventa();
    const avanzarMutation = useAvanzarRectificatoriaAnalisisDerivacionReparoPostventa();

    const controlesRectificatoriaAnalisisDerivacionReparoPostventaQuery = useControlesRectificatoriaAnalisisDerivacionReparoPostventa();
    

    const catalogo: ControlesRectificatoriaAnalisisDerivacionReparoPostventa = useMemo(() => {
        if (!controlesRectificatoriaAnalisisDerivacionReparoPostventaQuery.data?.status) {
            return EMPTY_CONTROLES_RECTIFICATORIA_ANALIS_DERIVACION_REPARO_POSTVENTA;
        }

        return (
            controlesRectificatoriaAnalisisDerivacionReparoPostventaQuery.data.detail ??
            EMPTY_CONTROLES_RECTIFICATORIA_ANALIS_DERIVACION_REPARO_POSTVENTA
        );
    }, [controlesRectificatoriaAnalisisDerivacionReparoPostventaQuery.data]);
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
            const loadedEntity = normalizeRectificatoriaAnalisisDerivacionReparoPostventa(
                data.detail,
                id_expediente,
            );

            setForm(loadedEntity);
            setIsDisabled(Number(data.detail.id_rectificatoria_analisis_derivacion_reparo_postventa) > 0);
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
                normalizeRectificatoriaAnalisisDerivacionReparoPostventa(
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

    const updateField = <K extends keyof RectificatoriaAnalisisDerivacionReparoPostventa>(
        field: K,
        value: RectificatoriaAnalisisDerivacionReparoPostventa[K],
    ) => {
        setForm((prev) => ({
            ...prev,
            [field]: value,
        }));
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

            const payload: RectificatoriaAnalisisDerivacionReparoPostventa = normalizeRectificatoriaAnalisisDerivacionReparoPostventa(
                {
                    ...form,
                    id_rectificatoria_analisis_derivacion_reparo_postventa: Number(
                        form.id_rectificatoria_analisis_derivacion_reparo_postventa ?? 0,
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
                    detail: "Actividad Rectificatoria Análisis y derivación reparo Postventa guardada correctamente",
                    life: 3000,
                });

                const savedEntity = normalizeRectificatoriaAnalisisDerivacionReparoPostventa(
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
            console.error("ERROR GUARDAR Rectificatoria Análisis y derivación reparo Postventa", error);

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
            console.error("ERROR AVANZAR Rectificatoria Análisis y derivación reparo Postventa", error);
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
                Rectificatoria Análisis y derivación reparo Postvent
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

                <AccordionTab header="Rectificatoria Análisis y derivación reparo Postventa">
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
                                <label className="font-semibold text-sm">Enviar reparo a:</label>

                                <div className="flex gap-4">
                                    {(catalogo.tiporeparo ?? []).map((item) => (
                                        <div key={item.code} className="flex items-center gap-2">
                                            <RadioButton
                                                inputId={`enviar_reparo_a-${item.code}`}
                                                name="enviar_reparo_a"
                                                value={item.code}

                                                onChange={(e) => updateField('enviar_reparo_a', e.value ?? null)}
                                                checked={form.enviar_reparo_a === item.code}
                                                disabled={isDisabled}
                                                className="form-radio-presto"
                                            />
                                            <label htmlFor={`enviar_reparo_a-${item.code}`}>
                                                {item.description}
                                            </label>
                                        </div>
                                    ))}
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
