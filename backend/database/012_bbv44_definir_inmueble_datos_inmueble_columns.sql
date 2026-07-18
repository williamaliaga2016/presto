-- BBV-44 - Columnas editables de Datos del Inmueble.
--
-- Base destino: BBVA_LEGALIZACION.
--
-- Agrega de forma idempotente las columnas usadas por DatosInmuebleSection en la
-- tabla propia de la actividad Definir Inmueble. Este script cubre ambientes donde
-- se aplico una version previa de 008_bbv44_bbva_legalizacion.sql.

ALTER TABLE public.definir_inmueble_bbva
    ADD COLUMN IF NOT EXISTS inmueble_definido BOOLEAN,
    ADD COLUMN IF NOT EXISTS tipo_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS estado_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS es_constructora_vip BOOLEAN,
    ADD COLUMN IF NOT EXISTS codigo_proyecto VARCHAR(50),
    ADD COLUMN IF NOT EXISTS descripcion_proyecto VARCHAR(300),
    ADD COLUMN IF NOT EXISTS departamento_inmueble VARCHAR(50),
    ADD COLUMN IF NOT EXISTS municipio_inmueble VARCHAR(50);

-- Validacion posterior sugerida:
--
-- SELECT column_name, data_type, character_maximum_length, is_nullable
-- FROM information_schema.columns
-- WHERE table_schema = 'public'
--   AND table_name = 'definir_inmueble_bbva'
--   AND column_name IN (
--       'inmueble_definido',
--       'tipo_inmueble',
--       'estado_inmueble',
--       'es_constructora_vip',
--       'codigo_proyecto',
--       'descripcion_proyecto',
--       'departamento_inmueble',
--       'municipio_inmueble'
--   )
-- ORDER BY column_name;
