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
