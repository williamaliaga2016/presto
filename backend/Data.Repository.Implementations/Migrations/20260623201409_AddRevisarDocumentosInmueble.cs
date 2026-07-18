using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisarDocumentosInmueble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "revisar_documentos_inmueble",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    documentos_correctos = table.Column<bool>(type: "boolean", nullable: true),
                    motivo_devolucion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revisar_documentos_inmueble", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_rdi_expediente",
                table: "revisar_documentos_inmueble",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisar_documentos_inmueble");
        }
    }
}
