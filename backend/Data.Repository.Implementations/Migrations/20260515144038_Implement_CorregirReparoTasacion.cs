using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_CorregirReparoTasacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "corregir_reparo_tasacion",
                columns: table => new
                {
                    id_corregir_reparo_tasacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
                    is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_corregir_reparo_tasacion", x => x.id_corregir_reparo_tasacion);
                });

            migrationBuilder.CreateIndex(
                name: "ix_corregir_reparo_tasacion_expediente",
                table: "corregir_reparo_tasacion",
                column: "id_expediente");

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION usp_select_corregir_reparo_tasacion(p_id_expediente BIGINT)
RETURNS TABLE (
    id_corregir_reparo_tasacion INTEGER,
    id_expediente BIGINT,
    id_usuario_solicitante INTEGER,
    is_subsanar BOOLEAN,
    observaciones VARCHAR(1000),
    solicitante TEXT,
    observaciones_reparo TEXT,
    fecha_ingreso TIMESTAMP,
    is_active BOOLEAN,
    row_status BOOLEAN,
    created_by INTEGER,
    created_date TIMESTAMP,
    modified_by INTEGER,
    modified_date TIMESTAMP
)
LANGUAGE sql
AS $$
    SELECT
        COALESCE(crt.id_corregir_reparo_tasacion, 0) AS id_corregir_reparo_tasacion,
        t.id_expediente,
        COALESCE(crt.id_usuario_solicitante, t.created_by) AS id_usuario_solicitante,
        COALESCE(crt.is_subsanar, FALSE) AS is_subsanar,
        crt.observaciones,
        COALESCE(NULLIF(TRIM(CONCAT_WS(' ', u.name, u.last_name_first, u.last_name_second)), ''), 'Sin solicitante')::TEXT AS solicitante,
        t.observaciones::TEXT AS observaciones_reparo,
        t.created_date AS fecha_ingreso,
        COALESCE(crt.is_active, TRUE) AS is_active,
        COALESCE(crt.row_status, TRUE) AS row_status,
        COALESCE(crt.created_by, 0) AS created_by,
        COALESCE(crt.created_date, LOCALTIMESTAMP) AS created_date,
        crt.modified_by,
        crt.modified_date
    FROM tasacion t
    LEFT JOIN users u ON u.user_id = t.created_by
    LEFT JOIN corregir_reparo_tasacion crt
        ON crt.id_expediente = t.id_expediente
       AND crt.is_active = TRUE
       AND crt.row_status = TRUE
    WHERE t.id_expediente = p_id_expediente
      AND t.is_enviar_reparo = TRUE
      AND t.is_active = TRUE
      AND t.row_status = TRUE
    ORDER BY t.id_tasacion DESC, crt.id_corregir_reparo_tasacion DESC
    LIMIT 1;
$$;
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS usp_select_corregir_reparo_tasacion(BIGINT);");

            migrationBuilder.DropTable(
                name: "corregir_reparo_tasacion");
        }
    }
}
