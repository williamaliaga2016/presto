using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class RevisarDatosOperacionCreditoYComprador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisar_datos_comprador");

            migrationBuilder.CreateTable(
                name: "revisar_datos_operacion_comprador",
                columns: table => new
                {
                    id_revisar_datos_operacion_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_revisar_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    tipo_persona = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    direccion_env_esc = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    region_env_esc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna_env_esc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    direccion_env_div = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    region_env_div = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna_env_div = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_dir_dividendo = table.Column<int>(type: "integer", nullable: true),
                    telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_comercial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_movil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    email2 = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_revisar_datos_operacion_comprador", x => x.id_revisar_datos_operacion_comprador);
                });

            migrationBuilder.CreateTable(
                name: "revisar_datos_operacion_credito",
                columns: table => new
                {
                    id_revisar_datos_operacion_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_revisar_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    santiago = table.Column<bool>(type: "boolean", nullable: true),
                    regiones = table.Column<bool>(type: "boolean", nullable: true),
                    tipo_operacion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    fines_generales = table.Column<bool>(type: "boolean", nullable: true),
                    nombre_proyecto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    credito_segunda_vivienda = table.Column<bool>(type: "boolean", nullable: true),
                    inmobiliaria = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    vivienda_social = table.Column<bool>(type: "boolean", nullable: true),
                    dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    propietario_0_1_dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    recepcion_final_mayor_2 = table.Column<bool>(type: "boolean", nullable: true),
                    porcentaje_impuesto = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    monto_credito_afecto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    impuesto_a_pagar = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_revisar_datos_operacion_credito", x => x.id_revisar_datos_operacion_credito);
                });

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_comprador_id_expediente",
                table: "revisar_datos_operacion_comprador",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_comprador_id_revisar_datos_operacion",
                table: "revisar_datos_operacion_comprador",
                column: "id_revisar_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_credito_id_expediente",
                table: "revisar_datos_operacion_credito",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_revisar_datos_operacion_credito_id_revisar_datos_operacion",
                table: "revisar_datos_operacion_credito",
                column: "id_revisar_datos_operacion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisar_datos_operacion_comprador");

            migrationBuilder.DropTable(
                name: "revisar_datos_operacion_credito");

            migrationBuilder.CreateTable(
                name: "revisar_datos_comprador",
                columns: table => new
                {
                    id_revisar_datos_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revisar_datos_comprador", x => x.id_revisar_datos_comprador);
                });

            migrationBuilder.CreateIndex(
                name: "ix_revisar_datos_comprador_expediente",
                table: "revisar_datos_comprador",
                column: "id_expediente");
        }
    }
}
