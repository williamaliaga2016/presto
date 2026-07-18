-- Seeds frontend menus/routes, creates Admin, and maps it to every active menu.
-- To create another full-access role, change v_role_code and v_role_name.

BEGIN;

DO $$
DECLARE
    v_role_id integer;
    v_role_code varchar(50) := 'ADMIN';
    v_role_name varchar(50) := 'Admin';
    v_audit_user integer := 1;
    v_now timestamp without time zone := now()::timestamp without time zone;
    v_total_active_menus integer;
    v_total_assigned_menus integer;
BEGIN
    LOCK TABLE public.role, public.menus, public.role_menu
        IN SHARE ROW EXCLUSIVE MODE;

    -- Imported/manual IDs can leave identity sequences behind MAX(id).
    PERFORM setval(
        pg_get_serial_sequence('public.role', 'role_id')::regclass,
        COALESCE((SELECT max(role_id) FROM public.role), 0) + 1,
        false
    );
    PERFORM setval(
        pg_get_serial_sequence('public.menus', 'menu_id')::regclass,
        COALESCE((SELECT max(menu_id) FROM public.menus), 0) + 1,
        false
    );
    PERFORM setval(
        pg_get_serial_sequence('public.role_menu', 'role_menu_id')::regclass,
        COALESCE((SELECT max(role_menu_id) FROM public.role_menu), 0) + 1,
        false
    );

    CREATE TEMP TABLE tmp_menu_seed (
        parent_key text,
        menu_key text NOT NULL,
        name varchar(50) NOT NULL,
        icon_name varchar(100) NOT NULL,
        description_alt varchar(100) NOT NULL,
        menu_url varchar(150) NOT NULL,
        is_show_navbar boolean NOT NULL,
        is_show_home_menu boolean NOT NULL,
        orden integer NOT NULL
    ) ON COMMIT DROP;

    INSERT INTO tmp_menu_seed (
        parent_key,
        menu_key,
        name,
        icon_name,
        description_alt,
        menu_url,
        is_show_navbar,
        is_show_home_menu,
        orden
    )
    VALUES
        (NULL, 'bandeja', 'Bandeja de Actividades', 'pi pi-home', 'Bandeja de Actividades', '/home/bandeja', true, true, 10),
        (NULL, 'reportes', 'Reportes', 'pi pi-chart-bar', 'Reportes', '/home/reportes', true, true, 20),
        (NULL, 'consulta_actividades', 'Consulta de Actividades', 'pi pi-search', 'Consulta de Actividades', '/home/consulta_actividades', true, true, 30),
        (NULL, 'carga_operacion_banco', 'Carga Operacion Banco', 'pi pi-plus-circle', 'Carga Operacion Banco', '/home/carga_operacion_banco', true, true, 40),
        (NULL, 'administracion', 'Administracion', 'pi pi-cog', 'Administracion', '#administracion', true, true, 50),
        ('administracion', 'administracion_usuarios', 'Usuarios', 'pi pi-users', 'Administracion de Usuarios', '/home/administracion_usuarios', true, true, 51),
        ('administracion', 'administracion_roles', 'Roles', 'pi pi-id-card', 'Administracion de Roles', '/home/administracion_roles', true, true, 52),
        ('administracion', 'reasignacion_desestimacion', 'Reasignar y Desestimar', 'pi pi-refresh', 'Reasignar y Desestimar Actividades', '/home/reasignacion-desestimacion', true, true, 53),
        (NULL, 'actividades', 'Actividades', 'pi pi-list', 'Rutas operativas de actividades', '#actividades', false, false, 100),
        ('actividades', 'carga_operacion_banco_detalle', 'Carga Operacion Banco Detalle', 'pi pi-file-edit', 'Carga Operacion Banco Detalle', '/home/carga_operacion_banco/:id_expediente', false, false, 101),
        ('actividades', 'recepcion_carga_fabrica', 'Recepcion Carga Fabrica', 'pi pi-file-edit', 'Recepcion Carga Fabrica', '/home/recepcion_carga_fabrica/:id_expediente', false, false, 102),
        ('actividades', 'corregir_reparo_fabrica', 'Corregir Reparo Fabrica', 'pi pi-file-edit', 'Corregir Reparo Fabrica', '/home/corregir_reparo_fabrica/:id_expediente', false, false, 103),
        ('actividades', 'corregir_reparo_tasacion', 'Corregir Reparo Tasacion', 'pi pi-file-edit', 'Corregir Reparo Tasacion', '/home/corregir_reparo_tasacion/:id_expediente', false, false, 104),
        ('actividades', 'generar_memo_escritura', 'Generar Memo Escritura', 'pi pi-file-edit', 'Generar Memo Escritura', '/home/generar_memo_escritura/:id_expediente', false, false, 105),
        ('actividades', 'corregir_reparo_generar_memo', 'Corregir Reparo Generar Memo', 'pi pi-file-edit', 'Corregir Reparo Generar Memo Escritura', '/home/corregir_reparo_generar_memo_escritura/:id_expediente', false, false, 106),
        ('actividades', 'corregir_reparo_generar_borrador', 'Corregir Reparo Generar Borrador', 'pi pi-file-edit', 'Corregir Reparo Generar Borrador Escritura', '/home/corregir_reparo_generar_borrador_escritura/:id_expediente', false, false, 107),
        ('actividades', 'corregir_reparo_calculo_doc', 'Corregir Reparo Calculo Documento', 'pi pi-file-edit', 'Corregir Reparo Calculo Documento', '/home/corregir_reparo_calculo_generacion_documento/:id_expediente', false, false, 108),
        ('actividades', 'corregir_reparo_datos_operacion', 'Corregir Reparo Datos Operacion', 'pi pi-file-edit', 'Corregir Reparo Datos Operacion', '/home/corregir_reparo_datos_operacion/:id_expediente', false, false, 109),
        ('actividades', 'datos_operacion', 'Datos Operacion', 'pi pi-file-edit', 'Datos Operacion', '/home/datos_operacion/:id_expediente', false, false, 110),
        ('actividades', 'revisar_datos_operacion', 'Revisar Datos Operacion', 'pi pi-file-edit', 'Revisar Datos Operacion', '/home/revisar_datos_operacion/:id_expediente', false, false, 111),
        ('actividades', 'asignar_escritura', 'Asignar Escritura', 'pi pi-file-edit', 'Asignar Escritura', '/home/asignar_escritura/:id_expediente', false, false, 112),
        ('actividades', 'asignar_estudio_titulos', 'Asignar Estudio Titulos', 'pi pi-file-edit', 'Asignar Estudio Titulos', '/home/asignar_estudio_titulos/:id_expediente', false, false, 113),
        ('actividades', 'calculo_generacion_documento', 'Calculo Generacion Documento', 'pi pi-file-edit', 'Calculo Generacion Documento', '/home/calculo_generacion_documento/:id_expediente', false, false, 114),
        ('actividades', 'registrar_tasacion', 'Registrar Tasacion', 'pi pi-file-edit', 'Registrar Tasacion', '/home/registrar_tasacion/:id_expediente', false, false, 115),
        ('actividades', 'corregir_reparo_estudio_titulos', 'Corregir Reparo Estudio Titulos', 'pi pi-file-edit', 'Corregir Reparo Estudio Titulos', '/home/corregir_reparo_estudio_titulos/:id_expediente', false, false, 116),
        ('actividades', 'generar_prefiniquito', 'Generar Prefiniquito', 'pi pi-file-edit', 'Generar Prefiniquito', '/home/generar_prefiniquito/:id_expediente', false, false, 117),
        ('actividades', 'corregir_reparo_prefiniquito', 'Corregir Reparo Prefiniquito', 'pi pi-file-edit', 'Corregir Reparo Prefiniquito', '/home/corregir_reparo_prefiniquito/:id_expediente', false, false, 118),
        ('actividades', 'revisar_ingreso_datos_credito', 'Revisar Ingreso Datos Credito', 'pi pi-file-edit', 'Revisar Ingreso Datos Credito', '/home/revisar_ingreso_datos_credito/:id_expediente', false, false, 119),
        ('actividades', 'verificar_reparo_estudio_titulos', 'Verificar Reparo Estudio Titulos', 'pi pi-file-edit', 'Verificar Reparo Estudio Titulos', '/home/verificar_reparo_estudio_titulos/:id_expediente', false, false, 120),
        ('actividades', 'generar_estudio_titulos', 'Generar Estudio Titulos', 'pi pi-file-edit', 'Generar Estudio Titulos', '/home/generar_estudio_titulos/:id_expediente', false, false, 121),
        ('actividades', 'generar_borrador_escritura', 'Generar Borrador Escritura', 'pi pi-file-edit', 'Generar Borrador Escritura', '/home/generar_borrador_escritura/:id_expediente', false, false, 122),
        ('actividades', 'corregir_reparo_visado', 'Corregir Reparo Visado', 'pi pi-file-edit', 'Corregir Reparo Visado', '/home/corregir_reparo_visado/:id_expediente', false, false, 123),
        ('actividades', 'corregir_carta_resguardo', 'Corregir Carta Resguardo', 'pi pi-file-edit', 'Corregir Carta Resguardo', '/home/corregir_carta_resguardo/:id_expediente', false, false, 124),
        ('actividades', 'verificar_reparo_ingreso_datos', 'Verificar Reparo Ingreso Datos', 'pi pi-file-edit', 'Verificar Reparo Ingreso Datos Operacion', '/home/verificar_reparo_ingreso_datos_operacion/:id_expediente', false, false, 125),
        ('actividades', 'realizar_control_credito', 'Realizar Control Credito', 'pi pi-file-edit', 'Realizar Control Credito', '/home/realizar_control_credito/:id_expediente', false, false, 126),
        ('actividades', 'corregir_reparo_control_credito', 'Corregir Reparo Control Credito', 'pi pi-file-edit', 'Corregir Reparo Control Credito', '/home/corregir_reparo_control_credito/:id_expediente', false, false, 127),
        ('actividades', 'aprobacion_comercial_legal_cdr', 'Aprobacion Comercial Legal CdR', 'pi pi-file-edit', 'Aprobacion Comercial Legal CdR', '/home/aprobacion_comercial_legal_cdr/:id_expediente', false, false, 128),
        ('actividades', 'realizar_revision_previo_firma', 'Revision Previo Firma Banco', 'pi pi-file-edit', 'Realizar Revision Previo Firma Banco', '/home/realizar_revision_previo_firma_banco/:id_expediente', false, false, 129),
        ('actividades', 'registrar_escritura_cbr', 'Registrar Escritura CBR', 'pi pi-file-edit', 'Registrar Escritura CBR', '/home/registrar_escritura_cbr/:id_expediente', false, false, 130),
        ('actividades', 'visar_operacion', 'Visar Operacion', 'pi pi-file-edit', 'Visar Operacion', '/home/visar_operacion/:id_expediente', false, false, 131),
        ('actividades', 'recepcionar_matriz', 'Recepcionar Matriz', 'pi pi-file-edit', 'Recepcionar Matriz', '/home/recepcionar_matriz/:id_expediente', false, false, 132),
        ('actividades', 'generar_recursos_pagos_cbr', 'Generar Recursos Pagos CBR', 'pi pi-file-edit', 'Generar Recursos Pagos CBR', '/home/generar_recursos_pagos_cbr/:id_expediente', false, false, 133),
        ('actividades', 'generar_carta_resguardo', 'Generar Carta Resguardo', 'pi pi-file-edit', 'Generar Carta Resguardo', '/home/generar_carta_resguardo/:id_expediente', false, false, 134),
        ('actividades', 'corregir_notaria_reparo_abogados', 'Corregir Notaria Reparo Abogados', 'pi pi-file-edit', 'Corregir Notaria Reparo Abogados', '/home/corregir_notaria_reparo_abogados/:id_expediente', false, false, 135),
        ('actividades', 'registrar_firma_comprador', 'Registrar Firma Comprador', 'pi pi-file-edit', 'Registrar Firma Comprador', '/home/registrar_firma_comprador/:id_expediente', false, false, 136),
        ('actividades', 'registrar_firma_vendedor', 'Registrar Firma Vendedor', 'pi pi-file-edit', 'Registrar Firma Vendedor', '/home/registrar_firma_vendedor/:id_expediente', false, false, 137),
        ('actividades', 'registrar_firma_banco_acreedor', 'Registrar Firma Banco Acreedor', 'pi pi-file-edit', 'Registrar Firma Banco Acreedor CG', '/home/registrar_firma_banco_acreedor_cg/:id_expediente', false, false, 138),
        ('actividades', 'corregir_reparo_cdr', 'Corregir Reparo CDR', 'pi pi-file-edit', 'Corregir Reparo CDR', '/home/corregir_reparo_cdr/:id_expediente', false, false, 139),
        ('actividades', 'corregir_reparo_copias_escrituras', 'Corregir Reparo Copias Escrituras', 'pi pi-file-edit', 'Corregir Reparo Copias Escrituras', '/home/corregir_reparo_copias_escrituras/:id_expediente', false, false, 140),
        ('actividades', 'cierre_copias_notaria', 'Cierre Copias Notaria', 'pi pi-file-edit', 'Cierre Copias Notaria', '/home/cierre_copias_notaria/:id_expediente', false, false, 141),
        ('actividades', 'verificar_correccion_escritura', 'Verificar Correccion Escritura', 'pi pi-file-edit', 'Verificar Correccion Escritura', '/home/verificar_correccion_escritura/:id_expediente', false, false, 142),
        ('actividades', 'corregir_reparo_cierre_copias', 'Corregir Reparo Cierre Copias', 'pi pi-file-edit', 'Corregir Reparo Cierre Copias Notaria', '/home/corregir_reparo_cierre_copias_notaria/:id_expediente', false, false, 143),
        ('actividades', 'verificar_reparo_cbr', 'Verificar Reparo CBR', 'pi pi-file-edit', 'Verificar Reparo CBR', '/home/verificar_reparo_cbr/:id_expediente', false, false, 144),
        ('actividades', 'recibir_instruccion_pago', 'Recibir Instruccion Pago', 'pi pi-file-edit', 'Recibir Instruccion Pago', '/home/recibir_instruccion_pago/:id_expediente', false, false, 145),
        ('actividades', 'entrega_carpeta', 'Entrega Carpeta', 'pi pi-file-edit', 'Entrega Carpeta', '/home/entrega_carpeta/:id_expediente', false, false, 146),
        ('actividades', 'corregir_reparo_inst_pago', 'Corregir Reparo Inst Pago', 'pi pi-file-edit', 'Corregir Reparo Instruccion Pago', '/home/corregir_reparo_inst_pago/:id_expediente', false, false, 147),
        ('actividades', 'corregir_reparos_gestor', 'Corregir Reparos Gestor', 'pi pi-file-edit', 'Corregir Reparos Gestor', '/home/corregir_reparos_gestor/:id_expediente', false, false, 148),
        ('actividades', 'corregir_reparo_entregar_carpeta', 'Corregir Reparo Entregar Carpeta', 'pi pi-file-edit', 'Corregir Reparo Entregar Carpeta', '/home/corregir_reparo_entregar_carpeta/:id_expediente', false, false, 149),
        ('actividades', 'revisar_inscripcion_cbr', 'Revisar Inscripcion CBR', 'pi pi-file-edit', 'Revisar Inscripcion CBR', '/home/revisar_inscripcion_cbr/:id_expediente', false, false, 150),
        ('actividades', 'reingresar_escritura_cbr', 'Reingresar Escritura CBR', 'pi pi-file-edit', 'Reingresar Escritura CBR', '/home/reingresar_escritura_cbr/:id_expediente', false, false, 151),
        ('actividades', 'corregir_reparo_liquidacion', 'Corregir Reparo Liquidacion', 'pi pi-file-edit', 'Corregir Reparo Liquidacion', '/home/corregir_reparo_liquidacion/:id_expediente', false, false, 152),
        ('actividades', 'revisar_liquidacion', 'Revisar Liquidacion', 'pi pi-file-edit', 'Revisar Liquidacion', '/home/revisar_liquidacion/:id_expediente', false, false, 153),
        ('actividades', 'revisar_desembolso', 'Revisar Desembolso', 'pi pi-file-edit', 'Revisar Desembolso', '/home/revisar_desembolso/:id_expediente', false, false, 154),
        ('actividades', 'control_escritura', 'Control Escritura', 'pi pi-file-edit', 'Control Escritura', '/home/control_escritura/:id_expediente', false, false, 155),
        ('actividades', 'corregir_control_escritura', 'Corregir Control Escritura', 'pi pi-file-edit', 'Corregir Control Escritura', '/home/corregir_control_escritura/:id_expediente', false, false, 156),
        ('actividades', 'generar_finiquito', 'Generar Finiquito', 'pi pi-file-edit', 'Generar Finiquito', '/home/generar_finiquito/:id_expediente', false, false, 157),
        ('actividades', 'registrar_firma_apoderado_banco', 'Registrar Firma Apoderado Banco', 'pi pi-file-edit', 'Registrar Firma Apoderado Banco', '/home/registrar_firma_apoderado_banco/:id_expediente', false, false, 158),
        ('actividades', 'reparo_formulario', 'Reparo Formulario', 'pi pi-file-edit', 'Reparo Formulario', '/home/reparo_formulario/:id_expediente', false, false, 159),
        ('actividades', 'rectificatoria_firma', 'Rectificatoria Firma', 'pi pi-file-edit', 'Rectificatoria Firma', '/home/rectificatoria_firma/:id_expediente', false, false, 160),
        ('actividades', 'validacion_rectificatoria_legal', 'Validacion Rectificatoria Legal', 'pi pi-file-edit', 'Validacion Rectificatoria Legal', '/home/validacion_rectificatoria_legal/:id_expediente', false, false, 161),
        ('actividades', 'gestion_rectificatoria', 'Gestion Rectificatoria', 'pi pi-file-edit', 'Gestion Rectificatoria', '/home/gestion_rectificatoria/:id_expediente', false, false, 162),
        ('actividades', 'gestion_rectificatoria_solucion', 'Gestion Rectificatoria Solucion', 'pi pi-file-edit', 'Gestion Rectificatoria Solucion Reparo', '/home/gestion_rectificatoria_solucion_reparo/:id_expediente', false, false, 163),
        ('actividades', 'gestion_reparo', 'Gestion Reparo', 'pi pi-file-edit', 'Gestion Reparo', '/home/gestion_reparo/:id_expediente', false, false, 164),
        ('actividades', 'valorizar_cbr', 'Valorizar CBR', 'pi pi-file-edit', 'Valorizar CBR', '/home/valorizar_cbr/:id_expediente', false, false, 165),
        ('actividades', 'registrar_fecha_registro_cbr', 'Registrar Fecha Registro CBR', 'pi pi-file-edit', 'Registrar Fecha Registro CBR', '/home/registrar_fecha_registro_cbr/:id_expediente', false, false, 166),
        ('actividades', 'rectificatoria_analisis_postventa', 'Rectif. Analisis Reparo Postventa', 'pi pi-file-edit', 'Rectificatoria Analisis Derivacion Reparo Postventa', '/home/rectificatoria_analisis_derivacion_reparo_postventa/:id_expediente', false, false, 167),
        ('actividades', 'rectificatoria_postventa_solucion', 'Rectif. Postventa Solucion Reparo', 'pi pi-file-edit', 'Rectificatoria Postventa Solucion Reparo', '/home/rectificatoria_postventa_solucion_reparo/:id_expediente', false, false, 168),
        ('actividades', 'validacion_rectificatoria_postventa', 'Validacion Rectificatoria Postventa', 'pi pi-file-edit', 'Validacion Rectificatoria Legal Postventa', '/home/validacion_rectificatoria_legal_postventa/:id_expediente', false, false, 169),
        ('actividades', 'gestion_rectificatoria_firmada', 'Gestion Rectif. Escritura Firmada', 'pi pi-file-edit', 'Gestion Rectificatoria Escritura Firmada', '/home/gestion_rectificatoria_escritura_firmada/:id_expediente', false, false, 170),
        ('actividades', 'rectificatoria_legal_carta_resguardo', 'Rectif. Legal Carta Resguardo', 'pi pi-file-edit', 'Rectificatoria Legal Carta Resguardo', '/home/rectificatoria_legal_carta_resguardo/:id_expediente', false, false, 171),
        ('actividades', 'rectificatoria_legal_firma_alzante', 'Rectif. Legal Firma Alzante', 'pi pi-file-edit', 'Rectificatoria Legal Firma Alzante', '/home/rectificatoria_legal_firma_alzante/:id_expediente', false, false, 172),
        ('actividades', 'rectificatoria_legal_cierre_copias', 'Rectif. Legal Cierre Copias', 'pi pi-file-edit', 'Rectificatoria Legal Cierre Copias', '/home/rectificatoria_legal_cierre_copias/:id_expediente', false, false, 173),
        ('actividades', 'rectificatoria_legal_cierre_postventa', 'Rectif. Legal Cierre Copias PV', 'pi pi-file-edit', 'Rectificatoria Legal Cierre Copias Postventa', '/home/rectificatoria_legal_cierre_copias_postventa/:id_expediente', false, false, 174),
        ('actividades', 'gestion_rectificatoria_firmada_pv', 'Gestion Rectif. Escritura Firmada PV', 'pi pi-file-edit', 'Gestion Rectificatoria Escritura Firmada Postventa', '/home/gestion_rectificatoria_escritura_firmada_postventa/:id_expediente', false, false, 175),
        ('actividades', 'rectificatoria_firma_post_venta', 'Rectificatoria Firma Post Venta', 'pi pi-file-edit', 'Rectificatoria Firma Post Venta', '/home/rectificatoria_firma_post_venta/:id_expediente', false, false, 176),
        ('actividades', 'revisar_copias_escrituras', 'Revisar Copias Escrituras', 'pi pi-file-edit', 'Revisar Copias Escrituras', '/home/revisar_copias_escrituras/:id_expediente', false, false, 177);

    UPDATE public.menus m
       SET name = s.name,
           icon_name = s.icon_name,
           description_alt = s.description_alt,
           is_show_navbar = s.is_show_navbar,
           is_show_home_menu = s.is_show_home_menu,
           orden = s.orden,
           is_active = true,
           row_status = true,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM tmp_menu_seed s
     WHERE m.menu_url = s.menu_url;

    INSERT INTO public.menus (
        menu_padre_id,
        name,
        icon_name,
        description_alt,
        menu_url,
        is_show_navbar,
        is_show_home_menu,
        orden,
        is_active,
        row_status,
        created_by,
        created_date
    )
    SELECT NULL,
           s.name,
           s.icon_name,
           s.description_alt,
           s.menu_url,
           s.is_show_navbar,
           s.is_show_home_menu,
           s.orden,
           true,
           true,
           v_audit_user,
           v_now
      FROM tmp_menu_seed s
     WHERE NOT EXISTS (
           SELECT 1
             FROM public.menus m
            WHERE m.menu_url = s.menu_url
       );

    UPDATE public.menus child
       SET menu_padre_id = parent.menu_id,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM tmp_menu_seed child_seed
      JOIN tmp_menu_seed parent_seed
        ON parent_seed.menu_key = child_seed.parent_key
      JOIN public.menus parent
        ON parent.menu_url = parent_seed.menu_url
     WHERE child.menu_url = child_seed.menu_url
       AND child_seed.parent_key IS NOT NULL;

    UPDATE public.menus child
       SET menu_padre_id = NULL,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM tmp_menu_seed child_seed
     WHERE child.menu_url = child_seed.menu_url
       AND child_seed.parent_key IS NULL
       AND child.menu_padre_id IS NOT NULL;

    SELECT r.role_id
      INTO v_role_id
      FROM public.role r
     WHERE upper(r.code) = upper(v_role_code)
        OR upper(r.name) = upper(v_role_name)
     ORDER BY r.role_id
     LIMIT 1;

    IF v_role_id IS NULL THEN
        INSERT INTO public.role (
            code,
            name,
            is_active,
            row_status,
            created_by,
            created_date
        )
        VALUES (
            v_role_code,
            v_role_name,
            true,
            true,
            v_audit_user,
            v_now
        )
        RETURNING role_id INTO v_role_id;
    ELSE
        UPDATE public.role
           SET code = v_role_code,
               name = v_role_name,
               is_active = true,
               row_status = true,
               modified_by = v_audit_user,
               modified_date = v_now
         WHERE role_id = v_role_id;
    END IF;

    UPDATE public.role_menu rm
       SET is_active = true,
           row_status = true,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM public.menus m
     WHERE rm.role_id = v_role_id
       AND rm.menu_id = m.menu_id
       AND m.is_active = true
       AND m.row_status = true
       AND (rm.is_active = false OR rm.row_status = false);

    INSERT INTO public.role_menu (
        role_id,
        menu_id,
        is_active,
        row_status,
        created_by,
        created_date
    )
    SELECT v_role_id,
           m.menu_id,
           true,
           true,
           v_audit_user,
           v_now
      FROM public.menus m
     WHERE m.is_active = true
       AND m.row_status = true
       AND NOT EXISTS (
           SELECT 1
             FROM public.role_menu rm
            WHERE rm.role_id = v_role_id
              AND rm.menu_id = m.menu_id
       );

    SELECT count(*)
      INTO v_total_active_menus
      FROM public.menus m
     WHERE m.is_active = true
       AND m.row_status = true;

    SELECT count(DISTINCT rm.menu_id)
      INTO v_total_assigned_menus
      FROM public.role_menu rm
     WHERE rm.role_id = v_role_id
       AND rm.is_active = true
       AND rm.row_status = true;

    RAISE NOTICE 'Seeded % menus. Role % (%) mapped to %/% active menus.',
        (SELECT count(*) FROM tmp_menu_seed),
        v_role_name,
        v_role_id,
        v_total_assigned_menus,
        v_total_active_menus;
END $$;

COMMIT;

SELECT r.role_id,
       r.code,
       r.name,
       r.is_active,
       r.row_status,
       count(DISTINCT rm.menu_id) AS assigned_active_menus
  FROM public.role r
  LEFT JOIN public.role_menu rm
    ON rm.role_id = r.role_id
   AND rm.is_active = true
   AND rm.row_status = true
 WHERE r.code = 'ADMIN'
 GROUP BY r.role_id, r.code, r.name, r.is_active, r.row_status;

SELECT m.menu_id,
       p.name AS parent_name,
       m.name,
       m.menu_url,
       m.is_show_navbar,
       m.orden
  FROM public.menus m
  LEFT JOIN public.menus p
    ON p.menu_id = m.menu_padre_id
 WHERE m.row_status = true
 ORDER BY COALESCE(p.orden, m.orden), m.menu_padre_id NULLS FIRST, m.orden, m.menu_id;
