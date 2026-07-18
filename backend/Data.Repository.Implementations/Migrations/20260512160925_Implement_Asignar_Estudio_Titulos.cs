using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_Asignar_Estudio_Titulos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "asignar_estudio_titulos",
                columns: table => new
                {
                    id_asignar_estudio_titulos = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    abogado = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asignar_estudio_titulos", x => x.id_asignar_estudio_titulos);
                });

            migrationBuilder.CreateIndex(
                name: "ix_asignar_estudio_titulos_expediente_actividad",
                table: "asignar_estudio_titulos",
                columns: new[] { "id_expediente", "id_actividad" },
                unique: true);

            migrationBuilder.Sql(@"
CREATE OR REPLACE FUNCTION usp_select_asignar_estudio_titulos(
    p_id_expediente bigint,
    p_id_actividad varchar
)
RETURNS TABLE (
    id_asignar_estudio_titulos integer,
    id_expediente bigint,
    id_actividad character varying,
    abogado character varying,
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
        aet.id_asignar_estudio_titulos,
        aet.id_expediente,
        aet.id_actividad,
        aet.abogado,
        aet.observaciones,
        aet.is_active,
        aet.row_status,
        aet.created_by,
        aet.created_date,
        aet.modified_by,
        aet.modified_date
    FROM asignar_estudio_titulos aet
    WHERE aet.id_expediente = p_id_expediente
      AND aet.id_actividad = p_id_actividad
      AND aet.is_active = true
      AND aet.row_status = true
    ORDER BY aet.id_asignar_estudio_titulos DESC
    LIMIT 1;
END;
$$;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS usp_select_asignar_estudio_titulos(bigint, varchar);");

            migrationBuilder.DropTable(
                name: "asignar_estudio_titulos");
        }
    }
}
