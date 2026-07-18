-- Ajusta Registro Contacto para comportamiento transversal global por expediente.
-- No ejecutar sin respaldo y validacion funcional previa.

UPDATE public.registro_contacto_bbva rc
SET nro_contacto = ranked.nro_contacto
FROM (
    SELECT
        id,
        ROW_NUMBER() OVER (
            PARTITION BY id_expediente
            ORDER BY fecha_contacto ASC NULLS LAST, created_date ASC NULLS LAST, id ASC
        ) AS nro_contacto
    FROM public.registro_contacto_bbva
    WHERE is_active = true
      AND row_status = true
) ranked
WHERE rc.id = ranked.id
  AND rc.nro_contacto IS DISTINCT FROM ranked.nro_contacto;

DROP INDEX IF EXISTS public.ix_registro_contacto_numero;

CREATE UNIQUE INDEX IF NOT EXISTS ix_registro_contacto_numero_expediente
    ON public.registro_contacto_bbva (id_expediente, nro_contacto)
    WHERE is_active = true
      AND row_status = true;

-- Validacion sugerida:
-- SELECT id_expediente, nro_contacto, COUNT(*)
-- FROM public.registro_contacto_bbva
-- WHERE is_active = true
--   AND row_status = true
-- GROUP BY id_expediente, nro_contacto
-- HAVING COUNT(*) > 1;

-- SELECT id_expediente, id_actividad, nro_contacto, fecha_contacto
-- FROM public.registro_contacto_bbva
-- WHERE is_active = true
--   AND row_status = true
-- ORDER BY id_expediente, nro_contacto;
