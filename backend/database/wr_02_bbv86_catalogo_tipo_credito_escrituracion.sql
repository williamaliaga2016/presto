-- ============================================================
-- BBV-86 — Firmar Escritura Cliente
-- Catálogos de tipo de crédito para enrutamiento de escrituración
-- ============================================================

-- ESCRITURACION_TIPO_CREDITO (CA02, CA05): todos los tipos de crédito que requieren escrituración
-- PENDIENTE: Faltan Leasing Nuevo, Leasing Usado, Leasing CXI, Remodelación para Ampliar/Hipotecar (no existen aún en TIPO_CREDITO, agregar cuando se creen)
-- Ojo estos campos los valores fueron sacados de Catalogo = TIPO_CREDITO
-- Refieren a los tipos de credito especificos que requieren ESCRITURACION (sería un subgrupo de TIPO_CREDITO)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO', 'Constructor Individual', 'CONSTRUCTOR_INDIVIDUAL_V1', NULL, true, 1);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO', 'Hipotecario Nuevo', 'HIPOTECARIO_NUEVO', NULL, true, 2);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO', 'Hipotecario Usado', 'HIPOTECARIO_USADO', NULL, true, 3);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO', 'Hipotecario CXI', 'HIPOTECARIO_CXI', NULL, true, 4);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO', 'Leaseback Habitacional', 'LEASEBACK_HABITACIONES_V1', NULL, true, 5);

-- ESCRITURACION_TIPO_CREDITO_CXI (CA04, CA09): productos que aplican para VB Prorrata (Gestión CXI)
-- PENDIENTE: Falta LEASING_CXI (no existe aún en TIPO_CREDITO)
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO_CXI', 'Hipotecario CXI', 'HIPOTECARIO_CXI', NULL, true, 1);

-- ESCRITURACION_TIPO_CREDITO_LEASING (CA04, CA09): productos que aplican para Gestión Leasing / Causar
-- PENDIENTE: Faltan Leasing Nuevo, Leasing Usado, Leasing CXI (no existen aún en TIPO_CREDITO)
-- NOTA: LEASEBACK_HABITACIONES_V1 es un placeholder temporal para pruebas, NO es un tipo Leasing real según la HU.
--       Remover cuando se creen los tipos correctos.
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('ESCRITURACION_TIPO_CREDITO_LEASING', 'Leaseback Habitacional', 'LEASEBACK_HABITACIONES_V1', NULL, true, 1);

-- TIPOLOGIA_ESCALAMIENTO (CA03): tipologías cuando escalamiento comercial = SI
-- Valores sacados de L4 — Tipología de Escalamiento
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('TIPOLOGIA_ESCALAMIENTO', 'Cliente no responde', 'MV-3', NULL, true, 1);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('TIPOLOGIA_ESCALAMIENTO', 'Documentos Incompletos', 'MV-4', NULL, true, 2);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('TIPOLOGIA_ESCALAMIENTO', 'Carta de aprobación Próxima a Vencer', 'MV-8', NULL, true, 3);

-- ============================================================
-- L38 — Representante Legal (CA08)
-- TODO: Si se requiere el campo "correo" en el futuro, migrar a una tabla
-- propia "representante_legal" (codigo, nombre, correo, habilitado).
-- Por ahora se usa catalogo con code=código empleado, description=nombre.
-- Revisar en excel para agregar todos 
-- ============================================================
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('L38_REPRESENTANTE_LEGAL', 'Andres Guillermo Gordon', 'C797509', NULL, true, 1);
INSERT INTO public.catalogo (tipo, descripcion, valor, id_padre, is_active, orden) VALUES ('L38_REPRESENTANTE_LEGAL', 'Ruben Dario Nieto Estevez', 'C782723', NULL, true, 2);
