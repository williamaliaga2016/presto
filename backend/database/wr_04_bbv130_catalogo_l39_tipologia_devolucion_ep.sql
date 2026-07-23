-- BBV-130 — Revisar EP Abogado: Catálogo L39
-- Script idempotente para poblar parametría de Tipología de Devolución Escritura Abogado.
-- ============================================================

-- L39 — Tipificación Devolución Escritura Abogado
WITH l39(codigo, descripcion, orden) AS (
    VALUES
        ('TDEPA-1', 'Solicitud Corrección Escritura', 1)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L39_TIPOLOGIA_DEVOLUCION_EP_ABOGADO', l39.descripcion, l39.codigo, NULL, true, l39.orden
FROM l39
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L39_TIPOLOGIA_DEVOLUCION_EP_ABOGADO' AND c.valor = l39.codigo
);
