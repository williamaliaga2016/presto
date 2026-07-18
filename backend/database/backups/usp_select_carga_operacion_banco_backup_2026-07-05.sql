-- ============================================================================
-- BACKUP de las definiciones ORIGINALES (antes de agregar columnas BBVA)
-- de las 4 funciones usp_select_carga_operacion_banco_* que no exponían
-- las columnas BBVA agregadas por las migraciones de EF Core.
--
-- Generado: 2026-07-05
-- Origen:   BD BBVA_LEGALIZACION (docker container "postgres"), extraído con
--           `psql -c "\sf <nombre_funcion>"` por cada una.
--
-- USO: si el script de arreglo (fix_usp_select_carga_operacion_banco_columnas_bbva.sql)
-- causa un problema, ejecutar este archivo completo para restaurar el
-- comportamiento anterior (CREATE OR REPLACE con la misma firma anterior).
-- ============================================================================

CREATE OR REPLACE FUNCTION public.usp_select_carga_operacion_banco_antecedente_comprador(p_id_expediente bigint)
 RETURNS TABLE(id_carga_operacion_banco_antecedente_comprador integer, id_carga_operacion_banco integer, id_expediente bigint, rut character varying, tipo_comprador character varying, razon_social character varying, nombres character varying, apellido_paterno character varying, apellido_materno character varying, fecha_nacimiento timestamp without time zone, genero character varying, estado_civil character varying, relacion_titular character varying, direccion character varying, region character varying, comuna character varying, telefono character varying, email character varying, nacionalidad character varying, profesion character varying, is_active boolean, row_status boolean, created_by integer, created_date timestamp without time zone, modified_by integer, modified_date timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ac.id_carga_operacion_banco_antecedente_comprador::integer,
        ac.id_carga_operacion_banco::integer,
        ac.id_expediente::bigint,
        ac.rut::varchar(20),
        ac.tipo_comprador::varchar(50),
        ac.razon_social::varchar(250),
        ac.nombres::varchar(150),
        ac.apellido_paterno::varchar(100),
        ac.apellido_materno::varchar(100),
        ac.fecha_nacimiento::timestamp without time zone,
        ac.genero::varchar(50),
        ac.estado_civil::varchar(50),
        ac.relacion_titular::varchar(50),
        ac.direccion::varchar(250),
        ac.region::varchar(50),
        ac.comuna::varchar(50),
        ac.telefono::varchar(50),
        ac.email::varchar(150),
        ac.nacionalidad::varchar(50),
        ac.profesion::varchar(150),
        ac.is_active::boolean,
        ac.row_status::boolean,
        ac.created_by::integer,
        ac.created_date::timestamp without time zone,
        ac.modified_by::integer,
        ac.modified_date::timestamp without time zone
    FROM public.carga_operacion_banco_antecedente_comprador ac
    WHERE ac.id_expediente = p_id_expediente
      AND ac.row_status = true
    ORDER BY ac.id_carga_operacion_banco_antecedente_comprador ASC;
END;
$function$
;

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
;

CREATE OR REPLACE FUNCTION public.usp_select_carga_operacion_banco_datos_operacion(p_id_expediente bigint)
 RETURNS TABLE(id_carga_operacion_banco_datos_operacion integer, id_carga_operacion_banco integer, id_expediente bigint, nro_mutuo bigint, tipo_operacion character varying, nro_registro bigint, ult_clasif_al character varying, segmento character varying, canal_venta character varying, nro_op_cartera bigint, modelo_operacion character varying, tipo_carpeta character varying, propietario character varying, inmobiliaria character varying, glosa_producto character varying, codigo_producto_comercial character varying, nro_piloto integer, banco_alzante character varying, nombre_proyecto character varying, is_active boolean, row_status boolean, created_by integer, created_date timestamp without time zone, modified_by integer, modified_date timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        dope.id_carga_operacion_banco_datos_operacion::integer,
        dope.id_carga_operacion_banco::integer,
        dope.id_expediente::bigint,
        dope.nro_mutuo::bigint,
        dope.tipo_operacion::varchar(50),
        dope.nro_registro::bigint,
        dope.ult_clasif_al::varchar(200),
        dope.segmento::varchar(50),
        dope.canal_venta::varchar(50),
        dope.nro_op_cartera::bigint,
        dope.modelo_operacion::varchar(50),
        dope.tipo_carpeta::varchar(50),
        dope.propietario::varchar(150),
        dope.inmobiliaria::varchar(50),
        dope.glosa_producto::varchar(50),
        dope.codigo_producto_comercial::varchar(50),
        dope.nro_piloto::integer,
        dope.banco_alzante::varchar(50),
        dope.nombre_proyecto::varchar(250),
        dope.is_active::boolean,
        dope.row_status::boolean,
        dope.created_by::integer,
        dope.created_date::timestamp without time zone,
        dope.modified_by::integer,
        dope.modified_date::timestamp without time zone
    FROM public.carga_operacion_banco_datos_operacion dope
    WHERE dope.id_expediente = p_id_expediente
      AND dope.row_status = true
    ORDER BY dope.id_carga_operacion_banco_datos_operacion DESC
    LIMIT 1;
END;
$function$
;

CREATE OR REPLACE FUNCTION public.usp_select_carga_operacion_banco_datos_comercial(p_id_expediente bigint)
 RETURNS TABLE(id_carga_operacion_banco_datos_comercial integer, id_carga_operacion_banco integer, id_expediente bigint, codigo_ejecutivo bigint, login_ejecutivo character varying, nombre_ejecutivo character varying, rut_ejecutivo bigint, codigo_oficina bigint, nombre_oficina character varying, codigo_curse bigint, glosa_curse character varying, codigo_ejecutivo_curse bigint, login_ejecutivo_curse character varying, nombre_ejecutivo_curse character varying, rut_ejecutivo_curse bigint, rut_banco bigint, renovacion_urbana character varying, nombre_banco character varying, tipo_hipoteca character varying, is_active boolean, row_status boolean, created_by integer, created_date timestamp without time zone, modified_by integer, modified_date timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        dc.id_carga_operacion_banco_datos_comercial::integer,
        dc.id_carga_operacion_banco::integer,
        dc.id_expediente::bigint,

        dc.codigo_ejecutivo::bigint,
        dc.login_ejecutivo::varchar(100),
        dc.nombre_ejecutivo::varchar(150),
        dc.rut_ejecutivo::bigint,
        dc.codigo_oficina::bigint,
        dc.nombre_oficina::varchar(150),
        dc.codigo_curse::bigint,
        dc.glosa_curse::varchar(250),
        dc.codigo_ejecutivo_curse::bigint,
        dc.login_ejecutivo_curse::varchar(100),
        dc.nombre_ejecutivo_curse::varchar(150),
        dc.rut_ejecutivo_curse::bigint,
        dc.rut_banco::bigint,
        dc.renovacion_urbana::varchar(150),
        dc.nombre_banco::varchar(150),
        dc.tipo_hipoteca::varchar(150),

        dc.is_active::boolean,
        dc.row_status::boolean,
        dc.created_by::integer,
        dc.created_date::timestamp without time zone,
        dc.modified_by::integer,
        dc.modified_date::timestamp without time zone
    FROM public.carga_operacion_banco_datos_comercial dc
    WHERE dc.id_expediente = p_id_expediente
      AND dc.row_status = true
    ORDER BY dc.id_carga_operacion_banco_datos_comercial DESC
    LIMIT 1;
END;
$function$
;
