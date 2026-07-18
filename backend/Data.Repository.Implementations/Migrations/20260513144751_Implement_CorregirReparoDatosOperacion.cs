using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_CorregirReparoDatosOperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // La tabla corregir_reparo_datos_operacion ya existe (creada en 20260512171121
            // AddCorregirReparoDatosOperacionTable). Ampliamos observaciones de 1000 a 2000
            // y removemos las columnas que ya no usa el código (id_usuario_solicitante,
            // is_subsanar) — el modelo de 5.12 mueve esos datos a la tabla detalle.
            migrationBuilder.Sql(@"
ALTER TABLE public.corregir_reparo_datos_operacion
    ALTER COLUMN observaciones TYPE character varying(2000);

ALTER TABLE public.corregir_reparo_datos_operacion
    DROP COLUMN IF EXISTS id_usuario_solicitante;

ALTER TABLE public.corregir_reparo_datos_operacion
    DROP COLUMN IF EXISTS is_subsanar;
");

            migrationBuilder.CreateTable(
                name: "reparo_datos_operacion_detalle",
                columns: table => new
                {
                    id_reparo_datos_operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reparo_datos_operacion_detalle", x => x.id_reparo_datos_operacion);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reparo_datos_operacion_detalle_expediente",
                table: "reparo_datos_operacion_detalle",
                column: "id_expediente");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION public.usp_select_corregir_reparo_datos_operacion(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_corregir_reparo_datos_operacion integer,
    id_expediente bigint,
    observaciones character varying,
    is_active boolean,
    row_status boolean,
    created_by integer,
    created_date timestamp without time zone,
    modified_by integer,
    modified_date timestamp without time zone
)
LANGUAGE sql
AS $$
    SELECT
        c.id_corregir_reparo_datos_operacion,
        c.id_expediente,
        c.observaciones,
        c.is_active,
        c.row_status,
        c.created_by,
        c.created_date,
        c.modified_by,
        c.modified_date
    FROM public.corregir_reparo_datos_operacion c
    WHERE c.id_expediente = p_id_expediente
      AND c.is_active = true
      AND c.row_status = true
    ORDER BY c.id_corregir_reparo_datos_operacion DESC
    LIMIT 1;
$$;

CREATE OR REPLACE FUNCTION public.usp_select_reparo_datos_operacion_detalle(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_reparo_datos_operacion integer,
    id_expediente bigint,
    solicitante character varying,
    observaciones character varying,
    fecha_ingreso timestamp without time zone,
    subsanar boolean,
    is_active boolean,
    row_status boolean,
    created_by integer,
    created_date timestamp without time zone,
    modified_by integer,
    modified_date timestamp without time zone
)
LANGUAGE sql
AS $$
    SELECT
        r.id_reparo_datos_operacion,
        r.id_expediente,
        r.solicitante,
        r.observaciones,
        r.fecha_ingreso,
        r.subsanar,
        r.is_active,
        r.row_status,
        r.created_by,
        r.created_date,
        r.modified_by,
        r.modified_date
    FROM public.reparo_datos_operacion_detalle r
    WHERE r.id_expediente = p_id_expediente
      AND r.is_active = true
      AND r.row_status = true
    ORDER BY r.id_reparo_datos_operacion DESC;
$$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP FUNCTION IF EXISTS public.usp_select_reparo_datos_operacion_detalle(bigint);
DROP FUNCTION IF EXISTS public.usp_select_corregir_reparo_datos_operacion(bigint);
");

            migrationBuilder.DropTable(
                name: "reparo_datos_operacion_detalle");

            migrationBuilder.Sql(@"
ALTER TABLE public.corregir_reparo_datos_operacion
    ADD COLUMN IF NOT EXISTS id_usuario_solicitante integer NOT NULL DEFAULT 0;

ALTER TABLE public.corregir_reparo_datos_operacion
    ADD COLUMN IF NOT EXISTS is_subsanar boolean NOT NULL DEFAULT false;

ALTER TABLE public.corregir_reparo_datos_operacion
    ALTER COLUMN observaciones TYPE character varying(1000);
");
        }
    }
}
