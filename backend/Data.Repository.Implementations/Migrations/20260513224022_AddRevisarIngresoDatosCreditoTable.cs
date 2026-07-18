using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class AddRevisarIngresoDatosCreditoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "tipo_propiedad",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_direccion",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "region",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "direccion",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "comuna",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "solicitante",
                table: "reparo_calculo_doc_detalle",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "reparo_calculo_doc_detalle",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_ingreso",
                table: "reparo_calculo_doc_detalle",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "revisar_ingreso_datos_credito",
                columns: table => new
                {
                    id_revisar_ingreso_datos_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_operacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    fines_generales = table.Column<bool>(type: "boolean", nullable: true),
                    nombre_proyecto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    credito_segunda_vivienda = table.Column<bool>(type: "boolean", nullable: true),
                    inmobiliaria = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    vivienda_social = table.Column<bool>(type: "boolean", nullable: true),
                    dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    propietario_dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    recepcion_final_mayor_2_anios = table.Column<bool>(type: "boolean", nullable: true),
                    porcentaje_impuesto = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    monto_credito_afecto_impuesto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    impuesto_a_pagar = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_revisar_ingreso_datos_credito", x => x.id_revisar_ingreso_datos_credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_revisar_ingreso_datos_credito_id_datos_operacion",
                table: "revisar_ingreso_datos_credito",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_ingreso_datos_credito_id_expediente",
                table: "revisar_ingreso_datos_credito",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisar_ingreso_datos_credito");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_propiedad",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_direccion",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "region",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "direccion",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "comuna",
                table: "rol_avaluo_calculo_doc_detalle",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "solicitante",
                table: "reparo_calculo_doc_detalle",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "observaciones",
                table: "reparo_calculo_doc_detalle",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2000)",
                oldMaxLength: 2000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "fecha_ingreso",
                table: "reparo_calculo_doc_detalle",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");
        }
    }
}
