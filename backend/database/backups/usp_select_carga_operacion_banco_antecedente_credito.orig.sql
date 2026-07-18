CREATE OR REPLACE FUNCTION public.usp_select_carga_operacion_banco_antecedente_credito(p_id_expediente bigint)
 RETURNS TABLE(id_carga_operacion_banco_antecedente_credito integer, id_carga_operacion_banco integer, id_expediente bigint, tipo_prestamo character varying, factor_conversion_uf numeric, destino_credito character varying, monto_solicitado numeric, tipo_tasa character varying, tasa numeric, plazo integer, fecha_inicio timestamp without time zone, monto_nominal numeric, monto_residual numeric, plazo_primer_periodo integer, periodo_gracia integer, comision numeric, plazo_segundo_periodo integer, tasa_primer_periodo numeric, meses_sabaticos integer, variabilidad_tasa character varying, tasa_segundo_periodo numeric, moneda character varying, tipo_tasa_mixta_prod_com character varying, tasa_maxima numeric, codigo_producto_cartera character varying, indicador_segunda_vivienda character varying, tipo_financiamiento character varying, precio_venta_pesos numeric, precio_venta_moneda_original character varying, cantidad_meses_sin_vencimiento integer, indicador_cred_comp integer, indicador_pac character varying, tipo_tasa_aplic_contab character varying, numero_cuenta_gastos bigint, prestamo_maximo numeric, is_active boolean, row_status boolean, created_by integer, created_date timestamp without time zone, modified_by integer, modified_date timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ac.id_carga_operacion_banco_antecedente_credito::integer,
        ac.id_carga_operacion_banco::integer,
        ac.id_expediente::bigint,

        ac.tipo_prestamo::varchar(50),
        ac.factor_conversion_uf::numeric(18, 6),
        ac.destino_credito::varchar(50),
        ac.monto_solicitado::numeric(18, 6),
        ac.tipo_tasa::varchar(50),
        ac.tasa::numeric(18, 6),
        ac.plazo::integer,
        ac.fecha_inicio::timestamp without time zone,
        ac.monto_nominal::numeric(18, 6),
        ac.monto_residual::numeric(18, 6),
        ac.plazo_primer_periodo::integer,
        ac.periodo_gracia::integer,
        ac.comision::numeric(18, 6),
        ac.plazo_segundo_periodo::integer,
        ac.tasa_primer_periodo::numeric(18, 6),
        ac.meses_sabaticos::integer,
        ac.variabilidad_tasa::varchar(100),
        ac.tasa_segundo_periodo::numeric(18, 6),
        ac.moneda::varchar(50),
        ac.tipo_tasa_mixta_prod_com::varchar(100),
        ac.tasa_maxima::numeric(18, 6),
        ac.codigo_producto_cartera::varchar(100),
        ac.indicador_segunda_vivienda::varchar(50),
        ac.tipo_financiamiento::varchar(100),
        ac.precio_venta_pesos::numeric(18, 6),
        ac.precio_venta_moneda_original::varchar(100),
        ac.cantidad_meses_sin_vencimiento::integer,
        ac.indicador_cred_comp::integer,
        ac.indicador_pac::varchar(50),
        ac.tipo_tasa_aplic_contab::varchar(100),
        ac.numero_cuenta_gastos::bigint,
        ac.prestamo_maximo::numeric(18, 6),

        ac.is_active::boolean,
        ac.row_status::boolean,
        ac.created_by::integer,
        ac.created_date::timestamp without time zone,
        ac.modified_by::integer,
        ac.modified_date::timestamp without time zone
    FROM public.carga_operacion_banco_antecedente_credito ac
    WHERE ac.id_expediente = p_id_expediente
      AND ac.row_status = true
    ORDER BY ac.id_carga_operacion_banco_antecedente_credito DESC
    LIMIT 1;
END;
$function$
