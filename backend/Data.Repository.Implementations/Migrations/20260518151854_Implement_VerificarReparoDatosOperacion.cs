using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_VerificarReparoDatosOperacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "verificar_reparo_datos_operacion",
                columns: table => new
                {
                    id_verificar_reparo_datos_operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verificar_reparo_datos_operacion", x => x.id_verificar_reparo_datos_operacion);
                });

            migrationBuilder.CreateIndex(
                name: "ix_verificar_reparo_datos_operacion_expediente",
                table: "verificar_reparo_datos_operacion",
                column: "id_expediente");

            migrationBuilder.CreateTable(
                name: "verificar_reparo_datos_operacion_detalle",
                columns: table => new
                {
                    id_verificar_reparo_datos_operacion_detalle = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("PK_verificar_reparo_datos_operacion_detalle", x => x.id_verificar_reparo_datos_operacion_detalle);
                });

            migrationBuilder.CreateIndex(
                name: "ix_verificar_reparo_datos_operacion_detalle_expediente",
                table: "verificar_reparo_datos_operacion_detalle",
                column: "id_expediente");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION public.usp_select_verificar_reparo_datos_operacion_detalle(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_verificar_reparo_datos_operacion_detalle integer,
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
        r.id_verificar_reparo_datos_operacion_detalle,
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
    FROM public.verificar_reparo_datos_operacion_detalle r
    WHERE r.id_expediente = p_id_expediente
      AND r.is_active     = true
      AND r.row_status    = true
    ORDER BY r.id_verificar_reparo_datos_operacion_detalle DESC
    LIMIT 1;
$$;
");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION public.usp_select_verificar_reparo_datos_operacion(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_verificar_reparo_datos_operacion integer,
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
        v.id_verificar_reparo_datos_operacion,
        v.id_expediente,
        v.observaciones,
        v.is_active,
        v.row_status,
        v.created_by,
        v.created_date,
        v.modified_by,
        v.modified_date
    FROM public.verificar_reparo_datos_operacion v
    WHERE v.id_expediente = p_id_expediente
      AND v.is_active     = true
      AND v.row_status    = true
    ORDER BY v.id_verificar_reparo_datos_operacion DESC
    LIMIT 1;
$$;
");

            migrationBuilder.Sql(@"
INSERT INTO public.cat_actividades_ws (
    actividad, id_actividad, id_proceso, proceso, id_role, tipo, page, etapa,
    tiempo_promedio, is_active, row_status, created_by, created_date
) SELECT
    'Verificar Reparo Ingresar Datos Operación',
    '_V8mQcK3xEejBd7_X9pL5s',
    '_yYE04AsMEeaJXe4_X2YDKw',
    'Principal',
    1,
    'actividad',
    'verificar_reparo_ingreso_datos_operacion',
    '1',
    1, true, true, 'ADMIN', CURRENT_TIMESTAMP
WHERE NOT EXISTS (
    SELECT 1 FROM public.cat_actividades_ws
    WHERE id_actividad = '_V8mQcK3xEejBd7_X9pL5s'
);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DELETE FROM public.cat_actividades_ws
WHERE id_actividad = '_V8mQcK3xEejBd7_X9pL5s';

DROP FUNCTION IF EXISTS public.usp_select_verificar_reparo_datos_operacion(bigint);
DROP FUNCTION IF EXISTS public.usp_select_verificar_reparo_datos_operacion_detalle(bigint);
");

            migrationBuilder.DropTable(
                name: "verificar_reparo_datos_operacion_detalle");

            migrationBuilder.DropTable(
                name: "verificar_reparo_datos_operacion");
        }
    }
}
