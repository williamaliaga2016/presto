using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisarDatosOperacionBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "revisar_datos_operacion_banco",
                columns: table => new
                {
                    id_revisar_datos_operacion_banco = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_revisar_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    cuenta_carta_resguardo = table.Column<bool>(type: "boolean", nullable: true),
                    institucion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
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
                    table.PrimaryKey("PK_revisar_datos_operacion_banco", x => x.id_revisar_datos_operacion_banco);
                });

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_banco_id_expediente",
                table: "revisar_datos_operacion_banco",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_banco_id_revisar_datos_operacion",
                table: "revisar_datos_operacion_banco",
                column: "id_revisar_datos_operacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisar_datos_operacion_banco");
        }
    }
}
