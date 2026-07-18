export interface RectificatoriaPostventaSolucionReparo {
    id_rectificatoria_postventa_solucion_reparo: number;
    id_expediente: number;
    id_usuario_solicitante: number;
    is_subsanar: boolean;
    observaciones?: string | null;
    modificar_datos_memo: boolean;
    descontabilizar_operacion: boolean;
    /**
     * Campos heredados/visualización.
     * Vienen desde el reparo generado en rectificatoria_analisis_derivacion_reparo_postventa
     * y/o desde la consulta con Users.
     */
    solicitante?: string | null;
    observaciones_reparo?: string | null;
    fecha_ingreso?: string | null;

    is_active: boolean;
    row_status: boolean;
    created_by: number;
    created_date: string;
    modified_by?: number | null;
    modified_date?: string | null;
}
