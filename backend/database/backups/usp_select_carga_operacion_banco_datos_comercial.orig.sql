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
