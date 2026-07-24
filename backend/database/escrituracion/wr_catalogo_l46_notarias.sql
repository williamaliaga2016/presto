-- L46 — Notarías (Escrituración y Garantías)
-- Base destino: BBVA_LEGALIZACION
-- Script idempotente

WITH l46(codigo, descripcion, orden) AS (
    VALUES
        ('NOT-1', 'Unica - Leticia', 1),
        ('NOT-2', 'Unica - Abejorral', 2),
        ('NOT-3', 'Unica - Amagá', 3)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L46_NOTARIAS', l46.descripcion, l46.codigo, NULL, true, l46.orden
FROM l46
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L46_NOTARIAS' AND c.valor = l46.codigo
);
