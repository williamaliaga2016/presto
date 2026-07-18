using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class RegistrarFirmaApoderadoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "registrar_firma_apoderado_banco",
                columns: table => new
                {
                    id_registrar_firma_apoderado_banco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha_firma = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_registrar_firma_apoderado_banco", x => x.id_registrar_firma_apoderado_banco);
                });

            migrationBuilder.CreateIndex(
                name: "ix_registrar_firma_apoderado_banco_expediente",
                table: "registrar_firma_apoderado_banco",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "registrar_firma_apoderado_banco");
        }
    }
}
