-- BBV-107 — Revisar Marcación de Cobertura
-- Catálogos de Tipo de Vivienda y Estado Proceso.
-- Reemplazan las listas fijas que traía el frontend en
-- SeccionMarcacionCobertura.tsx (ver contexto/BBV-107_RECETA_IMPLEMENTACION.md
-- sección 1.4: "se revisará al implementar si ya existen catálogos reutilizables").
-- Script idempotente.
-- ============================================================

-- TIPO_VIVIENDA_BBV107
WITH tipo_vivienda(codigo, descripcion, orden) AS (
    VALUES
        ('VIS', 'Vivienda de Interés Social', 1),
        ('VIP', 'Vivienda de Interés Prioritario', 2),
        ('NO_VIS', 'No VIS', 3)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'TIPO_VIVIENDA_BBV107', tipo_vivienda.descripcion, tipo_vivienda.codigo, NULL, true, tipo_vivienda.orden
FROM tipo_vivienda
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'TIPO_VIVIENDA_BBV107' AND c.valor = tipo_vivienda.codigo
);

-- ESTADO_PROCESO_BBV107
WITH estado_proceso(codigo, descripcion, orden) AS (
    VALUES
        ('EN_TRAMITE', 'En Trámite', 1),
        ('APROBADO', 'Aprobado', 2),
        ('RECHAZADO', 'Rechazado', 3)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESTADO_PROCESO_BBV107', estado_proceso.descripcion, estado_proceso.codigo, NULL, true, estado_proceso.orden
FROM estado_proceso
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'ESTADO_PROCESO_BBV107' AND c.valor = estado_proceso.codigo
);
