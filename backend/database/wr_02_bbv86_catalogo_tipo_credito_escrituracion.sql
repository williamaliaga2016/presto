-- ============================================================
-- BBV-86 — Firmar Escritura Cliente
-- Catálogos de tipo de crédito para enrutamiento de escrituración
--
-- NOTA: Los valores (campo "valor") deben coincidir exactamente
-- con los registros del catálogo tipo "TIPO_PRESTAMO".
-- Estos catálogos agrupan los tipos de préstamo según las reglas
-- de enrutamiento de CA04 (CXI → VB Prorrata, Leasing → Causación).
-- ============================================================

-- ESCRITURACION_TIPO_CREDITO_LEASING: productos que aplican para Gestión Leasing / Causar
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO_LEASING', 'Leasing Nuevo', 'LEASING_NUEVO', NULL, true, 1
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO_LEASING' AND valor = 'LEASING_NUEVO'
);

INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO_LEASING', 'Leasing Usado', 'LEASING_USADO', NULL, true, 2
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO_LEASING' AND valor = 'LEASING_USADO'
);

-- ESCRITURACION_TIPO_CREDITO_CXI: productos que aplican para VB Prorrata (Gestión CXI)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO_CXI', 'Hipotecario CXI', 'HIPOTECARIO_CXI', NULL, true, 1
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO_CXI' AND valor = 'HIPOTECARIO_CXI'
);

INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO_CXI', 'Leasing CXI', 'LEASING_CXI', NULL, true, 2
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO_CXI' AND valor = 'LEASING_CXI'
);

-- ESCRITURACION_TIPO_CREDITO: todos los tipos de crédito que requieren escrituración (CA02/CA05)
-- Se usa como guard en el frontend para verificar si la actividad aplica al expediente
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO', 'Constructor Individual', 'CONSTRUCTOR_INDIVIDUAL', NULL, true, 1
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO' AND valor = 'CONSTRUCTOR_INDIVIDUAL'
);

INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden)
SELECT 'ESCRITURACION_TIPO_CREDITO', 'Hipotecario Nuevo', 'HIPOTECARIO_NUEVO', NULL, true, 2
WHERE NOT EXISTS (
    SELECT 1 FROM public.catalogo WHERE tipo = 'ESCRITURACION_TIPO_CREDITO' AND valor = 'HIPOTECARIO_NUEVO'
);
