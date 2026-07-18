using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_CorregirReparoGenerarMemoEscritura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "corregir_reparo_generar_memo_escritura",
                columns: table => new
                {
                    id_corregir_reparo_generar_memo_escritura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_corregir_reparo_generar_memo_escritura", x => x.id_corregir_reparo_generar_memo_escritura);
                });

            migrationBuilder.CreateIndex(
                name: "ix_corregir_reparo_generar_memo_escritura_expediente",
                table: "corregir_reparo_generar_memo_escritura",
                column: "id_expediente");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION usp_select_corregir_reparo_generar_memo_escritura(
    p_id_expediente bigint
)
RETURNS TABLE (
    id_corregir_reparo_generar_memo_escritura integer,
    id_expediente bigint,
    is_subsanar boolean,
    observaciones character varying,
    is_active boolean,
    row_status boolean,
    created_by integer,
    created_date timestamp without time zone,
    modified_by integer,
    modified_date timestamp without time zone
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        crgme.id_corregir_reparo_generar_memo_escritura,
        crgme.id_expediente,
        crgme.is_subsanar,
        crgme.observaciones,
        crgme.is_active,
        crgme.row_status,
        crgme.created_by,
        crgme.created_date,
        crgme.modified_by,
        crgme.modified_date
    FROM corregir_reparo_generar_memo_escritura crgme
    WHERE crgme.id_expediente = p_id_expediente
      AND crgme.is_active = true
      AND crgme.row_status = true
    ORDER BY crgme.id_corregir_reparo_generar_memo_escritura DESC
    LIMIT 1;
END;
$$;");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION usp_select_reparo_generar_memo_escritura(
    p_id_expediente bigint,
    p_id_documento integer DEFAULT 99
)
RETURNS TABLE (
    id_reparo_generar_memo_escritura bigint,
    id_expediente bigint,
    solicitante text,
    observaciones text,
    fecha_ingreso timestamp without time zone,
    subsanar boolean
)
LANGUAGE plpgsql
AS $$
BEGIN
    RETURN QUERY
    SELECT
        ed.id_archivo AS id_reparo_generar_memo_escritura,
        ed.id_expediente,
        NULLIF(TRIM(CONCAT_WS(' ', u.name, u.last_name_first, u.last_name_second)), '') AS solicitante,
        ed.comentarios AS observaciones,
        ed.fecha_alta AS fecha_ingreso,
        COALESCE(crgme.is_subsanar, false) AS subsanar
    FROM expediente_digital ed
    LEFT JOIN users u
        ON u.user_id = ed.id_usuario
       AND u.is_active = true
       AND u.row_status = true
    LEFT JOIN LATERAL (
        SELECT x.is_subsanar
        FROM corregir_reparo_generar_memo_escritura x
        WHERE x.id_expediente = ed.id_expediente
          AND x.is_active = true
          AND x.row_status = true
        ORDER BY x.id_corregir_reparo_generar_memo_escritura DESC
        LIMIT 1
    ) crgme ON true
    WHERE ed.id_expediente = p_id_expediente
      AND ed.id_documento = p_id_documento
      AND ed.is_active = true
      AND ed.row_status = true
      AND COALESCE(NULLIF(TRIM(ed.comentarios), ''), '') <> ''
      AND ed.comentarios NOT ILIKE 'Memo generado por sistema%'
    ORDER BY ed.fecha_alta DESC NULLS LAST, ed.id_archivo DESC
    LIMIT 1;
END;
$$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS usp_select_reparo_generar_memo_escritura(bigint, integer);");
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS usp_select_corregir_reparo_generar_memo_escritura(bigint);");

            migrationBuilder.DropTable(
                name: "corregir_reparo_generar_memo_escritura");
        }
    }
}
