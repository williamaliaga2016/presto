-- Creates Comercial and Coordinador roles from "Permisos y Roles.xlsx".
-- Prerequisite: run 001_create_admin_role.sql to seed the frontend menus.
-- The script is idempotent and synchronizes these roles to the exact permission set below.

BEGIN;

DO $$
DECLARE
    v_audit_user integer := 1;
    v_now timestamp without time zone := now()::timestamp without time zone;
    v_missing_urls text;
BEGIN
    IF to_regclass('public.role') IS NULL
       OR to_regclass('public.menus') IS NULL
       OR to_regclass('public.role_menu') IS NULL THEN
        RAISE EXCEPTION
            'Required tables role, menus, or role_menu do not exist. Apply EF Core migrations first.';
    END IF;

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

    CREATE TEMP TABLE tmp_role_seed (
        role_code varchar(50) NOT NULL,
        role_name varchar(50) NOT NULL
    ) ON COMMIT DROP;

    INSERT INTO tmp_role_seed (role_code, role_name)
    VALUES
        ('COMERCIAL', 'Comercial'),
        ('COORDINADOR', 'Coordinador');

    -- Expediente Digital is embedded in activity pages and has no standalone frontend route.
    -- This hidden row acts as its explicit permission marker.
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
           'Expediente Digital',
           'pi pi-folder',
           'Permiso de acceso a Expediente Digital',
           '#expediente-digital',
           false,
           false,
           60,
           true,
           true,
           v_audit_user,
           v_now
     WHERE NOT EXISTS (
         SELECT 1
           FROM public.menus m
          WHERE m.menu_url = '#expediente-digital'
     );

    UPDATE public.menus
       SET name = 'Expediente Digital',
           icon_name = 'pi pi-folder',
           description_alt = 'Permiso de acceso a Expediente Digital',
           is_show_navbar = false,
           is_show_home_menu = false,
           is_active = true,
           row_status = true,
           modified_by = v_audit_user,
           modified_date = v_now
     WHERE menu_url = '#expediente-digital';

    CREATE TEMP TABLE tmp_role_permission (
        role_code varchar(50) NOT NULL,
        menu_url varchar(150) NOT NULL,
        source_permission varchar(100) NOT NULL
    ) ON COMMIT DROP;

    INSERT INTO tmp_role_permission (role_code, menu_url, source_permission)
    VALUES
        -- Comercial: Inicio + Bandeja (all tabs share the same route).
        ('COMERCIAL', '/home/bandeja', 'Inicio; Bandeja completa'),
        -- Comercial: Radicacion.
        ('COMERCIAL', '/home/carga_operacion_banco', 'Radicacion'),
        ('COMERCIAL', '/home/carga_operacion_banco/:id_expediente', 'Radicacion - detalle'),
        -- Comercial: Consulta de Folios.
        ('COMERCIAL', '/home/consulta_actividades', 'Consulta de Folios'),

        -- Coordinador: Inicio.
        ('COORDINADOR', '/home/bandeja', 'Inicio'),
        -- Coordinador: Radicacion.
        ('COORDINADOR', '/home/carga_operacion_banco', 'Radicacion'),
        ('COORDINADOR', '/home/carga_operacion_banco/:id_expediente', 'Radicacion - detalle'),
        -- Coordinador: Consulta de Folios.
        ('COORDINADOR', '/home/consulta_actividades', 'Consulta de Folios'),
        -- Coordinador: Reportes / Gestion de Garantias.
        ('COORDINADOR', '/home/reportes', 'Reportes - Gestion de Garantias'),
        -- Coordinador: Soporte / Reasignacion Masiva e Individual.
        ('COORDINADOR', '#administracion', 'Contenedor de Soporte'),
        ('COORDINADOR', '/home/reasignacion-desestimacion', 'Reasignacion Masiva e Individual'),
        -- Coordinador: Expediente Digital.
        ('COORDINADOR', '#expediente-digital', 'Expediente Digital');

    SELECT string_agg(required.menu_url, ', ' ORDER BY required.menu_url)
      INTO v_missing_urls
      FROM (
          SELECT DISTINCT p.menu_url
            FROM tmp_role_permission p
           WHERE NOT EXISTS (
               SELECT 1
                 FROM public.menus m
                WHERE m.menu_url = p.menu_url
           )
      ) required;

    IF v_missing_urls IS NOT NULL THEN
        RAISE EXCEPTION
            'Missing required menu URLs: %. Run 001_create_admin_role.sql first.',
            v_missing_urls;
    END IF;

    -- Reactivate/update roles already present by code or name.
    UPDATE public.role r
       SET code = s.role_code,
           name = s.role_name,
           is_active = true,
           row_status = true,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM tmp_role_seed s
     WHERE upper(r.code) = upper(s.role_code)
        OR upper(r.name) = upper(s.role_name);

    INSERT INTO public.role (
        code,
        name,
        is_active,
        row_status,
        created_by,
        created_date
    )
    SELECT s.role_code,
           s.role_name,
           true,
           true,
           v_audit_user,
           v_now
      FROM tmp_role_seed s
     WHERE NOT EXISTS (
         SELECT 1
           FROM public.role r
          WHERE upper(r.code) = upper(s.role_code)
             OR upper(r.name) = upper(s.role_name)
     );

    -- Remove stale permissions so both roles match the Excel exactly.
    UPDATE public.role_menu rm
       SET is_active = false,
           row_status = false,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM public.role r
     WHERE rm.role_id = r.role_id
       AND r.code IN ('COMERCIAL', 'COORDINADOR')
       AND NOT EXISTS (
           SELECT 1
             FROM tmp_role_permission p
             JOIN public.menus m
               ON m.menu_url = p.menu_url
            WHERE p.role_code = r.code
              AND m.menu_id = rm.menu_id
       )
       AND (rm.is_active = true OR rm.row_status = true);

    -- Reactivate required mappings that already exist.
    UPDATE public.role_menu rm
       SET is_active = true,
           row_status = true,
           modified_by = v_audit_user,
           modified_date = v_now
      FROM public.role r,
           public.menus m,
           tmp_role_permission p
     WHERE r.code = p.role_code
       AND m.menu_url = p.menu_url
       AND rm.role_id = r.role_id
       AND rm.menu_id = m.menu_id
       AND (rm.is_active = false OR rm.row_status = false);

    -- Insert required mappings that do not exist yet.
    INSERT INTO public.role_menu (
        role_id,
        menu_id,
        is_active,
        row_status,
        created_by,
        created_date
    )
    SELECT r.role_id,
           m.menu_id,
           true,
           true,
           v_audit_user,
           v_now
      FROM tmp_role_permission p
      JOIN public.role r
        ON r.code = p.role_code
      JOIN public.menus m
        ON m.menu_url = p.menu_url
     WHERE NOT EXISTS (
         SELECT 1
           FROM public.role_menu rm
          WHERE rm.role_id = r.role_id
            AND rm.menu_id = m.menu_id
     );

    RAISE NOTICE 'Comercial and Coordinador roles synchronized successfully.';
END $$;

COMMIT;

SELECT r.role_id,
       r.code,
       r.name,
       count(DISTINCT rm.menu_id) FILTER (
           WHERE rm.is_active = true AND rm.row_status = true
       ) AS active_permissions
  FROM public.role r
  LEFT JOIN public.role_menu rm
    ON rm.role_id = r.role_id
 WHERE r.code IN ('COMERCIAL', 'COORDINADOR')
 GROUP BY r.role_id, r.code, r.name
 ORDER BY r.code;

SELECT r.code,
       m.name AS menu_name,
       m.menu_url
  FROM public.role r
  JOIN public.role_menu rm
    ON rm.role_id = r.role_id
   AND rm.is_active = true
   AND rm.row_status = true
  JOIN public.menus m
    ON m.menu_id = rm.menu_id
 WHERE r.code IN ('COMERCIAL', 'COORDINADOR')
 ORDER BY r.code, m.orden, m.menu_id;
