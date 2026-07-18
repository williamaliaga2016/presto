using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FixCorregirReparoDatosOperacionCleanArchitecture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reparo_datos_operacion_detalle");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_datos_operacion",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_datos_operacion",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_subsanar",
                table: "corregir_reparo_datos_operacion",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "id_usuario_solicitante",
                table: "corregir_reparo_datos_operacion");

            migrationBuilder.DropColumn(
                name: "is_subsanar",
                table: "corregir_reparo_datos_operacion");

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "corregir_reparo_datos_operacion",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "reparo_datos_operacion_detalle",
                columns: table => new
                {
                    id_reparo_datos_operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    fecha_ingreso = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    solicitante = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    subsanar = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reparo_datos_operacion_detalle", x => x.id_reparo_datos_operacion);
                });

            migrationBuilder.CreateIndex(
                name: "ix_reparo_datos_operacion_detalle_expediente",
                table: "reparo_datos_operacion_detalle",
                column: "id_expediente");
        }
    }
}
