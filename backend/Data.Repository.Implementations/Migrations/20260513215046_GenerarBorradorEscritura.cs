using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class GenerarBorradorEscritura : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "generar_borrador_escritura",
                columns: table => new
                {
                    id_generar_borrador_escritura = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    existe_alzamiento = table.Column<bool>(type: "boolean", nullable: false),
                    seguro_cesantia = table.Column<bool>(type: "boolean", nullable: false),
                    mandato_judicial = table.Column<bool>(type: "boolean", nullable: false),
                    beneficios = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    id_notaria = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_generar_borrador_escritura", x => x.id_generar_borrador_escritura);
                });

            migrationBuilder.CreateTable(
                name: "generar_borrador_escritura_detalle",
                columns: table => new
                {
                    id_generar_borrador_escritura_detalle_entity = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_generar_borrador_escritura = table.Column<int>(type: "integer", nullable: false),
                    id_datos_operacion_fiador_garante = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_rol_comparecencia = table.Column<int>(type: "integer", nullable: false),
                    requiere_firma = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_generar_borrador_escritura_detalle", x => x.id_generar_borrador_escritura_detalle_entity);
                });

            migrationBuilder.CreateIndex(
                name: "ix_generar_borrador_escritura_id_expediente",
                table: "generar_borrador_escritura",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_generar_borrador_escritura_id_notaria",
                table: "generar_borrador_escritura",
                column: "id_notaria");

            migrationBuilder.CreateIndex(
                name: "ix_gbe_detalle_id_expediente",
                table: "generar_borrador_escritura_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_gbe_detalle_id_fiador_garante",
                table: "generar_borrador_escritura_detalle",
                column: "id_datos_operacion_fiador_garante");

            migrationBuilder.CreateIndex(
                name: "ix_gbe_detalle_id_gbe",
                table: "generar_borrador_escritura_detalle",
                column: "id_generar_borrador_escritura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "generar_borrador_escritura");

            migrationBuilder.DropTable(
                name: "generar_borrador_escritura_detalle");
        }
    }
}
