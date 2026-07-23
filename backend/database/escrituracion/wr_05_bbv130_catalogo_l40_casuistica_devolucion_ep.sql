-- BBV-130 — Revisar EP Abogado: Catálogo L40
-- Script idempotente para poblar parametría de Casuística de Devolución Escritura Abogado.
-- ============================================================

-- L40 — Casuística Devolución Escritura Abogado
WITH l40(codigo, descripcion, orden) AS (
    VALUES
        ('CDEPA-1', 'Informacion Errada cliente', 1),
        ('CDEPA-2', 'Informacion errada datos Inmueble', 2),
        ('CDEPA-3', 'Vencimiento aprobacion', 3),
        ('CDEPA-4', 'Escritura no firmada por las partes', 4),
        ('CDEPA-5', 'Informacion errada Valor y forma de pago', 5),
        ('CDEPA-6', 'Requiere cambio de representate legal', 6),
        ('CDEPA-7', 'A solicitud del representante Legal', 7)
)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'L40_CASUISTICA_DEVOLUCION_EP_ABOGADO', l40.descripcion, l40.codigo, NULL, true, l40.orden
FROM l40
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo c
    WHERE c.tipo = 'L40_CASUISTICA_DEVOLUCION_EP_ABOGADO' AND c.valor = l40.codigo
);
