using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_corregir_reparo_fabrica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "corregir_reparo_fabrica",
                columns: table => new
                {
                    id_corregir_reparo_fabrica = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_usuario_solicitante = table.Column<int>(type: "integer", nullable: false),
                    is_subsanar = table.Column<bool>(type: "boolean", nullable: false),
                    observaciones = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_corregir_reparo_fabrica", x => x.id_corregir_reparo_fabrica);
                });

            migrationBuilder.CreateIndex(
                name: "ix_corregir_reparo_fabrica_expediente",
                table: "corregir_reparo_fabrica",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "corregir_reparo_fabrica");
        }
    }
}
