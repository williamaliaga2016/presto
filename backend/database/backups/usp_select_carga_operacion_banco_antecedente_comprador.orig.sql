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
