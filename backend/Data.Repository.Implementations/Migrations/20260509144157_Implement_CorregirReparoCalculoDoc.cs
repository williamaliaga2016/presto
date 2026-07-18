using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_CorregirReparoCalculoDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "corregir_reparo_calculo_doc",
                columns: table => new
                {
                    id_corregir_reparo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_corregir_reparo_calculo_doc", x => x.id_corregir_reparo_calculo_doc);
                });

            migrationBuilder.CreateTable(
                name: "reparo_calculo_doc_detalle",
                columns: table => new
                {
                    id_reparo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reparo_calculo_doc_detalle", x => x.id_reparo_calculo_doc);
                });

            migrationBuilder.CreateTable(
                name: "rol_avaluo_calculo_doc_detalle",
                columns: table => new
                {
                    id_rol_avaluo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    tipo_direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    rol_avaluo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_avaluo_calculo_doc_detalle", x => x.id_rol_avaluo_calculo_doc);
                });

            migrationBuilder.CreateIndex(
                name: "ix_corregir_reparo_calculo_doc_expediente",
                table: "corregir_reparo_calculo_doc",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_reparo_calculo_doc_detalle_expediente",
                table: "reparo_calculo_doc_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_rol_avaluo_calculo_doc_detalle_expediente",
                table: "rol_avaluo_calculo_doc_detalle",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "corregir_reparo_calculo_doc");

            migrationBuilder.DropTable(
                name: "reparo_calculo_doc_detalle");

            migrationBuilder.DropTable(
                name: "rol_avaluo_calculo_doc_detalle");
        }
    }
}
