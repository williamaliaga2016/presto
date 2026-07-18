using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    public partial class AddDatosBancoAcreedor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "datos_banco_acreedor",
                columns: table => new
                {
                    id_datos_banco_acreedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    cuenta_carta_resguardo = table.Column<bool>(type: "boolean", nullable: true),
                    institucion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    rut_banco_acreedor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
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
                    table.PrimaryKey("PK_datos_banco_acreedor", x => x.id_datos_banco_acreedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_datos_banco_acreedor_id_expediente",
                table: "datos_banco_acreedor",
                column: "id_expediente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_banco_acreedor");
        }
    }
}
