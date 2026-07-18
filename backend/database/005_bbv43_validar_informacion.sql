-- BBV-43 - Validar Informacion (Contacto Cliente)
-- Script generado para revision/aplicacion manual. No se ejecuta desde el codigo.

ALTER TABLE IF EXISTS public.validar_informacion_bbva
  ADD COLUMN IF NOT EXISTS fecha_estimada_entrega DATE,
  ADD COLUMN IF NOT EXISTS garantia_constituida BOOLEAN,
  ADD COLUMN IF NOT EXISTS monto_otorgado_vivienda_original NUMERIC(18,2),
  ADD COLUMN IF NOT EXISTS origen_devolucion VARCHAR(50),
  ADD COLUMN IF NOT EXISTS codigo_oficina VARCHAR(50),
  ADD COLUMN IF NOT EXISTS descripcion_oficina VARCHAR(200),
  ADD COLUMN IF NOT EXISTS codigo_asesor VARCHAR(50);

CREATE TABLE IF NOT EXISTS public.titular_bbva (
  id BIGSERIAL PRIMARY KEY,
  id_expediente BIGINT NOT NULL,
  id_actividad VARCHAR(50),
  numero_titular INTEGER NOT NULL,
  tipo_identificacion VARCHAR(20),
  numero_identificacion VARCHAR(30),
  nombre_completo VARCHAR(200),
  celular_cliente VARCHAR(20),
  telefono_residente VARCHAR(20),
  email VARCHAR(150),
  direccion_residencia VARCHAR(300),
  telefono_declarativo VARCHAR(20),
  correo_declarativo VARCHAR(150),
  is_active BOOLEAN NOT NULL DEFAULT TRUE,
  row_status BOOLEAN NOT NULL DEFAULT TRUE,
  created_by INTEGER NOT NULL DEFAULT 0,
  created_date TIMESTAMP WITHOUT TIME ZONE NOT NULL DEFAULT NOW(),
  modified_by INTEGER,
  modified_date TIMESTAMP WITHOUT TIME ZONE
);

CREATE INDEX IF NOT EXISTS "IX_titular_bbva_expediente"
  ON public.titular_bbva (id_expediente);

ALTER TABLE IF EXISTS public.registro_contacto_bbva
  ADD COLUMN IF NOT EXISTS numero_contacto INTEGER,
  ADD COLUMN IF NOT EXISTS detalle VARCHAR(200),
  ADD COLUMN IF NOT EXISTS inmueble_definido BOOLEAN;

UPDATE public.registro_contacto_bbva rc
SET numero_contacto = seq.numero_contacto
FROM (
  SELECT
    id,
    ROW_NUMBER() OVER (
      PARTITION BY id_expediente
      ORDER BY fecha_contacto ASC, created_date ASC, id ASC
    ) AS numero_contacto
  FROM public.registro_contacto_bbva
  WHERE numero_contacto IS NULL
) seq
WHERE rc.id = seq.id;

CREATE INDEX IF NOT EXISTS "IX_registro_contacto_bbva_expediente_fecha"
  ON public.registro_contacto_bbva (id_expediente, fecha_contacto DESC, created_date DESC);

-- Consulta usada por la aplicacion para el consecutivo por folio:
-- SELECT COALESCE(MAX(numero_contacto), 0) + 1 AS siguiente_numero_contacto
-- FROM public.registro_contacto_bbva
-- WHERE id_expediente = @idExpediente;
