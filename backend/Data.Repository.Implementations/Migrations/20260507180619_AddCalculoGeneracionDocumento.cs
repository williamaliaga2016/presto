using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddCalculoGeneracionDocumento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "calculo_generacion_documento",
                columns: table => new
                {
                    id_calculo_generacion_documento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    tipo_direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    rol_avaluo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    revision_rol_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    valor_uf_fecha_hoy = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    fecha_calculo = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    valor_uf_fecha_calculo = table.Column<decimal>(type: "numeric(12,2)", nullable: true),
                    is_enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_calculo_generacion_documento", x => x.id_calculo_generacion_documento);
                });

            migrationBuilder.CreateTable(
                name: "valor_uf",
                columns: table => new
                {
                    id_valor_uf = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fecha = table.Column<DateTime>(type: "date", nullable: false),
                    valor = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valor_uf", x => x.id_valor_uf);
                });

            migrationBuilder.CreateIndex(
                name: "IX_calculo_generacion_documento_id_expediente",
                table: "calculo_generacion_documento",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_valor_uf_fecha",
                table: "valor_uf",
                column: "fecha",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "calculo_generacion_documento");

            migrationBuilder.DropTable(
                name: "valor_uf");
        }
    }
}
