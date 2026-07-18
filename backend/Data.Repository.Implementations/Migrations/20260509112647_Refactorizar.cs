using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Refactorizar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_banco_acreedor");

            migrationBuilder.Sql("DROP TABLE IF EXISTS datos_comprador CASCADE;");

            migrationBuilder.DropTable(
                name: "datos_credito");

            migrationBuilder.Sql("DROP TABLE IF EXISTS datos_personales_comprador CASCADE;");

            migrationBuilder.DropTable(
                name: "datos_propiedad");

            migrationBuilder.DropTable(
                name: "datos_vendedor");

            migrationBuilder.DropTable(
                name: "datos_vendedor_detalle");

            migrationBuilder.DropTable(
                name: "fiador_garante");

            migrationBuilder.CreateTable(
                name: "datos_operacion",
                columns: table => new
                {
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_operacion", x => x.id_datos_operacion);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_banco_acreedor",
                columns: table => new
                {
                    id_datos_operacion_banco_acreedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_datos_operacion_banco_acreedor", x => x.id_datos_operacion_banco_acreedor);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_comprador",
                columns: table => new
                {
                    id_datos_operacion_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
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
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_operacion_comprador", x => x.id_datos_operacion_comprador);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_datos_credito",
                columns: table => new
                {
                    id_datos_operacion_datos_credito = table.Column<int>(type: "integer", nullable: false)
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
                    table.PrimaryKey("PK_datos_operacion_datos_credito", x => x.id_datos_operacion_datos_credito);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_fiador_garante",
                columns: table => new
                {
                    id_datos_operacion_fiador_garante = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    rut = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    nombres = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_fijo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_movil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_datos_operacion_fiador_garante", x => x.id_datos_operacion_fiador_garante);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_propiedad",
                columns: table => new
                {
                    id_datos_operacion_propiedad = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_venta = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_construccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_direccion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    villa_condominio = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    numero_casa_habitantes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    conjunto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    manzana = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    lote = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    region = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    comuna = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rol_avaluo_1 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    rol_avaluo_2 = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    enviar_reparo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_datos_operacion_propiedad", x => x.id_datos_operacion_propiedad);
                });

            migrationBuilder.CreateTable(
                name: "datos_operacion_vendedor",
                columns: table => new
                {
                    id_datos_operacion_vendedor = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_datos_operacion = table.Column<int>(type: "integer", nullable: false),
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
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_operacion_vendedor", x => x.id_datos_operacion_vendedor);
                });

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_id_expediente",
                table: "datos_operacion",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_banco_acreedor_id_datos_operacion",
                table: "datos_operacion_banco_acreedor",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_banco_acreedor_id_expediente",
                table: "datos_operacion_banco_acreedor",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_comprador_id_datos_operacion",
                table: "datos_operacion_comprador",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_comprador_id_expediente",
                table: "datos_operacion_comprador",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_datos_credito_id_datos_operacion",
                table: "datos_operacion_datos_credito",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_datos_credito_id_expediente",
                table: "datos_operacion_datos_credito",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_fiador_garante_id_datos_operacion",
                table: "datos_operacion_fiador_garante",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_fiador_garante_id_expediente",
                table: "datos_operacion_fiador_garante",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_propiedad_id_datos_operacion",
                table: "datos_operacion_propiedad",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_propiedad_id_expediente",
                table: "datos_operacion_propiedad",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_vendedor_id_datos_operacion",
                table: "datos_operacion_vendedor",
                column: "id_datos_operacion");

            migrationBuilder.CreateIndex(
                name: "IX_datos_operacion_vendedor_id_expediente",
                table: "datos_operacion_vendedor",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "datos_operacion");

            migrationBuilder.DropTable(
                name: "datos_operacion_banco_acreedor");

            migrationBuilder.DropTable(
                name: "datos_operacion_comprador");

            migrationBuilder.DropTable(
                name: "datos_operacion_datos_credito");

            migrationBuilder.DropTable(
                name: "datos_operacion_fiador_garante");

            migrationBuilder.DropTable(
                name: "datos_operacion_propiedad");

            migrationBuilder.DropTable(
                name: "datos_operacion_vendedor");

            migrationBuilder.CreateTable(
                name: "datos_banco_acreedor",
                columns: table => new
                {
                    id_datos_banco_acreedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    cuenta_carta_resguardo = table.Column<bool>(type: "boolean", nullable: true),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
                    id_actividad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    institucion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    rut_banco_acreedor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_banco_acreedor", x => x.id_datos_banco_acreedor);
                });

            migrationBuilder.CreateTable(
                name: "datos_comprador",
                columns: table => new
                {
                    id_datos_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    enviar_reparo = table.Column<bool>(type: "boolean", nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_comprador", x => x.id_datos_comprador);
                });

            migrationBuilder.CreateTable(
                name: "datos_credito",
                columns: table => new
                {
                    id_datos_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    credito_segunda_vivienda = table.Column<bool>(type: "boolean", nullable: false),
                    dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
                    fines_generales = table.Column<bool>(type: "boolean", nullable: true),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_tipo_operacion = table.Column<int>(type: "integer", nullable: true),
                    impuesto_a_pagar = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    inmobiliaria = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    monto_credito_afecto_impuesto = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    nombre_proyecto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    porcentaje_impuesto = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    propietario_dfl2 = table.Column<bool>(type: "boolean", nullable: true),
                    recepcion_final_mayor_2_anios = table.Column<bool>(type: "boolean", nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    ubicacion = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    vivienda_social = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_credito", x => x.id_datos_credito);
                });

            migrationBuilder.CreateTable(
                name: "datos_personales_comprador",
                columns: table => new
                {
                    id_datos_personales_comprador = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comuna = table.Column<string>(type: "text", nullable: false),
                    comuna_env_div = table.Column<string>(type: "text", nullable: false),
                    comuna_env_esc = table.Column<string>(type: "text", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "text", nullable: false),
                    direccion_env_div = table.Column<string>(type: "text", nullable: false),
                    direccion_env_esc = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    email2 = table.Column<string>(type: "text", nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    id_relacion_titular = table.Column<int>(type: "integer", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    profesion = table.Column<string>(type: "text", nullable: true),
                    region = table.Column<string>(type: "text", nullable: false),
                    region_env_div = table.Column<string>(type: "text", nullable: false),
                    region_env_esc = table.Column<string>(type: "text", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    rut = table.Column<string>(type: "text", nullable: false),
                    telefono = table.Column<string>(type: "text", nullable: false),
                    telefono_comercial = table.Column<string>(type: "text", nullable: true),
                    telefono_movil = table.Column<string>(type: "text", nullable: true),
                    tipo_dir_dividendo = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_personales_comprador", x => x.id_datos_personales_comprador);
                });

            migrationBuilder.CreateTable(
                name: "datos_propiedad",
                columns: table => new
                {
                    id_datos_propiedad = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    comuna = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    conjunto = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    enviar_reparo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    estado = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    existe_rol_avaluo = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    lote = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    manzana = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    numero_casa_habitantes = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    region = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    rol_avaluo_1 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rol_avaluo_2 = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    tipo_construccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_direccion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_propiedad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    tipo_venta = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor_avaluo_pesos = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    villa_condominio = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_propiedad", x => x.id_datos_propiedad);
                });

            migrationBuilder.CreateTable(
                name: "datos_vendedor",
                columns: table => new
                {
                    id_datos_vendedor = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    enviar_a_reparo = table.Column<bool>(type: "boolean", nullable: true),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    observaciones = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_vendedor", x => x.id_datos_vendedor);
                });

            migrationBuilder.CreateTable(
                name: "datos_vendedor_detalle",
                columns: table => new
                {
                    id_datos_vendedor_detalle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    apellido_materno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    apellido_paterno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    comuna = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    email2 = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    estado_civil = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    fecha_nacimiento = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    genero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombres = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    profesion = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    razon_social = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    relacion_titular = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    rut = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    telefono = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_comercial = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    telefono_movil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_vendedor = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_datos_vendedor_detalle", x => x.id_datos_vendedor_detalle);
                });

            migrationBuilder.CreateTable(
                name: "fiador_garante",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    apellido_materno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    apellido_paterno = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    comuna = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    direccion = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    estado_civil = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    fecha_nacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    genero = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    id_actividad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    nacionalidad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    nombres = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    observaciones = table.Column<string>(type: "text", nullable: true),
                    profesion = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    region = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    relacion_titular = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    rut = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    telefono_fijo = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    telefono_movil = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fiador_garante", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_datos_banco_acreedor_id_expediente",
                table: "datos_banco_acreedor",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_credito_id_expediente",
                table: "datos_credito",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_id_expediente",
                table: "datos_vendedor",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_detalle_id_expediente",
                table: "datos_vendedor_detalle",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "IX_datos_vendedor_detalle_rut",
                table: "datos_vendedor_detalle",
                column: "rut");

            migrationBuilder.CreateIndex(
                name: "idx_fiador_garante_expediente",
                table: "fiador_garante",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "uq_expediente_rut_fiador",
                table: "fiador_garante",
                columns: new[] { "id_expediente", "rut" },
                unique: true);
        }
    }
}
