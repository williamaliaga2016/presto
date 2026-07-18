using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Data.Repository.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Implement_AntecCred_DatosComer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carga_operacion_banco_antecedente_credito",
                columns: table => new
                {
                    id_carga_operacion_banco_antecedente_credito = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    tipo_prestamo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    factor_conversion_uf = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    destino_credito = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    monto_solicitado = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    tipo_tasa = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tasa = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    plazo = table.Column<int>(type: "integer", nullable: true),
                    fecha_inicio = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    monto_nominal = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    monto_residual = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    plazo_primer_periodo = table.Column<int>(type: "integer", nullable: true),
                    periodo_gracia = table.Column<int>(type: "integer", nullable: true),
                    comision = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    plazo_segundo_periodo = table.Column<int>(type: "integer", nullable: true),
                    tasa_primer_periodo = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    meses_sabaticos = table.Column<int>(type: "integer", nullable: true),
                    variabilidad_tasa = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tasa_segundo_periodo = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    moneda = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_tasa_mixta_prod_com = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    tasa_maxima = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    codigo_producto_cartera = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    indicador_segunda_vivienda = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_financiamiento = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    precio_venta_pesos = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    precio_venta_moneda_original = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cantidad_meses_sin_vencimiento = table.Column<int>(type: "integer", nullable: true),
                    indicador_cred_comp = table.Column<int>(type: "integer", nullable: true),
                    indicador_pac = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    tipo_tasa_aplic_contab = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    numero_cuenta_gastos = table.Column<long>(type: "bigint", nullable: true),
                    prestamo_maximo = table.Column<decimal>(type: "numeric(18,6)", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco_antecedente_credito", x => x.id_carga_operacion_banco_antecedente_credito);
                });

            migrationBuilder.CreateTable(
                name: "carga_operacion_banco_datos_comercial",
                columns: table => new
                {
                    id_carga_operacion_banco_datos_comercial = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_carga_operacion_banco = table.Column<int>(type: "integer", nullable: false),
                    id_expediente = table.Column<long>(type: "bigint", nullable: false),
                    codigo_ejecutivo = table.Column<long>(type: "bigint", nullable: true),
                    login_ejecutivo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre_ejecutivo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    rut_ejecutivo = table.Column<long>(type: "bigint", nullable: true),
                    codigo_oficina = table.Column<long>(type: "bigint", nullable: true),
                    nombre_oficina = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    codigo_curse = table.Column<long>(type: "bigint", nullable: true),
                    glosa_curse = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    codigo_ejecutivo_curse = table.Column<long>(type: "bigint", nullable: true),
                    login_ejecutivo_curse = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    nombre_ejecutivo_curse = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    rut_ejecutivo_curse = table.Column<long>(type: "bigint", nullable: true),
                    rut_banco = table.Column<long>(type: "bigint", nullable: true),
                    renovacion_urbana = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    nombre_banco = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    tipo_hipoteca = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    row_status = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    modified_by = table.Column<int>(type: "integer", nullable: true),
                    modified_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carga_operacion_banco_datos_comercial", x => x.id_carga_operacion_banco_datos_comercial);
                });

            migrationBuilder.CreateIndex(
                name: "ix_cob_ant_credito_id_carga_operacion_banco",
                table: "carga_operacion_banco_antecedente_credito",
                column: "id_carga_operacion_banco");

            migrationBuilder.CreateIndex(
                name: "ix_cob_ant_credito_id_expediente",
                table: "carga_operacion_banco_antecedente_credito",
                column: "id_expediente");

            migrationBuilder.CreateIndex(
                name: "ix_cob_datos_comercial_id_carga_operacion_banco",
                table: "carga_operacion_banco_datos_comercial",
                column: "id_carga_operacion_banco");

            migrationBuilder.CreateIndex(
                name: "ix_cob_datos_comercial_id_expediente",
                table: "carga_operacion_banco_datos_comercial",
                column: "id_expediente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carga_operacion_banco_antecedente_credito");

            migrationBuilder.DropTable(
                name: "carga_operacion_banco_datos_comercial");
        }
    }
}
