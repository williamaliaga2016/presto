-- Base destino: BBVA_LEGALIZACION
-- Proposito: completar Registro Contacto para contactos historicos por expediente y actividad.

ALTER TABLE public.registro_contacto_bbva
    ADD COLUMN IF NOT EXISTS nro_contacto INTEGER,
    ADD COLUMN IF NOT EXISTS detalle_contacto VARCHAR(50),
    ADD COLUMN IF NOT EXISTS inmueble_definido BOOLEAN;

UPDATE public.registro_contacto_bbva rc
SET nro_contacto = ranked.nro_contacto
FROM (
    SELECT
        id,
        ROW_NUMBER() OVER (
            PARTITION BY id_expediente, id_actividad
            ORDER BY fecha_contacto ASC, id ASC
        ) AS nro_contacto
    FROM public.registro_contacto_bbva
    WHERE nro_contacto IS NULL
) ranked
WHERE rc.id = ranked.id;

ALTER TABLE public.registro_contacto_bbva
    ALTER COLUMN nro_contacto SET NOT NULL;

CREATE UNIQUE INDEX IF NOT EXISTS ix_registro_contacto_numero
    ON public.registro_contacto_bbva (id_expediente, id_actividad, nro_contacto)
    WHERE is_active = true AND row_status = true;

WITH l8(codigo, descripcion, orden) AS (
    VALUES
        ('RC-1', 'Cambio De Inmueble', 1),
        ('RC-2', 'En gestion', 2),
        ('RC-3', 'Requiere Cambio de Condiciones', 3),
        ('RC-4', 'Cancelar Gravamen', 4),
        ('RC-5', 'Pendiente Confirmar Intervencion Del Prescriptor', 5),
        ('RC-6', 'Cliente No Contactado', 6),
        ('RC-7', 'Inmueble Definido Entrega Posterior', 7),
        ('RC-8', 'Inmueble No Definido', 8),
        ('RC-9', 'Pendiente Documentos', 9),
        ('RC-10', 'Indeciso', 10)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'RESULTADO_CONTACTO', l8.descripcion, l8.codigo, NULL, true, l8.orden
FROM l8
WHERE NOT EXISTS (
    SELECT 1
    FROM public.catalogo c
    WHERE c.tipo = 'RESULTADO_CONTACTO'
      AND c.valor = l8.codigo
);

WITH l9(codigo, descripcion, padre_codigo, orden) AS (
    VALUES
        ('RCD-1', 'Ilocalizado', 'RC-6', 1),
        ('RCD-2', 'Telefono Fuera de Servicio', 'RC-6', 2),
        ('RCD-3', 'No Contesta', 'RC-6', 3),
        ('RCD-4', 'Llamada Finalizada Por El Cliente', 'RC-6', 4),
        ('RCD-5', 'Cliente', 'RC-9', 5),
        ('RCD-6', 'Constructor', 'RC-9', 6),
        ('RCD-7', 'Oficina/Comercial', 'RC-9', 7),
        ('RCD-8', 'Prescriptor', 'RC-9', 8),
        ('RCD-9', 'Desconoce Aprobacion', 'RC-10', 9),
        ('RCD-10', 'Desconoce Intervencion Prescriptor/ Constructora', 'RC-10', 10),
        ('RCD-11', 'Condiciones de aprobacion No Acorde A Sus Necesidades', 'RC-10', 11),
        ('RCD-12', 'Monto Aprobado Insuficiente', 'RC-10', 12),
        ('RCD-13', 'Cambio De Planes O Necesidades', 'RC-10', 13)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'DETALLE_CONTACTO', l9.descripcion, l9.codigo, padre.id, true, l9.orden
FROM l9
JOIN public.catalogo padre
    ON padre.tipo = 'RESULTADO_CONTACTO'
   AND padre.valor = l9.padre_codigo
WHERE NOT EXISTS (
    SELECT 1
    FROM public.catalogo c
    WHERE c.tipo = 'DETALLE_CONTACTO'
      AND c.valor = l9.codigo
);

-- Validacion sugerida:
-- SELECT id_expediente, id_actividad, nro_contacto, resultado_contacto, detalle_contacto, inmueble_definido
-- FROM public.registro_contacto_bbva
-- ORDER BY id_expediente, id_actividad, nro_contacto;
--
-- SELECT c.tipo, c.valor, c.descripcion, p.valor AS parent_code
-- FROM public.catalogo c
-- LEFT JOIN public.catalogo p ON p.id = c.id_padre
-- WHERE c.tipo IN ('RESULTADO_CONTACTO', 'DETALLE_CONTACTO')
-- ORDER BY c.tipo, c.orden, c.id;
