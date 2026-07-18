using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class CorrecionDatosOperacionCampos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP INDEX IF EXISTS ""IX_revisar_ingreso_datos_credito_id_datos_operacion"";
            ");

            migrationBuilder.Sql(
                @"DROP INDEX IF EXISTS ""IX_revisar_ingreso_datos_credito_id_expediente"";");

            migrationBuilder.DropColumn(
                name: "enviar_reparo",
                table: "datos_operacion_propiedad");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "datos_operacion_propiedad");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "datos_operacion_fiador_garante");

            migrationBuilder.DropColumn(
                name: "enviar_a_reparo",
                table: "datos_operacion_datos_credito");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "datos_operacion_datos_credito");

            migrationBuilder.DropColumn(
                name: "enviar_a_reparo",
                table: "datos_operacion_banco_acreedor");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "datos_operacion_banco_acreedor");

            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_operacion",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentaje_impuesto",
                table: "revisar_ingreso_datos_credito",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nombre_proyecto",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "monto_credito_afecto_impuesto",
                table: "revisar_ingreso_datos_credito",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modified_date",
                table: "revisar_ingreso_datos_credito",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inmobiliaria",
                table: "revisar_ingreso_datos_credito",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "impuesto_a_pagar",
                table: "revisar_ingreso_datos_credito",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "revisar_ingreso_datos_credito",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<bool>(
                name: "enviar_a_reparo",
                table: "datos_operacion",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "datos_operacion",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "enviar_a_reparo",
                table: "datos_operacion");

            migrationBuilder.DropColumn(
                name: "observaciones",
                table: "datos_operacion");

            migrationBuilder.AlterColumn<string>(
                name: "ubicacion",
                table: "revisar_ingreso_datos_credito",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_operacion",
                table: "revisar_ingreso_datos_credito",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentaje_impuesto",
                table: "revisar_ingreso_datos_credito",
                type: "numeric(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "revisar_ingreso_datos_credito",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "nombre_proyecto",
                table: "revisar_ingreso_datos_credito",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "monto_credito_afecto_impuesto",
                table: "revisar_ingreso_datos_credito",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "modified_date",
                table: "revisar_ingreso_datos_credito",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "inmobiliaria",
                table: "revisar_ingreso_datos_credito",
                type: "character varying(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "impuesto_a_pagar",
                table: "revisar_ingreso_datos_credito",
                type: "numeric(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "revisar_ingreso_datos_credito",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "enviar_reparo",
                table: "datos_operacion_propiedad",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "datos_operacion_propiedad",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "datos_operacion_fiador_garante",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "enviar_a_reparo",
                table: "datos_operacion_datos_credito",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "datos_operacion_datos_credito",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "enviar_a_reparo",
                table: "datos_operacion_banco_acreedor",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "observaciones",
                table: "datos_operacion_banco_acreedor",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_revisar_ingreso_datos_credito_id_datos_operacion",
                table: "revisar_ingreso_datos_credito",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_ingreso_datos_credito_id_expediente",
                table: "revisar_ingreso_datos_credito",
                column: "id_expediente");
        }
    }
}
