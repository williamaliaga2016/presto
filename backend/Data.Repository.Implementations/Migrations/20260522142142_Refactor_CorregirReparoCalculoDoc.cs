using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Refactor_CorregirReparoCalculoDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS reparo_calculo_doc_detalle CASCADE;");

            migrationBuilder.Sql("DROP TABLE IF EXISTS rol_avaluo_calculo_doc_detalle CASCADE;");    

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AddColumn<string>(
                name: "existe_rol_avaluo",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_calculo_doc",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_subsanar",
                table: "corregir_reparo_calculo_doc",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "rol_avaluo_editado",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_avaluo_pesos",
                table: "corregir_reparo_calculo_doc",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "existe_rol_avaluo",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "rol_avaluo_editado",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.DropColumn(
                name: "valor_avaluo_pesos",
                table: "corregir_reparo_calculo_doc");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_calculo_doc",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "reparo_calculo_doc_detalle",
                columns: table => new
                {
                    id_reparo_calculo_doc = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
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
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    rol_avaluo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    tipo_direccion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol_avaluo_calculo_doc_detalle", x => x.id_rol_avaluo_calculo_doc);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reparo_calculo_doc_detalle_expediente",
                table: "reparo_calculo_doc_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_rol_avaluo_calculo_doc_detalle_expediente",
                table: "rol_avaluo_calculo_doc_detalle",
                column: "id_expediente");
        }
    }
}
